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
    public partial class ProductSetup : System.Web.UI.Page
    {
        private string _uppath = "Product";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            try
            {
                //水印尺寸
                //是否强制约束大小，以及约束的宽高值
                cbIsWH.Checked = Business.Do<ISystemPara>()["Product_IsCompelSize"].Boolean ?? false;
                tbCompelWd.Text = Business.Do<ISystemPara>()["Product_CompelWidth"].String;
                tbCompelHg.Text = Business.Do<ISystemPara>()["Product_CompelHeight"].String;
                //图片默认缩短图宽高
                tbWidth.Text = Business.Do<ISystemPara>()["Product_ThumbnailWidth"].String;
                tbHeight.Text = Business.Do<ISystemPara>()["Product_ThumbnailHeight"].String;
                //二维码的宽高
                this.tbQrWH.Text = Business.Do<ISystemPara>()["Product_QrCode_WidthAndHeight"].String;
                //二维码的模板
                this.tbQrTextTmp.Text = Business.Do<ISystemPara>()["Product_QrCode_Template"].String;
            }
            catch (Exception)
            {
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
                //图片尺寸
                //是否强制约束大小，以及约束的宽高值
                Business.Do<ISystemPara>().Save("Product_IsCompelSize", cbIsWH.Checked.ToString());
                Business.Do<ISystemPara>().Save("Product_CompelWidth", tbCompelWd.Text.Trim());
                Business.Do<ISystemPara>().Save("Product_CompelHeight", tbCompelHg.Text.Trim());
                //图片默认缩短图宽高
                Business.Do<ISystemPara>().Save("Product_ThumbnailWidth", tbWidth.Text.Trim());
                Business.Do<ISystemPara>().Save("Product_ThumbnailHeight", tbHeight.Text.Trim());
                //保存二维码
                Business.Do<ISystemPara>().Save("Product_QrCode_WidthAndHeight", tbQrWH.Text);
                Business.Do<ISystemPara>().Save("Product_QrCode_Template", tbQrTextTmp.Text);
                //刷新全局参数
                Business.Do<ISystemPara>().Refresh();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 保存并批量生成二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                btnEnter_Click(null, null);
                //二维码的宽高
                int wh = Business.Do<ISystemPara>()["Product_QrCode_WidthAndHeight"].Int16 ?? 200;
                //二维码模板内容
                string template = Business.Do<ISystemPara>()["Product_QrCode_Template"].String;
                //开始批量生成
                int sum = 0;
                int size = 20;
                int index = 1;
                Song.Entities.Product[] entities;
                do
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                    entities = Business.Do<IContents>().ProductPager(org.Org_ID, null, "", null, null, null, null, "", size, index, out sum);
                    foreach (Song.Entities.Product entity in entities)
                    {
                        createQrCode(entity, _uppath, template, wh);
                    }
                } while (size * index++ < sum);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 生成产品的二维码
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private string createQrCode(Song.Entities.Product pd, string pathType,string template,int wh)
        {
            try
            {
                //二维码图片名称
                string img = "";
                if (pd != null && pd.Pd_QrCode != null && pd.Pd_QrCode != "")
                {
                    img = pd.Pd_QrCode;
                }
                else
                {
                    img = WeiSha.Common.Request.UniqueID() + ".png";
                    pd.Pd_QrCode = img;
                    Business.Do<IContents>().ProductSave(pd);
                }
                //创建二维码
                Song.Extend.QrCode.Creat4Entity(pd, template, Upload.Get[pathType].Physics + img, wh);
                return img;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
