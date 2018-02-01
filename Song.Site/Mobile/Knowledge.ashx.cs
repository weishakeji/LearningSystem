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

            Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();
            //上一条
            Song.Entities.Knowledge prev = Business.Do<IKnowledge>().KnowledgePrev(currCourse.Cou_ID, -1, id);
            this.Document.Variables.SetValue("prev", prev);
            //下一条
            Song.Entities.Knowledge next = Business.Do<IKnowledge>().KnowledgeNext(currCourse.Cou_ID, -1, id);
            this.Document.Variables.SetValue("next", next);
        }
    }
}