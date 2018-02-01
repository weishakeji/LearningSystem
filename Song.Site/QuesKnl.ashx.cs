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
    /// 试题的知识库
    /// </summary>
    public class QuesKnl : BasePage
    {
        int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前试题
            Song.Entities.Questions qus = Business.Do<IQuestions>().QuesSingle(id);
            if (qus != null)
            {
                Song.Entities.Knowledge knl = Business.Do<IKnowledge>().KnowledgeSingle(qus.Kn_ID);
                this.Document.Variables.SetValue("knl", knl);
            }
        }
    }
}