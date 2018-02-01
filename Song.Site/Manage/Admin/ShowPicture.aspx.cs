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
using System.IO;

namespace Song.Site.Manage.Admin
{
    public partial class ShowPicture : Extend.CustomPage
    {        
        //所归属的站点类型
        private string site = WeiSha.Common.Request.QueryString["site"].String ?? "web";
        //资料的所在路径
        private string _uppath = "ShowPic";
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
            if (string.IsNullOrWhiteSpace(site)) return;
            
            Song.Entities.Organization org = null;
            org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.ShowPicture[] shows = Business.Do<IStyle>().ShowPicAll(null, site, org.Org_ID);
            string path = Upload.Get[_uppath].Virtual;
            foreach (Song.Entities.ShowPicture n in shows)
            {
                n.Shp_File = path + n.Shp_File;
            }
            //绑定
            GridView1.DataSource = shows;
            GridView1.DataKeyNames = new string[] { "Shp_ID" };
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
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<IStyle>().ShowPicDelete(Convert.ToInt32(id));
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
            Business.Do<IStyle>().ShowPicDelete(id);
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
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.ShowPicture entity = Business.Do<IStyle>().ShowPicSingle(id);
            entity.Shp_IsShow = !entity.Shp_IsShow;
            Business.Do<IStyle>().ShowPicSave(entity);
            BindData(null, null);
        }
        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            //Company.CustomControl.GridView gv = (Company.CustomControl.GridView)gr.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);

            if (Business.Do<IStyle>().ShowPicUp(id))
            {
                BindData(null, null);
                
            }
            else
            {
                //Alert("该项已经处于其所属分类的最顶端，无法上移！");
                //GridView1.Rows[0]
            }
        }
        //下移
        protected void lbDown_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IStyle>().ShowPicDown(id))
            {
                BindData(null, null);
            }
            else
            {
                //Alert("该项已经处于其所属分类的最底端，无法下移！");
            }
        }
        #endregion

    }
}
