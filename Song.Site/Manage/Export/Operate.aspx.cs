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

namespace Song.Site.Manage.Export
{
    public partial class Operate : Extend.CustomPage
    {
        private string _uppath = "Temp";
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
            ////所属教师
            //Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            ////所属机构
            //Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            ////当前教师负责的课程
            //List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseAll(org.Org_ID, -1,th.Th_ID, null);
            //foreach (Song.Entities.Course c in cous)
            //{
            //    //课程图片
            //    if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
            //    {
            //        c.Cou_LogoSmall = Upload.Get[_uppath].Virtual + c.Cou_LogoSmall;
            //        c.Cou_Logo = Upload.Get[_uppath].Virtual + c.Cou_Logo;
            //    }
            //}
            //rptCourse.DataSource = cous;
            //rptCourse.DataBind();
        }

        protected void btnExpStudent_Click(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //创建文件
            string name = "学生信息导出" + "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
            string filePath = Upload.Get[_uppath].Physics + name;
            //filePath = Business.Do<IStudent>().StudentExport4Excel(filePath, org.Org_ID, -1);
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
        
    }
}
