using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace Song.SMS.Object
{
    /// <summary>
    /// 河南腾信的短信开发接口
    /// </summary>
    public class HenanTengxin: ISMS
    {
        private static readonly sms1086.WsAPIs ObjWsAPIs = new sms1086.WsAPIs();
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
        #region ISMS 成员

        public SmsState Send(string mobiles, string context)
        {
            return Send(mobiles, context, DateTime.Now);
        }

        public SmsState Send(string mobiles, string content, DateTime time)
        {
            //url编码处理发送内容
            string username = System.Web.HttpUtility.UrlEncode(Current.User, Encoding.UTF8);
            content = System.Web.HttpUtility.UrlEncode(content, Encoding.UTF8);
            //参数
            string url = string.Format("{0}Api/sendutf8.aspx?username={1}&password={2}&mobiles={3}&content={4}", 
                Current.Domain, username, Current.Password, mobiles, content);
            string result = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //做请求
            request.ContentType = "text/HTML";
            request.MaximumAutomaticRedirections = 4;
            request.MaximumResponseHeadersLength = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //读结果
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            result = readStream.ReadToEnd();
            response.Close();
            readStream.Dispose();
            //
            string stat = _getPara(result, "result");  //状态值
            string desc = _getPara(result, "description");  //详情
            desc = System.Web.HttpUtility.UrlDecode(desc);
            //发送状态
            SmsState state = new SmsState();
            state.Code = int.Parse(stat);
            state.Success = state.Code == 0;
            switch (state.Code)
            {
                case 1:
                    state.Result = "提交参数不能为空";
                    break;
                case 2:
                    state.Result = "用户名或密码错误";
                    break;
                case 3:
                    state.Result = "账号未启用";
                    break;
                case 4:
                    state.Result = "计费账号无效";
                    break;
                case 5:
                    state.Result = "定时时间无效";
                    break;
                case 6:
                    state.Result = "业务未开通";
                    break;
                case 7:
                    state.Result = "权限不足";
                    break;
                case 8:
                    state.Result = "余额不足";
                    break;
                case 9:
                    state.Result = "号码中含有无效号码";
                    break;
                default:
                    state.Result = desc;
                    break;
            }
            state.Description = desc;
            state.FailList = _getPara(result, "faillist");  //发送失败的电话号码列表
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
            //网址
            string url = Current.Domain;
            //参数
            string postString = "username={0}&password={1}";
            url = string.Format(url + "Api/Query.aspx?" + postString, user, pw);
            //获取信剩余条数
            string result = "";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                using (System.IO.Stream stream = client.OpenRead(url))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, System.Text.Encoding.GetEncoding("gb2312")))
                    {
                        //result=返回值&balance=条数&description=错误描述
                        result = reader.ReadToEnd();
                        reader.Close();
                    }
                    stream.Close();
                }
            }
           
            string state = _getPara(result, "result");  //状态值
            string number = _getPara(result, "balance");    //剩余条数
            string desc = _getPara(result, "description");  //详情
            if (!string.IsNullOrWhiteSpace(desc))
            {
                desc = System.Web.HttpUtility.UrlDecode(desc, System.Text.Encoding.GetEncoding("GB2312"));
            }
            if (state == "0")
            {
                return Convert.ToInt32(number);
            }
            else
            {
                throw new Exception(desc);            
            }
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="url"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string _getPara(string url, string key)
        {
            string value = string.Empty;
            if (url.IndexOf("?") > -1) url = url.Substring(url.LastIndexOf("?") + 1);
            string[] paras = url.Split('&');
            foreach (string para in paras)
            {
                if (string.IsNullOrWhiteSpace(para)) continue;
                string[] arr = para.Split('=');
                if (arr.Length < 2) continue;
                if (String.Equals(arr[0], key, StringComparison.CurrentCultureIgnoreCase))
                {
                    value = arr[1];
                    break;
                }
            }
            return value;
        }
        public string ReceiveSMS(DateTime from, string readflag)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
