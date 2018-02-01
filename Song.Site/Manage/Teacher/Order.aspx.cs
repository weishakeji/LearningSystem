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

namespace Song.Site.Manage.Teacher
{
    public partial class Order : Extend.CustomPage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {  
                BindData(null, null);
            }
        }
        /// <summary>
        /// °ó¶¨ÁÐ±í
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.TeacherComment[] eas = null;
            eas = Business.Do<ITeacher>().CommentOrder(org.Org_ID, true, null, 30, 20);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Thc_id" };
            GridView1.DataBind();
        }        
    }
}
