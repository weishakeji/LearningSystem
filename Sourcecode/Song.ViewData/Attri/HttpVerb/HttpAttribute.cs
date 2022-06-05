using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// HTTP谓词限制特性的父类
    /// </summary>
    public class HttpAttribute : WebAttribute
    {
       
        /// <summary>
        /// 验证是否满足特性的限定
        /// </summary>
        /// <param name="httpmethod">http请求方法，即谓词</param>
        /// <param name="method">执行的方法</param>
        /// <returns></returns>
        public static HttpAttribute Verify(string httpmethod, MemberInfo method)
        {
            if ("cache".Equals(httpmethod,StringComparison.CurrentCultureIgnoreCase)) return null;
            //如果方法没有设置任何HTTP限制，则不验证
            List<HttpAttribute> https = WeishaAttr.GetAttrs<HttpAttribute>(method);
            if (https.Count < 1) return null;

            //如果是第一次运行，获取所有特性
            Type[] attrs = Initialization();
            //通过http请求的谓词，获取需要验证的特性
            string httpattrname = string.Format("http{0}Attribute", httpmethod);
            Type type = null;
            foreach (Type att in attrs)
            {             
                if (att.Name.Equals(httpattrname, StringComparison.CurrentCultureIgnoreCase))
                {
                    type = att;
                    break;
                }
            }
            //验证特性
            HttpAttribute attr = null;
            //先验证对象，如果对象需验证，则下面方法全部需要验证登录，除非方法设置了[(Ignore = true)]
            object[] attrsObj = method.DeclaringType.GetCustomAttributes(type, true);
            if (attrsObj.Length > 0) attr = attrsObj[0] as HttpAttribute;
            //再验证方法
            object[] attrsMethod = method.GetCustomAttributes(type, true);
            if (attrsMethod.Length > 0)
            {
                HttpAttribute admin = attrsMethod[0] as HttpAttribute;
                if (attr == null) attr = admin;
                if (attr != null && admin.Ignore) attr.Ignore = admin.Ignore;
            }
            if (attr == null || attr.Ignore)
                throw new Exception(string.Format("当前接口方法 {0}.{1} 禁止 HTTP {2} 请求",
                    method.DeclaringType.Name, method.Name, httpmethod.ToUpper()));
            return attr;
        }
    }
}
