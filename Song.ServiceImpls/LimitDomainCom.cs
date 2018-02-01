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
    public class LimitDomainCom : ILimitDomain
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void DomainAdd(LimitDomain entity)
        {
            Gateway.Default.Save<LimitDomain>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void DomainSave(LimitDomain entity)
        {
            Gateway.Default.Save<LimitDomain>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void DomainDelete(int identify)
        {
            Gateway.Default.Delete<LimitDomain>(LimitDomain._.LD_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public LimitDomain DomainSingle(int identify)
        {
            return Gateway.Default.From<LimitDomain>().Where(LimitDomain._.LD_ID == identify).ToFirst<LimitDomain>();
        }
        /// <summary>
        /// 获取指定数量的实体
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public LimitDomain[] DomainCount(bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc &= LimitDomain._.LD_IsUse == (bool)isUse;
            return Gateway.Default.From<LimitDomain>().Where(wc).ToArray<LimitDomain>(count);
        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public bool DomainIsExist(LimitDomain entity)
        {
            //如果是一个已经存在的对象，则不匹配自己
            LimitDomain mm = Gateway.Default.From<LimitDomain>()
                   .Where(LimitDomain._.LD_Name == entity.LD_Name && LimitDomain._.LD_ID != entity.LD_ID)
                   .ToFirst<LimitDomain>();
            return mm != null;
        }
        /// <summary>
        /// 当前域名是否存在
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool DomainIsExist(string domain)
        {
            //如果是一个已经存在的对象，则不匹配自己
            int count = Gateway.Default.Count<LimitDomain>(LimitDomain._.LD_Name == domain);
            return count > 0;
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="search"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LimitDomain[] DomainPager(bool? isUse, string search, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (isUse != null) wc &= LimitDomain._.LD_IsUse == (bool)isUse;
            if (string.IsNullOrWhiteSpace(search)) wc &= LimitDomain._.LD_Name.Like("%" + search + "%");
            countSum = Gateway.Default.Count<LimitDomain>(wc);
            return Gateway.Default.From<LimitDomain>().Where(wc).ToArray<LimitDomain>(size, (index - 1) * size);
        }
    }
}
