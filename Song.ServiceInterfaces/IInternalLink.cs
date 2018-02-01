using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 内部链接的管理
    /// </summary>
    public interface IInternalLink : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加内部链接
        /// </summary>
        /// <param name="entity">业务实体</param>
        void LinkAdd(InternalLink entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void LinkSave(InternalLink entity);
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void LinkDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        InternalLink LinkSingle(int identify);
        /// <summary>
        /// 获取某个院系的所有链接项；
        /// </summary>
        /// <param name="isUse">是否使用</param>
        /// <returns></returns>
        InternalLink[] LinkAll(bool? isUse);
        /// 分页获取所有的链接项；
        /// </summary>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        InternalLink[] LinkPager(string searTxt, bool? isUse, int size, int index, out int countSum);

    }
}
