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

namespace Song.Site.Manage.Admin
{
    public partial class Setup_Stamp : Extend.CustomPage
    {
        private string _uppath = "Org";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {             
                fill();               
            }
        }

        private void fill()
        {
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
            string stamp = config["Stamp"].Value.String;
            //机构的公章
            if (!string.IsNullOrEmpty(stamp))
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + stamp;
            }
            //公章显示位置
            tbPosition.Text = config["StampPosition"].Value.String;
            if (string.IsNullOrEmpty(tbPosition.Text)) tbPosition.Text = "right-bottom";
        }
        protected void btn_Click(object sender, EventArgs e)
        {
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config); 
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = false;
                    //删除原有
                    string stamp = config["Stamp"].Value.String; 
                    if (!string.IsNullOrEmpty(stamp))
                    {
                        stamp = Upload.Get[_uppath].Physics + stamp;
                        if (System.IO.File.Exists(stamp)) System.IO.File.Delete(stamp);
                        
                    } 
                    fuLoad.SaveAs();
                    config["Stamp"].Text = fuLoad.File.Server.FileName;
                    imgShow.Src = fuLoad.File.Server.VirtualPath;

                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            //公章显示位置
            config["StampPosition"].Text = tbPosition.Text.Trim();
            try
            {
                org.Org_Config = config.XmlString;
                Business.Do<IOrganization>().OrganSave(org);
                this.Alert("操作成功！");
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
            }
        }
      
        
    }
}
