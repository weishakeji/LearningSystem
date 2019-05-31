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
    /// 知识库的详情
    /// </summary>
    public class Knowledge : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前知识
            int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
            Song.Entities.Knowledge kn = Business.Do<IKnowledge>().KnowledgeSingle(id);
            this.Document.Variables.SetValue("kn", kn);
            if (kn != null)
            {
                Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(kn.Cou_ID);
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
            }

            //上一条
            Song.Entities.Knowledge prev = Business.Do<IKnowledge>().KnowledgePrev(kn.Cou_ID, -1, id);
            this.Document.Variables.SetValue("prev", prev);
            //下一条
            Song.Entities.Knowledge next = Business.Do<IKnowledge>().KnowledgeNext(kn.Cou_ID, -1, id);
            this.Document.Variables.SetValue("next", next);
        }
    }
}