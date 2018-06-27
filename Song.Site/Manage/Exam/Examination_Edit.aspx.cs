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

namespace Song.Site.Manage.Exam
{
    public partial class Examination_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //专业
        Song.Entities.Subject[] sbjs = null;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
        }
        /// <summary>
        /// 学员分组的绑定
        /// </summary>
        private void InitBind()
        {
            Song.Entities.StudentSort[] sts = Business.Do<IStudent>().SortCount(org.Org_ID, true, 0);
            this.lbSort.DataSource = sts;
            this.lbSort.DataTextField = "Sts_Name";
            this.lbSort.DataValueField = "Sts_ID";
            this.lbSort.DataBind();
            //绑定专业
            if (this.sbjs == null)
            {
                sbjs = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, 0);
                //foreach (Song.Entities.Subject s in this.sbjs)
                //{
                //    int count = Business.Do<ITestPaper>().PagerOfCount(org.Org_ID, s.Sbj_ID, -1, -1, true);
                //    s.Sbj_Name = s.Sbj_Name + "  (" + count + ")";
                //}
            }
            ddlSubject.DataSource = sbjs;
            ddlSubject.DataTextField = "Sbj_Name";
            ddlSubject.DataValueField = "Sbj_ID";
            ddlSubject.Root = 0;
            ddlSubject.DataBind();
            if (ddlSubject.Items.Count < 1) ddlSubject.Items.Insert(0, new ListItem("--（没有专业）--", "-1"));
            if (ddlSubject.Items.Count > 0) ddlSubject.Items.Insert(0, new ListItem("", "-1"));
        }
        void fill()
        {
            Song.Entities.Examination mm;
            if (id != 0)
            {
                mm = Business.Do<IExamination>().ExamSingle(id);
                //是否使用
                cbIsUse.Checked = mm.Exam_IsUse;
                //唯一标识
                ViewState["UID"] = mm.Exam_UID;
                //所属参考人员分类
                ListItem ligroup = this.rblGroup.Items.FindByValue(mm.Exam_GroupType.ToString());
                if (ligroup != null)
                {
                    rblGroup.SelectedIndex = -1;
                    ligroup.Selected = true;
                }
                //按学员分类
                if (mm.Exam_GroupType == 2)
                {
                    Song.Entities.StudentSort[] sts = Business.Do<IExamination>().GroupForStudentSort(getUID());
                    lbSelected.DataSource = sts;
                    lbSelected.DataTextField = "Sts_Name";
                    lbSelected.DataValueField = "Sts_ID";
                    lbSelected.DataBind();
                    string stsStr = "";
                    foreach (Song.Entities.StudentSort d in sts)
                        stsStr += "," + d.Sts_ID + "|" + d.Sts_Name;
                    tbSortSelected.Text = stsStr;
                }
                //时间类型
                ListItem liDateType = this.rblExamTimeType.Items.FindByValue(mm.Exam_DateType.ToString());
                if (liDateType != null)
                {
                    rblExamTimeType.SelectedIndex = -1;
                    liDateType.Selected = true;
                    tbStartTime.Text = mm.Exam_Date.ToString("yyyy-MM-dd HH:mm");
                    tbStartOver.Text = mm.Exam_DateOver.ToString("yyyy-MM-dd HH:mm");
                    rblExamTimeType_SelectedIndexChanged(rblExamTimeType, null);
                }
                //一些参数
                cbExam_IsToggle.Checked = mm.Exam_IsToggle;  //是否允许切换窗口
                cbExam_IsShowBtn.Checked = mm.Exam_IsShowBtn;    //是否显示确认按钮
                cbExam_IsRightClick.Checked = mm.Exam_IsRightClick;  //是否禁用右键
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Examination();
                ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                tbStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                tbStartOver.Text = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd HH:mm");
            }
            //考试名称
            tbName.Text = mm.Exam_Title;
            //简介
            tbIntro.Text = mm.Exam_Intro;
            //考试场次的数据填充
            setExamItem();

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //考试主题
            Song.Entities.Examination theme = id < 1 ? new Song.Entities.Examination() : Business.Do<IExamination>().ExamSingle(id);

            //基本信息：考试名称
            theme.Exam_Title = tbName.Text.Trim();
            //UID，是否为考试主题（后面还分场次）
            theme.Exam_UID = getUID();
            theme.Exam_IsTheme = true;
            //是否使用与推荐
            theme.Exam_IsUse = cbIsUse.Checked;
            //简介
            theme.Exam_Intro = tbIntro.Text;
            //一些参数
            theme.Exam_IsToggle = cbExam_IsToggle.Checked;  //是否允许切换窗口
            theme.Exam_IsShowBtn = cbExam_IsShowBtn.Checked;    //是否显示确认按钮
            theme.Exam_IsRightClick = cbExam_IsRightClick.Checked;  //是否禁用右键
            //参考人员
            //相关关联对象
            List<Song.Entities.ExamGroup> groups = null;
            //分类
            theme.Exam_GroupType = Convert.ToInt16(rblGroup.SelectedValue);
            //如果是按学员分类
            if (theme.Exam_GroupType == 2)
                groups = this.getGroupForSts();
            //时间设定
            theme.Exam_DateType = Convert.ToInt32(rblExamTimeType.SelectedValue);
            DateTime dtStart = DateTime.Now, dtOver = DateTime.Now.AddMonths(1);
            DateTime.TryParse(tbStartTime.Text, out dtStart);
            DateTime.TryParse(tbStartOver.Text, out dtOver);
            theme.Exam_Date = dtStart;
            theme.Exam_DateOver = dtOver;            
            try
            {
                if (theme.Exam_DateType == 2)
                {
                    if (theme.Exam_DateOver <= theme.Exam_Date)
                    {
                        throw new Exception("设定时间区间时，结束时间不能小于等于开始时间。");
                    }
                }
                //获取场次
                List<Song.Entities.Examination> items = getExamItem();
                if (id < 1)
                {
                    Business.Do<IExamination>().ExamAdd(theme, items, groups);
                }
                else
                {
                    Business.Do<IExamination>().ExamSave(theme, items, groups);
                }
                Master.AlertCloseAndRefresh("操作完成！");

            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        #region 获取参考人员范围
        /// <summary>
        /// 获取参加考试的班组
        /// </summary>
        /// <returns></returns>
        private List<Song.Entities.ExamGroup> getGroupForSts()
        {
            List<Song.Entities.ExamGroup> groups = new List<ExamGroup>();
            foreach (string str in tbSortSelected.Text.Split(','))
            {
                if (str == string.Empty) continue;
                int teamid = Convert.ToInt32(str.Substring(0, str.IndexOf("|")));
                string teamname = str.Substring(str.IndexOf("|") + 1);
                //
                Song.Entities.ExamGroup g = new ExamGroup();
                g.Exam_UID = getUID();
                g.Eg_Type = 2;
                g.Sts_ID = teamid;
                groups.Add(g);
            }
            return groups;
        }
        #endregion

        #region 考试场次
        /// <summary>
        /// 绑定选择题子项的列表
        /// </summary>
        private void setExamItem()
        {
            //最多几项
            int maxItem = Convert.ToInt16(lbMaxnum.Text);
            Song.Entities.Examination[] ans = Business.Do<IExamination>().ExamItem(getUID());
            List<Song.Entities.Examination> list = new List<Song.Entities.Examination>();
            for (int i = 0; i < maxItem; i++)
            {
                if (i < ans.Length)
                {
                    list.Add(ans[i]);
                }
                else
                {
                    Song.Entities.Examination t = new Song.Entities.Examination();
                    t.Exam_ID = -1;
                    t.Exam_UID = getUID();
                    list.Add(t);
                }
                DateTime examDate = list[i].Exam_Date;
                list[i].Exam_Date = examDate.AddYears(100) < DateTime.Now ? DateTime.Now : examDate;
            }
            GvItem.DataSource = list;
            GvItem.DataKeyNames = new string[] { "Exam_ID" };
            GvItem.DataBind();
            for (int i = 0; i < GvItem.Rows.Count; i++)
            {
                GridViewRow gvr = GvItem.Rows[i];
                //考试时间
                TextBox tbDate = (TextBox)gvr.FindControl("tbDate");
                if (list[i].Exam_ID < 1)
                {
                    tbDate.Text = "";
                    ((TextBox)gvr.FindControl("tbSpan")).Text = "";
                }
            }

        }
        /// <summary>
        /// 获取考试场次
        /// </summary>
        /// <returns></returns>
        private List<Song.Entities.Examination> getExamItem()
        {
            List<Song.Entities.Examination> items = new List<Song.Entities.Examination>();
            for (int i = 0; i < GvItem.Rows.Count; i++)
            {
                GridViewRow gvr = GvItem.Rows[i];
                if (gvr.RowType != DataControlRowType.DataRow) continue;
                //id
                Label lbID = (Label)gvr.FindControl("lbID");
                int itemID = Convert.ToInt32(lbID.Text == string.Empty ? "-1" : lbID.Text);
               
                //考试名称
                string name = ((TextBox)gvr.FindControl("tbName")).Text;
                //总分与及格分
                TextBox tbTotal = (TextBox)gvr.FindControl("tbTotal");
                int total = tbTotal.Text.Trim() == "" ? 60 : Convert.ToInt32(tbTotal.Text);
                TextBox tbPassscore = (TextBox)gvr.FindControl("tbPassScore");
                int passcore = tbPassscore.Text.Trim() == "" ? 60 : Convert.ToInt32(tbPassscore.Text);
                //考试时间
                TextBox tbDate = (TextBox)gvr.FindControl("tbDate");
                DateTime date = Convert.ToDateTime(tbDate.Text == string.Empty ? DateTime.Now.AddYears(-100).ToString() : tbDate.Text);
                //考试时长
                TextBox tbSpan = (TextBox)gvr.FindControl("tbSpan");
                int span = Convert.ToInt32(tbSpan.Text == string.Empty ? "-1" : tbSpan.Text);
                //采用的试卷
                TextBox tbTestPager = (TextBox)gvr.FindControl("tbTestPager");
                int tpID = Convert.ToInt32(tbTestPager.Text == string.Empty ? "-1" : tbTestPager.Text);
                Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle(tpID);
                //专业
                DropDownList sbjddl = (DropDownList)gvr.FindControl("ddlSubject");
                int sbjID = 0;
                string sbjName = "";
                if (sbjddl != null && sbjddl.Items.Count > 0)
                {
                    sbjID = Convert.ToInt32(sbjddl.SelectedValue);
                    sbjName = sbjddl.SelectedItem.Text;
                }
                Song.Entities.Subject sbj = Business.Do<ISubject>().SubjectSingle(sbjID);
                if (sbj != null) sbjName = sbj.Sbj_Name;
                

                //创建场次对象
                Song.Entities.Examination exam = itemID < 1 ? new Song.Entities.Examination() : Business.Do<IExamination>().ExamSingle(itemID);
                exam = exam == null ? new Song.Entities.Examination() : exam;
                //赋值
                exam.Exam_UID = getUID();
                exam.Sbj_ID = sbjID;
                exam.Sbj_Name = sbjName;
                exam.Exam_Name = name;
                exam.Exam_Total = total;
                exam.Exam_PassScore = passcore;
                exam.Exam_Date = date;
                exam.Exam_Span = span;
                exam.Tp_Id = tpID;
                exam.Exam_Tax = i + 1;
                items.Add(exam);
            }
            //校验，各场次考试主题不可以相同
            bool isExists = false;
            foreach (Song.Entities.Examination e in items)
            {
                if (string.IsNullOrWhiteSpace(e.Exam_Name)) continue;
                foreach (Song.Entities.Examination j in items)
                {
                    if (e.Exam_Tax == j.Exam_Tax) continue;                   
                    if (e.Exam_Name == j.Exam_Name)
                    {
                        isExists = true;
                        break;
                    }
                }
                if (isExists) break;
            }
            if (isExists) throw new Exception("各场次考试主题不可以相同");
            return items;
        }

        /// <summary>
        /// 绑定场次中的学科信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //当为数据行时
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //学科/专业
                DropDownList ddl = (DropDownList)e.Row.FindControl("ddlSubject");
                ddl.DataSource = ddlSubject.DataSource;
                ddl.DataTextField = "Sbj_Name";
                ddl.DataValueField = "Sbj_ID";
                ddl.DataBind();
                if (ddl.Items.Count < 1) ddl.Items.Insert(0, new ListItem("--（没有专业）--", "-1"));
                if (ddl.Items.Count > 0) ddl.Items.Insert(0, new ListItem("", "-1"));

                //当前选中状态
                Song.Entities.Examination exam = (Song.Entities.Examination)e.Row.DataItem;
                ListItem liSubj = ddl.Items.FindByValue(exam.Sbj_ID.ToString());
                if (liSubj != null) liSubj.Selected = true;

                //当前试卷
                if (exam.Sbj_ID > 0)
                {
                    Song.Entities.TestPaper[] tps = Business.Do<ITestPaper>().PagerCount(org.Org_ID, (int)exam.Sbj_ID, -1, -1, true, 0);
                    DropDownList tpDdl = (DropDownList)e.Row.FindControl("ddlTestPager");
                    tpDdl.DataSource = tps;
                    tpDdl.DataTextField = "Tp_Name";
                    tpDdl.DataValueField = "Tp_Id";
                    tpDdl.DataBind();
                    for (int i = 0; i < tps.Length; i++)
                    {
                        tpDdl.Items[i].Attributes.Add("span", tps[i].Tp_Span.ToString());
                        tpDdl.Items[i].Attributes.Add("total", tps[i].Tp_Total.ToString());
                        tpDdl.Items[i].Attributes.Add("passScore", tps[i].Tp_PassScore.ToString());
                    }
                    ListItem liTp = tpDdl.Items.FindByValue(exam.Tp_Id.ToString());
                    if (liTp != null) liTp.Selected = true;
                }
            }

        }
        #endregion

        /// <summary>
        /// 考试时间类型设置，是固定时间，还是区间时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblExamTimeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            string type = rbl.SelectedValue;
            plDateTimeType1.Visible = type == "1";
            plDateTimeType2.Visible = type == "2";
        }

    }
}
