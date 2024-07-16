using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 设置方法是否需要管理员登录后执行
    /// </summary>
    public class AdminAttribute : LoginAttribute
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public override bool Logged(Letter letter)
        {
            return LoginAdmin.Status.IsLogin(letter);
        }

    }
}
