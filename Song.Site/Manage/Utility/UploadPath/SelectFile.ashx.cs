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
    /// 选择文件
    /// </summary>
    public class SelectFile : IHttpHandler
    {        
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //上传路径
                string uploadPathKey = WeiSha.Common.Request.QueryString["path"].String;
                string path = WeiSha.Common.Upload.Get[uploadPathKey].Virtual;
                string uid = WeiSha.Common.Request.QueryString["uid"].String;   //全局唯一值
                string file = WeiSha.Common.Request.QueryString["file"].String;
                //添加附件
                Song.Entities.Accessory acc = new Song.Entities.Accessory();
                //视频操作对象                
                string videoFile = HttpContext.Current.Server.MapPath(path + file); //上传后的视频文件
                WeiSha.Common.VideoHandler handler = WeiSha.Common.VideoHandler.Hanlder(videoFile);
                if (handler.Width > 0) acc = setAcc(acc, handler);
                if (!string.IsNullOrWhiteSpace(uid))
                {
                    //参数
                    acc.As_Name = Path.GetFileName(file);
                    acc.As_FileName = file;
                    acc.As_Uid = uid;
                    acc.As_IsOuter = false;     //非外部链接
                    acc.As_Type = uploadPathKey;
                    Business.Do<IAccessory>().Delete(uid, false);
                    Business.Do<IAccessory>().Add(acc);
                }
            }
            catch
            {
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
            acc.As_Extension = System.IO.Path.GetExtension(acc.As_Name);
            acc.As_Size = (int)handler.Size;
            return acc;
        }
    }
}