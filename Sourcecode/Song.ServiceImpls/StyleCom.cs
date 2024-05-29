using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Core;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;

namespace Song.ServiceImpls
{
    public class StyleCom : IStyle
    {
        #region 导航管理
        /// <summary>
        /// 添加导航项目
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void NaviAdd(Navigation entity)
        {
            entity.Nav_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //如果没有排序号，则自动计算
            if (entity.Nav_Tax < 1)
            {
                object obj = Gateway.Default.Max<Navigation>(Navigation._.Nav_Tax,
                    Navigation._.Org_ID == entity.Org_ID && Navigation._.Nav_Site == entity.Nav_Site && Navigation._.Nav_Type == entity.Nav_Type && Navigation._.Nav_PID == entity.Nav_PID);
                entity.Nav_Tax = obj is int ? (int)obj + 1 : 0;
            }
            if (!string.IsNullOrWhiteSpace(entity.Nav_Logo))
            {
                if (entity.Nav_Logo.IndexOf("/") > -1)
                {
                    entity.Nav_Logo = entity.Nav_Logo.Substring(entity.Nav_Logo.LastIndexOf("/") + 1);
                }
            }
            Gateway.Default.Save<Navigation>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void NaviSave(Navigation entity)
        {
            //验证父级节点是否正常
            if (entity.Nav_UID == entity.Nav_PID) throw new Exception("请勿将自身设置为父级");
            if (!string.IsNullOrWhiteSpace(entity.Nav_PID))
            {
                Navigation parent = this.NaviSingle(entity.Nav_PID);
                while (!string.IsNullOrWhiteSpace(entity.Nav_PID))
                {
                    if (entity.Nav_UID == parent.Nav_PID) throw new Exception("请勿将自身的下级设置为父级");
                    parent = this.NaviSingle(parent.Nav_PID);
                    if (parent == null) break;
                }
            }
            
            //假如当前导航的所在父级变化了，排序号重新生成
            Navigation old = this.NaviSingle(entity.Nav_ID);
            if (old != null && old.Nav_PID != entity.Nav_PID)
            {
                object obj = Gateway.Default.Max<Navigation>(Navigation._.Nav_Tax, 
                    Navigation._.Nav_Site == entity.Nav_Site && Navigation._.Nav_Type == entity.Nav_Type && Navigation._.Nav_PID == entity.Nav_PID);
                entity.Nav_Tax = obj is int ? (int)obj + 1 : 0;
            }
            if (!string.IsNullOrWhiteSpace(entity.Nav_Logo))
            {
                if (entity.Nav_Logo.IndexOf("/") > -1)
                {
                    entity.Nav_Logo = entity.Nav_Logo.Substring(entity.Nav_Logo.LastIndexOf("/") + 1);
                }
            }
            Gateway.Default.Save<Navigation>(entity);
        }
        /// <summary>
        /// 修改导航的显示状态
        /// </summary>
        /// <param name="id">导航id</param>
        /// <param name="show">是否显示</param>
        /// <returns></returns>
        public bool NaviState(int id, bool show)
        {
            Gateway.Default.Update<Navigation>(
                          new Field[] { Navigation._.Nav_IsShow },
                          new object[] { show },
                          Navigation._.Nav_ID == id);
            return true;
        }
        /// <summary>
        /// 单独修改导航的图片地址
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="logo"></param>
        public void NaviSaveLogo(Navigation entity, string logo)
        {
            if (logo.IndexOf("/") > -1)
            {
                logo = logo.Substring(logo.LastIndexOf("/") + 1);
            }
            Gateway.Default.Update<Navigation>(
                          new Field[] { Navigation._.Nav_Logo },
                          new object[] { logo },
                          Navigation._.Nav_ID == entity.Nav_ID);
        }
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void NaviDelete(int identify)
        {
            Navigation nav = NaviSingle(identify);
            if (nav == null) return;
            WeiSha.Core.Upload.Get["Org"].DeleteFile(nav.Nav_Logo);         
            //
            Navigation[] child = this.NaviChildren(nav.Nav_UID, null);
            foreach (Navigation n in child)
            {
                NaviDelete(n.Nav_ID);
            }            
            Gateway.Default.Delete<Navigation>(Navigation._.Nav_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Navigation NaviSingle(int identify)
        {
            return Gateway.Default.From<Navigation>().Where(Navigation._.Nav_ID == identify).ToFirst<Navigation>();
        }
        public Navigation NaviSingle(string uid)
        {
            return Gateway.Default.From<Navigation>().Where(Navigation._.Nav_UID == uid).ToFirst<Navigation>();
        }
        /// <summary>
        /// 获取所有导航
        /// </summary>
        /// <param name="isShow">是否在前台显示</param>
        /// <param name="site">站点分类，企业站web，手机站mobi，微网站weixin，默认为web</param>
        /// <param name="type">某一类导航</param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public List<Navigation> NaviAll(bool? isShow, string site, string type, int orgid)
        {
            WhereClip wc = Navigation._.Org_ID == orgid;
            if (isShow != null) wc.And(Navigation._.Nav_IsShow == (bool)isShow);
            //所属站点
            if (!string.IsNullOrWhiteSpace(site)) wc.And(Navigation._.Nav_Site == site);
            //导航分类
            if (!string.IsNullOrWhiteSpace(type)) wc.And(Navigation._.Nav_Type == type);
            return Gateway.Default.From<Navigation>().Where(wc).OrderBy(Navigation._.Nav_Tax.Asc).ToList<Navigation>();
        }
        public List<Navigation> NaviAll(bool? isShow, string site, string type, int orgid, string pid)
        {
            WhereClip wc = Navigation._.Org_ID == orgid;
            if (!string.IsNullOrWhiteSpace(pid)) wc.And(Navigation._.Nav_PID == pid);
            if (isShow != null) wc.And(Navigation._.Nav_IsShow == (bool)isShow);
            //所属站点
            if (!string.IsNullOrWhiteSpace(site) && site.Trim() != "")
                wc.And(Navigation._.Nav_Site == site);
            //导航分类
            if (!string.IsNullOrWhiteSpace(type) && type.Trim() != "")
                wc.And(Navigation._.Nav_Type == type);
            return Gateway.Default.From<Navigation>().Where(wc).OrderBy(Navigation._.Nav_Tax.Asc).ToList<Navigation>();
        }
        /// <summary>
        /// 当前分类的下级分类
        /// </summary>
        /// <param name="pid">父级id，如果小于等0，仍作为0使用</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        public Navigation[] NaviChildren(string pid, bool? isShow)
        {
            WhereClip wc = Navigation._.Nav_PID == pid;
            if (isShow != null) wc.And(Navigation._.Nav_IsShow == (bool)isShow);
            return Gateway.Default.From<Navigation>().Where(wc).OrderBy(Navigation._.Nav_Tax.Asc).ToArray<Navigation>();
        }
        /// <summary>
        /// 更新导航菜单树
        /// </summary>
        /// <param name="site"></param>
        /// <param name="type"></param>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool UpdateNavigation(string site, string type,int orgid,string pid, Navigation[] items)
        {
            List<Navigation> mms = this.NaviAll(null, site, type, orgid, pid);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Navigation m in mms)
                    {
                        tran.Delete<Navigation>(Navigation._.Nav_ID == m.Nav_ID);
                    }
                    foreach (Navigation item in items)
                    {
                        item.Org_ID = orgid;
                        if (string.IsNullOrWhiteSpace(item.Nav_Name))
                            item.Nav_Name = "null";
                        if (string.IsNullOrWhiteSpace(item.Nav_UID))
                            item.Nav_UID = WeiSha.Core.Request.UniqueID();
                        //如果图片不等空
                        if (!string.IsNullOrWhiteSpace(item.Nav_Logo))
                        {
                            if (item.Nav_Logo.IndexOf("/") > -1)
                                item.Nav_Logo = item.Nav_Logo.Substring(item.Nav_Logo.LastIndexOf("/")+1);
                        }
                        tran.Save<Navigation>(item);
                    }
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        #endregion

        #region 轮换图片管理
        /// <summary>
        /// 添加轮换图片
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ShowPicAdd(ShowPicture entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;            
            //如果没有排序号，则自动计算
            if (entity.Shp_Tax < 1)
            {
                object obj = Gateway.Default.Max<ShowPicture>(ShowPicture._.Shp_Tax,
                    ShowPicture._.Org_ID == entity.Org_ID && ShowPicture._.Shp_Site == entity.Shp_Site);
                entity.Shp_Tax = obj is int ? (int)obj + 1 : 0;
            }
            if (!string.IsNullOrWhiteSpace(entity.Shp_File))
            {
                if (entity.Shp_File.IndexOf("/") > -1)
                {
                    entity.Shp_File = entity.Shp_File.Substring(entity.Shp_File.LastIndexOf("/") + 1);
                }
            }
            Gateway.Default.Save<ShowPicture>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ShowPicSave(ShowPicture entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Shp_File))
            {
                if (entity.Shp_File.IndexOf("/") > -1)
                {
                    entity.Shp_File = entity.Shp_File.Substring(entity.Shp_File.LastIndexOf("/") + 1);
                }
            }
            Gateway.Default.Save<ShowPicture>(entity);
        }
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ShowPicDelete(int identify)
        {
            ShowPicture shp = ShowPicSingle(identify);
            if (shp == null) return;
            if (!string.IsNullOrWhiteSpace(shp.Shp_File))
                WeiSha.Core.Upload.Get["ShowPic"].DeleteFile(shp.Shp_File);
            Gateway.Default.Delete<ShowPicture>(ShowPicture._.Shp_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ShowPicture ShowPicSingle(int identify)
        {
            return Gateway.Default.From<ShowPicture>().Where(ShowPicture._.Shp_ID == identify).ToFirst<ShowPicture>();
        }
        /// <summary>
        /// 获取轮换图片
        /// </summary>
        /// <param name="isShow">是否在前台显示</param>
        /// <param name="site">站点分类，企业站web，手机站mobi，微网站weixin，默认为web</param>       
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public ShowPicture[] ShowPicAll(bool? isShow, string site, int orgid)
        {
            WhereClip wc = ShowPicture._.Org_ID == orgid;
            if (isShow != null) wc.And(ShowPicture._.Shp_IsShow == (bool)isShow);
            //所属站点
            if (!string.IsNullOrWhiteSpace(site) && site.Trim() != "")
                wc.And(ShowPicture._.Shp_Site == site);
            return Gateway.Default.From<ShowPicture>().Where(wc).OrderBy(ShowPicture._.Shp_Tax.Asc).ToArray<ShowPicture>();
        }
        /// <summary>
        /// 更改顺序
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool ShowUpdateTaxis(ShowPicture[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (ShowPicture item in items)
                    {
                        tran.Update<ShowPicture>(
                            new Field[] { ShowPicture._.Shp_Tax },
                            new object[] { item.Shp_Tax },
                            ShowPicture._.Shp_ID == item.Shp_ID);
                    }
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        #endregion
    }
}
