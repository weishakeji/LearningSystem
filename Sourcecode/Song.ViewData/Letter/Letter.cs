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
    public abstract class Letter : IEnumerable
    {
        #region 静态参数

        private static string _filterWords = WeiSha.Core.App.Get["FilterWords"].String;
        private static Dictionary<string, string> _filterWordsdic = new Dictionary<string, string>();
        /// <summary>
        /// 过滤字符
        /// </summary>
        public static Dictionary<string, string> FilterWords
        {
            get
            {
                if (_filterWordsdic.Count < 1)
                {
                    if (string.IsNullOrWhiteSpace(_filterWords)) return _filterWordsdic;
                    string[] arr = _filterWords.Split('|');
                    foreach (string s in arr)
                    {
                        if (string.IsNullOrWhiteSpace(s)) continue;
                        _filterWordsdic.Add(s, toSBC(s));
                    }
                }
                return _filterWordsdic;
            }
        }
        #region 全角转换半角以及半角转换为全角  
        ///转全角的函数(SBC case)  
        ///全角空格为12288，半角空格为32  
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248  
        private static string toSBC(string input)
        {
            // 半角转全角：  
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 32)
                {
                    array[i] = (char)12288;
                    continue;
                }
                if (array[i] < 127)
                    array[i] = (char)(array[i] + 65248);

            }
            return new string(array);
        }

        ///转半角的函数(DBC case)  
        ///全角空格为12288，半角空格为32  
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248//   
        private static string toDBC(string input)
        {
            char[] array = input.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == 12288)
                {
                    array[i] = (char)32;
                    continue;
                }
                if (array[i] > 65280 && array[i] < 65375)
                    array[i] = (char)(array[i] - 65248);
            }
            return new string(array);
        }
        #endregion
        #endregion
        #region 属性
        /// <summary>
        /// api的请求路径
        /// </summary>
        public string API_PATH { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Longitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Latitude { get; set; }
        /// <summary>
        /// 来源页，调用api方法时的所在页
        /// </summary>
        public Uri Referrer { get; set; }
        /// <summary>
        /// 请求来源的IP地址
        /// </summary>
        public string IP { get; set; }
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
        /// <summary>
        /// 当前Http会话ID，用于标识当前用户的会话
        /// </summary>
        public string SessionID { get; set; }
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
        /// <summary>
        /// 上传文件
        /// </summary>
        public HttpFileCollectionBase Files { get; set; }
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
        public Song.ViewData.Browser Browser { get; set; }
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
        public Letter() { }
        /// <summary>
        /// 构造函数，通过http线程获取
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static Letter Constructor(HttpContext context)
        {
            HttpRequest request = context.Request;//定义传统request对象
            //获取接口版本
            string[] arr = request.Url.Segments;
            for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Replace("/", "");
            string version = arr[2];
            if ("v1".Equals(version, StringComparison.OrdinalIgnoreCase)) return new Letter_v1(context);
            else
                return new Letter_v2(context);
        }
        /// <summary>
        /// 构造函数，来自MVC控制器的调用，客户端采用$api时会经过MVC控制器
        /// 通过此种方式，会获取get、post参数，并从当前线程获取cookies集合
        /// </summary>
        /// <param name="httprequest">api控制器的访问对象</param>
        public static Letter Constructor(HttpRequestMessage httprequest)
        {
            string[] arr = httprequest.RequestUri.Segments;    
            for (int i = 0; i < arr.Length; i++) arr[i] = arr[i].Replace("/", "");
            string version = arr[2];
            if ("v1".Equals(version, StringComparison.OrdinalIgnoreCase)) return new Letter_v1(httprequest);
            else
                return new Letter_v2(httprequest);
        }
       
        #endregion

        #region 获取参数的方法
        /// <summary>
        /// 获取头部Header参数
        /// </summary>
        /// <param name="header"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string HeadersParam(object header, string key)
        {
            if (header is System.Collections.Specialized.NameValueCollection)
            {
                System.Collections.Specialized.NameValueCollection collection = (System.Collections.Specialized.NameValueCollection)header;
                return collection[key] == null ? string.Empty : collection[key];
            }
            if (header is System.Net.Http.Headers.HttpRequestHeaders)
            {
                System.Net.Http.Headers.HttpRequestHeaders headers = (System.Net.Http.Headers.HttpRequestHeaders)header;
                try
                {
                    IEnumerable<string> values = headers.GetValues(key);
                    if (values.Count<string>() > 0) return values.First<string>();
                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 获取参数的值，等同GetParameter(string key)方法
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public ConvertToAnyValue this[string key]=>GetParameter(key);
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns>参数Value值</returns>
        public ConvertToAnyValue GetParameter(string key)
        {
            string val = _params.FirstOrDefault(kv => key.Trim().Equals(kv.Key, StringComparison.CurrentCultureIgnoreCase)).Value;        
            return new ConvertToAnyValue(val);
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public Dictionary<string, string> SetParameter(string key, string val)
        {
            //清理html标签
            if (!string.IsNullOrWhiteSpace(val)) val = Html.ClearScript(val);
            //过滤
            Dictionary<string, string> dic = Letter.FilterWords;
            foreach (KeyValuePair<string, string> kv in dic)
                val = val.Replace(kv.Key, kv.Value);
            _params[key] = val;
            return _params;
        }
        /// <summary>
        /// 设置cookies的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public void SetCookies(string key, string val) => _cookies[key] = Html.ClearScript(val);
        /// <summary>
        /// 是否存在某参数
        /// </summary>
        /// <param name="key">参数的Key值</param>
        /// <returns>参数Value值</returns>
        public bool ExistParameter(string key) => _params.ContainsKey(key);
        /// <summary>
        /// 将参数名称串联
        /// </summary>
        /// <returns></returns>
        public string ParamsNames() => this.ParamsNames(string.Empty);
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
