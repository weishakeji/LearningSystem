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
using System.Collections.Generic;

namespace Song.Site.Manage.Admin
{
    public partial class Courses : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        //是否为管理员管理状态
        private bool isAdmin = WeiSha.Common.Request.QueryString["admin"].Boolean ?? false;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                bindTree();
                BindData(null, null);
            }           
        }
        /// <summary>
        /// 绑定导航
        /// </summary>
        private void bindTree()
        {
            ddlSubject.Items.Clear();
            int depid = 0;
            Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(org.Org_ID, depid, "", null, 0, 0);
            ddlSubject.DataSource = sbjs;
            ddlSubject.DataTextField = "Sbj_Name";
            ddlSubject.DataValueField = "Sbj_ID";
            ddlSubject.DataBind();
            this.ddlSubject.Items.Insert(0, new ListItem(" -- 专业 -- ", "-1"));  
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            //总记录数
            int count = 0;
            bool? isUse = null;
            List<Song.Entities.Course> eas = null;
            int sbjid = Convert.ToInt32(ddlSubject.SelectedValue);
            eas = Business.Do<ICourse>().CoursePager(org.Org_ID, sbjid, -1, isUse, tbSear.Text, "tax", Pager1.Size, Pager1.Index, out count);
            foreach (Song.Entities.Course s in eas)
            {
                if (string.IsNullOrEmpty(s.Sbj_Name) || s.Sbj_Name.Trim() == "")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectSingle(s.Sbj_ID);
                    if (subject != null) s.Sbj_Name = subject.Sbj_Name;
                    Business.Do<ICourse>().CourseSave(s);
                }
                if (string.IsNullOrEmpty(s.Cou_Intro) || s.Cou_Intro.Trim() == "") continue;
                if (s.Cou_Intro.Length > 20)
                {
                    s.Cou_Intro = s.Cou_Intro.Substring(0, 20) + "...";
                }                
            }
            //DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(eas.ToArray());
            //WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            //tree.IdKeyName = "Cou_ID";
            //tree.ParentIdKeyName = "Cou_PID";
            //tree.TaxKeyName = "Cou_Tax";
            //tree.Root = 0;
            //dt = tree.BuilderTree(dt);


            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Cou_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;            
        }
        /// <summary>
        /// 获取当前专业下的课程数据
        /// </summary>
        /// <param name="sbjid"></param>
        /// <returns></returns>
        protected string GetCourseCount(object sbjid)
        {
            int sbj;
            int.TryParse(sbjid.ToString(), out sbj);
            int count = Business.Do<ICourse>().CourseOfCount(org.Org_ID, sbj, -1);
            return count.ToString();
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 清空当前专业的试题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lkbClear_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<ICourse>().CourseClear(id);
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
            Song.Entities.Course entity = Business.Do<ICourse>().CourseSingle(id);
            entity.Cou_IsUse = !entity.Cou_IsUse;
            Business.Do<ICourse>().CourseSave(entity);
            BindData(null, null);           
        }
        /// <summary>
        /// 修改是否推荐的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbRec_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.Course entity = Business.Do<ICourse>().CourseSingle(id);
            entity.Cou_IsRec = !entity.Cou_IsRec;
            Business.Do<ICourse>().CourseSave(entity);
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
                Business.Do<ICourse>().CourseDelete(Convert.ToInt32(id));
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
            Business.Do<ICourse>().CourseDelete(id);
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
            if (Business.Do<ICourse>().CourseUp(id))
            {
                BindData(null, null);
            }
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
            if (Business.Do<ICourse>().CourseDown(id))
            {
                BindData(null, null);
            }
        }

        #region 指派课程教师        
        
        /// <summary>
        /// GridView行绑定事件，主要是操作课程老师
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             //当为数据行时
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Song.Entities.Teacher[] eas = Business.Do<ITeacher>().TeacherCount(org.Org_ID, true, -1);
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlTeacher");
                ddl.DataSource = eas;
                ddl.DataTextField = "th_Name";
                ddl.DataValueField = "th_ID";
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("无", "-1"));
                //教师id
                Label lbThid = (Label)e.Row.FindControl("lbThID");
                ListItem liThid = ddl.Items.FindByValue(lbThid.Text);
                if (liThid != null) liThid.Selected = true;
            }
        }
        protected void ddlTeacher_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //教师下拉列表
            DropDownList ddl = (DropDownList)sender;
            //取当前课程
            GridViewRow gr = (GridViewRow)(ddl).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(id);
            //设置教师
            int thid;
            int.TryParse(ddl.SelectedValue.ToString(), out thid);
            cou.Th_ID = thid;
            cou.Th_Name = ddl.SelectedItem.Text;
            Business.Do<ICourse>().CourseSave(cou);
            BindData(null, null);
        }
        #endregion
    }
}
