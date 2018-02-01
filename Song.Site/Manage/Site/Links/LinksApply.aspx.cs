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

namespace Song.Site.Manage.Site.Links
{
    public partial class LinksApply : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlSortBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 栏目下拉绑定
        /// </summary>
        private void ddlSortBind()
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.LinksSort[] eas = Business.Do<ILinks>().GetSortAll(org.Org_ID, true, null);
            this.ddlSort.DataSource = eas;
            this.ddlSort.DataTextField = "Ls_Name";
            this.ddlSort.DataValueField = "Ls_Id";
            this.ddlSort.DataBind();
            //
            this.ddlSort.Items.Insert(0, new ListItem(" -- 所有分类 -- ", "-1"));
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //总记录数
            int count = 0;
            //当前选择的栏目id
            int col = Convert.ToInt16(this.ddlSort.SelectedItem.Value);
            Song.Entities.Links[] eas = null;
            eas = Business.Do<ILinks>().GetLinksPager(org.Org_ID, col, null, null, null, true, tbSear.Text.Trim(), Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "lk_id" };
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
            foreach (string id in keys.Split(','))
            {
                Business.Do<ILinks>().LinksDelete(Convert.ToInt16(id));
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
            Business.Do<ILinks>().LinksDelete(id);
            BindData(null, null);
        }
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnVeri_Click(object sender, ImageClickEventArgs e)
        {
            WeiSha.WebControl.RowVerify img = (WeiSha.WebControl.RowVerify)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Song.Entities.Links link = Business.Do<ILinks>().LinksSingle(id);
            Business.Do<ILinks>().LinksVerify(link);
            BindData(null, null);
        }
        #region 列表中的事件
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            StateShow ub = (StateShow)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Links entity = Business.Do<ILinks>().LinksSingle(id);
            entity.Lk_IsShow = !entity.Lk_IsShow;
            Business.Do<ILinks>().LinksSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateUse ub = (StateUse)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Links entity = Business.Do<ILinks>().LinksSingle(id);
            entity.Lk_IsUse = !entity.Lk_IsUse;
            Business.Do<ILinks>().LinksSave(entity);
            BindData(null, null);
        }
        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            //Company.WeiSha.WebControl.GridView gv = (Company.WeiSha.WebControl.GridView)gr.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            //当前选择的栏目id
            int col = Convert.ToInt16(this.ddlSort.SelectedItem.Value);
            Business.Do<ILinks>().LinksRemoveUp(id);
            BindData(null, null);
        }
        //下移
        protected void lbDown_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            //当前选择的栏目id
            int col = Convert.ToInt16(this.ddlSort.SelectedItem.Value);
            Business.Do<ILinks>().LinksRemoveDown(id);
            BindData(null, null);
        }
        #endregion
    }
}
