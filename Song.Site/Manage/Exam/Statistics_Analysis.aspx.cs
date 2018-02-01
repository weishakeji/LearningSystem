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
    public partial class Statistics_Analysis : Extend.CustomPage
    {
        //考试主题的id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0; 
        //各场次
        //Song.Entities.Examination[] exams = null;
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
                //当前考试的所有成绩
                DataTable dt = null;
                if (theme.Exam_GroupType == 3)
                {
                    //dt = Business.Do<IExamination>().Analysis4Team(id);
                }
                else
                {
                    //dt = Business.Do<IExamination>().Analysis4Depart(id);
                }
                gvList.DataSource = dt;
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            
           
        }
        
    }
}
