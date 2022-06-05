using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 方法是否支持缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : WeishaAttr
    {
        //缓存名称，依次为
        //二级域名，类名，方法名，参数集
        private static string _cacheName = "DataCache/{0}/{1}/{2}:{3}";
        private int _expires = 10;
        /// <summary>
        /// 过期时间，单位分钟
        /// </summary>
        public int Expires
        {
            get { return _expires; }
            set
            {
                _expires = value;
            }
        }

        private bool _adminDisable = false;
        /// <summary>
        /// 管理员是否不用缓存，为true时不用缓存，为false继续用缓存
        /// </summary>
        public bool AdminDisable
        {
            get;
            set;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static object GetResult(MethodInfo method, Letter letter)
        {
            //如果是本机，不缓存数据
            bool islocal = WeiSha.Core.Server.IsLocalIP;
            if (islocal) return null;
            //获取缓存数据
            string domain = WeiSha.Core.Request.Domain.TwoDomain;
            string cacheName = string.Format(_cacheName, domain, method.DeclaringType.Name, method.Name, letter.ToString());
            return HttpRuntime.Cache.Get(cacheName);
        }
        /// <summary>
        /// 移除和某个缓存
        /// </summary>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        public static void Remove(MethodInfo method, Letter letter)
        {
            string domain = WeiSha.Core.Request.Domain.TwoDomain;
            string cacheName = string.Format(_cacheName, domain, method.DeclaringType.Name, method.Name, letter.ToString());
            HttpRuntime.Cache.Remove(cacheName);
        }
        /// <summary>
        /// 创建缓存
        /// </summary>
        /// <param name="expires"></param>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        /// <param name="result"></param>
        public static void Insert(int expires, MethodInfo method, Letter letter, object result)
        {
            if (result == null) return;
            //缓存名称
            string domain = WeiSha.Core.Request.Domain.TwoDomain;
            string cacheName = string.Format(_cacheName, domain, method.DeclaringType.Name, method.Name, letter.ToString());
            //过期时间
            DateTime expTime = DateTime.Now.AddMinutes(expires);            
            HttpRuntime.Cache.Insert(cacheName, result, null, expTime, TimeSpan.Zero);
        }
        
    }
}
