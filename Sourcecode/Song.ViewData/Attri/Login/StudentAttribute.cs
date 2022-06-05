using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 必须是学员登录后才能调用
    /// </summary>
    public class StudentAttribute : LoginAttribute
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public override bool Logged(Letter letter)
        {
            return LoginAccount.Status.Login(letter);         
        }
    }
}
