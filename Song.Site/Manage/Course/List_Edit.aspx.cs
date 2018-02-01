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



namespace Song.Site.Manage.Course
{
    public partial class List_Edit : Extend.CustomPage
    {
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //上传资料的所有路径
        private string _uppath = "Course";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //学科/专业
            Song.Entities.Subject[] subs = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, 0, 0);
            this.ddlSubject.DataSource = subs;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.DataBind();
        }

        private void fill()
        {
            Song.Entities.Course cou = couid < 1 ? new Song.Entities.Course() : Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return;
            Cou_Name.Text = cou.Cou_Name;
            //所属专业
            ListItem liSbj = ddlSubject.Items.FindByValue(cou.Sbj_ID.ToString());
            if (liSbj != null)
            {
                ddlSubject.SelectedIndex = -1;
                liSbj.Selected = true;
            }            
            //简介，学习目标
            tbIntro.Text = cou.Cou_Intro;
            tbTarget.Text = cou.Cou_Target;
            //课程图片
            if (!string.IsNullOrEmpty(cou.Cou_LogoSmall) && cou.Cou_LogoSmall.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + cou.Cou_LogoSmall;
            }
            if (couid > 0)
            {
                cbIsUse.Checked = cou.Cou_IsUse;
            }
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Course cou = couid < 1 ? new Song.Entities.Course() : Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return;
            //名称
            cou.Cou_Name = Cou_Name.Text.Trim();
            //所属专业
            cou.Sbj_ID = Convert.ToInt32(ddlSubject.SelectedValue);
            cou.Sbj_Name = ddlSubject.SelectedItem.Text;
            //简介，学习目标
            cou.Cou_Intro = tbIntro.Text;
            cou.Cou_Target = tbTarget.Text;
            //主图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = true;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SmallHeight = 113;
                    fuLoad.SmallWidth = 200;
                    fuLoad.SaveAndDeleteOld(cou.Cou_Logo);
                    fuLoad.File.Server.ChangeSize(500, 500, false);
                    cou.Cou_Logo = fuLoad.File.Server.FileName;
                    cou.Cou_LogoSmall = fuLoad.File.Server.SmallFileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            cou.Cou_IsUse = cbIsUse.Checked;
            ////所属教师
            //Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            //cou.Th_ID = th.Th_ID;
            //cou.Th_Name = th.Th_Name;
            ////所属机构
            //Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //cou.Org_ID = org.Org_ID;
            //cou.Org_Name = org.Org_Name;
            try
            {
                if (couid < 1)
                {
                    //保存
                    Business.Do<ICourse>().CourseAdd(cou);
                }
                else
                {
                    Business.Do<ICourse>().CourseSave(cou);
                }
                Master.AlertCloseAndRefresh("操作成功");
            }
            catch
            {
                throw;
            }
            
        }
       
    }
}
