using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
namespace Song.Site.Manage.Questions
{
    public partial class Export : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!IsPostBack)
            {
                //题型
                string[] types = App.Get["QuesType"].Split(',');
                cblType.DataSource = types;
                cblType.DataBind();
                for (int i = 0; i < cblType.Items.Count; i++)
                {
                    cblType.Items[i].Selected = true;
                    cblType.Items[i].Value = (i + 1).ToString();
                }
                InitBind();
            }
        }
        #region 初始操作
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            //专业
            Song.Entities.Subject[] subjects = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, -1);
            ddlSubject.DataSource = subjects;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.Root = 0;
            this.ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, new ListItem("-专业-", "0"));
            //
            //this.SearchBind(this.searchBox);
            ddlSubject_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 专业选择变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlCourse.Items.Clear();
            int sbjid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            if (sbjid > 0)
            {
                //上级
                List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, true, -1);
                ddlCourse.DataSource = cous;
                this.ddlCourse.DataTextField = "Cou_Name";
                this.ddlCourse.DataValueField = "Cou_ID";
                this.ddlCourse.Root = 0;
                this.ddlCourse.DataBind();
            }
            ddlCourse.Items.Insert(0, new ListItem("-课程-", "0"));
            //this.SearchBind(this.searchBox);
            ddlCourse_SelectedIndexChanged(null, null);
        }
        /// <summary>
        /// 课程选择项变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlOutline.Items.Clear();
            //课程id
            int cou;
            int.TryParse(ddlCourse.SelectedValue, out cou);
            if (cou > 0)
            {
                Song.Entities.Outline[] outline = Business.Do<IOutline>().OutlineAll(cou, null);
                ddlOutline.DataSource = outline;
                this.ddlOutline.DataTextField = "Ol_Name";
                this.ddlOutline.DataValueField = "Ol_ID";
                ddlOutline.DataBind();
            }
            this.ddlOutline.Items.Insert(0, new ListItem("-章节-", "0"));
            ////this.SearchBind(this.searchBox);

        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //题型
            string types = "";
            foreach (ListItem li in cblType.Items)
                if (li.Selected) types += li.Value + ",";
            //专业，课程，章节
            int sbjid, couid, olid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            int.TryParse(ddlCourse.SelectedValue, out couid);
            int.TryParse(ddlOutline.SelectedValue, out olid);
            //难度
            string diffs = "";
            foreach (ListItem li in cblDiff.Items)
                if (li.Selected) diffs += li.Value + ",";
            //导出
            HSSFWorkbook hssfworkbook = null;
            //导出所有
            if (rblOther.SelectedValue == "1") hssfworkbook = Business.Do<IQuestions>().QuestionsExport(org.Org_ID, types, sbjid, couid, olid, diffs, null, null);
            //导出正常的试题，没有错误，没有用户反馈说错误的
            if (rblOther.SelectedValue == "2") hssfworkbook = Business.Do<IQuestions>().QuestionsExport(org.Org_ID, types, sbjid, couid, olid, diffs, false, false);
            //导出状态为错误的试题
            if (rblOther.SelectedValue == "3") hssfworkbook = Business.Do<IQuestions>().QuestionsExport(org.Org_ID, types, sbjid, couid, olid, diffs, true, null);
            //导出用户反馈说错误的试题
            if (rblOther.SelectedValue == "4") hssfworkbook = Business.Do<IQuestions>().QuestionsExport(org.Org_ID, types, sbjid, couid, olid, diffs, null, true);

            //创建文件
            string filename = "试题导出_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";
            string filePath = Upload.Get["Temp"].Physics + filename;
            FileStream file = new FileStream(filePath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
    }
}