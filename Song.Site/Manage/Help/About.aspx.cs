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

namespace Song.Site.Manage.Help
{
    public partial class About : Extend.CustomPage
    {
        //∞Ê»®–≈œ¢
        protected WeiSha.Common.Copyright<string, string> copyright = WeiSha.Common.Request.Copyright;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
