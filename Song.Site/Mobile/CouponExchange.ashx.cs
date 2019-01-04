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
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                //初次注册，送积分
                dic.Add("RegFirst", Business.Do<ISystemPara>()["RegFirst"].String);
                //登录积分，每天最多多少分
                dic.Add("LoginPoint", Business.Do<ISystemPara>()["LoginPoint"].String);
                dic.Add("LoginPointMax", Business.Do<ISystemPara>()["LoginPointMax"].String);
                //分享积分，每天最多多少分
                dic.Add("SharePoint", Business.Do<ISystemPara>()["SharePoint"].String);
                dic.Add("SharePointMax", Business.Do<ISystemPara>()["SharePointMax"].String);
                //注册积分，每天最多多少分
                dic.Add("RegPoint", Business.Do<ISystemPara>()["RegPoint"].String);
                dic.Add("RegPointMax", Business.Do<ISystemPara>()["RegPointMax"].String);
                //积分与卡券的兑换
                dic.Add("PointConvert", Business.Do<ISystemPara>()["PointConvert"].String);
                this.Document.Variables.SetValue("dic", dic);
            }
        }
        private void fill()
        {
            ////初次注册，送积分
            //tbRegFirst.Text = Business.Do<ISystemPara>()["RegFirst"].String;
            ////登录积分，每天最多多少分
            //tbLoginPoint.Text = Business.Do<ISystemPara>()["LoginPoint"].String;
            //tbLoginPointMax.Text = Business.Do<ISystemPara>()["LoginPointMax"].String;
            ////分享积分，每天最多多少分
            //tbSharePoint.Text = Business.Do<ISystemPara>()["SharePoint"].String;
            //tbSharePointMax.Text = Business.Do<ISystemPara>()["SharePointMax"].String;
            ////注册积分，每天最多多少分
            //tbRegPoint.Text = Business.Do<ISystemPara>()["RegPoint"].String;
            //tbRegPointMax.Text = Business.Do<ISystemPara>()["RegPointMax"].String;
            ////积分与卡券的兑换
            //tbPointConvert.Text = Business.Do<ISystemPara>()["PointConvert"].String;
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