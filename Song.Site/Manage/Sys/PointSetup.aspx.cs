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
    public partial class PointSetup : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            //初次注册，送积分
            tbRegFirst.Text = Business.Do<ISystemPara>()["RegFirst"].String;
            //登录积分，每天最多多少分
            tbLoginPoint.Text = Business.Do<ISystemPara>()["LoginPoint"].String;
            tbLoginPointMax.Text = Business.Do<ISystemPara>()["LoginPointMax"].String;
            //分享积分，每天最多多少分
            tbSharePoint.Text = Business.Do<ISystemPara>()["SharePoint"].String;
            tbSharePointMax.Text = Business.Do<ISystemPara>()["SharePointMax"].String;
            //注册积分，每天最多多少分
            tbRegPoint.Text = Business.Do<ISystemPara>()["RegPoint"].String;
            tbRegPointMax.Text = Business.Do<ISystemPara>()["RegPointMax"].String;
            //积分与卡券的兑换
            tbPointConvert.Text = Business.Do<ISystemPara>()["PointConvert"].String;
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                //初次注册，送积分
                Business.Do<ISystemPara>().Save("RegFirst", tbRegFirst.Text);
                //登录积分，每天最多多少分
                Business.Do<ISystemPara>().Save("LoginPoint", tbLoginPoint.Text);
                Business.Do<ISystemPara>().Save("LoginPointMax", tbLoginPointMax.Text);
                //分享积分，每天最多多少分
                Business.Do<ISystemPara>().Save("SharePoint", tbSharePoint.Text);
                Business.Do<ISystemPara>().Save("SharePointMax", tbSharePointMax.Text);
                //注册积分，每天最多多少分
                Business.Do<ISystemPara>().Save("RegPoint", tbRegPoint.Text);
                Business.Do<ISystemPara>().Save("RegPointMax", tbRegPointMax.Text);
                //积分与卡券的兑换
                Business.Do<ISystemPara>().Save("PointConvert", tbPointConvert.Text);
                //刷新参数
                Business.Do<ISystemPara>().Refresh();

                this.Alert("操作成功");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
    }
}