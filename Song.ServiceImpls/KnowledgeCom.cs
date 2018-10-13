using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
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
            return Gateway.Default.Save<Knowledge>(entity);
        }

        public void KnowledgeSave(Knowledge entity)
        {
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

        public Knowledge KnowledgePrev(int couid, int kns, int id)
        {
            WhereClip wc = new WhereClip();
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.From<Knowledge>().OrderBy(Knowledge._.Kn_ID.Desc)
               .Where(wc && Knowledge._.Kn_ID > id).ToFirst<Knowledge>();
        }

        public Knowledge KnowledgeNext(int couid, int kns, int id)
        {
            WhereClip wc = new WhereClip();
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (kns >= 0) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.From<Knowledge>().OrderBy(Knowledge._.Kn_ID.Asc)
              .Where(wc && Knowledge._.Kn_ID < id).ToFirst<Knowledge>();
        }
        public Knowledge[] KnowledgeCount(int orgid, bool? isUse, int kns, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > -1) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(count);
        }
        public Knowledge[] KnowledgeCount(int orgid, int couid, int kns, string searTxt, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (couid >= 0) wc.And(Knowledge._.Cou_ID == couid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > -1) wc.And(Knowledge._.Kns_ID == kns);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Like("%" + searTxt + "%"));
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(count);
        }
        public int KnowledgeOfCount(int orgid, int kns, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Knowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == (bool)isUse);
            if (kns > -1) wc.And(Knowledge._.Kns_ID == kns);
            return Gateway.Default.Count<Knowledge>(wc);
        }

        public Knowledge[] KnowledgePager(int orgid, bool? isUse, int kns, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Knowledge._.Org_ID == orgid;
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == isUse);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        public Knowledge[] KnowledgePager(int orgid, int couid, int kns, bool? isUse, bool? isHot, bool? isRec, bool? isTop, string searTxt, int size, int index, out int countSum)
        {

            WhereClip wc = Knowledge._.Org_ID == orgid;
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            if (kns > 0) wc.And(Knowledge._.Kns_ID == kns);
            if (isUse != null) wc.And(Knowledge._.Kn_IsUse == isUse);
            if (isHot != null) wc.And(Knowledge._.Kn_IsHot == isHot);
            if (isRec != null) wc.And(Knowledge._.Kn_IsRec == isRec);
            if (isTop != null) wc.And(Knowledge._.Kn_IsTop == isTop);
            if (searTxt != string.Empty) wc.And(Knowledge._.Kn_Title.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Knowledge>(wc);
            return Gateway.Default.From<Knowledge>().Where(wc).OrderBy(Knowledge._.Kn_CrtTime.Desc).ToArray<Knowledge>(size, (index - 1) * size);
        }
        public Knowledge[] KnowledgePager(int couid, string kns, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(Knowledge._.Cou_ID == couid);
            wc.And(Knowledge._.Kn_IsUse == true);
            if (!string.IsNullOrWhiteSpace(kns))
            {
                WhereClip wcSbjid = new WhereClip();
                foreach (string tm in kns.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(tm)) continue;
                    int sbj = 0;
                    int.TryParse(tm, out sbj);
                    if (sbj == 0) continue;
                    wcSbjid.Or(Knowledge._.Kns_ID == sbj);
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
                        int pid = Convert.ToInt32(xe.Attributes["pid"].Value);
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
            KnowledgeSort[] child = this.GetSortChilds(identify, -1, null);
            foreach (KnowledgeSort n in child)
            {
                SortDelete(n.Kns_ID);
            }
            Gateway.Default.Delete<Knowledge>(Knowledge._.Kns_ID == identify);
            Gateway.Default.Delete<KnowledgeSort>(KnowledgeSort._.Kns_ID == identify);
        }

        public KnowledgeSort SortSingle(int identify)
        {
            return Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == identify).ToFirst<KnowledgeSort>();
        }

        public KnowledgeSort[] GetSortAll(int orgid, int couid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(KnowledgeSort._.Org_ID == orgid);
            if (couid >= 0) wc.And(KnowledgeSort._.Cou_ID == couid);
            if (isUse != null) wc.And(KnowledgeSort._.Kns_IsUse == (bool)isUse);
            return Gateway.Default.From<KnowledgeSort>().Where(wc).OrderBy(KnowledgeSort._.Kns_Tax.Asc).ToArray<KnowledgeSort>();
        }

        public KnowledgeSort[] GetSortAll(int orgid, int couid, int pid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(KnowledgeSort._.Org_ID == orgid);
            if (couid > 0) wc.And(KnowledgeSort._.Cou_ID == couid);
            if (pid >= 0) wc.And(KnowledgeSort._.Kns_PID == pid);
            if (isUse != null) wc.And(KnowledgeSort._.Kns_IsUse == (bool)isUse);
            return Gateway.Default.From<KnowledgeSort>().Where(wc).OrderBy(KnowledgeSort._.Kns_Tax.Asc).ToArray<KnowledgeSort>();
        }

        public KnowledgeSort[] GetSortChilds(int pid, int couid, bool? isUse)
        {
            WhereClip wc = KnowledgeSort._.Kns_PID == pid;
            if (couid >= 0) wc.And(KnowledgeSort._.Cou_ID == couid);
            if (isUse != null) wc.And(KnowledgeSort._.Kns_IsUse == (bool)isUse);
            return Gateway.Default.From<KnowledgeSort>().Where(wc).OrderBy(KnowledgeSort._.Kns_Tax.Asc).ToArray<KnowledgeSort>();
        }
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool SortRemoveUp(int id)
        {
            //当前对象
            KnowledgeSort current = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == id).ToFirst<KnowledgeSort>();
            int tax = current.Kns_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            KnowledgeSort prev = Gateway.Default.From<KnowledgeSort>()
                .Where(KnowledgeSort._.Kns_Tax < tax && KnowledgeSort._.Kns_PID == current.Kns_PID && KnowledgeSort._.Cou_ID == current.Cou_ID)
                .OrderBy(KnowledgeSort._.Kns_Tax.Desc).ToFirst<KnowledgeSort>();
            if (prev == null) return false;
            //交换排序号
            current.Kns_Tax = prev.Kns_Tax;
            prev.Kns_Tax = prev.Kns_Tax == tax ? tax - 1 : tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<KnowledgeSort>(current);
                    tran.Save<KnowledgeSort>(prev);
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
        public bool SortRemoveDown(int id)
        {
            //当前对象
            KnowledgeSort current = Gateway.Default.From<KnowledgeSort>().Where(KnowledgeSort._.Kns_ID == id).ToFirst<KnowledgeSort>();
            int tax = current.Kns_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            KnowledgeSort next = Gateway.Default.From<KnowledgeSort>()
                .Where(KnowledgeSort._.Kns_Tax > tax && KnowledgeSort._.Kns_PID == current.Kns_PID && KnowledgeSort._.Cou_ID == current.Cou_ID)
                .OrderBy(KnowledgeSort._.Kns_Tax.Asc).ToFirst<KnowledgeSort>();
            if (next == null) return false;
            //交换排序号
            current.Kns_Tax = next.Kns_Tax;
            next.Kns_Tax = next.Kns_Tax == tax ? tax + 1 : tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<KnowledgeSort>(current);
                    tran.Save<KnowledgeSort>(next);
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
