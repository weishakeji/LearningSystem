using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 网站内容的栏目管理
    /// </summary>
    public interface IColumns : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        int Add(Columns entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Save(Columns entity);
        /// <summary>
        /// 修改排序
        /// </summary>
        /// <param name="xml"></param>
        void SaveOrder(string xml);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Columns Single(int identify);
        Columns Single(string uid);
        /// <summary>
        /// 获取所有栏目
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Columns[] All(int orgid, bool? isUse);
        /// <summary>
        /// 取某一类的栏目
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">栏目类型，产品Product，新闻news，图片Picture,视频video,下载download,单页article</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Columns[] ColumnCount(int orgid, string type, bool? isUse, int count);
        /// <summary>
        /// 取某一类的栏目
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">父级uid，为null取所有，为""取顶级</param>
        /// <param name="type">栏目类型，产品Product，新闻news，图片Picture,视频video,下载download,单页article</param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        Columns[] ColumnCount(int orgid, string pid, string type, bool? isUse, int count);
        /// <summary>
        /// 当前栏目下的子级栏目
        /// </summary>
        /// <param name="pid">当前栏目id,如果0，则取顶级栏目</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        Columns[] Children(string pid, bool? isUse);
        /// <summary>
        /// 是否有下级栏目
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        bool  IsChildren(string pid, bool? isUse);
        /// <summary>
        /// 当前栏目下的所有子栏目uid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        List<string> TreeID(string uid);
        /// <summary>
        /// 更新栏目结构
        /// </summary>
        /// <param name="items"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        bool UpdateColumnsTree(Columns[] items, int orgid);

    }
}
