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
        protected string servertime = string.Empty;
        protected string timediff = string.Empty;
        protected string timename = string.Empty;
        protected string stamp = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {           

        }
        public static string LegalName(string filename)
        {
            string s1 = "\\/:*?<>|\"";
            string s2 = "£Ü£¯£º¡ï£¿¡¼¡½£ü£¢";
            for (int i = 0; i < s1.Length; i++)
            {
                string t = s1.Substring(i, 1);
                string s = s2.Substring(i, 1);
                filename = filename.Replace(t, s);
            }
            return filename;
        }
        
    }
}
