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
    public class Account : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 根据ID查询学员账号
        /// </summary>
        /// <remarks>为了安全，返回的对象密码不显示</remarks>
        /// <param name="id"></param>
        /// <returns>学员账户的映射对象</returns>    
        public Song.Entities.Accounts ForID(int id)
        {          
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(id);
            return _tran(acc);
        }
        /// <summary>
        /// 当前登录的学员
        /// </summary>
        /// <remarks>登录状态通过cookies或session保持</remarks>
        /// <returns></returns>
        [Student]
        public Song.Entities.Accounts Current()
        {
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            return _tran(acc);
        }
        /// <summary>
        /// 根据账号获取学员，不支持模糊查询
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <returns></returns>
        public Song.Entities.Accounts ForAcc(string acc)
        {
            if (string.IsNullOrWhiteSpace(acc)) throw new Exception("学员账号不得为空");
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsSingle(acc, -1);
            return _tran(account);
        }
        /// <summary>
        /// 根据学员名称获取学员信息，支持模糊查询
        /// </summary>
        /// <param name="name">学员名称</param>
        /// <returns></returns>
        public Song.Entities.Accounts[] ForName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new Exception("学员名称不得为空");
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().Account4Name(name);
            if (accs == null) return accs;
            for (int i = 0; i < accs.Length; i++)
            {
                accs[i] = _tran(accs[i]);
            }
            return accs;
        }
        /// <summary>
        /// 按账号和姓名查询学员
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <param name="name">姓名，可模糊查询</param>
        /// <returns></returns>
        public Song.Entities.Accounts[] Seach(string acc, string name)
        {
            List<Song.Entities.Accounts> list = new List<Accounts>();
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().Account4Name(name);
            foreach (Song.Entities.Accounts ac in accs)
                list.Add(ac);
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsSingle(acc, -1);
            if (account != null)
            {
                bool isExist = false;
                foreach (Song.Entities.Accounts ac in accs)
                {
                    if (ac.Ac_ID == account.Ac_ID)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist) list.Add(account);
            }
            //
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = _tran(list[i]);
            }
            return list.ToArray<Accounts>();
        }
        /// <summary>
        /// 从学习记录中获取学员记录
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <param name="name">学员姓名</param>
        /// <returns></returns>
        public Song.Entities.LogForStudentStudy[] ForLogs(string acc, string name)
        {
            string sql = @"select Ac_ID,Ac_AccName, Ac_Name from (
                    select logs.* from Accounts right join 
                    (select * from LogForStudentStudy where 
                    {name} and {acc}) as logs
                    on Accounts.Ac_ID=Logs.Ac_ID) as tm
                     group by Ac_ID,Ac_AccName,Ac_Name";
            sql = sql.Replace("{name}", string.IsNullOrWhiteSpace(name) ? "1=1" : "Ac_Name like '%" + name + "%'");
            sql = sql.Replace("{acc}", string.IsNullOrWhiteSpace(acc) ? "1=1" : "Ac_AccName='" + acc + "'");

            Song.Entities.LogForStudentStudy[] accs = Business.Do<ISystemPara>().ForSql<LogForStudentStudy>(sql).ToArray<LogForStudentStudy>();
            return accs;
        }
        /// <summary>
        /// 学员的视频学习记录
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public Song.Entities.LogForStudentStudy[] StudyLog(int acid, int couid)
        {
            return Business.Do<IStudent>().LogForStudyCount(-1, couid, -1, acid, null, 0);
        }
        /// <summary>
        /// 分页获取学员信息
        /// </summary>
        /// <param name="index">页码，即第几页</param>
        /// <param name="size">每页多少条记录</param>
        /// <returns></returns>
        public ListResult Pager(int index, int size)
        {
            int sum = 0;
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().AccountsPager(-1, size, index, out sum);
            for (int i = 0; i < accs.Length; i++)
            {
                accs[i] = _tran(accs[i]);
            }
            Song.ViewData.ListResult result = new ListResult(accs);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        #region 私有方法，处理学员信息
        /// <summary>
        /// 处理学员信息，密码清空、头像转为全路径，并生成clone对象
        /// </summary>
        /// <param name="acc">学员账户的clone对象</param>
        /// <returns></returns>
        private Song.Entities.Accounts _tran(Song.Entities.Accounts acc)
        {
            if (acc == null) return acc;
            Song.Entities.Accounts curr = acc.Clone<Song.Entities.Accounts>();
            if (curr != null) curr.Ac_Pw = string.Empty;
            curr.Ac_Photo = WeiSha.Common.Upload.Get["Accounts"].Virtual + curr.Ac_Photo;
            return curr;
        }
        #endregion

        #region 学员组
        /// <summary>
        /// 学员组的信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult SortPager(int index, int size)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sum = 0;
            Song.Entities.StudentSort[] accs = Business.Do<IStudent>().SortPager(org.Org_ID, true, null, size, index, out sum);
            Song.ViewData.ListResult result = new ListResult(accs);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        #endregion
    }
}
