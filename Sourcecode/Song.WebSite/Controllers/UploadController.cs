using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;

namespace Song.WebSite.Controllers
{
    /// <summary>
    /// 上传资源
    /// 1、上传的资源全部在~/upload/文件夹，具体配置看
    /// 2、如下载桌面应用，路径为/Down/deskapp/，实际下载文件为deskapp_（实际文件名).(扩展名)
    /// 3、其中deskapp为资源所在文件夹
    /// </summary>
    public class UploadController : Controller
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public ActionResult Image()
        {
            // 获取控制器、方法的名称，以及id值
            string ctr = this.RouteData.Values["controller"].ToString();
            string act = this.RouteData.Values["action"].ToString();
            string id = this.RouteData.Values["id"] != null ? this.RouteData.Values["id"].ToString() : string.Empty;
            //客户端参数
            int width = WeiSha.Core.Request.QueryString["width"].Int32 ?? 0;
            int height= WeiSha.Core.Request.QueryString["height"].Int32 ?? 0;
            bool tojpg = WeiSha.Core.Request.QueryString["tojpg"].Boolean ?? false;   //是否转换成jpg格式
            bool small= WeiSha.Core.Request.QueryString["small"].Boolean ?? false;    //是否生成缩略图
            string old = WeiSha.Core.Request.QueryString["old"].String;   //原图片，上传时会删它

            Hashtable ht = new Hashtable();
            string imgurl = string.Empty, fileName = string.Empty;
            foreach (string key in Request.Files)
            {
                //这里只测试上传第一张图片file[0]
                HttpPostedFileBase file0 = Request.Files[key];
                //转换成byte,读取图片MIME类型
                Stream stream;
                int size = file0.ContentLength / 1024; //文件大小KB
                if (size > 2048)
                {
                    ht.Add("state", "error");
                    ht.Add("msg", "图片不能超过2M！");
                    ht.Add("code", "-2");
                    ht.Add("url", "");
                    return Json(ht);
                }
                byte[] fileByte = new byte[2];//contentLength，这里我们只读取文件长度的前两位用于判断就好了，这样速度比较快，剩下的也用不到。
                stream = file0.InputStream;
                stream.Read(fileByte, 0, 2);//contentLength，还是取前两位
                //获取图片宽和高
                //System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                //int width = image.Width;
                //int height = image.Height;
                string fileFlag = "";
                if (fileByte != null && fileByte.Length > 0)//图片数据是否为空
                {
                    fileFlag = fileByte[0].ToString() + fileByte[1].ToString();
                }
                string[] fileTypeStr = { "255216", "7173", "6677", "13780" };//对应的图片格式jpg,gif,bmp,png
                fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + Request.Files[key].FileName;
                if (fileTypeStr.Contains(fileFlag))
                {
                    //文件保存信息
                    string path = WeiSha.Core.Upload.Get[id].Physics;  //存储路径                                   
                    Request.Files[key].SaveAs(path + fileName);
                    //最后输出存储的图片路径信息
                    imgurl = WeiSha.Core.Upload.Get[id].Virtual + fileName;
                    //裁切
                    if(width>0 && height > 0)
                    {
                        if (small)
                        {
                            string smallfile = WeiSha.Core.Images.Name.ToSmall(path + fileName);
                            WeiSha.Core.Images.FileTo.Thumbnail(path + fileName, smallfile, width, height, 0);
                        }
                    }
                    //转jpg
                    if (tojpg && !".jpg".Equals(Path.GetExtension(fileName),StringComparison.CurrentCultureIgnoreCase))
                    {                       
                        using (System.Drawing.Image image = WeiSha.Core.Images.FileTo.ToImage(path + fileName))
                        {
                            image.Save(path + Path.ChangeExtension(fileName, "jpg"), ImageFormat.Jpeg);
                        }
                        System.IO.File.Delete(path + fileName);
                        imgurl = WeiSha.Core.Upload.Get[id].Virtual + Path.ChangeExtension(fileName, "jpg");
                    }
                }
                else
                {
                    ht.Add("state", "error");
                    ht.Add("msg", "图片格式不正确！");
                    ht.Add("code", "-1");
                    ht.Add("url", "");
                    return Json(ht);
                }
                stream.Close();
            }
            ht.Clear();
            ht.Add("state", "success");
            ht.Add("url", imgurl);
            ht.Add("file", fileName);
            ht.Add("msg", "上传成功！");
            ht.Add("code", "0");
            //删除原图片
            if (!string.IsNullOrWhiteSpace(old))
            {
                foreach(string s in old.Split(','))
                {
                    string filehy = this.Server.MapPath(s);
                    try
                    {
                        //删除原图
                        if (System.IO.File.Exists(filehy))
                            System.IO.File.Delete(filehy);
                        //删除缩略图，如果有
                        string smallfile= WeiSha.Core.Images.Name.ToSmall(filehy);
                        if (System.IO.File.Exists(smallfile))
                            System.IO.File.Delete(smallfile);
                    }
                    catch { }
                }
            }
            return Json(ht);
        
        }
        /// <summary>
        /// 接收分片上传的文件
        /// </summary>
        /// <returns></returns>
        public ActionResult Chunked()
        {
            Hashtable ht = new Hashtable();
            // 获取控制器、方法的名称，以及id值
            string ctr = this.RouteData.Values["controller"].ToString();
            string act = this.RouteData.Values["action"].ToString();
            string id = this.RouteData.Values["id"] != null ? this.RouteData.Values["id"].ToString() : string.Empty;
            //客户端参数
            int total = Convert.ToInt32(this.Request["total"]);
            int index = Convert.ToInt32(this.Request["index"]);
            string name = this.Request["name"].ToString();
            string uid = this.Request["uid"].ToString();


            foreach (string key in Request.Files)
            {
                //这里只测试上传第一张图片file[0]
                HttpPostedFileBase file0 = Request.Files[key];
                int size = file0.ContentLength / 1024; //文件大小KB

                //文件保存信息
                string tempHy = WeiSha.Core.Upload.Get["Temp"].Physics;
                //string path = tempHy + uid;  //存储路径
                //if (!System.IO.Directory.Exists(path))
                //    System.IO.Directory.CreateDirectory(path);
                string file = Path.Combine(tempHy, index + "." + uid);
                Request.Files[key].SaveAs(file);
                //上传完成的数量
                int complete= System.IO.Directory.GetFiles(tempHy, "*." + uid).Length;
                if (complete == total)
                {
                    //最终合并后的文件
                    string final = Path.Combine(tempHy, name);
                    using (FileStream fs = new FileStream(final, FileMode.Create))
                    {
                        for (int i = 1; i <= total; i++)
                        {
                            string part = Path.Combine(tempHy, i + "." + uid);
                            byte[] bytes = System.IO.File.ReadAllBytes(part);
                            fs.Write(bytes, 0, bytes.Length);
                            bytes = null;
                            System.IO.File.Delete(part);
                        }
                        fs.Close();
                    }
                    //System.IO.Directory.Delete(path);
                }
            }
            return Json(ht);
        }
    }
}