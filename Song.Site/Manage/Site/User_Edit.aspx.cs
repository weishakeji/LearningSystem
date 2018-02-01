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
    public partial class User_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlGroupBind();
                fill();
            }   
            //密码输入框的显示与否
            this.trPw1.Visible = this.trPw2.Visible = id == 0;
        }
        private void fill()
        {
            try
            {
                Song.Entities.User ea;
                if (id != 0)
                {
                    ea = Business.Do<IUser>().GetUserSingle(id);
                    //所属的组
                    Song.Entities.UserGroup egs = Business.Do<IUser>().GetGroup4User(id);
                    ListItem li = this.ddlGroup.Items.FindByValue(egs.UGrp_Id.ToString());
                    if (li != null) li.Selected = true;

                    //性别
                    string sex = ea.User_Sex.ToString().ToLower();
                    ListItem liSex = rbSex.Items.FindByValue(sex);
                    if (liSex != null) liSex.Selected = true;
                }
                else
                {
                    ea = new Song.Entities.User();
                    ea.User_RegTime = DateTime.Now;
                    ea.User_IsUse = true;
                }
                //员工帐号
                this.tbAccName.Text = ea.User_AccName;
                //员工名称
                this.tbName.Text = ea.User_Name;
                //员工登录密码，为空
                //邮箱
                this.tbEmail.Text = ea.User_Email;
                //是否在职
                this.cbIsUse.Checked = ea.User_IsUse;
                //联系方式
                tbTel.Text = ea.User_Tel;
                tbMobleTel.Text = ea.User_MobileTel;
                tbQQ.Text = ea.User_QQ;
                tbMsn.Text = ea.User_Msn;
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
                if (id == 0)
                {
                    obj = new Song.Entities.User();
                }
                else
                {
                    obj = Business.Do<IUser>().GetUserSingle(id);
                    //先清除与用户组的关联
                    Business.Do<IEmpGroup>().DelRelation4Emplyee(id);
                }
                //帐号
                obj.User_AccName = this.tbAccName.Text.Trim();
                //员工名称
                obj.User_Name = this.tbName.Text;
                //员工登录密码，为空
                if (tbPw1.Text != "")
                {
                    string md5 = WeiSha.Common.Request.Controls[tbPw1].MD5;
                    obj.User_Pw = md5;
                }
                //邮箱
                obj.User_Email = this.tbEmail.Text;
                //是否在职
                obj.User_IsUse = this.cbIsUse.Checked;
                //性别
                obj.User_Sex = Convert.ToInt16(rbSex.SelectedValue);
                //所属组
                obj.UGrp_Id = Convert.ToInt32(ddlGroup.SelectedItem.Value);
                obj.UGrp_Name = ddlGroup.SelectedItem.Text;
                //联系方式
                obj.User_Tel = tbTel.Text;
                obj.User_MobileTel = tbMobleTel.Text;
                obj.User_QQ = tbQQ.Text;
                obj.User_Msn = tbMsn.Text;
                if (id == 0)
                {
                    id = Business.Do<IUser>().AddUser(obj);
                }
                else
                {
                    Business.Do<IUser>().SaveUser(obj);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        /// <summary>
        /// 用户组下拉绑定
        /// </summary>
        private void ddlGroupBind()
        {
            try
            {
                Song.Entities.UserGroup[] group = Business.Do<IUser>().GetGroupAll(true);
                this.ddlGroup.DataSource = group;
                this.ddlGroup.DataTextField = "UGrp_Name";
                this.ddlGroup.DataValueField = "UGrp_Id";
                this.ddlGroup.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }            
        }
    }
}
