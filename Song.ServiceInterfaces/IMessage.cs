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
    public interface IMessage : WeiSha.Common.IBusinessInterface
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
        void Delete(Message entity);
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
        Message GetSingle(int identify);
        /// <summary>
        /// 获取对象；
        /// </summary>
        /// <returns></returns>
        Message[] GetAll();
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="sear"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Message[] GetPager(int orgid, int stid, string sear, DateTime startTime, DateTime endTime, int size, int index, out int countSum);

    }
}
