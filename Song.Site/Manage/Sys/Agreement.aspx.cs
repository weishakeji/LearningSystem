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

namespace Song.Site.Manage.Sys
{
    /// <summary>
    /// 注册协议
    /// </summary>
    public partial class Agreement : Extend.CustomPage
    {
        //状态，用于标明是学员注册，还是教师注册
        private string state = WeiSha.Common.Request.QueryString["state"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            //默认是学员注册协议，教师是teacher
            if (string.IsNullOrWhiteSpace(state)) state = "accounts";
            if (!this.IsPostBack)
            {
                 fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            //注册协议
            tbAgreement.Text = Business.Do<ISystemPara>().GetValue("Agreement_" + state);
            
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<ISystemPara>().Save("Agreement_" + state, tbAgreement.Text);
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                Message.Alert(ex.Message);
            }
        }
    }
}
