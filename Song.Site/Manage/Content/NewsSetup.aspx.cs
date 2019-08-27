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
    public partial class NewsSetup : Extend.CustomPage
    {
        private string _uppath = "News";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        private void fill()
        {
            //资讯最大置顶数
            tbMaxTop.Text = Business.Do<ISystemPara>()["NewsMaxTop"].String;
            //资讯最大推荐数
            tbMaxRec.Text = Business.Do<ISystemPara>()["NewsMaxRec"].String; 
            //是否需要审核
            cbIsVerify.Checked = Business.Do<ISystemPara>()["NewsIsVerify"].Boolean ?? true; 
            //修改后，是否需要重新审核
            this.cbIsReVeri.Checked = Business.Do<ISystemPara>()["NewsIsReVeri"].Boolean ?? false; 
            //资讯的常用来源
            tbSource.Text = Business.Do<ISystemPara>()["NewsSourceItem"].String; 

            //文章二维码
            tbArtQrWH.Text = Business.Do<ISystemPara>()["NewsArt_QrCode_WidthAndHeight"].String;
            tbArtQrTextTmp.Text = Business.Do<ISystemPara>()["NewsArt_QrCode_Template"].String; 
            //专题二维码
            tbSpecQrWH.Text = Business.Do<ISystemPara>()["NewsSpec_QrCode_WidthAndHeight"].String;
            tbSpecQrTextTmp.Text = Business.Do<ISystemPara>()["NewsSpec_QrCode_Template"].String;
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
                Business.Do<ISystemPara>().Save("NewsMaxTop", tbMaxTop.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("NewsMaxRec", tbMaxRec.Text.Trim(), false);
                Business.Do<ISystemPara>().Save("NewsIsVerify", cbIsVerify.Checked.ToString(), false);
                Business.Do<ISystemPara>().Save("NewsIsReVeri", cbIsReVeri.Checked.ToString(), false);
                Business.Do<ISystemPara>().Save("NewsSourceItem", tbSource.Text.Trim(), false);
                //刷新全局参数
                Business.Do<ISystemPara>().Refresh();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 保存资讯文章的二维码配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnArtEnter_Click(object sender, EventArgs e)
        {
            try
            {
                //文章二维码
                Business.Do<ISystemPara>().Save("NewsArt_QrCode_WidthAndHeight", tbArtQrWH.Text);
                Business.Do<ISystemPara>().Save("NewsArt_QrCode_Template", tbArtQrTextTmp.Text);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        #region 文章二维码
        /// <summary>
        /// 批量生成资讯文章二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnArtBuild_Click(object sender, EventArgs e)
        {
            btnArtEnter_Click(null, null);
            //二维码的宽高
            int wh = Business.Do<ISystemPara>()["NewsArt_QrCode_WidthAndHeight"].Int16 ?? 200;
            //二维码模板内容
            string template = Business.Do<ISystemPara>()["NewsArt_QrCode_Template"].String;
            ////开始批量生成
            //int sum = 0;
            //int size = 20;
            //int index = 1;
            //Song.Entities.Article[] entities;
            //do
            //{
            //    entities = Business.Do<IContents>().GetArticlePager(null, size, index, out sum);
            //    foreach (Song.Entities.Article entity in entities)
            //    {
            //        createArtQrCode(entity, _uppath, template, wh);
            //    }
            //} while (size * index++ < sum);
        }
        /// <summary>
        /// 生成资讯文章的二维码
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private string createArtQrCode(Song.Entities.Article na, string pathType, string template, int wh)
        {
            //二维码图片名称
            string img = "";
            //if (na != null && na.Art_QrCode != null && na.Art_QrCode != "")
            //{
            //    img = na.Art_QrCode;
            //}
            //else
            //{
            //    img = "NewsArt_" + WeiSha.Common.Request.UniqueId() + ".png";
            //    na.Art_QrCode = img;
            //    Business.Do<IContents>().ArticleSave(na);
            //}
            try
            {
                //创建二维码
                Song.Extend.QrCode.Creat4Entity(na, template, Upload.Get[pathType].Physics + img, wh);
                return img;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return null;
            }
        }
        #endregion

        #region 专题二维码
        /// <summary>
        /// 保存专题的二维码配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSpecEnter_Click(object sender, EventArgs e)
        {
            try
            {
                //专题二维码
                Business.Do<ISystemPara>().Save("NewsSpec_QrCode_WidthAndHeight", tbSpecQrWH.Text);
                Business.Do<ISystemPara>().Save("NewsSpec_QrCode_Template", tbSpecQrTextTmp.Text);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }        
        /// <summary>
        /// 批量生成专题二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSpecBuild_Click(object sender, EventArgs e)
        {
            try
            {
                btnSpecEnter_Click(null, null);
                //二维码的宽高
                int wh = Business.Do<ISystemPara>()["NewsSpec_QrCode_WidthAndHeight"].Int16 ?? 200;
                //二维码模板内容
                string template = Business.Do<ISystemPara>()["NewsSpec_QrCode_Template"].String;
                //所属机构的所有课程
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.Special[] entities = Business.Do<IContents>().SpecialCount(org.Org_ID, null, null, 0);
                foreach (Song.Entities.Special entity in entities)
                {
                    createSpecQrCode(entity, _uppath, template, wh);
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        private string createSpecQrCode(Song.Entities.Special ns, string pathType, string template, int wh)
        {
            try
            {
                //二维码图片名称
                string img = "";
                if (ns != null && ns.Sp_QrCode != null && ns.Sp_QrCode != "")
                {
                    img = ns.Sp_QrCode;
                }
                else
                {
                    img = "NewsSpec_" + WeiSha.Common.Request.UniqueID() + ".png";
                    ns.Sp_QrCode = img;
                    Business.Do<IContents>().SpecialSave(ns);
                }
                //创建二维码
                Song.Extend.QrCode.Creat4Entity(ns, template, Upload.Get[pathType].Physics + img, wh);
                return img;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return null;
            }
        }
        #endregion
    }
}
