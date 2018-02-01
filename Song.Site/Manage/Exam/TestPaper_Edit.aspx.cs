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

namespace Song.Site.Manage.Exam
{
    public partial class TestPaper_Edit : Extend.CustomPage
    {
        //试卷id
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //试卷类型，1为静态试卷，2为动态试卷
        private int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        //参数
        protected string query = string.Empty; 
        protected void Page_Load(object sender, EventArgs e)
        {
            query = this.ClientQueryString;   
            if (!this.IsPostBack)
            {
                string url = "TestPaper_Edit{0}.aspx?&type={0}&" + this.ClientQueryString;
                Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle(id);
                if (tp != null)
                {
                    url = string.Format(url, tp.Tp_Type, tp.Tp_Type);
                }
                else
                {
                    //统一试题的试卷还没有搞好，暂时先用这个
                    url = string.Format(url, 2, 2);
                }
                Response.Redirect(url);
            }
        }
    }
}
