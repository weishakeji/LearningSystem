using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using VTemplate.Engine;

namespace Song.Template.Tags.ListTag
{
    /// <summary>
    /// 课程
    /// </summary>
    public class Course : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int count = int.Parse(this.Tag.Attributes.GetValue("count", "10")); //要取的记录数
            int sbjid = int.Parse(this.Tag.Attributes.GetValue("sbjid", "-1")); //所属专业的id
            //int thid = int.Parse(this.ListTag.Attributes.GetValue("thid", "-1"));   //教师id
            //int pid = int.Parse(this.ListTag.Attributes.GetValue("pid", "-1"));     //上级课程
            string search = this.Tag.Attributes.GetValue("search", "");         //按课程名检索
            string order = this.Tag.Attributes.GetValue("order", "");         //排序方式，flux流量最大优先,def推荐、流量，tax排序号，new最新,rec推荐
             object from = this.Tag.Attributes.GetValue("from", null);
             if (from == null)
             {
                 List<Song.Entities.Course> cours = Business.Do<ICourse>().CourseCount(this.Organ.Org_ID, sbjid, search, true, order, count);
                 foreach (Song.Entities.Course c in cours)
                 {
                     c.Cou_LogoSmall = string.IsNullOrWhiteSpace(c.Cou_LogoSmall) ? "" : Upload.Get["Course"].Virtual + c.Cou_LogoSmall;
                     c.Cou_Logo = string.IsNullOrWhiteSpace(c.Cou_Logo) ? "" : Upload.Get["Course"].Virtual + c.Cou_Logo;
                     c.Cou_Intro = HTML.ClearTag(c.Cou_Intro);
                 }
                 tag.DataSourse = cours;
             }
        }
    }
}
