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
    public partial class PublicPay : System.Web.UI.Page
    {
        //微信授权code，通过它获取access_token和openid
        private string code = WeiSha.Common.Request.QueryString["code"].String;
        //自己传递的参数被回调回来了，面值、流水号、机构id(例如total_fee:{0},serial:{1},orgid:{2})
        private string state = WeiSha.Common.Request.QueryString["state"].String;
        int total_fee = 0;  //充值的钱数
        string serial = string.Empty;   //流水号，即商户订单号
        int orgid = 0;  //机构id
        string mchid = string.Empty;   //商户ID（微信支付中心的商户ID)
        string paykey = string.Empty;   //商户支付密钥
        string appid = string.Empty;    //公众号appid
        string secret = string.Empty;   //公众号secret
        string notify_url = string.Empty;   //回调地址
        string token = string.Empty;    
        string openid = string.Empty;        
        //支付接口id
        Song.Entities.PayInterface payInterface = null;
        Song.Entities.MoneyAccount moneyAccount = null;
        //当前账号
        protected Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
        protected string path = WeiSha.Common.Upload.Get["Accounts"].Virtual;
        //H5调起JS API参数
        public static string wxJsApiParam { get; set; } 
        protected void Page_Load(object sender, EventArgs e)
        {
            //初始化数据，主要是解析state
            if (!string.IsNullOrWhiteSpace(state)) initData(state);
            //获取token和openid
            if (!string.IsNullOrWhiteSpace(code)) GetOpenidAndAccessTokenFromCode(code);
            //当前登录账号
            if (acc == null) Response.Redirect("/mobile/login.ashx");
            //支付
            JsApiPayPage();

        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void initData(string state)
        {
            int pi = 0;
            foreach (string s in state.Split(','))
            {
                string[] arr = s.Split(':');
                if (arr[0] == "serial") serial = arr[1];
                if (arr[0] == "pi") int.TryParse(arr[1], out pi);
            }
            this.payInterface = Business.Do<IPayInterface>().PaySingle(pi);
            this.moneyAccount = Business.Do<IAccounts>().MoneySingle(serial);
            total_fee = (int)(moneyAccount.Ma_Money * 100);
            orgid = moneyAccount.Org_ID;
            appid = payInterface.Pai_ParterID;  //绑定支付的APPID（必须配置）
            secret = payInterface.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
            WeiSha.Common.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
            mchid = config["MCHID"].Value.String;    //商户id
            paykey = config["Paykey"].Value.String;  //支付密钥
            //回调地址
            notify_url = this.payInterface.Pai_Returl;
            if (string.IsNullOrWhiteSpace(notify_url)) notify_url = "http://" + WeiSha.Common.Server.Domain + "/";
            if (!notify_url.EndsWith("/")) notify_url += "/";
            notify_url += "Pay/Weixin/ResultNotifyPage.aspx";            
            
        }
        /// <summary>
        /// 获取token和openid
        /// </summary>
        /// <param name="code"></param>
        public void GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                //构造获取openid及access_token的url
                WxPayAPI.WxPayData data = new WxPayAPI.WxPayData();
                data.SetValue("appid", appid);
                data.SetValue("secret", secret);
                data.SetValue("code", code);
                data.SetValue("grant_type", "authorization_code");
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?" + data.ToUrl();
                //请求url以获取数据
                string result = WxPayAPI.HttpService.Get(url);
                //Log.Debug(this.GetType().ToString(), "GetOpenidAndAccessTokenFromCode response : " + result);
                //保存access_token，用于收货地址获取
                LitJson.JsonData jd = JsonMapper.ToObject(result);
                token = (string)jd["access_token"];
                //获取用户openid
                openid = (string)jd["openid"];                
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), ex.ToString());
                throw new WxPayAPI.WxPayException(ex.ToString());
            }
        }
        /// <summary>
        /// 生成js调用相关数据
        /// </summary>
        public void JsApiPayPage()
        {        
            if (!IsPostBack)
            {
                //检测是否给当前页面传递了相关参数
                if (string.IsNullOrEmpty(openid) || total_fee<=0)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                    WxPayAPI.Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");                   
                    return;
                }
                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay(this);
                jsApiPay.openid = openid;
                jsApiPay.total_fee = total_fee;

                //JSAPI支付预处理
                try
                {
                    //付款方信息
                    string buyer=string.Empty;
                    if (acc != null) buyer = string.IsNullOrWhiteSpace(acc.Ac_MobiTel1) ? acc.Ac_AccName : acc.Ac_MobiTel1;
                   
                    //统一下单
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(serial, appid, mchid, paykey, notify_url,buyer);
                    //获取H5调起JS API参数  
                    wxJsApiParam = jsApiPay.GetJsApiParameters(paykey);// 用于前端js调用
                    WxPayAPI.Log.Debug(this.GetType().ToString(), "wxJsApiParam : " + wxJsApiParam);
                    //在页面上显示订单信息
                    //Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                    //Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");

                }
                catch (Exception ex)
                {
                    Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试。请情：" + ex.Message + "</span>");                   
                }
            }
        }
    }
}