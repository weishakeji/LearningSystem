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
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;

namespace Song.Site.Manage.Common
{
    public partial class EmplyeeGroup : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {                
                BindData(null, null);
            }
        }
       
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //
                Song.Entities.EmpAccount[] eas = null;
                eas = Business.Do<IEmpGroup>().GetAll4Group(id);
                foreach (Song.Entities.EmpAccount ea in eas)
                {
                    ea.Acc_Photo = Upload.Get["Employee"].Virtual + ea.Acc_Photo;
                    if (!ea.Acc_IsOpenTel)
                        ea.Acc_Tel = "";
                    if (!ea.Acc_IsOpenMobile)
                        ea.Acc_MobileTel = "";
                }
                this.rpList.DataSource = eas;
                rpList.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
