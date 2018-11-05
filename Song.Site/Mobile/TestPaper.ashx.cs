using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using Song.Entities;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 试卷详情
    /// </summary>
    public class TestPaper : BasePage
    {
        //考试id
        protected int tpid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //课程id
        int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("couid", couid); 
            //当前试卷
            Song.Entities.TestPaper paper = null;            
            paper = Business.Do<ITestPaper>().PagerSingle(tpid);
            if (paper != null)
            {
                paper.Tp_Logo = string.IsNullOrWhiteSpace(paper.Tp_Logo) ? paper.Tp_Logo : Upload.Get["TestPaper"].Virtual + paper.Tp_Logo;
                //判断Logo是否存在
                string hylogo = WeiSha.Common.Server.MapPath(paper.Tp_Logo);
                if (!System.IO.File.Exists(hylogo)) paper.Tp_Logo = string.Empty;
                this.Document.SetValue("pager", paper);
                //试卷所属课程
                Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(paper.Cou_ID);
                this.Document.SetValue("course", course);
            }
        }
    }
}