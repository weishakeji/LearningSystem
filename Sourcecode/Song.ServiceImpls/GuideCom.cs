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
            if (entity.Gu_ID <= 0)
                entity.Gu_ID = WeiSha.Core.Request.SnowID();

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
            Song.Entities.GuideColumns nc = this.ColumnsSingle(entity.Gc_UID);
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
            Song.Entities.GuideColumns nc = this.ColumnsSingle(entity.Gc_UID);
            if (nc != null) entity.Gc_Title = nc.Gc_Title;
            Gateway.Default.Save<Guide>(entity);  
        }
        /// <summary>
        /// 修改，按条件修改
        /// </summary>
        /// <param name="guid">公告id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        public void GuideUpdate(long guid, Field[] fiels, object[] objs)
        {
            Gateway.Default.Update<Guide>(fiels, objs, Guide._.Gu_ID == guid);
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
                        Business.Do<IAccessory>().Delete(entity.Gu_Uid, string.Empty);
                        //删除图片文件
                        string img = WeiSha.Core.Upload.Get[_artUppath].Physics + entity.Gu_Logo;
                        if (System.IO.File.Exists(img))
                            System.IO.File.Delete(img);
                    }
                    tran.Delete<Guide>(Guide._.Gu_ID == entity.Gu_ID);
                    WeiSha.Core.Upload.Get["Guide"].DeleteDirectory(entity.Gu_ID.ToString());
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }       
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void GuideDelete(long identify)
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
            wc &= Guide._.Gu_IsUse == true;
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
            wc &= Guide._.Gu_IsUse == true;
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
        public Guide GuideSingle(long identify)
        {
            return Gateway.Default.From<Guide>().Where(Guide._.Gu_ID == identify).ToFirst<Guide>();
        }
        public Guide[] GuideCount(int orgid, long couid, string gcuid, bool? isShow, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();          
            if (orgid > 0) wc &= Guide._.Org_ID == orgid;
            if (couid > 0) wc &= Guide._.Cou_ID == couid;
            if (isShow != null) wc &= Guide._.Gu_IsShow == (bool)isShow;
            if (isUse != null) wc &= Guide._.Gu_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(gcuid))
            {
                WhereClip wcUid = new WhereClip();
                List<string> list = this.ColumnsTreeID(gcuid);
                foreach (string l in list)
                    wcUid.Or(Guide._.Gc_UID == l);
                wc.And(wcUid);
            }
            return Gateway.Default.From<Guide>().Where(wc).ToArray<Guide>();
        }
        /// <summary>
        /// 课程公告的数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="gcuid"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public int GuideOfCount(int orgid, long couid, string gcuid, bool? isShow, bool? isUse)
        {
            WhereClip wc = new WhereClip();          
            if (orgid > 0) wc &= Guide._.Org_ID == orgid;
            if (couid > 0) wc &= Guide._.Cou_ID == couid;
            if (isShow != null) wc &= Guide._.Gu_IsShow == (bool)isShow;
            if (isUse != null) wc &= Guide._.Gu_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(gcuid))
            {
                WhereClip wcUid = new WhereClip();
                List<string> list = this.ColumnsTreeID(gcuid);
                foreach (string l in list)
                    wcUid.Or(Guide._.Gc_UID == l);
                wc.And(wcUid);
            }
            return Gateway.Default.Count<Guide>(wc);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid">课程id</param>
        /// <param name="gcuid">考试指南分类</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Guide[] GuidePager(int orgid, long couid, string gcuid, string searTxt, bool? isShow, bool? isUse, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Guide._.Org_ID == orgid);
            if (couid > 0) wc.And(Guide._.Cou_ID == couid);
            if (isShow != null) wc &= Guide._.Gu_IsShow == (bool)isShow;
            if (isUse != null) wc &= Guide._.Gu_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(gcuid))
            {
                WhereClip wcUid = new WhereClip();
                List<string> list = this.ColumnsTreeID(gcuid);
                foreach (string l in list)
                    wcUid.Or(Guide._.Gc_UID == l);
                wc.And(wcUid);
            }      
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Guide._.Gu_Title.Contains(searTxt));
            countSum = Gateway.Default.Count<Guide>(wc);
            return Gateway.Default.From<Guide>().Where(wc).OrderBy(Guide._.Gu_PushTime.Desc).ToArray<Guide>(size, (index - 1) * size);
        }
        /// <summary>
        /// 当前分类下的所有子分类的uid
        /// </summary>
        /// <param name="uid">当前专业id</param>
        /// <returns></returns>
        public List<string> ColumnsTreeID(string uid)
        {
            List<string> list = new List<string>();
            GuideColumns item = Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Gc_UID == uid).ToFirst<GuideColumns>();
            if (item == null) return list;
            //取所有
            GuideColumns[] entities = Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Cou_ID == item.Cou_ID).ToArray<GuideColumns>();
            list = _treeid(uid, entities);
            return list;
        }
        private List<string> _treeid(string uid, GuideColumns[] items)
        {
            List<string> list = new List<string>();
            if (!string.IsNullOrWhiteSpace(uid)) list.Add(uid);
            foreach (GuideColumns o in items)
            {
                if (o.Gc_PID != uid) continue;
                List<string> tm = _treeid(o.Gc_UID, items);
                foreach (string t in tm)
                    list.Add(t);
            }
            return list;
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
            if (string.IsNullOrWhiteSpace(entity.Gc_PID)) entity.Gc_PID = "0";
            if (string.IsNullOrWhiteSpace(entity.Gc_UID)) entity.Gc_UID = WeiSha.Core.Request.UniqueID();
            //如果没有排序号，则自动计算
            if (entity.Gc_Tax < 1)
            {
                object obj = Gateway.Default.Max<GuideColumns>(GuideColumns._.Gc_Tax, GuideColumns._.Cou_ID == entity.Cou_ID && GuideColumns._.Gc_PID == entity.Gc_PID);
                entity.Gc_Tax = obj is int ? (int)obj + 1 : 0;
            }            
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
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
            Song.Entities.GuideColumns[] cols = GetColumnsChild(entity.Cou_ID, entity.Gc_UID, null);
            foreach (Song.Entities.GuideColumns cl in cols)
                ColumnsDelete(cl);
            Gateway.Default.Delete<Guide>(Guide._.Gc_UID == entity.Gc_UID);
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
        public GuideColumns ColumnsSingle(string uid)
        {
            return Gateway.Default.From<GuideColumns>().Where(GuideColumns._.Gc_UID == uid).ToFirst<GuideColumns>();
        }
        /// <summary>
        /// 获取对象；即所有分类；
        /// </summary>
        /// <returns></returns>
        public GuideColumns[] GetColumnsAll(long couid, string search, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(GuideColumns._.Cou_ID == couid);
            if (isUse != null) wc.And(GuideColumns._.Gc_IsUse == (bool)isUse);
            if(!string.IsNullOrWhiteSpace(search)) wc.And(GuideColumns._.Gc_Title.Contains(search));
            return Gateway.Default.From<GuideColumns>().Where(wc).OrderBy(GuideColumns._.Gc_Tax.Asc).ToArray<GuideColumns>();
        }
        /// <summary>
        /// 获取当前分类下的子分类
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public GuideColumns[] GetColumnsChild(long couid, string pid, bool? isUse)
        {
            WhereClip wc = GuideColumns._.Cou_ID == couid;
            if (!string.IsNullOrWhiteSpace(pid)) wc.And(GuideColumns._.Gc_PID == pid);
            if (isUse != null) wc.And(GuideColumns._.Gc_IsUse == (bool)isUse);
            return Gateway.Default.From<GuideColumns>().Where(wc).OrderBy(GuideColumns._.Gc_Tax.Asc).ToArray<GuideColumns>();
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="list">对象列表，Gc_ID、Gc_PID、Gc_Tax</param>
        /// <returns></returns>
        public bool ColumnsUpdateTaxis(GuideColumns[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.GuideColumns item in list)
                    {
                        tran.Update<GuideColumns>(
                            new Field[] { GuideColumns._.Gc_PID, GuideColumns._.Gc_Tax },
                            new object[] { item.Gc_PID, item.Gc_Tax },
                            GuideColumns._.Gc_ID == item.Gc_ID);
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
