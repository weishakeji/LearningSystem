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
using System.Collections.Generic;

namespace Song.Site.Manage.Admin
{
    /// <summary>
    /// 当前课程的学员
    /// </summary>
    public partial class Courses_Students : Extend.CustomPage
    {
        //课程id
        protected int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //课程上传资料的所在路径
        private string _uppath = "Course";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
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
            int count = 0;
            string stname = tbName.Text.Trim();
            string stmobi = tbMobi.Text.Trim();
            Song.Entities.Accounts[] eas = Business.Do<ICourse>().Student4Course(id, stname, stmobi, Pager1.Size, Pager1.Index, out count);
            
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Ac_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 课程完成度
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        protected string GetComplete(string acid)
        {
            int stid = 0;
            int.TryParse(acid, out stid);
            DataTable dtLog = Business.Do<IStudent>().StudentStudyCourseLog(stid, id);
            string complete = "0";
            if (dtLog != null)
            {
                foreach (DataRow dr in dtLog.Rows)
                {
                    if (dr["Cou_ID"].ToString() == id.ToString())
                    {
                        complete = dr["complete"].ToString();
                    }
                }
                return complete;
            }
            else
            {
                return "";
            }
        }
    }
}
