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
    public partial class Select : Extend.CustomPage
    {
        //所归属的站点类型
        private string site = WeiSha.Common.Request.QueryString["site"].String;
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
            {
                plWeb.Visible = site == "web";
                plMobi.Visible = site == "mobi";
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //刷新模板信息
            WeiSha.Common.Template.RefreshTemplate();
            //web模板列表
            if (site == "web")
            {
                WeiSha.Common.Templates.TemplateBank[] tem = Business.Do<Song.ServiceInterfaces.ITemplate>().WebTemplates();
                WeiSha.Common.Template.ForWeb.SetCurrent(org.Org_Template);
                rtpTemplate.DataSource = tem;
                rtpTemplate.DataBind();
            }
            //手机模板列表
            if (site == "mobi")
            {
                WeiSha.Common.Templates.TemplateBank[] mobitm = Business.Do<Song.ServiceInterfaces.ITemplate>().MobiTemplates();
                WeiSha.Common.Template.ForMobile.SetCurrent(org.Org_TemplateMobi);
                rtpTempMobi.DataSource = mobitm;
                rtpTempMobi.DataBind();
            }
        }
        /// <summary>
        /// 选择当前模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelectWeb_Click(object sender, EventArgs e)
        {            
            LinkButton ub = (LinkButton)sender;
            string tag = ub.CommandArgument;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Business.Do<Song.ServiceInterfaces.ITemplate>().SetWebCurr(org.Org_ID,tag);
            Song.Template.Cache.Clear();
            BindData(null, null);           
        }
        /// <summary>
        /// 选择当前手机模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSelectMobi_Click(object sender, EventArgs e)
        {
            LinkButton ub = (LinkButton)sender;
            string tag = ub.CommandArgument;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Business.Do<Song.ServiceInterfaces.ITemplate>().SetMobiCurr(org.Org_ID, tag);
            Song.Template.Cache.Clear();
            BindData(null, null);
        }
    }
}
