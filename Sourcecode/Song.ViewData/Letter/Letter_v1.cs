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
    public class Letter_v1 : Letter
    {
        #region 构造方法
        public Letter_v1(HttpContext context)
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

            API_PATH = request.Url.AbsolutePath;
            HTTP_METHOD = request.HttpMethod;
            Custom_METHOD = HTTP_METHOD;
            HTTP_HOST = request.Url.Authority;      //等同Params["HTTP_HOST"]，但是由于Params["HTTP_HOST"]可以在客户端更改，不安全
            //接口的所在页面
            this.Referrer = request.UrlReferrer;
            WEB_PAGE = Referrer != null ? Referrer.AbsolutePath : string.Empty;
            WEB_HOST = Referrer != null ? Referrer.Authority : HTTP_HOST;
            //Authorization的解析
            string auth = request.Params["HTTP_AUTHORIZATION"];
            if (!string.IsNullOrWhiteSpace(auth))
            {
                auth = auth.Substring(auth.IndexOf(" ")).Trim();
                auth = WeiSha.Core.DataConvert.DecryptForBase64(auth);
                List<string> users = new List<string>();
                foreach (string s in auth.Substring(0, auth.LastIndexOf(":")).Split(' '))
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    users.Add(s);
                }
                if (users.Count > 0) HTTP_Mark = users[0];
                if (users.Count > 1 && !HTTP_METHOD.Equals("get", StringComparison.CurrentCultureIgnoreCase)) HTTP_METHOD = users[1].ToUpper();
                if (users.Count > 2) ReturnType = users[2];
                if (users.Count > 3 && string.IsNullOrWhiteSpace(WEB_PAGE)) WEB_PAGE = users[3];
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
            this.ID = this["id"].Int32 ?? 0;
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
        public Letter_v1(HttpRequestMessage httprequest)
        {
            this.Request = httprequest;
            API_PATH = httprequest.RequestUri.AbsolutePath;
            Referrer = httprequest.Headers.Referrer;
            HTTP_METHOD = httprequest.Method.Method; //请求方法     
            Custom_METHOD = HTTP_METHOD;
            HTTP_HOST = httprequest.Headers.Host;
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
                foreach (string s in auth.Substring(0, auth.LastIndexOf(":")).Split(' '))
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    users.Add(s);
                }
                if (users.Count > 0) HTTP_Mark = users[0];
                if (users.Count > 1 && !HTTP_METHOD.Equals("get", StringComparison.CurrentCultureIgnoreCase)) HTTP_METHOD = users[1].ToUpper();
                if (users.Count > 2) ReturnType = users[2];
                if (users.Count > 3 && string.IsNullOrWhiteSpace(WEB_PAGE)) WEB_PAGE = users[3];
                //登录状态信息
                string pwstr = auth.Substring(auth.LastIndexOf(":") + 1);
                if (!string.IsNullOrWhiteSpace(pwstr)) this.LoginStatus = pwstr.Split(',');
            }
            //从请求地址中，分析类名与方法名
            string[] arr = httprequest.RequestUri.Segments;
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
            this.ID = this["id"].Int32 ?? 0;
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
