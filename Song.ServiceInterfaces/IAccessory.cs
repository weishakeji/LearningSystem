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
    public interface IAccessory : WeiSha.Common.IBusinessInterface
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
        void Delete(string uid);
        /// <summary>
        /// 删除，按系统唯一id
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="isDelfile">是否删除文件</param>
        void Delete(string uid, bool isDelfile);
        void Delete(string uid, WeiSha.Data.DbTrans tran);
        //void DeleteBatch(string uid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Accessory GetSingle(int identify);
        Accessory GetSingle(string uid); 
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
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        int OfCount(string uid, string type);  
    }
}
