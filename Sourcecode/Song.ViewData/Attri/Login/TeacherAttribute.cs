using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 必须是教师登录后才能调用
    /// </summary>
    public class TeacherAttribute : LoginAttribute
    {
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public override bool Logged(Letter letter)
        {       
            Song.Entities.Teacher th = LoginAccount.Status.Teacher();
            return th != null;
        }
    }
}
