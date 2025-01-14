﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 课程管理
    /// </summary>
    public interface ICourse : WeiSha.Core.IBusinessInterface
    {
        #region 课程管理
        /// <summary>
        /// 添加课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CourseAdd(Course entity);
        /// <summary>
        /// 批量添加课程，可用于导入时
        /// </summary>
        /// <param name="teacher">当前教师，即发布课程的教师</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="names">名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        Course CourseBatchAdd(Teacher teacher, int orgid, long  sbjid, string names);        
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CourseSave(Course entity);
        /// <summary>
        /// 修改课程的某些项
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool CourseUpdate(long couid, Field[] fields, object[] objs);
        /// <summary>
        /// 修改课程的某些项
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool CourseUpdate(long couid, Field field, object obj);
        /// <summary>
        /// 增加课程浏览数
        /// </summary>
        /// <param name="entity">课程</param>
        /// <param name="num"></param>
        /// <returns></returns>
        int CourseViewNum(Course entity, int num);
        /// <summary>
        /// 增加课程浏览数
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        int CourseViewNum(long couid, int num);
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CourseDelete(Course entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="couid">课程的主键id</param>
        void CourseDelete(long couid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Course CourseSingle(long identify);
        /// <summary>
        /// 获取单一实体对象，按UID；
        /// </summary>
        /// <param name="uid">唯一值</param>
        /// <returns></returns>
        Course CourseSingle(string uid);
        /// <summary>
        /// 获取课程名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="couid">课程的主键id</param>
        /// <returns></returns>
        string CourseName(long couid);
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        List<Course> CourseAll(int orgid, long  sbjid, int thid, bool? isUse);
        /// <summary>
        /// 某个课程的学习人数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="isAll">是否取全部值，如果为false，则仅取当前正在学习的</param>
        /// <returns></returns>
        int CourseStudentSum(long couid, bool? isAll);
        /// <summary>
        /// 清理课程内容
        /// </summary>
        /// <param name="course">课程实体</param>
        /// <param name="isfreshData">是否刷新相关数据，例如课程内容清空了，专业的试题是否重新统计</param>
        void CourseClear(Course course,bool isfreshData);
        /// <summary>
        /// 清除课程的内容
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="isfreshData">是否刷新相关数据，例如课程内容清空了，专业的试题是否重新统计</param>
        void CourseClear(long couid, bool isfreshData);
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="sbjid">专业id，等于0取所有</param>
        /// <param name="thid">教师id</param>
        /// <param name="pid">上级课程ID</param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        List<Course> CourseCount(int orgid, long  sbjid, int thid, int pid, string sear, bool? isUse, int count);
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        List<Course> CourseCount(int orgid, long  sbjid, string sear, string order, bool? isUse, int count);
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
        List<Course> CourseCount(int orgid, long  sbjid, int thid, bool? islive, string sear, bool? isUse, int count);
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="sbjid">专业id，等于0取所有</param>
        /// <param name="sear"></param>
        /// <param name="isUse"></param>
        /// <param name="order">排序方式，def:默认，先推荐，然后按访问量倒序;flux：按访问量倒序;tax：按自定义排序要求;new:按创建时间，最新发布在前面;rec:按推荐，先推荐，然后按tax排序</param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Course> CourseCount(int orgid, long  sbjid, string sear, bool? isUse, string order, int count);
        /// <summary>
        /// 课程数量,如果计算专业下的课程数，会计算专业所有子级专业的课程数
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isuse">是否包括启用的课程,null取所有，true取启用的，false取未启用的</param>
        /// <param name="isfree">是否免费</param>
        /// <returns></returns>
        int CourseOfCount(int orgid, long sbjid, int thid, bool? isuse, bool? isfree);
        /// <summary>
        /// 专业下的课程数，包括启用、不启用的，所有课程
        /// </summary>
        /// <param name="sbjid"></param>
        /// <returns></returns>
        int CourseOfCount(long  sbjid);
        /// <summary>
        /// 统计机构或专业下的课程数，并保存到机构或专业表
        /// </summary>
        /// <returns></returns>
        void CourseCountUpdate(int orgid,long sbjid);
        /// <summary>
        /// 更新课程的统计数据，包括课程的章节数、试题数等
        /// </summary>
        /// <param name="orgid">机构id，如果大于0，则刷新当前机构下的所有专业数据</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        bool UpdateStatisticalData(int orgid, long couid);
        /// <summary>
        /// 当前课程下是否有子级
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool CourseIsChildren(int orgid, long couid, bool? isUse);
       
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="thid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="order">排序方式，默认null按排序顺序，flux流量最大优先</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, long  sbjid, int thid, bool? isUse, string searTxt, string order, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id,多个id用逗号分隔</param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="searTxt"></param>
        /// <param name="order">排序方式，
        /// def:默认，按创建时间倒序;
        /// flux：按访问量倒序;
        /// tax：按自定义排序要求;
        /// new:按创建时间，最新发布在前面;
        /// last:按创建时间，倒序，即最发布在前面
        /// rec:按推荐，先推荐，然后按tax排序
        /// quesAsc:试题最少的排前面
        /// quesDesc：试题最多排前面
        /// videoAsc：视频最少排前面
        /// videoDesc：视频最多的排前面
        /// </param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, string sbjid, int thid, bool? isUse, string searTxt, string order, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isLive">是否是直播课</param>
        /// <param name="isFree">是否免费</param>
        /// <param name="searTxt">按课程名称检查</param>
        /// <param name="order">排序方式</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, string sbjid, int thid, bool? isUse, bool? isLive,bool? isFree, string searTxt, string order, int size, int index, out int countSum);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool CourseUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool CourseDown(int id);

        #endregion

        #region 课程的状态
        /// <summary>
        /// 课程是否已经存在
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="pid">上级id</param>
        /// <param name="name"></param>
        /// <returns></returns>
        Course Exist(int orgid, long sbjid, long pid, string name);
        /// <summary>
        /// 是否为直播课
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        bool IsLiveCourse(long couid);
        /// <summary>
        /// 是否为直播课
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="check">校验，如果为true，则检索课程下所有章节，有直播章节，则课程为直播课程</param>
        /// <returns></returns>
        bool IsLiveCourse(long couid, bool check);
        /// <summary>
        /// 课程是否在有视频
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        bool ExistVideo(long couid);
        /// <summary>
        /// 课程是否在有试题
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        bool ExistQuestion(long couid);
        #endregion

        #region 课程购买        
        /// <summary>
        /// 购买课程
        /// </summary>
        /// <param name="stc">学生与课程的关联对象</param>
        Student_Course Buy(Student_Course stc);
        /// <summary>
        /// 购买课程
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="price">价格项</param>
        /// <returns></returns>
        Student_Course Buy(int stid, long couid, Song.Entities.CoursePrice price);
        /// <summary>
        /// 购买课程
        /// </summary>
        /// <param name="account">学员账号</param>
        /// <param name="couid">课程id</param>
        /// <param name="price">价格项</param>
        /// <returns></returns>
        Student_Course Buy(Accounts account, long couid, Song.Entities.CoursePrice price);
        /// <summary>
        /// 免费学习
        /// </summary>
        /// <param name="stid">学习ID</param>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        Student_Course FreeStudy(int stid, long couid);
        /// <summary>
        /// 免费学习
        /// </summary>
        /// <param name="stid">学习ID</param>
        /// <param name="couid">课程ID</param>
        /// <param name="start">免费时效的开始时间，如果为空，则不记录开始时间</param>
        /// <param name="end">免费时效的结束时间</param>
        /// <returns></returns>
        Student_Course FreeStudy(int stid, long couid, DateTime? start, DateTime end);
        /// <summary>
        /// 学员是否购买了该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员Id</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <returns>返回课程</returns>
        Course IsBuyCourse(long couid, int stid, int state);
        /// <summary>
        /// 学员是否购买了该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        /// <returns></returns>
        bool IsBuy(long couid, int stid);
        /// <summary>
        /// 课程试用，默认试用一百年
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        Student_Course Tryout(int stid, long couid);
        /// <summary>
        /// 是否试用该课程
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stid"></param>
        /// <returns></returns>
        bool IsTryout(long couid, int stid);
        /// <summary>
        /// 是可以直接学习该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        /// <returns>如果是免费或限时免费、或试学的课程，可以学习并返回true，不可学习返回false</returns>
        bool AllowStudy(long couid, int stid);
        /// <summary>
        /// 是可以直接学习该课程
        /// </summary>
        /// <param name="course">课程对象</param>
        /// <param name="acc">学员账号对象</param>
        /// <returns></returns>
        bool AllowStudy(Course course, Accounts acc);
        /// <summary>
        /// 学生与课程的关联记录项
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        Student_Course StudentCourse(int stid, long couid);
        /// <summary>
        /// 学生与课程的关联记录项
        /// </summary>
        /// <param name="stcid">记录项的主键id</param>
        /// <returns></returns>
        Student_Course StudentCourse(int stcid);
        /// <summary>
        /// 学生与课程的关联记录项，如果autoCreate为true，当没有关联项时，且课程为免费状态，可以自动创建关联
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="autoCreate">是否自动创建关联记录项</param>
        /// <returns></returns>
        Student_Course StudentCourse(int stid, long couid, bool autoCreate);
        /// <summary>
        /// 直接开课，创建学员与课程的关联信息
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="couid">课程id</param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        int BeginCourse(int stid, DateTime start, DateTime end, long couid, int orgid);
        /// <summary>
        /// 更新学员购买课程的记录的信息
        /// </summary>
        /// <param name="sc"></param>
        /// <returns></returns>
        Student_Course StudentCourseUpdate(Student_Course sc);
        /// <summary>
        /// 保存学员的成绩记录
        /// </summary>
        /// <param name="sc"></param>
        /// <param name="study">学习记录，即视频观看进度</param>
        /// <param name="ques">试题练习记录，通过率</param>
        /// <param name="exam">结课考试的成绩</param>
        void StudentScoreSave(Student_Course sc, float study, float ques, float exam);        
        /// <summary>
        /// 取消课程学习，直接删除购买记录
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        void DeleteCourseBuy(int stid, long couid);
        /// <summary>
        /// 学员购买的该课程,以及学员组关联的课程，按到期时间倒序
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="sear">用于检索的字符</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <param name="enable">是否启用</param>
        /// <param name="istry">是否试用，为null时取所有</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CourseForStudent(int stid, string sear, int state, bool? enable, bool? istry, int size, int index, out int countSum);
        /// <summary>
        /// 学员购买的该课程,以及学员组关联的课程,不分页，不排序
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="sear"></param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <param name="enable">是否启用</param>
        /// <param name="istry"></param>
        /// <returns></returns>
        List<Course> CourseForStudent(int stid, string sear, int state, bool? enable, bool? istry);
        /// <summary>
        /// 获取收入最多的课程
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> RankIncome(int orgid, long sbjid, DateTime? start, DateTime? end, int size, int index, out int countSum);
        /// <summary>
        /// 课程收益
        /// </summary>
        /// <param name="couid"></param>
        /// <returns></returns>
        decimal Income(long couid);
        /// <summary>
        /// 课程收益汇总，按时间区间汇总
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        decimal Income(int orgid, long sbjid, DateTime? start, DateTime? end);
        #endregion

        #region 学习成果计算
        /// <summary>
        /// 计算学员的综合成绩
        /// </summary>
        /// <param name="sc">学员选修记录的实体</param>
        Student_Course ResultScoreCalc(Student_Course sc);
        /// <summary>
        /// 计算学员的综合成绩
        /// </summary>
        /// <param name="stcid">学员选修记录的主键id</param>
        Student_Course ResultScoreCalc(int stcid);
        /// <summary>
        /// 计算学员的综合成绩
        /// </summary>
        /// <param name="stid">学员账号id</param>
        /// <returns></returns>
        bool ResultScoreCalc4Student(int stid);
        /// <summary>
        /// 计算选修课程学员的综合成绩
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        bool ResultScoreCalc4Course(long couid);
        /// <summary>
        /// 批量计算学习关联课程的综合成绩，只有学员参与学习了才会有成绩
        /// </summary>
        /// <param name="lcsid">学习卡设置的id</param>
        /// <returns></returns>
        bool ResultScoreCalc4LearningCard(int lcsid);
        #endregion

        #region 课程关联管理（与学生或教师）        
        /// <summary>
        /// 获取选学人数最多的课程列表，从多到少
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> RankHot(int orgid, long sbjid, DateTime? start, DateTime? end, int size, int index, out int countSum);
        /// <summary>
        /// 某个学生是否正在学习某个课程
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        bool StudyForCourse(int stid, long couid);       
        /// <summary>
        /// 获取某个教师关联的课程
        /// </summary>
        /// <param name="thid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Course> Course4Teacher(int thid, int count);
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
        Accounts[] Student4Course(long couid, string stname, string stmobi, int size, int index, out int countSum);

        /// <summary>
        /// 快要过期的课程
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="day">剩余几天内的</param>
        /// <returns></returns>
        List<Student_Course> OverdueSoon(int acid, int day);
     
        #endregion

        #region 价格管理
        /// <summary>
        /// 添加价格记录
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PriceAdd(CoursePrice entity);
        /// <summary>
        /// 修改价格记录
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PriceSave(CoursePrice entity);
        /// <summary>
        /// 删除价格记录
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PriceDelete(CoursePrice entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void PriceDelete(int identify);
        /// <summary>
        /// 删除，按全局唯一标识
        /// </summary>
        /// <param name="uid"></param>
        void PriceDelete(string uid);
        /// <summary>
        /// 将产品价格写入到课程所在的表，取第一条价格
        /// </summary>
        /// <param name="uid">课程UID</param>
        void PriceSetCourse(string uid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        CoursePrice PriceSingle(int identify);
        /// <summary>
        /// 获取价格记录
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="uid"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        CoursePrice[] PriceCount(long couid, string uid, bool? isUse, int count);
        /// <summary>
        /// 更改价格记录的排序
        /// </summary>
        /// <param name="items">课程价格设置项</param>
        /// <returns></returns>
        bool PriceUpdateTaxis(Song.Entities.CoursePrice[] items);
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
        DataTable StudentLogPager(long couid,long stsid, string acc, string name, string idcard, string mobi,
            DateTime? start, DateTime? end, int size, int index, out int countSum);
        /// <summary>
        /// 当前课程的学员（即学习该课程的学员），并计算出完成度,导出为excel
        /// </summary>
        /// <param name="filepath">要导出的excel文件物理路径</param>
        /// <param name="course"></param>
        /// <param name="start">学员选修课程的开始时间的区间</param>
        /// <param name="end">学员选修课程的开始时间的区间</param>
        /// <returns>要导出的excel文件物理路径</returns>
        string StudentLogToExcel(string filepath, Course course, DateTime? start, DateTime? end);

        
        #endregion

        #region 统计信息
        /// <summary>
        /// 视频文件的存储大小
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isreal">是否真实大小，如果为true，则去硬盘验证是否存在该视频，并以物理文件大小计算文件大小；如果为false则以数据库记录的文件大小计算</param>
        /// <param name="count">视频个数</param>
        /// <returns>视频文件总大小，单位为字节</returns>
        long StorageVideo(int orgid, bool isreal, out int count);
        /// <summary>
        /// 课程图文资源存储大小，不包括视频
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isreal">是否真实大小，如果为true，则去硬盘验证是否存在该视频，并以物理文件大小计算文件大小；如果为false则以数据库记录的文件大小计算</param>
        /// <param name="count">视频个数</param>
        /// <returns>视频文件总大小，单位为字节</returns>
        long StorageResources(int orgid, bool isreal, out int count);
        #endregion

    }
}
