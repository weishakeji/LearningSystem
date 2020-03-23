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

namespace Song.Site.Manage.Student
{
    public partial class Course_Study : Extend.CustomPage
    {
        private string _uppath = "Course";
        Song.Entities.Organization org;
        //学习记录的datatable
        DataTable dtLog = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = this.Master.Account;
            if (st == null) return;
            org = Business.Do<IOrganization>().OrganCurrent();
            dtLog = Business.Do<IStudent>().StudentStudyCourseLog(this.Master.Account.Ac_ID);
            if (!this.IsPostBack)
            {  
                BindData(null, null);
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String.ToLower();
                string couid = WeiSha.Common.Request.Form["couid"].String.ToLower();
                string json = string.Empty;
                switch (action)
                {
                    case "getstc":
                        Song.Entities.Student_Course stc = GetStc(couid);
                        if (stc == null)
                        {
                            json = "{\"success\":\"0\"}";
                        }
                        else
                        {
                            json = "{\"success\":\"1\",data:"+stc.ToJson()+"}";
                        }
                        break;
                }
                Response.Write(json);
                Response.End();
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //当前学生的课程
            Song.Entities.Accounts st = this.Master.Account;
            if (st == null) return;
            //购买的课程(含概试用的）
            List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseForStudent(st.Ac_ID, null, 0,null,-1);
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
        /// <summary>
        /// 获取课程的购买信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected string getBuyInfo(object id)
        {
            int couid = 0;
            int.TryParse(id.ToString(), out couid);
            Student_Course sc= Business.Do<ICourse>().StudentCourse(Extend.LoginState.Accounts.CurrentUser.Ac_ID, couid);
            if (sc == null) return "";
            if (sc.Stc_IsFree && sc.Stc_EndTime > sc.Stc_StartTime.AddYears(100)) return "免费（无限期）";
            if (sc.Stc_IsFree && sc.Stc_EndTime < sc.Stc_StartTime.AddYears(100)) return string.Format("免费到{0}", sc.Stc_EndTime.ToString("yyyy年M月d日 HH:mm:ss"));
            if (sc.Stc_IsTry) return "试用";
            return sc.Stc_StartTime.ToString("yyyy年MM月dd日") + " - " + sc.Stc_EndTime.ToString("yyyy年MM月dd日 HH:mm:ss");
        }
        /// <summary>
        /// 取消课程学习
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbSelected_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;            
            int couid = Convert.ToInt32(lb.CommandArgument);    //课程id            
            Song.Entities.Accounts st = this.Master.Account;     //当前学生       
            //取消
            Business.Do<ICourse>().DelteCourseBuy(st.Ac_ID, couid);
            //重载当前面
            this.BindData(null, null);
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
            return Business.Do<ICourse>().StudentCourse(this.Master.Account.Ac_ID, couid);
        }
    }
}
