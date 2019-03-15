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
    public interface IOutline : WeiSha.Common.IBusinessInterface
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
        Outline OutlineBatchAdd(int orgid, int sbjid, int couid, string names);
        /// <summary>
        /// 是否已经存在章节
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程（或课程）id</param>
        /// <param name="pid">上级id</param>
        /// <param name="name"></param>
        /// <returns></returns>
        Outline OutlineIsExist(int orgid, int sbjid, int couid, int pid, string name);
        /// <summary>
        /// 修改章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OutlineSave(Outline entity);
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
        string OutlineExport4Excel(string path, int couid);
        /// <summary>
        /// 删除章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OutlineDelete(Outline entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void OutlineDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Outline OutlineSingle(int identify);
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
        Outline OutlineSingle(int couid, List<string> names);        
        /// <summary>
        /// 当前章节下的所有子章节id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<int> TreeID(int id);
        /// <summary>
        /// 获取某个课程下第一个章节
        /// </summary>
        /// <param name="couid">课程Id</param>
        /// <param name="isUse">是否包括只是允许的章节,null取所有范围，true只是允许采用的章节,false反之</param>
        /// <returns></returns>
        Outline OutlineFirst(int couid, bool? isUse);
        /// <summary>
        /// 获取章节名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        string OutlineName(int identify);
        /// <summary>
        /// 获取所有课程章节
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Outline[] OutlineAll(int couid, bool? isUse);
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
        void OutlineClear(int identify);
        /// <summary>
        /// 清理无效章节
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        int OutlineCleanup(int couid);
        ///// <summary>
        ///// 构建缓存
        ///// </summary>
        ///// <returns></returns>
        //List<Outline> OutlineBuildCache();
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        Outline[] OutlineCount(int couid, string search, bool? isUse, int count);
        /// <summary>
        /// 取指定数量的章节
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid">父id</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Outline[] OutlineCount(int couid, int pid, bool? isUse, int count);
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
        Outline[] OutlineCount(int orgid, int sbjid, int couid, int pid, bool? isUse, int count);
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int OutlineOfCount(int couid, int pid, bool? isUse);
        int OutlineOfCount(int orgid, int sbjid, int couid, int pid, bool? isUse);
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isVideo">是否有视频</param>
        /// <param name="isFinish">章节是否完节</param>
        /// <returns></returns>
        int OutlineOfCount(int couid, int pid, bool? isUse, bool? isVideo, bool? isFinish);
        /// <summary>
        /// 是否有子级章节
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool OutlineIsChildren(int couid, int pid, bool? isUse);
        /// <summary>
        /// 当前章节是否有试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool OutlineIsQues(int olid, bool? isUse);
        /// <summary>
        /// 当前章节的子级章节
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Outline[] OutlineChildren(int couid, int pid, bool? isUse, int count);
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
        Outline[] OutlinePager(int couid, bool? isUse, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 当前章节的试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Questions[] QuesCount(int olid, int type, bool? isUse, int count);        
        /// <summary>
        /// 当前章节有多少道试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="isAll">是否取所有（当前章节下所有子章节的试题一块算）</param>
        /// <returns></returns>
        int QuesOfCount(int olid, int type, bool? isUse, bool isAll);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool OutlineUp(int couid, int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool OutlineDown(int couid, int id);
        /// <summary>
        /// 将当前章节升级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool OutlineToLeft(int couid, int id);
        /// <summary>
        /// 将当前章节退级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool OutlineToRight(int couid, int id);
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
        OutlineEvent[] EventAll(int couid, int olid, int type, bool? isUse);
        /// <summary>
        /// 返回章节下所有事件
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="uid">章节的全局唯一值</param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        OutlineEvent[] EventAll(int couid, string uid, int type, bool? isUse);
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

        #region 事件
        /// <summary>
        /// 当章节更改时
        /// </summary>
        event EventHandler Save;
        event EventHandler Add;
        event EventHandler Delete;
        void OnSave(object sender, EventArgs e);
        void OnAdd(object sender, EventArgs e);
        void OnDelete(object sender, EventArgs e);
        #endregion
    }
}
