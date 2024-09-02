using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Web;
using System.Data.HashFunction.FNV;
using System.Data.HashFunction;

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

        //private bool _adminDisable = false;
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
            if (!WeiSha.Core.RESTfulAPI.Get.EnableCache) return null;
            //如果是本机，不缓存数据
            bool islocal = WeiSha.Core.Server.IsLocalIP;
            if (islocal) return null;
            string cacheName = _getCacheName(method, letter);
            return HttpRuntime.Cache.Get(cacheName);
        }
        private static readonly object _lock = new object();
        /// <summary>
        /// 移除和某个缓存
        /// </summary>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        public static void Remove(MethodInfo method, Letter letter)
        {
            string cacheName = _getCacheName(method, letter);
            lock (_lock) HttpRuntime.Cache.Remove(cacheName);
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
            if (!WeiSha.Core.RESTfulAPI.Get.EnableCache) return;
            //如果是本机，不缓存数据
            bool islocal = WeiSha.Core.Server.IsLocalIP;
            if (islocal) return;
            //缓存名称        
            string cacheName = _getCacheName(method, letter);
            //过期时间
            DateTime expTime = DateTime.Now.AddMinutes(expires);
            lock (_lock) HttpRuntime.Cache.Insert(cacheName, result, null, expTime, TimeSpan.Zero);
        }
        #region 私有方法
        /// <summary>
        /// 一个速度很快的哈希算法
        /// </summary>
        /// <param name="str">要加密的字符</param>
        /// <returns>哈希值的十六进制字符串</returns>
        private static string _FNV1a(string str)
        {
            IFNV1a fnv1a = FNV1aFactory.Instance.Create();
            IHashValue hashValue = fnv1a.ComputeHash(Encoding.UTF8.GetBytes(str));
            return hashValue.AsHexString();
        }
        /// <summary>
        /// 获取缓存名称
        /// </summary>
        /// <param name="method"></param>
        /// <param name="letter"></param>
        /// <returns></returns>
        private static string _getCacheName(MethodInfo method, Letter letter)
            => _FNV1a(string.Format(_cacheName, letter.HTTP_HOST, method.DeclaringType.Name, method.Name, letter.ToString()));
        
        #endregion
    }
}
