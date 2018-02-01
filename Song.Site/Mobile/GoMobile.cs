using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 记录网站访问量
    /// </summary>
    public class GoMobile : IHttpHandler
    {
        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return true; } 
        }

        public void ProcessRequest(HttpContext context)
        {
            if (WeiSha.Common.Browser.IsMobile)
            {
                context.Response.Redirect("~/Mobile/Index.aspx");
            }
            else
            {
                context.Response.WriteFile(context.Request.FilePath);
            }
                        
        }
        #endregion

    }
}
