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
    public partial class VideoRecycle : Extend.CustomPage
    {
        //当前图片分类的顶级类，如果是为0，则显示所有；
        int pcid = WeiSha.Common.Request.QueryString["pcid"].Int32 ?? 0;
        private string _uppath = "Video";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.Video[] eas = null;
                eas = Business.Do<IContents>().VideoPager(org.Org_ID, null, true, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
                //资源的路径
                string resPath = Upload.Get[_uppath].Virtual;
                foreach (Song.Entities.Video entity in eas)
                {
                    entity.Vi_FilePath = resPath + entity.Vi_FilePath;
                    entity.Vi_FilePathSmall = resPath + entity.Vi_FilePathSmall;
                }
                rptPict.DataSource = eas;
                rptPict.DataBind();
                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }
        /// <summary>
        /// 永久删除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDel_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton ub = (LinkButton)sender;
                int id = Convert.ToInt32(ub.CommandArgument);
                Business.Do<IContents>().VideoDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 图片还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbRestore_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton ub = (LinkButton)sender;
                int id = Convert.ToInt32(ub.CommandArgument);
                Business.Do<IContents>().VideoRecover(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
