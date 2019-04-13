using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Song.Site
{
    public partial class DesktopApp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext context = this.Context;
            //如果下载资料，则记录数加一
            string file = getFile();
            if (string.IsNullOrWhiteSpace(file))
            {
                context.Response.Write("客户端安装包不存在！");
            }
            else
            {
                //下载
                context.Response.Clear();
                context.Response.ClearContent();
                context.Response.ClearHeaders();
                context.Response.AddHeader("Content-Disposition", "attachment;filename=DesktopApp.exe");
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
        /// <summary>
        /// 获取下载的文件
        /// </summary>
        /// <returns></returns>
        private string getFile()
        {
            string path = this.Server.MapPath("/DesktopApp/");
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            string file = string.Empty;
            System.IO.FileInfo[] files = di.GetFiles("*.exe");
            if (files.Length > 0) file = files[0].FullName;
            return file;
        }

    }
}