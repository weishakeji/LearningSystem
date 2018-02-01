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
    public partial class SysPara_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
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
            try
            {
                Song.Entities.SystemPara mm;
                if (id != 0)
                {
                    mm = Business.Do<ISystemPara>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.SystemPara();
                }
                //参数的键、值、默认值
                tbKey.Text = mm.Sys_Key;
                tbValue.Text = mm.Sys_Value;
                tbDefault.Text = mm.Sys_Default;
                //单位、供选择的单位
                tbUnit.Text = mm.Sys_Unit;
                tbSecUnit.Text = mm.Sys_SelectUnit;
                //参数说明
                tbIntro.Text = mm.Sys_ParaIntro;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.SystemPara mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<ISystemPara>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.SystemPara();
                }
                //参数的键、值、默认值
                mm.Sys_Key = tbKey.Text.Trim();
                mm.Sys_Value = tbValue.Text.Trim();
                mm.Sys_Default = tbDefault.Text.Trim();
                //单位、供选择的单位
                mm.Sys_Unit = tbUnit.Text;
                mm.Sys_SelectUnit = tbSecUnit.Text.Trim();
                //参数说明
                mm.Sys_ParaIntro = tbIntro.Text.Trim();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                //确定操作
                if (id == 0)
                {
                    Business.Do<ISystemPara>().Add(mm);
                }
                else
                {
                    Business.Do<ISystemPara>().Save(mm);
                }
                Business.Do<ISystemPara>().Refresh();
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
