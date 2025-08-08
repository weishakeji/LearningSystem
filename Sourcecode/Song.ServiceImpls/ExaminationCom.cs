using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace Song.ServiceImpls
{
    public class ExaminationCom : IExamination
    {
        private static AccountsCom accountsCom = new AccountsCom();
        private static StudentCom studentCom = new StudentCom();
        private static OrganizationCom orgCom = new OrganizationCom();
        private static QuestionsCom quesCom = new QuestionsCom();
        private static TestPaperCom tpCom = new TestPaperCom();
        private static SubjectCom sbjCom = new SubjectCom();
        public int ExamAdd(Teacher teacher, Examination entity)
        {
            entity.Exam_CrtTime = DateTime.Now;
            //当前考试的创建人
            entity.Th_ID = teacher.Th_ID;
            entity.Th_Name = teacher.Th_Name;
            //
            Song.Entities.Organization org = orgCom.OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Examination>(entity);
        }

        public void ExamAdd(Teacher teacher, Examination theme, Examination[] items, ExamGroup[] groups)
        {
            Song.Entities.Organization org = orgCom.OrganCurrent();
            if (org != null)
            {
                theme.Org_ID = org.Org_ID;
                theme.Org_Name = org.Org_Name;
            }
            theme.Exam_CrtTime = DateTime.Now;
            //当前考试的创建人
            if (teacher != null)
            {
                theme.Th_ID = teacher.Th_ID;
                theme.Th_Name = teacher.Th_Name;
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
                                if (it.Exam_Date.AddYears(30) > DateTime.Now)
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
                    if (entity.Exam_IsTheme)
                        tran.Update<ExamResults>(new Field[] { ExamResults._.Exam_Title }, new object[] { entity.Exam_Title }, ExamResults._.Exam_UID == entity.Exam_UID);
                    else
                        tran.Update<ExamResults>(new Field[] { ExamResults._.Exam_Name }, new object[] { entity.Exam_Name }, ExamResults._.Exam_ID == entity.Exam_ID);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        public void ExamSave(Examination theme, Examination[] items, ExamGroup[] groups)
        {

            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    if (items == null || items.Length <= 0)
                    {
                        tran.Delete<Examination>(Examination._.Exam_UID == theme.Exam_UID && Examination._.Exam_IsTheme == false);
                    }
                    else
                    {
                        //获取原来的场次
                        Examination[] exams = Gateway.Default.From<Examination>()
                            .Where(Examination._.Exam_UID == theme.Exam_UID && Examination._.Exam_IsTheme == false)
                            .OrderBy(Examination._.Exam_CrtTime.Asc).ToArray<Examination>();
                        foreach (Examination old in exams)
                        {
                            bool isexist = false;
                            foreach (Examination newly in items)
                            {
                                if (old.Exam_ID == newly.Exam_ID)
                                {
                                    isexist = true;
                                    break;
                                }
                            }
                            if (!isexist) tran.Delete<Examination>(Examination._.Exam_ID == old.Exam_ID);
                        }

                        //考试主题时间
                        DateTime examDate = DateTime.MaxValue;
                        foreach (Song.Entities.Examination it in items)
                        {
                            it.Exam_DateType = theme.Exam_DateType;
                            if (theme.Exam_DateType == 1)
                            {
                                if (it.Exam_Date.AddYears(30) > DateTime.Now)
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
                                if (it.Exam_IsTheme)
                                    tran.Update<ExamResults>(new Field[] { ExamResults._.Exam_Title },
                                    new object[] { it.Exam_Title }, ExamResults._.Exam_UID == it.Exam_UID);
                                else
                                    tran.Update<ExamResults>(new Field[] { ExamResults._.Exam_Name },
                                  new object[] { it.Exam_Name }, ExamResults._.Exam_ID == it.Exam_ID);
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
                    foreach (ExamGroup g in groups ?? new ExamGroup[0])
                        tran.Save<ExamGroup>(g);

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
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
            return Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_ScoreFinal.Desc).ToFirst<ExamResults>();
        }
        /// <summary>
        /// 获取考试主题
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 计算当前考试成绩
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public Song.Entities.ExamResults ResultClacScore(ExamResults result)
        {
            //如果考试没有结束，且学员没有交卷，则不进行计算；
            if (DateTime.Now < result.Exr_OverTime && result.Exr_IsSubmit == false) return result;
            try
            {
                string resultXML = result.Exr_Results;
                result.Exr_Score = (float)_ClacScore(resultXML, out resultXML);
                result.Exr_Results = resultXML;
            }
            catch (Exception ex)
            {
                WeiSha.Core.Log.Error(this.GetType().FullName, ex);
            }
            //考试得分，加绘图得分，加综合评分
            result.Exr_ScoreFinal = result.Exr_Score + result.Exr_Draw + result.Exr_Colligate;
            result.Exr_IsCalc = true;
            result.Exr_CalcTime = DateTime.Now;
            //记录成绩
            Field[] fields = new Field[] {
                ExamResults._.Exr_Score, ExamResults._.Exr_ScoreFinal, ExamResults._.Exr_IsCalc, ExamResults._.Exr_CalcTime,
                ExamResults._.Exr_Results
            };
            object[] objs = new object[] {
                result.Exr_Score, result.Exr_ScoreFinal,
                result.Exr_IsCalc, result.Exr_CalcTime,result.Exr_Results
            };
            if (result.Exr_ID <= 0) result = ResultAdd(result);
            Gateway.Default.Update<ExamResults>(fields, objs, ExamResults._.Exr_ID == result.Exr_ID);
            return result;
        }
        /// <summary>
        /// 自助设置考试成绩得分
        /// </summary>
        /// <param name="exrid">考试的答题记录id</param>
        /// <param name="score">期望的得分</param>
        /// <returns></returns>
        public ExamResults ResultSetScore(int exrid, float score)
        {
            ExamResults exr = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exr_ID == exrid).ToFirst<ExamResults>();
            return ResultSetScore(exr, score);
        }
        /// <summary>
        /// 自助设置考试成绩得分
        /// </summary>
        /// <param name="result">考试的答题记录</param>
        /// <param name="score">期望的得分</param>
        public ExamResults ResultSetScore(ExamResults result, float score)
        {
            if (result == null) return null;
            if (string.IsNullOrWhiteSpace(result.Exr_Results)) return result;
            Exam.Results examresult = new Song.ServiceImpls.Exam.Results(result.Exr_Results);
            float exrscore = examresult.SetScore(score);
            result.Exr_Score = result.Exr_ScoreFinal = (float)Math.Round(exrscore * 100) / 100;
            result.Exr_Results = examresult.OutputXML(false);
            Gateway.Default.Save<ExamResults>(result);
            return result;
        }
        /// <summary>
        /// 自助设置考试成绩得分
        /// </summary>
        /// <param name="result">考试的答题记录</param>
        /// <param name="score">期望的得分</param>
        /// <param name="time">考试开始时间</param>
        /// <param name="duration">考试用时，单位分钟</param>
        /// <returns></returns>
        public ExamResults ResultSetScore(ExamResults result, float score, DateTime? time, int duration)
        {
            //考试限定开始时间，学员答题时间，答题结束时间
            DateTime beginTime, starttime, overtime;
            Examination exam= Gateway.Default.From<Examination>().Where(Examination._.Exam_ID == result.Exam_ID).ToFirst<Examination>();
            beginTime = exam.Exam_Date; //考试的限定开始时间
            starttime = time ?? result.Exr_CrtTime;
            if (starttime < beginTime) starttime = beginTime;
            overtime = starttime.AddMinutes(duration);
            //
            result.Exr_CrtTime=starttime;
            result.Exr_LastTime = result.Exr_OverTime = result.Exr_SubmitTime = overtime;
            result.Exr_IsSubmit = true;
            //生成答题信息
            Exam.Results examresult = new Song.ServiceImpls.Exam.Results(result.Exr_Results);
            float exrscore = examresult.SetScore(score);
            examresult.Begin = beginTime;
            examresult.Startime = starttime;
            examresult.Overtime = overtime;
            //
            result.Exr_Score = result.Exr_ScoreFinal = (float)Math.Round(exrscore * 100) / 100;
            result.Exr_Results = examresult.OutputXML(false);
            Gateway.Default.Save<ExamResults>(result);
            return result;
        }
        /// <summary>
        /// 自助设置考试成绩得分，如果没有成绩记录，则创建一个
        /// </summary>
        /// <param name="accid">学员账号id</param>
        /// <param name="examid">考试场次id</param>
        /// <param name="score">期望的得分</param>
        /// <param name="time">考试开始时间</param>
        /// <param name="duration">考试用时，单位分钟</param>
        /// <returns></returns>
        public ExamResults ResultSetScore(int examid, int accid, float score, DateTime? time, int duration)
        {
            ExamResults exr = this.ResultSingle(accid, examid);     //考试答题记录的对象
            //当前考试对象
            Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Exam_ID == examid).ToFirst<Examination>();
            if (exr == null)
            {
                exr = new ExamResults();
                //设置学员的信息
                Accounts acc = accountsCom.AccountsSingle(accid);
                if (acc == null) throw new Exception("学员账号不存在");
                exr.Ac_ID = acc.Ac_ID;
                exr.Ac_IDCardNumber = acc.Ac_IDCardNumber;  //身份证号
                exr.Ac_Name = acc.Ac_Name;
                exr.Ac_Sex = acc.Ac_Sex;
                exr.Sts_ID = acc.Sts_ID;    //学员组id
                //考试的信息
                exr.Exam_ID = examid;
                exr.Exam_Name = exam.Exam_Name;
                exr.Exam_UID = exam.Exam_UID;
                exr.Exam_Title = exam.Exam_Title;
                //机构信息
                Organization org = orgCom.OrganSingle(acc.Org_ID);
                exr.Org_ID = acc.Org_ID;
                if (org != null) exr.Org_Name = org.Org_Name;
                //试卷信息
                TestPaper tp = tpCom.PaperSingle(exam.Tp_Id);
                exr.Tp_Id = tp.Tp_Id;
                //专业信息
                Subject subject = sbjCom.SubjectSingle(tp.Sbj_ID);
                if (subject != null)
                {
                    exr.Sbj_ID = subject.Sbj_ID;
                    exr.Sbj_Name = subject.Sbj_Name;
                }
                //IP与Mac

                //出卷,生成试卷
                Song.ServiceImpls.Exam.Results results = Song.ServiceImpls.Exam.TestPaperHandler.Putout(exr.Tp_Id).ToResults(exam, acc);
                exr.Exr_Results = results.OutputXML(false);
            }
            exr = this.ResultSetScore(exr, score, time, duration);
            return exr;
        }
        /// <summary>
        /// 批量计算考试成绩
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <returns></returns>
        public bool ResultBatchClac(int examid)
        {
            List<ExamResults> ers = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exam_ID == examid).ToList<ExamResults>();
            for(int i = 0; i < ers.Count; i++)
            {
                ResultClacScore(ers[i]);
            }
            return true;
        }
        public Examination[] ExamItem(string uid)
        {
            return Gateway.Default.From<Examination>()
                .Where(Examination._.Exam_UID == uid && Examination._.Exam_IsTheme == false)
                .OrderBy(Examination._.Exam_ID.Asc).ToArray<Examination>();
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
            WhereClip wc = new WhereClip();
            wc &= Examination._.Exam_IsTheme == true;
            if (orgid > 0) wc &= Examination._.Org_ID == orgid;
            if (isUse != null) wc.And(Examination._.Exam_IsUse == (bool)isUse);
            count = count > 0 ? count : int.MaxValue;
            Song.Entities.Examination[] all = Gateway.Default.From<Examination>().Where(wc).OrderBy(Examination._.Exam_Date.Desc).ToArray<Examination>(count);
            List<Song.Entities.Examination> exams = new List<Examination>();
            foreach (Song.Entities.Examination t in all)
                _GetSelfExam_Add(exams, t);
            return exams;
        }
        /// <summary>
        /// 考试数
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="istheme">为true取考试主题数；false时，取场次数</param>
        /// <returns></returns>
        public int ExamOfCount(int orgid, bool? isUse, bool istheme)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Examination._.Org_ID == orgid;
            if (isUse != null) wc.And(Examination._.Exam_IsUse == (bool)isUse);
            wc &= Examination._.Exam_IsTheme == istheme;
            return Gateway.Default.Count<Examination>(wc);
        }
        public List<Examination> GetSelfExam(int stid, DateTime? start, DateTime? end, string search)
        {
            Song.Entities.Accounts acc = Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID == stid).ToFirst<Accounts>();
            if (acc == null) return null;
            #region 查询条件
            //查询条件
            WhereClip wc = Examination._.Exam_IsTheme == false && Examination._.Exam_IsUse == true; //基础条件
            if (!string.IsNullOrWhiteSpace(search)) wc.And(Examination._.Exam_Name.Contains(search));
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
        public List<Examination> GetSelfExam(int stid, DateTime? start, DateTime? end, string search, int size, int index, out int countSum)
        {
            List<Song.Entities.Examination> list = new List<Examination>();
            List<Song.Entities.Examination> exams = this.GetSelfExam(stid, start, end, search);
            if (exams == null)
            {
                countSum = 0;
                return list;
            }
            countSum = exams.Count;           
            index = index <= 0 ? 0 : index - 1;
            for (int i = 0; i < exams.Count; i++)
            {
                if (i < size * index) continue;
                if (i >= size * (index + 1)) break;
                list.Add(exams[i]);
            }
            return list;
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
            List<Examination> list = this.GetSelfExam(stid, null, null, string.Empty);
            foreach (Song.Entities.Examination exam in list)
            {
                if (exam.Exam_ID == examid) return true;
            }
            return false;
        }
        public Examination[] GetPager(int orgid, DateTime? start, DateTime? end, bool? isUse, string searName, int size, int index, out int countSum)
        {
            WhereClip wc = Examination._.Exam_IsTheme == true;
            if (orgid > 0) wc.And(Examination._.Org_ID == orgid);
            if (isUse != null) wc.And(Examination._.Exam_IsUse == (bool)isUse);
            if (searName != null && searName != "") wc.And(Examination._.Exam_Title.Contains(searName));
            if (start != null) wc.And(Examination._.Exam_Date >= (DateTime)start);
            if (end != null) wc.And(Examination._.Exam_Date < (DateTime)end);
            countSum = Gateway.Default.Count<Examination>(wc);
            return Gateway.Default.From<Examination>().Where(wc).OrderBy(Examination._.Exam_Date.Desc && Examination._.Exam_ID.Desc).ToArray<Examination>(size, (index - 1) * size);
        }
        public ExamResults[] GetAttendPager(int stid, long sbjid, int orgid, string sear, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (stid > 0) wc.And(ExamResults._.Ac_ID == stid);
            if (sbjid > 0) wc.And(ExamResults._.Sbj_ID == sbjid);
            if (orgid > 0) wc.And(ExamResults._.Org_ID == orgid);
            //是否考试结束，或已经交卷
            wc.And(ExamResults._.Exr_OverTime < DateTime.Now || ExamResults._.Exr_IsSubmit == true);

            if (sear != null && sear != "") wc.And(ExamResults._.Exam_Name.Contains(sear));
            countSum = Gateway.Default.Count<ExamResults>(wc);
            ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToArray<ExamResults>(size, (index - 1) * size);
            for (int i = 0; i < exr.Length; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc==false)
                    exr[i] = ResultClacScore(exr[i]);
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
            if (result.Exam_ID <= 0 || result.Tp_Id <= 0 || result.Ac_ID <= 0) return result;
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
                if(result.Exr_CrtTime<DateTime.Now.AddYears(-30))
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
        public ExamResults ResultSubmit(ExamResults result)
        {
            if (result.Exam_ID <= 0 || result.Tp_Id <= 0 || result.Ac_ID <= 0) return result;
            Song.Entities.ExamResults exr = this.ResultSingle(result.Exam_ID, result.Tp_Id, result.Ac_ID);
            if (exr == null)
            {
                exr = result;
                //exr.Exr_CrtTime = DateTime.Now;
                exr.Exr_SubmitTime = DateTime.Now;
                Song.Entities.Accounts st= Gateway.Default.From<Accounts>().Where(Accounts._.Ac_ID==result.Ac_ID).ToFirst<Accounts>();
                Song.Entities.Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == st.Org_ID).ToFirst<Organization>();
                if (org != null)
                {
                    exr.Org_ID = org.Org_ID;
                    exr.Org_Name = org.Org_Name;
                }
                Gateway.Default.Save<ExamResults>(exr);               
                return exr;
            }
            else
            {
                if (exr.Exr_IsCalc) return exr;
                //if (exr.Exr_Results == result.Exr_Results) return;
                exr.Exr_SubmitTime = DateTime.Now;
                exr.Exr_Results = result.Exr_Results;
                exr.Exr_IsSubmit = result.Exr_IsSubmit;
                exr.Exr_IP = result.Exr_IP;
                exr.Exr_Mac = result.Exr_Mac;
                Gateway.Default.Save<ExamResults>(exr);
            }
            //如果交卷，则删除缓存
            if (exr.Exr_IsSubmit)          
                Cache.ExamResultsCache.Delete(exr);
       
            return exr;
        }
        /// <summary>
        /// 删除考试成绩
        /// </summary>
        /// <param name="id"></param>
        public void ResultDelete(int id)
        {
            ExamResults exr = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exr_ID == id).ToFirst<ExamResults>();
            if (exr == null) return;

            List<ExamResults> results = Gateway.Default.From<ExamResults>()
                .Where(ExamResults._.Exam_ID == exr.Exam_ID && ExamResults._.Ac_ID == exr.Ac_ID)
                .ToList<ExamResults>();
            if (results.Count <= 1) ResultDelete(exr.Ac_ID, exr.Exam_ID);
            else
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
                Cache.ExamResultsCache.Delete(examid, exam.Tp_Id, stid);
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
        /// 删除考试下的所有成绩
        /// </summary>
        /// <param name="examid">考试id</param>
        public void ResultClear(int examid)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {                   
                    Cache.ExamResultsCache.Delete(examid);
                    tran.Delete<ExamResults>(ExamResults._.Exam_ID == examid);

                    //删除上传的附件
                    string filepath = Upload.Get["Exam"].Physics + examid;
                    if (System.IO.Directory.Exists(filepath)) System.IO.Directory.Delete(filepath, true);
                 
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
        /// 获取最新的答题信息（正式答题信息），获取时并不进行计算状态的判断
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="acid">考生id</param>
        /// <returns></returns>
        public ExamResults ResultSingle(int examid, long tpid, int acid)
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
        public ExamResults ResultForCache(int examid, long tpid, int acid)
        {
            ExamResults r = Cache.ExamResultsCache.GetResults(examid, tpid, acid);
            if (r == null || r.Exr_IsSubmit || r.Exr_OverTime > DateTime.Now) r = this.ResultSingle(examid, tpid, acid);
            return r;
        }
        /// <summary>
        /// 更新答题信息缓存
        /// </summary>
        /// <param name="exr"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public string ResultCacheUpdate(ExamResults exr, int expires)
        {
            return Cache.ExamResultsCache.Update(exr, expires);
        }
        /// <summary>
        /// 答题缓存的数量
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <returns></returns>
        public int ResultCacheCount(int examid)
        {
            return Cache.ExamResultsCache.Count(examid);
        }
        /// <summary>
        /// 学员在某个考试场次的得分
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <param name="acid">学员id</param>
        /// <returns></returns>
        public double? ResultScore(int acid, int examid)
        {
            object obj = Gateway.Default.Max<ExamResults>(ExamResults._.Exr_ScoreFinal, ExamResults._.Exam_ID == examid && ExamResults._.Ac_ID == acid);
            if (obj == null || obj.GetType().FullName == "System.DBNull") return null;
            double tm = obj is DBNull ? 0 : Convert.ToDouble(obj);
            tm = Math.Round(Math.Round(tm * 10000) / 10000, 2, MidpointRounding.AwayFromZero);
            return tm;
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
            //是否考试结束，或已经交卷
            wc.And(ExamResults._.Exr_OverTime < DateTime.Now || ExamResults._.Exr_IsSubmit == true);
            ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToArray<ExamResults>(count);
            for (int i = 0; i < exr.Length; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc == false)
                    exr[i] = ResultClacScore(exr[i]);
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
            //是否考试结束，或已经交卷
            wc.And(ExamResults._.Exr_OverTime < DateTime.Now || ExamResults._.Exr_IsSubmit == true);
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
                    next = ResultClacScore(exr);
            }
            return next;
        }
        public ExamResults ResultSingle(int exrid)
        {
            Song.Entities.ExamResults exr = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exr_ID == exrid).ToFirst<ExamResults>();
            if (exr != null)
            {
                if (exr.Exr_Score < 0 || exr.Exr_IsCalc == false)
                    exr = ResultClacScore(exr);
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
                long id = Convert.ToInt64(s[i]);
                Song.Entities.Questions q = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
                if (q == null) continue;
                quesList.Add(q);
            }
            //设置该题分数
            foreach (Song.Entities.Questions q in quesList)
            {
                for (int i = 0; i < nodeList.Count; i++)
                {
                    float num = 0;
                    float.TryParse(nodeList[i].Attributes["num"].Value, out num);
                    long id = 0;
                    long.TryParse(nodeList[i].Attributes["id"].Value,out id);                
                    if (q.Qus_ID == id)
                    {
                        q.Qus_Number = num;
                        break;
                    }
                }
            }
            return quesList;
        }
        #endregion


        #region 私有方法        
        /// <summary>
        /// 通过答题的XML信息，计算成绩
        /// </summary>
        /// <param name="resultXML"></param>
        /// <returns></returns>
        public static double _ClacScore(string resultXML,out string outXml)
        {
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null; 
            resXml.LoadXml(resultXML, false);
            XmlNode root = resXml.LastChild;
            long sbjid = 0;
            long.TryParse(root.Attributes["sbjid"] != null ? root.Attributes["sbjid"].Value : "0", out sbjid);
            var info = new
            {
                examid = Convert.ToInt64(root.Attributes["examid"].Value == null ? "0" : root.Attributes["examid"].Value),
                tpid = Convert.ToInt64(root.Attributes["tpid"].Value),
                stid = Convert.ToInt32(root.Attributes["stid"].Value),
                stname = root.Attributes["stname"].Value,
                sbjid = sbjid,
                sbjname = root.Attributes["sbjname"] != null ? root.Attributes["sbjname"].Value : "",
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
                        id = Convert.ToInt64(qnode[j].Attributes["id"].Value),
                        num = Convert.ToSingle(qnode[j].Attributes["num"].Value),
                        ans = type == 1 || type == 2 || type == 3 ? qnode[j].Attributes["ans"].Value : qnode[j].InnerText
                    };
                    //是否正确
                    float quesScore = quesCom.CalcScore(ques.id, ques.ans, ques.num);
                    score += quesScore;
                    ((XmlElement)qnode[j]).SetAttribute("sucess", (quesScore== ques.num).ToString().ToLower());
                    ((XmlElement)qnode[j]).SetAttribute("score", quesScore.ToString());
                    //如果错了，则记录为错题，以方便复习
                    if (quesScore != ques.num)
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
                        studentCom.QuesAdd(sq);
                    }
                }

            }
            #endregion
            outXml = resXml.InnerXml;
            return score;
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
            //下述Sql语句，兼容Sqlserver,postgresql,sqlite
            string sql = @"select  sts.""Sts_ID"", ""Sts_Name"",exr.count as Sts_Count from ""StudentSort"" as sts  inner join 
           (select sts_id, COUNT(*) as count from
                (select ""Ac_ID"", max(""Sts_ID"") as Sts_ID from ""ExamResults"" where  ""Exam_ID"" in
                    (
                        select ""Exam_ID"" from ""Examination""

                        where

                        ""Exam_UID"" in (select ""Exam_UID"" from ""Examination"" where ""Exam_ID"" = {0})

                        and ""Exam_IsTheme"" = false
                    )  group by ""Ac_ID""
				)as ac group by sts_id)  as exr
            on sts.""Sts_ID"" = exr.""sts_id"" order by sts.""Sts_Tax"" asc";
            sql = string.Format(sql, id.ToString());
            if (Gateway.Default.DbType != DbProviderType.PostgreSQL)
                sql = sql.Replace("true", "1").Replace("false", "0");
            return Gateway.Default.FromSql(sql).ToArray<StudentSort>();
        }
        /// <summary>
        /// 考试场次下的学员组
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public List<StudentSort> ResultSort4Exam(int examid)
        {
            //下述Sql语句，兼容Sqlserver,postgresql,sqlite
            string sql = @"select sts.""Sts_ID"", ""Sts_Name"",exr.count as ""Sts_Count"" from ""StudentSort"" as sts  inner join 
                        (select ""Sts_ID"", COUNT(*) as count from ""ExamResults"" where ""ExamResults"".""Exam_ID"" = {0} group by ""Sts_ID"") as exr
                        on sts.""Sts_ID"" = exr.""Sts_ID"" order by sts.""Sts_Tax"" asc";
            sql = string.Format(sql, examid);
            return Gateway.Default.FromSql(sql).ToList<StudentSort>();
        }
        /// <summary>
        /// 未参加考试的学员的学员组
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <returns></returns>
        public List<StudentSort> AbsenceSort4Exam(int examid)
        {
            //下述Sql语句，兼容Sqlserver,postgresql,sqlite
            string sql = @"select ""StudentSort"".""Sts_ID"", ""Sts_Name"",acc.count as ""Sts_Count"" from ""StudentSort"" inner join 
          (SELECT ""Accounts"".""Sts_ID"",COUNT(*) as count FROM ""Accounts"" WHERE 
                NOT EXISTS (SELECT ""ExamResults"".""Ac_ID"" FROM ""ExamResults"" WHERE ""ExamResults"".""Exam_ID"" = {0} and ""ExamResults"".""Ac_ID"" = ""Accounts"".""Ac_ID"")
             GROUP BY ""Accounts"".""Sts_ID"") as acc
        on ""StudentSort"".""Sts_ID"" = acc.""Sts_ID"" order by ""StudentSort"".""Sts_Tax"" asc";
            sql = string.Format(sql, examid);
            return Gateway.Default.FromSql(sql).ToList<StudentSort>();
        }
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="examid">当前考试主题的ID</param>
        /// <param name="stsid">学生分组的id，为0时取所有，为-1时取不在组的学员，大于0则取当前组学员</param>
        /// <returns></returns>
        public DataTable Result4Theme(int examid, long stsid)
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
            WhereClip wc = new WhereClip();
            foreach (Examination item in exams)           
                wc.Or(ExamResults._.Exam_ID == item.Exam_ID);           
            if (stsid > 0) wc.And(ExamResults._.Sts_ID == stsid);   //取所有已分组的学员
            if (stsid < 0) wc.And(ExamResults._.Sts_ID <= 0);   //取所有未分组的学员
            ExamResults[] results = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_ScoreFinal.Desc).ToArray<ExamResults>();
            //计算成绩
            for (int i = 0; i < results.Length; i++)
            {
                if (results[i].Exr_Score < 0 || results[i].Exr_IsCalc == false)
                    results[i] = ResultClacScore(results[i]);
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
        public DataTable Result4Theme(int id, string stsid)
        {
            DataTable dtFirst = null; 
            foreach (string s in stsid.Split(','))
            {
                if (string.IsNullOrWhiteSpace(s)) continue;
                int sid = 0;
                int.TryParse(s, out sid);
                //if (sid <= 0) continue;
                //取每个组的学员的考试成绩
                DataTable dtSecond = this.Result4Theme(id, sid);
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
        public DataTable Result4Theme(int id, long stsid, bool isAll)
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
                    results[i] = ResultClacScore(results[i]);
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
            //下述Sql语句，兼容Sqlserver,postgresql,sqlite
            //当前考试下的所有学员分组
            StudentSort[] sts = null;
            string sql = @"select distinct sts.* from ""StudentSort"" as sts 
                            inner join
                            (select* from ""ExamResults"" where ""Exam_ID"" ={0}) as exr
                            on sts.""Sts_ID"" = exr.""Sts_ID""";
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
        /// <param name="uid">考试唯一标识</param>
        /// <returns></returns>
        public double PassRate4Theme(string uid)
        {
            Examination[] exam = this.ExamItem(uid);
            double rate = 0;
            foreach (Examination ex in exam) rate += this.PassRate4Exam(ex);
            return rate / exam.Length;
        }
        /// <summary>
        /// 计算某场考试的及极率
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public double PassRate4Exam(int examid)
        {
         
            Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Exam_ID == examid).ToFirst<Examination>();
            if (exam == null) return 0;
            return this.PassRate4Exam(exam);
        }
        /// <summary>
        /// 计算某场考试的及极率
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        public double PassRate4Exam(Examination exam)
        {
            int sum = Gateway.Default.Count<ExamResults>(ExamResults._.Exam_ID == exam.Exam_ID && ExamResults._.Exr_ScoreFinal >= 0);
            if (sum < 1) return 0;
            TestPaper tp = Gateway.Default.From<TestPaper>().Where(TestPaper._.Tp_Id == exam.Tp_Id).ToFirst<TestPaper>();
            if (tp == null) return 0;
            int pass = Gateway.Default.Count<ExamResults>(ExamResults._.Exam_ID == exam.Exam_ID && ExamResults._.Exr_ScoreFinal >= tp.Tp_Total * 0.6);
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
        /// <param name="examid"></param>
        /// <returns></returns>
        public double Avg4Exam(int examid)
        {
            object obj = Gateway.Default.Avg<ExamResults>(ExamResults._.Exr_ScoreFinal, ExamResults._.Exam_ID == examid);
            if (obj == null || obj.GetType().FullName == "System.DBNull") return 0;
            double tm = obj is DBNull ? 0 : Convert.ToDouble(obj);
            tm = Math.Round(Math.Round(tm * 10000) / 10000, 2, MidpointRounding.AwayFromZero);
            return tm;
        }
        /// <summary>
        /// 某场考试的最高分
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public double Highest4Exam(int examid)
        {
            object obj = Gateway.Default.Max<ExamResults>(ExamResults._.Exr_ScoreFinal, ExamResults._.Exam_ID == examid);
            if (obj == null || obj.GetType().FullName == "System.DBNull") return 0;
            double tm = obj is DBNull ? 0 : Convert.ToDouble(obj);
            tm = Math.Round(Math.Round(tm * 10000) / 10000, 2, MidpointRounding.AwayFromZero);
            return tm;
        }
        /// <summary>
        /// 某场考试的最低分
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public double Lowest4Exam(int examid)
        {
            object obj = Gateway.Default.Min<ExamResults>(ExamResults._.Exr_ScoreFinal, ExamResults._.Exam_ID == examid);
            if (obj == null || obj.GetType().FullName == "System.DBNull") return 0;
            double tm = obj is DBNull ? 0 : Convert.ToDouble(obj);
            tm = Math.Round(Math.Round(tm * 10000) / 10000, 2, MidpointRounding.AwayFromZero);
            return tm;
        }       
        /// <summary>
        /// 参加考试主题的学员列表
        /// </summary>
        /// <param name="examid">考试主题的id</param>
        /// <param name="name"></param>
        /// <param name="idcard"></param>
        /// <param name="stsid"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Accounts> AttendThemeAccounts(int examid, string name, string idcard, long stsid, int size, int index, out int countSum)
        {
            //下述Sql语句，兼容Sqlserver,postgresql,sqlite
            //当前考试主题下的所有参考学员
            string sql = @"select ""Ac_ID"",max(""Ac_Name"") as Ac_Name,MAX(""Ac_Sex"") as ac_sex,MAX(""Ac_IDCardNumber"") as Ac_IDCardNumber,
                            MAX(""Exr_OverTime"") as Exr_OverTime, MAX(""Sts_ID"") as Sts_ID
            from ""ExamResults"" where {where} and ({examid}) group by ""Ac_ID""";
            //考试id的判断条件            
            Examination[] items = this.ExamItem(examid);
            //if (items.Length < 1) return null;
            string exam_id = string.Empty;
            for (int i = 0; items != null && i < items.Length; i++)
                exam_id += @"""Exam_ID""=" + items[0].Exam_ID + (i < items.Length - 1 ? " or " : "");
            sql = sql.Replace("{examid}", string.IsNullOrWhiteSpace(exam_id) ? "1=0" : exam_id);

            //查询条件
            string where = " {stsid} and {name} and {idcard}";
            where = where.Replace("{name}", string.IsNullOrWhiteSpace(name) ? "1=1" : @"""Ac_Name"" LIKE '%" + name + "%'");
            where = where.Replace("{idcard}", string.IsNullOrWhiteSpace(idcard) ? "1=1" : @"""Ac_IDCardNumber"" LIKE '%" + idcard + "%'");
            if (stsid > 0) where = where.Replace("{stsid}", @"""Sts_ID""=" + stsid);
            else if (stsid == -1) where = where.Replace("{stsid}", @"""Sts_ID""=0");
            else where = where.Replace("{stsid}", "1=1");
            sql = sql.Replace("{where}", where);

            //数据库类型不同，造成的差异
            if (Gateway.Default.DbType == DbProviderType.SQLServer) sql = sql.Replace("true", "1").Replace("false", "0");
            if (Gateway.Default.DbType == DbProviderType.PostgreSQL) sql = sql.Replace("LIKE", "ILIKE");

            //计算总数
            string total = "select COUNT(*) as count from ( " + sql + ") as t ";
            object obj = Gateway.Default.FromSql(total).ToScalar();
            countSum = obj == null ? 0 : Convert.ToInt32(obj);
            //查询结果
            if (Gateway.Default.DbType == DbProviderType.SQLServer)
            {
                int start = (index - 1) * size;
                int end = (index - 1) * size + size;
                string result = "select * from (select ROW_NUMBER() OVER(Order by Exr_OverTime desc) AS 'rowid',* from ( " + sql + ") as t ) as n where  rowid > {{start}} and rowid<={{end}}";
                result = result.Replace("{{start}}", start.ToString());
                result = result.Replace("{{end}}", end.ToString());
                return Gateway.Default.FromSql(result).ToList<Accounts>();
            }
            else
            {
                string result = "select * from ( " + sql + ") as t  LIMIT  {{size}} OFFSET {{index}}";
                result = result.Replace("{{size}}", size.ToString());
                result = result.Replace("{{index}}", ((index - 1) * size).ToString());
                return Gateway.Default.FromSql(result).ToList<Accounts>();
            }
        }

        /// <summary>
        /// 某个考试场次所缺考的学员列表
        /// </summary>
        /// <param name="examid">考试场次的id</param>
        /// <param name="name">学员姓名</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="stsid">学员组id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Accounts> AbsenceExamAccounts(int examid, string name, string idcard, string phone, long stsid, int size, int index, out int countSum)
        {
            Examination exam = this.ExamSingle(examid);
            if (!exam.Exam_IsTheme) exam= this.ExamSingle(exam.Exam_UID);
            //查询条件
            WhereClip wc = new WhereClip();
            //如果参考人员为按学员组
            if (exam.Exam_GroupType == 2)
            {
                StudentSort[] sts = this.GroupForStudentSort(exam.Exam_UID);
                foreach (StudentSort ss in sts) wc.Or(ExamResults._.Sts_ID == ss.Sts_ID);
            }
            if (!string.IsNullOrWhiteSpace(name)) wc.And(Accounts._.Ac_Name.Contains(name));
            if (!string.IsNullOrWhiteSpace(idcard)) wc.And(Accounts._.Ac_IDCardNumber.Contains(idcard));
            if (!string.IsNullOrWhiteSpace(phone)) wc.And(Accounts._.Ac_MobiTel1.Contains(phone));
            if (stsid > 0) wc.And(Accounts._.Sts_ID == stsid);
            if (stsid < 0) wc.And(Accounts._.Sts_ID == 0);

            //子查询条件
            WhereClip wcsub = ExamResults._.Exam_ID == examid && ExamResults._.Ac_ID == Accounts._.Ac_ID;
            QuerySection<ExamResults> querysub = Gateway.Default.From<ExamResults>().Where(wcsub).Select(ExamResults._.Ac_ID);
            wc.And(WhereClip.NotExists(querysub));

            //主查询
            QuerySection<Accounts> query = Gateway.Default.From<Accounts>().Where(wc);
            countSum = query.Count();   //记录总数
            //查询
            List<Accounts> list= query.ToList<Accounts>(size, (index - 1) * size);
            foreach (var ac in list)
            {
                if (ac.Ac_Birthday > DateTime.Now.AddYears(-100))
                {
                    ac.Ac_Age = (int)((DateTime.Now - ac.Ac_Birthday).TotalDays / (365 * 3 + 366) * 4);
                    ac.Ac_Age = ac.Ac_Age > 150 ? 0 : ac.Ac_Age;
                }
                else if (ac.Ac_Age > 0) ac.Ac_Age = DateTime.Now.Year - ac.Ac_Age;
            }
            return list;
        }
        /// <summary>
        /// 当前考试场次下的学员成绩
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="name">学员姓名</param>
        /// <param name="idcard">身份证号</param>
        /// <param name="stsid">学员组ID</param>
        /// <param name="min">按分数区间获取记录，此处是最低分</param>
        /// <param name="max">最高分</param>
        /// <param name="manual">是否批阅</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<ExamResults> ResultsPager(int examid, string name, string idcard, long stsid, float min, float max, bool? manual, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if(examid>0) wc.And(ExamResults._.Exam_ID == examid);
            if (stsid > 0) wc.And(ExamResults._.Sts_ID == stsid);
            if (stsid < 0) wc.And(ExamResults._.Sts_ID == 0);
            if (min >= 0) wc.And(ExamResults._.Exr_ScoreFinal >= min);         
            if (max >= 0)wc.And(ExamResults._.Exr_ScoreFinal <= max);
            //是否考试结束，或已经交卷
            wc.And(ExamResults._.Exr_OverTime < DateTime.Now || ExamResults._.Exr_IsSubmit == true);           

            if (manual != null) wc.And(ExamResults._.Exr_IsManual == (bool)manual);
            if (!string.IsNullOrWhiteSpace(name)) wc.And(ExamResults._.Ac_Name.Contains(name));           
            if (!string.IsNullOrWhiteSpace(idcard)) wc.And(ExamResults._.Ac_IDCardNumber.Contains(idcard));
            countSum = Gateway.Default.Count<ExamResults>(wc);
            List<ExamResults> exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_LastTime.Desc).ToList<ExamResults>(size, (index - 1) * size);
            for (int i = 0; i < exr.Count; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc == false)
                    exr[i] = ResultClacScore(exr[i]);
            }
            return exr;
        }

        /// <summary>
        /// 当前考试场次下的所有人员成绩
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public List<ExamResults> Results(int examid, int count)
        {
            WhereClip wc = ExamResults._.Exam_ID == examid;
            List<ExamResults> exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_CrtTime.Desc).ToList<ExamResults>();
            for (int i = 0; i < exr.Count; i++)
            {
                if (exr[i].Exr_Score < 0 || exr[i].Exr_IsCalc == false)
                    exr[i] = ResultClacScore(exr[i]);
            }
            exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_ScoreFinal.Desc).ToList<ExamResults>(count);
            return exr;
        }

        #region 成绩导出
        /// <summary>
        /// 某场考试的考试成绩按学员组导出
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="examid">考试场次id</param>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public string ExportResults4Exam(string filePath, int examid, long[] sorts)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Core.App.Get["ExcelInputConfig"].VirtualPath + "考试成绩.xml";
            xmldoc.Load(WeiSha.Core.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //            
            if (sorts == null || sorts.Length < 1)
            {
                //用考试名称，创建工作表对象
                Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Exam_ID == examid).ToFirst<Examination>();
                ISheet sheet = hssfworkbook.CreateSheet(exam.Exam_Name);
                //生成数据行
                ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(ExamResults._.Exam_ID == examid).OrderBy(ExamResults._.Exr_LastTime.Desc).ToArray<ExamResults>();
                setSheet(exr, sheet, nodes);
            }
            else
            {
                //考试主题下的所有参考人员（分过组的）成绩          
                foreach (long sid in sorts)
                {
                    StudentSort sts = studentCom.SortSingle(sid);
                    if (sts == null) continue;
                    WhereClip wc = new WhereClip();
                    wc.And(ExamResults._.Exam_ID == examid && ExamResults._.Sts_ID == sid);
                    ExamResults[] exr = Gateway.Default.From<ExamResults>().Where(wc).OrderBy(ExamResults._.Exr_LastTime.Desc).ToArray<ExamResults>();
                    if (exr.Length < 1) continue;
                    ISheet sheet = hssfworkbook.CreateSheet(sts.Sts_Name);
                    setSheet(exr, sheet, nodes);
                }
            }
            FileStream file = new FileStream(filePath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return filePath;
        }
        #region 导出成绩所用的私有方法
        /// <summary>
        /// 生成考试成绩的工作表
        /// </summary>
        /// <param name="exr"></param>
        /// <param name="sheet"></param>
        /// <param name="nodes"></param>
        private void setSheet(ExamResults[] exr, ISheet sheet, XmlNodeList nodes)
        {
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            for (int i = 0; i < exr.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                Type exrRef = exr[i].GetType();           //ExamResults对象的反射对象
                //当前学员对象
                Accounts account = accountsCom.AccountsSingle(exr[i].Ac_ID);
                Type accRef = account == null ? null : account.GetType();        //学员对象的反射对象
                for (int j = 0; j < nodes.Count; j++)
                {
                    object obj = null;
                    //是否需要计算
                    string calc = nodes[j].Attributes["Calc"] != null ? nodes[j].Attributes["Calc"].Value : "";
                    if (calc == "TimeSpan")
                    {
                        DateTime d1 = exr[i].Exr_CrtTime; //考试开始时间
                        DateTime d2 = exr[i].Exr_IsSubmit ? exr[i].Exr_SubmitTime : exr[i].Exr_LastTime;     //最后答题时间
                        d2 = exr[i].Exr_OverTime > d2 ? d2 : exr[i].Exr_OverTime;
                        if (!(d1 == null || d2 == null))
                            obj = Math.Round((d2 - d1).TotalMinutes);      //考试用时（分钟）   
                    }
                    else
                    {
                        //获取考试成绩表ExamResults的字段属性
                        System.Reflection.PropertyInfo propertyInfo = exrRef.GetProperty(nodes[j].Attributes["Field"].Value);
                        try
                        {
                            obj = propertyInfo.GetValue(exr[i], null);
                        }
                        catch
                        {
                            //取学员表Accounts的字段的属性值
                            if (accRef != null)
                            {
                                System.Reflection.PropertyInfo accproper = accRef.GetProperty(nodes[j].Attributes["Field"].Value); //获取指定名称的属性
                                obj = accproper.GetValue(account, null);
                            }
                        }
                    }
                    if (obj == null) continue;
                    this.setCellValue(obj, nodes[j], row.CreateCell(j));
                }
            }
            for (int i = 0; i < nodes.Count; i++)
                sheet.AutoSizeColumn(i);
        }
        /// <summary>
        /// 设置考试成绩的导出的单元格数据
        /// </summary>
        /// <param name="obj">单元格的值</param>
        /// <param name="node">节点类型</param>
        /// <param name="cell">单元格</param>
        private void setCellValue(object obj, XmlNode node, ICell cell)
        {
            string format = node.Attributes["Format"] != null ? node.Attributes["Format"].Value : "";
            string datatype = node.Attributes["DataType"] != null ? node.Attributes["DataType"].Value : "";
            string defvalue = node.Attributes["DefautValue"] != null ? node.Attributes["DefautValue"].Value : "";
            string value = "";
            switch (datatype)
            {
                case "date":
                    DateTime tm = Convert.ToDateTime(obj);
                    value = tm > DateTime.Now.AddYears(-30) ? tm.ToString(format) : "";
                    cell.SetCellValue(value);
                    break;
                case "float":
                    float f = 0;
                    float.TryParse(obj.ToString(), out f);
                    cell.SetCellValue(f);
                    break;
                case "int":
                    int integer = 0;
                    int.TryParse(obj.ToString(), out integer);
                    cell.SetCellValue(integer);
                    break;
                default:
                    value = obj.ToString();
                    cell.SetCellValue(value);
                    break;
            }
            //如果有默认值的特殊指定
            if (!string.IsNullOrWhiteSpace(defvalue))
            {
                foreach (string s in defvalue.Split('|'))
                {
                    string h = s.Substring(0, s.IndexOf("="));
                    string f = s.Substring(s.LastIndexOf("=") + 1);
                    if (value.ToLower() == h.ToLower()) value = f;
                }
                cell.SetCellValue(value);
            }
        }
        #endregion
        /// <summary>
        /// 考试主题下的所有成绩
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="examid">考试主题的id</param>
        /// <param name="sorts">学员组</param>
        /// <returns></returns>
        public string ExportResults4Theme(string filePath, int examid, long[] sorts)
        {
            //如果没有指定学员组，则取所有学员组
            if (sorts == null || sorts.Length < 1)
            {
                StudentSort[] list = this.StudentSort4Theme(examid);
                sorts = new long[list.Length];
                for (int i = 0; i < list.Length; i++)              
                    sorts[i] = list[i].Sts_ID;
            }
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //考试主题下的所有参考人员（分过组的）成绩
            foreach (long sid in sorts)
            {
                StudentSort sts = studentCom.SortSingle(sid);
                if (sts == null) continue;
                DataTable dt = this.Result4Theme(examid, sts.Sts_ID);
                if (dt.Rows.Count < 1) continue;
                ISheet sheet = hssfworkbook.CreateSheet(sts.Sts_Name);   //创建工作簿对象                
                IRow rowHead = sheet.CreateRow(0);          //创建数据行对象                
                for (int i = 0; i < dt.Columns.Count; i++)      //创建表头
                    rowHead.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
                ICellStyle style_size = hssfworkbook.CreateCellStyle();     //生成数据行
                style_size.WrapText = true;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //此处是两个数值（用$分隔），前面是成绩，后面是成绩记的id
                        string val = dt.Rows[i][j].ToString();
                        if (!string.IsNullOrWhiteSpace(val) && val.IndexOf("$") > -1)
                        {
                            val = val.Substring(0, val.LastIndexOf("$"));
                            double score = 0;
                            double.TryParse(val, out score);
                            row.CreateCell(j).SetCellValue(score);
                        }
                        else
                            row.CreateCell(j).SetCellValue(val);                      
                    }
                }
            }

            FileStream file = new FileStream(filePath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return filePath;
        }
        /// <summary>
        /// 导出某场考试的缺考人员
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="examid">考试场次id</param>
        /// <returns></returns>
        public string ExportAbsences4Exam(string filePath, int examid)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Core.App.Get["ExcelInputConfig"].VirtualPath + "学生信息.xml";
            xmldoc.Load(WeiSha.Core.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //
            //用考试名称，创建工作表对象
            Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Exam_ID == examid).ToFirst<Examination>();
            //ISheet sheet = hssfworkbook.CreateSheet(exam.Exam_Name);
            //生成数据行
            int total = 0;
            List<Accounts> accounts = this.AbsenceExamAccounts(examid, string.Empty, string.Empty, string.Empty, 0, int.MaxValue, 1, out total);           
           AccountsCom._export4Excel_to_sheet(hssfworkbook, exam.Exam_Name, accounts, nodes);

            FileStream file = new FileStream(filePath, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return filePath;
        }
        /// <summary>
        /// 学员在某个课程下的考试成绩
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="acid"></param>
        /// <returns>返回成绩记录</returns>
        public ExamResults StudentForCourseExam(long couid, int acid)
        {
            List<TestPaper> paper = _getCahceTestPaper(couid);
            if (paper == null || paper.Count < 1) return null;
            //
            ExamResults result = null;
            foreach (TestPaper tp in paper)
            {
                ExamResults tm = Gateway.Default.From<ExamResults>().Where(ExamResults._.Tp_Id == tp.Tp_Id && ExamResults._.Ac_ID == acid).ToFirst<ExamResults>();
                if (tm == null) continue;
                if (result == null) result = tm;
                if (tm.Exr_ScoreFinal > result.Exr_ScoreFinal)
                    result = tm;
            }
            return result;
        }
        //**临时方法
        //获取试卷信息，因为查询量大，把试卷信息放到缓存中读取
        private List<TestPaper> _getCahceTestPaper(long couid)
        {
            string cachekey = "Temporary_TestPaper_List";
            List<Song.Entities.TestPaper> list = null;
            System.Web.Caching.Cache cache = System.Web.HttpRuntime.Cache;

            object cachevalue = cache.Get(cachekey);
            if (cachevalue != null)
            {
                list = cachevalue as List<Song.Entities.TestPaper>;
            }
            else
            {
                list = Gateway.Default.From<TestPaper>().ToList<TestPaper>();
                //缓存两天失效
                cache.Insert(cachekey, list, null, DateTime.MaxValue, TimeSpan.FromSeconds(60 * 60 * 24 * 2));
            }
            //查询
            List< TestPaper> papers = new List<TestPaper>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Cou_ID == couid)
                {
                    papers.Add(list[i]);
                    break;
                }
            }
            return papers;
        }
        #endregion

        #region 统计
        /// <summary>
        /// 考试主题下允许参考的学员数量
        /// </summary>
        /// <param name="examid">考试主题的ID</param>
        /// <returns></returns>
        public int NumberOfStudent(int examid)
        {
            Examination exam = this.ExamSingle(examid);
            return NumberOfStudent(exam);
        }
        /// <summary>
        /// 考试主题下允许参考的学员数量
        /// </summary>
        /// <param name="exam">考试主题的ID</param>
        /// <returns></returns>
        public int NumberOfStudent(Examination exam)
        {
            if (!exam.Exam_IsTheme) exam = Gateway.Default.From<Examination>().Where(Examination._.Exam_IsTheme == true && Examination._.Exam_UID == exam.Exam_UID).ToFirst<Examination>();
            //先计算需要参加考试的人数
            int total = 0;
            //如果参考人员为全体学员
            if (exam.Exam_GroupType == 1) total = Gateway.Default.Count<Accounts>(Accounts._.Org_ID == exam.Org_ID);
            //按学员组计算
            if (exam.Exam_GroupType == 2)
            {
                StudentSort[] sts = this.GroupForStudentSort(exam.Exam_UID);
                WhereClip wc = new WhereClip();
                foreach (StudentSort ss in sts) wc.Or(Accounts._.Sts_ID == ss.Sts_ID);
                total = Gateway.Default.Count<Accounts>(wc);
            }
            return total;
        }
        /// <summary>
        /// 当前考试的参考人次，如果学员多次考试，则人次大于人数
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public int Numbertimes4Exam(int examid)
        {
            return Gateway.Default.Count<ExamResults>(ExamResults._.Exam_ID == examid);
        }
        /// <summary>
        /// 当前考试的参考人数
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <returns></returns>
        public int Number4Exam(int examid)
        {
            return Gateway.Default.From<ExamResults>().Where(ExamResults._.Exam_ID == examid).Select(ExamResults._.Ac_ID)
                .GroupBy(ExamResults._.Ac_ID.Group).Count();
        }
        /// <summary>
        /// 当前考试的缺考的人数
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public int NumberAbsence4Exam(int examid)
        {
            //先计算需要参加考试的人数
            int total = this.NumberOfStudent(examid);
            //已经参加考试的人数
            int examnum = this.Number4Exam(examid);
            return total - examnum;
        }
        /// <summary>
        /// 当前考试主题的参考人次，如果学员多次考试，则人次大于人数
        /// </summary>
        /// <param name="examid">考试主题的id</param>
        /// <returns></returns>
        public int Numbertimes4Theme(int examid)
        {
            Examination exam = this.ExamSingle(examid);
            if (!exam.Exam_IsTheme) return this.Numbertimes4Exam(examid);
            //考试主题下的所有考试场次
            Examination[] exams = this.ExamItem(exam.Exam_UID);
            if (exams.Length < 1) return 0;
            WhereClip wc = new WhereClip();
            foreach (Examination em in exams) wc.Or(ExamResults._.Exam_ID == em.Exam_ID);
            return Gateway.Default.Count<ExamResults>(wc);
        }
        /// <summary>
        /// 当前考试的参考人数
        /// </summary>
        /// <param name="examid">考试主题的id</param>
        /// <returns></returns>
        public int Number4Theme(int examid)
        {
            Examination exam = this.ExamSingle(examid);
            if (!exam.Exam_IsTheme) return this.Number4Exam(examid);
            //考试主题下的所有考试场次
            Examination[] exams = this.ExamItem(exam.Exam_UID);
            if (exams.Length < 1) return 0;
            WhereClip wc = new WhereClip();
            foreach (Examination em in exams) wc.Or(ExamResults._.Exam_ID == em.Exam_ID);
            //
            return Gateway.Default.From<ExamResults>().Where(wc).Select(ExamResults._.Ac_ID)
                .GroupBy(ExamResults._.Ac_ID.Group).Count();
        }
        /// <summary>
        /// 当前考试的缺考的人数
        /// </summary>
        /// <param name="examid">考试主题的id</param>
        /// <returns></returns>
        public int NumberAbsence4Theme(int examid)
        {
            //先计算需要参加考试的人数
            int total = this.NumberOfStudent(examid);
            //已经参加考试的人数
            int examnum = this.Number4Theme(examid);
            return total - examnum;
        }
        #endregion
    }
}
