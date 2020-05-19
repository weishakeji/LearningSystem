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

namespace Song.Site.Manage.Student
{
    public partial class StudyLog_Details : Extend.CustomPage
    {
        //课程id
        public int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //学员id
        private int acid = WeiSha.Common.Request.QueryString["acid"].Int32 ?? Extend.LoginState.Accounts.UserID;
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
            DataTable dt = Business.Do<IStudent>().StudentStudyOutlineLog(couid, acid);
            if (dt != null)
            {
                WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
                tree.IdKeyName = "OL_ID";
                tree.ParentIdKeyName = "OL_PID";
                tree.TaxKeyName = "Ol_Tax";
                tree.Root = 0;
                dt = tree.BuilderTree(dt);
                GridView1.DataSource = dt;
                GridView1.DataKeyNames = new string[] { "Ol_ID" };
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
        /// <summary>
        /// 计算时间总计
        /// </summary>
        /// <param name="time"></param>
        /// <param name="format">格式</param>
        /// <returns></returns>
        protected string CaleTotalTime(string time,string format)
        {
            int num = 0;
            int.TryParse(time, out num);
            if (num == 0) return "";
            num = num / 1000;
            //
            string tm = "{0}:{1}:{2}";
            if (num < 60) return string.Format(format,string.Format(tm, 0, 0, num));
            //计算分钟
            int ss = num % 60;
            num = num / 60;            
            if (num < 60) return string.Format(format,string.Format(tm, 0, num, ss));
            //计算小时
            int hh = num / 60;
            int mm = num % 60;
            //
            return string.Format(format, string.Format(tm, hh, mm, ss));
        }
       
    }
}
