using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Reflection;
using Song.Extend;

namespace Song.Site.API
{
    /// <summary>
    /// 来自其它网站的请求
    /// </summary>
    public class SSO : IHttpHandler
    {

        string appid = WeiSha.Common.Request.QueryString["appid"].String;       //appid  
        string user = WeiSha.Common.Request.QueryString["user"].String;         //用户名        
        string domain = WeiSha.Common.Request.QueryString["domain"].UrlDecode;  //来自请求的域名     
        string action = WeiSha.Common.Request.QueryString["action"].String;     //动作，login登录，logout退出登录      
        string ret = WeiSha.Common.Request.QueryString["return"].String;     //返回类型,xml或json
        string goto_url = WeiSha.Common.Request.QueryString["goto"].String;     //成功后的跳转地址

        public void ProcessRequest(HttpContext context)
        {
            
            SSO_State state = null;
            try
            {
                if (string.IsNullOrWhiteSpace(user))
                {
                    state = new SSO_State(false, 1, "账号不得为空");
                }
                else
                {
                    Song.Entities.SingleSignOn entity = Business.Do<ISSO>().GetSingle(appid);
                    if (entity == null) state = new SSO_State(false, 2, "接口对象不存在");
                    if (entity != null)
                    {
                        if (entity.SSO_Domain != domain.ToLower())
                        {
                            state = new SSO_State(false, 3, "该请求来自的域不合法");
                        }
                        else
                        {
                            //通过验证，进入登录状态            
                            Song.Entities.Accounts emp = Business.Do<IAccounts>().IsAccountsExist(user);
                            if (emp == null)
                            {
                                state = new SSO_State(false, 4, string.Format("当前账号({0})不存在", user));
                            }
                            else
                            {
                                if (!emp.Ac_IsPass || !emp.Ac_IsUse)
                                {
                                    state = new SSO_State(false, 5, string.Format("当前账号({0})被禁用或未通过审核", user));
                                }
                                else
                                {
                                    if (action == "logout")
                                    {
                                        LoginState.Accounts.Logout();
                                        state = new SSO_State(true, 7, string.Format("当前账号({0})退出登录", user));                                      
                                    }
                                    else
                                    {
                                        LoginState.Accounts.Write(emp);
                                        //登录成功
                                        Business.Do<IAccounts>().PointAdd4Login(emp, "协同站点登录", domain, "");   //增加登录积分
                                        Business.Do<IStudent>().LogForLoginAdd(emp);
                                        state = new SSO_State(true, 6, string.Format("当前账号({0})登录成功", user));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                state = new SSO_State(false, 0, ex.Message);
            }
            //如果成功，且转向地址不为空，则跳转
            if (state.success && !string.IsNullOrWhiteSpace(goto_url))
            {
                context.Response.Redirect(goto_url);
            }
            else
            {
                string reslut = state.ToReturn(ret);
                context.Response.Write(reslut);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    public class SSO_State
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 成功后需要转向的地址
        /// </summary>
        public string goto_url { get; set; }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="succ"></param>
        /// <param name="state"></param>
        /// <param name="msg"></param>
        public SSO_State(bool succ, int state, string msg)
        {
            this.success = succ;
            this.state = state;
            this.msg = msg;
        }
        /// <summary>
        /// 返回xml或json格式的值
        /// </summary>
        /// <param name="type">xml即返回xml格式值,json即返回json格式值，默认为xml</param>
        /// <returns></returns>
        public string ToReturn(string type)
        {
            return type == "json" ? this.ToJson() : this.ToXml();
        }
        /// <summary>
        /// 转换成xml格式
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            Type info = this.GetType();
            PropertyInfo[] properties = info.GetProperties();
            string str = "<xml>";
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                //当前属性的值
                object value = info.GetProperty(pi.Name).GetValue(this, null);
                //属性名（包括泛型名称）
                var nullableType = Nullable.GetUnderlyingType(pi.PropertyType);
                string typename = nullableType != null ? nullableType.Name : pi.PropertyType.Name;
                str += string.Format("<{0}>{1}</{0}>", pi.Name, _to_property(typename, value));
            }
            str += "</xml>";
            return str;
        }
        /// <summary>
        /// 转换成json格式
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            Type info = this.GetType();
            PropertyInfo[] properties = info.GetProperties();
            string str = "{";
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                //当前属性的值
                object value = info.GetProperty(pi.Name).GetValue(this, null);
                //属性名（包括泛型名称）
                var nullableType = Nullable.GetUnderlyingType(pi.PropertyType);
                string typename = nullableType != null ? nullableType.Name : pi.PropertyType.Name;
                str += string.Format("\"{0}\":\"{1}\",", pi.Name, _to_property(typename, value));
            }
            if (str.Length > 0 && str.Substring(str.Length - 1, 1) == ",") str = str.Substring(0, str.Length - 1);
            str += "}";
            return str;
        }
        /// <summary>
        /// 为json输出字段
        /// </summary>
        /// <param name="typename">字段的类型名称</param>
        /// <param name="value">字段的值</param>
        /// <returns></returns>
        private string _to_property(string typename, object value)
        {
            string str = "";
            //根据不同类型输出
            switch (typename)
            {
                case "DateTime":
                    System.DateTime time = System.DateTime.Now;
                    if (value != null) time = Convert.ToDateTime(value);
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
                    //将C#时间转换成JS时间字符串    
                    string JSstring = string.Format("eval('new ' + eval('/Date({0})/').source)", timeStamp);
                    str = JSstring;
                    break;
                case "String":
                    str = value == null ? "" : value.ToString().Replace("\"", "'");
                    break;
                case "Boolean":
                    str = value.ToString().ToLower();
                    break;
                default:
                    str = value == null ? "" : value.ToString();
                    break;
            }
            return str;
        }
    }
}