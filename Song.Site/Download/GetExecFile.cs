using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Song.Site.Download
{
    public class GetExecFile
    {
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <returns></returns>
        public static string File()
        {
            HttpContext context = System.Web.HttpContext.Current;
            string pagename = WeiSha.Common.Request.Page.Name;
            string path = context.Server.MapPath("~/Download/" + pagename + "/");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            string file = string.Empty;
            System.IO.FileInfo[] files = di.GetFiles("*.exe");
            if (files.Length > 0) file = files[0].FullName;
            return file;
        }
        /// <summary>
        /// 将要下载的文件写入流
        /// </summary>
        /// <param name="file">文件物理路径</param>
        public static void Write(string file)
        {
            HttpContext context = System.Web.HttpContext.Current;
            if (string.IsNullOrWhiteSpace(file))
            {
                context.Response.Write("应用程序不存在！");
            }
            else
            {
                string pagename = WeiSha.Common.Request.Page.Name;
                //下载
                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.ClearHeaders();
                context.Response.AddHeader("Content-Disposition", "attachment;filename=" + pagename + ".exe");
                System.IO.FileInfo fi = new System.IO.FileInfo(file);
                context.Response.AddHeader("Content-Length", fi.Length.ToString());
                context.Response.AddHeader("Content-Transfer-Encoding", "binary");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.WriteFile(file);
                context.Response.Flush();
            }
            context.Response.End(); 
        }
    }
}