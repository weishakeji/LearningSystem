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
    /// 在线试题练习
    /// </summary>
    public class Training : BasePage
    {
        //各种参数
        //专业id,课程id,章节id
        int sbjid = WeiSha.Common.Request.QueryString["sbjid"].Int32 ?? 0;
        int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        //题型，难度，题量
        int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        int diff = WeiSha.Common.Request.QueryString["diff"].Int32 ?? 0;
        int count = WeiSha.Common.Request.QueryString["count"].Int32 ?? 0;
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
            //基本信息布局
            _initLoyout();
            //
            //试题
            Song.Entities.Questions[] ques = null;         
            //登录且学员必须通过审核
            if (Extend.LoginState.Accounts.IsLogin && Extend.LoginState.Accounts.CurrentUser.Ac_IsPass)
            {
                if (couid > 0)
                {             
                        ques = Business.Do<IQuestions>().QuesCount(Organ.Org_ID, sbjid, couid, olid, type, diff, true, index - 1, size);
                }
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
        #region 初始布局
        private void _initLoyout()
        {
            //专业列表           
            Tag sbjTag = this.Document.GetChildTagById("subject");
            if (sbjTag != null)
            {
                Song.Entities.Subject[] subj = Business.Do<ISubject>().SubjectCount(Organ.Org_ID, null, true, 0, 0);
                this.Document.SetValue("subject", subj);
            }
            //题型
            this.Document.SetValue("quesType", WeiSha.Common.App.Get["QuesType"].Split(','));
            //难度
            Tag diffTag = this.Document.GetChildTagById("diff");
            if (diffTag != null)
            {
                string tm = diffTag.Attributes.GetValue("level", "");
                this.Document.SetValue("diff", tm.Split(','));
            }
            //课程        
            if (sbjid > 0)
            {
                List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseCount(Organ.Org_ID, sbjid, null, true, 0);
                this.Document.SetValue("courses", courses);
            }
            //章节     
            if (couid > 0)
            {
                Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineCount(couid, 0, true, 0);
                this.Document.SetValue("outlines", outlines);
                this.Document.Variables.SetValue("couid", couid);
            }
            //是否登录，且通过验证
            bool isPass = this.Account != null && this.Account.Ac_IsPass;
            this.Document.Variables.SetValue("isPass", isPass);
            //总题数
            int sumCount = 0;
            if (couid > 0)
            {                
                //是否购买
                bool isBuy = false, istry = false;
                if (Extend.LoginState.Accounts.IsLogin)
                {
                    isBuy = Business.Do<ICourse>().IsBuy(this.Account.Ac_ID, couid, 1);
                    istry = Business.Do<ICourse>().IsTryout(couid, this.Account.Ac_ID);
                }
                this.Document.Variables.SetValue("isBuy", isBuy);               
                this.Document.Variables.SetValue("isTry", istry);
                if (isBuy)
                {
                    sumCount = Business.Do<IQuestions>().QuesOfCount(Organ.Org_ID, sbjid, couid, olid, type, diff, true);
                }
                else
                {
                    sumCount = 10;
                }                
            }
            //计算分页，200个记录不分页，每页默认100条，最多10页
            QuestionPagerItem qpi = new QuestionPagerItem(200, 100, 10);
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
        }
        #endregion
        #region 页面上调用的方法
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
            if (ans != null)
            {
                for (int i = 0; i < ans.Length; i++)
                {
                    ans[i] = Extend.Questions.TranText(ans[i]);
                    //ans[i].Ans_Context = ans[i].Ans_Context.Replace("<", "&lt;");
                    //ans[i].Ans_Context = ans[i].Ans_Context.Replace(">", "&gt;");
                }
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
                if (ans != null)
                {
                    for (int i = 0; i < ans.Length; i++)
                    {
                        if (ans[i].Ans_IsCorrect)
                            ansStr += (char)(65 + i);
                    }
                }
            }
            if (qus.Qus_Type == 2)
            {
                Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                if (ans != null)
                {
                    for (int i = 0; i < ans.Length; i++)
                    {
                        if (ans[i].Ans_IsCorrect)
                            ansStr += (char)(65 + i) + "、";
                    }
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
                    if(st!=null)
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
        /// 获取序号
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected object GetOrder(object[] index)
        {
            int tax = 0;
            if (index.Length > 0)
                tax = Convert.ToInt32(index[0]);
            return (char)(tax - 1 + 65);
        }
        #endregion
    }
}