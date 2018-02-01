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

namespace Song.Site.Manage.Site
{
    public partial class UserGroup_Edit : Extend.CustomPage
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
                Song.Entities.UserGroup mm;
                if (id != 0)
                {
                    mm = Business.Do<IUser>().GetGroupSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.UserGroup();
                    mm.UGrp_IsUse = true;
                }
                tbName.Text = mm.UGrp_Name;
                //是否显示
                cbIsUse.Checked = mm.UGrp_IsUse;
                //说明
                tbIntro.Text = mm.UGrp_Intro;
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
                Song.Entities.UserGroup mm;
                if (id != 0)
                {
                    mm = Business.Do<IUser>().GetGroupSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.UserGroup();
                    mm.UGrp_IsUse = true;
                }
                //属性
                mm.UGrp_Name = tbName.Text;
                mm.UGrp_IsUse = cbIsUse.Checked;
                //说明
                mm.UGrp_Intro = tbIntro.Text;
                //确定操作
                if (id == 0)
                {
                    Business.Do<IUser>().AddGroup(mm);
                }
                else
                {
                    Business.Do<IUser>().SaveGroup(mm);
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
