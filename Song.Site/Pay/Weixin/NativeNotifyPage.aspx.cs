using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace WxPayAPI
{
    /// <summary>
    /// 微信扫码支付的回调方法
    /// </summary>
    public partial class NativeNotifyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ResultNotify resultNotify = new ResultNotify(this);
            //resultNotify.ProcessNotify();
            //获取结果
            WxPayData notifyData = resultNotify.GetNotifyData();
            string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
            Log.Info(this.GetType().ToString(), "商户流水号 : " + out_trade_no);
            if (!string.IsNullOrWhiteSpace(out_trade_no))
            {
                Song.Entities.MoneyAccount maccount = Business.Do<IAccounts>().MoneySingle(out_trade_no);
                if (maccount != null)
                {
                    //付款方与收款方（商户id)
                    maccount.Ma_Buyer = notifyData.GetValue("attach").ToString();
                    maccount.Ma_Seller = notifyData.GetValue("mch_id").ToString();
                    Business.Do<IAccounts>().MoneyConfirm(maccount);
                }
            }
        }
    }
}