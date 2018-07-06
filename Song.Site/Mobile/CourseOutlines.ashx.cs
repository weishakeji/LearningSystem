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
    /// 课程学习的章节列表展示
    /// </summary>
    public class CourseOutlines : BasePage
    {
        //课程ID，章节id
        protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //protected int olid = WeiSha.Common.Request.QueryString["olid"].Int32 ?? 0;       
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx");           
            //当前选中的课程
            Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();            
            if (currCourse != null)
            {                
                this.Document.SetValue("currCourse", currCourse);
                couid = currCourse.Cou_ID;
                this.Document.SetValue("couid", couid);               
                //当前课程下的章节
                Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);                  
                foreach (Song.Entities.Outline c in outlines)
                    c.Ol_Intro = Extend.Html.ClearHTML(c.Ol_Intro);
                this.Document.SetValue("outlines", outlines);
                //
                DataTable dt = WeiSha.WebControl.Tree.ObjectArrayToDataTable.To(outlines);
                WeiSha.WebControl.Tree.DataTableTree tree = new WeiSha.WebControl.Tree.DataTableTree();
                tree.IdKeyName = "OL_ID";
                tree.ParentIdKeyName = "OL_PID";
                tree.TaxKeyName = "Ol_Tax";
                tree.Root = 0;
                dt = tree.BuilderTree(dt);
                this.Document.Variables.SetValue("dtOutlines", dt);  
            }
            //课程资源、课程视频资源的所在的路径
            this.Document.SetValue("path", Upload.Get["Course"].Virtual);
            this.Document.SetValue("vpath", Upload.Get["CourseVideo"].Virtual);
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