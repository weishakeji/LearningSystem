using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 登录属性验证的父类
    /// 验证方法：
    /// 1、如果类加了特性，则类下所有方法必须登录才能调用；除非方法特性有Ignore=true;
    /// 2、如果类加了特性，且Ignore=true，则类下所有方法都可以调，即便方法添加特性用Ignore=false
    /// 3、如果类没有加特性，但方法加了特性，则必须登录才能调用；
     /// </summary>
    public abstract class LoginAttribute : WeishaAttr
    {   
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <returns></returns>
        public abstract bool Logged();
        /// <summary>
        /// 将执行结果写入日志
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="execResult"></param>
        public static void LogWrite(LoginAttribute attr, object execResult)
        {
            if (attr == null) return;

        }
        /// <summary>
        /// 验证是否满足特性的限定
        /// </summary>
        /// <param name="method">执行的方法</param>
        /// <returns></returns>
        public static LoginAttribute Verify(MemberInfo method)
        {
            string msg = string.Format("当前方法 {0}.{1} 需要", method.DeclaringType.Name, method.Name);
            LoginAttribute loginattr = null;
            loginattr = LoginAttribute.GetAttr<AdminAttribute>(method);
            if (loginattr != null && !loginattr.Ignore && !loginattr.Logged())
                throw new Exception(msg+"管理员登录后操作");
            loginattr = LoginAttribute.GetAttr<StudentAttribute>(method);
            if (loginattr != null && !loginattr.Ignore && !loginattr.Logged())
                throw new Exception(msg + "学员账户登录后操作");
            loginattr = LoginAttribute.GetAttr<TeacherAttribute>(method);
            if (loginattr != null && !loginattr.Ignore && !loginattr.Logged())
                throw new Exception(msg + "教师账号登录后操作");
            return loginattr;
        }
    }
}
