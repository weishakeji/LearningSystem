using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Core;
using Song.ServiceInterfaces;
using Song.Entities;
//using Newtonsoft.Json;
using WxPayAPI;

namespace Song.Site.Pay.Weixin
{
    public partial class Html5Pay : System.Web.UI.Page
    {
        //学员id
        private int acid = WeiSha.Core.Request.QueryString["acid"].Int32 ?? 0;
        //支付接口id
        protected int piid = 0;
        //交易流水号,即商户订单号
        protected string serial = string.Empty;
        //返回页的主域
        protected string retdomain = string.Empty;  
        //回调地址  
        protected string notify_url = string.Empty;  
        protected int total_fee = WeiSha.Core.Request.QueryString["money"].Int32 ?? 0;  //充值的钱数
        int orgid = 0;  //机构id
        string mchid = string.Empty;   //商户ID（微信支付中心的商户ID)
        protected string paykey = string.Empty;   //商户支付密钥
        protected string appid = string.Empty;    //公众号appid
        protected string secret = string.Empty;   //公众号secret
        //支付接口id
        Song.Entities.PayInterface payInterface = null;
        Song.Entities.MoneyAccount moneyAccount = null;
        //当前账号
        protected Song.Entities.Accounts acc = null;
        protected string path = WeiSha.Core.Upload.Get["Accounts"].Virtual;

        protected void Page_Load(object sender, EventArgs e)
        {
            acc = Business.Do<IAccounts>().AccountsSingle(this.acid);
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //当前登录账号
                if (acc == null) Response.Redirect("/mobile/login.ashx");
                piid = WeiSha.Core.Request.QueryString["piid"].Int32 ?? 0;
                serial = WeiSha.Core.Request.QueryString["serial"].String;
                //回调地址
                this.payInterface = Business.Do<IPayInterface>().PaySingle(piid);
                retdomain = this.payInterface.Pai_Returl;
                if (string.IsNullOrWhiteSpace(retdomain)) retdomain = "http://" + WeiSha.Core.Server.Domain + "/";
                if (!retdomain.EndsWith("/")) retdomain += "/";
                notify_url = retdomain + "Pay/Weixin/Html5PayResultNotify.aspx";
                notify_url = notify_url.ToLower();
            }
            //接收ajax请求
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                //微信授权code，通过它获取access_token和openid
                string code = WeiSha.Core.Request.Form["code"].String;
                piid = WeiSha.Core.Request.Form["piid"].Int32 ?? 0;
                serial = WeiSha.Core.Request.Form["serial"].String;
                notify_url = WeiSha.Core.Request.Form["returl"].String;
                initData();  
                //支付下单
                WxPayData result = JsApiPayPage();
                if (result != null) Response.Write(result.ToJson());
                if (result == null) Response.Write("0");
                Response.End();
            }
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void initData()
        {
            this.payInterface = Business.Do<IPayInterface>().PaySingle(piid);
            this.moneyAccount = Business.Do<IAccounts>().MoneySingle(serial);
            total_fee = (int)(moneyAccount.Ma_Money * 100);
            orgid = moneyAccount.Org_ID;
            appid = payInterface.Pai_ParterID;  //绑定支付的APPID（必须配置）
            secret = payInterface.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
            WeiSha.Core.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
            mchid = config["MCHID"].Value.String;    //商户id
            paykey = config["Paykey"].Value.String;  //支付密钥  
        }        
        /// <summary>
        /// 生成js调用相关数据
        /// </summary>
        public WxPayData JsApiPayPage()
        {
            //检测是否给当前页面传递了相关参数
            if (total_fee <= 0)
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                WxPayAPI.Log.Error(this.GetType().ToString(), "交易资金小于等于0");
                return null;
            }
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(this);
            //jsApiPay.openid = "";
            jsApiPay.total_fee = total_fee;
            //JSAPI支付预处理
            try
            {
                //付款方信息
                string buyer = string.Empty;
                if (acc != null) buyer = string.IsNullOrWhiteSpace(acc.Ac_MobiTel1) ? acc.Ac_AccName : acc.Ac_MobiTel1;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
                //统一下单
                return jsApiPay.GetUnifiedOrderResult("MWEB", org.Org_PlatformName, serial, appid, mchid, paykey, notify_url, buyer);                

            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), "支付下单失败 : " +ex.Message);
                WxPayAPI.Log.Error(this.GetType().ToString(), "支付下单失败 : " + ex.StackTrace);
                return null;
            }

        }
    }
}