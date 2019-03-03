using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using LitJson;
using WxPayAPI;

namespace Song.Site.Pay.Weixin
{
    public partial class miniProgramPay : System.Web.UI.Page
    {
        //支付接口id
        protected int piid = WeiSha.Common.Request.QueryString["piid"].Int32 ?? 0;
        //交易流水号,即商户订单号
        protected string serial = WeiSha.Common.Request.QueryString["serial"].String;
        protected int total_fee = WeiSha.Common.Request.QueryString["money"].Int32 ?? 0;  //充值的钱数
        protected int orgid = WeiSha.Common.Request.QueryString["org"].Int32 ?? 0;  //机构id
        protected string mchid = string.Empty;   //商户ID（微信支付中心的商户ID)
        protected string paykey = string.Empty;   //商户支付密钥
        protected string appid = string.Empty;    //小程序appid
        protected string secret = string.Empty;   //小程序secret
        //回调地址  
        protected string notify_url = string.Empty;  
        //支付接口id
        Song.Entities.PayInterface payInterface = null;
        protected void Page_Load(object sender, EventArgs e)
        {            
            initData();
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void initData()
        {
            this.payInterface = Business.Do<IPayInterface>().PaySingle(piid);
            if (this.payInterface == null) return;
            appid = payInterface.Pai_ParterID;  //绑定支付的APPID（必须配置）
            secret = payInterface.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
            WeiSha.Common.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
            mchid = config["MCHID"].Value.String;    //商户id
            paykey = config["Paykey"].Value.String;  //商户支付密钥  
            //回调地址
            this.payInterface = Business.Do<IPayInterface>().PaySingle(piid);
            string retdomain = this.payInterface.Pai_Returl;
            if (string.IsNullOrWhiteSpace(retdomain)) retdomain = "http://" + WeiSha.Common.Server.Domain + "/";
            if (!retdomain.EndsWith("/")) retdomain += "/";
            notify_url = retdomain + "Pay/Weixin/miniProgramResult.aspx";
            notify_url = notify_url.ToLower();

            //调试
            string msg = string.Format("小程序支付，第二步（调用小程序）：\r\n商户ID：{0}，商户密钥：{1}，商户订单号：{2},回调：{3}", mchid, paykey, serial, notify_url);
            WxPayAPI.Log.Info(this.GetType().FullName, msg);
        }
    }
}