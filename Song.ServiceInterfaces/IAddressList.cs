using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 通讯录的管理
    /// </summary>
    public interface IAddressList : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 清除所有信息
        /// </summary>
        void Clear();

        #region 通讯录
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void AddressAdd(AddressList entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void AddressSave(AddressList entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void AddressDelete(AddressList entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void AddressDelete(int identify);
        /// <summary>
        /// 删除所有
        /// </summary>
        void AddressDeleteAll();
        /// <summary>
        /// 删除，按人员名称
        /// </summary>
        /// <param name="name">人员名称</param>
        void AddressDelete(string name);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        AddressList AddressSingle(int identify);
        AddressList AddressSingle(string mobiTel);
        /// <summary>
        /// 获取某个院系的所有人员；
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        List<AddressList> AddressAll();

        List<AddressList> AddressAll(int? sortId);
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        List<AddressList> AddressPager(int accId, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="typeName">分类id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<AddressList> AddressPager(int accId, string typeName, string personName, int size, int index, out int countSum);
        #endregion

        #region 通讯录分类
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int SortAdd(AddressSort entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SortSave(AddressSort entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SortDelete(AddressSort entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void SortDelete(int identify);
        /// <summary>
        /// 清除所有分类
        /// </summary>
        void SortDeleteAll();
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        AddressSort SortSingle(int identify);
        /// <summary>
        /// 获取某个院系的所有人员；
        /// </summary>
        /// <param name="isUse">是否使用</param>
        /// <returns></returns>
        List<AddressSort> SortAll(bool? isUse);
        /// <summary>
        /// 分页获取；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        List<AddressSort> SortPager(int accId, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取所有的人员；
        /// </summary>
        /// <param name="accId">所属人员的id</param>
        /// <param name="sortName">分类名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<AddressSort> SortPager(int accId, string sortName, int size, int index, out int countSum);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SortRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SortRemoveDown(int id);
        #endregion
    }
}
