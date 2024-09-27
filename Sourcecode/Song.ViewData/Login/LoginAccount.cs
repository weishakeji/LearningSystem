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
        //token的管理
        private static LoginToken token = new LoginToken(LoginTokenType.Account);
        //登录的缓存
        public static readonly LoginCache Cache = LoginCache.Get<Accounts>(LoginTokenType.Account);

        private LoginAccount()
        {
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
            using(LoginToken t = token.Analysis(letter.LoginStatus))
            {
                if (t == null) return null;
                int accid = (int)t.RoleID;
                if (accid <= 0) return null;
                //从内存中获取
                Song.Entities.Accounts acc = LoginAccount.Cache.Get<Accounts>(accid);
                if (acc != null && (!acc.Ac_IsUse || !acc.Ac_IsPass)) acc = null;
                //从数据库获取
                if (acc == null)
                {
                    acc = Business.Do<IAccounts>().AccountsSingle(accid);
                    if (acc != null) LoginAccount.Cache.Add<Accounts>(acc, accid);
                }
                if (acc != null && (!acc.Ac_IsUse || !acc.Ac_IsPass)) acc = null;
                //校验checkUID
                if (acc == null || string.IsNullOrWhiteSpace(acc.Ac_CheckUID) || !acc.Ac_CheckUID.Equals(t.UniqueID)) return null;
                acc = acc.DeepClone<Song.Entities.Accounts>();
                if (!string.IsNullOrWhiteSpace(acc.Ac_Photo) && !acc.Ac_Photo.StartsWith("/"))
                    acc.Ac_Photo = System.IO.File.Exists(phyPath + acc.Ac_Photo) ? virPath + acc.Ac_Photo : "";
                acc.Ac_Pw = string.Empty;
                return acc;
            }           
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">当前账号</param>
        /// <returns></returns>
        public string Login(Accounts acc)
        {
            LoginAccount.Cache.Add<Accounts>(acc, acc.Ac_ID);
            //异步存储
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IAccounts>().RecordLoginCode(acc.Ac_ID, acc.Ac_CheckUID);
            }).Start();

            //返回加密状态码
            return Generate_Checkcode(acc);
        }
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public bool IsLogin(Letter letter)
        {
            return this.User(letter) != null;           
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
            LoginAccount.Cache.Add<Accounts>(acc, acc.Ac_ID);
            //异步存储
            Task.Run(() => Business.Do<IAccounts>().RecordLoginCode(acc.Ac_ID, acc.Ac_CheckUID));          
            return Generate_Checkcode(acc);
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
            string code = Generate_Checkcode(acc);
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
        public string Generate_Checkcode(Song.Entities.Accounts acc)
        {
            return token.Generate(acc.Ac_CheckUID, acc.Ac_ID);           
        }
    }
}
