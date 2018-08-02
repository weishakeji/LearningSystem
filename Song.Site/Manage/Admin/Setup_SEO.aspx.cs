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

namespace Song.Site.Manage.Admin
{
    public partial class Setup_SEO : Extend.CustomPage
    {
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                //网站关键字、简介
                Org_Keywords.Text = org.Org_Keywords;
                Org_Description.Text = org.Org_Description;
                //一些附加码，例如流量统计
                Org_Extracode.Text = org.Org_Extracode;
            }
        }

        protected void btnSeo_Click(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) throw new WeiSha.Common.ExceptionForAlert("当前机构不存在");
            org.Org_Keywords = Org_Keywords.Text.Trim();
            org.Org_Description = Org_Description.Text.Trim();
            org.Org_Extracode = Org_Extracode.Text.Trim();
            try
            {

                Business.Do<IOrganization>().OrganSave(org);

                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
    }
}
