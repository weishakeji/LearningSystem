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
    public partial class Employee : Extend.CustomPage
    {
        //超级管理员角色的id
        protected string superid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            Song.Entities.Position super = Business.Do<IPosition>().GetSuper();
            superid = super.Posi_Id.ToString();
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
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int orgid = org.Org_ID;
            Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(orgid, true, true);
            this.ddlDepart.DataSource = nc;
            this.ddlDepart.DataTextField = "dep_cnName";
            this.ddlDepart.DataValueField = "dep_id";
            this.ddlDepart.DataBind();
            //
            this.ddlDepart.Items.Insert(0, new ListItem(" -- 所有院系 -- ", "-1"));
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            //当前选择的院系id
            int depId = Convert.ToInt16(ddlDepart.SelectedItem.Value);
            bool? isUse = null;
            if (ddlIsUse.SelectedValue != "null")
            {
                isUse = Convert.ToBoolean(ddlIsUse.SelectedValue);
            }
            Song.Entities.Organization org;
            if (Extend.LoginState.Admin.IsSuperAdmin)
                org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
            else
                org = Business.Do<IOrganization>().OrganCurrent();
            EmpAccount[] eas = null;
            eas = Business.Do<IEmployee>().GetPager(org.Org_ID,depId, isUse, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);

            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "acc_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;

        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbOpenMobile_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.EmpAccount entity = Business.Do<IEmployee>().GetSingle(id);
            entity.Acc_IsOpenMobile = !entity.Acc_IsOpenMobile;
            Business.Do<IEmployee>().Save(entity);
            BindData(null, null);
    
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<IEmployee>().Delete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IEmployee>().Delete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
    }
}
