using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 员工组的管理
    /// </summary>
    public interface IEmpGroup : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(EmpGroup entity);
        /// <summary>
        /// 增加员工与组的关联
        /// </summary>
        /// <param name="empId"></param>
        /// <param name="grpId"></param>
        void AddRelation(int empId, int grpId);
        /// <summary>
        /// 根据员工Id,删除关联
        /// </summary>
        /// <param name="empId"></param>
        void DelRelation4Emplyee(int empId);
        /// <summary>
        /// 根据组Id,删除关联
        /// </summary>
        /// <param name="grpId"></param>
        void DelRelation4Group(int grpId);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(EmpGroup entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(EmpGroup entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        EmpGroup GetSingle(int identify);
        /// <summary>
        /// 获取对象；即所有用户组；
        /// </summary>
        /// <returns></returns>
        EmpGroup[] GetAll(int orgid);
        EmpGroup[] GetAll(int orgid, bool? isUse);
        /// <summary>
        /// 获取某员工所属的所有组；
        /// </summary>
        /// <param name="EmpAccountId">员工id</param>
        /// <returns></returns>
        EmpGroup[] GetAll4Emp(int EmpAccountId);
        /// <summary>
        /// 获取某个组的所有员工
        /// </summary>
        /// <param name="grpId">组id</param>
        /// <returns></returns>
        EmpAccount[] GetAll4Group(int grpId);
        /// <summary>
        /// 获取某个组的所有在职员工
        /// </summary>
        /// <param name="grpId"></param>
        /// <param name="use"></param>
        /// <returns></returns>
        EmpAccount[] GetAll4Group(int grpId,bool use);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        bool IsExist(EmpGroup entity);
        bool IsExist(string name, int id, int orgid);
        /// 更改排序
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        bool UpdateTaxis(EmpGroup[] entities);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="name"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        EmpGroup[] Pager(int orgid, bool? isUse, string name, int size, int index, out int countSum);
    }
}
