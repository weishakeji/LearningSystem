using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ServiceImpls.Cache
{
    /// <summary>
    /// 用于缓存一些常用实体
    /// </summary>
    public class EntitiesCache
    {
        /// <summary>
        /// 生成缓存的键名
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static string Key(string tablename, params object[] param)
        {
            StringBuilder suffix = new StringBuilder();
            for(int i = 0; i < param.Length; i++)
            {
                suffix.Append(param[i].ToString());
                if (i < param.Length - 1) suffix.Append("_");
            }
            return string.Format("WEISHAKEJI_EntitiesCache_{0}_{1}", tablename, suffix.ToString()).ToUpper();
        }

        private static readonly object _lock = new object();
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="param"></param>
        public static void Save<T>(object data, params object[] param) where T : WeiSha.Data.Entity
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            if (cache == null) return;
           
            lock (_lock)
            {
                string tablename = typeof(T).Name;
                string cachekey = Key(tablename, param);
                cache.Insert(cachekey, data);
            }      
        }
        /// <summary>
        /// 清除缓存内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        public static void Clear<T>(params object[] param) where T : WeiSha.Data.Entity
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            if (cache == null) return;

            lock (_lock)
            {
                string tablename = typeof(T).Name;
                string cachekey = Key(tablename, param);
                cache.Remove(cachekey);
            }
        }
        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object Get<T>(params object[] param) where T : WeiSha.Data.Entity
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            if (cache == null) return null;

            string tablename = typeof(T).Name;      
            string cachekey = Key(tablename, param);

            object cachevalue = cache.Get(cachekey);
            if (cachevalue != null) return cachevalue;
            return null;
        }
    }
}
