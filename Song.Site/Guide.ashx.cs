using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
namespace Song.Site
{
    /// <summary>
    /// 指南详情页（现在是公告）
    /// </summary>
    public class Guide : BasePage
    {
        //知id
        int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            
            //知    
            Song.Entities.Guide knl = Business.Do<IGuide>().GuideSingle(id);
            this.Document.Variables.SetValue("knl", knl);
            if (knl != null)
            {
                Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(knl.Cou_ID);
                this.Document.Variables.SetValue("course", cou);
                //上级专业
                if (cou != null)
                {
                    List<Song.Entities.Subject> sbjs = Business.Do<ISubject>().Parents(cou.Sbj_ID, true);
                    this.Document.Variables.SetValue("sbjs", sbjs);
                }
            }
        }
    }
}