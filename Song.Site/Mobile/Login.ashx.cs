using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 学员登录的界面
    /// </summary>
    public class Login : BasePage
    {
        //GET提交时的id,密码（md5编码格式）--------用于自动提交，数据来自localStorage
        int accid = WeiSha.Common.Request.QueryString["accid"].Int32 ?? 0;
        string accpw = WeiSha.Common.Request.QueryString["accpw"].String;       
        //用于标识布局的值
        protected string loyout = WeiSha.Common.Request.QueryString["loyout"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Extend.LoginState.Accounts.IsLogin)
                this.Response.Redirect("default.ashx");
            //相关参数
            WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
            //登录方式
            bool IsLoginForPw = config["IsLoginForPw"].Value.Boolean ?? true;    //启用账号密码登录
            bool IsLoginForSms = config["IsLoginForSms"].Value.Boolean ?? true;  //启用手机短信验证登录
            this.Document.SetValue("forpw", IsLoginForPw);
            this.Document.SetValue("forsms", IsLoginForSms);
            this.Document.SetValue("islogin", !IsLoginForPw && !IsLoginForSms);
            this.Document.SetValue("isWeixin", WeiSha.Common.Browser.IsWeixin); //是否在微信中
            //界面状态
            if (!IsLoginForPw && IsLoginForSms) loyout = "mobile";
            this.Document.SetValue("loyout", loyout);
            //来源页
            string from = WeiSha.Common.Request.QueryString["from"].String;
            if (string.IsNullOrWhiteSpace(from)) from = context.Request.UrlReferrer != null ? context.Request.UrlReferrer.PathAndQuery : "";
            this.Document.SetValue("from", from);
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
                }
                Response.End();
            }
            //如果localStorage存有用户id，会提交到这里
            if (accid > 0)
            {
                ajaxLogin(context);
                return;
            }
            //QQ登录
            this.Document.SetValue("QQLoginIsUse", Business.Do<ISystemPara>()["QQLoginIsUse"].Boolean ?? true);
            this.Document.SetValue("QQAPPID", Business.Do<ISystemPara>()["QQAPPID"].String);
            this.Document.SetValue("QQReturl", Business.Do<ISystemPara>()["QQReturl"].Value ?? "http://" + WeiSha.Common.Server.MainName);
            //微信登录
            this.Document.SetValue("WeixinLoginIsUse", Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? false);
            this.Document.SetValue("WeixinAPPID", Business.Do<ISystemPara>()["WeixinpubAPPID"].String);
            this.Document.SetValue("WeixinReturl", Business.Do<ISystemPara>()["WeixinpubReturl"].Value ?? "http://" + WeiSha.Common.Server.MainName);
            //金碟云之家登录
            this.Document.SetValue("YunzhijiaLoginIsuse", Business.Do<ISystemPara>()["YunzhijiaLoginIsuse"].Boolean ?? false);
            //记录当前机构到本地，用于QQ或微信注册时的账户机构归属问题
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("ORGID");
            cookie.Value = this.Organ.Org_ID.ToString();
            //如果是多机构，又不用IP访问，则用根域写入cookie
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) cookie.Domain = WeiSha.Common.Server.MainName;
            this.Response.Cookies.Add(cookie);
            //推荐人id
            string sharekeyid = WeiSha.Common.Request.QueryString["sharekeyid"].String;
            System.Web.HttpCookie cookieShare = new System.Web.HttpCookie("sharekeyid");
            cookieShare.Value = sharekeyid;
            //如果是多机构，又不用IP访问，则用根域写入cookie
            if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) cookieShare.Domain = WeiSha.Common.Server.MainName;
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
            bool sign = WeiSha.Common.Request.Form["sign"].Boolean ?? true; //是否免登录
            //通过验证，进入登录状态            
            Song.Entities.Accounts emp = Business.Do<IAccounts>().AccountsLogin(acc, pw, true);
            if (emp != null)
            {
                //如果没有设置免登录，则按系统设置的时效
                if (!sign)
                    LoginState.Accounts.Write(emp);
                else
                    LoginState.Accounts.Write(emp, 999);
                //登录成功
                Business.Do<IAccounts>().PointAdd4Login(emp, "手机网页", "账号密码登录", "");   //增加登录积分
                Business.Do<IStudent>().LogForLoginAdd(emp);
                string json = "{\"success\":\"1\",\"name\":\"" + emp.Ac_Name + "\",\"acid\":\"" + emp.Ac_ID + "\",\"sign\":\"" + (sign ? "1" :"") + "\",\"pw\":\"" + emp.Ac_Pw + "\"}";                
                Response.Write(json);
            }
            else
            {
                //登录失败
                Response.Write("{\"success\":\"-1\"}");
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
            bool sign = WeiSha.Common.Request.Form["sign"].Boolean ?? true;    //是否保持登录状态
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
                    if (sign)
                        LoginState.Accounts.Write(emp);
                    else
                        LoginState.Accounts.Write(emp, 999);
                    //登录成功
                    Business.Do<IAccounts>().PointAdd4Login(emp, "手机网页", "短信验证登录", "");   //增加登录积分
                    Business.Do<IStudent>().LogForLoginAdd(emp);
                    string json = "{\"success\":\"1\",\"name\":\"" + emp.Ac_Name + "\",\"acid\":\"" + emp.Ac_ID + "\",\"sign\":\"" + (sign ? "1" : "") + "\",\"pw\":\"" + emp.Ac_Pw + "\",\"state\":\"0\"}";
                    Response.Write(json);
                }
                else
                {
                    //登录失败
                    Response.Write("{\"success\":\"-1\"}");
                }
            }
        }
        #endregion

        #region 自动验证
        /// <summary>
        /// 用于接收ajax递交来的登录验证，此方式是通过accid与accpw（md5密文）来验证，没有验证码
        /// </summary>
        /// <param name="context"></param>
        private void ajaxLogin(HttpContext context)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Accounts emp = Business.Do<IAccounts>().AccountsLogin(accid, accpw, true);
            if (emp != null)
            {
                context.Response.Write(1);
                LoginState.Accounts.Write(emp);
            }
            else
            {
                context.Response.Write(0);
            }
            context.Response.End();
        }
        #endregion
    }
}