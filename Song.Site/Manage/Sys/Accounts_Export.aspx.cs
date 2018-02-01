using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
namespace Song.Site.Manage.Sys
{
    public partial class Accounts_Export : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                InitBind();
            }
        }
        #region 初始操作
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            Song.Entities.Organization[] eas = null;
            eas = Business.Do<IOrganization>().OrganCount(null, null, -1, -1);
            cblOrg.DataSource = eas;
            cblOrg.DataTextField = "org_name";
            cblOrg.DataValueField = "org_id";
            cblOrg.DataBind();
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string sorts = "";
            foreach (ListItem li in cblOrg.Items)
            {
                if (li.Selected) sorts += li.Value + ",";
            }
            //导出
            string filename = "学员导出_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";   //创建文件
            string filePath = Upload.Get["Temp"].Physics + filename;
            filePath = Business.Do<IAccounts>().AccountsExport4Excel(filePath, sorts);
            if (System.IO.File.Exists(filePath))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileInfo.Name));
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.AddHeader("Content-Transfer-Encoding", "binary");
                Response.ContentType = "application/-excel";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.WriteFile(fileInfo.FullName);
                Response.Flush();
                Response.End();
                File.Delete(filePath);
            }
        }
    }
}