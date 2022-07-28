using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Core;
using Song.ServiceInterfaces;
using Song.Entities;
using Newtonsoft.Json;
using WxPayAPI;

namespace Song.Site.Pay.Weixin
{
    public partial class miniProgramCall : System.Web.UI.Page
    {        
        //交易流水号,即商户订单号
        protected string serial = WeiSha.Core.Request.QueryString["serial"].String;
        protected int total_fee = WeiSha.Core.Request.QueryString["total_fee"].Int32 ?? 0;  //充值的钱数
        protected int orgid = WeiSha.Core.Request.QueryString["orgid"].Int32 ?? 0;  //机构id
        protected string mchid = WeiSha.Core.Request.QueryString["mchid"].String;   //商户ID（微信支付中心的商户ID)
        protected string paykey = WeiSha.Core.Request.QueryString["paykey"].String;  //商户支付密钥
        protected string appid = WeiSha.Core.Request.QueryString["appid"].String;    //小程序appid
        protected string secret = WeiSha.Core.Request.QueryString["secret"].String;   //小程序secret
        protected string openid = WeiSha.Core.Request.QueryString["openid"].String;   
        //回调地址  
        protected string notify_url = WeiSha.Core.Request.QueryString["notify_url"].String;   
        ////支付接口id
        //Song.Entities.PayInterface payInterface = null;
        protected void Page_Load(object sender, EventArgs e)
        {            
            Response.Write(JsApiPayPage());
            Response.End();
        }
        /// <summary>
        /// 生成js调用相关数据
        /// </summary>
        public string JsApiPayPage()
        {
            //检测是否给当前页面传递了相关参数
            if (total_fee <= 0)
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                WxPayAPI.Log.Error(this, "交易资金小于等于0");
                return null;
            }
            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(this);
            jsApiPay.openid = openid;
            jsApiPay.total_fee = total_fee;
            //JSAPI支付预处理
            try
            {
                //付款方信息
                string buyer = string.Empty;
                Song.Entities.Accounts acc = null;
                if (acc != null) buyer = string.IsNullOrWhiteSpace(acc.Ac_MobiTel1) ? acc.Ac_AccName : acc.Ac_MobiTel1;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
                WxPayAPI.Log.Debug(this, "回调域：" + notify_url);
                //统一下单                
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult("JSAPI", org.Org_PlatformName, serial, appid, mchid, paykey, notify_url, buyer);
                //获取H5调起JS API参数  
                string wxJsApiParam = jsApiPay.GetJsApiParameters(paykey);// 用于前端js调用
                WxPayAPI.Log.Debug(this, "获取H5调起JS API参数："+wxJsApiParam);
                return wxJsApiParam;

            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this, "支付下单失败 : " + ex.Message);
                WxPayAPI.Log.Error(this, "支付下单失败 : " + ex.StackTrace);
                return null;
            }

        }
    }
}