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
    /// 新闻
    /// </summary>
    public class News : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int count = int.Parse(this.Tag.Attributes.GetValue("count", "10")); //要取的记录数
            int columns = int.Parse(this.Tag.Attributes.GetValue("columns", "-1")); //新闻栏目id，默认取所有
            string order = this.Tag.Attributes.GetValue("order", "hot");    //排序方式
            object from = this.Tag.Attributes.GetValue("from", null);
            if (from == null)
            {
                Song.Entities.Article[] news = Business.Do<IContents>().ArticleCount(this.Organ.Org_ID, columns, count, order);
                foreach (Song.Entities.Article n in news)
                {
                    n.Art_Logo = Upload.Get["News"].Virtual + n.Art_Logo;
                    if (string.IsNullOrWhiteSpace(n.Art_Intro))
                    {
                        n.Art_Intro = WeiSha.Common.HTML.ClearTag(n.Art_Details, 200);
                    }
                }
                //指定数据源
                tag.DataSourse = news;
            }
        }
    }
}
