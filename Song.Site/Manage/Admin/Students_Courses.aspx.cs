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
    public partial class Students_Courses : Extend.CustomPage
    {
        //学员id
        protected int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //员工上传资料的所在路径
        private string _uppath = "Course";
        //学习记录的datatable
        DataTable dtLog = null;
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            dtLog = Business.Do<IStudent>().StudentStudyCourseLog(org.Org_ID, id);
            if (!this.IsPostBack)
            {
                fill();
            }
           
        }
        private void fill()
        {
            //当前学生的课程
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(id);
            if (st == null) return;
            //当前学员的名称
            lbAccName.Text = st.Ac_Name;
            Title = st.Ac_Name;
            //购买的课程(含概试用的）
            List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseForStudent(st.Ac_ID, null, 0, null, -1);
            foreach (Song.Entities.Course c in cous)
            {
                //课程图片
                if (!string.IsNullOrEmpty(c.Cou_LogoSmall) && c.Cou_LogoSmall.Trim() != "")
                    c.Cou_LogoSmall = Upload.Get[_uppath].Virtual + c.Cou_LogoSmall;
                c.Cou_IsStudy = true;
            }
            rptCourse.DataSource = cous;
            rptCourse.DataBind();
            plNoCourse.Visible = cous.Count < 1;     
        }
        #region 行内需要的方法
        /// <summary>
        /// 取消课程学习
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSelected_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            int couid = Convert.ToInt32(lb.CommandArgument);    //课程id 
            //取消课程
            Business.Do<ICourse>().DelteCourseBuy(id, couid);
            //重载当前面
            this.fill();
        }
        /// <summary>
        /// 获取课程的购买信息
        /// </summary>
        /// <param name="objid"></param>
        /// <returns></returns>
        protected string getBuyInfo(object objid)
        {
            int couid = 0;
            int.TryParse(objid.ToString(), out couid);
            Student_Course sc = Business.Do<ICourse>().StudyCourse(id, couid);
            if (sc == null) return "";
            if (sc.Stc_IsFree) return "免费（无限期）";
            if (sc.Stc_IsTry) return "试用";
            return sc.Stc_StartTime.ToString("yyyy年MM月dd日") + " - " + sc.Stc_EndTime.ToString("yyyy年MM月dd日");
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
            if (num < 60) return num + "秒钟";
            //计算分钟
            num = num / 60;
            int ss = num % 60;
            if (num < 60) return num + "分钟";
            //计算小时
            int hh = num / 60;
            int mm = num % 60;
            return string.Format("{0}小时{1}分钟", hh, mm);
        }
        /// <summary>
        /// 获取累计学习时间
        /// </summary>
        /// <param name="studyTime"></param>
        /// <returns></returns>
        protected string GetstudyTime(string couid)
        {
            string studyTime = "0";
            if (dtLog != null)
            {
                foreach (DataRow dr in dtLog.Rows)
                {
                    if (dr["Cou_ID"].ToString() == couid)
                    {
                        studyTime = dr["studyTime"].ToString();
                    }
                }
                return CaleStudyTime(studyTime);
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 课程完成度
        /// </summary>
        /// <param name="studyTime"></param>
        /// <returns></returns>
        protected string GetComplete(string couid)
        {
            string complete = "0";
            if (dtLog != null)
            {
                foreach (DataRow dr in dtLog.Rows)
                {
                    if (dr["Cou_ID"].ToString() == couid)
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
        /// <summary>
        /// 获取最后学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        protected string GetLastTime(string couid)
        {
            DateTime? lastTime = null;
            if (dtLog != null)
            {
                foreach (DataRow dr in dtLog.Rows)
                {
                    if (dr["Cou_ID"].ToString() == couid)
                    {
                        lastTime = Convert.ToDateTime(dr["LastTime"]);
                    }
                }
            }
            if (lastTime == null) return "（还没有学习）";
            return ((DateTime)lastTime).ToString();
        }
        /// <summary>
        /// 获取学员学习的课程记录
        /// </summary>
        /// <param name="couidstr"></param>
        /// <returns></returns>
        protected Song.Entities.Student_Course GetStc(string couidstr)
        {
            int couid = 0;
            int.TryParse(couidstr, out couid);
            return Business.Do<ICourse>().StudyCourse(id, couid);
        }
        #endregion
    }
}
