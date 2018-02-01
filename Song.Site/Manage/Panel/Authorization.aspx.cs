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

namespace Song.Site.Manage.Panel
{
    public partial class Authorization : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) init();
            //接收上传的授权文件
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                HttpPostedFile file=null;
                for (int i = 0; i < this.Request.Files.Count; ++i)                
                    file = this.Request.Files[i];
                if (file == null || file.ContentLength==0) return;
                if (!Extend.LoginState.Admin.IsSuperAdmin)
                {
                    lbShow.Text = "*此操作仅限超级管理员";
                    return;
                }
                //
                if (file.FileName.ToLower() != "license.txt")
                {
                    lbShow.Text = "*请上传license.txt文件";
                }
                else
                {
                    string serverfile = HttpContext.Current.Server.MapPath("\\") + file.FileName;
                    try
                    {
                        if (System.IO.File.Exists(serverfile))
                        {
                            string newfile = file.FileName.Substring(0, file.FileName.IndexOf("."));
                            newfile += "(" + DateTime.Now.ToString("yyyy-M-d hhmmss") + ").txt";
                            System.IO.File.Move(serverfile, HttpContext.Current.Server.MapPath("\\") + newfile);
                        }
                        file.SaveAs(serverfile);
                        lbShow.Text = "*授权文件上传成功！";
                        WeiSha.Common.License.Value.Init();
                        init();
                    }
                    catch
                    {
                        lbShow.Text = "*授权文件无法写入！";
                    }
                }
            }
        }
        protected void init()
        {
            WeiSha.Common.License lic = WeiSha.Common.License.Value;
            //是否获得授权
            if (lic.IsLicense)
            {
                plYesLic.Visible = true;
                plNoLic.Visible = false;
                //当前版本
                lbVersion.Text = lic.VersionName;
                //授权类型，授权主体，起始时间
                lbLicType.Text = lic.Type.ToString();
                if ((int)lic.Type == 1 || (int)lic.Type == 2)
                    lbLicInfo.Text = lic.Serial;
                else
                    lbLicInfo.Text = lic.Serial + ":" + lic.Port;
                lbStartTime.Text = lic.StartTime.ToString("yyyy-MM-dd");
                lbEndTime.Text = lic.EndTime.ToString("yyyy-MM-dd");
            }
            else
            {
                //没有获得授权
                plYesLic.Visible = false;
                plNoLic.Visible = true;
                rblActivType_SelectedIndexChanged(null, null);
            }
            //当前版本的限制
            rptLimit.DataSource = lic.LimitItems;
            rptLimit.DataBind();
        }
        /// <summary>
        /// 激活码类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblActivType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int type = Convert.ToInt16(rblActivType.SelectedValue);
            if (type == 1) lbActivCode.Text = WeiSha.Common.Activationcode.CodeForCPU;
            if (type == 2) lbActivCode.Text = WeiSha.Common.Activationcode.CodeForHardDisk;
            if (type == 3) lbActivCode.Text = WeiSha.Common.Activationcode.CodeForIP;
            if (type == 4) lbActivCode.Text = WeiSha.Common.Activationcode.CodeForDomain;
            if (type == 5) lbActivCode.Text = WeiSha.Common.Activationcode.CodeForRoot;         //主域激活码
        }
        
    }
}
