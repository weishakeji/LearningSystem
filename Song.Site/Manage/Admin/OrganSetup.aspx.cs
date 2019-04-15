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
    public partial class OrganSetup : Extend.CustomPage
    {
        private string _uppath = "Org";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                fillBase();
            }
        }

        #region 基础参数
        /// <summary>
        /// 基础信息的绑定
        /// </summary>
        private void fillBase()
        {
            //平台名称，二级域名
            Org_PlatformName.Text =  org.Org_PlatformName;
            Org_TwoDomain.Text = org.Org_TwoDomain;
            //根域名
            lbDomain.Text = WeiSha.Common.Server.MainName;
            //ICP备案
            Org_ICP.Text = org.Org_ICP;
            Org_GonganBeian.Text = org.Org_GonganBeian;
            //机构的Logo
            if (!string.IsNullOrEmpty(org.Org_Logo) && org.Org_Logo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + org.Org_Logo;
            }           
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //手机端
            this.cbDisenableWeixin.Checked = config["DisenableWeixin"].Value.Boolean ?? false;  //禁止在微信中使用
            this.cbDisenableMini.Checked = config["DisenableMini"].Value.Boolean ?? false;    //禁止在微信小程序中使用
            this.cbDisenableMweb.Checked = config["DisenableMweb"].Value.Boolean ?? false;    //禁止在手机网页中使用
            this.cbDisenableAPP.Checked = config["DisenableAPP"].Value.Boolean ?? false;     //禁止在手机APP中使用
            //手机端隐藏关于“充值收费”等资费相关信息
            this.cbIsMobileRemoveMoney.Checked = config["IsMobileRemoveMoney"].Value.Boolean ?? false; 
            //桌面端
            this.cbWebForDeskapp.Checked = config["WebForDeskapp"].Value.Boolean ?? false;  //当前系统必须运行于桌面应用之中
            this.cbStudyForDeskapp.Checked = config["StudyForDeskapp"].Value.Boolean ?? false;  //课程学习需要在桌面应用打开
            this.cbFreeForDeskapp.Checked = config["FreeForDeskapp"].Value.Boolean ?? false;    //免费课程和试用章节除外
            
        }
        protected void btnBase_Click(object sender, EventArgs e)
        {
            //平台名称，二维域名，ICP备案
            org.Org_PlatformName = Org_PlatformName.Text.Trim();
            org.Org_TwoDomain = Org_TwoDomain.Text.Trim();
            org.Org_ICP = Org_ICP.Text.Trim();
            org.Org_GonganBeian = Org_GonganBeian.Text.Trim();            
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //手机端
            config["DisenableWeixin"].Text = this.cbDisenableWeixin.Checked.ToString();      //禁止在微信中使用
            config["DisenableMini"].Text = this.cbDisenableMini.Checked.ToString();    //禁止在微信小程序中使用
            config["DisenableMweb"].Text = this.cbDisenableMweb.Checked.ToString();    //禁止在手机网页中使用
            config["DisenableAPP"].Text = this.cbDisenableAPP.Checked.ToString();     //禁止在手机APP中使用
            config["IsMobileRemoveMoney"].Text = this.cbIsMobileRemoveMoney.Checked.ToString();
            //桌面端
            config["WebForDeskapp"].Text = this.cbWebForDeskapp.Checked.ToString();  //当前系统必须运行于桌面应用之中
            config["StudyForDeskapp"].Text = this.cbStudyForDeskapp.Checked.ToString();     //课程学习需要在桌面应用打开
            config["FreeForDeskapp"].Text = this.cbFreeForDeskapp.Checked.ToString();    //免费课程和试用章节除外
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = false;
                    fuLoad.SaveAndDeleteOld(org.Org_Logo);
                    //fuLoad.File.Server.ChangeSize(150, 200, false);
                    org.Org_Logo = fuLoad.File.Server.FileName;
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

                org.Org_Config = config.XmlString;
                Business.Do<IOrganization>().OrganSave(org);                
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        #endregion

        
    }
}
