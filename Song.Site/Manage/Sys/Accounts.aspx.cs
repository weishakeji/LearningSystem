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
    public partial class Accounts : Extend.CustomPage
    {
        Song.Entities.Organization[] orgins = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = "";
            if (!this.IsPostBack)
            {
                ddlOrginBind();
                this.SearchBind(this.searchBox);               
                BindData(null, null);
            }
        }
        /// <summary>
        /// 机构下拉菜单
        /// </summary>
        private void ddlOrginBind()
        {
            if (orgins == null) orgins = Business.Do<IOrganization>().OrganCount(null, null, -1, -1);
            this.ddlOrgin.DataSource = orgins;
            this.ddlOrgin.DataTextField = "Org_Name";
            this.ddlOrgin.DataValueField = "Org_ID";
            this.ddlOrgin.DataBind();
            //
            this.ddlOrgin.Items.Insert(0, new ListItem(" -- 所有机构 -- ", "-1"));
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            int orgid = 0;
            int.TryParse(ddlOrgin.SelectedValue, out orgid);
            //总记录数
            int count = 0;
            Song.Entities.Accounts[] mm = Business.Do<IAccounts>().AccountsPager(orgid, -1, null, tbAccname.Text, tbName.Text, tbMobitel.Text, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = mm;
            GridView1.DataKeyNames = new string[] { "Ac_ID" };
            GridView1.DataBind();
            Pager1.RecordAmount = count;
        }
        #region 按钮事件    
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Qurey = this.SearchQuery(this.searchBox);
            Pager1.Index = 1;
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
                Business.Do<IAccounts>().AccountsDelete(Convert.ToInt32(id));
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
            Business.Do<IAccounts>().AccountsDelete(id);
            BindData(null, null);

        }
        #endregion

        #region 列表中的事件
        /// <summary>
        /// 获取账号所在机构名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected string GetOrgin(object obj)
        {
            if (orgins == null) orgins = Business.Do<IOrganization>().OrganCount(null, null, -1, -1);
            int orgid = 0;
            int.TryParse(obj.ToString(), out orgid);
            string orgname = string.Empty;
            foreach (Song.Entities.Organization o in orgins)
            {
                if (o.Org_ID == orgid)
                {
                    orgname = o.Org_Name;
                    break;
                }
            }
            return orgname;
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Accounts entity = Business.Do<IAccounts>().AccountsSingle(id);
            entity.Ac_IsUse = !entity.Ac_IsUse;
            Business.Do<IAccounts>().AccountsSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 注册审核是否通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbPass_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Accounts entity = Business.Do<IAccounts>().AccountsSingle(id);
            entity.Ac_IsPass = !entity.Ac_IsPass;
            Business.Do<IAccounts>().AccountsSave(entity);
            BindData(null, null);
        }
        #endregion
    }
}
