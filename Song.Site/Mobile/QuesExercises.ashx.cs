using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 试题练习
    /// </summary>
    public class QuesExercises : BasePage
    {
        //章节id
        protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //试题的启始索引与当前取多少条记录       
        protected int count = WeiSha.Common.Request.QueryString["count"].Int32 ?? 100;       
        protected override void InitPageTemplate(HttpContext context)
        {
            //题型
            this.Document.SetValue("quesType", WeiSha.Common.App.Get["QuesType"].Split(','));
            this.Document.SetValue("couid", couid);            
        }
    }
}