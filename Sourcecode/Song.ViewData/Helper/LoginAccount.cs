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
    /// 学员账号登录
    /// </summary>
    public class LoginAccount
    {
        private static readonly LoginAccount _singleton = new LoginAccount();
        //资源的虚拟路径和物理路径
        private static string virPath = WeiSha.Core.Upload.Get["Accounts"].Virtual;
        private static string phyPath = WeiSha.Core.Upload.Get["Accounts"].Physics;
        //登录相关参数, 密钥，键，过期时间（分钟）
        private static string secretkey = WeiSha.Core.Login.Get["Accounts"].Secretkey.String;
        private static string keyname = WeiSha.Core.Login.Get["Accounts"].KeyName.String;
        private static int expires = WeiSha.Core.Login.Get["Accounts"].Expires.Int32 ?? 0;
        private Timer timer;
        private LoginAccount()
        {
            // 每隔60分钟触发一次事件
            timer = new Timer(state => LoginAccount.CacheClearExpired(), null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
        }
        /// <summary>
        /// 当前单件对象
        /// </summary>
        public static LoginAccount Status => _singleton;
        /// <summary>
        /// 当前登录的管理员账号
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Accounts User()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Letter letter = Letter.Constructor(_context);
            return this.User(letter);
        }
        /// <summary>
        /// 当前登录账号
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Accounts User(Letter letter)
        {
            if (letter.LoginStatus == null || letter.LoginStatus.Length < 1) return null;
            //解析状态码
            //string key = WeiSha.Core.Login.Get["Accounts"].KeyName.String;   //标识,来自web.config配置
            string[] status = null;
            foreach (string s in letter.LoginStatus)
            {
                if (!s.StartsWith(keyname)) continue;
                string str = WeiSha.Core.DataConvert.DecryptForDES(s.Substring(keyname.Length), secretkey);
                if (string.IsNullOrWhiteSpace(str)) continue;
                //解析后信息,依次为：id,角色,时效,识别码,useragent
                status = str.Split(',');
            }
            //判断时效
            if (status == null || status.Length < 2) return null;
            DateTime exp = Convert.ToDateTime(status[2]);
            if (exp < DateTime.Now) return null;
            //判断浏览器userAgent
            string ua = letter.HTTP_HOST;
            if (status == null || status.Length < 4) return null;
            if (!ua.Equals(status[4])) return null;

            //判断登录id
            int accid = Convert.ToInt32(status[0]);
            //从内存中获取
            Song.Entities.Accounts acc = LoginAccount.CacheGet(accid);
            //从数据库获取
            //Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(accid);
            if (acc == null) acc = LoginAccount.CacheGet(accid);
            if (acc == null || string.IsNullOrWhiteSpace(acc.Ac_CheckUID) || !acc.Ac_CheckUID.Equals(status[3])) return null;
            acc = acc.DeepClone<Song.Entities.Accounts>();
            if(!string.IsNullOrWhiteSpace(acc.Ac_Photo) && !acc.Ac_Photo.StartsWith("/"))
                acc.Ac_Photo = System.IO.File.Exists(phyPath + acc.Ac_Photo) ? virPath + acc.Ac_Photo : "";
            acc.Ac_Pw = string.Empty;
            return acc;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">当前账号</param>
        /// <returns></returns>
        public string Login(Accounts acc)
        {
            //识别码，记录到数据库
            //string uid = WeiSha.Core.Request.UniqueID();
            //acc.Ac_CheckUID = uid;
            LoginAccount.CacheAdd(acc);
            //异步存储
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IAccounts>().RecordLoginCode(acc.Ac_ID, acc.Ac_CheckUID);
            }).Start();

            //返回加密状态码
            return Generate_checkcode(acc);
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool IsLogin(Letter letter)
        {
            Song.Entities.Accounts acc = this.User(letter);
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
        /// 当前登录对象所在的机构
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public Song.Entities.Organization Organ(Letter letter)
        {
            Song.Entities.Accounts acc = this.User(letter);
            if (acc == null) return null;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(acc.Org_ID);
            return org;
        }
        /// <summary>
        /// 当前登录的教师
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Teacher Teacher()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Letter letter = Letter.Constructor(_context);
            return this.Teacher(letter);
        }
        /// <summary>
        /// 当前登录的教师
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public Song.Entities.Teacher Teacher(Letter letter)
        {
            Song.Entities.Accounts acc = this.User(letter);
            if (acc == null) return null;
            if (!acc.Ac_IsTeacher) return null;
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherForAccount(acc.Ac_ID);
            if (teacher != null && teacher.Th_IsPass && teacher.Th_IsUse)
            {
                if (System.IO.File.Exists(WeiSha.Core.Upload.Get["Teacher"].Physics + teacher.Th_Photo))
                    teacher.Th_Photo = WeiSha.Core.Upload.Get["Teacher"].Virtual + teacher.Th_Photo;
                else
                    teacher.Th_Photo = "";
                return teacher;
            }
            return null;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            Song.Entities.Accounts acc = this.User();
            if (acc == null) return;
            //异步存储
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IAccounts>().RecordLoginCode(acc.Ac_ID, string.Empty);
            }).Start();               
        }
        /// <summary>
        /// 刷新登录状态
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public string Fresh(Song.Entities.Accounts acc)
        {
            if (acc == null) return string.Empty;
            LoginAccount.CacheAdd(acc);
            return Generate_checkcode(acc);
        }
        /// <summary>
        /// 刷新登录状态
        /// </summary>
        /// <param name="letter"></param>
        /// <param name="span">每次刷新的时间间隔，单位秒</param>
        /// <returns>返回新的token，登录验证的Ac_CheckUID并没有改变</returns>
        public string Fresh(Letter letter,int span)
        {
            Song.Entities.Accounts acc = this.User(letter);
            if (acc == null) return string.Empty;
            //添加登录记录
            Business.Do<IStudent>().LogForLoginFresh(acc, span, string.Empty);
            //生成新的token
            string code = Generate_checkcode(acc, letter);
            //暂时注释掉，不知道是否有影响，按道理Ac_CheckUID不应该改变
            //acc.Ac_CheckUID = code;
            //bool isexist = LoginAccount.Status.Fresh(acc);
            return code;
        }
        /// <summary>
        /// 生成登录校验码
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public string Generate_checkcode(Song.Entities.Accounts acc)
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Letter letter = Letter.Constructor(_context);
            return Generate_checkcode(acc, letter);
        }
        /// <summary>
        /// 生成登录校验码
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        public string Generate_checkcode(Song.Entities.Accounts acc, Letter letter)
        {
            int accid = acc.Ac_ID;
            string uid = acc.Ac_CheckUID;
            //校验码,依次为：id,角色,时效,识别码
            string checkcode = "{0},{1},{2},{3},{4}";
            string role = "student";      //角色
            //时效
            DateTime exp = DateTime.Now.AddMinutes(expires > 0 ? expires : 10);
            //userAgent
            string ua = letter.HTTP_HOST;    
            checkcode = string.Format(checkcode, accid, role, exp.ToString("yyyy-MM-dd HH:mm:ss"), uid, ua);
            //加密         
            checkcode = WeiSha.Core.DataConvert.EncryptForDES(checkcode, secretkey);
            return keyname + checkcode;
        }

        #region 内存数据管理
        private static object _lock_list = new object();
        private static readonly string _cachaName = "LoginAccount_Accounts_List";
        /// <summary>
        /// 缓存中的列表数据
        /// </summary>
        /// <returns></returns>
        public static List<Accounts> CacheList()
        {
            MemoryCache cache = MemoryCache.Default;
            List<Accounts> list = cache.Get(_cachaName) as List<Accounts>;
            if (list == null) list = new List<Accounts>();
            return list;
        }
        private static List<Accounts> CacheUpdate(List<Accounts> list)
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
        public static void CacheAdd(Accounts acc)
        {

            if (acc == null) return;
            acc.Ac_LastTime = DateTime.Now;
            lock (_lock_list)
            {
                List<Accounts> list = CacheList();
                int idx = list.FindIndex(x => x.Ac_ID == acc.Ac_ID);
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
            List<Accounts> list = CacheList();
            int idx = list.FindIndex(x => x.Ac_ID == id);
            if (idx >= 0) list.RemoveAt(idx);
            CacheUpdate(list);
        }
        /// <summary>
        /// 获取内存中的当前登录账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Song.Entities.Accounts CacheGet(int id)
        {
            List<Accounts> list = CacheList();
            Accounts acc = list.Find(x => x.Ac_ID == id);
            if (acc == null)
            {
                acc = Business.Do<IAccounts>().AccountsSingle(id);
                CacheAdd(acc);
            }
            if (acc == null) return null;
            return acc.Ac_IsUse && acc.Ac_IsPass ? acc : null;
        }
        /// <summary>
        /// 清理过期
        /// </summary>
        public static void CacheClearExpired()
        {
            lock (_lock_list)
            {
                List<Accounts> list = CacheList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Ac_LastTime > DateTime.Now.AddMinutes(-60))
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
