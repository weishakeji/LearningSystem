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
    public partial class Accounts_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //上传资料的所有路径
        private string _uppath = "Accounts";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            Song.Entities.Accounts acc = id != 0 ? Business.Do<IAccounts>().AccountsSingle(id) : new Song.Entities.Accounts();
            if (acc == null) return;
            if (id != 0)
            {   
                //性别
                string sex = acc.Ac_Sex.ToString().ToLower();
                ListItem liSex = rbSex.Items.FindByValue(sex);
                if (liSex != null)
                {
                    rbSex.SelectedIndex = -1;
                    liSex.Selected = true;
                }
                //年龄
                if (acc.Ac_Age > DateTime.Now.AddYears(-100).Year)
                {
                    tbAge.Text = (DateTime.Now.Year - acc.Ac_Age).ToString();
                }                
                //个人照片
                if (!string.IsNullOrEmpty(acc.Ac_Photo) && acc.Ac_Photo.Trim() != "")
                    this.imgShow.Src = Upload.Get[_uppath].Virtual + acc.Ac_Photo;
               
            }
            if (acc.Org_ID > 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganSingle(acc.Org_ID);
                if (org != null) lbOrgin.Text = org.Org_Name;
            }
            //员工帐号
            this.tbAccName.Text = acc.Ac_AccName;
            //身份证
            this.tbIdCard.Text = acc.Ac_IDCardNumber;
            //员工名称与拼音缩写
            this.tbName.Text = acc.Ac_Name;
            tbNamePinjin.Text = acc.Ac_Pinyin;
            //员工登录密码，为空
            //邮箱
            this.tbEmail.Text = acc.Ac_Email;           

            //联系方式
            tbTel.Text = acc.Ac_Tel;
            cbIsOpenTel.Checked = acc.Ac_IsOpenTel;
            tbMobleTel1.Text = acc.Ac_MobiTel1;
            tbMobleTel2.Text = acc.Ac_MobiTel2;
            cbIsOpenMobile.Checked = acc.Ac_IsOpenMobile;
            tbQQ.Text = acc.Ac_Qq;
            tbWeixin.Text = acc.Ac_Weixin;            
            
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Accounts acc = id != 0 ? Business.Do<IAccounts>().AccountsSingle(id) : new Song.Entities.Accounts();           
            //帐号
            acc.Ac_AccName = this.tbAccName.Text.Trim();
            //员工名称
            acc.Ac_Name = this.tbName.Text.Trim();
            acc.Ac_Pinyin = tbNamePinjin.Text.Trim();
            acc.Ac_IDCardNumber = tbIdCard.Text.Trim(); //身份证
            //性别
            acc.Ac_Sex = Convert.ToInt16(rbSex.SelectedValue);
            //年龄
            if (tbAge.Text.Trim() != "")
            {
                acc.Ac_Age = DateTime.Now.Year - Convert.ToInt32(tbAge.Text.Trim());
            }           
            //联系方式
            acc.Ac_Email = this.tbEmail.Text.Trim();    //邮箱  
            acc.Ac_Tel = tbTel.Text;
            acc.Ac_IsOpenTel = cbIsOpenTel.Checked;
            acc.Ac_MobiTel1 = tbMobleTel1.Text;
            acc.Ac_MobiTel2 = tbMobleTel2.Text;
            acc.Ac_IsOpenMobile = cbIsOpenMobile.Checked;
            acc.Ac_Qq = tbQQ.Text;
            acc.Ac_Weixin = tbWeixin.Text.Trim();            
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(acc.Ac_Photo);
                    fuLoad.File.Server.ChangeSize(150, 200, false);
                    acc.Ac_Photo = fuLoad.File.Server.FileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            try
            {               
                if (id == 0)
                {
                    id = Business.Do<IAccounts>().AccountsAdd(acc);                   
                }
                else
                {
                    Business.Do<IAccounts>().AccountsSave(acc);
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
