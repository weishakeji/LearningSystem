using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Song.Site.Manage.Utility.UploadPath
{
    /// <summary>
    /// 文件资源管理
    /// </summary>
    public partial class FileExplorer : Extend.CustomPage
    {
        //上传文件的配置项，参照web.config中的Platform/Upload节点
        string pathkey = WeiSha.Common.Request.QueryString["path"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            //当前文件夹
            ltCuurentPath.Text = WeiSha.Common.Upload.Get[pathkey].Virtual;
            //物理路径
            string path = WeiSha.Common.Upload.Get[pathkey].Physics;
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
            //下级文件夹
            System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
            rptDirs.DataSource = dirs;
            rptDirs.DataBind();
            //下级文件
            System.IO.FileInfo[] files = dir.GetFiles();
            rptFiles.DataSource = files;
            rptFiles.DataBind();
        }
    }
}