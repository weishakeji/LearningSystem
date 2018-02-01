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
    public partial class Product : Extend.CustomPage
    {
        //栏目分类
        int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
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
            try
            {
                //总记录数
                int count = 0;
                Song.Entities.Product[] eas = null;
                bool isNew = cbIsNew.Checked;
                bool isRec = cbIsRec.Checked;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                eas = Business.Do<IContents>().ProductPager(org.Org_ID, colid, this.tbSear.Text, false, null, isNew, isRec, "rec", Pager1.Size, Pager1.Index, out count);

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
        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (Business.Do<IContents>().ProductUp(org.Org_ID, id))
                {
                    BindData(null, null);
                }
                else
                {
                    Alert("该项已经处于其所属分类的最顶端，无法上移！");
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        //下移
        protected void lbDown_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (Business.Do<IContents>().ProductDown(org.Org_ID, id))
                {
                    BindData(null, null);
                }
                else
                {
                    Alert("该项已经处于其所属分类的最底端，无法下移！");
                }
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
                Song.Entities.Product entity = Business.Do<IContents>().ProductSingle(id);
                entity.Pd_IsUse = !entity.Pd_IsUse;
                Business.Do<IContents>().ProductSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改是否是新产品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbNew_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.Product entity = Business.Do<IContents>().ProductSingle(id);
                entity.Pd_IsNew = !entity.Pd_IsNew;
                Business.Do<IContents>().ProductSave(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbRec_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.Product entity = Business.Do<IContents>().ProductSingle(id);
                entity.Pd_IsRec = !entity.Pd_IsRec;
                Business.Do<IContents>().ProductSave(entity);
                BindData(null, null);
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
                    Business.Do<IContents>().ProductIsDelete(Convert.ToInt16(id));
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
                Business.Do<IContents>().ProductIsDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
