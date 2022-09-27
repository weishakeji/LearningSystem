using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Xml;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class KnowledgeCom : IKnowledge
    {


        #region IKnowledge 成员

        public int KnowledgeAdd(Knowledge entity)
        {
            entity.Kn_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            if (!string.IsNullOrWhiteSpace(entity.Kns_UID))
            {
                KnowledgeSort sort= Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_UID == entity.Kns_UID).ToFirst<KnowledgeSort>();
                if (sort != null)
                {
                    entity.Kns_Name = sort.Kns_Name;
                }
            }
            if (string.IsNullOrWhiteSpace(entity.Kn_Uid))            
                entity.Kn_Uid = WeiSha.Core.Request.UniqueID();
           
            return Gateway.Default.Save<Knowledge>(entity);
        }

        public void KnowledgeSave(Knowledge entity)
        {
            if (!string.IsNullOrWhiteSpace(entity.Kns_UID))
            {
                KnowledgeSort sort = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_UID == entity.Kns_UID).ToFirst<KnowledgeSort>();
                if (sort != null)
                {
                    entity.Kns_Name = sort.Kns_Name;
                }
            }
            if (string.IsNullOrWhiteSpace(entity.Kn_Uid))
                entity.Kn_Uid = WeiSha.Core.Request.UniqueID();
            Gateway.Default.Save<Knowledge>(entity);
        }

        public void KnowledgeDelete(int identify)
        {
            Gateway.Default.Delete<Knowledge>(Knowledge._.Kn_ID == identify);
        }

        public Knowledge KnowledgeSingle(int identify)
        {
            return Gateway.Default.From<Knowledge>().Where(Knowledge._.Kn_ID == identify).ToFirst<Knowledge>();
        }
        public Knowledge KnowledgeSingle(string uid)
        {
            return Gateway.Default.From<Knowledge>().Where(Knowledge._.Kn_Uid == uid).ToFirst<Knowledge>();
        }
        public Knowledge KnowledgePrev(long couid, string kns, int id)
        {
            WhereClip wc = new WhereClip();
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            return Gateway.Default.From<Knowledge>().OrderBy(Knowledge._.Kn_ID.Desc)
               .Where(wc && Knowledge._.Kn_ID > id).ToFirst<Knowledge>();
        }

        public Knowledge KnowledgeNext(long couid, string kns, int id)
        {
            WhereClip wc = new WhereClip();
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            return Gateway.Default.From<Knowledge>().OrderBy(Knowledge._.Kn_ID.Asc)
              .Where(wc && Knowledge._.Kn_ID < id).ToFirst<Knowledge>();
        }
        public Knowledge[] KnowledgeCount(int orgid, bool? isUse, string kns, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(count);
        }
        public Knowledge[] KnowledgeCount(int orgid, long couid, string kns, string searTxt, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Like("%" + searTxt + "%"));
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(count);
        }
        public int KnowledgeOfCount(int orgid, string kns, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            return Gateway.Default.Count<Knowledge>(wc);
        }
        public int KnowledgeOfCount(int orgid, long couid, string kns, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            return Gateway.Default.Count<Knowledge>(wc);
        }
        public Knowledge[] KnowledgePager(int orgid, bool? isUse, string kns, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == isUse);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        public Knowledge[] KnowledgePager(int orgid, long couid, string kns, bool? isUse, bool? isHot, bool? isRec, bool? isTop, string searTxt, int size, int index, out int countSum)
        {

            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            if (!string.IsNullOrWhiteSpace(kns)) wc.And(Knowledge._.Kns_UID == kns);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == isUse);
            if (isHot != null) wc.And(Knowledge._.Kn_IsHot == isHot);
            if (isRec != null) wc.And(Knowledge._.Kn_IsRec == isRec);
            if (isTop != null) wc.And(Knowledge._.Kn_IsTop == isTop);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        public Knowledge[] KnowledgePager(long couid, string kns, string searTxt, bool? isUse, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            wc.And(Knowledge._.Kn_IsUse == true);
            if (!string.IsNullOrWhiteSpace(kns))
            {
                WhereClip wcSbjid = new WhereClip();
                foreach (string tm in kns.Split(','))
                {
                    wcSbjid.Or(Knowledge._.Kns_UID == tm);
                }
                wc.And(wcSbjid);
            }  
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        #endregion

        #region 分类管理
        public int SortAdd(KnowledgeSort entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Kns_UID))
                entity.Kns_UID = WeiSha.Core.Request.UniqueID();

            entity.Kns_CrtTime = DateTime.Now;
            //如果没有排序号，则自动计算
            if (entity.Kns_Tax < 1 )
            {
                object obj = Gateway.Default.Max<KnowledgeSort>(KnowledgeSort._.Kns_Tax, KnowledgeSort._.Cou_ID == entity.Cou_ID && KnowledgeSort._.Kns_PID == entity.Kns_PID);
                entity.Kns_Tax = obj is int ? (int)obj + 1 : 0;
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<KnowledgeSort>(entity);
        }

        public void SortSave(KnowledgeSort entity)
        {

            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<KnowledgeSort>(entity);
                    tran.Update<Knowledge>(new Field[] { Knowledge._.Kns_Name }, new object[] { entity.Kns_Name }, Knowledge._.Kns_UID == entity.Kns_UID);

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

        public void SortSaveOrder(string xml)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    XmlDocument resXml = new XmlDocument();
                    resXml.XmlResolver = null; 
                    resXml.LoadXml(xml, false);
                    XmlNodeList nodeList = resXml.SelectSingleNode("nodes").ChildNodes;
                    //取rootid
                    XmlNode nodes = resXml.SelectSingleNode("nodes");
                    XmlElement xenodes = (XmlElement)nodes;
                    //遍历所有子节点 
                    foreach (XmlNode xn in nodeList)
                    {
                        XmlElement xe = (XmlElement)xn;
                        int id = Convert.ToInt32(xe.Attributes["id"].Value);
                        string pid = xe.Attributes["pid"].Value;
                        int tax = Convert.ToInt32(xe.Attributes["tax"].Value);
                        Song.Entities.KnowledgeSort nc = this.SortSingle(id);
                        if (nc != null)
                        {
                            nc.Kns_PID = pid;
                            nc.Kns_Tax = tax;
                            tran.Save<KnowledgeSort>(nc);
                        }
                    }
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

        public void SortDelete(int identify)
        {
            if (identify <= 0) return;
            KnowledgeSort sort = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == identify).ToFirst<KnowledgeSort>();
            if (sort == null) return;
            KnowledgeSort[] child = this.GetSortChilds(sort.Kns_UID, -1, null);
            foreach (KnowledgeSort n in child)
            {
                SortDelete(n.Kns_ID);
            }          
            Gateway.Default.Delete<Knowledge>(Knowledge._.Kns_UID == sort.Kns_UID);
            Gateway.Default.Delete<KnowledgeSort>(KnowledgeSort._.Kns_ID == identify);
        }

        public KnowledgeSort SortSingle(int identify)
        {
            return Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == identify).ToFirst<KnowledgeSort>();
        }
        /// <summary>
        /// 获取单一实体对象，
        /// </summary>
        /// <param name="uid">全局唯一值</param>
        /// <returns></returns>
        public KnowledgeSort SortSingle(string uid)
        {
            return Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_UID == uid).ToFirst<KnowledgeSort>();
        }
        public KnowledgeSort[] GetSortAll(int orgid, long couid, string search, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(KnowledgeSort._.Org_ID == orgid);
            if (couid >= 0) wc.And(KnowledgeSort._.Cou_ID == couid);
            if (isUse != null) wc.And(KnowledgeSort._.Kns_IsUse == (bool)isUse);
            if(!string.IsNullOrWhiteSpace(search)) wc.And(KnowledgeSort._.Kns_Name.Like("%" + search + "%"));
            return Gateway.Default.From<KnowledgeSort>().Where(wc).OrderBy(KnowledgeSort._.Kns_Tax.Asc).ToArray<KnowledgeSort>();
        }

        public KnowledgeSort[] GetSortAll(int orgid, long couid, int pid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(KnowledgeSort._.Org_ID == orgid);
            if (couid > 0) wc.And(KnowledgeSort._.Cou_ID == couid);
            if (pid >= 0) wc.And(KnowledgeSort._.Kns_PID == pid);
            if (isUse != null) wc.And(KnowledgeSort._.Kns_IsUse == (bool)isUse);
            return Gateway.Default.From<KnowledgeSort>().Where(wc).OrderBy(KnowledgeSort._.Kns_Tax.Asc).ToArray<KnowledgeSort>();
        }

        public KnowledgeSort[] GetSortChilds(string pid, long couid, bool? isUse)
        {
            WhereClip wc = KnowledgeSort._.Kns_PID == pid;
            if (couid >= 0) wc.And(KnowledgeSort._.Cou_ID == couid);
            if (isUse != null) wc.And(KnowledgeSort._.Kns_IsUse == (bool)isUse);
            return Gateway.Default.From<KnowledgeSort>().Where(wc).OrderBy(KnowledgeSort._.Kns_Tax.Asc).ToArray<KnowledgeSort>();
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="list">对象列表，Kns_ID、Kns_PID、Kns_Tax</param>
        /// <returns></returns>
        public bool SortUpdateTaxis(KnowledgeSort[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.KnowledgeSort item in list)
                    {
                        tran.Update<KnowledgeSort>(
                            new Field[] { KnowledgeSort._.Kns_PID, KnowledgeSort._.Kns_Tax },
                            new object[] { item.Kns_PID, item.Kns_Tax },
                            KnowledgeSort._.Kns_ID == item.Kns_ID);
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
