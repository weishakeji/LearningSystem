using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 考试管理
    /// </summary>
    public interface ITrPlan : WeiSha.Common.IBusinessInterface
    {

        #region 考试管理
        /// <summary>
        /// 添加培训计划
        /// </summary>
        /// <param name="theme">培训计划</param>
        /// <param name="groups">参加人员的范围</param>
        void TrpAdd(TrPlan theme, List<ExamGroup> groups);
        /// <summary>
        /// 修改培训计划
        /// </summary>
        /// <param name="theme">培训计划</param>
        /// <param name="yuanType">原参与类型</param>
        /// <param name="newType">新参与类型</param>
        /// <param name="groups">参加人员的范围</param>
        void TrpSave(TrPlan theme, int yuanType, int newType, List<ExamGroup> groups);
        /// <summary>
        /// 删除培训计划，使用主键ID
        /// </summary>
        /// <param name="identify">主键ID</param>
        void TrpDelete(int identify);
        /// <summary>
        /// 删除培训计划，使用页面唯一标识
        /// </summary>
        /// <param name="identify">页面唯一标识</param>
        void TrpDelete(string uid);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        TrPlan TrpSingle(int identify);
        /// <summary>
        /// 获取单一实体对象，通过全局唯一值
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        TrPlan TrpSingle(string uid);
        /// <summary>
        /// 判断指定的数据是否符合要求
        /// </summary>
        /// <param name="uid">页面唯一标识</param>
        /// <param name="depId">院系id（为-1时不作为条件）</param>
        /// <param name="teamId">班组id（为-1时不作为条件）</param>
        /// <returns></returns>
        bool TrpJudge(string uid, int depId, int teamId);
        /// <summary>
        /// 获取指定分类和院系id以及班组id的所有培训计划
        /// </summary>
        /// <param name="groupType">分组类型（1：所有人；2：按院系；3：按班组；-1：符合院系或班组）</param>
        /// <param name="depId">院系id，为-1时不加这个条件。</param>
        /// <param name="teamId">班组id，为-1时不加这个条件。</param>
        /// <returns></returns>
        TrPlan[] TrpItem(int groupType, int depId, int teamId);
   
        /// <summary>
        /// 获取符合条件的数据
        /// </summary>
        /// <param name="time">日期</param>
        /// <param name="startH">开始时间(时)</param>
        /// <param name="startM">开始时间(分)</param>
        /// <param name="endH">结束时间(时)</param>
        /// <param name="endM">结束时间(分)</param>
        /// <param name="depId">责任院系id</param>
        /// <param name="sbjId">专业id</param>
        /// <param name="groupType">分组类型</param>
        /// <param name="result">完成情况</param>
        /// <param name="teacher">培训教师</param>
        /// <param name="content">培训内容</param>
        /// <param name="size">每页显示的条数</param>
        /// <param name="index">当前页码</param>
        /// <param name="countSum">符合条件的数据数量</param>
        /// <returns></returns>
        TrPlan[] TrpItem(DateTime? timestall, DateTime? timeend, int? depId, int? sbjId, int? groupType, int? result, string teacher, string content, int size, int index, out int countSum);
        #endregion
    }
}
