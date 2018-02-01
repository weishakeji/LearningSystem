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
using WeiSha.WebControl;


namespace Song.Site.Manage.Sys
{
    public partial class SiteBackup : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                try
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganDefault();
                    tbName.Text = org.Org_Name;
                }
                catch (Exception ex)
                {
                    Message.ExceptionShow(ex);
                }
            }
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                //压缩包名称
                string site = tbName.Text + lbDate.Text + lbTime.Text + ".zip";
                //模板所在的物理路径
                string hyPath = Server.MapPath(ResolveUrl("/"));
                string zip = Server.MapPath(ResolveUrl("/" + site));
                //实行压缩
                Extend.Compress.ZipFile(hyPath, zip);
                FileInfo fileInfo = new FileInfo(zip);

                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(site));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                if (System.IO.File.Exists(zip))
                {
                    System.IO.File.Delete(zip);
                }

                Response.End();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            
        }
        /// <summary>
        /// 是否增加日期与时间的后缀
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbDate.Checked && cbTime.Checked)
                {
                    lbDate.Text = DateTime.Now.ToString("yyyy-MM-dd ");
                    this.lbTime.Text = DateTime.Now.ToString("hh-mm-ss");
                    return;
                }
                if (cbDate.Checked)
                {
                    lbDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    lbDate.Text = "";
                }
                if (cbTime.Checked)
                {
                    this.lbTime.Text = DateTime.Now.ToString("HH-mm-ss");
                }
                else
                {
                    lbTime.Text = "";
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
