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



namespace Song.Site.Manage.Site
{
    public partial class User_Password : Extend.CustomPage
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
            try
            {
                Song.Entities.User ea;
                if (id == 0) return;
                ea = Business.Do<IUser>().GetUserSingle(id);
                //员工帐号
                this.lbAcc.Text = ea.User_AccName;
                //员工名称
                this.lbName.Text = ea.User_Name;
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
                Song.Entities.User obj;
                if (id == 0) return;
                obj = Business.Do<IUser>().GetUserSingle(id);
                //员工登录密码，为空
                if (tbPw1.Text != "")
                {
                    string md5 = WeiSha.Common.Request.Controls[tbPw1].MD5;
                    obj.User_Pw = md5;
                }
                Business.Do<IUser>().SaveUser(obj);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
       
    }
}
