using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Song.ViewData
{
    /// <summary>
    /// 接口的父类
    /// </summary>
    public abstract class ViewMethod
    {
        /// <summary>
        /// 客户端传来的参数
        /// </summary>
        public Letter Letter
        {
            get
            {
                System.Web.HttpContext _context = System.Web.HttpContext.Current;
                return Letter.Constructor(_context);
            }
        }
        /// <summary>
        /// 当前HttpContext
        /// </summary>
        public System.Web.HttpContext Context
        {
            get
            {              
                return System.Web.HttpContext.Current;
            }
        }
        /// <summary>
        /// 当前登录的账号
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Accounts User
        {
            get
            {
                return LoginAccount.Status.User(this.Letter);
            }
        }
        /// <summary>
        /// 当前登录的管理员
        /// </summary>
        /// <returns></returns>
        public Song.Entities.EmpAccount Admin
        {
            get
            {
                return LoginAdmin.Status.User(this.Letter);
            }
        }
        /// <summary>
        /// 当前登录的教师
        /// </summary>
        public Song.Entities.Teacher Teacher
        {
            get
            {
                return LoginAccount.Status.Teacher(this.Letter);
            }
        }
        /// <summary>
        /// 上传来的文件
        /// </summary>
        public HttpFileCollectionBase Files
        {
            get
            {
                return this.Letter.Files;
            }
        }
    }
}
