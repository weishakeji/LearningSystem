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
using WeiSha.WebControl;
namespace Song.Site.Manage
{
    public partial class ElementUI : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Page.Response.Cache.SetNoStore();
            //
            if (!this.IsPostBack)
            {
                ////是否记录操作日志
                //bool isWork = Business.Do<ISystemPara>()["SysIsWorkLogs"].Boolean ?? true;
                ////记录操作日志
                //if (isWork)
                //    Business.Do<ILogs>().AddOperateLogs();
            }
        }
    }
}
