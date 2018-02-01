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

namespace Song.Site.Manage.Content
{
    public partial class VideoSetup : Extend.CustomPage
    {
        private string _uppath = "Video";
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
                //图片最大置顶数
                tbMaxTop.Text = Business.Do<ISystemPara>()["Video_MaxTop"].String;
                //图片最大推荐数
                tbMaxRec.Text = Business.Do<ISystemPara>()["Video_MaxRec"].String;
                //
                //水印尺寸
                //是否强制约束大小，以及约束的宽高值
                cbIsWH.Checked = Business.Do<ISystemPara>()["Video_IsCompelSize"].Boolean ?? false;
                tbCompelWd.Text = Business.Do<ISystemPara>()["Video_CompelWidth"].String;
                tbCompelHg.Text = Business.Do<ISystemPara>()["Video_CompelHeight"].String;
                //图片默认缩短图宽高
                tbWidth.Text = Business.Do<ISystemPara>()["Video_ThumbnailWidth"].String;
                tbHeight.Text = Business.Do<ISystemPara>()["Video_ThumbnailHeight"].String;
                //
                //
                //水印图片
                string imgFile = Business.Do<ISystemPara>()["Video_Watermark"].String;
                imgShow.Src = Upload.Get[_uppath].Virtual + imgFile;
                //水印图片透明度
                tbOpacity.Text = Business.Do<ISystemPara>()["Video_Watermark_Opacity"].String;
                //水印图片位置
                tbLocal.Text = Business.Do<ISystemPara>()["Video_Watermark_Local"].String;
                //是否添加水印
                cbIsAddWater.Checked = Business.Do<ISystemPara>()["Video_Watermark_IsAdd"].Boolean ?? false;
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

                //图片最大置顶数
                Business.Do<ISystemPara>().Save("Video_MaxTop", tbMaxTop.Text.Trim());
                //图片最大推荐数
                Business.Do<ISystemPara>().Save("Video_MaxRec", tbMaxRec.Text.Trim());
                //
                //图片尺寸
                //是否强制约束大小，以及约束的宽高值
                Business.Do<ISystemPara>().Save("Video_IsCompelSize", cbIsWH.Checked.ToString());
                Business.Do<ISystemPara>().Save("Video_CompelWidth", tbCompelWd.Text.Trim());
                Business.Do<ISystemPara>().Save("Video_CompelHeight", tbCompelHg.Text.Trim());
                //图片默认缩短图宽高
                Business.Do<ISystemPara>().Save("Video_ThumbnailWidth", tbWidth.Text.Trim());
                Business.Do<ISystemPara>().Save("Video_ThumbnailHeight", tbHeight.Text.Trim());
                //水印图片透明度
                Business.Do<ISystemPara>().Save("Video_Watermark_Opacity", tbOpacity.Text.Trim());
                Business.Do<ISystemPara>().Save("Video_Watermark_Local", tbLocal.Text.Trim());
                //是否添加水印
                Business.Do<ISystemPara>().Save("Video_Watermark_IsAdd", cbIsAddWater.Checked.ToString());
                //水印图片
                if (fuLoad.PostedFile.FileName != "")
                {
                    try
                    {
                        fuLoad.UpPath = _uppath;
                        fuLoad.NewName = "Watermark";
                        fuLoad.IsMakeSmall = false;
                        fuLoad.SaveAndDeleteOld(Business.Do<ISystemPara>()["Video_Watermark"].String);
                        Business.Do<ISystemPara>().Save("Video_Watermark", fuLoad.File.Server.FileName);
                        //
                        imgShow.Src = fuLoad.File.Server.VirtualPath;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
                //刷新全局参数
                Business.Do<ISystemPara>().Refresh();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
