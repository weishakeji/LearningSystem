using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Web;



namespace Song.ServiceImpls
{
    public class SMSCom : ISMS
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void MessageAdd(SmsMessage entity)
        {
            entity.Sms_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<SmsMessage>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void MessageSave(SmsMessage entity)
        {
            Gateway.Default.Save<SmsMessage>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void MessageDelete(SmsMessage entity)
        {
            Gateway.Default.Delete<SmsMessage>(SmsMessage._.SMS_Id == entity.SMS_Id);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void MessageDelete(int identify)
        {
            Gateway.Default.Delete<SmsMessage>(SmsMessage._.SMS_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public SmsMessage GetSingle(int identify)
        {
            return Gateway.Default.From<SmsMessage>().Where(SmsMessage._.SMS_Id == identify).ToFirst<SmsMessage>();
        }
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="keyname">写入cookis的key值名称</param>
        public bool SendVcode(string phone, string keyname)
        {           
            //获取短信接口
            string smsCurr = Business.Do<ISystemPara>().GetValue("SmsCurrent");
            WeiSha.SMS.ISMS sms = WeiSha.SMS.Gatway.GetService(smsCurr);
            sms.Current.User = Business.Do<ISystemPara>().GetValue(smsCurr + "SmsAcc");           
            //生成短信内容
            string rnd = WeiSha.Common.Request.Random(6, 1);    //验证码，随机四位数字
            string msg = Business.Do<ISystemPara>().GetValue(smsCurr + "_SmsTemplate");
            msg = this.MessageFormat(msg, rnd);
            if (string.IsNullOrWhiteSpace(msg)) return false;
            //发送状态
            try
            {
                //密码
                string smspw = Business.Do<ISystemPara>().GetValue(smsCurr + "SmsPw");
                smspw = WeiSha.Common.DataConvert.DecryptForBase64(smspw);    //将密码解密
                sms.Current.Password = smspw;
                //发送短信，phone手机号,msg是短信内容
                WeiSha.SMS.SmsState state = sms.Send(phone, msg);
                HttpContext context = System.Web.HttpContext.Current;
                if (state.Success)
                {
                    //存储随机数
                    HttpCookie cookie = new HttpCookie(keyname);
                    //将随机字符转用ＭＤ５加密
                    cookie.Value = new WeiSha.Common.Param.Method.ConvertToAnyValue(rnd).MD5;
                    //如果是多机构，又不用IP访问，则用根域写入cookie
                    int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
                    if (multi == 0 && !WeiSha.Common.Server.IsLocalIP) cookie.Domain = WeiSha.Common.Server.MainName;
                    cookie.Expires = DateTime.Now.AddMinutes(10);
                    context.Response.Cookies.Add(cookie);
                    return true;
                }
                else
                {
                    throw new Exception(state.Description + "；状态码" + state.Code);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 格式化短信内容，将一些替换符转成实际内容
        /// </summary>
        /// <param name="msg">短信内容。其中包括的替代符：{vcode}验证码，{platform}平台名称，{org}机构简称,{date}时间。</param>
        /// <param name="rnd">随机字符</param>
        /// <returns></returns>
        public string MessageFormat(string msg, string rnd)
        {
            //如果短信模板中不包括{vcode},则返回空
            if (msg.ToLower().IndexOf("{vcode}") < -1) return "";
            msg = Regex.Replace(msg, "{vcode}", rnd.ToString(), RegexOptions.IgnoreCase);
            //当前机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            msg = Regex.Replace(msg, "{plate}", org.Org_PlatformName, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{org}", org.Org_AbbrName, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{date}", DateTime.Now.ToString("M月d日"), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{time}", DateTime.Now.ToString("hh:mm:ss"), RegexOptions.IgnoreCase);
            return msg;
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="type">1为针对分类，2为针对个人，3为针对员工</param>
        /// <param name="box">1为草稿箱，2为已发送，3为垃圾箱</param>
        /// <param name="state">1为发送成功，2为发送失败，3为部分失败</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public SmsMessage[] MessagePager(int? type, int? box, int? state, string search, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (type != null) wc.And(SmsMessage._.Sms_Type == (int)type);
            if (box != null) wc.And(SmsMessage._.Sms_MailBox == (int)box);
            if (state != null) wc.And(SmsMessage._.Sms_State == (int)state);
            if (search != string.Empty) wc.And(SmsMessage._.Sms_Context.Like("%" + search + "%"));
            countSum = Gateway.Default.Count<SmsMessage>(wc);
            return Gateway.Default.From<SmsMessage>().Where(wc).OrderBy(SmsMessage._.Sms_CrtTime.Desc).ToArray<SmsMessage>(size, (index - 1) * size);
        }
    }
}
