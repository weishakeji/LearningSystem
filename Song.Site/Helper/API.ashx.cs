using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using VTemplate.Engine;
using System.Web.SessionState;
namespace Song.Site.Helper
{
    /// <summary>
    /// Song.ViewData方法的调用说明
    /// </summary>
    public class API : BasePage, IRequiresSessionState
    {
           
        protected override void InitPageTemplate(HttpContext context)
        {
           
        }
      
    }
}