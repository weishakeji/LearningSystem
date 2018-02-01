using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;



namespace Song.ServiceImpls
{
    /// <summary>
    /// 员工工作日志
    /// </summary>
    public class DailyLogCom : IDailyLog
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Add(DailyLog entity)
        {
            entity.Dlog_CrtTime = DateTime.Now;
            Gateway.Default.Save<DailyLog>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(DailyLog entity)
        {
            Gateway.Default.Save<DailyLog>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            Gateway.Default.Delete<DailyLog>(DailyLog._.Dlog_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public DailyLog GetSingle(int identify)
        {
            return Gateway.Default.From<DailyLog>().Where(DailyLog._.Dlog_Id == identify).ToFirst<DailyLog>();
        }
        /// <summary>
        /// 获取当前记录类别的上一个记录，如当前日志的上一个日志
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="type">记录类别,1为日志，2为周志，3为月志，4为季度总结，5为年度总结</param>
        /// <param name="accId">员工id</param>
        /// <returns></returns>
        public DailyLog GetPrevious(DateTime currTime, string type, int accId)
        {
           return Gateway.Default.From<DailyLog>()
               .Where(DailyLog._.Acc_Id == accId && DailyLog._.Dlog_Type == type && DailyLog._.Dlog_WrtTime < currTime)
               .OrderBy(DailyLog._.Dlog_WrtTime.Desc).ToFirst<DailyLog>();
               
        }
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="type">分类，1为日志，2为周志，3为月志，4为季度总结，5为年度总结</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public DailyLog[] GetPager(int accId, string type, int size, int index, out int countSum)
        {
            WhereClip wc = DailyLog._.Acc_Id == accId && DailyLog._.Dlog_Type==type;
            countSum = Gateway.Default.Count<DailyLog>(wc);
            return Gateway.Default.From<DailyLog>().Where(wc).OrderBy(DailyLog._.Dlog_WrtTime.Desc).ToArray<DailyLog>(size, (index - 1) * size);
        }
        public DailyLog[] GetPager(int accId, string type, DateTime start, DateTime end, int size, int index, out int countSum)
        {
            WhereClip wc = DailyLog._.Acc_Id == accId && DailyLog._.Dlog_Type == type;
            wc.And(DailyLog._.Dlog_WrtTime >= start && DailyLog._.Dlog_WrtTime <= end);
            countSum = Gateway.Default.Count<DailyLog>(wc);
            return Gateway.Default.From<DailyLog>().Where(wc).OrderBy(DailyLog._.Dlog_WrtTime.Desc).ToArray<DailyLog>(size, (index - 1) * size);
        }
    }
}
