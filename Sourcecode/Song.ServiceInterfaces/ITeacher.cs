using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 教师的管理
    /// </summary>
    public interface ITeacher : WeiSha.Core.IBusinessInterface
    {       

        #region 教师管理
        /// <summary>
        /// 添加教师
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>如果已经存在该教师，则返回-1</returns>
        int TeacherAdd(Teacher entity);
        /// <summary>
        /// 通过教师信息创建账号（用于教师账号存在，但对应的学员账号不存在的问题)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool TeacherCreateAccount(Teacher entity);
        /// <summary>
        /// 修改教师
        /// </summary>
        /// <param name="entity">业务实体</param>
        void TeacherSave(Teacher entity);
        /// <summary>
        /// 更改一些项目
        /// </summary>
        /// <param name="id">教师id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        int TeacherUpdate(int id, Field[] fiels, object[] objs);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void TeacherDelete(int identify);
        /// <summary>
        /// 删除教师
        /// </summary>
        /// <param name="entity"></param>
        void TeacherDelete(Teacher entity);
        /// <summary>
        /// 删除教师
        /// </summary>
        /// <param name="entity">教师数据实体</param>
        /// <param name="tran">事务</param>
        /// <param name="updateAccount">是否更新账号accounts表中的状态，true为更新，当教师删除后账号不具有教师角色</param>
        void TeacherDelete(Teacher entity, DbTrans tran, bool updateAccount);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Teacher TeacherSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按学员id；
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        Teacher TeacherForAccount(int accid);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="acc">账号，或身份证，或手机</param>
        /// <param name="pw">密码</param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Teacher TeacherLogin(string acc, string pw, int orgid);
        /// <summary>
        /// 获取单一实体对象，按网站教师名称
        /// </summary>
        /// <param name="name">帐号名称</param>
        /// <returns></returns>
        Teacher TeacherSingle(string accname, string pw, int orgid);
        /// <summary>
        /// 当前用帐号是否重名
        /// </summary>
        /// <param name="name">教师帐号</param>
        /// <returns></returns>
        bool IsTeacherExist(int orgid, string accname);
        bool IsTeacherExist(int orgid, Teacher entity);
        /// <summary>
        /// 获取对象；即所有网站教师；
        /// </summary>
        /// <returns></returns>
        Teacher[] TeacherAll(int orgid,bool? isUse);
        /// <summary>
        /// 获取教师
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Teacher[] TeacherCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// 获取教师人数
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int TeacherOfCount(int orgid, bool? isUse);
        /// <summary>
        /// 导出Excel格式的教师信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sortid">教师分组id，小于0为全部</param>
        /// <returns></returns>
        string TeacherExport4Excel(string path, int orgid, int sortid);
        /// <summary>
        /// 分页获取所有的网站教师帐号；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        Teacher[] TeacherPager(int orgid, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取某教师组，所有的网站教师帐号；
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thsid">教师分组id</param>
        /// <param name="isUse"></param>
        /// <param name="isShow">是否在前台显示</param>
        /// <param name="searName"></param>
        /// <param name="phone">按电话查询，包括固话与手机号</param>
        /// <param name="acc">按账号查询</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Teacher[] TeacherPager(int orgid, int thsid, bool? isUse, bool? isShow, string searName, string phone, string acc,string idcard, int size, int index, out int countSum);     
        #endregion

        #region 教师分类管理
        /// <summary>
        /// 添加学生分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SortAdd(TeacherSort entity);
        /// <summary>
        /// 修改学生分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SortSave(TeacherSort entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns>如果删除成功，返回0；如果组包括学生，返回-1；如果是默认组，返回-2</returns>
        int SortDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        TeacherSort SortSingle(int identify);
        /// <summary>
        /// 获取默认学生组
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        TeacherSort SortDefault(int orgid);
        /// <summary>
        /// 设置默认学生分类
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="identify"></param>
        /// <returns></returns>
        void SortSetDefault(int orgid, int identify);
        /// <summary>
        /// 获取对象；即所有教师职称；
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="search">检索字符，按名称检索</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        TeacherSort[] SortAll(int orgid, string search, bool? isUse);
        /// <summary>
        /// 获取指定个数的对象
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<TeacherSort> SortCount(int orgid, bool? isUse, int count);
        /// <summary>
        /// 教师数量
        /// </summary>
        /// <param name="sortid"></param>
        /// <returns></returns>
        int SortOfNumber(int sortid);
        /// <summary>
        /// 获取某网站学生所属的组；
        /// </summary>
        /// <param name="studentId">网站学生id</param>
        /// <returns></returns>
        TeacherSort Sort4Teacher(int studentId);
        /// <summary>
        /// 获取某个组的所有网站学生
        /// </summary>
        /// <param name="sortid">分类id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Teacher[] Teacher4Sort(int sortid, bool? isUse);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool SortIsExist(TeacherSort entity);
        /// <summary>
        /// 更改职称的排序
        /// </summary>
        /// <param name="items">实体数组</param>
        /// <returns></returns>
        bool SortUpdateTaxis(Song.Entities.TeacherSort[] items);
        /// <summary>
        /// 学员账号信息导出
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sorts">学员分组id，用逗号分隔</param>
        /// <param name="isall">当sorts为空时，true导所有，false导未分组的</param>
        /// <returns></returns>
        string Export4Excel(string path, int orgid, string sorts, bool isall);
        #endregion

        #region 教师履历管理
        /// <summary>
        /// 添加教师
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>如果已经存在该教师，则返回-1</returns>
        int HistoryAdd(TeacherHistory entity);
        /// <summary>
        /// 修改教师
        /// </summary>
        /// <param name="entity">业务实体</param>
        void HistorySave(TeacherHistory entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void HistoryDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        TeacherHistory HistorySingle(int identify);
        /// <summary>
        /// 获取对象；即所有网站教师；
        /// </summary>
        /// <returns></returns>
        TeacherHistory[] HistoryAll(int thid);
        /// <summary>
        /// 获取教师
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TeacherHistory[] HistoryCount(int thid, int count);        
        #endregion

        #region 教师评价管理
        /// <summary>
        /// 添加教师评价
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        int CommentAdd(TeacherComment entity);
        /// <summary>
        /// 修改教师评价信息
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CommentSave(TeacherComment entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void CommentDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        TeacherComment CommentSingle(int identify);
        /// <summary>
        /// 某天内，最近一次学员给教师的评价
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="accid">学员id</param>
        /// <param name="day">当前天数</param>
        /// <returns></returns>
        TeacherComment CommentSingle(int thid, int accid, int day);
        /// <summary>
        /// 计算教师的评分，从当前日期之前的多少天内
        /// </summary>
        /// <param name="thid">评分教师的id</param>
        /// <param name="day">从当前日期之前的多少天内，小于或等于0表示取所有</param>
        /// <returns></returns>
        double CommentScore(int thid, int day);
        /// <summary>
        /// 计算教师的评分，通过起始时间
        /// </summary>
        /// <param name="thid">评分教师的id</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        double CommentScore(int thid, DateTime? start, DateTime? end);
        /// <summary>
        /// 获取教师的评价
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TeacherComment[] CommentCount(int thid, bool? isUse, bool? isShow, int count);
        /// <summary>
        /// 教师评分排行
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="day">多少天内的排行</param>
        /// <param name="count">获取个数</param>
        /// <returns></returns>
        TeacherComment[] CommentOrder(int orgid, bool? isUse, bool? isShow, int day, int count);
        /// <summary>
        /// 某天内，学员给教师的评价数
        /// </summary>
        /// <param name="thid">教师id</param>
        /// <param name="accid">学员id</param>
        /// <param name="day">当前天数</param>
        /// <returns></returns>
        int CommentOfCount(int thid, int accid, int day);
        /// <summary>
        /// 当前机构下的所有教师评价信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TeacherComment[] CommentPager(int orgid, bool? isUse, bool? isShow, int size, int index, out int countSum);
        /// <summary>
        /// 当前机构下某个教师的评价信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thid">教师id</param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TeacherComment[] CommentPager(int orgid, int thid, bool? isUse, bool? isShow, int size, int index, out int countSum);
        /// <summary>
        /// 当前机构下某个教师的评价信息
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="thname">教师姓名</param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TeacherComment[] CommentPager(int orgid, string thname, bool? isUse, bool? isShow, int size, int index, out int countSum);
        #endregion
    }
}
