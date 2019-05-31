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
                Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(knl.Cou_ID);
                //是否免费，或是限时免费
                if (course.Cou_IsLimitFree)
                {
                    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        course.Cou_IsLimitFree = false;
                }
                this.Document.Variables.SetValue("course", course);
                //是否购买课程（免费的也可以学习）
                bool isBuy = course.Cou_IsFree || course.Cou_IsLimitFree ? true : Business.Do<ICourse>().IsBuy(course.Cou_ID, this.Account.Ac_ID);

                //上级专业
                if (course != null)
                {
                    List<Song.Entities.Subject> sbjs = Business.Do<ISubject>().Parents(course.Sbj_ID, true);
                    this.Document.Variables.SetValue("sbjs", sbjs);
                }
            }
        }
    }
}