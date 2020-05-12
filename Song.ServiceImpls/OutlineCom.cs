using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Linq;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml.Serialization;
using System.Threading;
using System.Xml;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using pili_sdk;

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
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    if (entity.Org_ID <= 0)
                    {
                        Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                        if (org != null) entity.Org_ID = org.Org_ID;
                    }
                    //所属专业
                    if (entity.Sbj_ID <= 0 && entity.Cou_ID > 0)
                    {
                        Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(entity.Cou_ID);
                        entity.Sbj_ID = cou.Sbj_ID;
                    }
                    //计算排序号
                    object obj = tran.Max<Outline>(Outline._.Ol_Tax, Outline._.Cou_ID == entity.Cou_ID && Outline._.Ol_PID == entity.Ol_PID);
                    entity.Ol_Tax = obj is int ? (int)obj + 1 : 1;
                    //唯一id
                    string uid = string.IsNullOrWhiteSpace(entity.Ol_UID) ? WeiSha.Common.Request.UniqueID() : entity.Ol_UID;
                    do
                    {
                        uid = WeiSha.Common.Request.UniqueID();
                    } while (Gateway.Default.Count<Outline>(Outline._.Ol_UID == uid) > 0);
                    entity.Ol_UID = uid;

                    ////层级
                    //entity.Ol_Level = _ClacLevel(entity);
                    //entity.Ol_XPath = _ClacXPath(entity);  
                    //如果是直播章节
                    if (entity.Ol_IsLive)
                    {
                        string liveid = string.Format("{0}_{1}_{2}", entity.Cou_ID, entity.Ol_ID, entity.Ol_UID);
                        try
                        {
                            pili_sdk.pili.Stream stream = Business.Do<ILive>().StreamCreat(liveid);
                            entity.Ol_LiveID = stream.Title;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("无法创建直播流，" + ex.Message);
                        }
                        tran.Update<Course>(Course._.Cou_ExistLive, true, Course._.Cou_ID == entity.Cou_ID);
                    }
                    tran.Save<Outline>(entity);
                    tran.Commit();

                    this.OnSave(null, EventArgs.Empty);
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
            foreach (string str in names.Split(','))
            {
                string s = str.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                if (s.Trim() != "") listName.Add(s.Trim());
            }
            //
            int pid = 0;
            Song.Entities.Outline last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.Outline current = OutlineIsExist(orgid, sbjid, couid, pid, listName[i]);
                if (current == null)
                {
                    current = new Outline();
                    current.Ol_Name = listName[i];
                    current.Ol_IsUse = true;
                    current.Org_ID = orgid;
                    current.Sbj_ID = sbjid;
                    current.Cou_ID = couid;
                    current.Ol_PID = pid;
                    current.Ol_IsFinish = true;     //默认为完结
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
            Outline old = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == entity.Ol_ID).ToFirst<Outline>();
            if (old.Ol_PID != entity.Ol_PID)
            {
                object obj = Gateway.Default.Max<Outline>(Outline._.Ol_Tax, Outline._.Org_ID == entity.Org_ID && Outline._.Ol_PID == entity.Ol_PID);
                entity.Ol_Tax = obj is int ? (int)obj + 1 : 0;
            }
            //如果有下级，设置为章节节点
            int childCount = Gateway.Default.Count<Outline>(Outline._.Ol_PID == entity.Ol_ID);
            entity.Ol_IsNode = childCount > 0;
            //如果有视频，设置视频节点
            int videoCount = Gateway.Default.Count<Accessory>(Accessory._.As_Type == "CourseVideo" && Accessory._.As_Uid==entity.Ol_UID);
            entity.Ol_IsVideo = videoCount > 0;
            //路径
            entity.Ol_Level = _ClacLevel(entity);
            entity.Ol_XPath = _ClacXPath(entity);
            //如果是直播章节
            if (entity.Ol_IsLive)
            {
                pili_sdk.pili.Stream stream = null;
                if (string.IsNullOrWhiteSpace(entity.Ol_LiveID))
                {
                    try
                    {
                        stream = Business.Do<ILive>().StreamCreat();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("直播流创建失败，" + ex.Message);
                    }
                    if (stream != null) entity.Ol_LiveID = stream.Title;
                }
                else
                {
                    try
                    {
                        stream = pili_sdk.Pili.API<pili_sdk.IStream>().GetForTitle(entity.Ol_LiveID);
                        if (stream != null) pili_sdk.Pili.API<pili_sdk.IStream>().Enable(stream);
                    }
                    catch
                    {
                    }
                }
                
            }
            else
            {
                //禁用直播
                if (!string.IsNullOrWhiteSpace(entity.Ol_LiveID))
                {
                    pili_sdk.pili.Stream stream = pili_sdk.Pili.API<pili_sdk.IStream>().GetForTitle(entity.Ol_LiveID);
                    if (stream != null) pili_sdk.Pili.API<pili_sdk.IStream>().Disable(stream);
                }                
            }
            Gateway.Default.Save<Outline>(entity);
            Business.Do<ICourse>().IsLiveCourse(entity.Cou_ID, true);
            this.OnSave(entity, EventArgs.Empty);
        }
        /// <summary>
        /// 更新章节的试题数
        /// </summary>
        /// <param name="olid">章节Id</param>
        /// <param name="count">试题数</param>
        /// <returns></returns>
        public int UpdateQuesCount(int olid, int count)
        {
            Gateway.Default.Update<Outline>(Outline._.Ol_QuesCount, count, Outline._.Ol_ID == olid);
            return count;
        }
        /// <summary>
        /// 导入章节，导入时不立即生成缓存
        /// </summary>
        /// <param name="entity"></param>
        public void OutlineInput(Outline entity)
        {
            //计算排序号
            object obj = Gateway.Default.Max<Outline>(Outline._.Ol_Tax, Outline._.Cou_ID == entity.Cou_ID && Outline._.Ol_PID == entity.Ol_PID);
            entity.Ol_Tax = obj is int ? (int)obj + 1 : 1;
            if (string.IsNullOrWhiteSpace(entity.Ol_UID))
                entity.Ol_UID = WeiSha.Common.Request.UniqueID();
            //层级
            entity.Ol_Level = _ClacLevel(entity);
            entity.Ol_XPath = _ClacXPath(entity);
            Gateway.Default.Save<Outline>(entity);
        }
        /// <summary>
        /// 导出课程章节到Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        public string OutlineExport4Excel(string path, int couid)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Common.App.Get["ExcelInputConfig"].VirtualPath + "课程章节.xml";
            xmldoc.Load(WeiSha.Common.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //当前课程的所有章节
            Song.Entities.Outline[] outls = this.OutlineCount(couid, -1, null, -1);
            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outls);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "OL_ID";
            tree.ParentIdKeyName = "OL_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);
            //取最大深度
            int level = 0;
            foreach (Outline ol in outls)
            {
                if (ol.Ol_Level > level) level = ol.Ol_Level;
            }
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            ISheet sheet = hssfworkbook.CreateSheet(course.Cou_Name);   //创建工作簿对象
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            int index = 0;
            for (int i = 0; i < nodes.Count; i++)
            {
                //是否导出
                string exExport = nodes[i].Attributes["export"] != null ? nodes[i].Attributes["export"].Value : "";
                if (exExport.ToLower() == "false") continue;
                //如果是章节，可能存在多级
                if (nodes[i].Attributes["Column"].Value == "章节名称" && level > 0)
                {
                    index = i;
                    rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
                    for (int l = 1; l <= level + 1; l++)
                    {
                        rowHead.CreateCell(i + l).SetCellValue(nodes[i].Attributes["Column"].Value + l.ToString());
                    }
                }
                else
                {
                    if (i > index)
                        rowHead.CreateCell(i + level).SetCellValue(nodes[i].Attributes["Column"].Value);
                    else
                        rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
                }
            }
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();

            style_size.WrapText = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);  //创建Excel行
                DataRow obj = dt.Rows[i];   //数据项
                for (int j = 0; j < nodes.Count; j++)
                {
                    string exExport = nodes[j].Attributes["export"] != null ? nodes[j].Attributes["export"].Value : ""; //是否导出
                    if (exExport.ToLower() == "false") continue;
                    string column = nodes[j].Attributes["Column"] != null ? nodes[j].Attributes["Column"].Value : "";
                    string field = nodes[j].Attributes["Field"] != null ? nodes[j].Attributes["Field"].Value : "";
                    string format = nodes[j].Attributes["Format"] != null ? nodes[j].Attributes["Format"].Value : "";
                    string datatype = nodes[j].Attributes["DataType"] != null ? nodes[j].Attributes["DataType"].Value : "";
                    string defvalue = nodes[j].Attributes["DefautValue"] != null ? nodes[j].Attributes["DefautValue"].Value : "";

                    string value = "";
                    switch (datatype)
                    {
                        case "date":
                            DateTime tm = Convert.ToDateTime(obj);
                            value = tm > DateTime.Now.AddYears(-100) ? tm.ToString(format) : "";
                            break;
                        default:
                            value = obj.ToString();
                            break;
                    }
                    if (defvalue.Trim() != "")
                    {
                        foreach (string s in defvalue.Split('|'))
                        {
                            string h = s.Substring(0, s.IndexOf("="));
                            string f = s.Substring(s.LastIndexOf("=") + 1);
                            if (value == h) value = f;
                        }
                    }
                    row.CreateCell(j).SetCellValue(value);
                }
            }
            //输出成Excel文件
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
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
                    Business.Do<IAccessory>().Delete(entity.Ol_UID, tran);
                    //先清理试题
                    tran.Delete<Questions>(Questions._.Ol_ID == entity.Ol_ID);
                    //清除学习记录
                    tran.Delete<LogForStudentStudy>(LogForStudentStudy._.Ol_ID == entity.Ol_ID);
                    tran.Delete<LogForStudentQuestions>(LogForStudentQuestions._.Ol_ID == entity.Ol_ID);
                    //删除章节
                    tran.Delete<Outline>(Outline._.Ol_ID == entity.Ol_ID);
                    tran.Commit();
                    //删除直播流
                    if (!string.IsNullOrWhiteSpace(entity.Ol_LiveID))
                    {
                        try
                        {
                            Pili.API<pili_sdk.IStream>().Delete(entity.Ol_LiveID);
                        }
                        catch
                        {
                        }
                    }
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
            //try
            //{
            //    //从缓存中读取
            //    List<Outline> list = WeiSha.Common.Cache<Outline>.Data.List;
            //    if (list == null || list.Count < 1) list = this.OutlineBuildCache();
            //    List<Outline> tm = (from l in list
            //                        where l.Ol_ID == identify
            //                        select l).ToList<Outline>();
            //    if (tm.Count > 0) curr = tm[0];
            //}
            //catch
            //{
            //}
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
            ////从缓存中读取
            //List<Outline> list = WeiSha.Common.Cache<Outline>.Data.List;
            //if (list == null || list.Count < 1) list = this.OutlineBuildCache();
            //List<Outline> tm = (from l in list
            //                    where l.Ol_UID == uid
            //                    select l).ToList<Outline>();
            //if (tm.Count > 0) curr = tm[0];
            if (curr == null) curr = Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == uid).ToFirst<Outline>();
            return curr;
        }
        /// <summary>
        /// 获取某个课程内的章节，按级别取
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="names">多级名称</param>
        /// <returns></returns>
        public Outline OutlineSingle(int couid, List<string> names)
        {
            Outline curr = null;
            int index = 0;
            while (curr != null && index >= names.Count)
            {
                curr = Gateway.Default.From<Outline>().
                    Where(Outline._.Cou_ID == couid && Outline._.Ol_Name == names[index])
                    .ToFirst<Outline>();
            }
            return curr;
        }
        /// <summary>
        /// 当前章节下的所有子章节id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> TreeID(int id)
        {
            List<int> ints = new List<int>();
            Outline ol = this.OutlineSingle(id);
            if (ol == null) return ints;
            //取同一个课程下的所有章节
            Outline[] ols = this.OutlineCount(ol.Cou_ID, -1, true, -1);

            ints = _treeid(id, ols);
            return ints;
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
                if (ol == null) ol = outlines[0];
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
            ////从缓存中读取
            //try
            //{
            //    List<Outline> list = WeiSha.Common.Cache<Outline>.Data.List;
            //    if (list == null || list.Count < 1) list = this.OutlineBuildCache();
            //    //linq查询
            //    var from = from l in list select l;
            //    if (couid > 0) from = from.Where<Outline>(p => p.Cou_ID == couid);
            //    if (isUse != null) from = from.Where<Outline>(p => p.Ol_IsUse == (bool)isUse);
            //    List<Outline> tm = from.OrderBy(c => c.Ol_Tax).ToList<Outline>();
            //    if (tm.Count > 0) return tm.ToArray<Outline>();
            //}
            //catch
            //{
            //}
            //orm查询
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(Outline._.Cou_ID == couid);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>();
        }
        #region 生成树形结构的章节列表
        /// <summary>
        /// 生成树形结构的章节列表
        /// </summary>
        /// <param name="outlines"></param>
        /// <returns></returns>
        public DataTable OutlineTree(Song.Entities.Outline[] outlines)
        {
            //计算树形的运算时间
            //DateTime beforDT = System.DateTime.Now;
            //WeiSha.Common.Log.Debug(this.GetType().Name, "---开始计算章节树形：" + beforDT.ToString("yyyy年MM月dd日 hh:mm:ss"));

            DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outlines);
            WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
            tree.IdKeyName = "OL_ID";
            tree.ParentIdKeyName = "OL_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);

            //DateTime afterDT = System.DateTime.Now;
            //TimeSpan ts = afterDT.Subtract(beforDT);
            //if (ts.TotalMilliseconds >= 500)
            //{
            //    //WeiSha.Common.Log.Debug(this.GetType().Name, string.Format("计算章节树形,耗时：{0}ms", ts.TotalMilliseconds));
            //}

            return  buildOutlineTree(dt, 0, 0, "");
        }
        /// <summary>
        /// 生成章节的等级序号
        /// </summary>
        /// <param name="outlines"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="level">层深</param>
        /// <param name="prefix">序号前缀</param>
        /// <returns></returns>
        private DataTable buildOutlineTree(DataTable outlines, int pid, int level, string prefix)
        {
            if (outlines == null) return null;
            int index = 1;
            foreach (DataRow ol in outlines.Rows)
            {
                if (Convert.ToInt32(ol["Ol_PID"]) == pid)
                {
                    ol["Ol_XPath"] = prefix + index.ToString() + ".";
                    ol["Ol_Level"] = level;
                    buildOutlineTree(outlines, Convert.ToInt32(ol["Ol_ID"]), level + 1, ol["Ol_XPath"].ToString());
                    index++;
                }
            }
            return outlines;
        }
        #endregion
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
        /// <summary>
        /// 清理无效章节
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        public int OutlineCleanup(int couid)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc &= Outline._.Cou_ID == couid;
            List<Song.Entities.Outline> outs = Gateway.Default.From<Outline>().Where(wc && Outline._.Ol_PID != 0).ToList<Outline>();
            int count = 0;
            foreach (Outline o in outs)
            {
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    try
                    {
                        int num = tran.Count<Outline>(Outline._.Ol_ID == o.Ol_PID);
                        if (num <= 0)
                        {
                            //先清理学习记录
                            tran.Delete<LogForStudentStudy>(LogForStudentStudy._.Ol_ID == o.Ol_ID);
                            tran.Delete<Outline>(Outline._.Ol_ID == o.Ol_ID);
                            tran.Commit();
                            count++;
                        }
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
            return count;
        }
        //private static object lock_cache_build = new object();
        ///// <summary>
        ///// 构建缓存
        ///// </summary>
        //public List<Outline> OutlineBuildCache()
        //{
        //    lock (lock_cache_build)
        //    {
        //        try
        //        {
        //            WeiSha.Common.Cache<Song.Entities.Outline>.Data.Clear();
        //        }
        //        catch
        //        {
        //        }
        //        finally
        //        {
        //            Song.Entities.Outline[] outls = Gateway.Default.From<Song.Entities.Outline>().ToArray<Outline>();
        //            WeiSha.Common.Cache<Song.Entities.Outline>.Data.Fill(outls);
        //        }
        //        ////计算每个章节下的试题数
        //        //foreach (Outline o in outls)
        //        //{
        //        //    o.Ol_QuesCount = this.QuesOfCount(o.Ol_ID, -1, true, true);
        //        //    Gateway.Default.Save<Outline>(o);
        //        //}                
        //        return WeiSha.Common.Cache<Outline>.Data.List;
        //    }
        //}
        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        public Outline[] OutlineCount(int couid, string search, bool? isUse, int count)
        {
            return OutlineCount(couid, null, search, isUse, count);
        }
        /// <summary>
        /// 直播中的章节
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Outline> OutlineLiving(int orgid, int count)
        {
            pili_sdk.pili.StreamList streams = null;
            try
            {
                streams = Business.Do<ILive>().StreamList(string.Empty, true, count);
            }
            catch
            {
                return null;
            }
            if (streams == null || streams.Streams.Count < 1) return null;
            List<Outline> list = new List<Outline>();
            
            foreach (pili_sdk.pili.Stream stream in streams.Streams)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc &= Outline._.Org_ID == orgid;
                Outline outline = Gateway.Default.From<Outline>().Where(
                    wc & Outline._.Ol_LiveID == stream.Title).ToFirst<Outline>();
                if (outline != null) 
                    list.Add(outline);
            }
            return list;
        }
        /// <summary>
        /// 获取指定个数的章节列表
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="islive">是否是直播章节</param>
        /// <param name="search"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Outline[] OutlineCount(int couid, bool? islive, string search, bool? isUse, int count)
        {
            WhereClip wc = Outline._.Cou_ID == couid;
            if (!string.IsNullOrWhiteSpace(search)) wc.And(Outline._.Ol_Name.Like("%" + search + "%"));
            if (islive != null) wc.And(Outline._.Ol_IsLive == (bool)islive);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToArray<Outline>(count);
        }
        public Outline[] OutlineCount(int couid, int pid, bool? isUse, int count)
        {
            //List<Outline> tm = null;
            //try
            //{
            //    //从缓存中读取
            //    List<Outline> list = WeiSha.Common.Cache<Outline>.Data.List;
            //    if (list == null || list.Count < 1) list = this.OutlineBuildCache();
            //    //linq查询
            //    var from = from l in list select l;
            //    if (couid > 0) from = from.Where<Outline>(p => p.Cou_ID == couid);
            //    if (pid >= 0) from = from.Where<Outline>(p => p.Ol_PID == pid);
            //    if (isUse != null) from = from.Where<Outline>(p => p.Ol_IsUse == (bool)isUse);

            //    if (count > 0)
            //    {
            //        tm = from.OrderBy(c => c.Ol_Tax).Take<Outline>(count).ToList<Outline>();
            //    }
            //    else
            //    {
            //        tm = from.OrderBy(c => c.Ol_Tax).ToList<Outline>();
            //    }
            //}
            //catch
            //{
            //}
            //if (tm != null && tm.Count > 0) return tm.ToArray<Outline>();
            //从数据库读取
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
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isVideo">是否有视频</param>
        /// <returns></returns>
        public int OutlineOfCount(int couid, int pid, bool? isUse, bool? isVideo, bool? isFinish)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc &= Outline._.Cou_ID == couid;
            if (pid > -1) wc &= Outline._.Ol_PID == pid;
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            if (isVideo != null) wc.And(Outline._.Ol_IsVideo == (bool)isVideo);
            if (isFinish != null) wc.And(Outline._.Ol_IsFinish == (bool)isFinish);
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
            WhereClip wc = Questions._.Qus_IsError == false && Questions._.Qus_IsWrong == false;      
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
                //if (!(sender is Outline)) return;
                //Outline ol = (Outline)sender;
                //Song.Entities.Outline old = this.OutlineSingle(ol.Ol_ID);
                //ol.Ol_QuesCount = Business.Do<IOutline>().QuesOfCount(ol.Ol_ID, -1, true, true);
                //WeiSha.Common.Cache<Song.Entities.Outline>.Data.Update(old, ol);
            }
            else
            {
                //填充章节数据到缓存
                //this.OutlineBuildCache();
            }
            if (Save != null)
                Save(sender, e);            
        }
        public void OnAdd(object sender, EventArgs e)
        {
            //当前对象（即sender），写入到缓存
            if (sender != null)
            {
                //if (!(sender is Outline)) return;
                //Outline ol = (Outline)sender;
                //WeiSha.Common.Cache<Song.Entities.Outline>.Data.Add(ol);
            }
            if (Add != null)
                Add(sender, e);
        }
        public void OnDelete(object sender, EventArgs e)
        {
            //当前对象（即sender），写入到缓存
            if (sender != null)
            {
                //if (!(sender is Outline)) return;
                //Outline ol = (Outline)sender;
                //WeiSha.Common.Cache<Song.Entities.Outline>.Data.Delete(ol);
                ////重建试题缓存（取所有试题进缓存）
                //new Thread(new ThreadStart(() =>
                //{
                //    Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, null, -1);
                //    Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Delete("all");
                //    Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Add(ques, int.MaxValue, "all");
                //})).Start();
            }
            if (Delete != null)
                Delete(sender, e);
        }
        #endregion
    }
}
