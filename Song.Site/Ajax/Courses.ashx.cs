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
        int sbjid = WeiSha.Common.Request.QueryString["sbjid"].Int32 ?? -1;  //专业id
        string search = WeiSha.Common.Request.QueryString["search"].String; 
        string order = WeiSha.Common.Request.QueryString["order"].String;   //排序，flux流量最大优先,def推荐、流量，tax排序号，new最新,rec推荐
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int sum = 0;
            List<Song.Entities.Course> cour = Business.Do<ICourse>().CoursePager(org.Org_ID, sbjid, -1, true, search, order, size, index, out sum);
            string tm = "{\"sum\":" + sum + ",\"index\":" + index + ",\"size\":" + size + ",\"sbjid\":" + sbjid + ",\"object\":[";
            for (int i = 0; i < cour.Count; i++)
            {
                Song.Entities.Course c = cour[i];
                c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                //是否免费，或是限时免费
                if (c.Cou_IsLimitFree)
                {
                    DateTime freeEnd = c.Cou_FreeEnd.AddDays(1).Date;
                    if (!(c.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        c.Cou_IsLimitFree = false;
                }
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                tm += "" + c.ToJson();
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