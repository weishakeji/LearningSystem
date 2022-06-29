using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using System.Data.Common;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 附件的管理
    /// </summary>
    public interface IAccessory : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(Accessory entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Accessory entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 删除，按系统唯一id
        /// </summary>
        /// <param name="uid">系统唯一id</param>
        void Delete(string uid, string type);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(Accessory entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="tran">事务</param>
        void Delete(string uid, WeiSha.Data.DbTrans tran);
        //void DeleteBatch(string uid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Accessory GetSingle(int identify);
        /// <summary>
        /// 通过Uid获取
        /// </summary>
        /// <param name="uid">附件关联对象的Uid，例如章节视频，此处为章节的uid</param>
        /// <param name="type">附件类型，例如CourseVideo（课程视频），Course（课程资料）</param>
        /// <returns></returns>
        Accessory GetSingle(string uid, string type);
        /// <summary>
        /// 某个主体（如新闻）的所有附件
        /// </summary>
        /// <param name="uid">主体的唯一标识</param>
        /// <returns></returns>
        List<Accessory> GetAll(string uid);
        /// <summary>
        /// 某个主体（如新闻）的所有附件
        /// </summary>
        /// <param name="uid">主体的唯一标识</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        List<Accessory> GetAll(string uid, string type);
        /// <summary>
        /// 共计多少个记录
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int OfCount(int orgid, string uid, string type);
    }
}
