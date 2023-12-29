using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Song.ViewData
{
    /// <summary>
    /// 记录客户端递交来的信息
    /// </summary>
    public class Letter_v2 : Letter
    {
        #region 构造方法
        public Letter_v2(HttpContext context)
        {
            HttpRequest request = context.Request;//定义传统request对象
            //上传来的文件
            HttpContextWrapper _context = new HttpContextWrapper(context);
            this.Files = _context.Request.Files;
            //获取类名与方法名，可以考虑增加默认方法，例如index或default
            string[] arr = request.Url.Segments;
            for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Replace("/", "");
            this.Version = arr[2];
            this.ClassName = arr[3];
            this.MethodName = arr[4];
            //客户端信息
            this.Browser = request.Browser.Browser + " " + request.Browser.Version;
            UserAgent = request.UserAgent;
            //接口的所在页面
            this.Referrer = request.UrlReferrer;
            WEB_PAGE = Referrer != null ? Referrer.AbsolutePath : string.Empty;
            WEB_HOST = Referrer != null ? Referrer.Authority : HTTP_HOST;
            //接口路径与方法
            API_PATH = request.Url.AbsolutePath;
            HTTP_METHOD = request.HttpMethod;
            HTTP_HOST = request.Url.Authority;      //等同Params["HTTP_HOST"]，但是由于Params["HTTP_HOST"]可以在客户端更改，不安全
            //头信息
            Encrypt = "true".Equals(HeadersParam(request.Headers, "Encrypt"), StringComparison.OrdinalIgnoreCase) ? true : false;
            //经纬度,longitude,latitude
            decimal lng, lat;
            decimal.TryParse(HeadersParam(request.Headers, "Longitude"), out lng);
            decimal.TryParse(HeadersParam(request.Headers, "Latitude"), out lat);
            Longitude = lng;
            Latitude = lat;
            //请求标识
            HTTP_Mark = HeadersParam(request.Headers, "X-Custom-Header");
            Custom_METHOD = HeadersParam(request.Headers, "X-Custom-Method").ToUpper();
            if (!HTTP_METHOD.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                HTTP_METHOD = Custom_METHOD;
            Custom_Action = HeadersParam(request.Headers, "X-Custom-Action");
            ReturnType = HeadersParam(request.Headers, "X-Custom-Return");
            //Authorization的解析
            string auth = request.Params["HTTP_AUTHORIZATION"];
            if (!string.IsNullOrWhiteSpace(auth))
            {
                auth = auth.Substring(auth.IndexOf(" ")).Trim();
                auth = WeiSha.Core.DataConvert.DecryptForBase64(auth);
                List<string> users = new List<string>();
                string[] prefix = auth.Substring(0, auth.LastIndexOf(":")).Split(' ');
                foreach (string s in prefix)
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    users.Add(s);
                }
                if (users.Count > 0 && !string.IsNullOrWhiteSpace(WEB_PAGE)) WEB_PAGE = users[0];
                //登录状态信息
                string pwstr = auth.Substring(auth.LastIndexOf(":") + 1);
                if (!string.IsNullOrWhiteSpace(pwstr)) this.LoginStatus = pwstr.Split(',');
            }
            #region 获取参数
            //获取get参数
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                string key = context.Request.QueryString.Keys[i].ToString().Trim();
                string val = Microsoft.JScript.GlobalObject.unescape(context.Request.QueryString[i].ToString().Trim());
                SetParameter(key, val);
            }
            //获取post参数，put,delete,patch,options都从这里获取
            for (int i = 0; i < context.Request.Form.Count; i++)
            {
                string key = context.Request.Form.Keys[i].ToString().Trim();
                string val = Microsoft.JScript.GlobalObject.unescape(context.Request.Form[i].ToString().Trim());
                SetParameter(key, val);
            }
            //清理脚本与DTD定义         
            this.ID = this["id"].Int64 ?? 0;
            //获取cookies
            for (int i = 0; i < context.Request.Cookies.Count; i++)
            {
                string key = context.Request.Cookies.Keys[i].ToString();
                string val = context.Request.Cookies[i].Value.ToString();
                SetCookies(key, val);
            }
            #endregion
        }
        /// <summary>
        /// 构造函数，来自MVC控制器的调用，客户端采用$api时会经过MVC控制器
        /// 通过此种方式，会获取get、post参数，并从当前线程获取cookies集合
        /// </summary>
        /// <param name="httprequest">api控制器的访问对象</param>
        public Letter_v2(HttpRequestMessage httprequest)
        {
            this.Request = httprequest;
            API_PATH = httprequest.RequestUri.AbsolutePath;
            Referrer = httprequest.Headers.Referrer;
            HTTP_METHOD = httprequest.Method.Method; //请求方法           
            HTTP_HOST = httprequest.Headers.Host;
            //是否返回加密数据
            Encrypt = "true".Equals(HeadersParam(httprequest.Headers, "Encrypt"), StringComparison.OrdinalIgnoreCase) ? true : false;
            //经纬度,longitude,latitude
            decimal lng, lat;
            decimal.TryParse(HeadersParam(httprequest.Headers, "Longitude"), out lng);
            decimal.TryParse(HeadersParam(httprequest.Headers, "Latitude"), out lat);
            Longitude = lng;
            Latitude = lat;
            //请求标识
            HTTP_Mark = HeadersParam(httprequest.Headers, "X-Custom-Header");
            Custom_METHOD = HeadersParam(httprequest.Headers, "X-Custom-Method").ToUpper();
            if (!HTTP_METHOD.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                HTTP_METHOD = Custom_METHOD;
            Custom_Action = HeadersParam(httprequest.Headers, "X-Custom-Action");
            ReturnType = HeadersParam(httprequest.Headers, "X-Custom-Return");
            //接口的所在页面
            WEB_PAGE = Referrer != null ? Referrer.AbsolutePath : string.Empty;
            WEB_HOST = Referrer != null ? Referrer.Authority : HTTP_HOST;
            //Authorization的解析
            string auth = string.Empty;
            if (httprequest.Headers.Authorization != null)
                auth = httprequest.Headers.Authorization.ToString();
            if (!string.IsNullOrWhiteSpace(auth))
            {
                auth = auth.Substring(auth.IndexOf(" ")).Trim();
                auth = WeiSha.Core.DataConvert.DecryptForBase64(auth);
                List<string> users = new List<string>();
                string[] prefix = auth.Substring(0, auth.LastIndexOf(":")).Split(' ');
                foreach (string s in prefix)
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    users.Add(s);
                }
                if (users.Count > 0 && !string.IsNullOrWhiteSpace(WEB_PAGE)) WEB_PAGE = users[0];

                //登录状态信息
                string pwstr = auth.Substring(auth.LastIndexOf(":") + 1);
                if (!string.IsNullOrWhiteSpace(pwstr)) this.LoginStatus = pwstr.Split(',');
            }
            //从请求地址中，分析类名与方法名
            string[] arr = httprequest.RequestUri.Segments;
            for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Replace("/", "");
            this.Version = arr[2];
            //获取类名与方法名
            string clasname = arr[3];
            string action = arr[4];
            if (clasname.EndsWith("/")) clasname = clasname.Substring(0, clasname.LastIndexOf("/"));
            if (action.EndsWith("/")) action = action.Substring(0, action.LastIndexOf("/"));
            this.ClassName = clasname;
            this.MethodName = action;
            //获取参数
            HttpContextBase context = (HttpContextBase)httprequest.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            //客户端信息
            this.Browser = request.Browser.Browser + " " + request.Browser.Version;
            this.UserAgent = request.UserAgent;
            //获取get参数
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                string key = context.Request.QueryString.Keys[i].ToString().Trim();
                string val = Microsoft.JScript.GlobalObject.unescape(context.Request.QueryString[i].ToString().Trim());
                SetParameter(key, val);
            }
            //获取post参数，put,delete,patch,options都从这里获取
            for (int i = 0; i < context.Request.Form.Count; i++)
            {
                string key = context.Request.Form.Keys[i].ToString().Trim();
                string tm = context.Request.Form[i];
                string val = Microsoft.JScript.GlobalObject.unescape(context.Request.Form[i].ToString().Trim());
                SetParameter(key, val);
            }          
            //上传来的文件
            this.Files = context.Request.Files;
            this.ID = this["id"].Int64 ?? 0;
            //获取cookies
            for (int i = 0; i < context.Request.Cookies.Count; i++)
            {
                string key = context.Request.Cookies.Keys[i].ToString();
                string val = context.Request.Cookies[i].Value.ToString();
                SetCookies(key, val);
            }
        }
        #endregion
    }
}
