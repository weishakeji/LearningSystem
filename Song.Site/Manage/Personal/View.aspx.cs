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
    public partial class View : Extend.CustomPage
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
                EmpAccount ea = this.currentUser;
                //员工帐号
                this.lbAcc.Text = ea.Acc_AccName;
                //员工名称
                this.lbName.Text = ea.Acc_Name;
                this.lbEmpCode.Text = ea.Acc_EmpCode;
                this.lbEmail.Text = ea.Acc_Email;
                this.lbRegTime.Text = ((DateTime)ea.Acc_RegTime).ToString("yyyy年M月d日");
                //院系
                Song.Entities.Depart depart = Business.Do<IDepart>().GetSingle((int)ea.Dep_Id);
                if (depart != null)
                {
                    lbDepart.Text = depart.Dep_CnName;
                }
                //角色
                Song.Entities.Position posi = Business.Do<IPosition>().GetSingle((int)ea.Posi_Id);
                if (posi != null)
                {
                    lbPosi.Text = posi.Posi_Name;
                }
                //用户组
                Song.Entities.EmpGroup[] egs = Business.Do<IEmpGroup>().GetAll4Emp(ea.Acc_Id);
                if (egs != null)
                {
                    for (int i = 0; i < egs.Length; i++)
                    {
                        if (i == egs.Length - 1)
                        {
                            lbGroup.Text += egs[i].EGrp_Name;
                        }
                        else
                        {
                            lbGroup.Text += egs[i].EGrp_Name + "，";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
            //
        }
    }
}
