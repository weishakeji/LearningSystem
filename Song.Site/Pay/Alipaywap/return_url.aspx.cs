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
using System.Collections.Specialized;
using System.Collections.Generic;
using Com.Alipaywap;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Com.Alipaywap
{
    /// <summary>
    /// 功能：页面跳转同步通知页面
    /// 版本：3.3
    /// 日期：2012-07-10
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// ///////////////////////页面功能说明///////////////////////
    /// 该页面可在本机电脑测试
    /// 可放入HTML等美化页面的代码、商户业务逻辑程序代码
    /// 该页面可以使用ASP.NET开发工具调试，也可以使用写文本函数LogResult进行调试
    /// </summary>
    public partial class return_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string return_url = "/mobile/Recharge.ashx?type=2&sccess={0}&money={1}&paiid={2}";
            SortedDictionary<string, string> sPara = GetRequestGet();
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                //商户订单号
                string out_trade_no = Request.QueryString["out_trade_no"];
                Song.Entities.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(out_trade_no);
                if (maccount == null)
                {
                    Response.Redirect(string.Format(return_url, false, "", ""));
                    return;
                }
                Notify aliNotify = new Notify(maccount.Pai_ID);
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

                if (verifyResult)//验证成功
                {
                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];

                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];

                    if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                    }
                    else
                    {
                        Response.Write("trade_status=" + Request.QueryString["trade_status"]);
                    }
                    //付款方与收款方
                    maccount.Ma_Buyer = Request.QueryString["buyer_email"];
                    maccount.Ma_Seller = Request.QueryString["seller_email"];
                    Business.Do<IAccounts>().MoneyConfirm(maccount);
                    Response.Redirect(string.Format(return_url, true, maccount.Ma_Money, maccount.Pai_ID));
                    return;
                }
                else//验证失败
                {
                    Response.Redirect(string.Format(return_url, false, maccount.Ma_Money, maccount.Pai_ID));
                }
            }
            else
            {
                Response.Write("无返回参数");
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
    }
}