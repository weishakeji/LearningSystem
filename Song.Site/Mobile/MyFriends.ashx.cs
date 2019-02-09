using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 我的朋友（分润）
    /// </summary>
    public class MyFriends : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {
            int accid = this.Account.Ac_ID;

            //好朋友（即直接分享注册的账户）
            int friends = Business.Do<IAccounts>().SubordinatesCount(accid,false);
            this.Document.Variables.SetValue("friends", friends);
            //朋友的朋友（即享注注册的账户再次分享所得）
            int friendsAll = Business.Do<IAccounts>().SubordinatesCount(accid, true);
            this.Document.Variables.SetValue("friendsAll", friendsAll);


            //分享得来的积分，分享访问+分享注册
            int sumPoint = Business.Do<IAccounts>().PointClac(accid, 2, null, null);
            sumPoint += Business.Do<IAccounts>().PointClac(accid, 3, null, null);
            this.Document.Variables.SetValue("sumPoint", sumPoint);
            //分享得来的现金
            int sumMoney = Business.Do<IAccounts>().MoneyClac(accid, 5, null, null);
            this.Document.Variables.SetValue("sumMoney", sumMoney);
            //分享得来的卡券
            int sumCoupon = Business.Do<IAccounts>().CouponClac(accid, 5, null, null);
            this.Document.Variables.SetValue("sumCoupon", sumCoupon);
        }
    }
}