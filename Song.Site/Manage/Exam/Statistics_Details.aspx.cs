using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Collections.Generic;

namespace Song.Site.Manage.Site
{
    public partial class Statistics_Details : Extend.CustomPage
    {
        //考试主题的id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0; 
        //各场次
        Song.Entities.Examination[] exams = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }

        void fill()
        {
            try
            {
                //考试主题的平均成绩
                Song.Entities.Examination theme = Business.Do<IExamination>().ExamSingle(id);
                if (theme == null) return;
                //各专业的平均成绩
                exams = Business.Do<IExamination>().ExamItem(theme.Exam_UID);
                rptExamItem.DataSource = exams;
                rptExamItem.DataBind();

                DataTable dt = Business.Do<IExamination>().Result4Theme(id);
                gvList.DataSource = dt;
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
           
        }
        /// <summary>
        /// 获取场次的平均分
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected double GetAvg(string id)
        {
            try
            {
                int mid = Convert.ToInt32(id);
                Song.Entities.Examination curr = null;
                if (exams != null)
                {
                    foreach (Song.Entities.Examination em in exams)
                    {
                        if (mid == em.Exam_ID)
                        {
                            curr = em;
                            break;
                        }
                    }
                }
                if (curr == null) return 0;
                double res = Business.Do<IExamination>().Avg4Exam(curr.Exam_ID);
                return Math.Round(res * 10000) / 10000;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return 0;
            }
        }
    }
}
