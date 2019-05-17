using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using System.Xml;
using Song.Extend;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 微信公从号内的登录
    /// </summary>
    public class WeixinpubLogin : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {
            #region 此段代码用于取token与openid
            string code = WeiSha.Common.Request.QueryString["code"].String;
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET" && !string.IsNullOrWhiteSpace(code))
            {
                string orgid = WeiSha.Common.Request.QueryString["state"].String;     //状码码是机构id                
                string openid = string.Empty;
                string access_token = getToken(out openid);
                //微信登录的回调域，（用途：如果回调域不是根域，则不再取当前机构二级域名)
                string returl = Business.Do<ISystemPara>()["WeixinpubReturl"].Value ?? WeiSha.Common.Server.MainName;
                if (!string.IsNullOrWhiteSpace(returl))
                {
                    if (returl.StartsWith("http://")) returl = returl.Substring(7);
                    if (returl.StartsWith("https://")) returl = returl.Substring(8);
                }               
                //如果回调域与根域相同，则转到当前机构的二级域名
                if (returl.Equals(WeiSha.Common.Server.MainName, StringComparison.CurrentCultureIgnoreCase))
                {
                    Song.Entities.Organization org = getOrgan(-1);
                    returl = org.Org_TwoDomain + "." + WeiSha.Common.Server.MainName;
                }                
                string uri = "{0}/mobile/{1}?token={2}&openid={3}&orgid={4}";
                uri = string.Format(uri, returl, WeiSha.Common.Request.Page.FileName, access_token, openid, orgid);
                this.Document.Variables.SetValue("gourl", uri); //传到客户端，进行跳转
                return;
            }
            #endregion

            #region 具体操作代码
            string token = WeiSha.Common.Request.QueryString["token"].String;
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET" && !string.IsNullOrWhiteSpace(token))
            {
                _StateForGET();
            }
            #endregion
          
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "Direct": _directLogin();   //直接登录
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
        }
        #region 取值方法
        /// <summary>
        /// 通过code获取access_token的接口
        /// </summary>
        /// <returns></returns>
        protected string getToken(out string openid)
        {
            string code = WeiSha.Common.Request.QueryString["code"].String;     //用户登录后，微信平台返回的代码
            string appid = Business.Do<ISystemPara>()["WeixinpubAPPID"].String;    //应用id
            string secret = Business.Do<ISystemPara>()["WeixinpubSecret"].String;  //密钥
            //
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            url = string.Format(url, appid, secret, code);
            string access_token = string.Empty;
            openid = "";
            try
            {
                string retjson = WeiSha.Common.Request.HttpGet(url);
                JObject jo = (JObject)JsonConvert.DeserializeObject(retjson);
                string errcode = jo["errcode"] != null ? jo["errcode"].ToString() : string.Empty;  //错误代码
                string errmsg = jo["errmsg"] != null ? jo["errmsg"].ToString() : string.Empty;
                if (!string.IsNullOrWhiteSpace(errcode))
                {
                    this.Document.Variables.SetValue("errcode", errcode);
                    this.Document.Variables.SetValue("errmsg", errmsg);
                }
                //返回正确
                access_token = jo["access_token"] != null ? jo["access_token"].ToString() : string.Empty;  //接口调用凭证
                openid = jo["openid"] != null ? jo["openid"].ToString() : string.Empty; ;    //授权用户唯一标识
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return access_token;
        }
        /// <summary>
        /// 获取当前机构
        /// </summary>
        protected Song.Entities.Organization getOrgan(int orgid)
        {
            Song.Entities.Organization org = null;
            if (orgid <= 0) orgid = WeiSha.Common.Request.QueryString["orgid"].Int32 ?? 0;
            if (orgid < 1) orgid = WeiSha.Common.Request.Cookies["ORGID"].Int32 ?? 0;
            if (orgid > 0) org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) org = Business.Do<IOrganization>().OrganDefault();
            this.Document.Variables.SetValue("org", org);
            return org;
        }
        /// <summary>
        /// 获取当前登录微信账号的详细信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <returns>xml对象</returns>
        private Song.Entities.Accounts getUserInfo(string access_token, string openid,out string unionid)
        {
            string userUrl = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
            //string userUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
            userUrl = string.Format(userUrl, access_token, openid);
            string retjson = WeiSha.Common.Request.HttpGet(userUrl);
            unionid = string.Empty;
            //解析微信账户信息
            Song.Entities.Accounts acc = null;
            JObject jo = (JObject)JsonConvert.DeserializeObject(retjson);
            string errcode = jo["errcode"] != null ? jo["errcode"].ToString() : string.Empty;  //错误代码
            string errmsg = jo["errmsg"] != null ? jo["errmsg"].ToString() : string.Empty;
            if (!string.IsNullOrWhiteSpace(errcode))
            {
                this.Document.Variables.SetValue("errcode", errcode);
                this.Document.Variables.SetValue("errmsg", errmsg);
                return acc;
            }
             if (string.IsNullOrEmpty(errcode))
            {
                acc = new Entities.Accounts();
                acc.Ac_Name = jo["nickname"] != null ? jo["nickname"].ToString() : string.Empty;    //昵称
                acc.Ac_Sex = jo["sex"] != null ? Convert.ToInt16(jo["sex"].ToString()) : 0;         //性别，1为男，2为女
                acc.Ac_Photo = jo["headimgurl"] != null ? jo["headimgurl"].ToString() : string.Empty;   //用户头像
                //取132的头像
                if (acc.Ac_Photo.IndexOf("/") > -1)
                    acc.Ac_Photo = acc.Ac_Photo.Substring(0, acc.Ac_Photo.LastIndexOf("/")+1) + "132";
                unionid = jo["unionid"] != null ? jo["unionid"].ToString() : string.Empty;    //unionid                
                acc.Ac_WeixinOpenID = unionid;
            }
            return acc;
        }
        #endregion
        #region 初始登录时，如果已经注册则直接进入
        /// <summary>
        /// get请求时
        /// </summary>
        protected void _StateForGET()
        {
            string token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.QueryString["openid"].String;
            this.Document.Variables.SetValue("openid", openid);
            this.Document.Variables.SetValue("token", token);
            //WeiSha.Common.Log.Write(string.Format("再次登录时，取token:{0},openid:{1}", token, openid));
            //设置主域，用于js跨根域
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) 
                this.Document.Variables.SetValue("domain", WeiSha.Common.Server.MainName);
            //当前机构
            Song.Entities.Organization org = getOrgan(-1);
            this.Document.Variables.SetValue("org", org);
            //WeiSha.Common.Log.Write(string.Format("再次登录时，机构{0}",org.Org_Name));
            //设置主域，用于js跨根域
            if (multi == 0 && !WeiSha.Common.Server.IsLocalIP)
                this.Document.Variables.SetValue("domain", WeiSha.Common.Server.MainName);
            this.Document.SetValue("domain2", org.Org_TwoDomain + "." + WeiSha.Common.Server.MainName);
            //获取帐户，如果已经注册，则直接实现登录
            string unionid = string.Empty;
            Song.Entities.Accounts acctm = getUserInfo(token, openid, out unionid);
            Song.Entities.Accounts acc = null;
            if (acctm != null && !string.IsNullOrWhiteSpace(unionid))
            {
                acc = Business.Do<IAccounts>().Account4Weixin(unionid);
            }
            if (acc != null)
            {
                this.Document.Variables.SetValue("acc", acc);
                //直接实现登录
                if (acc.Ac_IsPass && acc.Ac_IsUse)
                {
                    LoginState.Accounts.Write(acc);
                    Business.Do<IAccounts>().PointAdd4Login(acc, "手机网页", "微信登录", "");   //增加登录积分
                    Business.Do<IStudent>().LogForLoginAdd(acc);
                    this.Document.Variables.SetValue("success", "1");   //登录成功
                }
                else
                {
                    this.Document.Variables.SetValue("success", "-1");   //账户禁用中
                }
            }
            else
            {
                //账户不存在，以下用于注册
                //相关参数
                WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
                //登录方式
                bool IsLoginForPw = config["IsLoginForPw"].Value.Boolean ?? true;    //启用账号密码登录
                bool IsLoginForSms = config["IsLoginForSms"].Value.Boolean ?? true;  //启用手机短信验证登录
                this.Document.SetValue("forpw", IsLoginForPw);
                this.Document.SetValue("forsms", IsLoginForSms);
                this.Document.SetValue("IsWeixinDirect", Business.Do<ISystemPara>()["WeixinDirectIs"].Boolean ?? true); //是否允许微信直接注册登录
                //获取qq登录账户的信息
                if (acctm != null)
                {
                    this.Document.Variables.SetValue("name", acctm.Ac_Name);    //QQ昵称                    
                    this.Document.Variables.SetValue("photo", acctm.Ac_Photo);   //40*40头像
                    this.Document.Variables.SetValue("gender", acctm.Ac_Sex);   //性别
                }
                this.Document.Variables.SetValue("acctm", acctm);
            }
        }
        #endregion
        

        #region 注册登录的方法
        /// <summary>
        /// 没有注册，直接通过微信注册并登录
        /// </summary>
        private void _directLogin()
        {
            string openid = WeiSha.Common.Request.QueryString["openid"].String;
            string token = WeiSha.Common.Request.QueryString["token"].String;
            int sex = WeiSha.Common.Request.Form["sex"].Int16 ?? 0;
            string name = WeiSha.Common.Request.Form["name"].String;
            string photo = WeiSha.Common.Request.Form["photo"].String;
            Song.Entities.Organization org = getOrgan(-1);           
            //创建新账户
            //获取微信登录账户的信息
            string unionid = string.Empty;
            Song.Entities.Accounts tmp = getUserInfo(token, openid, out unionid);
            if (tmp != null && !string.IsNullOrWhiteSpace(unionid))
            {
                tmp.Ac_AccName = unionid;
                tmp.Org_ID = org.Org_ID;
                //头像图片           
                string photoPath = Upload.Get["Accounts"].Physics + unionid + ".jpg";
                WeiSha.Common.Request.LoadFile(photo, photoPath);
                tmp.Ac_Photo = unionid + ".jpg";
                //获取推荐人
                int recid = WeiSha.Common.Request.Cookies["sharekeyid"].Int32 ?? 0;
                Song.Entities.Accounts accRec = null;
                if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
                if (accRec != null && accRec.Ac_ID != tmp.Ac_ID)
                {
                    tmp.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                    Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
                }
                //如果需要审核通过                
                tmp.Ac_IsPass = tmp.Ac_IsUse = true;
                int id = Business.Do<IAccounts>().AccountsAdd(tmp);
                LoginState.Accounts.Write(tmp);
                Response.Write("{\"success\":\"1\",\"name\":\"" + tmp.Ac_Name + "\",\"acpw\":\"" + tmp.Ac_Pw + "\",\"acid\":\"" + tmp.Ac_ID + "\",\"state\":\"1\"}");
            }
            else
            {
                //没有获取到unionid，可能是公众号没有绑定微信开放平台
                Response.Write("{\"success\":\"-1\",\"state\":\"100\"}");
            }
           
        }
        /// <summary>
        /// 注册，但不验证手机号
        /// </summary>
        private void register1()
        {
            string token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.QueryString["openid"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号            
            int sex = WeiSha.Common.Request.Form["sex"].Int16 ?? 0;
            string name = WeiSha.Common.Request.Form["name"].String;
            string photo = WeiSha.Common.Request.Form["photo"].String;
            //验证手机号是否存在
            if (!string.IsNullOrWhiteSpace(mobi))
            {
                Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, mobi, 1);
                if (acc != null)
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号已经存在
                    return;
                }                
            }
            Song.Entities.Organization org = getOrgan(-1);           
            //创建新账户
            string unionid = string.Empty;
            Song.Entities.Accounts tmp = getUserInfo(token, openid, out unionid);
            if (tmp != null && !string.IsNullOrWhiteSpace(unionid))
            {
                tmp.Ac_AccName = string.IsNullOrWhiteSpace(mobi) ? openid : mobi;
                tmp.Ac_MobiTel1 = tmp.Ac_MobiTel2 = mobi;   //手机号
                tmp.Org_ID = org.Org_ID;
                //头像图片           
                string photoPath = Upload.Get["Accounts"].Physics + unionid + ".jpg";
                WeiSha.Common.Request.LoadFile(photo, photoPath);
                tmp.Ac_Photo = unionid + ".jpg";
                //获取推荐人
                int recid = WeiSha.Common.Request.Cookies["sharekeyid"].Int32 ?? 0;
                Song.Entities.Accounts accRec = null;
                if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
                if (accRec != null && accRec.Ac_ID != tmp.Ac_ID)
                {
                    tmp.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                    Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
                }
                //如果需要审核通过                
                tmp.Ac_IsPass = tmp.Ac_IsUse = true;
                int id = Business.Do<IAccounts>().AccountsAdd(tmp);
                LoginState.Accounts.Write(tmp);
                Response.Write("{\"success\":\"1\",\"name\":\"" + tmp.Ac_Name + "\",\"acpw\":\"" + tmp.Ac_Pw + "\",\"acid\":\"" + tmp.Ac_ID + "\",\"state\":\"1\"}");
            }
            else
            {
                //没有获取到unionid，可能是公众号没有绑定微信开放平台
                Response.Write("{\"success\":\"-1\",\"state\":\"100\"}");
            }
        }
        /// <summary>
        /// 手机号注册，需短信验证
        /// </summary>
        private void register2()
        {
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            string openid = WeiSha.Common.Request.QueryString["openid"].String;
            string token = WeiSha.Common.Request.QueryString["token"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号
            string sms = WeiSha.Common.Request.Form["sms"].MD5;  //输入的短信验证码  
            int sex = WeiSha.Common.Request.Form["sex"].Int16 ?? 0;
            string name = WeiSha.Common.Request.Form["name"].String;
            string photo = WeiSha.Common.Request.Form["photo"].String;
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
                //创建新账户
                string unionid = string.Empty;
                Song.Entities.Accounts tmp = getUserInfo(token, openid,out unionid);
                if (tmp != null && !string.IsNullOrWhiteSpace(unionid))
                {
                    Song.Entities.Organization org = getOrgan(-1);
                    tmp.Ac_AccName = string.IsNullOrWhiteSpace(unionid) ? mobi : unionid;
                    tmp.Ac_MobiTel1 = tmp.Ac_MobiTel2 = mobi;   //手机号
                    tmp.Org_ID = org.Org_ID;
                    //头像图片           
                    string photoPath = Upload.Get["Accounts"].Physics + unionid + ".jpg";
                    WeiSha.Common.Request.LoadFile(photo, photoPath);
                    tmp.Ac_Photo = unionid + ".jpg";
                    //获取推荐人
                    int recid = WeiSha.Common.Request.Cookies["sharekeyid"].Int32 ?? 0;
                    Song.Entities.Accounts accRec = null;
                    if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
                    if (accRec != null && accRec.Ac_ID != tmp.Ac_ID)
                    {
                        tmp.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                        Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
                    }
                    //如果需要审核通过                
                    tmp.Ac_IsPass = tmp.Ac_IsUse = true;
                    int id = Business.Do<IAccounts>().AccountsAdd(tmp);
                    LoginState.Accounts.Write(tmp);
                    //string domain = getOrganDomain(org);
                    Response.Write("{\"success\":\"1\",\"name\":\"" + tmp.Ac_Name + "\",\"acpw\":\"" + tmp.Ac_Pw + "\",\"acid\":\"" + tmp.Ac_ID + "\",\"state\":\"1\",\"btn\":\"" + btnName + "\"}");
                }
                else
                {
                    //没有获取到unionid，可能是公众号没有绑定微信开放平台
                    Response.Write("{\"success\":\"-1\",\"state\":\"100\"}");
                }
            }
        }
        #endregion

        #region 绑定已经存在账户
        /// <summary>
        /// 绑定，但不验证手机号
        /// </summary>
        private void bind1()
        {
            string token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.QueryString["openid"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号
            string pw = WeiSha.Common.Request.Form["pw"].MD5;    //登录密码
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            int sex = WeiSha.Common.Request.Form["sex"].Int16 ?? 0;
            string name = WeiSha.Common.Request.Form["name"].String;
            string photo = WeiSha.Common.Request.Form["photo"].String;
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            string unionid = string.Empty;
            Song.Entities.Accounts acctm = getUserInfo(token, openid, out unionid);
            if (acctm != null && !string.IsNullOrWhiteSpace(unionid))
            {
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
                    //是否已经绑过微信的openid
                    if (!string.IsNullOrWhiteSpace(acc.Ac_WeixinOpenID))
                    {
                        Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //已经绑定过openid
                        return;
                    }
                }
                //绑定
                if (acc != null)
                {
                    if (string.IsNullOrWhiteSpace(acc.Ac_WeixinOpenID))
                    {
                        acc.Ac_WeixinOpenID = unionid;
                        //用户头像，如果没有上传，或图片不存在
                        if (string.IsNullOrEmpty(acc.Ac_Photo) || acc.Ac_Photo.Trim() == "" || !System.IO.File.Exists(Upload.Get["Accounts"].Physics + acc.Ac_Photo))
                        {
                            string photoPath = Upload.Get["Accounts"].Physics + unionid + ".jpg";
                            WeiSha.Common.Request.LoadFile(photo, photoPath);
                            acc.Ac_Photo = unionid + ".jpg";
                        }
                        Business.Do<IAccounts>().AccountsSave(acc);
                        LoginState.Accounts.Write(acc);
                        //登录成功
                        Business.Do<IAccounts>().PointAdd4Login(acc, "电脑网页", "微信登录", "");   //增加登录积分
                        //string domain = getOrganDomain(this.getOrgan(-1));
                        Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"acpw\":\"" + acc.Ac_Pw + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\"}");
                    }
                    else
                    {
                        Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //已经绑定过微信号
                    }
                }
            }
            else
            {
                //没有获取到unionid，可能是公众号没有绑定微信开放平台
                Response.Write("{\"success\":\"-1\",\"state\":\"100\"}");
            }
        }
        /// <summary>
        /// 绑定已经存在账户，需要短信验证
        /// </summary>
        private void bind2()
        {
            string token = WeiSha.Common.Request.QueryString["token"].String;
            string openid = WeiSha.Common.Request.QueryString["openid"].String;
            string mobi = WeiSha.Common.Request.Form["mobi"].String;    //手机号
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            string sms = WeiSha.Common.Request.Form["sms"].MD5;  //输入的短信验证码  
            string photo = WeiSha.Common.Request.Form["photo"].String;
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
            string unionid = string.Empty;
            Song.Entities.Accounts acctm = getUserInfo(token, openid, out unionid);
            if (acctm != null && !string.IsNullOrWhiteSpace(unionid))
            {
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
                    //是否已经绑过微信的openid
                    if (!string.IsNullOrWhiteSpace(acc.Ac_WeixinOpenID))
                    {
                        Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //已经绑定过openid
                        return;
                    }
                }
                //绑定
                if (acc != null)
                {
                    if (string.IsNullOrWhiteSpace(acc.Ac_WeixinOpenID))
                    {
                        acc.Ac_WeixinOpenID = unionid;
                        //用户头像，如果没有上传，或图片不存在
                        if (string.IsNullOrEmpty(acc.Ac_Photo) || acc.Ac_Photo.Trim() == "" || !System.IO.File.Exists(Upload.Get["Accounts"].Physics + acc.Ac_Photo))
                        {
                            string photoPath = Upload.Get["Accounts"].Physics + unionid + ".jpg";
                            WeiSha.Common.Request.LoadFile(photo, photoPath);
                            acc.Ac_Photo = unionid + ".jpg";
                        }
                        Business.Do<IAccounts>().AccountsSave(acc);
                        LoginState.Accounts.Write(acc);
                        //登录成功
                        Business.Do<IAccounts>().PointAdd4Login(acc, "电脑网页", "微信登录", "");   //增加登录积分
                        //string domain = getOrganDomain(getOrgan(-1));
                        Response.Write("{\"success\":\"1\",\"name\":\"" + acc.Ac_Name + "\",\"acpw\":\"" + acc.Ac_Pw + "\",\"acid\":\"" + acc.Ac_ID + "\",\"state\":\"1\",\"btn\":\"" + btnName + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //已经绑定过微信号
                    }
                }
            }
            else
            {
                //没有获取到unionid，可能是公众号没有绑定微信开放平台
                Response.Write("{\"success\":\"-1\",\"state\":\"100\"}");
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