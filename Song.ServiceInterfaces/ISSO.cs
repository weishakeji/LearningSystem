using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;
using System.Data.Common;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 单点登录的管理
    /// </summary>
    public interface ISSO : WeiSha.Common.IBusinessInterface
    {        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(SingleSignOn entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(SingleSignOn entity);
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
        SingleSignOn GetSingle(int identify);
        /// <summary>
        /// 通过应用appid获取对象
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        SingleSignOn GetSingle(string appid);
        /// <summary>
        /// 所有
        /// </summary>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        SingleSignOn[] GetAll(bool? isuse);
        /// <summary>
        /// 某个主体（如新闻）的所有附件
        /// </summary>
        /// <param name="uid">主体的唯一标识</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        SingleSignOn[] GetAll(bool? isuse,string type);        
    }
}
