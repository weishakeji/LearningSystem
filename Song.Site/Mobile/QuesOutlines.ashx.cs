using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Data;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 试题练习的章节列表展示
    /// </summary>
    public class QuesOutlines : BasePage
    {
        //课程ID，章节id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //是否选学的当前课程
        bool isStudy = false;      
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");
            
            //当前选中的课程
            Song.Entities.Course course = Business.Do<ICourse>().CourseSingle(couid);         
            if (course != null)
            {
                //是否免费，或是限时免费
                if (course.Cou_IsLimitFree)
                {
                    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        course.Cou_IsLimitFree = false;
                }
                //是否学习当前课程
                isStudy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, couid);
                this.Document.Variables.SetValue("isStudy", isStudy);
                //是否免费，或是限时免费
                if (course.Cou_IsLimitFree)
                {
                    DateTime freeEnd = course.Cou_FreeEnd.AddDays(1).Date;
                    if (!(course.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))
                        course.Cou_IsLimitFree = false;
                }
                this.Document.SetValue("course", course);
                couid = course.Cou_ID;                            
                //当前课程下的章节
                Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);
                this.Document.SetValue("outlines", outlines);
                //树形章节输出
                if (outlines.Length > 0)
                    this.Document.Variables.SetValue("dtOutlines", Business.Do<IOutline>().OutlineTree(outlines));  
            }
            this.Document.SetValue("couid", couid);  
            //课程资源、课程视频资源的所在的路径
            this.Document.SetValue("path", Upload.Get["Course"].Virtual);
            this.Document.SetValue("vpath", Upload.Get["CourseVideo"].Virtual);
            //试题练习记录
            Song.Entities.LogForStudentQuestions log = Business.Do<ILogs>().QuestionSingle(this.Account.Ac_ID, couid, 0);
            this.Document.SetValue("log", log);
            //是否拥有子级
            this.Document.RegisterGlobalFunction(this.isChildren);
            this.Document.RegisterGlobalFunction(this.getChildren);            
        }
        /// <summary>
        /// 是否拥有子级
        /// </summary>
        /// <returns>0为没有子级，其它有子级</returns>
        protected object isChildren(object[] id)
        {
            int pid = 0;
            if (id.Length > 0 && id[0] is int)
                pid = Convert.ToInt32(id[0]);
            bool isChilid = Business.Do<IOutline>().OutlineIsChildren(couid, pid, true);
            return isChilid ? 0 : 1;
        }
        /// <summary>
        /// 获取当前章节的子级章节
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected object getChildren(object[] id)
        {
            int pid = 0;
            if (id.Length > 0 && id[0] is int)
                pid = Convert.ToInt32(id[0]);
            return Business.Do<IOutline>().OutlineChildren(couid, pid, true, 0);
        }       
    }
}