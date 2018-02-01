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
    /// 友情链接
    /// </summary>
    public class Links : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int count = int.Parse(this.Tag.Attributes.GetValue("count", "-1"));
            int sort =0;   //友情链接分类id
            //object sortStr = this.ListTag.Sort.GetValue();
            //int.TryParse(sortStr.ToString(), out sort);
            //分类名称
            string sortName = this.Tag.Attributes.GetValue("sortname", "");
            Song.Entities.Links[] links = null;
            if (!string.IsNullOrWhiteSpace(sortName))
            {
                links = Business.Do<ILinks>().GetLinks(Organ.Org_ID, sortName, true, true, count);
            }
            else
            {
                links = Business.Do<ILinks>().GetLinks(Organ.Org_ID, sort, true, true, count);
            }
            foreach (Song.Entities.Links s in links)
            {
                s.Lk_Logo = Upload.Get["Links"].Virtual + s.Lk_Logo;
                s.Lk_LogoSmall = Upload.Get["Links"].Virtual + s.Lk_LogoSmall;
            }
            tag.DataSourse = links;      
        }
    }
}
