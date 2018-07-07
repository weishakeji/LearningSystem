using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 试卷的管理
    /// </summary>
    public interface ITestPaper : WeiSha.Common.IBusinessInterface
    {
        #region 试卷管理
        /// <summary>
        /// 添加试卷
        /// </summary>
        /// <param name="entity">试卷对象</param>
        int PagerAdd(TestPaper entity);
        /// <summary>
        /// 修改试卷
        /// </summary>
        /// <param name="entity">业务实体</param>
        void PagerSave(TestPaper entity);
        /// <summary>
        /// 删除试卷，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void PagerDelete(int identify);
        /// <summary>
        /// 获取单一试卷实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        TestPaper PagerSingle(int identify);
        TestPaper PagerSingle(string name);
        /// <summary>
        /// 获取试卷
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="couid">课程id</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TestPaper[] PagerCount(int orgid, int sbjid, int couid, int diff, bool? isUse, int count);
        TestPaper[] PagerCount(string search, int orgid, int sbjid, int couid, int diff, bool? isUse, int count);
        /// <summary>
        /// 计算有多少个试卷
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        int PagerOfCount(int orgid, int sbjid, int couid, int diff, bool? isUse);
        /// <summary>
        /// 分页获取试卷
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">学科id</param>
        /// <param name="diff">难度等级</param>
        /// <param name="isUse">是否使用</param>
        /// <param name="sear">标题检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestPaper[] PaperPager(int orgid, int sbjid, int couid, int diff, bool? isUse, string sear, int size, int index, out int countSum);

        #endregion

        #region 试卷的试题项
        /// <summary>
        /// 按课程抽题时，试卷各题型占比与分数
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <returns></returns>
        TestPaperItem[] GetItemForAll(TestPaper tp);
        /// <summary>
        /// 按章节抽题时，各题型占比
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <returns></returns>
        TestPaperItem[] GetItemForOlPercent(TestPaper tp);
        /// <summary>
        /// 按章节抽题时，各章节题型数量
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <param name="olid">章节id，如果小于1，则取所有</param>
        /// <returns></returns>
        TestPaperItem[] GetItemForOlCount(TestPaper tp, int olid);
        /// <summary>
        /// 返回试卷的大项，不管是按课程，还是按章节
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        TestPaperItem[] GetItemForAny(TestPaper tp);
        #endregion

        #region 出卷
        /// <summary>
        /// 出卷，输出试卷内容
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <returns></returns>
        Dictionary<TestPaperItem, Questions[]> Putout(TestPaper tp);
        #endregion

        #region 试卷测试的答题
        /// <summary>
        /// 添加测试成绩,返回得分
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回得分</returns>
        float ResultsAdd(TestResults entity);
        /// <summary>
        /// 修改测试成绩,返回得分
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns>返回得分</returns>
        float ResultsSave(TestResults entity);
        /// <summary>
        /// 当前考试的及格率
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        float ResultsPassrate(int identify);
        /// <summary>
        /// 参考人次
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        int ResultsPersontime(int identify);
        /// <summary>
        /// 计算该试卷的所有测试的平均分
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        float ResultsAverage(int identify);
        /// <summary>
        /// 计算该试卷的所有测试的最高分
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        TestResults ResultsHighest(int identify);
        /// <summary>
        /// 计算该试卷的所有测试的最低分
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        TestResults ResultsLowest(int identify);
        /// <summary>
        /// 删除测试成绩，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ResultsDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        TestResults ResultsSingle(int identify);
        /// <summary>
        /// 获取某员工的测试成绩
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="sbjid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        TestResults[] ResultsCount(int stid, int sbjid, int couid, string sear, int count);
        /// <summary>
        /// 分页获取测试成绩
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestResults[] ResultsPager(int stid, int sbjid, int couid, int size, int index, out int countSum);
        TestResults[] ResultsPager(int stid, int sbjid, int couid, string sear, int size, int index, out int countSum);
        /// <summary>
        /// 按试卷分页返回测试成绩
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        TestResults[] ResultsPager(int stid, int tpid, int size, int index, out int countSum);
        #endregion


    }
}
