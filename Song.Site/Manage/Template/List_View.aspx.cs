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
    public partial class List_View : Extend.CustomPage
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
           
        }
       
        private void fill()
        {
            WeiSha.Common.Templates.TemplateBank tmp = WeiSha.Common.Template.GetTemplate(type, tag);
            ltName.Text = tmp.Name;
            ltAuthor.Text = tmp.Author;
            ltPhone.Text = tmp.Phone;
            ltQQ.Text = tmp.QQ;
            ltCrtTime.Text = tmp.CrtTime.ToString();
            ltIntro.Text = tmp.Intro;
            imgShow.Src = tmp.Logo + "?s=" + DateTime.Now.Ticks;
            lbFileName.Text = tmp.Path.Virtual;
        }        
       
    }
}
