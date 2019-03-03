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
    public partial class miniProgramOpenid : System.Web.UI.Page
    {
        //微信授权code，通过它获取access_token和openid
        private string code = WeiSha.Common.Request.Form["scode"].String;   
        string mchid = string.Empty;   //商户ID（微信支付中心的商户ID)
        string paykey = string.Empty;   //商户支付密钥
        string appid = string.Empty;    //公众号appid
        string secret = string.Empty;   //公众号secret
        string token = string.Empty;    
        string openid = string.Empty;        
        //支付接口id
        Song.Entities.PayInterface payInterface = null;
        //当前账号
        protected Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
        protected string path = WeiSha.Common.Upload.Get["Accounts"].Virtual;
        //H5调起JS API参数
        public static string wxJsApiParam { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            initData();

            //调试
            string msg = string.Format("小程序支付，第三步（获取Openid）：\r\n微信授权（来自小程序）Code：{0}", code);
            WxPayAPI.Log.Info(this.GetType().FullName, msg);

            if (!string.IsNullOrWhiteSpace(code)) GetOpenidAndAccessTokenFromCode(code);
            WxPayAPI.Log.Info(this, "小程序获取OpenID : " + openid);
            if (string.IsNullOrWhiteSpace(openid))
            {
                WxPayAPI.Log.Error(this, "小程序获取OpenID获取异常 : " + openid);
            }
            Response.Write(openid);
            Response.End();

        }
        /// <summary>
        /// 初始化接口
        /// </summary>
        private void initData()
        {
            Song.Entities.PayInterface[] pis = Business.Do<IPayInterface>().PayAll(-1, null, true);
            foreach (Song.Entities.PayInterface p in pis)
            {
                if (p.Pai_Pattern == "微信小程序支付")
                {
                    this.payInterface = p;
                    break;
                }
            }
            if (this.payInterface == null)
            {
                WxPayAPI.Log.Error(this, "小程序支付接口不存在！");
            }
            if (this.payInterface != null)
            {
                appid = payInterface.Pai_ParterID;  //绑定支付的APPID（必须配置）
                secret = payInterface.Pai_Key;      //公众帐号secert（仅JSAPI支付的时候需要配置）
                WeiSha.Common.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
                mchid = config["MCHID"].Value.String;    //商户id
                paykey = config["Paykey"].Value.String;  //支付密钥                
            }
        }
        /// <summary>
        /// 获取token和openid
        /// </summary>
        /// <param name="code"></param>
        public void GetOpenidAndAccessTokenFromCode(string code)
        {
            try
            {
                string url = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
                url = string.Format(url, appid, secret, code);
                //请求url以获取数据
                string result = WxPayAPI.HttpService.Get(url);
                WxPayAPI.Log.Debug(this, "GetOpenidAndAccessTokenFromCode response : " + result);
                //保存access_token，用于收货地址获取
                LitJson.JsonData jd = JsonMapper.ToObject(result);
                //token = (string)jd["access_token"];
                //获取用户openid
                openid = (string)jd["openid"];                
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this, ex.ToString());
                //throw new WxPayAPI.WxPayException(ex.ToString());
            }
        }

    }
}