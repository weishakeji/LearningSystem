using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 附件的管理
    /// </summary>
    public class Accessory : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 某一类的附件列表
        /// </summary>
        /// <param name="uid">这一类附件的uid</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<Song.Entities.Accessory> List(string uid, string type)
        {
            List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(uid,type);
            if (acs == null) return null;
            //资源的虚拟路径和物理路径
            string VirPath = WeiSha.Core.Upload.Get[type].Virtual;
            string PhyPath = WeiSha.Core.Upload.Get[type].Physics;
            foreach (Song.Entities.Accessory ac in acs)
            {
                if (System.IO.File.Exists(PhyPath + ac.As_FileName))
                    ac.As_FileName = VirPath + ac.As_FileName;
            }
            return acs;
        }

        /// <summary>
        /// 上传视频附件，其实已经上传到服务器，此处只是创建数据库记录
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <param name="type">附件关联主题的类型，例如新闻是News</param>
        /// <param name="fileinfo">文件信息</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public bool SaveOutlineVideoFile(int olid, string type, JObject fileinfo)
        {
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            if (outline == null)
            {
                throw new Exception("Not found entity for Outline！");
            }
            Business.Do<IAccessory>().Delete(outline.Ol_UID, type);
            //创建附件对象
            Song.Entities.Accessory acc = new Entities.Accessory();
            JObject jo = fileinfo;
            string filename = jo["filename"].ToString();
            acc.As_Name = jo["name"].ToString();
            acc.As_Type = type;
            acc.As_CrtTime = DateTime.Now;
            acc.As_Size = Convert.ToInt32(jo["size"].ToString());
            acc.As_Uid = outline.Ol_UID;
            acc.As_Extension = jo["ext"].ToString();
            acc.As_IsOther = false;
            acc.As_IsOuter = false;
            acc.As_FileName = filename;
            Business.Do<IAccessory>().Add(acc);
            return true;
        }
        /// <summary>
        /// 选择视频附件，其实已经上传到服务器，此处只是创建数据库记录
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <param name="type">附件关联主题的类型，例如新闻是News</param>
        /// <param name="fileinfo">文件信息</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public bool SelectOutlineVideoFile(int olid, string type, JObject fileinfo)
        {
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
            if (outline == null)
            {
                throw new Exception("Not found entity for Outline！");
            }
            //创建附件对象
            Song.Entities.Accessory acc = Business.Do<IAccessory>().GetSingle(outline.Ol_UID, "CourseVideo");
            if (acc == null) acc = new Entities.Accessory();
            JObject jo = fileinfo;
            string filename = jo["filename"].ToString();
            acc.As_Name = jo["name"].ToString();
            acc.As_Type = type;
            acc.As_CrtTime = DateTime.Now;
            acc.As_Size = Convert.ToInt32(jo["size"].ToString());
            acc.As_Uid = outline.Ol_UID;
            acc.As_Extension = jo["ext"].ToString();
            acc.As_IsOther = false;
            acc.As_IsOuter = false;
            acc.As_FileName = filename;
            if (acc.As_Id <= 0)
            {
                Business.Do<IAccessory>().Add(acc);
            }
            else
            {
                Business.Do<IAccessory>().Save(acc);
            }
            return true;
        }
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="uid">附件关联主题的uid，例如文章的Art_Uid</param>
        /// <param name="type">附件关联主题的类型，例如新闻是News</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        [Upload(Extension = "zip,rar,pdf,ppt,pptx,doc,docx,xls,xlsx", MaxSize = int.MaxValue, CannotEmpty = true)]
        public List<Song.Entities.Accessory> Upload(string uid, string type)
        {
            //资源的虚拟路径和物理路径
            string VirPath = WeiSha.Core.Upload.Get[type].Virtual;
            string PhyPath = WeiSha.Core.Upload.Get[type].Physics;

            List<Song.Entities.Accessory> list = new List<Song.Entities.Accessory>();
            if (this.Files.Count > 0)
            {
                foreach (string key in this.Files)
                {
                    Song.Entities.Accessory entity = new Song.Entities.Accessory();
                    HttpPostedFileBase file = this.Files[key];
                    string filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                    file.SaveAs(PhyPath + filename);
                    //
                    entity.As_Name = file.FileName;
                    entity.As_FileName = filename;
                    entity.As_Size = file.ContentLength;
                    entity.As_Uid = uid;
                    entity.As_Type = type;
                    Business.Do<IAccessory>().Add(entity);
                    list.Add(entity);
                }

            }

            return list;
        }
        /// <summary>
        /// 获取附件记录
        /// </summary>
        /// <param name="uid">附件关联对象的Uid，例如章节视频，此处为章节的uid</param>
        /// <returns></returns>
        public Song.Entities.Accessory ForUID(string uid,string type)
        {
            Song.Entities.Accessory entity = Business.Do<IAccessory>().GetSingle(uid, type);
            if (entity.As_Extension == "flv") entity.As_FileName = Path.ChangeExtension(entity.As_FileName, ".mp4");
            if (entity != null)
            {
                if (!entity.As_IsOther && !entity.As_IsOuter)
                {
                    //资源的虚拟路径和物理路径
                    string VirPath = WeiSha.Core.Upload.Get[entity.As_Type].Virtual;
                    string PhyPath = WeiSha.Core.Upload.Get[entity.As_Type].Physics;
                    if (System.IO.File.Exists(Path.Combine(PhyPath, entity.As_FileName)))
                    {
                        entity.As_FileName = Path.Combine(VirPath, entity.As_FileName);
                    }
                    else
                    {
                        entity.As_FileName = "";
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 修改附件信息
        /// </summary>
        /// <param name="entity">友情链接的分类</param>
        /// <returns></returns>
        [Admin,Teacher]
        [HttpPost]
        public bool Modify(Song.Entities.Accessory entity)
        {
            Song.Entities.Accessory old = Business.Do<IAccessory>().GetSingle(entity.As_Id);
            if (old == null) throw new Exception("Not found entity for Accessory！");

            old.Copy<Song.Entities.Accessory>(entity);
            Business.Do<IAccessory>().Save(old);
            return true;
        }
        /// <summary>
        /// 设置视频地址为其它视频网页的播放页
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <param name="outer">是否是外部视频</param>
        /// <param name="other">是否是其它平台的播放页</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        public bool SaveOtherVideo(string uid, string type, string url, bool outer, bool other)
        {
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(uid);
            if (outline == null)
            {
                throw new Exception("Not found entity for Outline！");
            }
            //创建附件对象
            Song.Entities.Accessory acc = Business.Do<IAccessory>().GetSingle(outline.Ol_UID, "CourseVideo");
            if (acc == null) acc = new Entities.Accessory();

            acc.As_FileName = url;
            acc.As_Name = outline.Ol_Name;
            acc.As_Type = type;
            acc.As_CrtTime = DateTime.Now;
            acc.As_Uid = outline.Ol_UID;
            acc.As_IsOther = other;
            acc.As_IsOuter = outer;
            if (acc.As_Id <= 0)
            {
                Business.Do<IAccessory>().Add(acc);
            }
            else
            {
                Business.Do<IAccessory>().Save(acc);
            }
            return true;
        }
        /// <summary>
        /// 设置视频地址为外部地址
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        public bool SaveOuterVideo(string uid, string type, string url)
        {
            Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(uid);
            if (outline == null)
            {
                throw new Exception("Not found entity for Outline！");
            }
            //创建附件对象
            Song.Entities.Accessory acc = Business.Do<IAccessory>().GetSingle(outline.Ol_UID, "CourseVideo");
            if (acc == null) acc = new Entities.Accessory();

            acc.As_FileName = url;
            acc.As_Name = outline.Ol_Name;
            acc.As_Type = type;
            acc.As_CrtTime = DateTime.Now;
            acc.As_Uid = outline.Ol_UID;
            acc.As_IsOther = false;
            acc.As_IsOuter = true;
            if (acc.As_Id <= 0)
            {
                Business.Do<IAccessory>().Add(acc);
            }
            else
            {
                Business.Do<IAccessory>().Save(acc);
            }
            return true;
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Admin, Teacher]
        public int DeleteForID(string id)
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
                    Business.Do<IAccessory>().Delete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpDelete]
        [Admin,Teacher]
        public bool DeleteForUID(string uid,string type)
        {
            Business.Do<IAccessory>().Delete(uid, type);
            return true;
        }
        #region 文件列表
        /// <summary>
        /// 获取上传的文件列表
        /// </summary>
        /// <param name="pathkey">路径的key,是web.config中Upload节点项的key值</param>
        /// <param name="search">按文件名检索</param>
        /// <param name="ext">后缀名，如mp4(不带点),只能写一个</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost,HttpGet]
        public ListResult PagerForFiles(string pathkey, string search, string ext, int index, int size)
        {
            //资源的虚拟路径和物理路径
            string virPath = WeiSha.Core.Upload.Get[pathkey].Virtual;
            string phyPath = WeiSha.Core.Upload.Get[pathkey].Physics;
            List<JObject> list = new List<JObject>();
            System.IO.DirectoryInfo dir = new DirectoryInfo(phyPath);
            System.IO.FileInfo[] files;
            if (string.IsNullOrWhiteSpace(search))
                files = dir.GetFiles("*." + ext);
            else
                files = dir.GetFiles("*" + search + "*." + ext);
            int sum = files.Length;
            for (int i = size * (index - 1); i < files.Length && i < size * index; i++)
            {
                System.IO.FileInfo f = files[i];
                JObject jo = new JObject();
                jo.Add("name", f.Name);
                jo.Add("size", f.Length);
                jo.Add("ext", f.Extension);
                jo.Add("fullname", Path.Combine(virPath, f.Name));
                jo.Add("crttime", f.CreationTime);
                jo.Add("lasttime", f.LastWriteTime);
                list.Add(jo);
            }
            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        #endregion
    }
}
