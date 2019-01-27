using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WeiSha.Common;
using WeiSha.WebControl;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Course
{
    public partial class Courses_Edit : Extend.CustomPage
    {
        //课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //上传资料的所有路径
        private string _uppath = "Course";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            //隐藏确定按钮
            EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
            btnEnter.Visible = couid > 0;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
            this.Master.Enter_Click += btnEnter_Click;
            this.Master.Next_Click += btnNext_Click;
        }
        #region 初始化
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            //所属专业
            Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(org.Org_ID, null, true, -1, -1);
            ddlSubject.DataSource = sbjs;
            this.ddlSubject.DataTextField = "Sbj_Name";
            this.ddlSubject.DataValueField = "Sbj_ID";
            this.ddlSubject.Root = 0;
            this.ddlSubject.DataBind();
            if (ddlSubject.Items.Count < 1)
            {
                ddlSubject.Items.Add(new ListItem("--（没有专业）--","-1"));
            }
            ddlSubject.Items.Insert(0,new ListItem("--所属专业--", "-1"));
        }
        /// <summary>
        /// 当专业选择改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sbjid;
            int.TryParse(ddlSubject.SelectedValue, out sbjid);
            //上级
            List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseCount(org.Org_ID, sbjid, null, true, -1);
            ddlTree.DataSource = cous;
            this.ddlTree.DataTextField = "Cou_Name";
            this.ddlTree.DataValueField = "Cou_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            //
            ddlTree.Items.Insert(0, new ListItem("   -- 顶级 --", "0"));
        }
        #endregion

        private void fill()
        {
            Song.Entities.Course cou = couid < 1 ? new Song.Entities.Course() : Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return;
            if (couid > 0)
            {
                cbIsUse.Checked = cou.Cou_IsUse;    //启用
                cbIsRec.Checked = cou.Cou_IsRec;    //推荐
            }
            Cou_Name.Text = cou.Cou_Name;
            //全局UID
            ViewState["UID"] = string.IsNullOrWhiteSpace(cou.Cou_UID) ? getUID() : cou.Cou_UID;
            //所属专业
            ListItem liSbj = ddlSubject.Items.FindByValue(cou.Sbj_ID.ToString());
            if (liSbj != null)
            {
                ddlSubject.SelectedIndex = -1;
                liSbj.Selected = true;
            }
            ddlSubject_SelectedIndexChanged(null, null);
            //上级课程
            ListItem liCou = ddlTree.Items.FindByValue(cou.Cou_PID.ToString());
            if (liCou != null)
            {
                ddlTree.SelectedIndex = -1;
                liCou.Selected = true;
            }
            //简介，学习目标
            tbIntro.Text = cou.Cou_Intro;
            tbTarget.Text = cou.Cou_Target;
            //课程图片
            if (!string.IsNullOrEmpty(cou.Cou_LogoSmall) && cou.Cou_LogoSmall.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + cou.Cou_LogoSmall;
            }         
            //访问人数
            tbViewNum.Text = cou.Cou_ViewNum.ToString();
            //学习人数
            lbStudentSum.Text = Business.Do<ICourse>().CourseStudentSum(couid, false).ToString();
            ltStudentSum.Text = Business.Do<ICourse>().CourseStudentSum(couid, true).ToString();

        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Course cou=this.Save();
            this.Alert("操作成功");
        }
        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Song.Entities.Course cou = this.Save();
            Master.Couid = cou.Cou_ID;
        }
        /// <summary>
        /// 保存当前课程
        /// </summary>
        /// <returns></returns>
        private Song.Entities.Course Save()
        {
            Song.Entities.Course cou = couid < 1 ? new Song.Entities.Course() : Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return null;
            //名称
            cou.Cou_Name = Cou_Name.Text.Trim();
            //访问人数
            int viewnum = 0;
            int.TryParse(tbViewNum.Text, out viewnum);
            cou.Cou_ViewNum = viewnum;
            //所属专业
            cou.Sbj_ID = Convert.ToInt32(ddlSubject.SelectedValue);
            cou.Sbj_Name = ddlSubject.SelectedText;
            //上级
            cou.Cou_PID = Convert.ToInt32(ddlTree.SelectedValue);
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
                    fuLoad.SmallWidth = 400;
                    fuLoad.SmallHeight = fuLoad.SmallWidth * 9 / 16;                   
                    fuLoad.SaveAndDeleteOld(cou.Cou_Logo);
                    //截取图片宽高
                    int width = fuLoad.File.Server.Width;
                    width = width > 1000 ? 1000 : width;
                    int height = width * 9 / 16;  //宽高比为16:9
                    fuLoad.File.Server.ChangeSize(width, height, false);
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
            cou.Cou_IsUse = cbIsUse.Checked;    //启用
            cou.Cou_IsRec = cbIsRec.Checked;    //推荐
            cou.Cou_UID = getUID();            
            try
            {
                if (couid < 1)
                {
                    //所属老师                    
                    if (Extend.LoginState.Accounts.IsLogin)
                    {
                        Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
                        if (th != null)
                        {
                            cou.Th_ID = th.Th_ID;
                            cou.Th_Name = th.Th_Name;
                        }
                    }
                    Business.Do<ICourse>().CourseAdd(cou);
                }
                else
                {
                    Business.Do<ICourse>().CourseSave(cou);
                }
                this.Alert("操作成功");
            }
            catch
            {
                throw;
            }
            return cou;
        }
    }
}