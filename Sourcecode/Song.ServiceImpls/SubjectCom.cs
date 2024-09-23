using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Collections;
using System.Threading.Tasks;

namespace Song.ServiceImpls
{
    public class SubjectCom : ISubject
    {
        public int SubjectAdd(Subject entity)
        {
            if (entity.Sbj_ID <= 0) entity.Sbj_ID = WeiSha.Core.Request.SnowID();
            entity.Sbj_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //如果没有排序号，则自动计算
            if (entity.Sbj_Tax < 1)
            {
                object obj = Gateway.Default.Max<Subject>(Subject._.Sbj_Tax, Subject._.Org_ID == org.Org_ID && Subject._.Sbj_PID == entity.Sbj_PID);
                entity.Sbj_Tax = obj is int ? (int)obj + 1 : 0;
            }
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            entity.Sbj_Level = _ClacLevel(entity);
            entity.Sbj_XPath = _ClacXPath(entity);
            //如果图片带有多余路径，只保留文件名
            if (!string.IsNullOrWhiteSpace(entity.Sbj_Logo) && entity.Sbj_Logo.IndexOf("/") > -1)
                entity.Sbj_Logo = entity.Sbj_Logo.Substring(entity.Sbj_Logo.LastIndexOf("/") + 1);
            if (!string.IsNullOrWhiteSpace(entity.Sbj_LogoSmall) && entity.Sbj_LogoSmall.IndexOf("/") > -1)
                entity.Sbj_LogoSmall = entity.Sbj_LogoSmall.Substring(entity.Sbj_LogoSmall.LastIndexOf("/") + 1);
            return Gateway.Default.Save<Subject>(entity);
        }
        /// <summary>
        /// 批量添加专业，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">专业名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        public Subject SubjectBatchAdd(int orgid, string names)
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
            Song.Entities.Subject last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.Subject current = SubjectIsExist(orgid, pid, listName[i]);
                if (current == null)
                {
                    current = new Subject();
                    current.Sbj_Name = listName[i];
                    current.Sbj_IsUse = true;
                    current.Org_ID = orgid;
                    current.Sbj_PID = pid;
                    this.SubjectAdd(current);
                }
                last = current;
                pid = current.Sbj_ID;
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
        public Subject SubjectIsExist(int orgid, long pid, string name)
        {
            WhereClip wc = Subject._.Org_ID == orgid;
            if (pid >= 0) wc &= Subject._.Sbj_PID == pid;
            return Gateway.Default.From<Subject>().Where(wc && Subject._.Sbj_Name == name.Trim()).ToFirst<Subject>();
        }
        public void SubjectSave(Subject entity)
        {
            //专业的id与pid不能相等
            if (entity.Sbj_PID == entity.Sbj_ID) throw new Exception("Subject table PID Can not be equal to ID");
            Subject old = SubjectSingle(entity.Sbj_ID);
            if (old.Sbj_PID != entity.Sbj_PID)
            {
                object obj = Gateway.Default.Max<Subject>(Subject._.Sbj_Tax, Subject._.Org_ID == entity.Org_ID && Subject._.Sbj_PID == entity.Sbj_PID);
                entity.Sbj_Tax = obj is int ? (int)obj + 1 : 0;
            }
            entity.Sbj_Level = _ClacLevel(entity);
            entity.Sbj_XPath = _ClacXPath(entity);
            if (entity.Dep_Id > 0)
            {
                Song.Entities.Depart depart = Gateway.Default.From<Depart>().Where(Depart._.Dep_Id == entity.Dep_Id).ToFirst<Depart>();
                if (depart != null)
                {
                    entity.Dep_CnName = depart.Dep_CnName;
                }
            }
            //如果图片带有多余路径，只保留文件名
            if (!string.IsNullOrWhiteSpace(entity.Sbj_Logo) && entity.Sbj_Logo.IndexOf("/") > -1)
                entity.Sbj_Logo = entity.Sbj_Logo.Substring(entity.Sbj_Logo.LastIndexOf("/") + 1);
            if (!string.IsNullOrWhiteSpace(entity.Sbj_LogoSmall) && entity.Sbj_LogoSmall.IndexOf("/") > -1)
                entity.Sbj_LogoSmall = entity.Sbj_LogoSmall.Substring(entity.Sbj_LogoSmall.LastIndexOf("/") + 1);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Subject>(entity);
                    tran.Update<Questions>(new Field[] { Questions._.Sbj_Name }, new object[] { entity.Sbj_Name }, Questions._.Sbj_ID == entity.Sbj_ID);
                    tran.Update<Course>(new Field[] { Course._.Sbj_Name }, new object[] { entity.Sbj_Name }, Course._.Sbj_ID == entity.Sbj_ID);
                    tran.Update<TestPaper>(new Field[] { TestPaper._.Sbj_Name }, new object[] { entity.Sbj_Name }, TestPaper._.Sbj_ID == entity.Sbj_ID);
                    tran.Update<TestResults>(new Field[] { TestResults._.Sbj_Name }, new object[] { entity.Sbj_Name }, TestResults._.Sbj_ID == entity.Sbj_ID);
                    tran.Update<Examination>(new Field[] { Examination._.Sbj_Name }, new object[] { entity.Sbj_Name }, Examination._.Sbj_ID == entity.Sbj_ID);
                    tran.Update<ExamResults>(new Field[] { ExamResults._.Sbj_Name }, new object[] { entity.Sbj_Name }, ExamResults._.Sbj_ID == entity.Sbj_ID);
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
        /// 修改专业的某些项
        /// </summary>
        /// <param name="sbjid">专业id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool SubjectUpdate(long sbjid, Field[] fields, object[] objs)
        {
            Gateway.Default.Update<Subject>(fields, objs,Subject._.Sbj_ID == sbjid);
            return true;
        }
        /// <summary>
        /// 修改专业的某些项
        /// </summary>
        /// <param name="sbjid">专业id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool SubjectUpdate(long sbjid, Field field, object obj)
        {
            Gateway.Default.Update<Subject>(field, obj, Subject._.Sbj_ID == sbjid);
            return true;
        }
        public void SubjectDelete(long identify)
        {
            //是否存在试题
            int count = Gateway.Default.Count<Questions>(Questions._.Sbj_ID == identify);
            if (count > 0) throw new WeiSha.Core.ExceptionForPrompt("当前专业下包括" + count + "道试题，请清空后再删除！");
            //是否存在课程
            count = Gateway.Default.Count<Course>(Course._.Sbj_ID == identify);
            if (count > 0) throw new WeiSha.Core.ExceptionForPrompt("当前专业下包括" + count + "个课程，请清空后再删除！");
            //是否有下级
            count = Gateway.Default.Count<Subject>(Subject._.Sbj_PID == identify);
            if (count > 0) throw new WeiSha.Core.ExceptionForPrompt("当前专业下包括" + count + "个子专业，请清空后再删除！");
            if (count < 1)
            {
                this.SubjectClear(identify);
                Subject subject = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == identify).ToFirst<Subject>();
                WeiSha.Core.Upload.Get["Subject"].DeleteFile(subject.Sbj_Logo);
              
                Gateway.Default.Delete<Subject>(Subject._.Sbj_ID == identify);
                WeiSha.Core.Upload.Get["Subject"].DeleteDirectory(identify.ToString());
            }
        }

        public void SubjectClear(long identify)
        {
            Subject sbj = this.SubjectSingle(identify);
            //清理课程
            Song.Entities.Course[] cous = Gateway.Default.From<Course>().Where(Course._.Sbj_ID == identify).ToArray<Course>();
            if (cous != null && cous.Length > 0)
            {
                foreach (Song.Entities.Course c in cous)
                    Business.Do<ICourse>().CourseClear(c, false);
            }
            //清理试题
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(Questions._.Sbj_ID == identify).ToArray<Questions>();
            if (ques != null && ques.Length > 0)
            {
                foreach (Song.Entities.Questions c in ques)
                    Business.Do<IQuestions>().QuesDelete(c);
            }
            //更新题库统计
            Business.Do<IQuestions>().QuesCountUpdate(sbj.Org_ID, identify, 0, -1);
        }

        public Subject SubjectSingle(long identify)
        {
            return Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == identify).ToFirst<Subject>();
        }
        /// <summary>
        /// 当前专业下的所有子专业id
        /// </summary>
        /// <param name="sbjid">当前专业id</param>
        /// <param name="orgid">专业所属机构的ID,如果小于等于零，则取从数据库读取sbjid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        public List<long> TreeID(long sbjid, int orgid)
        {
            List<long> list = new List<long>();
            if (orgid <= 0)
            {
                Subject sbj = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == sbjid).ToFirst<Subject>();
                if (sbj == null) return list;
                orgid = sbj.Org_ID;
            }
            //取同一个机构下的所有专业
            List<Subject> sbjs = Gateway.Default.From<Subject>().Where(Subject._.Org_ID == orgid).ToList<Subject>();
            list = _treeid(sbjid, sbjs);
            return list;
        }
        private List<long> _treeid(long id, List<Subject> sbjs)
        {
            List<long> list = new List<long>();
            if (id > 0) list.Add(id);
            foreach (Subject o in sbjs)
            {
                if (o.Sbj_PID != id) continue;
                List<long> tm = _treeid(o.Sbj_ID, sbjs);
                if (tm != null) list.AddRange(tm);
            }
            return list;
        }
        /// <summary>
        /// 获取专业名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="sbjid"></param>
        /// <returns></returns>
        public string SubjectName(long sbjid)
        {
            Subject entity = null;
            string xpath = string.Empty;
            do
            {
                entity = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == sbjid)
                    .Select(new Field[] { Subject._.Sbj_ID, Subject._.Sbj_PID, Subject._.Sbj_Name }).ToFirst<Subject>();
                if (entity != null)
                {
                    if (string.IsNullOrWhiteSpace(xpath))
                        xpath = entity.Sbj_Name;
                    else
                        xpath = entity.Sbj_Name + "," + xpath;               
                    sbjid = entity.Sbj_PID;
                }
            } while (entity != null && sbjid > 0);
            return xpath;
        }
        /// <summary>
        /// 当前专业，是否有子专业
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify">当前专业Id</param>
        /// <returns></returns>
        public bool SubjectIsChildren(int orgid, long identify, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Subject>(wc && Subject._.Sbj_PID == identify);
            return count > 0;
        }
        public List<Subject> SubjectCount(int orgid, string sear, bool? isUse, long pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Subject._.Sbj_Name.Contains(sear));
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(Subject._.Sbj_Tax.Asc && Subject._.Sbj_ID.Asc).ToList<Subject>(count);
        }
        public List<Subject> SubjectCount(int orgid, string sear, bool? isUse, long pid, string order, int index, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Subject._.Sbj_Name.Contains(sear));
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "def") wcOrder = Subject._.Sbj_IsRec.Desc & Subject._.Sbj_Tax.Asc && Subject._.Sbj_ID.Asc;
            if (order == "tax") wcOrder = Subject._.Sbj_Tax.Asc && Subject._.Sbj_ID.Asc;
            if (order == "rec")
            {
                //wc &= Subject._.Sbj_IsRec == true;
                wcOrder = Subject._.Sbj_IsRec.Desc && Subject._.Sbj_Tax.Asc && Subject._.Sbj_ID.Asc;
            }
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(wcOrder).ToList<Subject>(count, index);
        }
        /// <summary>
        /// 当前专业的上级父级
        /// </summary>
        /// <param name="sbjid"></param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<Subject> Parents(long sbjid, bool isself)
        {            
            Stack st = new Stack();
            Subject s = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == sbjid).ToFirst<Subject>();
            if (isself) st.Push(s);
            while (s.Sbj_PID != 0)
            {
                s = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == s.Sbj_PID).ToFirst<Subject>();
                if (s == null) break;
                st.Push(s);
            }
            List<Subject> list = new List<Subject>();
            foreach (object obj in st)
            {
                list.Add((Subject)obj);
            }
            return list;
        }

        public List<Subject> SubjectCount(int orgid, int depid, string sear, bool? isUse, long pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (depid > 0) wc.And(Subject._.Dep_Id == depid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Subject._.Sbj_Name.Contains(sear));
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(Subject._.Sbj_Tax.Asc && Subject._.Sbj_ID.Asc).ToList<Subject>(count);
        }

        public int SubjectOfCount(int orgid, long pid, bool? isUse, bool children)
        {
            if (pid < 0)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(Subject._.Org_ID == orgid);
                if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
                return Gateway.Default.Count<Subject>(wc);
            }
            //不包括子级，仅当前专业的直接下级专业
            if (!children)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(Subject._.Org_ID == orgid);
                if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
                if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
                return Gateway.Default.Count<Subject>(wc);
            }
            else
            {
                //包括子级，当前专业下的所有专业数
                List<long> list = new List<long>();
                //取同一个机构下的所有章节
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(Subject._.Org_ID == orgid);
                if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
                List<Subject> sbjs = Gateway.Default.From<Subject>().Where(wc).ToList<Subject>();
                list = _treeid(pid, sbjs);
                return list.Count;
            }
        }
        /// <summary>
        /// 更新专业的统计数据
        /// </summary>
        /// <param name="orgid">机构id，如果大于0，则刷新当前机构下的所有专业数据</param>
        /// <param name="sbjid">专业id</param>
        /// <returns></returns>
        public bool UpdateStatisticalData(int orgid, long sbjid)
        {
            if (orgid > 0)
            {
                List<Subject> subjects = this.SubjectCount(orgid, null, null, -1, 0);
                if (subjects != null && subjects.Count > 0)
                {
                    foreach (Subject sbj in subjects)
                        this.UpdateStatisticalData(-1, sbj.Sbj_ID);
                }
            }
            else if (sbjid > 0)
            {
                //试题数，试卷数，课程数
                int ques_count = Business.Do<IQuestions>().QuesOfCount(-1, sbjid, -1, -1, 0, -1, null);
                int paper_count = Business.Do<ITestPaper>().PaperOfCount(-1, sbjid, -1, -1, null);
                int course_count = Business.Do<ICourse>().CourseOfCount(sbjid);
                Business.Do<ISubject>().SubjectUpdate(sbjid,
                    new Field[] { Subject._.Sbj_QuesCount, Subject._.Sbj_TestCount, Subject._.Sbj_CourseCount },
                    new object[] { ques_count, paper_count, course_count });
            }
            return true;
        }
        public List<Subject> SubjectPager(int orgid, long pid, bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Subject._.Org_ID == orgid);
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (string.IsNullOrWhiteSpace(searTxt)) wc.And(Subject._.Sbj_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<Subject>(wc);
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(Subject._.Sbj_Tax.Asc && Subject._.Sbj_ID.Asc).ToList<Subject>(size, (index - 1) * size);
        }

        public List<Questions> QusForSubject(int orgid, long sbjid, int qusType, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (qusType > 0) wc.And(Questions._.Qus_Type == qusType);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            count = count < 0 ? int.MaxValue : count;
            return Gateway.Default.From<Questions>().Where(wc).ToList<Questions>(count);
        }
        public int QusCountForSubject(int orgid, long identify, int qusType, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (qusType > 0) wc.And(Questions._.Qus_Type == qusType);
            if (identify > 0) wc.And(Questions._.Sbj_ID == identify);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Questions>(wc);
            return count;
        }
        /// <summary>
        /// 更改专业的排序
        /// </summary>
        /// <param name="list">专业列表，对象中只有Sbj_ID、Sbj_PID、Sbj_Tax、Sbj_Level</param>
        /// <returns></returns>
        public bool UpdateTaxis(Subject[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.Subject sbj in list)
                    {
                        tran.Update<Subject>(
                            new Field[] { Subject._.Sbj_PID, Subject._.Sbj_Tax, Subject._.Sbj_Level },
                            new object[] { sbj.Sbj_PID, sbj.Sbj_Tax, sbj.Sbj_Level }, 
                            Subject._.Sbj_ID == sbj.Sbj_ID);
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

        #region 私有方法
        /// <summary>
        /// 计算当前对象在多级分类中的层深
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int _ClacLevel(Song.Entities.Subject entity)
        {
            //if (entity.Sbj_PID == 0) return 1;
            int level = 1;
            Song.Entities.Subject tm = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == entity.Sbj_PID).ToFirst<Subject>();
            while (tm != null)
            {
                level++;
                if (tm.Sbj_PID == 0) break;
                if (tm.Sbj_PID != 0)
                {
                    tm = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == tm.Sbj_PID).ToFirst<Subject>();
                }
            }
            entity.Sbj_Level = level;
            Gateway.Default.Save<Subject>(entity);
            Song.Entities.Subject[] childs = Gateway.Default.From<Subject>().Where(Subject._.Sbj_PID == entity.Sbj_ID).ToArray<Subject>();
            foreach (Subject s in childs)
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
        private string _ClacXPath(Song.Entities.Subject entity)
        {
            //if (entity.Sbj_PID == 0) return "";
            string xpath = "";
            Song.Entities.Subject tm = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == entity.Sbj_PID).ToFirst<Subject>();
            while (tm != null)
            {
                xpath = tm.Sbj_ID + "," + xpath;
                if (tm.Sbj_PID == 0) break;
                if (tm.Sbj_PID != 0)
                {
                    tm = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == tm.Sbj_PID).ToFirst<Subject>();
                }
            }
            entity.Sbj_XPath = xpath;
            Gateway.Default.Save<Subject>(entity);
            Song.Entities.Subject[] childs = Gateway.Default.From<Subject>().Where(Subject._.Sbj_PID == entity.Sbj_ID).ToArray<Subject>();
            foreach (Subject s in childs)
            {
                _ClacXPath(s);
            }
            return xpath;
        }
        #endregion
    }
}
