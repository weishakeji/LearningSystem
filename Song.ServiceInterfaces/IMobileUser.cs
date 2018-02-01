using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 手机客户端的管理
    /// </summary>
    public interface IMobileUser : WeiSha.Common.IBusinessInterface
    {        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Add(MobileUser entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(MobileUser entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(MobileUser entity);
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
        MobileUser GetSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按电话号码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        MobileUser GetSingle(string phone);
        /// <summary>
        /// 获取用户个数；
        /// </summary>
        /// <returns></returns>
        int GetCount();        
        /// <summary>
        /// 分页获取所有的网站用户帐号；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        MobileUser[] GetPager(int size, int index, out int countSum);
    }
}
