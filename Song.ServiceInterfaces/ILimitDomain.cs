using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 二级域名的管理
    /// </summary>
    public interface ILimitDomain : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void DomainAdd(LimitDomain entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void DomainSave(LimitDomain entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void DomainDelete(int identify);       
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        LimitDomain DomainSingle(int identify);
        /// <summary>
        /// 获取指定数量的实体
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        LimitDomain[] DomainCount(bool? isUse, int count);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        bool DomainIsExist(LimitDomain entity);
        /// <summary>
        /// 当前域名是否存在
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        bool DomainIsExist(string domain);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="search"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LimitDomain[] DomainPager(bool? isUse, string search, int size, int index, out int countSum);
    }
}
