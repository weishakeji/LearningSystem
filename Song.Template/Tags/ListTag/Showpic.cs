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
    /// 轮换图片
    /// </summary>
    public class Showpic : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            string site = this.Tag.Attributes.GetValue("site", "web");
            object from = this.Tag.Attributes.GetValue("from", null);
            if (from == null)
            {
                Song.Entities.ShowPicture[] shp = Business.Do<IStyle>().ShowPicAll(true, site, Organ.Org_ID);
                foreach (Song.Entities.ShowPicture s in shp)
                    s.Shp_File = Upload.Get["ShowPic"].Virtual + s.Shp_File;
                tag.DataSourse = shp;
            }
        }
    }
}
