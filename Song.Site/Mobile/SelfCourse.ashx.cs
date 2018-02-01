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
    /// 我的课程
    /// </summary>
    public class SelfCourse : BasePage
    {
        ////当前课程id
        //protected int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;       
        protected override void InitPageTemplate(HttpContext context)
        {
            if (!Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("login.ashx?" + context.Request.QueryString);
            //课程资源、课程视频资源的所在的路径
            this.Document.SetValue("path", Upload.Get["Course"].Virtual);
            this.Document.SetValue("vpath", Upload.Get["CourseVideo"].Virtual);
            //当前课程
            Song.Entities.Course currCou = Extend.LoginState.Accounts.Course();
            if (currCou != null)
            {
                //currCou = _trans(currCou);
                this.Document.SetValue("currCou", currCou);
            }
            int stid=Extend.LoginState.Accounts.CurrentUser.Ac_ID;
            //已经购买的课程
            List<Song.Entities.Course> buyCou = Business.Do<ICourse>().CourseForStudent(stid, null, 1, false, -1);
            for (int i = 0; i < buyCou.Count; i++) buyCou[i] = _trans(buyCou[i]);
            this.Document.SetValue("buyCou", buyCou);
            //付费且过期的课程
            List<Song.Entities.Course> overCou = Business.Do<ICourse>().CourseForStudent(stid, null, 2, false, -1);
            for (int i = 0; i < overCou.Count; i++) overCou[i] = _trans(overCou[i]);
            this.Document.SetValue("overCou", overCou);
            //试学的课程
            List<Song.Entities.Course> tryCou = Business.Do<ICourse>().CourseForStudent(stid, null, 0, true, -1);
            for (int i = 0; i < tryCou.Count; i++) tryCou[i] = _trans(tryCou[i]);
            this.Document.SetValue("tryCou", tryCou);
            //购买课程的时间区间
            this.Document.RegisterGlobalFunction(this.getBuyInfo);
        }
        /// <summary>
        /// 获取课程的购买信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected object getBuyInfo(object[] id)
        {
            int couid = 0;
            if (id.Length > 0 && id[0] is int)
                int.TryParse(id[0].ToString(), out couid);

            return Business.Do<ICourse>().StudyCourse(Extend.LoginState.Accounts.CurrentUser.Ac_ID, couid);
        }
        /// <summary>
        /// 处理课程
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected Song.Entities.Course _trans(Song.Entities.Course c)
        {
            c.Cou_Intro = Extend.Html.ClearHTML(c.Cou_Intro);
            c.Cou_LogoSmall = Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
            c.Cou_Logo = Upload.Get["Course"].Virtual + c.Cou_Logo;
            return c;
        }
    }
}