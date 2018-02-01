using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 院系职位的管理
    /// </summary>
    public interface IVote : WeiSha.Common.IBusinessInterface
    {
        #region 调查主题的操作
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ThemeAdd(Vote entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ThemeSave(Vote entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ThemeDelete(Vote entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ThemeDelete(int identify);
        /// <summary>
        /// 获取主题，如果当前主题不存在，则返回最新的调查主题
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        Vote GetTheme(int identify);
        /// <summary>
        /// 获取所有简单调查的主题
        /// </summary>
        /// <param name="isShow"></param>
        /// <param name="isUse"></param>
        /// <param name="count">如果小于等于0，则取所有</param>
        /// <returns></returns>
        Vote[] GetSimpleTheme(bool? isShow, bool? isUse,int count);
        #endregion

        #region 调查项的操作
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ItemAdd(Vote entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ItemSave(Vote entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ItemDelete(Vote entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ItemDelete(int identify);
        #endregion
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Vote GetSingle(int identify);
        /// <summary>
        /// 向某个选项，增加一个投票数
        /// </summary>
        /// <param name="identify"></param>
        void AddResult(int identify);
        /// <summary>
        /// 向某个选项，增加指定投票数
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="num">指定的票数</param>
        void AddResult(int identify,int num);
        /// <summary>
        /// 获取某个调查的调查子项
        /// </summary>
        /// <param name="uniqueid"></param>
        /// <returns></returns>
        DataTable GetVoteItem(string uniqueid);
        /// <summary>
        /// 获取某个调查的子项，并输出该调查总票数
        /// </summary>
        /// <param name="uniqueid"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        Vote[] GetVoteItem(string uniqueid, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="isShow"></param>
        /// <param name="isUse"></param>
        /// <param name="type">调查类型，1为简单调查，2为复合方多主题调查</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        Vote[] GetPager(bool? isShow, bool? isUse,int type, string searTxt, int size, int index, out int countSum);
        Vote[] GetPager(bool? isShow, bool? isUse, int type, string searTxt, DateTime start, DateTime end, int size, int index, out int countSum);
    }
}
