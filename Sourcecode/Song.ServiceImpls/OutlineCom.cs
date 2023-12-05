using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Core;
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
using System.Threading.Tasks;

namespace Song.ServiceImpls
{
    public class OutlineCom : IOutline
    {
        #region 课程章节管理
        /// <summary>
        /// 添加章节
        /// </summary>
        /// <param name="entity">业务实体</param>
        public Outline OutlineAdd(Outline entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    if (entity.Ol_ID <= 0)  entity.Ol_ID = WeiSha.Core.Request.SnowID();                
                    if (entity.Org_ID <= 0)
                    {
                        Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                        if (org != null) entity.Org_ID = org.Org_ID;
                    }
                    //所属专业
                    if (entity.Sbj_ID <= 0 && entity.Cou_ID > 0)
                    {
                        Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(entity.Cou_ID);
                        if (cou != null) entity.Sbj_ID = cou.Sbj_ID;
                    }
                    //计算排序号
                    object obj = tran.Max<Outline>(Outline._.Ol_Tax, Outline._.Cou_ID == entity.Cou_ID && Outline._.Ol_PID == entity.Ol_PID);
                    entity.Ol_Tax = obj is int ? (int)obj + 1 : 1;
                    //唯一id
                    entity.Ol_UID = WeiSha.Core.Request.SnowID().ToString();
                    //编辑时间
                    entity.Ol_CrtTime = DateTime.Now;
                    entity.Ol_ModifyTime = DateTime.Now;
                    ////层级
                    //entity.Ol_Level = _ClacLevel(entity);
                    //entity.Ol_XPath = _ClacXPath(entity);  
                    //如果是直播章节
                    if (entity.Ol_IsLive)
                    {
                        //string liveid = string.Format("{0}_{1}", entity.Cou_ID, entity.Ol_ID);
                        try
                        {
                            pili_sdk.pili.Stream stream = Business.Do<ILive>().StreamCreat(entity.Ol_ID.ToString());
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
                    this.BuildCache(entity.Cou_ID);
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
                return entity;
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
        public Outline OutlineBatchAdd(int orgid, long sbjid, long couid, string names)
        {
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);
            //整理名称信息
            names = names.Replace("，", ",");
            List<string> listName = new List<string>();
            foreach (string str in names.Split(','))
            {
                string s = str.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                if (s.Trim() != "") listName.Add(s.Trim());
            }
            //
            long pid = 0;
            Song.Entities.Outline last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.Outline current = OutlineIsExist(-1, -1, couid, pid, listName[i]);
                if (current == null)
                {
                    current = new Outline();
                    current.Ol_Name = listName[i];
                    current.Ol_ID = WeiSha.Core.Request.SnowID();
                    current.Ol_IsUse = true;        //默认为启用
                    current.Ol_IsChecked = true;
                    current.Org_ID = orgid;
                    current.Sbj_ID = course != null ? course.Sbj_ID : sbjid;
                    current.Cou_ID = couid;
                    current.Ol_PID = pid;
                    current.Ol_IsFinish = true;     //默认为完结
                    current.Ol_ModifyTime = DateTime.Now;
                    new Task(() => {
                        this.OutlineAdd(current);
                    }).Start();
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
        public Outline OutlineIsExist(int orgid, long sbjid, long couid, long pid, string name)
        {
            //从缓存中读取
            List<Outline> list = Cache.EntitiesCache.GetList<Outline>(couid);
            if (list == null || list.Count < 1) list = this.BuildCache(couid);
            //自定义查询条件
            Func<Outline, bool> exp = x =>
            {               
                var org_exp = orgid > 0 ? x.Org_ID == orgid : true;
                var sbj_exp = sbjid > 0 ? x.Sbj_ID == sbjid : true;
                var cou_exp = couid > 0 ? x.Cou_ID == couid : true;
                var pid_exp = pid > 0 ? x.Ol_PID == pid : true;
                var name_exp = !string.IsNullOrWhiteSpace(name) ? x.Ol_Name.Equals(name) : true;
                return org_exp && sbj_exp && cou_exp && pid_exp && name_exp;
            };
            return list.Where(exp).FirstOrDefault<Outline>();            
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
            //编辑时间
            //if (entity.Ol_ModifyTime < DateTime.Now.AddYears(-30))
                entity.Ol_ModifyTime = DateTime.Now;
            //如果有下级，设置为章节节点
            int childCount = Gateway.Default.Count<Outline>(Outline._.Ol_PID == entity.Ol_ID);
            entity.Ol_IsNode = childCount > 0;
            //如果有视频，设置视频节点
            int videoCount = Gateway.Default.Count<Accessory>(Accessory._.As_Type == "CourseVideo" && Accessory._.As_Uid==entity.Ol_UID);
            entity.Ol_IsVideo = videoCount > 0;
            //如果有附件
            int accCount = Gateway.Default.Count<Accessory>(Accessory._.As_Type == "Course" && Accessory._.As_Uid == entity.Ol_UID);
            entity.Ol_IsAccessory = accCount > 0;
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
                        stream = Business.Do<ILive>().StreamCreat(entity.Ol_ID.ToString());
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("直播流创建失败，" + ex.Message);
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
            this.BuildCache(entity.Cou_ID);
            Business.Do<ICourse>().IsLiveCourse(entity.Cou_ID, true);          
        }
        /// <summary>
        /// 修改课程的某些项
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool UpdateField(long olid, Field[] fiels, object[] objs)
        {
            try
            {
                Gateway.Default.Update<Outline>(fiels, objs, Outline._.Ol_ID == olid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新章节的试题数
        /// </summary>
        /// <param name="olid">章节Id</param>
        /// <param name="count">试题数</param>
        /// <returns></returns>
        public int UpdateQuesCount(long olid, int count)
        {
            Gateway.Default.Update<Outline>(Outline._.Ol_QuesCount, count, Outline._.Ol_ID == olid);
            new Task(()=> {
                Outline outline = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == olid).ToFirst<Outline>();
                if (outline != null)
                    this.BuildCache(outline.Cou_ID);
            }).Start();
            return count;
        }
        /// <summary>
        /// 导入章节，导入时不立即生成缓存
        /// </summary>
        /// <param name="entity"></param>
        public void OutlineInput(Outline entity)
        {
            new Task(() => {
                //计算排序号
                object obj = Gateway.Default.Max<Outline>(Outline._.Ol_Tax, Outline._.Cou_ID == entity.Cou_ID && Outline._.Ol_PID == entity.Ol_PID);
                entity.Ol_Tax = obj is int ? (int)obj + 1 : 1;
                if (string.IsNullOrWhiteSpace(entity.Ol_UID))
                    entity.Ol_UID = WeiSha.Core.Request.UniqueID();
                //层级
                entity.Ol_Level = _ClacLevel(entity);
                entity.Ol_XPath = _ClacXPath(entity);
                Gateway.Default.Save<Outline>(entity);
                this.BuildCache(entity.Cou_ID);
            }).Start();            
        }
        /// <summary>
        /// 导出课程章节到Excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        public string OutlineExport4Excel(string path, long couid)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Core.App.Get["ExcelInputConfig"].VirtualPath + "课程章节.xml";
            xmldoc.Load(WeiSha.Core.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //当前课程的所有章节
            List<Outline> outls = this.OutlineCount(couid, -1, null, -1);
            DataTable dt = WeiSha.Core.Tree.ObjectArrayToDataTable.To(outls.ToArray());
            WeiSha.Core.Tree.DataTableTree tree = new WeiSha.Core.Tree.DataTableTree();
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
                            value = tm > DateTime.Now.AddYears(-30) ? tm.ToString(format) : "";
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
        /// <param name="freshCache"></param>
        public void OutlineDelete(Outline entity, bool freshCache)
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
                    //刷新缓存
                    if(freshCache)
                        this.BuildCache(entity.Cou_ID);
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
        public void OutlineDelete(long identify)
        {
            Song.Entities.Outline ol = this.OutlineSingle(identify);
            this.OutlineDelete(ol, true);        
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Outline OutlineSingle(long identify)
        {
            return Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == identify).ToFirst<Outline>();
        }
        /// <summary>
        /// 获取单一实体对象，按唯一值，即UID；
        /// </summary>
        /// <param name="uid">全局唯一值</param>
        /// <returns></returns>
        public Outline OutlineSingle(string uid)
        {
            return Gateway.Default.From<Outline>().Where(Outline._.Ol_UID == uid).ToFirst<Outline>();
        }
        /// <summary>
        /// 获取某个课程内的章节，按级别取
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <param name="names">多级名称</param>
        /// <returns></returns>
        public Outline OutlineSingle(long couid, List<string> names)
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
        /// <param name="olid"></param>
        /// <returns></returns>
        public List<long> TreeID(long olid)
        {
            List<long> ints = new List<long>();
            Outline ol = this.OutlineSingle(olid);
            if (ol == null) return ints;
            //从缓存中读取
            List<Outline> list = Cache.EntitiesCache.GetList<Outline>(ol.Cou_ID);
            if (list == null || list.Count < 1) list = this.BuildCache(ol.Cou_ID);
            ints = _treeid(olid, list);
            return ints;
        }
        private List<long> _treeid(long id, List<Outline> ols)
        {
            List<long> list = new List<long>();
            if (id > 0) list.Add(id);
            foreach (Outline o in ols)
            {
                if (o.Ol_PID != id) continue;
                List<long> tm = _treeid(o.Ol_ID, ols);
                foreach (long t in tm)
                    list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 获取章节名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        public string OutlineName(long olid)
        {
            Outline entity = null;
            string xpath = string.Empty;
            do
            {
                entity = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == olid)
                    .Select(new Field[] { Outline._.Ol_ID, Outline._.Ol_PID, Outline._.Ol_Name }).ToFirst<Outline>();
                if (entity != null)
                {
                    if (string.IsNullOrWhiteSpace(xpath))
                        xpath = entity.Ol_Name;
                    else
                        xpath = entity.Ol_Name + "," + xpath;
                    olid = entity.Ol_PID;
                }
            } while (entity != null && olid > 0);
            return xpath;
        }
        /// <summary>
        /// 获取所有课程章节
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="use">是否启用</param>
        /// <param name="finish">章节是否完成</param>
        /// <param name="video">是否为视频章节</param>
        /// <returns></returns>
        public List<Outline> OutlineAll(long couid, bool? use, bool? finish, bool? video)
        {
            //从缓存中读取
            List<Outline> list = Cache.EntitiesCache.GetList<Outline>(couid);
            if (list == null || list.Count < 1) list = this.BuildCache(couid);
            //自定义查询条件
            Func<Outline, bool> exp = x =>
            {
                var use_exp = use != null ? x.Ol_IsUse == (bool)use : true;
                var finish_exp = finish != null ? x.Ol_IsFinish == (bool)finish : true;
                var video_exp = video != null ? x.Ol_IsVideo == (bool)video : true;
                return use_exp && finish_exp && video_exp;
            };
            List<Outline> result = list.Where(exp).ToList<Outline>();
            return result;           
        }

        /// <summary>
        /// 构建缓存，章节缓存以课程为单位存储
        /// </summary>
        public List<Outline> BuildCache(long couid)
        {
            Cache.EntitiesCache.Clear<Outline>(couid);
            List<Outline> list = Gateway.Default.From<Outline>().Where(Outline._.Cou_ID == couid)
                .OrderBy(Outline._.Ol_Tax.Asc).ToList<Outline>();
            foreach(Outline o in list)
            {
                if (!o.Ol_IsAccessory)
                {
                    //如果有视频，设置视频节点
                    int videoCount = Gateway.Default.Count<Accessory>(Accessory._.As_Type == "CourseVideo" && Accessory._.As_Uid == o.Ol_UID);
                    o.Ol_IsVideo = videoCount > 0;
                    //如果有附件
                    int accCount = Gateway.Default.Count<Accessory>(Accessory._.As_Type == "Course" && Accessory._.As_Uid == o.Ol_UID);
                    o.Ol_IsAccessory = accCount > 0;
                    if (videoCount > 0 || accCount > 0)
                    {
                        new Task(() =>
                        {
                            Gateway.Default.Save<Outline>(o);                         
                        }).Start();
                    }

                }
            }
            Cache.EntitiesCache.Save<Outline>(list, couid);
            return Cache.EntitiesCache.GetList<Outline>(couid);
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
            //WeiSha.Core.Log.Debug(this.GetType().Name, "---开始计算章节树形：" + beforDT.ToString("yyyy年MM月dd日 hh:mm:ss"));

            DataTable dt = WeiSha.Core.Tree.ObjectArrayToDataTable.To(outlines);
            WeiSha.Core.Tree.DataTableTree tree = new WeiSha.Core.Tree.DataTableTree();
            tree.IdKeyName = "OL_ID";
            tree.ParentIdKeyName = "OL_PID";
            tree.TaxKeyName = "Ol_Tax";
            tree.Root = 0;
            dt = tree.BuilderTree(dt);

            //DateTime afterDT = System.DateTime.Now;
            //TimeSpan ts = afterDT.Subtract(beforDT);
            //if (ts.TotalMilliseconds >= 500)
            //{
            //    //WeiSha.Core.Log.Debug(this.GetType().Name, string.Format("计算章节树形,耗时：{0}ms", ts.TotalMilliseconds));
            //}           
            foreach (DataRow ol in dt.Rows) ol["Ol_XPath"] = string.Empty;
            dt = buildOutlineTree(dt, 0, 0, "");
            return dt;            
        }
        /// <summary>
        /// 生成章节的等级序号
        /// </summary>
        /// <param name="outlines"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="level">层深</param>
        /// <param name="prefix">序号前缀</param>
        /// <returns></returns>
        private DataTable buildOutlineTree(DataTable outlines, long pid, int level, string prefix)
        {
            if (outlines == null) return null;
            int index = 1;
            foreach (DataRow ol in outlines.Rows)
            {
                if (Convert.ToInt64(ol["Ol_PID"]) == pid)
                {
                    ol["Ol_XPath"] = prefix + index.ToString() + ".";
                    ol["Ol_Level"] = level;
                    buildOutlineTree(outlines, Convert.ToInt64(ol["Ol_ID"]), level + 1, ol["Ol_XPath"].ToString());
                    index++;
                }
            }
            return outlines;
        }
        #endregion
        /// <summary>
        /// 清除章节下的试题、附件等
        /// </summary>
        /// <param name="olid"></param>
        public void OutlineClear(long olid)
        {         
            //清理附件
            Outline ol = this.OutlineSingle(olid);
            this.OutlineClear(ol, true);       
        }
        /// <summary>
        /// 清空章节下试题和附件
        /// </summary>
        /// <param name="entity">章节对象</param>
        /// <param name="freshCache"></param>
        public void OutlineClear(Outline entity, bool freshCache)
        {
            //清理试题
            Gateway.Default.Delete<Questions>(Questions._.Ol_ID == entity.Ol_ID);
            //清理附件
            Business.Do<IAccessory>().Delete(entity.Ol_UID, string.Empty);

            if (freshCache)
                this.BuildCache(entity.Cou_ID);
        }
        /// <summary>
        /// 清理无效章节
        /// </summary>
        /// <param name="couid">课程ID</param>
        /// <returns></returns>
        public int OutlineCleanup(long couid)
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
                        this.BuildCache(couid);
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

        /// <summary>
        /// 获取指定个数的课程列表
        /// </summary>
        /// <param name="couid">所属课程id</param>
        /// <param name="search"></param>
        /// <param name="isUse"></param>
        /// <param name="count">取多少条记录，如果小于等于0，则取所有</param>
        /// <returns></returns>
        public List<Outline> OutlineCount(long couid, string search, bool? isUse, int count)
        {
            return OutlineCount(couid, -1, null, search, isUse, count);
        }
        public List<Outline> OutlineCount(long couid, long pid, bool? isUse, int count)
        {
            return OutlineCount(couid, pid, null, string.Empty, isUse, count);
        }
        /// <summary>
        /// 获取指定个数的章节列表
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">章节父id</param>
        /// <param name="islive">是否是直播章节</param>
        /// <param name="search"></param>
        /// <param name="isUse"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Outline> OutlineCount(long couid, long pid, bool? islive, string search, bool? isUse, int count)
        {
            //从缓存中读取
            List<Outline> list = Cache.EntitiesCache.GetList<Outline>(couid);
            if (list == null || list.Count < 1) list = this.BuildCache(couid);
            //自定义查询条件
            Func<Outline, bool> exp = x =>
            {               
                var cou_exp = couid > 0 ? x.Cou_ID == couid : true;
                var pid_exp = pid > -1 ? x.Ol_PID == pid : true;

                var live_exp = islive != null ? x.Ol_IsLive == (bool)islive : true;
                var search_exp = !string.IsNullOrWhiteSpace(search) ? x.Ol_Name.Contains(search) : true;
                var use_exp = isUse != null ? x.Ol_IsUse == (bool)isUse : true;

                return cou_exp && pid_exp && live_exp && use_exp && search_exp;
            };
            if (count > 0)
            {
                IEnumerable<Outline> result = list.Where(exp).Take<Outline>(count);
                return result.ToList();
            }
            return list.Where(exp).ToList<Outline>();
        }
       
        public List<Outline> OutlineCount(int orgid, long sbjid, long couid, long pid, bool? islive, string search, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (pid >= 0) wc.And(Outline._.Ol_PID == pid);
            if (couid > 0) wc.And(Outline._.Cou_ID == couid);
            if (sbjid > 0) wc.And(Outline._.Sbj_ID == sbjid);
            if (orgid > 0) wc.And(Outline._.Org_ID == orgid);
            if (islive != null) wc.And(Outline._.Ol_IsLive == (bool)islive);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            if(!string.IsNullOrWhiteSpace(search)) wc.And(Outline._.Ol_Name.Like("%" + search + "%"));
            return Gateway.Default.From<Outline>().Where(wc).OrderBy(Outline._.Ol_Tax.Asc).ToList<Outline>(count);
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
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public int OutlineOfCount(long couid, long pid, bool? isUse)
        {
            return this.OutlineOfCount(couid, pid, isUse, null, null, null);
        }
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id<</param>
        /// <param name="pid"></param>
        /// <param name="isUse"></param>
        /// <param name="children">是否含下级</param>
        /// <returns></returns>
        public int OutlineOfCount(long couid, long pid, bool? isUse, bool children)
        {
            return this.OutlineOfCount(couid, pid, isUse, null, null, children);

            //包括子级，当前专业下的所有专业数
            List<long> list = new List<long>();
            //取同一个机构下的所有章节
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(Outline._.Cou_ID == couid);
            if (isUse != null) wc.And(Outline._.Ol_IsUse == (bool)isUse);
            //List<Outline> entities = Gateway.Default.From<Outline>().Where(wc).ToArray<Outline>();
            //list = _treeid(pid, entities);
            return list.Count;
        }
       
        /// <summary>
        /// 当前课程下的章节数
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isVideo">是否有视频</param>
        /// <returns></returns>
        public int OutlineOfCount(long couid, long pid, bool? isUse, bool? isVideo, bool? isFinish, bool? children)
        {
            //从缓存中读取
            List<Outline> list = Cache.EntitiesCache.GetList<Outline>(couid);
            if (list == null || list.Count < 1) list = this.BuildCache(couid);
            //自定义查询条件
            Func<Outline, bool> exp = x =>
            {            
                var cou_exp = couid > 0 ? x.Cou_ID == couid : true;
                var pid_exp = pid > 0 ? x.Ol_PID == pid : true;

                var video_exp = isVideo != null ? x.Ol_IsVideo == (bool)isVideo : true;
                var finish_exp = isFinish != null ? x.Ol_IsFinish == (bool)isFinish : true;
                var use_exp = isUse != null ? x.Ol_IsUse == (bool)isUse : true;

                return cou_exp && pid_exp && video_exp && finish_exp && use_exp;
            };
            List<Outline> result = list.Where(exp).ToList<Outline>();

            if(children!=null && !(bool)children)
            {
                List<long> count_list = new List<long>();
                count_list = _treeid(pid, result);
                return count_list.Count;
            }
            return result.Count;
        }
        /// <summary>
        /// 是否有子级章节
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="pid">父id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public bool OutlineIsChildren(long couid, long pid, bool? isUse)
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
        public bool OutlineIsQues(long olid, bool? isUse)
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
        public Outline[] OutlineChildren(long couid, long pid, bool? isUse,int count)
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
        public Outline[] OutlinePager(long couid, bool? isUse, string searTxt, int size, int index, out int countSum)
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
        public Questions[] QuesCount(long olid, int type, bool? isUse, int count)
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
        public int QuesOfCount(long olid, int type, bool? isUse, bool isAll)
        {
            WhereClip wc = Questions._.Qus_IsError == false && Questions._.Qus_IsWrong == false;      
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (olid > 0 && isAll)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = TreeID(olid);
                foreach (long l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);
            }
            return Gateway.Default.Count<Questions>(wc); 
        }
        /// <summary>
        /// 更改章节的排序
        /// </summary>
        /// <param name="list">专业列表，Ol_ID、Ol_PID、Ol_Tax、Ol_Level</param>
        /// <returns></returns>
        public bool UpdateTaxis(Outline[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.Outline sbj in list)
                    {
                        tran.Update<Outline>(
                            new Field[] { Outline._.Ol_PID, Outline._.Ol_Tax, Outline._.Ol_Level },
                            new object[] { sbj.Ol_PID, sbj.Ol_Tax, sbj.Ol_Level },
                            Outline._.Ol_ID == sbj.Ol_ID);
                    }
                    tran.Commit();
                    //this.BuildCache(couid);
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

        #region 试题统计
        /// <summary>
        /// 统计所有章节试题
        /// </summary>
        /// <returns></returns>
        public int StatisticalQuestion()
        {
            return StatisticalQuestion(new long[] { });
        }
        /// <summary>
        /// 统计某个章节的试题
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        public int StatisticalQuestion(long olid)
        {
            return StatisticalQuestion(new long[] { olid });
        }
        /// <summary>
        /// 统计指定章节的试题
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        public int StatisticalQuestion(long[] olid)
        {
            //更新章节试题数            
            string sql = @"select ol_id, COUNT(*) from Questions where {where} group by Ol_ID";
            if (olid.Length < 1)
            {
                sql = sql.Replace("{where}", "1=1");
            }
            else
            {
                string where = "";
                for(int i = 0; i < olid.Length; i++)
                {
                    where += " Ol_ID=" + olid[i];
                    if(i<olid.Length-1)
                        where += " or ";
                }
                sql = sql.Replace("{where}", where);
            }
            int num = 0;
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {
                while (reader.Read())
                {
                    //章节id
                    long id = reader.GetValue<long>(0);
                    if (id <= 0) continue;
                    //试题数
                    int count = reader.GetValue<int>(1);
                    //更新
                    Gateway.Default.Update<Outline>(Outline._.Ol_QuesCount, count, Outline._.Ol_ID == id);
                    num++;
                }
                reader.Close();
                reader.Dispose();

            }
            return num;
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
        public OutlineEvent[] EventAll(long couid, long olid, int type, bool? isUse)
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
        public OutlineEvent[] EventAll(long couid, string uid, int type, bool? isUse)
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
    }
}
