using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 学习卡管理
    /// </summary>
    public interface ILearningCard : WeiSha.Common.IBusinessInterface
    {
        #region 学习卡设置管理
        /// <summary>
        /// 添加学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SetAdd(LearningCardSet entity);
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SetSave(LearningCardSet entity);
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void SetDelete(LearningCardSet entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void SetDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        LearningCardSet SetSingle(int identify);
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        LearningCardSet[] SetCount(int orgid, bool? isEnable, int count);
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        int SetOfCount(int orgid, bool? isEnable);
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCardSet[] SetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 关联课程
        /// <summary>
        /// 获取关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        Course[] CoursesGet(LearningCardSet set);
        Course[] CoursesGet(string xml);
        /// <summary>
        /// 设置关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <param name="courses"></param>
        /// <returns>LearningCardSet对象中的Lcs_RelatedCourses将记录关联信息</returns>
        LearningCardSet CoursesSet(LearningCardSet set, Course[] courses);
        LearningCardSet CoursesSet(LearningCardSet set, int[] couid);
        /// <summary>
        /// 设置关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <param name="couids">课程id串，以逗号分隔</param>
        /// <returns></returns>
        LearningCardSet CoursesSet(LearningCardSet set, string  couids);
        #endregion

        #region 学习卡管理
        /// <summary>
        /// 生成学习卡
        /// </summary>
        /// <param name="set">学习卡的设置项</param>
        /// <param name="factor">随机因子</param>
        /// <returns></returns>
        LearningCard CardGenerate(LearningCardSet set, int factor = -1);
        /// <summary>
        /// 批量生成学习卡
        /// </summary>
        /// <param name="set">学习卡的设置项</param>
        /// <returns></returns>
        LearningCard[] CardGenerate(LearningCardSet set);
        /// <summary>
        /// 添加学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CardAdd(LearningCard entity);
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CardSave(LearningCard entity);
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void CardDelete(LearningCard entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void CardDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        LearningCard CardSingle(int identify);
        /// <summary>
        /// 校验学习卡是否存在，或过期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        LearningCard CardCheck(string code);
        /// <summary>
        /// 使用该学习卡
        /// </summary>
        /// <param name="entity"></param>
        void CardUse(LearningCard entity);
        /// <summary>
        /// 学习卡使用后的回滚，将删除学员的关联课程
        /// </summary>
        /// <param name="entity"></param>
        void CardRollback(LearningCard entity);
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        LearningCard[] CardCount(int orgid, int rsid, bool? isEnable, bool? isUsed, int count);
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <returns></returns>
        int CardOfCount(int orgid, int lcsid, bool? isEnable, bool? isUsed);
        /// <summary>
        /// 导出Excel格式的学习卡信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">学习卡设置项的id</param>
        /// <returns></returns>
        string Card4Excel(string path, int orgid, int rsid);
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        LearningCard[] CardPager(int orgid, int lcsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum);
        #endregion
    }
}
