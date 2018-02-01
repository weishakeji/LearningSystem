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
    public partial class Info : Extend.CustomPage
    {
        EmpAccount currentUser;
        //员工上传资料的所有路径
        private string _uppath = "Employee";
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
            EmpAccount ea = Extend.LoginState.Admin.CurrentUser;
            //员工帐号
            this.lbAcc.Text = ea.Acc_AccName;
            //员工名称
            this.tbName.Text = ea.Acc_Name;
            tbNamePinjin.Text = ea.Acc_NamePinyin;
            this.lbEmpCode.Text = ea.Acc_EmpCode;
            //性别
            string sex = ea.Acc_Sex.ToString().ToLower();
            ListItem liSex = rbSex.Items.FindByValue(sex);
            if (liSex != null) liSex.Selected = true;
            //院系
            Song.Entities.Depart depart = Business.Do<IDepart>().GetSingle(ea.Dep_Id);
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
            //职位（头衔）
            lbTeam.Text = ea.Team_Name;
            //联系方式
            tbTel.Text = ea.Acc_Tel;
            cbIsOpenTel.Checked = ea.Acc_IsOpenTel;
            tbMobile.Text = ea.Acc_MobileTel;
            cbIsOpenMobile.Checked = ea.Acc_IsOpenMobile;
            tbEmail.Text = ea.Acc_Email;
            tbQQ.Text = ea.Acc_QQ;
            tbWeixin.Text = ea.Acc_Weixin;
            //个人照片
            if (!string.IsNullOrEmpty(ea.Acc_Photo) && ea.Acc_Photo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + ea.Acc_Photo;
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
                if (currentUser == null) return;
                EmpAccount ea = currentUser;
                //名称与拼音缩写
                ea.Acc_Name = tbName.Text.Trim();
                ea.Acc_NamePinyin = tbNamePinjin.Text.Trim();
                //性别
                ea.Acc_Sex = Convert.ToInt16(rbSex.SelectedValue);
                //联系方式
                ea.Acc_Tel = tbTel.Text.Trim();
                ea.Acc_IsOpenTel = cbIsOpenTel.Checked;
                ea.Acc_MobileTel = tbMobile.Text.Trim();
                ea.Acc_IsOpenMobile = cbIsOpenMobile.Checked;
                ea.Acc_Email = tbEmail.Text.Trim();
                ea.Acc_QQ = tbQQ.Text.Trim();
                ea.Acc_Weixin = tbWeixin.Text.Trim();
                //图片
                if (fuLoad.PostedFile.FileName != "")
                {
                    try
                    {
                        fuLoad.UpPath = _uppath;
                        fuLoad.IsMakeSmall = false;
                        fuLoad.IsConvertJpg = true;
                        fuLoad.SaveAndDeleteOld(ea.Acc_Photo);
                        fuLoad.File.Server.ChangeSize(150, 200, false);
                        ea.Acc_Photo = fuLoad.File.Server.FileName;
                        //
                        imgShow.Src = fuLoad.File.Server.VirtualPath;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
                //
                Business.Do<IEmployee>().Save(ea);
                Master.AlertAndClose("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
    }
}
