using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using VTemplate.Engine;

namespace Song.Template.Tags.RepeaterTag
{
    /// <summary>
    /// 课程导航（现在叫通知公告）的分类
    /// </summary>
    public class Guidecolumns : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.RepeatTag tag = this.Tag as VTemplate.Engine.RepeatTag;
            if (tag == null) return;

            int couid = int.Parse(this.Tag.Attributes.GetValue("couid", "-1"));  //所属课程的id
            Song.Entities.GuideColumns[] sorts = Business.Do<IGuide>().GetColumnsAll(couid, true);
            //
            List<Song.Template.Tags.TreeObject> list = Song.Template.Tags.TreeObject.Bulder(sorts, "Gc_PID", "0", "Gc_ID");
            tag.DataSourse = list;    
        }
    }
}
