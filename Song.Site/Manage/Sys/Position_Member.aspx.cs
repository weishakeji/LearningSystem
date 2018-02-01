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

namespace Song.Site.Manage.Sys
{
    public partial class Position_Member : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            try
            {
                //输出所有员工
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.EmpAccount[] ea = Business.Do<IEmployee>().GetAll(orgid, -1, true, "");
                rtEmp.DataSource = ea;
                rtEmp.DataBind();
                //显示当前角色名
                Song.Entities.Position p = Business.Do<IPosition>().GetSingle(id);
                this.lbPosi.Text = p.Posi_Name;
                //输出隶属当前角色的所有员工
                Song.Entities.EmpAccount[] emp4posi = Business.Do<IPosition>().GetAllEmplyee(id);
                rbEmp4Posi.DataSource = emp4posi;
                rbEmp4Posi.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

    }
}
