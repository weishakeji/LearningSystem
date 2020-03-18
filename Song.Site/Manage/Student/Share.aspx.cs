using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Manage.Student
{
    public partial class Share : Extend.CustomPage
    {
        //分享链接
        protected string shareurl = string.Empty;
        //分享二维码
        protected string shareimg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts acc = this.Master.Account;
            if (acc == null) return;
            //分享链接
            string url = "http://{0}{1}/default.ashx?sharekeyid={2}";
            //端口
            string port = WeiSha.Common.Server.Port;
            port = port == "80" ? "" : ":" + port;
            shareurl = string.Format(url, Request.Url.Host, port, acc.Ac_ID);

            //二维码图片对象
            System.Drawing.Image image = null;
            string centerImg = Upload.Get["Accounts"].Physics + acc.Ac_Photo;
            if (System.IO.File.Exists(centerImg))
                image = WeiSha.Common.QrcodeHepler.Encode(shareurl, 200, centerImg, "#000", "#fff");
            else
                image = WeiSha.Common.QrcodeHepler.Encode(shareurl, 200, "#000", "#fff");
            //将image转为base64
            shareimg = WeiSha.Common.Images.ImageTo.ToBase64(image);
        } 
    }
}