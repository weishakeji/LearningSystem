using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using WeiSha.Core;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Net;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Threading.Tasks;

namespace Song.ServiceImpls
{
    public class StudentCom : IStudent
    {

        #region 学员分类
        public void SortAdd(StudentSort entity)
        {
            if (entity.Sts_ID <= 0) entity.Sts_ID = WeiSha.Core.Request.SnowID();
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<StudentSort>(StudentSort._.Sts_Tax, StudentSort._.Org_ID == entity.Org_ID);
            entity.Sts_Tax = obj is int ? (int)obj + 1 : 0;
            Gateway.Default.Save<StudentSort>(entity);
        }

        public void SortSave(StudentSort entity)
        {
            StudentSort original=Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == entity.Sts_ID).ToFirst<StudentSort>();
            if (original == null) return;
            //如果修改了使用状态
            if (entity.Sts_IsUse != original.Sts_IsUse)
            {
                this.SortUpdateUse(entity.Sts_ID, entity.Sts_IsUse);
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<StudentSort>(entity);
                    tran.Update<Accounts>(new Field[] { Accounts._.Sts_Name }, new object[] { entity.Sts_Name }, Accounts._.Sts_ID == entity.Sts_ID);
                    if (entity.Sts_IsDefault)
                    {
                        tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { false },
                            StudentSort._.Sts_ID != entity.Sts_ID && StudentSort._.Org_ID == entity.Org_ID);
                        tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { true },
                            StudentSort._.Sts_ID == entity.Sts_ID && StudentSort._.Org_ID == entity.Org_ID);
                    }
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
        /// 修改学员组的状态
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <param name="use">是否启用</param>
        /// <returns></returns>
        public bool SortUpdateUse(long stsid, bool use)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {

                    tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsUse }, new object[] { use }, StudentSort._.Sts_ID == stsid);

                    List<Course> list = Gateway.Default.From<Course>()
               .InnerJoin<StudentSort_Course>(Course._.Cou_ID == StudentSort_Course._.Cou_ID)
               .Where(StudentSort_Course._.Sts_ID == stsid).OrderBy(StudentSort_Course._.Ssc_ID.Desc).ToList<Course>();
                    foreach (Course cou in list)
                    {
                        tran.Update<Student_Course>(
                      new Field[] { Student_Course._.Stc_IsEnable },
                      new object[] { use },
                      Student_Course._.Stc_Type == 5 &&
                      Student_Course._.Sts_ID == stsid && Student_Course._.Cou_ID == cou.Cou_ID);
                    }

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
        public int SortDelete(long identify)
        {
            Accounts st = Gateway.Default.From<Accounts>().Where(Accounts._.Sts_ID == identify).ToFirst<Accounts>();
            if (st != null) throw new WeiSha.Core.ExceptionForAlert("当前学员组下有学员信息，不可删除");
            //学员组关联的课程数
            int count = this.SortCourseCount(identify);
            if (count > 0) throw new WeiSha.Core.ExceptionForAlert("当前学员组关联有课程，不可删除");

            return Gateway.Default.Delete<StudentSort>(StudentSort._.Sts_ID == identify);
        }

        public StudentSort SortSingle(long identify)
        {
            return Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == identify).ToFirst<StudentSort>();
        }

        public StudentSort SortDefault(int orgid)
        {
            return Gateway.Default.From<StudentSort>().Where(StudentSort._.Org_ID == orgid && StudentSort._.Sts_IsDefault == true).ToFirst<StudentSort>();
        }

        public void SortSetDefault(int orgid, long identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { false }, StudentSort._.Org_ID == orgid);
                    tran.Update<StudentSort>(new Field[] { StudentSort._.Sts_IsDefault }, new object[] { true }, StudentSort._.Sts_ID == identify);
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

        public StudentSort[] SortAll(int orgid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(StudentSort._.Org_ID == orgid);
            if (isUse != null) wc.And(StudentSort._.Sts_IsUse == isUse);
            return Gateway.Default.From<StudentSort>().Where(wc).OrderBy(StudentSort._.Sts_Tax.Asc).ToArray<StudentSort>();
        }

        public List<StudentSort> SortCount(int orgid, bool? isUse, int count)
        {
            WhereClip wc = StudentSort._.Org_ID == orgid;
            if (isUse != null) wc.And(StudentSort._.Sts_IsUse == isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<StudentSort>().Where(wc).OrderBy(StudentSort._.Sts_Tax.Asc).ToList<StudentSort>(count);
        }

        public StudentSort Sort4Student(int studentId)
        {
            Accounts st = Business.Do<IAccounts>().AccountsSingle(studentId);
            if (st == null) return null;
            return Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == st.Sts_ID).ToFirst<StudentSort>();
        }

        public Accounts[] Student4Sort(long stsid, bool? isUse)
        {
            WhereClip wc = Accounts._.Sts_ID == stsid;
            if (isUse != null) wc.And(Accounts._.Ac_IsUse == isUse);
            return Gateway.Default.From<Accounts>().Where(wc).ToArray<Accounts>();
        }
        /// <summary>
        /// 学员组中的学员数量
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        public int SortOfNumber(long stsid)
        {
            return Gateway.Default.Count<Accounts>(Accounts._.Sts_ID == stsid);
        }
        public bool SortIsExist(StudentSort entity)
        {
            //如果是一个已经存在的对象，则不匹配自己
            StudentSort mm = Gateway.Default.From<StudentSort>()
                   .Where(StudentSort._.Org_ID == entity.Org_ID && StudentSort._.Sts_Name == entity.Sts_Name && StudentSort._.Sts_ID != entity.Sts_ID)
                   .ToFirst<StudentSort>();
            return mm != null;
        }
        /// <summary>
        /// 分页获取学员组
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="name">分组名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public StudentSort[] SortPager(int orgid, bool? isUse, string name, int size, int index, out int countSum)
        {
            WhereClip wc = StudentSort._.Org_ID == orgid;            
            if (isUse != null) wc.And(StudentSort._.Sts_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(name) && name.Trim() != "") wc.And(StudentSort._.Sts_Name.Like("%" + name + "%"));
            countSum = Gateway.Default.Count<StudentSort>(wc);
            return Gateway.Default.From<StudentSort>().Where(wc).OrderBy(StudentSort._.Sts_Tax.Asc).ToArray<StudentSort>(size, (index - 1) * size);
        }
        /// <summary>
        /// 更改学员组的排序
        /// </summary>
        /// <param name="items">学员组的实体数组</param>
        /// <returns></returns>
        public bool SortUpdateTaxis(Song.Entities.StudentSort[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (StudentSort item in items)
                    {
                        tran.Update<StudentSort>(
                            new Field[] { StudentSort._.Sts_Tax },
                            new object[] { item.Sts_Tax },
                            StudentSort._.Sts_ID == item.Sts_ID);
                    }
                    tran.Commit();
                    return true;
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
        #endregion

        #region 学员组与课程
        /// <summary>
        /// 增加学员组与课程的关联
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public int SortCourseAdd(long stsid, long couid)
        {
            StudentSort_Course ssc = new StudentSort_Course();
            ssc.Sts_ID = stsid;
            ssc.Cou_ID = couid;
            ssc.Ssc_IsEnable = true;

            ssc.Ssc_StartTime = DateTime.Now;
            ssc.Ssc_EndTime = DateTime.Now.AddYears(200);
            return this.SortCourseAdd(ssc);
        }
        /// <summary>
        /// 增加学员组与课程的关联
        /// </summary>
        /// <param name="ssc"></param>
        /// <returns>新增对象的id</returns>
        public int SortCourseAdd(StudentSort_Course ssc)
        {
            int count = Gateway.Default.Count<StudentSort_Course>(StudentSort_Course._.Sts_ID == ssc.Sts_ID && StudentSort_Course._.Cou_ID == ssc.Cou_ID);
            if (count < 1)
            {   
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    try
                    {
                        tran.Save<StudentSort_Course>(ssc);
                        //在学员与课程的记录中，相关课程开启
                        tran.Update<Student_Course>(
                           new Field[] { Student_Course._.Stc_IsEnable },
                           new object[] { true },
                           Student_Course._.Stc_Type == 5 &&
                           Student_Course._.Sts_ID == ssc.Sts_ID && Student_Course._.Cou_ID == ssc.Cou_ID);
                        tran.Commit();
                        this._update_cache(ssc.Sts_ID);
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
            return ssc.Ssc_ID;
        }
        /// <summary>
        /// 修改学员组与课程的关联
        /// </summary>
        /// <param name="ssc"></param>
        /// <returns></returns>
        public int SortCourseSave(StudentSort_Course ssc)
        {
            Gateway.Default.Save<StudentSort_Course>(ssc);
            return ssc.Ssc_ID;
        }
        /// <summary>
        /// 学员组关联的课程数
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        public int SortCourseCount(long stsid)
        {
            return Gateway.Default.Count<StudentSort_Course>(StudentSort_Course._.Sts_ID == stsid);
        }
        /// <summary>
        /// 删除学员组与课程的关联
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public bool SortCourseDelete(long stsid, long couid)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<StudentSort_Course>(StudentSort_Course._.Sts_ID == stsid && StudentSort_Course._.Cou_ID == couid);
                    //在学员与课程的记录中，相关课程禁用
                    tran.Update<Student_Course>(
                       new Field[] { Student_Course._.Stc_IsEnable },
                       new object[] { false },
                       Student_Course._.Stc_Type == 5 &&
                       Student_Course._.Sts_ID == stsid && Student_Course._.Cou_ID == couid);
                    tran.Commit();
                    this._update_cache(stsid);
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
            return true;
        }
        /// <summary>
        /// 判断某个课程是否存在于学员组
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stsid">学员组id</param>
        /// <returns></returns>
        public bool SortExistCourse(long couid, long stsid)
        {
            if (stsid <= 0) return false;
            StudentSort sort = Gateway.Default.From<StudentSort>().Where(StudentSort._.Sts_ID == stsid).ToFirst<StudentSort>();
            if (sort == null || !sort.Sts_IsUse) return false;
            //
            object obj = Cache.EntitiesCache.GetObject<StudentSort_Course>(stsid);
            if (obj == null) obj = this._update_cache(stsid);
            if (obj == null) return false;

            if (!(obj is List<Course>)) return false;
            List<Course> list = (List<Course>)obj;
            int index = list.FindIndex(e => e.Cou_ID == couid);
            return index > 0;

            //int count = Gateway.Default.Count<StudentSort_Course>(StudentSort_Course._.Sts_ID == stsid && StudentSort_Course._.Cou_ID == couid);
            //return count > 0;
        }
        /// <summary>
        /// 将学员组关联的课程，创建到Student_Course表（学员与课程的关联）
        /// </summary>      
        /// <param name="couid">课程id</param>
        /// <param name="acid">学员账号id</param>
        /// <returns></returns>
        public Student_Course SortCourseToStudent(int acid, long couid)
        {
            Accounts acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == acid).ToFirst<Accounts>();
            if (acc == null) return null;
            return this.SortCourseToStudent(acc, couid);
        }
        /// <summary>
        /// 将学员组关联的课程，创建到Student_Course表（学员与课程的关联）
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public Student_Course SortCourseToStudent(Accounts acc, long couid)
        {
            //判断学员组是否存在或被禁用
            StudentSort sort = null;
            if (acc.Sts_ID > 0)
                sort = this.SortSingle(acc.Sts_ID);
            if (sort == null || !sort.Sts_IsUse) return null;

            //判断课程是否与当前学员组存在关联
            bool isexist = this.SortExistCourse(couid, acc.Sts_ID);
            if (!isexist) return null;

            //如果已经存在，则直接返回
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(acc.Ac_ID, couid);
            if (sc != null) return sc;

            //创建学员与课程的记录
            sc = new Entities.Student_Course();
            sc.Stc_CrtTime = DateTime.Now;
            sc.Sts_ID = acc.Sts_ID;

            sc.Cou_ID = couid;
            sc.Ac_ID = acc.Ac_ID;
            sc.Stc_Money = 0;
            sc.Stc_Coupon = 0;
            sc.Stc_StartTime = DateTime.Now;
            sc.Stc_EndTime = DateTime.Now;
            sc.Stc_IsFree = false;
            sc.Stc_IsTry = false;
            sc.Stc_IsEnable = true;
            sc.Stc_Type = 5;    //免费为0，试用为1，购买为2，后台开课为3,学员组关联为5
            sc.Org_ID = acc.Org_ID;
            if (sc.Org_ID <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                sc.Org_ID = org.Org_ID;
            }
            Gateway.Default.Save<Student_Course>(sc);
            return sc;
        }
        /// <summary>
        /// 学员组关联的所有课程
        /// </summary>
        /// <param name="stsid">学员组的id</param>
        /// <param name="name">按名称检索</param>
        /// <returns></returns>
        public List<Course> SortCourseList(long stsid, string name)
        {
            WhereClip wc = new WhereClip();
            if (!string.IsNullOrWhiteSpace(name)) wc.And(Course._.Cou_Name.Like("%" + name.Trim() + "%"));
            return Gateway.Default.From<Course>()
                .InnerJoin<StudentSort_Course>(Course._.Cou_ID == StudentSort_Course._.Cou_ID)
                .Where(wc).OrderBy(StudentSort_Course._.Ssc_ID.Desc).ToList<Course>();
        }
        /// <summary>
        /// 分页获取学员组关联的课程
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> SortCoursePager(long stsid, string name, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (stsid > 0) wc.And(StudentSort_Course._.Sts_ID == stsid);
            if (!string.IsNullOrWhiteSpace(name)) wc.And(Course._.Cou_Name.Like("%" + name.Trim() + "%"));
            //countSum = Gateway.Default.Count<StudentSort_Course>(wc);
            countSum = Gateway.Default.From<Course>()
                .InnerJoin<StudentSort_Course>(Course._.Cou_ID == StudentSort_Course._.Cou_ID)
                .Where(wc).OrderBy(StudentSort_Course._.Ssc_ID.Desc).Count();

            return Gateway.Default.From<Course>()
                .InnerJoin<StudentSort_Course>(Course._.Cou_ID == StudentSort_Course._.Cou_ID)
                .Where(wc).OrderBy(StudentSort_Course._.Ssc_ID.Desc).ToList<Course>(size, (index - 1) * size);          
        }
        /// <summary>
        /// 更新学员组与课程关联的缓存
        /// </summary>
        /// <param name="stsid"></param>
        private List<Course> _update_cache(long stsid)
        {
            List<Course> list = this.SortCourseList(stsid, string.Empty);
            Cache.EntitiesCache.Save<StudentSort_Course>(list, stsid);
            return list;
        }
        #endregion

        #region 学员登录与在线记录
        /// <summary>
        /// 添加登录记录
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public void LogForLoginAdd(Accounts st)
        {
            Song.Entities.LogForStudentOnline entity = new LogForStudentOnline();
            //当前机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            //学员信息
            if (st != null)
            {
                entity.Ac_ID = st.Ac_ID;
                entity.Ac_AccName = st.Ac_AccName;
                entity.Ac_Name = st.Ac_Name;
                entity.Lso_UID = st.Ac_CheckUID;
            }
            //登录相关时间
            entity.Lso_LoginDate = DateTime.Now.Date;
            entity.Lso_LoginTime = DateTime.Now;
            entity.Lso_CrtTime = DateTime.Now;
            entity.Lso_LastTime = DateTime.Now;
            entity.Lso_LogoutTime = DateTime.Now.AddMinutes(1);
            entity.Lso_OnlineTime = (int)(entity.Lso_LogoutTime - entity.Lso_LoginTime).TotalMinutes;
            //登录信息
            entity.Lso_IP = WeiSha.Core.Browser.IP;
            entity.Lso_OS = WeiSha.Core.Browser.OS;
            entity.Lso_Browser = WeiSha.Core.Browser.Name + " " + WeiSha.Core.Browser.Version;
            entity.Lso_Platform = WeiSha.Core.Browser.IsMobile ? "Mobi" : "PC";

            Gateway.Default.Save<LogForStudentOnline>(entity);
        }
        /// <summary>
        /// 修改登录记录
        /// </summary>
        public void LogForLoginFresh(Accounts st, int interval, string plat)
        {
            if (st == null) return;
            //取当前登录记录
            int acid = st.Ac_ID;
            string uid = st.Ac_CheckUID;
            Song.Entities.LogForStudentOnline entity = this.LogForLoginSingle(acid, uid, plat);
            if (entity == null) return;
            //记录时间
            //如果时间跨天了，重新生成记录
            if (entity.Lso_LoginTime.Date < DateTime.Now.Date)
            {
                //Extend.LoginState.Accounts.Write(st);
                this.LogForLoginAdd(st);
            }
            else
            {
                entity.Lso_LogoutTime = DateTime.Now.AddMinutes(1);
                entity.Lso_OnlineTime = (int)(entity.Lso_LogoutTime - entity.Lso_LoginTime).TotalMinutes;
                entity.Lso_BrowseTime += interval;
                Gateway.Default.Save<LogForStudentOnline>(entity);
            }
        }
        /// <summary>
        /// 退出登录之前的记录更新
        /// </summary>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        public void LogForLoginOut(Accounts st, string plat)
        {
            if (st == null) return;
            //取当前登录记录
            string uid = st.Ac_CheckUID;
            Song.Entities.LogForStudentOnline entity = this.LogForLoginSingle(st.Ac_ID, uid, plat);
            if (entity == null) return;
            //记录时间
            entity.Lso_LogoutTime = DateTime.Now;
            entity.Lso_OnlineTime = (int)(entity.Lso_LogoutTime - entity.Lso_LoginTime).TotalMinutes;
            Gateway.Default.Save<LogForStudentOnline>(entity);
        }
        /// <summary>
        /// 根据学员id与登录时生成的Uid返回实体
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="stuid">登录时生成的随机字符串，全局唯一</param>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        /// <returns></returns>
        public LogForStudentOnline LogForLoginSingle(int acid, string stuid, string plat)
        {
            WhereClip wc = new WhereClip();
            wc &= LogForStudentOnline._.Ac_ID == acid;
            wc &= LogForStudentOnline._.Lso_UID == stuid;
            if (!string.IsNullOrWhiteSpace(plat))
            {
                wc &= LogForStudentOnline._.Lso_Platform == plat;
            }
            return Gateway.Default.From<LogForStudentOnline>().Where(wc).OrderBy(LogForStudentOnline._.Lso_LoginTime.Desc).ToFirst<LogForStudentOnline>();
        }
        /// <summary>
        /// 返回记录
        /// </summary>
        /// <param name="identify">记录ID</param>
        /// <returns></returns>
        public LogForStudentOnline LogForLoginSingle(int identify)
        {
            return Gateway.Default.From<LogForStudentOnline>().Where(LogForStudentOnline._.Lso_ID == identify).ToFirst<LogForStudentOnline>();
        }
        /// <summary>
        /// 删除学员在线记录
        /// </summary>
        /// <param name="identify"></param>
        public void StudentOnlineDelete(int identify)
        {
            Gateway.Default.Delete<LogForStudentOnline>(LogForStudentOnline._.Lso_ID == identify);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="acid">学员Id</param>
        /// <param name="platform">学员文章平台，PC或Mobi</param>
        /// <param name="start">统计的开始时间</param>
        /// <param name="end">统计的结束时间</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LogForStudentOnline[] LogForLoginPager(int orgid, int acid, string platform, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            WhereClip wc = LogForStudentOnline._.Org_ID == orgid;
            if (acid > 0) wc.And(LogForStudentOnline._.Ac_ID == acid);
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentOnline._.Lso_Platform == platform);
                }
            }
            if (start != null) wc.And(LogForStudentOnline._.Lso_LoginDate >= (DateTime)start);
            if (end != null) wc.And(LogForStudentOnline._.Lso_LoginDate <= (DateTime)end);
            countSum = Gateway.Default.Count<LogForStudentOnline>(wc);
            return Gateway.Default.From<LogForStudentOnline>().Where(wc).OrderBy(LogForStudentOnline._.Lso_LoginDate.Desc).ToArray<LogForStudentOnline>(size, (index - 1) * size);
        }
        public LogForStudentOnline[] LogForLoginPager(int orgid, int acid, string platform, DateTime? start, DateTime? end, string stname, string stmobi, int size, int index, out int countSum)
        {
            WhereClip wc = LogForStudentOnline._.Org_ID == orgid;
            if (acid > 0) wc.And(LogForStudentOnline._.Ac_ID == acid);
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentOnline._.Lso_Platform == platform);
                }
            }
            if (start != null) wc.And(LogForStudentOnline._.Lso_LoginDate >= (DateTime)start);
            if (end != null) wc.And(LogForStudentOnline._.Lso_LoginDate <= (DateTime)end);
            if (!string.IsNullOrWhiteSpace(stname) && stname.Trim() != "") wc.And(LogForStudentOnline._.Ac_Name.Like("%" + stname + "%"));
            if (!string.IsNullOrWhiteSpace(stmobi) && stmobi.Trim() != "") wc.And(LogForStudentOnline._.Ac_AccName.Like("%" + stmobi + "%"));

            countSum = Gateway.Default.Count<LogForStudentOnline>(wc);
            return Gateway.Default.From<LogForStudentOnline>().Where(wc).OrderBy(LogForStudentOnline._.Lso_LoginDate.Desc).ToArray<LogForStudentOnline>(size, (index - 1) * size);
        }
        #endregion

        #region 学员在线学习的记录 
        /// <summary>
        /// 记录学员学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="st"></param>
        /// <param name="playTime">播放进度</param>
        /// <param name="studyInterval">学习时间，此为时间间隔，每次提交学习时间加这个数</param>
        /// <param name="totalTime">视频总长度</param>
        public void LogForStudyFresh(long couid, long olid, Accounts st, int playTime, int studyInterval, int totalTime)
        {
            if (st == null) return;
            //当前章节的学习记录
            Song.Entities.LogForStudentStudy entity = this.LogForStudySingle(st.Ac_ID, olid);
            if (entity == null)
            {
                LogForStudyUpdate(couid, olid, st, playTime, studyInterval, totalTime);
            }
            else
            {
                LogForStudyUpdate(couid, olid, st, playTime, entity.Lss_StudyTime + studyInterval, totalTime);
            }
        }
        /// <summary>
        /// 记录学员学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">章节id</param>
        /// <param name="st">学员账户</param>
        /// <param name="playTime">播放进度</param>
        /// <param name="studyTime">学习时间，此为累计时间</param>
        /// <param name="totalTime">视频总长度</param>
        /// <returns>学习进度百分比（相对于总时长）</returns>
        public void LogForStudyUpdate(long couid, long olid, Accounts st, int playTime, int studyTime, int totalTime)
        {
            if (st == null || olid <= 0) return;

            string ip = WeiSha.Core.Browser.IP;
            string os = WeiSha.Core.Browser.OS;
            string name = WeiSha.Core.Browser.Name;
            string ver = WeiSha.Core.Browser.Version;
            bool ismobi = WeiSha.Core.Browser.IsMobile;

            Task task = new Task(() =>
            {               
                _LogForStudyUpdate(couid, olid, st, playTime * 1000, studyTime, totalTime * 1000, ip, os, name, ver, ismobi);
            });
            task.Start();
        }
        protected void _LogForStudyUpdate(long couid, long olid, Accounts st, int playTime, int studyTime, int totalTime,
            string ip, string os, string name, string ver, bool ismobi)
        {
            if (couid <= 0)
            {
                Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == olid).ToFirst<Outline>();
                if (outline != null) couid = outline.Cou_ID;
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //当前章节的学习记录
                    //Song.Entities.LogForStudentStudy entity = this.LogForStudySingle(st.Ac_ID, olid);
                    string sql = "SELECT *  FROM [LogForStudentStudy] where Ac_ID={1} and Ol_ID={0} order by Lss_ID desc";
                    sql = string.Format(sql, olid, st.Ac_ID);
                    Song.Entities.LogForStudentStudy log = tran.FromSql(sql).ToFirst<LogForStudentStudy>();
                    if (log == null)
                    {
                        log = new LogForStudentStudy();
                        if (string.IsNullOrWhiteSpace(log.Lss_UID))
                            log.Lss_UID = WeiSha.Core.Request.UniqueID();
                        log.Lss_CrtTime = DateTime.Now;
                        log.Cou_ID = couid;
                        log.Ol_ID = olid;
                        log.Org_ID = st.Org_ID;
                        //学员信息
                        log.Ac_ID = st.Ac_ID;
                        log.Ac_AccName = st.Ac_AccName;
                        log.Ac_Name = st.Ac_Name;
                        //视频长度
                        log.Lss_Duration = totalTime;
                    }
                    if (log.Cou_ID == 0)
                        log.Cou_ID = couid;
                    //登录相关时间
                    log.Lss_LastTime = DateTime.Now;
                    log.Lss_PlayTime = playTime;
                    log.Lss_StudyTime = studyTime;
                    if (log.Lss_Duration < totalTime) log.Lss_Duration = totalTime;
                    //登录信息
                    log.Lss_IP = ip;
                    log.Lss_OS = os;
                    log.Lss_Browser = name + " " + ver;
                    log.Lss_Platform = ismobi ? "Mobi" : "PC";
                    log.Lss_Complete = Math.Floor((double)log.Lss_StudyTime * 1000 / (double)log.Lss_Duration * 10000) / 100;
                    log.Lss_Complete = log.Lss_Complete > 100 ? 100 : log.Lss_Complete;
                    //保存到数据库
                    tran.Save<LogForStudentStudy>(log);
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
        /// 根据学员id与章节id,返回学习记录
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public LogForStudentStudy LogForStudySingle(int acid, long olid)
        {
            WhereClip wc = new WhereClip();
            wc &= LogForStudentStudy._.Ac_ID == acid;
            wc &= LogForStudentStudy._.Ol_ID == olid;
            return Gateway.Default.From<LogForStudentStudy>().Where(wc).OrderBy(LogForStudentStudy._.Lss_ID.Desc).ToFirst<LogForStudentStudy>();
        }
        /// <summary>
        /// 返回记录
        /// </summary>
        /// <param name="identify">记录ID</param>
        /// <returns></returns>
        public LogForStudentStudy LogForStudySingle(int identify)
        {
            return Gateway.Default.From<LogForStudentStudy>().Where(LogForStudentStudy._.Lss_ID == identify).ToFirst<LogForStudentStudy>();
        }
        /// <summary>
        /// 返回学习记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="acid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public LogForStudentStudy[] LogForStudyCount(int orgid, long couid, long olid, int acid, string platform, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(LogForStudentStudy._.Ac_ID == acid);
            if (olid > 0) wc.And(LogForStudentStudy._.Ol_ID == olid);
            if (couid > 0) wc.And(LogForStudentStudy._.Cou_ID == couid);
            if (orgid > 0) wc.And(LogForStudentStudy._.Org_ID == orgid);
         
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentStudy._.Lss_Platform == platform);
                }
            }
            return Gateway.Default.From<LogForStudentStudy>().Where(wc).OrderBy(LogForStudentStudy._.Lss_LastTime.Desc).ToArray<LogForStudentStudy>(count);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="acid">学员Id</param>
        /// <param name="platform">学员文章平台，PC或Mobi</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LogForStudentStudy[] LogForStudyPager(int orgid, long couid, long olid, int acid, string platform, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(LogForStudentStudy._.Ac_ID == acid);
            if (olid > 0) wc.And(LogForStudentStudy._.Ol_ID == olid);
            if (couid > 0) wc.And(LogForStudentStudy._.Cou_ID == couid);         
           
            if (!string.IsNullOrWhiteSpace(platform))
            {
                if (platform.ToLower().Trim() == "pc") platform = "PC";
                if (platform.ToLower().Trim() == "mobi") platform = "Mobi";
                if (platform == "PC" || platform == "Mobi")
                {
                    wc.And(LogForStudentStudy._.Lss_Platform == platform);
                }
            }
            countSum = Gateway.Default.Count<LogForStudentStudy>(wc);
            return Gateway.Default.From<LogForStudentStudy>().Where(wc).OrderBy(LogForStudentStudy._.Lss_LastTime.Desc).ToArray<LogForStudentStudy>(size, (index - 1) * size);
        }
        /// <summary>
        /// 学员的学习记录
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <returns>datatable中,LastTime:最后学习时间； studyTime：累计学习时间，complete：完成度百分比</returns>
        public DataTable StudentStudyCourseLog(int acid)
        {
            Accounts student = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == acid).ToFirst<Accounts>();
            if (student == null) throw new Exception("当前学员不存在！");
            Organization org= Gateway.Default.From<Organization>().Where(Organization._.Org_ID == student.Org_ID).ToFirst<Organization>();
            if (org == null) throw new Exception("学员所在的机构不存在！");
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //容差，例如完成度小于5%，则默认100%
            int tolerance = config["VideoTolerance"].Value.Int32 ?? 5;

            ////清理掉不需要的数据，包括：“章节不存在，章节没有视频，章节禁用或未完成”的学习记录，全部删除
            //WhereClip wc = LogForStudentStudy._.Ac_ID == acid;
            //SourceReader lfs = Gateway.Default.FromSql(string.Format("select Ol_ID from [LogForStudentStudy] where Ac_ID={0} group by Ol_ID",acid)).ToReader();
            //while (lfs.Read())
            //{
            //    long olid = lfs.GetInt64("Ol_ID");
            //    Outline ol = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == olid).ToFirst<Outline>();
            //    if (ol == null || ol.Ol_IsVideo == false || ol.Ol_IsUse == false || ol.Ol_IsFinish == false)
            //    {
            //        Gateway.Default.Delete<LogForStudentStudy>(LogForStudentStudy._.Ol_ID == olid);
            //    }
            //} ;
            //开始计算
            string sql = @"
select c.Cou_ID,Cou_Name,Sbj_ID,lastTime,studyTime,complete from course as c inner join 
	(select cou_id, max(lastTime) as 'lastTime',sum(studyTime) as 'studyTime',		
		sum(case when complete>=100 then 100 else complete end) as 'complete'
		from 
			(SELECT top 90000 ol_id,MAX(cou_id) as 'cou_id', MAX(Lss_LastTime) as 'lastTime', 
				 sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
				 (case  when max(Lss_Duration)>0 then
					 cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100
					 else 0 end
				  ) as 'complete'
			 FROM [LogForStudentStudy]  where {acid}  group by ol_id 
			 ) as s where s.totalTime>0 group by s.cou_id
	) as tm on c.cou_id=tm.cou_id  ";
            //sql = sql.Replace("{orgid}", orgid > 0 ? "org_id=" + orgid : "1=1");
            sql = sql.Replace("{acid}", acid > 0 ? "ac_id=" + acid : "1=1");
            try
            {
                DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ///* 不要删除
                    //*****如果没有购买的，则去除
                    //购买的课程(含概试用的）
                    int count = 0;
                    List<Song.Entities.Course> cous = Business.Do<ICourse>().CourseForStudent(acid, null, 0,null, null);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bool isExist = false;
                        for (int j = 0; j < cous.Count; j++)
                        {
                            if (dt.Rows[i]["Cou_ID"].ToString() == cous[j].Cou_ID.ToString())
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            dt.Rows.RemoveAt(i);
                            i--;
                        }
                    }
                    // * */
                    //计算完成度                   
                    foreach (DataRow dr in dt.Rows)
                    {
                        //课程的累计完成度
                        double complete = Convert.ToDouble(dr["complete"].ToString());
                        //课程id
                        long couid = Convert.ToInt64(dr["Cou_ID"].ToString());
                        int olnum = Business.Do<IOutline>().OutlineOfCount(couid, -1, true, true, true, null);
                        //完成度
                        double peracent = Math.Floor(complete / olnum * 100) / 100;
                        dr["complete"] = peracent >= (100 - tolerance) ? 100 : peracent;
                    }
                }
                return dt;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 学员指定学习课程的记录
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couids">课程id,逗号分隔</param>
        /// <returns></returns>
        public DataTable StudentStudyCourseLog(int stid, string couids)
        {
            DataTable dt = this.StudentStudyCourseLog(stid);
            if (dt == null) return dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                //课程id
                long couid = Convert.ToInt64(dr["Cou_ID"].ToString());
                bool isexist = false;
                foreach (string id in couids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(id)) continue;
                    long sid = 0;
                    long.TryParse(id,out sid);
                    if (sid == 0) continue;
                    if (couid == sid)
                    {
                        isexist = true;
                        break;
                    }
                }
                if (!isexist)
                {
                    dt.Rows.RemoveAt(i);
                    i--;
                }
            }
            return dt;
        }
        /// <summary>
        /// 学员学习某一门课程的完成度
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public DataTable StudentStudyCourseLog(int acid, long couid)
        {
            //课程数据
            DataSet ds = Gateway.Default.FromSql("select * from Course where Cou_ID=" + couid).ToDataSet();
            DataTable dt = ds.Tables[0];
            dt.Columns.Add("lastTime", typeof(DateTime));
            dt.Columns.Add("studyTime", typeof(int));
            dt.Columns.Add("complete", typeof(double));
            if (dt.Rows.Count < 1) return dt;

            Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) throw new Exception("学员所在的机构不存在！");
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //容差，例如完成度小于5%，则默认100%
            int tolerance = config["VideoTolerance"].Value.Int32 ?? 5;

            //获取学习记录
            string sql_log = @"SELECT ol_id,Lss_LastTime,Lss_StudyTime,Lss_Duration,Lss_Complete FROM [LogForStudentStudy] where {acid} and {couid}";
            sql_log = sql_log.Replace("{acid}", acid > 0 ? "ac_id=" + acid : "1=1");
            sql_log = sql_log.Replace("{couid}", couid > 0 ? "Cou_ID=" + couid : "1=1");

            //当前课程的所有视频章节
            List<Song.Entities.Outline> outlines = Business.Do<IOutline>().OutlineAll(couid, true, true, true);
            //最后学习时间
            DateTime lasttime = DateTime.MinValue;
            //累计学习时间，累计完成度
            double totalStudy = 0, totalComplete = 0;
            int count = 0;
            using (SourceReader reader = Gateway.Default.FromSql(sql_log).ToReader())
            {
                while (reader.Read())
                {
                    //章节id
                    long olid = reader.GetValue<long>(0);
                    Song.Entities.Outline outline = outlines.Find(x => x.Ol_ID == olid);
                    if (outline == null) continue;
                    count++;
                    //时间
                    DateTime time = reader.GetValue<DateTime>(1);
                    if (time > lasttime) lasttime = time;
                    //学习时长,单位秒
                    double study = reader.GetValue<double>(2);
                    totalStudy += study;
                    //视频时长，单位秒
                    int dura = reader.GetValue<int>(3) / 1000;
                    //完成度
                    double complete = reader.GetValue<double>(4);
                    totalComplete += complete > 100 - tolerance ? 100 : complete;
                }
                reader.Close();
                reader.Dispose();
            }
            totalComplete = totalComplete / outlines.Count;
            if (double.IsNaN(totalComplete))           
                totalComplete = 0;
          
            totalComplete = totalComplete > 100 ? 100 : totalComplete;
            totalComplete = Math.Round(totalComplete * 100) / 100;
            //合并数据
            dt.Rows[0]["lastTime"] = lasttime;
            dt.Rows[0]["studyTime"] = (int)totalStudy;
            dt.Rows[0]["complete"] = totalComplete;
            return dt;

            ////开始计算
            //string sql = @"select * from course as c inner join 
            //            (
            //             select Cou_ID, max(lastTime) as 'lastTime', SUM(studyTime) as 'studyTime'	
            //             ,cast(convert(decimal(18,4), cast(sum(complete) as float)/COUNT(*)) as float) as 'complete'
            //              from 
            //             (	
            //              select c.*, s.lastTime
            //              ,CASE WHEN s.studyTime is null THEN 0 ELSE s.studyTime END as 'studyTime'
            //              ,CASE WHEN s.complete is null THEN 0 WHEN s.complete>100  THEN 100 ELSE s.complete END as 'complete'
            //              from 
            //              (
            //               (SELECT * from outline where {couid} and Ol_IsUse=1 and Ol_IsFinish=1 and Ol_IsVideo=1)  as c left join 
            //                (SELECT ol_id,MAX(cou_id) as 'cou_id', MAX(Lss_LastTime) as 'lastTime', 
            //                    sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
            //                    (case  when max(Lss_Duration)>0 then
            //                    cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100
            //                    else 0 end
            //                    ) as 'complete'
            //                 FROM [LogForStudentStudy]  where {couid} and  {acid}  group by ol_id 
            //                ) as s on c.Ol_ID=s.Ol_ID
            //              ) 
            //             ) as t group by Cou_ID
            //            ) as tm on c.Cou_ID=tm.Cou_ID";
            ////sql = sql.Replace("{orgid}", orgid > 0 ? "org_id=" + orgid : "1=1");
            //sql = sql.Replace("{acid}", acid > 0 ? "ac_id=" + acid : "1=1");
            //sql = sql.Replace("{couid}", couid > 0 ? "Cou_ID=" + couid : "1=1");
            //DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            //return ds.Tables[0];

        }
        /// <summary>
        /// 学员学习某一课程下所有章节的记录
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="acid">学员账户id</param>
        /// <returns>datatable中，LastTime：最后学习时间；totalTime：视频时间长；playTime：播放进度；studyTime：学习时间，complete：完成度百分比</returns>
        public DataTable StudentStudyOutlineLog(long couid, int acid)
        {
            //当前课程的所有视频章节
            List<Song.Entities.Outline> outlines = Business.Do<IOutline>().OutlineAll(couid, true, true, true);
            if (outlines == null || outlines.Count < 1) return null;
            DataTable dt = WeiSha.Core.DataConvert.ToDataTable<Song.Entities.Outline>(outlines, "Ol_ID");
            dt.Columns.Add("lastTime", typeof(DateTime));
            dt.Columns.Add("studyTime", typeof(int));
            dt.Columns.Add("totalTime", typeof(int));
            dt.Columns.Add("playTime", typeof(int));
            dt.Columns.Add("complete", typeof(double));
            if (dt.Rows.Count < 1) return dt;


            Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) throw new Exception("学员所在的机构不存在！");
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //容差，例如完成度小于5%，则默认100%
            int tolerance = config["VideoTolerance"].Value.Int32 ?? 5;

            //获取学习记录
            string sql_log = @"SELECT ol_id,Lss_LastTime,Lss_StudyTime,Lss_Duration,Lss_PlayTime,Lss_Complete FROM [LogForStudentStudy] where  {acid} and {couid}";
            sql_log = sql_log.Replace("{acid}", acid > 0 ? "ac_id=" + acid : "1=1");
            sql_log = sql_log.Replace("{couid}", couid > 0 ? "Cou_ID=" + couid : "1=1");
            using (SourceReader reader = Gateway.Default.FromSql(sql_log).ToReader())
            {
                while (reader.Read())
                {
                    //章节id
                    long olid = reader.GetValue<long>(0);
                    DataRow dr = dt.Rows.Find(olid);
                    if (dr == null) continue;
                    //最后时间
                    DateTime lasttime = reader.GetValue<DateTime>(1);
                    //学习时长,单位秒
                    double study = reader.GetValue<double>(2);
                    //视频时长，单位毫秒
                    int dura = reader.GetValue<int>(3);
                    //播放进度，单位毫秒
                    int play = reader.GetValue<int>(4);
                    //完成度
                    double complete = reader.GetValue<double>(5);
                    complete = complete > 100 - tolerance ? 100 : complete;

                    dr["lastTime"] = lasttime;
                    dr["studyTime"] = study;
                    dr["totalTime"] = dura;
                    dr["playTime"] = play;
                    dr["complete"] = complete;
                }
                reader.Close();
                reader.Dispose();
            }
            return dt;

            ////读取学员学习记录
            //string sql = @"select
            //            c.*,s.ol_id, s.lastTime,
            //            CASE WHEN s.studyTime is null THEN 0 ELSE s.studyTime END as 'studyTime',
            //            CASE WHEN s.totalTime is null THEN 0 ELSE s.totalTime END as 'totalTime',
            //            CASE WHEN s.playTime is null THEN 0 ELSE s.playTime END as 'playTime',
            //            CASE WHEN s.complete is null THEN 0 WHEN s.complete>100  THEN 100 ELSE s.complete END as 'complete'
            //            from outline as c left join 
            //            (SELECT ol_id, MAX(Lss_LastTime) as 'lastTime', 
	           //             sum(Lss_StudyTime) as 'studyTime', MAX(Lss_Duration) as 'totalTime', MAX([Lss_PlayTime]) as 'playTime',
            //                cast(convert(decimal(18,4),1000* cast(sum(Lss_StudyTime) as float)/sum(Lss_Duration)) as float)*100 as 'complete'

            //              FROM [LogForStudentStudy] where {acid} and  {couid}  and Lss_Duration>0
            //            group by ol_id ) as s
            //            on c.ol_id=s.ol_id where {couid} order by ol_tax asc";
            //sql = sql.Replace("{couid}", "cou_id=" + couid);
            //sql = sql.Replace("{acid}", "ac_id=" + acid);

            //DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            //return ds.Tables[0];           
        }
        /// <summary>
        /// 章节学习记录作弊，直接将学习进度设置为100
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        public void CheatOutlineLog(int stid, long olid)
        {
            Outline ol = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == olid).ToFirst<Outline>();
            if (ol == null) return;
            //获取原记录
            LogForStudentStudy log = Gateway.Default.From<LogForStudentStudy>()
                .Where(LogForStudentStudy._.Ac_ID == stid && LogForStudentStudy._.Ol_ID == olid)
                .ToFirst<LogForStudentStudy>();
            if (log != null)
            {
                Gateway.Default.Update<LogForStudentStudy>(new Field[] { LogForStudentStudy._.Lss_StudyTime, LogForStudentStudy._.Cou_ID },
                    new object[] { log.Lss_Duration / 1000, ol.Cou_ID },
                            LogForStudentStudy._.Lss_ID == log.Lss_ID);
            }
            else
            {
                Accessory acc = Gateway.Default.From<Accessory>().Where(Accessory._.As_Uid == ol.Ol_UID && Accessory._.As_Type == "CourseVideo").ToFirst<Accessory>();
                if (acc == null) return;
                log = new LogForStudentStudy();
                //学习记录中的课程信息
                log.Ol_ID = olid;
                log.Cou_ID = ol.Cou_ID;
                log.Org_ID = ol.Org_ID;
                //学员信息
                log.Ac_ID = stid;
                Accounts student = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == stid).ToFirst<Accounts>();
                if (student != null)
                {
                    log.Ac_Name = student.Ac_Name;
                    log.Ac_AccName = student.Ac_AccName;
                }
                //视频信息
                if (acc.As_IsOther && acc.As_IsOuter)
                {
                    throw new Exception("视频链接为外部视频，无法统计学习进度");
                }
                if (acc.As_Duration <= 0)
                {
                    throw new Exception("视频时间小于等于零，请在课程管理中设置该章节视频的时长");
                }
                log.Lss_Duration = acc.As_Duration * 1000;
                //学习时间
                log.Lss_StudyTime = acc.As_Duration;
                //视频播放进度
                int range = acc.As_Duration / 3 > 0 ? acc.As_Duration / 3 : acc.As_Duration;
                int rd = new Random().Next(0, range);
                int playtime = acc.As_Duration * 7 / 10 + rd;
                playtime = playtime > acc.As_Duration ? acc.As_Duration : playtime;
                log.Lss_PlayTime = playtime * 1000;
                //最后的播放时间
                Student_Course sc = Gateway.Default.From<Student_Course>().Where(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == ol.Cou_ID).ToFirst<Student_Course>();
                if (sc == null)
                {
                    throw new Exception("未查询到当前学员(" + student.Ac_Name + ":" + student.Ac_AccName + ")选修了该课程");
                }
                TimeSpan span = sc.Stc_EndTime - sc.Stc_StartTime;
                log.Lss_LastTime = sc.Stc_StartTime.AddSeconds(new Random().Next(0, (int)span.TotalSeconds));
                log.Lss_Complete = Math.Floor((double)log.Lss_StudyTime * 1000 / (double)log.Lss_Duration * 10000) / 100;
                log.Lss_Complete = log.Lss_Complete > 100 ? 100 : log.Lss_Complete;
                Gateway.Default.Save<LogForStudentStudy>(log);               
            }
        }
        #endregion

        #region 学员的错题回顾
        /// <summary>
        /// 添加添加学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void QuesAdd(Student_Ques entity)
        {
            Task task = new Task(() =>
            {
                _QuesAdd(entity);
            });
            task.Start();
        }
        private void _QuesAdd(Student_Ques entity)
        {
            if (entity == null) return;
            Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (qus == null) return;
            entity.Qus_Type = qus.Qus_Type;
            entity.Cou_ID = qus.Cou_ID;
            entity.Sbj_ID = qus.Sbj_ID;
            entity.Squs_Level = qus.Qus_Diff;
            entity.Qus_Diff = qus.Qus_Diff;
            entity.Squs_CrtTime = DateTime.Now;
            //
            WhereClip wc = Student_Ques._.Qus_ID == entity.Qus_ID && Student_Ques._.Ac_ID == entity.Ac_ID;
            Student_Ques sc = Gateway.Default.From<Student_Ques>().Where(wc).ToFirst<Student_Ques>();
            if (sc != null)
            {
                entity.Squs_ID = sc.Squs_ID;
                entity.Attach();
            }
            Gateway.Default.Save<Student_Ques>(entity);
        }
        /// <summary>
        /// 修改学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void QuesSave(Student_Ques entity)
        {
            Gateway.Default.Save<Student_Ques>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public void QuesDelete(int identify)
        {
            Gateway.Default.Delete<Student_Ques>(Student_Ques._.Squs_ID == identify);
        }
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="acid"></param>
        public void QuesDelete(long quesid, int acid)
        {
            Gateway.Default.Delete<Student_Ques>(Student_Ques._.Qus_ID == quesid && Student_Ques._.Ac_ID == acid);
        }
        /// <summary>
        /// 清空错题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        public int QuesClear(long couid, int stid)
        {
            return Gateway.Default.Delete<Student_Ques>(Student_Ques._.Cou_ID == couid && Student_Ques._.Ac_ID == stid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Student_Ques QuesSingle(int identify)
        {
            return Gateway.Default.From<Student_Ques>().Where(Student_Ques._.Squs_ID == identify).ToFirst<Student_Ques>();
        }
        /// <summary>
        /// 当前学员的所有错题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] QuesAll(int acid, long sbjid, long couid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Ques._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Ques._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Ques._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Ques>(Questions._.Qus_ID == Student_Ques._.Qus_ID)
                .Where(wc).OrderBy(Questions._.Qus_ID.Desc).ToArray<Questions>();
        }
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] QuesCount(int acid, long sbjid, long couid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Ques._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Ques._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Ques._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Ques._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Ques>(Questions._.Qus_ID == Student_Ques._.Qus_ID)
                .Where(wc).OrderBy(Questions._.Qus_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 学员错题的个数
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">专业 id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public int QuesOfCount(int stid, long sbjid, long couid, int type)
        {
            WhereClip wc = new WhereClip();
            if (stid > 0) wc.And(Student_Ques._.Ac_ID == stid);
            if (sbjid > 0) wc.And(Student_Ques._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Ques._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Ques._.Qus_Type == type);
            return Gateway.Default.Count<Student_Ques>(wc);
        }
        /// <summary>
        /// 高频错题
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="type">题型</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Questions[] QuesOftenwrong(long couid, int type, int count)
        {
            string sql = @"select {top} sq.count as Qus_Errornum,c.* from Questions as c inner join 
(SELECT qus_id,COUNT(qus_id) as 'count'  FROM [Student_Ques] where {couid} and {type} group by qus_id) as sq
on c.qus_id=sq.qus_id order by sq.count desc";
            sql = sql.Replace("{couid}", couid > 0 ? "cou_id=" + couid : "1=1");
            sql = sql.Replace("{type}", type > 0 ? "Qus_Type=" + type : "1=1");
            sql = sql.Replace("{top}", count > 0 ? "top " + count : "");
            return Gateway.Default.FromSql(sql).ToArray<Questions>();
        }
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Questions[] QuesPager(int acid, long sbjid, long couid, int type, int diff, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Ques._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Ques._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Ques._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Ques._.Qus_Type == type);
            if (diff > 0) wc.And(Student_Ques._.Qus_Diff == diff);
            countSum = Gateway.Default.Count<Student_Ques>(wc);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Ques>(Questions._.Qus_ID == Student_Ques._.Qus_ID)
                .Where(wc).OrderBy(Questions._.Qus_CrtTime.Desc).ToArray<Questions>(size, (index - 1) * size);
        }
        /// <summary>
        /// 错题所属的课程
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couname">课程名称，可模糊查询</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Course[] QuesForCourse(int stid, string couname, int size, int index, out int countSum)
        {
            //计算满足条件的数量
            string sumSql = @"select COUNT(*) from course as c inner join 
                            (select cou_id from Student_Ques where Ac_ID={stid} group by cou_id) as q
                            on c.cou_id=q.cou_id where {course}";
            sumSql = sumSql.Replace("{stid}", stid.ToString());
            sumSql = sumSql.Replace("{course}", string.IsNullOrWhiteSpace(couname) ? "1=1" : "Cou_Name LIKE '%" + couname + "%'");
            countSum = Convert.ToInt32(Gateway.Default.FromSql(sumSql).ToScalar());
            
            //分页获取数据
            string sql = @"SELECT [TMP_TABLE].* FROM 
	                ( select c.*,q.count,ROW_NUMBER() OVER( ORDER BY q.sid desc ) AS TMP__ROWID  from course as c inner join 
                (select cou_id,max(Squs_ID) as 'sid',COUNT(*) as 'count' from Student_Ques where Ac_ID={stid} group by cou_id) as q
                on c.cou_id=q.cou_id where {course}  ) 
	                [TMP_TABLE] WHERE TMP__ROWID BETWEEN {start} AND {end}";
            sql = sql.Replace("{stid}", stid.ToString());
            sql = sql.Replace("{course}", string.IsNullOrWhiteSpace(couname) ? "1=1" : "Cou_Name LIKE '%" + couname + "%'");
            //RowNum BETWEEN (页码-1)*页大小+1 AND 页码*页大小
            sql = sql.Replace("{start}", ((index - 1) * size + 1).ToString());
            sql = sql.Replace("{end}", (index * size).ToString());           

            return Gateway.Default.FromSql(sql).ToArray<Course>();

        }
        #endregion

        #region 学员的收藏
        /// <summary>
        /// 添加添加学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CollectAdd(Student_Collect entity)
        {
            Task task = new Task(() =>
            {
                _collectAdd(entity);
            });
            task.Start();
        }
        private void _collectAdd(Student_Collect entity)
        {
            if (entity == null) return;
            Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (qus == null) return;
            entity.Qus_Type = qus.Qus_Type;
            entity.Qus_Title = qus.Qus_Title;
            entity.Qus_Diff = qus.Qus_Diff;
            entity.Cou_ID = qus.Cou_ID;
            entity.Sbj_ID = qus.Sbj_ID;
            //
            WhereClip wc = Student_Collect._.Qus_ID == entity.Qus_ID && Student_Collect._.Ac_ID == entity.Ac_ID;
            Student_Collect sc = Gateway.Default.From<Student_Collect>().Where(wc).ToFirst<Student_Collect>();
            if (sc != null)
            {
                entity.Stc_ID = sc.Stc_ID;
                entity.Attach();
            }
            else
            {
                entity.Stc_CrtTime = DateTime.Now;
            }
            Gateway.Default.Save<Student_Collect>(entity);
        }
        /// <summary>
        /// 修改学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CollectSave(Student_Collect entity)
        {
            Gateway.Default.Save<Student_Collect>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public void CollectDelete(int identify)
        {
            Gateway.Default.Delete<Student_Collect>(Student_Collect._.Stc_ID == identify);
        }
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="acid"></param>
        public void CollectDelete(long quesid, int acid)
        {
            Gateway.Default.Delete<Student_Collect>(Student_Collect._.Qus_ID == quesid && Student_Collect._.Ac_ID == acid);
        }
        /// <summary>
        /// 清空试题收藏
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        public void CollectClear(long couid, int stid)
        {
            Gateway.Default.Delete<Student_Collect>(Student_Collect._.Cou_ID == couid && Student_Collect._.Ac_ID == stid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Student_Collect CollectSingle(int identify)
        {
            return Gateway.Default.From<Student_Collect>().Where(Student_Collect._.Stc_ID == identify).ToFirst<Student_Collect>();
        }
        public Student_Collect CollectSingle(int acid, long qid)
        {
            return Gateway.Default.From<Student_Collect>().Where(Student_Collect._.Ac_ID == acid && Student_Collect._.Qus_ID == qid).ToFirst<Student_Collect>();
        }
        /// <summary>
        /// 当前学员的所有错题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] CollectAll4Ques(int acid, long sbjid, long couid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Collect>(Questions._.Qus_ID == Student_Collect._.Qus_ID)
                .Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Questions>();
        }
        public Student_Collect[] CollectAll(int acid, long sbjid, long couid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            return Gateway.Default.From<Student_Collect>().Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Student_Collect>();
        }
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] CollectCount(int acid, long sbjid, long couid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Collect>(Questions._.Qus_ID == Student_Collect._.Qus_ID)
                .Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难易度</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Questions[] CollectPager(int acid, long sbjid, long couid, int type, int diff, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Collect._.Ac_ID == acid);
            if (sbjid > 0) wc.And(Student_Collect._.Sbj_ID == sbjid);
            if (type > 0) wc.And(Student_Collect._.Qus_Type == type);
            if (couid > 0) wc.And(Student_Collect._.Cou_ID == couid);
            if (diff > 0) wc.And(Student_Collect._.Qus_Diff == diff);
            countSum = Gateway.Default.Count<Student_Collect>(wc);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Collect>(Questions._.Qus_ID == Student_Collect._.Qus_ID)
                .Where(wc).OrderBy(Student_Collect._.Stc_CrtTime.Desc).ToArray<Questions>(size, (index - 1) * size);
        }
        #endregion

        #region 学员的笔记
        /// <summary>
        /// 添加添加学员的笔记
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void NotesAdd(Student_Notes entity)
        {
            new Task(() =>
            {
                _notesAdd(entity);
            }).Start();
        }
        private void _notesAdd(Student_Notes entity)
        {
            if (entity == null) return;
            Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (qus == null) return;
            entity.Qus_Type = qus.Qus_Type;
            entity.Qus_Title = qus.Qus_Title;
            entity.Cou_ID = qus.Cou_ID;
            //
            WhereClip wc = Student_Notes._.Qus_ID == entity.Qus_ID && Student_Notes._.Ac_ID == entity.Ac_ID;
            Student_Notes sn = Gateway.Default.From<Student_Notes>().Where(wc).ToFirst<Student_Notes>();
            if (sn != null)
            {
                entity.Stn_Title = entity.Stn_Title;
                entity.Stn_Context = sn.Stn_Context + "\n" + entity.Stn_Context;
                entity.Stn_ID = sn.Stn_ID;
                entity.Attach();
            }
            else
            {
                entity.Stn_CrtTime = DateTime.Now;
                //entity.Stn_Context = "------ " + DateTime.Now.ToString() + " ------\n" + entity.Stn_Context;
            }
            Gateway.Default.Save<Student_Notes>(entity);
        }
        /// <summary>
        /// 修改学员的笔记
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void NotesSave(Student_Notes entity)
        {
            Gateway.Default.Save<Student_Notes>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public void NotesDelete(int identify)
        {
            Gateway.Default.Delete<Student_Notes>(Student_Notes._.Stn_ID == identify);
        }
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="acid"></param>
        public void NotesDelete(long quesid, int acid)
        {
            Gateway.Default.Delete<Student_Notes>(Student_Notes._.Qus_ID == quesid && Student_Notes._.Ac_ID == acid);
        }
        /// <summary>
        /// 清空试题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        public void NotesClear(long couid, int stid)
        {
            Gateway.Default.Delete<Student_Notes>(Student_Notes._.Cou_ID == couid && Student_Notes._.Ac_ID == stid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Student_Notes NotesSingle(int identify)
        {
            return Gateway.Default.From<Student_Notes>().Where(Student_Notes._.Stn_ID == identify).ToFirst<Student_Notes>();
        }
        /// <summary>
        /// 获取单一实体对象，按试题id、学员id
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <param name="acid">学员id</param>
        /// <returns></returns>
        public Student_Notes NotesSingle(long quesid, int acid)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);
            if (quesid > 0) wc.And(Student_Notes._.Qus_ID == quesid);
            return Gateway.Default.From<Student_Notes>().Where(wc).ToFirst<Student_Notes>();
        }
        /// <summary>
        /// 当前学员的所有错题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Student_Notes[] NotesAll(int acid, int type)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);
            if (type > 0) wc.And(Student_Notes._.Qus_Type == type);
            return Gateway.Default.From<Student_Notes>()
                .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Student_Notes>();
        }
        /// <summary>
        /// 取当前学员的笔记
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Questions[] NotesCount(int stid, long couid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (stid > 0) wc.And(Student_Notes._.Ac_ID == stid);          
            if (couid > 0) wc.And(Student_Notes._.Cou_ID == couid);
            if (type > 0) wc.And(Student_Notes._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Notes>(Questions._.Qus_ID == Student_Notes._.Qus_ID)
                .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="quesid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Questions[] NotesCount(int acid, int type, int count)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);           
            if (type > 0) wc.And(Student_Notes._.Qus_Type == type);
            return Gateway.Default.From<Questions>()
                .InnerJoin<Student_Notes>(Questions._.Qus_ID == Student_Notes._.Qus_ID)
                .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="quesid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难易度</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Student_Notes[] NotesPager(int acid, long quesid, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (acid > 0) wc.And(Student_Notes._.Ac_ID == acid);
            if (quesid > 0) wc.And(Student_Notes._.Qus_ID == quesid);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Student_Notes._.Stn_Context.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Student_Notes>(wc);
            return Gateway.Default.From<Student_Notes>()
               .Where(wc).OrderBy(Student_Notes._.Stn_CrtTime.Desc).ToArray<Student_Notes>(size, (index - 1) * size);
        }
        #endregion
    }
}
