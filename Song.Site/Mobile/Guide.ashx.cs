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

        protected override void InitPageTemplate(HttpContext context)
        {           
            //考试指南
            int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
            Song.Entities.Guide guide = Business.Do<IGuide>().GuideSingle(id);
            if ((WeiSha.Common.Request.Cookies["Guide_" + guide.Gu_Id].Int32 ?? 0) == 0)
            {
                guide.Gu_Number++;
                Business.Do<IGuide>().GuideSave(guide);
                context.Response.Cookies["Guide_" + guide.Gu_Id].Value = guide.Gu_Id.ToString();
            }
            this.Document.Variables.SetValue("guide", guide);           
            //当前新闻的上一条
            Song.Entities.Guide artPrev = Business.Do<IGuide>().GuidePrev(guide);
            this.Document.Variables.SetValue("artPrev", artPrev);
            //当前新闻的下一条
            Song.Entities.Guide artNext = Business.Do<IGuide>().GuideNext(guide);
            this.Document.Variables.SetValue("artNext", artNext);
        }
    }
}