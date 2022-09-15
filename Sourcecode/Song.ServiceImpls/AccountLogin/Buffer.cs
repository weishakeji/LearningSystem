using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Song.ServiceInterfaces;
using WeiSha.Core;

namespace Song.ServiceImpls.AccountLogin
{
    public class Buffer
    {
        /// <summary>
        /// 缓存名称的前缀
        /// </summary>
        public static readonly string prefix = "AccountBuffer_";
        /// <summary>
        /// 生成缓存的名称
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GenerateName(string str)
        {
            //最大长度
            int maxLength = 6;
            if (str.Length >= maxLength)
                return str.Substring(0, 6);
            while (str.Length < maxLength)          
                str += "_"; 
            return str;
        }
        private static object _lock = new object();
        public static List<Song.Entities.Accounts> GetList(string name)
        {
            lock (_lock)
            {
                string cache_name = prefix + GenerateName(name);
                List<Song.Entities.Accounts> list = HttpRuntime.Cache.Get(cache_name) as List<Song.Entities.Accounts>;
                if (list == null) list = new List<Entities.Accounts>();
                return list;
            }
        }
        public static List<Song.Entities.Accounts> SetList(Song.Entities.Accounts acc)
        {
            lock (_lock)
            {
                string cache_name = prefix + GenerateName(acc.Ac_AccName);
                List<Song.Entities.Accounts> list = GetList(cache_name);
                bool exist = false;
               for(int i = 0; i < list.Count; i++)
                {
                    if (list[i].Ac_ID == acc.Ac_ID)
                    {
                        exist = true;
                        list[i] = acc;
                        break;
                    }
                }
                if (!exist) list.Add(acc);
                HttpRuntime.Cache.Insert(cache_name, list);
                return list;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            int size = 1000;
            int total = Business.Do<IAccounts>().AccountsOfCount(-1, true);
            int pager = total % size == 0 ? total / size : total / size + 1;
            for(int i = 1; i <= pager; i++)
            {
                Thread t1 = new Thread(() =>
                {
                    try
                    {
                        int sum = 0;
                        Song.Entities.Accounts[] accs = Business.Do<IAccounts>().AccountsPager(-1, null, size, i, out sum);
                        foreach(Song.Entities.Accounts acc in accs)
                        {
                            SetList(acc);
                        }
                    }
                    catch (Exception ex)
                    {
                        WeiSha.Core.Log.Error("AccountLogin.Buffer", ex);
                    }
                });
                t1.Start();
            }
        }
    }
}
