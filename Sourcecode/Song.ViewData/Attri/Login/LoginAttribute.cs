using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Web;

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
        //日志目录,相对于根路径
        private static string log_path = "logs_viewdata";
        private static int log_level = -1;
        /// <summary>
        /// 是否登录
        /// </summary>
        /// <param name="letter">客户端传来的消息对象</param>
        /// <returns></returns>
        public abstract bool Logged(Letter letter);
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
        /// <param name="letter">客户端传来的消息对象</param>
        /// <returns></returns>
        public static LoginAttribute Verify(MemberInfo method, Letter letter)
        {
            LoginAttribute loginattr = null;
            //验证所的登录特性，满足一个即可
            List<LoginAttribute> attrs = LoginAttribute.GetAttrs<LoginAttribute>(method);
            foreach (LoginAttribute att in attrs)
            {
                if (att.Logged(letter))
                {
                    loginattr = att;
                    break;
                }
            }
            if (loginattr != null) return loginattr;

            //逐个验证
            string msg = string.Format("接口 '{0}/{1}' 需要", method.DeclaringType.Name, method.Name);

            List<string> list = new List<string>();

            loginattr = LoginAttribute.GetAttr<AdminAttribute>(method);
            if (loginattr != null && !loginattr.Ignore && !loginattr.Logged(letter))
                list.Add("管理员");

            loginattr = LoginAttribute.GetAttr<SuperAdminAttribute>(method);
            if (loginattr != null && !loginattr.Ignore && !loginattr.Logged(letter))
                list.Add("超级管理员");

            loginattr = LoginAttribute.GetAttr<StudentAttribute>(method);
            if (loginattr != null && !loginattr.Ignore && !loginattr.Logged(letter))
                list.Add("学员");


            loginattr = LoginAttribute.GetAttr<TeacherAttribute>(method);
            if (loginattr != null && !loginattr.Ignore && !loginattr.Logged(letter))
                list.Add("教师");

            if (list.Count > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < list.Count; i++)
                {
                    str += "“" + list[i] + "”";
                    if (i < list.Count - 1) str += "或";
                }
                throw new Exception(msg + str + "；登录后操作");
            }
            return loginattr;
        }
    }
}
