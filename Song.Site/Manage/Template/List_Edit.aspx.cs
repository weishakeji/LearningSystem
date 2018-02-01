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



namespace Song.Site.Manage.Template
{
    public partial class List_Edit : Extend.CustomPage
    {
        //模板标识
        private string tag = WeiSha.Common.Request.QueryString["tag"].String;
        //模板类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
       
        private void fill()
        {
            WeiSha.Common.Templates.TemplateBank tmp = WeiSha.Common.Template.GetTemplate(type,tag);
            tbName.Text = tmp.Name;
            tbAuthor.Text = tmp.Author;
            tbPhone.Text = tmp.Phone;
            tbQQ.Text = tmp.QQ;
            tbCrtTime.Text = tmp.CrtTime.ToString("yyyy-MM-dd");
            tbIntro.Text = tmp.Intro;
            imgShow.Src = tmp.Logo + "?s=" + DateTime.Now.Ticks;
            lbFileName.Text = tmp.Path.Virtual;
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            WeiSha.Common.Templates.TemplateBank tmp = WeiSha.Common.Template.GetTemplate(type, tag);
            tmp.Name = tbName.Text.Trim();
            tmp.Author = tbAuthor.Text.Trim();
            tmp.Phone = tbPhone.Text.Trim();
            tmp.QQ = tbQQ.Text.Trim();
            tmp.CrtTime = Convert.ToDateTime(tbCrtTime.Text);
            tmp.Intro = tbIntro.Text.Trim();
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.NewName = "logo.jpg";
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAs(this.Server.MapPath(tmp.Logo));
                    WeiSha.Common.Images.FileTo.Zoom(this.Server.MapPath(tmp.Logo), 200, 200, false);
                    imgShow.Src = tmp.Logo + "?s=" + DateTime.Now.Ticks;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            tmp.Save();
            WeiSha.Common.Template.RefreshTemplate();
            Master.AlertCloseAndRefresh("操作成功！");
        }
       
    }
}
