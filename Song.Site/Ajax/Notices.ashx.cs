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
    public class Notices : IHttpHandler
    {
        int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
        int size = WeiSha.Common.Request.QueryString["size"].Int32 ?? 1;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sum = 0;
            Song.Entities.Notice[] notice = Business.Do<INotice>().GetPager(org.Org_ID, true, "", size, index, out sum);
            string tm = "{\"sum\":"+sum+",\"object\":[";
            for (int i = 0; i < notice.Length; i++)
            {
                notice[i].No_Context = "";
                tm += "" + notice[i].ToJson();
                if (i < notice.Length - 1) tm += ",";
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