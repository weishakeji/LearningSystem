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
    /// 加载简答题的附件，用于刷新考试场景时
    /// </summary>
    public class ExamFileLoad : IHttpHandler
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
            string msg = string.Empty;
            string error = string.Empty;
            //上传文件
            foreach (FileInfo f in new DirectoryInfo(filepath).GetFiles())
            {
                if (f.Name.IndexOf("_") > 0)
                {
                    string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                    if (idtm == qid.ToString()) file = f.Name;
                }
            }
            string res = "{ 'filename':'" + file + "','qid':'" + qid + "'}";          
            context.Response.Write(res);
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