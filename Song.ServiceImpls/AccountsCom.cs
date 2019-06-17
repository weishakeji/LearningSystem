using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using System.Xml;
using NPOI.SS.UserModel;
using System.IO;



namespace Song.ServiceImpls
{
    public class AccountsCom : IAccounts
    {
        #region 事件
        public event EventHandler Save;
        public event EventHandler Add;
        public event EventHandler Delete;
        public void OnSave(object sender, EventArgs e)
        {
            if (!(sender is Accounts)) return;
            Accounts acc = (Accounts)sender;
            if (acc == null) return;
            int currid = Extend.LoginState.Accounts.UserID;
            if (currid != acc.Ac_ID) return;
            Extend.LoginState.Accounts.Refresh(currid);

            if (Save != null) Save(sender, e);
        }
        public void OnAdd(object sender, EventArgs e)
        {
            if (Add != null) Add(sender, e);
        }
        public void OnDelete(object sender, EventArgs e)
        {
            if (Delete != null) Delete(sender, e);
        }
        #endregion
        /// <summary>
        /// 注册协议
        /// </summary>
        /// <returns></returns>
        public string RegAgreement()
        {
            Song.Entities.Organization org =  Business.Do<IOrganization>().OrganCurrent();
            //注册协议
            string agreement = Business.Do<ISystemPara>().GetValue("Agreement_accounts");
            if (string.IsNullOrWhiteSpace(agreement)) return agreement;
            agreement = Regex.Replace(agreement, "{platform}", org.Org_PlatformName, RegexOptions.IgnoreCase);
            agreement = Regex.Replace(agreement, "{org}", org.Org_AbbrName, RegexOptions.IgnoreCase);
            agreement = Regex.Replace(agreement, "{domain}", WeiSha.Common.Server.MainName, RegexOptions.IgnoreCase);
            return agreement;
        }
        #region 账户管理
        /// <summary>
        /// 添加账户
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>如果已经存在该账户，则返回-1</returns>
        public int AccountsAdd(Accounts entity)
        {
            if(!string.IsNullOrWhiteSpace(entity.Ac_IDCardNumber))
                entity.Ac_IDCardNumber = entity.Ac_IDCardNumber.Trim();
            //如果账号为空
            if (string.IsNullOrWhiteSpace(entity.Ac_AccName))
                throw new Exception("账号不得为空！");    
            else
                entity.Ac_AccName = entity.Ac_AccName.Trim();
            entity.Ac_RegTime = entity.Ac_LastTime = DateTime.Now;
            entity.Ac_IsUse = true;
            if (entity.Org_ID < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            //如果用户自己设置了年龄，则记录出生年份
            if (entity.Ac_Age > 0) entity.Ac_Age = DateTime.Now.Year - entity.Ac_Age;
            //计算年龄，如果设置了生日，则自动计算出生年月
            if (entity.Ac_Birthday > DateTime.Now.AddYears(-100))
                entity.Ac_Age = entity.Ac_Birthday.Year;
                  
            //如果密码为空
            if (string.IsNullOrWhiteSpace(entity.Ac_Pw))
                entity.Ac_Pw = WeiSha.Common.Login.Get["Accounts"].DefaultPw.MD5;
            Gateway.Default.Save<Accounts>(entity); 
            //初次注册时的积分
            int regPoint = Business.Do<ISystemPara>()["RegFirst"].Int32 ?? 0;                       
            PointAccount pa = new PointAccount();   //开始增加积分
            pa.Pa_Value = regPoint;
            pa.Ac_ID = entity.Ac_ID;
            pa.Pa_From = 0;     //分享注册
            pa.Pa_Info = "初次注册";
            this.PointAdd(pa);            
            return entity.Ac_ID;
        }
        /// <summary>
        /// 修改账户
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void AccountsSave(Accounts entity)
        {
            //如果密码不为空
            //if (!string.IsNullOrWhiteSpace(entity.Ac_Pw))
            //    entity.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(entity.Ac_Pw).MD5; 
           
            //获取原有数据
            //Accounts old = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == entity.Ac_ID).ToFirst<Accounts>();
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {                   
                    //if (old != null && old.Sts_ID != entity.Sts_ID)
                    //{
                        //同步考试成绩中的学员组
                    tran.Update<ExamResults>(new Field[] { ExamResults._.Sts_ID, ExamResults._.Ac_Sex, ExamResults._.Ac_Name, ExamResults._.Ac_IDCardNumber },
                        new object[] { entity.Sts_ID, entity.Ac_Sex, entity.Ac_Name, entity.Ac_IDCardNumber }, ExamResults._.Ac_ID == entity.Ac_ID);
                        //同步教师信息
                        tran.Update<Teacher>(new Field[] { Teacher._.Th_Sex, Teacher._.Th_Birthday, Teacher._.Th_IDCardNumber, Teacher._.Th_Nation, Teacher._.Th_Native },
                        new object[] { entity.Ac_Sex, entity.Ac_Birthday, entity.Ac_IDCardNumber, entity.Ac_Nation, entity.Ac_Native }, Teacher._.Ac_ID == entity.Ac_ID);
                    //}
                    tran.Save<Accounts>(entity);
                    tran.Commit();
                    this.OnSave(entity, EventArgs.Empty);
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }           
        }
        #region 私有方法
        /// <summary>
        /// 账号初始化
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        private Accounts _acc_init(Accounts ac)
        {
            if (ac != null)
            {
                if (ac.Ac_Birthday > DateTime.Now.AddYears(-100))
                {
                    ac.Ac_Age = (int)((DateTime.Now - ac.Ac_Birthday).TotalDays / (365 * 3 + 366) * 4);
                    ac.Ac_Age = ac.Ac_Age > 150 ? 0 : ac.Ac_Age;
                }
                else
                {
                    if (ac.Ac_Age > 0) ac.Ac_Age = DateTime.Now.Year - ac.Ac_Age;
                }
            }

            return ac;
        }
        #endregion
        /// <summary>
        /// 修改账户，按条件修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        public void AccountsUpdate(Accounts entity, Field[] fiels, object[] objs)
        {
            Gateway.Default.Update<Accounts>(fiels, objs, Accounts._.Ac_ID == entity.Ac_ID);
            this.OnSave(entity, EventArgs.Empty);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void AccountsDelete(int identify)
        {
            Song.Entities.Accounts ac = this.AccountsSingle(identify);
            if (ac == null) return;
            this.AccountsDelete(ac);
        }
        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="entity"></param>
        public void AccountsDelete(Song.Entities.Accounts entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Accounts>(Accounts._.Ac_ID == entity.Ac_ID);
                    if (!string.IsNullOrWhiteSpace(entity.Ac_Photo))
                        WeiSha.WebControl.FileUpload.Delete("Accounts", entity.Ac_Photo);                    
                    //删除相关数据
                    tran.Delete<Student_Ques>(Student_Ques._.Ac_ID == entity.Ac_ID);
                    tran.Delete<Student_Notes>(Student_Notes._.Ac_ID == entity.Ac_ID);
                    tran.Delete<Student_Course>(Student_Course._.Ac_ID == entity.Ac_ID);
                    tran.Delete<TestResults>(TestResults._.Ac_ID == entity.Ac_ID);
                    tran.Delete<MoneyAccount>(MoneyAccount._.Ac_ID == entity.Ac_ID);
                    tran.Delete<PointAccount>(PointAccount._.Ac_ID == entity.Ac_ID);
                    //下级学员全部提升一级                    
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_PID }, new object[] { entity.Ac_PID }, Accounts._.Ac_PID == entity.Ac_ID);
                    tran.Commit();
                    //删除教师
                    Song.Entities.Teacher th = this.GetTeacher(entity.Ac_ID, null);
                    if (th != null) Business.Do<ITeacher>().TeacherDelete(th, tran);
                    //从缓存中清除
                    Extend.LoginState.Accounts.CleanOut(entity);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Accounts AccountsSingle(int identify)
        {
            Song.Entities.Accounts ac = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == identify).ToFirst<Accounts>();
            return _acc_init(ac);
        }
        /// <summary>
        /// 通过账号名获取
        /// </summary>
        /// <param name="accname"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public Accounts AccountsSingle(string accname, int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            wc.And(Accounts._.Ac_AccName == accname);
            Song.Entities.Accounts ac = Gateway.Default.From<Accounts>().Where(wc).ToFirst<Accounts>();
            return _acc_init(ac);
        }
        /// <summary>
        /// 通过手机号获取账户
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="isPass">是否通过审核</param>
        /// <param name="isUse">是否启用</param>
        /// <returns></returns>
        public Accounts AccountsSingle(string phone, bool? isPass, bool? isUse)
        {
            if (string.IsNullOrWhiteSpace(phone)) return null;
            WhereClip wc = new WhereClip();
            if (isPass != null) wc.And(Accounts._.Ac_IsPass == (bool)isPass);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == (bool)isUse);
            WhereClip w2 = new WhereClip();
            w2 |= Accounts._.Ac_MobiTel1 == phone;
            w2 |= Accounts._.Ac_MobiTel2 == phone;
            Song.Entities.Accounts ac = Gateway.Default.From<Accounts>().Where(wc && w2).ToFirst<Accounts>();
            return _acc_init(ac);
        }
        /// <summary>
        /// 获取单一实体对象，按网站账户名称
        /// </summary>
        /// <param name="accname">帐号名称</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        public Accounts AccountsSingle(string accname, string pw, int orgid)
        {
            pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(pw).MD5;
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            wc.And(Accounts._.Ac_AccName == accname);
            wc.And(Accounts._.Ac_Pw == pw);
            Song.Entities.Accounts ac = Gateway.Default.From<Accounts>().Where(wc).ToFirst<Accounts>();
            return _acc_init(ac);
        }
        /// <summary>
        /// 获取单一实体，通过id与验证码
        /// </summary>
        /// <param name="id">账户Id</param>
        /// <param name="uid">账户登录时产生随机字符，用于判断同一账号不同人登录的问题</param>
        /// <returns></returns>
        public Accounts AccountsSingle(int id, string uid)
        {
            Song.Entities.Accounts ac = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == id && Accounts._.Ac_CheckUID == uid).ToFirst<Accounts>();
            return _acc_init(ac);
        }
        /// <summary>
        /// 通过QQ的openid获取账户
        /// </summary>
        /// <param name="qqopenid"></param>
        /// <returns></returns>
        public Accounts Account4QQ(string qqopenid)
        {
            Song.Entities.Accounts ac = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_QqOpenID == qqopenid || Accounts._.Ac_AccName == qqopenid).ToFirst<Accounts>();
            return _acc_init(ac);
        }
        /// <summary>
        /// 通过微信的openid获取账户
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public Accounts Account4Weixin(string openid)
        {
            Song.Entities.Accounts ac = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_WeixinOpenID == openid || Accounts._.Ac_AccName == openid).ToFirst<Accounts>();
            return _acc_init(ac);
        }
        /// <summary>
        /// 通过基础账号的id，获取教师账户
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        public Teacher GetTeacher(int acid, bool? isPass)
        {
            WhereClip wc = Teacher._.Ac_ID == acid;
            if (isPass != null) wc.And(Teacher._.Th_IsPass == (bool)isPass);
            return Gateway.Default.From<Teacher>().Where(Teacher._.Ac_ID == acid).ToFirst<Teacher>();
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="acc">账号，或身份证，或手机</param>
        /// <param name="pw">密码（明文，未经md5加密）</param>
        /// <param name="isPass">是否审核通过</param>
        /// <returns></returns>
        public Accounts AccountsLogin(string acc, string pw, bool? isPass)
        {
            WhereClip wc = Accounts._.Ac_IsUse == true;
            if (isPass != null) wc.And(Accounts._.Ac_IsPass == (bool)isPass);
            //密码
            wc.And(Accounts._.Ac_Pw == new WeiSha.Common.Param.Method.ConvertToAnyValue(pw).MD5);
            //账号、手机号、身份证，均可验证
            WhereClip w2 = new WhereClip();
            w2 |= Accounts._.Ac_AccName == acc;
            w2 |= Accounts._.Ac_MobiTel1 == acc;
            w2 |= Accounts._.Ac_MobiTel2 == acc;
            w2 |= Accounts._.Ac_IDCardNumber == acc;
            Song.Entities.Accounts entity = Gateway.Default.From<Accounts>().Where(wc && w2).ToFirst<Accounts>();           
            return _acc_init(entity);
        }
        /// <summary>
        /// 登录判断
        /// </summary>
        /// <param name="accid">账户id</param>
        /// <param name="pw">密码，md5加密后的</param>
        /// <param name="isPass">是否审核通过</param>
        /// <returns></returns>
        public Accounts AccountsLogin(int accid, string pw, bool? isPass)
        {
            WhereClip wc = Accounts._.Ac_IsUse == true;
            if (isPass != null) wc.And(Accounts._.Ac_IsPass == (bool)isPass);
            wc.And(Accounts._.Ac_ID == accid && Accounts._.Ac_Pw == pw);
            Song.Entities.Accounts entity = Gateway.Default.From<Accounts>().Where(wc).ToFirst<Accounts>();
            return _acc_init(entity);
        }
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="accname"></param>
        /// <returns></returns>
        public Accounts IsAccountsExist(string accname)
        {
            return IsAccountsExist(0, accname);
        }
        /// <summary>
        /// 判断账号是否存在
        /// </summary>
        /// <param name="accname"></param>
        /// <returns></returns>
        public Accounts IsAccountsExist(int orgid, string accname)
        {
            return IsAccountsExist(orgid, accname, 0);
        }
        /// <summary>
        /// 当前用帐号是否重名
        /// </summary>
        /// <param name="accname">账户帐号</param>
        /// <param name="type">判断类型，默认为账号，1为手机号,2为邮箱,-1为所有</param>
        /// <returns></returns>
        public Accounts IsAccountsExist(int orgid, string accname, int type)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Accounts._.Org_ID == orgid;
            switch (type)
            {
                case 1:
                    wc |= Accounts._.Ac_MobiTel1 == accname;
                    wc |= Accounts._.Ac_MobiTel2 == accname;
                    break;
                case 2:
                    wc |= Accounts._.Ac_Email == accname;
                    break;
                case -1:
                    wc |= Accounts._.Ac_MobiTel1 == accname;
                    wc |= Accounts._.Ac_MobiTel2 == accname;
                    wc |= Accounts._.Ac_Email == accname;
                    wc &= Accounts._.Ac_AccName == accname;
                    break;
                default:
                    wc &= Accounts._.Ac_AccName == accname;
                    break;
            }
            Accounts mm = Gateway.Default.From<Accounts>()
                .Where(wc).ToFirst<Accounts>();
            return _acc_init(mm);
        }
        /// <summary>
        /// 判断账户是否已经在存，将判断账号与手机号
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="enity"></param>
        /// <returns></returns>
        public Accounts IsAccountsExist(int orgid, Accounts enity)
        {
            WhereClip wc = Accounts._.Ac_ID != enity.Ac_ID;
            if (orgid > 0) wc &= Accounts._.Org_ID == orgid;
            WhereClip orWc = new WhereClip();
            orWc |= Accounts._.Ac_AccName == enity.Ac_AccName;
            orWc |= Accounts._.Ac_IDCardNumber == enity.Ac_IDCardNumber;
            orWc |= Accounts._.Ac_MobiTel1 == enity.Ac_MobiTel1;
            Accounts mm = Gateway.Default.From<Accounts>()
                .Where(wc && orWc).ToFirst<Accounts>();
            return _acc_init(mm);
        }
        /// <summary>
        /// 当前用帐号是否重名
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="accname"></param>
        /// <param name="answer">安全问题答案</param>
        /// <returns></returns>
        public Accounts IsAccountsExist(int orgid, string accname, string answer)
        {
            if (string.IsNullOrWhiteSpace(accname)) return null;
            if (string.IsNullOrWhiteSpace(answer)) return null;
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            Accounts mm = Gateway.Default.From<Accounts>()
               .Where(wc && Accounts._.Ac_AccName == accname && Accounts._.Ac_Ans == answer).ToFirst<Accounts>();
            return _acc_init(mm);
        }
        /// <summary>
        /// 获取账户
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Accounts[] AccountsCount(int orgid, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            count = count > 0 ? count : int.MaxValue;
            Accounts[] accs = Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>(count);
            foreach (Song.Entities.Accounts ac in accs)
                 _acc_init(ac);
            return accs;
        }
        /// <summary>
        /// 获取账户信息
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="sts">班组的id，多个id用逗号分隔</param>
        /// <param name="count">取多少条，小于1为所有</param>
        /// <returns></returns>
        public List<Accounts> AccountsCount(int orgid, bool? isUse, string sts, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            if (!string.IsNullOrWhiteSpace(sts))
            {
                string[] ids = sts.Split(',');
                WhereClip wcsts = new WhereClip();
                foreach (string s in ids)
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    int stsid = 0;
                    int.TryParse(s, out stsid);
                    if (stsid == 0) continue;
                    wcsts.Or(Accounts._.Sts_ID == stsid);
                }
                wc.And(wcsts);
            }
            count = count > 0 ? count : int.MaxValue;
            List<Accounts> accs = Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToList<Accounts>(count);
            foreach (Song.Entities.Accounts ac in accs)
                _acc_init(ac);
            return accs;
        }
        /// <summary>
        /// 计算有多少账户
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public int AccountsOfCount(int orgid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            return Gateway.Default.Count<Accounts>(wc);
        }
        /// <summary>
        /// 分页获取所有的网站账户帐号；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public Accounts[] AccountsPager(int orgid, int size, int index, out int countSum)
        {
            WhereClip wc = Accounts._.Org_ID == orgid;
            countSum = Gateway.Default.Count<Accounts>(wc);
            Accounts[] accs = Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>(size, (index - 1) * size);
            foreach (Song.Entities.Accounts ac in accs)
                _acc_init(ac);
            return accs;
        }
        /// <summary>
        /// 分页获取某账户组，所有的网站账户帐号；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">账户分类id</param>
        /// <param name="isUse"></param>
        /// <param name="name">账户名称</param>
        /// <param name="phone">账户账号</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Accounts[] AccountsPager(int orgid, int sortid, bool? isUse, string acc, string name, string phone, int size, int index, out int countSum)
        {
            return AccountsPager(orgid, sortid, -1, isUse, acc, name, phone, size, index, out countSum);
        }
        /// <summary>
        /// 分页获取某账户组，所有的网站账户帐号；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="pid">推荐人id</param>
        /// <param name="isUse"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Accounts[] AccountsPager(int orgid, int sortid, int pid, bool? isUse, string acc, string name, string phone, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accounts._.Org_ID == orgid);
            if (sortid > 0) wc.And(Accounts._.Sts_ID == sortid);
            if (pid > 0) wc.And(Accounts._.Ac_PID == pid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            if (!string.IsNullOrWhiteSpace(acc) && acc.Trim() != "") wc.And(Accounts._.Ac_AccName.Like("%" + acc.Trim() + "%"));
            if (!string.IsNullOrWhiteSpace(name) && name.Trim() != "") wc.And(Accounts._.Ac_Name.Like("%" + name.Trim() + "%"));
            if (!string.IsNullOrWhiteSpace(phone) && phone.Trim() != "")
            {
                WhereClip wc2 = new WhereClip();
                wc2.Or(Accounts._.Ac_MobiTel1.Like("%" + phone.Trim() + "%"));
                wc2.Or(Accounts._.Ac_MobiTel2.Like("%" + phone.Trim() + "%"));
                wc2.Or(Accounts._.Ac_AccName.Like("%" + phone.Trim() + "%"));
                wc.And(wc2);
            }
            countSum = Gateway.Default.Count<Accounts>(wc);
            Accounts[] accs = Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>(size, (index - 1) * size);
            foreach (Song.Entities.Accounts ac in accs)
                _acc_init(ac);
            return accs;
        }
        /// <summary>
        /// 学员账号信息导出
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sorts">学员分组id，用逗号分隔</param>
        /// <returns></returns>
        public string AccountsExport4Excel(string path, int orgid, string sorts)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();            
            string confing = WeiSha.Common.App.Get["ExcelInputConfig"].VirtualPath + "学生信息.xml";
            xmldoc.Load(WeiSha.Common.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Accounts._.Org_ID == orgid;
            Accounts[] accounts = null;
            //如果没有分组信息，则取当前机构的所有
            if (string.IsNullOrWhiteSpace(sorts))
            {
                accounts = Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>();
                _export4Excel_to_sheet(hssfworkbook, "学员信息", accounts, nodes);
            }
            else
            {
                //按学员组生成工作簿
                foreach (string s in sorts.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    int sortid=0;
                    int.TryParse(s,out sortid);
                    if(sortid==0)continue;
                    //
                    Song.Entities.StudentSort sts=Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == sortid).ToFirst<StudentSort>();
                    if(sts==null)continue;
                    accounts = Gateway.Default.From<Accounts>().Where(Accounts._.Org_ID == orgid && Accounts._.Sts_ID == sts.Sts_ID).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>();
                    _export4Excel_to_sheet(hssfworkbook, sts.Sts_Name, accounts, nodes);
                }
                //其它学员（没有分组的学员）
                WhereClip wcNosort = new WhereClip();
                Song.Entities.StudentSort[] stss = Gateway.Default.From<StudentSort>().Where(StudentSort._.Org_ID == orgid).ToArray<StudentSort>();
                foreach (StudentSort ss in stss) wcNosort.And(Accounts._.Sts_ID != ss.Sts_ID);
                accounts = Gateway.Default.From<Accounts>().Where(Accounts._.Org_ID == orgid && wcNosort).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>();
                _export4Excel_to_sheet(hssfworkbook, "未分组学员", accounts, nodes);
            }
            
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }
        /// <summary>
        /// 学员账户号导出
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgs">机构id,用逗号分隔</param>
        /// <returns></returns>
        public string AccountsExport4Excel(string path, string orgs)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Common.App.Get["ExcelInputConfig"].VirtualPath + "学生信息.xml";
            xmldoc.Load(WeiSha.Common.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //
            Accounts[] accounts = null;
            //如果没有分组信息，则取当前机构的所有
            if (string.IsNullOrWhiteSpace(orgs))
            {
                accounts = Gateway.Default.From<Accounts>().OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>();
                _export4Excel_to_sheet(hssfworkbook, "全部学员信息", accounts, nodes);
            }
            else
            {
                //按学员组生成工作簿
                foreach (string s in orgs.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    int orgid = 0;
                    int.TryParse(s, out orgid);
                    if (orgid == 0) continue;
                    //
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
                    if (org == null) continue;
                    accounts = Gateway.Default.From<Accounts>().Where(Accounts._.Org_ID == orgid).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>();
                    _export4Excel_to_sheet(hssfworkbook, org.Org_Name, accounts, nodes);
                }              
            }

            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }
        private void _export4Excel_to_sheet(HSSFWorkbook hssfworkbook, string sheetName, Accounts[] accounts, XmlNodeList nodes)
        {
            ISheet sheet = hssfworkbook.CreateSheet(sheetName);   //创建工作簿对象
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
            {
                string exExport = nodes[i].Attributes["export"] != null ? nodes[i].Attributes["export"].Value : ""; //是否导出
                if (exExport.ToLower() == "false") continue;
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            }
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;

            for (int i = 0; i < accounts.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < nodes.Count; j++)
                {
                    Type type = accounts[i].GetType();
                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(nodes[j].Attributes["Field"].Value); //获取指定名称的属性
                    object obj = propertyInfo.GetValue(accounts[i], null);
                    if (obj != null)
                    {
                        string exExport = nodes[j].Attributes["export"] != null ? nodes[j].Attributes["export"].Value : ""; //是否导出
                        if (exExport.ToLower() == "false") continue;
                        string format = nodes[j].Attributes["Format"] != null ? nodes[j].Attributes["Format"].Value : "";
                        string datatype = nodes[j].Attributes["DataType"] != null ? nodes[j].Attributes["DataType"].Value : "";
                        string defvalue = nodes[j].Attributes["DefautValue"] != null ? nodes[j].Attributes["DefautValue"].Value : "";
                        
                        string value = "";
                        switch (datatype)
                        {
                            case "date":
                                DateTime tm = Convert.ToDateTime(obj);
                                value = tm > DateTime.Now.AddYears(-100) ? tm.ToString(format) : "";
                                break;
                            default:
                                value = obj.ToString();
                                break;
                        }
                        if (defvalue.Trim() != "")
                        {
                            foreach (string s in defvalue.Split('|'))
                            {
                                string h = s.Substring(0, s.IndexOf("="));
                                string f = s.Substring(s.LastIndexOf("=") + 1);
                                if (value == h) value = f;
                            }
                        }
                        row.CreateCell(j).SetCellValue(value);
                    }
                }
            }
            return;
        }
        #endregion        

        #region 下级账户
        /// <summary>
        /// 下级会员数据
        /// </summary>
        /// <param name="acid">当前账号ID</param>
        /// <param name="isAll">是否包括所有下级，true是所有，false只取直接下级</param>
        /// <returns></returns>
        public int SubordinatesCount(int acid, bool isAll)
        {
            int count = Gateway.Default.Count<Accounts>(Accounts._.Ac_PID == acid);
            if (isAll && count > 0)
            {
                Accounts[] accs = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_PID == acid)
                    .Select(new Field[] { Accounts._.Ac_ID })
                    .ToArray<Accounts>();
                foreach(Accounts ac in accs)
                    count += SubordinatesCount(ac.Ac_ID, isAll);
            }
            return count;
        }
        /// <summary>
        /// 下级会员分页获取
        /// </summary>
        /// <param name="acid">当前账号id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="acc"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Accounts[] SubordinatesPager(int acid, bool? isUse, string acc, string name, string phone, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Accounts._.Ac_PID == acid);
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            if (!string.IsNullOrWhiteSpace(acc) && acc.Trim() != "") wc.And(Accounts._.Ac_AccName.Like("%" + acc.Trim() + "%"));
            if (!string.IsNullOrWhiteSpace(name) && name.Trim() != "") wc.And(Accounts._.Ac_Name.Like("%" + name.Trim() + "%"));
            if (!string.IsNullOrWhiteSpace(phone) && phone.Trim() != "")
            {
                WhereClip wc2 = new WhereClip();
                wc2.Or(Accounts._.Ac_MobiTel1.Like("%" + phone.Trim() + "%"));
                wc2.Or(Accounts._.Ac_MobiTel2.Like("%" + phone.Trim() + "%"));
                wc2.Or(Accounts._.Ac_AccName.Like("%" + phone.Trim() + "%"));
                wc.And(wc2);
            }
            countSum = Gateway.Default.Count<Accounts>(wc);
            Accounts[] accs = Gateway.Default.From<Accounts>().Where(wc).OrderBy(Accounts._.Ac_RegTime.Desc).ToArray<Accounts>(size, (index - 1) * size);
            foreach (Song.Entities.Accounts ac in accs)
                _acc_init(ac);
            return accs;
        }
        #endregion

        #region 积分管理
        /// <summary>
        /// 收入
        /// </summary>
        /// <param name="entity">业务实体</param>
        public PointAccount PointAdd(PointAccount entity)
        {
            if (entity.Ac_ID < 1) return entity;    //如果没有指定学员id，则返回
            if (entity.Pa_Value <= 0) return entity;    //如果要增加的值小于等于零，则返回
            entity.Pa_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            entity.Pa_Type = 2;           
            //流水号
            entity.Pa_Serial = Business.Do<ISystemPara>().Serial();

            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Song.Entities.Accounts student = tran.From<Accounts>().Where(Accounts._.Ac_ID == entity.Ac_ID).ToFirst<Accounts>();
                    entity.Pa_Total = student.Ac_Point + entity.Pa_Value;
                    entity.Pa_TotalAmount = student.Ac_PointAmount + entity.Pa_Value;
                    tran.Save<PointAccount>(entity);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_Point }, new object[] { entity.Pa_Total }, Accounts._.Ac_ID == entity.Ac_ID);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_PointAmount }, new object[] { entity.Pa_TotalAmount }, Accounts._.Ac_ID == entity.Ac_ID);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
                    return entity;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 增加登录积分
        /// </summary>
        /// <param name="acc">学员账户</param>
        /// <returns></returns>
        public int PointAdd4Login(Accounts acc)
        {
            return PointAdd4Login(acc, null, null, null);
        }
        public int PointAdd4Login(Accounts acc, string source, string info, string remark)
        {
            //每次登录增加的积分；
            int loginPoint = Business.Do<ISystemPara>()["LoginPoint"].Int32 ?? 0;
            if (loginPoint <= 0) return 0;
            //每天最多的登录积分；
            int maxPoint = Business.Do<ISystemPara>()["LoginPointMax"].Int32 ?? 0;
            if (loginPoint > maxPoint) return 0;
            //当前学员今天的登录积分；            
            int todaySum = PointClac(acc.Ac_ID, 1, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
            int surplus = maxPoint - todaySum;  //每天最多积分，减去已经得到的积分，获取剩余的加分空间
            if (surplus <= 0) return 0;

            //开始增加积分
            PointAccount pa = new PointAccount();
            pa.Pa_Value = surplus > loginPoint ? loginPoint : surplus;   //当前登录要增加的积分数
            pa.Ac_ID = acc.Ac_ID;
            pa.Pa_From = 1;     //登录积分
            pa.Pa_Source = source;
            pa.Pa_Info = info;
            pa.Pa_Remark = remark;
            this.PointAdd(pa);
            return pa.Pa_Value;
        }
        /// <summary>
        /// 增加分享链接的访问积分
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public int PointAdd4Share(Accounts acc)
        {
            //每次访问增加的积分；
            int loginPoint = Business.Do<ISystemPara>()["SharePoint"].Int32 ?? 0;
            if (loginPoint <= 0) return 0;
            //每天最多的登录积分；
            int maxPoint = Business.Do<ISystemPara>()["SharePointMax"].Int32 ?? 0;
            if (loginPoint > maxPoint) return 0;
            //当前学员今天的访问积分；            
            int todaySum = PointClac(acc.Ac_ID, 2, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
            int surplus = maxPoint - todaySum;  //每天最多积分，减去已经得到的积分，获取剩余的加分空间
            if (surplus <= 0) return 0;

            //开始增加积分
            PointAccount pa = new PointAccount();
            pa.Pa_Value = surplus > loginPoint ? loginPoint : surplus;   //当前登录要增加的积分数
            pa.Ac_ID = acc.Ac_ID;
            pa.Pa_From = 2;     //分享积分
            pa.Pa_Info = "分享链接";
            this.PointAdd(pa);
            return pa.Pa_Value;
        }
        /// <summary>
        /// 增加分享链接的注册积分
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public int PointAdd4Register(Accounts acc)
        {
            //每次分享注册增加的积分；
            int loginPoint = Business.Do<ISystemPara>()["RegPoint"].Int32 ?? 0;
            if (loginPoint <= 0) return 0;
            //每天最多的登录积分；
            int maxPoint = Business.Do<ISystemPara>()["RegPointMax"].Int32 ?? 0;
            if (loginPoint > maxPoint) return 0;
            //当前学员今天的分享注册积分；            
            int todaySum = PointClac(acc.Ac_ID, 3, DateTime.Now.Date, DateTime.Now.AddDays(1).Date);
            int surplus = maxPoint - todaySum;  //每天最多积分，减去已经得到的积分，获取剩余的加分空间
            if (surplus <= 0) return 0;

            //开始增加积分
            PointAccount pa = new PointAccount();
            pa.Pa_Value = surplus > loginPoint ? loginPoint : surplus;   //当前登录要增加的积分数
            pa.Ac_ID = acc.Ac_ID;
            pa.Pa_From = 3;     //分享注册
            pa.Pa_Info = "新增下级会员";
            this.PointAdd(pa);
            return pa.Pa_Value;
        }
        /// <summary>
        /// 支出
        /// </summary>
        /// <param name="entity">业务实体</param>
        public PointAccount PointPay(PointAccount entity)
        {
            entity.Pa_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            entity.Pa_Type = 1;
            entity.Ac_ID = entity.Ac_ID;
            //流水号
            entity.Pa_Serial = Business.Do<ISystemPara>().Serial();
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Song.Entities.Accounts student = tran.From<Accounts>().Where(Accounts._.Ac_ID == entity.Ac_ID).ToFirst<Accounts>();
                    entity.Pa_Total = student.Ac_Point - entity.Pa_Value;
                    tran.Save<PointAccount>(entity);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_Point }, new object[] { entity.Pa_Total }, Accounts._.Ac_ID == entity.Ac_ID);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
                    return entity;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 删除流水
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PointDelete(PointAccount entity)
        {
            Gateway.Default.Delete<PointAccount>(PointAccount._.Pa_ID == entity.Pa_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void PointDelete(int identify)
        {
            Gateway.Default.Delete<PointAccount>(PointAccount._.Pa_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public PointAccount PointSingle(int identify)
        {
            return Gateway.Default.From<PointAccount>().Where(PointAccount._.Pa_ID == identify).ToFirst<PointAccount>();
        }
        /// <summary>
        /// 获取单一实体对象，按流水号
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public PointAccount PointSingle(string serial)
        {
            if (string.IsNullOrWhiteSpace(serial)) return null;
            return Gateway.Default.From<PointAccount>().Where(PointAccount._.Pa_Serial == serial.Trim()).ToFirst<PointAccount>();
        }
        /// <summary>
        /// 修改流水信息
        /// </summary>
        /// <param name="entity"></param>
        public void PointSave(PointAccount entity)
        {
            Gateway.Default.Save<PointAccount>(entity);
        }
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public PointAccount[] PointCount(int orgid, int stid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= PointAccount._.Org_ID == orgid;
            if (stid > 0) wc &= PointAccount._.Ac_ID == stid;
            if (type > 0) wc &= PointAccount._.Pa_Type == type;
            return Gateway.Default.From<PointAccount>().Where(wc).OrderBy(PointAccount._.Pa_CrtTime.Desc).ToArray<PointAccount>(count);
        }
        /// <summary>
        /// 计算某一个时间区间的积分
        /// </summary>
        /// <param name="acid">学员账户</param>
        /// <param name="formType">来源分类，1登录，2分享访问；3分享注册；4兑换</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int PointClac(int acid, int formType, DateTime? start, DateTime? end)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc &= PointAccount._.Ac_ID == acid;
            if (formType > 0) wc &= PointAccount._.Pa_From == formType;
            if (start != null) wc &= PointAccount._.Pa_CrtTime >= (DateTime)start;
            if (end != null) wc &= PointAccount._.Pa_CrtTime < (DateTime)end;
            object obj= Gateway.Default.Sum<PointAccount>(PointAccount._.Pa_Value, wc);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public PointAccount[] PointPager(int orgid, int stid, int type, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= PointAccount._.Org_ID == orgid;
            if (stid > 0) wc &= PointAccount._.Ac_ID == stid;
            if (type > 0) wc &= PointAccount._.Pa_Type == type;
            countSum = Gateway.Default.Count<PointAccount>(wc);
            return Gateway.Default.From<PointAccount>()
                .Where(wc).OrderBy(PointAccount._.Pa_CrtTime.Desc).ToArray<PointAccount>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public PointAccount[] PointPager(int orgid, int stid, int type, string searTxt, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= PointAccount._.Org_ID == orgid;
            if (stid > 0) wc &= PointAccount._.Ac_ID == stid;
            if (type > 0) wc &= PointAccount._.Pa_Type == type;
            if (start != null && ((DateTime)start) > DateTime.Now.AddYears(-100)) wc &= PointAccount._.Pa_CrtTime >= ((DateTime)start).Date;
            if (end != null && ((DateTime)end) > DateTime.Now.AddYears(-100)) wc &= PointAccount._.Pa_CrtTime < ((DateTime)end).AddDays(1).Date;
            if (!string.IsNullOrWhiteSpace(searTxt)) wc &= PointAccount._.Pa_Info.Like("%" + searTxt + "%");
            countSum = Gateway.Default.Count<PointAccount>(wc);
            return Gateway.Default.From<PointAccount>()
                .Where(wc).OrderBy(PointAccount._.Pa_CrtTime.Desc).ToArray<PointAccount>(size, (index - 1) * size);
        }
        #endregion

        #region 卡券管理
        /// <summary>
        /// 收入
        /// </summary>
        /// <param name="entity">业务实体</param>
        public CouponAccount CouponAdd(CouponAccount entity)
        {
            entity.Ca_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            entity.Ca_Type = 2;
            entity.Ac_ID = entity.Ac_ID;
            //流水号
            entity.Ca_Serial = Business.Do<ISystemPara>().Serial();
            //计算总数
            Song.Entities.Accounts acc=Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID==entity.Ac_ID).ToFirst<Accounts>();
            if (acc == null) throw new Exception("当前账户不存在");
            entity.Ca_Total = acc.Ac_Coupon + entity.Ca_Value;
            //最大券值
            object amount = Gateway.Default.Max<CouponAccount>(CouponAccount._.Ca_TotalAmount, CouponAccount._.Ac_ID == entity.Ac_ID);
            if (amount == null) amount = 0;
            entity.Ca_TotalAmount = (int)amount + entity.Ca_Value;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<CouponAccount>(entity);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_Coupon }, new object[] { entity.Ca_Total }, Accounts._.Ac_ID == entity.Ac_ID);                    
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
                    return entity;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 支出
        /// </summary>
        /// <param name="entity">业务实体</param>
        public CouponAccount CouponPay(CouponAccount entity)
        {
            if (entity.Ca_Value == 0) return entity;
            entity.Ca_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            entity.Ca_Type = 1;
            entity.Ac_ID = entity.Ac_ID;
            //流水号
            entity.Ca_Serial = Business.Do<ISystemPara>().Serial();
            //计算总数
            Song.Entities.Accounts acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == entity.Ac_ID).ToFirst<Accounts>();
            if (acc == null) throw new Exception("当前账户不存在");
            if (acc.Ac_Coupon < entity.Ca_Value) throw new Exception("当前账户卡券不足");
            entity.Ca_Total = acc.Ac_Coupon - entity.Ca_Value;
            //最大券值
            object amount = Gateway.Default.Max<CouponAccount>(CouponAccount._.Ca_TotalAmount, CouponAccount._.Ac_ID == entity.Ac_ID);
            if (amount == null) amount = 0;
            entity.Ca_TotalAmount = (int)amount;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<CouponAccount>(entity);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_Coupon }, new object[] { entity.Ca_Total }, Accounts._.Ac_ID == entity.Ac_ID);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
                    return entity;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 积分兑换卡券
        /// </summary>
        /// <param name="accid">学员id</param>
        /// <param name="coupon">要兑换的卡券数量</param>
        /// <returns></returns>
        public void CouponExchange(int accid, int coupon)
        {
            Song.Entities.Accounts acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == accid).ToFirst<Accounts>();
            CouponExchange(acc, coupon);
        }
        /// <summary>
        /// 积分兑换卡券
        /// </summary>
        /// <param name="acc">学员</param>
        /// <param name="coupon">要兑换的卡券数量</param>
        public void CouponExchange(Accounts acc, int coupon)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int accid = acc.Ac_ID;
            //计算是否满足兑换
            int point = acc.Ac_Point;
            int ratio = Business.Do<ISystemPara>()["PointConvert"].Int32 ?? 0;
            if (ratio <= 0) throw new Exception("积分兑换比率不得小于等于零");
            int result = point / ratio;
            if (result < coupon) throw new Exception("可兑换数量不足");
            //积分的扣除计算
            PointAccount pa = new PointAccount();
            pa.Pa_CrtTime = DateTime.Now;
            pa.Ac_ID = accid;
            if (org != null) pa.Org_ID = org.Org_ID;
            pa.Pa_Type = 1;
            pa.Pa_Serial = Business.Do<ISystemPara>().Serial(); //流水号
            pa.Pa_Value = coupon * ratio;   //要扣除的积分；
            pa.Pa_Total = acc.Ac_Point = acc.Ac_Point - pa.Pa_Value;
            pa.Pa_TotalAmount = acc.Ac_PointAmount;
            pa.Pa_Info = "积分兑换";
            pa.Pa_Remark = string.Format("用{0}积分兑换{1}卡券", pa.Pa_Value, coupon);
            pa.Pa_Source = WeiSha.Common.Browser.IsMobile ? "手机端" : "电脑网页";
            //卡券的增加计算
            CouponAccount ca = new CouponAccount();
            ca.Ca_CrtTime = DateTime.Now;
            ca.Ac_ID = accid;
            if (org != null) ca.Org_ID = org.Org_ID;
            ca.Ca_Serial = Business.Do<ISystemPara>().Serial();
            ca.Ca_Value = coupon;
            ca.Ca_Total = acc.Ac_Coupon + ca.Ca_Value;
            ca.Ca_Info = "积分兑换";
            ca.Ca_Remark = string.Format("用{0}积分兑换{1}卡券", pa.Pa_Value, coupon);
            ca.Ca_Source = WeiSha.Common.Browser.IsMobile ? "手机端" : "电脑网页";
            //最大券值
            object amount = Gateway.Default.Max<CouponAccount>(CouponAccount._.Ca_TotalAmount, CouponAccount._.Ac_ID == accid);
            if (amount == null) amount = 0;
            ca.Ca_TotalAmount = (int)amount + ca.Ca_Value;
            //存入记录
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //存储积分
                    tran.Save<PointAccount>(pa);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_Point }, new object[] { pa.Pa_Total }, Accounts._.Ac_ID == accid);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_PointAmount }, new object[] { pa.Pa_TotalAmount }, Accounts._.Ac_ID == accid);
                    //存储卡券
                    tran.Save<CouponAccount>(ca);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_Coupon }, new object[] { ca.Ca_Total }, Accounts._.Ac_ID == accid);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(accid);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 删除流水
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CouponDelete(CouponAccount entity)
        {
            Gateway.Default.Delete<CouponAccount>(CouponAccount._.Ca_ID == entity.Ca_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void CouponDelete(int identify)
        {
            Gateway.Default.Delete<CouponAccount>(CouponAccount._.Ca_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public CouponAccount CouponSingle(int identify)
        {
            return Gateway.Default.From<CouponAccount>().Where(CouponAccount._.Ca_ID == identify).ToFirst<CouponAccount>();
        }
        /// <summary>
        /// 获取单一实体对象，按流水号
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public CouponAccount CouponSingle(string serial)
        {
            if (string.IsNullOrWhiteSpace(serial)) return null;
            return Gateway.Default.From<CouponAccount>().Where(CouponAccount._.Ca_Serial == serial.Trim()).ToFirst<CouponAccount>();
        }
        /// <summary>
        /// 修改流水信息
        /// </summary>
        /// <param name="entity"></param>
        public void CouponSave(CouponAccount entity)
        {
            Gateway.Default.Save<CouponAccount>(entity);
        }
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public CouponAccount[] CouponCount(int orgid, int stid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= CouponAccount._.Org_ID == orgid;
            if (stid > 0) wc &= CouponAccount._.Ac_ID == stid;
            if (type > 0) wc &= CouponAccount._.Ca_Type == type;
            return Gateway.Default.From<CouponAccount>().Where(wc).OrderBy(CouponAccount._.Ca_CrtTime.Desc).ToArray<CouponAccount>(count);
        }
        public int CouponClac(int acid, int formType, DateTime? start, DateTime? end)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc &= CouponAccount._.Ac_ID == acid;
            if (formType > 0) wc &= CouponAccount._.Ca_From == formType;
            if (start != null) wc &= CouponAccount._.Ca_CrtTime >= (DateTime)start;
            if (end != null) wc &= CouponAccount._.Ca_CrtTime < (DateTime)end;
            object obj = Gateway.Default.Sum<CouponAccount>(CouponAccount._.Ca_Value, wc);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public CouponAccount[] CouponPager(int orgid, int stid, int type, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= CouponAccount._.Org_ID == orgid;
            if (stid > 0) wc &= CouponAccount._.Ac_ID == stid;
            if (type > 0) wc &= CouponAccount._.Ca_Type == type;
            countSum = Gateway.Default.Count<CouponAccount>(wc);
            return Gateway.Default.From<CouponAccount>()
                .Where(wc).OrderBy(CouponAccount._.Ca_CrtTime.Desc).ToArray<CouponAccount>(size, (index - 1) * size);
        }
        public CouponAccount[] CouponPager(int orgid, int stid, int type, DateTime? start, DateTime? end,int size, int index, out int countSum){
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= CouponAccount._.Org_ID == orgid;
            if (stid > 0) wc &= CouponAccount._.Ac_ID == stid;
            if (type > 0) wc &= CouponAccount._.Ca_Type == type;
            if (start != null) wc &= CouponAccount._.Ca_CrtTime >= (DateTime)start;
            if (end != null) wc &= CouponAccount._.Ca_CrtTime < (DateTime)end;
            countSum = Gateway.Default.Count<CouponAccount>(wc);
            return Gateway.Default.From<CouponAccount>()
                .Where(wc).OrderBy(CouponAccount._.Ca_CrtTime.Desc).ToArray<CouponAccount>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public CouponAccount[] CouponPager(int orgid, int stid, int type, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= CouponAccount._.Org_ID == orgid;
            if (stid > 0) wc &= CouponAccount._.Ac_ID == stid;
            if (type > 0) wc &= CouponAccount._.Ca_Type == type;
            if (!string.IsNullOrWhiteSpace(searTxt)) wc &= CouponAccount._.Ca_Info.Like("%" + searTxt + "%");
            countSum = Gateway.Default.Count<CouponAccount>(wc);
            return Gateway.Default.From<CouponAccount>()
                .Where(wc).OrderBy(CouponAccount._.Ca_CrtTime.Desc).ToArray<CouponAccount>(size, (index - 1) * size);
        }
        #endregion

        #region 资金管理
        /// <summary>
        /// 收入
        /// </summary>
        /// <param name="entity">业务实体</param>
        public MoneyAccount MoneyIncome(MoneyAccount entity)
        {
            entity.Ma_CrtTime = DateTime.Now;
            if (entity.Org_ID < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            entity.Ma_Type = 2;
            //流水号
            entity.Ma_Serial = Business.Do<ISystemPara>().Serial();
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    _addition(entity, tran);
                    tran.Save<MoneyAccount>(entity);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
                    return entity;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 支出
        /// </summary>
        /// <param name="entity">业务实体</param>
        public MoneyAccount MoneyPay(MoneyAccount entity)
        {
            entity.Ma_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            entity.Ma_Type = 1;
            //流水号
            entity.Ma_Serial = Business.Do<ISystemPara>().Serial();
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    _subtraction(entity, tran);
                    tran.Save<MoneyAccount>(entity);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
                    return entity;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 通过流水号进行资金支出或收入的确认操作
        /// </summary>
        /// <param name="serial">流水号</param>
        /// <returns></returns>
        public MoneyAccount MoneyConfirm(string serial)
        {
            Song.Entities.MoneyAccount ma = Gateway.Default.From<MoneyAccount>().Where(MoneyAccount._.Ma_Serial == serial).ToFirst<MoneyAccount>();
            return MoneyConfirm(ma);
        }
        /// <summary>
        /// 通过交易记录的对象，进行资金支出或收入的确认操作
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public MoneyAccount MoneyConfirm(MoneyAccount ma)
        {
            if (ma == null) return null;
            if (ma.Ma_IsSuccess) return ma;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    ma.Ma_IsSuccess = true;
                    if (ma.Ma_Type == 1) _subtraction(ma, tran);
                    if (ma.Ma_Type == 2) _addition(ma, tran);
                    tran.Save<MoneyAccount>(ma);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(ma.Ac_ID);
                    return ma;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    tran.Close();
                }
            }
        }
        #region 资金加减
        /// <summary>
        /// 资金增加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private void _addition(MoneyAccount entity, DbTrans tran)
        {
            //原有的钱数
            Decimal deposit = 0;
            Song.Entities.Accounts student = tran.From<Accounts>().Where(Accounts._.Ac_ID == entity.Ac_ID).ToFirst<Accounts>();
            if (student == null) return;
            deposit = student.Ac_Money;
            if (entity.Ma_IsSuccess)
            {
                //当前总金额等于，之前的总金额加上当前收入
                entity.Ma_Total = deposit + entity.Ma_Money;
            }
            else
            {
                entity.Ma_Total = deposit;
            }
            tran.Update<Accounts>(new Field[] { Accounts._.Ac_Money }, new object[] { entity.Ma_Total }, Accounts._.Ac_ID == entity.Ac_ID);
        }
        /// <summary>
        /// 资金减除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private void _subtraction(MoneyAccount entity, DbTrans tran)
        {
            //最后一流水记录
            Song.Entities.MoneyAccount first = tran.From<MoneyAccount>().Where(MoneyAccount._.Ac_ID == entity.Ac_ID).OrderBy(MoneyAccount._.Ma_ID.Desc).ToFirst<MoneyAccount>();
            Decimal total = 0;
            Song.Entities.Accounts student = tran.From<Accounts>().Where(Accounts._.Ac_ID == entity.Ac_ID).ToFirst<Accounts>();
            total = student != null ? student.Ac_Money : first.Ma_Total;
            if (total - entity.Ma_Money < 0) throw new Exception("余额不足，无法支出！");
            if (entity.Ma_IsSuccess)
            {
                //当前总金额等于，之前的总金额减去当前支出
                entity.Ma_Total = total - (Decimal)entity.Ma_Money;
            }
            else
            {
                entity.Ma_Total = total;
            }
            tran.Update<Accounts>(new Field[] { Accounts._.Ac_Money }, new object[] { entity.Ma_Total }, Accounts._.Ac_ID == entity.Ac_ID);
        }
        #endregion
        
        /// <summary>
        /// 删除流水
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void MoneyDelete(MoneyAccount entity)
        {
            Gateway.Default.Delete<MoneyAccount>(MoneyAccount._.Ma_ID == entity.Ma_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void MoneyDelete(int identify)
        {
            Gateway.Default.Delete<MoneyAccount>(MoneyAccount._.Ma_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public MoneyAccount MoneySingle(int identify)
        {
            return Gateway.Default.From<MoneyAccount>().Where(MoneyAccount._.Ma_ID == identify).ToFirst<MoneyAccount>();
        }
        /// <summary>
        /// 获取单一实体对象，按流水号
        /// </summary>
        /// <param name="serial"></param>
        /// <returns></returns>
        public MoneyAccount MoneySingle(string serial)
        {
            if (string.IsNullOrWhiteSpace(serial)) return null;
            return Gateway.Default.From<MoneyAccount>().Where(MoneyAccount._.Ma_Serial == serial.Trim()).ToFirst<MoneyAccount>();
        }
        /// <summary>
        /// 计算资金收益
        /// </summary>
        /// <param name="accid">账号id</param>
        /// <param name="type">1支出，2收入（包括充值、分润等）</param>
        /// <param name="from">类型，来源，1为管理员操作，2为充值码充值；3这在线支付；4购买课程,5分润</param>
        /// <returns></returns>
        public decimal MoneySum(int accid, int type, int from)
        {
            WhereClip wc = MoneyAccount._.Ac_ID == accid;
            wc &= MoneyAccount._.Ma_IsSuccess == true;
            if (type > 0) wc &= MoneyAccount._.Ma_Type == type;
            if (from > 0) wc &= MoneyAccount._.Ma_From == from;
            object tm = Gateway.Default.Sum<MoneyAccount>(MoneyAccount._.Ma_Money, wc);
            decimal sum = 0;
            sum = tm is decimal ? (decimal)tm : 0;
            return sum;
        }
        /// <summary>
        /// 修改流水信息
        /// </summary>
        /// <param name="entity"></param>
        public void MoneySave(MoneyAccount entity)
        {
            Gateway.Default.Save<MoneyAccount>(entity);
        }
        /// <summary>
        /// 获取指定个数的记录
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="isSuccess">是否操作成功</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public MoneyAccount[] MoneyCount(int orgid, int stid, int type, bool? isSuccess, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= MoneyAccount._.Org_ID == orgid;
            if (stid > 0) wc &= MoneyAccount._.Ac_ID == stid;
            if (type > 0) wc &= MoneyAccount._.Ma_Type == type;
            if (isSuccess != null) wc &= MoneyAccount._.Ma_IsSuccess == (bool)isSuccess;
            return Gateway.Default.From<MoneyAccount>().Where(wc).OrderBy(MoneyAccount._.Ma_CrtTime.Desc).ToArray<MoneyAccount>(count);
        }
        /// <summary>
        /// 计算某一个时间区间的现金
        /// </summary>
        /// <param name="acid">学员账户</param>
        /// <param name="formType">1为管理员操作，2为充值码充值；3在线支付；4购买课程,5分润</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int MoneyClac(int acid, int formType, DateTime? start, DateTime? end)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc &= MoneyAccount._.Ac_ID == acid;
            if (formType > 0) wc &= MoneyAccount._.Ma_From == formType;
            if (start != null) wc &= MoneyAccount._.Ma_CrtTime >= (DateTime)start;
            if (end != null) wc &= MoneyAccount._.Ma_CrtTime < (DateTime)end;
            object obj = Gateway.Default.Sum<MoneyAccount>(MoneyAccount._.Ma_Money, wc);
            return Convert.ToInt32(obj);
        }
        /// <summary>
        /// 分页获取所有资金流水；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public MoneyAccount[] MoneyPager(int orgid, int stid, int type, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= MoneyAccount._.Org_ID == orgid;
            if (stid > 0) wc &= MoneyAccount._.Ac_ID == stid;
            if (type > 0) wc &= MoneyAccount._.Ma_Type == type;
            countSum = Gateway.Default.Count<MoneyAccount>(wc);
            return Gateway.Default.From<MoneyAccount>()
                .Where(wc).OrderBy(MoneyAccount._.Ma_CrtTime.Desc).ToArray<MoneyAccount>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有资金流水；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public MoneyAccount[] MoneyPager(int orgid, int stid, int type, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= MoneyAccount._.Org_ID == orgid;
            if (stid > 0) wc &= MoneyAccount._.Ac_ID == stid;
            if (type > 0) wc &= MoneyAccount._.Ma_Type == type;
            if (!string.IsNullOrWhiteSpace(searTxt)) wc &= MoneyAccount._.Ma_Info.Like("%" + searTxt + "%");
            countSum = Gateway.Default.Count<MoneyAccount>(wc);
            return Gateway.Default.From<MoneyAccount>()
                .Where(wc).OrderBy(MoneyAccount._.Ma_CrtTime.Desc).ToArray<MoneyAccount>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有资金流水；
        /// </summary>
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="st">学员id</param>
        /// <param name="type">类型，支出为1，转入2</param>
        /// <param name="searTxt">按信息检索</param>
        /// <param name="start">按时间检索区间，此为开始时间</param>
        /// <param name="end">按时间检索区间，此为结束时间</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public MoneyAccount[] MoneyPager(int orgid, int stid, int type, int from, string searTxt, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= MoneyAccount._.Org_ID == orgid;
            if (stid > -1) wc &= MoneyAccount._.Ac_ID == stid;
            if (type > 0) wc &= MoneyAccount._.Ma_Type == type;
            if (from > 0) wc &= MoneyAccount._.Ma_From == from;
            if (!string.IsNullOrWhiteSpace(searTxt)) wc &= MoneyAccount._.Ma_Info.Like("%" + searTxt + "%");
            if (start != null && ((DateTime)start) > DateTime.Now.AddYears(-100)) wc &= MoneyAccount._.Ma_CrtTime >= ((DateTime)start).Date;
            if (end != null && ((DateTime)end) > DateTime.Now.AddYears(-100)) wc &= MoneyAccount._.Ma_CrtTime < ((DateTime)end).AddDays(1).Date;
            countSum = Gateway.Default.Count<MoneyAccount>(wc);
            return Gateway.Default.From<MoneyAccount>()
                .Where(wc).OrderBy(MoneyAccount._.Ma_CrtTime.Desc).ToArray<MoneyAccount>(size, (index - 1) * size);
        }
        #endregion

        /// <summary>
        /// 当前账户的所有父级账户，依次向上
        /// </summary>
        /// <param name="accid">当前账户id</param>
        /// <returns></returns>
        public Accounts[] Parents(int accid)
        {
            Accounts acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == accid).ToFirst<Accounts>();
            return Parents(acc);
        }
        public Accounts[] Parents(Accounts acc)
        {
            List<Accounts> list = new List<Accounts>();            
            if (acc == null) return list.ToArray();
            //取父级
            Accounts parent = null;
            do
            {
                parent = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == acc.Ac_PID).ToFirst<Accounts>();
                if (parent != null)
                {
                    list.Add(parent);
                    acc = parent;
                }
            }
            while (parent != null);
            return list.ToArray();
        }
    }
}
