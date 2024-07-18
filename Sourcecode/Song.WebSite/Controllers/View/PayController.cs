using Song.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeiSha.Core;
using WxPayAPI;

namespace Song.WebSite.Controllers
{
    /// <summary>
    /// 支付所需的页面
    /// </summary>
    public class PayController : Controller
    {
        /// <summary>
        /// 没有对应的action时，默认对应到templates中的指定页面
        /// </summary>
        /// <param name="actionName">方法名，对应视图文件夹的html页</param>
        protected override void HandleUnknownAction(string actionName)
        {
            // 获取控制器、方法的名称，以及id值
            //string ctr = this.RouteData.Values["controller"].ToString();
            string action = this.RouteData.Values["action"].ToString();
            string id = this.RouteData.Values["id"] != null ? this.RouteData.Values["id"].ToString() : string.Empty;
            if (id.IndexOf(".") > -1) id = id.Substring(0, id.LastIndexOf("."));

            //如果是微信回调通知
            if ("weixin".Equals(actionName, StringComparison.OrdinalIgnoreCase))
            {
                //微信扫码支付的回调方法
                if ("NativePayNotify".Equals(id, StringComparison.OrdinalIgnoreCase)
                   || "PublicPayNotify".Equals(id, StringComparison.OrdinalIgnoreCase)      //公众号支付的回调
                   )
                {
                    ResultNotify resultNotify = new ResultNotify();
                    //获取结果
                    WxPayData notifyData = resultNotify.GetNotifyData();
                    string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                    //回调信息
                    WxPayAPI.Log.Info(this.GetType().ToString(), actionName + " 的回调信息" + notifyData.ToJson());
                    WxPayAPI.Log.Info(this.GetType().ToString(), "商户流水号 : " + out_trade_no);
                    if (!string.IsNullOrWhiteSpace(out_trade_no))
                    {
                        Song.Entities.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(out_trade_no);
                        if (maccount != null)
                        {
                            //付款方与收款方（商户id)
                            maccount.Ma_Buyer = notifyData.GetValue("attach").ToString();
                            maccount.Ma_Seller = notifyData.GetValue("mch_id").ToString();
                            Business.Do<IAccounts>().MoneyConfirm(maccount);

                            //刷新当前登录的学员信息
                            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(maccount.Ac_ID);
                            Song.ViewData.LoginAccount.Status.Fresh(acc);
                        }
                    }
                }
            }
            //返回方法名为视图的对象，只要Views文件夹存在actionName.html
            ViewResult vr = this.View(actionName);
            vr.ExecuteResult(this.ControllerContext);
        }
    }
}
