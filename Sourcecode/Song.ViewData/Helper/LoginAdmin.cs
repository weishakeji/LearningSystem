using System;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Song.ViewData
{
    /// <summary>
    /// 管理员账号登录
    /// </summary>
    public class LoginAdmin
    {
        private static readonly LoginAdmin _singleton = new LoginAdmin();
        //资源的虚拟路径和物理路径
        private string virPath = WeiSha.Core.Upload.Get["Admin"].Virtual;
        private string phyPath = WeiSha.Core.Upload.Get["Accounts"].Physics;
        //登录相关参数, 密钥，键，过期时间（分钟）
        private string secretkey = WeiSha.Core.Login.Get["Admin"].Secretkey.String;
        private string keyname = WeiSha.Core.Login.Get["Admin"].KeyName.String;
        private int expires = WeiSha.Core.Login.Get["Admin"].Expires.Int32 ?? 0;

        private Timer timer;
        private LoginAdmin()
        {
            // 每隔60分钟触发一次事件
            timer = new Timer(state => LoginAdmin.CacheClearExpired(), null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
        }

        /// <summary>
        /// 当前单件对象
        /// </summary>
        public static LoginAdmin Status => _singleton;
        /// <summary>
        /// 返回当前登录用户的实体
        /// </summary>
        /// <param name="letter">客户端传来的消息对象</param>
        /// <returns></returns>
        public Song.Entities.EmpAccount User(Letter letter)
        {
            if (letter.LoginStatus == null || letter.LoginStatus.Length < 1) return null;
            //解析状态码
            string[] status = null;
            foreach (string s in letter.LoginStatus)
            {
                string str = WeiSha.Core.DataConvert.DecryptForDES(s, secretkey);
                if (string.IsNullOrWhiteSpace(str)) continue;
                //解析后信息,依次为：标识,id,角色,时效,识别码
                string[] arr = str.Split(',');
                if (arr[0].Equals(keyname, StringComparison.CurrentCultureIgnoreCase))
                {
                    status = arr;
                    break;
                }
            }
            //判断时效
            if (status == null || status.Length < 3) return null;
            DateTime time = Convert.ToDateTime(status[3]);
            if (time < DateTime.Now) return null;
            //判断登录码
            int accid = Convert.ToInt32(status[1]);
            Song.Entities.EmpAccount acc = LoginAdmin.CacheGet(accid);
            if (acc == null || string.IsNullOrWhiteSpace(acc.Acc_CheckUID) || !acc.Acc_CheckUID.Equals(status[4])) return null;
            acc = acc.DeepClone<Song.Entities.EmpAccount>();
            if (!string.IsNullOrWhiteSpace(acc.Acc_Photo))
            {
                acc.Acc_Photo = string.IsNullOrWhiteSpace(acc.Acc_Photo) ? "" : WeiSha.Core.Upload.Get["Employee"].Virtual + acc.Acc_Photo;
                if (!System.IO.File.Exists(WeiSha.Core.Server.MapPath(acc.Acc_Photo))) acc.Acc_Photo = "";
            }
            acc.Acc_Pw = string.Empty;
            return acc;
        }
        /// <summary>
        /// 当前登录的管理员账号
        /// </summary>
        /// <returns></returns>
        public Song.Entities.EmpAccount User()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Letter letter = Letter.Constructor(_context);
            return this.User(letter);
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool IsLogin(Letter letter)
        {
            Song.Entities.EmpAccount acc = this.User(letter);
            return acc != null;
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool IsLogin()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Letter letter = Letter.Constructor(_context);
            return this.IsLogin(letter);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">当前账号</param>
        /// <returns></returns>
        public string Login(Song.Entities.EmpAccount acc)
        {                     
            //识别码，记录到数据库
            string uid = WeiSha.Core.Request.UniqueID();
            acc.Acc_CheckUID = uid;
            LoginAdmin.CacheAdd(acc);
            //异步存储
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IEmployee>().RecordLoginCode(acc.Acc_Id, uid);
            }).Start();

            //返回加密状态码
            return _generate_checkcode(acc.Acc_Id, uid);
        }
        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public bool IsSuperAdmin(Letter letter)
        {
            Song.Entities.EmpAccount acc = this.User(letter);
            if (acc == null) return false;
            return IsSuperAdmin(acc);
        }
        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool IsSuperAdmin(Song.Entities.EmpAccount acc)
        {
            if (acc == null) return false;
            //是否来自根机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(acc.Org_ID);
            if (org == null || !org.Org_IsRoot) return false;
            //当前登录对象的岗位
            Song.Entities.Position posi = Business.Do<IPosition>().GetSingle((int)acc.Posi_Id);
            if (posi == null) return false;
            return (bool)posi.Posi_IsAdmin;
        }
        /// <summary>
        /// 当前登录对象所在的机构
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public Song.Entities.Organization Organ(Letter letter)
        {
            Song.Entities.EmpAccount acc = this.User(letter);
            if (acc == null) return null;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(acc.Org_ID);
            return org;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            Song.Entities.EmpAccount acc = this.User();
            if (acc == null) return;
            Business.Do<IEmployee>().RecordLoginCode(acc.Acc_Id, string.Empty);           
        }
        /// <summary>
        /// 刷新登录状态
        /// </summary>
        public string Fresh(Letter letter)
        {
            Song.Entities.EmpAccount acc = this.User(letter);
            return this.Fresh(acc);
        }
        /// <summary>
        /// 刷新登录状态
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public string Fresh(Song.Entities.EmpAccount acc)
        {
            if (acc == null) return string.Empty;
            LoginAdmin.CacheAdd(acc);
            return _generate_checkcode(acc.Acc_Id, acc.Acc_CheckUID);           
        }
        /// <summary>
        /// 生成登录校验码
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="uid">登录校验码</param>
        /// <returns></returns>
        private string _generate_checkcode(int accid,string uid)
        {
            //校验码,依次为：标识,id,角色,时效,识别码
            string checkcode = "{0},{1},{2},{3},{4}";
            string role = "admin";      //角色
            //时效
            DateTime exp = DateTime.Now.AddMinutes(expires > 0 ? expires : 10);
            checkcode = string.Format(checkcode, keyname, accid, role, exp.ToString("yyyy-MM-dd HH:mm:ss"), uid);
            //加密         
            checkcode = WeiSha.Core.DataConvert.EncryptForDES(checkcode, secretkey);
            return checkcode;
        }

        #region 内存数据管理
        private static object _lock_list = new object();        
        private static readonly string _cachaName= "LoginAdmin_EmpAccount_List";
        /// <summary>
        /// 缓存中的列表数据
        /// </summary>
        /// <returns></returns>
        private static List<EmpAccount> CacheList()
        {
            MemoryCache cache = MemoryCache.Default;
            List<EmpAccount> list = cache.Get(_cachaName) as List<EmpAccount>;
            if (list == null) list = new List<EmpAccount>();
            return list;
        }
        private static List<EmpAccount> CacheUpdate(List<EmpAccount> list)
        {
            MemoryCache cache = MemoryCache.Default;
            if (cache.Contains(_cachaName)) cache.Remove(_cachaName);
            cache.Add(_cachaName, list, DateTimeOffset.UtcNow.AddYears(2));        
            return list;
        }
        /// <summary>
        /// 添加在线账号
        /// </summary>
        /// <param name="acc"></param>
        public static void CacheAdd(EmpAccount acc)
        {
           
            if (acc == null) return;
            acc.Acc_LastTime = DateTime.Now;
            lock (_lock_list)
            {
                List<EmpAccount> list = CacheList();
                int idx = list.FindIndex(x => x.Acc_Id == acc.Acc_Id);
                if (idx >= 0) list[idx] = acc;
                else list.Add(acc);
                CacheUpdate(list);
            }
        }
        /// <summary>
        /// 清除掉某个登录账号
        /// </summary>
        /// <param name="id"></param>
        public static void CacheRemove(int id)
        {
            List<EmpAccount> list = CacheList();
            int idx = list.FindIndex(x => x.Acc_Id == id);
            if (idx >= 0) list.RemoveAt(idx);
            CacheUpdate(list);
        }
        /// <summary>
        /// 获取内存中的当前登录账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Song.Entities.EmpAccount CacheGet(int id)
        {
            List<EmpAccount> list = CacheList();
            EmpAccount acc = list.Find(x => x.Acc_Id == id);
            if (acc == null)
            {
                acc = Business.Do<IEmployee>().GetSingle(id);
                CacheAdd(acc);
            }
            return acc.Acc_IsUse ? acc : null;
        }
        /// <summary>
        /// 清理过期
        /// </summary>
        public static void CacheClearExpired()
        {
            lock (_lock_list)
            {
                List<EmpAccount> list = CacheList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Acc_LastTime > DateTime.Now.AddMinutes(-60))
                    {
                        list.RemoveAt(i);
                        i--;
                    }
                }
                CacheUpdate(list);
            }
        }
        #endregion
    }
}
