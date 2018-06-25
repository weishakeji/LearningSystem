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

namespace Song.Site.Json
{
    /// <summary>
    /// 取当前时间
    /// </summary>
    public partial class Now : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(WeiSha.Common.Server.getTime());
            Response.End();
        }
    }
}
