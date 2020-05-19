using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Data;

namespace Song.Site.Manage.Course
{
    public partial class Courses_Outlines : Extend.CustomPage
    {
        //课程id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;      
        protected string UID
        {
            get { return this.getUID(); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //隐藏确定按钮
                EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
                btnEnter.Visible = false;
                BindData(null, null);                
            }
        }
        /// <summary>
        /// 绑定章节列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, null);
            //计算试题数
            foreach (Song.Entities.Outline c in outlines)
            {
                c.Ol_QuesCount = Business.Do<IOutline>().QuesOfCount(c.Ol_ID, -1, true, true);
                Business.Do<IOutline>().UpdateQuesCount(c.Ol_ID, c.Ol_QuesCount);
            }
            //生成树形
            DataTable dt = Business.Do<IOutline>().OutlineTree(outlines);
            GridView1.DataSource = dt;
            GridView1.DataKeyNames = new string[] { "Ol_ID" };
            GridView1.DataBind();
        }
        #region 行内事件        
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
            Business.Do<IOutline>().OutlineDelete(id);
            BindData(null, null);            
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IOutline>().OutlineUp(couid, id))            
                BindData(null, null);            
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IOutline>().OutlineDown(couid, id))          
                BindData(null, null);            
        }
        /// <summary>
        /// 进入编辑行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnToEditor_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int index = gr.RowIndex;
            GridView1.EditIndex = index;
                BindData(null, null);
        }
        /// <summary>
        /// 退出编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditBack_Click(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            BindData(null, null);
        }
        /// <summary>
        /// 编辑当前数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditEnter_Click(object sender, EventArgs e)
        {
            //当前点击的按钮
            System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
            //当前输入框
            GridViewRow gr = (GridViewRow)btn.Parent.Parent;
            TextBox tb = (TextBox)gr.FindControl("tbName");
            if (tb == null || tb.Text.Trim()=="") return;
            //当前章节
            int id = int.Parse(this.GridView1.DataKeys[gr.RowIndex].Value.ToString());
            if (id > 0)
            {
                Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(id);
                if (outline == null) return;
                outline.Ol_Name = tb.Text.Trim();
                Business.Do<IOutline>().OutlineSave(outline);
            }
            else
            {
                //新增
                Song.Entities.Outline outline = new Song.Entities.Outline();
                outline.Ol_Name = tb.Text.Trim();
                //上级ID
                Label lbPid = (Label)gr.FindControl("lbPID");
                outline.Ol_PID = Convert.ToInt32(lbPid.Text);  
                //序号
                Label lbTax = (Label)gr.FindControl("lbTax");
                outline.Ol_Tax = Convert.ToInt32(lbTax.Text); 
                //所属课程
                outline.Cou_ID = couid;
                outline.Ol_IsUse = true;
                Business.Do<IOutline>().OutlineAdd(outline);
            }
            //退出编辑状态
            GridView1.EditIndex = -1;
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Outline entity = Business.Do<IOutline>().OutlineSingle(id);
            entity.Ol_IsUse = !entity.Ol_IsUse;
            Business.Do<IOutline>().OutlineSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否收费的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbFree_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Outline entity = Business.Do<IOutline>().OutlineSingle(id);
            entity.Ol_IsFree = !entity.Ol_IsFree;
            Business.Do<IOutline>().OutlineSave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 修改是否完结的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbFinish_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Outline entity = Business.Do<IOutline>().OutlineSingle(id);
            entity.Ol_IsFinish = !entity.Ol_IsFinish;
            Business.Do<IOutline>().OutlineSave(entity);
            BindData(null, null);
        }
        #endregion

        #region 添加下级章节
        /// <summary>
        /// 添加下级章节的按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddSub_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)btn.Parent.Parent;
            int olid = int.Parse(this.GridView1.DataKeys[gr.RowIndex].Value.ToString());
            //从数据库获取章节
            Song.Entities.Outline[] outline = Business.Do<IOutline>().OutlineAll(couid, null);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outline);
            //是否有下级，下级的最大tax
            int tax = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int pid = Convert.ToInt32(dt.Rows[i]["Ol_PID"].ToString());
                if (pid == olid)
                {
                    int currtax = Convert.ToInt32(dt.Rows[i]["Ol_Tax"].ToString());
                    tax = tax < currtax ? currtax : tax;
                }
            }
            DataRow dr = dt.NewRow();
            dr["Ol_ID"] = -1;
            dr["Ol_PID"] = olid;
            dr["Ol_Tax"] = tax+1;
            dt.Rows.Add(dr);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "OL_ID";
            tree.ParentIdKeyName = "OL_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);
            //取新增项索引
            int index = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Ol_Name"].ToString() == "")
                {
                    index = i;
                    break;
                }
            }
            GridView1.DataSource = dt;
            GridView1.DataKeyNames = new string[] { "Ol_ID" };
            GridView1.EditIndex = index;
            GridView1.DataBind();
        }
        #endregion

        #region 上方的按钮事件
        /// <summary>
        /// 批量启用章节
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnModfiy_Click(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string s in keys.Split(','))
            {
                int id = 0;
                int.TryParse(s, out id);
                Song.Entities.Outline entity = Business.Do<IOutline>().OutlineSingle(id);
                if (entity == null) continue;
                entity.Ol_IsUse = !entity.Ol_IsUse;
                Business.Do<IOutline>().OutlineSave(entity);
            }
            BindData(null, null);
        }
        protected void btnFinish_Click(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string s in keys.Split(','))
            {
                int id = 0;
                int.TryParse(s, out id);
                Song.Entities.Outline entity = Business.Do<IOutline>().OutlineSingle(id);
                if (entity == null) continue;
                entity.Ol_IsFinish = !entity.Ol_IsFinish;
                Business.Do<IOutline>().OutlineSave(entity);
            }
            BindData(null, null);
        }
        protected void btnFree_Click(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string s in keys.Split(','))
            {
                int id = 0;
                int.TryParse(s, out id);
                Song.Entities.Outline entity = Business.Do<IOutline>().OutlineSingle(id);
                if (entity == null) continue;
                entity.Ol_IsFree = true;
                Business.Do<IOutline>().OutlineSave(entity);
            }
            BindData(null, null);
        }
        protected void btnNofree_Click(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string s in keys.Split(','))
            {
                int id = 0;
                int.TryParse(s, out id);
                Song.Entities.Outline entity = Business.Do<IOutline>().OutlineSingle(id);
                if (entity == null) continue;
                entity.Ol_IsFree = false;
                Business.Do<IOutline>().OutlineSave(entity);
            }
            BindData(null, null);
        }
        #endregion
    }
}