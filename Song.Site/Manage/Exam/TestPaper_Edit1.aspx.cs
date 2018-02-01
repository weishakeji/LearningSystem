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
    public partial class TestPaper_Edit1 : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        private int type = WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        //题型分类汉字名称
        protected string[] typeStr = App.Get["QuesType"].Split(',');
        private string _uppath = "TestPaper";
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
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //学科/专业
            Song.Entities.Subject[] subs = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, 0);
            this.ddlSubject.DataSource = subs;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.DataBind();
        }
        /// <summary>
        /// 当专业下拉菜单更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //课程
            int sbjid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            if (sbjid > 0)
            {
                ddlCourse.Items.Clear();
                List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, true, -1);
                ddlCourse.DataSource = cous;
                this.ddlCourse.DataTextField = "Cou_Name";
                this.ddlCourse.DataValueField = "Cou_ID";
                this.ddlCourse.Root = 0;
                this.ddlCourse.DataBind();
            }
            //
            if (ddlCourse.Items.Count < 1) ddlCourse.Items.Add(new ListItem("--没有课程--", "0"));

        }
        void fill()
        {
            Song.Entities.TestPaper mm = id != 0 ? Business.Do<ITestPaper>().PagerSingle(id) : new Song.Entities.TestPaper();
            if (id != 0)
            {
                mm = Business.Do<ITestPaper>().PagerSingle(id);
                //是否使用与推荐
                cbIsUse.Checked = mm.Tp_IsUse;
                cbIsRec.Checked = mm.Tp_IsRec;
                //唯一标识
                ViewState["UID"] = mm.Tp_UID;              
                //所属专业
                ListItem liSubj = ddlSubject.Items.FindByValue(mm.Sbj_ID.ToString());
                if (liSubj != null)
                {
                    ddlSubject.SelectedIndex = -1;
                    liSubj.Selected = true;
                    ddlSubject_SelectedIndexChanged(null, null);
                }
                //所属课程
                ListItem liCous = this.ddlCourse.Items.FindByValue(mm.Cou_ID.ToString());
                if (liCous != null)
                {
                    ddlCourse.SelectedIndex = -1;
                    liCous.Selected = true;
                }
                //难度
                ListItem liDiff = ddlDiff.Items.FindByValue(mm.Tp_Diff.ToString());
                if (liDiff != null)
                {
                    ddlDiff.SelectedIndex = -1;
                    liDiff.Selected = true;
                }
                ListItem liDiff2 = ddlDiff2.Items.FindByValue(mm.Tp_Diff2.ToString());
                if (liDiff2 != null)
                {
                    ddlDiff2.SelectedIndex = -1;
                    liDiff2.Selected = true;
                }
                //时长，总分，及格分
                tbSpan.Text = mm.Tp_Span.ToString();
                tbTotal.Text = mm.Tp_Total.ToString();
                tbPassScore_.Text = mm.Tp_PassScore.ToString();
                //各项题型分值
                Song.Entities.TestPaperItem[] tpi = Business.Do<ITestPaper>().GetItemForAll(mm);
                foreach (Song.Entities.TestPaperItem pi in tpi)
                {
                    int type = (int)pi.TPI_Type;
                    //几道题
                    TextBox tbCount = (TextBox)this.getWebControl("tbItem" + type + "Count");
                    //占多少分数比
                    TextBox tbScore = (TextBox)this.getWebControl("tbItem" + type + "Score");
                    //占多少分
                    Label lbNumber = (Label)this.getWebControl("lbItem" + type + "Number");
                    TextBox tbNumber = (TextBox)this.getWebControl("tbItem" + type + "Number");
                    tbCount.Text = pi.TPI_Count.ToString();
                    tbScore.Text = pi.TPI_Percent.ToString();
                }
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.TestPaper();
                ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                ddlSubject_SelectedIndexChanged(null, null);
            }

            //试卷名称
            tbName.Text = mm.Tp_Name;
            //简介
            tbIntro.Text = mm.Tp_Intro;
            //个人照片
            if (!string.IsNullOrEmpty(mm.Tp_Logo) && mm.Tp_Logo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + mm.Tp_Logo;
            }
            
        }
        /// <summary>
        /// 获取页面中控件
        /// </summary>
        /// <param name="ctlName"></param>
        /// <returns></returns>
        private Control getWebControl(string ctlName)
        {
            ContentPlaceHolder cph = null;
            foreach (Control control in this.Form.Controls)
            {
                if (control.ID == "cphMain")
                {
                    cph = (ContentPlaceHolder)control;
                    break;
                }
            }
            foreach (Control control in cph.Controls)
            {
                if (control.ID == ctlName)
                {
                    return control;
                }
            }
            return null;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.TestPaper mm = id != 0 ? Business.Do<ITestPaper>().PagerSingle(id) : new Song.Entities.TestPaper();
            try
            {
                //试卷名称
                mm.Tp_Name = tbName.Text.Trim();
                //是否使用与推荐
                mm.Tp_IsUse = cbIsUse.Checked;
                mm.Tp_IsRec = cbIsRec.Checked;
                //类型
                mm.Tp_Type =1;
                //专业，难度
                mm.Sbj_ID = Convert.ToInt32(ddlSubject.SelectedValue);
                mm.Sbj_Name = ddlSubject.SelectedText;
                mm.Cou_ID = Convert.ToInt32(ddlCourse.SelectedValue);
                mm.Tp_Diff = Convert.ToInt32(ddlDiff.SelectedValue);
                mm.Tp_Diff2 = Convert.ToInt32(ddlDiff2.SelectedValue);
                //时长与总分
                mm.Tp_Span = Convert.ToInt32(tbSpan.Text);
                mm.Tp_Total = Convert.ToInt32(tbTotal.Text);
                mm.Tp_PassScore = Convert.ToInt32(tbPassScore_.Text);
                //简介
                mm.Tp_Intro = tbIntro.Text;
                //UID
                mm.Tp_UID = getUID();
                //图片
                if (fuLoad.PostedFile.FileName != "")
                {
                    try
                    {
                        fuLoad.UpPath = _uppath;
                        fuLoad.IsMakeSmall = false;
                        fuLoad.IsConvertJpg = true;
                        fuLoad.SaveAndDeleteOld(mm.Tp_Logo);
                        fuLoad.File.Server.ChangeSize(200, 200, false);
                        mm.Tp_Logo = fuLoad.File.Server.FileName;
                        //
                        imgShow.Src = fuLoad.File.Server.VirtualPath;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Alert(ex);
            }

            //确定操作
            try
            {
                //各项题型占比
                Song.Entities.TestPaperItem[] tpi = new TestPaperItem[5];
                for (int i = 0; i < tpi.Length; i++)
                {
                    Song.Entities.TestPaperItem pi = new TestPaperItem();
                    //几道题
                    TextBox tbCount = (TextBox)this.getWebControl("tbItem" + (i + 1) + "Count");
                    //占多少分数比
                    TextBox tbScore = (TextBox)this.getWebControl("tbItem" + (i + 1) + "Score");
                    TextBox tbNumber = (TextBox)this.getWebControl("tbItem" + (i + 1) + "Number");
                    pi.TPI_Count = Convert.ToInt32(tbCount.Text.Trim() == "" ? "0" : tbCount.Text);
                    pi.TPI_Percent = Convert.ToInt32(tbScore.Text.Trim() == "" ? "0" : tbScore.Text);
                    pi.TPI_Number = Convert.ToInt32(tbNumber.Text.Trim() == "" ? "0" : tbNumber.Text);
                    pi.TPI_Type = i + 1;
                    tpi[i] = pi;
                }
                //if (id == 0)
                    //id = Business.Do<ITestPaper>().PagerAdd(mm, tpi);
                //else
                    //Business.Do<ITestPaper>().PagerSave(mm, tpi);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
