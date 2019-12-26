using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site
{
    /// <summary>
    /// 课程交流咨询，此页面供直播客户端调用
    /// </summary>
    public class CourseChat : BasePage
    {       
        //int artid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            
        }
    }
}