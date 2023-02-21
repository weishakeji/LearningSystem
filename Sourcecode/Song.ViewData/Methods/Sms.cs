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
            return Song.SMS.Config.SmsItems;
        }
        /// <summary>
        /// 获取所有短信接口,并刷新缓存
        /// </summary>
        /// <returns></returns>
        public Song.SMS.SmsItem[] ItemsFresh()
        {
            Song.SMS.Config.Fresh();
            string current = Business.Do<ISystemPara>().GetValue("SmsCurrent");
            Song.SMS.Config.SetCurrent(current);           
            return Song.SMS.Config.SmsItems;
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
        /// 获取某短信接口的短信数
        /// </summary>
        /// <param name="mark">接口标识名</param>
        /// <returns></returns>
        public int Count(string mark)
        {
            //账号与密码
            string smsacc = Business.Do<ISystemPara>().GetValue(mark + "SmsAcc");
            string smspw = Business.Do<ISystemPara>().GetValue(mark + "SmsPw");
            if (string.IsNullOrWhiteSpace(smspw)) throw new Exception("密码不得为空");

            int num = -1;

            smspw = WeiSha.Core.DataConvert.DecryptForBase64(smspw);    //将密码解密
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
        public JObject UserInfo(string mark)
        {
            JObject jo = new JObject();
            Song.SMS.ISMS sms = Song.SMS.Gatway.GetService(mark);
            jo.Add("name", sms.Current.Name);
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
        ///  短信模板实际效果
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string TemplateFormat(string msg)
        {            
            return Business.Do<ISMS>().MessageFormat(msg, DateTime.Now.ToString("mmss"));
        }
    }
}
