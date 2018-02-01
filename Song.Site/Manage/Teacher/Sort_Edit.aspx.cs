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
    public partial class Sort_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //员工上传资料的所有路径
        //private string _uppath = "Teacher";
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
            Song.Entities.TeacherSort th = id == 0 ? new Song.Entities.TeacherSort() : Business.Do<ITeacher>().SortSingle(id);
            if (th == null) return;
            if (id != 0) this.EntityBind(th);          
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.TeacherSort th = id == 0 ? new Song.Entities.TeacherSort() : Business.Do<ITeacher>().SortSingle(id);
            th = this.EntityFill(th) as Song.Entities.TeacherSort;
            //判断是否重名
            if (Business.Do<ITeacher>().SortIsExist(th))
            {
                Master.Alert("当前学生分组已经存在！");
            }
            else
            {
                try
                {
                    if (id == 0)
                    {
                        Business.Do<ITeacher>().SortAdd(th);
                    }
                    else
                    {
                        Business.Do<ITeacher>().SortSave(th);
                    }

                    Master.AlertCloseAndRefresh("操作成功！");
                }
                catch (Exception ex)
                {
                    Master.Alert(ex.Message);
                }
            }
        }
       
       
       
    }
}
