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

namespace Song.Site.Manage.Sys
{
    public partial class OrganizationInfo : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                 fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {

            Song.Entities.Organization org;
            if (Extend.LoginState.Admin.IsSuperAdmin)
                org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
            else
                org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) return;
            //中文名称
            tbName.Text = org.Org_Name;
            tbAbbrName.Text = org.Org_AbbrName;
            //英文名称
            tbEnName.Text = org.Org_EnName;
            tbAbbrEnName.Text = org.Org_AbbrEnName;
            //地址,地理信息（经纬度）
            tbAddress.Text = org.Org_Address;
            tbLng.Text = org.Org_Longitude;
            tbLat.Text = org.Org_Latitude;
            //电话
            tbPhone.Text = org.Org_Phone;
            //传真
            tbFax.Text = org.Org_Fax;
            //邮编
            tbZip.Text = org.Org_Zip;
            //电子信息
            tbMail.Text = org.Org_Email;
            //联系人与联系人电话
            tbLinkman.Text = org.Org_Linkman;
            tbLinkmanPhone.Text = org.Org_LinkmanPhone;
            tbLinkmanQQ.Text = org.Org_LinkmanQQ;
            //企业微信
            tbWeixin.Text = org.Org_Weixin;

        }

        protected void BtnEnter_Click(object sender, EventArgs e)
        {

            Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
            if (org == null) return;

            //中文名称
            org.Org_Name = tbName.Text.Trim();
            org.Org_AbbrName = tbAbbrName.Text.Trim();
            //英文名称
            org.Org_EnName = tbEnName.Text.Trim();
            org.Org_AbbrEnName = tbAbbrEnName.Text.Trim();
            //地址
            org.Org_Address = tbAddress.Text.Trim();
            org.Org_Longitude = tbLng.Text.Trim();
            org.Org_Latitude = tbLat.Text.Trim();
            //电话
            org.Org_Phone = tbPhone.Text.Trim();
            //传真
            org.Org_Fax = tbFax.Text.Trim();
            //邮编
            org.Org_Zip = tbZip.Text.Trim();
            //电子信息
            org.Org_Email = tbMail.Text.Trim();
            //联系人与联系人电话
            org.Org_Linkman = tbLinkman.Text.Trim();
            org.Org_LinkmanPhone = tbLinkmanPhone.Text.Trim();
            org.Org_LinkmanQQ = tbLinkmanQQ.Text.Trim();
            //企业微信
            org.Org_Weixin = tbWeixin.Text.Trim();
            //
            //保存
            Business.Do<IOrganization>().OrganSave(org);

        }
    }
}
