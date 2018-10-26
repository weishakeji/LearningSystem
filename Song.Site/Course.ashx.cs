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
                //图片路径
                cou.Cou_Logo = string.IsNullOrWhiteSpace(cou.Cou_Logo) ? "" : Upload.Get["Course"].Virtual + cou.Cou_Logo;
                cou.Cou_LogoSmall = string.IsNullOrWhiteSpace(cou.Cou_LogoSmall) ? "" : Upload.Get["Course"].Virtual + cou.Cou_LogoSmall;
                //是否免费，或是限时免费
                if (cou.Cou_IsLimitFree)
                {
                    DateTime freeEnd = cou.Cou_FreeEnd.AddDays(1).Date;
                    if (!(cou.Cou_FreeStart <= DateTime.Now && freeEnd >= DateTime.Now))                       
                        cou.Cou_IsLimitFree = false;                   
                }
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
            this.Document.Variables.SetValue("olTree", buildOutlineTree(outline, 0, 0, ""));
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
        /// 生成章节的等级序号
        /// </summary>
        /// <param name="outlines"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="level">层深</param>
        /// <param name="prefix">序号前缀</param>
        /// <returns></returns>
        private Song.Entities.Outline[] buildOutlineTree(Song.Entities.Outline[] outlines, int pid, int level, string prefix)
        {
            int index = 1;           
            foreach (Song.Entities.Outline ol in outlines)
            {
                if (ol.Ol_PID == pid)
                {                   
                    ol.Ol_XPath = prefix + index.ToString() + ".";
                    ol.Ol_Level = level;
                    buildOutlineTree(outlines, ol.Ol_ID, level + 1, ol.Ol_XPath);
                    index++;
                }
            }
            return outlines;
        }
    }
}