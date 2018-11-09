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
    public partial class General : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        protected void fill()
        {
            
            //是否记录登录信息
            this.cbLogin.Checked = Business.Do<ISystemPara>()["SysIsLoginLogs"].Boolean ?? true;
            this.cbWork.Checked = Business.Do<ISystemPara>()["SysIsWorkLogs"].Boolean ?? true;
            //时限长度
            this.tbLoginTimeSpan.Text = Business.Do<ISystemPara>()["SysLoginTimeSpan"].String;
            this.tbWorkTimeSpan.Text = Business.Do<ISystemPara>()["SysWorkTimeSpan"].String;
            //系统名称
            this.tbSysName.Text = Business.Do<ISystemPara>()["SystemName"].String;
            //是否启用多机构，默认启用
            int multi = Business.Do<ISystemPara>()["MultiOrgan"].Int32 ?? 0;
            ListItem li = rblMultiOrgan.Items.FindByValue(multi.ToString());
            if (li != null) li.Selected = true;
            
        }
        /// <summary>
        /// 将参数保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {            
            //是否记录登录信息
            Business.Do<ISystemPara>().Save("SysIsLoginLogs", cbLogin.Checked.ToString(), false);
            Business.Do<ISystemPara>().Save("SysIsWorkLogs", this.cbWork.Checked.ToString(), false);
            //记录登录信息的时效
            Business.Do<ISystemPara>().Save("SysLoginTimeSpan", tbLoginTimeSpan.Text, false);
            Business.Do<ISystemPara>().Save("SysWorkTimeSpan", tbWorkTimeSpan.Text, false);
            //管理平台的名称称称
            Business.Do<ISystemPara>().Save("SystemName", this.tbSysName.Text.Trim(), false);
            //是否启用多机构
            Business.Do<ISystemPara>().Save("MultiOrgan", this.rblMultiOrgan.SelectedValue, false);
            //刷新全局参数
            Business.Do<ISystemPara>().Refresh();            
        }

    }
}
