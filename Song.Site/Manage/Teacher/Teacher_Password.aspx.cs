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

namespace Song.Site.Manage.Teacher
{
    public partial class Teacher_Password : Extend.CustomPage
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
            Song.Entities.Teacher ea;
            if (id == 0) return;
            ea = Business.Do<ITeacher>().TeacherSingle(id);          
            //员工名称
            this.lbName.Text = ea.Th_Name;           
            
        }

        /// <summary>
        /// 验证账号是否已经存在
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        protected void cusv_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Teacher th = Business.Do<ITeacher>().TeacherSingle(id);
            if (th == null) th = new Entities.Teacher();
            th.Org_ID = org.Org_ID;
            //判断是否通过验证
            bool isAccess = Business.Do<ITeacher>().IsTeacherExist(org.Org_ID,th);
            args.IsValid = !isAccess;
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
                Song.Entities.Teacher obj;
                obj = Business.Do<ITeacher>().TeacherSingle(id);
                Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(obj.Ac_ID);
                //员工登录密码，为空
                if (tbPw1.Text.Trim() != "")
                {                    
                    obj.Th_Pw = tbPw1.Text.Trim();
                    if (acc != null) acc.Ac_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(obj.Th_Pw).MD5;    
                }
                obj.Th_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(obj.Th_Pw).MD5;
                Business.Do<IAccounts>().AccountsSave(acc);
                Business.Do<ITeacher>().TeacherSave(obj);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
       
    }
}
