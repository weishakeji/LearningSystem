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

namespace Song.Site.Manage.Utility
{
    public partial class QrBuilder : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            //资源的路径
            string resPath = Upload.Get["Org"].Virtual;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganDefault();
            org.Org_Logo = resPath + org.Org_Logo;
            org.Org_QrCode = resPath + org.Org_QrCode;
            imgQr.ImageUrl = org.Org_QrCode;
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuilerQr_Click(object sender, EventArgs e)
        {
            //内容
            string context = tbContent.Text.Trim();           
            //前景色与背景色
            string color = tbColor.Text.Trim();
            string bgcolor = tbBgcolor.Text.Trim();
            //宽高
            int wh = tbWh.Text.Trim() == "" ? 200 : Convert.ToInt32(tbWh.Text.Trim());
            //是否生成中心logo
            bool isCenterImg = cbIsLogo.Checked;
            //二维码的中心图标
            string centerImg = Upload.Get["Org"].Physics + "QrCodeLogo.png";
            //资源的路径
            string resPath = Upload.Get["Temp"].Physics + "QrCode\\";
            //如果文件夹不存在则创建，否则删除原有图片
            if (!System.IO.Directory.Exists(resPath))
            {
                System.IO.Directory.CreateDirectory(resPath);
            }
            else
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(resPath);
                foreach (System.IO.FileInfo fi in di.GetFiles("*.png"))
                {
                    if (fi.Name.IndexOf("_") > -1)
                    {
                        //string tm = fi.Name.Substring(0, fi.Name.IndexOf("_"));
                        if (fi.Name.Substring(0, fi.Name.IndexOf("_")) == "qrcode")
                        {
                            fi.Delete();
                        }
                    }
                }
            }
            string file = "qrcode_" + WeiSha.Common.Request.UniqueID() + ".png";
            //
            System.Drawing.Image image = null;
            try
            {
                if (isCenterImg)
                    image=WeiSha.Common.QrcodeHepler.Encode(context, wh, centerImg, color, bgcolor);
                else
                    image = WeiSha.Common.QrcodeHepler.Encode(context, wh, color, bgcolor);
                string imgpath = resPath + file;
                image.Save(imgpath);
                imgQr.ImageUrl = Upload.Get["Temp"].Virtual + "QrCode/" + file;
            }
            catch (Exception ex)
            {
                new Extend.Scripts(this.Page).Alert(ex.Message);
            }
        }
    }
}