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
    /// 直播频道
    /// </summary>
    public class Live : BasePage
    {
        //直播截图的域名
        string snapshot = Business.Do<ILive>().GetSnapshot;
        //直播空间名
        string hubname = Business.Do<ILive>().GetLiveSpace;
        protected override void InitPageTemplate(HttpContext context)
        {
            //正在直播的章节
            List<Song.Entities.Outline> lives = Business.Do<IOutline>().OutlineLiving(Organ.Org_ID, 10);
            if (lives != null)
            {
                this.Document.SetValue("lives", lives);
            }
            this.Document.RegisterGlobalFunction(this.GetSnapshot);
            this.Document.RegisterGlobalFunction(this.GetCourse);
            //课程列表           
            Tag couTag = this.Document.GetChildTagById("course");
            if (couTag != null)
            {
                int size = int.Parse(couTag.Attributes.GetValue("size", "12"));
                int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                List<Song.Entities.Course> cour = Business.Do<ICourse>().CoursePager(Organ.Org_ID, "", true, true, string.Empty, "rec", size, index, out sum);
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
        /// <summary>
        /// 获取直播的实时图片
        /// </summary>
        /// <param name="liveid">直播唯一ID</param>
        /// <returns></returns>
        protected object GetSnapshot(object[] liveid)
        {
            string title = string.Empty;
            if (liveid.Length > 0) title = liveid[0].ToString();
            //封面地址
            string cover = string.Format("http://{0}/{1}/{2}.jpg", snapshot, hubname, title);
            return cover;
        }
        /// <summary>
        /// 获取试题的类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object GetCourse(object[] olid)
        {
            int couid = 0;
            if (olid.Length > 0)
                int.TryParse(olid[0].ToString(), out couid);
            if (couid < 1) return null;
            return Business.Do<ICourse>().CourseSingle(couid);
        }
    }
}