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
    /// 通知公告
    /// </summary>
    public class Notice : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int noCount = int.Parse(this.Tag.Attributes.GetValue("count", "10"));
            object from = this.Tag.Attributes.GetValue("from", null);
            if (from == null)
            {
                Song.Entities.Notice[] notice = Business.Do<INotice>().GetCount(Organ.Org_ID, true, noCount);
                tag.DataSourse = notice;
            }
        }
    }
}
