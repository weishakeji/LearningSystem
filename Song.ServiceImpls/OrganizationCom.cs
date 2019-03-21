using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.IO;

namespace Song.ServiceImpls
{
    public class OrganizationCom : IOrganization
    {

        public void OrganAdd(Organization entity)
        {
            entity.Org_RegTime = DateTime.Now;
            entity.Org_IsRoot = false;
            entity.Org_IsPass = false;
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
            WeiSha.Common.Param.Method.Position posi = WeiSha.Common.Request.Position(entity.Org_Longitude, entity.Org_Latitude);
            entity.Org_Province = posi.Province;
            entity.Org_City = posi.City;
            entity.Org_District = posi.District;
            entity.Org_Street = posi.Street;
            //
            Gateway.Default.Save<Organization>(entity);
            this.OrganBuildCache();  //重新构建缓存
        }

        /// <summary>
        /// 修改机构信息
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void OrganSave(Organization entity)
        {
            if (entity.Org_ID < 1) return;
            //解析地址
            WeiSha.Common.Param.Method.Position posi = WeiSha.Common.Request.Position(entity.Org_Longitude, entity.Org_Latitude);
            entity.Org_Province = posi.Province;
            entity.Org_City = posi.City;
            entity.Org_District = posi.District;
            entity.Org_Street = posi.Street;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Organization>(entity);
                    //员工所属机构名称
                    tran.Update<EmpAccount>(new Field[] { EmpAccount._.Org_Name }, new object[] { entity.Org_Name }, EmpAccount._.Org_ID == entity.Org_ID);
                    tran.Update<Depart>(new Field[] { Depart._.Org_Name }, new object[] { entity.Org_Name }, Depart._.Org_ID == entity.Org_ID);
                    //专业、课程、试卷，考试
                    tran.Update<Subject>(new Field[] { Subject._.Org_Name }, new object[] { entity.Org_Name }, Subject._.Org_ID == entity.Org_ID);
                    tran.Update<Course>(new Field[] { Course._.Org_Name }, new object[] { entity.Org_Name }, Course._.Org_ID == entity.Org_ID);
                    tran.Update<TestPaper>(new Field[] { TestPaper._.Org_Name }, new object[] { entity.Org_Name }, TestPaper._.Org_ID == entity.Org_ID);
                    tran.Update<Examination>(new Field[] { Examination._.Org_Name }, new object[] { entity.Org_Name }, Examination._.Org_ID == entity.Org_ID);
                    //学员与教师                  
                    tran.Update<StudentSort>(new Field[] { StudentSort._.Org_Name }, new object[] { entity.Org_Name }, StudentSort._.Org_ID == entity.Org_ID);
                    tran.Update<Teacher>(new Field[] { Teacher._.Org_Name }, new object[] { entity.Org_Name }, Teacher._.Org_ID == entity.Org_ID);
                    tran.Update<TeacherSort>(new Field[] { TeacherSort._.Org_Name }, new object[] { entity.Org_Name }, TeacherSort._.Org_ID == entity.Org_ID);
                    tran.Commit();
                    this.OrganBuildCache();
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
            this.OrganBuildQrCode(entity);
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
            List<Organization> list = WeiSha.Common.Cache<Organization>.Data.List;
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
            List<Organization> list = WeiSha.Common.Cache<Organization>.Data.List;
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
            List<Organization> list = WeiSha.Common.Cache<Organization>.Data.List;
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
                string twoDomain = WeiSha.Common.Request.Domain.TwoDomain;
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
            List<Organization> list = WeiSha.Common.Cache<Organization>.Data.List;
            if (list == null || list.Count < 1) list = this.OrganBuildCache();
            List<Organization> tm = (from l in list
                                     where l.Org_ID == identify
                                     select l).ToList<Organization>();
            if (tm.Count > 0) curr = tm[0];
            if (curr == null) curr = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == identify).ToFirst<Organization>();
            return curr;
        }

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
                    tran.Delete<Team>(Team._.Org_ID == identify);
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
            this.OrganBuildCache();  //重新构建缓存
        }
        public void OrganBuildQrCode()
        {
            this.OrganBuildCache();  //重新构建缓存  
            //批量生成二维码
            List<Organization> orgs = WeiSha.Common.Cache<Organization>.Data.List;
            if (orgs != null)
            {
                for (int i = 0; i < orgs.Count; i++)
                {
                    OrganBuildQrCode(orgs[i]);
                }
            }
        }
        /// <summary>
        /// 生成当前机构的手机端二维码
        /// </summary>
        /// <param name="entity"></param>
        public void OrganBuildQrCode(Organization entity)
        {
            //如果没有二级域名，则直接退出
            if (string.IsNullOrWhiteSpace(entity.Org_TwoDomain)) return;
            //二维码的Url路径
            string url = entity.Org_QrCodeUrl;
            if (string.IsNullOrWhiteSpace(url))
            {
                string domain = WeiSha.Common.Server.MainName;
                string port = WeiSha.Common.Server.Port;
                url = "http://" + entity.Org_TwoDomain + "." + domain + ":" + port + "/Mobile/default.ashx";
            } 
            //各项配置           
            WeiSha.Common.CustomConfig config = CustomConfig.Load(entity.Org_Config);   //自定义配置项           
            string color = config["QrColor"].Value.String;  //二维码前景色            
            bool isQrcenter = config["IsQrConterImage"].Value.Boolean ?? false; //是否启用中心图片           
            string centerImg = Upload.Get["Org"].Virtual + config["QrConterImage"].Value.String;     //中心图片
            centerImg=WeiSha.Common.Server.MapPath(centerImg);
            //二维码图片对象
            System.Drawing.Image image = null;
            if (isQrcenter && System.IO.File.Exists(centerImg))
                image = WeiSha.Common.QrcodeHepler.Encode(url, 200, centerImg, color, "#fff");           
            else            
                image = WeiSha.Common.QrcodeHepler.Encode(url, 200, color, "#fff");
            //将image转为base64
            string base64 = WeiSha.Common.Images.ImageTo.ToBase64(image);
            entity.Org_QrCode = base64;
            Gateway.Default.Save<Organization>(entity);                    
        }
        public Organization[] OrganAll(bool? isUse, int level)
        {
            //从缓存中读取
            List<Organization> list = WeiSha.Common.Cache<Organization>.Data.List;
            if (list == null || list.Count < 1) list = this.OrganBuildCache();
            //linq查询
            var from = from l in list select l;
            if (level > 0) from = from.Where<Organization>(p => p.Olv_ID == level);
            if (isUse != null) from = from.Where<Organization>(p => p.Org_IsUse == (bool)isUse);
            List<Organization> tm = from.ToList<Organization>();
            if (tm.Count > 0) return tm.ToArray<Organization>();
            //orm查询
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(Organization._.Org_IsUse == (bool)isUse);
            if (level > -1) wc.And(Organization._.Olv_ID == level);
            return Gateway.Default.From<Organization>().Where(wc)
                .OrderBy(Organization._.Org_RegTime.Desc).ToArray<Organization>();
        }

        public Organization[] OrganCount(bool? isUse, bool? isShow, int level, int count)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(Organization._.Org_IsUse == (bool)isUse);
            if (isShow != null) wc.And(Organization._.Org_IsShow == (bool)isShow);
            if (level > 0) wc.And(Organization._.Olv_ID == level);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Organization>().Where(wc)
                .OrderBy(Organization._.Org_RegTime.Desc).ToArray<Organization>(count);
        }
        /// <summary>
        /// 清理缓存文件
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="day">清理多少天之前的</param>
        public void OrganClearTemp(int orgid, int day)
        {
            string tmpPath = WeiSha.Common.Upload.Get["Temp"].Physics;
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
        private static object lock_cache_build = new object();
        /// <summary>
        /// 构建缓存
        /// </summary>
        public List<Organization> OrganBuildCache()
        {
            lock (lock_cache_build)
            {
                try
                {
                    WeiSha.Common.Cache<Song.Entities.Organization>.Data.Clear();
                }
                catch
                {
                }
                finally
                {
                    Song.Entities.Organization[] org = Gateway.Default.From<Organization>()
                        .OrderBy(Organization._.Org_RegTime.Desc).ToArray<Organization>();
                    WeiSha.Common.Cache<Song.Entities.Organization>.Data.Fill(org);
                }
                return WeiSha.Common.Cache<Organization>.Data.List;
            }
        }
        public Organization[] OrganPager(bool? isUse, int level, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(Organization._.Org_IsUse == (bool)isUse);
            if (level > 0) wc.And(Organization._.Olv_ID == level);
            if (!string.IsNullOrWhiteSpace(searTxt) && searTxt.Trim() != "")
                wc.And(Organization._.Org_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<LinksSort>(wc);
            return Gateway.Default.From<Organization>().Where(wc)
                .OrderBy(Organization._.Org_RegTime.Desc)
                .ToArray<Organization>(size, (index - 1) * size);
        }
        
        #region 机构等级管理
        /// <summary>
        /// 添加机构等级
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void LevelAdd(OrganLevel entity)
        {
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
        public OrganLevel[] LevelAll(bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc.And(OrganLevel._.Olv_IsUse == (bool)isUse);
            return Gateway.Default.From<OrganLevel>().Where(wc).OrderBy(OrganLevel._.Olv_Level.Desc).ToArray<OrganLevel>();

        }
        /// <summary>
        /// 根据主键删除公司。
        /// </summary>
        /// <param name="identify">主键id</param>
        public void LevelDelete(int identify)
        {
            Gateway.Default.Delete<OrganLevel>(OrganLevel._.Olv_ID == identify);
        }
        /// <summary>
        /// 将当前栏目向上移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool LevelUp(int id)
        {
            //当前对象
            OrganLevel current = Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_ID == id).ToFirst<OrganLevel>();
            //当前对象排序号
            int orderValue = (int)current.Olv_Tax;
            //上一个对象，即兄长对象；
            OrganLevel up = Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_Tax < orderValue).OrderBy(OrganLevel._.Olv_Tax.Desc).ToFirst<OrganLevel>();
            //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
            if (up == null) return false;
            //交换排序号
            current.Olv_Tax = up.Olv_Tax;
            up.Olv_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<OrganLevel>(current);
                    tran.Save<OrganLevel>(up);
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
        /// <summary>
        /// 将当前栏目向下移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于最底端，则返回false；移动成功，返回true</returns>
        public bool LevelDown(int id)
        {
            //当前对象
            OrganLevel current = Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_ID == id).ToFirst<OrganLevel>();
            //当前对象排序号
            int orderValue = (int)current.Olv_Tax;
            //下一个对象，即弟弟对象；
            OrganLevel down = Gateway.Default.From<OrganLevel>().Where(OrganLevel._.Olv_Tax > orderValue).OrderBy(OrganLevel._.Olv_Tax.Asc).ToFirst<OrganLevel>();
            //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
            if (down == null) return false;
            //交换排序号
            current.Olv_Tax = down.Olv_Tax;
            down.Olv_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<OrganLevel>(current);
                    tran.Save<OrganLevel>(down);
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
        
    }
}
