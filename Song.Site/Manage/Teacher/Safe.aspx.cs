using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Manage.Teacher
{
    public partial class Safe : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //private string _uppath = "Teacher";
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
            Song.Entities.Teacher th = id == 0 ? Extend.LoginState.Accounts.Teacher : Business.Do<ITeacher>().TeacherSingle(id);
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
            Song.Entities.Teacher obj = Extend.LoginState.Accounts.Teacher;
            
            try
            {
                string pw = this.tbOldPw.Text.Trim();
                obj = Business.Do<ITeacher>().TeacherSingle(obj.Th_AccName, pw, obj.Org_ID);
                this.lbShow.Visible = obj == null;
                if (obj == null) return;
                //员工登录密码，为空
                if (tbPw1.Text != "")
                {
                    string md5 = WeiSha.Common.Request.Controls[tbPw1].MD5;
                    obj.Th_Pw = md5;
                }
                Business.Do<ITeacher>().TeacherSave(obj);
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
            Song.Entities.Teacher obj = Extend.LoginState.Accounts.Teacher;
            obj = this.EntityFill(obj) as Song.Entities.Teacher;
            Business.Do<ITeacher>().TeacherSave(obj);
            this.Alert("操作成功！");
        }
    }
}