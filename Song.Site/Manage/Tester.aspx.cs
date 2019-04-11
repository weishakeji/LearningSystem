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


namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();

            this.Title = org.Org_Name;
            

        }

    }
}
