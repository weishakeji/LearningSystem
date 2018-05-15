using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 支付接口管理
    /// </summary>
    public interface IPayInterface : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PayAdd(PayInterface entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PaySave(PayInterface entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PayDelete(PayInterface entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void PayDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        PayInterface PaySingle(int identify);
        /// <summary>
        /// 获取所有；
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="platform">接口平台，电脑为web，手机为mobi</param>
        /// <param name="isEnable">是否允许</param>
        /// <returns></returns>
        PayInterface[] PayAll(int orgid, string platform, bool? isEnable);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        PayInterface PayIsExist(int orgid, PayInterface entity);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool PayRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool PayRemoveDown(int id);
    }
}
