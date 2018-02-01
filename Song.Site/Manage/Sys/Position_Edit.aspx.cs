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
    public partial class Position_Edit : Extend.CustomPage
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
                Song.Entities.Position mm;
                if (id != 0)
                {
                    mm = Business.Do<IPosition>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Position();
                    mm.Posi_IsUse = true;
                }
                tbName.Text = mm.Posi_Name;
                //是否显示
                cbIsUse.Checked = mm.Posi_IsUse;
                if (mm.Posi_IsAdmin) cbIsUse.Enabled = false;
                //说明
                tbIntro.Text = mm.Posi_Intro;
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
                Song.Entities.Position mm;
                if (id != 0)
                {
                    mm = Business.Do<IPosition>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Position();
                    mm.Posi_IsUse = true;
                }
                //属性
                mm.Posi_Name = tbName.Text;
                mm.Posi_IsUse = cbIsUse.Checked;
                //说明
                mm.Posi_Intro = tbIntro.Text;
                //确定操作
                if (id == 0)
                {
                    Song.Entities.EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                    mm.Org_ID = acc.Org_ID;
                    mm.Org_Name = acc.Org_Name;
                    Business.Do<IPosition>().Add(mm);
                }
                else
                {
                    Business.Do<IPosition>().Save(mm);
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
