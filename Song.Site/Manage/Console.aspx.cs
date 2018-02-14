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
using System.Xml;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;

namespace Song.Site.Manage
{
    public partial class Console : System.Web.UI.Page
    {
        //当前登录的员工id
        protected int id =0;
        //当前登录的员工姓名
        protected string name="";
        //当前应用程序的状态
        protected string appState = "";
        //是否为超级管理员，真为0
        protected int isAdmin = 1;

        Song.Entities.EmpAccount acc = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            fill();
            //当前页面的来源页
            object referer = this.Request.ServerVariables["HTTP_REFERER"];
            string rawUrl = referer!=null ? referer.ToString() : "   ";
            rawUrl = rawUrl.Substring(rawUrl.LastIndexOf("/")+1);
            //如果是从首页传过来的，则默认已经登录；否则判断是否登录
            if (rawUrl.ToLower() != "index.aspx")
            {
                if (acc == null) this.Response.Redirect("index.aspx"); //如果未登录                
            }
            //如果当前登录管理员，不是根机构，则退出。
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganRoot();
            if (org.Org_ID > 0 && (acc == null || org.Org_ID != acc.Org_ID))
            {
                this.Response.Redirect("index.aspx");
            }
            
            DateTime now=DateTime.Now;
            string[] weekdays ={ "星期日" ,"星期一" ,"星期二" ,"星期三" ,"星期四" ,"星期五" ,"星期六" };  
            string week=weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];  
            this.lbTime.Text = now.ToString("yyyy年M月d日 ") + week;
            //判断是否登录
            //Extend.ManageSession.Session.VerifyLogin();
            //当前版本
            consVersionName.InnerText = WeiSha.Common.License.Value.VersionName;
            
           
        }
        private void fill()
        {
           acc = Extend.LoginState.Admin.CurrentUser;
            if (acc != null)
            {
                id = acc.Acc_Id;
                name = acc.Acc_Name;
                isAdmin = Extend.LoginState.Admin.IsAdmin ? 0 : 1;                
                lbName.Text = acc.Acc_Name;
            }
            //系统名称,即管理平台上方的名称
            string sysName = Business.Do<ISystemPara>()["SystemName"].String;
            if (sysName != null)
            {
                //consName.InnerText = sysName;
                this.Title += sysName;
            }
            appState = App.Get["appState"].String;
        }
    }
}
