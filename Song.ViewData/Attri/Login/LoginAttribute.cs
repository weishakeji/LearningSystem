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
    }
}
