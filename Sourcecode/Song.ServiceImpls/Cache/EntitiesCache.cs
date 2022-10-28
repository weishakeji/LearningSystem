using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

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
        public static object GetObject<T>(params object[] param) where T : WeiSha.Data.Entity
        {
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;          
            if (cache == null) return null;

            string tablename = typeof(T).Name;      
            string cachekey = Key(tablename, param);

            object cachevalue = cache.Get(cachekey);
            if (cachevalue != null) return cachevalue;
            return null;
        }
        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T GetEntity<T>(params object[] param) where T : WeiSha.Data.Entity
        {
            object obj = GetObject<T>(param);
            if (obj == null) return default(T);
            return (T)obj;
        }
        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(params object[] param) where T : WeiSha.Data.Entity
        {
            object obj = GetObject<T>(param);
            if (obj == null) return default;
            if (obj is List<T>) return (List<T>)obj;
            return default;
        }
        /// <summary>
        /// 获取缓存的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable GetDataTable<T>(params object[] param) where T : WeiSha.Data.Entity
        {
            object obj = GetObject<T>(param);
            if (obj == null) return null;
            if (obj is List<T>)
            {
                List<T> list = (List<T>)obj;
                if (list.Count < 1) return null;

                //取出第一个实体的所有Propertie
                Type entityType = list[0].GetType();
                PropertyInfo[] properties = entityType.GetProperties();

                DataTable dt = new DataTable(typeof(T).Name);
                for (int i = 0; i < properties.Length; i++)              
                    dt.Columns.Add(properties[i].Name, properties[i].PropertyType);
                //将所有entity添加到DataTable中
                foreach (object entity in list)
                {
                    //检查所有的的实体都为同一类型
                    if (entity.GetType() != entityType)
                        throw new Exception("要转换的集合元素类型不一致");
                    object[] entityValues = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)                 
                        entityValues[i] = properties[i].GetValue(entity, null);
                    dt.Rows.Add(entityValues);
                }
                return dt;
            }
            return default;
        }
    }
}
