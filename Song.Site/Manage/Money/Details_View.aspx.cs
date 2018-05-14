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
using WeiSha.WebControl;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Money
{
    public partial class Details_View : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0; 
        protected int type = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitBind();
                fill();
            }
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
           
        }
        void fill()
        {
            Song.Entities.MoneyAccount mm = Business.Do<IAccounts>().MoneySingle(id);
            this.EntityBind(mm);
            //学员
            Song.Entities.Accounts student = Business.Do<IAccounts>().AccountsSingle(mm.Ac_ID);
            if (student != null) Ac_ID.Text = student.Ac_Name;
            //支出或充值
            Ma_Type.Text = mm.Ma_Type == 1 ? "支出" : "充值";
            type = mm.Ma_Type;
            //支付方式
            if (mm.Ma_From == 1) Ma_From.Text = "管理员操作";
            if (mm.Ma_From == 2) Ma_From.Text = "充值码充值";
            if (mm.Ma_From == 3) Ma_From.Text = "在线支付";
            if (mm.Ma_From == 4) Ma_From.Text = "购买课程";
            //支付账号，收款账号
            Ma_Buyer.Text = mm.Ma_Buyer;
            Ma_Seller.Text = mm.Ma_Seller;
            //状态
            Ma_IsSuccess.Text = mm.Ma_IsSuccess ? "成功" : "失败";
            //充值码
            trCode.Visible = mm.Ma_From == 2;
            Rc_Code.Text = mm.Rc_Code;
            //支付平台
            if (mm.Ma_From == 3)
            {
                trPayinterface.Visible = true;
                Song.Entities.PayInterface payi = Business.Do<IPayInterface>().PaySingle(mm.Pai_ID);
                if (payi != null)
                {
                    Pai_ID.Text = payi.Pai_Name;
                }
            }
        }
       
    }
}
