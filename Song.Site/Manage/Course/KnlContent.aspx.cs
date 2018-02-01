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
    public partial class KnlContent : Extend.CustomPage
    {
        //课程id，知识库分类id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected int knsid = WeiSha.Common.Request.QueryString["knsid"].Int32 ?? -1;
        //状态
        protected string state = WeiSha.Common.Request.QueryString["state"].String;
        //private string _uppath = "Guide";
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                if (state == "add" || state == "edit")
                {
                    plEditArea.Visible = true;
                    plListArea.Visible = false;
                }
                else
                {
                    plEditArea.Visible = false;
                    plListArea.Visible = true;

                }
                //栏目名称
                spanColName.Visible = knsid > 0;
                if (knsid != 0)
                {
                    Song.Entities.KnowledgeSort col = Business.Do<IKnowledge>().SortSingle(knsid);
                    if (col != null) lbColunms.Text = col.Kns_Name;
                }
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
            Song.Entities.Knowledge[] eas = Business.Do<IKnowledge>().KnowledgePager(org.Org_ID, couid, knsid, null, null, null, null, "", Pager1.Size, Pager1.Index, out count);

            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Kn_ID" };
            GridView1.DataBind();
            Pager1.RecordAmount = count;
            
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
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
            Song.Entities.Knowledge entity = Business.Do<IKnowledge>().KnowledgeSingle(id);
            entity.Kn_IsUse = !entity.Kn_IsUse;
            Business.Do<IKnowledge>().KnowledgeSave(entity);
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
                Business.Do<IKnowledge>().KnowledgeDelete(Convert.ToInt32(id));
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
            Business.Do<IKnowledge>().KnowledgeDelete(id);
            BindData(null, null);

        }
        #region 添加
        /// <summary>
        /// 通过新增进入编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddEvent(object sender, EventArgs e)
        {
            plEditArea.Visible = true;
            plListArea.Visible = false;
            //上级
            ddlTree.Items.Clear();
            Song.Entities.KnowledgeSort[] cous = Business.Do<IKnowledge>().GetSortAll(org.Org_ID, couid, null);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Kns_Name";
            this.ddlTree.DataValueField = "Kns_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            ddlTree.Items.Insert(0, new ListItem("   -- 顶级 --", "0"));

            lbID.Text = "";
            tbTitle.Text = "";
            //
            ListItem li = ddlTree.Items.FindByValue(knsid.ToString());
            if (li != null) li.Selected = true;
        }
        /// <summary>
        /// 进入编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            plEditArea.Visible = true;
            plListArea.Visible = false;
            //
            WeiSha.WebControl.RowEdit img = (WeiSha.WebControl.RowEdit)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());            
            //上级
            ddlTree.Items.Clear();
            Song.Entities.KnowledgeSort[] cous = Business.Do<IKnowledge>().GetSortAll(org.Org_ID, couid, null);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Kns_Name";
            this.ddlTree.DataValueField = "Kns_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            ddlTree.Items.Insert(0, new ListItem("   -- 顶级 --", "0"));
            //
            Song.Entities.Knowledge kn = Business.Do<IKnowledge>().KnowledgeSingle(id);
            lbID.Text = kn.Kn_ID.ToString();
            tbTitle.Text = kn.Kn_Title;            
            tbDetails.Text = kn.Kn_Details;
            ListItem li = ddlTree.Items.FindByValue(kn.Kns_ID.ToString());
            if (li != null) li.Selected = true;
        }
        /// <summary>
        /// 退出编辑环境
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddBack_Click(object sender, EventArgs e)
        {
            plEditArea.Visible = false;
            plListArea.Visible = true;
        }
        /// <summary>
        /// 新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEnter_Click(object sender, EventArgs e)
        {
            int id = 0;
            int.TryParse(lbID.Text, out id);
            Song.Entities.Knowledge kn = id == 0 ? new Song.Entities.Knowledge() : Business.Do<IKnowledge>().KnowledgeSingle(id);
            kn.Kn_Title = tbTitle.Text;
            kn.Kn_Details = tbDetails.Text;
            //
            int gcolid=0;
            int.TryParse(ddlTree.SelectedValue, out gcolid);            
            kn.Kns_ID = gcolid;
            kn.Cou_ID = couid;
            kn.Org_ID = org.Org_ID;
            kn.Kn_IsUse = cbIsShow.Checked;
            //
            if (string.IsNullOrWhiteSpace(kn.Kn_Uid)) kn.Kn_Uid = getUID();
            if (id == 0)
                Business.Do<IKnowledge>().KnowledgeAdd(kn);
            else
                Business.Do<IKnowledge>().KnowledgeSave(kn);
            BindData(null, null);
            btnAddBack_Click(null, null);
        }
        #endregion       
    }
}
