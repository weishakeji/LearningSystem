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
    public partial class Special_Article : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                Song.Entities.Special mm = Business.Do<IContents>().SpecialSingle(id);
                if (mm != null)
                {
                    lbName.Text = mm.Sp_Name;
                }
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
                //总记录数
                int count = 0;
                //当前选择的栏目id
                //int col = Convert.ToInt16(ddlColumn.SelectedItem.Value);
                Song.Entities.Article[] eas = null;
                eas = Business.Do<IContents>().SpecialArticlePager(id, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
                //eas = Business.Do<IContents>().Special4Article(id,tbSear.Text);

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "art_id" };
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
        /// 从当前专题移除文章
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ltRemove_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton ub = (LinkButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int artid = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IContents>().SpecialAndArticleDel(id, artid);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
