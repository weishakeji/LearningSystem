using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 学员的管理
    /// </summary>
    public interface IStudent : WeiSha.Core.IBusinessInterface
    {
        #region 学员分类（组）管理
        /// <summary>
        /// 添加学员分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SortAdd(StudentSort entity);
        /// <summary>
        /// 修改学员分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SortSave(StudentSort entity);
        /// <summary>
        /// 修改学员组的状态
        /// </summary>
        /// <param name="stsid">学员组id</param>
        /// <param name="use">是否启用</param>
        /// <returns></returns>
        bool SortUpdateUse(long stsid, bool use);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns>如果删除成功，返回0；如果组包括学员，返回-1；如果是默认组，返回-2</returns>
        int SortDelete(long identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        StudentSort SortSingle(long identify);
        /// <summary>
        /// 根据学员组名称获取学员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        StudentSort SortSingle(string name,int orgid);
        /// <summary>
        /// 获取默认学员组
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        StudentSort SortDefault(int orgid);
        /// <summary>
        /// 设置默认学员分类
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify"></param>
        /// <returns></returns>
        void SortSetDefault(int orgid, long identify);
        /// <summary>
        /// 获取对象；即所有学员组；
        /// </summary>
        /// <returns></returns>
        StudentSort[] SortAll(int orgid, bool? isUse);
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<StudentSort> SortCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// 获取某网站学员所属的组；
        /// </summary>
        /// <param name="studentId">学员id</param>
        /// <returns></returns>
        StudentSort Sort4Student(int studentId);
        /// <summary>
        /// 获取某个组的所有网站学员
        /// </summary>
        /// <param name="stsid">分类id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Accounts[] Student4Sort(long stsid, bool? isUse);
        /// <summary>
        /// 学员组中的学员数量
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        int SortOfNumber(long stsid);
        /// <summary>
        /// 更新学员组中的学员数量
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        void SortUpdateNumber(long stsid);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool SortIsExist(StudentSort entity);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="name">学员组名称</param>
        /// <param name="id">学员组id</param>
        /// <param name="orgid">所在机构id</param>
        /// <returns></returns>
        bool SortIsExist(string name, long id, int orgid);
        /// <summary>
        /// 更改学员组的排序
        /// </summary>
        /// <param name="items">学员组的实体数组</param>
        /// <returns></returns>
        bool SortUpdateTaxis(Song.Entities.StudentSort[] items);
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
        StudentSort[] SortPager(int orgid, bool? isUse, string name, int size, int index, out int countSum);
        #endregion

        #region 学员组与课程
        /// <summary>
        /// 增加学员组与课程的关联
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        int SortCourseAdd(long stsid, long couid);
        /// <summary>
        /// 增加学员组与课程的关联
        /// </summary>
        /// <param name="ssc"></param>
        /// <returns></returns>
        int SortCourseAdd(StudentSort_Course ssc);
        /// <summary>
        /// 修改学员组与课程的关联
        /// </summary>
        /// <param name="ssc"></param>
        /// <returns></returns>
        int SortCourseSave(StudentSort_Course ssc);
        /// <summary>
        /// 学员组关联的课程数
        /// </summary>
        /// <param name="stsid"></param>
        /// <returns></returns>
        int SortCourseCount(long stsid);
        /// <summary>
        /// 删除学员组与课程的关联
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        bool SortCourseDelete(long stsid, long couid);
        /// <summary>
        /// 判断某个课程是否存在于学员组
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stsid">学员组id</param>
        /// <returns></returns>
        bool SortExistCourse(long couid, long stsid);
        /// <summary>
        /// 将学员组关联的课程，创建到Student_Course表（学员与课程的关联）
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="acid">学员账号id</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        Student_Course SortCourseToStudent(int acid, long couid);
        /// <summary>
        /// 将学员组关联的课程，创建到Student_Course表（学员与课程的关联）
        /// </summary>
        /// <param name="acc">学员账号</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        Student_Course SortCourseToStudent(Accounts acc, long couid);
        /// <summary>
        /// 学员组关联的所有课程
        /// </summary>
        /// <param name="stsid">学员组的id</param>
        /// <param name="name">按名称检索</param>
        /// <returns></returns>
        List<Course> SortCourseList(long stsid, string name);
        /// <summary>
        /// 分页获取学员组关联的课程
        /// </summary>
        /// <param name="stsid"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> SortCoursePager(long stsid, string name, int size, int index, out int countSum);
        #endregion

        #region 学员登录与在线记录
        /// <summary>
        /// 添加登录记录
        /// </summary>
        /// <returns></returns>
        void LogForLoginAdd(Accounts st);
        /// <summary>
        /// 修改登录记，刷新一下登录信息，例如在线时间
        /// </summary>
        /// <param name="interval">数据提交的间隔时间，也是每次提交的增加的在线时间数，单位秒</param>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        void LogForLoginFresh(Accounts st, int interval, string plat);
        /// <summary>
        /// 退出登录之前的记录更新
        /// </summary>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        void LogForLoginOut(Accounts st, string plat);
        /// <summary>
        /// 根据学员id与登录时生成的Uid返回实体
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="stuid">登录时生成的随机字符串，全局唯一</param>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        /// <returns></returns>
        LogForStudentOnline LogForLoginSingle(int stid, string stuid, string plat);
        /// <summary>
        /// 返回记录
        /// </summary>
        /// <param name="identify">记录ID</param>
        /// <returns></returns>
        LogForStudentOnline LogForLoginSingle(int identify);
        /// <summary>
        /// 删除学员在线记录
        /// </summary>
        /// <param name="identify"></param>
        void StudentOnlineDelete(int identify);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="stid">学员Id</param>
        /// <param name="platform">学员文章平台，PC或Mobi</param>
        /// <param name="start">统计的开始时间</param>
        /// <param name="end">统计的结束时间</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentOnline[] LogForLoginPager(int orgid, int stid, string platform, DateTime? start, DateTime? end, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="stid"></param>
        /// <param name="platform"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="stname">学员名称</param>
        /// <param name="stmobi">学员手机号</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentOnline[] LogForLoginPager(int orgid, int stid, string platform, DateTime? start, DateTime? end, string stname, string stmobi, int size, int index, out int countSum);
        #endregion

        #region 学员视频学习的记录        
        /// <summary>
        /// 记录学员学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">章节id</param>
        /// <param name="st">学员账户</param>
        /// <param name="playTime">播放进度</param>
        /// <param name="studyInterval">学习时间，此为时间间隔，每次提交学习时间加这个数</param>
        /// <param name="totalTime">视频总长度</param>
        void LogForStudyFresh(long couid, long olid, Accounts st, int playTime, int studyInterval, int totalTime);
        /// <summary>
        /// 记录学员学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">章节id</param>
        /// <param name="st">学员账户</param>
        /// <param name="playTime">播放进度</param>
        /// <param name="studyTime">学习时间，此为累计时间</param>
        /// <param name="totalTime">视频总长度</param>
        /// <returns>学习进度百分比（相对于总时长），如果为-1，则表示失败</returns>
        void LogForStudyUpdate(long couid, long olid, Accounts st, int playTime, int studyTime, int totalTime);
        /// <summary>
        /// 根据学员id与章节id
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int stid, long olid);
        /// <summary>
        /// 返回记录
        /// </summary>
        /// <param name="identify">记录ID</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int identify);
        /// <summary>
        /// 返回学习记录
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="stid">学员id</param>
        /// <param name="platform">平台，PC或Mobi</param>
        /// <param name="count"></param>
        /// <returns></returns>
        LogForStudentStudy[] LogForStudyCount(int orgid, long couid, long olid, int stid, string platform, int count);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="stid">学员Id</param>
        /// <param name="platform">学员文章平台，PC或Mobi</param>    
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LogForStudentStudy[] LogForStudyPager(int orgid, long couid, long olid, int stid, string platform, int size, int index, out int countSum);
        /// <summary>
        /// 学员所有学习课程的记录
        /// </summary>
        /// <param name="stid"></param>
        /// <returns>datatable中LastTime列为学习时间；studyTime：学习时间</returns>
        DataTable StudentStudyCourseLog(int stid);
        /// <summary>
        /// 学员指定学习课程的记录
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couids">课程id,逗号分隔</param>
        /// <returns></returns>
        DataTable StudentStudyCourseLog(int stid, string couids);
        /// <summary>
        /// 学员所有学习某一课程的记录
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        DataTable StudentStudyCourseLog(int stid, long couid);
        /// <summary>
        /// 学员学习某一课程下所有章节的记录
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员账户id</param>
        /// <returns>datatable中，LastTime：最后学习时间；totalTime：视频时间长；playTime：播放进度；studyTime：学习时间，complete：完成度百分比</returns>
        DataTable StudentStudyOutlineLog(long couid, int stid);
        /// <summary>
        /// 章节学习记录作弊，直接将学习进度设置为100
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        void CheatOutlineLog(int stid, long olid);
        #endregion

        #region 学员的错题回顾
        /// <summary>
        /// 添加添加学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        void QuesAdd(Student_Ques entity);
        /// <summary>
        /// 修改学员的错题
        /// </summary>
        /// <param name="entity">业务实体</param>
        void QuesSave(Student_Ques entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        void QuesDelete(int identify);
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <param name="stid">学员id</param>
        void QuesDelete(long quesid, int stid);
        /// <summary>
        /// 清空错题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        /// <returns>清除的题数</returns>
        int QuesClear(long couid, int stid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Student_Ques QuesSingle(int identify);
        /// <summary>
        /// 当前学员的所有错题
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid"></param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Questions[] QuesAll(int stid, long sbjid, long couid, int type);
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid"></param>
        /// <param name="type">试题类型</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] QuesCount(int stid, long sbjid, long couid, int type, int count);
        /// <summary>
        /// 学员错题的个数
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">专业 id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        int QuesOfCount(int stid, long sbjid, long couid, int type);
        /// <summary>
        /// 高频错题
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="type">题型</param>
        /// <param name="count">取多少条</param>
        /// <returns>试题的完整结构+count列，取试题的错误次数</returns>
        Questions[] QuesOftenwrong(long couid, int type, int count);
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid"></param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难易度</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] QuesPager(int stid, long sbjid, long couid, int type, int diff, int size, int index, out int countSum);
        /// <summary>
        /// 错题所属的课程
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couname">课程名称，可模糊查询</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Course[] QuesForCourse(int stid, string couname, int size, int index, out int countSum);
        #endregion

        #region 学员的试题收藏
        /// <summary>
        /// 添加添加学员收藏的试题
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CollectAdd(Student_Collect entity);
        /// <summary>
        /// 修改学员收藏的试题
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CollectSave(Student_Collect entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        void CollectDelete(int identify);
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="stid"></param>
        void CollectDelete(long quesid, int stid);
        /// <summary>
        /// 清空试题收藏
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        void CollectClear(long couid, int stid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Student_Collect CollectSingle(int identify);
        /// <summary>
        /// 获取单一实体，通过学员和试题id
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        Student_Collect CollectSingle(int acid, long qid);
        /// <summary>
        /// 当前学员收藏的试题
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Questions[] CollectAll4Ques(int stid, long sbjid, long couid, int type);
        Student_Collect[] CollectAll(int stid, long sbjid, long couid, int type);
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Questions[] CollectCount(int stid, long sbjid, long couid, int type, int count);
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难易度</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] CollectPager(int stid, long sbjid, long couid, int type, int diff, int size, int index, out int countSum);
        #endregion

        #region 学员的笔记
        /// <summary>
        /// 添加添加学员的笔记
        /// </summary>
        /// <param name="entity">业务实体</param>
        void NotesAdd(Student_Notes entity);
        /// <summary>
        /// 修改学员的笔记
        /// </summary>
        /// <param name="entity">业务实体</param>
        void NotesSave(Student_Notes entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        void NotesDelete(int identify);
        /// <summary>
        /// 删除，按试题id与试题id
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="stid"></param>
        void NotesDelete(long quesid, int stid);
        /// <summary>
        /// 清空试题笔记
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        void NotesClear(long couid, int stid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Student_Notes NotesSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按试题id、学员id
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <param name="stid">学员id</param>
        /// <returns></returns>
        Student_Notes NotesSingle(long quesid, int stid);
        /// <summary>
        /// 当前学员的所有笔记
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Student_Notes[] NotesAll(int stid, int type);
        /// <summary>
        /// 取当前学员的笔记
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] NotesCount(int stid, long couid, int type, int count);
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="type">试题类型</param>
        /// <param name="count">数量</param>
        /// <returns></returns>
        Questions[] NotesCount(int stid, int type, int count);
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="quesid">试题id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Student_Notes[] NotesPager(int stid, long quesid, string searTxt, int size, int index, out int countSum);
        #endregion
    }
}
