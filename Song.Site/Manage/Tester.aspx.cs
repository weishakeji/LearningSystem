using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Drawing;
using System.Reflection;
using System.Xml.Serialization;
using Spring.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Xml;

namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string xml = this.MapPath("/copyright.xml");
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xml);
            XmlNode first = xmlDoc.LastChild;
            dynamic d = new System.Dynamic.ExpandoObject();
            if (first != null)
            {
                XmlNodeList list = first.ChildNodes;
                
                //创建属性，并赋值。
                foreach (XmlNode node in list)
                {
                    if (node.NodeType != XmlNodeType.Element) continue;
                    (d as System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>)
                        .Add(new System.Collections.Generic.KeyValuePair<string, object>(node.Name, node.InnerText));
                }
            }            
            Response.End();
        }
        
    }
}
