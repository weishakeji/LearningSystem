using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song.Entities;
using System.Reflection;

namespace Song.ServiceImpls.Exam
{
    /// <summary>
    /// XML扩展类
    /// </summary>
    public static class HandlerExtensions
    {
        /// <summary>
        /// 设置节点属性，如果属性不存在，则创建属性并赋值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlNode SetAttr(this XmlNode node, string attr, string value)
        {
            if (node.Attributes == null || node.Attributes[attr] == null)
            {
                // 如果属性不存在，则创建并添加
                XmlAttribute newAttr = node.OwnerDocument.CreateAttribute(attr);
                newAttr.Value = value;
                node.Attributes.Append(newAttr);
            }
            else node.Attributes[attr].Value = value;
            return node;
        }
        /// <summary>
        /// 获取节点的属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static T GetAttr<T>(this XmlNode node, string attr)
        {
            string val = node.Attributes[attr]?.Value;
            if (string.IsNullOrEmpty(val)) return default(T);
            return val.Convert<T>();
        }
        /// <summary>
        /// 获取节点的属性值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string GetAttr(this XmlNode node, string attr) => node.Attributes[attr]?.Value;
        /// <summary>
        /// 将节点属性转Base64编码
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static XmlNode AttrEncryptForBase64(this XmlNode node)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                string val = attr.Value;
                val = val.Replace("＜", "<");
                val = val.Replace("＞", ">");
                val = val.Replace("（", "(");
                val = val.Replace("）", ")");
                val = val.Replace("＆", "&");
                val = val.Replace("〓", "=");
                val = val.Replace("＂", "\"");
                val = val.Replace("｀", "'");
                val = val.Replace("＼", "\\");
                attr.Value = WeiSha.Core.DataConvert.EncryptForBase64(val);
            }
            return node;
        }
        /// <summary>
        /// 将节点属性进行Base64解码
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static XmlNode AttrDecryptForBase64(this XmlNode node)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                string val = WeiSha.Core.DataConvert.DecryptForBase64(attr.Value);
                val = val.Replace("<", "＜");
                val = val.Replace(">", "＞");
                val = val.Replace("(", "（");
                val = val.Replace(")", "）");
                val = val.Replace("&", "＆");
                val = val.Replace("=", "〓");
                val = val.Replace("\"", "＂");
                val = val.Replace("'", "｀");
                val = val.Replace("\\", "＼");
                attr.Value = val;
            }
            return node;
        }
        /// <summary>
        /// 将时间对象转为毫秒数（长整型）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long TimeStamp(this DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        /// <summary>
        /// 长整型转时间
        /// </summary>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long lng)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime date = dtStart.Add(new TimeSpan(lng * 10000));
            return date;
        }
        /// <summary>
        /// 字符串转换方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Convert<T>(this string str)
        {
            if (string.IsNullOrEmpty(str)) return default(T);
            Type type = typeof(T);
            switch (type.Name)
            {
                case "DateTime":
                    long lng = long.TryParse(str, out long tm) ? tm : 0;
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    DateTime date = dtStart.Add(new TimeSpan(lng * 10000));
                    return (T)System.Convert.ChangeType(date, type);
                default:
                    object obj = System.Convert.ChangeType(str, type);
                    return (T)obj;
            }
            return default(T);
        }
    }
}
