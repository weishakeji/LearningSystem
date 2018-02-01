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
    public partial class Organization : Extend.CustomPage
    {
        //根域名
        protected string domain = WeiSha.Common.Request.Domain.MainName;
        //端口
        protected string port = WeiSha.Common.Server.Port;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(domain)||domain.Trim()=="") domain = "www";
            if (!this.IsPostBack)
            {
                init();
                BindData(null, null);
            }
        }
        private void init()
        {
            Song.Entities.OrganLevel[] orglv = Business.Do<IOrganization>().LevelAll(true);
            ddlLevel.DataSource = orglv;
            ddlLevel.DataTextField = "olv_name";
            ddlLevel.DataValueField = "olv_id";
            ddlLevel.DataBind();
            ddlLevel.Items.Insert(0,new ListItem("-- 所有 --","-1"));
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            int level = Convert.ToInt32(ddlLevel.SelectedValue == "" ? "-1" : ddlLevel.SelectedValue);
            Song.Entities.Organization[] orgs = Business.Do<IOrganization>().OrganAll(null, level);
            GridView1.DataSource = orgs;
            GridView1.DataKeyNames = new string[] { "Org_ID" };
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
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<IOrganization>().OrganDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
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
                Business.Do<IOrganization>().OrganDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.Organization entity = Business.Do<IOrganization>().OrganSingle(id);
                entity.Org_IsUse = !entity.Org_IsUse;
                Business.Do<IOrganization>().OrganSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改是否显示在前端的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.Organization entity = Business.Do<IOrganization>().OrganSingle(id);
                entity.Org_IsShow = !entity.Org_IsShow;
                Business.Do<IOrganization>().OrganSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 是否通过审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbPass_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.Organization entity = Business.Do<IOrganization>().OrganSingle(id);
                entity.Org_IsPass = !entity.Org_IsPass;
                Business.Do<IOrganization>().OrganSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 设置默认机构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbDefault_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IOrganization>().OrganSetDefault(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        #region 列表中的事件
        /// <summary>
        /// 获取管理员的状态
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public string GetAdminState(object orgid)
        {
            //根据公司id获取管理员。
            Song.Entities.EmpAccount admin = Business.Do<IEmployee>().GetAdminByOrgId(Convert.ToInt32(orgid));
            //获取当前分厂的员工
            Song.Entities.EmpAccount[] emps = Business.Do<IEmployee>().GetAll4Org(Convert.ToInt32(orgid),true,"");
            //如果有管理员
            if (admin != null) return admin.Acc_Name;
            //如果没有管理员，但有其它员工
            if (emps.Length > 0) return "【设置管理员】";

            return "【添加】";
        }

        #endregion
    }
}
