using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 日志的管理
    /// </summary>
    public interface ILogs : WeiSha.Common.IBusinessInterface
    {
        #region 管理登录日志
        /// <summary>
        /// 增加登录日志
        /// </summary>
        void AddLoginLogs();
        /// <summary>
        /// 增加操作日志
        /// </summary>
        void AddOperateLogs();
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(Logs entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 根据分类、对象id删除
        /// </summary>
        /// <param name="accId">用户id</param>
        void Delete4Acc(int accId);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Logs GetSingle(int identify);
        /// <summary>
        /// 清理多少天之前的日志
        /// </summary>
        /// <param name="day">天数</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        void Clear(int day, string type);
        /// <summary>
        /// 获取某用户最近访问的操作项
        /// </summary>
        /// <param name="accId">用户id</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="count">返回的个数</param>
        /// <returns></returns>
        DataSet GetLately(int accId, string type, int count);
        /// <summary>
        /// 获取某用户某时间段内，访问次数最多的操作项
        /// </summary>
        /// <param name="accId">用户id</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="count">返回的个数</param>
        /// <returns></returns>
        DataSet GetFrequently(int accId, string type, int count);
        /// <summary>
        /// 分页获取所有日志记录
        /// </summary>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        Logs[] GetPager(string type,int size, int index, out int countSum);
        Logs[] GetPager(string type,DateTime start,DateTime end, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取所有日志记录
        /// </summary>
        /// <param name="accId">员工id</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        Logs[] GetPager(int accId,string type, int size, int index, out int countSum);
        Logs[] GetPager(int accId,string type, DateTime start, DateTime end, int size, int index, out int countSum);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accId"></param>
        /// <param name="mmSear">菜单名称</param>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Logs[] GetPager(int accId, string mmSear,string type, DateTime start, DateTime end, int size, int index, out int countSum);
        #endregion

        #region 学员练习记录
        /// <summary>
        /// 添加练习记录
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="ques">试题id</param>
        /// <param name="index">试题页面中的索引</param>
        /// <returns></returns>
        LogForStudentQuestions QuestionAdd(int acid, int couid, int olid, int ques, int index);
        /// <summary>
        /// 修改练习记录
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="ques">试题id</param>
        /// <param name="index">试题页面中的索引</param>
        /// <returns></returns>
        LogForStudentQuestions QuestionUpdate(int acid, int couid, int olid, int ques, int index);
        /// <summary>
        /// 获取练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        LogForStudentQuestions QuestionSingle(int acid, int couid, int olid);
        /// <summary>
        /// 删除学员的练习记录
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        void QuestionDelete(int acid, int couid, int olid);
        #endregion
    }
}
