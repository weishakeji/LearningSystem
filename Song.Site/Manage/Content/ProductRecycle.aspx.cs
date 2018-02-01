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
    public partial class Recycle : Extend.CustomPage
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
            //Song.Entities.ProductsSort[] nc = Business.Do<IContents>().SortAll(true);
            //this.ddlColumn.DataSource = nc;
            //this.ddlColumn.DataTextField = "Ps_Name";
            //this.ddlColumn.DataValueField = "Ps_Id";
            //this.ddlColumn.DataBind();
            ////
            //this.ddlColumn.Items.Insert(0, new ListItem(" -- 所有分类 -- ", "-1"));
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
                Song.Entities.Product[] eas = null;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                eas = Business.Do<IContents>().ProductPager(org.Org_ID, col, this.tbSear.Text, true, Pager1.Size, Pager1.Index, out count);

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "Pd_id" };
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
                    Business.Do<IContents>().ProductDelete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        } /// <summary>
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
                Business.Do<IContents>().ProductDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 还原
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
                    Business.Do<IContents>().ProductRecover(Convert.ToInt16(id));
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
                Business.Do<IContents>().ProductRecover(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
