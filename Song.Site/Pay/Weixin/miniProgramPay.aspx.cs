using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using LitJson;
using WxPayAPI;

namespace Song.Site.Pay.Weixin
{
    public partial class miniProgramPay : System.Web.UI.Page
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            WxPayAPI.Log.Info(this.GetType().ToString(), "进入支付页面 : ");
        }
    }
}