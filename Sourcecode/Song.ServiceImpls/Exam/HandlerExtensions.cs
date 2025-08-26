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
    }
}
