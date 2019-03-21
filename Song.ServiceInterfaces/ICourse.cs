using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 课程管理
    /// </summary>
    public interface ICourse : WeiSha.Common.IBusinessInterface
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
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="names">名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        Course CourseBatchAdd(int orgid, int sbjid, string names);
        /// <summary>
        /// 是否已经存在课程
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="pid">上级id</param>
        /// <param name="name"></param>
        /// <returns></returns>
        Course CourseIsExist(int orgid, int sbjid, int pid, string name);
        /// <summary>
        /// 修改课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CourseSave(Course entity);
        /// <summary>
        /// 增加课程浏览数
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        int CourseViewNum(Course entity, int num);
        /// <summary>
        /// 增加课程浏览数
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        int CourseViewNum(int couid, int num);
        /// <summary>
        /// 删除课程
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CourseDelete(Course entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void CourseDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Course CourseSingle(int identify);
        /// <summary>
        /// 获取课程名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        string CourseName(int identify);       
        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        List<Course> CourseAll(int orgid, int sbjid, int thid, bool? isUse);
        /// <summary>
        /// 某个课程的学习人数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="isAll">是否取全部值，如果为false，则仅取当前正在学习的</param>
        /// <returns></returns>
        int CourseStudentSum(int couid, bool? isAll);
        /// <summary>
        /// 清除课程的内容
        /// </summary>
        /// <param name="identify"></param>
        void CourseClear(int identify);
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
        List<Course> CourseCount(int orgid, int sbjid, int thid, int pid, string sear, bool? isUse, int count);
        List<Course> CourseCount(int orgid, int sbjid, string sear, bool? isUse, int count);
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
        List<Course> CourseCount(int orgid, int sbjid, string sear, bool? isUse, string order, int count);
        /// <summary>
        /// 课程数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="thid">教师id</param>
        /// <returns></returns>
        int CourseOfCount(int orgid, int sbjid, int thid);
        /// <summary>
        /// 当前课程下是否有子级
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool CourseIsChildren(int orgid, int couid, bool? isUse);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, int thid, bool? isUse, string searTxt, int size, int index, out int countSum);
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
        List<Course> CoursePager(int orgid, int sbjid, int thid, bool? isUse, string searTxt, string order, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid">专业id,多个id用逗号分隔</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="order">排序方式，def:默认，先推荐，然后按访问量倒序;flux：按访问量倒序;tax：按自定义排序要求;new:按创建时间，最新发布在前面;rec:按推荐，先推荐，然后按tax排序</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Course> CoursePager(int orgid, string sbjid, bool? isUse, string searTxt, string order, int size, int index, out int countSum);
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
        Student_Course Buy(int stid, int couid, Song.Entities.CoursePrice price);
        /// <summary>
        /// 免费学习
        /// </summary>
        /// <param name="stid">学习ID</param>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        Student_Course FreeStudy(int stid, int couid);
        /// <summary>
        /// 免费学习
        /// </summary>
        /// <param name="stid">学习ID</param>
        /// <param name="couid">课程ID</param>
        /// <param name="start">免费时效的开始时间，如果为空，则不记录开始时间</param>
        /// <param name="end">免费时效的结束时间</param>
        /// <returns></returns>
        Student_Course FreeStudy(int stid, int couid, DateTime? start, DateTime end);
        /// <summary>
        /// 学员是否购买了该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员Id</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <returns>返回课程</returns>
        Course IsBuyCourse(int couid, int stid, int state);
        /// <summary>
        /// 学员是否购买了该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        /// <returns></returns>
        bool IsBuy(int couid, int stid);
        /// <summary>
        /// 课程试用，默认试用一百年
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="couid">课程id</param>
        Student_Course Tryout(int stid, int couid);
        /// <summary>
        /// 是否试用该课程
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="stid"></param>
        /// <returns></returns>
        bool IsTryout(int couid, int stid);
        /// <summary>
        /// 直接学习该课程
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="stid">学员id</param>
        /// <returns>如果是免费或限时免费、或试学的课程，可以学习并返回true，不可学习返回false</returns>
        bool Study(int couid, int stid);
        /// <summary>
        /// 学生购买课程的记录项
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        Student_Course StudentCourse(int stid, int couid);
        /// <summary>
        /// 取消课程学习，直接删除购买记录
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        void DelteCourseBuy(int stid, int couid);        
        /// <summary>
        /// 学员购买的该课程
        /// </summary>
        /// <param name="stid">学员Id</param>
        /// <param name="sear">用于检索的字符</param>
        /// <param name="state">0不管是否过期，1必须是购买时效内的，2必须是购买时效外的</param>
        /// <param name="istry">是否试用，为null时取所有</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        List<Course> CourseForStudent(int stid, string sear, int state, bool? istry, int count);
        #endregion

        #region 课程关联管理（与学生或教师）        
        /// <summary>
        /// 获取选学人数最多的课程列表，从多到少
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        DataSet CourseHot(int orgid, int sbjid, int count);
        /// <summary>
        /// 某个学生是否正在学习某个课程
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        bool StudyIsCourse(int stid, int couid);        
        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="stid">学生Id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        void CourseBuy(int stid, int couid, float money, DateTime startTime, DateTime endTime); 
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
        Accounts[] Student4Course(int couid, string stname, string stmobi, int size, int index, out int countSum);
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
        CoursePrice[] PriceCount(int couid, string uid, bool? isUse, int count);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool PriceUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool PriceDown(int id);
        #endregion
    }
}
