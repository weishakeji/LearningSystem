using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 支付接口管理
    /// </summary>
    public interface IPayInterface : WeiSha.Core.IBusinessInterface
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
        /// 接口是否存在（接口名称不得重复）
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="entity">业务实体</param>
        /// <param name="scope">查询范围，1查询所有接口；2同一类型（Pai_InterfaceType），同一设备端（Pai_Platform），不重名</param>
        /// <returns></returns>
        PayInterface PayIsExist(int orgid, PayInterface entity, int scope);
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateTaxis(PayInterface[] items);
        /// <summary>
        /// 计算某一个支付接口的收入
        /// </summary>
        /// <param name="paid">支付接口的id</param>
        /// <returns></returns>
        decimal Summary(int paid);
    }
}
