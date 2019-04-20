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
    /// 试题练习
    /// </summary>
    public class QuesExercisesItems : BasePage
    {
        //章节id
        protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //试题的启始索引与当前取多少条记录       
        protected int count = WeiSha.Common.Request.QueryString["count"].Int32 ?? 100;       
        //当前学员收藏的试题
        Song.Entities.Questions[] collectQues = null;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("couid", couid);
            //当前章节
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            this.Document.SetValue("outline", outline);
            //当前课程
            Song.Entities.Course course = null;
            if (outline != null)
            {
                couid = outline.Cou_ID;
                course = Business.Do<ICourse>().CourseSingle(couid);
                this.Document.SetValue("course", course);
            }
            if (course == null && couid > 0) course = Business.Do<ICourse>().CourseSingle(couid);
            //是否购买该课程
            int accid = 0;
            if (Extend.LoginState.Accounts.IsLogin) accid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
            Song.Entities.Course couBuy = Business.Do<ICourse>().IsBuyCourse(couid, accid, 1);
            bool isBuy = couBuy != null;
            //是否购买，如果免费也算已经购买
            this.Document.SetValue("isBuy", isBuy || course.Cou_IsFree);
            if (couBuy == null) couBuy = course;
            this.Document.SetValue("couBuy", couBuy);
            //是否可以学习,如果是免费或已经选修便可以学习，否则当前课程允许试用且当前章节是免费的，也可以学习
            bool canStudy = isBuy || course.Cou_IsFree || course.Cou_IsLimitFree ? true : (course.Cou_IsTry && outline.Ol_IsFree && outline.Ol_IsFinish);
            canStudy = canStudy && outline.Ol_IsUse && outline.Ol_IsFinish && this.Account != null;
            this.Document.Variables.SetValue("canStudy", canStudy);
            if (!canStudy) return;
            //总题数
            int sumCount = canStudy  ? Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, olid, -1, true) : 0;
            //this.Document.SetValue("sumCount", sumCount);
            int total = Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, olid, -1, true);
            this.Document.SetValue("Total", total);  //试题的总数
            this.Document.SetValue("couid", WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0);
            //题型
            this.Document.SetValue("quesType", WeiSha.Common.App.Get["QuesType"].Split(','));
            //试题列表
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().CacheQuestions("all");
            List<Song.Entities.Questions> list = new List<Entities.Questions>();
            //按章节取题
            if (olid > 0)
            {
                List<int> olids = Business.Do<IOutline>().TreeID(olid);
                list = GetQues(olids, ques);
            }
            else
            {
                //按钮课程取题
                foreach (Song.Entities.Questions q in ques)
                {
                    if (q.Cou_ID != couid) continue;
                    list.Add(q);
                }
            }
            ques = list.ToArray();
            if (ques == null)
            {
                ques = Business.Do<IQuestions>().QuesCount(-1, -1, couid, olid, -1, -1, true, 0 - 1, count);
            }
            //清理试题文本格式
            for (int i = 0; i < ques.Length; i++)
            {
                ques[i] = Extend.Questions.TranText(ques[i]);
                ques[i].Qus_Title = ques[i].Qus_Title.Replace("&lt;", "<");
                ques[i].Qus_Title = ques[i].Qus_Title.Replace("&gt;", ">");
                ques[i].Qus_Title = Extend.Html.ClearHTML(ques[i].Qus_Title, "p", "div", "font", "span", "a");
                ques[i].Qus_Explain = Extend.Html.ClearHTML(ques[i].Qus_Explain, "p", "div", "font", "span", "a");
                ques[i].Qus_Answer = Extend.Html.ClearHTML(ques[i].Qus_Answer, "p", "div", "font", "span", "a");
                ques[i].Qus_Title = ques[i].Qus_Title.Replace("\n", "<br/>");
                if (!string.IsNullOrWhiteSpace(ques[i].Qus_Answer))
                    ques[i].Qus_Answer = ques[i].Qus_Answer.Replace("&nbsp;", " ");
            }
            //List<Song.Entities.Questions> list = new List<Entities.Questions>();
            //if (isBuy || course.Cou_IsFree) list = ques.ToList<Song.Entities.Questions>();  //如果已经购买或免费；
            //if (!(isBuy || course.Cou_IsFree))
            //{
            //    for (int i = 0; i < sumCount; i++) list.Add(ques[i]);   //如果还在试用
            //}
            //this.Document.SetValue("uid", uid);           
            this.Document.SetValue("ques", ques);

            this.Document.RegisterGlobalFunction(this.AnswerItems);
            this.Document.RegisterGlobalFunction(this.IsCollect);
            this.Document.RegisterGlobalFunction(this.GetAnswer);
        }

        /// <summary>
        /// 按章节输出试题
        /// </summary>
        /// <param name="olid">多个章节id</param>
        /// <returns></returns>
        private List<Song.Entities.Questions> GetQues(List<int> olid, Song.Entities.Questions[] ques)
        {

            List<Song.Entities.Questions> list = new List<Entities.Questions>();
            foreach (int id in olid)
            {
                foreach (Song.Entities.Questions q in ques)
                {
                    if (q.Ol_ID != id || q.Qus_IsUse == false || q.Qus_IsError == true || q.Qus_IsWrong == true) continue;
                    list.Add(q);
                }
            }
            return list;
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
        /// 试题的答案
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        protected string GetAnswer(object[] objs)
        {
            //当前试题
            Song.Entities.Questions qus = null;
            if (objs.Length > 0) qus = objs[0] as Song.Entities.Questions;            
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
                if (ansStr.Length > 0 && ansStr.EndsWith("、")) ansStr = ansStr.Substring(0, ansStr.Length - 1);               
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