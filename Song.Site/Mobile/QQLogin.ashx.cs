using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using WeiSha.Common;
using Song.ServiceInterfaces;
using System.Xml;
using Song.Extend;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Song.Site.Mobile
{
    /// <summary>
    /// QQ登录接入
    /// </summary>
    public class QQLogin : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {
            #region 具体操作代码
            string access_token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.QueryString["openid"].String;
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET" && !string.IsNullOrWhiteSpace(access_token))
            {
                this.Document.Variables.SetValue("token", access_token);
                this.Document.Variables.SetValue("openid", openid);
                //设置主域，用于js跨根域
                int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
                if (multi == 0 && !WeiSha.Common.Server.IsLocalIP)
                    this.Document.Variables.SetValue("domain", WeiSha.Common.Server.MainName);
                //QQ回调域
                string qqreturl = Business.Do<ISystemPara>()["QQReturl"].Value;
                if (string.IsNullOrWhiteSpace(qqreturl)) qqreturl = WeiSha.Common.Server.MainName;
                this.Document.SetValue("QQReturl", qqreturl + "/qqlogin.ashx");
                //当前机构
                Song.Entities.Organization org = getOrgan();
                this.Document.SetValue("domain2", getOrganDomain(org));
                //获取帐户，如果已经注册，则直接实现登录
                Song.Entities.Accounts acc = _ExistAcc(openid);
                if (acc == null)
                {                    
                    //账户不存在，以下用于注册
                    WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
                    this.Document.SetValue("forpw", config["IsLoginForPw"].Value.Boolean ?? true);  //启用账号密码登录
                    this.Document.SetValue("forsms", config["IsLoginForSms"].Value.Boolean ?? true);     //启用手机短信验证登录
                    this.Document.SetValue("IsQQDirect", Business.Do<ISystemPara>()["QQDirectIs"].Boolean ?? true);    //是否允许qq直接注册登录
                    //获取qq登录账户的信息
                    acc = getUserInfo(access_token, openid);
                    this.Document.Variables.SetValue("name", acc.Ac_Name);    //QQ昵称                    
                    this.Document.Variables.SetValue("photo2", acc.Ac_Photo);   //100*100头像
                    this.Document.Variables.SetValue("gender", acc.Ac_Sex);   //性别
                }
            }
            #endregion

            #region ajax请求
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "Direct": _DirectLogin();   //直接登录
                        break;
                    case "getRegSms": sendSmsVcode();  //验证手机注册时，获取短信时的验证码
                        break;
                    case "register1": register1(); //直接注册，无需验证手机号
                        break;
                    case "register2": register2(); //用手机注册，需短信验证手机号
                        break;
                    case "bind1": bind1();  //绑定已经存在账户，不验证手机
                        break;
                    case "bind2": bind2();  //绑定已经存在账户，验证手机号
                        break;
                    default:
                        //acclogin_verify();  //验证账号登录时的密码
                        break;
                }
                Response.End();
            }
            #endregion
        }
        #region 取值方法
        /// <summary>
        /// 获取当前机构
        /// </summary>
        protected Song.Entities.Organization getOrgan()
        {
            Song.Entities.Organization org = null;
            int orgid = WeiSha.Common.Request.QueryString["orgid"].Int32 ?? 0;
            if (orgid < 1) orgid = WeiSha.Common.Request.Cookies["ORGID"].Int32 ?? 0;
            if (orgid > 0) org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) org = Business.Do<IOrganization>().OrganDefault();
            this.Document.Variables.SetValue("org", org);
            return org;
        }
        /// <summary>
        /// 获取当前机构的二级域名
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        protected string getOrganDomain(Song.Entities.Organization org)
        {
            string root = WeiSha.Common.Server.MainName;
            return org.Org_TwoDomain + "." + root;
        }
        /// <summary>
        /// 获取当前登录QQ的详细信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns>xml对象</returns>
        private Song.Entities.Accounts getUserInfo(string access_token, string openid)
        {
            string userUrl = "https://graph.qq.com/user/get_user_info?access_token={0}&oauth_consumer_key={1}&openid={2}";
            string appid = Business.Do<ISystemPara>()["QQAPPID"].String;
            userUrl = string.Format(userUrl, access_token, appid, openid);           
            //解析QQ账户信息
            Song.Entities.Accounts acc = new Entities.Accounts();
            try
            {
                string infoJson = WeiSha.Common.Request.HttpGet(userUrl);
                JObject jo = (JObject)JsonConvert.DeserializeObject(infoJson);
                string ret = jo["ret"] != null ? jo["ret"].ToString() : string.Empty;  //返回码
                string msg = jo["msg"] != null ? jo["msg"].ToString() : string.Empty;  //返回的提示信息
                if (ret == "0")
                {
                    acc.Ac_AccName = acc.Ac_QqOpenID = openid;
                    acc.Ac_Name = jo["nickname"] != null ? jo["nickname"].ToString() : string.Empty;    //姓名
                    string gender = jo["gender"] != null ? jo["gender"].ToString() : string.Empty;
                    acc.Ac_Sex = gender == "男" ? 1 : 2;
                    acc.Ac_Age = jo["year"] != null ? Convert.ToInt32(jo["year"].ToString()) : 0;   //年龄
                    acc.Ac_Photo = jo["figureurl_qq_2"] != null ? jo["figureurl_qq_2"].ToString() : string.Empty;   //头像
                    acc.Org_ID = WeiSha.Common.Request.QueryString["orgid"].Int32 ?? 0; //所属机构
                }
                else
                {
                    this.Document.Variables.SetValue("errcode", ret);
                    this.Document.Variables.SetValue("errmsg", msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return acc;
        }        
        #endregion

        #region 登录方法
        /// <summary>
        /// 如果已经存在账号，直接登录
        /// </summary>
        /// <param name="openid"></param>
        private Song.Entities.Accounts _ExistAcc(string openid)
        {
            //获取帐户，如果已经注册，则直接实现登录
            Song.Entities.Accounts acc = Business.Do<IAccounts>().Account4QQ(openid);
            if (acc != null)
            {
                this.Document.Variables.SetValue("acc", acc);
                //直接实现登录
                if (acc.Ac_IsPass && acc.Ac_IsUse)
                {
                    LoginState.Accounts.Write(acc);
                    Business.Do<IAccounts>().PointAdd4Login(acc, "电脑网页", "QQ登录", "");   //增加登录积分
                    Business.Do<IStudent>().LogForLoginAdd(acc);
                    this.Document.Variables.SetValue("success", "1");   //登录成功
                }
                else
                {
                    this.Document.Variables.SetValue("success", "-1");   //账户禁用中
                }
            }
            return acc;
        }
        /// <summary>
        /// 直接登录
        /// </summary>
        private void _DirectLogin()
        {
            string access_token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.Form["openid"].String;
            Song.Entities.Organization org = getOrgan();
            //获取qq登录账户的信息
            Song.Entities.Accounts acc = Business.Do<IAccounts>().Account4QQ(openid);
            if (acc == null)
            {
                acc = getUserInfo(access_token, openid);
                //头像图片
                string photoPath = Upload.Get["Accounts"].Physics + openid + ".jpg";
                WeiSha.Common.Request.LoadFile(acc.Ac_Photo, photoPath);
                acc.Ac_Photo = openid + ".jpg";
                //获取推荐人
                int recid = WeiSha.Common.Request.Cookies["sharekeyid"].Int32 ?? 0;
                Song.Entities.Accounts accRec = null;
                if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
                if (accRec != null && accRec.Ac_ID != acc.Ac_ID)
                {
                    acc.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                    Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
                }
                //如果需要审核通过                
                acc.Ac_IsPass = acc.Ac_IsUse = true;
                int id = Business.Do<IAccounts>().AccountsAdd(acc);
                LoginState.Accounts.Write(acc);
            }
            string domain = getOrganDomain(org);
            Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"domain\":\"" + domain + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\"}");
        }
        /// <summary>
        /// 注册，但不验证手机号
        /// </summary>
        private void register1()
        {
            string access_token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.Form["openid"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号
            Song.Entities.Accounts acc = null;
            //验证手机号是否存在
            if (!string.IsNullOrWhiteSpace(mobi))
            {
                acc = Business.Do<IAccounts>().IsAccountsExist(-1, mobi, 1);
                if (acc != null)
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号已经存在
                    return;
                }
            }
            //获取qq登录账户的信息
            acc = getUserInfo(access_token, openid);
            acc.Ac_AccName = string.IsNullOrWhiteSpace(mobi) ? openid : mobi;
            acc.Ac_MobiTel1 = acc.Ac_MobiTel2 = mobi;   //手机号
            //头像图片
            string photoPath = Upload.Get["Accounts"].Physics + openid + ".jpg";
            WeiSha.Common.Request.LoadFile(acc.Ac_Photo, photoPath);
            acc.Ac_Photo = openid + ".jpg";
            //获取推荐人
            int recid = WeiSha.Common.Request.Cookies["sharekeyid"].Int32 ?? 0;
            Song.Entities.Accounts accRec = null;
            if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
            if (accRec != null && accRec.Ac_ID != acc.Ac_ID)
            {
                acc.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
            }
            //如果需要审核通过                
            acc.Ac_IsPass = acc.Ac_IsUse = true;
            int id = Business.Do<IAccounts>().AccountsAdd(acc);
            LoginState.Accounts.Write(acc);
            string domain = getOrganDomain(getOrgan());
            Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"domain\":\"" + domain + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\"}");
        }
        /// <summary>
        /// 手机号注册，需短信验证
        /// </summary>
        private void register2()
        {
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            string access_token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.Form["openid"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号
            string sms = WeiSha.Common.Request.Form["sms"].MD5;  //输入的短信验证码  
            //短信验证码Cookie名称
            string smsName = WeiSha.Common.Request.Form["smsname"].String;
            string btnName = WeiSha.Common.Request.Form["smsbtn"].String;
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\",\"btn\":\"" + btnName + "\"}");   //图片验证码不正确
                return;
            }
            //验证手机号是否存在
            Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, mobi, 1);
            if (acc != null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"2\",\"btn\":\"" + btnName + "\"}");   //手机号已经存在
                return;
            }
            //验证短信验证码
            bool isSmsCode = true;      //是否短信验证；
            string smsCode = WeiSha.Common.Request.Cookies[smsName].ParaValue;
            if (isSmsCode && sms != smsCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\",\"btn\":\"" + btnName + "\"}");  //短信验证失败             
                return;
            }
            else
            {
                //获取qq登录账户的信息
                acc = getUserInfo(access_token, openid);
                acc.Ac_AccName = acc.Ac_MobiTel1 = acc.Ac_MobiTel2 = mobi;   //手机号
                //头像图片
                string photoPath = Upload.Get["Accounts"].Physics + openid + ".jpg";
                WeiSha.Common.Request.LoadFile(acc.Ac_Photo, photoPath);
                acc.Ac_Photo = openid + ".jpg";
                //获取推荐人
                int recid = WeiSha.Common.Request.Cookies["sharekeyid"].Int32 ?? 0;
                Song.Entities.Accounts accRec = null;
                if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
                if (accRec != null && accRec.Ac_ID != acc.Ac_ID)
                {
                    acc.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                    Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
                }
                //如果需要审核通过                
                acc.Ac_IsPass = acc.Ac_IsUse = true;
                int id = Business.Do<IAccounts>().AccountsAdd(acc);
                LoginState.Accounts.Write(acc);
                string domain = getOrganDomain(getOrgan());
                Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"domain\":\"" + domain + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\",\"btn\":\"" + btnName + "\"}");
            }
        }
        #endregion

        #region 绑定已经存在账户
        /// <summary>
        /// 绑定，但不验证手机号
        /// </summary>
        private void bind1()
        {
            string access_token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.Form["openid"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号
            string pw = WeiSha.Common.Request.Form["pw"].MD5;    //登录密码
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            Song.Entities.Accounts acc = null;
            //验证手机号是否存在
            if (!string.IsNullOrWhiteSpace(mobi))
            {
                acc = Business.Do<IAccounts>().IsAccountsExist(-1, mobi, 1);
                if (acc == null)
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号不存在
                    return;
                }
                //验证密码
                if (!string.Equals(acc.Ac_Pw, pw, StringComparison.CurrentCultureIgnoreCase))
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"3\"}");   //登录密码不正确
                    return;
                }
                //是否已经绑过QQ的openid
                if (!string.IsNullOrWhiteSpace(acc.Ac_QqOpenID))
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //已经绑定过openid
                    return;
                } 
            }
            //绑定
            if (acc != null)
            {
                acc.Ac_QqOpenID = openid;
                Business.Do<IAccounts>().AccountsSave(acc);
                LoginState.Accounts.Write(acc);
                //登录成功
                Business.Do<IAccounts>().PointAdd4Login(acc, "电脑网页", "QQ登录", "");   //增加登录积分
                string domain = getOrganDomain(getOrgan());
                Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"domain\":\"" + domain + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\"}");
            }
        }
        /// <summary>
        /// 绑定已经存在账户，需要短信验证
        /// </summary>
        private void bind2()
        {
            string access_token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.Form["openid"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            string sms = WeiSha.Common.Request.Form["sms"].MD5;  //输入的短信验证码    
            //短信验证码Cookie名称
            string smsName = WeiSha.Common.Request.Form["smsname"].String;
            string btnName = WeiSha.Common.Request.Form["smsbtn"].String;
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\",\"btn\":\"" + btnName + "\"}");   //图片验证码不正确
                return;
            }
            //验证短信验证码
            bool isSmsCode = true;      //是否短信验证；
            string smsCode = WeiSha.Common.Request.Cookies[smsName].ParaValue;
            if (isSmsCode && sms != smsCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\",\"btn\":\"" + btnName + "\"}");  //短信验证失败             
                return;
            }
            Song.Entities.Accounts acc = null;
            //验证手机号是否存在
            if (!string.IsNullOrWhiteSpace(mobi))
            {
                acc = Business.Do<IAccounts>().IsAccountsExist(-1, mobi, 1);
                if (acc == null)
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"2\",\"btn\":\"" + btnName + "\"}");   //手机号不存在
                    return;
                }
                //是否已经绑过QQ的openid
                if (!string.IsNullOrWhiteSpace(acc.Ac_QqOpenID))
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //已经绑定过openid
                    return;
                } 
            }
            //绑定
            if (acc != null)
            {
                acc.Ac_QqOpenID = openid;
                Business.Do<IAccounts>().AccountsSave(acc);
                LoginState.Accounts.Write(acc);
                //登录成功
                Business.Do<IAccounts>().PointAdd4Login(acc, "电脑网页", "QQ登录", "");   //增加登录积分
                string domain = getOrganDomain(getOrgan());
                Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"domain\":\"" + domain + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\",\"btn\":\"" + btnName + "\"}");
            }
        }
        #endregion

        #region 手机短信验证
        /// <summary>
        /// 获取短信之间的验证
        /// </summary>
        private void sendSmsVcode()
        {
            //取图片验证码
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            //输入的手机号
            string phone = WeiSha.Common.Request.Form["phone"].String;
            //短信验证码Cookie名称
            string smsName = WeiSha.Common.Request.Form["smsname"].String;
            string btnName = WeiSha.Common.Request.Form["smsbtn"].String;
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\",\"btn\":\"" + btnName + "\"}");   //图片验证码不正确
                return;
            }
            //发送短信验证码
            try
            {
                bool success = Business.Do<ISMS>().SendVcode(phone, smsName);
                //bool success = true;
                if (success) Response.Write("{\"success\":\"1\",\"state\":\"0\",\"btn\":\"" + btnName + "\"}");  //短信发送成功  
               
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\",\"btn\":\"" + btnName + "\",\"desc\":\"" + ex.Message + "\"}");  //短信发送失败   
            }
        }        
        #endregion
    }
}