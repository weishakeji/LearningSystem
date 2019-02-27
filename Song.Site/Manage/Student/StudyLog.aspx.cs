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

namespace Song.Site.Manage.Student
{
    public partial class StudyLog : Extend.CustomPage
    {
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = this.Master.Account;
            if (st == null) return;
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
            DataTable dt = Business.Do<IStudent>().StudentStudyCourseLog(org.Org_ID, this.Master.Account.Ac_ID);
            if (dt != null)
            {
                GridView1.DataSource = dt;
                GridView1.DataKeyNames = new string[] { "Cou_ID" };
                GridView1.DataBind();
            }
        }
        /// <summary>
        /// 计算累计学习时间
        /// </summary>
        /// <param name="studyTime"></param>
        /// <returns></returns>
        protected string CaleStudyTime(string studyTime)
        {
            int num = 0;
            int.TryParse(studyTime, out num);
            if (num == 0) return "";
            if (num < 60) return num + "秒钟";
            //计算分钟
            int ss = num / 60;
            if (ss < 60 && num % 60 > 0) return string.Format("{0}分{1}秒", ss, num % 60);
            if (ss < 60 && num % 60 == 0) return string.Format("{0}分", ss);
            //计算小时
            int hh = ss / 60;
            int mm = ss % 60;
            return string.Format("{0}小时{1}分", hh, mm);
        }
     
    }
}
