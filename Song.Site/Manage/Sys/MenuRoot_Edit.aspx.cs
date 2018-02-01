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
using System.IO;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Sys
{
    public partial class MenuRoot_Edit : Extend.CustomPage
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
                Song.Entities.ManageMenu mm;
                if (id != 0)
                {
                    mm = Business.Do<IManageMenu>().GetSingle(id);
                    //是否显示
                    cbIsShow.Checked = mm.MM_IsShow;
                    cbIsUse.Checked = mm.MM_IsUse;
                    //是否粗体
                    cbIsBold.Checked = mm.MM_IsBold;
                    //是否斜体
                    cbIsItalic.Checked = mm.MM_IsItalic;   
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.ManageMenu();
                }
                //名称
                tbName.Text = mm.MM_Name;      
                //标识
                tbMarker.Text = mm.MM_Marker;
                //说明
                tbIntro.Text = mm.MM_Intro;
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
                Song.Entities.ManageMenu mm;
                if (id != 0)
                {
                    mm = Business.Do<IManageMenu>().GetSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new ManageMenu();
                    mm.MM_IsShow = true;
                }
                //属于功能菜单
                mm.MM_Func = "func";
                //类别
                mm.MM_Type = "item";
                //名称
                mm.MM_Name = tbName.Text.Trim();
                mm.MM_Marker = tbMarker.Text.Trim();
                //是否粗体
                mm.MM_IsBold = cbIsBold.Checked;
                //是否斜体
                mm.MM_IsItalic = cbIsItalic.Checked;
                //是否显示
                mm.MM_IsShow = cbIsShow.Checked;
                mm.MM_IsUse = cbIsUse.Checked;
                //说明
                mm.MM_Intro = tbIntro.Text;
                //确定操作
                if (id == 0)
                {
                    Business.Do<IManageMenu>().RootAdd(mm);
                }
                else
                {
                    Business.Do<IManageMenu>().RootSave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<IManageMenu>().RootDelete(id);
                Master.AlertCloseAndRefresh("成功删除");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

        

    }
}
