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
    /// 考试时，简答题附件的删除
    /// </summary>
    public class ExamFileDel : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //学员id，考试id
            int stid = WeiSha.Common.Request.QueryString["stid"].Int32 ?? 0;
            int examid = WeiSha.Common.Request.QueryString["examid"].Int32 ?? 0;
            int qid = WeiSha.Common.Request.QueryString["qid"].Int32 ?? 0;
            //文件所在路径
            string filepath = Upload.Get["Exam"].Physics + examid + "\\" + stid + "\\";
            if (!System.IO.Directory.Exists(filepath))
                System.IO.Directory.CreateDirectory(filepath);
            //文件名
            string file = "";
            foreach (FileInfo f in new DirectoryInfo(filepath).GetFiles())
            {
                if (f.Name.IndexOf("_") > 0)
                {
                    string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                    if (idtm == qid.ToString()) file = f.FullName;
                }
            }
            if (!string.IsNullOrWhiteSpace(file))
            {
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            context.Response.Write("0");
            context.Response.End();

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