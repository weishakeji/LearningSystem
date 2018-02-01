using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;

namespace Song.Site.Manage.Questions
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Knowledge : IHttpHandler
    {
        //知识库分类id
        private int knsid = WeiSha.Common.Request.QueryString["knsid"].Int32 ?? 0;
        //课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        private int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
        private int size = WeiSha.Common.Request.QueryString["size"].Int32 ?? 10;
        private string sear = WeiSha.Common.Request.QueryString["sear"].String;
        public void ProcessRequest(HttpContext context)
        {
            //机构信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //总记录数
            int count = 0;
            Song.Entities.Knowledge[] kn = Business.Do<IKnowledge>().KnowledgePager(org.Org_ID, couid,knsid, true, null, null, null, sear,
                size, index, out count);
            string tm = "[";
            for (int i = 0; i < kn.Length; i++)
            {
                kn[i].Kn_Details = "";
                kn[i].Kn_Intro = "";
                tm += "" + kn[i].ToJson() + ",";
                //if (i < kn.Length - 1) tm += ",";
            }
            tm += "{\"SumCount\":" + count + "}";
            tm += "]";
            context.Response.ContentType = "text/plain";
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
