using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTemplate.Engine;

namespace Song.Template.Tags
{
    public abstract class TagElement
    {
        /// <summary>
        /// 当前机构
        /// </summary>
        public Song.Entities.Organization Organ { get; internal set; }
        /// <summary>
        /// 标签类型
        /// </summary>
        public string Type { get; internal set; }
        /// 标签对象
        /// </summary>
        public VTemplate.Engine.Tag Tag { get; internal set; }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        public abstract void DataBind();
    }
}
