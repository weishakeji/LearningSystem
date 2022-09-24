using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 问题消息管理
    /// </summary>
    public interface IMessage : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Add(Message entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Message entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Delete(Message entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        int Delete(int identify);
        /// <summary>
        /// 删除，按主键id和学员id
        /// </summary>
        /// <param name="identify">主键id</param>
        /// <param name="acid">学员账户id</param>
        int Delete(int identify,int acid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Message GetSingle(int identify);
        /// <summary>
        /// 获取留言
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        Message[] GetAll(long couid, int olid);
        /// <summary>
        /// 获取留言
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <param name="order">排序方式，desc或asc</param>
        /// <returns></returns>
        Message[] GetAll(int olid,string order);
        /// <summary>
        /// 获取留言
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="count"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        Message[] GetCount(long couid, int olid, string order, int count);
        /// <summary>
        /// 获取留言数量
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        int GetOfCount(long couid, int olid);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="stid"></param>
        /// <param name="sear"></param>
        /// <param name="startTime">创建时间，起始范围</param>
        /// <param name="endTime">创建时间，结束的范围</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Message[] GetPager(int orgid,long couid, int olid, int stid, string sear, DateTime? startTime, DateTime? endTime, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="stid"></param>
        /// <param name="sear"></param>
        /// <param name="startPlay"></param>
        /// <param name="endPlay"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Message[] GetPager(long couid, int olid, int stid, string sear, int startPlay, int endPlay, int size, int index, out int countSum);
    }
}
