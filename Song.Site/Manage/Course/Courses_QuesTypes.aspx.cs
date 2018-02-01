using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using WeiSha.WebControl;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Course
{
    public partial class Courses_QuesTypes : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                //隐藏确定按钮
                EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
                btnEnter.Visible = false; 
                //
                _init();
                BindPriceData();
            }            
        }
        #region 初始化
        private void _init()
        {
            ddlBaseType.Items.Add(new ListItem(" -- 请选择基础类型 -- ","-1"));
            for (int i = 0; i < typeStr.Length; i++)
            {
                ddlBaseType.Items.Add(new ListItem(typeStr[i], (i+1).ToString()));
            }
        }
        /// <summary>
        /// 绑定试题类型信息的列表
        /// </summary>
        protected void BindPriceData()
        {
            Song.Entities.Course cou = id < 1 ? new Song.Entities.Course() : Business.Do<ICourse>().CourseSingle(id);
            if (cou == null) return;
            //课程名称
            lbName.Text = cou.Cou_Name;
            //当前课程的试题类型
            Song.Entities.QuesTypes[] types = Business.Do<IQuestions>().TypeCount(cou.Cou_ID, null, -1);
            gvPrice.DataSource = types;
            gvPrice.DataKeyNames = new string[] { "Qt_ID" };
            gvPrice.DataBind();
        }
        #endregion

        #region 类型管理        
        /// <summary>
        /// 添加类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.QuesTypes qt = new QuesTypes();
            qt.Cou_ID = id;
            qt.Org_ID = org.Org_ID;
            //名称
            qt.Qt_Name = tbName.Text.Trim();
            //基础类型
            int type;
            int.TryParse(ddlBaseType.SelectedValue, out type);
            qt.Qt_Type = type;
            qt.Qt_TypeName = ddlBaseType.SelectedItem.Text;
            //简介
            qt.Qt_Intro = this.tbIntro.Text.Trim();
            qt.Qt_IsUse = cbIsUse.Checked;
            Business.Do<IQuestions>().TypeAdd(qt);
            this.Reload();
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
            int id = int.Parse(this.gvPrice.DataKeys[index].Value.ToString());
            Business.Do<IQuestions>().TypeDelete(id);
            BindPriceData();
        }
        /// <summary>
        /// 进入编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {

            WeiSha.WebControl.RowEdit img = (WeiSha.WebControl.RowEdit)sender;
            int typeid = 0;
            int.TryParse(img.CommandArgument, out typeid);
            Song.Entities.QuesTypes qt = Business.Do<IQuestions>().TypeSingle(typeid);
            if (qt == null) return;
            //名称
            tbName.Text = qt.Qt_Name;
            //基础类型
            ListItem liType = ddlBaseType.Items.FindByValue(qt.Qt_Type.ToString());
            if (liType != null)
            {
                ddlBaseType.SelectedIndex = -1;
                liType.Selected = true;
            }
            //简介
            tbIntro.Text = qt.Qt_Intro;
            cbIsUse.Checked = qt.Qt_IsUse;
            lbTypeID.Text = qt.Qt_ID.ToString();
            //改一些标识
            lbTypeTitle.Text = "编辑";
            btnAddEnter.Visible = false;
            plEditBox.Visible = true;            
        }
        /// <summary>
        /// 编辑当前数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditEnter_Click(object sender, EventArgs e)
        {
            int typeid = 0;
            int.TryParse(lbTypeID.Text, out typeid);
            Song.Entities.QuesTypes qt = Business.Do<IQuestions>().TypeSingle(typeid);
            if (qt == null) return;
            qt.Cou_ID = id;
            qt.Org_ID = org.Org_ID;
            //名称
            qt.Qt_Name = tbName.Text.Trim();
            //基础类型
            int type;
            int.TryParse(ddlBaseType.SelectedValue, out type);
            qt.Qt_Type = type;
            qt.Qt_TypeName = ddlBaseType.SelectedItem.Text;
            //简介
            qt.Qt_Intro = this.tbIntro.Text.Trim();
            qt.Qt_IsUse = cbIsUse.Checked;
            Business.Do<IQuestions>().TypeSave(qt);
            //
            BindPriceData();
        }
        /// <summary>
        /// 退出编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            this.Reload();
        }
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvPrice.DataKeys[index].Value.ToString());
            //
            Song.Entities.QuesTypes entity = Business.Do<IQuestions>().TypeSingle(id);
            entity.Qt_IsUse = !entity.Qt_IsUse;
            Business.Do<IQuestions>().TypeSave(entity);
            BindPriceData();
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            gvPrice.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvPrice.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IQuestions>().TypeRemoveUp(id)) BindPriceData();

        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            gvPrice.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvPrice.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IQuestions>().TypeRemoveDown(id)) BindPriceData();
        }
        #endregion

    }
}