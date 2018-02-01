using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Collections;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Extend
{
    /// <summary>
    /// 记录在线信息，如员工登录状态
    /// </summary>
    public class LoginState
    {
        /// <summary>
        /// 管理人员的登录管理
        /// </summary>
        public static Login.Admin Admin
        {
            get
            {
                return Login.Admin.Singleton;
            }
        }
        /// <summary>
        /// 账号的登录管理
        /// </summary>
        public static Login.Accounts Accounts
        {
            get
            {
                return Login.Accounts.Singleton;
            }
        }
        /// <summary>
        /// 当前登录的教师
        /// </summary>
        public static Song.Entities.Teacher Teacher
        {
            get
            {
                return Login.Accounts.Singleton.Teacher;
            }
        }
    }
    
}
