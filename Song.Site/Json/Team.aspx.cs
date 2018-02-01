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

using Song.ServiceInterfaces;
using Song.Entities;
using System.Text;
using System.Reflection;
using WeiSha.Common;
namespace Song.Site.Json
{
    public partial class Team : System.Web.UI.Page
    {       
        //Ժϵid
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Team[] kn = null;
            kn = Business.Do<ITeam>().GetTeam(true, id, 0);
            string tm = "[";
            for (int i = 0; i < kn.Length; i++)
            {
                kn[i].Team_Intro = "";
                tm += "" + kn[i].ToJson();
                if (i < kn.Length - 1) tm += ",";
            }
            tm += "]";
            Response.Write(tm);
            Response.End();
        }
    }
}
