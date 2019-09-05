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
    public partial class Message : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                tbStart.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                tbEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
                BindData(null,null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;               
            //查询字符
            string sear = tbSear.Text.Trim();
            DateTime startTime = DateTime.Now.AddYears(-100);
            //开始时间与结束时间
            if (tbStart.Text.Trim() != "")
            {
                startTime = Convert.ToDateTime(tbStart.Text.Trim());
            }
            DateTime endTime = DateTime.Now.AddYears(100);
            if (this.tbEnd.Text.Trim() != "")
            {
                endTime = Convert.ToDateTime(tbEnd.Text.Trim());
            }           
            Song.Entities.Message[] msg = null;
            msg = Business.Do<IMessage>().GetPager(org.Org_ID, 0, 0, sear, startTime, endTime, Pager1.Size, Pager1.Index, out count);

            GridView1.DataSource = msg;
            GridView1.DataKeyNames = new string[] { "Msg_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;

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
            foreach (string mid in keys.Split(','))
            {
                Business.Do<IMessage>().Delete(Convert.ToInt32(mid));
            }
            BindData(null, null);   
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<IMessage>().Delete(id);
            BindData(null, null);
        }
    }
}
