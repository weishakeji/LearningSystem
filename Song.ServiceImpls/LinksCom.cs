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
            WhereClip wc = Links._.Org_ID == orgid;
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
            WhereClip wc = Links._.Org_ID == orgid;
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
            WhereClip wc = Links._.Org_ID == orgid;
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }
        public Links[] GetLinksPager(int orgid, int sortId, int size, int index, out int countSum)
        {
            WhereClip wc = Links._.Org_ID == orgid;
            if (sortId > 0) wc.And(Links._.Ls_Id == sortId);
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有链接项
        /// </summary>
        /// <param name="sortId">分类id</param>
        /// <param name="isUse">是否使用</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Links[] GetLinksPager(int orgid, int sortId, bool? isUse, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Links._.Org_ID == orgid;
            if (sortId > -1) wc.And(Links._.Ls_Id == sortId);
            if (isUse != null) wc.And(Links._.Lk_IsUse == (bool)isUse);
            if (isShow != null) wc.And(Links._.Lk_IsShow == (bool)isShow);
            if (searTxt != null || searTxt.Trim() != "") wc.And(Links._.Lk_Name.Like("%" + searTxt.Trim() + "%"));
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }
        public Links[] GetLinksPager(int orgid, int sortId, bool? isUse, bool? isShow, bool? isVeri, bool? isApply, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Links._.Org_ID == orgid;
            if (sortId > -1) wc.And(Links._.Ls_Id == sortId);
            if (isUse != null) wc.And(Links._.Lk_IsUse == (bool)isUse);
            if (isShow != null) wc.And(Links._.Lk_IsShow == (bool)isShow);
            if (isVeri != null) wc.And(Links._.Lk_IsVerify == (bool)isVeri);
            if (isApply != null) wc.And(Links._.Lk_IsApply == (bool)isApply);
            if (searTxt != null || searTxt.Trim() != "") wc.And(Links._.Lk_Name.Like("%" + searTxt.Trim() + "%"));
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }
        public Links[] GetLinksPager(int orgid, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = Links._.Org_ID == orgid;
            if (isShow != null) wc.And(Links._.Lk_IsShow == (bool)isShow);
            countSum = Gateway.Default.Count<Links>(wc);
            return Gateway.Default.From<Links>().Where(wc).OrderBy(Links._.Lk_Tax.Asc).ToArray<Links>(size, (index - 1) * size);
        }

        public bool LinksRemoveUp(int id)
        {
            //当前对象
            Links current = Gateway.Default.From<Links>().Where(Links._.Lk_Id == id).ToFirst<Links>();
            //当前对象父节点id;
            int lsId = (int)current.Ls_Id;
            //当前对象排序号
            int orderValue = (int)current.Lk_Tax;
            //上一个对象，即兄长对象；
            Links up = Gateway.Default.From<Links>().Where(Links._.Org_ID == current.Org_ID && Links._.Ls_Id == lsId && Links._.Lk_Tax < orderValue).OrderBy(Links._.Lk_Tax.Desc).ToFirst<Links>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.Lk_Tax = up.Lk_Tax;
            up.Lk_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Links>(current);
                    tran.Save<Links>(up);
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
            return true;
        }

        public bool LinksRemoveDown(int id)
        {
            //当前对象
            Links current = Gateway.Default.From<Links>().Where(Links._.Lk_Id == id).ToFirst<Links>();
            //当前对象父节点id;
            int lsId = (int)current.Ls_Id;
            //当前对象排序号
            int orderValue = (int)current.Lk_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            Links next = Gateway.Default.From<Links>()
                .Where(Links._.Lk_Tax > orderValue && Links._.Org_ID == current.Org_ID && Links._.Ls_Id == lsId)
                .OrderBy(Links._.Lk_Tax.Asc).ToFirst<Links>();
            if (next == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.Lk_Tax = next.Lk_Tax;
            next.Lk_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Links>(current);
                    tran.Save<Links>(next);
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
            return true;
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
                WeiSha.WebControl.FileUpload.Delete("Links", entity.Lk_Logo);
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
            if (entity.Ls_PatId < 0)
            {
                //如果父节点小于0，违反逻辑
                return -1;
            }
            if (!(entity.Ls_Name != null && entity.Ls_Name.Trim() != ""))
            {
                //如果名称为空
                return -1;
            }
            if (entity.Ls_PatId > 0)
            {
                LinksSort parent = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_PatId == entity.Ls_Id).ToFirst<LinksSort>();
                if (parent == null)
                {
                    //如果父节点不存在
                    return -1;
                }
                //新增对象的属性，遵循上级；
                entity.Ls_IsUse = parent.Ls_IsUse;
            }
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<LinksSort>(LinksSort._.Ls_Tax, LinksSort._.Ls_PatId == entity.Ls_PatId);
            int tax = 0;
            if (obj is int)
            {
                tax = (int)obj;
            }
            entity.Ls_Tax = tax + 1;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            int id = Gateway.Default.Save<LinksSort>(entity);
            return id;
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
        /// <suPsary>
        /// 删除
        /// </suPsary>
        /// <param name="entity">业务实体</param>
        public void SortDelete(LinksSort entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<LinksSort>(LinksSort._.Ls_Id == entity.Ls_Id);
                    tran.Delete<Links>(Links._.Ls_Id == entity.Ls_Id);
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
        /// 删除，按主键ID；
        /// </suPsary>
        /// <param name="identify">实体的主键</param>
        public void SortDelete(int identify)
        {
            Gateway.Default.Delete<LinksSort>(LinksSort._.Ls_Id == identify);
        }
        /// <suPsary>
        /// 删除，按栏目名称
        /// </suPsary>
        /// <param name="name">栏目名称</param>
        public void SortDelete(string name)
        {
            LinksSort entity = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Name == name).ToFirst<LinksSort>();
            if (entity == null) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<LinksSort>(LinksSort._.Ls_Id == entity.Ls_Id);
                    tran.Delete<Links>(Links._.Ls_Id == entity.Ls_Id);
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
        public LinksSort[] GetSortAll(int orgid, bool? isUse, bool? isShow)
        {
            WhereClip wc = LinksSort._.Org_ID == orgid;
            if (isUse != null) wc.And(LinksSort._.Ls_IsUse == isUse);
            if (isShow != null) wc.And(LinksSort._.Ls_IsShow == isShow);
            return Gateway.Default.From<LinksSort>().Where(wc).OrderBy(LinksSort._.Ls_Tax.Asc).ToArray<LinksSort>();
        }
        public LinksSort[] GetSortCount(int orgid, bool? isUse, bool? isShow, int count)
        {
            WhereClip wc = LinksSort._.Org_ID == orgid;
            if (isUse != null) wc.And(LinksSort._.Ls_IsUse == isUse);
            if (isShow != null) wc.And(LinksSort._.Ls_IsShow == isShow);
            return Gateway.Default.From<LinksSort>().Where(wc).OrderBy(LinksSort._.Ls_Tax.Asc).ToArray<LinksSort>(count);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="isShow"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LinksSort[] GetSortPager(int orgid, bool? isUse, bool? isShow, string searTxt, int size, int index, out int countSum)
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
        /// <suPsary>
        /// 将当前栏目向上移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </suPsary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool SortRemoveUp(int id)
        {
            //当前对象
            LinksSort current = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Id == id).ToFirst<LinksSort>();
            //当前对象排序号
            int orderValue = (int)current.Ls_Tax;
            //上一个对象，即兄长对象；
            LinksSort up = Gateway.Default.From<LinksSort>().Where(LinksSort._.Org_ID == current.Org_ID && LinksSort._.Ls_Tax < orderValue).OrderBy(LinksSort._.Ls_Tax.Desc).ToFirst<LinksSort>();
            if (up == null)
            {
                //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
                return false;
            }
            //交换排序号
            current.Ls_Tax = up.Ls_Tax;
            up.Ls_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<LinksSort>(current);
                    tran.Save<LinksSort>(up);
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
            return true;
        }
        /// <suPsary>
        /// 将当前栏目向下移动；仅在当前对象的同层移动，即同一父节点下的对象之间移动；
        /// </suPsary>
        /// <param name="id"></param>
        /// <returns>如果已经处于最底端，则返回false；移动成功，返回true</returns>
        public bool SortRemoveDown(int id)
        {
            //当前对象
            LinksSort current = Gateway.Default.From<LinksSort>().Where(LinksSort._.Ls_Id == id).ToFirst<LinksSort>();
            //当前对象排序号
            int orderValue = (int)current.Ls_Tax;
            //下一个对象，即弟弟对象；
            LinksSort down = Gateway.Default.From<LinksSort>().Where(LinksSort._.Org_ID == current.Org_ID && LinksSort._.Ls_Tax > orderValue).OrderBy(LinksSort._.Ls_Tax.Asc).ToFirst<LinksSort>();
            if (down == null)
            {
                //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
                return false;
            }
            //交换排序号
            current.Ls_Tax = down.Ls_Tax;
            down.Ls_Tax = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<LinksSort>(current);
                    tran.Save<LinksSort>(down);
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
            return true;
        }
        #endregion
    }
}
