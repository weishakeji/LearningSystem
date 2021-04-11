using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Song.Extend;
using System.IO;
using System.Text;
using VTemplate.Engine;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Web.SessionState;
using System.Collections.Generic;

namespace Song.Site
{
    public abstract class BasePage : System.Web.UI.Page, IHttpHandler, IRequiresSessionState
    {
        private static string _version = string.Empty;
        /// <summary>
        /// 系统版本号
        /// </summary>
        protected static string version
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_version))
                {
                    _version= System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
                return _version;
            }
        }
        /// <summary>
        /// 当前页面的模板文档对象
        /// </summary>
        protected TemplateDocument Document { get; set; }
        /// <summary>
        /// 当前页面所在模板库
        /// </summary>
        protected WeiSha.Common.Templates.TemplateBank TmBank{ get; set; }
        /// <summary>
        /// 当前页面的模板文档的配置参数
        /// </summary>
        protected virtual TemplateDocumentConfig DocumentConfig
        {
            get { return TemplateDocumentConfig.Default; }
        }
        /// <summary>
        /// 初始化当前页面模板数据
        /// </summary>
        protected abstract void InitPageTemplate(HttpContext context);
        /// <summary>
        /// 装载当前页面的模板文档
        /// </summary>
        public virtual void LoadCurrentTemplate()
        {
            //是否是手机端网页
            string filePath = this.Request.Url.AbsolutePath;
            bool isMobi = isMobilePage(out filePath);     //处理后filePath为文件名（不含扩展名）  
            if (isMobi && LoginState.Accounts.IsLogin)
                LoginState.Accounts.Refresh(LoginState.Accounts.CurrentUser);
            //取模板对象
            if (this.Organ != null)
            {
                this.TmBank = isMobi ?
                    WeiSha.Common.Template.ForMobile.SetCurrent(this.Organ.Org_TemplateMobi)
                    : WeiSha.Common.Template.ForWeb.SetCurrent(this.Organ.Org_Template);
            }
            else
            {
                this.TmBank = isMobi ? WeiSha.Common.Template.ForMobile.Default : WeiSha.Common.Template.ForWeb.Default;
            }
            if (TmBank == null) throw new Exception("没有任何模板库可用！");
            //是否是公共页面
            if (TmBank.Config.Public == null) throw new Exception("未找到公共模板库！");
            bool isPublic = TmBank.Config.Public.PageExists(filePath);
            if (isPublic) TmBank = TmBank.Config.Public;
            //当前模板的所在路径
            string tmFile = TmBank.Path.Physics + filePath + ".htm";
            //装载模板
            this.Document = null;
            if (!System.IO.File.Exists(tmFile))
            {
                tmFile = TmBank.Config.Default.Path.Physics + filePath + ".htm";
                if (!System.IO.File.Exists(tmFile)) tmFile = TmBank.Config.Public.Path.Physics + "Notfound.htm";                             
            }
            this.Document = TemplateDocument.FromFileCache(tmFile, Encoding.UTF8, this.DocumentConfig);
            //this.Document = new TemplateDocument(tmFile, Encoding.UTF8, this.DocumentConfig);   //不采用缓存 
        }
        /// <summary>
        /// 是否是手机端
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected bool isMobilePage(out string path)
        {
            bool ismobi = false;
            string prefix = "/mobile";
            path = this.Request.Url.AbsolutePath;
            if (path.Length >= prefix.Length)
            {
                string pre = path.Substring(0, prefix.Length);
                if (pre.ToLower() == prefix.ToLower()) ismobi = true;
            }
            //如果是手机端页面，则去除/mobile/路径
            if (ismobi) path = path.Substring(prefix.Length);
            if (path.IndexOf(".") > -1)
                path = path.Substring(path.IndexOf("/") + 1, path.LastIndexOf(".") - 1);
            else
                path = path.Substring(path.IndexOf("/") + 1);
            path = path.Replace("/", "\\");
            //自定义配置项
            if (this.Organ != null)
            {
                WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
                bool isNoaccess = false;    //是否禁止访问
                                            //如果是手机端
                if (ismobi)
                {
                    //如果禁止微信中使用，且又处于微信中时
                    if ((config["DisenableWeixin"].Value.Boolean ?? false) && WeiSha.Common.Browser.IsWeixin) isNoaccess = true;
                    if ((config["DisenableMini"].Value.Boolean ?? false) && WeiSha.Common.Browser.IsWeixinApp) isNoaccess = true;
                    if ((config["DisenableMweb"].Value.Boolean ?? false) && (!WeiSha.Common.Browser.IsAPICloud && !WeiSha.Common.Browser.IsWeixin))
                        isNoaccess = true;
                    if ((config["DisenableAPP"].Value.Boolean ?? false) && WeiSha.Common.Browser.IsAPICloud) isNoaccess = true;
                }
                else
                {
                    if ((config["WebForDeskapp"].Value.Boolean ?? false) && !WeiSha.Common.Browser.IsDestopApp) isNoaccess = true;
                }
                //如果被限制访问
                if (isNoaccess) path = "Noaccess";
            }
            return ismobi;
        }
        #region 初始化的操作
        protected new HttpContext Context { get; private set; }
        protected new HttpApplicationState Application { get; private set; }
        protected new HttpRequest Request { get; private set; }
        protected new HttpResponse Response { get; private set; }
        protected new HttpServerUtility Server { get; private set; }
        protected new HttpSessionState Session { get; private set; }
        //当前所在机构
        protected Song.Entities.Organization Organ { get; private set; }
        //学生，教师，管理员
        protected Song.Entities.Accounts Account { get; private set; }
        protected Song.Entities.Teacher Teacher { get; private set; }
        protected Song.Entities.EmpAccount Admin { get; private set; }
        /// <summary>
        /// 初始化上下文数据
        /// </summary>
        /// <param name="context"></param>
        private void InitContext(HttpContext context)
        {
            //初始化参数
            this.Context = context;
            this.Application = context.Application;
            this.Request = context.Request;
            this.Response = context.Response;
            this.Server = context.Server;
            this.Session = context.Session;

            //机构信息
            try
            {
                this.Organ = Business.Do<IOrganization>().OrganCurrent();
                if (this.Organ == null) throw new Exception("机构不存在！");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //登录的信息
            if (Extend.LoginState.Accounts.IsLogin)
            {
                this.Account = Song.Extend.LoginState.Accounts.CurrentUser;
                this.Teacher = Song.Extend.LoginState.Accounts.Teacher;
            }
            if (Extend.LoginState.Admin.IsLogin)
                this.Admin = Song.Extend.LoginState.Admin.CurrentUser;
        }
        public new bool IsReusable
        {
            get { return false; }
        }

        public new void ProcessRequest(HttpContext context)
        {
            string gourl = WeiSha.Common.Skip.GetUrl();   //跳转
            if (!string.IsNullOrWhiteSpace(gourl))
            {
                context.Response.Redirect(gourl);
                return;
            }
            //计算运算时间
            DateTime beforDT = System.DateTime.Now;

            this.InitContext(context);  //初始化页面参数
            //取缓存内容
            bool iscache = CachePage.Exist;
            if (iscache && !string.IsNullOrWhiteSpace(CachePage.Html))
            {
                context.Response.Write(CachePage.Html);
                return;
            }
            //输出数据
            this.LoadCurrentTemplate(); //装载当前页面的模板文档
            try
            {
                //一些公共对象
                this.Document.SetValue("org", this.Organ);
                this.Document.SetValue("orgpath", Upload.Get["Org"].Virtual);
                //学生、教师、管理员是否登录
                if (Extend.LoginState.Accounts.IsLogin)
                {
                    this.Document.SetValue("Account", this.Account);
                    this.Document.SetValue("stuid", Extend.LoginState.Accounts.UID);
                }
                if (Extend.LoginState.Accounts.IsLogin) this.Document.SetValue("Teacher", this.Teacher);
                if (Extend.LoginState.Admin.IsLogin) this.Document.SetValue("Admin", this.Admin);
                //各种路径
                this.Document.SetValue("stpath", Upload.Get["Accounts"].Virtual);
                this.Document.SetValue("thpath", Upload.Get["Teacher"].Virtual);
                this.Document.SetValue("adminpath", Upload.Get["Employee"].Virtual);
                //当前模板的路径
                this.Document.SetValue("TempPath", this.TmBank.Path.Virtual);
                //自定义配置项
                if (this.Organ != null)
                {
                    WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
                    //手机端隐藏关于“充值收费”等资费相关信息
                    bool IsMobileRemoveMoney = config["IsMobileRemoveMoney"].Value.Boolean ?? false;
                    this.Document.SetValue("mremove", IsMobileRemoveMoney);
                    //电脑端隐藏资费
                    bool IsWebRemoveMoney = config["IsWebRemoveMoney"].Value.Boolean ?? false;
                    this.Document.SetValue("wremove", IsWebRemoveMoney);
                }
            }
            catch { }
            //时间
            string WeekStr = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
            this.Document.SetValue("week", WeekStr);
            this.Document.SetValue("tick", DateTime.Now.Ticks);
            //系统版本号          
            this.Document.SetValue("version", version);

            //如果当前页面没有重构，则继续用原来的方法
            if (!Reconsi.Exist)
            {
                //导航菜单
                this.Document.RegisterGlobalFunction(this.Navi);
                this.Document.RegisterGlobalFunction(this.NaviDrop);
                //版权信息
                this.Document.SetValue("copyright", WeiSha.Common.Request.Copyright);
                //用本地模板引擎处理标签
                Song.Template.Handler.Start(this.Document);
            }



            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            if (ts.TotalMilliseconds >= 1)
            {
                WeiSha.Common.Log.Debug(this.GetType().FullName, string.Format("页面输出,耗时：{0}ms", ts.TotalMilliseconds));
            }
            //开始输出
            this.InitPageTemplate(context);
            if (iscache)
            {
                context.Response.Write(CachePage.Add(this.Document));
            }
            else
            {
                this.Document.Render(this.Response.Output);
            }
        }
        #endregion

        #region 导航菜单
        /// <summary>
        /// 获取导航菜单
        /// </summary>
        /// <param name="p">只用一个参数，为菜单类型</param>
        /// <returns></returns>
        protected Song.Entities.Navigation[] Navi(object[] p)
        {
            string type = null;
            if (p.Length > 0) type = p[0].ToString();
            int pid = 0;
            if (p.Length > 1) int.TryParse(p[1].ToString(), out pid);
            //是否是手机端网页
            string filePath = this.Request.Url.AbsolutePath;
            bool isMobi = isMobilePage(out filePath);
            string device = isMobi ? "mobi" : "web"; //设备
            Song.Entities.Navigation[] navi = Business.Do<IStyle>().NaviAll(true, device, type, Organ.Org_ID, pid);
            if (navi.Length < 1)
            {
                Song.Entities.Organization o = Business.Do<IOrganization>().OrganDefault();
                navi = Business.Do<IStyle>().NaviAll(true, device, type, o.Org_ID, pid);
            }
            return navi;
        }
        /// <summary>
        /// 获取导航菜单的下拉代码
        /// </summary>
        /// <param name="p">只用一个参数，为菜单类型</param>
        /// <returns></returns>
        protected string NaviDrop(object[] p)
        {
            string type = null;
            if (p.Length > 0) type = p[0].ToString();
            //是否是手机端网页
            string filePath = this.Request.Url.AbsolutePath;
            bool isMobi = isMobilePage(out filePath);
            string device = isMobi ? "mobi" : "web"; //设备
            Song.Entities.Navigation[] navi = Business.Do<IStyle>().NaviAll(true, device, type, Organ.Org_ID, 0);
            if (navi.Length < 1)
            {
                Song.Entities.Organization o = Business.Do<IOrganization>().OrganDefault();
                navi = Business.Do<IStyle>().NaviAll(true, device, type, o.Org_ID, -1);
            }
            string html = "";
            if (navi.Length > 0)
            {
                foreach (Song.Entities.Navigation n in navi)
                {
                    html += _NaviDropHtml(n.Nav_ID, type);
                }
            }
            return html;
        }
        private string _NaviDropHtml(int nid, string type)
        {
            string html = "";
            Song.Entities.Navigation[] navi = Business.Do<IStyle>().NaviChildren(nid, true);
            if (navi.Length > 0)
            {
                html += string.Format("<div pid=\"{0}\" class=\"naviBox\" style=\"display:none;\">", nid);
                foreach (Song.Entities.Navigation n in navi)
                {
                    html += string.Format("<div nid=\"{1}\" class=\"naviItem\"><a href=\"{3}\" target=\"{4}\" title=\"{5}\" style=\"{6}{7}{8}\">{2}</a></div>",
                        n.Nav_PID, n.Nav_ID, n.Nav_Name, n.Nav_Url, n.Nav_Target, n.Nav_Title,
                        string.IsNullOrEmpty(n.Nav_Color) ? "" : "color: " + n.Nav_Color + ";",
                        string.IsNullOrEmpty(n.Nav_Font) ? "" : "font-family: " + n.Nav_Font + ";",
                        !n.Nav_IsBold ? "" : "font-weight:bold;");
                }
                html += "</div>";
                foreach (Song.Entities.Navigation n in navi)
                    html += _NaviDropHtml(n.Nav_ID, type);
            }
            return html;
        }
        #endregion

    }
    /// <summary>
    /// 将要重构的文件判断
    /// </summary>
    public class Reconsi
    {
        /// <summary>
        /// 所有将要重构的页面
        /// </summary>
        public static List<string> Files
        {
            get
            {
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                if (cache == null) return null;
                //取缓存数据
                string cachekey = "Reconsitution_webpage";
                object cachevalue = cache.Get(cachekey);
                if (cachevalue != null) return (List<string>)cachevalue;
                //如果没有缓存，则读取并创建缓存
                string filepath = System.AppDomain.CurrentDomain.BaseDirectory + "Reconsitution.txt";
                List<string> _files = new List<string>();
                using (StreamReader sr = new StreamReader(filepath, Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                        if (!string.IsNullOrWhiteSpace(line)) _files.Add(line);
                    sr.Close();
                    cache.Insert(cachekey, _files, new System.Web.Caching.CacheDependency(filepath));
                }
                return _files;
            }
        }
        /// <summary>
        /// 是否存在重构
        /// </summary>
        public static bool Exist
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string path = context.Request.Url.AbsolutePath;
                List<string> _files = Files;
                foreach(string s in _files)
                {
                    if (path.Equals(s, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
    /// <summary>
    /// 缓存页面
    /// </summary>
    public class CachePage
    {
        /// <summary>
        /// 所有将要缓存的页面
        /// </summary>
        public static List<string> Files
        {
            get
            {
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                if (cache == null) return null;
                //取缓存数据
                string cachekey = "cache_page_webpage";
                object cachevalue = cache.Get(cachekey);
                if (cachevalue != null) return (List<string>)cachevalue;
                //如果没有缓存，则读取并创建缓存
                string filepath = System.AppDomain.CurrentDomain.BaseDirectory + "cache_page.txt";
                List<string> _files = new List<string>();
                using (StreamReader sr = new StreamReader(filepath, Encoding.Default))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                        if (!string.IsNullOrWhiteSpace(line)) _files.Add(line);
                    sr.Close();
                    cache.Insert(cachekey, _files, new System.Web.Caching.CacheDependency(filepath));
                }
                return _files;
            }
        }
        /// <summary>
        /// 是否存在重构
        /// </summary>
        public static bool Exist
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string path = context.Request.Url.AbsolutePath;
                List<string> _files = Files;
                foreach (string s in _files)
                {
                    if (path.Equals(s, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public static string Html
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string path = context.Request.Url.AbsolutePath;
                Song.Template.Cache_Item cache = Song.Template.Cache.Get(path);
                if (cache != null) return cache.Value;
                return string.Empty;
            }
        }
        public static void Add(string key,string val)
        {
            Song.Template.Cache.Add(key, val);
        }
        /// <summary>
        /// 页面内容写入缓存
        /// </summary>
        /// <param name="document"></param>
        /// <param name="key">缓存key值</param>
        /// <returns></returns>
        public static string Add(TemplateDocument document)
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string path = context.Request.Url.AbsolutePath;
            //如果没有缓存，则计算后输出
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            document.Render(writer);
            writer.Flush();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            Song.Template.Cache.Add(path, text);
            return text;
        }
    }
}
