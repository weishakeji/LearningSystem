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
using System.Text;
using System.Collections.Generic;

namespace Song.Site.Manage.SMS
{
    public partial class SMSSetup_Msg : Extend.CustomPage
    {
        //要操作的对象主键
        private string remarks = WeiSha.Common.Request.QueryString["remarks"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack && Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            //短信模板
            tbSmsTemplate.Text = Business.Do<ISystemPara>().GetValue(remarks + "_SmsTemplate");    
            //模板实际效果
            lbMsg.Text = Business.Do<ISMS>().MessageFormat(tbSmsTemplate.Text, DateTime.Now.ToString("mmss")); 
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
                Business.Do<ISystemPara>().Save(remarks + "_SmsTemplate", tbSmsTemplate.Text.Trim());
                //模板实际效果
                lbMsg.Text = Business.Do<ISMS>().MessageFormat(tbSmsTemplate.Text, DateTime.Now.ToString("mmss")); 
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }  
           
        }

    }
}
