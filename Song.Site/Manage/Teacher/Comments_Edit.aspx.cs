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
    public partial class Comments_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
            this.Form.DefaultButton = this.btnEnter.UniqueID;
        }
        private void fill()
        {
            Song.Entities.TeacherComment th = Business.Do<ITeacher>().CommentSingle(id);
            if (th == null) return;
            if (id != 0) this.EntityBind(th);    
            //评价人，即学员
            Song.Entities.Accounts acc = Business.Do<IAccounts>().AccountsSingle(th.Ac_ID);
            if (acc != null) this.EntityBind(this.plAccount, acc);
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                if (id < 1) throw new Exception("参数不正确！");
                Song.Entities.TeacherComment thc = Business.Do<ITeacher>().CommentSingle(id);
                thc.Thc_Reply = Thc_Reply.Text.Trim();
                thc.Thc_IsUse = Thc_IsUse.Checked;
                thc.Thc_IsShow = Thc_IsShow.Checked;

                Business.Do<ITeacher>().CommentSave(thc);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }

        } 
       
    }
}
