using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Song.ViewData.Attri
{
    public class WeishaAttr : Attribute
    {
        //所有特性
        public static Type[] Attrs = null;
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
        /// 获取方法的某一类特性
        /// </summary>
        /// <param name="method">要验证的方法</param>
        /// <returns></returns>
        public static T GetAttr<T>(MemberInfo method) where T : WeishaAttr
        {
            T attr = null;
            //先验证对象，如果对象需验证，则下面方法全部需要验证登录，除非方法设置了[Admin(Ignore = true)]
            object[] attrsObj = method.DeclaringType.GetCustomAttributes(typeof(T), true);
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
        /// <summary>
        /// 获取方法的特性
        /// </summary>
        /// <param name="method">要验证的方法</param>
        /// <returns></returns>
        public static List<T> GetAttrs<T>(MemberInfo method) where T : WeishaAttr
        {
            List<T> list = new List<T>();
            //先验证对象，如果对象需验证，则下面方法全部需要验证登录，除非方法设置了[Admin(Ignore = true)]
            object[] attrsObj = method.DeclaringType.GetCustomAttributes(typeof(T), true);
            foreach (object o in attrsObj) list.Add(o as T);
            //再验证方法
            object[] attrsMethod = method.GetCustomAttributes(typeof(T), true);
            foreach (object b in attrsMethod)
            {
                bool isExist = false;
                foreach (T t in list)
                {
                    if (t.GetType().FullName == b.GetType().FullName)
                    {
                        isExist = true;
                        T tm = b as T;
                        if (tm.Ignore) t.Ignore = tm.Ignore;
                        break;
                    }
                }
                if (!isExist) list.Add(b as T);
            }
            return list;
        }
        /// <summary>
        /// 初始化，获取所有特性
        /// </summary>
        public static Type[] Initialization()
        {
            if (Attrs != null) return Attrs;
            string assemblyName = "Song.ViewData";
            Assembly assembly = Assembly.Load(assemblyName);
            Attrs = assembly.GetExportedTypes()
                .Where(t => t.FullName.StartsWith("Song.ViewData.Attri") && !t.IsAbstract)
                .OrderBy(c => c.Name).ToArray();
            return Attrs;
        }
    }
}
