using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;
using Song.Entities;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// 管理员登录状态的管理对象
    /// </summary>
    public class Admin : IRequiresSessionState
    {
        private static readonly Admin _singleton = new Admin();
        /// <summary>
        /// 获取参数
        /// </summary>
        public static Admin Singleton
        {
            get { return _singleton; }
        }
        #region 属性

        /// <summary>
        /// 后台管理登录的状态管理方式，值为Cookies或Session
        /// </summary>
        public static LoginPatternEnum LoginPattern
        {
            get
            {
                string tm = WeiSha.Common.Login.Get["Admin"].Pattern.String;
                if (tm.Equals("Session", StringComparison.CurrentCultureIgnoreCase)) return LoginPatternEnum.Session;
                if (tm.Equals("Cookies", StringComparison.CurrentCultureIgnoreCase)) return LoginPatternEnum.Cookies;
                return LoginPatternEnum.Cookies;
            }
        }
        private List<Song.Entities.EmpAccount> _onlineUser = new List<EmpAccount>();
        /// <summary>
        /// 在线用户
        /// </summary>
        public List<Song.Entities.EmpAccount> OnlineUser
        {
            get { return _onlineUser; }
        }
        
        
        #region 属性
        /// <summary>
        /// 用于存储Cookie或Session的key
        /// </summary>
        public string Key
        {
            get
            {
                string twoDomain = WeiSha.Common.Request.Domain.TwoDomain;
                //登录标识名
                string key = WeiSha.Common.Login.Get["Admin"].KeyName.String;
                key = twoDomain + "_" + key;
                return key;
            }
        }
        /// <summary>
        /// 存储于Cookie或Session的值
        /// </summary>
        public string Value
        {
            get
            {
                string encrypt = string.Empty;
                //登录标识名
                string key = this.Key;
                if (Admin.LoginPattern == LoginPatternEnum.Cookies)
                    encrypt = WeiSha.Common.Request.Cookies[key].String;
                if (Admin.LoginPattern == LoginPatternEnum.Session)
                    encrypt = WeiSha.Common.Request.Session[key].String;
                return encrypt;
            }            
        }
        /// <summary>
        /// Cookie或Session的时效，单位为分钟
        /// </summary>
        public int Expires
        {
            get
            {
                string exp = WeiSha.Common.Login.Get["Admin"].Expires.String;
                if (exp.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                    return int.MaxValue;
                return WeiSha.Common.Login.Get["Admin"].Expires.Int32 ?? 10;
            }
        }
        /// <summary>
        /// 当前登录用户的id
        /// </summary>
        public int UserID
        {
            get
            {
                string encrypt = string.Empty;
                //登录标识名
                string key = WeiSha.Common.Login.Get["Admin"].KeyName.String;
                string domain = WeiSha.Common.Request.Domain.TwoDomain;
                key = domain + "_" + key;
                if (Admin.LoginPattern == LoginPatternEnum.Cookies)
                    encrypt = WeiSha.Common.Request.Cookies[key].String;
                if (Admin.LoginPattern == LoginPatternEnum.Session)
                    encrypt = WeiSha.Common.Request.Session[key].String;
                //转换数值
                string[] str = DecryptForDES(encrypt);
                if (str == null) return 0;
                //时间
                long lTime = long.Parse(str[1]);
                DateTime time = new DateTime(lTime);
                if (time < DateTime.Now) return 0;
                //id
                int accid = 0;
                int.TryParse(str[0], out accid);                
                return accid;
            }
        }
        /// <summary>
        /// 返回当前登录用户的实体
        /// </summary>
        /// <returns></returns>
        public Song.Entities.EmpAccount CurrentUser
        {
            get
            {
                int userid = this.UserID;
                Song.Entities.EmpAccount curr = null;
                //首先从在线列表中取当前登录的用户
                if (this._onlineUser.Count > 0)
                {
                    foreach (Song.Entities.EmpAccount em in this.OnlineUser)
                    {
                        if (em == null) continue;
                        if (em.Acc_Id == userid)
                        {
                            curr = em;
                            break;
                        }
                    }
                }
                //内存中没有的话就从数据库取
                if (curr == null)
                {
                    if (userid > 0) curr = Business.Do<IEmployee>().GetSingle(userid);
                    if (curr != null) this._onlineUser.Add(curr);
                    if (curr == null && userid > 0) this.Logout();
                }
                return curr;
            }
        }
        #endregion
        /// <summary>
        /// 当前登录用户是否为超级管理员
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return Business.Do<IEmployee>().IsSuperAdmin(this.UserID); }
        }

        #endregion
        #region 加密解密方法
        /// <summary>
        /// 加密要写入的内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string EncryptForDES(int id)
        {
            DateTime time = DateTime.Now.AddMinutes(this.Expires);
            string str = id + "|" + time.Ticks;
            string twoDomain = WeiSha.Common.Request.Domain.TwoDomain;
            return WeiSha.Common.DataConvert.EncryptForDES(str, twoDomain); 
        }
        private string[] DecryptForDES(string encrypt)
        {
            string twoDomain = WeiSha.Common.Request.Domain.TwoDomain;
            string str = WeiSha.Common.DataConvert.DecryptForDES(encrypt, twoDomain);
            if (string.IsNullOrWhiteSpace(str)) return null;
            return str.Split('|');
        }
        #endregion
        #region 登录与注销的方法
        /// <summary>
        /// 将已经登录入的用户，写入seesion或cookies
        /// </summary>
        /// <param name="acc"></param>
        public void Write(Song.Entities.EmpAccount acc)
        {
            string domain = WeiSha.Common.Request.Domain.TwoDomain;
            Write(acc, domain);
        }
        /// <summary>
        /// 将已经登录入的用户，写入seesion或cookies
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="twoDomain">机构的二级域名，如果没有，则用机构id</param>
        public void Write(Song.Entities.EmpAccount acc, string twoDomain)
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //登录标识名
            string key = this.Key;
            if (Admin.LoginPattern == LoginPatternEnum.Cookies)
            {
                System.Web.HttpCookie cookie = new System.Web.HttpCookie(key);
                //cookie的值
                cookie.Value = this.EncryptForDES(acc.Acc_Id);
                cookie.Expires = DateTime.Now.AddMinutes(this.Expires);               
                _context.Response.Cookies.Add(cookie);
            }
            if (Admin.LoginPattern == LoginPatternEnum.Session)
            {
                _context.Session[key] = this.EncryptForDES(acc.Acc_Id);
            }
            this.Register(acc);
        }
        /// <summary>
        /// 写入当前用户
        /// </summary>
        public void Write()
        {
            //先读取当前用户，再写入
            this.Write(this.CurrentUser);
        }
        /// <summary>
        /// 注册某个用户到在线列表中
        /// </summary>
        /// <param name="acc"></param>
        private void Register(Song.Entities.EmpAccount acc)
        {
            if (acc == null) return;
            //登录时间，该时间不入数据库，仅为临时使用
            acc.Acc_LastTime = DateTime.Now;
            //登录用户是否已经存在;
            bool isHav = false;
            for (int i = 0; i < this._onlineUser.Count; i++)
            {
                EmpAccount e = this._onlineUser[i];
                if (e == null) continue;
                if (e.Acc_Id == acc.Acc_Id)
                {
                    this._onlineUser[i] = acc;
                    isHav = true;
                    break;
                }
            }
            //如果未登录，则注册进去
            if (!isHav) this._onlineUser.Add(acc);
        }
        /// <summary>
        /// 注销当前用户
        /// </summary>
        public void Logout()
        {
            int userid = this.UserID;
            if (userid < 1) return;
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //登录标识名
            string key = this.Key;
            if (Admin.LoginPattern == LoginPatternEnum.Cookies)
                _context.Response.Cookies[key].Expires = DateTime.Now.AddYears(-1);
            if (Admin.LoginPattern == LoginPatternEnum.Session)
                _context.Session.Abandon();
        }
        /// <summary>
        /// 清理超时用户
        /// </summary>
        public void CleanOut()
        {
            //设置超时时间，单位分钟
            int outTimeNumer = 3;
            List<Song.Entities.EmpAccount> _tm = new List<EmpAccount>();
            foreach (EmpAccount em in this.OnlineUser)
            {
                if (em == null) continue;
                if (DateTime.Now.AddMinutes(-outTimeNumer) > em.Acc_LastTime)
                {
                    _tm.Add(em);
                }
            }
            this._onlineUser = _tm;
        }
        #endregion
    }
}
