using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 样式的管理
    /// </summary>
    public interface IStyle : WeiSha.Common.IBusinessInterface
    {
        #region 导航管理
        /// <summary>
        /// 添加导航项目
        /// </summary>
        /// <param name="entity">业务实体</param>
        void NaviAdd(Navigation entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void NaviSave(Navigation entity);
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void NaviDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        Navigation NaviSingle(int identify);
        /// <summary>
        /// 获取所有导航
        /// </summary>
        /// <param name="isShow">是否在前台显示</param>
        /// <param name="site">站点分类，企业站web，手机站mobi，微网站weixin，默认为web</param>
        /// <param name="type">某一类导航</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        Navigation[] NaviAll(bool? isShow, string site, string type, int orgid);
        Navigation[] NaviAll(bool? isShow, string site, string type, int orgid, int pid);
        /// <summary>
        /// 当前分类的下级分类
        /// </summary>
        /// <param name="pId">父级id，如果小于等0，仍作为0使用</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        Navigation[] NaviChildren(int pid, bool? isShow);
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool NaviRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool NaviRemoveDown(int id);
        #endregion

        #region 轮换图片管理
        /// <summary>
        /// 添加轮换图片
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ShowPicAdd(ShowPicture entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ShowPicSave(ShowPicture entity);
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ShowPicDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ShowPicture ShowPicSingle(int identify);
        /// <summary>
        /// 获取轮换图片
        /// </summary>
        /// <param name="isShow">是否在前台显示</param>
        /// <param name="site">站点分类，企业站web，手机站mobi，微网站weixin，默认为web</param>       
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        ShowPicture[] ShowPicAll(bool? isShow, string site, int orgid);
        /// <summary>
        /// 将当前项目向上移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ShowPicUp(int id);
        /// <summary>
        /// 将当前项目向下移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ShowPicDown(int id);
        #endregion
    }
}
