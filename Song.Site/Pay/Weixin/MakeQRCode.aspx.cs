using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
//using ThoughtWorks;
//using ThoughtWorks.QRCode;
//using ThoughtWorks.QRCode.Codec;
//using ThoughtWorks.QRCode.Codec.Data;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;

namespace WxPayAPI
{
    public partial class MakeQRCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["data"]))
            {
                string str = Request.QueryString["data"];
                //
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                //各项配置           
                WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);   //自定义配置项           
                string color = config["QrColor"].Value.String;  //二维码前景色            
                bool isQrcenter = config["IsQrConterImage"].Value.Boolean ?? false; //是否启用中心图片           
                string centerImg = Upload.Get["Org"].Virtual + config["QrConterImage"].Value.String;     //中心图片
                centerImg = WeiSha.Common.Server.MapPath(centerImg);

                System.Drawing.Image image = WeiSha.Common.QrcodeHepler.Encode(str, 200, centerImg, "", "#fff");   


                ////初始化二维码生成工具
                //QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                //qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                //qrCodeEncoder.QRCodeVersion = 0;
                //qrCodeEncoder.QRCodeScale = 4;
                
                ////将字符串生成二维码图片
                //Bitmap image = qrCodeEncoder.Encode(str, Encoding.Default);

                //保存为PNG到内存流  
                MemoryStream ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);

                //输出二维码图片
                Response.BinaryWrite(ms.GetBuffer());
                Response.End();
            }
        }
    }
}