using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using System.Text.RegularExpressions;
namespace Song.Site.Teacher
{
    /// <summary>
    /// 教师注册
    /// </summary>
    public class Register : BasePage
    {
        //操作步聚
        int step = WeiSha.Common.Request.QueryString["step"].Int16 ?? 1;
        protected override void InitPageTemplate(HttpContext context)
        {
            //是否允许注册
            WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
            this.Document.SetValue("IsRegTeacher", config["IsRegTeacher"].Value.Boolean ?? true);
            //操作步骤
            this.Document.SetValue("step", step);
            //注册协议
            if (step == 1)
            {
                string agreement = Business.Do<ISystemPara>().GetValue("Agreement_teacher");
                if (!string.IsNullOrWhiteSpace(agreement))
                {
                    agreement = Regex.Replace(agreement, "{platform}", this.Organ.Org_PlatformName, RegexOptions.IgnoreCase);
                    agreement = Regex.Replace(agreement, "{org}", this.Organ.Org_AbbrName, RegexOptions.IgnoreCase);
                    agreement = Regex.Replace(agreement, "{domain}", WeiSha.Common.Request.Domain.MainName, RegexOptions.IgnoreCase);
                }
                this.Document.SetValue("Agreement", agreement);
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
            Song.Entities.Accounts curr = Extend.LoginState.Accounts.CurrentUser;
            if (curr == null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //当前账号未登录；
                return;
            }
            if (phone != curr.Ac_MobiTel1 && phone != curr.Ac_MobiTel2)
            {
                Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, phone, 1);
                if (acc != null)
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号已经存在
                    return;
                }
            }
            //发送短信验证码
            try
            {
                bool success = Business.Do<ISMS>().SendVcode(phone, "reg_mobi_" + vname);
                if (success) Response.Write("{\"success\":\"1\",\"state\":\"0\"}");  //短信发送成功   
            }
            catch (Exception ex)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\",\"desc\":\"" + ex.Message + "\"}");  //短信发送失败   
            }
        }
        /// <summary>
        /// 教师注册的验证
        /// </summary>
        private void mobiregister_verify()
        {
            string vname = WeiSha.Common.Request.Form["vname"].String;
            string imgCode = WeiSha.Common.Request.Cookies[vname].ParaValue;    //取图片验证码
            string userCode = WeiSha.Common.Request.Form["vcode"].MD5;  //取输入的验证码
            string phone = WeiSha.Common.Request.Form["phone"].String;  //输入的手机号
            string sms = WeiSha.Common.Request.Form["sms"].MD5;  //输入的短信验证码       
            string name = WeiSha.Common.Request.Form["name"].String;     //姓名
            string email = WeiSha.Common.Request.Form["email"].String;     //邮箱
            string qq = WeiSha.Common.Request.Form["qq"].String;  //qq
            string idcard = WeiSha.Common.Request.Form["idcard"].String;  //身份证号
            string intro = WeiSha.Common.Request.Form["intro"].Text;  //简介
            //验证图片验证码
            if (imgCode != userCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"1\"}");   //图片验证码不正确
                return;
            }
            //验证手机号是否存在
            Song.Entities.Accounts curr = Extend.LoginState.Accounts.CurrentUser;
            if (curr == null)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"4\"}");   //当前账号未登录；
                return;
            }
            if (phone != curr.Ac_MobiTel1 && phone != curr.Ac_MobiTel2)
            {
                Song.Entities.Accounts acc = Business.Do<IAccounts>().IsAccountsExist(-1, phone, 1);
                if (acc != null)
                {
                    Response.Write("{\"success\":\"-1\",\"state\":\"2\"}");   //手机号已经存在
                    return;
                }
            }
            //验证短信验证码
            bool isSmsCode = true;      //是否短信验证；
            string smsCode = WeiSha.Common.Request.Cookies["reg_mobi_" + vname].ParaValue;
            if (isSmsCode && sms != smsCode)
            {
                Response.Write("{\"success\":\"-1\",\"state\":\"3\"}");  //短信验证失败             
                return;
            }
            else
            {                
                Song.Entities.Accounts tmp = Extend.LoginState.Accounts.CurrentUser;
                tmp.Ac_IsTeacher = true;    //当前账户有了教师身份
                tmp.Ac_MobiTel2 = phone;
                Business.Do<IAccounts>().AccountsSave(tmp);
                //创建教师账户
                Song.Entities.Teacher th = Business.Do<IAccounts>().GetTeacher(curr.Ac_ID, null);
                if (th == null) th = new Entities.Teacher();
                th.Ac_ID = tmp.Ac_ID;       //关联学员账户
                th.Ac_UID = tmp.Ac_UID;
                th.Th_PhoneMobi = phone;    //教师手机号，基本账号中已经有记录，此处再记一次
                th.Th_Name = name;      //教师的名称
                th.Th_IDCardNumber = idcard;    //教师的身份证号
                th.Th_Email = email;
                th.Th_Qq = qq;
                th.Th_Intro = intro;    //教师的简介
              
                //如果需要审核通过
                WeiSha.Common.CustomConfig config = CustomConfig.Load(this.Organ.Org_Config);
                th.Th_IsPass = !(bool)(config["IsVerifyTeahcer"].Value.Boolean ?? true);
                th.Th_IsUse = th.Th_IsPass;
                if (th.Th_ID < 1)
                {
                    int id = Business.Do<ITeacher>().TeacherAdd(th);
                }
                else
                {
                    Business.Do<ITeacher>().TeacherSave(th);
                }

                //注册成功，以下为判断是否审核通过
                if (th.Th_IsPass)
                {
                    LoginState.Accounts.Write(tmp);
                    Response.Write("{\"success\":\"1\",\"name\":\"" + th.Th_Name + "\",\"state\":\"1\"}");
                }
                else
                {
                    //注册成功，但待审核
                    Response.Write("{\"success\":\"1\",\"name\":\"" + th.Th_Name + "\",\"state\":\"0\"}");
                }
            }
        }

        
    }
}