using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using Song.Extend;

namespace Song.Site
{
    /// <summary>
    /// 练习题，用于在章节学习时的习题练习
    /// </summary>
    public class Exercises : BasePage
    {
        //章节id ,题型，难度，题量
        int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        int diff = WeiSha.Common.Request.QueryString["diff"].Int32 ?? 0;
        int count = WeiSha.Common.Request.QueryString["count"].Int32 ?? int.MaxValue;
        //试题的启始索引与当前取多少条记录
        protected string indexPara = WeiSha.Common.Request.QueryString["index"].String;
        protected int index = 1;
        protected int size = 1;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        //当前学员收藏的试题
        Song.Entities.Questions[] collectQues = null;
        protected override void InitPageTemplate(HttpContext context)
        {
            //登录且学员必须通过审核
            if (!(Extend.LoginState.Accounts.IsLogin && Extend.LoginState.Accounts.CurrentUser.Ac_IsPass))
            {
                return;
            }
            #region 基本信息布局
            //题型
            this.Document.SetValue("quesType", WeiSha.Common.App.Get["QuesType"].Split(','));
            //难度
            Tag diffTag = this.Document.GetChildTagById("diff");
            if (diffTag != null)
            {
                string tm = diffTag.Attributes.GetValue("level", "");
                this.Document.SetValue("diff", tm.Split(','));
            }
            //每页的题量***********
            //总题数
            int sumCount = 0;
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            if (outline != null)
            {
                bool isBuy = Business.Do<ICourse>().IsBuy(outline.Cou_ID, Extend.LoginState.Accounts.CurrentUserId, 1);
                if (isBuy)
                {
                    sumCount = Business.Do<IQuestions>().QuesOfCount(Organ.Org_ID, 0, 0, olid, type, diff, true);
                }
                else
                {
                    //是否在试用中
                    bool istry = Business.Do<ICourse>().IsTryout(outline.Cou_ID, this.Account.Ac_ID);
                    Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(outline.Cou_ID);
                    if (course.Cou_IsTry)
                    {
                        sumCount = Business.Do<IQuestions>().QuesOfCount(Organ.Org_ID, 0, 0, olid, type, diff, true);
                        sumCount = sumCount > course.Cou_TryNum ? course.Cou_TryNum : sumCount;
                    }
                    this.Document.Variables.SetValue("isTry", istry);
                }

            }
            //计算分页，200个记录不分页，每页默认100条，最多10页
            QuestionPagerItem qpi = new QuestionPagerItem(200, 100, 10);
            //sumCount = WeiSha.Common.Request.QueryString["sum"].Int32 ?? 0;   //仅供测试
            List<QuestionPagerItem> pager = qpi.Builder(sumCount);
            this.Document.SetValue("pager", pager);
            this.Document.SetValue("sumCount", sumCount);
            //与当前索引页码与取值长度
            if (!string.IsNullOrWhiteSpace(indexPara))
            {
                if (indexPara.IndexOf("-") > -1)
                {
                    int.TryParse(indexPara.Substring(0, indexPara.IndexOf("-")), out index);
                    int.TryParse(indexPara.Substring(indexPara.IndexOf("-") + 1), out size);
                }
            }
            else
            {
                index = pager.First<QuestionPagerItem>().Index;
                size = pager.First<QuestionPagerItem>().Size;
            }
            this.Document.SetValue("index", index);
            #endregion
            //            
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(Organ.Org_Config);
            //如果需要学员登录后才能学习
            bool isTraningLogin = config["IsTraningLogin"].Value.Boolean ?? false;
            this.Document.SetValue("isTraningLogin", isTraningLogin);
            //试题
            Song.Entities.Questions[] ques = null;
            if (isTraningLogin)
            {
                //登录且学员必须通过审核
                if (Extend.LoginState.Accounts.IsLogin && Extend.LoginState.Accounts.CurrentUser.Ac_IsPass)
                {
                    ques = Business.Do<IQuestions>().QuesCount(Organ.Org_ID, 0, 0, olid, type, diff, true, index - 1, size);
                    //ques = Business.Do<IQuestions>().QuesRandom(Organ.Org_ID, -1, -1, olid, type, diff, diff, true, count);
                }
            }
            else
            {
                ques = Business.Do<IQuestions>().QuesCount(Organ.Org_ID, 0, 0, olid, type, diff, true, index - 1, size);
                //ques = Business.Do<IQuestions>().QuesRandom(Organ.Org_ID, -1, -1, olid, type, diff, diff, true, count);
            }
            if (ques != null)
            {
                for (int i = 0; i < ques.Length; i++)
                {
                    ques[i] = Extend.Questions.TranText(ques[i]);
                    ques[i].Qus_Title = ques[i].Qus_Title.Replace("&lt;", "<");
                    ques[i].Qus_Title = ques[i].Qus_Title.Replace("&gt;", ">");
                    ques[i].Qus_Title = Extend.Html.ClearHTML(ques[i].Qus_Title, "p", "div", "font", "pre");
                    ques[i].Qus_Title = ques[i].Qus_Title.Replace("\n", "<br/>");
                }
            }
            this.Document.SetValue("ques", ques);
            this.Document.RegisterGlobalFunction(this.GetTypeName);
            this.Document.RegisterGlobalFunction(this.AnswerItems);
            this.Document.RegisterGlobalFunction(this.GetAnswer);
            this.Document.RegisterGlobalFunction(this.GetOrder);
            this.Document.RegisterGlobalFunction(this.IsCollect);

        }
        /// <summary>
        /// 获取试题的类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object GetTypeName(object[] type)
        {
            int tp = 1;
            if (type.Length > 0)
                int.TryParse(type[0].ToString(), out tp);
            if (tp > 0 && tp <= typeStr.Length)
            {
                return typeStr[tp - 1].Trim();
            }
            return "未知题型";
        }
        /// <summary>
        /// 当前试题的选项，仅用于单选与多选
        /// </summary>
        /// <returns></returns>
        protected object AnswerItems(object[] p)
        {
            Song.Entities.Questions qus = null;
            if (p.Length > 0)
                qus = (Song.Entities.Questions)p[0];
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            for (int i = 0; i < ans.Length; i++)
            {
                ans[i] = Extend.Questions.TranText(ans[i]);
                //ans[i].Ans_Context = ans[i].Ans_Context.Replace("<", "&lt;");
                //ans[i].Ans_Context = ans[i].Ans_Context.Replace(">", "&gt;");
            }
            return ans;
        }
        /// <summary>
        /// 试题的答案
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected object GetAnswer(object[] objs)
        {
            //当前试题
            Song.Entities.Questions qus = (Song.Entities.Questions)objs[0];
            string ansStr = "";
            if (qus.Qus_Type == 1)
            {
                //当前试题的答案
                Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                for (int i = 0; i < ans.Length; i++)
                {
                    if (ans[i].Ans_IsCorrect)
                        ansStr += (char)(65 + i);
                }
            }
            if (qus.Qus_Type == 2)
            {
                Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                for (int i = 0; i < ans.Length; i++)
                {
                    if (ans[i].Ans_IsCorrect)
                        ansStr += (char)(65 + i) + "、";
                }
                ansStr = ansStr.Substring(0, ansStr.LastIndexOf("、"));
            }
            if (qus.Qus_Type == 3)
                ansStr = qus.Qus_IsCorrect ? "正确" : "错误";
            if (qus.Qus_Type == 4)
            {
                if (qus != null && !string.IsNullOrEmpty(qus.Qus_Answer))
                    ansStr = qus.Qus_Answer;
            }
            if (qus.Qus_Type == 5)
            {
                //当前试题的答案
                Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                for (int i = 0; i < ans.Length; i++)
                    ansStr += (char)(65 + i) + "、" + ans[i].Ans_Context + "<br/>";
            }
            return ansStr;
        }

        /// <summary>
        /// 试题是否被当前学员收藏
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected object IsCollect(object[] objs)
        {
            int qid = 0;
            if (objs.Length > 0)
                qid = Convert.ToInt32(objs[0]);
            //当前收藏            
            if (collectQues == null)
            {
                if (Extend.LoginState.Accounts.IsLogin)
                {
                    Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
                    if (st != null)
                        collectQues = Business.Do<IStudent>().CollectAll4Ques(st.Ac_ID, 0, -1, 0);
                }
            }
            if (collectQues != null)
            {
                foreach (Song.Entities.Questions q in collectQues)
                {
                    if (qid == q.Qus_ID) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 将选择题选项，由数字序号转为字母
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected object GetOrder(object[] index)
        {
            int tax = 0;
            if (index.Length > 0) int.TryParse(index[0].ToString(), out tax);
            return (char)(tax - 1 + 65);
        }
    }

}