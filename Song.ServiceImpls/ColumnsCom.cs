using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Common;
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
                    trans.Update<Article>(new Field[] { Article._.Col_Name }, new object[] { entity.Col_Name }, Article._.Col_Id == entity.Col_ID);
                    trans.Update<Product>(new Field[] { Product._.Col_Name }, new object[] { entity.Col_Name }, Product._.Col_Id == entity.Col_ID);
                    trans.Update<Picture>(new Field[] { Picture._.Col_Name }, new object[] { entity.Col_Name }, Picture._.Col_Id == entity.Col_ID);
                    trans.Update<Video>(new Field[] { Video._.Col_Name }, new object[] { entity.Col_Name }, Video._.Col_Id == entity.Col_ID);
                    trans.Update<Download>(new Field[] { Download._.Col_Name }, new object[] { entity.Col_Name }, Download._.Col_Id == entity.Col_ID);
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
                        int pid = Convert.ToInt32(xe.Attributes["pid"].Value);
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
                finally
                {
                    tran.Close();
                }
            }
        }

        public void Delete(int identify)
        {
            Columns[] child = this.Children(identify, null);
            foreach (Columns n in child)
                Delete(n.Col_ID);
            Columns col = Gateway.Default.From<Columns>().Where(Columns._.Col_ID == identify).ToFirst<Columns>();
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //如果是新闻栏目，则删除所有新闻
                    if (col.Col_Type == "News") Business.Do<IContents>().ArticleDeleteAll(-1, identify);
                    if (col.Col_Type == "Product") Business.Do<IContents>().ProductDeleteAll(-1, identify); //删除当前栏目下的产品
                    if (col.Col_Type == "Picture") Business.Do<IContents>().PictureDeleteAll(-1, identify); //删除图片
                    if (col.Col_Type == "Video") Business.Do<IContents>().VideoDeleteAll(-1, identify);     //删除视频
                    if (col.Col_Type == "Download") Business.Do<IContents>().DownloadDeleteAll(-1, identify);   //删除下载资料
                    if (col.Col_Type == "Article") Business.Do<IContents>().ArticleDeleteAll(-1, identify);
                    tran.Delete<Columns>(Columns._.Col_ID == identify);
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

        public Song.Entities.Columns Single(int identify)
        {
            return Gateway.Default.From<Columns>().Where(Columns._.Col_ID == identify).ToFirst<Columns>();
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
            return ColumnCount(orgid, -1, type, isUse, count);
        }
        public Columns[] ColumnCount(int orgid, int pid, string type, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Columns._.Org_ID == orgid);
            if (pid >= 0) wc.And(Columns._.Col_PID == pid);
            if (isUse != null) wc.And(Columns._.Col_IsUse == (bool)isUse);
            if (type != null && type.Length > 0)
            {
                type = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(type.ToLower());
                wc.And(Columns._.Col_Type == type);
            }
            return Gateway.Default.From<Columns>().Where(wc).OrderBy(Columns._.Col_Tax.Asc).ToArray<Columns>(count);

        }
        public Song.Entities.Columns[] Children(int pid, bool? isUse)
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
        public bool IsChildren(int pid, bool? isUse)
        {
            WhereClip wc = Columns._.Col_PID == pid;
            if (isUse != null) wc.And(Columns._.Col_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Columns>(wc);
            return count > 0;
        }
        /// <summary>
        /// 当前分类下的所有子分类id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> TreeID(int id)
        {
            List<int> list = new List<int>();
            list.Add(id);
            Columns[] ols = Gateway.Default.From<Columns>().Where(Columns._.Col_PID == id).ToArray<Columns>();
            foreach (Columns o in ols)
            {
                List<int> tm = TreeID(o.Col_ID);
                foreach (int t in tm)
                    list.Add(t);
            }
            return list;
        }

        public bool RemoveUp(int orgid, int id)
        {
            //当前对象
            Columns current = Gateway.Default.From<Columns>().Where(Columns._.Col_ID == id).ToFirst<Columns>();
            int tax = (int)current.Col_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            Columns prev = Gateway.Default.From<Columns>()
                .Where(Columns._.Col_Tax < tax && Columns._.Col_PID == current.Col_PID && Columns._.Org_ID == orgid)
                .OrderBy(Columns._.Col_Tax.Desc).ToFirst<Columns>();
            if (prev == null) return false;
            //交换排序号
            current.Col_Tax = prev.Col_Tax;
            prev.Col_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Columns>(current);
                    tran.Save<Columns>(prev);
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

        public bool RemoveDown(int orgid, int id)
        {
            //当前对象
            Columns current = Gateway.Default.From<Columns>().Where(Columns._.Col_ID == id).ToFirst<Columns>();
            int tax = (int)current.Col_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            Columns next = Gateway.Default.From<Columns>()
                .Where(Columns._.Col_Tax > tax && Columns._.Col_PID == current.Col_PID && Columns._.Org_ID == orgid)
                .OrderBy(Columns._.Col_Tax.Asc).ToFirst<Columns>();
            if (next == null) return false;
            //交换排序号
            current.Col_Tax = next.Col_Tax;
            next.Col_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Columns>(current);
                    tran.Save<Columns>(next);
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
    }
}
