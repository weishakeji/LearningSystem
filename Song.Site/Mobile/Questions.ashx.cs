using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 单个试题
    /// </summary>
    public class Questions : BasePage
    {
        //试题id
        protected int qid = WeiSha.Common.Request.QueryString["qid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //题型
            string[] types = WeiSha.Common.App.Get["QuesType"].Split(',');            
            //试题
            Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(qid);
            if (ques == null) return;
            ques = Extend.Questions.TranText(ques);
            ques.Qus_Title = ques.Qus_Title.Replace("&lt;", "<");
            ques.Qus_Title = ques.Qus_Title.Replace("&gt;", ">");
            ques.Qus_Title = ques.Qus_Title.Replace("\n", "<br/>");
            ques.Qus_Title = Extend.Html.ClearHTML(ques.Qus_Title, "p", "div", "font");
            //
            this.Document.SetValue("q", ques);
            this.Document.SetValue("quesType", types[ques.Qus_Type-1]);
            this.Document.RegisterGlobalFunction(this.GetNote);
            this.Document.RegisterGlobalFunction(this.AnswerItems);
            this.Document.RegisterGlobalFunction(this.GetAnswer);
            this.Document.RegisterGlobalFunction(this.GetOrder);
        }
        /// <summary>
        /// 当前试题的笔记
        /// </summary>
        /// <returns></returns>
        protected object GetNote(object[] id)
        {
            int qid = 0;
            if (id.Length > 0)
                int.TryParse(id[0].ToString(), out qid);
            if (!Extend.LoginState.Accounts.IsLogin) return null;
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            //笔记
            Song.Entities.Student_Notes note = Business.Do<IStudent>().NotesSingle(qid, st.Ac_ID);
            return note;
        }
        /// <summary>
        /// 当前试题的选项，仅用于单选与多选
        /// </summary>
        /// <returns>0为没有子级，其它有子级</returns>
        protected object AnswerItems(object[] p)
        {
            Song.Entities.Questions qus= null;
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