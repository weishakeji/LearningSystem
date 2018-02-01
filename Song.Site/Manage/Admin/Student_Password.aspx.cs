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
    public partial class Student_Password : Extend.CustomPage
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
            //员工帐号
            this.tbStudentAcc.Text = ea.Ac_AccName;
            //员工名称
            this.lbName.Text = ea.Ac_Name;

        }

        /// <summary>
        /// 验证账号是否已经存在
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cusv_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Accounts th = Business.Do<IAccounts>().AccountsSingle(this.tbStudentAcc.Text.Trim(), org.Org_ID);
            if (th == null) th = new Entities.Accounts();
            th.Org_ID = org.Org_ID;
            th.Ac_AccName = this.tbStudentAcc.Text.Trim();
            //判断是否通过验证
            Song.Entities.Accounts t = Business.Do<IAccounts>().IsAccountsExist(org.Org_ID, th);
            args.IsValid = t==null;
        }

        /// <summary>
        /// 修改账号信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAcc_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("非管理员无权此操作权限！");
                if (id == 0) throw new Exception("当前信息不存在！");
                //验证码错误
                if (!cusv.IsValid)
                    return;
                Song.Entities.Accounts obj;
                obj = Business.Do<IAccounts>().AccountsSingle(id);
                obj.Ac_AccName = tbStudentAcc.Text.Trim();
                Business.Do<IAccounts>().AccountsSave(obj);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPw_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Extend.LoginState.Admin.IsAdmin) throw new Exception("非管理员无权此操作权限！");
                if (id == 0) throw new Exception("当前信息不存在！");
                //验证码错误
                if (!cusv.IsValid)
                    return;
                Song.Entities.Accounts obj;
                obj = Business.Do<IAccounts>().AccountsSingle(id);
                //员工登录密码，为空
                if (tbPw1.Text.Trim() != "")
                    obj.Ac_Pw = tbPw1.Text.Trim();
                obj.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(obj.Ac_Pw).MD5;  
                Business.Do<IAccounts>().AccountsSave(obj);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }

    }
}
