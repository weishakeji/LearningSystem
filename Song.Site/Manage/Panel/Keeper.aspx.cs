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
using Song.Extend;

namespace Song.Site.Manage.Panel
{
    /// <summary>
    /// 保持登录状态的方法，由客户端ajax时实调用
    /// </summary>
    public partial class Keeper : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //清理超时用户
            LoginState.Admin.CleanOut();          
            //判断是否登录
            if (LoginState.Admin.IsLogin)
            {
                //重新写入session
                LoginState.Admin.Write();
                Response.Write("0");
            }
            else
            {
                Response.Write("1");
            }
            Response.End();
        }
    }
}
