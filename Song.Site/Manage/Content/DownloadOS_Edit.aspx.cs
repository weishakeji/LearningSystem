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

namespace Song.Site.Manage.Content
{
    public partial class DownloadOS_Edit : Extend.CustomPage
    {
        //当前信息的id
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
                Song.Entities.DownloadOS mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().DownloadOSSingle(id);
                    cbIsUse.Checked = mm.Dos_IsUse;
                }
                else
                {
                    mm = new Song.Entities.DownloadOS();
                }
                tbName.Text = mm.Dos_Name;
                tbIntro.Text = mm.Dos_Intro;
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
            Song.Entities.DownloadOS mm;
            if (id != 0)
            {
                mm = Business.Do<IContents>().DownloadOSSingle(id);
            }
            else
            {
                mm = new Song.Entities.DownloadOS();
            }
            mm.Dos_Name = tbName.Text.Trim();
            mm.Dos_IsUse = cbIsUse.Checked;
            mm.Dos_Intro = tbIntro.Text.Trim();
            try
            {
                if (id != 0)
                {
                    Business.Do<IContents>().DownloadOSSave(mm);
                }
                else
                {
                    Business.Do<IContents>().DownloadOSAdd(mm);
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
