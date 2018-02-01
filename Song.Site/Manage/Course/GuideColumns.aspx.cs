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

namespace Song.Site.Manage.Course
{
    public partial class GuideColumns : Extend.CustomPage
    {
        //课程id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
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
            Song.Entities.GuideColumns[] eas = Business.Do<IGuide>().GetColumnsAll(couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(eas);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Gc_ID";
            tree.ParentIdKeyName = "Gc_PID";
            tree.TaxKeyName = "Gc_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);            
            gvColumns.DataSource = dt;
            gvColumns.DataKeyNames = new string[] { "Gc_ID" };
            gvColumns.DataBind();
            
        }
        #region 新增区域的按钮
        /// <summary>
        /// 添加新栏目的按钮，此处只是进处添加的界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            plAddColumn.Visible = true;
            gvColumns.Visible = false;
            tbTitle.Text = "";
            //上级
            Song.Entities.GuideColumns[] cous = Business.Do<IGuide>().GetColumnsAll(couid,null);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Gc_title";
            this.ddlTree.DataValueField = "Gc_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            ddlTree.Items.Insert(0, new ListItem("   -- 顶级 --", "0"));
        }
        /// <summary>
        /// 退出新增环境
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddBack_Click(object sender, EventArgs e)
        {
            plAddColumn.Visible = false;
            gvColumns.Visible = true;
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.GuideColumns col = new Song.Entities.GuideColumns();
            col.Gc_Title = tbTitle.Text.Trim();
            col.Gc_PID = Convert.ToInt32(ddlTree.SelectedValue);
            col.Cou_ID = couid;
            col.Gc_Intro = tbIntro.Text.Trim();
            col.Gc_IsUse = cbIsUse.Checked;
            Business.Do<IGuide>().ColumnsAdd(col);
            BindData(null, null);
            gvColumns.EditIndex = -1;
            btnAddBack_Click(null, null);
        }
        
        #endregion

        #region Gridview 行内事件
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {

            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvColumns.DataKeys[index].Value.ToString());
            Business.Do<IGuide>().ColumnsDelete(id);
            BindData(null, null);
        }
        /// <summary>
        /// 进入编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {

            WeiSha.WebControl.RowEdit img = (WeiSha.WebControl.RowEdit)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;                      
            gvColumns.EditIndex = index;
            BindData(null, null);            
            //绑定树
            DropDownTree tree = (DropDownTree)gvColumns.Rows[index].FindControl("ddlColTree");
            Song.Entities.GuideColumns[] cous = Business.Do<IGuide>().GetColumnsAll(couid, null);
            tree.DataSource = cous;
            tree.DataTextField = "Gc_title";
            tree.DataValueField = "Gc_ID";
            tree.Root = 0;
            tree.DataBind();
            tree.Items.Insert(0, new ListItem("   -- 顶级 --", "0"));
            //当前父级
            int pid = Convert.ToInt32(img.CommandArgument);
            ListItem li = tree.Items.FindByValue(pid.ToString());
            if (li != null) li.Selected = true;
        }
        /// <summary>
        /// 编辑当前数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditEnter_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
            int index = ((GridViewRow)(btn.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvColumns.DataKeys[index].Value.ToString());  
            //
            Song.Entities.GuideColumns col = Business.Do<IGuide>().ColumnsSingle(id);
            if (col != null)
            {
                //名称
                TextBox tb = (TextBox)gvColumns.Rows[index].FindControl("tbTitle");
                col.Gc_Title = tb.Text.Trim();
                //父级
                DropDownTree tree = (DropDownTree)gvColumns.Rows[index].FindControl("ddlColTree");
                col.Gc_PID = Convert.ToInt32(tree.SelectedValue);
                //是否可用
                CheckBox cb = (CheckBox)gvColumns.Rows[index].FindControl("cbIsUse");
                col.Gc_IsUse = cb.Checked;
                //简介
                TextBox tbintro = (TextBox)gvColumns.Rows[index].FindControl("tbIntro");
                col.Gc_Intro = tbintro.Text.Trim();
                //
                Business.Do<IGuide>().ColumnsSave(col);
            }
            gvColumns.EditIndex = -1;
            BindData(null, null);
        }
        /// <summary>
        /// 退出编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditBack_Click(object sender, EventArgs e)
        {
            gvColumns.EditIndex = -1;
            BindData(null, null);
        }
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvColumns.DataKeys[index].Value.ToString());
            //
            Song.Entities.GuideColumns entity = Business.Do<IGuide>().ColumnsSingle(id);
            entity.Gc_IsUse = !entity.Gc_IsUse;
            Business.Do<IGuide>().ColumnsSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            gvColumns.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvColumns.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IGuide>().ColumnsRemoveUp(id)) BindData(null, null);

        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            gvColumns.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvColumns.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IGuide>().ColumnsRemoveDown(id)) BindData(null, null);
        }
        #endregion

    }
}
