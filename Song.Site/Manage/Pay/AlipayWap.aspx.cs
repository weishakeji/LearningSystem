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
using System.Text.RegularExpressions;



namespace Song.Site.Manage.Pay
{
    public partial class AlipayWap : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
                Pai_Pattern.DdlInterFace.Enabled = false;
            } 
        }
        private void fill()
        {
            Song.Entities.PayInterface pi = id <= 0 ? null : Business.Do<IPayInterface>().PaySingle(id);
            if (pi == null) return;
            this.EntityBind(pi);
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(pi.Pai_Config);
            tbPrivatekey.Text = config["Privatekey"].Value.String;           
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.PayInterface pi = id <= 0 ? new Song.Entities.PayInterface() : Business.Do<IPayInterface>().PaySingle(id);
            pi = this.EntityFill(pi) as Song.Entities.PayInterface;
            //支付方式
            pi.Pai_Pattern = Pai_Pattern.DdlInterFace.SelectedItem.Text;
            //自定义配置项
            WeiSha.Common.CustomConfig config = CustomConfig.Load(pi.Pai_Config);
            string privateKey = Regex.Replace(tbPrivatekey.Text.Trim(), @"\r|\n|\s", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            config["Privatekey"].Text = privateKey;
            pi.Pai_Config = config.XmlString;
            //所用的平台
            pi.Pai_Platform = "mobi";
            //只能根机构才可以设置支付接口（也就是说，钱全到根机构账上）
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganRoot();
            if (org != null) pi.Org_ID = org.Org_ID;
            try
            {
                if (id <= 0)
                {
                    Business.Do<IPayInterface>().PayAdd(pi);
                }
                else
                {
                    Business.Do<IPayInterface>().PaySave(pi);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
    }
}
