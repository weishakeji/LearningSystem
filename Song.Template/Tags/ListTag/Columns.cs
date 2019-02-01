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
    /// 内容分类
    /// </summary>
    public class Columns : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int count = int.Parse(this.Tag.Attributes.GetValue("count", "10"));
            //内容分类的类别，产品Product，新闻news，图片Picture,视频video,下载download,单页article
            string type = this.Tag.Attributes.GetValue("ctype", "News");  
             object from = this.Tag.Attributes.GetValue("from", null);
             if (from == null)
             {
                 Song.Entities.Columns[] sts = Business.Do<IColumns>().ColumnCount(Organ.Org_ID, type, true, count);
                 tag.DataSourse = sts;
             }
        }
    }
}
