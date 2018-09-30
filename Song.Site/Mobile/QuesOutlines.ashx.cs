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
                //当前课程下的章节
                Song.Entities.Outline[] outlines = Business.Do<IOutline>().OutlineAll(couid, true);
                foreach (Song.Entities.Outline c in outlines)
                {
                    c.Ol_Intro = Extend.Html.ClearHTML(c.Ol_Intro);
                    //计算每个章节下的试题数
                    if (c.Ol_QuesCount <= 0)
                    {
                        c.Ol_QuesCount = Business.Do<IOutline>().QuesOfCount(c.Ol_ID, -1, true, true);
                        Business.Do<IOutline>().OutlineSave(c);
                    }
                }
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
            //当前章节试题数
            //this.Document.RegisterGlobalFunction(this.getQuesCount);
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
        /// <summary>
        ///// 获取试题数量
        ///// </summary>
        ///// <param name="paras"></param>
        ///// <returns></returns>
        //protected object getQuesCount(object[] paras)
        //{
        //    //课程id,章节id
        //    int couid=0, olid = 0;
        //    if (paras == null) return 0;
        //    if (paras.Length > 0 && paras[0]!=null)
        //        int.TryParse(paras[0].ToString(), out couid);
        //    if (paras.Length > 1 && paras[1] != null)
        //        int.TryParse(paras[1].ToString(), out olid);
        //    //是否取当前章节下子章节
        //    bool isAll = false;
        //    if (paras.Length > 1) isAll = paras[2].ToString().ToLower() == "all";
        //    int count = 0;
        //    if (couid == -1)
        //    {
        //        count = Business.Do<IOutline>().QuesOfCount(olid, -1, true, isAll);
        //    }
        //    else
        //    {
        //        count = Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, -1, -1, true);
        //    }
        //    return count;
        //}        
    }
}