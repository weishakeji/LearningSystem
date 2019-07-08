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

namespace Song.Site.Manage.Sys
{
    public partial class Subject_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        public int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //上传资料的所有路径
        private string _uppath = "Subject";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                init();
                fill();
            }
        }
        private void init()
        {
            int depid = 0;          
            Song.Entities.Subject[] eas = null;
            eas = Business.Do<ISubject>().SubjectCount(org.Org_ID, depid, null, true, -1, -1);
            ddlTree.DataSource = eas;
            this.ddlTree.DataTextField = "Sbj_Name";
            this.ddlTree.DataValueField = "Sbj_ID";
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            //
            ddlTree.Items.Insert(0, new ListItem("   -- 顶级 --", "0"));
            //当前Id
            ddlTree.Attributes.Add("currid", this.id.ToString());
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            Song.Entities.Subject mm = id == 0 ? new Song.Entities.Subject() : Business.Do<ISubject>().SubjectSingle(id);
            if (mm == null) return;
            if (id > 0)
            {
                this.EntityBind(mm);

                //专业Logo图片
                if (!string.IsNullOrEmpty(mm.Sbj_LogoSmall) && mm.Sbj_LogoSmall.Trim() != "")
                {
                    this.imgShow.Src = Upload.Get[_uppath].Virtual + mm.Sbj_LogoSmall;
                }
                //所属上级
                ListItem liPID = ddlTree.Items.FindByValue(mm.Sbj_PID.ToString());
                if (liPID != null)
                {
                    ddlTree.SelectedIndex = -1;
                    liPID.Selected = true;
                }
               
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Subject mm = id == 0 ? new Song.Entities.Subject() : Business.Do<ISubject>().SubjectSingle(id);
            mm = this.EntityFill(mm) as Song.Entities.Subject;
            mm.Sbj_Name = Sbj_Name.Text.Trim();
            //所属院系
            int depid = 0;
            mm.Dep_Id = depid;
            //所属上级
            int pid = 0;
            int.TryParse(ddlTree.SelectedValue, out pid);
            mm.Sbj_PID = pid;
            //主图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = true;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SmallHeight = 200;
                    fuLoad.SmallWidth = 200;
                    fuLoad.SaveAndDeleteOld(mm.Sbj_Logo);
                    //fuLoad.File.Server.ChangeSize(500, 500, false);
                    mm.Sbj_Logo = fuLoad.File.Server.FileName;
                    mm.Sbj_LogoSmall = fuLoad.File.Server.SmallFileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            //确定操作
            try
            {
                if (id == 0)
                {
                    Business.Do<ISubject>().SubjectAdd(mm);
                }
                else
                {
                    Business.Do<ISubject>().SubjectSave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }

    }
}
