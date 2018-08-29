using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using System.Reflection;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 学员注册(手机端）
    /// </summary>
    public class Register : BasePage
    {
        //状态代码
        int state = WeiSha.Common.Request.QueryString["state"].Int16 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
                this.Document.SetValue("IsRegStudent", config["IsRegStudent"].Value.Boolean ?? true);   //是否允许注册
                this.Document.SetValue("IsRegSms", config["IsRegSms"].Value.Boolean ?? true);    //是否要短信验证
                //来源
                string from = WeiSha.Common.Request.Form["from"].String;
                string referrer = context.Request.UrlReferrer == null ? "" : context.Request.UrlReferrer.ToString();
                from = string.IsNullOrWhiteSpace(from) ? referrer : from;
                this.Document.SetValue("from", from);
            }
            //状态
            this.Document.SetValue("state", state);
            if (state == 1)
            {
                //将账号信息写入
                Song.Entities.Accounts emp = Extend.LoginState.Accounts.CurrentUser;
                if (emp != null)
                {
                    this.Document.SetValue("accid", emp.Ac_ID);
                    this.Document.SetValue("accpw", emp.Ac_Pw);
                }
                return;
            }

            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "getSms":
                        mobivcode_verify();  //验证手机登录时，获取短信时的验证码
                        break;
                    case "mobiregister":
                        mobiregister_verify();   //用手机号注册
                        break;
                }
                Response.End();
            }
        }
        /// <summary>
        /// 获取短信之前的验证
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
            if (acc != null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号已经存在
                return;
            }
            //发送短信验证码
            try
            {
                bool success = Business.Do<ISMS>().SendVcode(phone, "reg_mobi_" + vname);
                //bool success = true;
                if (success) Response.Write("{\"success\":\"1\",\"state\":\"0\"}");  //短信发送成功                
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\",\"desc\":\"" + ex.Message + "\"}");  //短信发送失败   
            }
        }
        /// <summary>
        /// 手机注册的验证
        /// </summary>
        private void mobiregister_verify()
        {
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["tbCode"].MD5;  //取输入的验证码
            string phone = WeiSha.Common.Request.Form["Ac_MobiTel1"].String;  //输入的手机号
            string sms = WeiSha.Common.Request.Form["tbSms"].MD5;  //输入的短信验证码          
            string rec = WeiSha.Common.Request.Form["rec"].String;  //推荐人的电话
            int recid = WeiSha.Common.Request.Form["recid"].Int32 ?? 0;  //推荐人的账户id
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            //验证手机号是否存在
            Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, phone, 1);
            if (acc != null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号已经存在
                return;
            }
            //验证短信验证码
            bool isSmsCode = true;      //是否短信验证；
            WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
            isSmsCode = config["IsRegSms"].Value.Boolean ?? true;
            string smsCode = WeiSha.Common.Request.Cookies["reg_mobi_" + vname].ParaValue;
            if (isSmsCode && sms != smsCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\"}");  //短信验证失败             
                return;
            }
            else
            {
                //创建新账户
                Song.Entities.Accounts tmp = new Entities.Accounts();
                tmp = fillData(tmp);
                tmp.Ac_AccName = tmp.Ac_MobiTel1;
                //获取推荐人
                Song.Entities.Accounts accRec = null;
                if (!string.IsNullOrWhiteSpace(rec)) accRec = Business.Do<IAccounts>().AccountsSingle(rec, true, true);
                if (accRec == null && recid > 0) accRec = Business.Do<IAccounts>().AccountsSingle(recid);
                if (accRec != null && accRec.Ac_ID != tmp.Ac_ID)
                {
                    tmp.Ac_PID = accRec.Ac_ID;  //设置推荐人，即：当前注册账号为推荐人的下线                   
                    Business.Do<IAccounts>().PointAdd4Register(accRec);   //增加推荐人积分
                }
                //如果需要审核通过
                tmp.Ac_IsPass = !(bool)(config["IsVerifyStudent"].Value.Boolean ?? true);
                tmp.Ac_IsUse = tmp.Ac_IsPass;
                int id = Business.Do<IAccounts>().AccountsAdd(tmp);               
                //以下为判断是否审核通过
                if (tmp.Ac_IsPass)
                {
                    LoginState.Accounts.Write(tmp);
                    Response.Write("{\"success\":\"1\",\"name\":\"" + tmp.Ac_Name + "\",\"state\":\"1\"}");
                }
                else
                {
                    //注册成功，但待审核
                    Response.Write("{\"success\":\"1\",\"name\":\"" + tmp.Ac_Name + "\",\"state\":\"0\"}");
                }
            }
        }

        /// <summary>
        /// 将注册信息填充到实体
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        private Song.Entities.Accounts fillData(Song.Entities.Accounts acc)
        {
            //遍历实体属性
            Type info = acc.GetType();
            PropertyInfo[] properties = info.GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                string value = WeiSha.Common.Request.Form[pi.Name].String;
                if (pi.Name == "Ac_Pw") value = WeiSha.Common.Request.Form[pi.Name].MD5;
                if (string.IsNullOrWhiteSpace(value)) continue;
                //获取值，转的成属性的数据类型，并赋值
                var property = acc.GetType().GetProperty(pi.Name);
                object tm = string.IsNullOrEmpty(value) ? null : WeiSha.Common.DataConvert.ChangeType(value, property.PropertyType);
                property.SetValue(acc, tm, null);
            }
            return acc;
        }
    }
}