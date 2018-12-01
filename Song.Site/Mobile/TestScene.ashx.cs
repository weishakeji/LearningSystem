using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using Song.Entities;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 模拟考试现场
    /// </summary>
    public class TestScene : BasePage
    {

        //考试id
        protected int tpid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //课程id
        int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //行为
        protected string action = WeiSha.Common.Request.QueryString["action"].String;
        //当前学员收藏的试题
        Song.Entities.Questions[] collectQues = null;
        string[] types = null;
        //当前试卷
        Song.Entities.TestPaper paper = null;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("action", action);
            this.Document.SetValue("couid", couid);
            this.Document.SetValue("tpid", tpid);
            //唯一值
            string uid = WeiSha.Common.Request.UniqueID();
            this.Document.Variables.SetValue("uid", uid);
            //服务器端时间
            this.Document.Variables.SetValue("Time", WeiSha.Common.Server.getTime());
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("Login.ashx");
            //当前试卷
            paper = Business.Do<ITestPaper>().PagerSingle(tpid);
            this.Document.SetValue("pager", paper);
            //试卷所属课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(paper.Cou_ID);
            this.Document.SetValue("course", course);
            //启始页状态
            if (string.IsNullOrWhiteSpace(action))
            {
                //
            }
            //开始考试
            if (action == "start")
            {
                //题型
                this.Document.SetValue("quesType", WeiSha.Common.App.Get["QuesType"].Split(','));
                //取果是第一次打开，则随机生成试题，此为获取试卷
                //难度区间
                int diff1 = paper.Tp_Diff > paper.Tp_Diff2 ? (int)paper.Tp_Diff2 : (int)paper.Tp_Diff;
                int diff2 = paper.Tp_Diff > paper.Tp_Diff2 ? (int)paper.Tp_Diff : (int)paper.Tp_Diff2;
                //开始抽题
                List<Song.Entities.Questions> ques = new List<Entities.Questions>();
                Dictionary<TestPaperItem, Song.Entities.Questions[]> dic = Business.Do<ITestPaper>().Putout(paper);
                foreach (var d in dic)
                {
                    Song.Entities.Questions[] qs = (Song.Entities.Questions[])d.Value;
                    for (int n = 0; n < qs.Length; n++)
                    {
                        qs[n].Qus_Explain = "";
                        qs[n].Qus_Answer = "";
                        qs[n] = Extend.Questions.TranText(qs[n]);
                        qs[n].Qus_Title = qs[n].Qus_Title.Replace("&lt;", "<");
                        qs[n].Qus_Title = qs[n].Qus_Title.Replace("&gt;", ">");
                        qs[n].Qus_Title = Extend.Html.ClearHTML(qs[n].Qus_Title, "p", "div");
                        ques.Add(qs[n]);
                    }
                }
                this.Document.SetValue("ques", ques);
                this.Document.RegisterGlobalFunction(this.getQuesType);
                this.Document.RegisterGlobalFunction(this.AnswerItems);
                this.Document.RegisterGlobalFunction(this.GetOrder);
                this.Document.RegisterGlobalFunction(this.IsCollect);
            }
        }
        /// <summary>
        /// 获取试题类型
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        protected string getQuesType(object[] id)
        {
            int type = 0;
            if (id.Length > 0)
                int.TryParse(id[0].ToString(), out type);
            if (types == null) types = WeiSha.Common.App.Get["QuesType"].Split(',');
            if (type < 1) return null;
            return types[type - 1].Trim();
        }
        /// <summary>
        /// 计算每道试题的分数
        /// </summary>
        /// <param name="ques">试题</param>
        /// <param name="total">试题的总分</param>
        /// <returns></returns>
        protected Song.Entities.Questions[] clacScore(Song.Entities.Questions[] ques, float total)
        {
            float surplus = total;
            for (int j = 0; j < ques.Length; j++)
            {
                ques[j].Qus_Explain = ques[j].Qus_Answer = ques[j].Qus_ErrorInfo = "";
                ques[j] = Extend.Questions.TranText(ques[j]);
                //当前试题的分数
                float curr = total / ques.Length;
                curr = ((float)Math.Round(curr * 10)) / 10;
                if (j < ques.Length - 1)
                {
                    ques[j].Qus_Number = curr;
                    surplus = surplus - curr;
                }
                else
                {
                    ques[j].Qus_Number = surplus;
                }
            }
            return ques;
        }
        /// <summary>
        /// 当前试题的选项，仅用于单选与多选
        /// </summary>
        /// <returns>0为没有子级，其它有子级</returns>
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
                    collectQues = Business.Do<IStudent>().CollectAll4Ques(st.Ac_ID, 0, couid, 0);
                }
                else
                {
                    collectQues = Business.Do<IStudent>().CollectAll4Ques(0, 0, couid, 0);
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
    }
}