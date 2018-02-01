using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Content
{
    /// <summary>
    /// 记录下载的次数
    /// </summary>
    public class LoadNumber : IHttpHandler
    {
        #region IHttpHandler 成员

        public void ProcessRequest(HttpContext context)
        {            
            string filePath = context.Request.FilePath;
            if (filePath.IndexOf(".") > -1)
            {
                //如果是图片（下载信息的图片logo）
                if (filePath.Substring(filePath.LastIndexOf(".") + 1).ToLower() == "jpg")
                {
                    context.Response.WriteFile(context.Request.FilePath);
                }
                else
                {
                    //如果下载资料，则记录数加一
                    string file = filePath.Substring(filePath.LastIndexOf("/") + 1);
                    Business.Do<IContents>().DownloadNumber(file, 1);
                    //下载
                    context.Response.Clear();
                    context.Response.ClearContent();
                    context.Response.ClearHeaders();
                    context.Response.AddHeader("Content-Disposition", "attachment;filename=" + file);
                    System.IO.FileInfo fi = new System.IO.FileInfo(context.Server.MapPath(filePath));
                    context.Response.AddHeader("Content-Length", fi.Length.ToString());
                    context.Response.AddHeader("Content-Transfer-Encoding", "binary");
                    context.Response.ContentType = "application/octet-stream";
                    context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                    context.Response.WriteFile(filePath);
                    context.Response.Flush();
                    context.Response.End(); 
                }
            }            
        }

        public bool IsReusable
        {
            get { return true; } 
        }       

        #endregion
    }
}
