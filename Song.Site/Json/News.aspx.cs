using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace Song.Site.Json
{
    public partial class News : System.Web.UI.Page
    {

        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //所属机构的所有课程
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Article[] eas = Business.Do<IContents>().ArticleCount(org.Org_ID,id, 15, "hot");
            string tm = "[";
            for (int i = 0; i < eas.Length; i++)
            {
                eas[i].Art_Details = "";
                tm += "" + eas[i].ToJson();
                if (i < eas.Length - 1) tm += ",";
            }
            tm += "]";
            Response.Write(tm);
            Response.End();
        }
    }
}
