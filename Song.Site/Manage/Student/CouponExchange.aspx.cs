using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
namespace Song.Site.Manage.Student
{
    public partial class CouponExchange : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = Extend.LoginState.Accounts.CurrentUser;
            if (st == null) return;
            if (!IsPostBack)
            {
                //当前学员的卡券与积分
                ltCoupon.Text = Extend.LoginState.Accounts.CurrentUser.Ac_Coupon.ToString();
                lbPoint.Text = Extend.LoginState.Accounts.CurrentUser.Ac_Point.ToString();
                //兑换比例
                ltPointConvert.Text = Business.Do<ISystemPara>()["PointConvert"].Value;
            }
        }
        /// <summary>
        /// 积分兑换卡券
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCode_Click(object sender, EventArgs e)
        {
            //输入的要兑换的卡券数
            int coupon = 0;
            int.TryParse(tbNumber.Text,out coupon);
            if (coupon<=0) return;
            try
            {
                //兑换
                Business.Do<IAccounts>().CouponExchange(Extend.LoginState.Accounts.CurrentUser.Ac_ID, coupon);
                //当前学员的卡券与积分
                ltCoupon.Text = Extend.LoginState.Accounts.CurrentUser.Ac_Coupon.ToString();
                lbPoint.Text = Extend.LoginState.Accounts.CurrentUser.Ac_Point.ToString();

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                this.cv3.ErrorMessage = ex.Message;
                this.cv3.IsValid = false;
            }
        }
        /// <summary>
        /// 在线充值的按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOnlinePay_Click(object sender, EventArgs e)
        {
            ////取图片验证码
            //string imgCode = WeiSha.Common.Request.Cookies["vpaycode"].ParaValue;
            ////取输入的验证码
            //string userCode = WeiSha.Common.Request.Controls[this.tbVerCode].MD5;
            ////验证
            //if (imgCode != userCode)
            //{
            //    cvCode.IsValid = false;
            //}
            //else
            //{
            //    string script = "<script language='JavaScript' type='text/javascript'>submitPay();</script>";
            //    if (!this.ClientScript.IsStartupScriptRegistered(this.GetType(), "submitPay"))
            //    {
            //        this.ClientScript.RegisterStartupScript(this.GetType(), "submitPay", script);
            //    }
            //}
        }       
    }
}