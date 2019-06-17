using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using System.Web.SessionState;
namespace Song.Site.Admin
{
    /// <summary>
    /// Login1 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {
        //账号，密码，验证码
        string acc = WeiSha.Common.Request.Form["tbAcc"].String;
        string pw = WeiSha.Common.Request.Form["tbPw"].String;
        string code = WeiSha.Common.Request.Form["tbCode"].String;
        protected HttpResponse Response { get; private set; }
        public void ProcessRequest(HttpContext context)
        {
            this.Response = context.Response;
            if (!string.IsNullOrWhiteSpace(acc))
            {
                isLogin();
            }
            else
            {
                if (context.Request.UrlReferrer != null)
                {
                    context.Response.Redirect(context.Request.UrlReferrer.ToString());
                }
                else
                {
                    context.Response.Redirect("index.ashx");
                }
            }
        }
        #region 验证登录
        private void isLogin()
        {
            //账号为空
            if (string.IsNullOrWhiteSpace(acc))
                this.Response.Redirect("index.ashx?error=1");
            //验证码不正确
            if (!isCodeImg())
                this.Response.Redirect("index.ashx?error=2&acc=" + acc);
            //当前机构，通过二级域名判断，如果不存在或无法判断，则返回默认机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            //通过验证，进入登录状态            
            Song.Entities.EmpAccount emp = Business.Do<IEmployee>().EmpLogin(acc, pw, org.Org_ID);
            if (emp != null)
            {
                LoginState.Admin.Write(emp);
                this.Response.Redirect("panel.ashx");
            }
            else
            {
                //密码不正确
                this.Response.Redirect("index.ashx?error=3&acc=" + acc);
            }
        }
        /// <summary>
        /// 验证图片验证是否正确
        /// </summary>
        /// <returns></returns>
        private bool isCodeImg()
        {
            //取图片验证码
            string imgCode = WeiSha.Common.Request.Cookies["admincode"].ParaValue;
            //取员工输入的验证码
            string userCode = new WeiSha.Common.Param.Method.ConvertToAnyValue(code).MD5;
            //验证
            return imgCode == userCode;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}