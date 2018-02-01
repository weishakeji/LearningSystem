using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Ajax
{
    /// <summary>
    /// Notices 的摘要说明
    /// </summary>
    public class News : IHttpHandler
    {
        int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
        int size = WeiSha.Common.Request.QueryString["size"].Int32 ?? 1;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sum = 0;
            Song.Entities.Article[] arts = Business.Do<IContents>().ArticlePager(org.Org_ID, -1, true, "", size, index, out sum);
            string tm = "{\"sum\":" + sum + ",\"index\":" + index + ",\"object\":[";
            for (int i = 0; i < arts.Length; i++)
            {
                arts[i].Art_Details = "";
                arts[i].Art_Intro = "";
                arts[i].Art_Title = arts[i].Art_Title.Replace("\"", "&quot;");
                tm += "" + arts[i].ToJson();
                if (i < arts.Length - 1) tm += ",";
            }
            tm += "]}";
            context.Response.Write(tm);
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