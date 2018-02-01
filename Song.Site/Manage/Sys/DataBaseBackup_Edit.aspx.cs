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
    public partial class DataBaseBackup_Edit : Extend.CustomPage
    {
        //备份文件名
        private string backfile = WeiSha.Common.Request.QueryString["id"].Decrypt().String ?? "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                full();
            }
           
        }
        private void full()
        {
            try
            {
                if (backfile != String.Empty)
                {
                    FileInfo file = new FileInfo(backfile);
                    //文件名称
                    this.tbName.Text = file.Name.Substring(0, file.Name.LastIndexOf('.'));
                    //文件大小
                    lbSize.Text = (file.Length / 1024).ToString() + " Kb";
                    //创建时间
                    lbTime.Text = file.CreationTime.ToString();

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
            try
            {
                FileInfo file = new FileInfo(backfile);
                //所在路径
                string path = file.FullName;
                path = path.Substring(0, path.LastIndexOf('\\') + 1);
                //后缀名
                string ext = file.Extension.ToLower();
                if (ext == "backup" || ext == "bak")
                {
                    //目标文件
                    string newBack = path + tbName.Text + ext;
                    file.MoveTo(newBack);
                }
                //重新载入
                //this.Response.Redirect("DataBaseBackup_Edit.aspx?id=" + Server.UrlEncode(newBack));
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
            //Business.Do<IPosition>().Delete(id);
            //Master.AlertAndClose("成功删除！");
        }

        

    }
}
