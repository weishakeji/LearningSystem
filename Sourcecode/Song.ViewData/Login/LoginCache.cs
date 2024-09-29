using Song.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;
using System.Threading;

namespace Song.ViewData
{
    public class LoginCache
    {
        private string _cachaName = "LoginState_Cache_{0}_List";
        //private Type _type;
        //定时任务
        private Timer timer;
        /// <summary>
        /// 登录的类型
        /// </summary>
        public LoginTokenType TokenType { get; set; }
        private LoginCache(LoginTokenType logintype)
        {
            this.TokenType = logintype;
            //this._type = type;
            this._cachaName = String.Format(this._cachaName, this.TokenType);
        }
        /// <summary>
        /// 创建一个登录缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logintype"></param>
        /// <returns></returns>
        public static LoginCache Get<T>(LoginTokenType logintype) where T : WeiSha.Data.Entity
        {
            //Type type = typeof(T);
            LoginCache cache= new LoginCache(logintype);
            // 每隔60分钟触发一次事件
            cache.timer = new Timer(state => cache.ClearExpired<T>(state), null, TimeSpan.Zero, TimeSpan.FromMinutes(60));
            return cache;
        }
        private readonly object _lock_list = new object();
        /// <summary>
        /// 缓存列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<long, T> List<T>() where T : WeiSha.Data.Entity
        {
            MemoryCache cache = MemoryCache.Default;
            Dictionary<long, T> dic = cache.Get(_cachaName) as Dictionary<long, T>;
            if (dic == null) dic = new Dictionary<long, T>();
            return dic;
        }
        private Dictionary<long, T> Update<T>(Dictionary<long, T> dic) where T : WeiSha.Data.Entity
        {
            MemoryCache cache = MemoryCache.Default;
            if (cache.Contains(_cachaName)) cache.Remove(_cachaName);
            cache.Add(_cachaName, dic, DateTimeOffset.UtcNow.AddYears(2));
            return dic;
        }
        /// <summary>
        /// 添加在线账号
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="id"></param>
        public void Add<T>(T acc, long id) where T : WeiSha.Data.Entity
        {

            if (acc == null) return;
            if (this.TokenType == LoginTokenType.Admin && acc is Song.Entities.EmpAccount)
                (acc as Song.Entities.EmpAccount).Acc_LastTime = DateTime.Now;
            if (this.TokenType == LoginTokenType.Account && acc is Song.Entities.Accounts)
                (acc as Song.Entities.Accounts).Ac_LastTime = DateTime.Now;

            lock (_lock_list)
            {
                Dictionary<long, T> dic = List<T>();
                if (dic.ContainsKey(id)) dic[id] = acc;
                else dic.Add(id, acc);
                Task.Run(() => Update<T>(dic));
            }
        }
        /// <summary>
        /// 清除掉某个登录账号
        /// </summary>
        /// <param name="id"></param>
        public void Remove<T>(long id) where T : WeiSha.Data.Entity
        {
            Dictionary<long, T> dic = List<T>();
            lock (_lock_list)
            {               
                if (dic.ContainsKey(id)) dic.Remove(id);
            }
            Task.Run(() => Update<T>(dic));          
        }
        /// <summary>
        /// 获取内存中的当前登录账号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(long id) where T : WeiSha.Data.Entity
        {
            Dictionary<long, T> dic = List<T>();
            if (dic.ContainsKey(id)) return dic[id];
            return null;
        }
        /// <summary>
        /// 清理过期
        /// </summary>
        public void ClearExpired<T>(object state) where T : WeiSha.Data.Entity
        {
            lock (_lock_list)
            {
                Dictionary<long, T> dic = List<T>();
                List<long> keys = new List<long>(dic.Keys);
                for (int i = 0; i < keys.Count; i++)
                {
                    long id = keys[i];
                    if (!dic.ContainsKey(id)) continue;
                    T acc = dic[id];
                    //学员账号的清理
                    if (this.TokenType == LoginTokenType.Account && acc is Song.Entities.Accounts)
                    {
                        Song.Entities.Accounts accs = acc as Song.Entities.Accounts;
                        if (accs.Ac_LastTime > DateTime.Now.AddMinutes(-60))
                        {
                            dic.Remove(id);
                            i--;
                        }
                    }
                    //管理员账号的清理
                    if (this.TokenType == LoginTokenType.Admin && acc is Song.Entities.EmpAccount)
                    {
                        Song.Entities.EmpAccount emp = acc as Song.Entities.EmpAccount;
                        if (emp.Acc_LastTime > DateTime.Now.AddMinutes(-60))
                        {
                            dic.Remove(id);
                            i--;
                        }
                    }
                }
                Task.Run(() => Update<T>(dic));
            }
        }        
    }
}
