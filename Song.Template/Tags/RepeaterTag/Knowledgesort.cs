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
    /// 知识库的分类
    /// </summary>
    public class Knowledgesort : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.RepeatTag tag = this.Tag as VTemplate.Engine.RepeatTag;
            if (tag == null) return;

            int couid = int.Parse(this.Tag.Attributes.GetValue("couid", "-1"));  //所属课程的id
            Song.Entities.KnowledgeSort[] sorts = Business.Do<IKnowledge>().GetSortAll(-1, couid, true);
            //
            List<Song.Template.Tags.TreeObject> list = Song.Template.Tags.TreeObject.Bulder(sorts, "Kns_PID", "0", "Kns_ID");
            tag.DataSourse = list;    
        }
    }
}
