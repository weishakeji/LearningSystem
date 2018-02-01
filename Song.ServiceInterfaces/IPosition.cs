using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface IPosition : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(Position entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Position entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(Position entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 删除，按职位名称
        /// </summary>
        /// <param name="name">职位名称</param>
        void Delete(int orgid, string name);
        /// <summary>
        /// 删除与员工之间的关联
        /// </summary>
        /// <param name="identify"></param>
        void DeleteRelation4Emp(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Position GetSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按职位名称
        /// </summary>
        /// <param name="name">职位名称</param>
        /// <returns></returns>
        Position GetSingle(int orgid,string name);
        /// <summary>
        /// 获取超级管理员角色
        /// </summary>
        /// <returns></returns>
        Position GetSuper();
        /// <summary>
        /// 获取对象；即所有职位；
        /// </summary>
        /// <returns></returns>
        Position[] GetAll(int orgid);
        Position[] GetAll(int orgid,bool? isUse);
        /// <summary>
        /// 获取当前角色的所有员工
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmpAccount[] GetAllEmplyee(int posid);
        /// <summary>
        /// 获取当前角色的所有在职员工
        /// </summary>
        /// <param name="posid"></param>
        /// <param name="use">是否在职</param>
        /// <returns></returns>
        EmpAccount[] GetAllEmplyee(int posid,bool use);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        bool IsExist(int orgid, Position entity);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveUp(int orgid, int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool RemoveDown(int orgid, int id);
        /// <summary>
        /// 获取机构的管理岗位
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        Position GetAdmin(int orgid);
    }
}
