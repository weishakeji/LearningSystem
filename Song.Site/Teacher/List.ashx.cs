using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Teacher
{
    /// <summary>
    /// 教师列表
    /// </summary>
    public class List : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //老师列表
            Tag thTag = this.Document.GetChildTagById("teachers");
            if (thTag != null)
            {
                int thSize = int.Parse(thTag.Attributes.GetValue("size", "4"));
                int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                index = index > 0 ? index : 1;
                int sum = 0;
                Song.Entities.Teacher[] teachers = Business.Do<ITeacher>().TeacherPager(this.Organ.Org_ID, -1, true, null, "", thSize, index, out sum);
                foreach (Song.Entities.Teacher c in teachers)
                {
                    c.Th_Photo = Upload.Get["Teacher"].Virtual + c.Th_Photo;
                }
                this.Document.SetValue("teachers", teachers);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)thSize);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
                this.Document.RegisterGlobalFunction(this.getCourse);
            }
        }
        /// <summary>
        /// 当前老师的课程
        /// </summary>
        /// <returns></returns>
        private List<Song.Entities.Course> getCourse(object[] id)
        {
            int thid = 0;
            if (id.Length > 0 && id[0] is int)
                thid = Convert.ToInt32(id[0]);
            List<Song.Entities.Course> list = new List<Entities.Course>();
            List<Song.Entities.Course> courses = Business.Do<ICourse>().CourseCount(-1, -1, thid, -1, null, true, 3);
            for (int i = 0; i < courses.Count; i++)
            {
                Song.Entities.Course c = courses[i];
                c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                list.Add(c);
            }
            return list;
        }
    }
}