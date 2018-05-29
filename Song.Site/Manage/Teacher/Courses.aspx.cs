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

namespace Song.Site.Manage.Teacher
{
    public partial class Courses : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        //是否为管理员管理状态
        private bool isAdmin = WeiSha.Common.Request.QueryString["admin"].Boolean ?? false;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "课程管理";
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
            //院系
            Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(org.Org_ID, true,true);
            this.ddlDepart.DataSource = nc;
            this.ddlDepart.DataTextField = "dep_cnName";
            this.ddlDepart.DataValueField = "dep_id";
            this.ddlDepart.DataBind();
            //
            this.ddlDepart.Items.Insert(0, new ListItem(" -- 院系 -- ", "-1"));
            //如果是教师登录，则只显示所在院系
            if (Extend.LoginState.Accounts.IsLogin)
            {
                Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
                ListItem li = ddlDepart.Items.FindByValue(th.Dep_Id.ToString());
                if (li != null)
                {
                    ddlDepart.SelectedIndex = -1;
                    li.Selected = true;
                    ddlDepart.Enabled = false;
                }
            }
            if (Extend.LoginState.Admin.IsAdmin && isAdmin)
            {
                ddlDepart.Enabled = true;
                ddlDepart.SelectedIndex = 0;
            }
            ddlDepart_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 当院系的选择变动时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSubject.Items.Clear();
            int depid = 0;
            int.TryParse(ddlDepart.SelectedValue, out depid);            
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
            bool? isUse = null;
            List<Song.Entities.Course> eas = null;
            int sbjid = Convert.ToInt32(ddlSubject.SelectedValue);
            //当前教师
            Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            if (th == null) return;
            eas = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, th.Th_ID,-1, tbSear.Text, isUse, -1);
            foreach (Song.Entities.Course s in eas)
            {
                if (string.IsNullOrEmpty(s.Cou_Intro) || s.Cou_Intro.Trim() == "") continue;
                if (s.Cou_Intro.Length > 20)
                {
                    s.Cou_Intro = s.Cou_Intro.Substring(0, 20) + "...";
                }
            }
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(eas.ToArray());
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "Cou_ID";
            tree.ParentIdKeyName = "Cou_PID";
            tree.TaxKeyName = "Cou_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);

            
            GridView1.DataSource = dt;
            GridView1.DataKeyNames = new string[] { "Cou_ID" };
            GridView1.DataBind();
            
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
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<ICourse>().CourseDelete(Convert.ToInt16(id));
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
    }
}
