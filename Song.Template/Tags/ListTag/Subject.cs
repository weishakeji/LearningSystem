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
    /// 专业
    /// </summary>
    public class Subject : TagElement
    {
        public override void DataBind()
        {
            VTemplate.Engine.ListTag tag = this.Tag as VTemplate.Engine.ListTag;
            if (tag == null) return;
            //
            int count = tag.Count;
            int start = int.Parse(this.Tag.Attributes.GetValue("start", "0")); //起始索引号
            string search = this.Tag.Attributes.GetValue("search", "");         //检索字符
            //上级专业的id
            int pid = 0;
            int.TryParse(this.Tag.Attributes.GetValue("pid", "-1"), out pid);
            //排序方式，def默认排序（先推荐，后排序号），tax按排序号,rec按推荐
            string order = this.Tag.Attributes.GetValue("order", "def");

            //取上传标签的数据源
            if (this.Tag.Parent != null)
            {
                VTemplate.Engine.ListTag t = this.Tag.Parent as VTemplate.Engine.ListTag;
                if (t != null && t.DataSourse != null)
                {
                    Song.Entities.Subject[] sbjt = t.DataSourse as Song.Entities.Subject[];
                    int sid = sbjt[0].Sbj_ID;
                    object o = tag.Index.Value;
                }
            }
            //数据来源
            string from = this.Tag.Attributes.GetValue("from");
            //如果没有指定数据来源（如果指定了但值为空，仍然认为已经指定），才会读取数据库。
            if (string.IsNullOrWhiteSpace(from))
            {
                Song.Entities.Subject[] sbjs = Business.Do<ISubject>().SubjectCount(this.Organ.Org_ID, search, true, pid, order, start, count);
                string path = Upload.Get["Subject"].Virtual;
                foreach (Song.Entities.Subject c in sbjs)
                {
                    c.Sbj_Logo = path + c.Sbj_Logo;
                    c.Sbj_LogoSmall = path + c.Sbj_LogoSmall;
                    //如果别名为空，则别名等于专业名称
                    if (string.IsNullOrWhiteSpace(c.Sbj_ByName) || c.Sbj_ByName.Trim() == "")
                        c.Sbj_ByName = c.Sbj_Name;
                    c.Sbj_Intro = HTML.ClearTag(c.Sbj_Intro);
                }
                tag.DataSourse = sbjs;
            }
            
        }
       
    }
}
