using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 新闻
    /// </summary>
    [HttpGet]
    public class News : ViewMethod, IViewAPI
    {
        //课程资源的虚拟路径和物理路径
        public static string VirPath = WeiSha.Core.Upload.Get["News"].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get["News"].Physics;

        #region 栏目查询

        /// <summary>
        /// 新闻栏目
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Cache(Expires = 60)]
        public Song.Entities.Columns ColumnsForUID(string uid)
        {
            Song.Entities.Columns col = Business.Do<IColumns>().Single(uid);
            return col;
        }
        /// <summary>
        /// 用于前端显示的新闻栏目
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="pid">上级id</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Song.Entities.Columns[] ColumnsShow(int orgid,string pid,int count)
        {
            Song.Entities.Columns[] cols = Business.Do<IColumns>().ColumnCount(orgid, pid, "news", true, count);
            return cols;
        }
        /// <summary>
        /// 下级新闻栏目
        /// </summary>      
        /// <param name="pid">上级uid</param>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        public Song.Entities.Columns[] ColumnsChildren(string pid, bool isuse)
        {           
            return Business.Do<IColumns>().Children(pid, isuse);
        }
        #endregion

        #region 栏目管理
        /// <summary>
        /// 新闻的栏目，为树形数据
        /// </summary>
        /// <returns></returns>
        [HttpGet] 
        public JArray ColumnsTree(int orgid)
        {
            Song.Entities.Columns[] cols = Business.Do<IColumns>().ColumnCount(orgid, null, "news", null, -1);
            return cols.Length > 0 ? _columnsNode(null, cols.ToList<Song.Entities.Columns>()) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _columnsNode(Song.Entities.Columns item, List<Song.Entities.Columns> items)
        {
            JArray jarr = new JArray();

            foreach (Song.Entities.Columns m in items)
            {

                if (item == null)
                {
                    if (m.Col_PID != "" ) continue;
                }
                else
                {
                    if (m.Col_PID != item.Col_UID) continue;
                }             

                string j = m.ToJson("", "Col_CrtTime");

                JObject jo = JObject.Parse(j);
                jo.Add("id", "node_" + m.Col_UID.ToString());
                jo.Add("label", m.Col_Name);
                jarr.Add(jo);
                //计算下级
                JArray charray = _columnsNode(m, items);
                if (charray.Count > 0)
                    jo.Add("children", charray);              
            }
            return jarr;
        }

        #region 更新栏目
        /// <summary>
        /// 更新新闻栏目
        /// </summary>
        /// <param name="tree">来自客户端提交的json</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool ColumnsUpdate(string tree,int orgid)
        {
            List<Song.Entities.Columns> mlist = new List<Entities.Columns>();
            _ColumnsUpdate(tree, "", mlist);
            Business.Do<IColumns>().UpdateColumnsTree(mlist.ToArray(), orgid);
            return true;
        }
        private void _ColumnsUpdate(string tree, string pid, List<Song.Entities.Columns> mlist)
        {
            JArray jarr = JArray.Parse(tree);
            for (int i = 0; i < jarr.Count; i++)
            {
                string childJson = string.Empty;
                Song.Entities.Columns m = _ColumnsParse((JObject)jarr[i], out childJson);
                if (string.IsNullOrWhiteSpace(m.Col_UID))
                    m.Col_UID = WeiSha.Core.Request.UniqueID();
                m.Col_Tax = i;
                m.Col_PID = pid;
                mlist.Add(m);
                if (m.Col_IsChildren)
                {
                    _ColumnsUpdate(childJson, m.Col_UID, mlist);
                }
            }
        }
        private Song.Entities.Columns _ColumnsParse(JObject jo, out string childJson)
        {
            childJson = string.Empty;
            Song.Entities.Columns mm = new Entities.Columns();
            Type target = mm.GetType();
            IEnumerable<JProperty> properties = jo.Properties();
            foreach (JProperty item in properties)
            {
                string key = item.Name;
                string val = item.Value.ToString();

                PropertyInfo targetPP = target.GetProperty(key);
                if (targetPP != null)
                {
                    object tm = string.IsNullOrEmpty(val) ? null : WeiSha.Core.DataConvert.ChangeType(val.Trim(), targetPP.PropertyType);
                    targetPP.SetValue(mm, tm, null);
                }
                if (key.Equals("children", StringComparison.InvariantCultureIgnoreCase))
                {
                    childJson = item.Value.ToString();
                    mm.Col_IsChildren = childJson != "[]";
                }
            }
            return mm;
        }
        #endregion
        #endregion

        #region 文章查询
        /// <summary>
        /// 取若干新闻文章，包括栏目中的级下栏目
        /// </summary>
        /// <param name="uid">栏目uid</param>
        /// <param name="count">取多少条</param>
        /// <param name="order">获取类别，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <returns></returns>
        public Song.Entities.Article[] Articles(string uid, int count, string order)
        {
            Song.Entities.Article[] news = Business.Do<IContents>().ArticleCount(-1, uid, count, order);
            foreach (Song.Entities.Article art in news)
            {
                art.Art_Logo = System.IO.File.Exists(PhyPath + art.Art_Logo) ? VirPath + art.Art_Logo : "";
                if (string.IsNullOrWhiteSpace(art.Art_Intro))
                {
                    art.Art_Intro = WeiSha.Core.HTML.ClearTag(art.Art_Details, 200);
                }
            }
            return news;
        }
        /// <summary>
        /// 取若干新闻文章，包括栏目中的级下栏目
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="uid">栏目uid</param>
        /// <param name="count">取多少条</param>
        /// <param name="order">获取类别，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <returns></returns>
        public Song.Entities.Article[] ArticlesShow(int orgid,string uid, int count, string order)
        {
            Song.Entities.Article[] news = Business.Do<IContents>().ArticleCount(orgid, uid, count,true, order);
            foreach (Song.Entities.Article art in news)
            {
                art.Art_Logo = System.IO.File.Exists(PhyPath + art.Art_Logo) ? VirPath + art.Art_Logo : "";               
                if (string.IsNullOrWhiteSpace(art.Art_Intro))
                {
                    art.Art_Intro = WeiSha.Core.HTML.ClearTag(art.Art_Details, 200);
                }
            }
            return news;
        }
        /// <summary>
        /// 分页获取新闻内容(前端）
        /// </summary>
        /// <param name="uid">栏目uid</param>
        /// <param name="search"></param>
        /// <param name="order">排序方式，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ArticlePagerShow(string uid, string search, string order, int size, int index)
        {
            int count = 0;
            Song.Entities.Article[] news = Business.Do<IContents>().ArticlePager(-1, uid, search, null, false, order, size, index, out count);

            ListResult result = new ListResult(news);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 分页获取新闻内容(后端）
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="uid"></param>
        /// <param name="search">检索</param>
        /// <param name="verify">是否通过审核</param>
        /// <param name="del">是否被删除</param>
        /// <param name="order">排序方式，默认null取最新置顶的优先，hot热点优先，flux流量最大优先,img为图片新闻</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ArticlePager(int orgid, string uid, string search, bool? verify, bool? del, string order, int size, int index)
        {
            int count = 0;
            Song.Entities.Article[] news = Business.Do<IContents>().ArticlePager(orgid, uid, search, verify, del, order, size, index, out count);

            ListResult result = new ListResult(news);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 新闻文章
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        [Cache(Expires = 60 * 24, AdminDisable = true)]
        [HttpGet]
        public Song.Entities.Article Article(int id)
        {
            Song.Entities.Article art = Business.Do<IContents>().ArticleSingle(id);
            if (art == null) return null;
            art.Art_Logo = System.IO.File.Exists(PhyPath + art.Art_Logo) ? VirPath + art.Art_Logo : "";
            return art;
        }
        /// <summary>
        /// 新闻访问数加一
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public int VisitPlusOne(int id)
        {
            return Business.Do<IContents>().ArticleAddNumber(id, 1);
        }
        /// <summary>
        /// 新闻的附件
        /// </summary>
        /// <param name="uid">新闻文章的uid</param>
        /// <returns></returns>
        [Cache(Expires = 60 * 24)]
        [HttpGet]
        public Song.Entities.Accessory[] Accessory(string uid)
        {
            List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(uid);
            foreach (Song.Entities.Accessory ac in acs)
            {
                ac.As_FileName = System.IO.File.Exists(PhyPath + ac.As_FileName) ? VirPath + ac.As_FileName : "";
            }
            return acs.ToArray();
        }
        #endregion

        #region 文章管理
        ///<summary>
        /// 创建新闻文章
        /// </summary>
        /// <param name="entity">文章对象的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "entity")]
        public Song.Entities.Article Add(Song.Entities.Article entity)
        {
            try
            {
                string filename = string.Empty, smallfile = string.Empty;
                try
                {                 
                    //如果有上传文件
                    if (this.Files.Count > 0)
                    {
                        //只保存第一张图片
                        foreach (string key in this.Files)
                        {
                            HttpPostedFileBase file = this.Files[key];
                            filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                            file.SaveAs(PhyPath + filename);
                            break;
                        }
                        entity.Art_Logo = filename;                     
                    }
                    Business.Do<IContents>().ArticleAdd(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改新闻文章
        /// </summary>
        /// <param name="entity">文章对象的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "entity")]
        public Song.Entities.Article Modify(Song.Entities.Article entity)
        {
            try
            {
                string filename = string.Empty, smallfile = string.Empty;
                try
                {
                    Song.Entities.Article old = Business.Do<IContents>().ArticleSingle(entity.Art_Id);
                    if (old == null) throw new Exception("Not found entity for Article！");
                    //如果有上传文件
                    if (this.Files.Count > 0)
                    {
                        //只保存第一张图片
                        foreach (string key in this.Files)
                        {
                            HttpPostedFileBase file = this.Files[key];
                            filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                            file.SaveAs(PhyPath + filename);                          
                            break;
                        }
                        entity.Art_Logo = filename;                      
                        if (!string.IsNullOrWhiteSpace(old.Art_Logo))
                            WeiSha.Core.Upload.Get["News"].DeleteFile(old.Art_Logo);
                    }
                    //如果没有上传图片，且新对象没有图片，则删除旧图
                    else if (string.IsNullOrWhiteSpace(entity.Art_Logo))
                    {
                        WeiSha.Core.Upload.Get["News"].DeleteFile(old.Art_Logo);
                    }

                    //如果名称为空，则不修改
                    string exculde = "Art_CrtTime,Art_Number,Art_Uid,Org_ID,Org_Name";
                    if (string.IsNullOrWhiteSpace(entity.Art_Title))
                        exculde += ",Art_Title";                 
                    old.Copy<Song.Entities.Article>(entity, exculde);
                    Business.Do<IContents>().ArticleSave(old);
                    return old;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改文章的状态
        /// </summary>
        /// <param name="id">文章的id</param>
        /// <param name="use">是否启用</param>
        /// <param name="verify">是否审核通过</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool ArticleModifyState(int id, bool use, bool verify)
        {
            try
            {
                Business.Do<IContents>().ArticleUpdate(id,
                    new WeiSha.Data.Field[] {
                        Song.Entities.Article._.Art_IsUse,
                        Song.Entities.Article._.Art_IsVerify },
                    new object[] { use, verify });
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">课程id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int ArticleDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IContents>().ArticleDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        #endregion
    }
}
