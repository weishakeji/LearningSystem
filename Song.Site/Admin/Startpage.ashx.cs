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
            //统计
            //课程数//教师数，学生数，试题数，试卷数
            this.Document.Variables.SetValue("couNum", Business.Do<ICourse>().CourseOfCount(Organ.Org_ID, -1, -1));            
            this.Document.Variables.SetValue("thNum", Business.Do<ITeacher>().TeacherOfCount(Organ.Org_ID, null));
            this.Document.Variables.SetValue("stNum", Business.Do<IAccounts>().AccountsOfCount(Organ.Org_ID, null));
            this.Document.Variables.SetValue("qusNum", Business.Do<IQuestions>().QuesOfCount(Organ.Org_ID,-1, -1,-1,-1,null));
            this.Document.Variables.SetValue("testNum", Business.Do<ITestPaper>().PagerOfCount(Organ.Org_ID, -1, -1, -1, null));
            //web模板列表
            this.Document.Variables.SetValue("tempweb",Business.Do<Song.ServiceInterfaces.ITemplate>().WebTemplates());
            //手机模板列表
            this.Document.Variables.SetValue("tempwap", Business.Do<Song.ServiceInterfaces.ITemplate>().MobiTemplates());
            //热门课程
            DataSet ds = Business.Do<ICourse>().CourseHot(this.Organ.Org_ID, -1, 20);
            this.Document.Variables.SetValue("hotcourse", ds.Tables[0]);

        }
    }
}