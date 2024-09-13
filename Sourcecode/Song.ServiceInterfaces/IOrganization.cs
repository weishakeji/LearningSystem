using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface IOrganization : WeiSha.Core.IBusinessInterface
    {
        #region 机构管理
        /// <summary>
        /// 添加机构
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OrganAdd(Organization entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OrganSave(Organization entity);
        /// <summary>
        /// 设置默认机构
        /// </summary>
        /// <returns></returns>
        void OrganSetDefault(int identify);
        /// <summary>
        /// 系统默认采用的机构（注：不是Root机构)
        /// </summary>
        /// <returns></returns>
        Organization OrganDefault();
        /// <summary>
        /// 用于系统管理的机构（注：即Root机构)
        /// </summary>
        /// <returns></returns>
        Organization OrganRoot();
        /// <summary>
        /// 当前机构,通过二级域名判断，如果不存在则返回默认机构
        /// </summary>
        /// <returns></returns>
        Organization OrganCurrent();
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        Organization OrganSingle(int identify);
        /// <summary>
        /// 当前机构是否重名
        /// </summary>
        /// <param name="name">机构的名称</param>
        /// <param name="id">机构的id</param>   
        /// <returns></returns>
        bool ExistName(string name, int id);
        /// <summary>
        /// 机构平台名称是否重复
        /// </summary>
        /// <param name="name">机构的平台名称</param>
        /// <param name="id">机构的id</param>
        /// <returns></returns>
        bool ExistPlatform(string name, int id);
        /// <summary>
        /// 机构的二级域否重复
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id">机构的id</param>
        /// <returns></returns>
        bool ExistDomain(string name, int id);
        /// <summary>
        /// 根据主键删除公司。
        /// </summary>
        /// <param name="identify">主键id</param>
        void OrganDelete(int identify);
        /// <summary>
        /// 取所有机构
        /// </summary>
        /// <param name="isUse">是否启用</param>
        /// <param name="level">机构等级</param>
        /// <param name="search"></param>
        /// <returns></returns>
        List<Organization> OrganAll(bool? isUse, int level, string search);
        /// <summary>
        /// 获取指定数量的对象
        /// </summary>
        /// <param name="isUse">是否使用</param>
        /// <param name="isShow">是否在前端显示</param>
        /// <param name="level">机构等级</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        List<Organization> OrganCount(bool? isUse, bool? isShow, int level, int count);
        /// <summary>
        /// 清理临时文件
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="day">清理多少天之前的</param>
        void OrganClearTemp(int orgid, int day);
        /// <summary>
        /// 清理当前机构的数据
        /// </summary>
        /// <param name="orgid"></param>
        void OrganClear(int orgid);
        /// <summary>
        /// 构建缓存
        /// </summary>
        List<Organization> OrganBuildCache();
        /// <summary>
        /// 分页获取机构
        /// </summary>
        /// <param name="isUse">是否使用</param>
        /// <param name="level">机构等级</param>
        /// <param name="searTxt">机构名称关键字</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        List<Organization> OrganPager(bool? isUse, int level, string searTxt, int size, int index, out int countSum);

        #endregion

        #region 机构等级
        /// <summary>
        /// 添加机构等级
        /// </summary>
        /// <param name="entity">业务实体</param>
        void LevelAdd(OrganLevel entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void LevelSave(OrganLevel entity);
        /// <summary>
        /// 设置默认等级，默认等级只有一个
        /// </summary>
        /// <param name="identify"></param>
        void LevelSetDefault(int identify);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns></returns>
        OrganLevel LevelSingle(int identify);
        /// <summary>
        /// 默认的机构等级
        /// </summary>
        /// <returns></returns>
        OrganLevel LevelDefault();
        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <returns></returns>
        OrganLevel[] LevelAll(string search, bool? isUse);
        /// <summary>
        /// 当前机构等级下，有几个机构
        /// </summary>
        /// <param name="lvid">机构等级id</param>
        /// <returns></returns>
        int LevelOrganCount(int lvid);
        /// <summary>
        /// 根据主键删除公司。
        /// </summary>
        /// <param name="identify">主键id</param>
        bool LevelDelete(int identify);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="name">机构等级的名称</param>
        /// <param name="id">机构等级的id</param>   
        /// <returns></returns>
        bool LevelNameExist(string name, int id);
        /// <summary>
        /// 机构等级的tag标识是否重名
        /// </summary>
        /// <param name="tag">机构等级的tag标识</param>
        /// <param name="id">机构等级的id</param>   
        /// <returns></returns>
        bool LevelTagExist(string tag, int id);
        /// <summary>
        /// 更改机构等级的排序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool LevelUpdateTaxis(OrganLevel[] items);

        #endregion

        #region 统计数据
        /// <summary>
        /// 有多少课程被选修过
        /// </summary>
        /// <param name="orgid">机构id</param>    
        /// <param name="isfree">是否包括免费的</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int CourseCountBuy(int orgid, bool? isfree, DateTime? start, DateTime? end);
        /// <summary>
        /// 有多少学员购买过课程
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isfree">是否包括免费的</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int StudentCountBuy(int orgid, bool? isfree, DateTime? start, DateTime? end);
        /// <summary>
        /// 购买课程的次数，即学员购买课程的次数
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isfree">是否包括免费的</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        int CourseSumBuy(int orgid, bool? isfree, DateTime? start, DateTime? end);

        /// <summary>
        /// 创建统计数据的定时任务
        /// </summary>
        void UpdateStatisticalData_CronJob();
        /// <summary>
        /// 统计数据延迟执行
        /// </summary>
        /// <param name="minute">延迟的分钟数</param>
        void UpdateStatisticalData_Delay(int minute);
        /// <summary>
        /// 更新机构的统计数据
        /// </summary>
        void UpdateStatisticalData();
        #endregion
    }
}
