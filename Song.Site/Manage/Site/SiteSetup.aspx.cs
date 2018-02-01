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

namespace Song.Site.Manage.Site
{
    public partial class SiteSetup : Extend.CustomPage
    {
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
                //站点是否采用静态页
                this.cbIsStatic.Checked = Business.Do<ISystemPara>()["IsWebsiteSatatic"].Boolean ?? false;
                //文章管理，是否需要通过权限控制
                this.cbIsArticlePurview.Checked = Business.Do<ISystemPara>()["IsArticlePurview"].Boolean ?? false;
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
                //站点是否采用静态页
                Business.Do<ISystemPara>().Save("IsWebsiteSatatic", cbIsStatic.Checked.ToString(), false);
                //文章管理，是否需要通过权限控制
                Business.Do<ISystemPara>().Save("IsArticlePurview", cbIsArticlePurview.Checked.ToString(), false);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
            
        }
    }
}
