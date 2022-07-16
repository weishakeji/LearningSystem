using System;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData
{
    /// <summary>
    /// 学员账号登录
    /// </summary>
    public class LoginAccount
    {
        private static readonly LoginAccount _singleton = new LoginAccount();
        //资源的虚拟路径和物理路径
        public static string VirPath = WeiSha.Core.Upload.Get["Accounts"].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get["Accounts"].Physics;
        //登录相关参数, 密钥，键，过期时间（分钟）
        public static string secretkey = WeiSha.Core.Login.Get["Accounts"].Secretkey.String;
        public static string keyname = WeiSha.Core.Login.Get["Accounts"].KeyName.String;
        public static int expires = WeiSha.Core.Login.Get["Accounts"].Expires.Int32 ?? 0;
        /// <summary>
        /// 当前单件对象
        /// </summary>
        public static LoginAccount Status
        {
            get { return _singleton; }
        }
        /// <summary>
        /// 当前登录的户
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public Song.Entities.Accounts User(Letter letter)
        {
            return LoginAccount.Current(letter);
        }
        /// <summary>
        /// 当前登录账号
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Accounts User()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Letter letter = new Letter(_context);
            return this.User(letter);
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool Login(Letter letter)
        {
            Song.Entities.Accounts acc = this.User(letter);
            return acc != null;
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Letter letter = new Letter(_context);
            return this.Login(letter);
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
        /// <param name="letter"></param>
        /// <returns></returns>
        public Song.Entities.Teacher Teacher(Letter letter)
        {
            Song.Entities.Accounts acc = this.User(letter);
            if (acc == null) return null;
            if (!acc.Ac_IsTeacher) return null;
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherForAccount(acc.Ac_ID);
            if (teacher != null)
            {
                if (System.IO.File.Exists(Upload.Get["Teacher"].Physics + teacher.Th_Photo))
                    teacher.Th_Photo = Upload.Get["Teacher"].Virtual + teacher.Th_Photo;
                else
                    teacher.Th_Photo = "";
            }
            return teacher;
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public void Logout()
        {
            Song.Entities.Accounts acc = this.User();
            if (acc == null) return;
            Business.Do<IAccounts>().RecordLoginCode(acc.Ac_ID, string.Empty);           
        }
        /// <summary>
        /// 刷新登录状态
        /// </summary>
        public string Fresh(Letter letter)
        {
            Song.Entities.Accounts acc = this.User(letter);
            if (acc == null) return string.Empty;
            string code = Generate_checkcode(acc);
            acc.Ac_CheckUID = code;
            LoginAccount.Fresh(acc);
            return code;
        }
        /// <summary>
        /// 生成登录校验码
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="uid">登录校验码</param>
        /// <returns></returns>
        public string Generate_checkcode(Song.Entities.Accounts acc)
        {
            int accid = acc.Ac_ID;
            string uid = acc.Ac_CheckUID;
            //校验码,依次为：标识,id,角色,时效,识别码
            string checkcode = "{0},{1},{2},{3},{4}";
            string role = "student";      //角色
            //时效
            DateTime exp = DateTime.Now.AddMinutes(expires > 0 ? expires : 10);
            checkcode = string.Format(checkcode, keyname, accid, role, exp.ToString("yyyy-MM-dd HH:mm:ss"), uid);
            //加密         
            checkcode = WeiSha.Core.DataConvert.EncryptForDES(checkcode, secretkey);
            return checkcode;
        }

        #region 内存数据管理
        /// <summary>
        /// 当前登录的学员，记录在内存，方便查询登录状态
        /// </summary>
        public static readonly List<Song.Entities.Accounts> list = new List<Entities.Accounts>();
        private static object _lock_list = new object();
        /// <summary>
        /// 添加在线学员
        /// </summary>
        /// <param name="acc"></param>
        public static List<Song.Entities.Accounts> Add(Song.Entities.Accounts acc)
        {
            lock (_lock_list)
            {
                bool isexsit = Fresh(acc);
                if (!isexsit) list.Add(acc);
                return list;
            }            
        }
        /// <summary>
        /// 刷新内存中的登录状态
        /// </summary>
        /// <param name="acc"></param>
        /// <returns>为true表示已经刷新（数据存在）,为false表示未刷新（数据不存在）</returns>
        public static bool Fresh(Song.Entities.Accounts acc)
        {
            lock (_lock_list)
            {
                bool isexsit = false;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Ac_ID == acc.Ac_ID)
                    {
                        list[i] = acc;
                        isexsit = true;
                        //Business.Do<IAccounts>().RecordLoginCode(acc.Ac_ID, acc.Ac_CheckUID);
                        break;
                    }
                }
                return isexsit;
            }
        }
        /// <summary>
        /// 获取内存中的当前登录账号
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        public static Song.Entities.Accounts GetCurrent(int acid)
        {
            Song.Entities.Accounts acc = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Ac_ID == acid)
                {
                    acc = list[i];
                    break;
                }
            }
            return acc;
        }
        /// <summary>
        /// 返回当前登录用户的实体
        /// </summary>
        /// <param name="letter">客户端传来的消息对象</param>
        /// <returns></returns>
        public static Song.Entities.Accounts Current(Letter letter)
        {
            if (letter.LoginStatus == null || letter.LoginStatus.Length < 1) return null;
            //解析状态码
            //string domain = WeiSha.Core.Server.Domain;
            //string key = WeiSha.Core.Login.Get["Accounts"].KeyName.String;   //标识,来自web.config配置
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
            //从内存中获取
            Song.Entities.Accounts acc = LoginAccount.GetCurrent(accid);
            //从数据库获取
            //Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(accid);
            if (acc == null)
                acc = Business.Do<IAccounts>().AccountsSingle(accid);

            if (acc == null || string.IsNullOrWhiteSpace(acc.Ac_CheckUID) || !acc.Ac_CheckUID.Equals(status[4])) return null;
            acc = acc.DeepClone<Song.Entities.Accounts>();
            acc.Ac_Photo = System.IO.File.Exists(PhyPath + acc.Ac_Photo) ? VirPath + acc.Ac_Photo : "";
            acc.Ac_Pw = string.Empty;
            return acc;
        }
        #endregion
    }
}
