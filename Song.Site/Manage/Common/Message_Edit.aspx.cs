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

namespace Song.Site.Manage.Common
{
    public partial class Message_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private int targetId = WeiSha.Common.Request.QueryString["targetId"].Int32 ?? 0;
        protected Song.Entities.Message msg;
        //是否让修改信息，该变量用于前端页面js调用。
        protected string allowModify = "true";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        private void fill()
        {
            msg = Business.Do<IMessage>().GetSingle(id);
            //消息内容
            tbMsg.Text = msg.Msg_Context;
            tbReply.Text = msg.Msg_ReContext;
            //联系方式
            tbQQ.Text = msg.Msg_QQ;
            tbPhone.Text = msg.Msg_Phone;
            //时间
            lbCrtTime.Text = msg.Msg_CrtTime.ToString();
            //是否阅读
            if (msg.Msg_State == 0)
            {
                msg.Msg_State = 1;
                Business.Do<IMessage>().Save(msg);
            }
         
        }
        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAnswer_Click(object sender, EventArgs e)
        {
            try
            {
                Song.Entities.Message msg = Business.Do<IMessage>().GetSingle(id);
                //消息内容与回复
                msg.Msg_Context = tbMsg.Text.Trim();
                msg.Msg_ReContext = tbReply.Text.Trim();
                if (!string.IsNullOrWhiteSpace(msg.Msg_ReContext))
                {
                    msg.Msg_IsReply = true;
                }
                //联系方式
                msg.Msg_QQ = tbQQ.Text;
                msg.Msg_Phone = tbPhone.Text;
                Business.Do<IMessage>().Save(msg);
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.Alert(ex);
            }
        }
    }
}
