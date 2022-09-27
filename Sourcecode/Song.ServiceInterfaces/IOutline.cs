using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 章节管理
    /// </summary>
    public interface IOutline : WeiSha.Core.IBusinessInterface
    {
        #region 章节管理
        /// <summary>
        /// 添加章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OutlineAdd(Outline entity);
        /// <summary>
        /// 批量添加章节，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程（或课程）id</param>
        /// <param name="names">名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        Outline OutlineBatchAdd(int orgid, long  sbjid, long couid, string names);
        /// <summary>
        /// 是否已经存在章节
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程（或课程）id</param>
        /// <param name="pid">上级id</param>
        /// <param name="name"></param>
        /// <returns></returns>
        Outline OutlineIsExist(int orgid, long  sbjid, long couid, long pid, string name);
        /// <summary>
        /// 修改章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OutlineSave(Outline entity);
        /// <summary>
        /// 更新章节的试题数
        /// </summary>
        /// <param name="olid">章节Id</param>
        /// <param name="count">试题数</param>
        /// <returns></returns>
        int UpdateQuesCount(long olid, int count);
        /// <summary>
        /// 导入章节，导入时不立即生成缓存
        /// </summary>
        /// <param name="entity"></param>
        void OutlineInput(Outline entity);
        /// <summary>
        /// 导出课程章节到Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        string OutlineExport4Excel(string path, long couid);
        /// <summary>
        /// 删除章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OutlineDelete(Outline entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void OutlineDelete(long identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Outline OutlineSingle(long identify);
        /// <summary>
        /// 获取单一实体对象，按唯一值，即UID；
        /// </summary>
        /// <param name="uid">全局唯一值</param>
        /// <returns></returns>
        Outline OutlineSingle(string uid);
        /// <summary>
        /// 获取某个课程内的章节，按级别取
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="names">多级名称</param>
        /// <returns></returns>
        Outline OutlineSingle(long couid, List<string> names);        
        /// <summary>
        /// 当前章节下的所有子章节id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<long> TreeID(long id);
        /// <summary>
        /// 获取某个课程下第一个章节
        /// </summary>
        /// <param name="couid">课程Id</param>
        /// <param name="isUse">是否包括只是允许的章节,null取所有范围，true只是允许采用的章节,false反之</param>
        /// <returns></returns>
        Outline OutlineFirst(long couid, bool? isUse);
        /// <summary>
        /// 获取章节名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        string OutlineName(long identify);
        /// <summary>
        /// 获取所有课程章节
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Outline[] OutlineAll(long couid, bool? isUse);
        /// <summary>
        /// 生成树形结构的章节列表
        /// </summary>
        /// <param name="outlines"></param>
        /// <returns></returns>
        DataTable OutlineTree(Song.Entities.Outline[] outlines);
        /// <summary>
        /// 清空章节下试题和附件
        /// </summary>
        /// <param name="identify"></param>
        void OutlineClear(long identify);
        /// <summary>
        /// 清理无效章节
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        int OutlineCleanup(long couid);
        ///// <summary>
        ///// 构建缓存
        ///// </summary>
        ///// <returns></returns>
        //List<Outline> OutlineBuildCache();
        /// <summary>
        /// 获取指定个数的章节列表
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        Outline[] OutlineCount(long couid, string search, bool? isUse, int count);
        /// <summary>
        /// 获取指定个数的章节列表
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="islive">是否是直播章节</param>
        /// <param name="search"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Outline[] OutlineCount(long couid, bool? islive, string search, bool? isUse, int count);
        /// <summary>
        /// 直播中的章节
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<Outline> OutlineLiving(int orgid, int count);
        /// <summary>
        /// 取指定数量的章节
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid">父id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Outline[] OutlineCount(long couid, long pid, bool? isUse, int count);
        /// <summary>
        /// 取指定数量的章节
        /// </summary>
        /// <param name="orgid">机构Id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="pid">章节上级Id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Outline[] OutlineCount(int orgid, long  sbjid, long couid, long pid, bool? isUse, int count);
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int OutlineOfCount(long couid, long pid, bool? isUse);
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id<</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <param name="children">是否含下级</param>
        /// <returns></returns>
        int OutlineOfCount(long couid, long pid, bool? isUse, bool children);
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isVideo">是否有视频</param>
        /// <param name="isFinish">章节是否完节</param>
        /// <returns></returns>
        int OutlineOfCount(long couid, long pid, bool? isUse, bool? isVideo, bool? isFinish);
        /// <summary>
        /// 是否有子级章节
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool OutlineIsChildren(long couid, long pid, bool? isUse);
        /// <summary>
        /// 当前章节是否有试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool OutlineIsQues(long olid, bool? isUse);
        /// <summary>
        /// 当前章节的子级章节
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Outline[] OutlineChildren(long couid, long pid, bool? isUse, int count);
        /// <summary>
        /// 分页取课程章节的信息
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Outline[] OutlinePager(long couid, bool? isUse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 当前章节的试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] QuesCount(long olid, int type, bool? isUse, int count);        
        /// <summary>
        /// 当前章节有多少道试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="isAll">是否取所有（当前章节下所有子章节的试题一块算）</param>
        /// <returns></returns>
        int QuesOfCount(long olid, int type, bool? isUse, bool isAll);        
        /// <summary>
        /// 更改章节的排序
        /// </summary>
        /// <param name="list">专业列表，Ol_ID、Ol_PID、Ol_Tax、Ol_Level</param>
        /// <returns></returns>
        bool UpdateTaxis(Outline[] list);
        #endregion

        #region 章节事件
        /// <summary>
        /// 添加章节中视频播放事件
        /// </summary>
        /// <param name="entity"></param>
        void EventAdd(OutlineEvent entity);
        /// <summary>
        /// 修改播放事件
        /// </summary>
        /// <param name="entity"></param>
        void EventSave(OutlineEvent entity);
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="entity"></param>
        void EventDelete(OutlineEvent entity);
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="identify"></param>
        void EventDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        OutlineEvent EventSingle(int identify);
        /// <summary>
        /// 返回章节下所有事件
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="olid">章节ID，不可以为零，否则会取所有</param>
        /// <param name="type">事件类型，1为提醒，2为知识展示，3课堂提问，4实时反馈（例如，选择某项后跳转到某秒）</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        OutlineEvent[] EventAll(long couid, long olid, int type, bool? isUse);
        /// <summary>
        /// 返回章节下所有事件
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="uid">章节的全局唯一值</param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        OutlineEvent[] EventAll(long couid, string uid, int type, bool? isUse);
        /// <summary>
        /// 获取试题类型的信息
        /// </summary>
        /// <param name="oeid"></param>
        /// <returns></returns>
        DataTable EventQues(int oeid);
        /// <summary>
        /// 获取时间反馈的信息
        /// </summary>
        /// <param name="oeid"></param>
        /// <returns></returns>
        DataTable EventFeedback(int oeid);
        #endregion
    }
}
