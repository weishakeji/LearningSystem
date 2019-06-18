using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 考试管理
    /// </summary>
    public interface IExamination : WeiSha.Common.IBusinessInterface
    {

        #region 考试管理
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ExamAdd(Examination entity);
        /// <summary>
        /// 整体添加
        /// </summary>
        /// <param name="theme">考试主题</param>
        /// <param name="items">考试的场次</param>
        /// <param name="groups">参考人员的范围</param>
        void ExamAdd(Examination theme, List<Examination> items, List<ExamGroup> groups);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ExamSave(Examination entity);
        /// <summary>
        /// 整体修改
        /// </summary>
        /// <param name="theme">考试主题</param>
        /// <param name="items">考试的场次</param>
        /// <param name="groups">参考人员的范围</param>
        void ExamSave(Examination theme, List<Examination> items, List<ExamGroup> groups);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ExamDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；此处获取的是考试主题或场次
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Examination ExamSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，通过全局唯一值，此处获取的是考试主题
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Examination ExamSingle(string uid);
        /// <summary>
        /// 获取单一实体对象，取最近一次考试；此处获取的是考试主题或场次
        /// </summary>
        /// <returns></returns>
        Examination ExamLast();
        /// <summary>
        /// 获取当前考试的考试项目
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Examination[] ExamItem(string uid);
        Examination[] ExamItem(int id);
        /// <summary>
        /// 当前考试主题关联的学生分类
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        StudentSort[] GroupForStudentSort(string uid);  
        /// <summary>
        /// 获取考试，不分页
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Examination> ExamCount(int orgid, bool? isUse, int count);        
        /// <summary>
        /// 获取当前学生要参加的考试
        /// </summary>
        /// <param name="start">时间范围查询的开始时间</param>
        /// <param name="end">时间范围查询的结束时间</param>
        /// <returns></returns>
        List<Examination> GetSelfExam(int stid, DateTime? start, DateTime? end);
        List<Examination> GetCountExam(int stid, DateTime? start, DateTime? end, bool? isUse, int count);
        /// <summary>
        /// 判断某个考试是否允许某个学生参加
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="stid">学生id</param>
        /// <returns></returns>
        bool ExamIsForStudent(int examid, int stid);
        /// <summary>
        /// 获取指定时间内容的考试
        /// </summary>
        /// <param name="start">时间区间检索的开始时间</param>
        /// <param name="end">时间区间检索的末尾时间</param>
        /// <param name="isUse"></param>
        /// <param name="searName"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Examination[] GetPager(int orgid, DateTime? start, DateTime? end, bool? isUse, string searName, int size, int index, out int countSum);
        /// <summary>
        /// 获取当前学生参加的的考试
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="sbjid">学科id</param>
        /// <param name="orgid"></param>
        /// <param name="sear"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        ExamResults[] GetAttendPager(int stid, int sbjid, int orgid, string sear, int size, int index, out int countSum);
        #endregion

        #region 考试成绩提交等
        /// <summary>
        /// 添加考试答题信息
        /// </summary>
        /// <param name="result"></param>
        ExamResults ResultAdd(ExamResults result);
        /// <summary>
        /// 保存考试答题信息
        /// </summary>
        /// <param name="result"></param>
        void ResultSave(ExamResults result);
        /// <summary>
        /// 成绩提交
        /// </summary>
        /// <param name="result"></param>
        void ResultSubmit(ExamResults result);
        /// <summary>
        /// 计算成绩并保存
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Song.Entities.ExamResults ClacScore(ExamResults result);
        /// <summary>
        /// 删除考试成绩
        /// </summary>
        /// <param name="id">成绩记录的id</param>
        void ResultDelete(int id);
        /// <summary>
        /// 删除某个学生的某个考试的成绩
        /// </summary>
        /// <param name="stid">学员账号id</param>
        /// <param name="examid">考试id</param>
        void ResultDelete(int stid, int examid);
        /// <summary>
        /// 获取最新的答题信息（临时信息）
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="stid">考生id</param>
        /// <returns></returns>
        ExamResultsTemp ExamResultsTempSingle(int examid, int tpid, int stid);
        /// <summary>
        /// 获取最新的答题信息（正式答题信息）
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="acid">考生id</param>
        /// <returns></returns>
        ExamResults ResultSingle(int examid, int tpid, int acid);
        /// <summary>
        /// 从缓存中获取考试答题信息
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        ExamResults ResultSingleForCache(int examid, int tpid, int acid);
        /// <summary>
        /// 获取当前考试的所有考生答题信息
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ExamResults[] ResultCount(int examid, int count);
        /// <summary>
        /// 当前考试信息中，下一个
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="stid"></param>
        /// <param name="isCorrect">是否是人工判卷过的，false下一个未判卷的信息</param>
        /// <returns></returns>
        ExamResults ResultSingleNext(int examid, int stid, bool? isCorrect);
        /// <summary>
        /// 通过答案id获取答题信息（正式答题信息）
        /// </summary>
        /// <param name="exrid"></param>
        /// <returns></returns>
        ExamResults ResultSingle(int exrid);
        /// <summary>
        /// 通过学员ID与考试ID，获取成绩（最好成绩）
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="examid"></param>
        /// <returns></returns>
        ExamResults ResultSingle(int accid, int examid);
        /// <summary>
        /// 计算当前考试结果的成绩
        /// </summary>
        /// <param name="resu"></param>
        /// <returns></returns>
        ExamResults ResultClacScore(ExamResults resu);
        /// <summary>
        /// 根据答题信息，获取试题（针对答题过程中死机，又上线时）
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        List<Questions> QuesForResults(string results);
        #endregion

        #region 成绩统计
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DataTable Result4Theme(int id);
        /// <summary>
        /// 考试主题下的所有参考人员的班组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StudentSort[] StudentSort4Theme(int id);
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id">当前考试主题的ID</param>
        /// <param name="stsid">学生分组的id，为0时取所有，为-1时取不在组的学员，大于0则取当前组学员</param>
        /// <returns></returns>
        DataTable Result4Theme(int examid, int stsid);
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id">当前考试主题的ID</param>
        /// <param name="stsid">学生分组的id，为0时取所有，为-1时取不在组的学员，大于0则取当前组学员</param>
        /// <returns></returns>
        DataTable Result4Theme(int examid, string stsid);
        /// <summary>
        /// 考试主题下的所有参考人员成绩
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stsid">学生分组的id，为0时取所有，为-1时取不在组的学员，大于0则取当前组学员</param>
        /// <param name="isAll">是否取所有人员（含缺考人员）,false为仅参考人员</param>
        /// <returns></returns>
        DataTable Result4Theme(int id, int stsid, bool isAll);
        /// <summary>
        /// 当前考试主题下的各学员分组成绩排行
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        DataTable Result4StudentSort(int examid);        
        /// <summary>
        /// 计算某个考试主题的及格率
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        double PassRate4Theme(string uid);
        /// <summary>
        /// 计算某场考试的及极率
        /// </summary>
        /// <param name="exam"></param>
        /// <returns></returns>
        double PassRate4Exam(Examination exam);
        /// <summary>
        /// 计算某个考试主题的平均分
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        double Avg4Theme(string uid);
        /// <summary>
        /// 计算某场考试的平均分
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        double Avg4Exam(int examid);
        /// <summary>
        /// 当前考试的参考人数
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        int Number4Exam(int examid);
        /// <summary>
        /// 当前考试场次下的所有人员成绩
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        ExamResults[] Results(int examid, int size, int index, out int countSum);
        ExamResults[] Results(string examuid, int size, int index, out int countSum);
        /// <summary>
        /// 当前考试场次下的所有人员成绩
        /// </summary>
        /// <param name="examid">考试场次id</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        ExamResults[] Results(int examid, int count);
        #endregion
    }
}
