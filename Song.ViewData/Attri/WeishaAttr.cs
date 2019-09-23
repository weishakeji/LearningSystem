using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Song.ViewData.Attri
{
    public class WeishaAttr : Attribute
    {

        /// <summary>
        /// 忽略此方法，默认为false
        /// 说明：则某个类设置了特性后，下面的所有方法都需要验证，除非设置[Admin(Ignore=true)]
        /// </summary>
        public bool Ignore { get; set; }
        public WeishaAttr(bool ignore)
        {
            Ignore = ignore;
        }
        public WeishaAttr()
        {
        }
        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="obj">要执行的对象，先验证它是否需要登录</param>
        /// <param name="method">要验证的方法</param>
        /// <returns></returns>
        public static T AuthenticateLoginControl<T>(object obj, MemberInfo method) where T : WeishaAttr
        {
            T attr = null;
            //先验证对象，如果对象需验证，则下面方法全部需要验证登录，除非方法设置了[Admin(Ignore = true)]
            object[] attrsObj = obj.GetType().GetCustomAttributes(typeof(T), true);
            if (attrsObj.Length > 0) attr = attrsObj[0] as T;
            //再验证方法
            object[] attrsMethod = method.GetCustomAttributes(typeof(T), true);
            if (attrsMethod.Length > 0)
            {
                T admin = attrsMethod[0] as T;
                if (attr == null) attr = admin;
                if (attr != null && admin.Ignore) attr.Ignore = admin.Ignore;
            }
            return attr;
        }

    }
}
