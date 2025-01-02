using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
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
using Song.ViewData;


namespace Song.ViewData.Methods
{

    /// <summary>
    /// 账号管理，涉及学员账号的注册、登录（包括第三方账号登录）、修改、列表查询；以及学员组等
    /// </summary>
    [HttpPut, HttpGet]
    public class Account : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        private static string _pathKey = "Accounts";
        private static string _virPath = WeiSha.Core.Upload.Get[_pathKey].Virtual;
        private static string _phyPath = WeiSha.Core.Upload.Get[_pathKey].Physics;
        #region 登录注册相关
        /// <summary>
        /// 当前登录的学员
        /// </summary>
        /// <returns></returns> 
        [HttpPost]
        public Song.Entities.Accounts Current()
        {
            Song.Entities.Accounts acc = LoginAccount.Status.User(this.Letter);
            return acc;           
        }
        /// <summary>
        /// 账号缓存数量
        /// </summary>
        /// <returns></returns>
        public int BufferCount()
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
        /// <param name="span">每次刷新的时间间隔，单位秒</param>
        /// <returns>返回登录状态信息，包含登录账号与时效，以加密方式</returns>
        [HttpPost]
        public string Fresh(int span)
        {  
            return LoginAccount.Status.Fresh(this.Letter, span);
        }
        /// <summary>
        /// 清除CheckUID
        /// </summary>
        /// <param name="acc">账号或手机号</param>
        /// <param name="type">mobi为手机号，acc为账号</param>
        /// <returns></returns>
        public bool ClearCheckUID(string acc, string type)
        {
            Song.Entities.Accounts account = null;
            if (type == "mobi")
                account = Business.Do<IAccounts>().AccountsForMobi(acc, -1, null, null);
            if (type == "acc")
                account = Business.Do<IAccounts>().AccountsSingle(acc, -1);
            if (acc != null)
                Business.Do<IAccounts>().AccountsUpdate(account,
                    new WeiSha.Data.Field[] { Song.Entities.Accounts._.Ac_CheckUID },
                    new object[] { "" });
            return true;
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
            string val = ConvertToAnyValue.Create(acc + vcode).MD5;
            if (!val.Equals(vmd5, StringComparison.CurrentCultureIgnoreCase))
                throw VExcept.Verify("验证码错误", 101);
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (account == null) throw VExcept.Verify("密码错误或账号不存在", 102);
            if (!(bool)account.Ac_IsUse) throw VExcept.Verify("当前账号被禁用", 103);
            //LoginAccount.CacheAdd(account);
           
            //克隆当前对象,用于发向前端
            Song.Entities.Accounts user = account.DeepClone<Song.Entities.Accounts>();
            user.Ac_Photo = System.IO.File.Exists(_phyPath + user.Ac_Photo) ? _virPath + user.Ac_Photo : "";
            //登录，密码被设置成加密状态值
            user.Ac_CheckUID = account.Ac_CheckUID;
            //user.Ac_Pw = LoginAccount.Status.Generate_checkcode(account, this.Letter);
            user.Ac_Pw= LoginAccount.Status.Login(user);
            return user;
        }
        /// <summary>
        /// 记录登录信息
        /// </summary>
        /// <param name="source">页面请求信息，例如：电脑网页、手机网页、微信小程序</param>
        /// <param name="info">登录信息，这里指登录方式，例如：账号密码登录、微信登录、QQ登录</param>
        [HttpPost]
        public void LoginLog(string source, string info)
        {
            Letter letter = this.Letter;
            Song.Entities.Accounts user = LoginAccount.Status.User(letter);
            if (user == null) return;
            //添加登录记录
            Business.Do<IStudent>().LogForLoginAdd(user, source, info, letter.IP, letter.Longitude, letter.Latitude);
        }
        /// <summary>
        /// 退出登录
        /// </summary>
        [HttpPost]
        public void Logout()
        {
            Letter letter = this.Letter;
            Song.Entities.Accounts user = LoginAccount.Status.User(letter);
            if (user == null) return;
            Business.Do<IStudent>().LogForLoginOut(user, string.Empty);
        }
        /// <summary>
        /// 短信登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="sms"></param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts LoginSms(string phone, string sms)
        {
            string val = ConvertToAnyValue.Create(phone + sms).MD5;
            Song.Entities.Accounts account = Business.Do<IAccounts>().AccountsLoginSms(phone, val);
            if (account == null) throw VExcept.Verify("验证码不正确", 105);
            if (!(bool)account.Ac_IsUse) throw VExcept.Verify("当前账号被禁用", 103);
            LoginAccount.Cache.Add<Accounts>(account, account.Ac_ID);         
            //克隆当前对象,用于发向前端
            Song.Entities.Accounts user = account.DeepClone<Song.Entities.Accounts>();
            user.Ac_Photo = System.IO.File.Exists(_phyPath + user.Ac_Photo) ? _virPath + user.Ac_Photo : "";
            //登录，密码被设置成加密状态值
            user.Ac_CheckUID = account.Ac_CheckUID;
            user.Ac_Pw = LoginAccount.Status.Generate_Checkcode(account);
            return user;
        }
        /// <summary>
        /// 注册学员
        /// </summary>
        /// <param name="acc">账号</param>
        /// <param name="pw">密码（明文）</param>
        /// <param name="rec">推荐人的账号，可以为空</param>
        /// <param name="recid">推荐人id</param>
        /// <param name="code">校验码（明文）</param>
        /// <param name="vcode">校验码的密文</param>
        /// <param name="sms">短信验证码</param>
        /// <param name="vsms">短信验证码的密文</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts Register(string acc, string pw, string rec, int recid, string code, string vcode, string sms, string vsms)
        {
            //当前机构的配置信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            if ((bool)(config["IsRegStudent"].Value.Boolean ?? false)) return null;
            //验证图形校验码
            if (!this.RegisterVerifyVcode(code, vcode)) throw VExcept.Verify("校验码错误", 101);
            //验证短信验证码
            if ((bool)(config["IsRegSms"].Value.Boolean ?? false))
                if (!this.RegisterSMSVcode(sms, vsms)) throw VExcept.Verify("短信验证错误", 102);

            //账号是否存在
            Song.Entities.Accounts account = Business.Do<IAccounts>().IsAccountsExist(acc);
            if (account == null && Regex.IsMatch(acc, @"^1\d{10}$"))
                account = Business.Do<IAccounts>().IsAccountsExist(-1, acc, 1);
            if (account != null) throw VExcept.Verify("账号已经存在", 102);
            //
            //创建新账户
            Song.Entities.Accounts tmp = new Entities.Accounts();
            tmp.Ac_AccName = acc;
            tmp.Ac_Pw = ConvertToAnyValue.Create(pw).MD5;
            //是否是手机号注册
            if ((bool)(config["IsRegSms"].Value.Boolean ?? false))
                tmp.Ac_MobiTel1 = tmp.Ac_MobiTel2 = acc;
            //获取推荐人
            Song.Entities.Accounts accRec = null;
            if (!string.IsNullOrWhiteSpace(rec)) accRec = Business.Do<IAccounts>().AccountsForMobi(rec, -1, true, true);
            if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
            if (accRec != null && accRec.Ac_ID != tmp.Ac_ID)
            {
                tmp.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
            }
            //如果需要审核通过
            tmp.Ac_IsPass = !(bool)(config["IsVerifyStudent"].Value.Boolean ?? false);
            tmp.Ac_IsUse = tmp.Ac_IsPass;
            //生成登录校验码
            tmp.Ac_CheckUID = LoginAccount.Status.Generate_Checkcode(tmp);
            tmp.Ac_ID = Business.Do<IAccounts>().AccountsAdd(tmp);
            _tran(tmp);
            if (tmp.Ac_IsPass)
            {
                tmp = Business.Do<IAccounts>().AccountsLogin(tmp);
                tmp.Ac_Pw = LoginAccount.Status.Generate_Checkcode(tmp);
                LoginAccount.Cache.Add<Accounts>(tmp, tmp.Ac_ID);
            }
            return tmp;
        }
        /// <summary>
        /// 校验注册时的验证码是否正确
        /// </summary>
        /// <param name="code">录入的校验码</param>
        /// <param name="vcode">加密的校验码</param>
        /// <returns>正确返回true</returns>
        public bool RegisterVerifyVcode(string code, string vcode)
        {
            //当前机构的配置信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            string val = ConvertToAnyValue.Create(org.Org_PlatformName + code).MD5;
            return val.Equals(vcode, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// 验证短信验证码
        /// </summary>
        /// <param name="sms">验证码</param>
        /// <param name="vsms">验证码的密文</param>
        /// <returns></returns>
        public bool RegisterSMSVcode(string sms, string vsms)
        {
            //当前机构的配置信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            string val = ConvertToAnyValue.Create(org.Org_PlatformName + sms).SHA256;
            return val.Equals(vsms, StringComparison.CurrentCultureIgnoreCase);
        }
        /// <summary>
        /// 账号注册后的修改
        /// </summary>
        /// <param name="acc">账号的实体</param>
        /// <param name="acid">账号的id</param>
        /// <param name="uid">账号的校验值</param>
        /// <returns></returns>    
        [HttpPost]
        [HtmlClear(Not = "acc")]
        [Upload(Config = "AccountPhoto")]
        public bool RegisterModify(Accounts acc, int acid,string uid)
        {
            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(acid);
            if (old == null) throw new Exception("Not found entity for Accounts！");
            //校验值是否正确
            //Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(old.Org_ID);

            if(!uid.Equals(old.Ac_CheckUID,StringComparison.OrdinalIgnoreCase)) throw new Exception("校验值不合法！");
            //注册后一个小时内可以修改
            if (old.Ac_RegTime.AddHours(-1) > DateTime.Now) throw new Exception("编辑超时！");
            //图片
            string filename = _uploadLogo();
            //如果有上传的图片，且之前也有图片,则删除原图片
            if ((!string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(acc.Ac_Photo)) && !string.IsNullOrWhiteSpace(old.Ac_Photo))
                WeiSha.Core.Upload.Get[_pathKey].DeleteFile(old.Ac_Photo);
            if (!string.IsNullOrWhiteSpace(filename)) old.Ac_Photo = filename;
            //账号，密码，登录状态值，不更改
            old.Copy<Song.Entities.Accounts>(acc, "Ac_Pw,Ac_CheckUID");
            if (!string.IsNullOrWhiteSpace(filename)) old.Ac_Photo = filename;
            Business.Do<IAccounts>().AccountsSave(old);
            Song.ViewData.LoginAccount.Status.Fresh(old);
            return true;
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
        /// <summary>
        /// 默认密码，当管理员添加学员时的默认密码
        /// </summary>
        /// <returns></returns>
        [HttpGet,Admin,SuperAdmin]
        public string DefaultPw()
        {
            return WeiSha.Core.Login.Get["Account"].DefaultPw.Value;
        }
        #endregion

        #region 登录日志
        /// <summary>
        /// 获取账号登录信息的记录
        /// </summary>
        /// <param name="id">记录id</param>
        /// <returns></returns>
        public Song.Entities.LogForStudentOnline LoginLogForID(int id)
        {
            return Business.Do<IStudent>().LogForLoginSingle(id);

        }
        /// <summary>
        /// 删除账号登录信息的记录
        /// </summary>
        /// <param name="id">登录信息记录的id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int LoginLogDelete(string id)
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
                    Business.Do<IStudent>().StudentOnlineDelete(idval);
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
        /// 账号登录信息的记录
        /// </summary>
        /// <param name="acid">账号id</param>
        /// <param name="start">查询时间区间的起始日期</param>
        /// <param name="end">查询时间区间的结束日期</param>
        /// <param name="index">分页的索引</param>
        /// <param name="size">分页数据的每页记录数</param>
        /// <returns></returns>
        public ListResult LoginLogs(int acid, DateTime? start, DateTime? end, int index, int size)
        {
            int sum = 0;
            Song.Entities.LogForStudentOnline[] list = Business.Do<IStudent>().LogForLoginPager(acid, string.Empty, start, end, size, index, out sum);
           
            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 按机构获取学员登录日志
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="name">学员姓名</param>
        /// <param name="acname">学员账号</param>
        /// <param name="start">查询时间区间的起始时间</param>
        /// <param name="end">查询时间区间的结束时间</param>
        /// <param name="index">分页的索引</param>
        /// <param name="size">分页数据的每页记录数</param>
        /// <returns></returns>
        public ListResult LoginLogs(int orgid,string name,string acname, DateTime? start, DateTime? end, int index, int size)
        {
            int sum = 0;
            Song.Entities.LogForStudentOnline[] list = Business.Do<IStudent>().LogForLoginPager(orgid, 0, string.Empty, start, end, name, acname, size, index, out sum);

            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 登录日志的统计信息，如果province与city都为空，取省级单位的数据
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="province">省份，当前省份下的所有地市数据</param>
        /// <param name="city">地市名称，当前地市下所有区县数据</param>
        /// <returns></returns>
        public JArray LoginLogsSummary(int orgid, DateTime? start, DateTime? end, string province, string city)
        {
            JArray jarr = new JArray();
            //登录日志的统计数据
            DataTable dt = Business.Do<IStudent>().LoginLogsSummary(orgid, start, end, province, city);
            //行政区划(数据来自/Utilities/AreaCodeInfo.csv)
            Dictionary<int, string> area = WeiSha.Core.LBS.Provinces();
            if (string.IsNullOrWhiteSpace(province) && string.IsNullOrWhiteSpace(city))
                area = WeiSha.Core.LBS.Provinces();
            else if (!string.IsNullOrWhiteSpace(province))
                area = WeiSha.Core.LBS.Cities(province);
            else if (!string.IsNullOrWhiteSpace(city))
                area = WeiSha.Core.LBS.Districts(city);
            //补全统计数据的行政区划信息，
            //例如当某个行政单位没有访问记录时，在dt中是没有这一项的，将从area中补全该行政单位
            foreach (KeyValuePair<int, string> pair in area)
            {
                DataRow row = dt.AsEnumerable().FirstOrDefault(dr => pair.Value.Equals(dr.IsNull("area") ? "" : dr["area"].ToString()));
                jarr.Add(new JObject(
                            new JProperty("area", pair.Value),
                            new JProperty("code", pair.Key),
                            new JProperty("count", row != null ? Convert.ToInt32(row["count"].ToString()) : 0)
                      ));
                if (row != null) dt.Rows.Remove(row);
            }
            //其它区域，没有在行政区划内的数值，可能是没有采集到地理信息
            //int sum = dt.AsEnumerable().Sum(row => row.Field<int>("count"));
            int sum = 0;
            foreach(DataRow dr in dt.Rows)
                sum += dr.IsNull("count") ? 0 : Convert.ToInt32(dr["count"].ToString());
           
            if (sum > 0)
            {
                jarr.Add(new JObject(
                           new JProperty("area", "Other"),
                           new JProperty("code", -1),
                           new JProperty("count", sum)
                     ));
            }
            return jarr;
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
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">学员分组id</param>
        /// <param name="use">是否启用</param>
        /// <param name="acc">账号</param>
        /// <param name="name">姓名</param>
        /// <param name="phone">电话</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="gender">性别,0为所有，1为男，2为女</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="orderpattr">排序方式，asc或desc</param> 
        /// <param name="index">页码，即第几页</param>
        /// <param name="size">每页多少条记录</param>
        /// <returns></returns>
        public ListResult Pager(int orgid, long sortid, bool? use, string acc, string name, string phone,string idcard,int gender, string orderby, string orderpattr, int index, int size)
        {
            int sum = 0;
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().AccountsPager(orgid, sortid, use, acc, name, phone, idcard, gender, orderby, orderpattr, size, index, out sum);
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
            Song.Entities.Accounts[] accs = Business.Do<IAccounts>().AccountsPager(orgid, -1, null, search, search, search, search, -1, string.Empty, string.Empty, size, index, out sum);
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
            curr.Ac_Photo = System.IO.File.Exists(_phyPath + curr.Ac_Photo) ? _virPath + curr.Ac_Photo : "";

            return curr;
        }
        #endregion
        #endregion

        #region 修改账号信息
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="acc">账号名称</param>
        /// <param name="id">账号id，如果账号已经存在，则不判断自身</param>
        /// <returns></returns>
        [HttpGet]
        public bool IsExistAcc(string acc, int id)
        {
            return Business.Do<IAccounts>().IsAccountExist(acc, id);
        }
        /// <summary>
        /// 新增学员
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        [HttpPost]
        [Upload(Config = "AccountPhoto")]
        [Admin, SuperAdmin]
        [HtmlClear(Not = "acc")]
        public Accounts Add(Accounts acc)
        {
            acc.Ac_Photo = _uploadLogo();
            if (!string.IsNullOrWhiteSpace(acc.Ac_Pw))
                acc.Ac_Pw = ConvertToAnyValue.Create(acc.Ac_Pw).MD5;
            Business.Do<IAccounts>().AccountsAdd(acc);
            acc.Ac_Photo = System.IO.File.Exists(_phyPath + acc.Ac_Photo) ? _virPath + acc.Ac_Photo : "";
            return acc;
        }
        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <param name="acc">员工账号的实体</param>
        /// <returns></returns>
        [Admin]
        [Student]
        [HttpPost][HtmlClear(Not = "acc")]
        [Upload(Config = "AccountPhoto")]
        public bool Modify(Accounts acc)
        {
            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(acc.Ac_ID);
            if (old == null) throw new Exception("Not found entity for Accounts！");
            //图片
            string filename = _uploadLogo();
            //如果有上传的图片，且之前也有图片,则删除原图片
            if ((!string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(acc.Ac_Photo)) && !string.IsNullOrWhiteSpace(old.Ac_Photo))
                WeiSha.Core.Upload.Get[_pathKey].DeleteFile(old.Ac_Photo);      
            if (!string.IsNullOrWhiteSpace(filename)) old.Ac_Photo = filename;
            //账号，密码，登录状态值，不更改
            old.Copy<Song.Entities.Accounts>(acc, "Ac_Pw,Ac_CheckUID");
            if (!string.IsNullOrWhiteSpace(filename)) old.Ac_Photo = filename;
            Business.Do<IAccounts>().AccountsSave(old);
            Song.ViewData.LoginAccount.Status.Fresh(old);
            return true;
        }
        /// <summary>
        ///  图片上传的处理方法
        /// </summary>
        /// <returns></returns>
        private string _uploadLogo()
        {
            string filename = string.Empty;
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(_phyPath + filename);
                break;
            }
            //转jpg
            if (!string.IsNullOrWhiteSpace(filename))
            {
                if (!".jpg".Equals(Path.GetExtension(filename), StringComparison.CurrentCultureIgnoreCase))
                {
                    string old = filename;
                    using (System.Drawing.Image image = WeiSha.Core.Images.FileTo.ToImage(_phyPath + filename))
                    {
                        filename = Path.ChangeExtension(filename, "jpg");
                        image.Save(_phyPath + Path.ChangeExtension(filename, "jpg"), ImageFormat.Jpeg);
                    }
                    System.IO.File.Delete(_phyPath + old);
                }
            }
            return filename;
        }
        /// <summary>
        ///  修改账号信息
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        [Admin]
        [Student]
        [HttpPost]
        [HtmlClear(Not = "acc")]
        public bool ModifyJson(JObject acc)
        {
            int acid = 0;
            int.TryParse(acc["Ac_ID"].ToString(), out acid);
            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(acid);
            if (old == null) throw new Exception("Not found entity for Accounts！");

            old.Copy<Song.Entities.Accounts>(acc);
            Business.Do<IAccounts>().AccountsSave(old);
            Song.ViewData.LoginAccount.Status.Fresh(old);
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
        [Upload(Config = "AccountPhoto", Required = true)]
        [Admin, Student]
        public Accounts ModifyPhoto(Accounts account)
        {
            string filename = string.Empty;
            //只保存第一张图片
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                string phyfile = _phyPath + filename;
                file.SaveAs(phyfile);              
                WeiSha.Core.Images.FileTo.Zoom(phyfile, 200, 200, false);
                break;
            }

            Song.Entities.Accounts old = Business.Do<IAccounts>().AccountsSingle(account.Ac_ID);
            if (old == null)
            {
                account.Ac_Photo = System.IO.File.Exists(_phyPath + account.Ac_Photo) ? _virPath + account.Ac_Photo : "";
                return account;
            }
            if (!string.IsNullOrWhiteSpace(old.Ac_Photo))
            {
                string filehy = _phyPath + old.Ac_Photo;
                try
                {
                    //删除原图
                    if (System.IO.File.Exists(filehy)) System.IO.File.Delete(filehy);
                    //删除缩略图，如果有
                    string smallfile = WeiSha.Core.Images.Name.ToSmall(filehy);
                    if (System.IO.File.Exists(smallfile)) System.IO.File.Delete(smallfile);
                }
                catch { }
            }
            old.Ac_Photo = filename;
            Business.Do<IAccounts>().AccountsUpdate(old.Ac_ID, new WeiSha.Data.Field[] {
                    Song.Entities.Accounts._.Ac_Photo
                }, new object[] { filename });
            Song.ViewData.LoginAccount.Status.Fresh(old);
            //
            old.Ac_Photo = System.IO.File.Exists(_phyPath + old.Ac_Photo) ? _virPath + old.Ac_Photo : "";
            return old;
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
                Song.ViewData.LoginAccount.Status.Fresh(old);
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
        /// 批量删除账户
        /// </summary>
        /// <param name="ids">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int DeleteBatch(string ids)
        {         
            int i = 0;
            if (string.IsNullOrWhiteSpace(ids)) return i;
            string[] arr = ids.Split(',');
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
        /// <summary>
        /// 按id删除账户
        /// </summary>
        /// <param name="id">账户id</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int Delete(int id)
        {
            Business.Do<IAccounts>().AccountsDelete(id);
            LoginAdmin.Cache.Remove<Accounts>(id); //清理登录状态
            return id;
        }
        #endregion


        #region 学员组
        /// <summary>
        /// 获取学员组的对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Song.Entities.StudentSort SortForID(long id)
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
        public int SortOfNumber(long sortid)
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
        /// 学员组是否存在
        /// </summary>
        /// <param name="name">账号名称</param>
        /// <param name="id">账号id，如果名称已经存在，则不断判断自身</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        [HttpGet]
        public bool SortIsExist(string name, long id,int orgid)
        {
            return Business.Do<IStudent>().SortIsExist(name, id, orgid);
        }
        /// <summary>
        /// 修改学员组的使用状态
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <param name="use">是否启用</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool SortUpdateUse(long stsid, bool use)
        {
            return Business.Do<IStudent>().SortUpdateUse(stsid, use);           
        }
        /// <summary>
        /// 设置默认学员组
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">学员组id</param>
        /// <returns></returns>
        public bool SortSetDefault(int orgid, long id)
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
                long idval = 0;
                long.TryParse(s, out idval);
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
        /// <summary>
        /// 移除某个学员组下的学员
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int SortRemoveStudent(long stsid, string id)
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
                    Business.Do<IAccounts>().AccountsUpdate(idval,
                        new WeiSha.Data.Field[] { Song.Entities.Accounts._.Sts_ID, Song.Entities.Accounts._.Sts_Name },
                        new object[] { 0, "" });                    
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            Business.Do<IStudent>().SortUpdateCount(stsid);
            return i;
        }
        /// <summary>
        /// 新增某个学员组下的学员
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public int SortAddStudent(long stsid, string id)
        {
            Song.Entities.StudentSort sort = Business.Do<IStudent>().SortSingle(stsid);
            if (sort == null) throw new Exception("Not found entity for StudentSort！");
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
                    Business.Do<IAccounts>().AccountsUpdate(idval,
                        new WeiSha.Data.Field[] { Song.Entities.Accounts._.Sts_ID, Song.Entities.Accounts._.Sts_Name },
                        new object[] { sort.Sts_ID, sort.Sts_Name });
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            Business.Do<IStudent>().SortUpdateCount();
            return i;
        }
        /// <summary>
        /// 更新学员组中的学员数量
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <returns></returns>
        public int SortUpdateCount(long stsid)
        {
            return Business.Do<IStudent>().SortUpdateCount(stsid);
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
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("账号导出{0}.({1}).xls", organs, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IAccounts>().AccountsExport4Excel(filePath, organs);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + filename);
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
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("账号导出{0}.({1}).xls", sorts, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IAccounts>().AccountsExport4Excel(filePath, orgid, sorts, false);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + filename);
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
        public bool ExcelDelete(string filename, string path)
            => ViewData.Helper.Excel.DeleteFile(filename, path, "Temp");
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(string path)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + path + "\\";
            JArray jarr = new JArray();
            if (!System.IO.Directory.Exists(rootpath)) return jarr;
            
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            FileInfo[] files = dir.GetFiles("*.xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + path + "/" + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 学员成绩导出
        /// <summary>
        /// 学员的学习成果
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="search">按课程搜索</param>
        /// <param name="start">按时间区间查询时，选修课程的开始时间</param>
        /// <param name="end">按时间区间查询时，选修课程的开始时间的结束</param>
        /// <param name="size">每页多少条</param>
        /// <param name="index">第几页</param>      
        /// <returns>Student_Course、Course、Accounts三个表的数据合集</returns>
        public ListResult Outcomes4Student(int acid, long sbjid, string search, DateTime? start, DateTime? end, int size, int index)
        {
            int sum;
            DataTable dt= Business.Do<IStudent>().Outcomes4Student(acid, sbjid, search, start, end, size, index, out sum);
            Song.ViewData.ListResult result = new ListResult(dt);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 学员选修的课程的专业信息
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <returns></returns>
        public DataTable Subject4Student(int acid)
        {
            return Business.Do<IStudent>().Subject4Student(acid);
        }
        /// <summary>
        /// 学员选修的课程的个数
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <returns></returns>
        public int CourseCount(int acid)
        {
            return Business.Do<IStudent>().CourseCount(acid);
        }
        #endregion

        #region 学员组成绩导出
        /// <summary>
        /// 学员组的学员的学习成果
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <param name="learned">是否包括未学习的学员，如果为false，则仅导出已经参与学习的</param>
        /// <param name="all">学员组所有学员的学习成绩，包括自主选修的，如果为false，则仅包括学员组选修的课程</param>
        /// <returns></returns>
        public JObject SortOutcomesToExcel(long stsid, bool learned, bool all)
        {
            string outputPath = "SortOutcomesToExcel";
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + stsid.ToString() + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("学习成果{0}.({1}).xls", stsid, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IStudent>().LearningOutcomesToExcel(filePath, stsid, learned, all);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + stsid.ToString() + "/" + filename);
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 已经生成的员组的学员的学习成果Excel文件
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray SortOutcomesExcelFiles(long stsid)
        {
            string outputPath = "SortOutcomesToExcel";
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + stsid.ToString() + "\\";
            JArray jarr = new JArray();
            if (!System.IO.Directory.Exists(rootpath)) return jarr;          
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            FileInfo[] files = dir.GetFiles("*.xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + stsid.ToString() + "/" + f.Name);
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
        /// <param name="xls">服务器端的excel文件名，即上传后的excel</param>
        /// <param name="sheet"></param>
        /// <param name="config">配置文件</param>
        /// <param name="matching">excel列与字段的匹配关联</param>
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
            //手机号,如果第一个手机号不为空，且第二个手机号为空，则设置相等。相等的意思即绑定手机号，用于短信登录
            if (!string.IsNullOrWhiteSpace(obj.Ac_MobiTel1) && string.IsNullOrWhiteSpace(obj.Ac_MobiTel2))
                obj.Ac_MobiTel2 = obj.Ac_MobiTel1;
            if (isExist)
            {
                Business.Do<IAccounts>().AccountsSave(obj);
                Song.ViewData.LoginAccount.Status.Fresh(obj);
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
        private long _getSortsId(List<Song.Entities.StudentSort> sorts, Song.Entities.Organization org, string sortName)
        {
            try
            {
                long sortId = 0;
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

        #region 选修课程
        /// <summary>
        ///  直接开课，创建学员与课程的关联信息
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="couid">课程id</param>
        /// <returns>关联的课程数</returns>
        public int BeginCourse(int stid, DateTime start, DateTime end, long[] couid)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int i = 0;
            foreach (long c in couid)
            {
                Business.Do<ICourse>().BeginCourse(stid, start, end, c, org.Org_ID);
                i++;
            }
            return i;
        }
        /// <summary>
        /// 购买过课程的学员
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <param name="acc"></param>
        /// <param name="name"></param>
        /// <param name="idcard"></param>
        /// <param name="mobi"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns>Ac_CurrCourse列为学员选修的课程数</returns>
        public ListResult PurchasePager(int orgid, long stsid, long couid,
            string acc, string name, string idcard, string mobi,
           DateTime? start, DateTime? end, int size, int index)
        {
            int sum = 0;
            List<Accounts> accs = Business.Do<IStudent>().PurchasePager(orgid, stsid, couid, acc, name, idcard, mobi,start,end, size, index, out sum);
            for (int i = 0; i < accs.Count; i++)           
                accs[i] = _tran(accs[i]);
           
            Song.ViewData.ListResult result = new ListResult(accs);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        #endregion


        #region 绑定信息
        /// <summary>
        /// 绑定手机号
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts PhoneBind(int acid, string phone)
        {
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(acid);
            if (acc == null)
            {
                throw new Exception("账号不存在");
            }
            acc.Ac_MobiTel1 = acc.Ac_MobiTel2 = phone;
            Song.Entities.Accounts accphone = Business.Do<IAccounts>().AccountsForMobi(phone, -1, null, null);
            if (accphone != null && accphone.Ac_ID != acc.Ac_ID)
                throw new Exception("手机号已经占用");
            Business.Do<IAccounts>().AccountsSave(acc);
            LoginAccount.Status.Fresh(acc);
            return _tran(acc);
        }
        /// <summary>
        /// 解除手机绑定
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts PhoneUnbind(int acid)
        {
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(acid);
            if (acc == null)
            {
                throw new Exception("账号不存在");
            }
            acc.Ac_MobiTel2 = "";
            Business.Do<IAccounts>().AccountsSave(acc);
            LoginAccount.Status.Fresh(acc);
            return _tran(acc);
        }
        #endregion

        #region 第三方登录的账号，通过openid获取已经存在账户
        /// <summary>
        /// 通过openid获取已经存在账户
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="field">accounts表中的相关字段</param>
        /// <returns></returns>
        public Song.Entities.Accounts User4Openid(string openid, string field)
        {
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountThirdparty(openid,field);
            return _tran(acc);
        }
        /// <summary>
        /// 采用第三方平台openid登录
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Song.Entities.Accounts UserLogin(string openid, string type)
        {
            Song.Entities.Accounts acc = this.User4Openid(openid, type);
            if (acc != null)
            {
                acc = Business.Do<IAccounts>().AccountsLogin(acc);               
                acc.Ac_Pw = LoginAccount.Status.Generate_Checkcode(acc);
            }
            LoginAccount.Status.Fresh(acc);
            return acc;
        }
        /// <summary>
        /// 绑定第三方账号，如果openid为空，则为取消绑定
        /// </summary>
        /// <param name="openid">第三方平台账号的唯一id</param>
        /// <param name="nickname"></param>
        /// <param name="headurl"></param>
        /// <param name="field">在本系统accounts表的字段,不带Ac_前缀</param>
        /// <returns></returns>
        [HttpPost, Student]
        public Song.Entities.Accounts UserBind(string openid, string nickname, string headurl, string field)
        {
            Song.Entities.Accounts acc = this.User;
            if (string.IsNullOrWhiteSpace(openid))
            {
                acc = Business.Do<IAccounts>().UnBindThirdparty(acc, field);
            }
            else
            {
                acc = Business.Do<IAccounts>().BindThirdparty(acc, openid, nickname, headurl, field);
            }          
            acc = Business.Do<IAccounts>().AccountsSingle(acc.Ac_ID);
            LoginAccount.Status.Fresh(acc);
            return _tran(acc);
        }
        /// <summary>
        /// 取消当前登录账号的第三方登录绑定
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [HttpPost, Student]
        public Song.Entities.Accounts UserUnbind(string field)
        {
            Song.Entities.Accounts acc = this.User;
            acc = Business.Do<IAccounts>().UnBindThirdparty(acc, field);
            acc = Business.Do<IAccounts>().AccountsSingle(acc.Ac_ID);
            LoginAccount.Status.Fresh(acc);
            return _tran(acc);
        }
        /// <summary>
        /// 用第三方平台信息创建新用户
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="openid"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.Accounts UserCreate(Song.Entities.Accounts acc, string openid,string field)
        {
            if (string.IsNullOrWhiteSpace(acc.Ac_AccName)) acc.Ac_AccName = WeiSha.Core.Request.SnowID().ToString();
            //判断账号是否已经存在
            Accounts accorig = Business.Do<IAccounts>().IsAccountsExist(acc.Ac_AccName);
            //如果不存在，则新增
            if (accorig == null)
            {
                //默认账号为审核通过和启用
                acc.Ac_IsPass = acc.Ac_IsUse = true;
                acc.Ac_Pw = WeiSha.Core.Request.UniqueID();
                //头像图片
                string headurl = acc.Ac_Photo;
                if (!string.IsNullOrWhiteSpace(headurl))
                {
                    string photoPath = _phyPath + openid + ".jpg";
                    WeiSha.Core.Request.LoadFile(headurl, photoPath);
                    acc.Ac_Photo = openid + ".jpg";
                }
                Business.Do<IAccounts>().AccountsAdd(acc);
                Business.Do<IAccounts>().BindThirdparty(acc, openid, acc.Ac_Name, headurl, field);
                accorig = Business.Do<IAccounts>().AccountsSingle(acc.Ac_ID);
            }
            else
            {
                //生成绑定记录
                Business.Do<IAccounts>().BindThirdparty(accorig, openid, acc.Ac_Name, acc.Ac_Photo, field);
            }

            _tran(accorig);
            accorig = Business.Do<IAccounts>().AccountsLogin(accorig);
            accorig.Ac_Pw = LoginAccount.Status.Generate_Checkcode(accorig);
            LoginAccount.Status.Fresh(accorig);
            return accorig;
        }
        /// <summary>
        /// 获取学员账号绑定的第三方平台的信息
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="tag">第三方平台的标识，来自config.js中的tag</param>
        /// <returns>包括第三方平台账号的名称与头像地址</returns>
        public Song.Entities.ThirdpartyAccounts UserThirdparty(int acid,string tag)
        {
            return Business.Do<IAccounts>().ThirdpartyAccount(acid,tag);
        }
        #endregion

        #region 统计信息
        /// <summary>
        /// 统计各个年龄段的学员
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="interval">年龄间隔，即某个年龄段</param>
        /// <returns></returns>
        public DataTable AgeGroup(int orgid, int interval)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            return Business.Do<IAccounts>().AgeGroup(orgid, interval);
        }
        /// <summary>
        /// 统计学员注册的数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="interval">间隔单位，y为年,m为月,d为日</param>
        /// <param name="start">统计区间的起始时间</param>
        /// <param name="end">统计区间的结束时间</param>
        /// <returns></returns>
        public DataTable RegTimeGroup(int orgid, string interval, DateTime? start, DateTime? end)
        {
            DateTime s = start == null ? DateTime.Now.AddYears(-1) : (DateTime)start;
            DateTime e = end == null ? DateTime.Now : (DateTime)end;
            return Business.Do<IAccounts>().RegTimeGroup(orgid, interval, s, e);
        }
        /// <summary>
        /// 统计学员登录情况
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="interval">间隔单位，y为年,m为月,d为日</param>
        /// <param name="start">统计区间的起始时间</param>
        /// <param name="end">统计区间的结束时间</param>
        /// <returns></returns>
        public DataTable LoginTimeGroup(int orgid, string interval, DateTime? start, DateTime? end)
        {
            DateTime s = start == null ? DateTime.Now.AddYears(-1) : (DateTime)start;
            DateTime e = end == null ? DateTime.Now : (DateTime)end;
            return Business.Do<IAccounts>().LoginTimeGroup(orgid, interval, s, e);
        }
        #endregion
    }
}
