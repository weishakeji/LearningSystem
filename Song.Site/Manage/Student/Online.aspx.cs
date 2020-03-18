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
using System.IO;

namespace Song.Site.Manage.Student
{
    public partial class Online : Extend.CustomPage
    {
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = this.Master.Account;
            if (st == null) return;
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                init();
                BindData(null, null);
            }
        }
        private void init()
        {
            DateTime now = DateTime.Now;
            DateTime d1 = new DateTime(now.Year, now.Month, 1);
            tbStartTime.Text = d1.ToString("yyyy-MM-dd");
            DateTime d2 = d1.AddMonths(1).AddDays(-1);
            tbEndTime.Text = d2.ToString("yyyy-MM-dd");

        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            DateTime? start = (DateTime?)Convert.ToDateTime(tbStartTime.Text);
            DateTime? end = (DateTime?)Convert.ToDateTime(tbEndTime.Text);
            int stid = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
            //
            Song.Entities.LogForStudentOnline[] eas = null;
            eas = Business.Do<IStudent>().LogForLoginPager(org.Org_ID, stid, "", start, end, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Lso_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }        
    }
}
