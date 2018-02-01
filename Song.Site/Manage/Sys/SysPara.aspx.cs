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

namespace Song.Site.Manage.Sys
{
    public partial class SysPara : Extend.CustomPage
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
            string key = tbKey.Text.Trim();
            string intro = tbIntro.Text.Trim();
            DataTable mm = Business.Do<ISystemPara>().GetAll(key, intro);
            GridView1.DataSource = mm;
            GridView1.DataKeyNames = new string[] { "Sys_Id" };
            GridView1.DataBind();
          
        }
        #region 按钮事件       
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
                Business.Do<ISystemPara>().Delete(Convert.ToInt32(id));
            }
            BindData(null, null);
        }
        #endregion  
    }
}
