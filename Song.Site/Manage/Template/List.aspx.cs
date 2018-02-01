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

namespace Song.Site.Manage.Template
{
    public partial class List : Extend.CustomPage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
            {  
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //web模板列表
            WeiSha.Common.Templates.TemplateBank[] tem = Business.Do<Song.ServiceInterfaces.ITemplate>().WebTemplates();
            rtpTemplate.DataSource = tem;
            rtpTemplate.DataBind();
            //手机模板列表
            WeiSha.Common.Templates.TemplateBank[] mobitm = Business.Do<Song.ServiceInterfaces.ITemplate>().MobiTemplates();
            rtpTempMobi.DataSource = mobitm;
            rtpTempMobi.DataBind();
        }

    }
}
