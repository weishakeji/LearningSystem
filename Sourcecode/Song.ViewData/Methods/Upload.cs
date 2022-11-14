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
        public JArray Files(string pathkey,long dataid)
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
    }
}
