using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 知识库管理
    /// </summary>
    public interface IKnowledge : WeiSha.Common.IBusinessInterface
    {
        #region 知识库管理
        /// <summary>
        /// 添加知识库
        /// </summary>
        /// <param name="entity">业务实体</param>
        int KnowledgeAdd(Knowledge entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void KnowledgeSave(Knowledge entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void KnowledgeDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Knowledge KnowledgeSingle(int identify);
        /// <summary>
        /// 当前知识的上一条知识
        /// </summary>
        /// <param name="couid">课程id，当小于0时取所有（等于0什么也不取）</param>
        /// <param name="kns">分类Id</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Knowledge KnowledgePrev(int couid, int kns, int id);
        /// <summary>
        /// 当前知识的下一条知识
        /// </summary>
        /// <param name="couid">课程id，当小于0时取所有（等于0什么也不取）</param>
        /// <param name="kns">分类Id</param>
        /// <param name="id"></param>
        /// <returns></returns>
        Knowledge KnowledgeNext(int couid, int kns, int id);
        /// <summary>
        /// 获取知识库
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="kns">分类id</param>
        /// <param name="count"></param>
        /// <returns></returns>
        Knowledge[] KnowledgeCount(int orgid, bool? isUse, int kns, int count);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid">课程id，当小于0时取所有（等于0什么也不取）</param>
        /// <param name="kns"></param>
        /// <param name="searTxt"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Knowledge[] KnowledgeCount(int orgid, int couid, int kns, string searTxt, bool? isUse, int count);
        /// <summary>
        /// 计算有多少条
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="kns"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int KnowledgeOfCount(int orgid, int kns, bool? isUse);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="kns">分类id</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Knowledge[] KnowledgePager(int orgid, bool? isUse, int kns, string searTxt, int size, int index, out int countSum);
        Knowledge[] KnowledgePager(int orgid, int couid, int kns, bool? isUse, bool? isHot, bool? isRec, bool? isTop, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 当前课程的知识库
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="kns">分类id，逗号分隔</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Knowledge[] KnowledgePager(int couid, string kns, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 知识库分类管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int SortAdd(KnowledgeSort entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SortSave(KnowledgeSort entity);
        /// <summary>
        /// 修改分类排序
        /// </summary>
        /// <param name="xml"></param>
        void SortSaveOrder(string xml);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void SortDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        KnowledgeSort SortSingle(int identify);
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="orgid">所属机构</param>
        /// <param name="couid">课程id，当小于0时取所有（等于0什么也不取）</param>
        /// <param name="IsUse"></param>
        /// <returns></returns>
        KnowledgeSort[] GetSortAll(int orgid, int couid, bool? isUse);
        /// <summary>
        /// 获取所有分类
        /// </summary>
        /// <param name="orgid">所属机构id</param>
        /// <param name="couid">课程id，当小于0时取所有（等于0什么也不取）</param>
        /// <param name="pid">父id（多级分类）</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        KnowledgeSort[] GetSortAll(int orgid, int couid, int pid, bool? isUse);
        /// <summary>
        /// 获取当前对象的下一级子对象；
        /// </summary>
        /// <param name="couid">课程id，当小于0时取所有（等于0什么也不取）</param>
        /// <param name="pid">上级</param>
        /// <returns>当前对象的下一级子对象</returns>
        KnowledgeSort[] GetSortChilds(int pid, int couid, bool? isUse);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SortRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool SortRemoveDown(int id);
        #endregion

    }
}
