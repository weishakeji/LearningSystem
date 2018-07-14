using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 积分兑换成卡券
    /// </summary>
    public class CouponExchange : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            //积分兑换卡券的比率
            int ratio = Business.Do<ISystemPara>()["PointConvert"].Int32 ?? 0;
            this.Document.Variables.SetValue("ratio", ratio);
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "exchange":
                        exchange();
                        break;
                }
                Response.End();
            }
        }
        /// <summary>
        /// 积分兑换的具体方法
        /// </summary>
        protected void exchange()
        {
            //输入的要兑换的卡券数
            int coupon = WeiSha.Common.Request.Form["coupon"].Int32 ?? 0;
            if (coupon <= 0) return;
            Song.Entities.Accounts acc =  Extend.LoginState.Accounts.CurrentUser;
            //返回值
            string json = "\"state\":{0},\"coupon\":{1},\"point\":{2},\"error\":\"{3}\"";
            try
            {
                if (acc == null) throw new Exception("未登录");
                //兑换
                Business.Do<IAccounts>().CouponExchange(acc, coupon);
                Response.Write(string.Format(json, 1, acc.Ac_Coupon, acc.Ac_Point, ""));
            }
            catch (Exception ex)
            {
                Response.Write(string.Format(json, 0, 0, 0, ex.Message));
            }
        }
    }
}