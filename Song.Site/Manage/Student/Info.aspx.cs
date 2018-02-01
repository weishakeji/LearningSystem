using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Manage.Student
{
    public partial class Info : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private string _uppath = "Accounts";
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
            //如果id不存在，则取当前登录的账户
            Song.Entities.Accounts acc = id == 0 ? this.Master.Account : Business.Do<IAccounts>().AccountsSingle(id);
            if (acc == null) return;
            this.EntityBind(acc);           
            //出生年月
            Ac_Birthday.Text = acc.Ac_Birthday < DateTime.Now.AddYears(-100) ? "" : acc.Ac_Birthday.ToString("yyyy-MM-dd");
            //个人照片
            if (!string.IsNullOrEmpty(acc.Ac_Photo) && acc.Ac_Photo.Trim() != "")
            {               
                this.imgShow.Src = Upload.Get[_uppath].Virtual + acc.Ac_Photo;
            }
            //Ac_Name.Enabled = Ac_Name.Text == "";
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Accounts acc = id == 0 ? this.Master.Account : Business.Do<IAccounts>().AccountsSingle(id);
            acc = this.EntityFill(acc) as Song.Entities.Accounts;
            if (acc.Ac_Birthday > DateTime.Now.AddYears(-100))
            {
                acc.Ac_Age = Convert.ToInt32((DateTime.Now - acc.Ac_Birthday).TotalDays / 365);
                //th.Ac_Age = DateTime.Now.Year - th.Ac_Birthday.Year;
            }
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(acc.Ac_Photo);
                    fuLoad.File.Server.ChangeSize(150, 200, false);
                    acc.Ac_Photo = fuLoad.File.Server.FileName;
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
                Business.Do<IAccounts>().AccountsSave(acc);
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