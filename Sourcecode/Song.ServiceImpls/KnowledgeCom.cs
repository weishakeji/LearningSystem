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
            if (entity.Kn_ID <= 0)
                entity.Kn_ID = WeiSha.Core.Request.SnowID();
            entity.Kn_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            if (entity.Kns_ID>0)
            {
                KnowledgeSort sort= Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == entity.Kns_ID).ToFirst<KnowledgeSort>();
                if (sort != null)
                {
                    entity.Kns_Name = sort.Kns_Name;
                }
            }
            return Gateway.Default.Save<Knowledge>(entity);
        }

        public void KnowledgeSave(Knowledge entity)
        {
            if (entity.Kns_ID > 0)
            {
                KnowledgeSort sort = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == entity.Kns_ID).ToFirst<KnowledgeSort>();
                if (sort != null)
                {
                    entity.Kns_Name = sort.Kns_Name;
                }
            }
            Gateway.Default.Save<Knowledge>(entity);
        }

        public void KnowledgeDelete(long identify)
        {
            Gateway.Default.Delete<Knowledge>(Knowledge._.Kn_ID == identify);
            WeiSha.Core.Upload.Get["Knowledge"].DeleteDirectory(identify.ToString());
        }

        public Knowledge KnowledgeSingle(long identify)
        {
            return Gateway.Default.From<Knowledge>().Where(Knowledge._.Kn_ID == identify).ToFirst<Knowledge>();
        }
        public Knowledge KnowledgeSingle(string uid)
        {
            return Gateway.Default.From<Knowledge>().Where(Knowledge._.Kn_Uid == uid).ToFirst<Knowledge>();
        }
        public Knowledge KnowledgePrev(long couid, long kns, int id)
        {
            WhereClip wc = new WhereClip();
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.From<Knowledge>().OrderBy(Knowledge._.Kn_ID.Desc)
               .Where(wc && Knowledge._.Kn_ID > id).ToFirst<Knowledge>();
        }

        public Knowledge KnowledgeNext(long couid, long kns, int id)
        {
            WhereClip wc = new WhereClip();
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.From<Knowledge>().OrderBy(Knowledge._.Kn_ID.Asc)
              .Where(wc && Knowledge._.Kn_ID < id).ToFirst<Knowledge>();
        }
        public Knowledge[] KnowledgeCount(int orgid, bool? isUse, long kns, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(count);
        }
        public Knowledge[] KnowledgeCount(int orgid, long couid, long kns, string searTxt, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Contains(searTxt));
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(count);
        }
        public int KnowledgeOfCount(int orgid, long kns, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.Count<Knowledge>(wc);
        }
        public int KnowledgeOfCount(int orgid, long couid, long kns, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.Count<Knowledge>(wc);
        }
        public Knowledge[] KnowledgePager(int orgid, bool? isUse, long kns, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == isUse);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Contains(searTxt));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        public Knowledge[] KnowledgePager(int orgid, long couid, long kns, bool? isUse, bool? isHot, bool? isRec, bool? isTop, string searTxt, int size, int index, out int countSum)
        {

            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == isUse);
            if (isHot != null) wc.And(Knowledge._.Kn_IsHot == isHot);
            if (isRec != null) wc.And(Knowledge._.Kn_IsRec == isRec);
            if (isTop != null) wc.And(Knowledge._.Kn_IsTop == isTop);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Contains(searTxt));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        public Knowledge[] KnowledgePager(long couid, long kns, string searTxt, bool? isUse, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = this.TreeID(kns, 0);
                foreach (long l in list)
                    wcSbjid.Or(Knowledge._.Kns_ID == l);
                wc.And(wcSbjid);
            } 
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Contains(searTxt));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        #endregion

        #region 分类管理
        public int SortAdd(KnowledgeSort entity)
        {

            if (entity.Kns_ID <= 0)
                entity.Kns_ID = WeiSha.Core.Request.SnowID();            

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
                    tran.Update<Knowledge>(new Field[] { Knowledge._.Kns_Name }, new object[] { entity.Kns_Name }, Knowledge._.Kns_ID == entity.Kns_ID);

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
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
                        long id = Convert.ToInt64(xe.Attributes["id"].Value);
                        long pid = Convert.ToInt64(xe.Attributes["pid"].Value);
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
            }
        }

        public void SortDelete(long identify)
        {
            if (identify <= 0) return;
            KnowledgeSort sort = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == identify).ToFirst<KnowledgeSort>();
            if (sort == null) return;
            this.SortDelete(sort);
        }
        public void SortDelete(KnowledgeSort sort)
        {
            KnowledgeSort[] child = this.GetSortChilds(sort.Kns_ID, -1, null);
            foreach (KnowledgeSort n in child)
            {
                SortDelete(n.Kns_ID);
            }
            Gateway.Default.Delete<Knowledge>(Knowledge._.Kns_ID == sort.Kns_ID);
            Gateway.Default.Delete<KnowledgeSort>(KnowledgeSort._.Kns_ID == sort.Kns_ID);
        }
        public KnowledgeSort SortSingle(long identify)
        {
            return Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == identify).ToFirst<KnowledgeSort>();
        }
        public KnowledgeSort[] GetSortAll(int orgid, long couid, string search, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(KnowledgeSort._.Org_ID == orgid);
            if (couid >= 0) wc.And(KnowledgeSort._.Cou_ID == couid);
            if (isUse != null) wc.And(KnowledgeSort._.Kns_IsUse == (bool)isUse);
            if(!string.IsNullOrWhiteSpace(search)) wc.And(KnowledgeSort._.Kns_Name.Contains(search));
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

        public KnowledgeSort[] GetSortChilds(long pid, long couid, bool? isUse)
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
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 当前分类下的所有子专业id
        /// </summary>
        /// <param name="kns">当前分类id</param>
        /// <param name="orgid">机构的ID</param>
        public List<long> TreeID(long kns, int orgid)
        {
            List<long> list = new List<long>();
            if (orgid <= 0)
            {
                KnowledgeSort sort = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == kns).ToFirst<KnowledgeSort>();
                if (sort == null) return list;
                orgid = sort.Org_ID;
            }
            //取同一个机构下的所有章节
            KnowledgeSort[] sorts = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Org_ID == orgid).ToArray<KnowledgeSort>();
            list = _treeid(kns, sorts);
            return list;
        }
        private List<long> _treeid(long id, KnowledgeSort[] arr)
        {
            List<long> list = new List<long>();
            if (id > 0) list.Add(id);
            foreach (KnowledgeSort o in arr)
            {
                if (o.Kns_PID != id) continue;
                List<long> tm = _treeid(o.Kns_ID, arr);
                foreach (long t in tm)
                    list.Add(t);
            }
            return list;
        }
        #endregion
    }
}
