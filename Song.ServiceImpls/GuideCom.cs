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
    public class GuideCom :IGuide
    {
        private string _artUppath = "Guide";
        #region 考试指南
        /// <summary>
        /// 添加考试指南
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void GuideAdd(Guide entity)
        {
            //创建时间
            entity.Gu_CrtTime = DateTime.Now;
            if (entity.Gu_PushTime < DateTime.Now.AddYears(-100))
                entity.Gu_PushTime = entity.Gu_CrtTime;
            //所在机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //所属栏目
            Song.Entities.GuideColumns nc = this.ColumnsSingle((int)entity.Gc_ID);
            if (nc != null) entity.Gc_Title = nc.Gc_Title;
            Gateway.Default.Save<Guide>(entity);             
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void GuideSave(Guide entity)
        {
            //最后编辑时间
            entity.Gu_LastTime = DateTime.Now;
            if (entity.Gu_PushTime < DateTime.Now.AddYears(-100))
                entity.Gu_PushTime = entity.Gu_CrtTime;
            //所在机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //所属栏目
            Song.Entities.GuideColumns nc = this.ColumnsSingle((int)entity.Gc_ID);
            if (nc != null) entity.Gc_Title = nc.Gc_Title;
            Gateway.Default.Save<Guide>(entity);  
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void GuideDelete(Guide entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(entity.Gu_Uid))
                    {
                        //删除附件
                        Business.Do<IAccessory>().Delete(entity.Gu_Uid);
                        //删除图片文件
                        string img = WeiSha.Common.Upload.Get[_artUppath].Physics + entity.Gu_Logo;
                        if (System.IO.File.Exists(img))
                            System.IO.File.Delete(img);
                    }
                    tran.Delete<Guide>(Guide._.Gu_Id == entity.Gu_Id);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }       
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void GuideDelete(int identify)
        {
            Song.Entities.Guide guide = this.GuideSingle(identify);
            GuideDelete(guide);
        }
        /// <summary>
        /// 当前新闻的上一条新闻
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Guide GuidePrev(Guide entity)
        {
            WhereClip wc = new WhereClip();
            wc &= Guide._.Gu_IsShow == true;
            wc &= Guide._.Cou_ID == entity.Cou_ID;
            wc &= Guide._.Gu_CrtTime > entity.Gu_CrtTime;
            return Gateway.Default.From<Guide>().OrderBy(Guide._.Gu_CrtTime.Asc)
                .Where(wc).ToFirst<Guide>();
        }
        /// <summary>
        /// 当前新闻的下一条新闻
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Guide GuideNext(Guide entity)
        {
            WhereClip wc = new WhereClip();
            wc &= Guide._.Gu_IsShow == true;
            wc &= Guide._.Cou_ID == entity.Cou_ID;
            wc &= Guide._.Gu_CrtTime < entity.Gu_CrtTime;
            return Gateway.Default.From<Guide>().OrderBy(Guide._.Gu_CrtTime.Desc)
                .Where(wc).ToFirst<Guide>();
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Guide GuideSingle(int identify)
        {
            return Gateway.Default.From<Guide>().Where(Guide._.Gu_Id == identify).ToFirst<Guide>();
        }
        public Guide[] GuideCount(int orgid, int couid, int gcid, int count)
        {
            WhereClip wc = new WhereClip();
            wc &= Guide._.Gu_IsShow == true;
            if (orgid > 0) wc &= Guide._.Org_ID == orgid;
            if (couid > 0) wc &= Guide._.Cou_ID == couid;
            if (gcid > 0) wc &= Guide._.Gc_ID == gcid;
            return Gateway.Default.From<Guide>().Where(wc).ToArray<Guide>();
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid">课程id</param>
        /// <param name="gcid">考试指南分类</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Guide[] GetGuidePager(int orgid, int couid, int gcid, string searTxt, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Guide._.Org_ID == orgid);
            if (couid > 0) wc.And(Guide._.Cou_ID == couid);
            if (gcid >=0) wc.And(Guide._.Gc_ID == gcid);
            if (isShow != null) wc.And(Guide._.Gu_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Guide._.Gu_Title.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Guide>(wc);
            return Gateway.Default.From<Guide>().Where(wc).OrderBy(Guide._.Gu_PushTime.Desc).ToArray<Guide>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="gcids">考试指南分类,多个id，逗号分隔</param>
        /// <param name="searTxt"></param>
        /// <param name="isShow"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Guide[] GetGuidePager(int orgid, int couid, string gcids, string searTxt, bool? isShow, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Guide._.Org_ID == orgid);
            if (couid > 0) wc.And(Guide._.Cou_ID == couid);
            if (!string.IsNullOrWhiteSpace(gcids))
            {
                WhereClip wcSbjid = new WhereClip();
                foreach (string tm in gcids.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(tm)) continue;
                    int sbj = 0;
                    int.TryParse(tm, out sbj);
                    if (sbj == 0) continue;
                    wcSbjid.Or(Guide._.Gc_ID == sbj);
                }
                wc.And(wcSbjid);
            }  
            if (isShow != null) wc.And(Guide._.Gu_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Guide._.Gu_Title.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Guide>(wc);
            return Gateway.Default.From<Guide>().Where(wc).OrderBy(Guide._.Gu_PushTime.Desc).ToArray<Guide>(size, (index - 1) * size);
        }
        #endregion

        #region 考试指南分类
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int ColumnsAdd(GuideColumns entity)
        {
            entity.Gc_CrtTime = DateTime.Now;
            //如果没有排序号，则自动计算
            if (entity.Gc_Tax < 1)
            {
                object obj = Gateway.Default.Max<GuideColumns>(GuideColumns._.Gc_Tax, GuideColumns._.Cou_ID == entity.Cou_ID && GuideColumns._.Gc_PID == entity.Gc_PID);
                entity.Gc_Tax = obj is int ? (int)obj + 1 : 0;
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<GuideColumns>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ColumnsSave(GuideColumns entity)
        {
            Song.Entities.GuideColumns old = this.ColumnsSingle(entity.Gc_ID);
            if (old.Gc_PID != entity.Gc_PID)
            {
                object obj = Gateway.Default.Max<GuideColumns>(GuideColumns._.Gc_Tax, GuideColumns._.Cou_ID == entity.Cou_ID && GuideColumns._.Gc_PID == entity.Gc_PID);
                entity.Gc_Tax = obj is int ? (int)obj + 1 : 0;
            }
            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Save<GuideColumns>(entity);
                    trans.Update<GuideColumns>(new Field[] { Guide._.Gc_Title }, new object[] { entity.Gc_Title, }, GuideColumns._.Gc_ID == entity.Gc_ID);                   
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                finally
                {
                    trans.Close();
                }
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ColumnsDelete(GuideColumns entity)
        {
            Song.Entities.GuideColumns[] cols = GetColumnsChild(entity.Cou_ID, entity.Gc_ID, null);
            foreach (Song.Entities.GuideColumns cl in cols)
                ColumnsDelete(cl);
            Gateway.Default.Delete<Guide>(Guide._.Gc_ID == entity.Gc_ID);
            Gateway.Default.Delete<GuideColumns>(GuideColumns._.Gc_ID == entity.Gc_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ColumnsDelete(int identify)
        {
            Song.Entities.GuideColumns col = ColumnsSingle(identify);
            ColumnsDelete(col);
        }        
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public GuideColumns ColumnsSingle(int identify)
        {
            return Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Gc_ID == identify).ToFirst<GuideColumns>();
        }
        /// <summary>
        /// 获取同一父级下的最大排序号；
        /// </summary>
        ///<param name="couid">课程id</param>
        ///<param name="pid">学科id</param>
        /// <returns></returns>
        public int ColumnsMaxTaxis(int couid, int pid)
        {
            object obj = Gateway.Default.Max<GuideColumns>(GuideColumns._.Gc_Tax, GuideColumns._.Cou_ID == couid && GuideColumns._.Gc_PID == pid);
            int tax = obj is int ? (int)obj + 1 : 0;
            return tax;
        }
        /// <summary>
        /// 获取对象；即所有分类；
        /// </summary>
        /// <returns></returns>
        public GuideColumns[] GetColumnsAll(int couid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(GuideColumns._.Cou_ID == couid);
            if (isUse != null) wc.And(GuideColumns._.Gc_IsUse == (bool)isUse);
            return Gateway.Default.From<GuideColumns>().Where(wc).OrderBy(GuideColumns._.Gc_Tax.Asc).ToArray<GuideColumns>();
        }
        /// <summary>
        /// 获取当前分类下的子分类
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public GuideColumns[] GetColumnsChild(int couid, int pid, bool? isUse)
        {
            WhereClip wc = GuideColumns._.Cou_ID == couid;
            if (pid >= 0) wc.And(GuideColumns._.Gc_PID == pid);
            if (isUse != null) wc.And(GuideColumns._.Gc_IsUse == (bool)isUse);
            return Gateway.Default.From<GuideColumns>().Where(wc).OrderBy(GuideColumns._.Gc_Tax.Asc).ToArray<GuideColumns>();
        }

        public bool ColumnsIsChildren(int couid, int pid, bool? isUse)
        {
            WhereClip wc = GuideColumns._.Cou_ID == couid;
            if (pid >= 0) wc.And(GuideColumns._.Gc_PID == pid);
            if (isUse != null) wc.And(GuideColumns._.Gc_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<GuideColumns>(wc);
            return count > 0;
        }
        /// <summary>
        /// 当前对象名称是否重名
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <returns></returns>
        public bool ColumnsIsExist(int couid, int pid, GuideColumns entity)
        {
            WhereClip wc = GuideColumns._.Gc_Title == entity.Gc_Title && GuideColumns._.Cou_ID == couid && GuideColumns._.Gc_PID == pid;
            //如果是一个已经存在的对象
            if (entity.Gc_ID == 0) wc = wc.And(GuideColumns._.Gc_ID == entity.Gc_ID);
            GuideColumns mm = Gateway.Default.From<GuideColumns>().Where(wc).ToFirst<GuideColumns>();           
            return mm != null;
        }
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ColumnsRemoveUp(int id)
        {
            //当前对象
            GuideColumns current = Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Gc_ID == id).ToFirst<GuideColumns>();
            int tax = (int)current.Gc_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            GuideColumns prev = Gateway.Default.From<GuideColumns>()
                .Where(GuideColumns._.Gc_Tax < tax && GuideColumns._.Gc_PID == current.Gc_PID && GuideColumns._.Cou_ID == current.Cou_ID)
                .OrderBy(GuideColumns._.Gc_Tax.Desc).ToFirst<GuideColumns>();
            if (prev == null) return false;
            //交换排序号
            current.Gc_Tax = prev.Gc_Tax;
            prev.Gc_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<GuideColumns>(current);
                    tran.Save<GuideColumns>(prev);
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
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ColumnsRemoveDown(int id)
        {
            //当前对象
            GuideColumns current = Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Gc_ID == id).ToFirst<GuideColumns>();
            int tax = (int)current.Gc_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            GuideColumns next = Gateway.Default.From<GuideColumns>()
                .Where(GuideColumns._.Gc_Tax > tax && GuideColumns._.Gc_PID == current.Gc_PID && GuideColumns._.Cou_ID == current.Cou_ID)
                .OrderBy(GuideColumns._.Gc_Tax.Asc).ToFirst<GuideColumns>();
            if (next == null) return false;
            //交换排序号
            current.Gc_Tax = next.Gc_Tax;
            next.Gc_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<GuideColumns>(current);
                    tran.Save<GuideColumns>(next);
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
