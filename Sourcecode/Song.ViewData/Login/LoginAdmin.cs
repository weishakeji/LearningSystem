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
        private string virPath = WeiSha.Core.Upload.Get["Employee"].Virtual;
        private string phyPath = WeiSha.Core.Upload.Get["Employee"].Physics;
        //token的管理
        private static LoginToken token = new LoginToken(LoginTokenType.Admin);
        //登录的缓存
        public static readonly LoginCache Cache = LoginCache.Get<EmpAccount>(LoginTokenType.Admin);

        private LoginAdmin()
        {
           
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
            using (LoginToken t = token.Analysis(letter.LoginStatus))
            {
                if (t == null) return null;
                int accid = (int)t.RoleID;
                if (accid <= 0) return null;
                //从内存中获取
                Song.Entities.EmpAccount acc = LoginAdmin.Cache.Get<EmpAccount>(accid);
                if (acc != null && !acc.Acc_IsUse) acc = null;
                //从数据库获取
                if (acc == null)
                {
                    acc = Business.Do<IEmployee>().GetSingle(accid);
                    if (acc != null) LoginAccount.Cache.Add<EmpAccount>(acc, accid);
                }
                if (acc != null && !acc.Acc_IsUse) acc = null;
                //校验checkUID
                if (acc == null || string.IsNullOrWhiteSpace(acc.Acc_CheckUID) || !acc.Acc_CheckUID.Equals(t.UniqueID)) return null;
                acc = acc.DeepClone<Song.Entities.EmpAccount>();
                if (!string.IsNullOrWhiteSpace(acc.Acc_Photo))
                {
                    acc.Acc_Photo = string.IsNullOrWhiteSpace(acc.Acc_Photo) ? "" : WeiSha.Core.Upload.Get["Employee"].Virtual + acc.Acc_Photo;
                    if (!System.IO.File.Exists(WeiSha.Core.Server.MapPath(acc.Acc_Photo))) acc.Acc_Photo = "";
                }
                acc.Acc_Pw = string.Empty;
                return acc;
            }
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
        /// 登录
        /// </summary>
        /// <param name="acc">当前账号</param>
        /// <returns></returns>
        public string Login(Song.Entities.EmpAccount acc)
        {                     
            //识别码，记录到数据库
            acc.Acc_CheckUID = WeiSha.Core.Request.UniqueID();
            LoginAdmin.Cache.Add<EmpAccount>(acc, acc.Acc_Id);
            //异步存储
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IEmployee>().RecordLoginCode(acc.Acc_Id, acc.Acc_CheckUID);
            }).Start();

            //返回加密状态码
            return token.Generate(acc.Acc_CheckUID, acc.Acc_Id); 
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
            //异步存储
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IEmployee>().RecordLoginCode(acc.Acc_Id, string.Empty);
            }).Start();                   
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
            LoginAdmin.Cache.Add<EmpAccount>(acc, acc.Acc_Id);
            //重新生成token,过期时间被刷新
            return token.Generate(acc.Acc_CheckUID, acc.Acc_Id);
        }
    }
}
