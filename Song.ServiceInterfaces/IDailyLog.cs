using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface IDailyLog : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(DailyLog entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(DailyLog entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        DailyLog GetSingle(int identify);
        /// <summary>
        /// 获取某个时间的上一个记录，如当前日志的上一个日志
        /// </summary>
        /// <param name="currTime">当前时间</param>
        /// <param name="type">记录类别,1为日志，2为周志，3为月志，4为季度总结，5为年度总结</param>
        /// <param name="accId">员工id</param>
        /// <returns></returns>
        DailyLog GetPrevious(DateTime currTime,string type,int accId);
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="type">分类，1为日志，2为周志，3为月志，4为季度总结，5为年度总结</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        DailyLog[] GetPager(int accId, string type,int size, int index, out int countSum);
        DailyLog[] GetPager(int accId, string type, DateTime start,DateTime end, int size, int index, out int countSum);
        
    }
}
