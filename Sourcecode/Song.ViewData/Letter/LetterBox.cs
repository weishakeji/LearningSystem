using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Song.ViewData
{
    /// <summary>
    /// letter的缓存管理
    /// </summary>
    public class LetterBox
    {
        //过期时间，单位秒
        private static int _expires = 60;
        private static string _cacheName = "Letter_";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionid"></param>
        /// <param name="letter"></param>
        public static void Insert(long sessionid, Letter letter)
        {       
            //过期时间
            DateTime expTime = DateTime.Now.AddSeconds(_expires);
            string cacheName = _cacheName + sessionid;
            HttpRuntime.Cache.Insert(cacheName, letter, null, expTime, TimeSpan.Zero);
        }
        public static Letter Get(long sessionid)
        {
            return Get(sessionid.ToString());
        }
        public static Letter Get(string sessionid)
        {
            string cacheName = _cacheName + sessionid;
            Object obj=HttpRuntime.Cache.Get(cacheName);
            if (obj == null || !(obj is Letter)) return null;
            return (Letter)obj;           
        }
        /// <summary>
        /// 缓存中的所有Letter
        /// </summary>
        /// <returns></returns>
        public static List<Letter> Collects()
        {
            List<Letter> list = new List<Letter>();
            // 遍历缓存中的所有项
            foreach (DictionaryEntry entry in HttpRuntime.Cache)
            {
                string key = (string)entry.Key;
                if (!key.StartsWith(_cacheName)) continue;
                object value = entry.Value;
                if (value == null || !(value is Letter)) continue;

                //
                list.Add((Letter)value);
            }
            return list;
        }
    }
}
