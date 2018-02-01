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
    public partial class Archives : Extend.CustomPage
    {
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!IsPostBack)
            {
                InitBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            
            //    //学科/专业
            //Song.Entities.Subject[] subs = Business.Do<ISubject>().SubjectCount(org.Org_ID, true, 0);
            //this.ddlSubject.DataSource = subs;
            //this.ddlSubject.DataTextField = "Sbj_Name";
            //this.ddlSubject.DataValueField = "Sbj_ID";
            //this.ddlSubject.DataBind();
            //ddlSubject.Items.Insert(0, new ListItem(" -- 全部 -- ", "-1"));
            ////默认选中自身所在的专业
            //Song.Entities.Team team = Business.Do<ITeam>().TeamSingle(Extend.LoginState.Admin.CurrentUser.Team_ID);
            //if (team != null)
            //{
            //    Song.Entities.Subject sbj = Business.Do<ISubject>().SubjectSingle(team.Sbj_ID);
            //    if (sbj != null)
            //    {
            //        ListItem liDdl = ddlSubject.Items.FindByValue(sbj.Sbj_ID.ToString());
            //        if (liDdl != null) liDdl.Selected = true;
            //    }
            //}
            
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            Song.Entities.Examination[] eas = null;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //时间类型
            int spanType = Convert.ToInt32(ddlTime.SelectedValue);
            DateTime? start = null;
            DateTime? end = null;
            //所有时间
            if (spanType == -1)
            {
                eas = Business.Do<IExamination>().GetPager(org.Org_ID, null, null, null, tbTheme.Text, Pager1.Size, Pager1.Index, out count);
            }
            else
            {
                //今天
                if (spanType == 1)
                {
                    start = (DateTime?)DateTime.Now.Date;
                    end = (DateTime?)DateTime.Now.AddDays(1).Date;
                }
                //本周
                if (spanType == 2)
                {
                    DateTime week = (DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1)).Date;
                    start = (DateTime?)week;
                    end = (DateTime?)week.AddDays(7);
                }
                //本月
                if (spanType == 3)
                {
                    DateTime month = (DateTime.Now.AddDays(-DateTime.Now.Day + 1)).Date;
                    start = (DateTime?)month;
                    end = (DateTime?)month.AddMonths(1);
                }
                eas = Business.Do<IExamination>().GetPager(org.Org_ID, start, end, null, tbTheme.Text, Pager1.Size, Pager1.Index, out count);
            }
            rtpExamThem.DataSource = eas;
            rtpExamThem.DataBind();

            Pager1.RecordAmount = count;
        }
        protected void rtpExamThem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lbTitle = (Label)e.Item.FindControl("lbTitle");
            string name = lbTitle.Text;
            //考试场次
            Repeater rpt = (Repeater)e.Item.FindControl("rtpExam");
            Song.Entities.Examination[] exams = Business.Do<IExamination>().ExamItem(lbTitle.CssClass);
            for (int i = 0; i < exams.Length; i++)
            {
                DateTime examDate = exams[i].Exam_Date < DateTime.Now.AddYears(-100) ? DateTime.Now : (DateTime)exams[i].Exam_Date;
                exams[i].Exam_Date = examDate.AddYears(100) < DateTime.Now ? DateTime.Now : examDate;
            }
            rpt.DataSource = exams;
            rpt.DataBind();
        }
        /// <summary>
        /// 获取参考人员类型
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected string getGroupType(string type,string uid)
        {
            if (type == "1") return "全体学生";
            if (type == "2")
            {
                Song.Entities.StudentSort[] sts = Business.Do<IExamination>().GroupForStudentSort(uid);
                string strDep = "";
                for (int i = 0; i < sts.Length; i++)
                {
                    strDep += sts[i].Sts_Name;
                    if (i < sts.Length - 1) strDep += ",";
                }
                return strDep;
            }
            return "";
        }
        /// <summary>
        /// 获取当前考试的平均分
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        protected double getAvg4Exam(object examid)
        {
            int id = Convert.ToInt32(examid);
            double tm = Business.Do<IExamination>().Avg4Exam(id);
            tm = Math.Round(tm * 100) / 100;
            return tm;
        }
        protected int getNumber4Exam(object examid)
        {
            int id = Convert.ToInt32(examid);
            return  Business.Do<IExamination>().Number4Exam(id);
        }
        /// <summary>
        /// 判断当前考试是否要显示“批阅试卷”按钮
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        protected bool getIsCorrect(object examid)
        {
            int id = Convert.ToInt32(examid);
            //考生数，如果没有人考试，则不需要批阅
            int students= Business.Do<IExamination>().Number4Exam(id);
            if (students < 1) return false;
            Song.Entities.Examination exas = Business.Do<IExamination>().ExamSingle(id);
            Song.Entities.TestPaper pager = Business.Do<ITestPaper>().PagerSingle(exas.Tp_Id);
            if (pager == null) return false;
            Song.Entities.TestPaperItem[] items = Business.Do<ITestPaper>().GetItemForAny(pager);
            foreach (Song.Entities.TestPaperItem ti in items)
            {
                if (ti.TPI_Type == 4) return true;
            }
            return false;
        }
    }
}
