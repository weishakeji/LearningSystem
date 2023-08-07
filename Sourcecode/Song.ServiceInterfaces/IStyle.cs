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
    public interface IStyle : WeiSha.Core.IBusinessInterface
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
        /// <summary>
        /// 修改导航的显示状态
        /// </summary>
        /// <param name="id">导航id</param>
        /// <param name="show">是否显示</param>
        /// <returns></returns>
        bool NaviState(int id, bool show);
        /// <summary>
        /// 单独修改导航的图片地址
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logo"></param>
        void NaviSaveLogo(Navigation entity, string logo);
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
        ///  获取单一实体对象，按Uid；
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Navigation NaviSingle(string uid);
        /// <summary>
        /// 获取所有导航
        /// </summary>
        /// <param name="isShow">是否在前台显示</param>
        /// <param name="site">站点分类，企业站web，手机站mobi，微网站weixin，默认为web</param>
        /// <param name="type">某一类导航</param>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        List<Navigation> NaviAll(bool? isShow, string site, string type, int orgid);
        List<Navigation> NaviAll(bool? isShow, string site, string type, int orgid, string pid);
        /// <summary>
        /// 当前分类的下级分类
        /// </summary>
        /// <param name="pid">父级id</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        Navigation[] NaviChildren(string pid, bool? isShow);
        /// <summary>
        /// 更新导航菜单树
        /// </summary>
        /// <param name="site"></param>
        /// <param name="type"></param>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        bool UpdateNavigation(string site, string type, int orgid,string pid, Navigation[] items);
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
        /// <summary>
        /// 更改顺序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        bool ShowUpdateTaxis(ShowPicture[] items);       
        #endregion
    }
}
