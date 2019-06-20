using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;


namespace Song.ViewData.Methods
{
    
    /// <summary>
    /// 管理账号
    /// </summary>
    [HttpGet]
    public class Account
    {
        [HttpGet(IsAllow = false)]
        public Song.Entities.EmpAccount Single(int id)
        {
            Song.Entities.EmpAccount acc = Business.Do<IEmployee>().GetSingle(id);
            return acc;
        }
        public Song.Entities.EmpAccount Single()
        {
            Song.Entities.EmpAccount acc = Business.Do<IEmployee>().GetSingle(1);
            return acc;
        }
        public Song.Entities.EmpAccount[] List()
        {
            return Business.Do<IEmployee>().GetAll(-1);
            //return Business.Do<IEmployee>().Save
        }
        public ListResult Pager(int index, int size)
        {
            int sum = 0;
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().AccountsPager(-1, size, index, out sum);
            Song.ViewData.ListResult result = new ListResult(accs);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        public bool Save(Letter p)
        {
            int count = p.Params.Count;
            foreach (KeyValuePair<string, string> kv in p)
            {
                string key = kv.Key;
                string val = kv.Value;
            }
            //填充参数到实体
            //Song.Entities.EmpAccount acc = Business.Do<IEmployee>().GetSingle(id);
            //Parameter.FillEntity(acc);
            return true;
        }
        public int Save(Letter p, out int id)
        {
            int count = p.Params.Count;
            id = 13;
            //填充参数到实体
            //Song.Entities.EmpAccount acc = Business.Do<IEmployee>().GetSingle(id);
            //Parameter.FillEntity(acc);
            return count;
        }
    }
}
