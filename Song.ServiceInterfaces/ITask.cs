using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface ITask : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Add(Task entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Task entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(Task entity);
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
        Task GetSingle(int identify);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveDown(int id);
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="level">等级</param>
        /// <param name="size">每页几条信息</param>
        /// <param name="index">第几页</param>
        /// <param name="countSum">数据记录的总数</param>
        /// <returns></returns>
        Task[] GetPager(int level,int size, int index, out int countSum);
        /// <summary>
        /// 分页获取自己派发任务
        /// </summary>
        /// <param name="accId">员工id</param>
        /// <param name="isGoback">是否是退回的任务</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="state">任务的状态，1完成，2未完成，3逾期未完成，4正在处理，5关闭</param>
        /// <param name="level">任务的优先级</param>
        /// <param name="searStr">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Task[] GetMyPager(int accId,bool isGoback,DateTime start, DateTime end, string state, int level, string searStr, int size, int index, out int countSum);
        /// <summary>
        /// 获取自己承接的任务
        /// </summary>
        /// <param name="accId">承接任务的员工Id</param>
        /// <param name="isGoback"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="state"></param>
        /// <param name="level"></param>
        /// <param name="searStr"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Task[] GetWorkerPager(int accId, bool isGoback, DateTime start, DateTime end, string state, int level, string searStr, int size, int index, out int countSum);
    }
}
