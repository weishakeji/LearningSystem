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
        private static  Accounts _singleton = null;
        private static readonly object _lockobj = new object();     
        //用于记录uid的cookie名称
        private readonly string _uid = "AccountsUID";
        /// <summary>
        /// 获取参数
        /// </summary>
        public static Accounts Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    lock (_lockobj)
                    {
                        if (_singleton == null)
                        {
                            _singleton = new Accounts();
                        }
                    }
                }
                return _singleton;
            }
        }
        /// <summary>
        /// 登录时生成的随机字符串，用于区分学员每次不同的登录
        /// </summary>
        public string UID
        {
            get { return WeiSha.Common.Request.Cookies[_uid].String; }
        }
        #region 属性

        /// <summary>
        /// 后台管理登录的状态管理方式，值为Cookies或Session
        /// </summary>
        public static LoginPatternEnum LoginPattern
        {
            get
            {
                string tm = WeiSha.Common.Login.Get["Accounts"].Pattern.String;
                if (tm.Equals("Session", StringComparison.CurrentCultureIgnoreCase)) return LoginPatternEnum.Session;
                if (tm.Equals("Cookies", StringComparison.CurrentCultureIgnoreCase)) return LoginPatternEnum.Cookies;
                return LoginPatternEnum.Cookies;
            }
        }
        private List<Song.Entities.Accounts> _onlineUser = new List<Song.Entities.Accounts>();
        private static object _lock = new object();
        /// <summary>
        /// 添加在线人数
        /// </summary>
        /// <param name="acc"></param>
        public void OnlineUserAdd(Song.Entities.Accounts acc)
        {
            lock (_lock)
            {
                this._onlineUser.Add(acc);
            }
        }
        /// <summary>
        /// 在线用户数
        /// </summary>
        public int OnlineCount
        {
            get
            {
                this.CleanOut();
                return this._onlineUser.Count;
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
                int acid = this.CurrentUserId;  //当前用户的账号id
                if (acid < 1) return null;
                string uid = WeiSha.Common.Request.Cookies[_uid].String; //用于判断是否重复登录
                Song.Entities.Accounts curr = null; //当前用户
                //首先从在线列表中取当前登录的用户
                if (this._onlineUser.Count > 0)
                {
                    for (int i = this._onlineUser.Count - 1; i >= 0; i--)
                    {
                        Song.Entities.Accounts em = this._onlineUser[i];
                        if (em.Ac_ID == acid)
                        {
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
                int acid = this.CurrentUserId;  //当前用户的账号id
                if (acid <= 0) return null;
                Song.Entities.Teacher th = Business.Do<IAccounts>().GetTeacher(acid, true);
                return th;
            }
        }
        /// <summary>
        /// 当前登录用户的id
        /// </summary>
        public int CurrentUserId
        {
            get
            {
                int accid = 0;
                string key = WeiSha.Common.Login.Get["Accounts"].KeyName.String;    //登录标识名
                if (Accounts.LoginPattern == LoginPatternEnum.Cookies)
                    accid = WeiSha.Common.Request.Cookies[key].Int32 ?? 0;
                if (Accounts.LoginPattern == LoginPatternEnum.Session)
                    accid = WeiSha.Common.Request.Session[key].Int32 ?? 0;
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
            //登录标识名
            string key = WeiSha.Common.Login.Get["Accounts"].KeyName.String;
            if (Accounts.LoginPattern == LoginPatternEnum.Cookies)
            {
                System.Web.HttpCookie cookie = new System.Web.HttpCookie(key);
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
                        string exp = WeiSha.Common.Login.Get["Accounts"].Expires.String;
                        if (!exp.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                            cookie.Expires = DateTime.Now.AddMinutes(WeiSha.Common.Login.Get["Accounts"].Expires.Int32 ?? 10);
                    }
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(expiresDay);
                }
                _context.Response.Cookies.Add(cookie);
            }
            if (Accounts.LoginPattern == LoginPatternEnum.Session)
            {
                if (_context.Session[key] == null)
                {
                    _context.Session.Add(key, acc.Ac_ID);
                }
                else
                {
                    _context.Session[key] = acc.Ac_ID;
                }
            }
            //注册到内存，用于判断在线用户
            this._register(acc);
        }     
        /// <summary>
        /// 获取当前登录用户的对象
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Accounts Read()
        {
            return this.CurrentUser;
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
            string uid = WeiSha.Common.Request.UniqueID();
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(_uid, uid);
            //如果是多机构，又不用IP访问，则用根域写入cookie
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) cookie.Domain = WeiSha.Common.Server.MainName;
            _context.Response.Cookies.Add(cookie);
            acc.Ac_CheckUID = uid;
            //登录用户是否已经存在;
            bool isHav = false;
            for (int i = 0; i < this._onlineUser.Count; i++)
            {
                Song.Entities.Accounts e = this._onlineUser[i];
                if (e == null) continue;
                if (e.Ac_ID == acc.Ac_ID)
                {
                    this._onlineUser[i] = acc;
                    isHav = true;
                    break;
                }
            }
            Business.Do<IAccounts>().AccountsUpdate(acc,
                new Field[] { Song.Entities.Accounts._.Ac_LastTime, Song.Entities.Accounts._.Ac_LastIP, Song.Entities.Accounts._.Ac_CheckUID },
                new object[] { acc.Ac_LastTime, acc.Ac_LastIP, acc.Ac_CheckUID });
            //如果未登录，则注册进去
            if (!isHav) this.OnlineUserAdd(acc);
        }
        /// <summary>
        /// 刷新某个登录账户的信息
        /// </summary>
        /// <param name="st">用户</param>
        public void Refresh(Song.Entities.Accounts st)
        {
            if (st == null) return;
            lock (_lock)
            {
                //登录时间，该时间不入数据库，仅为临时使用
                st.Ac_LastTime = DateTime.Now;
                for (int i = 0; i < this._onlineUser.Count; i++)
                {
                    Song.Entities.Accounts e = this._onlineUser[i];
                    if (e == null) continue;
                    if (e.Ac_ID == st.Ac_ID)
                    {
                        this._onlineUser[i] = st;
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
            int accid = this.CurrentUserId;
            if (accid < 1) return;
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //登录标识名
            string key = WeiSha.Common.Login.Get["Accounts"].KeyName.String;
            if (Accounts.LoginPattern == LoginPatternEnum.Cookies)
            {
                //如果是多机构，又不用IP访问，则用根域写入cookie
                int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
                //清理当前域名下的cookie
                System.Web.HttpCookie cookie = _context.Response.Cookies[key];
                if (cookie != null)
                {
                    if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) 
                        cookie.Domain = WeiSha.Common.Server.MainName;
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    _context.Response.Cookies.Add(cookie);
                }
            }
            if (Accounts.LoginPattern == LoginPatternEnum.Session)
                _context.Session.Abandon();
            this.CleanOut(accid);
        }
        /// <summary>
        /// 清理超时用户
        /// </summary>
        public void CleanOut()
        {
            //设置超时时间，单位分钟
            int outTimeNumer = 100;
            string exp = WeiSha.Common.Login.Get["Accounts"].Expires.String;
            if (!exp.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                outTimeNumer = WeiSha.Common.Login.Get["Accounts"].Expires.Int32 ?? 10;
            lock (_lock)
            {
                for (int i = this._onlineUser.Count - 1; i >= 0; i--)
                {
                    Song.Entities.Accounts em = this._onlineUser[i];
                    if (DateTime.Now < em.Ac_LastTime.AddMinutes(outTimeNumer))
                    {
                        this._onlineUser.RemoveAt(i);
                    }
                }
            }
        }
        public void CleanOut(Song.Entities.Accounts acc)
        {
            this.CleanOut(acc.Ac_ID);
        }
        public void CleanOut(int accid)
        {
            lock (_lock)
            {
                for (int i = this._onlineUser.Count - 1; i >= 0; i--)
                {
                    if (this._onlineUser[i].Ac_ID == accid)
                    {
                        this._onlineUser.RemoveAt(i);
                    }
                }              
            }
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
            if (!this.IsLogin) return null;
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
                    //if (c != null)
                    //{
                    //    c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                    //    c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                    //}
                }
            }
            return c;
        }
        #endregion

        #region 权限判断
        /// <summary>
        /// 验证是否拥操作当前页面的权限
        /// </summary>
        public void VerifyPurview()
        {

        }
        #endregion
    }
}
