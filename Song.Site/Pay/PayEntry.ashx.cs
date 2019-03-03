using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using Com.Alipayweb;

namespace Song.Site.Pay
{
    /// <summary>
    /// 这里是支付入口，通过这里转向具体的支付接口
    /// </summary>
    public class PayEntry : IHttpHandler
    {
        //钱数
        private Decimal money = (Decimal)(WeiSha.Common.Request.Form["money"].Double ?? 0);
        //验证码
        string vpaycode = WeiSha.Common.Request.Form["vpaycode"].String; 
        //支付接口的id
        private int paiid = WeiSha.Common.Request.Form["paiid"].Int32 ?? 0;
        //是否校验验证码
        private bool isVerify = WeiSha.Common.Request.QueryString["isVerify"].Boolean ?? true;
        protected HttpResponse Response { get; private set; }
        public void ProcessRequest(HttpContext context)
        {
            this.Response = context.Response;
            //来源
            string from = "Recharge.ashx";
            if (context.Request.UrlReferrer != null) from = context.Request.UrlReferrer.ToString();            
            if (!Extend.LoginState.Accounts.IsLogin)
            {
                context.Response.Redirect(WeiSha.Common.Request.Page.AddPara(from, "err=1", "money=" + money, "paiid=" + paiid));
                return;
            }
            ////验证码不正确
            //if (isVerify && !isCodeImg())
            //{
            //    this.Response.Redirect(addPara(from, "err=3"));
            //    return;
            //}
            //支付接口的设置项
            Song.Entities.PayInterface pi = Business.Do<IPayInterface>().PaySingle(paiid);
            if (pi == null)
            {
                //设置项不存在
                context.Response.Redirect(WeiSha.Common.Request.Page.AddPara(from, "err=2", "money=" + money, "paiid=" + paiid));
                return;
            }
            else
            {
                //产生支付流水号
                MoneyAccount ma = new MoneyAccount();
                ma.Ma_Money = money;
                ma.Ac_ID = Extend.LoginState.Accounts.CurrentUser.Ac_ID;
                ma.Ma_Source = pi.Pai_Pattern;
                ma.Pai_ID = pi.Pai_ID;
                ma.Ma_From = 3;
                ma.Ma_IsSuccess = false;
                ma = Business.Do<IAccounts>().MoneyIncome(ma);
                //调用指定的支付接口
                if (pi.Pai_Pattern == "支付宝手机支付") Alipaywap(pi, ma);
                if (pi.Pai_Pattern == "支付宝网页直付") Alipayweb(pi, ma);
                if (pi.Pai_Pattern == "微信公众号支付") Weixinpubpay(pi, ma);
                if (pi.Pai_Pattern == "微信扫码支付") WeixinNativepay(pi, ma);
                if (pi.Pai_Pattern == "微信小程序支付") WeixinMiniProgramPay(pi, ma);
                if (pi.Pai_Pattern == "微信Html5支付") WeixinH5Pay(pi, ma);
            }
        }
        /// <summary>
        /// 支付宝手机支付
        /// </summary>
        private void Alipaywap(Song.Entities.PayInterface pi, Song.Entities.MoneyAccount ma)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////
            //回调域
            string domain = "http://" + WeiSha.Common.Server.Domain + ":" + WeiSha.Common.Server.Port + "/";
            domain = string.IsNullOrWhiteSpace(pi.Pai_Returl) ? domain : pi.Pai_Returl;
            if (!domain.EndsWith("/")) domain += "/";
            //支付类型
            string payment_type = "1";
            //商户订单号
            string out_trade_no = ma.Ma_Serial;
            //商户网站订单系统中唯一订单号，必填
            //必填，不能修改
            //服务器异步通知页面路径
            string notify_url = domain + "Pay/Alipaywap/notify_url.aspx";
            //需http://格式的完整路径，不能加?id=123这类自定义参数
            //页面跳转同步通知页面路径
            string return_url = domain + "Pay/Alipaywap/return_url.aspx";
            //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/
            //订单名称
            string name = string.IsNullOrWhiteSpace(Extend.LoginState.Accounts.CurrentUser.Ac_Name) ? "" : Extend.LoginState.Accounts.CurrentUser.Ac_Name;
            string subject = name + "(" + Extend.LoginState.Accounts.CurrentUser.Ac_AccName + ")充值";
            //必填
            //付款金额
            string total_fee = money.ToString();
            //必填
            //商品展示地址
            string show_url = domain + "Mobile/Recharge.ashx";
            //必填，需以http://开头的完整路径，例如：http://www.商户网址.com/myorder.html
            //订单描述
            string body = subject;
            //选填
            //超时时间
            string it_b_pay = "10";
            //选填
            //钱包token
            string extern_token = "";
            ////////////////////////////////////////////////////////////////////////////////////////////////
            //把请求参数打包成数组
            Com.Alipaywap.Config config = new Com.Alipaywap.Config(pi);
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", config.Partner);
            sParaTemp.Add("seller_id", config.Seller_id);
            sParaTemp.Add("_input_charset", config.Input_charset.ToLower());
            sParaTemp.Add("service", "alipay.wap.create.direct.pay.by.user");
            sParaTemp.Add("payment_type", payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);  //不要用Url编码处理中文
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("body", body);    //不要用Url编码处理中文
            sParaTemp.Add("it_b_pay", it_b_pay);
            sParaTemp.Add("extern_token", extern_token);
            //建立请求
            Com.Alipaywap.Submit submit = new Com.Alipaywap.Submit(config);
            string sHtmlText = submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
        }
        /// <summary>
        /// 支付宝网页直付
        /// </summary>
        /// <param name="pi"></param>
        private void Alipayweb(Song.Entities.PayInterface pi, Song.Entities.MoneyAccount ma)
        {
            ////////////////////////////////////////////请求参数////////////////////////////////////////////

            //商户订单号，商户网站订单系统中唯一订单号，必填
            string out_trade_no = ma.Ma_Serial;
            //订单名称，必填
            string name = string.IsNullOrWhiteSpace(Extend.LoginState.Accounts.CurrentUser.Ac_Name) ? "" : Extend.LoginState.Accounts.CurrentUser.Ac_Name;
            string subject = name + "(" + Extend.LoginState.Accounts.CurrentUser.Ac_AccName + ")充值";
            //付款金额，必填
            string total_fee = money.ToString();
            //商品描述，可空
            string body = subject;
            ////////////////////////////////////////////////////////////////////////////////////////////////
            //服务器异步通知页面路径
            string domain = "http://" + WeiSha.Common.Server.Domain + ":" + WeiSha.Common.Server.Port + "/";
            domain = string.IsNullOrWhiteSpace(pi.Pai_Returl) ? domain : pi.Pai_Returl;
            if (!domain.EndsWith("/")) domain += "/";
            string notify_url = domain + "Pay/Alipayweb/notify_url.aspx";
            //需http://格式的完整路径，不能加?id=123这类自定义参数
            //页面跳转同步通知页面路径
            string return_url = domain + "Pay/Alipayweb/return_url.aspx";
            
            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            Com.Alipayweb.Config config = new Com.Alipayweb.Config(pi);
            sParaTemp.Add("service", Config.service);
            sParaTemp.Add("partner", config.Partner);
            sParaTemp.Add("seller_id", config.Seller_id);
            sParaTemp.Add("_input_charset", config.Input_charset.ToLower());
            sParaTemp.Add("payment_type", Config.payment_type);
            sParaTemp.Add("notify_url", notify_url);
            sParaTemp.Add("return_url", return_url);
            sParaTemp.Add("anti_phishing_key", Config.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", Config.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);  //不要用Url编码处理中文
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);    //不要用Url编码处理中文
            //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
            //如sParaTemp.Add("参数名","参数值");

            //建立请求
            Submit submit = new Submit(config);
            string sHtmlText = submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
        }
        /// <summary>
        /// 微信公众号支付
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="ma"></param>
        private void Weixinpubpay(Song.Entities.PayInterface pi, Song.Entities.MoneyAccount ma)
        {
            string host = System.Web.HttpContext.Current.Request.Url.Host + ":" + WeiSha.Common.Server.Port + "/";
            if (!string.IsNullOrWhiteSpace(pi.Pai_Returl)) host = pi.Pai_Returl;
            if (!host.EndsWith("/")) host += "/";
            //构造网页授权获取code的URL          
            string path = "Pay/Weixin/PublicPay.aspx";
            //System.Web.HttpContext.Current.Response.Write(host + path);
            string redirect_uri = HttpUtility.UrlEncode(host + path.ToLower());
            WxPayAPI.WxPayData data = new WxPayAPI.WxPayData();
            data.SetValue("appid", pi.Pai_ParterID);
            data.SetValue("redirect_uri", redirect_uri);
            data.SetValue("response_type", "code");
            data.SetValue("scope", "snsapi_base");
            //返回的状态值，接口id、流水号
            string state = "pi:{0},serial:{1}";
            state = string.Format(state, pi.Pai_ID, ma.Ma_Serial);
            data.SetValue("state", state + "#wechat_redirect");
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?" + data.ToUrl();
            System.Web.HttpContext.Current.Response.Redirect(url);
        }
        /// <summary>
        /// 微信扫码支付
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="ma"></param>
        private void WeixinNativepay(Song.Entities.PayInterface pi, Song.Entities.MoneyAccount ma)
        {
            string host = System.Web.HttpContext.Current.Request.Url.Host + ":" + WeiSha.Common.Server.Port + "/";
            if (!string.IsNullOrWhiteSpace(pi.Pai_Returl)) host = pi.Pai_Returl;
            if (!host.EndsWith("/")) host += "/";
            //用于生成支付二维码的URL          
            string url = "Pay/Weixin/NativePayPage.aspx?pi={0}&serial={1}";
            url = host + string.Format(url.ToString(), pi.Pai_ID, ma.Ma_Serial);
            System.Web.HttpContext.Current.Response.Redirect(url);
        }
        /// <summary>
        /// 微信小程序支付
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="ma"></param>
        private void WeixinMiniProgramPay(Song.Entities.PayInterface pi, Song.Entities.MoneyAccount ma)
        {    
            string url = "Weixin/miniProgramPay.aspx?piid={0}&serial={1}&money={2}&org={3}";
            url = string.Format(url, pi.Pai_ID, ma.Ma_Serial, money * 100, ma.Org_ID);
            //调试
            string msg = string.Format("小程序支付，第一步（启动）：\r\n接口ID：{0}，流水号：{1}，金额：{2}元", pi.Pai_ID, ma.Ma_Serial, money);
            WxPayAPI.Log.Info(this.GetType().FullName, msg);

            System.Web.HttpContext.Current.Response.Redirect(url);
        }
        /// <summary>
        /// 微信html5支付，即手机网页支付
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="ma"></param>
        private void WeixinH5Pay(Song.Entities.PayInterface pi, Song.Entities.MoneyAccount ma)
        {
            string url = "Weixin/Html5Pay.aspx?piid={0}&serial={1}&money={2}";
            url = string.Format(url, pi.Pai_ID, ma.Ma_Serial, money*100);
            System.Web.HttpContext.Current.Response.Redirect(url);
        }
        #region 其它
        /// <summary>
        /// 验证图片验证是否正确
        /// </summary>
        /// <returns></returns>
        private bool isCodeImg()
        {
            //取图片验证码
            string imgCode = WeiSha.Common.Request.Cookies["vpaycode"].ParaValue;
            //取员工输入的验证码
            string userCode = new WeiSha.Common.Param.Method.ConvertToAnyValue(vpaycode).MD5;
            //验证
            return imgCode == userCode;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion
    }
}