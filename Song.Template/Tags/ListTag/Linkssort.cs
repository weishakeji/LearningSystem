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
    /// 友情链接分类
    /// </summary>
    public class Linkssort : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int count = int.Parse(this.Tag.Attributes.GetValue("count", "-1"));            
            Song.Entities.LinksSort[] sorts = Business.Do<ILinks>().GetSortCount(Organ.Org_ID, true, true, count);
            foreach (Song.Entities.LinksSort s in sorts)
            {
                s.Ls_Logo = Upload.Get["Links"].Virtual + s.Ls_Logo;
            }
            tag.DataSourse = sorts;    
        }
    }
}
