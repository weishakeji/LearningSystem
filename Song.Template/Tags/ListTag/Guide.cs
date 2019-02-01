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
    /// 课程公告（原来的课程指南）
    /// </summary>
    public class Guide : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            int size = 20, index = 1;
            int sumcount = 0;
            //
            int couid = int.Parse(this.Tag.Attributes.GetValue("couid", "-1"));     //所属课程的id
            int noCount = int.Parse(this.Tag.Attributes.GetValue("count", "10"));
            object from = this.Tag.Attributes.GetValue("from", null);
            if (from == null)
            {
                Song.Entities.Guide[] entities = Business.Do<IGuide>().GetGuidePager(Organ.Org_ID, couid, -1, null, true, size, index, out sumcount);
                tag.DataSourse = entities;
            }
        }
    }
}
