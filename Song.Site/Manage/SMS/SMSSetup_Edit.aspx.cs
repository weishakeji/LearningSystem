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
    public partial class SMSSetup_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private string remarks = WeiSha.Common.Request.QueryString["id"].Decrypt().String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack && Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                fill();
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                string action = WeiSha.Common.Request.Form["action"].String;
                switch (action)
                {
                    case "getnumber":
                        GetNumber();
                        break;
                }
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            WeiSha.SMS.ISMS sms = WeiSha.SMS.Gatway.GetService(remarks);
            //短信平台名称
            ltName.Text = sms.Current.Name;
            //账号与密码
            tbSmsAcc.Text = Business.Do<ISystemPara>().GetValue(remarks + "SmsAcc");
            //tbSmsPw.Text = Business.Do<ISystemPara>().GetValue(remarks + "SmsPw");            
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
                Business.Do<ISystemPara>().Save(remarks + "SmsAcc", tbSmsAcc.Text.Trim());
                //密码加密存储
                string pw = WeiSha.Common.DataConvert.EncryptForBase64(tbSmsPw.Text.Trim());
                Business.Do<ISystemPara>().Save(remarks + "SmsPw", pw);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }  
        }
        /// <summary>
        /// 获取短信条数
        /// </summary>
        /// <param name="marks"></param>
        /// <returns></returns>
        protected void GetNumber()
        {
            string marks = WeiSha.Common.Request.QueryString["id"].Decrypt().String;
            int num = -1;
            try
            {
                //账号与密码
                string smsacc = Business.Do<ISystemPara>().GetValue(marks + "SmsAcc");
                string smspw = Business.Do<ISystemPara>().GetValue(marks + "SmsPw");
                if (!string.IsNullOrWhiteSpace(smspw))
                {
                    smspw = WeiSha.Common.DataConvert.DecryptForBase64(smspw);    //将密码解密
                    //短信平台操作对象
                    WeiSha.SMS.ISMS sms = WeiSha.SMS.Gatway.GetService(marks);
                    //设置账号与密码
                    sms.Current.User = smsacc;
                    sms.Current.Password = smspw;
                    num = sms.Query();
                    string json = "{\"num\":" + num + ",\"marks\":\"" + marks + "\"}";
                    Response.Write(json);
                }
                else
                {
                    throw new Exception("未填写密码！");
                }
            }
            catch(Exception ex)
            {
                num = -1;
                string json = "{\"num\":" + num + ",\"marks\":\"" + marks + "\",\"desc\":\"" + ex.Message + "\"}";
                Response.Write(json);
            }
            Response.End();
        }
    }
}
