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

namespace Song.Site.Manage.Course
{
    public partial class List : Extend.CustomPage
    {
        private string _uppath = "Course";
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
            //所属教师
            Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            //所属机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //当前教师负责的课程
            List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseAll(org.Org_ID, -1,th.Th_ID, null);
            foreach (Song.Entities.Course c in cous)
            {
                //课程图片
                if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
                {
                    c.Cou_LogoSmall = Upload.Get[_uppath].Virtual + c.Cou_LogoSmall;
                    c.Cou_Logo = Upload.Get[_uppath].Virtual + c.Cou_Logo;
                }
            }
            rptCourse.DataSource = cous;
            rptCourse.DataBind();
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int id = Convert.ToInt32(lb.CommandArgument);
            Business.Do<ICourse>().CourseDelete(id);
            this.Reload();
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            LinkButton ub = (LinkButton)sender;
            int id = int.Parse(ub.CommandArgument);
            //
            Song.Entities.Course entity = Business.Do<ICourse>().CourseSingle(id);
            entity.Cou_IsUse = !entity.Cou_IsUse;
            Business.Do<ICourse>().CourseSave(entity);
            BindData(null, null);
           
        }
    }
}
