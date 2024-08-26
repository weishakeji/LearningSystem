using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 管理后台的菜单
    /// </summary>
    public interface IManageMenu : WeiSha.Core.IBusinessInterface
    {
        #region 菜单树的管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int RootAdd(ManageMenu entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        int RootSave(ManageMenu entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        int RootDelete(ManageMenu entity);
        int RootDelete(string uid);
        /// <summary>
        /// 获取根节点对象；即菜单树的名称
        /// </summary>
        /// <param name="func">功能分类</param>
        /// <returns></returns>
        ManageMenu[] GetRoot(string func);
        ManageMenu[] GetRoot(int identify);
        ManageMenu[] GetRoot(bool? isShow);
        ManageMenu[] GetRoot(string func, bool? isShow);
        ManageMenu[] GetRoot(int identify, bool? isShow);
        /// <summary>
        /// 获取树对象,即所有栏目；
        /// </summary>
        /// <param name="func"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        ManageMenu[] GetTree(string func, bool? isShow);
        ManageMenu[] GetTree(string func, bool? isShow, bool? isUse);


        #endregion
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Add(ManageMenu entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(ManageMenu entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(ManageMenu entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 删除，按栏目名称
        /// </summary>
        /// <param name="name">栏目名称</param>
        void Delete(string name);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ManageMenu GetSingle(int identify);
        /// <summary>
        /// 获取单一实体对象
        /// </summary>
        /// <param name="uid">菜单的全局一标识</param>
        /// <returns></returns>
        ManageMenu GetSingle(string uid);
        /// <summary>
        /// 通过标识获取根节点
        /// </summary>
        /// <param name="marker"></param>
        /// <returns></returns>
        ManageMenu GetRootMarker(string marker);
        /// <summary>
        /// 获取同一父级下的最大排序号；
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns></returns>
        int GetMaxTaxis(string parentId);
        /// <summary>
        /// 获取对象；即所有栏目；
        /// </summary>
        /// <returns></returns>
        List<ManageMenu> GetAll();
        /// <summary>
        /// 获取对象；即所有可用栏目；
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        List<ManageMenu> GetAll(bool? isUse, bool? isShow);
        /// <summary>
        /// 获取所有对象，功能菜单或系统菜菜
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<ManageMenu> GetAll(bool? isUse, bool? isShow, string type);
        /// <summary>
        /// 获取某一个根菜单下的所有分级
        /// </summary>
        /// <param name="uid">根节点id的uid</param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<ManageMenu> GetFunctionMenu(string uid, bool? isUse, bool? isShow);

        /// <summary>
        /// 获取当前对象的下一级子对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns>当前对象的下一级子对象</returns>
        ManageMenu[] GetChilds(int identify);
        ManageMenu[] GetChilds(int identify, bool? isUse, bool? isShow);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        bool IsExist(ManageMenu entity);
        /// <summary>
        /// 在当前对象的同级（兄弟中），该对象是否重名，
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="isSibling">是否限制在当前层的判断；true，表示仅在当前层判断，false表示在所有对象中判断</param>
        /// <returns></returns>
        bool IsExist(ManageMenu entity, bool isSibling);
        /// <summary>
        /// <summary>
        /// 更改根菜单顺序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateTaxis(ManageMenu[] items);
        /// <summary>
        /// 更新系统菜单树
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateSystemTree(ManageMenu[] items);
        /// <summary>
        /// 更新功能菜单树
        /// </summary>
        /// <param name="uid">根菜单uid</param>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateFunctionTree(string uid, ManageMenu[] items);
    }
}
