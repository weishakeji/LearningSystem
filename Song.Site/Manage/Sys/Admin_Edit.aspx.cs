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
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Sys
{
    public partial class Admin_Edit : Extend.CustomPage
    {
        //机构id
        private string ids = WeiSha.Common.Request.QueryString["id"].String;
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(id);
            if (org != null) lbOrg.Text = org.Org_Name;
            //根据公司id获取公司管理员信息。
            EmpAccount ea = Business.Do<IEmployee>().GetAdminByOrgId(id);
            if (ea != null)
            {
                tbAccName.Text = ea.Acc_AccName;
                tbName.Text = ea.Acc_Name;
                tbMobile.Text = ea.Acc_MobileTel;
                rbSex.Items.FindByValue(ea.Acc_Sex.ToString()).Selected = true;
                if((DateTime.Now.Year-(int)ea.Acc_Age)<1000)
                    tbAge.Text =(DateTime.Now.Year-ea.Acc_Age).ToString();
                tbIdCard.Text = ea.Acc_IDCardNumber;                    
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            EmpAccount ea = Business.Do<IEmployee>().GetAdminByOrgId(id);
            if (ea == null)
            {
                Song.Entities.Position posi = Business.Do<IPosition>().GetAdmin(id);
                ea = new EmpAccount();
                ea.Posi_Id = posi.Posi_Id;
                ea.Posi_Name = posi.Posi_Name;              
            }
            if (tbAccName.Text.Trim() == "")
            {
                ea.Acc_AccName = WeiSha.Common.Request.UniqueID();
            }
            else
            {
                ea.Acc_AccName = tbAccName.Text;
            }
            ea.Acc_Name = tbName.Text;
            ea.Acc_MobileTel = tbMobile.Text;
            ea.Acc_Sex = short.Parse(rbSex.SelectedValue);
            ea.Acc_Age = string.IsNullOrWhiteSpace(tbAge.Text.Trim()) ? DateTime.Now.AddYears(-1000).Year : DateTime.Now.AddYears(-Convert.ToInt32(tbAge.Text)).Year;
            ea.Acc_IDCardNumber = tbIdCard.Text;
            ea.Acc_IsUse = true;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(id);
            if (org != null)
            {
                ea.Org_Name = org.Org_Name;
                ea.Org_ID = org.Org_ID;
            }
            
            try
            {
                bool isExists = Business.Do<IEmployee>().IsExists(org.Org_ID, ea);
                if (isExists)
                {
                    Master.Alert("当前帐号已经存在！");
                    return;
                }               
                
                if (ea.Acc_Id < 1)
                {
                    //密码
                    ea.Acc_Pw = tbPw.Text.Trim();
                    Business.Do<IEmployee>().Add(ea);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(tbPw.Text.Trim()))
                    {
                        ea.Acc_Pw = new WeiSha.Common.Param.Method.ConvertToAnyValue(tbPw.Text.Trim()).MD5;
                    }
                    Business.Do<IEmployee>().Save(ea);
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
