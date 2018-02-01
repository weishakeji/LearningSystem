using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 论坛的管理
    /// </summary>
    public interface IMessageBoard : WeiSha.Common.IBusinessInterface
    {
        #region 论坛主题的管理
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ThemeAdd(MessageBoard entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ThemeSave(MessageBoard entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ThemeDelete(MessageBoard entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ThemeDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        MessageBoard ThemeSingle(int identify);
        /// <summary>
        /// 取多少条主题
        /// </summary>
        /// <param name="searTxt"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        MessageBoard[] ThemeCount(int orgid, int couid, string searTxt, int count);
        MessageBoard[] ThemePager(int orgid, int couid, bool? isDel, bool? isShow, string searTxt, int size, int index, out int countSum);
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="isDel">是否删除</param>
        /// <param name="couid">课程id</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="isAns">是否回复</param>
        /// <param name="searTxt">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MessageBoard[] ThemePager(int orgid, int couid, bool? isDel, bool? isShow, bool? isAns, string searTxt, int size, int index, out int countSum);
        #endregion

        MessageBoard GetSingle(int identify);
        /// <summary>
        /// 添加回复留言信息
        /// </summary>
        /// <param name="entity"></param>
        void AnswerAdd(MessageBoard entity);
        /// <summary>
        /// 修改回复信息
        /// </summary>
        /// <param name="entity"></param>
        void AnswerSave(MessageBoard entity);
        /// <summary>
        /// 删除回复信息
        /// </summary>
        /// <param name="identify"></param>
        void AnswerDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        MessageBoard AnswerSingle(int identify);
        /// <summary>
        /// 帖子的列表
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        MessageBoard[] ListPager(string uid, int size, int index, out int countSum);
    }
}
