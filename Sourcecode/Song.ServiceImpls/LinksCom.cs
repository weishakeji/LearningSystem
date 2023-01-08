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
    public class LinksCom : ILinks
    {

        #region 友情链接项

        public void LinksAdd(Links entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<Links>(Links._.Lk_Tax, Links._.Ls_Id == entity.Ls_Id && Links._.Org_ID == org.Org_ID);
            int tax = obj is int ? (int)obj : 0;
            entity.Lk_Tax = tax + 1;
            LinksSort ls = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Id == entity.Ls_Id).ToFirst<LinksSort>();
            if (ls != null)
            {
                entity.Ls_Name = ls.Ls_Name;
            }
            entity.Lk_IsApply = false;
            entity.Lk_IsVerify = true;
            //Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //如果图片带有多余路径，只保留文件名
            if (!string.IsNullOrWhiteSpace(entity.Lk_Logo) && entity.Lk_Logo.IndexOf("/") > -1)
                entity.Lk_Logo = entity.Lk_Logo.Substring(entity.Lk_Logo.LastIndexOf("/") + 1);
            if (!string.IsNullOrWhiteSpace(entity.Lk_LogoSmall) && entity.Lk_LogoSmall.IndexOf("/") > -1)
                entity.Lk_LogoSmall = entity.Lk_LogoSmall.Substring(entity.Lk_LogoSmall.LastIndexOf("/") + 1);

            Gateway.Default.Save<Links>(entity);
        }
        public void LinksApply(Links entity)
        {
            entity.Lk_IsApply = true;
            entity.Lk_IsVerify = false;
            Gateway.Default.Save<Links>(entity);
        }
        public void LinksVerify(Links entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            entity.Lk_IsVerify = true;
            entity.Lk_Tax = LinksMaxTaxis((int)entity.Ls_Id, org.Org_ID) + 1;
            LinksSort ls = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Id == entity.Ls_Id).ToFirst<LinksSort>();
            if (ls != null)
            {
                entity.Ls_Name = ls.Ls_Name;
            }
            Gateway.Default.Save<Links>(entity);
        }
        public void LinksSave(Links entity)
        {
            LinksSort ls = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Id == entity.Ls_Id).ToFirst<LinksSort>();
            if (ls != null)
            {
                entity.Ls_Name = ls.Ls_Name;
            }
            //如果图片带有多余路径，只保留文件名
            if (!string.IsNullOrWhiteSpace(entity.Lk_Logo) && entity.Lk_Logo.IndexOf("/") > -1)
                entity.Lk_Logo = entity.Lk_Logo.Substring(entity.Lk_Logo.LastIndexOf("/") + 1);
            if (!string.IsNullOrWhiteSpace(entity.Lk_LogoSmall) && entity.Lk_LogoSmall.IndexOf("/") > -1)
                entity.Lk_LogoSmall = entity.Lk_LogoSmall.Substring(entity.Lk_LogoSmall.LastIndexOf("/") + 1);

            Gateway.Default.Save<Links>(entity);
        }

        public void LinksDelete(Links entity)
        {
            _LinksDelete(entity);
        }

        public void LinksDelete(int identify)
        {
            _LinksDelete(this.LinksSingle(identify));
        }

        public void LinksDelete(int orgid, string name)
        {
            _LinksDelete(this.LinksSingle(orgid, name));
        }

        public Links LinksSingle(int identify)
        {
            return Gateway.Default.From<Links>().Where(Links._.Lk_Id == identify).ToFirst<Links>();
        }

        public Links LinksSingle(int orgid, string ttl)
        {
            return Gateway.Default.From<Links>().Where(Links._.Org_ID == orgid && Links._.Lk_Name == ttl).ToFirst<Links>();
        }

        public int LinksMaxTaxis(int orgid, int sortId)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<Links>(Links._.Lk_Tax, Links._.Ls_Id == sortId && Links._.Org_ID == orgid);
            int tax = 0;
            if (obj is int)
            {
                tax = (int)obj;
            }
            return tax;
        }
        public Links[] GetLinksAll(int orgid, bool? isShow)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Links._.Org_ID == orgid);
            if (isShow != null) wc.And(Links._.Lk_IsShow == isShow);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>();
        }
        /// <summary>
        /// 取友情链接
        /// </summary>
        /// <param name="sortId">分类Id，如果为空则取所有</param>
        /// <param name="isShow"></param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        public Links[] GetLinks(int orgid, int sortId, bool? isShow, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Links._.Org_ID == orgid);
            if (sortId > 0) wc.And(Links._.Ls_Id == sortId);
            if (isShow != null) wc.And(Links._.Lk_IsShow == isShow);
            if (isUse != null) wc.And(Links._.Lk_IsUse == isUse);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc && Links._.Lk_Id.Asc).ToArray<Links>(count);
        }

        public Links[] GetLinks(int orgid, string sortName, bool? isShow, bool? isUse, int count)
        {
            if (string.IsNullOrWhiteSpace(sortName) || sortName.Trim() == "") return null;
            Song.Entities.LinksSort sort = Gateway.Default.From<LinksSort>().Where(LinksSort._.Org_ID == orgid && LinksSort._.Ls_Name == sortName.Trim()).ToFirst<LinksSort>();
            if (sort == null) return null;
            if (isShow != null && (bool)isShow && !sort.Ls_IsShow && !sort.Ls_IsUse) return null;
            return GetLinks(orgid, sort.Ls_Id, isShow, isUse, count);
        }
        public Links[] GetLinksPager(int orgid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Links._.Org_ID == orgid);
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }
        public Links[] GetLinksPager(int orgid, int sortId, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Links._.Org_ID == orgid);
            if (sortId > 0) wc.And(Links._.Ls_Id == sortId);
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有链接项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sortId">分类id</param>
        /// <param name="isUse">是否使用</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="name"></param>
        /// <param name="link"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Links[] GetLinksPager(int orgid, int sortId, bool? isUse, bool? isShow, string name, string link, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Links._.Org_ID == orgid);
            if (sortId > 0) wc.And(Links._.Ls_Id == sortId);
            if (isUse != null) wc.And(Links._.Lk_IsUse == (bool)isUse);
            if (isShow != null) wc.And(Links._.Lk_IsShow == (bool)isShow);
            if (!string.IsNullOrWhiteSpace(name)) wc.And(Links._.Lk_Name.Like("%" + name.Trim() + "%"));
            if (!string.IsNullOrWhiteSpace(link)) wc.And(Links._.Lk_Url.Like("%" + link.Trim() + "%"));
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }       
        public Links[] GetLinksPager(int orgid, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Links._.Org_ID == orgid);
            if (isShow != null) wc.And(Links._.Lk_IsShow == (bool)isShow);
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 私有对象，用于删除对象的子级，以及相关信息
        /// </summary>
        /// <param name="entity"></param>
        private void _LinksDelete(Links entity)
        {
            if (entity == null)
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(entity.Lk_Logo))
            {
                //删除图片
                WeiSha.Core.Upload.Get["Links"].DeleteFile(entity.Lk_Logo);
            }
            //删除自身            
            Gateway.Default.Delete<Links>(entity);
        }
        #endregion

        #region 友情链接的分类
        /// <suPsary>
        /// 添加
        /// </suPsary>
        /// <param name="entity">业务实体</param>
        public int SortAdd(LinksSort entity)
        {
            //如果父节点小于0，违反逻辑
            if (entity.Ls_PatId < 0) return -1;
            //如果名称为空
            if (!(entity.Ls_Name != null && entity.Ls_Name.Trim() != "")) return -1;
            if (entity.Ls_PatId > 0)
            {
                LinksSort parent = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_PatId == entity.Ls_Id).ToFirst<LinksSort>();
                if (parent == null) return -1;//如果父节点不存在
                //新增对象的属性，遵循上级；
                entity.Ls_IsUse = parent.Ls_IsUse;
            }
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<LinksSort>(LinksSort._.Ls_Tax, LinksSort._.Ls_PatId == entity.Ls_PatId);       
            entity.Ls_Tax = obj is int ?  (int)obj + 1 : entity.Ls_Tax;
            if (entity.Org_ID < 1)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null)
                {
                    entity.Org_ID = org.Org_ID;
                    entity.Org_Name = org.Org_Name;
                }
            }
            return Gateway.Default.Save<LinksSort>(entity);           
        }
        /// <suPsary>
        /// 修改
        /// </suPsary>
        /// <param name="entity">业务实体</param>
        public void SortSave(LinksSort entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<LinksSort>(entity);
                    tran.Update<Links>(new Field[] { Links._.Ls_Name }, new object[] { entity.Ls_Name }, Links._.Ls_Id == entity.Ls_Id);
                    tran.Commit();
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
        /// 修改属性
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        public void SortUpdate(int id, Field[] fiels, object[] objs)
        {
            Gateway.Default.Update<LinksSort>(fiels, objs, LinksSort._.Ls_Id == id);
        }
        /// <suPsary>
        /// 删除
        /// </suPsary>
        /// <param name="entity">业务实体</param>
        public void SortDelete(LinksSort entity)
        {
            this.SortDelete(entity.Ls_Id);
        }
        /// <suPsary>
        /// 删除，按主键ID；
        /// </suPsary>
        /// <param name="identify">实体的主键</param>
        public void SortDelete(int identify)
        {
            if (identify <= 0) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<LinksSort>(LinksSort._.Ls_Id == identify);
                    tran.Delete<Links>(Links._.Ls_Id == identify);
                    tran.Commit();
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
        /// <suPsary>
        /// 删除，按栏目名称
        /// </suPsary>
        /// <param name="name">栏目名称</param>
        public void SortDelete(string name)
        {
            LinksSort entity = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Name == name).ToFirst<LinksSort>();
            if (entity == null) return;
            this.SortDelete(entity);            
        }
        /// <suPsary>
        /// 获取单一实体对象，按主键ID；
        /// </suPsary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public LinksSort SortSingle(int identify)
        {
            return Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Id == identify).ToFirst<LinksSort>();
        }
        /// <suPsary>
        /// 获取单一实体对象，按栏目名称
        /// </suPsary>
        /// <param name="name">栏目名称</param>
        /// <returns></returns>
        public LinksSort SortSingle(string name)
        {
            return Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Name == name).ToFirst<LinksSort>();
        }
        /// <suPsary>
        /// 获取同一父级下的最大排序号；
        /// </suPsary>
        /// <param name="orgid"></param>
        /// <param name="parentId">父Id</param>
        /// <returns></returns>
        public int SortMaxTaxis(int orgid, int parentId)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<LinksSort>(LinksSort._.Ls_Tax, LinksSort._.Ls_PatId == parentId && LinksSort._.Org_ID == orgid);
            int tax = 0;
            if (obj is int)
            {
                tax = (int)obj;
            }
            return tax;
        }
        /// <suPsary>
        /// 获取对象；即所有栏目；
        /// </suPsary>
        /// <returns></returns>
        public LinksSort[] SortAll(int orgid, bool? isUse, bool? isShow)
        {
            WhereClip wc = LinksSort._.Org_ID == orgid;
            if (isUse != null) wc.And(LinksSort._.Ls_IsUse == isUse);
            if (isShow != null) wc.And(LinksSort._.Ls_IsShow == isShow);
            return Gateway.Default.From<LinksSort>().Where(wc).OrderBy(LinksSort._.Ls_Tax.Asc).ToArray<LinksSort>();
        }
        public LinksSort[] SortCount(int orgid, bool? isUse, bool? isShow, int count)
        {
            WhereClip wc = LinksSort._.Org_ID == orgid;
            if (isUse != null) wc.And(LinksSort._.Ls_IsUse == isUse);
            if (isShow != null) wc.And(LinksSort._.Ls_IsShow == isShow);
            return Gateway.Default.From<LinksSort>().Where(wc).OrderBy(LinksSort._.Ls_Tax.Asc).ToArray<LinksSort>(count);
        }
        /// <summary>
        /// 当前链接分类下，有多少条链接记录
        /// </summary>
        /// <param name="lsid">链接分类id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isShow">是否显示</param>
        /// <returns></returns>
        public int SortOfCount(int lsid, bool? isUse, bool? isShow)
        {
            WhereClip wc = Links._.Ls_Id == lsid;
            if (isUse != null) wc.And(Links._.Lk_IsUse == (bool)isUse);
            if (isShow != null) wc.And(Links._.Lk_IsShow == (bool)isShow);
            return Gateway.Default.Count<Links>(wc);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LinksSort[] SortPager(int orgid, bool? isUse, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = LinksSort._.Org_ID == orgid;
            if (isUse != null) wc.And(LinksSort._.Ls_IsUse == isUse);
            if (isShow != null) wc.And(LinksSort._.Ls_IsShow == isShow);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(LinksSort._.Ls_Name.Like("%" + searTxt.Trim() + "%"));
            countSum = Gateway.Default.Count<LinksSort>(wc);
            return Gateway.Default.From<LinksSort>().Where(wc).OrderBy(LinksSort._.Ls_Tax.Asc).ToArray<LinksSort>(size, (index - 1) * size);
        }
        /// <suPsary>
        /// 当前对象名称是否重名
        /// </suPsary>
        /// <param name="orgid"></param>
        /// <param name="entity">业务实体</param>
        /// <returns>重名返回true，否则返回false</returns>
        public bool SortIsExist(int orgid, LinksSort entity)
        {
            LinksSort Ps = null;
            //如果是一个新对象
            if (entity.Ls_Id == 0)
            {
                Ps = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Name == entity.Ls_Name && LinksSort._.Org_ID == orgid).ToFirst<LinksSort>();
            }
            else
            {
                //如果是一个已经存在的对象，则不匹配自己
                Ps = Gateway.Default.From<LinksSort>().Where(LinksSort._.Org_ID == orgid && LinksSort._.Ls_Name == entity.Ls_Name && LinksSort._.Ls_Id != entity.Ls_Id).ToFirst<LinksSort>();
            }
            return Ps != null;
        }
        /// <summary>
        /// 更改链接分类的排序
        /// </summary>
        /// <param name="items">链接分类的实体数组</param>
        /// <returns></returns>
        public bool SortUpdateTaxis(Song.Entities.LinksSort[] items)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (LinksSort item in items)
                    {
                        tran.Update<LinksSort>(
                            new Field[] { LinksSort._.Ls_Tax },
                            new object[] { item.Ls_Tax },
                            LinksSort._.Ls_Id == item.Ls_Id);
                    }
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
