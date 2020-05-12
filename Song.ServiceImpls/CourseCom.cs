using System;
using System.Collections.Generic;
using System.Data;
using Song.Entities;
using Song.ServiceInterfaces;
using WeiSha.Common;
using WeiSha.Data;

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
            entity.Cou_CrtTime = DateTime.Now;            
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //object obj = Gateway.Default.Max<Course>(Course._.Cou_Tax, Course._.Org_ID == entity.Org_ID && Course._.Sbj_ID == entity.Sbj_ID && Course._.Cou_PID == entity.Cou_PID);
            object obj = Gateway.Default.Max<Course>(Course._.Cou_Tax, new WhereClip());
            entity.Cou_Tax = obj is int ? (int)obj + 1 : 0;
            //默认为免费课程
            entity.Cou_IsFree = true;
            //
            if (string.IsNullOrWhiteSpace(entity.Cou_UID))
                entity.Cou_UID = WeiSha.Common.Request.UniqueID();
            entity.Cou_Level = _ClacLevel(entity);
            entity.Cou_XPath = _ClacXPath(entity);
            Gateway.Default.Save<Course>(entity);
        }
        /// <summary>
        /// 批量添加课程，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        public Course CourseBatchAdd(int orgid, int sbjid, string names)
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
            int pid = 0;
            Song.Entities.Course last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.Course current = CourseIsExist(orgid, sbjid, pid, listName[i]);
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
                    current.Cou_IsTry = true;
                    //所属老师                    
                    if (Extend.LoginState.Accounts.IsLogin)
                    {
                        Song.Entities.Teacher th = Extend.LoginState.Accounts.Teacher;
                        if (th != null)
                        {
                            current.Th_ID = th.Th_ID;
                            current.Th_Name = th.Th_Name;
                        }
                    }
                    this.CourseAdd(current);
                }
                last = current;
                pid = current.Cou_ID;
            }
            return last;
        }
        /// <summary>
        /// 是否已经存在专业
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Course CourseIsExist(int orgid, int sbjid, int pid, string name)
        {
            WhereClip wc = Course._.Org_ID == orgid;
            if (sbjid > 0) wc &= Course._.Sbj_ID == sbjid;
            if (pid >= 0) wc &= Course._.Cou_PID == pid;
            return Gateway.Default.From<Course>().Where(wc && Course._.Cou_Name == name.Trim()).ToFirst<Course>();
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
            entity.Cou_Level = _ClacLevel(entity);
            entity.Cou_XPath = _ClacXPath(entity);
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
                    }
                    tran.Save<Course>(entity);
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
        /// 是否为直播课
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        public bool IsLiveCourse(int couid)
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
        public bool IsLiveCourse(int couid, bool check)
        {
            if (!check) return this.IsLiveCourse(couid);
            Outline[] outs = Business.Do<IOutline>().OutlineCount(couid, -1, null, 0);
            bool isExist = false;
            foreach (Outline o in outs)
            {
                if (o.Ol_IsLive)
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
        /// 增加课程浏览数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public int CourseViewNum(Course entity, int num)
        {
            entity.Cou_ViewNum += num;
            Gateway.Default.Update<Course>(
                new Field[] { Course._.Cou_ViewNum },
                new object[] { entity.Cou_ViewNum++ }, Course._.Cou_ID == entity.Cou_ID);
            return entity.Cou_ViewNum;
        }
        public int CourseViewNum(int couid, int num)
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

            Song.Entities.Outline[] oul = Business.Do<IOutline>().OutlineAll(entity.Cou_ID, null);
            Song.Entities.GuideColumns[] gcs = Business.Do<IGuide>().GetColumnsAll(entity.Cou_ID, null);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    this.CourseClear(entity.Cou_ID);
                    tran.Delete<Course>(Course._.Cou_ID == entity.Cou_ID);
                    foreach (Song.Entities.Outline ac in oul)
                    {
                        Business.Do<IOutline>().OutlineDelete(ac);
                    }
                    foreach (Song.Entities.GuideColumns gc in gcs)
                    {
                        Business.Do<IGuide>().ColumnsDelete(gc);
                    }
                    tran.Delete<CoursePrice>(CoursePrice._.Cou_UID == entity.Cou_UID);
                    if (!string.IsNullOrWhiteSpace(entity.Cou_Logo))
                    {
                        WeiSha.WebControl.FileUpload.Delete("Course", entity.Cou_Logo);
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
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void CourseDelete(int identify)
        {
            Song.Entities.Course ol = this.CourseSingle(identify);
            this.CourseDelete(ol);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Course CourseSingle(int identify)
        {
            return Gateway.Default.From<Course>().Where(Course._.Cou_ID == identify).ToFirst<Course>();
        }
        /// <summary>
        /// 获取课程名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public string CourseName(int identify)
        {
            Course entity = Gateway.Default.From<Course>().Where(Course._.Cou_ID == identify).ToFirst<Course>();
            if (entity == null) return "";
            string xpath = entity.Cou_Name;
            Song.Entities.Course tm = Gateway.Default.From<Course>().Where(Course._.Cou_ID == entity.Cou_PID).ToFirst<Course>();
            while (tm != null)
            {
                xpath = tm.Cou_Name + "," + xpath;
                if (tm.Cou_PID == 0) break;
                if (tm.Cou_PID != 0)
                {
                    tm = Gateway.Default.From<Course>().Where(Course._.Cou_ID == entity.Cou_PID).ToFirst<Course>();
                }
            }
            return xpath;
        }
        /// <summary>
        /// 学员是否购买了该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员Id</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <returns></returns>
        public Course IsBuyCourse(int couid, int stid, int state)
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
        public bool IsBuy(int couid, int stid)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null || !course.Cou_IsUse) return false;
            WhereClip wc = Student_Course._.Cou_ID == couid && Student_Course._.Ac_ID == stid;
            wc.And(Student_Course._.Stc_IsTry == false && Student_Course._.Stc_IsFree == false);
            wc.And(Student_Course._.Stc_StartTime < DateTime.Now && Student_Course._.Stc_EndTime > DateTime.Now);              
            Student_Course sc = Gateway.Default.From<Student_Course>().Where(wc).ToFirst<Student_Course>();
            return sc != null;
        }
        /// <summary>
        /// 学员购买的该课程
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="sear">用于检索的字符</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <returns></returns>
        public List<Course> CourseForStudent(int stid, string sear, int state, bool? istry, int count)
        {
            if (stid <= 0) return null;
            WhereClip wc = Student_Course._.Ac_ID == stid;
            //Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //if (org != null)
            //{
            //    wc.And(Student_Course._.Org_ID == org.Org_ID);
            //}
            if (istry != null) wc.And(Student_Course._.Stc_IsTry == (bool)istry);
            if (state == 1)
            {
                WhereClip wc2 = new WhereClip();
                wc2.And(Student_Course._.Stc_StartTime < DateTime.Now && Student_Course._.Stc_EndTime > DateTime.Now);
                //wc2.Or(Student_Course._.Stc_IsFree == true);                
                wc.And(wc2);
            }
            if (state == 2)
            {
                //wc.And(Student_Course._.Stc_IsFree == false);
                wc.And(Student_Course._.Stc_EndTime < DateTime.Now);
            }
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Like("%" + sear + "%"));
            return Gateway.Default.From<Course>()
                    .InnerJoin<Student_Course>(Student_Course._.Cou_ID == Course._.Cou_ID)
                    .Where(wc).OrderBy(Student_Course._.Stc_StartTime.Desc).ToList<Course>(count);
        }
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public List<Course> CourseAll(int orgid, int sbjid, int thid, bool? isUse)
        {
            return CourseCount(orgid, sbjid, thid, -1, null, isUse, -1);
        }
        /// <summary>
        /// 某个课程的学习人数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="isAll">是否取全部值，如果为false，则仅取当前正在学习的</param>
        /// <returns></returns>
        public int CourseStudentSum(int couid, bool? isAll)
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
        /// 清除课程的内容
        /// </summary>
        /// <param name="identify"></param>
        public void CourseClear(int identify)
        {
            //删除章节
            List<Song.Entities.Outline> outline = Gateway.Default.From<Outline>().Where(Outline._.Cou_ID == identify).ToList<Outline>();
            if (outline != null && outline.Count > 0)
            {
                foreach (Song.Entities.Outline ol in outline)
                {
                    Business.Do<IOutline>().OutlineClear(ol.Ol_ID);
                    Business.Do<IOutline>().OutlineDelete(ol.Ol_ID);
                }
            }
            //删除试卷
            List<Song.Entities.TestPaper> tps = Gateway.Default.From<TestPaper>().Where(TestPaper._.Cou_ID == identify).ToList<TestPaper>();
            if (tps != null && tps.Count > 0)
            {
                foreach (Song.Entities.TestPaper t in tps)
                    Business.Do<ITestPaper>().PagerDelete(t.Tp_Id);
            }
            //考试指南
            List<Song.Entities.GuideColumns> gcs = Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Cou_ID == identify).ToList<GuideColumns>();
            if (gcs != null && gcs.Count > 0)
            {
                foreach (Song.Entities.GuideColumns t in gcs)
                    Business.Do<IGuide>().ColumnsDelete(t);
            }
            //清理试题
            List<Song.Entities.Questions> ques = Gateway.Default.From<Questions>().Where(Questions._.Cou_ID == identify).ToList<Questions>();
            if (ques != null && ques.Count > 0)
            {
                foreach (Song.Entities.Questions c in ques)
                    Business.Do<IQuestions>().QuesDelete(c.Qus_ID);
            }
        }
        public int CourseOfCount(int orgid, int sbjid, int thid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<ISubject>().TreeID(sbjid);
                foreach (int l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            return Gateway.Default.Count<Course>(wc);
        }

        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        public List<Course> CourseCount(int orgid, int sbjid, int thid, int pid, string sear, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<ISubject>().TreeID(sbjid);
                foreach (int l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (pid > 0) wc.And(Course._.Cou_ID == pid);
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            return Gateway.Default.From<Course>().Where(wc)
                .OrderBy(Course._.Cou_Tax.Desc).ToList<Course>(count);
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
        public List<Course> CourseCount(int orgid, int sbjid, string sear, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<ISubject>().TreeID(sbjid);
                foreach (int l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Like("%" + sear + "%"));
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            return Gateway.Default.From<Course>().Where(wc)
               .OrderBy(Course._.Cou_Tax.Desc).ToList<Course>(count);
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
        public List<Course> CourseCount(int orgid, int sbjid, int thid, bool? islive, string sear, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<ISubject>().TreeID(sbjid);
                foreach (int l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (islive != null) wc.And(Course._.Cou_ExistLive == (bool)islive);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Like("%" + sear + "%"));
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            return Gateway.Default.From<Course>().Where(wc)
               .OrderBy(Course._.Cou_Tax.Desc).ToList<Course>(count);
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
        public List<Course> CourseCount(int orgid, int sbjid, string sear, bool? isUse, string order, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<ISubject>().TreeID(sbjid);
                foreach (int l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Course._.Cou_Name.Like("%" + sear + "%"));
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "flux") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "def") wcOrder = Course._.Cou_IsRec.Desc & Course._.Cou_ViewNum.Asc;
            if (order == "tax") wcOrder = Course._.Cou_Tax.Desc & Course._.Cou_CrtTime.Desc;
            if (order == "new") wcOrder = Course._.Cou_CrtTime.Desc;    //最新发布
            if (order == "rec") wcOrder = Course._.Cou_IsRec.Desc & Course._.Cou_Tax.Desc & Course._.Cou_CrtTime.Desc;
            return Gateway.Default.From<Course>().Where(wc)
               .OrderBy(wcOrder).ToList<Course>(count);
        }
        public bool CourseIsChildren(int orgid, int couid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Course>(wc && Course._.Cou_PID == couid);
            return count > 0;
        }
        /// <summary>
        /// 分页取课程信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> CoursePager(int orgid, int thid, bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Course._.Org_ID == orgid);
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(Course._.Cou_Name.Like("%" + searTxt + "%"));
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            countSum = Gateway.Default.Count<Course>(wc);
            return Gateway.Default.From<Course>().Where(wc).OrderBy(Course._.Cou_Tax.Desc).ToList<Course>(size, (index - 1) * size);
            //如果是采用多个教师对应一个课程，用下面的方法
            //if (thid < 1)
            //{
            //    countSum = Gateway.Default.Count<Course>(wc);
            //    return Gateway.Default.From<Course>().Where(wc).OrderBy(Course._.Cou_Tax.Desc).ToList<Course>(size, (index - 1) * size);
            //}
            //else
            //{
            //    countSum = Gateway.Default.From<Course>()
            //        .InnerJoin<Teacher_Course>(Teacher_Course._.Cou_ID == Course._.Cou_ID)
            //        .Where(Teacher_Course._.Th_ID == thid)
            //        .OrderBy(Course._.Cou_Tax.Desc).Count();
            //    return Gateway.Default.From<Course>()
            //        .InnerJoin<Teacher_Course>(Teacher_Course._.Cou_ID == Course._.Cou_ID)
            //        .Where(Teacher_Course._.Th_ID == thid)
            //        .OrderBy(Course._.Cou_Tax.Desc).ToList<Course>(size, (index - 1) * size);
            //}
        }
        public List<Course> CoursePager(int orgid, int sbjid, int thid, bool? isUse, string searTxt, string order, int size, int index, out int countSum)
        {
            WhereClip wc = Course._.Org_ID == orgid;
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<ISubject>().TreeID(sbjid);
                foreach (int l in list)
                    wcSbjid.Or(Course._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (thid > 0) wc.And(Course._.Th_ID == thid);
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(Course._.Cou_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Course>(wc);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "flux") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "def") wcOrder = Course._.Cou_IsRec.Desc && Course._.Cou_ViewNum.Asc;
            if (order == "tax") wcOrder = Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            if (order == "new") wcOrder = Course._.Cou_CrtTime.Desc;    //最新发布
            if (order == "rec") wcOrder = Course._.Cou_IsRec.Desc && Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            return Gateway.Default.From<Course>().Where(wc).OrderBy(wcOrder).ToList<Course>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid">专业id,多个id用逗号分隔</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> CoursePager(int orgid, string sbjid, bool? isUse, string searTxt, string order, int size, int index, out int countSum)
        {
            return this.CoursePager(orgid, sbjid, isUse, null, searTxt, order, size, index, out countSum);
        }
 
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="isUse"></param>
        /// <param name="isLive">是否是直播课</param>
        /// <param name="searTxt"></param>
        /// <param name="order"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Course> CoursePager(int orgid, string sbjid, bool? isUse, bool? isLive, string searTxt, string order, int size, int index, out int countSum)
        {
            WhereClip wc = Course._.Org_ID == orgid;
            if (!string.IsNullOrWhiteSpace(sbjid))
            {
                WhereClip wcSbjid = new WhereClip();
                foreach (string tm in sbjid.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(tm)) continue;
                    int sbj = 0;
                    int.TryParse(tm, out sbj);
                    if (sbj == 0) continue;
                    wcSbjid.Or(Course._.Sbj_ID == sbj);
                    //当前专业的下级专业也包括
                    List<int> list = Business.Do<ISubject>().TreeID(sbj);
                    foreach (int l in list)
                        wcSbjid.Or(Course._.Sbj_ID == l);
                }
                wc.And(wcSbjid);
            }
            if (isUse != null) wc.And(Course._.Cou_IsUse == (bool)isUse);
            if (order == "live") isLive = true;
            if (isLive != null) wc.And(Course._.Cou_ExistLive == (bool)isLive);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(Course._.Cou_Name.Like("%" + searTxt.Trim() + "%"));
            countSum = Gateway.Default.Count<Course>(wc);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "flux") wcOrder = Course._.Cou_ViewNum.Desc;
            if (order == "def") wcOrder = Course._.Cou_IsRec.Desc && Course._.Cou_ViewNum.Asc;
            if (order == "tax") wcOrder = Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            if (order == "new") wcOrder = Course._.Cou_CrtTime.Desc;    //最新发布
            if (order == "rec") wcOrder = Course._.Cou_IsRec.Desc && Course._.Cou_Tax.Desc && Course._.Cou_CrtTime.Desc;
            if (order == "free")
            {
                wc.And(Course._.Cou_IsFree == true);
                wcOrder = Course._.Cou_IsFree.Desc & Course._.Cou_Tax.Desc;
            }
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
        /// 计算当前对象在多级分类中的层深
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int _ClacLevel(Song.Entities.Course entity)
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
                _ClacLevel(s);
            }
            return level;
        }
        /// <summary>
        /// 计算当前对象在多级分类中的路径
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string _ClacXPath(Song.Entities.Course entity)
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
                _ClacXPath(s);
            }
            return xpath;
        }
        #endregion

        #endregion

        #region 课程关联管理（与学生或教师）
        /// <summary>
        /// 获取选学人数最多的课程列表，从多到少
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public DataSet CourseHot(int orgid, int sbjid, int count)
        {
           
            string sql = @"select top {count} ISNULL(b.count,0) as 'count', c.* from course as c left join 
                            (SELECT cou_id, count(cou_id) as 'count'
                              FROM [Student_Course]  group by cou_id ) as b
                              on c.cou_id=b.cou_id where org_id={orgid} and {sbjid} order by [count] desc";
            count = count <= 0 ? int.MaxValue : count;
            sql = sql.Replace("{count}", count.ToString());
            sql = sql.Replace("{orgid}", orgid.ToString());
            //按专业选取（包括专业的下级专业）
            string sbjWhere = string.Empty;
            if (sbjid > 0)
            {
                List<int> sbjids = Business.Do<ISubject>().TreeID(sbjid);
                
                for (int i = 0; i < sbjids.Count; i++)
                {
                    sbjWhere += "sbj_id=" + sbjids[i] + " ";
                    if (i < sbjids.Count - 1) sbjWhere += " or ";
                }                
            }
            sql = sql.Replace("{sbjid}", sbjid > 0 ? "(" + sbjWhere + ")" : "1=1");
            
            return Gateway.Default.FromSql(sql).ToDataSet();
        }
        /// <summary>
        /// 某个学生是否正在学习某个课程
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public bool StudyIsCourse(int stid, int couid)
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
        public Student_Course StudentCourse(int stid, int couid)
        {
            return Gateway.Default.From<Student_Course>().Where(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid)
                .ToFirst<Student_Course>();
        }
        /// <summary>
        /// 课程学习
        /// </summary>
        /// <param name="stid">学生Id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public void CourseBuy(int stid, int couid, float money, DateTime startTime, DateTime endTime)
        {
            Song.Entities.Student_Course sc = new Student_Course();
            sc.Ac_ID = stid;
            sc.Cou_ID = couid;
            sc.Stc_Money = money;
            sc.Stc_StartTime = startTime;
            sc.Stc_EndTime = endTime;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            sc.Org_ID = org.Org_ID;
            sc.Stc_CrtTime = DateTime.Now;
            Gateway.Default.Save<Student_Course>(sc);
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
        public Student_Course Buy(int stid, int couid, Song.Entities.CoursePrice price)
        {
            Course course = Gateway.Default.From<Course>().Where(Course._.Cou_ID == couid).ToFirst<Course>();
            if (course == null) throw new Exception("要购买的课程不存在！");            
            Accounts st = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == stid).ToFirst<Accounts>();
            if (st == null) throw new Exception("当前学员不存在！");
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
            ma.Ac_ID = ca.Ac_ID = stid;
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
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null)
            {
                sc = new Entities.Student_Course();
                sc.Stc_CrtTime = DateTime.Now;
            }
            sc.Cou_ID = couid;
            sc.Ac_ID = stid;
            sc.Stc_Money = price.CP_Price;
            sc.Stc_StartTime = DateTime.Now;
            sc.Stc_EndTime = end;
            sc.Stc_IsFree = false;
            sc.Stc_IsTry = false;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            sc.Org_ID = org.Org_ID;
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
        public Student_Course FreeStudy(int stid, int couid)
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
        public Student_Course FreeStudy(int stid, int couid, DateTime? start, DateTime end)
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
            sc.Stc_EndTime = end;
            sc.Stc_IsFree = true;
            sc.Stc_IsTry = false;
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
        public Student_Course Tryout(int stid, int couid)
        {
            //生成学员与课程的关联
            Song.Entities.Student_Course sc = Business.Do<ICourse>().StudentCourse(stid, couid);
            if (sc == null) sc = new Entities.Student_Course();
            sc.Cou_ID = couid;
            sc.Ac_ID = stid;
            sc.Stc_StartTime = DateTime.Now;
            sc.Stc_EndTime = DateTime.Now.AddYears(100);
            sc.Stc_IsFree = false;
            sc.Stc_IsTry = true;
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
        public bool IsTryout(int couid, int stid)
        {
            WhereClip wc = Student_Course._.Cou_ID == couid && Student_Course._.Ac_ID == stid;
            wc &= Student_Course._.Stc_IsTry == true;
            Student_Course sc = Gateway.Default.From<Student_Course>().Where(wc).ToFirst<Student_Course>();
            return sc != null;
        }
        /// <summary>
        /// 直接学习该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        /// <returns>如果是免费或限时免费、或试学的课程，可以学习并返回true，不可学习返回false</returns>
        public bool Study(int couid, int stid)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            if (course == null || !course.Cou_IsUse) return false;
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
                bool isbuy = this.IsBuy(couid, stid);
                if (isbuy) return true;
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
        public void DelteCourseBuy(int stid, int couid)
        {
            //Gateway.Default.Delete<Student_Course>(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Student_Course>(Student_Course._.Ac_ID == stid && Student_Course._.Cou_ID == couid);
                    tran.Update<Accounts>(new Field[] { Accounts._.Ac_CurrCourse }, new object[] { -1 }, Accounts._.Ac_ID == stid && Accounts._.Ac_CurrCourse == couid);
                    tran.Commit();
                    Extend.LoginState.Accounts.Refresh(stid);
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
        public Accounts[] Student4Course(int couid, string stname, string stmobi, int size, int index, out int countSum)
        {
            WhereClip wc = Student_Course._.Cou_ID == couid;

            if (!string.IsNullOrWhiteSpace(stname) && stname.Trim() != "") wc.And(Accounts._.Ac_Name.Like("%" + stname + "%"));
            if (!string.IsNullOrWhiteSpace(stmobi) && stmobi.Trim() != "")
            {
                WhereClip wcOr = new WhereClip();              
                wcOr.Or(Accounts._.Ac_MobiTel1.Like("%" + stmobi + "%"));
                wcOr.Or(Accounts._.Ac_MobiTel2.Like("%" + stmobi + "%"));
                wc.And(wcOr);
            }            
            countSum = Gateway.Default.From<Accounts>().InnerJoin<Student_Course>(Student_Course._.Ac_ID == Accounts._.Ac_ID)
                   .Where(wc).OrderBy(Accounts._.Ac_LastTime.Desc).Count();

            return Gateway.Default.From<Accounts>()
                   .InnerJoin<Student_Course>(Student_Course._.Ac_ID == Accounts._.Ac_ID)
                   .Where(wc).OrderBy(Accounts._.Ac_LastTime.Desc).ToArray<Accounts>(size, (index - 1) * size);

        }
        #endregion

        #region 价格管理
        /// <summary>
        /// 添加价格记录
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PriceAdd(CoursePrice entity)
        {
            object obj = Gateway.Default.Max<CoursePrice>(CoursePrice._.CP_Tax, CoursePrice._.Cou_UID == entity.Cou_UID);
            entity.CP_Tax = obj is int ? (int)obj + 1 : 0;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            //校验是否已经存在,同一个时间单位，只准设置一个
            CoursePrice cp = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.Cou_UID == entity.Cou_UID &&
                CoursePrice._.CP_Span == entity.CP_Span && CoursePrice._.CP_Unit == entity.CP_Unit).ToFirst<CoursePrice>();
            if (cp != null) throw new Exception(string.Format("{0}{1}的价格已经设置过，请修改之前设置", entity.CP_Span, entity.CP_Unit));            
            Gateway.Default.Save<CoursePrice>(entity);
            PriceSetCourse(entity.Cou_UID);
        }
        /// <summary>
        ///  将产品价格写入到课程所在的表，取第一条价格
        /// </summary>
        /// <param name="uid">课程UID</param>
        public void PriceSetCourse(string uid){           
            CoursePrice[] prices = PriceCount(0, uid, true, 0);
            if (prices.Length > 0)
            {
                CoursePrice p = prices[0];
                Song.Entities.Course course = Gateway.Default.From<Course>().Where(Course._.Cou_UID == uid).ToFirst<Course>();
                if (course != null)
                {
                    course.Cou_Price = p.CP_Price;
                    course.Cou_PriceSpan = p.CP_Span;
                    course.Cou_PriceUnit = p.CP_Unit;
                    Gateway.Default.Save<Course>(course);
                }
            }
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
        public CoursePrice[] PriceCount(int couid, string uid, bool? isUse, int count)
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
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool PriceUp(int id)
        {
            //当前对象
            CoursePrice current = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.CP_ID == id).ToFirst<CoursePrice>();
            int tax = (int)current.CP_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            CoursePrice prev = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.CP_Tax < tax && CoursePrice._.Cou_UID == current.Cou_UID)
                .OrderBy(CoursePrice._.CP_Tax.Desc).ToFirst<CoursePrice>();
            if (prev == null) return false;
            //交换排序号
            current.CP_Tax = prev.CP_Tax;
            prev.CP_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<CoursePrice>(current);
                    tran.Save<CoursePrice>(prev);
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
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool PriceDown(int id)
        {
            //当前对象
            CoursePrice current = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.CP_ID == id).ToFirst<CoursePrice>();
            int tax = (int)current.CP_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            CoursePrice next = Gateway.Default.From<CoursePrice>().Where(CoursePrice._.CP_Tax > tax && CoursePrice._.Cou_UID == current.Cou_UID)
                .OrderBy(CoursePrice._.CP_Tax.Asc).ToFirst<CoursePrice>();
            if (next == null) return false;
            //交换排序号
            current.CP_Tax = next.CP_Tax;
            next.CP_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<CoursePrice>(current);
                    tran.Save<CoursePrice>(next);
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
