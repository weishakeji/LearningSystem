using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Xml;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 测试成绩回顾
    /// </summary>
    public class ExamReview : BasePage
    {
        //考试id
        int examid = WeiSha.Common.Request.QueryString["examid"].Int32 ?? 0;
        //考试成绩记录的Id
        int exrid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        int accid = 0;
        XmlDocument resXml = new XmlDocument();
        protected override void InitPageTemplate(HttpContext context)
        {
            accid = Extend.LoginState.Accounts.UserID;
            Song.Entities.ExamResults result = null;
            if (exrid != 0)
            {
                result = Business.Do<IExamination>().ResultSingle(exrid);
            }
            else
            {
                result = Business.Do<IExamination>().ResultSingle(accid,examid);
                if (result != null) exrid = result.Exr_ID;
            }

            if (result == null) return;
            Song.Entities.Examination exam = Business.Do<IExamination>().ExamSingle(result.Exam_ID);
            if (exam == null) return;
            //加载答题信息
            resXml.LoadXml(result.Exr_Results, false);
            //判断开始时间与结束时间，是否考试结束等
            bool isOver;
            //判断是否已经开始、是否已经结束
            if (exam.Exam_DateType == 1)
            {
                //固定时间开始               
                isOver = DateTime.Now > exam.Exam_Date.AddMinutes(exam.Exam_Span);   //是否结束
            }
            else
            {
                isOver = DateTime.Now > exam.Exam_DateOver;   //是否结束                         
                if (result != null && !string.IsNullOrWhiteSpace(result.Exr_Results))
                {
                    XmlNode xn = resXml.LastChild;
                    //考试的开始与结束时间，防止学员刷新考试界面，导致时间重置
                    long lover;
                    long.TryParse(xn.Attributes["overtime"] != null ? xn.Attributes["overtime"].Value : "0", out lover);
                    lover = lover * 10000;
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    DateTime overTime = dtStart.Add(new TimeSpan(lover));    //得到转换后的结束时间
                    isOver = DateTime.Now > overTime;
                }
            }
            this.Document.Variables.SetValue("isOver", isOver);
            //
            result.Exam_Name = exam.Exam_Name;
            this.Document.Variables.SetValue("result", result);
            //试卷
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle((int)result.Tp_Id);
            this.Document.Variables.SetValue("pager", tp);
            //考生
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle((int)result.Ac_ID);
            this.Document.Variables.SetValue("st", st);
            
            //获取试题项
            Song.Entities.TestPaperItem[] tpi = getTpi(tp);
            this.Document.Variables.SetValue("tpi", tpi);           
            //计算得分
            this.Document.RegisterGlobalFunction(this.getTypeName);
            this.Document.RegisterGlobalFunction(this.getTypeNumber);
            this.Document.RegisterGlobalFunction(this.getAnswerCount);
            this.Document.RegisterGlobalFunction(this.getSucessCount);
            this.Document.RegisterGlobalFunction(this.getErrorCount);
            //展示答题状态
            this.Document.RegisterGlobalFunction(this.getQues);
            this.Document.RegisterGlobalFunction(this.getItems);
            this.Document.RegisterGlobalFunction(this.getSucessAnswer);
            //计算答题状态等
            this.Document.RegisterGlobalFunction(this.getAnswerState);
            //获取学生答题内容与正确答案
            this.Document.RegisterGlobalFunction(this.getResult);
            this.Document.RegisterGlobalFunction(this.getQuesScore);
        }
        /// <summary>
        /// 获取试题的大项
        /// </summary>
        /// <returns></returns>
        private Song.Entities.TestPaperItem[] getTpi(Song.Entities.TestPaper tp)
        {
            Song.Entities.TestPaperItem[] tpi = Business.Do<ITestPaper>().GetItemForAny(tp);
            List<TestPaperItem> list = new List<TestPaperItem>();
            XmlNodeList quesNodes = resXml.LastChild.ChildNodes;    //当前答案中的试题分类项
            foreach (Song.Entities.TestPaperItem t in tpi)
            {
                for (int i = 0; i < quesNodes.Count; i++)
                {
                    int type = Convert.ToInt32(quesNodes[i].Attributes["type"].Value);
                    if (t.TPI_Type == type)
                    {
                        list.Add(t);
                        break;
                    }
                }
            }
            return list.ToArray();
        }
        #region 计算详细得分的方法
        /// <summary>
        /// 获取类型名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getTypeName(object[] id)
        {
            int type = 0;
            if (id.Length > 0 && id[0] is int) type = Convert.ToInt32(id[0]);
            return WeiSha.Common.App.Get["QuesType"].Split(',')[type-1];
        }
        /// <summary>
        /// 该类型得分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getTypeNumber(object[] id)
        {
            int type = 0;
            if (id.Length > 0 && id[0] is int) type = Convert.ToInt32(id[0]);
            XmlNode root = resXml.LastChild;
            XmlNodeList quesNodes = root.ChildNodes;
            double score = 0;
            for (int i = 0; i < quesNodes.Count; i++)
            {
                int tp = Convert.ToInt32(quesNodes[i].Attributes["type"].Value);
                if (tp == type)
                {
                    //小题的节点
                    XmlNodeList qnode = quesNodes[i].ChildNodes;
                    for (int j = 0; j < qnode.Count; j++)
                    {
                        if (qnode[j].Attributes["sucess"] != null)
                        {
                            bool isSuccess = Convert.ToBoolean(qnode[j].Attributes["sucess"].Value);
                            if (isSuccess)
                            {
                                double num = Convert.ToDouble(qnode[j].Attributes["num"].Value);
                                score += num;
                            }
                        }
                    }
                }

            }
            return score.ToString();
        }
        /// <summary>
        /// 答题数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getAnswerCount(object[] id)
        {
            int type = 0;
            if (id.Length > 0 && id[0] is int) type = Convert.ToInt32(id[0]);
            XmlNode root = resXml.LastChild;
            XmlNodeList quesNodes = root.ChildNodes;
            int count = 0;
            for (int i = 0; i < quesNodes.Count; i++)
            {
                int tp = Convert.ToInt32(quesNodes[i].Attributes["type"].Value);
                if (tp == type)
                {
                    //小题的节点
                    XmlNodeList qnode = quesNodes[i].ChildNodes;
                    for (int j = 0; j < qnode.Count; j++)
                    {
                        string ans="";
                        ans = type == 1 || type == 2 || type == 3 ? qnode[j].Attributes["ans"].Value : qnode[j].InnerText;
                        ans = ans.Replace(",","");
                        if (!(string.IsNullOrWhiteSpace(ans) || ans == "" || ans == "")) count++;
                    }
                }
            }
            return count.ToString();
        }
        /// <summary>
        /// 答对的数目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getSucessCount(object[] id)
        {
            int type = 0;
            if (id.Length > 0 && id[0] is int) type = Convert.ToInt32(id[0]);
            XmlNode root = resXml.LastChild;
            XmlNodeList quesNodes = root.ChildNodes;
            int count = 0;
            for (int i = 0; i < quesNodes.Count; i++)
            {
                int tp = Convert.ToInt32(quesNodes[i].Attributes["type"].Value);
                if (tp == type)
                {
                    //小题的节点
                    XmlNodeList qnode = quesNodes[i].ChildNodes;
                    for (int j = 0; j < qnode.Count; j++)
                    {
                        if (qnode[j].Attributes["sucess"] != null)
                        {
                            string sucess = qnode[j].Attributes["sucess"].Value;
                            if (sucess == "true") count++;
                        }
                    }
                }
            }
            return count.ToString();
        }
        /// <summary>
        /// 答错的数目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getErrorCount(object[] id)
        {
            int type = 0;
            if (id.Length > 0 && id[0] is int) type = Convert.ToInt32(id[0]);
            XmlNode root = resXml.LastChild;
            XmlNodeList quesNodes = root.ChildNodes;
            int count = 0;
            for (int i = 0; i < quesNodes.Count; i++)
            {
                int tp = Convert.ToInt32(quesNodes[i].Attributes["type"].Value);
                if (tp == type)
                {
                    //小题的节点
                    XmlNodeList qnode = quesNodes[i].ChildNodes;
                    for (int j = 0; j < qnode.Count; j++)
                    {
                        if (qnode[j].Attributes["sucess"] != null)
                        {
                            string sucess = qnode[j].Attributes["sucess"].Value;
                            if (sucess == "false") count++;
                        }
                    }
                }
            }
            return count.ToString();
        }
        #endregion

        #region 展示试题的方法
        /// <summary>
        /// 返回当前大题下的试题
        /// </summary>
        /// <param name="id">试题分类</param>
        /// <returns></returns>
        private List<Song.Entities.Questions> getQues(object[] id)
        {
            int type = 0;
            if (id.Length > 0 && id[0] is int) type = Convert.ToInt32(id[0]);
            List<Song.Entities.Questions> list = new List<Entities.Questions>();
            XmlNode root = resXml.LastChild;
            XmlNodeList quesNodes = root.ChildNodes;
            for (int i = 0; i < quesNodes.Count; i++)
            {
                int tp = Convert.ToInt32(quesNodes[i].Attributes["type"].Value);
                if (tp == type)
                {
                    //小题的节点
                    XmlNodeList qnode = quesNodes[i].ChildNodes;
                    for (int j = 0; j < qnode.Count; j++)
                    {
                        int qid = Convert.ToInt32(qnode[j].Attributes["id"].Value);
                        Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(qid);
                        if (ques == null) continue;
                        ques = Extend.Questions.TranText(ques);
                        ques.Qus_Title = Extend.Html.ClearHTML(ques.Qus_Title, "pre", "p");
                        ques.Qus_Explain = Extend.Html.ClearHTML(ques.Qus_Explain, "pre", "p");
                        ques.Qus_Title = ques.Qus_Title.Replace("&quot;","\"");
                        list.Add(ques);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 获取试题的答案选项
        /// </summary>
        /// <param name="id">uid</param>
        /// <returns></returns>
        private List<Song.Entities.QuesAnswer> getItems(object[] p)
        {
            Song.Entities.Questions qus = null;
            if (p.Length > 0)
                qus = (Song.Entities.Questions)p[0];
            //当前试题的答案
            Song.Entities.QuesAnswer[] ans = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
            List<Song.Entities.QuesAnswer> list = new List<Entities.QuesAnswer>();
            for (int i = 0; i < ans.Length; i++)
            {
                ans[i] = Extend.Questions.TranText(ans[i]);
                list.Add(ans[i]);
            }
            return list;
        }
        /// <summary>
        /// 显示试题正确答案
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private string getSucessAnswer(object[] p)
        {
            Song.Entities.Questions qus = null;
            if (p.Length > 0) qus = (Song.Entities.Questions)p[0];
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
            {
                ansStr = qus.Qus_IsCorrect ? "正确" : "错误";
            }
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
                    ansStr += (char)(65 + i) + "、" + ans[i].Ans_Context + "&nbsp;&nbsp;";
            }
            return ansStr;
        }
        #endregion

        #region 计算
        /// <summary>
        /// 计算答题状态，0为正确，1为错误，2为未答题
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string  getAnswerState(object[] obj)
        {
            string id = "";
            if (obj.Length > 0 && obj[0] is int) id = obj[0].ToString();
            XmlNode root = resXml.LastChild;
            XmlNodeList xnl = ((XmlElement)root).GetElementsByTagName("q");
            int state = 2;
            foreach (XmlNode q in xnl)
            {                
                string qid = q.Attributes["id"].Value;
                if (qid == id)
                {
                    int type = Convert.ToInt32(q.ParentNode.Attributes["type"].Value);
                    bool sucess=false;
                    if (q.Attributes["sucess"] != null)
                        bool.TryParse(q.Attributes["sucess"].Value,out sucess);
                    if (sucess) { state = 0; }
                    else
                    {
                        string ans = "";
                        //如果是填空与简答
                        if (type == 4 || type == 5)
                        {
                            ans = q.InnerText;
                        }
                        else
                        {
                            ans = q.Attributes["ans"].Value;
                        }
                        ans = ans.Replace(",", "");
                        if (!string.IsNullOrWhiteSpace(ans)) state = 1;
                    }
                    break;
                }
            }
            return state.ToString();
        }
        /// <summary>
        /// 获取用户答题信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string getResult(object[] obj)
        {
            string id = "";
            if (obj.Length > 0 && obj[0] is int) id = obj[0].ToString();
            XmlNode root = resXml.LastChild;
            XmlNodeList xnl = ((XmlElement)root).GetElementsByTagName("q");
            string ans = "";
            foreach (XmlNode q in xnl)
            {
                string qid = q.Attributes["id"].Value;
                if (qid == id)
                {
                    int type = Convert.ToInt32(q.ParentNode.Attributes["type"].Value);                   
                    //如果是填空与简答
                    if (type == 4 || type == 5)
                        ans = q.InnerText;
                    else
                        ans = q.Attributes["ans"].Value;
                    break;
                }
            }
            string[] tm = ans.Split(',');
            ans = "";
            for (int i = 0; i < tm.Length; i++)
            {
                if (tm[i].Trim() == "") continue;
                ans += tm[i] + "、";               
            }
            if (ans.IndexOf("、") > -1 && ans.Substring(ans.Length - 1) == "、")
            {
                ans = ans.Substring(0, ans.Length - 1);
            }                    
            return ans;
        }
        /// <summary>
        /// 获取试题的得分
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string getQuesScore(object[] obj)
        {
            string id = "";
            if (obj.Length > 0 && obj[0] is int) id = obj[0].ToString();
            XmlNode root = resXml.LastChild;
            XmlNodeList xnl = ((XmlElement)root).GetElementsByTagName("q");
            double score = 0;
            foreach (XmlNode q in xnl)
            {
                string qid = q.Attributes["id"].Value;
                if (qid == id)
                {
                    if (q.Attributes["score"] != null) 
                    score = Convert.ToDouble(q.Attributes["score"].Value);
                    break;
                }
            }
            return score.ToString();
        }
        #endregion
    }
}