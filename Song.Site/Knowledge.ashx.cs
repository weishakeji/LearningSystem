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
    /// 知识库详情页
    /// </summary>
    public class Knowledge : BasePage
    {
        //知识库的id
        int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            
            //知识       
            Song.Entities.Knowledge knl = Business.Do<IKnowledge>().KnowledgeSingle(id);
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