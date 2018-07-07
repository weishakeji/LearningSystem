using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 通知公告详细页
    /// </summary>
    public class Guide : BasePage
    {
        //当前公告所属课程的id
        public int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //公告id
        public int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {             
            Song.Entities.Guide guide = Business.Do<IGuide>().GuideSingle(id);
            if ((WeiSha.Common.Request.Cookies["Guide_" + guide.Gu_Id].Int32 ?? 0) == 0)
            {
                guide.Gu_Number++;
                Business.Do<IGuide>().GuideSave(guide);
                context.Response.Cookies["Guide_" + guide.Gu_Id].Value = guide.Gu_Id.ToString();
            }
            this.Document.Variables.SetValue("guide", guide);           
            
        }
    }
}