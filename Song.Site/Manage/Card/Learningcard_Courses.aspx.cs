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
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Card
{
    public partial class Learningcard_Courses : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Song.Entities.Organization[] orgs = Business.Do<IOrganization>().OrganAll(true, -1);
                ddlOrg.DataSource = orgs;
                ddlOrg.DataBind();
                ddlOrg_SelectedIndexChanged(null, null);
                
            }
        }
        /// <summary>
        /// 机构变更时，更新专业信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
        {
            int orgid = 0;
            int.TryParse(ddlOrg.SelectedValue, out orgid);
            Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(orgid, null, true, -1, 0);
            ddlSubject.DataSource = sbjs;
            ddlSubject.DataTextField = "Sbj_Name";
            ddlSubject.DataValueField = "Sbj_ID";
            ddlSubject.DataBind();
            this.ddlSubject.Items.Insert(0, new ListItem(" -- 专业 -- ", "-1"));
            ddlSubject_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 当专业变更时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData(null, null);
        }
         /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            bool? isUse = null;
            List<Song.Entities.Course> eas = null;
            int orgid = 0;
            int.TryParse(ddlOrg.SelectedValue, out orgid);
            int sbjid = Convert.ToInt32(ddlSubject.SelectedValue);
            eas = Business.Do<ICourse>().CoursePager(orgid, sbjid, -1, isUse, tbCour.Text, "tax", Pager1.Size, Pager1.Index, out count);
            foreach (Song.Entities.Course s in eas)
            {
                if (string.IsNullOrEmpty(s.Sbj_Name) || s.Sbj_Name.Trim() == "")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectSingle(s.Sbj_ID);
                    if (subject != null) s.Sbj_Name = subject.Sbj_Name;
                    Business.Do<ICourse>().CourseSave(s);
                }
                if (string.IsNullOrEmpty(s.Cou_Intro) || s.Cou_Intro.Trim() == "") continue;
                if (s.Cou_Intro.Length > 20)
                {
                    s.Cou_Intro = s.Cou_Intro.Substring(0, 20) + "...";
                }
            }
            gvCourses.DataSource = eas;
            gvCourses.DataKeyNames = new string[] { "Cou_ID" };
            gvCourses.DataBind();

            Pager1.RecordAmount = count;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
    }
}
