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

namespace Song.Site.Manage.Admin
{
    public partial class Sort : Extend.CustomPage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            if (!this.IsPostBack)
            {
                this.SearchBind(this.searchBox);
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
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.StudentSort[] eas = null;
            eas = Business.Do<IStudent>().SortPager(org.Org_ID, null, tbName.Text.Trim(), Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "sts_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;

        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            string query = this.SearchQuery(this.searchBox);
            Pager1.Qurey = query;
            Pager1.Index = 1;
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUseClick(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.StudentSort entity = Business.Do<IStudent>().SortSingle(id);
            entity.Sts_IsUse = !entity.Sts_IsUse;
            Business.Do<IStudent>().SortSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否默认组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbDefault_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            Business.Do<IStudent>().SortSetDefault(orgid, id);
            
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
            try
            {
                foreach (string id in keys.Split(','))
                    Business.Do<IStudent>().SortDelete(Convert.ToInt32(id));
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
            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            try
            {
                Business.Do<IStudent>().SortDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IStudent>().SortRemoveUp(orgid, id))
                BindData(null, null);
        }
        //下移
        protected void lbDown_Click(object sender, EventArgs e)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IStudent>().SortRemoveDown(orgid, id))                
                BindData(null, null); 
        }
    }
}
