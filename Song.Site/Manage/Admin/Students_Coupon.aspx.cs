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
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Admin
{
    public partial class Students_Coupon : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            //密码输入框的显示与否
            //this.trPw1.Visible = this.trPw2.Visible = id == 0;
        }
        private void fill()
        {
            Song.Entities.Accounts ea;
            if (id == 0) return;
            ea = Business.Do<IAccounts>().AccountsSingle(id);
            //员工名称
            this.lbName.Text = ea.Ac_Name;
            //账户余额
            lbCoupon.Text = ea.Ac_Coupon.ToString();

        }

        /// <summary>
        /// 充值或扣费
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddMoney_Click(object sender, EventArgs e)
        {            
            if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("非管理员无权此操作权限！");
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(id);
            if (st == null) throw new Exception("当前信息不存在！");
            //操作类型
            int type = 2;
            int.TryParse(rblOpera.SelectedItem.Value, out type);
            //操作金额
            int coupon = 0;
            int.TryParse(tbCoupon.Text, out coupon);
            //操作对象
            Song.Entities.CouponAccount ca = new CouponAccount();
            ca.Ca_Value = coupon;
            ca.Ca_Total = st.Ac_Coupon; //当前卡券总数
            ca.Ca_Remark = tbRemark.Text.Trim();
            ca.Ac_ID = st.Ac_ID;
            ca.Ca_Source = "管理员操作";
            //充值方式，管理员充值
            ca.Ca_From = 1;
            //操作者
            Song.Entities.EmpAccount emp = Extend.LoginState.Admin.CurrentUser;
            try
            {
                string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
                //如果是充值
                if (type == 2)
                {                   
                    ca.Ca_Info = string.Format("管理员{0}（{1}{2}）向您充值{3}个卡券", emp.Acc_Name, emp.Acc_AccName, mobi, coupon);
                    Business.Do<IAccounts>().CouponAdd(ca);
                }
                //如果是转出
                if (type == 1)
                {
                    ca.Ca_Info = string.Format("管理员{0}（{1}{2}）扣除您{3}个卡券", emp.Acc_Name, emp.Acc_AccName, mobi, coupon);
                    Business.Do<IAccounts>().CouponPay(ca);
                }
                Extend.LoginState.Accounts.Refresh(st.Ac_ID);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }

        protected void rblOpera_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = (RadioButtonList)sender;
            if (rbl.SelectedValue == "1")
            {
                lbOperator.Text = "-";
                if (tbCoupon.Attributes["numlimit"] == null)
                {
                    tbCoupon.Attributes.Add("numlimit", lbCoupon.Text);
                }
                else
                {
                    tbCoupon.Attributes["numlimit"] = lbCoupon.Text;
                }
            }
            if (rbl.SelectedValue == "2")
            {
                lbOperator.Text = "+";
                if (tbCoupon.Attributes["numlimit"] == null)
                {
                    tbCoupon.Attributes.Add("numlimit", "0");
                }
                else
                {
                    tbCoupon.Attributes["numlimit"] = "0";
                }
            }
        }

    }
}
