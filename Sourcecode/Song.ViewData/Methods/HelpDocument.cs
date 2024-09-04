using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Reflection;
using System.Xml;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using NPOI.XWPF.UserModel;
using System.IO;

namespace Song.ViewData.Methods
{

    /// <summary>
    /// 帮助说明文件的管理
    /// </summary> 
    [HttpPut, HttpGet]
    public class HelpDocument : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 判断某个文件是否存在
        /// </summary>
        /// <param name="file">文件名，带虚拟路径</param>
        /// <returns></returns>
        public bool FileExist(string file)
        {
            string fileHy = WeiSha.Core.Server.MapPath(file);
            return System.IO.File.Exists(fileHy);
        }
        /// <summary>
        /// 帮助文件的文本内容
        /// </summary>
        /// <param name="file">帮助文件</param>
        /// <returns></returns>
        public string FileContext(string file)
        {
            string fileHy = WeiSha.Core.Server.MapPath(file);
            if (!System.IO.File.Exists(fileHy))return  string.Empty;
            //return File.ReadAllText(fileHy, Encoding.UTF8);
            string context = string.Empty;
            using (StreamReader sr = new StreamReader(fileHy, Encoding.UTF8))
            {
                context = sr.ReadToEnd();                
                sr.Close();
                sr.Dispose();
            }
            return context;
        }
        /// <summary>
        /// 保存帮助文件
        /// </summary>
        /// <param name="file">帮助文件</param>
        /// <param name="context">文件内容</param>
        /// <returns></returns>
        [Admin]
        [HtmlClear(Not = "context")]
        public bool FileSave(string file,string context)
        {
            string fileHy = WeiSha.Core.Server.MapPath(file);
            //if (!System.IO.File.Exists(fileHy)) File.Create(fileHy);
            using (StreamWriter sw = new StreamWriter(fileHy, false, Encoding.UTF8))
            {
                sw.Write(context);
                sw.Close();
                sw.Dispose();
            }
            return true;
        }
    }
  
}
