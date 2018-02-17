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
    public partial class MoneyRecharge : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = this.Master.Account;
            //this.Form.Attributes.Add("target", "_blank");
            if (st == null) return;
            if (!IsPostBack)
            {
                //当前学员的钱数
                ltMoney.Text = Extend.LoginState.Accounts.CurrentUser.Ac_Money.ToString("0.00");
                //在线充值接口
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.PayInterface[] pis = Business.Do<IPayInterface>().PayAll(org.Org_ID, "web", true);
                rptPi.DataSource = pis;
                rptPi.DataBind();
                onlinePay.Visible = pis.Length > 0;
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