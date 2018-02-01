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
namespace Song.Site.Manage.Student
{
    public partial class Parents : System.Web.UI.MasterPage
    {
        private Song.Entities.Accounts _acc = null;
        /// <summary>
        /// 当前登录账户
        /// </summary>
        public Song.Entities.Accounts Account
        {
            get
            {
                if (_acc == null) _acc = Extend.LoginState.Accounts.CurrentUser;
                return _acc;
            }
            set { _acc = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Page.Response.Cache.SetNoStore();
            //假如没有登录
            if (!Extend.LoginState.Accounts.IsLogin)
            {
                cphMain.Visible = false;
                plNoLogin.Visible = true;
             }
            else
            {
                _acc = Extend.LoginState.Accounts.CurrentUser;
                if (_acc == null)
                {
                    cphMain.Visible = false;
                    plLoginKick.Visible = true;
                }
            }
            
            if (!this.IsPostBack)
            {
                ////是否记录操作日志
                //bool isWork = Business.Do<ISystemPara>()["SysIsWorkLogs"].Boolean ?? true;
                ////记录操作日志
                //if (isWork)
                //    Business.Do<ILogs>().AddOperateLogs();
            }
        }
    }
}
