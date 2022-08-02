using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Text;
using WeiSha.Core;
using Song.ServiceInterfaces;
using Song.Entities;
using Newtonsoft.Json;
using WxPayAPI;

namespace WxPayAPI
{
    /// <summary>
    /// 扫码支付
    /// </summary>
    public partial class NativePayPage : System.Web.UI.Page
    {       
        //支付接口ID
        int pi = WeiSha.Core.Request.QueryString["pi"].Int32 ?? 0;
        //流水号，即商户订单号
        string serial = WeiSha.Core.Request.QueryString["serial"].String;
        protected int total_fee = 0;  //充值的钱数
        int orgid = 0;  //机构id
        string mchid = string.Empty;   //商户ID（微信支付中心的商户ID)
        string paykey = string.Empty;   //商户支付密钥
        string appid = string.Empty;    //公众号appid
        string secret = string.Empty;   //公众号secret
        string notify_url = string.Empty;   //回调地址
        //支付接口id
        Song.Entities.PayInterface payInterface = null;
        Song.Entities.MoneyAccount moneyAccount = null;
        //当前账号
        protected Song.Entities.Accounts acc = null;
        protected string accphoto = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //初始化数据
                initData();

                if (!System.IO.File.Exists(WeiSha.Core.Upload.Get["Accounts"].Physics + acc.Ac_Photo))
                {
                    accphoto = WeiSha.Core.Upload.Get["Accounts"].Virtual + acc.Ac_Photo;
                }
                Song.Entities.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(serial);
                if (maccount != null && maccount.Ma_IsSuccess) return;
                //开始生成二维码
                NativePay nativePay = new NativePay();
                //****生成扫码支付模式二url
                //付款方信息
                string buyer = string.Empty;
                if (acc != null) buyer = string.IsNullOrWhiteSpace(acc.Ac_MobiTel1) ? acc.Ac_AccName : acc.Ac_MobiTel1;
                //平台名称,会显示在支付界面（微信扫码后，在手机中显示）
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                string url2 = nativePay.GetPayUrl(total_fee.ToString(), org.Org_PlatformName, serial, total_fee, appid, mchid, paykey, notify_url, buyer);
                //将url生成二维码图片
                //Image1.ImageUrl = "MakeQRCode.aspx?data=" + HttpUtility.UrlEncode(url1);
                Image2.ImageUrl = url2;
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                //监听充值是否完成，通过商户订号判断
                Song.Entities.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(serial);
                int state = 0;
                Log.Info(this.GetType().ToString(), "商户流水号 : " + serial);
                if (maccount != null)
                {
                    Log.Info(this.GetType().ToString(), "支付状态 : " + maccount.Ma_IsSuccess);
                    if (maccount.Ma_IsSuccess) state = 1;
                }
                Response.Write(state);
                Response.End();
            }
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void initData()
        {
            this.payInterface = Business.Do<IPayInterface>().PaySingle(pi);
            this.moneyAccount = Business.Do<IAccounts>().MoneySingle(serial);
            this.acc = Business.Do<IAccounts>().AccountsSingle(this.moneyAccount.Ac_ID);
            total_fee = (int)(moneyAccount.Ma_Money * 100);
            orgid = moneyAccount.Org_ID;
            appid = payInterface.Pai_ParterID;  //绑定支付的APPID（必须配置）
            secret = payInterface.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
            WeiSha.Core.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
            mchid = config["MCHID"].Value.String;    //商户id
            paykey = config["Paykey"].Value.String;  //支付密钥
            //回调地址
            notify_url = this.payInterface.Pai_Returl;
            if (string.IsNullOrWhiteSpace(notify_url)) notify_url = "http://" + WeiSha.Core.Server.Domain + "/";
            if (!notify_url.EndsWith("/")) notify_url += "/";
            notify_url += "Pay/Weixin/NativeNotifyPage.aspx";

        }
    }
}