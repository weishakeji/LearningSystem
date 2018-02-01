using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Utility
{
    /// <summary>
    /// 考试时，简答题附件上传
    /// </summary>
    public class ExamFileUp : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //学员id，考试id
            int stid = WeiSha.Common.Request.QueryString["stid"].Int32 ?? 0;
            int examid = WeiSha.Common.Request.QueryString["examid"].Int32 ?? 0;
            int qid = WeiSha.Common.Request.QueryString["qid"].Int32 ?? 0; 

            HttpFileCollection files = context.Request.Files;//这里只能用<input type="file" />才能有效果,因为服务器控件是HttpInputFile类型
            string msg = string.Empty;
            string error = string.Empty;
            string filename;
            if (files.Count > 0 && stid > 0)
            {
                filename = System.IO.Path.GetFileName(files[0].FileName);    
                //判断是否是允许的文件类型
                bool isAllow = false;   //是否允许
                string ext = string.Empty;  //上传的扩展名
                string[] fileallow = WeiSha.Common.App.Get["ExamFileUp"].Split(',');
                if (files[0].FileName.IndexOf('.') > -1)                
                    ext = files[0].FileName.Substring(files[0].FileName.LastIndexOf('.') + 1).ToLower();
                foreach (string s in fileallow)
                {
                    if (string.IsNullOrWhiteSpace(s) || s.Trim() == "") continue;
                    if (s.ToLower() == ext)
                    {
                        isAllow = true;
                        break;
                    }
                }
                if (!isAllow)
                {
                    context.Response.Write(json("上传文件仅限" + string.Join("、", fileallow) + "格式", "上传失败", filename, qid));
                    context.Response.End();
                    return;
                }
                
                //文件所在路径
                string filepath = Upload.Get["Exam"].Physics + examid + "\\" + stid + "\\";
                if (!System.IO.Directory.Exists(filepath))
                    System.IO.Directory.CreateDirectory(filepath);
                //文件名
                string file = qid + "_" + System.IO.Path.GetFileName(files[0].FileName);
                //上传文件
                foreach (FileInfo f in new DirectoryInfo(filepath).GetFiles())
                {
                    if (f.Name.IndexOf("_") > 0)
                    {
                        string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                        if (idtm == qid.ToString()) f.Delete();
                    }
                }
                files[0].SaveAs(filepath + file);
                msg = " 成功! 文件大小为:" + files[0].ContentLength;                           
                context.Response.Write(json(error,msg,filename,qid));
                context.Response.End();
            }
        }
        /// <summary>
        /// 返回Json字符串
        /// </summary>
        /// <param name="error"></param>
        /// <param name="msg"></param>
        /// <param name="file"></param>
        /// <param name="qid"></param>
        /// <returns></returns>
        private string json(string error, string msg, string file, int qid)
        {
            string res = "{ error:'" + error + "', msg:'" + msg + "',filename:'" + file + "',qid:'" + qid + "'}";
            return res;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}