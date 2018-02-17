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
    public partial class Students_Point : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }           
        }
        private void fill()
        {
            Song.Entities.Accounts ea;
            if (id == 0) return;
            ea = Business.Do<IAccounts>().AccountsSingle(id);
            //员工名称
            this.lbName.Text = ea.Ac_Name;
            //账户积分余额
            lbPoint.Text = ea.Ac_Point.ToString();
            lbPointmax.Text = ea.Ac_PointAmount.ToString();

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
            //操作积分数
            int point = 0;
            int.TryParse(tbPoint.Text, out point);
            //操作对象
            Song.Entities.PointAccount ma = new PointAccount();
            ma.Pa_Value = point;
            ma.Pa_Remark = tbRemark.Text.Trim();
            ma.Ac_ID = st.Ac_ID;
            ma.Pa_Source = "管理员操作";
            //充值方式，管理员充值
            ma.Pa_From = 1;
            //操作者
            Song.Entities.EmpAccount emp=Extend.LoginState.Admin.CurrentUser;
            try
            {
                string mobi = !string.IsNullOrWhiteSpace(emp.Acc_MobileTel) && emp.Acc_AccName != emp.Acc_MobileTel ? emp.Acc_MobileTel : "";
                //如果是充值
                if (type == 2)
                {                   
                    ma.Pa_Info = string.Format("管理员{0}（{1}{2}）向您充值{3}分", emp.Acc_Name, emp.Acc_AccName, mobi, point);
                    Business.Do<IAccounts>().PointAdd(ma);
                }
                //如果是转出
                if (type == 1)
                {
                    ma.Pa_Info = string.Format("管理员{0}（{1}{2}）扣除您{3}分", emp.Acc_Name, emp.Acc_AccName, mobi, point);
                    Business.Do<IAccounts>().PointPay(ma);
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
                if (tbPoint.Attributes["numlimit"] == null)
                {
                    tbPoint.Attributes.Add("numlimit", lbPoint.Text);
                }
                else
                {
                    tbPoint.Attributes["numlimit"] = lbPoint.Text;
                }
            }
            if (rbl.SelectedValue == "2")
            {
                lbOperator.Text = "+";
                if (tbPoint.Attributes["numlimit"] == null)
                {
                    tbPoint.Attributes.Add("numlimit", "0");
                }
                else
                {
                    tbPoint.Attributes["numlimit"] = "0";
                }
            }
        }       

    }
}
