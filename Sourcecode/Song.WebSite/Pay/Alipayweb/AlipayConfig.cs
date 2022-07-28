using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;
using WeiSha.Core;
using Song.ServiceInterfaces;
using Song.Entities;
using System.Text.RegularExpressions;

namespace Com.Alipayweb
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.4
    /// 修改日期：2016-03-08
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// </summary>
    public class Config
    {
        #region 字段
        private string partner = "";
        private string seller_id = "";
        private string private_key = "";
        private string public_key = "";
        private string input_charset = "";
        private string sign_type = "";
        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public string Seller_id
        {
            get { return seller_id; }
            set { seller_id = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public string Private_key
        {
            get { return private_key; }
            set { private_key = value; }
        }

        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public string Public_key
        {
            get { return public_key; }
            set { public_key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public string Sign_type
        {
            get { return sign_type; }
        }
        #endregion

        // 调试用，创建TXT日志文件夹路径，见AlipayCore.cs类中的LogResult(string sWord)打印方法。
        public static string log_path = HttpRuntime.AppDomainAppPath.ToString() + "log\\";
        // 调用的接口名，无需修改
        public static string service = "create_direct_pay_by_user";
        // 支付类型 ，无需修改
        public static string payment_type = "1";
        //防钓鱼时间戳  若要使用请调用类文件submit中的Query_timestamp函数
        public static string anti_phishing_key = "";
        //客户端的IP地址 非局域网的外网IP地址，如：221.0.0.1
        public static string exter_invoke_ip = "";
        /// <summary>
        /// 配置项
        /// </summary>
        /// <param name="paiid">系统中的接口ID</param>
        public Config(Song.Entities.PayInterface payInterface)
        {
            _init(payInterface);
        }
        public Config(int paiid)
        {
            Song.Entities.PayInterface payInterface = Business.Do<IPayInterface>().PaySingle(paiid);
            _init(payInterface);
        }

        private void _init(Song.Entities.PayInterface payInterface)
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
            partner = payInterface.Pai_ParterID;

            // 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
            seller_id = partner;

            //商户的私钥,原始格式，RSA公私钥生成：https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.nBDxfy&treeId=58&articleId=103242&docType=1
            WeiSha.Core.CustomConfig config = CustomConfig.Load(payInterface.Pai_Config);
            private_key = config["Privatekey"].Value.String;
            private_key = Regex.Replace(private_key, @"\r|\n|\s", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);

            //支付宝的公钥，查看地址：https://b.alipay.com/order/pidAndKey.htm 
            public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

            //// 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
            //notify_url = "http://商户网关地址/create_direct_pay_by_user-CSHARP-UTF-8/notify_url.aspx";

            //// 页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
            //return_url = "http://商户网关地址/create_direct_pay_by_user-CSHARP-UTF-8/return_url.aspx";

            // 签名方式
            sign_type = "RSA";

            // 字符编码格式 目前支持 gbk 或 utf-8
            input_charset = "utf-8";
        }

    }
}