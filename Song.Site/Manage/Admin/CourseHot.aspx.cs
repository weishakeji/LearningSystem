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

namespace Song.Site.Manage.Admin
{
    public partial class CourseHot : Extend.CustomPage
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
            Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(org.Org_ID, -1, "", null, 0, 0);
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
            int sbjid;          
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            //
            DataSet ds = Business.Do<ICourse>().CourseHot(org.Org_ID, sbjid, 20);  
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataKeyNames = new string[] { "Cou_ID" };
            GridView1.DataBind();
            
        }

        protected void btnsear_Click(object sender, EventArgs e)
        {
            BindData(null, null);
        }

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
                //Song.Entities.Teacher[] eas = Business.Do<ITeacher>().TeacherCount(org.Org_ID, true, -1);
                //DropDownList ddl = (DropDownList)e.IRow.FindControl("ddlTeacher");
                //ddl.DataSource = eas;
                //ddl.DataTextField = "th_Name";
                //ddl.DataValueField = "th_ID";
                //ddl.DataBind();
                //ddl.Items.Insert(0, new ListItem("无", "-1"));
                ////教师id
                //Label lbThid = (Label)e.IRow.FindControl("lbThID");
                //ListItem liThid = ddl.Items.FindByValue(lbThid.Text);
                //if (liThid != null) liThid.Selected = true;
            }
        }

    }
}
