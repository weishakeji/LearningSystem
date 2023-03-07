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
    public class Letter : IEnumerable
    {
        #region 属性
        /// <summary>
        /// api的请求路径
        /// </summary>
        public string API_PATH { get; set; }
        /// <summary>
        /// 来源页，调用api方法时的所在页
        /// </summary>
        public Uri Referrer { get; private set; }
        /// <summary>
        /// HTTP请求谓词，即Get、Post、Put等，且全部是符合http协议的请求方法
        /// </summary>
        public string HTTP_METHOD { get; set; }
        /// <summary>
        /// Http的自定义方法，例如可能会有Cache之类，并不是http协议规定的方法
        /// </summary>
        public string Custom_METHOD { get; set; }
        /// <summary>
        /// 动作，即way冒号后面的关键字，一般用于cache方法的动作，例如clear清除缓存
        /// </summary>
        public string Custom_Action { get; set; }
        /// <summary>
        /// http请求的域名
        /// </summary>
        public string HTTP_HOST { get; set; }
        /// <summary>
        /// 请求标识，只允许是weishakeji
        /// </summary>
        public string HTTP_Mark { get; set; }
        /// <summary>
        /// 当前请求所在的网页地址
        /// </summary>
        public string WEB_PAGE { get; set; }
        /// <summary>
        /// 当前请求所在网页的域名，即客户端地址的主域
        /// </summary>
        public string WEB_HOST { get; set; }
        /// <summary>
        /// 返回数据的类型，默认为json，可以设置为xml
        /// </summary>
        public string ReturnType { get; set; }
        /// <summary>
        /// 登录状态的信息，来自客户端auth
        /// </summary>
        public string[] LoginStatus { get; set; }
        /// <summary>
        /// 接口版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 是否加密处理，当返回时数据采用Base64处理
        /// </summary>
        public bool Encrypt { get; set; }

        /// <summary>
        /// 所请求接口的类名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 所请求接口的方法
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// id参数，id是作为普通参数，因为常用，所以单独列出
        /// </summary>
        public long ID { get; set; }

        private Dictionary<string, string> _params = new Dictionary<string, string>();
        /// <summary>
        /// 参数集，包括Get和Post，即地址栏参数和form参数；
        /// 如果参数名称有重名，后者会替换前者的值
        /// </summary>
        public Dictionary<string, string> Params
        {
            get
            {
                return _params;
            }
            set
            {
                _params = value;
            }
        }
        private Dictionary<string, string> _cookies = new Dictionary<string, string>();
        /// <summary>
        /// Cookies参数集
        /// </summary>
        public Dictionary<string, string> Cookies
        {
            get
            {
                return _cookies;
            }
            set
            {
                _cookies = value;
            }
        }
        private HttpFileCollectionBase _files;
        /// <summary>
        /// 上传文件
        /// </summary>
        public HttpFileCollectionBase Files { 
            get
            {
                return _files;
            }            
        }
        /// <summary>
        /// 服务器端的信息
        /// </summary>
        public Song.ViewData.Server Sever
        {
            get
            {
                return Song.ViewData.Server.GetServer();
            }
        }

        /// <summary>
        /// 请求的来源，浏览器信息
        /// </summary>
        public string Browser { get; set; }
        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// 当前web请求
        /// </summary>
        public HttpRequestMessage Request { get; set; }
        #endregion

        #region 构造方法
        public Letter(HttpContext context)
        {
            HttpRequest request = context.Request;//定义传统request对象
            //客户端信息
            this.Browser = request.Browser.Browser + " " + request.Browser.Version;           
            UserAgent = request.UserAgent;
            //接口的所在页面
            Referrer = request.UrlReferrer;          
            WEB_PAGE = Referrer.AbsolutePath;
            WEB_HOST = Referrer.Authority;
            API_PATH = request.Url.AbsolutePath;
            HTTP_METHOD = request.HttpMethod;           
            HTTP_HOST = request.Url.Authority;      //等同Params["HTTP_HOST"]，但是由于Params["HTTP_HOST"]可以在客户端更改，不安全

            Encrypt = "true".Equals(HeadersParam(request.Headers, "Encrypt"), StringComparison.OrdinalIgnoreCase) ? true : false;
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
            ////用于调试，勿删除
            //string pas = string.Empty;
            //for (int i = 0; i < request.Params.Count; i++)
            //{
            //    string key = request.Params.Keys[i];
            //    string val = request.Params[key];
            //    pas += string.Format("{0}、{1}:{2}\n", (i + 1).ToString(), key,val);
            //}
            //获取类名与方法名，可以考虑增加默认方法，例如index或default
            string[] arr = request.Url.Segments;
            string clasname = arr[3];
            string action = arr[4];
            if (clasname.EndsWith("/")) clasname = clasname.Substring(0, clasname.LastIndexOf("/"));
            if (action.EndsWith("/")) action = action.Substring(0, action.LastIndexOf("/"));
            this.ClassName = clasname;
            this.MethodName = action;

            #region 获取参数
            //获取get参数
            for (int i = 0; i < context.Request.QueryString.Count; i++)
            {
                string key = context.Request.QueryString.Keys[i].ToString().Trim();
                string val = Microsoft.JScript.GlobalObject.unescape(context.Request.QueryString[i].ToString().Trim());
                if (_params.ContainsKey(key))
                    _params[key] = val;
                else
                    _params.Add(key, val);
            }
            //获取post参数，put,delete,patch,options都从这里获取
            for (int i = 0; i < context.Request.Form.Count; i++)
            {
                string key = context.Request.Form.Keys[i].ToString().Trim();
                string val = Microsoft.JScript.GlobalObject.unescape(context.Request.Form[i].ToString().Trim());
                if (_params.ContainsKey(key))
                    _params[key] = val;
                else
                    _params.Add(key, val);
            }
            //清理脚本与DTD定义
            _params = _params.ToDictionary(x => x.Key, x => Html.ClearScript(x.Value));
            //上传来的文件
            HttpContextWrapper _context = new HttpContextWrapper(context);
            this._files = _context.Request.Files;

            this.ID = this["id"].Int64 ?? 0;
            //获取cookies
            for (int i = 0; i < context.Request.Cookies.Count; i++)
            {
                string key = context.Request.Cookies.Keys[i].ToString();
                string val = context.Request.Cookies[i].Value.ToString();
                if (_cookies.ContainsKey(key))
                    _cookies[key] = val;
                else
                    _cookies.Add(key, val);
            }
            #endregion
        }
        /// <summary>
        /// 构造函数，来自MVC控制器的调用，客户端采用$api时会经过MVC控制器
        /// 通过此种方式，会获取get、post参数，并从当前线程获取cookies集合
        /// </summary>
        /// <param name="httprequest">api控制器的访问对象</param>
        public Letter(HttpRequestMessage httprequest)
        {
            this.Request = httprequest;

            API_PATH = httprequest.RequestUri.AbsolutePath;
            Referrer = httprequest.Headers.Referrer;
            HTTP_METHOD = httprequest.Method.Method; //请求方法           
            HTTP_HOST = httprequest.Headers.Host;
            //是否返回加密数据
            Encrypt = "true".Equals(HeadersParam(httprequest.Headers, "Encrypt"), StringComparison.OrdinalIgnoreCase) ? true : false;
            HTTP_Mark = HeadersParam(httprequest.Headers, "X-Custom-Header");
            Custom_METHOD = HeadersParam(httprequest.Headers, "X-Custom-Method").ToUpper();
            if (!HTTP_METHOD.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                HTTP_METHOD = Custom_METHOD;
            Custom_Action = HeadersParam(httprequest.Headers, "X-Custom-Action");
            ReturnType = HeadersParam(httprequest.Headers, "X-Custom-Return");
            //接口的所在页面
            WEB_PAGE = Referrer.AbsolutePath;
            WEB_HOST = Referrer.Authority;
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
                if (_params.ContainsKey(key))
                    _params[key] = val;
                else
                    _params.Add(key, val);
            }
            //获取post参数，put,delete,patch,options都从这里获取
            for (int i = 0; i < context.Request.Form.Count; i++)
            {
                string key = context.Request.Form.Keys[i].ToString().Trim();
                string tm = context.Request.Form[i];
                string val = Microsoft.JScript.GlobalObject.unescape(context.Request.Form[i].ToString().Trim());
                if (_params.ContainsKey(key))
                    _params[key] = val;
                else
                    _params.Add(key, val);
            }
            //清理脚本与DTD定义
            _params = _params.ToDictionary(x => x.Key, x => Html.ClearScript(x.Value));
            //上传来的文件
            this._files = context.Request.Files;
            //foreach (string key in context.Request.Files)
            //{
            //    //这里只测试上传第一张图片file[0]
            //    HttpPostedFileBase file0 = context.Request.Files[key];
            //}
            this.ID = this["id"].Int64 ?? 0;
            //获取cookies
            for (int i = 0; i < context.Request.Cookies.Count; i++)
            {
                string key = context.Request.Cookies.Keys[i].ToString();
                string val = context.Request.Cookies[i].Value.ToString();
                if (_cookies.ContainsKey(key))
                    _cookies[key] = val;
                else
                    _cookies.Add(key, val);
            }
        }
        /// <summary>
        /// 构造方法，直接用字符串传递参数，服务器端@Api调用时用此方法
        /// 通过此种方式，利用参数2（string paramseters）解析所需参数，并从当前线程获取cookies集合
        /// </summary>
        /// <param name="method">格式：类名/方法名</param>
        /// <param name="paramseters">格式："p1:str,p2:str"</param>
        public Letter(string method, string paramseters)
        {
            //获取类名和方法
            string classname = string.Empty, methodname = string.Empty;
            if (method.IndexOf("/") > -1)
            {
                classname = method.Substring(0, method.LastIndexOf("/"));
                methodname = method.Substring(method.LastIndexOf("/") + 1);
            }
            this.ClassName = classname;
            this.MethodName = methodname;
            //解析参数
            string rep = DateTime.Now.Ticks.ToString();
            paramseters = paramseters.Replace(@"\,", rep);      //将\,逗号处理一下
            foreach (string item in paramseters.Split(','))
            {
                string t = item.Replace(rep, @",");     //将逗号再弄回来
                t = t.Replace(@"\:", rep);
                string[] arr = t.Split(':');
                if (arr.Length < 2) continue;
                arr[1] = arr[1].Replace(rep, @":").Trim();
                //去除参数的前后单引号
                if (arr[1].StartsWith("'")) arr[1] = arr[1].Substring(1);
                if (arr[1].EndsWith("'")) arr[1] = arr[1].Length > 1 ? arr[1].Substring(0, arr[1].Length - 1) : "";
                _params.Add(arr[0].Trim(), arr[1].Trim());
            }
            //清理脚本与DTD定义
            _params = _params.ToDictionary(x => x.Key, x => Html.ClearScript(x.Value));
            this.ID = this["id"].Int64 ?? 0;
            //获取cookies       
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            Referrer = context.Request.UrlReferrer;
            for (int i = 0; i < context.Request.Cookies.Count; i++)
            {
                string key = context.Request.Cookies.Keys[i].ToString();
                string val = context.Request.Cookies[i].Value.ToString();
                if (_cookies.ContainsKey(key))
                    _cookies[key] = val;
                else
                    _cookies.Add(key, val);
            }
        }
        #endregion

        #region 获取参数的方法
        /// <summary>
        /// 获取头部Header参数
        /// </summary>
        /// <param name="header"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string HeadersParam(object header,string key)
        {
            if(header is System.Collections.Specialized.NameValueCollection)
            {
                System.Collections.Specialized.NameValueCollection collection = (System.Collections.Specialized.NameValueCollection)header;
                return collection[key] == null ? string.Empty : collection[key];
            }
            if (header is System.Net.Http.Headers.HttpRequestHeaders)
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = (System.Net.Http.Headers.HttpRequestHeaders)header;
                IEnumerable<string> values= headers.GetValues(key);
                if (values.Count<string>() > 0) return values.First<string>();
                return string.Empty;
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取参数的值，等同GetParameter(string key)方法
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public ConvertToAnyValue this[string key]
        {
            get
            {
                return GetParameter(key);
            }
        }
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数Value值</returns>
        public ConvertToAnyValue GetParameter(string key)
        {
            string val = string.Empty;
            foreach (KeyValuePair<string, string> kv in _params)
            {
                if (key.Trim().Equals(kv.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    val = kv.Value;
                    break;
                }
            }
            return new ConvertToAnyValue(val);
        }
        /// <summary>
        /// 是否存在某参数
        /// </summary>
        /// <param name="key">参数的Key值</param>
        /// <returns>参数Value值</returns>
        public bool ExistParameter(string key)
        {
            if (_params.ContainsKey(key))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 将参数名称串联
        /// </summary>
        /// <returns></returns>
        public string ParamsNames()
        {
            return this.ParamsNames(string.Empty);
        }
        /// <summary>
        /// 将参数名称串联
        /// </summary>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public string ParamsNames(string separator)
        {
            if (string.IsNullOrWhiteSpace(separator)) separator = ",";

            string str = string.Empty;
            List<string> list = new List<string>(_params.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                str += list[i];
                str += i < list.Count - 1 ? separator : "";
            }
            return str;
        }
        /// <summary>
        /// 获取cookie值
        /// </summary>
        /// <param name="key">cookie名称</param>
        /// <returns>cookie的值</returns>
        public ConvertToAnyValue GetCookie(string key)
        {
            string val = string.Empty;
            foreach (KeyValuePair<string, string> kv in _cookies)
            {
                if (key.Equals(kv.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    val = kv.Value;
                    break;
                }
            }
            return new ConvertToAnyValue(val);
        }
        #endregion

        #region 重构的一些方法或接口实现
        /// <summary>
        /// 重写ToString方法，将参数串连成字符串
        /// </summary>
        /// <returns>格式：key=value;</returns>
        public override string ToString()
        {
            string str = string.Empty;
            foreach (KeyValuePair<string, string> kv in _params)
            {
                str += kv.Key + "=" + kv.Value + ";";
            }
            return str;
        }
        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach (KeyValuePair<string, string> kv in _params)
            {
                yield return kv;
            }
        }
        #endregion
        
        /// <summary>
        /// 将参数转换为其它类型，常见转换成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Convert<T>() where T : new()
        {
            T t = new T();
            Type type = t.GetType();
            PropertyInfo[] properties = type.GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                object piValue = new object();
                foreach (KeyValuePair<string, string> kv in _params)
                {
                    string val = kv.Value;
                    if (string.IsNullOrWhiteSpace(val) || val == "undefined") continue;
                    if (kv.Key.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        switch (pi.PropertyType.Name)
                        {
                            case "DateTime":
                                if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                                {
                                    piValue = DateTime.Now;
                                    break;
                                }
                                DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                long lTime = long.Parse(val + "0000");
                                TimeSpan toNow = new TimeSpan(lTime);
                                piValue = dt.Add(toNow);
                                break;
                            default:
                                piValue = System.Convert.ChangeType(val, pi.PropertyType);
                                break;
                        }
                        pi.SetValue(t, piValue, null);
                        break;
                    }
                }               
            }
            return t;
        }
    }
}
