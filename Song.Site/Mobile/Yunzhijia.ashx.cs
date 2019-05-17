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
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                string json = string.Empty;
                switch (action)
                {
                    case "getuser":
                        json = getUser();
                        break;
                }
                context.Response.Write(json);
                context.Response.End();
            }
        }
        private string getUser()
        {
            bool isUse = Business.Do<ISystemPara>()["YunzhijiaLoginIsuse"].Boolean ?? true;
            string appid = Business.Do<ISystemPara>()["YunzhijiaAppid"].String;
            string secret = Business.Do<ISystemPara>()["YunzhijiaAppSecret"].String;
            string domain = Business.Do<ISystemPara>()["YunzhijiaDomain"].Value;    //云之家的域名
            string accname = Business.Do<ISystemPara>()["YunzhijiaAcc"].Value;  //指定云之家用户信息的某个字段作为当前系统的账号字段
            string ticket = WeiSha.Common.Request.QueryString["ticket"].String;
            string json = string.Empty;
            try
            {
                //验证
                if (!isUse) throw new Exception("禁止云之家账号登录学习系统");
                if (string.IsNullOrWhiteSpace(appid)) throw new Exception("AppID不得为空");
                if (string.IsNullOrWhiteSpace(secret)) throw new Exception("AppSecret不得为空");
                if (string.IsNullOrWhiteSpace(domain)) throw new Exception("云之家域名不正确");
                if (string.IsNullOrWhiteSpace(accname)) throw new Exception("需要指定的账号字段未设置");

                //获取token
                string urlToken = string.Format("{0}/openauth2/api/token?grant_type=client_credential&appid={1}&secret={2}", domain, appid, secret);
                string resultToken = WeiSha.Common.Request.HttpGet(urlToken);
                JObject jo = (JObject)JsonConvert.DeserializeObject(resultToken);
                string token = jo["access_token"] != null ? jo["access_token"].ToString() : string.Empty; //token
                if (!string.IsNullOrWhiteSpace(token))
                {
                    string urlUser = string.Format("{0}/openauth2/api/getcontext?ticket={1}&access_token={2}", domain, ticket, token);
                    string resultUser = WeiSha.Common.Request.HttpGet(urlUser);
                    jo = (JObject)JsonConvert.DeserializeObject(resultUser);
                    string accout = jo[accname] != null ? jo[accname].ToString() : string.Empty; //获取账号信息
                    if (string.IsNullOrWhiteSpace(accout)) throw new Exception("未获取到账号信息");
                    json = resultUser;
                }
            }
            catch (Exception ex)
            {
                JObject joUser = new JObject();
                joUser.Add("appid", "0");
                joUser.Add("message", ex.Message);
                json = joUser.ToString(Newtonsoft.Json.Formatting.None, null);
            }
            return json;
        }
    }
}