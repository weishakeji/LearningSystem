using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using WeiSha.Core;

namespace Song.SMS.Object
{
    /// <summary>
    /// 短信王的短信发送类
    /// </summary>
    public class DuanXinWang: ISMS
    {
        private SmsItem _current;
        public SmsItem Current
        {
            get { return _current; }
            set { _current = value; }
        }
        /// <summary>
        /// 用户的账号
        /// </summary>
        public string User
        {
            get { return _current.User; }
            set { _current.User = value; }
        }
        /// <summary>
        /// 用户的密码
        /// </summary>
        public string Password
        {
            get { return _current.Password; }
            set { _current.Password = value; }
        }
        public SmsState Send(string mobiles, string context)
        {
            return Send(mobiles, context, DateTime.Now);
        }

        public SmsState Send(string mobiles, string context, DateTime time)
        {
            //短信帐号与密码
            string account = Current.User;
            string pw = Current.Password;
            
            pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            
            //地址
            string url = "http://223.4.201.174/tx/";
            //参数
            string postString = "user={0}&pass={1}&mobile={2}&content={3}&time={4}&encode=utf8";
            context = new WeiSha.Core.Param.Method.ConvertToAnyValue(context).UrlEncode;
            postString = string.Format(postString, account, pw, mobiles, context, time.ToString());
            byte[] postData = Encoding.UTF8.GetBytes(postString); 
            //WebClient webClient = new WebClient();
            ////采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  
            //webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            ////得到返回字符流，并解码
            //byte[] responseData = webClient.UploadData(url, "POST", postData);
            //string result = Encoding.UTF8.GetString(responseData);

            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream stream = client.OpenRead(url+"?"+postString))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
             //发送状态
            SmsState state = new SmsState();
            state.Success = result == "100";
            //发送结果的状态
            string[] resultItem = new string[]{"100|发送成功","101|验证失败","102|短信不足","103|操作失败","104|非法字符",
               "105|内容过多","106|号码过多","107|频率过快","108|号码内容空",
               "109|账号冻结","110|禁止频繁单条发送","111|系统暂定发送",
               "112|号码不正确","120|系统升级"};
            foreach (string str in resultItem)
            {
                string s = str.Substring(0, str.IndexOf("|"));
                string e = str.Substring(str.IndexOf("|") + 1);
                if (result == s)
                {
                    state.Result = s;
                    state.Description = e;
                }
            }
            return state;
        }

        public int Query()
        {
            //短信帐号与密码
            string account = Current.User;
            string pw = Current.Password;
            return Query(account, pw);
        }
         /// <summary>
        /// 查询剩余的短信条数
        /// </summary>
        /// <param name="user">账号</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        public int Query(string user, string pw)
        {
            pw = new WeiSha.Core.Param.Method.ConvertToAnyValue(pw).MD5;
            //网址
            string url = "http://223.4.201.174/mm/?user={0}&pass={1}";
            url = string.Format(url, user, pw);
            //获取信剩余条数
            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream stream = client.OpenRead(url))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
            if (result == "") throw new Exception("获取失败！");
            if (result.IndexOf("|") > -1)
            {
                string state = result.Substring(0, result.IndexOf("|"));
                //如果获取正常
                if (state == "100")
                {
                    string num = result.Substring(result.IndexOf("-") + 1);
                    try
                    {
                        return System.Convert.ToInt32(num);
                    }
                    catch
                    {
                        return -1;
                    }
                }
            }
            return -1;
        }

        public string ReceiveSMS(DateTime from, string readflag)
        {
            throw new Exception("The method or operation is not implemented.");
        }


    }
}
