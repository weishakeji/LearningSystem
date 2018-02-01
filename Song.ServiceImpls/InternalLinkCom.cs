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
    public class InternalLinkCom : IInternalLink
    {
        /// <summary>
        /// 添加内部链接
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void LinkAdd(InternalLink entity)
        {
            entity.IL_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<InternalLink>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void LinkSave(InternalLink entity)
        {
            Gateway.Default.Save<InternalLink>(entity);
        }
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void LinkDelete(int identify)
        {
            Gateway.Default.Delete<InternalLink>(InternalLink._.IL_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public InternalLink LinkSingle(int identify)
        {
            return Gateway.Default.From<InternalLink>().Where(InternalLink._.IL_ID == identify).ToFirst<InternalLink>();
        }
        /// <summary>
        /// 获取某个院系的所有链接项；
        /// </summary>
        /// <param name="isUse">是否使用</param>
        /// <returns></returns>
        public InternalLink[] LinkAll(bool? isUse)
        {
            WhereClip wc = InternalLink._.IL_ID > -1;
            if (isUse != null) wc.And(InternalLink._.IL_IsUse == (bool)isUse);
            return Gateway.Default.From<InternalLink>().Where(wc).ToArray<InternalLink>();
        }
        /// 分页获取所有的链接项；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public InternalLink[] LinkPager(string searTxt, bool? isUse, int size, int index, out int countSum)
        {
            WhereClip wc = InternalLink._.IL_ID > -1;
            if (isUse != null) wc.And(InternalLink._.IL_IsUse == (bool)isUse);
            if (searTxt != null && searTxt.Length > 0) wc.And(InternalLink._.IL_Name.Like("%"+searTxt+"%"));
            countSum = Gateway.Default.Count<InternalLink>(wc);
            return Gateway.Default.From<InternalLink>().Where(wc).OrderBy(InternalLink._.IL_CrtTime.Desc).ToArray<InternalLink>(size, (index - 1) * size);
        }
    }
}
