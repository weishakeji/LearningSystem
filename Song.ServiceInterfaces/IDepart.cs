using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 管理后台的菜单
    /// </summary>
    public interface IDepart : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Add(Depart entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Depart entity);
        /// <summary>
        /// 修改院系排序
        /// </summary>
        /// <param name="xml"></param>
        void SaveOrder(string xml);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(Depart entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 删除，按院系名称
        /// </summary>
        /// <param name="name">院系名称</param>
        void Delete(string name);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Depart GetSingle(int identify);        
        /// <summary>
        /// 获取单一实体对象，按院系名称
        /// </summary>
        /// <param name="name">院系名称</param>
        /// <returns></returns>
        Depart GetSingle(string name);
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns></returns>
        Depart GetParent(int identify);
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="name">当前院系的名称</param>
        /// <returns></returns>
        Depart GetParent(string name);
        /// <summary>
        /// 获取对象；即所有院系；
        /// </summary>
        /// <returns></returns>
        Depart[] GetAll(int orgid);
        Depart[] GetAll(int orgid, bool? isUse, bool? isShow);
        /// <summary>
        /// 获取当前对象的下一级子对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns>当前对象的下一级子对象</returns>
        Depart[] GetChilds(int identify);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        bool IsExist(int orgid, Depart entity);
        /// <summary>
        /// 在当前对象的同级（兄弟中），该对象是否重名，
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="isSibling">是否限制在当前层的判断；true，表示仅在当前层判断，false表示在所有对象中判断</param>
        /// <returns></returns>
        bool IsExist(int orgid, Depart entity, bool isSibling);
        /// <summary>
        /// 移动对象到其它节点下；
        /// </summary>
        /// <param name="currentId">当前对象id</param>
        /// <param name="parentId">要移动到某个节点下的id，即父节点id</param>
        /// <returns></returns>
        bool Remove(int currentId, int parentId);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveDown(int id);
        
    }
}
