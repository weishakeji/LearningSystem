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
using System.Collections.Generic;

namespace Song.Site.Manage.SMS
{
    public partial class SMSSend : Extend.CustomPage
    {
        //针对通讯录时，通讯录的分类id
        int sortid = WeiSha.Common.Request.QueryString["sortid"].Int32 ?? -1;
        //当从草稿箱等打开时
        int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? -1;
        //消息id
        int smsid = WeiSha.Common.Request.QueryString["smsid"].Int32 ?? -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                init();
                fill();
            }
        }
        private void init()
        {
            Song.Entities.SmsMessage sms = Business.Do<ISMS>().GetSingle(id);
            if (sms == null) return;
            if (sms.Sms_Type == 1)
            {
                this.Response.Redirect("smsSend.aspx?sortid=" + sms.Sms_SendId + "&smsid=" + sms.SMS_Id);
            }
            if (sms.Sms_Type == 2)
            {
                this.Response.Redirect("SMSSendSingle.aspx?id=" + sms.Sms_SendId + "&smsid=" + sms.SMS_Id);
            }
            if (sms.Sms_Type == 3)
            {
                this.Response.Redirect("SMSSendSingle.aspx?empid=" + sms.Sms_SendId + "&smsid=" + sms.SMS_Id);
            }
            if (smsid > -1)
            {
                Song.Entities.SmsMessage tm = Business.Do<ISMS>().GetSingle(smsid);
                tbContext.Text = tm.Sms_Context;
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
           
            List<Song.Entities.AddressSort> asort = Business.Do<IAddressList>().SortAll(true);
            ddlTpye.DataSource = asort;
            ddlTpye.DataTextField = "ads_name";
            ddlTpye.DataValueField = "ads_id";
            ddlTpye.DataBind();
            this.ddlTpye.Items.Insert(0, new ListItem(" -- 所有分类 -- ", "-1"));

            ListItem li = ddlTpye.Items.FindByValue(sortid.ToString());
            if (li != null)
            {
                ddlTpye.SelectedIndex = -1;
                li.Selected = true;
            }
           
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //分类
            int sortId = Convert.ToInt32(ddlTpye.SelectedValue);
            //联系人
            List<Song.Entities.AddressList> addr = Business.Do<IAddressList>().AddressAll(sortId);
            string moibles = "";
            foreach (Song.Entities.AddressList l in addr)
            {
                if (l.Adl_MobileTel != string.Empty)
                    moibles += l.Adl_MobileTel + ",";
            }
            if (moibles.Length > 0 && moibles.Substring(moibles.Length - 1) == ",")
            {
                moibles = moibles.Substring(0, moibles.Length - 1);
            }
            //发送
            DateTime time = tbSendTime.Text.Trim() == "" ? DateTime.Now : Convert.ToDateTime(tbSendTime.Text);
            WeiSha.SMS.SmsState state = WeiSha.SMS.Gatway.Service.Send(moibles, tbContext.Text.Trim(),time);
            if (state.Success)
            {
                lblSend.Text = "恭喜，短信群发成功！";
                Song.Entities.SmsMessage sms = new SmsMessage();
                sms.Sms_Context = tbContext.Text.Trim();
                //2为已经发送
                sms.Sms_MailBox = 2;
                //针对的类型,1针对分类
                sms.Sms_Type = 1;
                sms.Sms_SendId = Convert.ToInt32(ddlTpye.SelectedValue);
                sms.Sms_SendName = ddlTpye.SelectedItem.Text.Replace("-", "").Trim();

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
        /// <summary>
        /// 保存到草稿箱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Song.Entities.SmsMessage sms = new SmsMessage();
            sms.Sms_Context = tbContext.Text.Trim();
            //1为草稿箱
            sms.Sms_MailBox = 1;
            //0为未发送
            sms.Sms_State = 0;
            //针对的类型,1针对分类
            sms.Sms_Type = 1;

            //
            sms.Sms_SendId = Convert.ToInt32(ddlTpye.SelectedValue);
            sms.Sms_SendName = ddlTpye.SelectedItem.Text.Replace("-","").Trim();
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
