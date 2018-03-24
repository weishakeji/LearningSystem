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
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;

namespace Song.Site.Manage
{
    public partial class Index : System.Web.UI.Page
    {
        //版权信息
        protected WeiSha.Common.Copyright<string, string> copyright = WeiSha.Common.Request.Copyright;
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganRoot();
            if (!this.IsPostBack)
            {
                //清除
                Extend.LoginState.Admin.Logout();
            }
            //系统名称,即管理平台上方的名称
            string sysName = copyright["product"];
            if (sysName != null)
            {
                consName.InnerText = sysName;
                this.Title = sysName;
            }
            
        }
        /// <summary>
        /// 确定按钮的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //验证码错误
            if (!CustomValidator1.IsValid)
                return;
            //验证密码
            //if (!CustomValidator2.IsValid)
            //    return;
            if (!this.CustomValidator2_ServerValidate(null, null))
            {
                return;
            }
            //验证，作为超管登录，必须是根机构员工       
            Song.Entities.EmpAccount emp = Business.Do<IEmployee>().GetSingle(org.Org_ID, tbAccName.Text.Trim(), this.tbPw1.Text.Trim());
            //写入Session
            LoginState.Admin.Write(emp);
            //记录 登录日志
            bool isLogin = Business.Do<ISystemPara>()["SysIsLoginLogs"].Boolean ?? true;
            if(isLogin)Business.Do<ILogs>().AddLoginLogs();
            Response.Redirect("console.aspx");
        }
        /// <summary>
        /// 验证，验证码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //取图片验证码
            string imgCode = WeiSha.Common.Request.Cookies["logindex"].ParaValue;
            //取员工输入的验证码
            string userCode = WeiSha.Common.Request.Controls[this.tbCode].MD5;
            //验证
            args.IsValid = imgCode == userCode;
        }
        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected bool CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string acc = this.tbAccName.Text.Trim();
            //判断是否通过验证
            bool isAccess = Business.Do<IEmployee>().LoginCheck(org.Org_ID, acc, tbPw1.Text.Trim());
            //验证
            CustomValidator2.IsValid = isAccess;
            return CustomValidator2.IsValid;
        }
    }
}
