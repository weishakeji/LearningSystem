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

namespace Song.Site.Manage.Content
{
    public partial class NewsRecycle : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //Song.Entities.Position super = Business.Do<IPosition>().GetSuper();
                //superid = super.Posi_Id.ToString();
                ddlColumnBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 院系下拉绑定
        /// </summary>
        private void ddlColumnBind()
        {
            //Song.Entities.NewsColumn[] nc = Business.Do<IContents>().ColumnAll(true, true);
            //this.ddlColumn.DataSource = nc;
            //this.ddlColumn.DataTextField = "Nc_Name";
            //this.ddlColumn.DataValueField = "Nc_Id";
            //this.ddlColumn.DataBind();
            ////
            //this.ddlColumn.Items.Insert(0, new ListItem(" -- 所有栏目 -- ", "-1"));
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                //当前选择的栏目id
                int col = Convert.ToInt16(ddlColumn.SelectedItem.Value);
                Song.Entities.Article[] eas = null;
                //所属机构的所有课程
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                eas = Business.Do<IContents>().ArticlePager(org.Org_ID, col, null, true, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "na_id" };
                GridView1.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 批量删除
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
                    Business.Do<IContents>().ArticleDelete(Convert.ToInt16(id));
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
                Business.Do<IContents>().ArticleDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 批量还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RecoverEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<IContents>().ArticleRecover(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 单个还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRecover_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowRecover img = (WeiSha.WebControl.RowRecover)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IContents>().ArticleRecover(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
