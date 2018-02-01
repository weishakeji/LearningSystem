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

namespace Song.Site.Manage.Personal
{
    public partial class Password : Extend.CustomPage
    {
        EmpAccount currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = Extend.LoginState.Admin.CurrentUser;
            if (!this.IsPostBack)
            {
                fill();
            } 
        }
        private void fill()
        {
            try
            {
                EmpAccount ea = currentUser;
                if (ea == null) return;
                //员工帐号
                this.lbAcc.Text = ea.Acc_AccName;
                //员工名称
                this.lbName.Text = ea.Acc_Name;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 

        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentUser == null) return;
                EmpAccount obj = currentUser;
                string name = obj.Acc_AccName;
                string pw = this.tbOldPw.Text.Trim();
                obj = Business.Do<IEmployee>().GetSingle(name, pw);
                this.lbShow.Visible = obj == null;
                if (obj == null) return;
                //员工登录密码，为空
                if (tbPw1.Text != "")
                {
                    string md5 = WeiSha.Common.Request.Controls[tbPw1].MD5;
                    obj.Acc_Pw = md5;
                }
                Business.Do<IEmployee>().Save(obj);
                Master.AlertAndClose("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
    }
}
