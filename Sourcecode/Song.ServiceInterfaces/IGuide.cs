using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 课程指南，后来改叫课程通知
    /// </summary>
    public interface IGuide : WeiSha.Core.IBusinessInterface
    {
        #region 指南
        /// <summary>
        /// 添加指南
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
        /// <param name="gcuid">分类uid</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Guide[] GuideCount(int orgid, long couid, string gcuid, int count);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid">课程id</param>
        /// <param name="gcuid">分类</param>
        /// <param name="searTxt"></param>
        /// <param name="isShow">是否显示</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Guide[] GuidePager(int orgid, long couid, string gcuid, string searTxt, bool? isShow, int size, int index, out int countSum); 
        #endregion

        #region 指南分类
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
        GuideColumns ColumnsSingle(string uid);
        /// <summary>
        /// 获取对象；即所有分类；
        /// </summary>
        /// <returns></returns>
        GuideColumns[] GetColumnsAll(long couid, string search, bool? isUse);
        /// <summary>
        /// 获取当前分类下的子分类
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        GuideColumns[] GetColumnsChild(long couid, string pid, bool? isUse);
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="list">对象列表，Gc_ID、Gc_PID、Gc_Tax</param>
        /// <returns></returns>
        bool ColumnsUpdateTaxis(GuideColumns[] list);

        #endregion
    }
}
