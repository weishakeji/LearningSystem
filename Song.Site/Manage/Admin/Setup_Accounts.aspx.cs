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
using WeiSha.WebControl;

namespace Song.Site.Manage.Admin
{
    public partial class Setup_Accounts : Extend.CustomPage
    {
        //private string _uppath = "Org";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                fillLogin();
            }
        }



        #region 学习相关（注册登录等）
        /// <summary>
        /// 注册登录的相关初始值
        /// </summary>
        private void fillLogin()
        {
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //是否允许教师、学员注册
            cbIsRegTeacher.Checked = config["IsRegTeacher"].Value.Boolean ?? true;
            cbIsRegStudent.Checked = config["IsRegStudent"].Value.Boolean ?? true;
            cbIsRegSms.Checked = config["IsRegSms"].Value.Boolean ?? true;
            //教师与学员注册后是否需要审核
            cbIsVerifyTeahcer.Checked = config["IsVerifyTeahcer"].Value.Boolean ?? true;
            cbIsVerifyStudent.Checked = config["IsVerifyStudent"].Value.Boolean ?? true;
            //登录方式
            cbIsLoginForPw.Checked = config["IsLoginForPw"].Value.Boolean ?? true;    //启用账号密码登录
            cbIsLoginForSms.Checked = config["IsLoginForSms"].Value.Boolean ?? true;  //启用手机短信验证登录
            //在线练习，是否学员登录后才能用
            cbIsTraningLogin.Checked = config["IsTraningLogin"].Value.Boolean ?? false;            
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            //是否允许教师、学员注册
            config["IsRegTeacher"].Text = cbIsRegTeacher.Checked.ToString();
            config["IsRegStudent"].Text = cbIsRegStudent.Checked.ToString();
            config["IsRegSms"].Text = cbIsRegSms.Checked.ToString();
            //教师与学员注册后是否需要审核
            config["IsVerifyTeahcer"].Text = cbIsVerifyTeahcer.Checked.ToString();
            config["IsVerifyStudent"].Text = cbIsVerifyStudent.Checked.ToString();
            //登录方式
            config["IsLoginForPw"].Text = cbIsLoginForPw.Checked.ToString();    //启用账号密码登录
            config["IsLoginForSms"].Text = cbIsLoginForSms.Checked.ToString();  //启用手机短信验证登录
            //在线练习，是否学员登录后才能用
            config["IsTraningLogin"].Text = cbIsTraningLogin.Checked.ToString();         
            //保存
            org.Org_Config = config.XmlString;
            try
            {
                Business.Do<IOrganization>().OrganSave(org);

                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        #endregion

  

        
    }
}
