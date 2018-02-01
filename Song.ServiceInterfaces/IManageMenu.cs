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
    public interface IManageMenu : WeiSha.Common.IBusinessInterface
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
        int RootDelete(int identify);
        /// <summary>
        /// 获取根节点对象；即菜单树的名称
        /// </summary>
        /// <param name="func">功能分类</param>
        /// <returns></returns>
        ManageMenu[] GetRoot(string func);
        ManageMenu[] GetRoot(int identify);
        ManageMenu[] GetRoot(bool? isShow);
        ManageMenu[] GetRoot(string func,bool? isShow);
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
        /// 修改排序
        /// </summary>
        /// <param name="xml"></param>
        void SaveOrder(string xml);
        /// <summary>
        /// 将节点移动到另一个主分类
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="rootid"></param>
        void Move(ManageMenu entity,int rootid);
        /// <summary>
        /// 将节点复制到另一个主分类
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="rootid"></param>
        void Copy(ManageMenu entity, int rootid);
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
        /// 获取单一实体对象，按栏目名称
        /// </summary>
        /// <param name="name">栏目名称</param>
        /// <returns></returns>
        ManageMenu GetSingle(string name);
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
        int GetMaxTaxis(int parentId);
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns></returns>
        ManageMenu GetParent(int identify);
        /// <summary>
        /// 获取当前对象的父级对象；
        /// </summary>
        /// <param name="name">当前栏目的名称</param>
        /// <returns></returns>
        ManageMenu GetParent(string name);
        /// <summary>
        /// 获取对象；即所有栏目；
        /// </summary>
        /// <returns></returns>
        ManageMenu[] GetAll();
        /// <summary>
        /// 获取对象；即所有可用栏目；
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        ManageMenu[] GetAll(bool? isUse,bool? isShow);
        /// <summary>
        /// 获取所有对象，功能菜单或系统菜菜
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ManageMenu[] GetAll(bool? isUse, bool? isShow,string type);
        /// <summary>
        /// 获取某一个根菜单下的所有分级
        /// </summary>
        /// <param name="rootid">根节点id</param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        ManageMenu[] GetAll(int rootid, bool? isUse, bool? isShow, string type);
       
        /// <summary>
        /// 获取当前对象的下一级子对象；
        /// </summary>
        /// <param name="identify">当前实体的主键</param>
        /// <returns>当前对象的下一级子对象</returns>
        ManageMenu[] GetChilds(int identify);
        ManageMenu[] GetChilds(int identify,bool? isUse, bool? isShow);
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
        bool IsExist(ManageMenu entity, bool isSibling);/// <summary>
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
        /// <summary>
        /// 获取当前菜单项的功能点
        /// </summary>
        /// <param name="identify">菜单项的主键id</param>
        /// <returns></returns>
        DataTable GetFuncPoint(int identify);
    }
}
