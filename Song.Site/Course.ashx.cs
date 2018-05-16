using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using Song.Entities;
namespace Song.Site
{
    /// <summary>
    /// 课程详情
    /// </summary>
    public class Course : BasePage
    {
        //课程章节
        Song.Entities.Outline[] outline;
        //是否购买当前课程
        bool isBuy = false;
        protected override void InitPageTemplate(HttpContext context)
        {            
            //当前课程信息
            int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
            Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(id);
            if (cou == null) return;
            if (cou != null)
            {
                if ((WeiSha.Common.Request.Cookies["Course_" + cou.Cou_ID].Int32 ?? 0) == 0)
                {
                    cou.Cou_ViewNum++;
                    Business.Do<ICourse>().CourseSave(cou);
                    context.Response.Cookies["Course_" + cou.Cou_ID].Value = cou.Cou_ID.ToString();
                }
                cou.Cou_Logo = string.IsNullOrWhiteSpace(cou.Cou_Logo) ? "" : Upload.Get["Course"].Virtual + cou.Cou_Logo;
                cou.Cou_LogoSmall = string.IsNullOrWhiteSpace(cou.Cou_LogoSmall) ? "" : Upload.Get["Course"].Virtual + cou.Cou_LogoSmall;
                this.Document.Variables.SetValue("course", cou);
            }
            //是否学习当前课程
            if (this.Account != null)
            {   
                //是否购买
                isBuy = Business.Do<ICourse>().StudyIsCourse(this.Account.Ac_ID, cou.Cou_ID);
                this.Document.Variables.SetValue("isStudy", isBuy);
                //是否在试用中
                bool istry = Business.Do<ICourse>().IsTryout(cou.Cou_ID, this.Account.Ac_ID);
                this.Document.Variables.SetValue("isTry", istry);
            }
            //课程章节列表
            outline = Business.Do<IOutline>().OutlineAll(cou.Cou_ID, true);
            this.Document.Variables.SetValue("Outline", outline);
            //树形章节输出
            this.Document.Variables.SetValue("olTree", buildOutlineHtml(outline, 0, 0, ""));
            //课程公告
            Song.Entities.Guide[] guides = Business.Do<IGuide>().GuideCount(-1, cou.Cou_ID, -1, 20);
            this.Document.Variables.SetValue("guides", guides); 
            //当前课程的主讲老师
            Song.Entities.Teacher teacher = Business.Do<ITeacher>().TeacherSingle(cou.Th_ID);
            if (teacher != null)
            {
                teacher.Th_Photo = Upload.Get["Teacher"].Virtual + teacher.Th_Photo;
                this.Document.Variables.SetValue("th", teacher);
            }
            //所有资源库的分类
            Song.Entities.KnowledgeSort[] ncs = Business.Do<IKnowledge>().GetSortAll(Organ.Org_ID, id, true);
            WeiSha.WebControl.MenuTree mt = new WeiSha.WebControl.MenuTree();
            mt.Title = "全部";
            mt.DataTextField = "Kns_Name";
            mt.IdKeyName = "Kns_Id";
            mt.ParentIdKeyName = "Kns_PID";
            mt.TaxKeyName = "Kns_Tax";
            mt.SourcePath = "/manage/Images/tree";
            //mt.TypeKeyName = "Kns_Type";
            mt.IsUseKeyName = "Kns_IsUse";
            //mt.IsShowKeyName = "Nc_IsShow";
            mt.DataSource = ncs;
            mt.DataBind();
            this.Document.Variables.SetValue("tree", mt.HTML); 
            //上级专业
            List<Subject> sbjs = Business.Do<ISubject>().Parents(cou.Sbj_ID, true);
            this.Document.Variables.SetValue("parentsbjs", sbjs);
            //当前课程的学员            
            Tag stTag = this.Document.GetChildTagById("students");
            if (stTag != null)
            {
                int count = int.Parse(stTag.Attributes.GetValue("count", "5"));
                Song.Entities.Accounts[] eas = null;
                eas = Business.Do<ICourse>().Student4Course(cou.Cou_ID, null, null, count, 1, out count);
                this.Document.SetValue("students", eas);
            }
        }
         /// <summary>
        /// 生成章节的多级结构html
        /// </summary>
        /// <param name="outline"></param>
        /// <param name="pid"></param>
        /// <param name="level"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private string buildOutlineHtml(Song.Entities.Outline[] outline,int pid, int level,string prefix)
        {
            int index = 1;
            string html = "";
            html += "<div class=\"outline" + (level > 0 ? " indent" : "") + "\" level=\"" + level + "\">";
            foreach (Song.Entities.Outline ol in outline)
            {
                if (ol.Ol_PID == pid)
                {
                    html += "<div class=\"olitem\" olid=\"" + ol.Ol_ID + "\">";
                    html += prefix + index.ToString() + ".";
                    if (isBuy)
                    {
                        html += "<a href=\"CourseStudy.ashx?id=" + ol.Ol_ID + "\">" + ol.Ol_Name + "</a>";
                    }
                    else
                    {
                        html += "<span>" + ol.Ol_Name + "</span>";
                    }
                    html += "</div>";
                    html += buildOutlineHtml(outline, ol.Ol_ID, ++level, prefix + index.ToString() + ".");
                    index++;
                }
            }
            html += "</div>";
            return html;
        }        
    }
}