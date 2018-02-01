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
    public partial class EmplyeeBook : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlDepartBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 院系下拉绑定
        /// </summary>
        private void ddlDepartBind()
        {
            try
            {
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(orgid,true,true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                //
                this.ddlDepart.Items.Insert(0, new ListItem(" -- 所有院系 -- ", "-1"));
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //当前选择的院系id
                int depId = Convert.ToInt16(ddlDepart.SelectedItem.Value);
                //
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.EmpAccount[] eas = null;
                eas = Business.Do<IEmployee>().GetAll(orgid,depId, true, this.tbSear.Text.Trim());
                foreach (Song.Entities.EmpAccount ea in eas)
                {
                    if (!ea.Acc_IsOpenTel)
                        ea.Acc_Tel = "";
                    if (!ea.Acc_IsOpenMobile)
                        ea.Acc_MobileTel = "";
                }
                this.rpList.DataSource = eas;
                rpList.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
