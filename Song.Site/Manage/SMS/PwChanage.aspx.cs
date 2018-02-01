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
    public partial class PwChanage : Extend.CustomPage
    {      
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
            //短信帐号           
            lbName.Text = Business.Do<ISystemPara>()["SMS_Account"].String;    
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //// 返回的结果,XML字符串
            //string strReturnXMLData = string.Empty;
            //// 结果代码
            //string Code = string.Empty;
            //// 描述信息
            //string Description = string.Empty;

            //strReturnXMLData = SMSService.ChgPwd(this.lbName.Text.Trim(), this.tbPwOld.Text.Trim(), this.tbPw1.Text.Trim());
            //XmlDocument xd = new XmlDocument();
            //xd.LoadXml(strReturnXMLData);
            //Code = xd.DocumentElement.SelectSingleNode("Result").InnerText.Trim();
            //Description = xd.DocumentElement.SelectSingleNode("Description").InnerText.Trim();
            //if (Code.Trim() == "0")
            //{
            //    lbSend.Text = "恭喜，密码修改成功，请使用新密码登录系统。";
            //    Parameters.Global.Value.Set("SMS_Password", tbPw1.Text.Trim());
            //}
            //else
            //{
            //    lbSend.Text = "密码修改失败，原因可能是：" + Description.Trim() + "。";
            //}
        }
    }
}
