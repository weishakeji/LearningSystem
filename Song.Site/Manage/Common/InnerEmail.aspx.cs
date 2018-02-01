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

namespace Song.Site.Manage.Common
{
    public partial class InnerEmail : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            ////总记录数
            //int count = 0;
            ////当前选择的用户id
            //int accid = Convert.ToInt16(this.ddlEmp.SelectedItem.Value);
            //DateTime start = DateTime.Now.AddYears(-100);
            //DateTime end = DateTime.Now.AddYears(100);
            //if (tbStart.Text.Trim() != "")
            //{
            //    start = Convert.ToDateTime(tbStart.Text.Trim());
            //}
            //if (this.tbEnd.Text.Trim() != "")
            //{
            //    end = Convert.ToDateTime(tbEnd.Text.Trim());
            //}
            //Logs[] eas = null;
            //eas = Business.Do<ILogs>().GetPager(accid, this.tbSear.Text.Trim(), "operate", start, end, Pager1.Size, Pager1.Index, out count);

            //GridView1.DataSource = eas;
            //GridView1.DataKeyNames = new string[] { "Log_Id" };
            //GridView1.DataBind();

            //Pager1.RecordAmount = count;
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
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
                //Business.Do<ILogs>().Delete(Convert.ToInt16(id));
            }
            BindData(null, null);
        }
    }
}
