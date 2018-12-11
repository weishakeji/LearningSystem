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
namespace Song.Site.Manage.Admin
{
    public partial class Student_Export : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
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
            Song.Entities.StudentSort[] eas = null;
            eas = Business.Do<IStudent>().SortCount(org.Org_ID, null, -1);
            cblSort.DataSource = eas;
            cblSort.DataTextField = "sts_name";
            cblSort.DataValueField = "sts_id";
            cblSort.DataBind();
            //学员总数
            int sumcount = Business.Do<IAccounts>().AccountsOfCount(org.Org_ID, true);
            lbSumcount.Text = sumcount.ToString();
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string sorts = "";
            foreach (ListItem li in cblSort.Items)
            {
                if (li.Selected) sorts += li.Value + ",";
            }
            //导出
            string filename = "学员导出_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm") + ".xls";   //创建文件
            string filePath = Upload.Get["Temp"].Physics + filename;
            filePath = Business.Do<IAccounts>().AccountsExport4Excel(filePath, org.Org_ID, sorts);
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