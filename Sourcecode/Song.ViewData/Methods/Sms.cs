using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 短信接口
    /// </summary>
    [HttpPut, HttpGet]
    public class Sms : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 当前短信接口的名称
        /// </summary>
        /// <returns></returns>
        public string Current()
        {
            return  Business.Do<ISystemPara>().GetValue("SmsCurrent");           
        }
        /// <summary>
        /// 所有短信接口
        /// </summary>
        /// <returns></returns>
        public Song.SMS.SmsItem[] Items()
        {
            string current = Business.Do<ISystemPara>().GetValue("SmsCurrent");
            Song.SMS.Config.SetCurrent(current);
            SMS.SmsItem[] items = Song.SMS.Config.SmsItems;
            foreach (SMS.SmsItem item in items)
            {
                item.User = Business.Do<ISystemPara>().GetValue(item.Mark + "SmsAcc");
                item.Password = Business.Do<ISystemPara>().GetValue(item.Mark + "SmsPw");
                if (!string.IsNullOrWhiteSpace(item.Password))
                    item.Password = WeiSha.Core.DataConvert.DecryptForBase64(item.Password);    //将密码解密
            }
            return items;
        }
        /// <summary>
        /// 获取所有短信接口,并刷新缓存
        /// </summary>
        /// <returns></returns>
        [HttpPost, SuperAdmin]
        public Song.SMS.SmsItem[] ItemsFresh()
        {
            Song.SMS.Config.Fresh();
            string current = Business.Do<ISystemPara>().GetValue("SmsCurrent");
            Song.SMS.Config.SetCurrent(current);
            SMS.SmsItem[] items = Song.SMS.Config.SmsItems;
            foreach (SMS.SmsItem item in items)
            {
                item.User = Business.Do<ISystemPara>().GetValue(item.Mark + "SmsAcc");
                item.Password = Business.Do<ISystemPara>().GetValue(item.Mark + "SmsPw");
                if (!string.IsNullOrWhiteSpace(item.Password))
                    item.Password = WeiSha.Core.DataConvert.DecryptForBase64(item.Password);    //将密码解密
            }
            return items;
        }
        /// <summary>
        /// 设置当前接口
        /// </summary>
        /// <param name="mark">接口标识名</param>
        /// <returns></returns>
        [HttpPost,SuperAdmin]
        public bool SetCurrent(string mark)
        {
            Business.Do<ISystemPara>().Save("SmsCurrent", mark);
            Song.SMS.Config.SetCurrent(mark);
            return true;
        }
        /// <summary>
        /// 通过mark，获取接口对象
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public Song.SMS.SmsItem GetItem(string mark)
        {
            return Song.SMS.Config.GetItem(mark);
        }
        /// <summary>
        /// 获取某短信接口的短信数
        /// </summary>
        /// <param name="mark">接口标识名</param>
        /// <param name="smsacc"></param>
        /// <param name="smspw"></param>
        /// <returns></returns>
        [Admin]
        public int Count(string mark,string smsacc,string smspw)
        {
            //账号
            if(string.IsNullOrWhiteSpace(smsacc))
                smsacc = Business.Do<ISystemPara>().GetValue(mark + "SmsAcc");
            //密码
            if (string.IsNullOrWhiteSpace(smspw))
            {
                smspw = Business.Do<ISystemPara>().GetValue(mark + "SmsPw");
                if (string.IsNullOrWhiteSpace(smspw)) throw new Exception("密码不得为空");
                smspw = WeiSha.Core.DataConvert.DecryptForBase64(smspw);    //将密码解密
            }
            int num = -1;         
            //短信平台操作对象
            Song.SMS.ISMS sms = Song.SMS.Gatway.GetService(mark);
            //设置账号与密码
            sms.Current.User = smsacc;
            sms.Current.Password = smspw;
            num = sms.Query();
            return num;
        }
        /// <summary>
        /// 短信接口的账号与密码
        /// </summary>
        /// <param name="mark">接口标识</param>
        /// <returns>user账号,pw密码</returns>
       [HttpGet,SuperAdmin]
        public JObject UserInfo(string mark)
        {
            JObject jo = new JObject();
            Song.SMS.SmsItem sms = Song.SMS.Config.GetItem(mark);
            if (sms == null) throw new Exception("短信接口“"+mark+"”不存在");
            jo.Add("name", sms.Name);
            //账号与密码
            string user = Business.Do<ISystemPara>().GetValue(mark + "SmsAcc");
            string pw = Business.Do<ISystemPara>().GetValue(mark + "SmsPw");
            if (!string.IsNullOrWhiteSpace(pw))
                pw = WeiSha.Core.DataConvert.DecryptForBase64(pw);
            jo.Add("user", user);
            jo.Add("pw", pw);
            return jo;
        }
        /// <summary>
        /// 修改短信接口的账号与密码
        /// </summary>
        /// <param name="mark">接口标识名</param>
        /// <param name="account">短信平台的账号</param>
        /// <param name="pwd">短信平台的密码</param>
        /// <returns></returns>
        [HttpPost]
        public bool Modify(string mark,string account, string pwd)
        {
            Business.Do<ISystemPara>().Save(mark + "SmsAcc", account);
            //密码加密存储
            string pw = WeiSha.Core.DataConvert.EncryptForBase64(pwd);
            Business.Do<ISystemPara>().Save(mark + "SmsPw", pw);

            return true;
        }
        /// <summary>
        /// 更新短信模板内容
        /// </summary>
        /// <param name="mark">接口标识名</param>
        /// <param name="msg">短信消息模板</param>
        /// <returns></returns>
        [HttpPost]
        public bool TemplateUpdate(string mark,string msg)
        {
            Business.Do<ISystemPara>().Save(mark + "_SmsTemplate", msg);
            return true;           
        }
        /// <summary>
        /// 短信模板
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public string TemplateSMS(string mark)
        {
            return Business.Do<ISystemPara>().GetValue(mark + "_SmsTemplate"); ;
        }
        /// <summary>
        ///  短信模板实际效果
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string TemplateFormat(string msg)
        {            
            return Business.Do<ISMS>().MessageFormat(msg, DateTime.Now.ToString("mmss"));
        }
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="len"></param>
        /// <returns>返回phone与验证码的md5值</returns>
        public string SendVcode(string phone, int len)
        {
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsForMobi(phone, -1, true, true);
            if (acc == null) throw new Exception("当前手机号不存在");

            string vcode = "666888";
            //string vcode = Business.Do<ISMS>().SendVcode(phone, len);
            acc.Ac_CheckUID = new Song.ViewData.ConvertToAnyValue(phone + vcode).MD5;
            Business.Do<IAccounts>().AccountsSave(acc);

            return acc.Ac_CheckUID;
        }
    }
}
