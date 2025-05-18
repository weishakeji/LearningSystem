using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Song.Entities;
using Song.ServiceInterfaces;
using WeiSha.Core;
using WeiSha.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Song.ServiceImpls
{
    public class CourseCom : ICourse
    {
        #region 课程管理
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CourseAdd(Course entity)
        {
            //课程id为雪花后，需要自主分配数值
            if (entity.Cou_ID <= 0)           
                entity.Cou_ID = WeiSha.Core.Request.SnowID();
            //在改为雪花id前，为了创建表关联，用了uid
            if (string.IsNullOrWhiteSpace(entity.Cou_UID))
                entity.Cou_UID = WeiSha.Core.Request.UniqueID();

            entity.Cou_CrtTime = DateTime.Now;            
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            if (string.IsNullOrEmpty(entity.Sbj_Name))
            {
                Subject sbj = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == entity.Sbj_ID).ToFirst<Subject>();
                if (sbj != null) entity.Sbj_Name = sbj.Sbj_Name;
            }
            //object obj = Gateway.Default.Max<Course>(Course._.Cou_Tax, Course._.Org_ID == entity.Org_ID && Course._.Sbj_ID == entity.Sbj_ID && Course._.Cou_PID == entity.Cou_PID);
            object obj = Gateway.Default.Max<Course>(Course._.Cou_Tax, new WhereClip());
            entity.Cou_Tax = obj is int ? (int)obj + 1 : 0;
            //默认为免费课程
            entity.Cou_IsFree = true;
            entity.Cou_Allowedit = true;    //默认允许编辑

            entity.Cou_Level = _calcLevel(entity);
            entity.Cou_XPath = _calcXPath(entity);
            Gateway.Default.Save<Course>(entity);

            this.CourseCountUpdate(entity.Org_ID, entity.Sbj_ID);
        }
        /// <summary>
        /// 批量添加课程，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        public Course CourseBatchAdd(Teacher teacher, int orgid, long sbjid, string names)
        {
            //整理名称信息
            names = names.Replace("，", ",");
            List<string> listName = new List<string>();
            foreach (string str in names.Split(','))
            {
                string s = str.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                if (s.Trim() != "") listName.Add(s.Trim());
            }
            //
            long pid = 0;
            Song.Entities.Course last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.Course current = Exist(orgid, sbjid, pid, listName[i]);
                if (current == null)
                {
                    current = new Course();
                    current.Cou_Name = listName[i].Trim();
                    current.Cou_IsUse = true;
                    current.Org_ID = orgid;
                    current.Sbj_ID = sbjid;
                    current.Cou_PID = pid;
                    current.Cou_IsUse = true;
                    current.Cou_IsFree = true;
                    current.Cou_Allowedit = true;
                    current.Cou_IsTry = true;
                    //所属老师
                    if (teacher != null)
                    {
                        current.Th_ID = teacher.Th_ID;
                        current.Th_Name = teacher.Th_Name;
                    }
                    this.CourseAdd(current);
                }
                last = current;
                pid = current.Cou_ID;
            }
            //更新统计数据
            Task.Run(() => this.CourseCountUpdate(orgid, sbjid));
            return last;
        }       
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CourseSave(Course entity)
        {
            Course old = CourseSingle(entity.Cou_ID);
            if (old.Cou_PID != entity.Cou_PID)
            {
                object obj = Gateway.Default.Max<Course>(Course._.Cou_Tax, Course._.Org_ID == entity.Org_ID && Course._.Cou_PID == entity.Cou_PID);
                entity.Cou_Tax = obj is int ? (int)obj + 1 : 0;
            }
            //如果图片带有多余路径，只保留文件名
            if (!string.IsNullOrWhiteSpace(entity.Cou_Logo) && entity.Cou_Logo.IndexOf("/") > -1)
                entity.Cou_Logo = entity.Cou_Logo.Substring(entity.Cou_Logo.LastIndexOf("/") + 1);
            if (!string.IsNullOrWhiteSpace(entity.Cou_LogoSmall) && entity.Cou_LogoSmall.IndexOf("/") > -1)
                entity.Cou_LogoSmall = entity.Cou_LogoSmall.Substring(entity.Cou_LogoSmall.LastIndexOf("/") + 1);
            entity.Cou_Level = _calcLevel(entity);
            entity.Cou_XPath = _calcXPath(entity);
            //专业名称
            if (entity.Sbj_ID > 0)
            {
                Subject sbj = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == entity.Sbj_ID).ToFirst<Subject>();
                if (sbj != null) entity.Sbj_Name = sbj.Sbj_Name;
            }
            //限时免费
            entity.Cou_FreeStart = entity.Cou_FreeStart.Date;
            DateTime freeEnd = entity.Cou_FreeEnd.AddDays(1).Date.AddSeconds(-1);
            entity.Cou_FreeEnd = freeEnd;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //如果课程原来免费，但是现在不再免费
                    if (old.Cou_IsFree && !entity.Cou_IsFree)
                    {
                        tran.Update<Student_Course>(
                            new Field[] { Student_Course._.Stc_EndTime }, new object[] { DateTime.Now },
                            Student_Course._.Cou_ID == entity.Cou_ID && Student_Course._.Stc_IsFree == true);
                    }
                    //如果课程更改了专业
                    if (old.Sbj_ID != entity.Sbj_ID)
                    {
                        tran.Update<Questions>(
                                    new Field[] { Questions._.Sbj_ID, Questions._.Sbj_Name },
                                    new object[] { entity.Sbj_ID, entity.Sbj_Name }, Questions._.Cou_ID == entity.Cou_ID);
                        tran.Update<Outline>(new Field[] { Outline._.Sbj_ID }, new object[] { entity.Sbj_ID }, Outline._.Cou_ID == entity.Cou_ID);
                        tran.Update<TestPaper>(
                                    new Field[] { TestPaper._.Cou_Name, TestPaper._.Sbj_ID, TestPaper._.Sbj_Name },
                                    new object[] { entity.Cou_Name, entity.Sbj_ID, entity.Sbj_Name },
                                    TestPaper._.Cou_ID == entity.Cou_ID);
                        Task.Run(() =>
                        {
                            this.CourseCountUpdate(-1, entity.Sbj_ID);
                            this.CourseCountUpdate(-1, old.Sbj_ID);
                        });
                    }
                    else
                    {
                        Task.Run(() => this.CourseCountUpdate(-1, entity.Sbj_ID));
                    }
                    tran.Save<Course>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 修改课程的某些项
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="fields"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool CourseUpdate(long couid, Field[] fields, object[] objs)
        {
            try
            {
                Gateway.Default.Update<Course>(fields, objs, Course._.Cou_ID == couid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改课程的某些项
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool CourseUpdate(long couid, Field field, object obj)
        {
            Gateway.Default.Update<Course>(field, obj, Course._.Cou_ID == couid);
            return true;
        }
        /// <summary>
        /// 增加课程浏览数
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int CourseViewNum(Course entity, int num)
        {
            if (entity == null) return -1;
            if (num <= 0) return entity.Cou_ViewNum;

            entity.Cou_ViewNum += num;
            try
            {
                Gateway.Default.Update<Course>(
                    new Field[] { Course._.Cou_ViewNum },
                    new object[] { entity.Cou_ViewNum }, Course._.Cou_ID == entity.Cou_ID);
            }
            catch { }
            return entity.Cou_ViewNum;
        }
        public int CourseViewNum(long couid, int num)
        {
            Course course = this.CourseSingle(couid);
            return CourseViewNum(course, num);
        }
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CourseDelete(Course entity)
        {
            if (entity == null) return;
            //是否有下级
            bool isExist = CourseIsChildren(entity.Org_ID, entity.Cou_ID, null);
            if (isExist) throw new Exception("当前课程下还有子课程，请先删除子课程。");
          
            //Song.Entities.GuideColumns[] gcs = Business.Do<IGuide>().GetColumnsAll(entity.Cou_ID,string.Empty, null);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {                   
                    tran.Delete<CoursePrice>(CoursePrice._.Cou_UID == entity.Cou_UID);
                    //删除购买记录
                    tran.Delete<Student_Course>(Student_Course._.Cou_ID == entity.Cou_ID && Student_Course._.Stc_Type != 2);
                    //删除学员组与课程的关联
                    tran.Delete<StudentSort_Course>(StudentSort_Course._.Cou_ID == entity.Cou_ID);

                    WeiSha.Core.Upload.Get["Course"].DeleteFile(entity.Cou_Logo);
                    WeiSha.Core.Upload.Get["Course"].DeleteDirectory(entity.Cou_ID.ToString());
                    tran.Delete<Course>(Course._.Cou_ID == entity.Cou_ID);
                    tran.Commit();
                    Task.Run(() =>
                    {
                        this.CourseClear(entity, true);
                    });
                    //更新统计数据
                    this.CourseCountUpdate(entity.Org_ID, entity.Sbj_ID);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void CourseDelete(long identify)
        {
            Song.Entities.Course course = this.CourseSingle(identify);
            this.CourseDelete(course);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Course CourseSingle(long identify)
        {            
            return Gateway.Default.From<Course>().Where(Course._.Cou_ID == identify).ToFirst<Course>();
        }
        /// <summary>
        /// 获取单一实体对象，按UID；
        /// </summary>
        /// <param name="uid">唯一值</param>
        /// <returns></returns>
        public Course CourseSingle(string uid)
        {
            return Gateway.Default.From<Course>().Where(Course._.Cou_UID == uid).ToFirst<Course>();
        }
        /// <summary>
        /// 获取课程名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        public string CourseName(long couid)
        {
            Course entity = Gateway.Default.From<Course>().Where(Course._.Cou_ID == couid).ToFirst<Course>();
            if (entity == null) return "";
            return entity.Cou_Name;
        }
        /// <summary>
        /// 学员是否购买了该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员Id</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <returns></returns>
        public Course IsBuyCourse(long couid, int stid, int state)
        {
            WhereClip wc = Student_Course._.Cou_ID == couid && Student_Course._.Ac_ID == stid;
            wc.And(Student_Course._.Stc_IsTry == false);
            if (state == 1)
                wc.And(Student_Course._.Stc_StartTime < DateTime.Now && Student_Course._.Stc_EndTime > DateTime.Now);
            if (state == 2)
                wc.And(Student_Course._.Stc_EndTime < DateTime.Now);
            return Gateway.Default.From<Course>()
                    .InnerJoin<Student_Course>(Student_Course._.Cou_ID == Course._.Cou_ID)
                    .Where(wc).ToFirst<Course>();

        }
        /// <summary>
        /// 学员是否购买了该课程
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stid"></param>
        /// <returns></returns>
        public bool IsBuy(long couid, int stid)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null || !course.Cou_IsUse) return false;
            WhereClip wc = Student_Course._.Cou_ID == couid && Student_Course._.Ac_ID == stid;
            wc.And(Student_Course._.Stc_IsEnable == true && Student_Course._.Stc_Type != 5);
            //wc.And(Student_Course._.Stc_IsTry == false && Student_Course._.Stc_IsFree == false);
            wc.And(Student_Course._.Stc_StartTime < DateTime.Now && Student_Course._.Stc_EndTime > DateTime.Now);              
            Student_Course sc = Gateway.Default.From<Student_Course>().Where(wc).ToFirst<Student_Course>();
            return sc != null;
        }
        /// <summary>
        /// 学员购买的该课程
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="sear">用于检索课程的字符</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <param name="enable">是否启用</param>
        /// <param name="istry">是否试用，为null时取所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> CourseForStudent(int stid, string sear, int state, bool? enable, bool? istry, int size, int index, out int countSum)
        {
            object[] parameters = new object[] { stid, sear, state, enable, istry, size, index, 0 };
            List<Course> list = DataQuery.DbQuery.Call<List<Course>>(parameters);
            countSum = (int)parameters[parameters.Length - 1];
            return list; 
        }
        /// <summary>
        /// 学员购买的该课程,以及学员组关联的课程,不分页，不排序
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sear"></param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <param name="enable">是否启用</param>
        /// <param name="istry"></param>
        /// <returns></returns>
        public List<Course> CourseForStudent(int stid, string sear, int state, bool? enable, bool? istry)
        {
            object[] parameters = new object[] { stid, sear, state, enable, istry};
            List<Course> list = DataQuery.DbQuery.Call<List<Course>>(parameters);
            return list;
        }
        /// <summary>
        /// 获取收入最多的课程
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> RankIncome(int orgid, long sbjid, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            object[] parameters = new object[] { orgid, sbjid, start, end, size, index, 0 };
            List<Course> list = DataQuery.DbQuery.Call<List<Course>>(parameters);
            countSum = (int)parameters[parameters.Length - 1];
            return list;
        }
        /// <summary>
        /// 课程收益
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        public decimal Income(long couid)
        {
            object obj = Gateway.Default.Sum<Student_Course>(Student_Course._.Stc_Money, Student_Course._.Cou_ID == couid);
            return obj == null ? 0 : Convert.ToDecimal(obj);
        }
        /// <summary>
        /// 课程收益汇总
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public decimal Income(int orgid, long sbjid, DateTime? start, DateTime? end)
        {
            return DataQuery.DbQuery.Call<decimal>(new object[] { orgid, sbjid, start, end });          
        }
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public List<Course> CourseAll(int orgid, long sbjid, int thid, bool? isUse)
        {
            return CourseCount(orgid, sbjid, thid, -1, null, isUse, -1);
        }
        /// <summary>
        /// 某个课程的学习人数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="isAll">是否取全部值，如果为false，则仅取当前正在学习的</param>
        /// <returns></returns>
        public int CourseStudentSum(long couid, bool? isAll)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(Student_Course._.Cou_ID == couid);
            if (isAll == null || isAll == false)
            {
                wc.And(Student_Course._.Stc_StartTime <= DateTime.Now);
                wc.And(Student_Course._.Stc_EndTime > DateTime.Now);
            }
            return Gateway.Default.Count<Student_Course>(wc);
        }
        /// <summary>
        /// 清理课程内容
        /// </summary>
        /// <param name="course">课程实体</param>
        /// <param name="isfreshData">是否刷新相关数据，例如课程内容清空了，专业的试题是否重新统计</param>
        public void CourseClear(Course course,bool isfreshData)
        {
            if (course == null) return;
            long couid = course.Cou_ID;
            //删除章节
            List<Song.Entities.Outline> outline = Gateway.Default.From<Outline>().Where(Outline._.Cou_ID == couid).ToList<Outline>();
            if (outline != null && outline.Count > 0)
            {
                foreach (Song.Entities.Outline ol in outline)
                {
                    Business.Do<IOutline>().OutlineClear(ol);
                    Business.Do<IOutline>().OutlineDelete(ol);
                }
            }           
            //删除试卷
            List<Song.Entities.TestPaper> tps = Gateway.Default.From<TestPaper>().Where(TestPaper._.Cou_ID == couid).ToList<TestPaper>();
            if (tps != null && tps.Count > 0)
            {
                foreach (Song.Entities.TestPaper t in tps)
                    Business.Do<ITestPaper>().PaperDelete(t.Tp_Id);
            }
            //考试指南
            List<Song.Entities.GuideColumns> gcs = Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Cou_ID == couid).ToList<GuideColumns>();
            if (gcs != null && gcs.Count > 0)
            {
                foreach (Song.Entities.GuideColumns t in gcs)
                    Business.Do<IGuide>().ColumnsDelete(t);
            }
            List<Song.Entities.KnowledgeSort> knlsoft = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Cou_ID == couid).ToList<KnowledgeSort>();
            if (gcs != null && knlsoft.Count > 0)
            {
                foreach (Song.Entities.KnowledgeSort t in knlsoft)
                    Business.Do<IKnowledge>().SortDelete(t);
            }
            //清理试题
            List<Song.Entities.Questions> ques = Gateway.Default.From<Questions>().Where(Questions._.Cou_ID == couid).ToList<Questions>();
            if (ques != null && ques.Count > 0)
            {
                foreach (Song.Entities.Questions c in ques)
                    Business.Do<IQuestions>().QuesDelete(c);
                //更新试题数
                if (isfreshData)
                {
                    new Task(() =>
                    {
                        Business.Do<IQuestions>().QuesCountUpdate(course.Org_ID, course.Sbj_ID, course.Cou_ID, -1);
                    }).Start();
                }
            }
            //删除留言
            Gateway.Default.Delete<Message>(Message._.Cou_ID == couid);
            Gateway.Default.Delete<MessageBoard>(MessageBoard._.Cou_ID == couid);
        }
        /// <summary>
        /// 清除课程的内容
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="isfreshData">是否刷新相关数据，例如课程内容清空了，专业的试题是否重新统计</param>
        public void CourseClear(long couid, bool isfreshData)
        {
            Course course = this.CourseSingle(couid);
            this.CourseClear(course, isfreshData);
        }
        /// <summary>
        /// 课程数量,如果计算专业下的课程数，会计算专业所有子级专业的课程数
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isuse">是否包括启用的课程,null取所有，true取启用的，false取未启用的</param>
        /// <param name="isfree">是否免费</param>
        /// <returns></returns>
        public int CourseOfCount(int orgid, long sbjid, int thid, bool? isuse, bool? isfree)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            //if (sbjid > 0) wc.And(Course._.Sbj_ID == sbjid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (isuse != null) wc.And(Course._.Cou_IsUse == (bool)isuse);
            if (isfree != null) wc.And(Course._.Cou_IsFree == (bool)isfree);
            return Gateway.Default.Count<Course>(wc);
        }
        /// <summary>
        /// 专业下的课程数，包括启用、不启用的，所有课程
        /// </summary>
        /// <param name="sbjid"></param>
        /// <returns></returns>
        public int CourseOfCount(long sbjid)
        {
            return Gateway.Default.Count<Course>(Course._.Sbj_ID == sbjid);          
        }
        /// <summary>
        /// 统计机构或专业下的课程数，并保存到机构或专业表
        /// </summary>
        /// <returns></returns>
        public void CourseCountUpdate(int orgid, long sbjid)
        {
            if (sbjid > 0)
            {
                int sbjcount = Gateway.Default.Count<Course>(Course._.Sbj_ID == sbjid);
                Gateway.Default.Update<Subject>(new Field[] { Subject._.Sbj_CourseCount }, new object[] { sbjcount }, Subject._.Sbj_ID == sbjid);
            }
            if (orgid > 0)
            {
                int orgcount = Gateway.Default.Count<Course>(Course._.Org_ID == orgid);
                Gateway.Default.Update<Organization>(new Field[] { Organization._.Org_CourseCount }, new object[] { orgcount }, Organization._.Org_ID == orgcount);
            }
        }
        /// <summary>
        /// 更新课程的统计数据，包括课程的章节数、试题数等
        /// </summary>
        /// <param name="orgid">机构id，如果大于0，则刷新当前机构下的所有专业数据</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public bool UpdateStatisticalData(int orgid, long couid)
        {
            if (orgid > 0)
            {
                List<Course> courses = this.CourseCount(orgid, -1, string.Empty, string.Empty, null, 0);
                if (courses != null && courses.Count > 0)
                {
                    foreach (Course cou in courses)
                        this.UpdateStatisticalData(-1, cou.Cou_ID);
                }
            }
            else if (couid > 0)
            {
                //更新课程的试卷数，章节数，视频数
                int paper_count = Business.Do<ITestPaper>().PaperOfCount(-1, -1, couid, -1, null);
                int outline_count = Business.Do<IOutline>().OutlineOfCount(couid, -1, null);
                int video_count = Business.Do<IOutline>().OutlineOfCount(couid, -1, null, true, null, null);
                Business.Do<ICourse>().CourseUpdate(couid,
                    new Field[] { Course._.Cou_TestCount, Course._.Cou_OutlineCount, Course._.Cou_VideoCount },
                    new object[] { paper_count, outline_count, video_count });
                //更新课程与章节下的试题数量
                Business.Do<IQuestions>().QuesCountUpdate(-1, -1, couid, -1);
            }
            return true;
        }
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="sbjid"></param>
        /// <param name="thid">教师id</param>
        /// <param name="pid"></param>
        /// <param name="sear"></param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        public List<Course> CourseCount(int orgid, long sbjid, int thid, int pid, string sear, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (pid > 0) wc.And(Course._.Cou_ID == pid);
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            return Gateway.Default.From<Course>().Where(wc)
                .OrderBy(Course._.Cou_ID.Desc).ToList<Course>(count);
            //如果是采用多个教师对应一个课程，用下面的方法
            //count = count < 1 ? int.MaxValue : count;
            //if (thid < 1)
            //{
            //    WhereClip wc = Course._.Org_ID == orgid;
            //    if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            //    return Gateway.Default.From<Course>().Where(wc).OrderBy(Course._.Cou_Tax.Desc).ToList<Course>();
            //}
            //return Gateway.Default.From<Course>()
            //    .InnerJoin<Teacher_Course>(Teacher_Course._.Cou_ID == Course._.Cou_ID)
            //    .Where(Teacher_Course._.Th_ID == thid)
            //    .OrderBy(Course._.Cou_Tax.Desc).ToList<Course>();

        }
        public List<Course> CourseCount(int orgid, long sbjid, string sear, string order, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            OrderByClip wcOrder = new OrderByClip();
            if (order == "flux") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "def") wcOrder = Course._.Cou_CrtTime.Desc;
            if (order == "tax") wcOrder = Course._.Cou_Tax.Desc & Course._.Cou_CrtTime.Desc;
            if (order == "new") wcOrder = Course._.Cou_CrtTime.Desc;    //最新发布
            if (order == "rec") wcOrder = Course._.Cou_IsRec.Desc & Course._.Cou_Tax.Desc & Course._.Cou_CrtTime.Desc;
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Contains(sear));
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            wcOrder = wcOrder & Course._.Cou_ID.Desc;
            return Gateway.Default.From<Course>().Where(wc)
               .OrderBy(wcOrder).ToList<Course>(count);
        }
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="thid">教师id</param>
        /// <param name="islive">是否有直播课</param>
        /// <param name="sear"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Course> CourseCount(int orgid, long sbjid, int thid, bool? islive, string sear, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (islive != null) wc.And(Course._.Cou_ExistLive == (bool)islive);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Contains(sear));
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            return Gateway.Default.From<Course>().Where(wc)
               .OrderBy(Course._.Cou_ID.Desc).ToList<Course>(count);
        }
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="sbjid">专业id，等于0取所有</param>
        /// <param name="sear"></param>
        /// <param name="isUse"></param>
        /// <param name="order">排序方式，默认null按排序顺序，flux流量最大优先,def推荐、流量，tax排序号，new最新,rec推荐</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Course> CourseCount(int orgid, long sbjid, string sear, bool? isUse, string order, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Contains(sear));
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "flux") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "def") wcOrder = Course._.Cou_CrtTime.Desc;
            if (order == "tax") wcOrder = Course._.Cou_Tax.Desc & Course._.Cou_CrtTime.Desc;
            if (order == "new") wcOrder = Course._.Cou_CrtTime.Desc;    //最新发布
            if (order == "rec") wcOrder = Course._.Cou_IsRec.Desc && Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            wcOrder = wcOrder & Course._.Cou_ID.Desc;
            return Gateway.Default.From<Course>().Where(wc)
               .OrderBy(wcOrder).ToList<Course>(count);
        }
        public bool CourseIsChildren(int orgid, long couid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Course>(wc && Course._.Cou_PID == couid);
            return count > 0;
        }

        public List<Course> CoursePager(int orgid, long sbjid, int thid, bool? isUse, string searTxt, string order, int size, int index, out int countSum)
        {
            WhereClip wc = Course._.Org_ID == orgid;
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(Course._.Cou_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<Course>(wc);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "flux") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "def") wcOrder = Course._.Cou_CrtTime.Desc;
            if (order == "tax") wcOrder = Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            if (order == "new") wcOrder = Course._.Cou_CrtTime.Desc;    //最新发布
            if (order == "rec") wcOrder = Course._.Cou_IsRec.Desc && Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            wcOrder = wcOrder & Course._.Cou_ID.Desc;
            return Gateway.Default.From<Course>().Where(wc).OrderBy(wcOrder).ToList<Course>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid">专业id,多个id用逗号分隔</param>
        /// <param name="thid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="order"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> CoursePager(int orgid, string sbjid, int thid, bool? isUse, string searTxt, string order, int size, int index, out int countSum)
        {
            return this.CoursePager(orgid, sbjid, thid, isUse, null, null, searTxt, order, size, index, out countSum);
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="thid"></param>
        /// <param name="isUse"></param>
        /// <param name="isLive">是否是直播课</param>
        /// <param name="searTxt"></param>
        /// <param name="order"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> CoursePager(int orgid, string sbjid, int thid, bool? isUse, bool? isLive, bool? isFree, string searTxt, string order, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (!string.IsNullOrWhiteSpace(sbjid))
            {
                WhereClip wcSbjid = new WhereClip();
                foreach (string tm in sbjid.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(tm)) continue;
                    long sbj = 0;
                    long.TryParse(tm, out sbj);
                    if (sbj <= 0) continue;
                    wcSbjid.Or(Course._.Sbj_ID == sbj);
                    //当前专业的下级专业也包括
                    List<long> list = Business.Do<ISubject>().TreeID(sbj, orgid);
                    foreach (long l in list)
                        wcSbjid.Or(Course._.Sbj_ID == l);
                }
                wc.And(wcSbjid);
            }
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            if (isFree != null) wc.And(Course._.Cou_IsFree == (bool)isFree);
            if (order == "live") isLive = true;
            if (isLive != null) wc.And(Course._.Cou_ExistLive == (bool)isLive);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(Course._.Cou_Name.Contains(searTxt.Trim()));
            countSum = Gateway.Default.Count<Course>(wc);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "flux") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "hot") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "def") wcOrder = Course._.Cou_CrtTime.Desc;
            if (order == "tax") wcOrder = Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            if (order == "new") wcOrder = Course._.Cou_CrtTime.Desc;    //最新发布
            if (order == "last") wcOrder = Course._.Cou_CrtTime.Asc;    //最后发布
            //按试题数量的正序，倒序
            if ("quesAsc".Equals(order,StringComparison.CurrentCultureIgnoreCase)) wcOrder = Course._.Cou_QuesCount.Asc;
            if ("quesDesc".Equals(order, StringComparison.CurrentCultureIgnoreCase)) wcOrder = Course._.Cou_QuesCount.Desc;
            //按视频数量的排序
            if ("videoAsc".Equals(order, StringComparison.CurrentCultureIgnoreCase)) wcOrder = Course._.Cou_VideoCount.Asc;
            if ("videoDesc".Equals(order, StringComparison.CurrentCultureIgnoreCase)) wcOrder = Course._.Cou_VideoCount.Desc;
            //
            if (order == "rec") wcOrder = Course._.Cou_IsRec.Desc && Course._.Cou_Tax.Asc && Course._.Cou_CrtTime.Desc;
            if (order == "free")
            {
                wc.And(Course._.Cou_IsFree == true);
                wcOrder = Course._.Cou_IsFree.Desc & Course._.Cou_Tax.Desc;
            }
            wcOrder = wcOrder & Course._.Cou_ID.Desc;
            //if (order == "live") wc.And();
            return Gateway.Default.From<Course>().Where(wc).OrderBy(wcOrder).ToList<Course>(size, (index - 1) * size);
        }
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象向前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool CourseUp(int id)
        {
            //当前对象
            Course current = Gateway.Default.From<Course>().Where(Course._.Cou_ID == id).ToFirst<Course>();
            int tax = (int)current.Cou_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            Course next = Gateway.Default.From<Course>()
                .Where(Course._.Cou_Tax > tax)
                .OrderBy(Course._.Cou_Tax.Asc).ToFirst<Course>();
            if (next == null) return false;

            //交换排序号
            current.Cou_Tax = next.Cou_Tax;
            next.Cou_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Course>(current);
                    tran.Save<Course>(next);
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }          
           
        }
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象向后移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool CourseDown(int id)
        {
            //当前对象
            Course current = Gateway.Default.From<Course>().Where(Course._.Cou_ID == id).ToFirst<Course>();
            int tax = (int)current.Cou_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            Course prev = Gateway.Default.From<Course>()
                .Where(Course._.Cou_Tax < tax)
                .OrderBy(Course._.Cou_Tax.Desc).ToFirst<Course>();
            if (prev == null) return false;
            //交换排序号
            current.Cou_Tax = prev.Cou_Tax;
            prev.Cou_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Course>(current);
                    tran.Save<Course>(prev);
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }        

        #region 私有方法
        /// <summary>
        /// 计算当前对象在多级分类中的层深
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int _calcLevel(Song.Entities.Course entity)
        {
            //if (entity.Cou_PID == 0) return 1;
            int level = 1;
            Song.Entities.Course tm = Gateway.Default.From<Course>().Where(Course._.Cou_ID == entity.Cou_PID).ToFirst<Course>();
            while (tm != null)
            {
                level++;
                if (tm.Cou_PID == 0) break;
                if (tm.Cou_PID != 0)
                {
                    tm = Gateway.Default.From<Course>().Where(Course._.Cou_ID == tm.Cou_PID).ToFirst<Course>();
                }
            }
            entity.Cou_Level = level;
            Gateway.Default.Save<Course>(entity);
            List<Song.Entities.Course> childs = Gateway.Default.From<Course>().Where(Course._.Cou_PID == entity.Cou_ID).ToList<Course>();
            foreach (Course s in childs)
            {
                _calcLevel(s);
            }
            return level;
        }
        /// <summary>
        /// 计算当前对象在多级分类中的路径
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string _calcXPath(Song.Entities.Course entity)
        {
            //if (entity.Cou_PID == 0) return "";
            string xpath = "";
            Song.Entities.Course tm = Gateway.Default.From<Course>().Where(Course._.Cou_ID == entity.Cou_PID).ToFirst<Course>();
            while (tm != null)
            {
                xpath = tm.Cou_ID + "," + xpath;
                if (tm.Cou_PID == 0) break;
                if (tm.Cou_PID != 0)
                {
                    tm = Gateway.Default.From<Course>().Where(Course._.Cou_ID == tm.Cou_PID).ToFirst<Course>();
                }
            }
            entity.Cou_XPath = xpath;
            Gateway.Default.Save<Course>(entity);
            List<Song.Entities.Course> childs = Gateway.Default.From<Course>().Where(Course._.Cou_PID == entity.Cou_ID).ToList<Course>();
            foreach (Course s in childs)
            {
                _calcXPath(s);
            }
            return xpath;
        }
        #endregion

        #endregion

        #region 课程状态
        /// <summary>
        /// 课程是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Course Exist(int orgid, long sbjid, long pid, string name)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Course._.Org_ID == orgid;
            if (sbjid > 0) wc &= Course._.Sbj_ID == sbjid;
            if (pid >= 0) wc &= Course._.Cou_PID == pid;
            return Gateway.Default.From<Course>().Where(wc && Course._.Cou_Name == name.Trim()).ToFirst<Course>();
        }
        /// <summary>
        /// 是否为直播课
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        public bool IsLiveCourse(long couid)
        {
            Course cou = this.CourseSingle(couid);
            if (cou == null) return false;
            return cou.Cou_ExistLive;
        }
        /// <summary>
        /// 是否为直播课
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="check">校验，如果为true，则检索课程下所有章节，有直播章节，则课程为直播课程</param>
        /// <returns></returns>
        public bool IsLiveCourse(long couid, bool check)
        {
            if (!check) return this.IsLiveCourse(couid);
            List<Outline> outs = Business.Do<IOutline>().OutlineCount(couid, -1, null, 0);
            bool isExist = false;
            foreach (Outline o in outs)
            {
                if (o.Ol_IsUse && o.Ol_IsLive)
                {
                    isExist = true;
                    break;
                }
            }
            Gateway.Default.Update<Course>(
                new Field[] { Course._.Cou_ExistLive },
                new object[] { isExist }, Course._.Cou_ID == couid);

            return isExist;
        }
        /// <summary>
        /// 课程是否在有视频
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        public bool ExistVideo(long couid)
        {
            int count = Gateway.Default.From<Outline>().Where(Outline._.Cou_ID == couid && Outline._.Ol_IsVideo==true 
                && Outline._.Ol_IsUse==true && Outline._.Ol_IsFinish==true).Count();
            return count > 0;
        }
        /// <summary>
        /// 课程是否在有试题
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        public bool ExistQuestion(long couid)
        {
            int count = Gateway.Default.From<Questions>().Where(Questions._.Cou_ID == couid && Questions._.Qus_IsUse == true).Count();
            return count > 0;
        }
        #endregion

        #region 课程关联管理（与学生或教师）
        /// <summary>
        /// 获取选学人数最多的课程列表，从多到少
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <returns></returns>
        public List<Course> RankHot(int orgid, long sbjid, DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            object[] parameters = new object[] { orgid, sbjid, start, end, size, index, 0 };
            List<Course> list = DataQuery.DbQuery.Call<List<Course>>(parameters);
            countSum = (int)parameters[parameters.Length - 1];
            return list;           
        }
        /// <summary>
        /// 某个学生是否正在学习某个课程
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public bool StudyForCourse(int stid, long couid)
        {
            Song.Entities.Student_Course sc = Gateway.Default.From<Student_Course>()
                   .Where(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid && Student_Course._.Stc_IsTry==false
                   && Student_Course._.Stc_StartTime < DateTime.Now && Student_Course._.Stc_EndTime > DateTime.Now)
                   .ToFirst<Student_Course>();
            return sc != null;
        }
        /// <summary>
        /// 学生购买课程的记录项
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public Student_Course StudentCourse(int stid, long couid)
        {
            return Gateway.Default.From<Student_Course>().Where(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid)
                .ToFirst<Student_Course>();           
        }
        /// <summary>
        /// 学生与课程的关联记录项
        /// </summary>
        /// <param name="stcid">记录项的主键id</param>
        /// <returns></returns>
        public Student_Course StudentCourse(int stcid)
        {
            return Gateway.Default.From<Student_Course>().Where(Student_Course._.Stc_ID == stcid).ToFirst<Student_Course>();
        }
        /// <summary>
        /// 学生与课程的关联记录项，如果autoCreate为true，当没有关联项时，且课程为免费状态，可以自动创建关联
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="autoCreate">是否自动创建关联记录项</param>
        /// <returns></returns>
        public Student_Course StudentCourse(int stid, long couid, bool autoCreate)
        {
            Student_Course sc = this.StudentCourse(stid, couid);
            if (sc != null || !autoCreate) return sc;

            //autoCreate为true,
            //获取课程信息
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            //是否免费，或是限时免费
            if (course.Cou_IsLimitFree)
            {
                DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                    course.Cou_IsLimitFree = false;
            }
            //课程在启用状态下，免费或限时免费中，创建记录
            if (course.Cou_IsUse && (course.Cou_IsFree || course.Cou_IsLimitFree))
            {
                if (course.Cou_IsFree)
                    sc = this.FreeStudy(stid, couid, null, DateTime.Now.AddYears(100));
                else
                    sc = this.FreeStudy(stid, couid, course.Cou_FreeStart, course.Cou_FreeEnd);
            }
            return sc;
        }
        /// <summary>
        /// 直接开课，创建学员与课程的关联信息
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public int BeginCourse(int stid, DateTime start, DateTime end, long couid, int orgid)
        {
            Student_Course sc = Gateway.Default.From<Student_Course>().Where(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid)
                .ToFirst<Student_Course>();
            if (sc == null)
            {
                sc = new Student_Course();
                sc.Stc_CrtTime = DateTime.Now;
                sc.Ac_ID = stid;
                sc.Cou_ID = couid;
                sc.Org_ID = orgid;
            }
            if (start > DateTime.Now) start = DateTime.Now;
            sc.Stc_StartTime = start.Date;
            //过期时间，为当天11：59：59结束
            DateTime enddate = end.AddDays(1);
            sc.Stc_EndTime = enddate.Date.AddSeconds(-1);

            sc.Stc_IsEnable = true;
            sc.Stc_Type = 3;    //免费为0，试用为1，购买为2，后台开课为3
            Gateway.Default.Save<Student_Course>(sc);
            return sc.Stc_ID;
        }
        /// <summary>
        /// 更新学员购买课程的记录的信息
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        public Student_Course StudentCourseUpdate(Student_Course sc)
        {
            Gateway.Default.Save<Student_Course>(sc);
            return sc;
        }
        /// <summary>
        /// 保存学员的成绩记录
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="study">学习记录，即视频观看进度</param>
        /// <param name="ques">试题练习记录，通过率</param>
        /// <param name="exam">结课考试的成绩</param>
        public void StudentScoreSave(Student_Course sc, float study, float ques, float exam)
        {
            if (sc == null) return;
            if (study >= 100) study = 100;
            if (ques >= 100) ques = 100;
            if (exam >= 100) exam = 100;
            //
            bool ischange = false;
            if (study >= 0 && sc.Stc_StudyScore != study)
            {
                sc.Stc_StudyScore = study;
                ischange = true;
            }
            if (ques >= 0 && sc.Stc_QuesScore != ques) {
                sc.Stc_QuesScore = ques;
                ischange = true;
            }              
            if (exam >= 0 && sc.Stc_ExamScore != exam) {
                sc.Stc_ExamScore = exam;
                ischange = true;
            }
            if (!ischange) return;
            //此处要计算综合成绩
            sc = this.ResultScoreCalc(sc);
            //保存更新
            Gateway.Default.Update<Student_Course>(
                new Field[] { Student_Course._.Stc_StudyScore, Student_Course._.Stc_QuesScore, Student_Course._.Stc_ExamScore },
                new object[] { sc.Stc_StudyScore, sc.Stc_QuesScore, sc.Stc_ExamScore },
                Student_Course._.Stc_ID == sc.Stc_ID);
        }
        
        /// <summary>
        /// 购买课程
        /// </summary>
        /// <param name="stc">学生与课程的关联对象</param>
        public Student_Course Buy(Student_Course stc)
        {
            Course course = Gateway.Default.From<Course>().Where(Course._.Cou_ID == stc.Cou_ID).ToFirst<Course>();
            if (course == null) throw new Exception("要购买的课程不存在！");
            Accounts st = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == stc.Ac_ID).ToFirst<Accounts>();
            if (st == null) throw new Exception("当前学员不存在！");
            //判断学员与课程是否在一个机构下，
            //if (st.Org_ID != course.Org_ID) throw new Exception("当前员员与课程不隶属同一机构，不可选修！");
            //
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            stc.Org_ID = org.Org_ID;
            stc.Stc_CrtTime = DateTime.Now;
            stc.Stc_IsTry = false;
            stc.Stc_IsEnable = true;
            stc.Sts_ID = 0;
            stc.Stc_Type = 2;    //免费为0，试用为1，购买为2，后台开课为3
            Gateway.Default.Save<Student_Course>(stc);
            return stc;
        }
        /// <summary>
        /// 购买课程
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="price">价格项</param>
        /// <returns></returns>
        public Student_Course Buy(int stid, long couid, Song.Entities.CoursePrice price)
        {
            Accounts st = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == stid).ToFirst<Accounts>();
            if (st == null) throw new Exception("当前学员不存在！");
            return this.Buy(st, couid, price);
        }
        public Student_Course Buy(Accounts st, long couid, Song.Entities.CoursePrice price)
        {
            Course course = Gateway.Default.From<Course>().Where(Course._.Cou_ID == couid).ToFirst<Course>();
            if (course == null) throw new Exception("要购买的课程不存在！");           
         
            //余额是否充足
            decimal money = st.Ac_Money;    //资金余额
            int coupon = st.Ac_Coupon;      //卡券余额
            int mprice = price.CP_Price;    //价格，所需现金
            int cprice = price.CP_Coupon;   //价格，可以用来抵扣的卡券
            bool tm = money >= mprice || money >= (mprice - (coupon > cprice ? cprice : coupon));
            if (!tm) throw new Exception("资金余额或卡券不足");
            //计算需要扣除的金额，优先扣除券
            cprice = cprice >= coupon ? coupon : cprice;    //减除的卡券数
            mprice = mprice - cprice;   //减除的现金数

            //判断学员与课程是否在一个机构下，
            //if (st.Org_ID != course.Org_ID) throw new Exception("当前员员与课程不隶属同一机构，不可选修！");
            //*********************生成流水账的操作对象
            Song.Entities.MoneyAccount ma = new Song.Entities.MoneyAccount();
            Song.Entities.CouponAccount ca = new Song.Entities.CouponAccount();
            ma.Ac_ID = ca.Ac_ID = st.Ac_ID;
            ma.Ma_Money = mprice;  //购买价格
            ca.Ca_Value = cprice;   //要扣除的卡券
            //购买结束时间
            DateTime start = DateTime.Now, end = DateTime.Now;
            if (price.CP_Unit == "日" || price.CP_Unit == "天") end = start.AddDays(price.CP_Span);
            if (price.CP_Unit == "周") end = start.AddDays(price.CP_Span * 7);
            if (price.CP_Unit == "月") end = start.AddMonths(price.CP_Span);
            if (price.CP_Unit == "年") end = start.AddYears(price.CP_Span);
            //int span = (end - start).Days;
            ma.Ma_From = ca.Ca_From = 4;
            ma.Ma_Source = ca.Ca_Source = "购买课程";
            ma.Ma_Info = ca.Ca_Info = "购买课程:" + course.Cou_Name + "；" + DateTime.Now.ToString("yyyy-MM-dd") + " 至 " + end.ToString("yyyy-MM-dd");

            //***************
            //生成学员与课程的关联
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(st.Ac_ID, couid);
            if (sc == null)
            {
                sc = new Entities.Student_Course();
                sc.Stc_CrtTime = DateTime.Now;
            }
            sc.Cou_ID = couid;
            sc.Ac_ID = st.Ac_ID;
            sc.Stc_Money = mprice;
            sc.Stc_Coupon = cprice;
            sc.Stc_StartTime = DateTime.Now;
            //过期时间，为当天11：59：59结束
            DateTime enddate = end.AddDays(1);
            sc.Stc_EndTime = enddate.Date.AddSeconds(-1);

            sc.Stc_IsFree = false;
            sc.Stc_IsTry = false;
            sc.Stc_IsEnable = true;
            sc.Sts_ID = 0;
            sc.Stc_Type = 2;    //免费为0，试用为1，购买为2，后台开课为3            
            sc.Org_ID = st.Org_ID;
            //
            ma.Ma_IsSuccess = true;
            if (mprice > 0) Business.Do<IAccounts>().MoneyPay(ma);
            if (cprice > 0) Business.Do<IAccounts>().CouponPay(ca);
            //分润
            Business.Do<IProfitSharing>().Distribution(course, st, mprice, cprice);
            Gateway.Default.Save<Student_Course>(sc);
            return sc;
        }
        /// <summary>
        /// 免费学习
        /// </summary>
        /// <param name="stid">学习ID</param>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        public Student_Course FreeStudy(int stid, long couid)
        {
            return FreeStudy(stid, couid, DateTime.Now, DateTime.Now.AddYears(101));
        }
        /// <summary>
        /// 免费学习
        /// </summary>
        /// <param name="stid">学习ID</param>
        /// <param name="couid">课程ID</param>
        /// <param name="start">免费时效的开始时间</param>
        /// <param name="end">免费时效的结束时间</param>
        /// <returns></returns>
        public Student_Course FreeStudy(int stid, long couid, DateTime? start, DateTime end)
        {
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null)
            {
                sc = new Entities.Student_Course();
                sc.Stc_StartTime = start == null ? DateTime.Now : (DateTime)start;
            }
            sc.Stc_CrtTime = DateTime.Now;
            sc.Cou_ID = couid;
            sc.Ac_ID = stid;
            sc.Stc_StartTime = start == null ? sc.Stc_StartTime : (DateTime)start;
            //过期时间，为当天11：59：59结束
            DateTime enddate = end.AddDays(1);
            sc.Stc_EndTime = enddate.Date.AddSeconds(-1);          

            sc.Stc_IsFree = true;
            sc.Stc_IsTry = false;
            sc.Stc_IsEnable = true;
            sc.Stc_Type = 0;    //免费为0，试用为1，购买为2，后台开课为3
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) sc.Org_ID = org.Org_ID;
            Gateway.Default.Save<Student_Course>(sc);
            return sc;
        }
        /// <summary>
        /// 课程试用
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        public Student_Course Tryout(int stid, long couid)
        {
            //生成学员与课程的关联
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null) sc = new Entities.Student_Course();
            sc.Cou_ID = couid;
            sc.Ac_ID = stid;
            sc.Stc_StartTime = DateTime.Now;
            sc.Stc_EndTime = DateTime.Now.AddYears(200);
            sc.Stc_IsFree = false;
            sc.Stc_IsTry = true;
            sc.Stc_IsEnable = true;
            sc.Stc_Type = 1;    //免费为0，试用为1，购买为2，后台开课为3
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            sc.Org_ID = org.Org_ID;
            sc.Stc_CrtTime = DateTime.Now;
            Gateway.Default.Save<Student_Course>(sc);
            return sc;
        }
        /// <summary>
        /// 是否试用该课程
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stid"></param>
        /// <returns></returns>
        public bool IsTryout(long couid, int stid)
        {
            WhereClip wc = Student_Course._.Cou_ID == couid && Student_Course._.Ac_ID == stid;
            wc &= Student_Course._.Stc_IsTry == true;
            Student_Course sc = Gateway.Default.From<Student_Course>().Where(wc).ToFirst<Student_Course>();
            return sc != null;
        }
        /// <summary>
        /// 是否直接学习该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        /// <returns>如果是免费或限时免费、或试学的课程，可以学习并返回true，不可学习返回false</returns>
        public bool AllowStudy(long couid, int stid)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(stid);
            return this.AllowStudy(course, acc);
        }
        /// <summary>
        /// 是否可以直接学习该课程
        /// </summary>
        /// <param name="course">课程对象</param>
        /// <param name="acc">学员账号对象</param>
        /// <returns></returns>
        public bool AllowStudy(Course course, Accounts acc)
        {          
            if (course == null || !course.Cou_IsUse) return false;
            if(acc==null) return false;          

            int stid = acc.Ac_ID;
            long couid = course.Cou_ID;

            //是否存在于学员组所关联的课程
            bool isExistSort = Business.Do<IStudent>().SortExistCourse(couid, acc.Sts_ID);
            if (isExistSort) return true;

            //获取学员与课程的关联
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null)
            {
                //免费
                if (course.Cou_IsFree)
                {
                    sc = this.FreeStudy(stid, couid);
                }
                else
                {
                    //限时免费
                    if (course.Cou_IsLimitFree)
                    {
                        DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                        if (course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now)
                        {
                            sc = this.FreeStudy(stid, couid, DateTime.Now, course.Cou_FreeEnd.AddDays(1).Date);
                            course.Cou_IsFree = true;
                        }
                    }
                    else
                    {
                        //试学
                        if (course.Cou_IsTry) sc = this.Tryout(stid, couid);
                    }
                }
                return course.Cou_IsFree || course.Cou_IsTry;
            }
            else
            {
                if (sc.Stc_IsEnable == false) return false;
                //是否购买
                bool isbuy = this.IsBuy(couid, stid);
                if (isbuy && sc.Stc_IsEnable) return true;
                //课程免费
                if (course.Cou_IsFree)
                {
                    sc = this.FreeStudy(stid, couid, null, DateTime.Now.AddYears(101));
                    return true;
                }
                else
                {
                    //限时免费
                    if (course.Cou_IsLimitFree)
                    {
                        DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                        if (course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now)
                        {
                            sc = this.FreeStudy(stid, couid, null, course.Cou_FreeEnd.AddDays(1).Date);
                            return true;
                        }
                    }
                    else
                    {
                        //试学
                        if (course.Cou_IsTry)
                        {
                            sc = this.Tryout(stid, couid);
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        /// 取消课程学习
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        public void DeleteCourseBuy(int stid, long couid)
        {
            //Gateway.Default.Delete<Student_Course>(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Student_Course>(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_CurrCourse }, new object[] { -1 }, Accounts._.Ac_ID == stid && Accounts._.Ac_CurrCourse == couid);
                    tran.Commit();
                    System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(() =>
                    {
                        //学习记录
                        Gateway.Default.Delete<LogForStudentQuestions>(LogForStudentQuestions._.Ac_ID == stid && LogForStudentQuestions._.Cou_ID == couid);
                        Gateway.Default.Delete<LogForStudentStudy>(LogForStudentStudy._.Ac_ID == stid && LogForStudentStudy._.Cou_ID == couid);
                        //试题，收藏、笔记、错题
                        Gateway.Default.Delete<Student_Collect>(Student_Collect._.Ac_ID == stid && Student_Collect._.Cou_ID == couid);
                        Gateway.Default.Delete<Student_Notes>(Student_Notes._.Ac_ID == stid && Student_Notes._.Cou_ID == couid);
                        Gateway.Default.Delete<Student_Ques>(Student_Ques._.Ac_ID == stid && Student_Ques._.Cou_ID == couid);
                        //模拟测试
                        Gateway.Default.Delete<TestResults>(TestResults._.Ac_ID == stid && TestResults._.Cou_ID == couid);

                    });
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取某个教师关联的课程
        /// </summary>
        /// <param name="thid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Course> Course4Teacher(int thid, int count)
        {
            count = count < 1 ? int.MaxValue : count;
            return Gateway.Default.From<Course>()
                    .InnerJoin<Teacher_Course>(Teacher_Course._.Cou_ID == Course._.Cou_ID)
                    .Where(Teacher_Course._.Th_ID == thid)
                    .OrderBy(Course._.Cou_Tax.Desc).ToList<Course>(count);
        }
        /// <summary>
        /// 学习某个课程的学员
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stname"></param>
        /// <param name="stmobi"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Accounts[] Student4Course(long couid, string stname, string stmobi, int size, int index, out int countSum)
        {
            WhereClip wc = Student_Course._.Cou_ID == couid;

            if (!string.IsNullOrWhiteSpace(stname) && stname.Trim() != "") wc.And(Accounts._.Ac_Name.Contains(stname));
            if (!string.IsNullOrWhiteSpace(stmobi) && stmobi.Trim() != "")
            {
                WhereClip wcOr = new WhereClip();              
                wcOr.Or(Accounts._.Ac_MobiTel1.Contains(stmobi));
                wcOr.Or(Accounts._.Ac_MobiTel2.Contains(stmobi));
                wc.And(wcOr);
            }            
            countSum = Gateway.Default.From<Accounts>().InnerJoin<Student_Course>(Student_Course._.Ac_ID == Accounts._.Ac_ID)
                   .Where(wc).OrderBy(Accounts._.Ac_LastTime.Desc).Count();

            return Gateway.Default.From<Accounts>()
                   .InnerJoin<Student_Course>(Student_Course._.Ac_ID == Accounts._.Ac_ID)
                   .Where(wc).OrderBy(Accounts._.Ac_LastTime.Desc).ToArray<Accounts>(size, (index - 1) * size);

        }
        /// <summary>
        /// 快要过期的课程
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="day">剩余几天内的</param>
        /// <returns></returns>
        public List<Student_Course> OverdueSoon(int acid, int day)
        {
            WhereClip wc = Student_Course._.Ac_ID == acid;
            //免费为0，试用为1，购买为2，后台开课为3（机构管理员在学员管理中，为学员单独开课），学习卡为4，学员组关联为5
            WhereClip wctype = new WhereClip();
            foreach (int t in new int[] { 2, 3, 4 })
                wctype.Or(Student_Course._.Stc_Type == t);
            wc.And(wctype);
            //时间
            //wc.And(Student_Course._.Stc_StartTime < DateTime.Now && Student_Course._.Stc_EndTime >= DateTime.Now);      //课程仍处于学习时间内
            wc.And(Student_Course._.Stc_EndTime> DateTime.Now && Student_Course._.Stc_EndTime <= DateTime.Now.AddDays(day));      //结束时间剩余几天内的
            //是否启用的，（即便是购买的过程，管理员也可以设置不让学习，所以此处要做判断）
            wc.And(Student_Course._.Stc_IsEnable == true);

            return Gateway.Default.From<Student_Course>()
                 .Where(wc).OrderBy(Student_Course._.Stc_EndTime.Desc).ToList<Student_Course>();
        }
        #endregion

        #region 学习成果计算
        /// <summary>
        /// 计算学员的综合成绩
        /// </summary>
        /// <param name="sc">学员选修记录的实体</param>
        public Student_Course ResultScoreCalc(Student_Course sc)
        {
            if (sc == null) return sc;
            Course course = Gateway.Default.From<Course>().Where(Course._.Cou_ID == sc.Cou_ID).ToFirst<Course>();
            if (course == null) return sc;
            //课程所在机构
            Organization org = Business.Do<IOrganization>().OrganSingle(course.Org_ID);
            if (org == null) org = Business.Do<IOrganization>().OrganCurrent();
            //计算综合成绩时，要获取机构的相关参数
            WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //视频学习的权重   //试题通过率的权重   //结课考试的权重
            float weight_video = (float)(config["finaltest_weight_video"].Value.Double ?? 33.3333) / 100;
            float weight_ques = (float)(config["finaltest_weight_ques"].Value.Double ?? 33.3333) / 100;
            float weight_exam = (float)(config["finaltest_weight_exam"].Value.Double ?? 33.3333) / 100;
            //视频完成度的容差
            float video_lerance = (float)(config["VideoTolerance"].Value.Double ?? 0);

            //课程是否有视频与视频
            bool existvideo = this.ExistVideo(course.Cou_ID);
            bool existques = this.ExistQuestion(course.Cou_ID);
            if (!existvideo && weight_video > 0)
            {
                //如果课程没有视频，则权重分摊到试题与结课考试
                weight_ques *= 1 / (1 - weight_video);
                weight_exam *= 1 / (1 - weight_video);
                weight_video = 0;
            }
            if (!existques && weight_ques > 0)
            {
                //如果没有试是，则权重分摊到视频与结果考试
                weight_video *= 1 / (1 - weight_ques);
                weight_exam *= 1 / (1 - weight_ques);
                weight_ques = 0;
            }
            //结课考试
            TestPaper test = Business.Do<ITestPaper>().FinalPaper(course.Cou_ID, true);
            float examScore = 0;
            if (test != null)
            {
                //总分与及格分,及格转为百分制
                float total = test.Tp_Total;
                float pass = (float)test.Tp_PassScore / (float)test.Tp_Total * 100;
                //成绩转为百分制
                examScore = sc.Stc_ExamScore;
                examScore = examScore / total * 100;
                examScore = examScore >= 100 ? 100 : examScore;
                //及格分转为60分制
                examScore = examScore - pass > 0 ?
                    (examScore - pass) / (100 - pass) * 40 + 60 :
                     (examScore - pass) / pass * 60 + 60;
            } else if (weight_exam > 0)
            {
                weight_ques *= 1 / (1 - weight_exam);
                weight_video *= 1 / (1 - weight_exam);
                weight_exam = 0;
            }
            weight_video = float.IsNaN(weight_video) ? 0: weight_video;
            weight_ques = float.IsNaN(weight_ques) ? 0 : weight_ques;
            weight_exam = float.IsNaN(weight_exam) ? 0 : weight_exam;
            //开始计算综合成绩
            float score = 0, video = sc.Stc_StudyScore, ques = sc.Stc_QuesScore, exam = examScore;
            video = video > 0 ? video + (float)video_lerance : video;
            video = video >= 100 ? 100 : video;
            score = video * (float)weight_video + ques * (float)weight_ques + exam * (float)weight_exam;
            score = float.IsNaN(score) ? 0 : score;
            score = score > 0 && score >= 100 ? 100 : (float)Math.Round(score * 100) / 100;           
            sc.Stc_ResultScore = score;
            //保存结果
            Gateway.Default.Update<Student_Course>(new Field[] { Student_Course._.Stc_ResultScore }, new object[] { score }, Student_Course._.Stc_ID == sc.Stc_ID);
            return sc;
        }
        /// <summary>
        /// 计算学员的综合成绩
        /// </summary>
        /// <param name="stcid">学员选修记录的主键id</param>
        public Student_Course ResultScoreCalc(int stcid)
        {
            Student_Course sc = Gateway.Default.From<Student_Course>().Where(Student_Course._.Stc_ID == stcid).ToFirst<Student_Course>();
            return ResultScoreCalc(sc);
        }
        /// <summary>
        /// 计算学员的综合成绩
        /// </summary>
        /// <param name="stid">学员账号id</param>
        /// <returns></returns>
        public bool ResultScoreCalc4Student(int stid)
        {
            //获取学员的学习记录
            WhereClip wccalc = Student_Course._.Ac_ID == stid;
            wccalc.And(Student_Course._.Stc_StudyScore > 0 || Student_Course._.Stc_QuesScore > 0 || Student_Course._.Stc_ExamScore > 0);
            List<Student_Course> list = Gateway.Default.From<Student_Course>().Where(wccalc).ToList<Student_Course>();
            //循环计算
            foreach (Student_Course stc in list) this.ResultScoreCalc(stc);
            return true;
        }
        /// <summary>
        /// 计算选修课程学员的综合成绩
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public bool ResultScoreCalc4Course(long couid)
        {
            //获取学习记录
            WhereClip wccalc = Student_Course._.Cou_ID == couid;
            wccalc.And(Student_Course._.Stc_StudyScore > 0 || Student_Course._.Stc_QuesScore > 0 || Student_Course._.Stc_ExamScore > 0);
            List<Student_Course> list = Gateway.Default.From<Student_Course>().Where(wccalc).ToList<Student_Course>();
            //循环计算
            foreach (Student_Course stc in list) this.ResultScoreCalc(stc);
            return true;
        }

        /// <summary>
        /// 批量计算学习关联课程的综合成绩，只有学员参与学习了才会有成绩
        /// </summary>
        /// <param name="lcsid">学习卡设置的id</param>
        /// <returns></returns>
        public bool ResultScoreCalc4LearningCard(int lcsid)
        {
            WhereClip wccalc = LearningCard._.Lcs_ID == lcsid;
            wccalc.And(Student_Course._.Stc_StudyScore > 0 || Student_Course._.Stc_QuesScore > 0 || Student_Course._.Stc_ExamScore > 0);
            List<Student_Course> list = Gateway.Default.From<Student_Course>()
                .InnerJoin<LearningCard>(Student_Course._.Lc_Code == LearningCard._.Lc_Code)
                .ToList<Student_Course>();
            //循环计算
            foreach (Student_Course stc in list) this.ResultScoreCalc(stc);
            return true;
        }
        #endregion

        #region 价格管理
        /// <summary>
        /// 添加价格记录
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PriceAdd(CoursePrice entity)
        {
            //验证价格设置项所属的课程是否存在
            Course cou=Gateway.Default.From<Course>().Where(Course._.Cou_UID == entity.Cou_UID).ToFirst<Course>();
            if (cou == null) throw new Exception("价格设置项所属的课程不存在");
            entity.Cou_ID = cou.Cou_ID;

            object obj = Gateway.Default.Max<CoursePrice>(CoursePrice._.CP_Tax, CoursePrice._.Cou_UID == entity.Cou_UID);
            entity.CP_Tax = obj is int ? (int)obj + 1 : 0;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            //校验是否已经存在,同一个时间单位，只准设置一个
            CoursePrice cp = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.Cou_UID == entity.Cou_UID &&
                CoursePrice._.CP_Span == entity.CP_Span && CoursePrice._.CP_Unit == entity.CP_Unit).ToFirst<CoursePrice>();
            if (cp != null) throw new Exception(string.Format("{0}{1}的价格已经设置过，请修改之前设置", entity.CP_Span, entity.CP_Unit));
            //抵用券不得大于价格
            entity.CP_Coupon = entity.CP_Coupon > entity.CP_Price ? entity.CP_Price : entity.CP_Coupon;

            Gateway.Default.Save<CoursePrice>(entity);
            PriceSetCourse(entity.Cou_UID);
        }
        /// <summary>
        ///  将产品价格写入到课程所在的表，取第一条价格
        /// </summary>
        /// <param name="uid">课程UID</param>
        public void PriceSetCourse(string uid) {

            Song.Entities.Course course = Gateway.Default.From<Course>().Where(Course._.Cou_UID == uid).ToFirst<Course>();
            if (course == null) return;

            CoursePrice[] prices = PriceCount(0, uid, true, 0);
            if (prices == null || prices.Length == 0)
            {
                course.Cou_Price = course.Cou_PriceSpan = 0;
                course.Cou_PriceUnit = course.Cou_Prices = string.Empty;
            } else
            {
                //记录第一条价格
                CoursePrice cp = prices[0];
                course.Cou_Price = cp.CP_Price;
                course.Cou_PriceSpan = cp.CP_Span;
                course.Cou_PriceUnit = cp.CP_Unit;
                //记录所有价格
                JArray jarr=new JArray();
                foreach (CoursePrice p in prices)
                {
                    JObject jo=new JObject();
                    jo.Add("CP_Price", p.CP_Price);
                    jo.Add("CP_Span", p.CP_Span);
                    jo.Add("CP_Unit", p.CP_Unit);
                    jo.Add("CP_Coupon", p.CP_Coupon);
                    jo.Add("CP_ID", p.CP_ID);
                    jarr.Add(jo);
                }
                course.Cou_Prices = jarr.ToString();
            }
            Gateway.Default.Save<Course>(course);
        }
        /// <summary>
        /// 修改价格记录
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PriceSave(CoursePrice entity)
        {
            //校验是否已经存在,同一个时间单位，只准设置一个
            CoursePrice cp = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.Cou_UID == entity.Cou_UID && CoursePrice._.CP_ID != entity.CP_ID &&
                CoursePrice._.CP_Span == entity.CP_Span && CoursePrice._.CP_Unit == entity.CP_Unit).ToFirst<CoursePrice>();
            if (cp != null) throw new Exception(string.Format("{0}{1}的价格已经设置过，请修改之前设置", entity.CP_Span, entity.CP_Unit));
            //抵用券不得大于价格
            entity.CP_Coupon = entity.CP_Coupon > entity.CP_Price ? entity.CP_Price : entity.CP_Coupon;

            Gateway.Default.Save<CoursePrice>(entity);
            PriceSetCourse(entity.Cou_UID);
        }
        /// <summary>
        /// 删除价格记录
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PriceDelete(CoursePrice entity)
        {
            Gateway.Default.Delete<CoursePrice>(entity);
            PriceSetCourse(entity.Cou_UID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void PriceDelete(int identify)
        {
            CoursePrice p = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.CP_ID == identify).ToFirst<CoursePrice>();
            if (p != null) PriceDelete(p);
        }
        /// <summary>
        /// 删除，按全局唯一标识
        /// </summary>
        /// <param name="uid"></param>
        public void PriceDelete(string uid)
        {
            Gateway.Default.Delete<CoursePrice>(CoursePrice._.Cou_UID == uid);
            PriceSetCourse(uid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public CoursePrice PriceSingle(int identify)
        {
            return Gateway.Default.From<CoursePrice>().Where(CoursePrice._.CP_ID == identify).ToFirst<CoursePrice>();
        }
        /// <summary>
        /// 获取价格记录
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="uid"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public CoursePrice[] PriceCount(long couid, string uid, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc &= CoursePrice._.CP_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(uid))
            {
                wc &= CoursePrice._.Cou_UID == uid;
            }
            else
            {                
                wc &= CoursePrice._.Cou_ID == couid;
            }
            return Gateway.Default.From<CoursePrice>().Where(wc).OrderBy(CoursePrice._.CP_Tax.Asc).ToArray<CoursePrice>(count);
        }
        public bool PriceUpdateTaxis(Song.Entities.CoursePrice[] items)
        {
            if (items == null || items.Length < 1) return false;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    string uid = string.Empty;
                    foreach (CoursePrice item in items)
                    {
                        uid = item.Cou_UID;
                        tran.Update<CoursePrice>(
                            new Field[] { CoursePrice._.CP_Tax },
                            new object[] { item.CP_Tax },
                            CoursePrice._.CP_ID == item.CP_ID);
                    }
                    //第一条记录，同步到课程信息中
                    CoursePrice first =null;
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].CP_IsUse)
                        {
                            first = items[i];
                            break;
                        }
                    }
                    if (first != null)
                    {
                        tran.Update<Course>(
                               new Field[] { Course._.Cou_Price, Course._.Cou_PriceSpan, Course._.Cou_PriceUnit },
                               new object[] { first.CP_Price, first.CP_Span, first.CP_Unit },
                               Course._.Cou_UID == first.Cou_UID);
                    }
                    tran.Commit();
                    if(!string.IsNullOrWhiteSpace(uid))
                        PriceSetCourse(uid);
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

        #region 课程学习记录
        /// <summary>
        /// 分页获取当前课程的学员（即学习该课程的学员），并计算出完成度
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stsid"></param>
        /// <param name="acc">学员账号或姓名</param>
        /// <param name="name">学员的姓名</param>
        /// <param name="idcard"></param>
        /// <param name="mobi"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public DataTable StudentLogPager(long couid, long stsid, string acc, string name, string idcard, string mobi,
            DateTime? start, DateTime? end, int size, int index, out int countSum)
        {
            object[] parameters = new object[] { couid, stsid, acc, name, idcard,mobi,
                start,end,size, index, 0 };
            DataTable dt = DataQuery.DbQuery.Call<DataTable>(parameters);
            countSum = (int)parameters[parameters.Length - 1];
            //将成绩得分截为最大小数点后2位
            string[] scores = new string[] { "Stc_StudyScore", "Stc_QuesScore", "Stc_ExamScore", "Stc_ResultScore" };
            foreach (DataRow row in dt.Rows)
            {
                foreach (string col in scores)
                {
                    double score = Convert.ToDouble(row[col].ToString());
                    row[col] = Math.Round(score, 2);
                }
            }
            return dt;
        }
        /// <summary>
        /// 当前课程的学员（即学习该课程的学员），并计算出完成度,导出为excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        public string StudentLogToExcel(string path, Course course,DateTime? start, DateTime? end)
        {

            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Core.App.Get["ExcelInputConfig"].VirtualPath + "课程的学习成果.xml";
            xmldoc.Load(WeiSha.Core.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");

            //创建工作簿，每个工作簿多少条
            int size = 100000, index = 1;

            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;

            int total = 0, totalPage = 0;
            do
            {
                DataTable dt = this.StudentLogPager(course.Cou_ID, -1,null, null, null, null, start, end, size, index, out total);
                if (total < 1)               
                    throw new Exception("未获取到选修该课程的学员信息"); 
                totalPage = (total + size - 1) / size;
                ISheet sheet = _studentToExcel_CreateSheet(hssfworkbook, nodes, index);
                //遍历行               
                for (int r = 0; r < dt.Rows.Count; r++)
                {
                    IRow row = sheet.CreateRow(r + 1);
                    DataRow dr = dt.Rows[r];
                    //遍历列
                    for (int c = 0; c < dt.Columns.Count; c++)
                    {
                        //遍历配置项
                        for (int n = 0; n < nodes.Count; n++)
                        {
                            string field = nodes[n].Attributes["Field"].Value;
                            if (dt.Columns[c].ColumnName.Equals(field))
                            {
                                object obj = dr[c];
                                bool isnull = obj == null || obj.GetType().FullName == "System.DBNull" || obj is DBNull;
                                if (isnull) continue;
                                string format = nodes[n].Attributes["Format"] != null ? nodes[n].Attributes["Format"].Value : "";
                                string datatype = nodes[n].Attributes["DataType"] != null ? nodes[n].Attributes["DataType"].Value : "";
                                string defvalue = nodes[n].Attributes["DefautValue"] != null ? nodes[n].Attributes["DefautValue"].Value : "";
                                string value = "";
                                switch (datatype)
                                {
                                    case "date":
                                        DateTime tm = isnull ? DateTime.MinValue : Convert.ToDateTime(obj);
                                        value = tm > DateTime.Now.AddYears(-100) ? tm.ToString(format) : "";
                                        break;
                                    default:
                                        value = isnull ? string.Empty : obj.ToString();
                                        break;
                                }
                                if (defvalue.Trim() != "")
                                {
                                    foreach (string s in defvalue.Split('|'))
                                    {
                                        string h = s.Substring(0, s.IndexOf("="));
                                        string f = s.Substring(s.LastIndexOf("=") + 1);
                                        if (value.ToLower() == h.ToLower()) value = f;
                                    }
                                }
                                row.CreateCell(n).SetCellValue(value);

                            }
                        }
                    }                                   
                }
                index++;
            } while (index <= totalPage);

            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }
        //**临时方法
        //获取考试信息，因为查询量大，把考试信息放到缓存中读取
        private Song.Entities.Examination _getCahceExam(int examid)
        {
            string cachekey = "Temporary_Examination_List";
            List<Song.Entities.Examination> list = null;
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;
            
            object cachevalue = cache.Get(cachekey);
            if (cachevalue != null)
            {
                list = cachevalue as List<Song.Entities.Examination>;
            }
            else
            {
                list = Gateway.Default.From<Examination>().ToList<Examination>();
                //缓存两天失效
                cache.Insert(cachekey, list, null, DateTime.MaxValue, TimeSpan.FromSeconds(60 * 60 * 24 * 2));
            }
            //查询
            Examination exam = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Exam_ID == examid)
                {
                    exam = list[i];
                    break;
                }
            }
            return exam;
        }
        /// <summary>
        /// 生成表头
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="nodes"></param>
        /// <param name="index"></param>
        /// <returns>当前索引起始</returns>
        private ISheet _studentToExcel_CreateSheet(HSSFWorkbook hssfworkbook, XmlNodeList nodes,int index)
        {
            //创建工作簿对象       
            ISheet sheet = hssfworkbook.CreateSheet(string.Format("{0:00}",index));
            //创建数据行对象，第一行
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);          
            return sheet;
        }
        #endregion

        #region 统计信息
        /// <summary>
        /// 视频文件的存储大小
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isreal">是否真实大小，如果为true，则去硬盘验证是否存在该视频，并以物理文件大小计算文件大小；如果为false则以数据库记录的文件大小计算</param>
        /// <param name="count">视频个数</param>
        /// <returns>视频文件总大小，单位为字节</returns>
        public long StorageVideo(int orgid, bool isreal, out int count)
        {
            string PathKey = "CourseVideo";
            string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
            //视频文件总大小
            long totalSize = 0;
            int totalCount = 0; //视频数量
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accessory._.Org_ID == orgid);
            wc.And(Accessory._.As_Type == "CourseVideo");
            wc.And(Accessory._.As_IsOther == false);
            wc.And(Accessory._.As_IsOuter == false);
            using (SourceReader reader = Gateway.Default.From<Accessory>().Where(wc).ToReader())
            {
                while (reader.Read())
                {
                    string filename = reader["As_FileName"] != null ? reader["As_FileName"].ToString() : string.Empty;
                    if (string.IsNullOrWhiteSpace(filename)) continue;
                    long size = (long)reader["As_Size"];
                    if (isreal)
                    {
                        string file = PhyPath + filename;
                        if (!File.Exists(file)) continue;
                        totalSize += new FileInfo(file).Length;
                        totalCount++;
                    }
                    else
                    {
                        totalSize += size;
                        totalCount++;
                    }                    
                }
                reader.Close();
                reader.Dispose();

            }
            count = totalCount; //视频总数
            return totalSize;
        }
        /// <summary>
        /// 课程图文资源存储大小，不包括视频
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isreal">是否真实大小，如果为true，则去硬盘验证是否存在该视频，并以物理文件大小计算文件大小；如果为false则以数据库记录的文件大小计算</param>
        /// <param name="count">资源个数</param>
        /// <returns>资源文件总大小，单位为字节</returns>
        public long StorageResources(int orgid, bool isreal, out int count)
        {
            string PathKey = "Course";
            string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;
            //文件总大小
            long totalSize = 0;
            int totalCount = 0; //数量
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Accessory._.Org_ID == orgid);
            wc.And(Accessory._.As_Type == "Course");
            using (SourceReader reader = Gateway.Default.From<Accessory>().Where(wc).ToReader())
            {
                while (reader.Read())
                {
                    string filename = reader["As_FileName"] != null ? reader["As_FileName"].ToString() : string.Empty;
                    if (string.IsNullOrWhiteSpace(filename)) continue;
                    long size = (long)reader["As_Size"];
                    if (isreal)
                    {
                        string file = PhyPath + filename;
                        if (!File.Exists(file)) continue;
                        totalSize += new FileInfo(file).Length;
                        totalCount++;
                    }
                    else
                    {
                        totalSize += size;
                        totalCount++;
                    }
                }
                reader.Close();
                reader.Dispose();

            }
            count = totalCount; //总数
            return totalSize;
        }
        #endregion
    }
}
