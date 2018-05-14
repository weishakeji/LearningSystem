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
    public partial class ResultNotifyPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Log.Info(this.GetType().ToString(), "支付成功后返回的QueryString参数 : ");
            //foreach (string s in this.Request.QueryString)
            //{
            //    Log.Info(this.GetType().ToString(), string.Format("参数：{0},值:{1}", s, Request.QueryString[s]));
            //}
            //Log.Info(this.GetType().ToString(), "支付成功后返回的Form参数 : ");
            //foreach (string s in this.Request.Form)
            //{
            //    Log.Info(this.GetType().ToString(), string.Format("参数：{0},值:{1}", s, Request.Form[s]));
            //}
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
            notifyData.ToPrintStr();
        }       
    }
}