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
    public interface IStudent : WeiSha.Common.IBusinessInterface
    {
        #region 学员分类管理
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
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns>如果删除成功，返回0；如果组包括学员，返回-1；如果是默认组，返回-2</returns>
        int SortDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        StudentSort SortSingle(int identify);
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
        void SortSetDefault(int orgid, int identify);
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
        StudentSort[] SortCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// 获取某网站学员所属的组；
        /// </summary>
        /// <param name="StudentId">网站学员id</param>
        /// <returns></returns>
        StudentSort Sort4Student(int studentId);
        /// <summary>
        /// 获取某个组的所有网站学员
        /// </summary>
        /// <param name="sortid">分类id</param>
        /// <returns></returns>
        Accounts[] Student4Sort(int sortid, bool? isUse);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool SortIsExist(StudentSort entity);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SortRemoveUp(int orgid, int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SortRemoveDown(int orgid, int id);
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

        //#region 学员管理
        ///// <summary>
        ///// 添加学员
        ///// </summary>
        ///// <param name="entity">业务实体</param>
        ///// <returns>如果已经存在该学员，则返回-1</returns>
        //int StudentAdd(Student entity);
        ///// <summary>
        ///// 修改学员
        ///// </summary>
        ///// <param name="entity">业务实体</param>
        //void StudentSave(Student entity);
        ///// <summary>
        ///// 删除，按主键ID；
        ///// </summary>
        ///// <param name="identify">实体的主键</param>
        //void StudentDelete(int identify);        
        ///// <summary>
        ///// 删除，按网站学员帐号名
        ///// </summary>
        ///// <param name="accname">网站学员账号</param>
        ///// <param name="orgid"></param>
        //void StudentDelete(string accname, int orgid);
        ///// <summary>
        ///// 删除学员
        ///// </summary>
        ///// <param name="entity"></param>
        //void StudentDelete(Song.Entities.Student entity);
        ///// <summary>
        ///// 删除学员
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <param name="tran"></param>
        //void StudentDelete(Song.Entities.Student entity, DbTrans tran);
        ///// <summary>
        ///// 获取单一实体对象，按主键ID；
        ///// </summary>
        ///// <param name="identify">实体的主键</param>
        ///// <returns></returns>
        //Student StudentSingle(int identify);
        ///// <summary>
        ///// 获取单一实体，按账号
        ///// </summary>
        ///// <param name="accname"></param>
        ///// <param name="orgid"></param>
        ///// <returns></returns>
        //Student StudentSingle(string accname, int orgid);
        ///// <summary>
        ///// 获取单一实体对象，按网站学员名称
        ///// </summary>
        ///// <param name="name">帐号名称</param>
        ///// <returns></returns>
        //Student StudentSingle(string accname, string pw, int orgid);
        ///// <summary>
        ///// 获取单一实体，通过id与验证码
        ///// </summary>
        ///// <param name="id">学员Id</param>
        ///// <param name="uid">学员登录时产生随机字符，用于判断同一账号不同人登录的问题</param>
        ///// <returns></returns>
        //Student StudentSingle(int id, string uid);
        ///// <summary>
        ///// 登录验证
        ///// </summary>
        ///// <param name="acc">账号，或身份证，或手机</param>
        ///// <param name="pw">密码（明文，未经md5加密）</param>
        ///// <param name="orgid"></param>
        ///// <returns></returns>
        //Student StudentLogin(string acc, string pw, int orgid, bool? isPass);
        ///// <summary>
        ///// 登录判断
        ///// </summary>
        ///// <param name="accid">学员id</param>
        ///// <param name="pw">密码，md5加密后的</param>
        ///// <param name="orgid"></param>
        ///// <param name="isPass"></param>
        ///// <returns></returns>
        //Student StudentLogin(int accid, string pw, int orgid, bool? isPass);
        ///// <summary>
        ///// 当前用帐号是否重名
        ///// </summary>
        ///// <param name="name">学员帐号</param>
        ///// <returns></returns>
        //bool IsStudentExist(int orgid, string accname);
        ///// <summary>
        ///// 判断学员是否已经在存，将判断账号与手机号
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <param name="enity"></param>
        ///// <returns></returns>
        //bool IsStudentExist(int orgid, Student enity);
        ///// <summary>
        ///// 当前用帐号是否重名
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <param name="accname"></param>
        ///// <param name="answer">安全问题答案</param>
        ///// <returns></returns>
        //bool IsStudentExist(int orgid, string accname, string answer);
        ///// <summary>
        ///// 获取对象；即所有网站学员；
        ///// </summary>
        ///// <returns></returns>
        //Student[] StudentAll(int orgid, bool? isUse);
        ///// <summary>
        ///// 获取学员
        ///// </summary>
        ///// <param name="orgid">机构id</param>
        ///// <param name="isUse"></param>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //Student[] StudentCount(int orgid, bool? isUse, int count);
        ///// <summary>
        ///// 计算有多少学员
        ///// </summary>
        ///// <param name="orgid"></param>
        ///// <param name="isUse"></param>
        ///// <returns></returns>
        //int StudentOfCount(int orgid, bool? isUse);
        ///// <summary>
        ///// 导出Excel格式的学员信息
        ///// </summary>
        ///// <param name="path">导出文件的路径（服务器端）</param>
        ///// <param name="orgid">机构id</param>
        ///// <param name="sortid">学员分组id，小于0为全部</param>
        ///// <returns></returns>
        //string StudentExport4Excel(string path, int orgid, int sortid);
        ///// <summary>
        ///// 分页获取所有的网站学员帐号；
        ///// </summary>
        ///// <param name="size">每页显示几条记录</param>
        ///// <param name="index">当前第几页</param>
        ///// <param name="countSum">记录总数</param>
        ///// <returns></returns>
        //Student[] StudentPager(int orgid, int size, int index, out int countSum);
        ///// <summary>
        ///// 分页获取某学员组，所有的网站学员帐号；
        ///// </summary>
        ///// <param name="orgid">机构id</param>
        ///// <param name="sortid">学员分类id</param>
        ///// <param name="isUse"></param>
        ///// <param name="name">学员名称</param>
        ///// <param name="phone">学员账号</param>
        ///// <param name="size"></param>
        ///// <param name="index"></param>
        ///// <param name="countSum"></param>
        ///// <returns></returns>
        //Student[] StudentPager(int orgid, int? sortid, bool? isUse, string name, string phone, int size, int index, out int countSum);
        //#endregion

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
        void LogForLoginFresh(int interval, string plat);
        /// <summary>
        /// 退出登录之前的记录更新
        /// </summary>
        /// <param name="plat">设备名称，PC为电脑端，Mobi为手机端</param>
        void LogForLoginOut(string plat);
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

        #region 学员在线学习的记录        
        /// <summary>
        /// 记录学员学习时间
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid">章节id</param>
        /// <param name="st">学员账户</param>
        /// <param name="playTime">播放进度</param>
        /// <param name="studyInterval">学习时间，此为时间间隔，每次提交学习时间加这个数</param>
        /// <param name="totalTime">视频总长度</param>
        void LogForStudyFresh(int couid, int olid, Accounts st, int playTime, int studyInterval, int totalTime);
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
        double LogForStudyUpdate(int couid, int olid, Accounts st, int playTime, int studyTime, int totalTime);
        /// <summary>
        /// 根据学员id与登录时生成的Uid返回实体
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int stid, int olid);
        /// <summary>
        /// 返回记录
        /// </summary>
        /// <param name="identify">记录ID</param>
        /// <returns></returns>
        LogForStudentStudy LogForStudySingle(int identify);
        /// <summary>
        /// 返回学习记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="stid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        LogForStudentStudy[] LogForStudyCount(int orgid, int couid, int olid, int stid, string platform, int count);   
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
        LogForStudentStudy[] LogForStudyPager(int orgid, int couid, int olid, int stid, string platform, int size, int index, out int countSum);
        /// <summary>
        /// 学员所有学习课程的记录
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="stid"></param>
        /// <returns>datatable中LastTime列为学习时间；studyTime：学习时间</returns>
        DataTable StudentStudyCourseLog(int stid);
        /// <summary>
        /// 学员所有学习某一课程的记录
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        DataTable StudentStudyCourseLog(int stid,int couid);
        /// <summary>
        /// 学员学习某一课程下所有章节的记录
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="acid">学员账户id</param>
        /// <returns>datatable中，LastTime：最后学习时间；totalTime：视频时间长；playTime：播放进度；studyTime：学习时间，complete：完成度百分比</returns>
        DataTable StudentStudyOutlineLog(int couid, int stid);
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
        void QuesDelete(int quesid, int stid);
        /// <summary>
        /// 清空错题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        void QuesClear(int couid, int stid);
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
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Questions[] QuesAll(int stid, int sbjid, int couid, int type);
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Questions[] QuesCount(int stid, int sbjid, int couid, int type, int count);
        /// <summary>
        /// 高频错题
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="type">题型</param>
        /// <param name="count">取多少条</param>
        /// <returns>试题的完整结构+count列，取试题的错误次数</returns>
        Questions[] QuesOftenwrong(int couid, int type, int count);
        /// <summary>
        /// 分页获取学员的错误试题
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难易度</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Questions[] QuesPager(int stid, int sbjid, int couid, int type, int diff, int size, int index, out int countSum);
        #endregion

        #region 学员的收藏
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
        void CollectDelete(int quesid, int stid);
        /// <summary>
        /// 清空错题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        void CollectClear(int couid, int stid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Student_Collect CollectSingle(int identify);
        /// <summary>
        /// 当前学员收藏的试题
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Questions[] CollectAll4Ques(int stid, int sbjid, int couid, int type);
        Student_Collect[] CollectAll(int stid, int sbjid, int couid, int type);
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        Questions[] CollectCount(int stid, int sbjid, int couid, int type, int count);
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
        Questions[] CollectPager(int stid, int sbjid, int couid, int type, int diff, int size, int index, out int countSum);
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
        void NotesDelete(int quesid, int stid);
        /// <summary>
        /// 清空试题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        void NotesClear(int couid, int stid);
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
        Student_Notes NotesSingle(int quesid, int stid);
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
        Questions[] NotesCount(int stid, int couid, int type, int count);
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
        Student_Notes[] NotesPager(int stid, int quesid, string searTxt, int size, int index, out int countSum);
        #endregion
    }
}
