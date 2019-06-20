using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using VTemplate.Engine;
using System.Web.SessionState;
namespace Song.Site.Student
{
    /// <summary>
    /// 学员登录
    /// </summary>
    public class Index : BasePage, IRequiresSessionState
    {
        //用于标识布局的值
        protected string loyout = WeiSha.Common.Request.QueryString["loyout"].String;        
        protected override void InitPageTemplate(HttpContext context)
        {
            //设置主域，用于js跨根域
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                //来源页
                string from = WeiSha.Common.Request.QueryString["from"].String;
                if (string.IsNullOrWhiteSpace(from)) from = context.Request.UrlReferrer != null ? context.Request.UrlReferrer.PathAndQuery : "";
                this.Document.SetValue("from", from);
                this.Document.SetValue("frompath", context.Request.UrlReferrer != null ? context.Request.UrlReferrer.ToString() : "");
                //设置主域，用于js跨根域                
                if (multi == 0 && !WeiSha.Common.Server.IsLocalIP)
                    this.Document.Variables.SetValue("domain", WeiSha.Common.Server.MainName);
                //相关参数
                WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
                //登录方式
                bool IsLoginForPw = config["IsLoginForPw"].Value.Boolean ?? true;    //启用账号密码登录
                bool IsLoginForSms = config["IsLoginForSms"].Value.Boolean ?? true;  //启用手机短信验证登录
                this.Document.SetValue("forpw", IsLoginForPw);
                this.Document.SetValue("forsms", IsLoginForSms);
                this.Document.SetValue("islogin", !IsLoginForPw && !IsLoginForSms);
                //界面状态
                if (!IsLoginForPw && IsLoginForSms) loyout = "mobile";
                this.Document.SetValue("loyout", loyout);
            }

            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {                    
                    case "accloginvcode":
                        accloginvcode_verify(); //验证账号登录时的验证码
                        break;
                    case "acclogin":
                        acclogin_verify();  //验证账号登录时的密码
                        break;
                    case "getSms":
                        mobivcode_verify();  //验证手机登录时，获取短信时的验证码
                        break;
                    case "mobilogin":
                        mobiLogin_verify();
                        break;
                    default:
                        acclogin_verify();  //验证账号登录时的密码
                        break;
                }
                Response.End();
            }
            //QQ登录
            this.Document.SetValue("QQLoginIsUse", Business.Do<ISystemPara>()["QQLoginIsUse"].Boolean ?? true);
            this.Document.SetValue("QQAPPID", Business.Do<ISystemPara>()["QQAPPID"].String);
            string returl = Business.Do<ISystemPara>()["QQReturl"].Value ?? "http://" + WeiSha.Common.Server.MainName;
            this.Document.SetValue("QQReturl", returl);
            //微信登录
            this.Document.SetValue("WeixinLoginIsUse", Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? false);
            this.Document.SetValue("WeixinAPPID", Business.Do<ISystemPara>()["WeixinAPPID"].String);
            this.Document.SetValue("WeixinReturl", Business.Do<ISystemPara>()["WeixinReturl"].Value ?? "http://" + WeiSha.Common.Server.MainName);
            //记录当前机构到本地，用于QQ或微信注册时的账户机构归属问题
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("ORGID");
            cookie.Value = this.Organ.Org_ID.ToString();
            //设置主域，用于js跨根域
            if (multi == 0 && !WeiSha.Common.Server.IsLocalIP)
                cookie.Domain = WeiSha.Common.Server.MainName;
            this.Response.Cookies.Add(cookie);
            //推荐人id
            string sharekeyid = WeiSha.Common.Request.QueryString["sharekeyid"].String;
            System.Web.HttpCookie cookieShare = new System.Web.HttpCookie("sharekeyid");
            cookieShare.Value = sharekeyid;
            if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) 
                cookieShare.Domain = WeiSha.Common.Server.MainName;
            this.Response.Cookies.Add(cookieShare);
        }
        #region 账号登录验证
        /// <summary>
        /// 验证账号登录的校验吗
        /// </summary>
        private void accloginvcode_verify()
        {            
            //取图片验证码
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;
            //取输入的验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;
            //验证
            string json = "{\"success\":\"" + (imgCode == userCode ? 1 : -1) + "\"}";
            Response.Write(json);            
        }
        /// <summary>
        /// 验证账号+密码的登录
        /// </summary>
        private void acclogin_verify()
        {
            string acc = WeiSha.Common.Request.Form["acc"].String;  //账号
            string pw = WeiSha.Common.Request.Form["pw"].String;    //密码
            string sign = WeiSha.Common.Request.Form["sign"].String;
            int signnum = WeiSha.Common.Request.Form["signnum"].Int32 ?? 1;
            string succurl = WeiSha.Common.Request.Form["succurl"].String;  //登录成功跳转的页面，用于非ajax请求时（政务版专用）
            string failurl = WeiSha.Common.Request.Form["failurl"].String;  //登录失败要跳转的页面，用于非ajax请求时（政务版专用）
            //通过验证，进入登录状态            
            Song.Entities.Accounts emp = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (emp != null)
            {
                //如果没有设置免登录，则按系统设置的时效
                if (sign == "")
                    LoginState.Accounts.Write(emp);
                else
                    LoginState.Accounts.Write(emp, signnum);
                //登录成功
                Business.Do<IAccounts>().PointAdd4Login(emp, "电脑网页", "账号密码登录", "");   //增加登录积分
                Business.Do<IStudent>().LogForLoginAdd(emp);
                if (string.IsNullOrWhiteSpace(succurl))
                    Response.Write("{\"success\":\"1\",\"acid\":\"" + emp.Ac_ID + "\",\"name\":\"" + emp.Ac_Name + "\"}");
                else
                    Response.Redirect(succurl);
            }
            else
            {
                //登录失败
                if (string.IsNullOrWhiteSpace(failurl))
                    Response.Write("{\"success\":\"-1\"}");
                else
                    Response.Redirect(failurl);
            }
        }
        #endregion

        #region 手机短信验证
        /// <summary>
        /// 获取短信之间的验证
        /// </summary>
        private void mobivcode_verify()
        {
            //取图片验证码
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;
            //取输入的验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;
            //输入的手机号
            string phone = WeiSha.Common.Request.Form["phone"].String;
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            //验证手机号是否存在
            Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, phone, 1);
            if (acc == null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号不存在
                return;
            }
            //发送短信验证码
            try
            {
                bool success = Business.Do<ISMS>().SendVcode(phone, "mobi_" + vname);
                //bool success = true;
                if (success) Response.Write("{\"success\":\"1\",\"state\":\"0\"}");  //短信发送成功   
                Business.Do<IStudent>().LogForLoginAdd(acc);
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\",\"desc\":\"" + ex.Message + "\"}");  //短信发送失败   
            }
        }
        /// <summary>
        /// 手机登录的验证
        /// </summary>
        private void mobiLogin_verify()
        {            
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            string phone = WeiSha.Common.Request.Form["phone"].String;  //输入的手机号
            string sms = WeiSha.Common.Request.Form["sms"].MD5;  //输入的短信验证码
            string sign = WeiSha.Common.Request.Form["sign"].String;    //是否保持登录状态
            int signnum = WeiSha.Common.Request.Form["signnum"].Int32 ?? 1;     //保持登录的时效
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            //验证手机号是否存在
            Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, phone, 1);
            if (acc == null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号不存在
                return;
            }
            //验证短信验证码
            string smsCode = WeiSha.Common.Request.Cookies["mobi_" + vname].ParaValue;
            if (sms != smsCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\"}");  //短信验证失败             
                return;
            }            
            else
            {
                //通过验证，进入登录状态       
                Song.Entities.Accounts emp = Business.Do<IAccounts>().AccountsSingle(phone, true, true);
                if (emp != null)
                {
                    //如果没有设置免登录，则按系统设置的时效
                    if (sign == "")
                        LoginState.Accounts.Write(emp);
                    else
                        LoginState.Accounts.Write(emp, signnum);
                    //登录成功
                    Business.Do<IAccounts>().PointAdd4Login(emp, "电脑网页", "短信验证登录", "");   //增加登录积分
                    LoginState.Accounts.Refresh(emp);
                    Response.Write("{\"success\":\"1\",\"name\":\"" + emp.Ac_Name + "\",\"acid\":\"" + emp.Ac_ID + "\",\"state\":\"0\"}");
                }
                else
                {
                    //登录失败
                    Response.Write("{\"success\":\"-1\"}");
                }               
            }
        }
        #endregion
    }
}