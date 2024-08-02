using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song.ViewData.Attri;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 自定义版权信息
    /// </summary>
    [HttpGet,HttpPut]
    public class Copyright : IViewAPI
    {
        private string xmlPath = WeiSha.Core.Copyright.File;
        /// <summary>
        /// 版本信息,key值为节点名称
        /// </summary>
        /// <returns></returns>
        [Cache]
        public Dictionary<string, string> Info()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!System.IO.File.Exists(xmlPath)) return null;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.LastChild;

            foreach (XmlNode n in root.ChildNodes)
            {
                dic.Add(n.Name, Microsoft.JScript.GlobalObject.escape(n.InnerText));
            }
            return dic;
        }
        /// <summary>
        /// 版本信息
        /// </summary>
        /// <returns>name:节点名称,remark:备注，type:信息类型（一般为text，如果image则为base编码），text节点内容</returns>
        [Cache]
        public JArray Datas()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string file = xmlPath;
            if (!System.IO.File.Exists(file)) return null;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(file);
            XmlNode root = doc.LastChild;
            JArray jarray = new JArray();
            foreach (XmlElement n in root.ChildNodes)
            {
                JObject jo = new JObject();
                jo.Add("name", n.Name);
                jo.Add("remark", n.GetAttribute("remark"));
                jo.Add("type", n.GetAttribute("type"));
                string fix = n.GetAttribute("fixed");
                jo.Add("fixed", string.IsNullOrWhiteSpace(fix) ? false : Convert.ToBoolean(fix));
                jo.Add("text", Microsoft.JScript.GlobalObject.escape(n.InnerText));
                jarray.Add(jo);
            }
            return jarray;           
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        [HtmlClear(Not = "json")]
        public bool Update(JArray json)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            XmlDeclaration xmlSM = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlSM);
            xmlDoc.AppendChild(xmlDoc.CreateProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/Utilities/Copyright.xsl'"));
            XmlElement xml = xmlDoc.CreateElement("", "Copyright", "");

            foreach (JToken jt in json)
            {
                JObject jo = (JObject)jt;
                string nodename = string.Empty;
                foreach (JProperty item in jo.Children())
                {
                    if ("name".Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        nodename = item.Value.ToString();
                        break;
                    }
                }
                if (string.IsNullOrWhiteSpace(nodename)) continue;
                XmlElement node = xmlDoc.CreateElement("", nodename, "");
                foreach (JProperty item in jo.Children())
                {
                    if ("name".Equals(item.Name, StringComparison.OrdinalIgnoreCase)) continue;
                    if ("remark".Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                        node.SetAttribute(item.Name, item.Value.ToString());
                    if ("type".Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                        node.SetAttribute(item.Name, item.Value.ToString());
                    if ("fixed".Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                        node.SetAttribute(item.Name, item.Value.ToString().ToLower());
                    if ("text".Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        XmlCDataSection cd = xmlDoc.CreateCDataSection(item.Value.ToString());
                        node.AppendChild(cd);
                    }
                }
                xml.AppendChild(node);
            }
            xmlDoc.AppendChild(xml);
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            try
            {
                xmlDoc.Save(xmlPath);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 版本信息，key值为mark
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> Mark()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();           
            //string file = xmlPath;
            if (!System.IO.File.Exists(xmlPath)) return null;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.LastChild;

            foreach (XmlElement n in root.ChildNodes)
            {

                string remark = n.GetAttribute("remark");
                if (!dic.ContainsKey(remark))
                {
                    dic.Add(remark, Microsoft.JScript.GlobalObject.escape(n.InnerText));
                }
            }
            return dic;
        }
    }
}
