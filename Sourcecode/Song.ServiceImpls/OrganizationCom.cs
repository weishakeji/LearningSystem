using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.IO;
using System.Collections;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using Quartz;
using Quartz.Impl;

namespace Song.ServiceImpls
{
    public class OrganizationCom : IOrganization
    {
        #region 机构管理
        public void OrganAdd(Organization entity)
        {
            entity.Org_RegTime = DateTime.Now;
            entity.Org_IsRoot = false;
            entity.Org_IsPass = false;
            if (!string.IsNullOrWhiteSpace(entity.Org_Logo) && entity.Org_Logo.IndexOf("/") > -1)
            {
                entity.Org_Logo = entity.Org_Logo.Substring(entity.Org_Logo.LastIndexOf("/") + 1);
            }
            //所属机构等级
            Song.Entities.OrganLevel lv;
            if (entity.Olv_ID > 0)
            {
                lv = Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_ID == entity.Olv_ID).ToFirst<OrganLevel>();
                entity.Olv_Name = lv.Olv_Name;
            }
            else
            {
                lv = Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_IsDefault == true).ToFirst<OrganLevel>();
                entity.Olv_ID = lv.Olv_ID;
                entity.Olv_Name = lv.Olv_Name;
            }
            //解析地址
            WeiSha.Core.Param.Method.Position p = WeiSha.Core.Request.Position(entity.Org_Longitude, entity.Org_Latitude);
            entity.Org_Province = p.Province;
            entity.Org_City = p.City;
            entity.Org_District = p.District;
            entity.Org_Street = p.Street;
            //
            Gateway.Default.Save<Organization>(entity);
            //创建管理岗位
            Song.Entities.Position posi =new  Song.Entities.Position();
            posi.Posi_IsAdmin = true;
            posi.Posi_IsUse = true;
            posi.Posi_Name = "机构管理";
            posi.Org_ID = entity.Org_ID;
            Business.Do<IPosition>().Add(posi);

            this.OrganBuildCache();  //重新构建缓存
        }

        /// <summary>
        /// 修改机构信息
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void OrganSave(Organization entity)
        {
            if (entity.Org_ID < 1) return;
            //机构标志图片
            if (!string.IsNullOrWhiteSpace(entity.Org_Logo) && entity.Org_Logo.IndexOf("/") > -1)
            {
                entity.Org_Logo = entity.Org_Logo.Substring(entity.Org_Logo.LastIndexOf("/") + 1);
            }
            //解析地址
            WeiSha.Core.Param.Method.Position posi = WeiSha.Core.Request.Position(entity.Org_Longitude, entity.Org_Latitude);
            entity.Org_Province = posi.Province;
            entity.Org_City = posi.City;
            entity.Org_District = posi.District;
            entity.Org_Street = posi.Street;
           
            //机构等级
            Song.Entities.OrganLevel level= Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_ID == entity.Olv_ID).ToFirst<OrganLevel>();
            entity.Olv_Name = level == null ? "" : level.Olv_Name;

            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Organization>(entity);
                    ////员工所属机构名称
                    ////tran.Update<EmpAccount>(new Field[] { EmpAccount._.Org_Name }, new object[] { entity.Org_Name }, EmpAccount._.Org_ID == entity.Org_ID);
                    ////tran.Update<Depart>(new Field[] { Depart._.Org_Name }, new object[] { entity.Org_Name }, Depart._.Org_ID == entity.Org_ID);
                    ////专业、课程、试卷，考试
                    //tran.Update<Subject>(new Field[] { Subject._.Org_Name }, new object[] { entity.Org_Name }, Subject._.Org_ID == entity.Org_ID);
                    //tran.Update<Course>(new Field[] { Course._.Org_Name }, new object[] { entity.Org_Name }, Course._.Org_ID == entity.Org_ID);
                    //tran.Update<TestPaper>(new Field[] { TestPaper._.Org_Name }, new object[] { entity.Org_Name }, TestPaper._.Org_ID == entity.Org_ID);
                    //tran.Update<Examination>(new Field[] { Examination._.Org_Name }, new object[] { entity.Org_Name }, Examination._.Org_ID == entity.Org_ID);
                    ////学员与教师                  
                    //tran.Update<StudentSort>(new Field[] { StudentSort._.Org_Name }, new object[] { entity.Org_Name }, StudentSort._.Org_ID == entity.Org_ID);
                    //tran.Update<Teacher>(new Field[] { Teacher._.Org_Name }, new object[] { entity.Org_Name }, Teacher._.Org_ID == entity.Org_ID);
                    //tran.Update<TeacherSort>(new Field[] { TeacherSort._.Org_Name }, new object[] { entity.Org_Name }, TeacherSort._.Org_ID == entity.Org_ID);
                    tran.Commit();
                    this.OrganBuildCache();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            this.OrganBuildCache();  //重新构建缓存    
        }
        public void OrganSetDefault(int identify)
        {
            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Update<Organization>(new Field[] { Organization._.Org_IsDefault },
                        new object[] { true }, Organization._.Org_ID == identify);
                    trans.Update<Organization>(new Field[] { Organization._.Org_IsDefault },
                        new object[] { false }, Organization._.Org_ID != identify);
                    trans.Commit();
                    this.OrganBuildCache();
                    //清除缓存
                    IDictionaryEnumerator cache = HttpRuntime.Cache.GetEnumerator();
                    while (cache.MoveNext())
                        HttpRuntime.Cache.Remove(cache.Key.ToString());
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Close();
                }
            }
            this.OrganBuildCache();  //重新构建缓存
        }
        /// <summary>
        /// 系统默认采用的机构（注：不是Root机构)
        /// </summary>
        /// <returns></returns>
        public Organization OrganDefault()
        {           
            //当前机构
            Organization curr = null;
            //从缓存中读取
            List<Organization> list = Cache.EntitiesCache.GetList<Organization>();
            if (list == null || list.Count < 1) list = this.OrganBuildCache();
            List<Organization> tm = (from l in list
                                     where l.Org_IsUse == true && l.Org_IsPass == true && l.Org_IsShow == true && l.Org_IsDefault == true
                                     select l).ToList<Organization>();
            if (tm.Count > 0) curr = tm[0];
            //如果缓存中没有，则读取数据库
            if (curr == null)
            {
                WhereClip wc = new WhereClip();
                wc &= Organization._.Org_IsUse == true;
                wc &= Organization._.Org_IsPass == true;
                wc &= Organization._.Org_IsShow == true;
                curr = Gateway.Default.From<Organization>().Where(wc && Organization._.Org_IsDefault == true).ToFirst<Organization>();
            }
            return curr == null ? this.OrganRoot() : curr;
        }
        /// <summary>
        /// 用于系统管理的机构（注：即Root机构)
        /// </summary>
        /// <returns></returns>
        public Organization OrganRoot()
        {
            //当前机构
            Organization curr = null;
            //从缓存中读取
            List<Organization> list = Cache.EntitiesCache.GetList<Organization>();
            if (list == null || list.Count < 1) list = this.OrganBuildCache();
            List<Organization> tm = (from l in list
                                     where l.Org_IsRoot == true
                                     select l).ToList<Organization>();
            if (tm.Count > 0) curr = tm[0];
             //如果缓存中没有，则读取数据库
            if (curr == null)
            {
                curr = Gateway.Default.From<Organization>().Where(Organization._.Org_IsRoot == true).ToFirst<Organization>();
                if (curr == null) curr = Gateway.Default.From<Organization>().ToFirst<Organization>();               
            }
            return curr;
        }
        /// <summary>
        /// 当前机构
        /// </summary>
        /// <returns></returns>
        public Organization OrganCurrent()
        {
            //当前机构
            Organization curr = null;
            WhereClip wc = new WhereClip();
            wc &= Organization._.Org_IsUse == true;
            wc &= Organization._.Org_IsPass == true;
            wc &= Organization._.Org_IsShow == true;
            List<Organization> list = Cache.EntitiesCache.GetList<Organization>();
            if (list == null || list.Count < 1) list = this.OrganBuildCache();
            //是否启用多机构，0为多机构，1为单机构
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            if (multi == 1)
            {

                List<Organization> tm = (from l in list
                                         where l.Org_IsUse == true && l.Org_IsPass == true && l.Org_IsShow == true && l.Org_IsDefault == true
                                         select l).ToList<Organization>();
                if (tm.Count > 0) curr = tm[0];
                //如果缓存中没有，则读取数据库
                if (curr == null) curr = Gateway.Default.From<Organization>().Where(wc && Organization._.Org_IsDefault == true).ToFirst<Organization>();
            }
            else
            {
                //当前机构的二级域名
                string twoDomain = WeiSha.Core.Request.Domain.TwoDomain;
                List<Organization> tm = (from l in list
                                         where l.Org_IsUse == true && l.Org_IsPass == true && l.Org_IsShow == true && l.Org_TwoDomain == twoDomain
                                         select l).ToList<Organization>();
                if (tm.Count > 0) curr = tm[0];
                //如果缓存中没有，则读取数据库
                if (curr == null) curr = Gateway.Default.From<Organization>().Where(wc && Organization._.Org_TwoDomain == twoDomain).ToFirst<Organization>();
                if (curr == null) curr = this.OrganDefault();
            }
            return curr;
        }  
        public Organization OrganSingle(int identify)
        {
            //当前机构
            Organization curr = null;
            //从缓存中读取
            List<Organization> list = Cache.EntitiesCache.GetList<Organization>();
            if (list == null || list.Count < 1) list = this.OrganBuildCache();
            List<Organization> tm = (from l in list
                                     where l.Org_ID == identify
                                     select l).ToList<Organization>();
            if (tm.Count > 0) curr = tm[0];
            if (curr == null) curr = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == identify).ToFirst<Organization>();
            return curr;
        }
        /// <summary>
        /// 当前机构是否重名
        /// </summary>
        /// <param name="name">机构的名称</param>
        /// <param name="id">机构的id</param>   
        /// <returns></returns>
        public bool ExistName(string name, int id)
        {
            Organization mm = Gateway.Default.From<Organization>()
                  .Where(Organization._.Org_ID != id && Organization._.Org_Name == name)
                  .ToFirst<Organization>();
            return mm != null;
        }
        /// <summary>
        /// 机构平台名称是否重复
        /// </summary>
        /// <param name="name">机构的平台名称</param>
        /// <param name="id">机构的id</param>
        /// <returns></returns>
        public bool ExistPlatform(string name, int id)
        {
            Organization mm = Gateway.Default.From<Organization>()
                  .Where(Organization._.Org_ID != id && Organization._.Org_PlatformName == name)
                  .ToFirst<Organization>();
            return mm != null;
        }
        /// <summary>
        /// 机构的二级域否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id">机构的id</param>
        /// <returns></returns>
        public bool ExistDomain(string name, int id)
        {
            Organization mm = Gateway.Default.From<Organization>()
                  .Where(Organization._.Org_ID != id && Organization._.Org_TwoDomain == name)
                  .ToFirst<Organization>();
            return mm != null;
        }
        /// <summary>
        /// 删除机构，按id
        /// </summary>
        /// <param name="identify"></param>
        public void OrganDelete(int identify)
        {
            Organization org = this.OrganSingle(identify);
            if (org.Org_IsRoot) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Organization>(Organization._.Org_ID == identify);
                    //删除机构下属组织
                    tran.Delete<Depart>(Depart._.Org_ID == identify);               
                    tran.Delete<Position>(Position._.Org_ID == identify);
                    tran.Delete<EmpAccount>(EmpAccount._.Org_ID == identify);
                    tran.Delete<EmpGroup>(EmpGroup._.Org_ID == identify);
                    tran.Delete<EmpTitle>(EmpTitle._.Org_ID == identify);
                    //删除专业
                    tran.Delete<Subject>(Subject._.Org_ID == identify);
                    //删除教师与学生
                    tran.Delete<Teacher>(Teacher._.Org_ID == identify);
                    //tran.Delete<TeacherHistory>(TeacherHistory._.Org_ID == identify);
                    tran.Delete<TeacherSort>(TeacherSort._.Org_ID == identify);
                    tran.Delete<Accounts>(Accounts._.Org_ID == identify);
                    tran.Delete<StudentSort>(StudentSort._.Org_ID == identify);
                    //删除课程
                    tran.Delete<Course>(Course._.Org_ID == identify);
                    tran.Delete<Student_Course>(Student_Course._.Org_ID == identify);
                    //删除试题、试卷、考试、成绩
                    tran.Delete<Questions>(Questions._.Org_ID == identify);
                    //tran.Delete<Student_Ques>(Student_Ques._.Org_ID == identify);
                    tran.Delete<TestPaper>(TestPaper._.Org_ID == identify);
                    tran.Delete<Examination>(Examination._.Org_ID == identify);
                    //tran.Delete<ExamResults>(ExamResults._.Org_ID == identify);
                    //删除知识库
                    tran.Delete<KnowledgeSort>(KnowledgeSort._.Org_ID == identify);
                    tran.Delete<Knowledge>(Knowledge._.Org_ID == identify);
                    //删除新闻、通知
                    tran.Delete<Columns>(Columns._.Org_ID == identify);
                    tran.Delete<Article>(Article._.Org_ID == identify);
                    tran.Delete<Notice>(Notice._.Org_ID == identify);
                    tran.Commit();
                }
                catch (Exception ex) { tran.Rollback(); throw ex; }
            }
            this.OrganBuildCache();  //重新构建缓存
        }       
        public List<Organization> OrganAll(bool? isUse, int level, string search)
        {
            List<Organization> list = Cache.EntitiesCache.GetList<Organization>();
            if (list == null || list.Count < 1) list = this.OrganBuildCache();
            //自定义查询条件
            Func<Organization, bool> exp = x =>
            {
                var use_exp = isUse != null ? x.Org_IsUse = (bool)isUse : true;
                var level_exp = level > 0 ? x.Olv_ID == level : true;
                var search_exp = !string.IsNullOrWhiteSpace(search) ? x.Org_Name.Contains(search) : true;
                return use_exp && level_exp && search_exp;
            };
            List<Organization> result = list.Where(exp).ToList<Organization>();
            return result;
            ////orm查询
            //WhereClip wc = new WhereClip();
            //if (isUse != null) wc.And(Organization._.Org_IsUse == (bool)isUse);
            //if (level > 0) wc.And(Organization._.Olv_ID == level);
            //if (!string.IsNullOrWhiteSpace(search)) wc.And(Organization._.Org_Name.Contains(search));
            //return Gateway.Default.From<Organization>().Where(wc)
            //    .OrderBy(Organization._.Org_RegTime.Desc).ToArray<Organization>();
        }

        public List<Organization> OrganCount(bool? isUse, bool? isShow, int level, int count)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(Organization._.Org_IsUse == (bool)isUse);
            if (isShow != null) wc.And(Organization._.Org_IsShow == (bool)isShow);
            if (level > 0) wc.And(Organization._.Olv_ID == level);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Organization>().Where(wc)
                .OrderBy(Organization._.Org_RegTime.Desc).ToList<Organization>(count);
        }
        /// <summary>
        /// 清理缓存文件
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="day">清理多少天之前的</param>
        public void OrganClearTemp(int orgid, int day)
        {
            string tmpPath = WeiSha.Core.Upload.Get["Temp"].Physics;
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(tmpPath);
            foreach (System.IO.FileInfo fi in di.GetFiles())
            {
                if (fi.CreationTime < DateTime.Now.AddDays(-day)) fi.Delete();
            }
        }
        /// <summary>
        /// 清理当前机构的数据
        /// </summary>
        /// <param name="orgid"></param>
        public void OrganClear(int identify)
        {
            Organization org = this.OrganSingle(identify);
            if (org.Org_IsRoot) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //删除教师与学生
                    tran.Delete<Teacher>(Teacher._.Org_ID == identify);
                    //tran.Delete<TeacherHistory>(TeacherHistory._.Org_ID == identify);
                    tran.Delete<TeacherSort>(TeacherSort._.Org_ID == identify);
                    tran.Delete<Accounts>(Accounts._.Org_ID == identify);
                    tran.Delete<StudentSort>(StudentSort._.Org_ID == identify);
                    //删除课程
                    tran.Delete<Course>(Course._.Org_ID == identify);
                    tran.Delete<Student_Course>(Student_Course._.Org_ID == identify);
                    //删除试题、试卷、考试、成绩
                    tran.Delete<Questions>(Questions._.Org_ID == identify);
                    //tran.Delete<Student_Ques>(Student_Ques._.Org_ID == identify);
                    tran.Delete<TestPaper>(TestPaper._.Org_ID == identify);
                    tran.Delete<Examination>(Examination._.Org_ID == identify);
                    //tran.Delete<ExamResults>(ExamResults._.Org_ID == identify);
                    //删除知识库
                    tran.Delete<KnowledgeSort>(KnowledgeSort._.Org_ID == identify);
                    tran.Delete<Knowledge>(Knowledge._.Org_ID == identify);
                    //删除新闻、通知
                    tran.Delete<Columns>(Columns._.Org_ID == identify);
                    tran.Delete<Article>(Article._.Org_ID == identify);
                    tran.Delete<Notice>(Notice._.Org_ID == identify);
                    tran.Commit();
                }
                catch (Exception ex) { tran.Rollback(); throw ex; }
            }
        }
        private static readonly object lock_cache_build = new object();
        /// <summary>
        /// 构建缓存
        /// </summary>
        public List<Organization> OrganBuildCache()
        {
            lock (lock_cache_build)
            {
                Cache.EntitiesCache.Clear<Organization>();
                List<Organization> orgs = Gateway.Default.From<Organization>()
                    .OrderBy(Organization._.Org_ID.Desc).ToList<Organization>();
                Cache.EntitiesCache.Save<Organization>(orgs);
                //刷新模板相关配置
                WeiSha.Core.Business.Do<ITemplate>().SetPlateOrganInfo();
                return Cache.EntitiesCache.GetList<Organization>();
            }
        }
        public List<Organization> OrganPager(bool? isUse, int level, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(Organization._.Org_IsUse == (bool)isUse);
            if (level > 0) wc.And(Organization._.Olv_ID == level);
            if (!string.IsNullOrWhiteSpace(searTxt))
                wc.And(Organization._.Org_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<Organization>(wc);
            return Gateway.Default.From<Organization>().Where(wc)
                .OrderBy(Organization._.Org_RegTime.Desc)
                .ToList<Organization>(size, (index - 1) * size);
        }
        #endregion

        #region 机构等级管理
        /// <summary>
        /// 添加机构等级
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void LevelAdd(OrganLevel entity)
        {
            bool nameexist = this.LevelNameExist(entity.Olv_Name, entity.Olv_ID);
            if (nameexist) throw new Exception("名称已经存在");
            bool tageexist = this.LevelTagExist(entity.Olv_Name, entity.Olv_ID);
            if (tageexist) throw new Exception("标识已经存在");

            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<OrganLevel>(OrganLevel._.Olv_Tax, OrganLevel._.Olv_Tax > -1);
            int tax = obj is int ? (int)obj : 0;
            entity.Olv_Tax = tax + 1;
            //判断是否默认
            obj = Gateway.Default.Count<OrganLevel>(OrganLevel._.Olv_IsDefault==true);
            bool isDef = (obj is int ? (int)obj : 0) > 0;
            if (!isDef) entity.Olv_IsDefault = true;
            Gateway.Default.Save<OrganLevel>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void LevelSave(OrganLevel entity)
        {
            bool nameexist = this.LevelNameExist(entity.Olv_Name, entity.Olv_ID);
            if (nameexist) throw new Exception("名称已经存在");
            bool tageexist = this.LevelTagExist(entity.Olv_Name, entity.Olv_ID);
            if (tageexist) throw new Exception("标识已经存在");

            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Save<OrganLevel>(entity);
                    trans.Update<Organization>(new Field[] { Organization._.Olv_Name },
                        new object[] { entity.Olv_Name },
                        Organization._.Olv_ID == entity.Olv_ID);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Close();
                }
            }          
        }
        public void LevelSetDefault(int identify)
        {
            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Update<OrganLevel>(new Field[] { OrganLevel._.Olv_IsDefault },
                        new object[] { true }, OrganLevel._.Olv_ID == identify);
                    trans.Update<OrganLevel>(new Field[] { OrganLevel._.Olv_IsDefault },
                        new object[] { false }, OrganLevel._.Olv_ID != identify);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Close();
                }
            }
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        public OrganLevel LevelSingle(int identify)
        {
            return Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_ID == identify).ToFirst<OrganLevel>();
        }
        /// <summary>
        /// 默认的机构等级
        /// </summary>
        /// <returns></returns>
        public OrganLevel LevelDefault()
        {
            return Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_IsDefault == true).ToFirst<OrganLevel>();
        }
        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        public OrganLevel[] LevelAll(string search, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(OrganLevel._.Olv_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(search)) wc.And(OrganLevel._.Olv_Name.Contains(search));
            return Gateway.Default.From<OrganLevel>().Where(wc).OrderBy(OrganLevel._.Olv_Tax.Asc).ToArray<OrganLevel>();

        }
        /// <summary>
        /// 当前机构等级下，有几个机构
        /// </summary>
        /// <param name="lvid">机构等级id</param>
        /// <returns></returns>
        public int LevelOrganCount(int lvid)
        {
            return Gateway.Default.Count<Organization>(Organization._.Olv_ID == lvid);
        }
        /// <summary>
        /// 根据主键删除机构等级。
        /// </summary>
        /// <param name="identify">主键id</param>
        public bool LevelDelete(int identify)
        {
            int count = this.LevelOrganCount(identify);
            if (count > 0) throw new Exception("请删除等级下的机构");
          
            Gateway.Default.Delete<OrganLevel>(OrganLevel._.Olv_ID == identify);
            //设置默认机构等级
            int defcount = Gateway.Default.Count<OrganLevel>(OrganLevel._.Olv_IsDefault == true);
            if (defcount < 1)
            {
                OrganLevel def = Gateway.Default.From<OrganLevel>().OrderBy(OrganLevel._.Olv_Level.Asc).ToFirst<OrganLevel>();
                if (def != null)
                {
                    Gateway.Default.Update<OrganLevel>(new Field[] { OrganLevel._.Olv_IsDefault }, new object[] { true },
                        OrganLevel._.Olv_ID == def.Olv_ID);                    
                }
            }
            return true;
        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="name">机构等级的名称</param>
        /// <param name="id">机构等级的id</param>   
        /// <returns></returns>
        public bool LevelNameExist(string name, int id)
        {
            WhereClip wc = new WhereClip();         
            if (id > 0) wc.And(OrganLevel._.Olv_ID != id);
            //如果是一个已经存在的对象，则不匹配自己
            OrganLevel mm = Gateway.Default.From<OrganLevel>()
                   .Where(wc && OrganLevel._.Olv_Name == name)
                   .ToFirst<OrganLevel>();
            return mm != null;
        }
        /// <summary>
        /// 机构等级的tag标识是否重名
        /// </summary>
        /// <param name="tag">机构等级的tag标识</param>
        /// <param name="id">机构等级的id</param>   
        /// <returns></returns>
        public bool LevelTagExist(string tag, int id)
        {
            WhereClip wc = new WhereClip();
            if (id > 0) wc.And(OrganLevel._.Olv_ID != id);
            //如果是一个已经存在的对象，则不匹配自己
            OrganLevel mm = Gateway.Default.From<OrganLevel>()
                   .Where(wc && OrganLevel._.Olv_Tag == tag)
                   .ToFirst<OrganLevel>();
            return mm != null;
        }
        /// <summary>
        /// 更改机构等级的排序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool LevelUpdateTaxis(OrganLevel[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (OrganLevel item in items)
                    {
                        tran.Update<OrganLevel>(
                            new Field[] { OrganLevel._.Olv_Tax },
                            new object[] { item.Olv_Tax },
                            OrganLevel._.Olv_ID == item.Olv_ID);
                    }
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }                
            }
        }
        #endregion

        #region 统计数据
        /// <summary>
        /// 有多少课程被选修过
        /// </summary>
        /// <param name="orgid">机构id</param>    
        /// <param name="isfree">是否包括免费的</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int CourseCountBuy(int orgid, bool? isfree, DateTime? start, DateTime? end)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Student_Course._.Org_ID == orgid;
            if (isfree != null) wc &= Student_Course._.Stc_IsFree == (bool)isfree;
            if (start != null) wc &= Student_Course._.Stc_StartTime >= (DateTime)start;
            if (end != null) wc &= Student_Course._.Stc_StartTime < (DateTime)end;

            object o = Gateway.Default.From<Student_Course>().Where(wc).SubQuery("c")
            .Select(Student_Course._.Cou_ID.At("c"))
            .GroupBy(Student_Course._.Cou_ID.At("c").Group)
            .Count();          
          
            return o==null ? 0 : Convert.ToInt32(o);
        }
    
        /// <summary>
        /// 有多少学员进行过学习
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isfree">是否包括免费的</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int StudentCountBuy(int orgid, bool? isfree, DateTime? start, DateTime? end)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Student_Course._.Org_ID == orgid;
            if (isfree != null) wc &= Student_Course._.Stc_IsFree == (bool)isfree;
            if (start != null) wc &= Student_Course._.Stc_StartTime >= (DateTime)start;
            if (end != null) wc &= Student_Course._.Stc_StartTime < (DateTime)end;

            object o = Gateway.Default.From<Student_Course>().Where(wc).SubQuery("c")
            .Select(Student_Course._.Ac_ID.At("c"))
            .GroupBy(Student_Course._.Ac_ID.At("c").Group)
            .Count();

            return o == null ? 0 : Convert.ToInt32(o);
        }
        /// <summary>
        /// 学习人次，即学员学习课程的次数
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isfree">是否包括免费的</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int CourseSumBuy(int orgid, bool? isfree, DateTime? start, DateTime? end)
        {

            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Student_Course._.Org_ID == orgid;
            if (isfree != null) wc &= Student_Course._.Stc_IsFree == (bool)isfree;
            if (start != null) wc &= Student_Course._.Stc_StartTime >= (DateTime)start;
            if (end != null) wc &= Student_Course._.Stc_StartTime < (DateTime)end;

            object o = Gateway.Default.From<Student_Course>().Where(wc).Count();

            return o == null ? 0 : Convert.ToInt32(o);
        }

        #endregion

        #region 定时统计数据

        /// <summary>
        /// 创建统计数据的定时任务
        /// </summary>
        public void UpdateStatisticalData_CronJob()
        {
            // 创建调度器  
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            // 启动调度器  
            scheduler.Start().Wait();

            // 定义任务  
            IJobDetail job = JobBuilder.Create<MyJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            // 定义触发器，设置为每天中午12:30执行  
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithCronSchedule("0 30 12 * * ?") // 每天中午12:30  
                .Build();

            // 将任务和触发器添加到调度器  
            scheduler.ScheduleJob(job, trigger).Wait();

            // 关闭调度器  
            scheduler.Shutdown().Wait();
        }
        /// <summary>
        /// 更新机构的统计数据
        /// </summary>
        public void UpdateStatisticalData()
        {
            if (!Gateway.Default.IsCorrect) return;
            WeiSha.Core.Log.Info(this.GetType().FullName, "更新机构的统计数据");

            List<Organization> orgs = this.OrganCount(null, null, -1, 0);
            foreach (Organization org in orgs)
            {
                //统计机构的课程数
                int org_count = Business.Do<IQuestions>().QuesOfCount(org.Org_ID, -1, -1, -1, 0, -1, null);
                Gateway.Default.Update<Organization>(new Field[] { Organization._.Org_QuesCount }, new object[] { org_count }, Organization._.Org_ID == org.Org_ID);
            }
            _update_Subject_StatisticalData();
            _update_Course_StatisticalData();
        }
        #region 专业的统计数据
        /// <summary>
        /// 更新专业的数据，包括试题数，试卷数，课程数
        /// </summary>
        private static void _update_Subject_StatisticalData()
        {
            List<Subject> list = Business.Do<ISubject>().SubjectCount(-1, null, null, 0, 0);
            List<Task> tasks = new List<Task>();
            foreach (Subject item in list)
            {
                tasks.Add(Task.Run(() => _update_Subject_StatisticalData_task(item)));
            }
            // 逐个等待任务完成
            foreach (Task task in tasks)
            {
                task.Wait();
            }
        }
        /// <summary>
        /// 更新专业的数据，包括试题数，试卷数，课程数
        /// </summary>
        /// <param name="subject">专业</param>
        private static void _update_Subject_StatisticalData_task(Subject subject)
        {
            int ques_count = Business.Do<IQuestions>().QuesOfCount(-1, subject.Sbj_ID, -1, -1, 0, -1, null);
            int paper_count = Business.Do<ITestPaper>().PaperOfCount(-1, subject.Sbj_ID, -1, -1, null);
            int course_count = Business.Do<ICourse>().CourseOfCount(subject.Sbj_ID);
            Business.Do<ISubject>().SubjectUpdate(subject.Sbj_ID,
                new Field[] { Subject._.Sbj_QuesCount, Subject._.Sbj_TestCount, Subject._.Sbj_CourseCount },
                new object[] { ques_count, paper_count, course_count });
        }
        #endregion

        #region 课程的统计数据
        private static void _update_Course_StatisticalData()
        {
            List<Course> list = Business.Do<ICourse>().CourseCount(-1, -1, -1, -1, null, null, 0);
            List<Task> tasks = new List<Task>();
            foreach (Course item in list)
            {
                tasks.Add(Task.Run(() => _update_Course_StatisticalData_task(item)));
            }
            // 逐个等待任务完成
            foreach (Task task in tasks)
            {
                task.Wait();
            }
        }
        private static void _update_Course_StatisticalData_task(Course course)
        {
            //更新课程的试卷数，章节数，视频数
            int paper_count = Business.Do<ITestPaper>().PaperOfCount(-1, -1, course.Cou_ID, -1, null);
            int outline_count = Business.Do<IOutline>().OutlineOfCount(course.Cou_ID, -1, null);
            int video_count = Business.Do<IOutline>().OutlineOfCount(course.Cou_ID, -1, null, true, null, null);
            Business.Do<ICourse>().CourseUpdate(course.Cou_ID,
                new Field[] { Course._.Cou_TestCount, Course._.Cou_OutlineCount, Course._.Cou_VideoCount },
                new object[] { paper_count, outline_count, video_count });
            //更新课程与章节下的试题数量
            Business.Do<IQuestions>().QuesCountUpdate(-1, -1, course.Cou_ID, -1);
        }
        #endregion
        #endregion

    }
    // 定义任务类  
    public class MyJob : IJob
    {
        Task IJob.Execute(IJobExecutionContext context)
        {
            WeiSha.Core.Log.Info("定时任务", "更新机构的统计数据");
            Task task = new Task(() =>
            {
                //更新统计数据
                WeiSha.Core.Business.Do<IOrganization>().UpdateStatisticalData();
            });
            return task;
        }
    }
}
