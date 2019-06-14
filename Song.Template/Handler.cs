using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using VTemplate.Engine;

namespace Song.Template
{
    public class Handler
    {
        /// <summary>
        /// 处理模板中的标签
        /// </summary>
        /// <param name="doc">模板对象</param>
        public static void Start(TemplateDocument doc)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //列表标签的处理
            ElementCollection<Tag> tags = doc.GetCustomTags("list,repeat");
            foreach (Element item in tags)
            {
                if (item is Tag)
                {
                    Tag tag = (Tag)item;
                    //如果已经指定了数据来源，则不再继续往下走了
                    string from = tag.Attributes.GetValue("from");
                    if (!string.IsNullOrWhiteSpace(from)) continue;
                    //如果没有数据源，则通过以下解析，指定数据源                   
                    string tagname = tag.TagName;    //标签名称
                    string tagSpace = string.Empty;     //要实现标签解析的命名空间名称
                    if (tagname == "list") tagSpace = "ListTag";
                    if (tagname == "repeat") tagSpace = "RepeaterTag";
                    if (tagname == "detail") tagSpace = "DetailTag";
                    //组件标签类的完整路径名
                    string classFullName = "Song.Template.Tags.{0}.{1}";
                    classFullName = String.Format(classFullName, tagSpace, tag.Type);
                    Type info = System.Type.GetType(classFullName); //创建反射
                    
                    if (info == null) continue;
                    object obj = System.Activator.CreateInstance(info);     //实例化标签对象
                    Song.Template.Tags.TagElement el = (Song.Template.Tags.TagElement)obj;
                    if (el == null) continue;
                    el.Organ = org;
                    el.Type = tag.Type;
                    el.Tag = tag;
                    el.DataBind();  //绑定数据源                
                }
            }
            //注册方法
            RegisterFunction.Register(doc);
        }

        
    }
}
