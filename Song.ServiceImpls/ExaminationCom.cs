using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml;



namespace Song.ServiceImpls
{
    public class ExaminationCom : IExamination
    {

        public int ExamAdd(Examination entity)
        {
            entity.Exam_CrtTime = DateTime.Now;
            //当前考试的创建人
            entity.Th_ID = Extend.LoginState.Accounts.Teacher.Th_ID;
            entity.Th_Name = Extend.LoginState.Accounts.Teacher.Th_Name;
            //
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Examination>(entity);
        }

        public void ExamAdd(Examination theme, List<Examination> items, List<ExamGroup> groups)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                theme.Org_ID = org.Org_ID;
                theme.Org_Name = org.Org_Name;
            }
            theme.Exam_CrtTime = DateTime.Now;
            //当前考试的创建人
            if (Extend.LoginState.Accounts.Teacher != null)
            {
                theme.Th_ID = Extend.LoginState.Accounts.Teacher.Th_ID;
                theme.Th_Name = Extend.LoginState.Accounts.Teacher.Th_Name;
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {                   
                    if (items != null)
                    {
                        //考试主题时间
                        DateTime examDate = DateTime.MaxValue;
                        foreach (Song.Entities.Examination it in items)
                        {
                            it.Exam_DateType = theme.Exam_DateType;
                            if (theme.Exam_DateType == 1)
                            {
                                if (it.Exam_Date.AddYears(100) > DateTime.Now)
                                    examDate = it.Exam_Date < examDate ? it.Exam_Date : examDate;                               
                            }
                            if (theme.Exam_DateType == 2)
                            {
                                it.Exam_Date = theme.Exam_Date;
                                it.Exam_DateOver = theme.Exam_DateOver;
                            }
                            if (it.Sbj_ID < 1) continue;
                            it.Exam_CrtTime = DateTime.Now;
                            it.Exam_Title = theme.Exam_Title;
                            it.Exam_GroupType = theme.Exam_GroupType;
                            it.Exam_IsUse = theme.Exam_IsUse;
                            it.Exam_IsRightClick = theme.Exam_IsRightClick;
                            it.Exam_IsShowBtn = theme.Exam_IsShowBtn;
                            it.Exam_IsToggle = theme.Exam_IsToggle;
                            tran.Save<Examination>(it);
                        }
                        if (theme.Exam_DateType == 1)
                        {
                            theme.Exam_Date = examDate;
                            theme.Exam_DateOver = examDate;
                        }
                        
                    }
                    tran.Save<Examination>(theme);
                    if (groups != null)
                    {
                        foreach (Song.Entities.ExamGroup g in groups)
                        {
                            g.Org_ID = org.Org_ID;
                            g.Org_Name = org.Org_Name;
                            tran.Save<ExamGroup>(g);
                        }
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

        public void ExamSave(Examination entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Examination>(entity);
                    tran.Update<Examination>(new Field[] { Examination._.Exam_IsUse }, new object[] { entity.Exam_IsUse }, Examination._.Exam_UID == entity.Exam_UID);
                    tran.Update<ExamResults>(new Field[] { ExamResults._.Exam_Name }, new object[] { entity.Exam_Name }, ExamResults._.Exam_UID == entity.Exam_UID);
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

        public void ExamSave(Examination theme, List<Examination> items, List<ExamGroup> groups)
        {           

            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {                    
                    //场次
                    if (items != null && items.Count > 0)
                    {
                        //考试主题时间
                        DateTime examDate = DateTime.MaxValue;
                        foreach (Song.Entities.Examination it in items)
                        {
                            it.Exam_DateType = theme.Exam_DateType;
                            if (theme.Exam_DateType == 1)
                            {
                                if (it.Exam_Date.AddYears(100) > DateTime.Now)
                                    examDate = it.Exam_Date < examDate ? it.Exam_Date : examDate;                                
                            }
                            if (theme.Exam_DateType == 2)
                            {
                                it.Exam_Date = theme.Exam_Date;
                                it.Exam_DateOver = theme.Exam_DateOver;
                            }
                            if (it.Sbj_ID < 1)
                            {
                                Gateway.Default.Delete<Examination>(it);
                            }
                            else
                            {
                                it.Exam_Title = theme.Exam_Title;
                                it.Exam_GroupType = theme.Exam_GroupType;
                                it.Exam_IsUse = theme.Exam_IsUse;
                                it.Exam_IsRightClick = theme.Exam_IsRightClick;
                                it.Exam_IsShowBtn = theme.Exam_IsShowBtn;
                                it.Exam_IsToggle = theme.Exam_IsToggle;
                                tran.Update<ExamResults>(new Field[] { ExamResults._.Exam_Name },
                                    new object[] { it.Exam_Name }, ExamResults._.Exam_UID == it.Exam_UID);
                                tran.Save<Examination>(it);
                            }
                        }
                        if (theme.Exam_DateType == 1)
                        {
                            theme.Exam_Date = examDate;
                            theme.Exam_DateOver = examDate;
                        }
                    }
                    
                    tran.Save<Examination>(theme);
                    //参考人员范围
                    tran.Delete<ExamGroup>(ExamGroup._.Exam_UID == theme.Exam_UID);
                    if (groups != null)
                    {
                        foreach (Song.Entities.ExamGroup g in groups)
                        {
                            tran.Save<ExamGroup>(g);
                        }
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
        public void ExamDelete(int identify)
        {
            Song.Entities.Examination exam = this.ExamSingle(identify);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Examination>(Examination._.Exam_ID == identify);
                    tran.Delete<Examination>(Examination._.Exam_UID == exam.Exam_UID && Examination._.Exam_IsTheme == false);
                    tran.Delete<ExamGroup>(ExamGroup._.Exam_UID == exam.Exam_UID);
                    tran.Delete<ExamResults>(ExamResults._.Exam_ID == exam.Exam_ID);
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

        public Examination ExamSingle(int identify)
        {
            return Gateway.Default.From<Examination>().Where(Examination._.Exam_ID == identify).ToFirst<Examination>();
        }
        /// <summary>
        /// 通过学员ID与考试ID，获取成绩（最好成绩）
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="examid"></param>
        /// <returns></returns>
        public ExamResults ResultSingle(int accid, int examid)
        {
            WhereClip wc = new WhereClip();
            wc.And(ExamResults._.Ac_ID == accid);
            wc.And(ExamResults._.Exam_ID == examid);
            return Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_Score.Desc).ToFirst<ExamResults>();
        }
        public Examination ExamSingle(string uid)
        {
            return Gateway.Default.From<Examination>().Where(Examination._.Exam_UID == uid && Examination._.Exam_IsTheme == true).ToFirst<Examination>();
        }
        /// <summary>
        /// 获取单一实体对象，取最近一次考试；此处获取的是考试主题或场次
        /// </summary>
        /// <returns></returns>
        public Examination ExamLast()
        {
            return Gateway.Default.From<Examination>().Where(Examination._.Exam_IsTheme == false)
                .OrderBy(Examination._.Exam_Date.Desc)
                .ToFirst<Examination>();
        }
        public ExamResults ResultClacScore(ExamResults resu)
        {
            return ClacScore(resu);
        }
        public Examination[] ExamItem(string uid)
        {
            return Gateway.Default.From<Examination>()
                .Where(Examination._.Exam_UID == uid && Examination._.Exam_IsTheme == false)
                .OrderBy(Examination._.Exam_CrtTime.Asc).ToArray<Examination>();
        }

        public Examination[] ExamItem(int id)
        {
            Song.Entities.Examination exam = this.ExamSingle(id);
            if (exam == null) return null;
            return this.ExamItem(exam.Exam_UID);
        }

        /// <summary>
        /// 当前考试主题关联的学生分类
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public StudentSort[] GroupForStudentSort(string uid)
        {
            //所在班组的考试
            Song.Entities.StudentSort[] sts = Gateway.Default.From<StudentSort>().InnerJoin<ExamGroup>(ExamGroup._.Sts_ID == StudentSort._.Sts_ID)
                .Where(ExamGroup._.Exam_UID == uid && ExamGroup._.Eg_Type == 2).ToArray<StudentSort>();
            return sts;            
        }

        public List<Examination> ExamCount(int orgid, bool? isUse, int count)
        {
            WhereClip wc = Examination._.Org_ID == orgid && Examination._.Exam_IsTheme == true;
            if (isUse != null) wc.And(Examination._.Exam_IsUse == (bool)isUse);
            count = count > 0 ? count : int.MaxValue;
            Song.Entities.Examination[] all = Gateway.Default.From<Examination>().Where(wc).OrderBy(Examination._.Exam_Date.Desc).ToArray<Examination>(count);
            List<Song.Entities.Examination> exams = new List<Examination>();
            foreach (Song.Entities.Examination t in all)
                _GetSelfExam_Add(exams, t);
            return exams;
        }
        public List<Examination> GetSelfExam(int stid, DateTime? start, DateTime? end)
        {
            Song.Entities.Accounts acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == stid).ToFirst<Accounts>();
            if (acc == null) return null;
            #region 查询条件
            //查询条件
            WhereClip wc = Examination._.Exam_IsTheme == false && Examination._.Exam_IsUse == true; //基础条件
            //准时开始的条件
            WhereClip wcType1 = Examination._.Exam_DateType == 1;
            if (start != null) wcType1.And(Examination._.Exam_Date >= (DateTime)start);
            if (end != null) wcType1.And(Examination._.Exam_Date < (DateTime)end);
            //区间开始的条件
            WhereClip wcType2 = Examination._.Exam_DateType == 2;
            if (start != null) wcType2.And(Examination._.Exam_Date >= (DateTime)start || Examination._.Exam_DateOver > (DateTime)start);
            if (end != null) wcType2.And(Examination._.Exam_DateOver < (DateTime)end);
            //
            WhereClip wcType = wcType1.Or(wcType2);
            wc.And(wcType);
            #endregion
            //全员参加的考试
            Song.Entities.Examination[] all = Gateway.Default.From<Examination>().Where(wc && Examination._.Exam_GroupType == 1).ToArray<Examination>();
            //所在学生组的考试            
            Song.Entities.Examination[] group = Gateway.Default.From<Examination>().InnerJoin<ExamGroup>(ExamGroup._.Exam_UID == Examination._.Exam_UID)
                .Where(wc && Examination._.Exam_GroupType == 2 && ExamGroup._.Sts_ID == acc.Sts_ID).ToArray<Examination>();          
            //合并到一起
            List<Song.Entities.Examination> exams = new List<Examination>();
            foreach (Song.Entities.Examination t in all)
                _GetSelfExam_Add(exams, t);
            foreach (Song.Entities.Examination t in group)
                _GetSelfExam_Add(exams, t);
            //排序
            for (int i = 0; i < exams.Count; i++)
            {
                for (int j = i; j < exams.Count; j++)
                {
                    if (exams[i].Exam_Date < exams[j].Exam_Date)
                    {
                        Song.Entities.Examination temp = exams[i];
                        exams[i] = exams[j];
                        exams[j] = temp;
                    }
                }
            }
            return exams;
        }
        public List<Examination> GetCountExam(int stid, DateTime? start, DateTime? end, bool? isUse, int count)
        {
            //查询条件
            WhereClip wc = Examination._.Exam_IsTheme == false && Examination._.Exam_IsUse == true;
            if (isUse != null) wc.And(Examination._.Exam_IsUse == (bool)isUse);
            if (start != null) wc.And(Examination._.Exam_Date >= (DateTime)start);
            if (end != null) wc.And(Examination._.Exam_Date < (DateTime)end);
            count = count > 0 ? count : int.MaxValue;
            //全员参加的考试
            Song.Entities.Examination[] all = Gateway.Default.From<Examination>().Where(wc).ToArray<Examination>();
            List<Song.Entities.Examination> exams = new List<Examination>();
            foreach (Song.Entities.Examination t in all)
                _GetSelfExam_Add(exams, t);
            //排序
            for (int i = 0; i < exams.Count; i++)
            {
                for (int j = i; j < exams.Count; j++)
                {
                    if (exams[i].Exam_Date > exams[j].Exam_Date)
                    {
                        Song.Entities.Examination temp = exams[i];
                        exams[i] = exams[j];
                        exams[j] = temp;
                    }
                }
            }
            return exams;
        }
        public List<Examination> _GetSelfExam_Add(List<Examination> list, Examination exam)
        {
            foreach (Song.Entities.Examination t in list)
            {
                if (t.Exam_ID == exam.Exam_ID) return list;
            }
            list.Add(exam);
            return list;
        }
        /// <summary>
        /// 判断某个考试是否允许某个学生参加
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="stid">学生id</param>
        /// <returns></returns>
        public bool ExamIsForStudent(int examid, int stid)
        {
            List<Examination> list = this.GetSelfExam(stid, null, null);
            foreach (Song.Entities.Examination exam in list)
            {
                if (exam.Exam_ID == examid) return true;
            }
            return false;
        }
        public Examination[] GetPager(int orgid, DateTime? start, DateTime? end, bool? isUse, string searName, int size, int index, out int countSum)
        {
            WhereClip wc = Examination._.Org_ID == orgid && Examination._.Exam_IsTheme == true;
            if (isUse != null) wc.And(Examination._.Exam_IsUse == (bool)isUse);
            if (searName != null && searName != "") wc.And(Examination._.Exam_Title.Like("%" + searName + "%"));
            if (start != null) wc.And(Examination._.Exam_Date >= (DateTime)start);
            if (end != null) wc.And(Examination._.Exam_Date < (DateTime)end);
            countSum = Gateway.Default.Count<Examination>(wc);
            return Gateway.Default.From<Examination>().Where(wc).OrderBy(Examination._.Exam_Date.Desc && Examination._.Exam_ID.Desc).ToArray<Examination>(size, (index - 1) * size);
        }
        public ExamResults[] GetAttendPager(int stid, int sbjid, int orgid, string sear, int size, int index, out int countSum)
        {
            WhereClip wc = ExamResults._.Exr_SubmitTime > DateTime.Now.AddYears(-100);
            if (stid > 0) wc.And(ExamResults._.Ac_ID == stid);
            if (sbjid > 0) wc.And(ExamResults._.Sbj_ID == sbjid);
            if (orgid > 0) wc.And(ExamResults._.Org_ID == orgid);
            if (sear != null && sear != "") wc.And(ExamResults._.Exam_Title.Like("%" + sear + "%"));
            countSum = Gateway.Default.Count<ExamResults>(wc);
            ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToArray<ExamResults>(size, (index - 1) * size);
            for (int i = 0; i < exr.Length; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc==false)
                    exr[i] = ClacScore(exr[i]);
            }
            return exr;
        }
        #region
        /// <summary>
        /// 添加考试答题信息
        /// </summary>
        /// <param name="result"></param>
        public ExamResults ResultAdd(ExamResults result)
        {
            WhereClip wc = ExamResults._.Exam_ID == result.Exam_ID && ExamResults._.Tp_Id == result.Tp_Id && ExamResults._.Ac_ID == result.Ac_ID;
            Song.Entities.ExamResults exr = Gateway.Default.From<ExamResults>().Where(wc).ToFirst<ExamResults>();
            if (exr != null)
            {
                if (exr.Exr_Results == result.Exr_Results) return exr;   //如果答案与记录的相同
                if (exr.Exr_IsSubmit) return exr;        //如果已经交卷
                Gateway.Default.Update<ExamResults>(
                    new Field[] { ExamResults._.Exr_Results, ExamResults._.Exr_IsSubmit, ExamResults._.Exr_IP, ExamResults._.Exr_Mac, ExamResults._.Exr_SubmitTime, ExamResults._.Exr_LastTime },
                    new object[] { result.Exr_Results, result.Exr_IsSubmit, result.Exr_IP, result.Exr_Mac, DateTime.Now, DateTime.Now },
                    ExamResults._.Exr_ID == exr.Exr_ID);
            }
            else
            {
                result.Exr_CrtTime = DateTime.Now;
                result.Exr_Score = -1;
                //考试主题
                Examination tm = this.ExamSingle((int)result.Exam_ID);
                if (tm != null)
                {
                    result.Exam_Title = tm.Exam_Title;
                    result.Exam_UID = tm.Exam_UID;
                    result.Exam_Name = tm.Exam_Name;
                }
                Gateway.Default.Save<ExamResults>(result);
            }
            return result;
        }
        /// <summary>
        /// 保存考试答题信息
        /// </summary>
        /// <param name="result"></param>
        public void ResultSave(ExamResults result)
        {
            Gateway.Default.Save<ExamResults>(result);
        }
        /// <summary>
        /// 成绩提交
        /// </summary>
        /// <param name="result"></param>
        public void ResultSubmit(ExamResults result)
        {
            Song.Entities.ExamResults exr = this.ResultSingle(result.Exam_ID, result.Tp_Id, result.Ac_ID);
            if (exr == null)
            {
                exr = result;
                exr.Exr_CrtTime = DateTime.Now;
                exr.Exr_SubmitTime = DateTime.Now;
                Song.Entities.Accounts st= Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID==result.Ac_ID).ToFirst<Accounts>();
                Song.Entities.Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == st.Org_ID).ToFirst<Organization>();
                if (org != null)
                {
                    exr.Org_ID = org.Org_ID;
                    exr.Org_Name = org.Org_Name;
                } 
            }
            else
            {
                //if (exr.Exr_Results == result.Exr_Results) return;
                exr.Exr_SubmitTime = DateTime.Now;
                exr.Exr_Results = result.Exr_Results;
                exr.Exr_IsSubmit = result.Exr_IsSubmit;
                exr.Exr_IP = result.Exr_IP;
                exr.Exr_Mac = result.Exr_Mac;                
            }
            Gateway.Default.Save<ExamResults>(exr);
        }
        /// <summary>
        /// 删除考试成绩
        /// </summary>
        /// <param name="id"></param>
        public void ResultDelete(int id)
        {
            Gateway.Default.Delete<ExamResults>(ExamResults._.Exr_ID == id);
        }
        /// <summary>
        /// 删除某个员工的某个考试的成绩
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="examid"></param>
        public void ResultDelete(int stid, int examid)
        {
            Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Exam_ID == examid).ToFirst<Examination>();
            if (exam != null)
            {
                string uid = string.Format("ExamResults：{0}-{1}-{2}", examid, exam.Tp_Id, stid);    //缓存的uid
                QuestionsMethod.QuestionsCache.Singleton.Delete(uid);
                //删除上传的附件
                string filepath = Upload.Get["Exam"].Physics + examid + "\\" + stid + "\\";
                if (System.IO.Directory.Exists(filepath)) System.IO.Directory.Delete(filepath, true);
            }
            WhereClip wc = new WhereClip();
            if (stid > -1) wc.And(ExamResults._.Ac_ID == stid);
            if (examid > -1) wc.And(ExamResults._.Exam_ID == examid);
            Gateway.Default.Delete<ExamResults>(wc);
        }
        /// <summary>
        /// 获取最新的答题信息（临时信息）
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="accid">考生id</param>
        /// <returns></returns>
        public ExamResultsTemp ExamResultsTempSingle(int examid, int tpid, int accid)
        {
            WhereClip wc = ExamResultsTemp._.Exam_ID == examid && ExamResultsTemp._.Tp_Id == tpid && ExamResultsTemp._.Acc_Id == accid;
            Song.Entities.ExamResultsTemp exr = Gateway.Default.From<ExamResultsTemp>().Where(wc).ToFirst<ExamResultsTemp>();
            return exr;
        }
        /// <summary>
        /// 获取最新的答题信息（正式答题信息），获取时并不进行计算状态的判断
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="acid">考生id</param>
        /// <returns></returns>
        public ExamResults ResultSingle(int examid, int tpid, int acid)
        {
            WhereClip wc = new WhereClip();
            if (examid > 0) wc.And(ExamResults._.Exam_ID == examid);
            if (tpid > 0) wc.And(ExamResults._.Tp_Id == tpid);
            if (acid > 0) wc.And(ExamResults._.Ac_ID == acid);
            Song.Entities.ExamResults exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToFirst<ExamResults>();            
            return exr;
        }
        /// <summary>
        /// 从缓存中获取考试答题信息
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        public ExamResults ResultSingleForCache(int examid, int tpid, int acid)
        {
            string uid = string.Format("ExamResults：{0}-{1}-{2}", examid, tpid, acid);    //缓存的uid
            ExamResults r = QuestionsMethod.QuestionsCache.Singleton.GetExamResults(uid);
            if (r == null) r = this.ResultSingle(examid, tpid, acid);
            return r;
        }
        /// <summary>
        /// 获取当前考试的所有考生答题信息
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ExamResults[] ResultCount(int examid, int count)
        {
            WhereClip wc = new WhereClip();
            if (examid > 0) wc.And(ExamResults._.Exam_ID == examid);
            ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToArray<ExamResults>(count);
            for (int i = 0; i < exr.Length; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc == false)
                    exr[i] = ClacScore(exr[i]);
            }
            return exr;
        }
        /// <summary>
        /// 当前考试信息中，下一个
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="stid"></param>
        /// <param name="isCorrect">是否是人工判卷过的，false下一个未判卷的信息</param>
        /// <returns></returns>
        public ExamResults ResultSingleNext(int examid, int stid, bool? isCorrect)
        {
            WhereClip wc = new WhereClip();
            if (examid > 0) wc.And(ExamResults._.Exam_ID == examid);          
            if (stid > 0) wc.And(ExamResults._.Ac_ID == stid);
            Song.Entities.ExamResults exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToFirst<ExamResults>();
            //
            Song.Entities.ExamResults next = null;
            if (isCorrect != null)
            {
                if ((bool)isCorrect)
                {
                    wc.And(ExamResults._.Exr_Score <= ExamResults._.Exr_ScoreFinal);
                }
                else
                {
                    wc.And(ExamResults._.Exr_Score == ExamResults._.Exr_ScoreFinal);
                }
            }
            next = Gateway.Default.From<ExamResults>()
                    .Where(wc && ExamResults._.Exr_CrtTime < exr.Exr_CrtTime).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToFirst<ExamResults>();
            if (next != null)
            {
                if (next.Exr_Score < 0 || next.Exr_IsCalc == false)
                    next = ClacScore(exr);
            }
            return next;
        }
        public ExamResults ResultSingle(int exrid)
        {
            Song.Entities.ExamResults exr = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exr_ID == exrid).ToFirst<ExamResults>();
            if (exr != null)
            {
                if (exr.Exr_Score < 0 || exr.Exr_IsCalc == false)
                    exr = ClacScore(exr);
            }
            return exr;
        }
        /// <summary>
        /// 根据答题信息，获取试题（针对答题过程中死机，又上线时）
        /// </summary>
        /// <returns></returns>
        public List<Questions> QuesForResults(string results)
        {
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null; 
            resXml.LoadXml(results, false);
            XmlNodeList nodeList = resXml.SelectSingleNode("results").ChildNodes;
            string ids = "";
            for (int i = 0; i < nodeList.Count;i++ )
            {
                ids += nodeList[i].Attributes["id"].Value;
                if (i < nodeList.Count - 1) ids += ",";
            }
            string[] s = ids.Split(',');
            List<Questions> quesList = new List<Questions>();
            //取数据库中的题
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Trim() == "") continue;
                int id = Convert.ToInt32(s[i]);
                Song.Entities.Questions q = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
                if (q == null) continue;
                quesList.Add(q);
            }
            //设置该题分数
            foreach (Song.Entities.Questions q in quesList)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    double num= Convert.ToDouble(nodeList[i].Attributes["num"].Value);
                    int id = Convert.ToInt32(nodeList[i].Attributes["id"].Value);
                    if (q.Qus_ID == id)
                    {
                        q.Qus_Number = (float)num;
                        break;
                    }
                }
            }
            return quesList;
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 计算当前考试成绩
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public Song.Entities.ExamResults ClacScore(ExamResults result)
        {
            //如果考试没有结束，且学员没有交卷，则不进行计算；
            if (DateTime.Now < result.Exr_OverTime && result.Exr_IsSubmit == false) return result;
            string resultXML = result.Exr_Results;
            result.Exr_Score = _ClacScore(resultXML, out resultXML);
            result.Exr_Results = resultXML;
            //考试得分，加绘图得分，加综合评分
            result.Exr_ScoreFinal = result.Exr_Score + result.Exr_Draw + result.Exr_Colligate;
            result.Exr_IsCalc = true;
            result.Exr_CalcTime = DateTime.Now;
            //记录成绩
            Field[] fields = new Field[] { ExamResults._.Exr_Score, ExamResults._.Exr_ScoreFinal, ExamResults._.Exr_IsCalc, ExamResults._.Exr_CalcTime };
            object[] objs = new object[] { result.Exr_Score, result.Exr_ScoreFinal, result.Exr_IsCalc, result.Exr_CalcTime };
            if (result.Exr_ID <= 0) result = ResultAdd(result);    
            Gateway.Default.Update<ExamResults>(fields, objs, ExamResults._.Exr_ID == result.Exr_ID);
            return result;
        }
        /// <summary>
        /// 通过答题的XML信息，计算成绩
        /// </summary>
        /// <param name="resultXML"></param>
        /// <returns></returns>
        public static float _ClacScore(string resultXML,out string outXml)
        {
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null; 
            resXml.LoadXml(resultXML, false);
            XmlNode root = resXml.LastChild;           
            var info = new
            {
                examid = Convert.ToInt32(root.Attributes["examid"].Value == null ? "0" : root.Attributes["examid"].Value),
                tpid = Convert.ToInt32(root.Attributes["tpid"].Value),
                stid = Convert.ToInt32(root.Attributes["stid"].Value),
                stname = root.Attributes["stname"].Value,
                sbjid = Convert.ToInt32(root.Attributes["sbjid"].Value),
                sbjname = root.Attributes["sbjname"].Value,
                patter = Convert.ToInt32(root.Attributes["patter"].Value)
            };
            
            #region 计算成绩
            //得分记录
            double score = 0;
            //大题的节点
            XmlNodeList quesNodes = root.ChildNodes;
            for (int i = 0; i < quesNodes.Count; i++)
            {
                int type = Convert.ToInt32(quesNodes[i].Attributes["type"].Value);
                //小题的节点
                XmlNodeList qnode = quesNodes[i].ChildNodes;
                ((XmlElement)quesNodes[i]).SetAttribute("count",qnode.Count.ToString());
                for (int j = 0; j < qnode.Count; j++)
                {
                    //当前试题的信息，id，分数，答案
                    var ques = new
                    {
                        id = Convert.ToInt32(qnode[j].Attributes["id"].Value),
                        num = Convert.ToDouble(qnode[j].Attributes["num"].Value),
                        ans = type == 1 || type == 2 || type == 3 ? qnode[j].Attributes["ans"].Value : qnode[j].InnerText
                    };
                    //是否正确
                    bool isSucess = Business.Do<IQuestions>().ClacQues(ques.id, ques.ans);
                    score += isSucess ? ques.num : 0;
                    ((XmlElement)qnode[j]).SetAttribute("sucess", isSucess.ToString().ToLower());
                    ((XmlElement)qnode[j]).SetAttribute("score", isSucess ? ques.num.ToString() : "0");
                    //如果错了，则记录为错题，以方便复习
                    if (!isSucess)
                    {
                        Song.Entities.Student_Ques sq = new Student_Ques();
                        //学生Id
                        sq.Ac_ID = info.stid;
                        //试题id与类型
                        sq.Qus_ID = ques.id;
                        sq.Qus_Type = type;
                        //学科
                        sq.Sbj_ID = info.sbjid;
                        ////难度
                        //sq.Qus_Diff = qus.Qus_Diff;
                        Business.Do<IStudent>().QuesAdd(sq);
                    }
                }

            }
            #endregion
            outXml = resXml.InnerXml;
            return (float)score;
        }
        #endregion
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable Result4Theme(int id)
        {
            return Result4Theme(id, -1);
            
        }
        /// <summary>
        /// 考试主题下的所有参考人员的班组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StudentSort[] StudentSort4Theme(int id)
        {
            Examination theme = this.ExamSingle(id);
            string sql = @"select distinct sts.* from studentsort as sts  inner join 
                        (select  * from ExamResults where ExamResults.Exam_UID='{0}') as exr 
                        on sts.sts_id=exr.sts_id";
            sql=string.Format(sql,theme.Exam_UID);
            return Gateway.Default.FromSql(sql).ToArray<StudentSort>();
        }
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id">当前考试主题的ID</param>
        /// <param name="stsid">学生分组的id，为0时取所有，为-1时取不在组的学员，大于0则取当前组学员</param>
        /// <returns></returns>
        public DataTable Result4Theme(int examid, int stsid)
        {
            Examination theme = this.ExamSingle(examid);
            Examination[] exams = this.ExamItem(theme.Exam_UID);    //当前考试下的多场考试
            DataTable dt = new DataTable("DataBase");
            //人员id与姓名
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("姓名", typeof(string)));
            dt.Columns.Add(new DataColumn("账号", typeof(string)));
            dt.Columns.Add(new DataColumn("性别", typeof(string)));
            dt.Columns.Add(new DataColumn("身份证", typeof(string)));
            foreach (Examination ex in exams)
            {
                dt.Columns.Add(new DataColumn(ex.Exam_Name, Type.GetType("System.String")));               
            }
            //取出所有的成绩
            WhereClip wc = ExamResults._.Exam_UID == theme.Exam_UID;
            if (stsid > 0) wc.And(ExamResults._.Sts_ID == stsid);   //取所有已分组的学员
            if (stsid < 0) wc.And(ExamResults._.Sts_ID <= 0);   //取所有未分组的学员
            ExamResults[] results = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_ScoreFinal.Desc).ToArray<ExamResults>();
            //计算成绩
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i].Exr_Score < 0 || results[i].Exr_IsCalc == false)
                    results[i] = ClacScore(results[i]);
            }
            foreach (ExamResults er in results)
            {
                bool isHav = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int accid = Convert.ToInt32(dt.Rows[i]["ID"]);
                    if (er.Ac_ID == accid)
                    {
                        isHav = true;
                        break;
                    }
                }
                if (isHav == false)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = er.Ac_ID;
                    dr["姓名"] = er.Ac_Name;
                    Accounts student = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == er.Ac_ID).ToFirst<Accounts>();
                    if (student != null)
                    {
                        dr["账号"] = student.Ac_AccName;
                    }
                    dr["性别"] = er.Ac_Sex == 0 ? "未知" : (er.Ac_Sex == 1 ? "男" : "女");
                    dr["身份证"] = er.Ac_IDCardNumber;
                    dt.Rows.Add(dr);
                }
            }
            foreach (Examination ex in exams)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int accid = Convert.ToInt32(dt.Rows[i]["ID"]);
                    foreach (ExamResults er in results)
                    {
                        if (er.Ac_ID == accid && er.Exam_ID == ex.Exam_ID)
                        {
                            double score = Math.Floor(er.Exr_ScoreFinal * 100) / 100;
                            //此处是两个数值（用$分隔），前面是成绩，后面是成绩记的id
                            dt.Rows[i][ex.Exam_Name] = score.ToString() + "$" + er.Exr_ID;
                            break;
                        }
                    }
                }
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "姓名 Asc";
            return dv.ToTable();
        }
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id">当前考试主题的ID</param>
        /// <param name="stsid">学生分组的id，多个组用逗号分隔</param>
        /// <returns></returns>
        public DataTable Result4Theme(int examid, string stsid)
        {
            DataTable dtFirst = null; 
            foreach (string s in stsid.Split(','))
            {
                if (string.IsNullOrWhiteSpace(s)) continue;
                int sid = 0;
                int.TryParse(s, out sid);
                //if (sid <= 0) continue;
                //取每个组的学员的考试成绩
                DataTable dtSecond = this.Result4Theme(examid, sid);
                if (dtSecond == null) continue;
                if (dtFirst == null || dtFirst.Rows.Count < 1) dtFirst = dtSecond;
                if (!dtFirst.Equals(dtSecond))
                {
                    //将两个表都存在的数据，合并
                    for (int i = 0; i < dtFirst.Rows.Count; i++)
                    {
                        DataRow dr1 = dtFirst.Rows[i];                        
                        foreach (DataRow dr2 in dtSecond.Rows)
                        {
                            if (dr1["ID"].ToString() == dr2["ID"].ToString())
                            {
                                for (var n = 0; n < dr1.ItemArray.Length; n++)
                                {
                                    if (!string.IsNullOrWhiteSpace(dr2[n].ToString()))
                                        dr1[n] = dr2[n];
                                }                                
                            }
                        }                        
                    }
                    //如果第二个表比第一个表多，则添加
                    foreach (DataRow dr2 in dtSecond.Rows)
                    {
                        bool isExist = false;
                        foreach (DataRow dr1 in dtFirst.Rows)
                        {                            
                            if (dr1["ID"].ToString() == dr2["ID"].ToString())
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (!isExist)
                        {
                            DataRow drnew = dtFirst.NewRow();
                            for (var n = 0; n < drnew.ItemArray.Length; n++)                            
                                drnew[n] = dr2[n];                            
                            dtFirst.Rows.Add(drnew);
                        }
                    }
                }
            }
            return dtFirst;
        }
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stsid">学生分组的id，为0时取所有，为-1时取不在组的学员，大于0则取当前组学员</param>
        /// <param name="isAll">是否取所有人员（含缺考人员）,false为仅参考人员</param>
        /// <returns></returns>
        public DataTable Result4Theme(int id, int stsid, bool isAll)
        {
            if (!isAll) return Result4Theme(id, stsid);
            Examination theme = this.ExamSingle(id);
            Examination[] exams = this.ExamItem(theme.Exam_UID);    //当前考试下的多场考试
            //构建表结构
            DataTable dt = new DataTable("DataBase");
            //人员id与姓名
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("姓名", typeof(string)));
            dt.Columns.Add(new DataColumn("账号", typeof(string)));
            dt.Columns.Add(new DataColumn("性别", typeof(string)));
            dt.Columns.Add(new DataColumn("身份证", typeof(string)));
            foreach (Examination ex in exams)
            {
                dt.Columns.Add(new DataColumn(ex.Exam_Name, typeof(string)));               
            }
            //取学员
            WhereClip wcAcc = Accounts._.Org_ID == theme.Org_ID;
            if (stsid > 0) wcAcc.And(Accounts._.Sts_ID == stsid);   //取所有已分组的学员
            if (stsid < 0) wcAcc.And(Accounts._.Sts_ID <= 0);   //取所有未分组的学员
            Accounts[] accs = Gateway.Default.From<Accounts>().Where(wcAcc).ToArray<Accounts>();
            foreach (Accounts ac in accs)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = ac.Ac_ID;
                dr["姓名"] = ac.Ac_Name;
                dr["账号"] = ac.Ac_AccName;
                dr["性别"] = ac.Ac_Sex == 0 ? "未知" : (ac.Ac_Sex == 1 ? "男" : "女");
                dr["身份证"] = ac.Ac_IDCardNumber;
                dt.Rows.Add(dr);
            }
            //取出所有的成绩
            WhereClip wcEr = ExamResults._.Exam_UID == theme.Exam_UID;
            if (stsid > 0) wcEr.And(ExamResults._.Sts_ID == stsid);   //取所有已分组的学员
            if (stsid < 0) wcEr.And(ExamResults._.Sts_ID <= 0);   //取所有未分组的学员
            ExamResults[] results = Gateway.Default.From<ExamResults>().Where(wcEr).ToArray<ExamResults>();            
            for (int i = 0; i < results.Length; i++)
            {
                //如果没有计算，则计算成绩
                if (results[i].Exr_Score < 0 || results[i].Exr_IsCalc == false)
                    results[i] = ClacScore(results[i]);
            }
            //遍历学员datatable，向其填充考试成绩
            foreach (DataRow dr in dt.Rows)
            {
                int accid = Convert.ToInt32(dr["ID"]);
                foreach (ExamResults er in results)
                {
                    if (er.Ac_ID == accid) dr[er.Exam_Name] = er.Exr_ScoreFinal; 
                }
            }            
            //排序，优先显示已经考试的，成绩倒序显示
            string order = "";
            foreach (ExamResults er in results)
                order += er.Exam_Name + " DESC,";
            if (order.EndsWith(",")) order = order.Substring(0, order.Length - 1);
            DataView dv = dt.DefaultView;
            dv.Sort = order;
            return dv.ToTable();
        }
        /// <summary>
        /// 当前考试主题下的各学员分组成绩排行
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public DataTable Result4StudentSort(int examid)
        {
            //当前考试下的所有学员分组
            StudentSort[] sts = null;
            string sql = @"select distinct sts.* from studentsort as sts  inner join (select  * from ExamResults where ExamResults.Exam_ID='{0}') as exr on sts.sts_id=exr.sts_id";
            sql = string.Format(sql, examid.ToString());
            sts = Gateway.Default.FromSql(sql).ToArray<StudentSort>();
            //数据集
            DataTable dt = new DataTable("DataBase");
            dt.Columns.Add(new DataColumn("ID", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("Sts_Name", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("参训率", Type.GetType("System.Double")));
            dt.Columns.Add(new DataColumn("平均分", Type.GetType("System.Double")));
            foreach (StudentSort s in sts)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = s.Sts_ID;
                dr["Sts_Name"] = s.Sts_Name;
                //++++++++++++++++++++++++++++++++++++++++++++ 平均分
                object avgobj = Gateway.Default.Avg<ExamResults>(ExamResults._.Exr_ScoreFinal, ExamResults._.Sts_ID == s.Sts_ID);
                double avg = 0;
                double.TryParse(avgobj.ToString(), out avg);
                dr["平均分"] = Math.Round(Math.Round(avg * 10000) / 10000, 2, MidpointRounding.AwayFromZero);
                //++++++++++++++++++++++++++++++++++++++++++++ 参训度，当前分组参加考试的比率
                double attend = 0;
                //当前分组学员数量
                int stTotal = Gateway.Default.Count<Accounts>(Accounts._.Sts_ID == s.Sts_ID);
                if (stTotal <= 0) dr["参训率"] = 0;
                //当前分组参加考试的数量
                int stCount = Gateway.Default.Count<ExamResults>(ExamResults._.Sts_ID == s.Sts_ID);
                if (stCount <= 0) dr["参训率"] = 0;
                attend = (double)stCount / (double)stTotal * 100;
                attend = Math.Round(Math.Round(attend * 10000) / 10000, 2, MidpointRounding.AwayFromZero);
                dr["参训率"] = attend > 100 ? 100 : attend;
                //++++++++++++++++++++++++++++++++++++++++++++
                dt.Rows.Add(dr);
            }
            DataView dv = dt.DefaultView;
            dv.Sort = "平均分 Desc";
            DataTable dt2 = dv.ToTable();
            return dt2;
        }        
        ///// <summary>
        ///// 统计各院系在某个考试中的平均分
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public DataTable Analysis4Depart(int id)
        //{
        //    Examination theme = this.ExamSingle(id);
        //    Examination[] exams = this.ExamItem(theme.Exam_UID);
        //    DataTable dt = new DataTable("DataBase");
        //    //院系id与名称
        //    dt.Columns.Add(new DataColumn("Dep_Id", Type.GetType("System.Int32")));
        //    dt.Columns.Add(new DataColumn("院系", Type.GetType("System.String")));
        //    foreach (Examination ex in exams)
        //        dt.Columns.Add(new DataColumn(ex.Sbj_Name, Type.GetType("System.String")));
        //    //取出所有的成绩
        //    ExamResults[] results = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exam_UID == theme.Exam_UID).ToArray<ExamResults>();
        //    for (int i = 0; i < results.Length; i++)
        //    {
        //        if (results[i].Exr_Score < 0)               
        //            results[i] = _ClacScore(results[i]);                  
        //    }
        //    Song.Entities.Depart[] deps = this.GroupForDepart(theme.Exam_UID);
        //    if (deps.Length < 1) deps = Gateway.Default.From<Depart>().OrderBy(Depart._.Dep_Tax.Asc).ToArray<Depart>();
        //    foreach (Depart dep in deps)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Dep_Id"] = dep.Dep_Id;
        //        dr["院系"] = dep.Dep_CnName;
        //        dt.Rows.Add(dr);
        //    }
        //    foreach (Examination ex in exams)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            int depid = Convert.ToInt32(dt.Rows[i]["Dep_Id"]);
        //            double score = 0;
        //            int tmNum = 0;
        //            foreach (ExamResults er in results)
        //            {
        //                if (er.Dep_Id == depid && er.Sbj_Name == ex.Sbj_Name)
        //                {
        //                    score += (float)er.Exr_Score;
        //                    tmNum++;
        //                    break;
        //                }
        //            }
        //            double avg = tmNum == 0 ? 0 : score / tmNum;
        //            avg = Math.Round(avg * 100) / 100;
        //            dt.Rows[i][ex.Sbj_Name] = avg.ToString();
        //        }
        //    }
        //    return dt;
        //}
        ///// <summary>
        ///// 统计各班组在某个考试中的平均分
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public DataTable Analysis4Team(int id)
        //{
        //    Examination theme = this.ExamSingle(id);
        //    Examination[] exams = this.ExamItem(theme.Exam_UID);
        //    DataTable dt = new DataTable("DataBase");
        //    //院系id与名称
        //    dt.Columns.Add(new DataColumn("Team_ID", Type.GetType("System.Int32")));
        //    dt.Columns.Add(new DataColumn("班组", Type.GetType("System.String")));
        //    foreach (Examination ex in exams)
        //        dt.Columns.Add(new DataColumn(ex.Sbj_Name, Type.GetType("System.String")));
        //    //取出所有的成绩
        //    ExamResults[] results = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exam_UID == theme.Exam_UID).ToArray<ExamResults>();
        //    for (int i = 0; i < results.Length; i++)
        //    {
        //        if (results[i].Exr_Score < 0)
        //            results[i] = _ClacScore(results[i]);
        //    }
        //    Song.Entities.Team[] teams = this.GroupForTeam(theme.Exam_UID);
        //    foreach (Team team in teams)
        //    {
        //        DataRow dr = dt.NewRow();
        //        dr["Team_ID"] = team.Team_ID;
        //        dr["班组"] = team.Team_Name;
        //        dt.Rows.Add(dr);
        //    }
        //    foreach (Examination ex in exams)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            int tmid = Convert.ToInt32(dt.Rows[i]["Team_ID"]);
        //            double score = 0;
        //            int tmNum = 0;
        //            foreach (ExamResults er in results)
        //            {
        //                if (er.Team_ID == tmid && er.Sbj_Name == ex.Sbj_Name)
        //                {
        //                    score += (float)er.Exr_Score;
        //                    tmNum++;
        //                    break;
        //                }
        //            }
        //            double avg = tmNum == 0 ? 0 : score / tmNum;
        //            avg = Math.Round(avg * 100) / 100;
        //            dt.Rows[i][ex.Sbj_Name] = avg.ToString();
        //        }
        //    }
        //    return dt;
        //}
        /// <summary>
        /// 计算某个考试主题的及格率
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public double PassRate4Theme(string uid)
        {
            Examination[] exam = this.ExamItem(uid);
            double rate = 0;
            foreach (Examination ex in exam)
            {
                rate += this.PassRate4Exam(ex);
            }
            return rate / exam.Length;
        }
        /// <summary>
        /// 计算某场考试的及极率
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public double PassRate4Exam(Examination exam)
        {
            int sum = Gateway.Default.Count<ExamResults>(ExamResults._.Exam_ID == exam.Exam_ID && ExamResults._.Exr_Score >= 0);
            if (sum < 1) return 0;
            TestPaper tp = Gateway.Default.From<TestPaper>().Where(TestPaper._.Tp_Id == exam.Tp_Id).ToFirst<TestPaper>();
            int pass = Gateway.Default.Count<ExamResults>(ExamResults._.Exam_ID == exam.Exam_ID && ExamResults._.Exr_Score >= tp.Tp_Total * 0.6);
            double s = (double)sum;
            double p = (double)pass;
            double rate = Math.Round(p / s * 10000) / 100;
            return rate;
        }
        /// <summary>
        /// 计算某个考试主题的平均分
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public double Avg4Theme(string uid)
        {
            Examination[] exam = this.ExamItem(uid);
            double avg = 0;
            foreach (Examination ex in exam)
            {
                avg += this.Avg4Exam(ex.Exam_ID);
            }
            return avg / exam.Length;
        }
        /// <summary>
        /// 计算某场考试的平均分
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public double Avg4Exam(int examid)
        {
            object obj = Gateway.Default.Avg<ExamResults>(ExamResults._.Exr_Score, ExamResults._.Exam_ID == examid);
            if (obj == null || obj.GetType().FullName == "System.DBNull") return 0;
            double tm = obj is DBNull ? 0 : Convert.ToDouble(obj);
            return tm;
        }
        /// <summary>
        /// 当前考试的参考人数
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public int Number4Exam(int examid)
        {
            ExamResults[] erx = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exam_ID == examid)
                .SubQuery().Select(ExamResults._.Exr_ID.Sum()).GroupBy(ExamResults._.Ac_ID.Group).ToArray<ExamResults>();
            //return Gateway.Default.Count<ExamResults>(ExamResults._.Exam_ID == examid);
            return erx.Length;
        }
        public ExamResults[] Results(int examid, int size, int index, out int countSum)
        {
            WhereClip wc = ExamResults._.Exam_ID == examid && ExamResults._.Exr_SubmitTime > DateTime.Now.AddYears(-100);          
            countSum = Gateway.Default.Count<ExamResults>(wc);
            ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToArray<ExamResults>(size, (index - 1) * size);
            for (int i = 0; i < exr.Length; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc == false)
                    exr[i] = ClacScore(exr[i]);
            }
            return exr;
        }

        public ExamResults[] Results(string examuid, int size, int index, out int countSum)
        {
            WhereClip wc = ExamResults._.Exam_UID == examuid && ExamResults._.Exr_SubmitTime > DateTime.Now.AddYears(-100);
            countSum = Gateway.Default.Count<ExamResults>(wc);
            ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToArray<ExamResults>(size, (index - 1) * size);
            for (int i = 0; i < exr.Length; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc == false)
                    exr[i] = ClacScore(exr[i]);
            }
            return exr;
        }
        /// <summary>
        /// 当前考试场次下的所有人员成绩
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public ExamResults[] Results(int examid, int count)
        {
            WhereClip wc = ExamResults._.Exam_ID == examid && ExamResults._.Exr_SubmitTime > DateTime.Now.AddYears(-100);
            ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToArray<ExamResults>();
            for (int i = 0; i < exr.Length; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc == false)
                    exr[i] = ClacScore(exr[i]);
            }
            exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_ScoreFinal.Desc).ToArray<ExamResults>(count);
            return exr;
        }
    }
}
