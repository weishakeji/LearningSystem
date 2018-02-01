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
    public partial class EmpGroup_Edit : Extend.CustomPage
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
                Song.Entities.EmpGroup mm;
                if (id != 0)
                {
                    mm = Business.Do<IEmpGroup>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.EmpGroup();
                    mm.EGrp_IsUse = true;
                }
                //组名称
                tbName.Text = mm.EGrp_Name;
                //如果是系统组，则不允许修改
                tbName.Enabled = !mm.EGrp_IsSystem;
                lbIsSytem.Visible = mm.EGrp_IsSystem;
                //是否显示
                cbIsUse.Checked = mm.EGrp_IsUse;
                //说明
                tbIntro.Text = mm.EGrp_Intro;
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
            try
            {
                Song.Entities.EmpGroup mm;
                if (id != 0)
                {
                    mm = Business.Do<IEmpGroup>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.EmpGroup();
                    mm.EGrp_IsUse = true;
                }
                //属性
                mm.EGrp_Name = tbName.Text;
                mm.EGrp_IsUse = cbIsUse.Checked;
                //说明
                mm.EGrp_Intro = tbIntro.Text;
                //确定操作
                if (id == 0)
                {
                    Song.Entities.EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                    mm.Org_ID = acc.Org_ID;
                    mm.Org_Name = acc.Org_Name;
                    Business.Do<IEmpGroup>().Add(mm);
                }
                else
                {
                    Business.Do<IEmpGroup>().Save(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
