using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 班级管理
    /// </summary>
    public interface ITeam : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加班组
        /// </summary>
        /// <param name="entity">业务实体</param>
        int TeamAdd(Team entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void TeamSave(Team entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void TeamDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Team TeamSingle(int identify);
        /// <summary>
        /// 获取班级
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Team[] GetTeam(bool? isUse, int count);
        Team[] GetTeam(bool? isUse, int? depid, int count);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Team[] GetTeamPager(bool? isUse, string searTxt, int size, int index, out int countSum);
        Team[] GetTeamPager(int depid,bool? isUse, string searTxt, int size, int index, out int countSum);
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
