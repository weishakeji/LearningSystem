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
                fillLogo();
                fillLogin();
                fillSEO();
            }
        }


        //protected void btnPara_Click(object sender, EventArgs e)
        //{
        //    Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
        //    if (org == null) throw new WeiSha.Common.ExceptionForAlert("当前机构不存在");
        //    org.Org_PlatformName = Org_PlatformName.Text.Trim();
        //    org.Org_TwoDomain = Org_TwoDomain.Text.Trim();
        //    //
        //    WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
        //    //是否允许教师、学员注册
        //    config["IsRegTeacher"].Text = cbIsRegTeacher.Checked.ToString();
        //    config["IsRegStudent"].Text = cbIsRegStudent.Checked.ToString();
        //    //教师与学员注册后是否需要审核
        //    config["IsVerifyTeahcer"].Text = cbIsVerifyTeahcer.Checked.ToString();
        //    config["IsVerifyStudent"].Text = cbIsVerifyStudent.Checked.ToString();
        //    //登录方式
        //    config["IsLoginForPw"].Text = cbIsLoginForPw.Checked.ToString();    //启用账号密码登录
        //    config["IsLoginForSms"].Text = cbIsLoginForSms.Checked.ToString();  //启用手机短信验证登录
        //    //在线练习，是否学员登录后才能用
        //    config["IsTraningLogin"].Text = cbIsTraningLogin.Checked.ToString();
        //    //保存
        //    org.Org_Config = config.XmlString;
        //    try
        //    {

        //        Business.Do<IOrganization>().OrganSave(org);

        //        this.Alert("操作成功！");
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Alert(ex.Message);
        //    }
        //}
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
            lbDomain.Text = WeiSha.Common.Request.Domain.MainName;
            //ICP备案
            Org_ICP.Text = org.Org_ICP;
            //二维码，颜色、是否显示中心Logo
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //二维码图片的前景色
            tbQrColor.Text = config["QrColor"].Value.String;
            //是否启用中心图片
            cbQrCodeImg.Checked = config["IsQrConterImage"].Value.Boolean ?? false;
            //中心图片
            string centerImg = Upload.Get["Org"].Virtual + config["QrConterImage"].Value.String;
            imgQrCenter.Src = centerImg;
            //二维码信息
            Org_QrCodeUrl.Text = org.Org_QrCodeUrl;
        }
        protected void btnBase_Click(object sender, EventArgs e)
        {
            //平台名称，二维域名，ICP备案
            org.Org_PlatformName = Org_PlatformName.Text.Trim();
            org.Org_TwoDomain = Org_TwoDomain.Text.Trim();
            org.Org_ICP = Org_ICP.Text.Trim();
            org.Org_QrCodeUrl = Org_QrCodeUrl.Text.Trim();
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //二维码图片的前景色
            config["QrColor"].Text = tbQrColor.Text;
            //是否启用中心图片
            config["IsQrConterImage"].Text = cbQrCodeImg.Checked.ToString();
            //保存上传文件
            if (fuQrCenter.PostedFile.FileName != "")
            {
                try
                {
                    fuQrCenter.UpPath = "Org";
                    fuQrCenter.IsMakeSmall = false;
                    fuQrCenter.NewName = org.Org_ID + "_QrCodeLogo";
                    //删除原有
                    //中心图片
                    string centerImg = Upload.Get["Org"].Virtual + config["QrConterImage"].Value.String;
                    centerImg = WeiSha.Common.Server.MapPath(centerImg);
                    if (System.IO.File.Exists(centerImg))
                    {
                        System.IO.File.Delete(centerImg);
                    }
                    fuQrCenter.SaveAs();
                    imgQrCenter.Src = fuQrCenter.File.Server.VirtualPath;
                    config["QrConterImage"].Text = fuQrCenter.File.Server.FileName;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            org.Org_Config = config.XmlString;
            try
            {
                Business.Do<IOrganization>().OrganBuildQrCode(org);
                Business.Do<IOrganization>().OrganSave(org);                
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        #endregion

        #region Logo图标
        private void fillLogo()
        {
            //机构的Logo
            if (!string.IsNullOrEmpty(org.Org_Logo) && org.Org_Logo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + org.Org_Logo;
            }
        }
        protected void btnLogo_Click(object sender, EventArgs e)
        {
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
                Business.Do<IOrganization>().OrganSave(org);
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        #endregion

        #region 学习相关（注册登录等）
        /// <summary>
        /// 注册登录的相关初始值
        /// </summary>
        private void fillLogin()
        {
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //是否允许教师、学员注册
            cbIsRegTeacher.Checked = config["IsRegTeacher"].Value.Boolean ?? true;
            cbIsRegStudent.Checked = config["IsRegStudent"].Value.Boolean ?? true;
            cbIsRegSms.Checked = config["IsRegSms"].Value.Boolean ?? true;
            //教师与学员注册后是否需要审核
            cbIsVerifyTeahcer.Checked = config["IsVerifyTeahcer"].Value.Boolean ?? true;
            cbIsVerifyStudent.Checked = config["IsVerifyStudent"].Value.Boolean ?? true;
            //登录方式
            cbIsLoginForPw.Checked = config["IsLoginForPw"].Value.Boolean ?? true;    //启用账号密码登录
            cbIsLoginForSms.Checked = config["IsLoginForSms"].Value.Boolean ?? true;  //启用手机短信验证登录
            //在线练习，是否学员登录后才能用
            cbIsTraningLogin.Checked = config["IsTraningLogin"].Value.Boolean ?? false;
            //视频质量
            int qscale = config["VideoConvertQscale"].Value.Int32 ?? 4;
            ListItem qsli = ddlQscale.Items.FindByText(qscale.ToString());
            if (qsli != null)
            {
                ddlQscale.SelectedIndex = -1;
                qsli.Selected = true;
            }
            //开始视频防下载
            cbIsVideoNoload.Checked = config["IsVideoNoload"].Value.Boolean ?? false;
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //是否允许教师、学员注册
            config["IsRegTeacher"].Text = cbIsRegTeacher.Checked.ToString();
            config["IsRegStudent"].Text = cbIsRegStudent.Checked.ToString();
            config["IsRegSms"].Text = cbIsRegSms.Checked.ToString();
            //教师与学员注册后是否需要审核
            config["IsVerifyTeahcer"].Text = cbIsVerifyTeahcer.Checked.ToString();
            config["IsVerifyStudent"].Text = cbIsVerifyStudent.Checked.ToString();
            //登录方式
            config["IsLoginForPw"].Text = cbIsLoginForPw.Checked.ToString();    //启用账号密码登录
            config["IsLoginForSms"].Text = cbIsLoginForSms.Checked.ToString();  //启用手机短信验证登录
            //在线练习，是否学员登录后才能用
            config["IsTraningLogin"].Text = cbIsTraningLogin.Checked.ToString();
            //视频质量
            config["VideoConvertQscale"].Text = ddlQscale.SelectedItem.Text;
            //开始视频防下载
            config["IsVideoNoload"].Text = cbIsVideoNoload.Checked.ToString();
            //保存
            org.Org_Config = config.XmlString;
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
        #endregion

        #region SEO相关
        private void fillSEO()
        {
            //网站关键字、简介
            Org_Keywords.Text = org.Org_Keywords;
            Org_Description.Text = org.Org_Description;
            //一些附加码，例如流量统计
            Org_Extracode.Text = org.Org_Extracode;
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
        #endregion

        
    }
}
