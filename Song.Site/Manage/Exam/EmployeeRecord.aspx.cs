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
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Song.Site.Manage.Exam
{
    public partial class EmployeeRecord : Extend.CustomPage
    {
        //试题的类型
        int type = WeiSha.Common.Request.QueryString["type"].Int16 ?? 1;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnSear.UniqueID;
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
            try
            {
                //院系
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(orgid,true,true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                this.ddlDepart.Items.Insert(0, new ListItem(" -- 所有院系 -- ", "-1"));
                //班组
                Song.Entities.Team[] teams = Business.Do<ITeam>().GetTeam(true, 0);
                this.ddlTeam.DataSource = teams;
                this.ddlTeam.DataTextField = "Team_Name";
                this.ddlTeam.DataValueField = "Team_ID";
                this.ddlTeam.DataBind();
                this.ddlTeam.Items.Insert(0, new ListItem(" -- 全部 -- ", "-1"));

                ddlTeam_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                //考试场次
                int examid = Convert.ToInt32(ddlExam2.SelectedValue);
                //考试uid
                string examuid = ddlExam.SelectedValue;
                Song.Entities.ExamResults[] eas = null;
                if (examid > 0)
                {
                    eas = Business.Do<IExamination>().Results(examid, Pager1.Size, Pager1.Index, out count);
                }
                else
                {
                    eas = Business.Do<IExamination>().Results(examuid, Pager1.Size, Pager1.Index, out count);
                }                
                GridView1.DataSource = eas;                
                GridView1.DataKeyNames = new string[] { "Exr_ID" };
                GridView1.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }

        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            try
            {
                Pager1.Index = 1;
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IExamination>().ResultDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }

        }
        /// <summary>
        /// 当院系更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //院系
                int depid = Convert.ToInt32(this.ddlDepart.SelectedValue);
                if (depid < 0) return;
                //班组
                Song.Entities.Team[] teams = Business.Do<ITeam>().GetTeam(true, depid, 0);
                this.ddlTeam.DataSource = teams;
                this.ddlTeam.DataTextField = "Team_Name";
                this.ddlTeam.DataValueField = "Team_ID";
                this.ddlTeam.DataBind();
                this.ddlTeam.Items.Insert(0, new ListItem(" -- 全部 -- ", "-1"));

                //
                ddlTeam_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 班组更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //
            List<Song.Entities.Examination> teams = Business.Do<IExamination>().ExamCount(org.Org_ID,true,0);
            this.ddlExam.DataSource = teams;
            this.ddlExam.DataTextField = "Exam_Title";
            this.ddlExam.DataValueField = "Exam_UID";
            this.ddlExam.DataBind();

            ddlExam_SelectedIndexChanged(null, null);           
        }
        /// <summary>
        /// 考试信息更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int examid = Convert.ToInt32(ddlExam.SelectedValue == string.Empty ? "0" : ddlExam.SelectedValue);
                //if (examid < 1) return;

                string uid = ddlExam.SelectedValue;
                Song.Entities.Examination[] exam = Business.Do<IExamination>().ExamItem(uid);
                this.ddlExam2.DataSource = exam;
                this.ddlExam2.DataTextField = "Sbj_Name";
                this.ddlExam2.DataValueField = "Exam_ID";
                this.ddlExam2.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

        /// <summary>
        /// 获取试卷名称
        /// </summary>
        /// <param name="tpid"></param>
        /// <returns></returns>
        protected string getTestPager(string tpid)
        {
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PagerSingle(Convert.ToInt32(tpid));
            return tp != null ? tp.Tp_Name : "【试卷不存在，可能已经被删除】";
        }
    }
}
