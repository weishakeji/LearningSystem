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
using System.IO;
using WeiSha.Common;
using NPOI.HSSF.UserModel;
using NPOI.SS.Converter;
using Song.ServiceInterfaces;
using System.Text.RegularExpressions;
using pili_sdk.pili;


namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            //ÊÇ²»ÊÇiPad
            isiPad.Text = WeiSha.Common.Browser.IsIPad.ToString();
            isMobile.Text = WeiSha.Common.Browser.IsMobile.ToString();
            isPhone.Text = WeiSha.Common.Browser.IsIPhone.ToString();
            lbOs.Text = WeiSha.Common.Browser.MobileOS;

            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            string u = _context.Request.ServerVariables["HTTP_USER_AGENT"];
            lbUseagent.Text = u;
        }

    }
}
