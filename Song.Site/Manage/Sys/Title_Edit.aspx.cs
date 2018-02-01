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
    public partial class Title_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
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
                Song.Entities.EmpTitle mm;
                if (id != 0)
                {
                    mm = Business.Do<IEmployee>().TitleSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.EmpTitle();
                    mm.Title_IsUse = true;
                }
                tbName.Text = mm.Title_Name;
                //是否显示
                cbIsUse.Checked = mm.Title_IsUse;
                //说明
                tbIntro.Text = mm.Title_Intro;
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
            Song.Entities.EmpTitle mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IEmployee>().TitleSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.EmpTitle();
                    mm.Title_IsUse = true;
                }
                //属性
                mm.Title_Name = tbName.Text;
                mm.Title_IsUse = cbIsUse.Checked;
                //说明
                mm.Title_Intro = tbIntro.Text;
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
                    int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                    mm.Org_ID = orgid;
                    Business.Do<IEmployee>().TitileAdd(mm);
                }
                else
                {
                    Business.Do<IEmployee>().TitleSave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
