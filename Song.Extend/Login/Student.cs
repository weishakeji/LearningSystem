using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Extend.Login
{
    public class Student
    {
        private static readonly Student _singleton = new Student();
        /// <summary>
        /// 获取参数
        /// </summary>
        public static Student Singleton
        {
            get { return _singleton; }
        }
        #region 属性
        /// <summary>
        /// 登录时生成的随机字符串，用于区分学员每次不同的登录
        /// </summary>
        public string UID
        {
            get
            {
                return WeiSha.Common.Request.Cookies["StuedentUID"].String;
            }
        }        

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
        private List<Song.Entities.Student> _onlineUser = new List<Song.Entities.Student>();
        /// <summary>
        /// 在线用户
        /// </summary>
        public List<Song.Entities.Student> OnlineUser
        {
            get { return _onlineUser; }
        }
        /// <summary>
        /// 返回当前登录用户的实体
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Student CurrentUser
        {
            get
            {
                int currid = this.CurrentUserId;
                string stuid = WeiSha.Common.Request.Cookies["stuid"].String;
                Song.Entities.Student curr = null;
                //首先从在线列表中取当前登录的用户
                if (this._onlineUser.Count > 0)
                {
                    foreach (Song.Entities.Student em in this.OnlineUser)
                    {
                        if (em == null) continue;
                        if (em.St_ID == currid && em.St_CheckUID == stuid)
                        {
                            curr = em;
                            break;
                        }
                    }
                }
                //内存中没有的话就从数据库取
                if (curr == null)
                {
                    if (currid > 0) 
                        curr = Business.Do<IStudent>().StudentSingle(currid, stuid);                  
                    if (curr != null) this._onlineUser.Add(curr);
                    if (curr == null && currid > 0) this.Logout();
                }
                //当前学员是否处在当前机构，主要是在切换机构后，学员是否仍然在线的问题
                if (curr != null)
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                    curr = curr.Org_ID == org.Org_ID ? curr : null;
                }
                return curr;
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
                //登录标识名
                string key = WeiSha.Common.Login.Get["Accounts"].KeyName.String;
                string domain = WeiSha.Common.Request.Domain.TwoDomain;
                key = domain + "_" + key;
                if (Student.LoginPattern == LoginPatternEnum.Cookies)
                    accid = WeiSha.Common.Request.Cookies[key].Int32 ?? 0;
                if (Student.LoginPattern == LoginPatternEnum.Session)
                    accid = WeiSha.Common.Request.Session[key].Int32 ?? 0;
                return accid;
            }
        }
        /// <summary>
        /// 用户是否登录
        /// </summary>
        public bool IsLogin
        {
            get { return this.CurrentUserId != 0 && this.CurrentUser != null; }
        }

        /// <summary>
        /// 是否在试用
        /// </summary>
        public bool IsTryout
        {
            get
            {
                string str = WeiSha.Common.Request.Cookies["Tryout"].String;
                return str == "Tryout";
            }
        }
        public bool Tryout()
        {
            string key = "Tryout";
            System.Web.HttpCookie cookie = new System.Web.HttpCookie(key);
            cookie.Value = key;
            cookie.Expires = DateTime.Now.AddYears(1);
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            _context.Response.Cookies.Add(cookie);
            return true;
        }
        /// <summary>
        /// 将要学习的课程记录下来
        /// </summary>
        /// <param name="cou">课程</param>
        public void Course(Song.Entities.Course cou)
        {
            //记录到数据库
            Song.Entities.Student st = this.CurrentUser;
            st.St_CurrCourse = cou.Cou_ID;
            Business.Do<IStudent>().StudentSave(st);

        }
        /// <summary>
        /// 获取当前学习的课程
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Course Course()
        {
            if (!this.IsLogin) return null;
            Song.Entities.Course c = null;
            Song.Entities.Student st = this.CurrentUser;
            if (st != null && st.St_CurrCourse > 0)
            {
                //是否购买
                bool isBuy = false, istry = false;
                isBuy = Business.Do<ICourse>().StudyIsCourse(st.St_ID, st.St_CurrCourse);
                if (!isBuy) istry = Business.Do<ICourse>().IsTryout(st.St_ID, st.St_CurrCourse);
                if (isBuy || istry)
                {
                    c = Business.Do<ICourse>().CourseSingle(st.St_CurrCourse);
                    if (c != null)
                    {
                        c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                        c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                    }                    
                }
            }
            return c;
        }
        #endregion

        #region 登录与注销的方法
        /// <summary>
        /// 将已经登录入的用户，写入seesion
        /// </summary>
        /// <param name="acc"></param>
        public void Write(Song.Entities.Student acc)
        {
            this.Write(acc, -1);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">学员对象</param>
        /// <param name="expiresDay">有效时间，单位：天</param>
        public void Write(Song.Entities.Student acc, int expiresDay)
        {            
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //生成此次登录的唯一标识
            _context.Response.Cookies.Add(new System.Web.HttpCookie("StuedentUID",WeiSha.Common.Request.UniqueID()));
            //登录标识名
            string key = WeiSha.Common.Login.Get["Accounts"].KeyName.String;
            if (Student.LoginPattern == LoginPatternEnum.Cookies)
            {
                System.Web.HttpCookie cookie = new System.Web.HttpCookie(key);
                cookie.Value = acc.St_ID.ToString();
                string exp = WeiSha.Common.Login.Get["Accounts"].Expires.String;
                if (expiresDay <= 0)
                {
                    if (!exp.Equals("auto", StringComparison.CurrentCultureIgnoreCase))
                        cookie.Expires = DateTime.Now.AddMinutes(WeiSha.Common.Login.Get["Accounts"].Expires.Int32 ?? 10);
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(expiresDay);
                }
                _context.Response.Cookies.Add(cookie);
            }
            if (Student.LoginPattern == LoginPatternEnum.Session)
            {
                _context.Session[key] = acc.St_ID;
            }
            //记录登录日志
            Business.Do<IStudent>().LogForLoginAdd(acc);
            //注册到内存，用于判断在线用户
            this.Register(acc);
        }
        /// <summary>
        /// 获取当前登录用户的对象
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Student Read()
        {
            int accid = this.CurrentUserId;
            if (accid < 1) return null;

            Song.Entities.Student acc = Business.Do<IStudent>().StudentSingle(accid);
            return acc;
        }
        /// <summary>
        /// 注册已经登录的在线用户，如果已经注册，则更新注册时间
        /// </summary>
        public void Register()
        {
            Song.Entities.Student ea = this.Read();
            if (ea != null) Register(ea);
            //更新注册信息
            //Business.Do<IStudent>().LogForLoginFresh();
        }
        /// <summary>
        /// 注册某个用户到在线列表中
        /// </summary>
        /// <param name="acc"></param>
        public void Register(Song.Entities.Student acc)
        {
            if (acc == null) return;
            //登录时间，该时间不入数据库，仅为临时使用
            acc.St_LastTime = DateTime.Now;
            //写入登录时的IP
            acc.St_LastIP = WeiSha.Common.Browser.IP;
            //写入唯一值，用于判断同一个账号是否多人登录
            string uid = WeiSha.Common.Request.UniqueID();
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            _context.Response.Cookies.Add(new System.Web.HttpCookie("stuid", uid));
            acc.St_CheckUID = uid;
            //登录用户是否已经存在;
            bool isHav = false;
            for (int i = 0; i < this._onlineUser.Count; i++)
            {
                Song.Entities.Student e = this._onlineUser[i];
                if (e == null) continue;
                if (e.St_ID == acc.St_ID)
                {
                    this._onlineUser[i] = acc;
                    isHav = true;
                    break;
                }
            }
            Business.Do<IStudent>().StudentSave(acc);
            //如果未登录，则注册进去
            if (!isHav)
            {
                this._onlineUser.Add(acc);
            }
        }
        /// <summary>
        /// 刷新某个登录学员的信息
        /// </summary>
        /// <param name="st">学员</param>
        public void Refresh(Song.Entities.Student st)
        {
            if (st == null) return;            
            //登录时间，该时间不入数据库，仅为临时使用
            st.St_LastTime = DateTime.Now;
            for (int i = 0; i < this._onlineUser.Count; i++)
            {
                Song.Entities.Student e = this._onlineUser[i];
                if (e == null) continue;
                if (e.St_ID == st.St_ID)
                {
                    this._onlineUser[i] = st;
                    break;
                }
            }
        }
        /// <summary>
        /// 刷新某个登录学员的信息
        /// </summary>
        /// <param name="stid">学员id</param>
        public void Refresh(int stid)
        {
            Song.Entities.Student acc = Business.Do<IStudent>().StudentSingle(stid);
            Refresh(acc);
        }

        /// <summary>
        /// 注销当前用户
        /// </summary>
        public void Logout()
        {
            int accid = this.CurrentUserId;
            if (accid < 1) return;
            //更新注册信息
            Business.Do<IStudent>().LogForLoginOut();
            //清除登录信息
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            //登录标识名
            string key = WeiSha.Common.Login.Get["Accounts"].KeyName.String;
            string domain = WeiSha.Common.Request.Domain.TwoDomain;
            key = domain + "_" + key;
            if (Student.LoginPattern == LoginPatternEnum.Cookies)
                _context.Response.Cookies[key].Expires = DateTime.Now.AddYears(-1);
            if (Student.LoginPattern == LoginPatternEnum.Session)
                _context.Session.Abandon();
            CleanOut();
        }
        /// <summary>
        /// 清理超时用户
        /// </summary>
        public void CleanOut()
        {
            //设置超时时间，单位分钟
            int outTimeNumer = 3;
            List<Song.Entities.Student> _tm = new List<Song.Entities.Student>();
            foreach (Song.Entities.Student em in this.OnlineUser)
            {
                if (em == null) continue;
                if (DateTime.Now.AddMinutes(-outTimeNumer) > em.St_LastTime)
                {
                    _tm.Add(em);
                }
            }
            this._onlineUser = _tm;
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
                    //_context.Response.Redirect(url);
                }
            }
        }
        #endregion
    }
}
