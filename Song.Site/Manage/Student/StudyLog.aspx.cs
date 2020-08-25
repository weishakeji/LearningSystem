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
        protected Song.Entities.Accounts st = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            st = this.Master.Account;
            if (st == null) return;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// ���б�
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            DataTable dt = Business.Do<IStudent>().StudentStudyCourseLog(this.Master.Account.Ac_ID);
            if (dt != null)
            {
                GridView1.DataSource = dt;
                GridView1.DataKeyNames = new string[] { "Cou_ID" };
                GridView1.DataBind();
            }
        }
        /// <summary>
        /// �����ۼ�ѧϰʱ��
        /// </summary>
        /// <param name="studyTime"></param>
        /// <returns></returns>
        protected string CaleStudyTime(string studyTime)
        {
            int num = 0;
            int.TryParse(studyTime, out num);
            if (num == 0) return "";
            if (num < 60) return num + "����";
            //�������
            int ss = num / 60;
            if (ss < 60 && num % 60 > 0) return string.Format("{0}��{1}��", ss, num % 60);
            if (ss < 60 && num % 60 == 0) return string.Format("{0}��", ss);
            //����Сʱ
            int hh = ss / 60;
            int mm = ss % 60;
            return string.Format("{0}Сʱ{1}��", hh, mm);
        }
     
    }
}
