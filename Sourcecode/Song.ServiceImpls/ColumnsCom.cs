using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml;

namespace Song.ServiceImpls
{
    public class ColumnsCom : IColumns
    {

        public int Add(Song.Entities.Columns entity)
        {
            entity.Col_CrtTime = DateTime.Now;
            //如果没有排序号，则自动计算
            if (entity.Col_Tax < 1)
            {
                object obj = Gateway.Default.Max<Columns>( Columns._.Col_Tax,Columns._.Col_PID == entity.Col_PID);
                entity.Col_Tax = obj is int ? (int)obj + 1 : 0;
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Columns>(entity);
        }

        public void Save(Song.Entities.Columns entity)
        {
            using (DbTrans trans = Gateway.Default.BeginTrans())
            {
                try
                {
                    trans.Save<Columns>(entity);                    
                    //新闻，产品，图片，视频，下载
                    trans.Update<Article>(new Field[] { Article._.Col_Name }, new object[] { entity.Col_Name }, Article._.Col_UID == entity.Col_UID);                                 
                            
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

        public void SaveOrder(string xml)
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
                        Song.Entities.Columns nc = this.Single(id);
                        if (nc != null)
                        {
                            nc.Col_PID = pid;
                            nc.Col_Tax = tax;
                            tran.Save<Columns>(nc);
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

        public void Delete(int identify)
        {
            Columns col = Gateway.Default.From<Columns>().Where(Columns._.Col_ID == identify).ToFirst<Columns>();
            Columns[] child = this.Children(col.Col_UID, null);
            foreach (Columns n in child)
                Delete(n.Col_ID);
           
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //如果是新闻栏目，则删除所有新闻
                    if (col.Col_Type == "News") Business.Do<IContents>().ArticleDeleteAll(-1, col.Col_UID); 
                           
                    if (col.Col_Type == "Article") Business.Do<IContents>().ArticleDeleteAll(-1, col.Col_UID);
                    tran.Delete<Columns>(Columns._.Col_ID == identify);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        public Song.Entities.Columns Single(int identify)
        {
            return Gateway.Default.From<Columns>().Where(Columns._.Col_ID == identify).ToFirst<Columns>();
        }
        public Columns Single(string uid)
        {
            return Gateway.Default.From<Columns>().Where(Columns._.Col_UID == uid).ToFirst<Columns>();
        }
        public Song.Entities.Columns[] All(int orgid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Columns._.Org_ID == orgid);
            if (isUse != null) wc.And(Columns._.Col_IsUse == (bool)isUse);
            return Gateway.Default.From<Columns>().Where(wc).OrderBy(Columns._.Col_Tax.Asc).ToArray<Columns>();
        }

        public Columns[] ColumnCount(int orgid, string type, bool? isUse, int count)
        {
            return ColumnCount(orgid, null, type, isUse, count);
        }
        public Columns[] ColumnCount(int orgid, string pid, string type, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Columns._.Org_ID == orgid);
            if (pid != null) wc.And(Columns._.Col_PID == pid);
            if (isUse != null) wc.And(Columns._.Col_IsUse == (bool)isUse);
            if (type != null && type.Length > 0)
            {
                type = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(type.ToLower());
                wc.And(Columns._.Col_Type == type);
            }
            return Gateway.Default.From<Columns>().Where(wc).OrderBy(Columns._.Col_Tax.Asc).ToArray<Columns>(count);

        }
        public Song.Entities.Columns[] Children(string pid, bool? isUse)
        {
            WhereClip wc = Columns._.Col_PID == pid;
            if (isUse != null) wc.And(Columns._.Col_IsUse == (bool)isUse);
            return Gateway.Default.From<Columns>().Where(wc).OrderBy(Columns._.Col_Tax.Asc).ToArray<Columns>();
        }
        /// <summary>
        /// 是否有下级栏目
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public bool IsChildren(string pid, bool? isUse)
        {
            WhereClip wc = Columns._.Col_PID == pid;
            if (isUse != null) wc.And(Columns._.Col_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Columns>(wc);
            return count > 0;
        }
        /// <summary>
        /// 当前分类下的所有子分类id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<string> TreeID(string uid)
        {
            List<string> list = new List<string>();
            list.Add(uid);
            Columns[] ols = Gateway.Default.From<Columns>().Where(Columns._.Col_PID == uid).ToArray<Columns>();
            foreach (Columns o in ols)
            {
                List<string> tm = TreeID(o.Col_UID);
                foreach (string t in tm)
                    list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// 更新栏目结构
        /// </summary>
        /// <param name="items"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public bool UpdateColumnsTree(Columns[] items, int orgid)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Columns>(Columns._.Org_ID == orgid);
                    foreach (Columns item in items)
                    {
                        item.Org_ID = orgid;
                        if (string.IsNullOrWhiteSpace(item.Col_Name))
                            item.Col_Name = "null";
                        tran.Save<Columns>(item);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
    }
}
