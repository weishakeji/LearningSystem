using Song.Entities;
using Song.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                object cachevalue = cache.Get(configFile);
                if (cachevalue != null) dic = (Dictionary<string, List<string>>)cachevalue;
                else
                    dic = this.InitializedData();
                return dic;
            }
        }
        private PageCheck()
        {
            configPath = WeiSha.Core.Server.MapPath(configPath);
            Business.Do<IManageMenu>().OnChanged += PageCheck_Changed;
            Business.Do<IPurview>().OnChanged += PageCheck_Changed;
            this.InitializedData();
        }
        /// <summary>
        /// 当管理菜单或权限变更时，刷新菜单信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageCheck_Changed(object sender, EventArgs e)
        {
            this.InitializedMenu();
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
                XmlNodeList nodes = xmldoc.LastChild.ChildNodes;
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
                cache.Insert(configFile, dic, new System.Web.Caching.CacheDependency(configPath + configFile));
                return dic;
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
        private static Dictionary<string ,Dictionary<int, List<ManageMenu>>> menu = null;               
        private static object lockObj = new object();
        /// <summary>
        /// 初始化权限菜单项
        /// </summary>
        public void InitializedMenu()
        {
            lock (lockObj)
            {
                menu = new Dictionary<string, Dictionary<int, List<ManageMenu>>>();

                List<Organization> orgs = Business.Do<IOrganization>().OrganAll(null, -1, string.Empty);
                string[] keys = { "organAdmin", "student", "teacher", "manage" };
                //取各角色、各机构的菜单项
                for (int i = 0; i < keys.Length - 1; i++)
                {
                    Dictionary<int, List<ManageMenu>> dic = new Dictionary<int, List<ManageMenu>>();
                    foreach (Organization org in orgs)
                    {
                        List<ManageMenu> mms = Business.Do<IPurview>().GetOrganPurview(org, keys[i]);
                        dic.Add(org.Org_ID, mms);
                    }
                    menu.Add(keys[i], dic);
                }
                {
                    //超管菜单
                    List<Song.Entities.ManageMenu> list = Business.Do<IManageMenu>().GetFunctionMenu("0", true, true);
                    //系统菜单
                    foreach (ManageMenu m in
                    Business.Do<IManageMenu>().GetAll(true, true, "sys"))
                    {
                        if (!list.Exists(m2 => m2.MM_Id == m.MM_Id)) list.Add(m);
                    }
                    Dictionary<int, List<ManageMenu>> dic = new Dictionary<int, List<ManageMenu>>();
                    dic.Add(0, list);
                    menu.Add("manage", dic);
                }
            }
        }
        /// <summary>
        /// 某一类角色的菜单项，包括各个机构的
        /// </summary>
        /// <param name="key">角色名称</param>
        /// <returns></returns>
        public Dictionary<int, List<ManageMenu>> Menus(string key)
        {
            if (menu == null) this.InitializedMenu();
            if (key.Equals("orgadmin")) return menu["organAdmin"];
            foreach (string str in menu.Keys)
            {
                if(String.Equals(str, key, StringComparison.OrdinalIgnoreCase))                      
                    return menu[str];
            }
            return null;          
        }
        /// <summary>
        /// 某一角色在所在机构的菜单项
        /// </summary>
        /// <param name="key">角色名称</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public List<ManageMenu> Menus(string key,int orgid)
        {
            Dictionary<int, List<ManageMenu>> dic = this.Menus(key);
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
            //如果不是限制的页面
            List<string> list = this.PageList(device);
            if (list == null || list.Contains(page)) return true;
            //
            List<ManageMenu> menus = null;          
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
            foreach (Song.Entities.ManageMenu item in menus)
                if (!string.IsNullOrWhiteSpace(item.MM_Link) && item.MM_Link.ToLower() == page) return true;
            return false;
        }
        #endregion
    }
}
