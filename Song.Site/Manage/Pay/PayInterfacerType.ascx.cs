using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
namespace Song.Site.Manage.Pay
{
    public partial class PayInterfacerType : System.Web.UI.UserControl
    {
        //支付接口的id
        protected int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        public DropDownList DdlInterFace
        {
            get { return ddlInterFace; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Song.Entities.PayInterface pi = Business.Do<IPayInterface>().PaySingle(id);
                if (pi != null)
                {
                    ListItem li = ddlInterFace.Items.FindByText(pi.Pai_Pattern);
                    if (li != null)
                    {
                        ddlInterFace.SelectedIndex = -1;
                        li.Selected = true;
                    }
                }
                else
                {
                    string pagename = WeiSha.Common.Request.Page.FileName;
                    ListItem li = ddlInterFace.Items.FindByValue(pagename.ToLower());
                    if (li != null)
                    {
                        ddlInterFace.SelectedIndex = -1;
                        li.Selected = true;
                    }
                }
            }
        }
    }
}