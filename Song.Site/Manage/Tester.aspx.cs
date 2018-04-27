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
            string domain = "xx";
            string access_token = "access_token";
            string openid = "openid";
            string orgid = "orgid";
            //登录成功后的返回地址
             string uri = "{0}/{1}?token={2}&openid={3}&orgid={4}";
             uri = string.Format(uri, domain, WeiSha.Common.Request.Page.FileName, access_token, openid, orgid);
            Response.End();
        }
        
    }
}
