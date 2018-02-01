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
using Song.Extend;

namespace Song.Site.Manage
{
    public partial class ReLogin : Extend.CustomPage
    {
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganRoot();
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
            //通过验证，进入登录状态            
            Song.Entities.EmpAccount emp = Business.Do<IEmployee>().GetSingle(org.Org_ID, tbAccName.Text.Trim(), this.tbPw1.Text.Trim());
            //写入Session
            LoginState.Admin.Write(emp);
            Master.Close();
        }
        /// <summary>
        /// 验证，验证码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //取图片验证码
            string imgCode = WeiSha.Common.Request.Cookies["relogin"].ParaValue;
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
            //员工的帐号
            string acc = this.tbAccName.Text.Trim();
            //判断是否通过验证
            bool isAccess = Business.Do<IEmployee>().LoginCheck(org.Org_ID, acc, tbPw1.Text.Trim());
            //验证
            CustomValidator2.IsValid = isAccess;
            return CustomValidator2.IsValid;
        }
    }
}
