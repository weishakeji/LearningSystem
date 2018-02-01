using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface IPurview : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(Purview entity);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="memberId">成员id，即权限赋予对象的id</param>
        /// <param name="mmids">管理菜单的id</param>
        /// <param name="type">成员类型</param>
        void AddBatch(int memberId, string mmids, string type);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Purview entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(Purview entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 根据分类、对象id删除
        /// </summary>
        /// <param name="memberId">角色或院系、组的id</param>
        /// <param name="type">用于区分不同权限分配，"Posi"角色(岗位)、"Group"组、"Depart"院系</param>
        void Delete(int memberId, string type);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Purview GetSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，按权限主题的id
        /// </summary>
        /// <param name="MMId">功能菜单的id</param>
        /// <param name="type">用于区分不同权限分配，"Posi"角色(岗位)、"Group"组、"Depart"院系</param>
        /// <returns></returns>
        Purview GetSingle4Member(int MMId,string type);
        /// <summary>
        /// 获取所有对象，按权限主题的id
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="type">用于区分不同权限分配，"Posi"角色(岗位)、"Group"组、"Depart"院系</param>
        /// <returns></returns>
        Purview[] GetAll(int memberId,string type);
        /// <summary>
        /// 获取某个员工所拥用的全部操作权限，包括所在组、所属角色、所在院系的所有权限
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        ManageMenu[] GetAll4Emplyee(int empId);
        /// <summary>
        /// 机构的权限(机构权限与机构等级权限为并集)
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        ManageMenu[] GetAll4Org(int orgid);
        /// <summary>
        /// 获取机构的某一个根菜单项的权限
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="marker">例如教师管理teacher,学生管理student,机构管理organAdmin</param>
        /// <returns></returns>
        ManageMenu[] GetAll4Org(int orgid, string marker);
        /// <summary>
        /// 获取机构的权限
        /// </summary>
        /// <returns></returns>
        ManageMenu[] GetOrganPurview();
        /// <summary>
        /// 获取机构的某一个根菜单项的权限
        /// </summary>
        /// <param name="marker">例如教师管理teacher,学生管理student,机构管理organAdmin</param>
        /// <returns></returns>
        ManageMenu[] GetOrganPurview(string marker);
    }
}
