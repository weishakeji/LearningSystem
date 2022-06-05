using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Song.WebSite.Controllers
{
    /// <summary>
    /// 下载资源
    /// 1、所有供下载的资源全部在~/Download/文件夹，资源放置在action同名的文件夹下
    /// 2、如下载桌面应用，路径为/Down/deskapp/，实际下载文件为deskapp_（实际文件名).(扩展名)
    /// 3、其中deskapp为资源所在文件夹
    /// </summary>
    public class DownController : Controller
    {
        //资源下载
        public FileStreamResult Index()
        {
            //路由的id，作为要下载的文件名
            string id = this.RouteData.Values["id"] != null ? this.RouteData.Values["id"].ToString() : string.Empty;
            string path = Server.MapPath(string.Format("~/{0}/{1}/", "download", id));
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo fileinfo = null;
            string extensions = "*.exe,*.zip,*.rar";
            foreach(string ext in extensions.Split(','))
            {
                System.IO.FileInfo[] files = di.GetFiles(ext);
                if (files.Length > 0) fileinfo = files[0];
                if (fileinfo != null) break;
            }
            string filename = string.Format("{0}_{1}", id, fileinfo.Name);
            return File(new FileStream(fileinfo.FullName, FileMode.Open), "application/octet-stream", Server.UrlEncode(filename));
        }
    }
}