using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Song.ServiceInterfaces;
using WeiSha.Common;
using System.IO;

namespace Song.Site.Manage.Money
{
    public partial class RechargeCode_Output : System.Web.UI.Page
    {
        //充值码设置项的id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.RechargeSet mm = Business.Do<IRecharge>().RechargeSetSingle(id);
            //创建文件
            string name = string.Format("{0}-充值码（{1}）.xls", mm.Rs_Theme, DateTime.Now.ToString("yyyy-MM-dd hh-mm"));
            string filePath = Upload.Get["Temp"].Physics + name;
            filePath = Business.Do<IRecharge>().RechargeCode4Excel(filePath, org.Org_ID, id);
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