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
    public partial class DownloadType_Edit : Extend.CustomPage
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
                Song.Entities.DownloadType mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().DownloadTypeSingle(id);
                    cbIsUse.Checked = mm.Dty_IsUse;
                }
                else
                {
                    mm = new Song.Entities.DownloadType();
                }
                tbName.Text = mm.Dty_Name;
                tbIntro.Text = mm.Dty_Intro;
                tbDetails.Text = mm.Dty_Details;
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
            Song.Entities.DownloadType mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IContents>().DownloadTypeSingle(id);
                }
                else
                {
                    mm = new Song.Entities.DownloadType();
                }
                mm.Dty_Name = tbName.Text.Trim();
                mm.Dty_IsUse = cbIsUse.Checked;
                mm.Dty_Intro = tbIntro.Text.Trim();
                mm.Dty_Details = tbDetails.Text;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                if (id != 0)
                {
                    Business.Do<IContents>().DownloadTypeSave(mm);
                }
                else
                {
                    Business.Do<IContents>().DownloadTypeAdd(mm);
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
