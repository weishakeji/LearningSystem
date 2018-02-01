using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using WeiSha.Common;
using Song.ServiceInterfaces;
namespace Song.Site.Manage
{
    public partial class WeixinApi : System.Web.UI.Page
    {
        //Token
        private readonly string token = App.Get["WeixinToken"].String;
        protected void Page_Load(object sender, EventArgs e)
        {

            string postStr = "";

            if (Request.HttpMethod.ToLower() == "post")
            {
                System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                postStr = System.Text.Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr))
                {
                    //ResponseMsg(postStr);                    
                    Response.Write(ResponseMsg(postStr));
                    Response.End();
                }
                //WriteLog("postStr:" + postStr);
            }
            else
            {
                Valid();
            }
        }


        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        /// <returns></returns>
        private bool CheckSignature()
        {
            string signature = WeiSha.Common.Request.QueryString["signature"].String;
            string timestamp = WeiSha.Common.Request.QueryString["timestamp"].String;
            string nonce = WeiSha.Common.Request.QueryString["nonce"].String;
            string[] ArrTmp = { token, timestamp, nonce };
            //字典排序
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            return tmpStr.ToLower() == signature;
        }


        private void Valid()
        {
            string echoStr = WeiSha.Common.Request.QueryString["echoStr"].String;
            if (CheckSignature())
            {
                if (!string.IsNullOrEmpty(echoStr))
                {
                    Business.Do<ISystemPara>().Save("微信测试", "时间：" + DateTime.Now.ToString());
                    Response.Write(echoStr);
                    Response.End();
                }
            }
        }


        /// <summary>
        /// 返回信息结果(微信信息返回)
        /// </summary>
        /// <param name="weixinXML"></param>
        private string ResponseMsg(string weixinXML)
        {
            string tm = this.Server.UrlEncode(weixinXML);
            Business.Do<ISystemPara>().Save("微信消息测试", tm);
            ///这里写你的返回信息代码
            return "你说什么？";
        }

        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>

        private int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        /// <summary>  
        /// 发送文字消息  
        /// </summary>  
        /// <param name="wx">获取的收发者信息</param>  
        /// <param name="content">笑话内容</param>  
        /// <returns></returns>  
        private string sendTextMessage(string content)
        {
            string res = string.Format(@"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[text]]></MsgType>
                                <Content><![CDATA[{3}]]></Content>
                                <FuncFlag>0</FuncFlag>
                            </xml> ",
                content);
            return res;
        } 
        /// <summary>
        /// 写日志(用于跟踪)
        /// </summary>
        private void WriteLog(string strMemo)
        {
            string filename = Server.MapPath("/logs/log.txt");
            if (!Directory.Exists(Server.MapPath("//logs//")))
                Directory.CreateDirectory("//logs//");
            StreamWriter sr = null;
            try
            {
                if (!File.Exists(filename))
                {
                    sr = File.CreateText(filename);
                }
                else
                {
                    sr = File.AppendText(filename);
                }
                sr.WriteLine(strMemo);
            }
            catch { }
            finally
            {
                if (sr != null)
                    sr.Close();
            }
        }
    }
}
