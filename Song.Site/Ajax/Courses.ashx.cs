using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Ajax
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Courses : IHttpHandler
    {
        int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
        int size = WeiSha.Common.Request.QueryString["size"].Int32 ?? 1;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sum = 0;
            List<Song.Entities.Course> cour = Business.Do<ICourse>().CoursePager(org.Org_ID, -1, -1, true, "", "def", size, index, out sum);
            string tm = "{\"sum\":" + sum + ",\"index\":" + index + ",\"object\":[";
            for (int i = 0; i < cour.Count; i++)
            {
                Song.Entities.Course c = cour[i];
                c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                //c.Cou_Intro = HTML.ClearTag(c.Cou_Intro);
                cour[i].Cou_Intro = "";
                cour[i].Cou_Name = cour[i].Cou_Name.Replace("\"", "&quot;");
                tm += "" + cour[i].ToJson();
                if (i < cour.Count - 1) tm += ",";
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