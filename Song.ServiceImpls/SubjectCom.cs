using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Collections;



namespace Song.ServiceImpls
{
    public class SubjectCom : ISubject
    {
        public int SubjectAdd(Subject entity)
        {
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
            foreach (string s in names.Split(','))
                if (s.Trim() != "") listName.Add(s.Trim());
            //
            int pid = 0;
            Song.Entities.Subject last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.Subject current = SubjectIsExist(orgid, pid, listName[i]);
                if (current == null)
                {
                    current = new Subject();
                    current.Sbj_Name = listName[i].Trim();
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
        public Subject SubjectIsExist(int orgid, int pid, string name)
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
                finally
                {
                    tran.Close();
                }
            }
        }

        public void SubjectDelete(int identify)
        {
            //是否存在试题
            int count = Gateway.Default.Count<Questions>(Questions._.Sbj_ID == identify);
            if (count > 0) throw new WeiSha.Common.ExceptionForPrompt("当前专业下包括" + count + "道试题，请清空后再删除！");
            //是否存在课程
            count = Gateway.Default.Count<Course>(Course._.Sbj_ID == identify);
            if (count > 0) throw new WeiSha.Common.ExceptionForPrompt("当前专业下包括" + count + "个课程，请清空后再删除！");
            //是否有下级
            count = Gateway.Default.Count<Subject>(Subject._.Sbj_PID == identify);
            if (count > 0) throw new WeiSha.Common.ExceptionForPrompt("当前专业下包括" + count + "个子专业，请清空后再删除！");
            if (count < 1)
            {
                this.SubjectClear(identify);
                Subject subject = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == identify).ToFirst<Subject>();
                if (!string.IsNullOrWhiteSpace(subject.Sbj_Logo))
                {
                    WeiSha.WebControl.FileUpload.Delete("Subject", subject.Sbj_Logo);
                }
                Gateway.Default.Delete<Subject>(Subject._.Sbj_ID == identify);
            }
        }

        public void SubjectClear(int identify)
        {
            //清理课程
            Song.Entities.Course[] cous = Gateway.Default.From<Course>().Where(Course._.Sbj_ID == identify).ToArray<Course>();
            if (cous != null && cous.Length > 0)
            {
                foreach (Song.Entities.Course c in cous)
                    Business.Do<ICourse>().CourseClear(c.Cou_ID);
            }
            //清理试题
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(Questions._.Sbj_ID == identify).ToArray<Questions>();
            if (ques != null && ques.Length > 0)
            {
                foreach (Song.Entities.Questions c in ques)
                    Business.Do<IQuestions>().QuesDelete(c.Qus_ID);
            }
        }

        public Subject SubjectSingle(int identify)
        {
            return Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == identify).ToFirst<Subject>();
        }
        /// <summary>
        /// 当前专业下的所有子专业id
        /// </summary>
        /// <param name="sbjid">当前专业id</param>
        /// <returns></returns>
        public List<int> TreeID(int sbjid)
        {
            List<int> list = new List<int>();
            Subject sbj = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == sbjid).ToFirst<Subject>();
            if (sbj == null) return list;  
            //取同一个机构下的所有章节
            Subject[] sbjs = Gateway.Default.From<Subject>().Where(Subject._.Org_ID == sbj.Org_ID).ToArray<Subject>();
            list = _treeid(sbjid, sbjs);
            return list;
        }
        private List<int> _treeid(int id, Subject[] sbjs)
        {
            List<int> list = new List<int>();
            list.Add(id);
            foreach (Subject o in sbjs)
            {
                if (o.Sbj_PID != id) continue;
                List<int> tm = _treeid(o.Sbj_ID, sbjs);
                foreach (int t in tm)
                    list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 获取专业名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public string SubjectName(int identify)
        {
            Subject entity = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == identify).ToFirst<Subject>();
            if (entity == null) return "";
            string xpath = entity.Sbj_Name;
            Song.Entities.Subject tm = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == entity.Sbj_PID).ToFirst<Subject>();
            while (tm != null)
            {
                xpath = tm.Sbj_Name + "," + xpath;
                if (tm.Sbj_PID == 0) break;
                if (tm.Sbj_PID != 0)
                {
                    tm = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == tm.Sbj_PID).ToFirst<Subject>();
                }
            }
            return xpath;
        }
        /// <summary>
        /// 当前专业，是否有子专业
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify">当前专业Id</param>
        /// <returns></returns>
        public bool SubjectIsChildren(int orgid, int identify, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Subject>(wc && Subject._.Sbj_PID == identify);
            return count > 0;
        }
        public Subject[] SubjectCount(bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Subject._.Sbj_ID > -1);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(Subject._.Sbj_Tax.Asc).ToArray<Subject>(count);
        }

        public Subject[] SubjectCount(int orgid, string sear, bool? isUse, int pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Subject._.Sbj_Name.Like("%" + sear + "%"));
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(Subject._.Sbj_Tax.Asc).ToArray<Subject>(count);
        }
        public Subject[] SubjectCount(int orgid, string sear, bool? isUse, int pid, string order, int index, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Subject._.Sbj_Name.Like("%" + sear + "%"));
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            OrderByClip wcOrder = new OrderByClip();
            if (order == "def") wcOrder = Subject._.Sbj_IsRec.Desc & Subject._.Sbj_Tax.Asc;
            if (order == "tax") wcOrder = Subject._.Sbj_Tax.Asc;
            if (order == "rec")
            {
                //wc &= Subject._.Sbj_IsRec == true;
                wcOrder = Subject._.Sbj_IsRec.Desc && Subject._.Sbj_Tax.Asc;
            }
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(wcOrder).ToArray<Subject>(count, index);
        }
        /// <summary>
        /// 当前专业的上级父级
        /// </summary>
        /// <param name="sbjid"></param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<Subject> Parents(int sbjid, bool isself)
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

        public Subject[] SubjectCount(int orgid, int depid, string sear, bool? isUse, int pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (depid > 0) wc.And(Subject._.Dep_Id == depid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(Subject._.Sbj_Name.Like("%" + sear + "%"));
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(Subject._.Sbj_Tax.Asc).ToArray<Subject>(count);
        }

        public int SubjectOfCount(int orgid, bool? isUse, int pid)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(Subject._.Org_ID == orgid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == (bool)isUse);
            if (pid >= 0) wc.And(Subject._.Sbj_PID == pid);
            return Gateway.Default.Count<Subject>(wc);
        }
        public Subject[] SubjectPager(int orgid, int depid, bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Subject._.Org_ID == orgid);
            if (depid > 0) wc.And(Subject._.Dep_Id == depid);
            if (isUse != null) wc.And(Subject._.Sbj_IsUse == isUse);
            if (searTxt != string.Empty) wc.And(Subject._.Sbj_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Subject>(wc);
            return Gateway.Default.From<Subject>().Where(wc).OrderBy(Subject._.Sbj_Tax.Asc).ToArray<Subject>(size, (index - 1) * size);
        }

        public Questions[] QusForSubject(int orgid, int identify, int qusType, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (qusType > 0) wc.And(Questions._.Qus_Type == qusType);
            if (identify > 0) wc.And(Questions._.Sbj_ID == identify);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            count = count < 0 ? int.MaxValue : count;
            return Gateway.Default.From<Questions>().Where(wc).ToArray<Questions>(count);
        }
        public int QusCountForSubject(int orgid, int identify, int qusType, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (qusType > 0) wc.And(Questions._.Qus_Type == qusType);
            if (identify > 0) wc.And(Questions._.Sbj_ID == identify);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Questions>(wc);
            return count;
        }
        public bool RemoveUp(int id)
        {
            //当前对象
            Subject current = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == id).ToFirst<Subject>();
            //当前对象排序号
            int orderValue = (int)current.Sbj_Tax;
            //上一个对象，即兄长对象；
            Subject up = Gateway.Default.From<Subject>()
                .Where(Subject._.Sbj_Tax < orderValue && Subject._.Org_ID == current.Org_ID && Subject._.Sbj_PID == current.Sbj_PID)
                .OrderBy(Subject._.Sbj_Tax.Desc).ToFirst<Subject>();
            if (up == null) return false;
            //交换排序号
            current.Sbj_Tax = up.Sbj_Tax;
            up.Sbj_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Subject>(current);
                    tran.Save<Subject>(up);
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

        public bool RemoveDown(int id)
        {
            //当前对象
            Subject current = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == id).ToFirst<Subject>();
            //当前对象排序号
            int orderValue = (int)current.Sbj_Tax;
            //下一个对象，即弟弟对象；
            Subject down = Gateway.Default.From<Subject>()
                .Where(Subject._.Sbj_Tax > orderValue && Subject._.Org_ID == current.Org_ID && Subject._.Sbj_PID == current.Sbj_PID)
                .OrderBy(Subject._.Sbj_Tax.Asc).ToFirst<Subject>();
            if (down == null) return false;
            //交换排序号
            current.Sbj_Tax = down.Sbj_Tax;
            down.Sbj_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Subject>(current);
                    tran.Save<Subject>(down);
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
