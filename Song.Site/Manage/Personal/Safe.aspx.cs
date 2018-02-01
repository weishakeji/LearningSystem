using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Manage.Personal
{
    public partial class Safe : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnEnter.UniqueID;
            if (!IsPostBack)
            {
                fill();
            }
        }

        private void fill()
        {
            Song.Entities.EmpAccount th = id == 0 ? Extend.LoginState.Admin.CurrentUser : Business.Do<IEmployee>().GetSingle(id);
            if (th == null) return;
            this.EntityBind(th);
            
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.EmpAccount obj = Extend.LoginState.Admin.CurrentUser;
            try
            {
                string pw = this.tbOldPw.Text.Trim();
                bool isHav = Business.Do<IEmployee>().LoginCheck(obj.Org_ID, obj.Acc_AccName, pw);
                this.lbShow.Visible = !isHav;
                if (!isHav) return;
                //员工登录密码，为空
                if (tbPw1.Text != "")
                {
                    string md5 = WeiSha.Common.Request.Controls[tbPw1].MD5;
                    obj.Acc_Pw = md5;
                }
                Business.Do<IEmployee>().Save(obj);
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
            
        }
        /// <summary>
        /// 保存安全问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSafeQues_Click(object sender, EventArgs e)
        {
            Song.Entities.EmpAccount obj = Extend.LoginState.Admin.CurrentUser;
            obj = this.EntityFill(obj) as Song.Entities.EmpAccount;
            Business.Do<IEmployee>().Save(obj);
            this.Alert("操作成功！");
        }
    }
}