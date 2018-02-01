using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Manage.Teacher
{
    public partial class Info : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private string _uppath = "Teacher";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnEnter.UniqueID;
            if (!IsPostBack)
            {
                fill();
            }
        }

        private void fill()
        {
            Song.Entities.Teacher th = id == 0 ? Extend.LoginState.Accounts.Teacher : Business.Do<ITeacher>().TeacherSingle(id);
            if (th == null) return;
            this.EntityBind(th);           
            //出生年月
            Th_Birthday.Text = th.Th_Birthday < DateTime.Now.AddYears(-100) ? "" : th.Th_Birthday.ToString("yyyy-MM-dd");
            //个人照片
            if (!string.IsNullOrEmpty(th.Th_Photo) && th.Th_Photo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + th.Th_Photo;
            }
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
            th = this.EntityFill(th) as Song.Entities.Teacher;
            if (th.Th_Birthday > DateTime.Now.AddYears(-100))
            {
                th.Th_Age = Convert.ToInt32((DateTime.Now - th.Th_Birthday).TotalDays / 365);
                //th.Th_Age = DateTime.Now.Year - th.Th_Birthday.Year;
            }
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(th.Th_Photo);
                    fuLoad.File.Server.ChangeSize(150, 200, false);
                    th.Th_Photo = fuLoad.File.Server.FileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            try
            {
                Business.Do<ITeacher>().TeacherSave(th);
                this.Alert("操作完成");
                this.JsFunction("top.setPhoto", imgShow.Src);
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
    }
}