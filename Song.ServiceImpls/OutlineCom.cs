using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml.Serialization;
using System.Threading;

namespace Song.ServiceImpls
{
    public class OutlineCom : IOutline
    {
        #region 课程章节管理
        /// <summary>
        /// 添加章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void OutlineAdd(Outline entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;                

            //计算排序号
            object obj = Gateway.Default.Max<Outline>(Outline._.Ol_Tax, Outline._.Cou_ID == entity.Cou_ID && Outline._.Ol_PID == entity.Ol_PID);
            entity.Ol_Tax = obj is int ? (int)obj + 1 : 1;
            if (string.IsNullOrWhiteSpace(entity.Ol_UID))
                entity.Ol_UID = WeiSha.Common.Request.UniqueID();
            //层级
            entity.Ol_Level = _ClacLevel(entity);
            entity.Ol_XPath = _ClacXPath(entity);
            Gateway.Default.Save<Outline>(entity);
            this.OnSave(null, EventArgs.Empty);
        }
        /// <summary>
        /// 批量添加章节，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程（或课程）id</param>
        /// <param name="names">名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        public Outline OutlineBatchAdd(int orgid, int sbjid, int couid, string names)
        {
            //整理名称信息
            names = names.Replace("，", ",");
            List<string> listName = new List<string>();
            foreach (string s in names.Split(','))
                if (s.Trim() != "") listName.Add(s.Trim());
            //
            int pid = 0;
            Song.Entities.Outline last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.Outline current = OutlineIsExist(orgid, sbjid, couid, pid, listName[i]);
                if (current == null)
                {
                    current = new Outline();
                    current.Ol_Name = listName[i].Trim();
                    current.Ol_IsUse = true;
                    current.Org_ID = orgid;
                    current.Sbj_ID = sbjid;
                    current.Cou_ID = couid;
                    current.Ol_PID = pid;
                    this.OutlineAdd(current);
                }
                last = current;
                pid = current.Ol_ID;
            }
            return last;
        }
        /// <summary>
        /// 是否已经存在章节
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程（或课程）id</param>
        /// <param name="pid">上级id</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Outline OutlineIsExist(int orgid, int sbjid, int couid, int pid, string name)
        {
            WhereClip wc = Outline._.Org_ID == orgid;
            if (sbjid > 0) wc &= Outline._.Sbj_ID == sbjid;
            if (couid > 0) wc &= Outline._.Cou_ID == couid;
            if (pid >= 0) wc &= Outline._.Ol_PID == pid;
            return Gateway.Default.From<Outline>().Where(wc && Outline._.Ol_Name == name.Trim()).ToFirst<Outline>();
        }
        /// <summary>
        /// 修改章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void OutlineSave(Outline entity)
        {
            Outline old = OutlineSingle(entity.Ol_ID);
            if (old.Ol_PID != entity.Ol_PID)
            {
                object obj = Gateway.Default.Max<Outline>(Outline._.Ol_Tax, Outline._.Org_ID == entity.Org_ID && Outline._.Ol_PID == entity.Ol_PID);
                entity.Ol_Tax = obj is int ? (int)obj + 1 : 0;
            }
            entity.Ol_Level = _ClacLevel(entity);
            entity.Ol_XPath = _ClacXPath(entity);
            Gateway.Default.Save<Outline>(entity);
            this.OnSave(entity, EventArgs.Empty);
        }
        /// <summary>
        /// 删除章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void OutlineDelete(Outline entity)
        {
            if (entity == null) return;
            List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(entity.Ol_UID);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //删除附件
                    foreach (Song.Entities.Accessory ac in acs)                   
                        Business.Do<IAccessory>().Delete(ac.As_Id);
                    //先清理试题
                    tran.Delete<Questions>(Questions._.Ol_ID == entity.Ol_ID);
                    tran.Delete<Outline>(Outline._.Ol_ID == entity.Ol_ID);                                    
                    tran.Commit();
                    this.OnDelete(entity, null);
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
        public void OutlineDelete(int identify)
        {
            Song.Entities.Outline ol = this.OutlineSingle(identify);
            this.OutlineDelete(ol);
            
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Outline OutlineSingle(int identify)
        {
            //当前章节
            Outline curr = null;
            //从缓存中读取
            List<Outline> list = WeiSha.Common.Cache<Outline>.Data.List;
            if (list == null || list.Count < 1) list = this.OutlineBuildCache();
            List<Outline> tm = (from l in list
                                where l.Ol_ID == identify
                                select l).ToList<Outline>();
            if (tm.Count > 0) curr = tm[0];
            if (curr == null) curr = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == identify).ToFirst<Outline>();
            return curr;
           
        }
        /// <summary>
        /// 获取单一实体对象，按唯一值，即UID；
        /// </summary>
        /// <param name="uid">全局唯一值</param>
        /// <returns></returns>
        public Outline OutlineSingle(string uid)
        {
            //当前章节
            Outline curr = null;
            //从缓存中读取
            List<Outline> list = WeiSha.Common.Cache<Outline>.Data.List;
            if (list == null || list.Count < 1) list = this.OutlineBuildCache();
            List<Outline> tm = (from l in list
                                where l.Ol_UID == uid
                                select l).ToList<Outline>();
            if (tm.Count > 0) curr = tm[0];
            if (curr == null) curr = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == uid).ToFirst<Outline>();
            return curr;
        }
        /// <summary>
        /// 当前章节下的所有子章节id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> TreeID(int id)
        {
            List<int> list = new List<int>();
            Outline ol = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == id).ToFirst<Outline>();
            if (ol == null) return list;     
            //取同一个课程下的所有章节
            Outline[] ols = Gateway.Default.From<Outline>().Where(Outline._.Cou_ID == ol.Cou_ID).ToArray<Outline>();
            list = _treeid(id, ols);
            return list;
        }
        private List<int> _treeid( int id,Outline[] ols)
        {
            List<int> list = new List<int>();
            list.Add(id);
            foreach (Outline o in ols)
            {
                if (o.Ol_PID != id) continue;
                List<int> tm = _treeid(o.Ol_ID, ols);
                foreach (int t in tm)
                    list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 获取某个课程下第一个章节
        /// </summary>
        /// <param name="couid">课程Id</param>
        /// <param name="isUse">是否包括只是允许的章节,null取所有范围，true只是允许采用的章节,false反之</param>
        /// <returns></returns>
        public Outline OutlineFirst(int couid, bool? isUse)
        {
            Song.Entities.Outline[] outlines = null;
            WhereClip wc = Outline._.Cou_ID == couid;
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            outlines = Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>();
            Song.Entities.Outline ol = null;
            if (outlines != null && outlines.Length > 0)
            {
                foreach (Song.Entities.Outline t in outlines)
                {
                    if (t.Ol_PID == 0)
                    {
                        ol = t;
                        break;
                    }
                }
            }
            return ol;
        }
        /// <summary>
        /// 获取章节名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public string OutlineName(int identify)
        {
            Outline entity = this.OutlineSingle(identify);
            if (entity == null) return "";
            string xpath = entity.Ol_Name;
            Song.Entities.Outline tm = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == entity.Ol_PID).ToFirst<Outline>();
            while (tm != null)
            {
                xpath = tm.Ol_Name + "," + xpath;
                if (tm.Ol_PID == 0) break;
                if (tm.Ol_PID != 0)
                {
                    tm = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == entity.Ol_PID).ToFirst<Outline>();
                }
            }
            return xpath;
        }
        /// <summary>
        /// 获取所有课程章节
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public Outline[] OutlineAll(int couid, bool? isUse)
        {
            //从缓存中读取
            List<Outline> list = WeiSha.Common.Cache<Outline>.Data.List;
            if (list == null || list.Count < 1) list = this.OutlineBuildCache();
            //linq查询
            var from = from l in list select l;
            if (couid > 0) from = from.Where<Outline>(p => p.Cou_ID == couid);
            if (isUse != null) from = from.Where<Outline>(p => p.Ol_IsUse == (bool)isUse);
            List<Outline> tm = from.OrderBy(c => c.Ol_Tax).ToList<Outline>();
            if (tm.Count > 0) return tm.ToArray<Outline>();

            //orm查询
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(Outline._.Cou_ID == couid);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>();
        }
        /// <summary>
        /// 清除章节下的试题、附件等
        /// </summary>
        /// <param name="identify"></param>
        public void OutlineClear(int identify)
        {
            //清理试题
            Gateway.Default.Delete<Questions>(Questions._.Ol_ID == identify);
            //清理附件
            Outline ol = this.OutlineSingle(identify);
            if (ol != null)
            {
                Business.Do<IAccessory>().Delete(ol.Ol_UID);
            }

        }
        private static object lock_cache_build = new object();
        /// <summary>
        /// 构建缓存
        /// </summary>
        public List<Outline> OutlineBuildCache()
        {
            lock (lock_cache_build)
            {
                WeiSha.Common.Cache<Song.Entities.Outline>.Data.Clear();
                Song.Entities.Outline[] outls = Gateway.Default.From<Song.Entities.Outline>().ToArray<Outline>();
                //计算每个章节下的试题数
                foreach (Outline o in outls)
                {
                    o.Ol_QuesCount = this.QuesOfCount(o.Ol_ID, -1, true, true);
                    Gateway.Default.Save<Outline>(o);
                }
                WeiSha.Common.Cache<Song.Entities.Outline>.Data.Fill(outls);
                return WeiSha.Common.Cache<Outline>.Data.List;
            }
        }
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        public Outline[] OutlineCount(int couid, string search, bool? isUse, int count)
        {
            WhereClip wc = Outline._.Cou_ID == couid;
            if (!string.IsNullOrWhiteSpace(search)) wc.And(Outline._.Ol_Name.Like("%" + search + "%"));
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>(count);
        }
        public Outline[] OutlineCount(int couid, int pid, bool? isUse, int count)
        {
            WhereClip wc = Outline._.Cou_ID == couid;
            if (pid >= 0) wc.And(Outline._.Ol_PID == pid);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>(count);
        }
        public Outline[] OutlineCount(int orgid, int sbjid, int couid, int pid, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Outline._.Org_ID == orgid;
            if (sbjid > 0) wc &= Outline._.Sbj_ID == sbjid;
            if (couid > 0) wc &= Outline._.Cou_ID == couid;
            if (pid > -1) wc &= Outline._.Ol_PID == pid;
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>(count);
        }
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public int OutlineOfCount(int couid, int pid, bool? isUse)
        {
            WhereClip wc = Outline._.Cou_ID == couid;
            if (pid >= 0) wc.And(Outline._.Ol_PID == pid);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.Count<Outline>(wc);
        }
        public int OutlineOfCount(int orgid, int sbjid, int couid, int pid, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Outline._.Org_ID == orgid;
            if (sbjid > 0) wc &= Outline._.Sbj_ID == sbjid;
            if (couid > 0) wc &= Outline._.Cou_ID == couid;
            if (pid > -1) wc &= Outline._.Ol_PID == pid;
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.Count<Outline>(wc);
        }
        /// <summary>
        /// 是否有子级章节
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public bool OutlineIsChildren(int couid, int pid, bool? isUse)
        {
            WhereClip wc = Outline._.Cou_ID == couid;
            if (pid >= 0) wc.And(Outline._.Ol_PID == pid);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Outline>(wc);
            return count > 0;
        }
        /// <summary>
        /// 当前章节是否有试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public bool OutlineIsQues(int olid, bool? isUse)
        {
            WhereClip wc = Questions._.Ol_ID == olid;
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<Questions>(wc);
            return count > 0;
        }
        /// <summary>
        /// 当前章节的子级章节
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public Outline[] OutlineChildren(int couid, int pid, bool? isUse,int count)
        {
            WhereClip wc = Outline._.Cou_ID == couid;
            if (pid >= 0) wc.And(Outline._.Ol_PID == pid);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>(count);
        }
        /// <summary>
        /// 分页取课程章节的信息
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Outline[] OutlinePager(int couid, bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Outline._.Cou_ID == couid;
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(Outline._.Ol_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Outline>(wc);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>(size, (index - 1) * size);
        }
        /// <summary>
        /// 当前章节的试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Questions[] QuesCount(int olid, int type, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (olid > 0) wc.And(Questions._.Ol_ID == olid);
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.From<Questions>().Where(wc).OrderBy(Questions._.Qus_ID.Desc).ToArray<Questions>(count);
        }
        /// <summary>
        /// 当前章节有多少道试题
        /// </summary>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <param name="isAll">是否取所有（当前章节下所有子章节的试题一块算）</param>
        /// <returns></returns>
        public int QuesOfCount(int olid, int type, bool? isUse, bool isAll)
        {
            WhereClip wc = new WhereClip();            
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (olid > 0 && isAll)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = TreeID(olid);
                foreach (int l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);
            }
            return Gateway.Default.Count<Questions>(wc); 
        }

        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool OutlineUp(int couid, int id)
        {
            //当前对象
            Outline current = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == id).ToFirst<Outline>();
            int tax = (int)current.Ol_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            Outline prev = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax < tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == current.Ol_PID)
                .OrderBy(Outline._.Ol_Tax.Desc).ToFirst<Outline>();
            //有当前等级哥哥对像
            if (prev != null)
            {
                //交换排序号
                current.Ol_Tax = prev.Ol_Tax;
                prev.Ol_Tax = tax;
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    try
                    {
                        tran.Save<Outline>(current);
                        tran.Save<Outline>(prev);
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
                this.OnSave(null, EventArgs.Empty);
                return true;
            }
            else
            {
                if (current.Ol_PID == 0) return false;
                //没有当前等级哥哥对像
                Song.Entities.Outline top = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == current.Ol_PID).ToFirst<Outline>();
                if (top == null) return false;
                //父级的哥哥
                Song.Entities.Outline topPrev = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax < top.Ol_Tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == top.Ol_PID)
                .OrderBy(Outline._.Ol_Tax.Desc).ToFirst<Outline>();
                if (topPrev == null) return false;
                //父级哥哥的子级的最大序号
                object obj = Gateway.Default.Max<Outline>(Outline._.Ol_Tax, Outline._.Ol_PID == topPrev.Ol_ID);
                current.Ol_Tax = obj is int ? (int)obj + 1 : 1;
                current.Ol_PID = topPrev.Ol_ID;
                current.Ol_Level = 1;
                Gateway.Default.Save<Outline>(current);
                this.OnSave(null, EventArgs.Empty);
                return true;
            }
        }
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool OutlineDown(int couid, int id)
        {
            //当前对象
            Outline current = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == id).ToFirst<Outline>();
            int tax = (int)current.Ol_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            Outline next = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax > tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == current.Ol_PID)
                .OrderBy(Outline._.Ol_Tax.Asc).ToFirst<Outline>();
            if (next != null)
            {
                current.Ol_Tax = next.Ol_Tax;
                next.Ol_Tax = tax;
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    try
                    {
                        tran.Save<Outline>(current);
                        tran.Save<Outline>(next);
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
                this.OnSave(null, EventArgs.Empty);
                return true;
            }
            else
            {
                if (current.Ol_PID == 0) return false;
                //没有当前等级父对象
                Song.Entities.Outline parent = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == current.Ol_PID).ToFirst<Outline>();
                if (parent == null) return false;
                //父级的弟弟
                Song.Entities.Outline topNext = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax > parent.Ol_Tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == parent.Ol_PID)
                .OrderBy(Outline._.Ol_Tax.Asc).ToFirst<Outline>();
                if (topNext == null) return false;
                //父级弟弟的子级
                Song.Entities.Outline[] child = this.OutlineCount(couid, topNext.Ol_ID, null, -1);
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    try
                    {
                        current.Ol_Tax = 1;
                        current.Ol_PID = topNext.Ol_ID;
                        tran.Save<Outline>(current);
                        //父级弟弟的子级的排序号重排
                        for (int i = 0; i < child.Length; i++)
                            tran.Update<Outline>(Outline._.Ol_Tax, i + 2, Outline._.Ol_ID == child[i].Ol_ID);
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
            this.OnSave(null, EventArgs.Empty);
            return true;
        }
        /// <summary>
        /// 将当前章节升级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool OutlineToLeft(int couid, int id)
        {
            //当前对象
            Outline current = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == id).ToFirst<Outline>();
            //当前父对象
            Outline parent = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == current.Ol_PID).ToFirst<Outline>();
            //顶级列表
            Song.Entities.Outline[] top = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax > parent.Ol_Tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == parent.Ol_PID)
                .OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>();
            //当前父的同级中，比自己大的子级（即自身后面的）
            Song.Entities.Outline[] child = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax > current.Ol_Tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == current.Ol_PID)
                .OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>();
            //
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    current.Ol_PID = 0;
                    current.Ol_Level = 0;
                    current.Ol_Tax = parent.Ol_Tax + 1;
                    current.Attach();
                    tran.Save<Outline>(current);
                    //处理升级后的顶级排序问题
                    for (int i = 0; i < top.Length; i++)
                    {
                        top[i].Ol_Tax = top[i].Ol_Tax + 1;
                        top[i].Attach();
                        tran.Save<Outline>(top[i]);
                    }
                    //处理升级后，同级的子级的排序号
                    for (int i = 0; i < child.Length; i++)
                    {
                        child[i].Ol_Tax = i + 1;
                        child[i].Ol_PID = current.Ol_ID;
                        child[i].Ol_Level = 1;
                        child[i].Attach();
                        tran.Save<Outline>(child[i]);
                    }
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
            this.OnSave(null, EventArgs.Empty);
            return true;
        }
        /// <summary>
        /// 将当前章节退级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool OutlineToRight(int couid, int id)
        {
            //当前对象
            Outline current = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == id).ToFirst<Outline>();
            if (current.Ol_Tax == 1) throw new WeiSha.Common.ExceptionForAlert("最顶级的第一个章节，不可以降级");
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            Outline prev = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax < current.Ol_Tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == current.Ol_PID)
                .OrderBy(Outline._.Ol_Tax.Desc).ToFirst<Outline>();
            object objmaxTax = Gateway.Default.Max<Outline>(Outline._.Ol_Tax, Outline._.Ol_PID == prev.Ol_ID);
            int maxTax = objmaxTax is int ? (int)objmaxTax : 0;
            //当前对象的子级
            Song.Entities.Outline[] child = Gateway.Default.From<Outline>()
                .Where(Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == current.Ol_ID)
                .OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>();
            //顶级列表
            Song.Entities.Outline[] top = Gateway.Default.From<Outline>()
                .Where(Outline._.Ol_Tax > prev.Ol_Tax && Outline._.Cou_ID == current.Cou_ID && Outline._.Ol_PID == prev.Ol_PID && Outline._.Ol_ID != current.Ol_ID)
                .OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>();
            //
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    current.Ol_PID = prev.Ol_ID;
                    current.Ol_Level = 1;
                    current.Ol_Tax = maxTax + 1;
                    tran.Save<Outline>(current);
                    //处理升级后，同级的子级的排序号
                    for (int i = 0; i < child.Length; i++)
                    {
                        child[i].Ol_Tax = i + 2 + maxTax;
                        child[i].Ol_PID = prev.Ol_ID;
                        child[i].Attach();
                        tran.Save<Outline>(child[i]);
                    }
                    for (int i = 0; i < top.Length; i++)
                    {
                        top[i].Ol_Tax = top[i].Ol_Tax - 1;
                        top[i].Attach();
                        tran.Save<Outline>(top[i]);
                    }
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
            this.OnSave(null, EventArgs.Empty);
            return true;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 计算当前对象在多级分类中的层深
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private int _ClacLevel(Song.Entities.Outline entity)
        {
            //if (entity.Ol_PID == 0) return 1;
            int level = 0;
            Song.Entities.Outline tm = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == entity.Ol_PID).ToFirst<Outline>();
            while (tm != null)
            {
                level++;
                if (tm.Ol_PID == 0) break;
                if (tm.Ol_PID != 0)
                {
                    tm = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == tm.Ol_PID).ToFirst<Outline>();
                }
            }
            entity.Ol_Level = level;
            Gateway.Default.Save<Outline>(entity);
            Song.Entities.Outline[] childs = Gateway.Default.From<Outline>().Where(Outline._.Ol_PID == entity.Ol_ID).ToArray<Outline>();
            foreach (Outline s in childs)
            {
                _ClacLevel(s);
            }
            return level;
        }
        /// <summary>
        /// 计算当前对象在多级分类中的路径
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private string _ClacXPath(Song.Entities.Outline entity)
        {
            //if (entity.Ol_PID == 0) return "";
            string xpath = "";
            Song.Entities.Outline tm = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == entity.Ol_PID).ToFirst<Outline>();
            while (tm != null)
            {
                xpath = tm.Ol_ID + "," + xpath;
                if (tm.Ol_PID == 0) break;
                if (tm.Ol_PID != 0)
                {
                    tm = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == tm.Ol_PID).ToFirst<Outline>();
                }
            }
            entity.Ol_XPath = xpath;
            Gateway.Default.Save<Outline>(entity);
            Song.Entities.Outline[] childs = Gateway.Default.From<Outline>().Where(Outline._.Ol_PID == entity.Ol_ID).ToArray<Outline>();
            foreach (Outline s in childs)
            {
                _ClacXPath(s);
            }
            return xpath;
        }
        #endregion

        #region 章节视频事件
        /// <summary>
        /// 添加章节中视频播放事件
        /// </summary>
        /// <param name="entity"></param>
        public void EventAdd(OutlineEvent entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            entity.Oe_CrtTime = DateTime.Now;
            Gateway.Default.Save<OutlineEvent>(entity);
        }
        /// <summary>
        /// 修改播放事件
        /// </summary>
        /// <param name="entity"></param>
        public void EventSave(OutlineEvent entity)
        {
            Gateway.Default.Save<OutlineEvent>(entity);
        }
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="entity"></param>
        public void EventDelete(OutlineEvent entity)
        {
            Gateway.Default.Delete<OutlineEvent>(entity);
        }
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="identify"></param>
        public void EventDelete(int identify)
        {
            Gateway.Default.Delete<OutlineEvent>(OutlineEvent._.Oe_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public OutlineEvent EventSingle(int identify)
        {
            return Gateway.Default.From<OutlineEvent>().Where(OutlineEvent._.Oe_ID == identify).ToFirst<OutlineEvent>();
        }
        /// <summary>
        /// 返回章节下的事件
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="olid">章节ID，不可以为零，否则会取所有</param>
        /// <param name="type">事件类型，1为提醒，2为知识展示，3课堂提问，4实时反馈（例如，选择某项后跳转到某秒）</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public OutlineEvent[] EventAll(int couid, int olid, int type, bool? isUse)
        {
            WhereClip wc = OutlineEvent._.Ol_ID == olid;
            if (couid > 0) wc.And(OutlineEvent._.Oe_IsUse == (bool)isUse);
            if (type > 0) wc.And(OutlineEvent._.Oe_EventType == type);
            if (isUse != null) wc.And(OutlineEvent._.Oe_IsUse == (bool)isUse);
            return Gateway.Default.From<OutlineEvent>().Where(wc).OrderBy(OutlineEvent._.Oe_TriggerPoint.Asc).ToArray<OutlineEvent>();
        }
        /// <summary>
        /// 返回章节下所有事件
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="uid">章节的全局唯一值</param>
        /// <param name="type"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public OutlineEvent[] EventAll(int couid, string uid, int type, bool? isUse)
        {
            WhereClip wc = OutlineEvent._.Ol_UID == uid;
            if (couid > 0) wc.And(OutlineEvent._.Oe_IsUse == (bool)isUse);
            if (type > 0) wc.And(OutlineEvent._.Oe_EventType == type);
            if (isUse != null) wc.And(OutlineEvent._.Oe_IsUse == (bool)isUse);
            return Gateway.Default.From<OutlineEvent>().Where(wc).OrderBy(OutlineEvent._.Oe_TriggerPoint.Asc).ToArray<OutlineEvent>();
        }
        /// <summary>
        /// 获取试题类型的信息,事件类型：1为提醒，2为知识展示，3课堂提问，4实时反馈
        /// </summary>
        /// <param name="oeid"></param>
        /// <returns></returns>
        public DataTable EventQues(int oeid)
        {
            OutlineEvent oev = Gateway.Default.From<OutlineEvent>().Where(OutlineEvent._.Oe_ID == oeid).ToFirst<OutlineEvent>();
            if (oev == null) return null;
            //1为提醒，2为知识展示，3课堂提问，4实时反馈
            if (oev.Oe_EventType != 3) return null;
            string xml = oev.Oe_Datatable;
            if (string.IsNullOrWhiteSpace(oev.Oe_Datatable) || oev.Oe_Datatable.Trim() == "") return null;
            //转成datatable
            try
            {
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DataTable));
                DataTable dt = (DataTable)xmlSerial.Deserialize(new System.IO.StringReader(oev.Oe_Datatable));
                return dt;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获取时间反馈的信息
        /// </summary>
        /// <param name="oeid"></param>
        /// <returns></returns>
        public DataTable EventFeedback(int oeid)
        {
            OutlineEvent oev = Gateway.Default.From<OutlineEvent>().Where(OutlineEvent._.Oe_ID == oeid).ToFirst<OutlineEvent>();
            if (oev == null) return null;
            //1为提醒，2为知识展示，3课堂提问，4实时反馈
            if (oev.Oe_EventType != 4) return null;
            string xml = oev.Oe_Datatable;
            if (string.IsNullOrWhiteSpace(oev.Oe_Datatable) || oev.Oe_Datatable.Trim() == "") return null;
            //转成datatable
            try
            {
                XmlSerializer xmlSerial = new XmlSerializer(typeof(DataTable));
                DataTable dt = (DataTable)xmlSerial.Deserialize(new System.IO.StringReader(oev.Oe_Datatable));
                return dt;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 事件
        public event EventHandler Save;
        public event EventHandler Add;
        public event EventHandler Delete;
        public void OnSave(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (!(sender is Outline)) return;
                Outline ol = (Outline)sender;
                Song.Entities.Outline old = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == ol.Ol_ID).ToFirst<Outline>();
                ol.Ol_QuesCount = Business.Do<IOutline>().QuesOfCount(ol.Ol_ID, -1, true, true);
                WeiSha.Common.Cache<Song.Entities.Outline>.Data.Update(old, ol);
            }
            else
            {
                //填充章节数据到缓存
                this.OutlineBuildCache();
            }
            if (Save != null)
                Save(sender, e);            
        }
        public void OnAdd(object sender, EventArgs e)
        {
            //当前对象（即sender），写入到缓存
            if (sender != null)
            {
                if (!(sender is Outline)) return;
                Outline ol = (Outline)sender;
                WeiSha.Common.Cache<Song.Entities.Outline>.Data.Add(ol);
            }
            if (Add != null)
                Add(sender, e);
        }
        public void OnDelete(object sender, EventArgs e)
        {
            //当前对象（即sender），写入到缓存
            if (sender != null)
            {
                if (!(sender is Outline)) return;
                Outline ol = (Outline)sender;
                WeiSha.Common.Cache<Song.Entities.Outline>.Data.Delete(ol);
                //重建试题缓存（取所有试题进缓存）
                new Thread(new ThreadStart(() =>
                {
                    Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, null, -1);
                    Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Delete("all");
                    Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Add(ques, int.MaxValue, "all");
                })).Start();
            }
            if (Delete != null)
                Delete(sender, e);
        }
        #endregion
    }
}
