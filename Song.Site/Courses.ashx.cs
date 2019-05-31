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
    /// 课程列表页
    /// </summary>
    public class Courses : BasePage
    {
        //上级专业id
        //private int pid = WeiSha.Common.Request.QueryString["pid"].Int32 ?? -1;
        //当前专业id
        private int sbjid = WeiSha.Common.Request.QueryString["sbjid"].Int32 ?? 0;
        //检索字符
        private string search = WeiSha.Common.Request.QueryString["search"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("sbjid", sbjid);
            //专业列表           
            Tag sbjTag = this.Document.GetChildTagById("subject");
            if (sbjTag != null)
            {
                Song.Entities.Subject[] subj = Business.Do<ISubject>().SubjectCount(Organ.Org_ID, null, true, sbjid, 0);
                this.Document.SetValue("subject", subj);
            }            
            //当前专业             
            Song.Entities.Subject sbj = Business.Do<ISubject>().SubjectSingle(sbjid);
            this.Document.SetValue("sbj", sbj);
            //上级专业
            if (sbj != null)
            {
                List<Song.Entities.Subject> sbjs = Business.Do<ISubject>().Parents(sbj.Sbj_ID, true);
                this.Document.Variables.SetValue("sbjs", sbjs);
            }
            //课程列表           
            Tag couTag = this.Document.GetChildTagById("course");
            if (couTag != null)
            {
                int size = int.Parse(couTag.Attributes.GetValue("size", "12"));
                int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                List<Song.Entities.Course> cour = Business.Do<ICourse>().CoursePager(Organ.Org_ID, sbjid, -1, true, search, "rec", size, index, out sum);
                foreach (Song.Entities.Course c in cour)
                {
                    c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                    c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
                    c.Cou_Intro = HTML.ClearTag(c.Cou_Intro);
                }
                this.Document.SetValue("course", cour);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)size);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
            }
        }
    }
}