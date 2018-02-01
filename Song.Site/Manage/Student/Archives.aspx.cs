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


namespace Song.Site.Manage.Student
{
    public partial class Archives : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            if (!IsPostBack)
            {
                BindData(null, null);
            }
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //总记录数
            int count = 0;
            Song.Entities.Accounts acc = this.Master.Account;
            if (acc == null) return;          
            Song.Entities.ExamResults[] eas = null;
            eas = Business.Do<IExamination>().GetAttendPager(acc.Ac_ID, -1,-1, tbSear.Text.Trim(), Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Exr_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
           
        }
        
    }
}
