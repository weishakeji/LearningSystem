using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Linq;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;



namespace Song.ServiceImpls
{
    public class EmployeeCom : IEmployee
    {
        #region 员工信息管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int Add(EmpAccount entity)
        {
            //如果密码为空
            if (string.IsNullOrWhiteSpace(entity.Acc_Pw))
                entity.Acc_Pw = WeiSha.Common.Login.Get["Admin"].DefaultPw.MD5;
            else
                entity.Acc_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(entity.Acc_Pw).MD5;
            //员所在院系、所处岗位、所在机构
            Depart dep = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == entity.Dep_Id).ToFirst<Depart>();
            if (dep != null) entity.Dep_CnName = dep.Dep_CnName;
            Position pos = Gateway.Default.From<Position>().Where(Position._.Posi_Id == entity.Posi_Id).ToFirst<Position>();
            if (pos != null) entity.Posi_Name = pos.Posi_Name;
            try
            {
                IDCardNumber card = IDCardNumber.Get(entity.Acc_IDCardNumber);
                entity.Acc_Age = card.Birthday.Year;
                entity.Acc_Sex = card.Sex;
                entity.Acc_Birthday = card.Birthday;
            }
            catch { }
            entity.Acc_RegTime = DateTime.Now;
            entity.Acc_OutTime = DateTime.Now.AddYears(1000);
            Gateway.Default.Save<EmpAccount>(entity);

            //获取当前名称的最后一个对象，即刚录入的对象
            entity = Gateway.Default.From<EmpAccount>().OrderBy(EmpAccount._.Acc_Id.Desc).ToFirst<EmpAccount>();
            return entity.Acc_Id;
            
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(EmpAccount entity)
        {
            try
            {
                //解析身份证信息，取年龄、性别等
                IDCardNumber card = IDCardNumber.Get(entity.Acc_IDCardNumber);
                entity.Acc_Age = card.Birthday.Year;
                entity.Acc_Sex = card.Sex;
                entity.Acc_Birthday = card.Birthday;
            }
            catch { }         
            //员所在院系，与所处岗位
            Song.Entities.Depart dep = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == entity.Dep_Id).ToFirst<Depart>();
            if (dep != null)      
                entity.Dep_CnName = dep.Dep_CnName;
            Song.Entities.Position pos = Gateway.Default.From<Position>().Where(Position._.Posi_Id == entity.Posi_Id).ToFirst<Position>();
            if (pos != null)
                entity.Posi_Name = pos.Posi_Name;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {

                    //当修改员工帐号时
                    tran.Save<EmpAccount>(entity);
                    //tran.Update<Picture>(new Field[] { Picture._.Acc_Name }, new object[] { entity.Acc_Name }, Picture._.Acc_Id == entity.Acc_Id,tran);
                    //Gateway.Default.Update<Product>(new Field[] { Product._.Acc_Name }, new object[] { entity.Acc_Name }, Product._.Acc_Id == entity.Acc_Id,tran);
                    tran.Update<Article>(new Field[] { Article._.Acc_Name }, new object[] { entity.Acc_Name }, Article._.Acc_Id == entity.Acc_Id);
                    //Gateway.Default.Update<Video>(new Field[] { Video._.Acc_Name }, new object[] { entity.Acc_Name }, Video._.Acc_Id == entity.Acc_Id);
                    //Gateway.Default.Update<Download>(new Field[] { Download._.Acc_Name }, new object[] { entity.Acc_Name }, Download._.Acc_Id == entity.Acc_Id);
                    //工作日志的信息
                    tran.Update<DailyLog>(new Field[] { DailyLog._.Acc_Name }, new object[] { entity.Acc_Name }, DailyLog._.Acc_Id == entity.Acc_Id);
                    //任务管理
                    tran.Update<Task>(new Field[] { Task._.Acc_Name }, new object[] { entity.Acc_Name }, Task._.Acc_Id == entity.Acc_Id);
                    tran.Update<Task>(new Field[] { Task._.Task_WorkerName }, new object[] { entity.Acc_Name }, Task._.Task_WorkerId == entity.Acc_Id);
                    tran.Commit();
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
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Delete(EmpAccount entity)
        {
            if (entity == null) return;
            //如果用户属于超管角色，则不允许删除
            Position p = Gateway.Default.From<Position>().Where(Position._.Posi_Id == entity.Posi_Id).ToFirst<Position>();
            if (p != null && p.Posi_IsAdmin == true) throw new Exception("管理员不可以删除！");
            //删除与用户组的关联
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<EmpAcc_Group>(EmpAcc_Group._.Acc_Id == entity.Acc_Id);
                    tran.Delete<EmpAccount>(entity);                   
                    WeiSha.WebControl.FileUpload.Delete("Employee", entity.Acc_Photo);
                    tran.Commit();
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
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {           
            EmpAccount ea = this.GetSingle(identify);
            this.Delete(ea);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public EmpAccount GetSingle(int identify)
        {
            return Gateway.Default.From<EmpAccount>().Where(EmpAccount._.Acc_Id == identify)
                .OrderBy(EmpAccount._.Acc_Id.Desc).ToFirst<EmpAccount>();
        }
        /// <summary>
        /// 根据公司id获取当前公司管理员。
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public EmpAccount GetAdminByOrgId(int orgid)
        {
            //先获取普通管理员岗位
            Position pos = Gateway.Default.From<Position>().Where(Position._.Posi_IsAdmin == true && Position._.Org_ID == orgid).ToFirst<Position>();
            if (pos == null) return null;
            EmpAccount acc = Gateway.Default.From<EmpAccount>().Where(EmpAccount._.Posi_Id == pos.Posi_Id).OrderBy(EmpAccount._.Acc_Id.Asc).ToFirst<EmpAccount>();
            return acc;
        }
        /// <summary>
        /// 获取单一实体对象，按员工手机号码
        /// </summary>
        /// <param name="acc">员工手机号码</param>
        /// <returns></returns>
        public EmpAccount GetSingleByPhone(string phoneNumber)
        {
            return Gateway.Default.From<EmpAccount>().Where(EmpAccount._.Acc_MobileTel == phoneNumber).ToFirst<EmpAccount>();
        }
        /// <summary>
        /// 获取单一实体对象，按员工名称
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public EmpAccount GetSingleByName(string name)
        {
            return Gateway.Default.From<EmpAccount>().Where(EmpAccount._.Acc_Name == name).ToFirst<EmpAccount>();
        }
        /// <summary>
        /// 获取单一实体对象，按员工帐号名称与密码
        /// </summary>
        /// <param name="acc">员工帐号名称</param>
        /// <param name="pw">员工密码,MD5加密字符串</param>
        /// <returns></returns>
        public EmpAccount GetSingle(string acc, string pw)
        {
            return GetSingle(-1, acc, pw);
        }
        public EmpAccount GetSingle(int orgid, string acc, string pw)
        {
            Song.Entities.EmpAccount ea;
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= EmpAccount._.Org_ID == orgid;            
            wc.And(EmpAccount._.Acc_Pw == new WeiSha.Common.Param.Method.ConvertToAnyValue(pw).MD5);
            //先用帐号
            ea = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_AccName == acc).ToFirst<EmpAccount>();
            //如果帐号不正确，用手机号
            if (ea == null) ea = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_MobileTel == acc).ToFirst<EmpAccount>();
            //用员工编号
            if (ea == null) ea = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_IDCardNumber == acc).ToFirst<EmpAccount>();
  
            return ea;
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">账号，或身份证，或手机</param>
        /// <param name="pw">密码</param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public EmpAccount EmpLogin(string acc, string pw, int orgid)
        {
            WhereClip wc = EmpAccount._.Org_ID == orgid;
            string pwMd5= new WeiSha.Common.Param.Method.ConvertToAnyValue(pw).MD5;
            wc.And(EmpAccount._.Acc_Pw == pwMd5);
            Song.Entities.EmpAccount entity = null;
            if (entity == null) entity = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_AccName == acc).ToFirst<EmpAccount>();
            if (entity == null) entity = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_MobileTel == acc).ToFirst<EmpAccount>();
            if (entity == null) entity = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_IDCardNumber == acc).ToFirst<EmpAccount>();
            return entity;
        }
        /// <summary>
        /// 获取当前员工所在的院系
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Depart Get4Depart(int identify)
        {
            EmpAccount ea = this.GetSingle(identify);
            if (ea == null) return null;
            Depart dep = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == ea.Dep_Id).ToFirst<Depart>();
            return dep;
        }
        /// <summary>
        /// 当前员工是否为超级管理员
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public bool IsSuperAdmin(int identify)
        {
            if (identify < 1) return false;
            //先获取员工对象，如果为空则返回false;
            EmpAccount ea = this.GetSingle(identify);
            if (ea == null) return false;
            return IsSuperAdmin(ea);
        }
        public bool IsForRoot(int identify)
        {
            if (identify < 1) return false;
            //先获取员工对象，如果为空则返回false;
            EmpAccount ea = this.GetSingle(identify);
            Song.Entities.Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_IsRoot == true && Organization._.Org_ID == ea.Org_ID).ToFirst<Organization>();
            return org != null;
        }
        /// <summary>
        /// 当前用户是否为超级管理员
        /// </summary>
        /// <param name="acc">当前用户对象</param>
        /// <returns></returns>
        public bool IsSuperAdmin(EmpAccount acc)
        {
            if (acc == null) return false;
            //获取所在机构
            Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == acc.Org_ID).ToFirst<Organization>();
            if (org == null) return false;
            //通过员工所处的岗位ID，获取岗位对象，如果岗位不存在，则返回false;
            Position p = Gateway.Default.From<Position>().Where(Position._.Posi_Id == acc.Posi_Id).ToFirst<Position>();
            if (p == null) return false;
            //通过岗位属性，判断是否为超管
            return org.Org_IsRoot && p.Posi_IsAdmin;
        }
        public bool IsAdmin(int identify)
        {
            if (identify < 1) return false;
            //先获取员工对象，如果为空则返回false;
            EmpAccount ea = this.GetSingle(identify);
            //通过员工所处的岗位ID，获取岗位对象，如果岗位不存在，则返回false;
            Position p = Gateway.Default.From<Position>().Where(Position._.Posi_Id == ea.Posi_Id).ToFirst<Position>();
            if (p == null) return false;
            return p.Posi_IsAdmin;
        }
        /// <summary>
        /// 当前员工是否存在（通过帐号判断）
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool IsExists(int orgid, EmpAccount acc)
        {
            WhereClip wc = new WhereClip();
            if (orgid < 1) wc.And(EmpAccount._.Org_ID == acc.Org_ID);
            else
                wc.And(EmpAccount._.Org_ID == orgid);
            wc.And(EmpAccount._.Acc_AccName == acc.Acc_AccName);
            int obj = Gateway.Default.Count<EmpAccount>(wc && EmpAccount._.Acc_Id != acc.Acc_Id);
            return obj > 0;
        }
        public bool LoginCheck(int orgid, string acc, string pw)
        {
            pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(pw).MD5;
            WhereClip wc = EmpAccount._.Acc_Pw == pw;
            wc.And(EmpAccount._.Org_ID == orgid);            
            Song.Entities.EmpAccount ea;
            //先用帐号
            ea = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_AccName == acc).ToFirst<EmpAccount>();
            //如果帐号不正确，用手机号
            if (ea == null)
            {
                ea = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_MobileTel == acc).ToFirst<EmpAccount>();
            }
            //用员工编号
            if (ea == null)
            {
                ea = Gateway.Default.From<EmpAccount>().Where(wc && EmpAccount._.Acc_EmpCode == acc).ToFirst<EmpAccount>();
            }        
             if (ea == null) return false;
            //判断是否在职
             return ea.Acc_IsUse;            
        }
        /// <summary>
        /// 通过手机号码验证，当前员工是否为在职员工
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        public bool IsOnJob(string phoneNumber)
        {
            //验证员工帐号与密码
            WhereClip wc = EmpAccount._.Acc_MobileTel.Like("%" + phoneNumber + "%");
            //员工必须在职
            wc.And(EmpAccount._.Acc_IsUse == true);
            EmpAccount ac = Gateway.Default.From<EmpAccount>().Where(wc).ToFirst<EmpAccount>();
            if (ac == null) return false;
            return true;
        }
        /// <summary>
        /// 获取对象；即所有员工帐号；
        /// </summary>
        /// <returns></returns>
        public EmpAccount[] GetAll(int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(EmpAccount._.Org_ID == orgid);
            return Gateway.Default.From<EmpAccount>().Where(wc)
                .OrderBy(EmpAccount._.Acc_NamePinyin.Asc).ToArray<EmpAccount>();
        }
       
        public EmpAccount[] GetAll(int orgid, int depId, bool? isUse, string searTxt)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(EmpAccount._.Org_ID == orgid);
            if (depId > 0) wc.And(EmpAccount._.Dep_Id == depId);
            if (isUse != null) wc.And(EmpAccount._.Acc_IsUse == isUse);
            if (!string.IsNullOrWhiteSpace(searTxt) && searTxt != "") wc.And(EmpAccount._.Acc_Name.Like("%" + searTxt + "%"));
            return Gateway.Default.From<EmpAccount>().Where(wc).OrderBy(EmpAccount._.Acc_NamePinyin.Asc).ToArray<EmpAccount>();
        }
        /// <summary>
        /// 获取某个分厂的所有员工帐号；
        /// </summary>
        /// <param name="orgid">分厂id</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt">员工名称</param>
        /// <returns></returns>
        public EmpAccount[] GetAll4Org(int orgid, bool? isUse, string searTxt)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(EmpAccount._.Org_ID == orgid);
            if (isUse != null) wc.And(EmpAccount._.Acc_IsUse == (bool)isUse);
            if (!string.IsNullOrEmpty(searTxt) && searTxt != "") wc.And(EmpAccount._.Acc_Name.Like("%" + searTxt + "%"));
            return Gateway.Default.From<EmpAccount>().Where(wc).OrderBy(EmpAccount._.Acc_EmpCode.Asc).ToArray<EmpAccount>();
        }
        /// <summary>
        /// 分页获取所有的员工帐号；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public EmpAccount[] GetPager(int orgid, int size, int index, out int countSum)
        {
            WhereClip wc = EmpAccount._.Org_ID == orgid;
            countSum = Gateway.Default.Count<EmpAccount>(wc);
            return Gateway.Default.From<EmpAccount>().Where(wc).OrderBy(EmpAccount._.Acc_RegTime.Desc).ToArray<EmpAccount>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取某院系，所有的员工帐号；如果院系dep_id为0，则取不归属院系的员工
        /// </summary>
        /// <param name="dep_id">院系Id，如果为0，则取不归属院系的员工</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public EmpAccount[] GetPager(int orgid, int dep_id, int size, int index, out int countSum)
        {
            WhereClip wc = EmpAccount._.Org_ID == orgid && EmpAccount._.Dep_Id == dep_id;
            countSum = Gateway.Default.Count<EmpAccount>(wc);
            return Gateway.Default.From<EmpAccount>().Where(wc).OrderBy(EmpAccount._.Acc_RegTime.Desc).ToArray<EmpAccount>(size, (index - 1) * size);
        }
        public EmpAccount[] GetPager(int orgid, int? dep_id, bool? isUse, string searName, int size, int index, out int countSum)
        {
            WhereClip wc = EmpAccount._.Org_ID == orgid;
            if (dep_id > -1) wc.And(EmpAccount._.Dep_Id == dep_id);
            if (isUse != null) wc.And(EmpAccount._.Acc_IsUse == isUse);
            if (!string.IsNullOrWhiteSpace(searName) && searName != "") wc.And(EmpAccount._.Acc_Name.Like("%" + searName + "%"));       
            countSum = Gateway.Default.Count<EmpAccount>(wc);
            return Gateway.Default.From<EmpAccount>().Where(wc).OrderBy(EmpAccount._.Acc_RegTime.Desc).ToArray<EmpAccount>(size, (index - 1) * size);
        }
        #endregion

        #region 职务（头衔）管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void TitileAdd(EmpTitle entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<EmpTitle>(EmpTitle._.Title_Tax, EmpTitle._.Title_Tax > -1);
            int tax = obj is int ? (int)obj : 0;
            entity.Title_Tax = tax + 1;
            Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == entity.Org_ID).ToFirst<Organization>();
            if (org != null) entity.Org_Name = org.Org_Name; 
            Gateway.Default.Save<EmpTitle>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void TitleSave(EmpTitle entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<EmpTitle>(entity);
                    tran.Update<EmpAccount>(new Field[] { EmpAccount._.Title_Name }, new object[] { entity.Title_Name }, EmpAccount._.Title_Id == entity.Title_Id);
                    tran.Commit();
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
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void TitleDelete(EmpTitle entity)
        {
            this.TitleDelete(entity.Title_Id);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void TitleDelete(int identify)
        {
            Gateway.Default.Delete<EmpTitle>(EmpTitle._.Title_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public EmpTitle TitleSingle(int identify)
        {
            return Gateway.Default.From<EmpTitle>().Where(EmpTitle._.Title_Id == identify).OrderBy(EmpTitle._.Title_Id.Desc).ToFirst<EmpTitle>();
        }
        /// <summary>
        /// 获取对象；即所有职位；
        /// </summary>
        /// <returns></returns>
        public EmpTitle[] TitleAll(int orgid)
        {
            return Gateway.Default.From<EmpTitle>().Where(EmpTitle._.Org_ID == orgid).OrderBy(EmpTitle._.Title_Tax.Asc).ToArray<EmpTitle>();
        }
        public EmpTitle[] TitleAll(int orgid,bool? isUse)
        {
            if (isUse == null) return this.TitleAll(orgid);
            return Gateway.Default.From<EmpTitle>()
                .Where(EmpTitle._.Org_ID == orgid && EmpTitle._.Title_IsUse == (bool)isUse)
                .OrderBy(EmpTitle._.Title_Tax.Asc).ToArray<EmpTitle>();
        }
        /// <summary>
        /// 获取当前职务的所有员工
        /// </summary>
        /// <param name="titleid">职务Id</param>
        /// <param name="isUse">是否在职</param>
        /// <returns></returns>
        public EmpAccount[] Title4Emplyee(int titleid, bool? isUse)
        {
            WhereClip wc = EmpAccount._.Title_Id == titleid;
            if (isUse != null) wc.And(EmpAccount._.Acc_IsUse == (bool)isUse);
            return Gateway.Default.From<EmpAccount>().Where(wc).OrderBy(EmpAccount._.Acc_Id.Asc).ToArray<EmpAccount>();
        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        public bool TitleIsExist(int orgid,EmpTitle entity)
        {
            EmpTitle mm = null;
            //如果是一个新对象
            if (entity.Title_Id == 0)
            {
                mm = Gateway.Default.From<EmpTitle>().Where(EmpTitle._.Org_ID == orgid && EmpTitle._.Title_Name == entity.Title_Name).ToFirst<EmpTitle>();
            }
            else
            {
                //如果是一个已经存在的对象，则不匹配自己
                mm = Gateway.Default.From<EmpTitle>()
                    .Where(EmpTitle._.Org_ID == orgid && EmpTitle._.Title_Name == entity.Title_Name && EmpTitle._.Title_Id != entity.Title_Id)
                    .ToFirst<EmpTitle>();
            }
            return mm != null;
        }
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool TitleRemoveUp(int orgid,int id)
        {
            //当前对象
            EmpTitle current = Gateway.Default.From<EmpTitle>().Where(EmpTitle._.Title_Id == id).ToFirst<EmpTitle>();
            //当前对象排序号
            int orderValue = (int)current.Title_Tax; ;
            //上一个对象，即兄长对象；
            EmpTitle up = Gateway.Default.From<EmpTitle>()
                .Where(EmpTitle._.Org_ID == orgid && EmpTitle._.Title_Tax < orderValue)
                .OrderBy(EmpTitle._.Title_Tax.Desc).ToFirst<EmpTitle>();
            if (up == null) return false;
            //交换排序号
            current.Title_Tax = up.Title_Tax;
            up.Title_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<EmpTitle>(current);
                    tran.Save<EmpTitle>(up);
                    tran.Commit();
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
            return true;
        }
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool TitleRemoveDown(int orgid,int id)
        {
            //当前对象
            EmpTitle current = Gateway.Default.From<EmpTitle>().Where(EmpTitle._.Title_Id == id).ToFirst<EmpTitle>();
            //当前对象排序号
            int orderValue = (int)current.Title_Tax;
            //下一个对象，即弟弟对象；
            EmpTitle down = Gateway.Default.From<EmpTitle>()
                .Where(EmpTitle._.Org_ID == orgid && EmpTitle._.Title_Tax > orderValue)
                .OrderBy(EmpTitle._.Title_Tax.Asc).ToFirst<EmpTitle>();
            if (down == null) return false;
            //交换排序号
            current.Title_Tax = down.Title_Tax;
            down.Title_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<EmpTitle>(current);
                    tran.Save<EmpTitle>(down);
                    tran.Commit();
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
            return true;
        }
        #endregion
    }
}
