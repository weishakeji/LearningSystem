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

namespace Song.Site.Manage.Sys
{
    public partial class MenuTree : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Song.Entities.ManageMenu m = Business.Do<IManageMenu>().GetSingle(id);
                lbName.Text = m.MM_Name;
                BindData();
            }
        }
        /// <summary>
        /// 绑定菜单树列表，到下拉菜单
        /// </summary>
        protected void BindData()
        {            
            Song.Entities.ManageMenu[] mm;
            mm = Business.Do<IManageMenu>().GetRoot("func");
            //移动到……
            ddlMove.DataSource = mm;
            ddlMove.DataTextField = "MM_Name";
            ddlMove.DataValueField = "MM_Id";
            ddlMove.DataBind();
            ListItem li = ddlMove.Items.FindByValue(id.ToString());
            if (li != null) li.Selected = true;
            li.Attributes.Add("style", "background-color: #CCCCCC;");
            //复制到
            ddlCopy.DataSource = mm;
            ddlCopy.DataTextField = "MM_Name";
            ddlCopy.DataValueField = "MM_Id";
            ddlCopy.DataBind();
            ListItem li2 = ddlCopy.Items.FindByValue(id.ToString());
            if (li2 != null) li2.Selected = true;
            li2.Attributes.Add("style", "background-color: #CCCCCC;");           
        }
    }
}
