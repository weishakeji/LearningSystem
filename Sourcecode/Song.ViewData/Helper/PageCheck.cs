using Song.Entities;
using Song.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WeiSha.Core;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// 页面访问校验
    /// </summary>
    public class PageCheck
    {

        private static string configPath = "/Utilities/PageCheck";
        private static string configFile = "Config.weisha";
        //       
        private static readonly PageCheck _instance = new PageCheck();
        /// <summary>
        /// 单例对象
        /// </summary>
        public static PageCheck Instance => _instance;

        /// <summary>
        /// 不受限制的页面项，key值为模板库名称，value为页面列表
        /// </summary>
        public Dictionary<string, List<string>> Items
        {
            get
            {
                Dictionary<string, List<string>> dic;
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                object cachevalue = cache.Get(configFile + "Items");
                if (cachevalue != null) dic = (Dictionary<string, List<string>>)cachevalue;
                else dic = this.InitializedData();
                return dic;
            }
        }
        /// <summary>
        /// 不受限制的页面项，支持正则表达式
        /// </summary>
        public List<string> Allows
        {
            get
            {
                List<string> list;
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                object cachevalue = cache.Get(configFile + "Allows");
                if (cachevalue != null) list = (List<string>)cachevalue;
                else list = this.InitializedAllow();
                return list;
            }
        }
        private PageCheck()
        {
            configPath = WeiSha.Core.Server.MapPath(configPath);
            Business.Do<IManageMenu>().OnChanged += (object sender, EventArgs e) => this.InitializedMenu();
            Business.Do<IPurview>().OnChanged += (object sender, EventArgs e) => this.InitializedMenu();
            this.InitializedData();
        } 
        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<string>> InitializedData()
        {
            lock (this)
            {
                Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
                XmlDocument xmldoc = new XmlDocument();
                if (!File.Exists(configPath + configFile)) return dic;
                xmldoc.Load(configPath + configFile);
                //需要权限管控的路由，即根节点配置项
                XmlNodeList nodes = xmldoc.LastChild.FirstChild.ChildNodes;
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];
                    string key = node.Name;
                    List<string> list = new List<string>();
                    //不需要权限管理的页面
                    XmlNodeList childs = node.ChildNodes;
                    for (int j = 0; j < childs.Count; j++)
                    {
                        string page = childs[j].Attributes["value"].Value;
                        if (!string.IsNullOrWhiteSpace(page))
                        {
                            if (page.EndsWith("/") || page.EndsWith("\\")) page = page.Substring(0, page.Length - 1);
                            list.Add(page.ToLower());
                        }
                    }
                    dic.Add(key, list);
                }
                //加入缓存
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                cache.Insert(configFile + "Items", dic, new System.Web.Caching.CacheDependency(configPath + configFile));
                return dic;
            }
        }
        /// <summary>
        /// 初始化允许通过的路由
        /// </summary>
        /// <returns></returns>
        public List<string> InitializedAllow()
        {
            lock (this)
            {
                List<string> list = new List<string>();
                XmlDocument xmldoc = new XmlDocument();
                if (!File.Exists(configPath + configFile)) return list;
                xmldoc.Load(configPath + configFile);
                //需要权限管控的路由，即根节点配置项
                XmlNodeList nodes = xmldoc.LastChild.SelectNodes("allow/item");
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];
                    if (node.NodeType == XmlNodeType.Element)
                        list.Add(node.Attributes["value"].Value);
                }
                //加入缓存
                System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
                cache.Insert(configFile + "Allows", list, new System.Web.Caching.CacheDependency(configPath + configFile));
                return list;
            }
        }
        /// <summary>
        /// 获取某个路由下所有不受限制的页面
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> PageList(string key)
        {
            Dictionary<string, List<string>> items = this.Items;
            return items != null && items.ContainsKey(key) ? items[key] : null;
        }
        /// <summary>
        /// 菜单项
        /// </summary>
        private static Dictionary<string ,Dictionary<int, HashSet<string>>> menu = null;               
        private static object lockObj = new object();
        /// <summary>
        /// 初始化权限菜单项
        /// </summary>
        public void InitializedMenu()
        {
            lock (lockObj)
            {
                menu = new Dictionary<string, Dictionary<int, HashSet<string>>>();

                List<Organization> orgs = Business.Do<IOrganization>().OrganAll(null, -1, string.Empty);
                string[] keys = { "organAdmin", "student", "teacher", "manage" };
                //取各角色、各机构的菜单项
                for (int i = 0; i < keys.Length - 1; i++)
                {
                    Dictionary<int, HashSet<string>> dic = new Dictionary<int, HashSet<string>>();
                    foreach (Organization org in orgs)
                    {
                        List<ManageMenu> mms = Business.Do<IPurview>().GetOrganPurview(org, keys[i]);
                        HashSet<string> hset = new HashSet<string>();
                        for (int j = 0; j < mms.Count; j++)
                        {
                            if (string.IsNullOrWhiteSpace(mms[j].MM_Link) || mms[j].MM_Link.StartsWith("http"))
                                continue;
                            hset.Add(mms[j].MM_Link.ToLower());
                        }
                        dic.Add(org.Org_ID, hset);
                    }
                    menu.Add(keys[i], dic);
                }
                {
                    Dictionary<int, HashSet<string>> dic = new Dictionary<int, HashSet<string>>();
                    HashSet<string> hset = new HashSet<string>();
                    //超管菜单
                    foreach (ManageMenu m in Business.Do<IManageMenu>().GetFunctionMenu("0", true, true))
                    {
                        if (string.IsNullOrWhiteSpace(m.MM_Link) || m.MM_Link.StartsWith("http")) continue;
                        hset.Add(m.MM_Link.ToLower());
                    }
                    //系统菜单
                    foreach (ManageMenu m in Business.Do<IManageMenu>().GetAll(true, true, "sys"))
                    {
                        if (string.IsNullOrWhiteSpace(m.MM_Link) || m.MM_Link.StartsWith("http")) continue;
                        hset.Add(m.MM_Link.ToLower());
                    }
                    dic.Add(0, hset);
                    menu.Add("manage", dic);
                }
            }
        }
        /// <summary>
        /// 某一类角色的菜单项，包括各个机构的
        /// </summary>
        /// <param name="key">角色名称</param>
        /// <returns></returns>
        public Dictionary<int, HashSet<string>> Menus(string key)
        {
            if (menu == null) this.InitializedMenu();
            if (key.Equals("orgadmin")) return menu["organAdmin"];
            foreach (string str in menu.Keys)          
                if(String.Equals(str, key, StringComparison.OrdinalIgnoreCase))                      
                    return menu[str];           
            return null;          
        }
        /// <summary>
        /// 某一角色在所在机构的菜单项
        /// </summary>
        /// <param name="key">角色名称</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public HashSet<string> Menus(string key,int orgid)
        {
            Dictionary<int, HashSet<string>> dic = this.Menus(key);
            if (dic == null) return null;
            return dic.ContainsKey(orgid) ? dic[orgid] : null;
        }
        #region 校验
        /// <summary>
        /// 检测API请求的所在页面是否拥有权限
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public bool CheckPageAccess(Letter letter)
        {
            string pagepath = letter.WEB_PAGE;
            if (string.IsNullOrWhiteSpace(pagepath)) return false;
            if (pagepath == "/") return true;
            string[] pages = pagepath.Split(',');
            if (pages.Length < 1) return false;
            for (int i = 0; i < pages.Length; i++)
                if (pages[i].EndsWith("/")) pages[i] = pages[i].Substring(0, pages[i].Length - 1);
            string root = pages[pages.Length - 1];  //根页面，一般是管理后台的地址，如果不处理管理框架内，则是自身
            string self = pages[0];     //自身页面
            string purview = pages.Length >= 2 ? pages[pages.Length - 2] : root;    //需要判断权限的页面
            //页面所在控制器路由，在当前系统框架中，往往用于描述为“设备(device)”
            string controller = root.Split('/').FirstOrDefault(s => !string.IsNullOrWhiteSpace(s)); //模板库的类型

            //判断是否拥有权限
            bool ispass = this.CheckPageAccess(purview, controller, letter);
            string msg = string.Format("当前页面 {0} 没有操作权限", self);
            if (!ispass) throw VExcept.Verify(msg, 100);
            return true;
        }
        /// <summary>
        /// 检测页面是否拥有权限
        /// </summary>
        /// <param name="page">用于权限判断的页面</param>
        /// <param name="device">页面所在控制器路由，在当前系统框架中，往往用于描述为“设备(device)”</param>
        /// <param name="letter"></param>
        /// <returns></returns>
        public bool CheckPageAccess(string page, string device, Letter letter)
        {
            //如果不是模板库限制的页面
            List<string> list = this.PageList(device);
            if (list == null || list.Contains(page)) return true;
            //模板库之外的不限制页面
            foreach(string s in this.Allows)           
                if (Regex.IsMatch(page, s)) return true;
            //
            HashSet<string> menus = null;          
            Song.Entities.Organization org = null;
            //根据登录状态判断权限
            switch (device)
            {
                case "orgadmin":
                    //当前机构管理员
                    EmpAccount emp = LoginAdmin.Status.User(letter);
                    if (emp != null) org = Business.Do<IOrganization>().OrganSingle(emp.Org_ID);
                    break;
                case "student":
                    Accounts acc = LoginAccount.Status.User(letter);
                    if (acc != null) org = Business.Do<IOrganization>().OrganSingle(acc.Org_ID);
                    break;
                case "teacher":
                    Teacher th = LoginAccount.Status.Teacher(letter);
                    if (th != null) org = Business.Do<IOrganization>().OrganSingle(th.Org_ID);
                    break;
                case "manage":
                    org = new Organization();
                    break;
                default:
                    return true;
            }
            if (org == null) return false;
            menus = Menus(device, org.Org_ID);
            if (menus == null) return false;
            return menus.Contains(page);
        }
        #endregion
    }
}
