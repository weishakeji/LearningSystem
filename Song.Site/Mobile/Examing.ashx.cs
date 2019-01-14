using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using System.Xml;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 在线考试场景（手机端）
    /// </summary>
    public class Examing : BasePage
    {
        //当前考试的ID
        int examid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //考试的状态
        protected Examing_State state = null;
        protected override void InitPageTemplate(HttpContext context)
        {
            //删除考试成绩
            if (WeiSha.Common.Request.QueryString["again"].Int16 != null)
            {
                //删除考试成绩记录，以及清除缓存
                Business.Do<IExamination>().ResultDelete(Account.Ac_ID, examid);
                this.Response.Redirect(string.Format("examing.ashx?id={0}&rnd={1}", examid, DateTime.Now.Ticks));
                return;
            }
            #region 判断是否登录
            state = new Examing_State(this.Document);
            //如果未登录
            if (!Extend.LoginState.Accounts.IsLogin || this.Account == null)
            {
                state.Set(false, -1);   //
                return;
            }
            #endregion

            #region 当前考试信息
            //当前考试的场次
            Song.Entities.Examination exam = Business.Do<IExamination>().ExamSingle(examid);
            if (exam == null || !exam.Exam_IsUse || exam.Exam_IsTheme)
            {
                state.Set(false, 0);    //没有考试，状态为0;
                return;
            }
            this.Document.SetValue("exam", exam);
            this.Document.SetValue("uid", exam.Exam_UID);
            //当前考试的主题
            Song.Entities.Examination theme = Business.Do<IExamination>().ExamSingle(exam.Exam_UID);
            this.Document.SetValue("theme", theme);
            //当前试卷
            Song.Entities.TestPaper pager = Business.Do<ITestPaper>().PagerSingle(exam.Tp_Id);
            this.Document.SetValue("pager", pager);
            #endregion

            #region 是否允许参加考试
            //当前考生是否可以参加该场考试
            bool isAllow = Business.Do<IExamination>().ExamIsForStudent(examid, Account.Ac_ID);
            if (!isAllow)
            {
                state.Set(false, 2);    //不需要参加考试，状态2
                return;
            }
            #endregion

            #region 判断是否开始、结束，是否交卷
            bool isStart, isOver, isSubmit;
            DateTime startTime, overTime;
            //答题记录
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultSingleForCache(examid, exam.Tp_Id, Account.Ac_ID);
            //判断是否已经开始、是否已经结束
            if (exam.Exam_DateType == 1)
            {
                //固定时间开始
                isStart = exam.Exam_Date <= DateTime.Now;    //是否开始
                isOver = DateTime.Now > exam.Exam_Date.AddMinutes(exam.Exam_Span);   //是否结束
                startTime = exam.Exam_Date;           //开始时间
                overTime = exam.Exam_Date.AddMinutes(exam.Exam_Span);     //结束时间
                isSubmit = exr != null ? exr.Exr_IsSubmit : false;    //是否交卷
            }
            else
            {
                //按时间区间
                isStart = DateTime.Now > exam.Exam_Date && DateTime.Now < exam.Exam_DateOver;    //是否开始
                isOver = DateTime.Now > exam.Exam_DateOver;   //是否结束
                startTime = exam.Exam_Date <= DateTime.Now ? DateTime.Now : exam.Exam_Date;        //开始时间
                overTime = exam.Exam_DateOver.AddMinutes(exam.Exam_Span);     //结束时间
                isSubmit = exr != null ? exr.Exr_IsSubmit : false;    //是否交卷               
                if (exr != null && !string.IsNullOrWhiteSpace(exr.Exr_Results))
                {
                    XmlDocument resXml = new XmlDocument();
                    resXml.LoadXml(exr.Exr_Results, false);
                    XmlNode xn = resXml.LastChild;
                    //考试的开始与结束时间，防止学员刷新考试界面，导致时间重置
                    long lbegin, lover;
                    long.TryParse(xn.Attributes["begin"] != null ? xn.Attributes["begin"].Value : "0", out lbegin);
                    long.TryParse(xn.Attributes["overtime"] != null ? xn.Attributes["overtime"].Value : "0", out lover);
                    lbegin = lbegin * 10000;
                    lover = lover * 10000;
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    DateTime beginTime = dtStart.Add(new TimeSpan(lbegin));
                    overTime = dtStart.Add(new TimeSpan(lover));    //得到转换后的结束时间
                    startTime = exam.Exam_Date <= beginTime ? beginTime : exam.Exam_Date;        //开始时间
                    isOver = DateTime.Now > overTime;
                    isSubmit = DateTime.Now > overTime || exr.Exr_IsSubmit; //是否交卷
                }
            }
            this.Document.SetValue("isStart", isStart.ToString().ToLower());
            this.Document.SetValue("isOver", isOver.ToString().ToLower());
            this.Document.SetValue("startTime", WeiSha.Common.Server.getTime(startTime));
            this.Document.SetValue("overTime", WeiSha.Common.Server.getTime(overTime)); //转成时间戳
            this.Document.SetValue("isSubmit", isSubmit.ToString().ToLower());
            if (!isStart) state.Set(true, 4);  //还没有开始
            if (isOver) state.Set(false, 1);    //考试已经结束，状态为1;
            if (isSubmit) state.Set(false, 3);  //已经参加过考试            
            if (startTime <= DateTime.Now && overTime > DateTime.Now && !isSubmit)
                state.Set(true, 5); //正在考试

            #endregion

            //基本参数，uid,服务器端时间           
            this.Document.SetValue("servertime", WeiSha.Common.Server.getTime()); 
            this.Document.RegisterGlobalFunction(this.getSubjectPath);    //专业的上级专业等
            //试题类型
            this.Document.SetValue("types", WeiSha.Common.App.Get["QuesType"].String);
        }
        /// <summary>
        /// 获取当前专业的上级专业
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected string getSubjectPath(object[] para)
        {
            int sbjid = 0;
            if (para.Length > 0 && para[0] is int)
                int.TryParse(para[0].ToString(), out sbjid);
            string sbjstr = "";
            Song.Entities.Subject s = Business.Do<ISubject>().SubjectSingle(sbjid);
            if (s != null)
            {
                sbjstr += s.Sbj_Name;
                while (s.Sbj_PID != 0)
                {
                    s = Business.Do<ISubject>().SubjectSingle(s.Sbj_PID);
                    if (s == null) break;
                    sbjstr = s.Sbj_Name + " &gt;&gt; " + sbjstr;
                }
            }
            return sbjstr;
        }

    }

    /// <summary>
    /// 考试状态
    /// </summary>
    public class Examing_State
    {
        /// <summary>
        /// 是否确认可以考试
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 考试状态，-2异常；-1未登录，0考试不存在，1考试已经结束，2不需要参加，3已经参加过考试（交过卷），4未开始，5正在考试
        /// </summary>
        public int State { get; set; }
        protected VTemplate.Engine.TemplateDocument Document { get; set; }
        public Examing_State(VTemplate.Engine.TemplateDocument document)
        {
            this.Document = document;
            this.IsEnable = false;
            this.State = -2;
            this.Document.SetValue("state", this);
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="state"></param>
        public void Set(bool isEnable, int state)
        {
            this.IsEnable = isEnable;
            this.State = state;
            this.Document.SetValue("state", this);
        }
    } 
}