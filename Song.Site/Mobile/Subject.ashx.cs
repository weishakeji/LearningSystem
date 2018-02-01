using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 专业列表展示
    /// </summary>
    public class Subject : BasePage
    {
        //专业ID
        protected int sbjid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //搜索字符
        protected string search = WeiSha.Common.Request.Form["search"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("search", search);
            //当前专业
            Song.Entities.Subject subject = Business.Do<ISubject>().SubjectSingle(sbjid);
            if (subject != null)
            {
                subject.Sbj_Logo = string.IsNullOrWhiteSpace(subject.Sbj_Logo) ? subject.Sbj_Logo : Upload.Get["Subject"].Virtual + subject.Sbj_Logo;
                subject.Sbj_LogoSmall = string.IsNullOrWhiteSpace(subject.Sbj_LogoSmall) ? subject.Sbj_LogoSmall : Upload.Get["Subject"].Virtual + subject.Sbj_LogoSmall;
                //是否有子级，如果没有，则直接跳到课程选择页
                if (isChildren(new object[]{subject.Sbj_ID}).ToString() != "0")
                {
                    this.Response.Redirect("Courses.ashx?sbjid=" + subject.Sbj_ID);
                }
            }
            this.Document.SetValue("subject", subject);
            //当前专业下的子专业，如果是顶级，则显示所有顶级专业
            Song.Entities.Subject[] subjects;
            if (string.IsNullOrWhiteSpace(search))
            {
                subjects = Business.Do<ISubject>().SubjectCount(this.Organ.Org_ID, null, true, sbjid, 0);
            }
            else
            {
                subjects = Business.Do<ISubject>().SubjectCount(this.Organ.Org_ID, search, true, -1, 0);
            }

            foreach (Song.Entities.Subject s in subjects)
            {
                s.Sbj_Logo = string.IsNullOrWhiteSpace(s.Sbj_Logo) ? s.Sbj_Logo : Upload.Get["Subject"].Virtual + s.Sbj_Logo;
                s.Sbj_LogoSmall = string.IsNullOrWhiteSpace(s.Sbj_LogoSmall) ? s.Sbj_LogoSmall : Upload.Get["Subject"].Virtual + s.Sbj_LogoSmall;
            }
            this.Document.SetValue("subjects", subjects);
            //是否拥有子级
            this.Document.RegisterGlobalFunction(this.isChildren);
        }
        /// <summary>
        /// 是否拥有子级
        /// </summary>
        /// <returns>0为没有子级，其它有子级</returns>
        protected object isChildren(object[] id)
        {
            int sbjid = 0;
            if (id.Length > 0 && id[0] is int)
                sbjid = Convert.ToInt32(id[0]);
            bool isChilid = Business.Do<ISubject>().SubjectIsChildren(this.Organ.Org_ID, sbjid, true);
            return isChilid ? 0 : 1;
        }
    }
}