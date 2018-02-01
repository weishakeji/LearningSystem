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

namespace Song.Site.Manage.Teacher
{
    public partial class History : Extend.CustomPage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {  
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            int thid = Extend.LoginState.Accounts.Teacher.Th_ID;
            Song.Entities.TeacherHistory[] eas = null;
            eas = Business.Do<ITeacher>().HistoryAll(thid);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Thh_id" };
            GridView1.DataBind();
           
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
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
                Business.Do<ITeacher>().HistoryDelete(Convert.ToInt32(id));
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
            Business.Do<ITeacher>().HistoryDelete(id);
            BindData(null, null);           
        }
    }
}
