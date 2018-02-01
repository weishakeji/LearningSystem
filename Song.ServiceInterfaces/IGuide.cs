using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 考试指南的管理
    /// </summary>
    public interface IGuide : WeiSha.Common.IBusinessInterface
    {
        #region 考试指南
        /// <summary>
        /// 添加考试指南
        /// </summary>
        /// <param name="entity">业务实体</param>
        void GuideAdd(Guide entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void GuideSave(Guide entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void GuideDelete(Guide entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void GuideDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Guide GuideSingle(int identify);
        /// <summary>
        /// 当前课程公告的上一条课程公告
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Guide GuidePrev(Guide entity);
        /// <summary>
        /// 当前课程公告的下一条课程公告
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Guide GuideNext(Guide entity);
        /// <summary>
        /// 取多少条
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid">课程id</param>
        /// <param name="gcid">分类id</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Guide[] GuideCount(int orgid, int couid, int gcid, int count);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid">课程id</param>
        /// <param name="gcid">考试指南分类</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Guide[] GetGuidePager(int orgid, int couid, int gcid, string searTxt, bool? isShow, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="gcids">考试指南分类,多个id，逗号分隔</param>
        /// <param name="searTxt"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Guide[] GetGuidePager(int orgid, int couid, string gcids, string searTxt, bool? isShow, int size, int index, out int countSum);

        #endregion

        #region 考试指南分类
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ColumnsAdd(GuideColumns entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ColumnsSave(GuideColumns entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ColumnsDelete(GuideColumns entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ColumnsDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        GuideColumns ColumnsSingle(int identify);
        /// <summary>
        /// 获取同一父级下的最大排序号；
        /// </summary>
        ///<param name="couid">课程id</param>
        ///<param name="pid">学科id</param>
        /// <returns></returns>
        int ColumnsMaxTaxis(int couid,int pid);
        /// <summary>
        /// 获取对象；即所有分类；
        /// </summary>
        /// <returns></returns>
        GuideColumns[] GetColumnsAll(int couid, bool? isUse);
        /// <summary>
        /// 获取当前分类下的子分类
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        GuideColumns[] GetColumnsChild(int couid, int pid, bool? isUse);
        /// <summary>
        /// 是否有子级
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool ColumnsIsChildren(int couid, int pid, bool? isUse);
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        bool ColumnsIsExist(int couid, int pid, GuideColumns entity);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ColumnsRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ColumnsRemoveDown(int id);

        #endregion
    }
}
