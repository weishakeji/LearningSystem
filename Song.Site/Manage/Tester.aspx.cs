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
using System.Collections.Generic;
using System.IO;
using WeiSha.Common;


namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string path = this.Request.Url.PathAndQuery;

            WeiSha.Common.PageCache cache = WeiSha.Common.PageCache.Get[path];


            //txt = _replacePath(txt, "body|link|script|img");
            string a = "false";
            bool b=true;
            bool.TryParse(a,out b);
           


        }

    }
}
