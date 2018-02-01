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
    public partial class DataClear : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        #region 内容管理部分的清理
        /// <summary>
        /// 删除新闻文章
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnNewsArticle_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<IContents>().ArticleDeleteAll(-1, -1);
                new Extend.Scripts(this).Alert("成功清空新闻文章");
            }
            catch (Exception ex)
            {
                new Extend.Scripts(this).Alert(ex.Message);
            }
            
        }
        #endregion

        /// <summary>
        /// 清除通讯录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAddressBook_Click(object sender, EventArgs e)
        {
            try
            {
                Business.Do<IAddressList>().Clear();
                Alert("成功清空通讯录信息！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }

    }
}
