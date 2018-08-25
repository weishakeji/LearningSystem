using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Song.ServiceInterfaces;
using WeiSha.Common;
using System.IO;

namespace Song.Site.Manage.Money
{
    public partial class Output_Qrcode : Extend.CustomPage
    {
        //充值卡设置项的id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected Song.Entities.Organization org;       

        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.RechargeSet set = Business.Do<IRecharge>().RechargeSetSingle(id);
            this.EntityBind(set);
            if (set != null) this.Title += set.Rs_Theme; 
            
            //当前充值卡的编码
            Song.Entities.RechargeCode[] cards = Business.Do<IRecharge>().RechargeCodeCount(-1, set.Rs_ID, true, null, -1);
            if (cards != null)
            {
                //生成二维码的配置
                //各项配置           
                WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);   //自定义配置项  
                string centerImg = Upload.Get["Org"].Virtual + config["QrConterImage"].Value.String;     //中心图片
                centerImg = WeiSha.Common.Server.MapPath(centerImg);
                string color = config["QrColor"].Value.String;  //二维码前景色  
                //生成二维码的字符串
                string[] qrcodes = new string[cards.Length];
                string url = lbUrl.Text.Trim();
                string domain = this.Request.Url.Scheme + "://" + this.Request.Url.Host + ":" + this.Request.Url.Port;
                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i].Rc_IsUsed) continue;
                    qrcodes[i] = string.Format(url, domain, cards[i].Rc_Code, cards[i].Rc_Pw);
                }
                //批量生成二维码
                System.Drawing.Image[] images = WeiSha.Common.QrcodeHepler.Encode(qrcodes, 200, centerImg, color, "#fff");
                for (int i = 0; i < cards.Length; i++)
                {
                    if (images[i] == null)
                    {
                        cards[i].Rc_QrcodeBase64 = lbUsedImg.Text;
                        continue;
                    }
                    cards[i].Rc_QrcodeBase64 = "data:image/JPG;base64," + WeiSha.Common.Images.ImageTo.ToBase64(images[i]);
                }
                rtpCode.DataSource = cards;
                rtpCode.DataBind();
            }
           
        }
        /// <summary>
        /// 生成二维码的字符串（网址）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        protected string buildUrl(string code, string pw)
        {
            string url = lbUrl.Text.Trim();
            string domain = this.Request.Url.Scheme + "://" + this.Request.Url.Host + ":" + this.Request.Url.Port;
            url = string.Format(url, domain, code, pw);
            return url;
        }
    }
}