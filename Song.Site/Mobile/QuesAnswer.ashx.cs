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
    /// 试题答案
    /// </summary>
    public class QuesAnswer : BasePage
    {
        //试题id
        protected int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("qid", id);
            string answer = GetAnswer(id);
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                Response.Write(answer);
                Response.End();
            }
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                this.Document.SetValue("answer", answer);
            }
        }
        /// <summary>
        /// 试题的答案
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected string GetAnswer(int quesid)
        {
            //当前试题
            Song.Entities.Questions qus = null;
            qus = Business.Do<IQuestions>().QuesSingle4Cache(quesid);
            if (qus == null) qus = Business.Do<IQuestions>().QuesSingle(quesid);
            if (qus == null) return "";
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
    }
}