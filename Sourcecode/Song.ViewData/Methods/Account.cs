using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;


namespace Song.ViewData.Methods
{

    /// <summary>
    /// 账号
    /// </summary>
    [HttpGet]
    public class Account : ViewMethod, IViewAPI
    { 
        //资源的虚拟路径和物理路径
        public static string PathKey = "Accounts";
        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
        #region 登录注册相关
        /// <summary>
        /// 当前登录的学员
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public Song.Entities.Accounts Current()
        {          
            return LoginAccount.Status.User(this.Letter);           
        }
        public int atest()
        {
            int total = 0;
            //foreach (System.Web.Caching.Cache c in HttpRuntime.Cache)
            //{
            //    total++;
            //    //string key=c.
            //}

            //HttpRuntime.Cache
            System.Collections.IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                total++;
                string key = cacheEnum.Key.ToString();
                if (key.StartsWith("AccountBuffer_"))
                {
                    object val = cacheEnum.Value;
                }
                //cacheEnum.Key.ToString()为缓存名称，cacheEnum.Value为缓存值 
            }
            return total;
        }
        /// <summary>
        /// 刷新登录状态
        /// </summary>
        /// <returns>返回登录状态信息，包含登录账号与时效，以加密方式</returns>
        [HttpPost]
        public string Fresh()
        {
            return LoginAccount.Status.Fresh(this.Letter);
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="acc">验证</param>
        /// <param name="pw">密码</param>
        /// <param name="vcode">提交的验证码</param>
        /// <param name="vmd5">用于验证码的加密字符串</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts Login(string acc, string pw, string vcode, string vmd5)
        {
            string val = new Song.ViewData.ConvertToAnyValue(acc + vcode).MD5;
            if (!val.Equals(vmd5, StringComparison.CurrentCultureIgnoreCase))
                throw VExcept.Verify("验证码错误", 101);
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (account == null) throw VExcept.Verify("密码错误或账号不存在", 102);
            if (!(bool)account.Ac_IsUse) throw VExcept.Verify("当前账号被禁用", 103);
            LoginAccount.Add(account);

            //克隆当前对象,用于发向前端
            Song.Entities.Accounts user = account.DeepClone<Song.Entities.Accounts>();
            user.Ac_Photo = System.IO.File.Exists(PhyPath + user.Ac_Photo) ? VirPath + user.Ac_Photo : "";
            //登录，密码被设置成加密状态值
            user.Ac_CheckUID = account.Ac_CheckUID;
            user.Ac_Pw = LoginAccount.Status.Generate_checkcode(account);
            return user;
        }
        /// <summary>
        /// 注册学员
        /// </summary>
        /// <param name="acc">账号</param>
        /// <param name="pw">密码（明文）</param>
        /// <param name="rec">推荐人的账号，可以为空</param>
        /// <param name="recid">推荐人id</param>
        /// <param name="vcode">校验码（明文）</param>
        /// <param name="vcodemd5">校验码的密文</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts Register(string acc, string pw, string rec, int recid, string vcode, string vcodemd5)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            if (!(bool)(config["IsRegStudent"].Value.Boolean ?? false))
            {

            }
            string val = new Song.ViewData.ConvertToAnyValue(org.Org_PlatformName + vcode).MD5;
            if (!val.Equals(vcodemd5, StringComparison.CurrentCultureIgnoreCase))
                throw VExcept.Verify("校验码错误", 101);
            //账号是否存在
            try
            {
                Song.Entities.Accounts account = Business.Do<IAccounts>().IsAccountsExist(acc);
                if (account == null && Regex.IsMatch(acc, @"^1\d{10}$"))
                    account = Business.Do<IAccounts>().IsAccountsExist(-1, acc, 1);
                if (account != null) throw VExcept.Verify("账号已经存在", 102);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //
            //创建新账户
            Song.Entities.Accounts tmp = new Entities.Accounts();
            tmp.Ac_AccName = acc;
            tmp.Ac_Pw = new Song.ViewData.ConvertToAnyValue(pw).MD5;
            //获取推荐人
            try
            {
                Song.Entities.Accounts accRec = null;
                if (!string.IsNullOrWhiteSpace(rec)) accRec = Business.Do<IAccounts>().AccountsForMobi(rec, -1, true, true);
                if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
                if (accRec != null && accRec.Ac_ID != tmp.Ac_ID)
                {
                    tmp.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                    Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //如果需要审核通过
            tmp.Ac_IsPass = !(bool)(config["IsVerifyStudent"].Value.Boolean ?? false);
            tmp.Ac_IsUse = tmp.Ac_IsPass;
            try
            {
                int id = Business.Do<IAccounts>().AccountsAdd(tmp);
                //登录，密码被设置成加密状态值
                tmp.Ac_Pw = LoginAccount.Status.Generate_checkcode(tmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            LoginAccount.Add(tmp);
            return tmp;
        }
        /// <summary>
        /// 校验证学员安全问题是否回答正确
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <param name="answer">安全问题的答案</param>
        /// <returns></returns>
        public Song.Entities.Accounts CheckQues(string acc, string answer)
        {
            if (string.IsNullOrWhiteSpace(acc)) throw new Exception("学员账号不得为空");
            Song.Entities.Accounts account = Business.Do<IAccounts>().IsAccountsExist(-1, acc, answer);
            return _tran(account);
        }

        #endregion

        #region 获取账号
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
        /// 通过学员身份证号来获取学员
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Song.Entities.Accounts ForIDCard(string card)
        {
            if (string.IsNullOrWhiteSpace(card)) throw new Exception("信息不得为空");
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsForIDCard(card, -1, null, null);
            return _tran(account);
        }
        /// <summary>
        /// 通过学员的手机号来获取学员信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public Song.Entities.Accounts ForMobi(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) throw new Exception("信息不得为空");
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsForMobi(phone, -1, null, null);
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
        /// 分页获取学员信息
        /// </summary>
        /// <param name="index">页码，即第几页</param>
        /// <param name="size">每页多少条记录</param>
        /// <returns></returns>
        public ListResult Pager(int index, int size)
        {
            int sum = 0;
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().AccountsPager(-1, size, index, null, out sum);
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
        /// <summary>
        /// 所有账号列表
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">检索的字符</param>      
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult PagerOfAll(int orgid, string search, int index, int size)
        {
            int sum = 0;
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().AccountsPager(orgid, -1, null, search, search, search, size, index, out sum);
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
            if (curr != null)
            {
                curr.Ac_Pw = string.Empty;
                curr.Ac_CheckUID = string.Empty;
                curr.Ac_Ans = string.Empty;
            }
            curr.Ac_Photo = System.IO.File.Exists(PhyPath + curr.Ac_Photo) ? VirPath + curr.Ac_Photo : "";

            return curr;
        }
        #endregion
        #endregion

        #region 修改账号信息
        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <param name="acc">员工账号的实体</param>
        /// <returns></returns>
        [Admin]
        [Student]
        [HttpPost]
        public bool Modify(Accounts acc)
        {
            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(acc.Ac_ID);
            if (old == null) throw new Exception("Not found entity for Accounts！");
            //账号，密码，登录状态值，不更改
            old.Copy<Song.Entities.Accounts>(acc, "Ac_AccName,Ac_Pw,Ac_CheckUID");
            Business.Do<IAccounts>().AccountsSave(old);
            Song.ViewData.LoginAccount.Fresh(old);
            return true;
        }
        /// <summary>
        ///  修改账号信息
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        [Admin]
        [Student]
        [HttpPost]
        public bool ModifyJson(JObject acc)
        {
            int acid = 0;
            int.TryParse(acc["Ac_ID"].ToString(), out acid);
            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(acid);
            if (old == null) throw new Exception("Not found entity for Accounts！");

            old.Copy<Song.Entities.Accounts>(acc);
            Business.Do<IAccounts>().AccountsSave(old);
            Song.ViewData.LoginAccount.Fresh(old);
            return true;
        }
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="use">使用状态</param>
        /// <param name="pass">审核状态</param>
        /// <returns>学员id</returns>
        [HttpPost]
        [Admin,SuperAdmin]
        public int ModifyState(int acid, bool use, bool pass)
        {
            try
            {
                Business.Do<IAccounts>().AccountsUpdate(acid,
                    new WeiSha.Data.Field[] {
                        Song.Entities.Accounts._.Ac_IsUse,
                        Song.Entities.Accounts._.Ac_IsPass },
                    new object[] { use, pass });
                return acid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改学员的照片
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [Upload(Extension = "jpg,png,gif", MaxSize = 512, CannotEmpty = true)]
        [Admin, Student]
        public Accounts ModifyPhoto(Accounts account)
        {
            string filename = string.Empty;
            try
            {
                //只保存第一张图片
                foreach (string key in this.Files)
                {
                    HttpPostedFileBase file = this.Files[key];
                    filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                    file.SaveAs(PhyPath + filename);
                    break;
                }

                Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(account.Ac_ID);
                if (old == null)
                {
                    account.Ac_Photo = System.IO.File.Exists(PhyPath + account.Ac_Photo) ? VirPath + account.Ac_Photo : "";
                    return account;
                }
                if (!string.IsNullOrWhiteSpace(old.Ac_Photo))
                {
                    string filehy = PhyPath + old.Ac_Photo;
                    try
                    {
                        //删除原图
                        if (System.IO.File.Exists(filehy))
                            System.IO.File.Delete(filehy);
                        //删除缩略图，如果有
                        string smallfile = WeiSha.Core.Images.Name.ToSmall(filehy);
                        if (System.IO.File.Exists(smallfile))
                            System.IO.File.Delete(smallfile);
                    }
                    catch { }
                }
                old.Ac_Photo = filename;
                //Business.Do<IAccounts>().AccountsSave(old);
                Business.Do<IAccounts>().AccountsUpdate(old,new WeiSha.Data.Field[] { 
                    Song.Entities.Accounts._.Ac_Photo
                },new object[] { filename });
                Song.ViewData.LoginAccount.Fresh(old.Ac_ID);
                //
                old.Ac_Photo = System.IO.File.Exists(PhyPath + old.Ac_Photo) ? VirPath + old.Ac_Photo : "";
                return old;
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }
        /// <summary>
        /// 修改自身账号信息
        /// </summary>
        /// <param name="acc">账号的实体</param>
        /// <returns></returns>
        [Student]
        [HttpPost]
        [HttpGet(Ignore = true)]
        public bool ModifySelf(Accounts acc)
        {
            Song.Entities.Accounts self = LoginAccount.Status.User(this.Letter);
            //不是自身账号
            if (self.Ac_ID != acc.Ac_ID) throw new Exception("It's not your own account！");

            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(self.Ac_ID);
            //账号，密码，登录状态值，不更改
            old.Copy<Song.Entities.Accounts>(acc, "Ac_AccName,Ac_Pw,Ac_CheckUID");
            try
            {
                Business.Do<IAccounts>().AccountsSave(old);
                Song.ViewData.LoginAccount.Fresh(old);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改登录密码
        /// </summary>
        /// <param name="oldpw">原密码</param>
        /// <param name="newpw">新密码</param>
        /// <returns></returns>
        [Student]
        [HttpPost]
        [HttpGet(Ignore = true)]
        public bool ModifyPassword(string oldpw, string newpw)
        {
            Song.Entities.Accounts self = LoginAccount.Status.User(this.Letter);
            if (self == null) return false;
            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(self.Ac_ID);
            string md5pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(oldpw).MD5;
            if (!old.Ac_Pw.Equals(md5pw, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("原密码不正确");                
            }
            string md5new = new WeiSha.Core.Param.Method.ConvertToAnyValue(newpw).MD5;
            Business.Do<IAccounts>().AccountsUpdate(old, new WeiSha.Data.Field[] { Song.Entities.Accounts._.Ac_Pw },
                new object[] { md5new });
            return true;
        }
        /// <summary>
        /// 重置学员登录密码
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <param name="pw">新密码，为明文发送</param>
        /// <returns></returns>
        [Admin,SuperAdmin]
        [HttpPost]
        [HttpGet(Ignore = true)]
        public bool ResetPassword(int acid, string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
                throw new Exception("密码不得为空");
            string md5new = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            Business.Do<IAccounts>().AccountsUpdate(acid, new WeiSha.Data.Field[] { Song.Entities.Accounts._.Ac_Pw },
             new object[] { md5new });
            return true;
        }
        /// <summary>
        /// 通过安全问题修改密码
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <param name="answer">安全问题的答案</param>
        /// <param name="pw">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public bool ModifyPwForAnswer(string acc, string answer, string pw)
        {
            if (string.IsNullOrWhiteSpace(pw)) throw new Exception("新密码为空");
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(acc, -1);
            if (st == null) throw new Exception("账号不存在");
            if (!answer.Equals(st.Ac_Ans, StringComparison.OrdinalIgnoreCase))
                throw new Exception("安全问题回答不正确");
            try
            {
                Business.Do<IAccounts>().AccountsUpdatePw(st, pw);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return false;
        }
        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int Delete(string id)
        {         
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IAccounts>().AccountsDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;           
        }
        #endregion

        #region 学习记录
        /// <summary>
        /// 从学习记录中获取学员记录
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <param name="name">学员姓名</param>
        /// <returns></returns>
        public Song.Entities.LogForStudentStudy[] ForLogs(string acc, string name)
        {
            if (string.IsNullOrWhiteSpace(acc) && string.IsNullOrWhiteSpace(name)) return null;
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
        #endregion

        #region 学员组
        /// <summary>
        /// 获取学员组的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.StudentSort SortForID(int id)
        {
            return Business.Do<IStudent>().SortSingle(id);
        }
        /// <summary>
        /// 获取默认学员组
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public Song.Entities.StudentSort SortDefault(int orgid)
        {
            if (orgid < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            return Business.Do<IStudent>().SortDefault(orgid);
        }
        /// <summary>
        /// 学员组下的学员数量
        /// </summary>
        /// <param name="sortid">学员组的id</param>
        /// <returns></returns>
        public int SortOfNumber(int sortid)
        {
            return Business.Do<IStudent>().SortOfNumber(sortid);
        }
        /// <summary>
        /// 添加学员组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool SortAdd(Song.Entities.StudentSort entity)
        {
            try
            {
                Business.Do<IStudent>().SortAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改学员组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool SortModify(StudentSort entity)
        {
            Song.Entities.StudentSort old = Business.Do<IStudent>().SortSingle(entity.Sts_ID);
            if (old == null) throw new Exception("Not found entity for StudentSort！");

            old.Copy<Song.Entities.StudentSort>(entity);
            Business.Do<IStudent>().SortSave(old);
            return true;
        }
        /// <summary>
        /// 设置默认学员组
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool SortSetDefault(int orgid, int id)
        {
            if (orgid < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            try
            {
                Business.Do<IStudent>().SortSetDefault(orgid, id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }
        /// <summary>
        /// 删除学员组
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int SortDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IStudent>().SortDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 学员组的信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="use"></param>
        /// <param name="search"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult SortPager(int orgid, bool? use, string search, int index, int size)
        {
            if (orgid < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            int sum = 0;
            Song.Entities.StudentSort[] accs = Business.Do<IStudent>().SortPager(orgid, use, search, size, index, out sum);
            Song.ViewData.ListResult result = new ListResult(accs);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 获取当前机构下的所有分组
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="use">是否启用</param>
        /// <returns></returns>
        [HttpGet]
        public StudentSort[] SortAll(int orgid, bool? use)
        {
            if (orgid < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            return Business.Do<IStudent>().SortAll(orgid, use);
        }
        /// <summary>
        /// 更改分学员组的排序
        /// </summary>
        /// <param name="items">链接学员组的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool SortUpdateTaxis(Song.Entities.StudentSort[] items)
        {
            try
            {
                Business.Do<IStudent>().SortUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 导出学员信息       
        /// <summary>
        /// 生成excel,按机构导出
        /// </summary>
        /// <param name="organs">机构id,多个id用逗号分隔</param>
        /// <param name="path"></param> 
        /// <returns></returns>
        public JObject ExcelOutputForOrg(string organs)
        {
            string outputPath = "AccountsToExcelForOrgan";
            //导出文件的位置
            string rootpath = Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("账号导出{0}.({1}).xls", organs, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IAccounts>().AccountsExport4Excel(filePath, organs);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", Upload.Get["Temp"].Virtual + outputPath + "/" + filename);
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 生成excel，按学员组导出
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sorts">分组id,多个id用逗号分隔</param> 
        /// <returns></returns>
        public JObject ExcelOutputForSort(int orgid,string sorts)
        {
            string outputPath = "AccountsToExcelForSort";
            //导出文件的位置
            string rootpath = Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("账号导出{0}.({1}).xls", sorts, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IAccounts>().AccountsExport4Excel(filePath, orgid, sorts, false);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", Upload.Get["Temp"].Virtual + outputPath + "/" + filename);
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool ExcelDelete(string filename,string path)
        {
            string rootpath = Upload.Get["Temp"].Physics + path + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            string filePath = rootpath + filename;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(string path)
        {
            string rootpath = Upload.Get["Temp"].Physics + path + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            JArray jarr = new JArray();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            foreach (System.IO.FileInfo f in dir.GetFiles("*.xls"))
            {
                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", Upload.Get["Temp"].Virtual + path + "/" + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 导入学员信息
        /// <summary>
        /// 学员导入
        /// </summary>
        /// <param name="xls"></param>
        /// <param name="sheet"></param>
        /// <param name="config"></param>
        /// <param name="matching"></param>
        /// <returns>success:成功数;error:失败数</returns>
        public JObject ExcelImport(string xls, int sheet, string config, JArray matching)
        {
            //获取Excel中的数据
            string phyPath = WeiSha.Core.Upload.Get["Temp"].Physics;
            DataTable dt = ViewData.Helper.Excel.SheetToDatatable(phyPath + xls, sheet, config);

            //当前机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //学员组
            List<Song.Entities.StudentSort> sorts = Business.Do<IStudent>().SortCount(org.Org_ID, null, 0);
            //开始导入，并计数
            int success = 0, error = 0;
            List<DataRow> errorDataRow = new List<DataRow>();
            List<Exception> errorOjb = new List<Exception>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //throw new Exception();
                    //将数据逐行导入数据库
                    _inputData(dt.Rows[i], org, sorts, matching);
                    success++;
                }
                catch(Exception ex)
                {
                    //如果出错，将错误行计数
                    error++;
                    errorDataRow.Add(dt.Rows[i]);
                    errorOjb.Add(ex);
                }
            }
            JObject jo = new JObject();
            jo.Add("success", success);
            jo.Add("error", error);
            //错误数据
            JArray jarr = new JArray();
            for(int i = 0; i < errorDataRow.Count; i++)
            {
                DataRow dr = errorDataRow[i];
                JObject jrow = new JObject();
                foreach(DataColumn dc in dr.Table.Columns)
                {
                    jrow.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                jrow.Add("exception", errorOjb[i].Message);
                jarr.Add(jrow);
            }
            jo.Add("datas", jarr);
            return jo;
        }
        /// <summary>
        /// 将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="org"></param>
        /// <param name="sorts"></param>
        /// <param name="mathing">excel列与字段的匹配关联</param>
        private void _inputData(DataRow dr, Song.Entities.Organization org, List<Song.Entities.StudentSort> sorts, JArray mathing)
        {
            Song.Entities.Accounts obj = null;
            bool isExist = false;
            for (int i = 0; i < mathing.Count; i++)
            {
                //Excel的列的值
                string column = dr[mathing[i]["column"].ToString()].ToString();
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                if (field == "Ac_AccName")
                {
                    obj = Business.Do<IAccounts>().AccountsSingle(column, -1);
                    isExist = obj != null;
                    continue;
                }
            }
            if (obj == null) obj = new Entities.Accounts();
            for (int i = 0; i < mathing.Count; i++)
            {
                //Excel的列的值
                string column = dr[mathing[i]["column"].ToString()].ToString();
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                if (field == "Ac_Sex")
                {
                    obj.Ac_Sex = (short)(column == "男" ? 1 : (column == "女" ? 2 : 0));
                    continue;
                }
                PropertyInfo[] properties = obj.GetType().GetProperties();
                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo pi = properties[j];
                    if (field == pi.Name && !string.IsNullOrEmpty(column))
                    {
                        pi.SetValue(obj, Convert.ChangeType(column, pi.PropertyType), null);
                    }
                }
            }
            //设置分组
            if (!string.IsNullOrWhiteSpace(obj.Sts_Name)) obj.Sts_ID = _getSortsId(sorts, org, obj.Sts_Name);
            if (!string.IsNullOrWhiteSpace(obj.Ac_Pw)) obj.Ac_Pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(obj.Ac_Pw).MD5;
            obj.Org_ID = org.Org_ID;
            obj.Ac_IsPass = true;
            obj.Ac_IsUse = true;
            if (isExist)
            {
                Business.Do<IAccounts>().AccountsSave(obj);
                Song.ViewData.LoginAccount.Fresh(obj);
            }
            else
            {
                Business.Do<IAccounts>().AccountsAdd(obj);
            }
        }
        /// <summary>
        /// 获取分组id
        /// </summary>
        /// <param name="sorts"></param>
        /// <param name="org"></param>
        /// <param name="sortName"></param>
        /// <returns></returns>
        private int _getSortsId(List<Song.Entities.StudentSort> sorts, Song.Entities.Organization org, string sortName)
        {
            try
            {
                int sortId = 0;
                foreach (Song.Entities.StudentSort s in sorts)
                {
                    if (sortName.Trim() == s.Sts_Name)
                    {
                        sortId = s.Sts_ID;
                        break;
                    }
                }
                if (sortId == 0 && sortName.Trim() != "")
                {
                    Song.Entities.StudentSort nwsort = new Song.Entities.StudentSort();
                    nwsort.Sts_Name = sortName;
                    nwsort.Sts_IsUse = true;
                    nwsort.Org_ID = org.Org_ID;
                    Business.Do<IStudent>().SortAdd(nwsort);
                    sortId = nwsort.Sts_ID;
                    sorts.Add(nwsort);
                }
                return sortId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
