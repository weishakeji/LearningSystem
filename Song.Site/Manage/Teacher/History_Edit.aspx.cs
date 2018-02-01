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
    public partial class History_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {                
                fill();
                Thh_Type_SelectedIndexChanged(null, null);
            }
        }
        private void fill()
        {
            Song.Entities.TeacherHistory th =
                id == 0 ? new Song.Entities.TeacherHistory() : Business.Do<ITeacher>().HistorySingle(id);
            if (th == null) return;
            this.EntityBind(th);
            if (th.Thh_EndTime > DateTime.Now.AddYears(100))
                Thh_EndTime.Text = "";
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.TeacherHistory th =
               id == 0 ? new Song.Entities.TeacherHistory() : Business.Do<ITeacher>().HistorySingle(id);
            th = this.EntityFill(th) as Song.Entities.TeacherHistory;
            //如果没有填写结整时间，则为最大值
            if (Thh_EndTime.Text.Trim() == "") th.Thh_EndTime = DateTime.MaxValue;
            //所属教师
            Song.Entities.Teacher curth = Extend.LoginState.Accounts.Teacher;
            th.Th_ID = curth.Th_ID;
            th.Th_Name = curth.Th_Name;
            try
            {
                if (id < 1)
                {
                    Business.Do<ITeacher>().HistoryAdd(th);
                }
                else
                {
                    Business.Do<ITeacher>().HistorySave(th);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }

        protected void Thh_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = this.Thh_Type;
            plStudy.Visible = ddl.SelectedValue == "学习";
            plWork.Visible = ddl.SelectedValue != "学习";
        } 
    }
}
