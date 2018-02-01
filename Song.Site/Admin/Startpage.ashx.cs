using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Song.Extend;
using WeiSha.Common;
using Song.ServiceInterfaces;
using System.Data;

namespace Song.Site.Admin
{
    /// <summary>
    /// Startpage 的摘要说明
    /// </summary>
    public class Startpage : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //当前版本
            string version = WeiSha.Common.License.Value.VersionName;
            this.Document.Variables.SetValue("version", version);
            //当前版本的各项限制
            IDictionary<string, int> limit = WeiSha.Common.License.Value.LimitItems;
            this.Document.Variables.SetValue("limit", limit);
            //统计
            //课程数//教师数，学生数，试题数，试卷数
            this.Document.Variables.SetValue("couNum", Business.Do<ICourse>().CourseOfCount(Organ.Org_ID, -1, -1));            
            this.Document.Variables.SetValue("thNum", Business.Do<ITeacher>().TeacherOfCount(Organ.Org_ID, null));
            this.Document.Variables.SetValue("stNum", Business.Do<IAccounts>().AccountsOfCount(Organ.Org_ID, null));
            this.Document.Variables.SetValue("qusNum", Business.Do<IQuestions>().QuesOfCount(Organ.Org_ID,-1, -1,-1,-1,null));
            this.Document.Variables.SetValue("testNum", Business.Do<ITestPaper>().PagerOfCount(Organ.Org_ID, -1,-1, -1, null));
            this.Document.Variables.SetValue("knNum", Business.Do<IKnowledge>().KnowledgeOfCount(Organ.Org_ID, -1, null));
            //专业
            Song.Entities.Subject[] subject = Business.Do<ISubject>().SubjectCount(Organ.Org_ID, null, true, -1, 0);
            this.Document.Variables.SetValue("subject", subject);
            //各专业的课程
            string cusNumTxt = "";
            for (int i = 0; i < subject.Length;i++ )
            {
                int cus = Business.Do<ICourse>().CourseOfCount(Organ.Org_ID, subject[i].Sbj_ID, -1);
                cusNumTxt += cus;
                if (i < subject.Length - 1) cusNumTxt += ",";
            }
            this.Document.Variables.SetValue("cusNumTxt", cusNumTxt);
        }
    }
}