using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;


namespace Song.ViewData.Methods
{
    
    /// <summary>
    /// 教师账号的相关操作
    /// </summary> 
    public class Teacher
    {
        
        /// <summary>
        /// 教师登录
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public Song.Entities.Teacher Login(string acc,string pw)
        {
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (account == null) return null;
            if (!account.Ac_IsTeacher) return null;
            //
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(account.Ac_AccName, -1);
            return teacher;
        }

        public int test()
        {


            return 0;
        }

    }
}
