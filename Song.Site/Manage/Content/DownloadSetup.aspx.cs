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
    public partial class DownloadSetup : Extend.CustomPage
    {
        private string _uppath = "Download";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        { 
            //二维码
            tbQrWH.Text = Business.Do<ISystemPara>()["Download_QrCode_WidthAndHeight"].String;
            tbQrTextTmp.Text = Business.Do<ISystemPara>()["Download_QrCode_Template"].String;
        }
        /// <summary>
        /// 保存下载信息的二维码配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            //文章二维码
            Business.Do<ISystemPara>().Save("Download_QrCode_WidthAndHeight", tbQrWH.Text);
            Business.Do<ISystemPara>().Save("Download_QrCode_Template", tbQrTextTmp.Text);
        }
        /// <summary>
        /// 批量生成下载信息的二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                btnEnter_Click(null, null);
                //二维码的宽高
                int wh = Business.Do<ISystemPara>()["Download_QrCode_WidthAndHeight"].Int16 ?? 200;
                //二维码模板内容
                string template = Business.Do<ISystemPara>()["Download_QrCode_Template"].String;
                //开始批量生成
                int sum = 0;
                int size = 20;
                int index = 1;
                Song.Entities.Download[] entities;
                do
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                    entities = Business.Do<IContents>().DownloadPager(org.Org_ID, null, "", null, null, size, index, out sum);
                    foreach (Song.Entities.Download entity in entities)
                    {
                        createQrCode(entity, _uppath, template, wh);
                    }
                } while (size * index++ < sum);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 生成下载信息二维码
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private string createQrCode(Song.Entities.Download di, string pathType, string template, int wh)
        {
            try
            {
                //二维码图片名称
                string img = "";
                if (di != null && di.Dl_QrCode != null && di.Dl_QrCode != "")
                {
                    img = di.Dl_QrCode;
                }
                else
                {
                    img = "Di_" + WeiSha.Common.Request.UniqueID() + ".png";
                    di.Dl_QrCode = img;
                    Business.Do<IContents>().DownloadSave(di);
                }
                //创建二维码
                Song.Extend.QrCode.Creat4Entity(di, template, Upload.Get[pathType].Physics + img, wh);
                return img;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return null;
            }
        }
    }
}
