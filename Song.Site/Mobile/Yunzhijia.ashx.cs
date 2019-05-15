using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Configuration;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WeiSha.Common;
using Song.ServiceInterfaces;


namespace Song.Site.Mobile
{
    /// <summary>
    /// 云之家的访问
    /// </summary>
    public class Yunzhijia : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            bool isUse = Business.Do<ISystemPara>()["YunzhijiaLoginIsuse"].Boolean ?? true;
            string appid = Business.Do<ISystemPara>()["YunzhijiaAppid"].String;
            string secret = Business.Do<ISystemPara>()["YunzhijiaAppSecret"].String;
            string domain = Business.Do<ISystemPara>()["YunzhijiaDomain"].Value;
            if (string.IsNullOrWhiteSpace(domain)) domain = "https://www.yunzhijia.com";
            //获取token
            string urlToken = string.Format("{0}/gateway/oauth2/token/getAccessToken", domain);
            string resultToken = HttpPost(urlToken, new accessTokenJson(appid, secret).toJson());
            this.Document.SetValue("token", resultToken);
            JObject jo = (JObject)JsonConvert.DeserializeObject(resultToken);            
            string errcode = jo["errorCode"] != null ? jo["errorCode"].ToString() : string.Empty;  //错误代码
            //调用成功
            if (errcode == "0")
            {
                //获取用户信息的地址
                string urlUser = "{0}/gateway/ticket/user/acquirecontext?accessToken={1}";
                string token = jo["data"]["accessToken"] != null ? jo["data"]["accessToken"].ToString() : string.Empty;
                urlUser = string.Format(urlUser, domain, token);
                //请求数据（json格式）
                string ticket = WeiSha.Common.Request.QueryString["ticket"].String;
                JObject joUser = new JObject();
                joUser.Add("appid", appid);
                joUser.Add("ticket", ticket);
                string json = joUser.ToString(Newtonsoft.Json.Formatting.None, null);
                string resultUser = HttpPost(urlUser, json);
                this.Document.SetValue("user", resultUser);
            }
        }

        private string HttpPost(string Url, string json)
        {
            byte[] bs = Encoding.ASCII.GetBytes(json);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Url);
            req.Method = "POST";
            req.ContentType = "application/json";
            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }
            HttpWebResponse hwr = req.GetResponse() as HttpWebResponse;
            System.IO.StreamReader myreader = new System.IO.StreamReader(hwr.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            return responseText;
        }
    }
    /// <summary>
    /// 生成获取token的json数据
    /// </summary>
    public class accessTokenJson
    {
        public string appId { get; set; }
        public string secret { get; set; }
        public long timestamp { get; set; }
        public string scope { get; set; }
        public accessTokenJson(string appid, string secret)
        {
            this.appId = appid; //轻应用id
            this.secret = secret;   //轻应用secret,即appsecret
            //当前北京时间，Unix格式13位时间戳，精确到毫秒，3分钟内有效。
            DateTime time = DateTime.Now;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位 
            this.timestamp = t;
            this.scope = "app"; //授权级别：app
        }
        public string toJson()
        {
            string str = "\"appId\": \"{0}\",\"secret\": \"{1}\",\"timestamp\": {2},\"scope\": \"app\"";
            str = string.Format(str, this.appId, this.secret, this.timestamp.ToString());
            return "{" + str + "}";
        }
    }
}