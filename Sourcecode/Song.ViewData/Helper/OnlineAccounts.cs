using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// 在线登录的账号
    /// </summary>
    public class OnlineAccounts
    {
        //
        public static readonly List<Song.Entities.Accounts> list = new List<Entities.Accounts>();
        private static object _lock = new object();
    }
}
