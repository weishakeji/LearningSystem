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
    public partial class News : Extend.CustomPage
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
            //总记录数
            int count = 0;
            Song.Entities.Article[] eas = null;
            //条件
            //是否置顶，如果没有选择，则为空
            bool? istop = !cbIsTop.Checked ? null : (bool?)true;
            bool? ishot = !cbIsHot.Checked ? null : (bool?)true;
            bool? isrec = !cbIsRec.Checked ? null : (bool?)true;
            bool? isimg = !cbIsImg.Checked ? null : (bool?)true;
            //
            //所属机构的所有课程
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            eas = Business.Do<IContents>().ArticlePager(org.Org_ID, colid, tbSear.Text.Trim(), null, false,
                istop, ishot, isrec, isimg,
                Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "art_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;

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
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<IContents>().ArticleIsDelete(Convert.ToInt16(id));
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
            Business.Do<IContents>().ArticleDelete(id);
            BindData(null, null);
        }
        #region 行内操作

        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Article entity = Business.Do<IContents>().ArticleSingle(id);
            entity.Art_IsShow = !entity.Art_IsShow;
            Business.Do<IContents>().ArticleSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 修改时间为最新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCrtTime_Click(object sender, EventArgs e)
        {
            LinkButton ub = (LinkButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Article entity = Business.Do<IContents>().ArticleSingle(id);
            entity.Art_CrtTime = DateTime.Now;
            Business.Do<IContents>().ArticleSave(entity);
            BindData(null, null);
        }
        #endregion

        
    }
}
