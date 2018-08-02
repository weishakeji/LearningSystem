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
    public partial class Setup_Qrcode : Extend.CustomPage
    {
        private string _uppath = "Org";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {             
                fill();               
            }
        }

        private void fill()
        {
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
        protected void btn_Click(object sender, EventArgs e)
        {
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
      
        
    }
}
