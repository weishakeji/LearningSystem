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
    public partial class TestArchives : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = this.Master.Account;
            if (st == null) return;
            //this.Form.DefaultButton = this.btnSear.UniqueID;
            if (!IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //总记录数
            int count = 0;
            Song.Entities.Accounts st = this.Master.Account;
            if (st == null) return;
            Song.Entities.TestResults[] trs = null;
            trs = Business.Do<ITestPaper>().ResultsPager(st.Ac_ID, -1, -1, "", Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = trs;
            GridView1.DataKeyNames = new string[] { "Tr_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {            
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<ITestPaper>().ResultsDelete(Convert.ToInt16(id));
            }
            BindData(null, null);
            
        }
    }
}
