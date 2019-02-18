using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.Common;

namespace Song.Site.Manage.Utility.UploadPath
{
    /// <summary>
    /// 上传操作
    /// </summary>
    public class Uploading : IHttpHandler
    {
        
        public void ProcessRequest(HttpContext context)
        {
            string serverFileName = "", path = "";
            try
            {               
                //上传路径
                string uploadPath = WeiSha.Common.Request.QueryString["path"].String;
                path = WeiSha.Common.Upload.Get[uploadPath].Virtual;
                //全局唯一值
                string uid = WeiSha.Common.Request.QueryString["uid"].String;
                HttpPostedFile file;
                for (int i = 0; i < context.Request.Files.Count; ++i)
                {
                    file = context.Request.Files[i];
                    if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName)) continue;
                    string ext = Path.GetExtension(file.FileName);
                    if (string.IsNullOrWhiteSpace(ext) || ext.ToLower() != ".mp4") continue;
                   
                    serverFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + ext;
                    //上传后的视频文件
                    string videoFile = HttpContext.Current.Server.MapPath(path + serverFileName);
                    file.SaveAs(videoFile);
                    if (!string.IsNullOrWhiteSpace(uid))
                    {
                        //添加附件
                        Song.Entities.Accessory acc = new Song.Entities.Accessory();
                        //视频操作对象
                        WeiSha.Common.VideoHandler handler = WeiSha.Common.VideoHandler.Hanlder(videoFile);
                        if (handler.Width > 0) acc = setAcc(acc, handler);
                        ////视频质量
                        //Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                        //WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
                        //int qscale = config["VideoConvertQscale"].Value.Int32 ?? 4;
                        ////先转为flv格式
                        //string flvFile = handler.Convert().ToFlv(qscale);
                        //handler = WeiSha.Common.VideoHandler.Hanlder(flvFile);
                        //if (handler.Width > 0) acc = setAcc(acc, handler);
                        //string mp4File = handler.Convert().ToMP4(qscale, true);
                        //if (acc.As_Width <= 0)
                        //{
                        //    handler = WeiSha.Common.VideoHandler.Hanlder(mp4File);
                        //    if (handler.Width > 0) acc = setAcc(acc, handler);
                        //}
                        //参数
                        acc.As_Name = Path.GetFileName(file.FileName);                        
                        //acc.As_FileName = System.IO.Path.ChangeExtension(serverFileName, ".flv");    
                        acc.As_FileName = serverFileName;
                        acc.As_Uid = uid;
                        acc.As_Extension = ext.Replace(".", ""); 
                        acc.As_Type = uploadPath;
                        //
                        handler = WeiSha.Common.VideoHandler.Hanlder(videoFile);
                        handler.Delete("flv,mp4");                        
                        Business.Do<IAccessory>().Add(acc);
                    }
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 700;
                context.Response.Write(ex.Message+" 详情请查看错误日志");
                //写入Log
                string log = context.Server.MapPath(path) + "errorlog.txt";
                using (System.IO.StreamWriter sw = new StreamWriter(log, true))
                {
                    sw.WriteLine("执行时间：" + DateTime.Now.ToString());
                    sw.WriteLine("错误信息：" + ex.Message);
                    sw.WriteLine("堆栈信息：" + ex.StackTrace);
                    sw.WriteLine("");
                    sw.Close();
                }
                context.Response.End();
            }
            finally
            {
                context.Response.Write(path + serverFileName);
                context.Response.End();
            }
        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 设置附件的各项参数
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        Song.Entities.Accessory setAcc(Song.Entities.Accessory acc, WeiSha.Common.VideoHandler handler)
        {
            //视频时长
            acc.As_Duration = (int)handler.Duration.TotalSeconds;
            acc.As_Duration = acc.As_Duration > 0 ? acc.As_Duration : 0;         
            acc.As_Width = handler.Width;
            acc.As_Height = handler.Height;                  
            acc.As_Size = (int)handler.Size;
            return acc;
        }
    }
}