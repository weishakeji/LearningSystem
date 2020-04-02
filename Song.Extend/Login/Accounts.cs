using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.Data;
using System.Web.SessionState;
using System.Web;

namespace Song.Extend.Login
{
    public class Accounts
    {
        #region 单例
        private static Accounts _singleton = null;
        private static readonly object _lockobj = new object();
        /// <summary>
        /// 获取参数
        /// </summary>
        public static Accounts Singleton
        {
            get
            {
                lock (_lockobj)
                {
                    if (_singleton == null)
                        _singleton = new Accounts();
                    return _singleton;
                }
            }
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int Expires { get; set; }
        /// <summary>
        ///  登录标识名
        /// </summary>
        public string KeyName { get; set; } 
        /// <summary>
        /// 后台管理登录的状态管理方式，值为Cookies或Session
        /// </summary>
        public LoginPatternEnum LoginPattern { get; set; } 

        private Accounts(){
            //计算过期时间（单位分钟），当登录多少分钟后失效
            string exp = WeiSha.Common.Login.Get["Accounts"].Expires.String;
            if (!exp.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                Expires = WeiSha.Common.Login.Get["Accounts"].Expires.Int32 ?? 10;
            // 后台管理登录的状态管理方式，值为Cookies或Session
            string tm = WeiSha.Common.Login.Get["Accounts"].Pattern.String;
            if (tm.Equals("Session", StringComparison.CurrentCultureIgnoreCase)) LoginPattern = LoginPatternEnum.Session;
            if (tm.Equals("Cookies", StringComparison.CurrentCultureIgnoreCase)) LoginPattern = LoginPatternEnum.Cookies;
            //登录标识名
            this.KeyName = WeiSha.Common.Login.Get["Accounts"].KeyName.String;   
        }
        #endregion

        #region 在线学员
        private static object _lock = new object();          
        /// <summary>
        /// 在线学员
        /// </summary>
        public List<Song.Entities.Accounts> OnlineUser
        {
            get
            {
                lock (_lock)
                {
                    List<Song.Entities.Accounts> list = HttpRuntime.Cache.Get("AccountsCache") as List<Song.Entities.Accounts>;
                    if (list == null) list = new List<Entities.Accounts>();
                    return list;
                }
            }
            set
            {
                lock (_lock)
                {
                    HttpRuntime.Cache.Insert("AccountsCache", value);
                }
            }
        }
        /// <summary>
        /// 登录时生成的随机字符串，用于区分学员每次不同的登录
        /// </summary>
        public string UID
        {
            get { return WeiSha.Common.Request.Cookies["AccountsUID"].String; }
            set
            {
                System.Web.HttpContext _context = System.Web.HttpContext.Current;
                System.Web.HttpCookie cookie = new System.Web.HttpCookie("AccountsUID", value);
                //如果是多机构，又不用IP访问，则用根域写入cookie
                int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
                if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) cookie.Domain = WeiSha.Common.Server.MainName;
                cookie.Expires = DateTime.Now.AddDays(30);
                _context.Response.Cookies.Add(cookie);
            }
        }        

        /// <summary>
        /// 添加在线学员
        /// </summary>
        /// <param name="acc"></param>
        public void OnlineUserAdd(Song.Entities.Accounts acc)
        {
            List<Song.Entities.Accounts> list = this.OnlineUser;
            list.Add(acc);
            this.OnlineUser = list;
        }
        /// <summary>
        /// 在线用户数
        /// </summary>
        public int OnlineCount
        {
            get
            {
                return this.CleanOut();
            }
        }
        /// <summary>
        /// 返回当前登录用户的实体
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Accounts CurrentUser
        {
            get
            {
                int acid = this.UserID;  //当前用户的账号id
                if (acid < 1) return null;
                string uid =this.UID; //用于判断是否重复登录
                Song.Entities.Accounts curr = null; //当前用户
                //首先从在线列表中取当前登录的用户
                List<Song.Entities.Accounts> list = this.OnlineUser;
                if (list.Count > 0)
                {
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        Song.Entities.Accounts em = list[i];
                        if (em == null) continue;
                        if (em.Ac_ID == acid)
                        {
                            //如果不是在微信中，同一账号不能同时登录
                            if (!WeiSha.Common.Browser.IsWeixin && em.Ac_CheckUID == uid)
                                curr = em;
                            break;
                        }
                    }                    
                }
                //内存中没有的话就从数据库取
                if (curr == null)
                {
                    if (acid > 0)
                    {
                        if (WeiSha.Common.Browser.IsWeixin)
                        {
                            curr = Business.Do<IAccounts>().AccountsSingle(acid);
                        }
                        else
                        {
                            curr = Business.Do<IAccounts>().AccountsSingle(acid, uid);
                        }
                    }
                    if (curr != null) this.OnlineUserAdd(curr);
                    //if (curr == null && acid > 0) this.Logout();    //如果没有用户但又有acid，则注销
                }
                return curr;
            }
        }
        /// <summary>
        /// 当前登录的教师信息
        /// </summary>
        public Song.Entities.Teacher Teacher
        {
            get
            {
                int acid = this.UserID;  //当前用户的账号id
                if (acid <= 0) return null;
                Song.Entities.Teacher th = Business.Do<IAccounts>().GetTeacher(acid, true);
                return th;
            }
        }
        /// <summary>
        /// 当前登录用户的ID
        /// </summary>
        public int UserID
        {
            get
            {
                int accid = 0;
                if (this.LoginPattern == LoginPatternEnum.Cookies)
                    accid = WeiSha.Common.Request.Cookies[KeyName].Int32 ?? 0;
                if (this.LoginPattern == LoginPatternEnum.Session)
                    accid = WeiSha.Common.Request.Session[KeyName].Int32 ?? 0;
                return accid;
            }
        }
        /// <summary>
        /// 用户是否登录
        /// </summary>
        public bool IsLogin
        {
            get { return this.CurrentUser != null; }
        }
        #endregion

        #region 登录与注销的方法
        /// <summary>
        /// 将已经登录入的用户，写入cookies或session
        /// </summary>
        /// <param name="acc"></param>
        public void Write(Song.Entities.Accounts acc)
        {
            this.Write(acc, -1);
        }
        /// <summary>
        /// 将已经登录入的用户，写入cookies或session
        /// </summary>
        /// <param name="accid"></param>
        public void Write(int accid)
        {
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(accid);
            this.Write(acc, -1);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">学员对象</param>
        /// <param name="expiresDay">有效时间，单位：天</param>
        public void Write(Song.Entities.Accounts acc, int expiresDay)
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            if (this.LoginPattern == LoginPatternEnum.Cookies)
            {
                System.Web.HttpCookie cookie = new System.Web.HttpCookie(KeyName);
                cookie.Value = acc.Ac_ID.ToString();
                //如果是多机构，又不用IP访问，则用根域写入cookie
                int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
                if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) cookie.Domain = WeiSha.Common.Server.MainName;
                //设置过期时间
                if (expiresDay <= 0)
                {
                    //如果是在微信中，登录时效尽可能长一些
                    if (WeiSha.Common.Browser.IsWeixin)
                    {
                        cookie.Expires = DateTime.Now.AddMinutes(999);
                    }
                    else
                    {
                       cookie.Expires = DateTime.Now.AddMinutes(this.Expires);
                    }
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(expiresDay);
                }
                _context.Response.Cookies.Add(cookie);
            }
            if (this.LoginPattern == LoginPatternEnum.Session)
            {
                if (_context.Session[KeyName] == null)
                {
                    _context.Session.Add(KeyName, acc.Ac_ID);
                }
                else
                {
                    _context.Session[KeyName] = acc.Ac_ID;
                }
            }
            //注册到内存，用于判断在线用户
            this._register(acc);
        }     
        /// <summary>
        /// 注册某个用户到在线列表中
        /// </summary>
        /// <param name="acc"></param>
        private void _register(Song.Entities.Accounts acc)
        {
            if (acc == null) return;
            //登录时间，该时间不入数据库，仅为临时使用
            acc.Ac_LastTime = DateTime.Now;
            acc.Ac_LastIP = WeiSha.Common.Browser.IP;   //写入登录时的IP
            //写入唯一值，用于判断同一个账号是否多人登录
            acc.Ac_CheckUID = WeiSha.Common.Request.UniqueID();
            this.UID = acc.Ac_CheckUID;
            //登录用户是否已经存在;
            bool isHav = false;
            List<Song.Entities.Accounts> list = this.OnlineUser;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null) continue;
                if (list[i].Ac_ID == acc.Ac_ID)
                {
                    list[i] = acc;
                    isHav = true;
                    break;
                }
            }
            Business.Do<IAccounts>().AccountsUpdate(acc,
                new Field[] { Song.Entities.Accounts._.Ac_LastTime, Song.Entities.Accounts._.Ac_LastIP, Song.Entities.Accounts._.Ac_CheckUID },
                new object[] { acc.Ac_LastTime, acc.Ac_LastIP, acc.Ac_CheckUID });
            //如果没有在缓存中记录，则注册进去
            if (!isHav) this.OnlineUserAdd(acc);
        }
        /// <summary>
        /// 刷新某个登录账户的信息
        /// </summary>
        /// <param name="st">用户</param>
        public void Refresh(Song.Entities.Accounts st)
        {
            if (st == null) return;
            List<Song.Entities.Accounts> list = this.OnlineUser;
            lock (_lock)
            {               
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == null) continue;
                    if (list[i].Ac_ID == st.Ac_ID)
                    {
                        //登录时间，该时间不入数据库，仅为临时使用
                        st.Ac_LastTime = DateTime.Now;
                        list[i] = st;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 刷新某个登录学员的信息
        /// </summary>
        /// <param name="accid">用户id</param>
        public void Refresh(int accid)
        {
            if (accid < 1) return;
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(accid);
            Refresh(acc);
        }
        /// <summary>
        /// 注销当前用户
        /// </summary>
        public void Logout()
        {
            int accid = this.UserID;
            if (accid < 1) return;
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            if (this.LoginPattern == LoginPatternEnum.Cookies)
            {
                //如果是多机构，又不用IP访问，则用根域写入cookie
                int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
                //清理当前域名下的cookie
                System.Web.HttpCookie cookie = _context.Response.Cookies[KeyName];
                if (cookie != null)
                {
                    if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) 
                        cookie.Domain = WeiSha.Common.Server.MainName;
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    _context.Response.Cookies.Add(cookie);
                }
            }
            if (this.LoginPattern == LoginPatternEnum.Session)
                _context.Session.Abandon();
            this.CleanOut(accid);
        }
        /// <summary>
        /// 清理超时用户
        /// </summary>
        public int CleanOut()
        {
            //设置超时时间，单位分钟
            int outTimeNumer = this.Expires > 720 ? 720 : this.Expires;
            List<Song.Entities.Accounts> list = this.OnlineUser;
            lock (_lock)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    Song.Entities.Accounts em = list[i];
                    if (em == null) continue;
                    if (DateTime.Now > em.Ac_LastTime.AddMinutes(outTimeNumer))
                    {
                        list.RemoveAt(i);
                    }
                }
            }
            this.OnlineUser = list;
            return list.Count;
        }
        public void CleanOut(Song.Entities.Accounts acc)
        {
            this.CleanOut(acc.Ac_ID);
        }
        public void CleanOut(int accid)
        {
            List<Song.Entities.Accounts> list = this.OnlineUser;
            lock (_lock)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (list[i] == null) continue;
                    if (list[i].Ac_ID == accid)
                    {
                        list.RemoveAt(i);
                    }
                }              
            }
            this.OnlineUser = list;
        }
        /// <summary>
        /// 验证是否登录，没有登录，则跳转
        /// </summary>
        public void VerifyLogin()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //获取当前文件的文件名
            string path = _context.Request.ServerVariables["PATH_INFO"];
            path = path.Substring(path.LastIndexOf("/") + 1);
            //如果不是首页
            if (path != "index.aspx")
            {
                if (!this.IsLogin)
                {
                    //如果未登录
                    string url = WeiSha.Common.Login.Get["Accounts"].NoLoginPath.String ?? "/";
                    _context.Response.Redirect(url);
                }
            }
        }
        #endregion

        #region 当前课程
        /// <summary>
        /// 将要学习的课程记录下来
        /// </summary>
        /// <param name="cou">课程</param>
        public void Course(Song.Entities.Course cou)
        {
            //记录到数据库
            Song.Entities.Accounts st = this.CurrentUser;
            if (st != null)
            {
                st.Ac_CurrCourse = cou.Cou_ID;
                Business.Do<IAccounts>().AccountsSave(st);
            }

        }
        /// <summary>
        /// 获取当前学习的课程
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Course Course()
        {          
            Song.Entities.Course c = null;
            Song.Entities.Accounts st = this.CurrentUser;
            if (st != null && st.Ac_CurrCourse > 0)
            {
                //是否购买
                bool isBuy = false, istry = false;
                isBuy = Business.Do<ICourse>().StudyIsCourse(st.Ac_ID, st.Ac_CurrCourse);
                if (!isBuy) istry = Business.Do<ICourse>().IsTryout(st.Ac_ID, st.Ac_CurrCourse);
                if (isBuy || istry)
                {
                    c = Business.Do<ICourse>().CourseSingle(st.Ac_CurrCourse);
                }
            }
            return c;
        }
        #endregion

    }
}
