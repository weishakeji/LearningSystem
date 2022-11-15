using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.IO;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 上传文件管理，包括普通上传、大文件分上传、上传资源查看
    /// </summary>
    public class Upload : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 获取指定文件夹下的文件信息
        /// </summary>
        /// <param name="pathkey">路径配置项，来自web.config中的upload节点</param>
        /// <param name="dataid">数据id，为了保持资源的存放与数据对应，例如试题编辑时，试题相关联的图片等存放在试题id为命名的文件下，方便删除试题时一起删除相关资源</param>
        /// <returns></returns>
        public new JArray  Files(string pathkey,long dataid)
        {
            JArray arr = new JArray();
            if (string.IsNullOrWhiteSpace(pathkey)) return arr;
            string virPath = WeiSha.Core.Upload.Get[pathkey].Virtual + (dataid > 0 ? dataid.ToString()+"/" : "");
            string phyPath = WeiSha.Core.Upload.Get[pathkey].Physics + (dataid > 0 ? dataid.ToString() : "");
            if (!Directory.Exists(phyPath)) return arr;

            DirectoryInfo dir = new DirectoryInfo(phyPath);           
            foreach(FileInfo f in dir.GetFiles())
            {
                JObject jo = new JObject();
                jo.Add("name",f.Name);
                jo.Add("path", virPath);
                jo.Add("full", virPath + f.Name);
                jo.Add("ext", string.IsNullOrWhiteSpace(f.Extension) ? "" : f.Extension.Replace(".", ""));
                jo.Add("length", f.Length.ToString());
                jo.Add("size", _filesize_convert(f.Length));
                arr.Add(jo);
            }
            return arr;
        }
        /// <summary>
        /// 图片资源
        /// </summary>
        /// <param name="pathkey">路径配置项，来自web.config中的upload节点</param>
        /// <param name="dataid">数据id，</param>
        /// <returns></returns>
        public JArray Images(string pathkey, long dataid)
        {
            JArray arr = new JArray();
            if (string.IsNullOrWhiteSpace(pathkey)) return arr;
            string virPath = WeiSha.Core.Upload.Get[pathkey].Virtual + (dataid > 0 ? dataid.ToString() + "/" : "");
            string phyPath = WeiSha.Core.Upload.Get[pathkey].Physics + (dataid > 0 ? dataid.ToString() : "");
            if (!Directory.Exists(phyPath)) return arr;

            DirectoryInfo dir = new DirectoryInfo(phyPath);
            string[] exts = "jpg,jpeg,gif,png,bmp".Split(',');
            List<FileInfo> files = new List<FileInfo>();
            foreach (FileInfo f in dir.GetFiles())
            {
                string abbr = f.Name.IndexOf(".") > -1 ? f.Name.Substring(0, f.Name.LastIndexOf(".")) : f.Name;
                if (abbr.EndsWith("_small")) continue;
                foreach (string ext in exts)
                {
                    if (f.Extension.ToLower() == "." + ext)
                    {
                        files.Add(f);
                        break;
                    }
                }
            }
           
            foreach (FileInfo f in files)
            {
                JObject jo = new JObject();
                jo.Add("name", f.Name);
                jo.Add("path", virPath);
                jo.Add("full", virPath + f.Name);
                jo.Add("ext", string.IsNullOrWhiteSpace(f.Extension) ? "" : f.Extension.Replace(".", ""));
                jo.Add("length", f.Length.ToString());
                jo.Add("size", _filesize_convert(f.Length));
                //图片格式
                try
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(f.FullName))
                    {
                        jo.Add("width", image.Width);
                        jo.Add("height", image.Height);                       
                    }
                }
                catch { }
                arr.Add(jo);
            }
            return arr;
        }
        private string _filesize_convert(long length)
        {
            int KB = 1024;
            int MB = KB * 1024;
            int GB = MB * 1024; 
            if (length / GB >= 1)
                return Math.Round(length / (double)GB, 2) + " GB";
            else if (length / MB >= 1)
                return Math.Round(length / (float)MB, 2) + " MB";
            else if (length / KB >= 1)
                return Math.Round(length / (float)KB, 2) + " KB";
            else
                return length + " Byte";
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="pathkey"></param>
        /// <param name="dataid"></param>
        /// <param name="small">是否要生成缩略图，为true时生成</param>
        /// <param name="swidth">缩略图的宽度</param>
        /// <param name="sheight">缩略图的高度</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = true)]
        public JObject ImageSave(string pathkey, long dataid, bool small,int swidth,int sheight)
        {
            if (base.Files.Count < 1) return null;
            if (string.IsNullOrWhiteSpace(pathkey)) return null;

            string virPath = WeiSha.Core.Upload.Get[pathkey].Virtual + (dataid > 0 ? dataid.ToString() + "/" : "");
            string phyPath = WeiSha.Core.Upload.Get[pathkey].Physics + (dataid > 0 ? dataid.ToString() + "/" : "");
            if (!Directory.Exists(phyPath)) Directory.CreateDirectory(phyPath);
            string filename = string.Empty, smallfile = string.Empty;
            //只保存第一张图片
            foreach (string key in base.Files)
            {
                HttpPostedFileBase file = base.Files[key];
                filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                file.SaveAs(phyPath + filename);

                //生成缩略图
                if (small)
                {
                    smallfile = WeiSha.Core.Images.Name.ToSmall(filename);
                    WeiSha.Core.Images.FileTo.Thumbnail(phyPath + filename, phyPath + smallfile, swidth, sheight, 0);
                }
                break;
            }
            JObject jo = new JObject();
            System.IO.FileInfo f = new FileInfo(phyPath + filename);
            jo.Add("name", f.Name);
            jo.Add("path", virPath);
            jo.Add("full", virPath + f.Name);
            jo.Add("ext", string.IsNullOrWhiteSpace(f.Extension) ? "" : f.Extension.Replace(".", ""));
            jo.Add("length", f.Length.ToString());
            jo.Add("size", _filesize_convert(f.Length));
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(f.FullName))
            {
                jo.Add("width", image.Width);
                jo.Add("height", image.Height);
            }
            if (small) jo.Add("small", smallfile);
            return jo;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="pathkey"></param>
        /// <param name="dataid"></param>
        /// <param name="small">是否要生成缩略图，为true时生成</param>
        /// <param name="swidth">缩略图的宽度</param>
        /// <param name="sheight">缩略图的高度</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        public JObject ImageLoad(string url,string pathkey, long dataid, bool small, int swidth, int sheight)
        {
            if (string.IsNullOrWhiteSpace(pathkey)) return null;         

            string virPath = WeiSha.Core.Upload.Get[pathkey].Virtual + (dataid > 0 ? dataid.ToString() + "/" : "");
            string phyPath = WeiSha.Core.Upload.Get[pathkey].Physics + (dataid > 0 ? dataid.ToString() + "/" : "");
            if (!Directory.Exists(phyPath)) Directory.CreateDirectory(phyPath);

            string filename = string.Empty, smallfile = string.Empty;
            filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(url);

            try
            {

                WeiSha.Core.Request.LoadFile(url, phyPath + filename);
                //生成缩略图
                if (small)
                {
                    smallfile = WeiSha.Core.Images.Name.ToSmall(filename);
                    WeiSha.Core.Images.FileTo.Thumbnail(phyPath + filename, phyPath + smallfile, swidth, sheight, 0);
                }
                JObject jo = new JObject();
                System.IO.FileInfo f = new FileInfo(phyPath + filename);
                jo.Add("name", f.Name);
                jo.Add("path", virPath);
                jo.Add("full", virPath + f.Name);
                jo.Add("ext", string.IsNullOrWhiteSpace(f.Extension) ? "" : f.Extension.Replace(".", ""));
                jo.Add("length", f.Length.ToString());
                jo.Add("size", _filesize_convert(f.Length));
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(f.FullName))
                {
                    jo.Add("width", image.Width);
                    jo.Add("height", image.Height);
                }
                if (small) jo.Add("small", smallfile);
                return jo;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">文件所在完整虚拟路径</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpDelete, HttpGet(Ignore = true)]
        public bool Delete(string file)
        {
            string phy = this.Context.Server.MapPath(file);
            if (File.Exists(phy))
                File.Delete(phy);
            else
                throw new Exception("文件不存在");
            return true;
        }
    }
}
