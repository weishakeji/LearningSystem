using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Common;
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
            Gateway.Default.Save<Navigation>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void NaviSave(Navigation entity)
        {
            //验证父级节点是否正常
            if (entity.Nav_ID == entity.Nav_PID) throw new Exception("请勿将自身设置为父级");
            if (entity.Nav_PID > 0)
            {
                Navigation parent = this.NaviSingle(entity.Nav_PID);
                while (parent.Nav_PID != 0)
                {
                    if (entity.Nav_ID == parent.Nav_PID) throw new Exception("请勿将自身的下级设置为父级");
                    parent = this.NaviSingle(parent.Nav_PID);
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

            Gateway.Default.Save<Navigation>(entity);
        }
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void NaviDelete(int identify)
        {
            Navigation nav = NaviSingle(identify);
            if (nav == null) return;
            if (!string.IsNullOrWhiteSpace(nav.Nav_Logo))
                WeiSha.WebControl.FileUpload.Delete("Org", nav.Nav_Logo);
            //
            Navigation[] child = this.NaviChildren(identify, null);
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
        /// <summary>
        /// 获取所有导航
        /// </summary>
        /// <param name="isShow">是否在前台显示</param>
        /// <param name="site">站点分类，企业站web，手机站mobi，微网站weixin，默认为web</param>
        /// <param name="type">某一类导航</param>
        /// <returns></returns>
        public Navigation[] NaviAll(bool? isShow, string site, string type, int orgid)
        {
            WhereClip wc = Navigation._.Org_ID == orgid;
            if (isShow != null) wc.And(Navigation._.Nav_IsShow == (bool)isShow);
            //所属站点
            if (!string.IsNullOrWhiteSpace(site) && site.Trim() != "")
                wc.And(Navigation._.Nav_Site == site);
            //导航分类
            if (!string.IsNullOrWhiteSpace(type) && type.Trim() != "") 
                wc.And(Navigation._.Nav_Type == type);
            return Gateway.Default.From<Navigation>().Where(wc).OrderBy(Navigation._.Nav_Tax.Asc).ToArray<Navigation>();
        }
        public Navigation[] NaviAll(bool? isShow, string site, string type, int orgid, int pid)
        {
            WhereClip wc = Navigation._.Org_ID == orgid && Navigation._.Nav_ID != Navigation._.Nav_PID;
            if (pid >= 0) wc.And(Navigation._.Nav_PID == pid);
            if (isShow != null) wc.And(Navigation._.Nav_IsShow == (bool)isShow);
            //所属站点
            if (!string.IsNullOrWhiteSpace(site) && site.Trim() != "")
                wc.And(Navigation._.Nav_Site == site);
            //导航分类
            if (!string.IsNullOrWhiteSpace(type) && type.Trim() != "")
                wc.And(Navigation._.Nav_Type == type);
            return Gateway.Default.From<Navigation>().Where(wc).OrderBy(Navigation._.Nav_Tax.Asc).ToArray<Navigation>();
        }
        /// <summary>
        /// 当前分类的下级分类
        /// </summary>
        /// <param name="pId">父级id，如果小于等0，仍作为0使用</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        public Navigation[] NaviChildren(int pid, bool? isShow)
        {
            WhereClip wc = Navigation._.Nav_PID == pid;
            if (isShow != null) wc.And(Navigation._.Nav_IsShow == (bool)isShow);
            return Gateway.Default.From<Navigation>().Where(wc).OrderBy(Navigation._.Nav_Tax.Asc).ToArray<Navigation>();
        }
        /// <summary>
        /// 将当前栏目向上移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool NaviRemoveUp(int id)
        {
            //当前对象
            Navigation current = Gateway.Default.From<Navigation>().Where(Navigation._.Nav_ID == id).ToFirst<Navigation>();
            int tax = (int)current.Nav_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            Navigation prev = Gateway.Default.From<Navigation>().Where(Navigation._.Nav_Tax < tax &&
                Navigation._.Org_ID == current.Org_ID && Navigation._.Nav_Site == current.Nav_Site && Navigation._.Nav_Type == current.Nav_Type && Navigation._.Nav_PID == current.Nav_PID)
                .OrderBy(Navigation._.Nav_Tax.Desc).ToFirst<Navigation>();
            if (prev == null) return false;

            //交换排序号
            current.Nav_Tax = prev.Nav_Tax;
            prev.Nav_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Navigation>(current);
                    tran.Save<Navigation>(prev);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 将当前栏目向下移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于最底端，则返回false；移动成功，返回true</returns>
        public bool NaviRemoveDown(int id)
        {
            //当前对象
            Navigation current = Gateway.Default.From<Navigation>().Where(Navigation._.Nav_ID == id).ToFirst<Navigation>();
            int tax = (int)current.Nav_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            Navigation next = Gateway.Default.From<Navigation>().Where(Navigation._.Nav_Tax > tax &&
                 Navigation._.Org_ID == current.Org_ID && Navigation._.Nav_Site == current.Nav_Site && Navigation._.Nav_Type == current.Nav_Type && Navigation._.Nav_PID == current.Nav_PID)
                 .OrderBy(Navigation._.Nav_Tax.Asc).ToFirst<Navigation>();
            if (next == null) return false;
            //交换排序号
            current.Nav_Tax = next.Nav_Tax;
            next.Nav_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Navigation>(current);
                    tran.Save<Navigation>(next);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
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
            Gateway.Default.Save<ShowPicture>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ShowPicSave(ShowPicture entity)
        {
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
                WeiSha.WebControl.FileUpload.Delete("ShowPic", shp.Shp_File);
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
        /// 将当前项目向上移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ShowPicUp(int id)
        {
            //当前对象
            ShowPicture current = Gateway.Default.From<ShowPicture>().Where(ShowPicture._.Shp_ID == id).ToFirst<ShowPicture>();
            int tax = (int)current.Shp_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            ShowPicture prev = Gateway.Default.From<ShowPicture>().Where(ShowPicture._.Shp_Tax < tax &&
                ShowPicture._.Org_ID == current.Org_ID && ShowPicture._.Shp_Site == current.Shp_Site)
                .OrderBy(ShowPicture._.Shp_Tax.Desc).ToFirst<ShowPicture>();
            if (prev == null) return false;
            //交换排序号
            current.Shp_Tax = prev.Shp_Tax;
            prev.Shp_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ShowPicture>(current);
                    tran.Save<ShowPicture>(prev);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 将当前项目向下移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ShowPicDown(int id)
        {
            //当前对象
            ShowPicture current = Gateway.Default.From<ShowPicture>().Where(ShowPicture._.Shp_ID == id).ToFirst<ShowPicture>();
            int tax = (int)current.Shp_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            ShowPicture next = Gateway.Default.From<ShowPicture>().Where(ShowPicture._.Shp_Tax > tax &&
                 ShowPicture._.Org_ID == current.Org_ID && ShowPicture._.Shp_Site == current.Shp_Site)
                 .OrderBy(ShowPicture._.Shp_Tax.Asc).ToFirst<ShowPicture>();
            if (next == null) return false;
            //交换排序号
            current.Shp_Tax = next.Shp_Tax;
            next.Shp_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ShowPicture>(current);
                    tran.Save<ShowPicture>(next);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    tran.Close();
                }
            }
        }
        #endregion
    }
}
