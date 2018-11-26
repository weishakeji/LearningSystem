using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Sys
{
    public partial class OtherLogin : Extend.CustomPage
    {
        //根域
        protected string domain = WeiSha.Common.Server.MainName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                init();
            }
        }

        private void init()
        {
            //qq登录
            cbIsQQLogin.Checked = Business.Do<ISystemPara>()["QQLoginIsUse"].Boolean ?? true;
            cbIsQQDirect.Checked = Business.Do<ISystemPara>()["QQDirectIs"].Boolean ?? true;    //是否允许qq直接注册登录
            tbQQAppid.Text = Business.Do<ISystemPara>()["QQAPPID"].String;
            tbQQAppkey.Text = Business.Do<ISystemPara>()["QQAPPKey"].String;
            tbQQReturl.Text = Business.Do<ISystemPara>()["QQReturl"].Value;
            if (tbQQReturl.Text.Trim() == "") tbQQReturl.Text = "http://" + domain;
            //微信登录
            cbIsWeixinLogin.Checked = Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? false;
            cbIsWeixinDirect.Checked = Business.Do<ISystemPara>()["WeixinDirectIs"].Boolean ?? true;    //是否允许微信直接注册登录
            //微信开放平台设置
            tbWeixinAppid.Text = Business.Do<ISystemPara>()["WeixinAPPID"].String;
            tbWeixinSecret.Text = Business.Do<ISystemPara>()["WeixinSecret"].String;
            tbWeixinReturl.Text = Business.Do<ISystemPara>()["WeixinReturl"].Value;
            //微信公众号设置
            tbWeixinpubAppid.Text = Business.Do<ISystemPara>()["WeixinpubAPPID"].String;
            tbWeixinpubSecret.Text = Business.Do<ISystemPara>()["WeixinpubSecret"].String;
            tbWeixinpubReturl.Text = Business.Do<ISystemPara>()["WeixinpubReturl"].Value;
            if (tbWeixinReturl.Text.Trim() == "") tbWeixinReturl.Text = "http://"+domain;
        }
        /// <summary>
        /// QQ登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQQlogin_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<ISystemPara>().Save("QQLoginIsUse", cbIsQQLogin.Checked.ToString(), false);
                Business.Do<ISystemPara>().Save("QQDirectIs", cbIsQQDirect.Checked.ToString(), false);
                Business.Do<ISystemPara>().Save("QQAPPID", tbQQAppid.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("QQAPPKey", tbQQAppkey.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("QQReturl", tbQQReturl.Text.Trim(), false);
                Business.Do<ISystemPara>().Refresh();
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnWeixnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<ISystemPara>().Save("WeixinLoginIsUse", cbIsWeixinLogin.Checked.ToString(), false);
                Business.Do<ISystemPara>().Save("WeixinDirectIs", cbIsWeixinDirect.Checked.ToString(), false);
                Business.Do<ISystemPara>().Save("WeixinAPPID", tbWeixinAppid.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("WeixinSecret", tbWeixinSecret.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("WeixinReturl", tbWeixinReturl.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("WeixinpubAPPID", tbWeixinpubAppid.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("WeixinpubSecret", tbWeixinpubSecret.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("WeixinpubReturl", tbWeixinpubReturl.Text.Trim(), false);
                Business.Do<ISystemPara>().Refresh();
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
    }
}