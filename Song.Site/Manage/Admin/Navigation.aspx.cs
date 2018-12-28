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
    public partial class Navigation : Extend.CustomPage
    {
        //导航分类
        protected string type = WeiSha.Common.Request.QueryString["type"].String;
        //所归属的站点类型
        private string site = WeiSha.Common.Request.QueryString["site"].String ?? "web";
        //资料的所在路径
        private string _uppath = "Org";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //如果是手机端导航，只留一个导航分类
                //if (site == "mobi")
                //{
                //    if (rblType.Items.Count > 0)
                //    {
                //        for (int i = 1; i < rblType.Items.Count; i++)
                //        {
                //            rblType.Items[i].Attributes.Add("style", "display:none");
                //        }
                //    }
                //}
                init();
                BindData(null, null);
            }
        }
        private void init()
        {
            ListItem litype = rblType.Items.FindByValue(type);
            if (litype != null)
            {
                rblType.SelectedIndex = -1;
                litype.Selected = true;
            }
            else
            {
                rblType.SelectedIndex = 0;
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                rblType_SelectedIndexChanged(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(type)) return;
            
            Song.Entities.Organization org = null;
            org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Navigation[] navi = Business.Do<IStyle>().NaviAll(null, site, type, org.Org_ID);
            string path = Upload.Get[_uppath].Virtual;
            foreach(Song.Entities.Navigation n in navi){
                n.Nav_Logo = path + n.Nav_Logo;
            }
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(navi);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Nav_ID";
            tree.ParentIdKeyName = "Nav_PID";
            tree.TaxKeyName = "Nav_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);
            //绑定
            GridView1.DataSource = dt;
            GridView1.DataKeyNames = new string[] { "Nav_ID" };
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
                Business.Do<IStyle>().NaviDelete(Convert.ToInt32(id));
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
            Business.Do<IStyle>().NaviDelete(id);
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
            Song.Entities.Navigation entity = Business.Do<IStyle>().NaviSingle(id);
            entity.Nav_IsShow = !entity.Nav_IsShow;
            Business.Do<IStyle>().NaviSave(entity);
            BindData(null, null);
        }
        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            //Company.CustomControl.GridView gv = (Company.CustomControl.GridView)gr.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);

            if (Business.Do<IStyle>().NaviRemoveUp(id))
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
            if (Business.Do<IStyle>().NaviRemoveDown(id))
            {
                BindData(null, null);
            }
            else
            {
                //Alert("该项已经处于其所属分类的最底端，无法下移！");
            }
        }
        #endregion

        /// <summary>
        /// 菜单类别切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //当前选择的菜单分类
            string type = rblType.SelectedItem.Value;
            //切换            
            string fileName = Path.GetFileName(this.Request.FilePath);
            this.Response.Redirect(fileName + "?site=" + site + "&type=" + type);    
           
           
        }
    }
}
