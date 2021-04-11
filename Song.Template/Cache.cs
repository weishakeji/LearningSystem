using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Song.Template
{
    /// <summary>
    /// 缓存
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// 缓存项集合
        /// </summary>
        public static  List<Cache_Item> Items = new List<Cache_Item>();
        private static int _TemplateCacheTime = WeiSha.Common.App.Get["TemplateCacheTime"].Int32 ?? 60;

        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Add(string key, string value)
        {
            Cache_Item item = Get(key); //获取当前模板缓存项
            if (item == null)
            {
                item = new Cache_Item(key, DateTime.Now, value);
                Items.Add(item);
            }
            else
            {
                item.CreateTime = DateTime.Now;
                item.Value = value;
            }
        }
        /// <summary>
        /// 添加缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="time"></param>
        /// <param name="value"></param>
        public static void Add(string key, DateTime time, string value)
        {
            Cache_Item item = Get(key); //获取当前模板缓存项
            if (item == null)
            {
                item = new Cache_Item(key, time, value);
                Items.Add(item);
            }
            else
            {
                item.CreateTime = time;
                item.Value = value;
            }
        }
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static Cache_Item Get(string key)
        {
            Refresh();
            Cache_Item item = null;
            foreach (Cache_Item it in Items)
            {
                if (it.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    item = it;
                    break;
                }
            }
            return item;
        }
        /// <summary>
        /// 获取缓存项的内容
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            Cache_Item item = Get(key);           
            return item!=null ? item.Value : "";
        }
        /// <summary>
        /// 刷新缓存列表
        /// </summary>
        public static void Refresh()
        {
            DateTime overTime = DateTime.Now.AddSeconds(-_TemplateCacheTime);    //过期时间
            //if (WeiSha.Common.Server.IsLocalIP) overTime = DateTime.Now;    //如果是在本地调试，则不作缓存
            for (int i = Items.Count-1; i >= 0; i--)
            {
                Cache_Item item = Items[i];
                if (item.CreateTime < overTime)
                {
                    Items.Remove(item);
                }
            }
        }
        /// <summary>
        /// 移除缓存项
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            for (int i = Items.Count-1; i >= 0; i--)
            {
                Cache_Item item = Items[i];
                if (item.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    Items.Remove(item);
                }
            }
        }
        public static void Clear()
        {
            for(int i = 0; i < Items.Count; i++)
            {
                Items.Remove(Items[i]);
                i--;
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExists(string key)
        {
            Refresh();
            bool isExt = false;
            foreach (Cache_Item it in Items)
            {
                if (it.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    isExt = true;
                    break;
                }
            }
            return isExt;
        }
    }
    public class Cache_Item
    {
        /// <summary>
        /// 缓存项的键值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 缓存创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 缓存项的value
        /// </summary>
        public string Value { get; set; }

        public Cache_Item(string key, DateTime time, string value)
        {
            Key = key;
            CreateTime = time;
            Value = value;
        }
    }
}
