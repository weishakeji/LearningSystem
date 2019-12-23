using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 必须是超级管理员登录后才能操作
    /// </summary>
    public class SuperAdminAttribute : LoginAttribute
    {
        /// <summary>
        /// 超级管理员是否登录
        /// </summary>
        /// <returns></returns>
        public override bool Logged()
        {
            if (Extend.LoginState.Admin.CurrentUser == null) return false;
            return Extend.LoginState.Admin.IsSuperAdmin;
        }

    }
}
