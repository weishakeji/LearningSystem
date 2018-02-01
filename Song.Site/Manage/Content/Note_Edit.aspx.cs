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
    public partial class Note_Edit : Extend.CustomPage
    {
        //当前分类的顶级类，如果是为0，则显示所有；
        public int pid = WeiSha.Common.Request.QueryString["pid"].Int32 ?? 0;
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

                Song.Entities.NewsNote mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().NoteSingle(id);
                    lbProvince.Text = mm.Nn_Province;
                    lbCity.Text = mm.Nn_City;
                    lbIP.Text = mm.Nn_IP;
                    lbCrtTime.Text = mm.Nn_CrtTime.ToString();

                    tbTitle.Text = mm.Nn_Title;
                    tbDetails.Text = mm.Nn_Details;

                    cbIsShow.Checked = mm.Nn_IsShow;
                }
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
            Song.Entities.NewsNote mm;
            if (id != 0)
            {
                mm = Business.Do<IContents>().NoteSingle(id);
                mm.Nn_Title = tbTitle.Text.Trim();
                mm.Nn_Details = tbDetails.Text.Trim();
                mm.Nn_IsShow = cbIsShow.Checked;
                try
                {
                    Business.Do<IContents>().NoteSave(mm);
                    Master.AlertCloseAndRefresh("操作成功！");
                }
                catch (Exception ex)
                {
                    Master.Alert(ex.Message);
                }
            }
        }
       
    }
}
