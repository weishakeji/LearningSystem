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
    public partial class Organization_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //导航分类
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        //所归属的站点类型
        private string site = WeiSha.Common.Request.QueryString["site"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                init();
                fill();
            }
        }
        private void init()
        {
            Song.Entities.OrganLevel[] orglv = Business.Do<IOrganization>().LevelAll(true);
            ddlLevel.DataSource = orglv;
            ddlLevel.DataTextField = "olv_name";
            ddlLevel.DataValueField = "olv_id";
            ddlLevel.DataBind();
            //根域名
            lbDomain.Text = WeiSha.Common.Server.MainName;
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            Song.Entities.Organization org;
            if (id != 0)
            {
                org = Business.Do<IOrganization>().OrganSingle(id);
                cbIsUse.Checked = org.Org_IsUse;
            }
            else 
            {
                org = new Song.Entities.Organization();
            }
            //平台名称
            Org_PlatformName.Text = org.Org_PlatformName;
            //中文名称
            tbName.Text = org.Org_Name;
            tbAbbrName.Text = org.Org_AbbrName;
            //英文名称
            tbEnName.Text = org.Org_EnName;
            tbAbbrEnName.Text = org.Org_AbbrEnName;
            //上级导航
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
            //企业微信
            tbWeixin.Text = org.Org_Weixin;
            //所在机构等级
            ListItem liLv = ddlLevel.Items.FindByValue(org.Olv_ID.ToString());
            if (liLv != null)
            { liLv.Selected = true; }
            else
            {
                Song.Entities.OrganLevel lv = Business.Do<IOrganization>().LevelDefault();
                if (lv != null)
                {
                    liLv = ddlLevel.Items.FindByValue(lv.Olv_ID.ToString());
                    if (liLv != null) liLv.Selected = true;
                }
            }
            //域名
            tbDomain.Text = org.Org_TwoDomain;
            tbTemplate.Text = org.Org_Template;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Organization org;
            try
            {
                if (id!=0)
                {
                    org = Business.Do<IOrganization>().OrganSingle(id);
                }
                else org = new Song.Entities.Organization();
                //平台名称
                org.Org_PlatformName = Org_PlatformName.Text.Trim();
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
                //企业微信
                org.Org_Weixin = tbWeixin.Text.Trim();
                //所在等级
                org.Olv_ID = Convert.ToInt32(ddlLevel.SelectedValue);
                org.Olv_Name = ddlLevel.SelectedItem.Text;
                //是否启用
                org.Org_IsUse = cbIsUse.Checked;
                //域名
                org.Org_TwoDomain = tbDomain.Text.Trim();
                org.Org_Template = tbTemplate.Text.Trim();
                if (id != 0)
                {
                    Business.Do<IOrganization>().OrganSave(org);
                }
                else
                {
                    Business.Do<IOrganization>().OrganAdd(org);
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
