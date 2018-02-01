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
using System.Xml;

namespace Song.Site.Manage.SMS
{
    public partial class SMSSendSingle : Extend.CustomPage
    {
        //联系人id
        int addrid = WeiSha.Common.Request.QueryString["id"].Int32 ?? -1;
        //员工id
        int empid = WeiSha.Common.Request.QueryString["empid"].Int32 ?? -1;
        //消息id
        int smsid = WeiSha.Common.Request.QueryString["smsid"].Int32 ?? -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {

            if (addrid > -1)
            {
                Song.Entities.AddressList addr = Business.Do<IAddressList>().AddressSingle(addrid);
                if (addr != null)
                {
                    lbName.Text = addr.Adl_Name;
                    lbMobile.Text = addr.Adl_MobileTel;
                    lbCompany.Text = addr.Adl_Company;
                }
            }
            if (empid > -1)
            {
                Song.Entities.EmpAccount ea = Business.Do<IEmployee>().GetSingle(empid);
                lbName.Text = ea.Acc_Name;
                lbMobile.Text = ea.Acc_MobileTel;
                lbCompany.Text = ea.Dep_CnName;
            }
            if (smsid > -1)
            {
                Song.Entities.SmsMessage tm = Business.Do<ISMS>().GetSingle(smsid);
                tbContext.Text = tm.Sms_Context;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
           //电话号码
            string mobile = lbMobile.Text;
            //发送
            DateTime time = tbSendTime.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(tbSendTime.Text);
            WeiSha.SMS.SmsState state = WeiSha.SMS.Gatway.Service.Send(mobile, tbContext.Text.Trim(), time);
            if (state.Success)
            {
                lblSend.Text = "恭喜，短信群发成功！";
                Song.Entities.SmsMessage sms = new SmsMessage();
                sms.Sms_Context = tbContext.Text.Trim();
                //2为已经发送
                sms.Sms_MailBox = 2;
                //针对的类型,1针对分类
                if (addrid > -1)
                {
                    sms.Sms_Type = 2;
                    sms.Sms_SendId = addrid;
                    sms.Sms_SendName = lbName.Text;
                }
                if (empid > -1)
                {
                    sms.Sms_Type = 3;
                    sms.Sms_SendId = empid;
                    sms.Sms_SendName = lbName.Text;
                }

                if (state.FailList.Trim() != "")
                {
                    lblSend.Text += "其中，发送失败的号码为：" + state.FailList + "。";
                    //3为部分失败
                    sms.Sms_State = 3;
                }
                else
                {
                    sms.Sms_State = 1;
                }
                Business.Do<ISMS>().MessageAdd(sms);
            }
            else
            {
                lblSend.Text = "发送失败，原因可能是：" + state.Description + "。";
            }           
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Song.Entities.SmsMessage sms = new SmsMessage();
            sms.Sms_Context = tbContext.Text.Trim();
            //1为草稿箱
            sms.Sms_MailBox = 1;
            //0为未发送
            sms.Sms_State = 0;
            //针对的类型,1针对分类
            if (addrid > -1)
            {
                sms.Sms_Type = 2;
                sms.Sms_SendId = addrid;
                sms.Sms_SendName = lbName.Text;               
            }
            if (empid > -1)
            {
                sms.Sms_Type = 3;
                sms.Sms_SendId = empid;
                sms.Sms_SendName = lbName.Text;   
            } 
            //
            try
            {
                Business.Do<ISMS>().MessageAdd(sms);
            }
            catch (Exception ex)
            {
                new Song.Extend.Scripts(this).Alert(ex.Message);
            }
        }
    }
}
