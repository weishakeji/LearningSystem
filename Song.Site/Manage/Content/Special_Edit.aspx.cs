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
using WeiSha.Common.Images;

namespace Song.Site.Manage.Content
{
    public partial class Special_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private string type = "Special";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
                PictureBindData(null,null);
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                Song.Entities.Special mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().SpecialSingle(id);
                    //唯一标识
                    ViewState["UID"] = mm.Sp_Uid == null || mm.Sp_Uid.Length < 1 ? WeiSha.Common.Request.UniqueID() : mm.Sp_Uid;
                    //是否显示，是否显示，是否外部连接
                    cbIsUse.Checked = mm.Sp_IsUse;
                    cbIsShow.Checked = mm.Sp_IsShow;
                    cbIsOut.Checked = mm.Sp_IsOut;
                    //二维码
                    this.imgQrCode.ImageUrl = Upload.Get[type].Virtual + mm.Sp_QrCode;
                    //上线时间
                    tbPushTime.Text = mm.Sp_PushTime.ToString();
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Special();
                    ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                    //上线时间
                    tbPushTime.Text = DateTime.Now.ToString();
                }
                //专题名称
                tbName.Text = mm.Sp_Name;
                //外部链接地址
                tbOutUrl.Text = mm.Sp_OutUrl;
                //说明，简介，标签
                this.tbTootip.Text = mm.Sp_Tootip;
                tbIntro.Text = mm.Sp_Intro;
                tbLabel.Text = mm.Sp_Label;
                //详情
                tbDetails.Text = mm.Sp_Details;
                //发布设置：关键字，destion,发布时间
                tbKeywords.Text = mm.Sp_Keywords;
                tbDescr.Text = mm.Sp_Descr;
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
            Song.Entities.Special mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IContents>().SpecialSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Special();
                }
                //专题名称
                mm.Sp_Name = tbName.Text.Trim();
                //是否显示，是否显示，是否外部连接
                mm.Sp_IsUse = cbIsUse.Checked;
                mm.Sp_IsShow = cbIsShow.Checked;
                mm.Sp_IsOut = cbIsOut.Checked;
                //外部链接地址
                mm.Sp_OutUrl = tbOutUrl.Text;
                //说明，简介，标签
                mm.Sp_Tootip = this.tbTootip.Text.Trim();
                mm.Sp_Intro = tbIntro.Text.Trim();
                mm.Sp_Label = tbLabel.Text.Trim();
                //详情
                tbDetails.Text = mm.Sp_Details;
                //发布设置：关键字，destion,发布时间
                mm.Sp_Keywords = tbKeywords.Text.Trim();
                mm.Sp_Descr = tbDescr.Text.Trim();
                //Uid
                mm.Sp_Uid = getUID();
                //生成二维码
                mm.Sp_QrCode = createQrCode(mm);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                if (id != 0)
                {
                    Business.Do<IContents>().SpecialSave(mm);
                }
                else
                {
                    //如果是新增
                    Business.Do<IContents>().SpecialAdd(mm);
                }
                //二维码
                this.imgQrCode.ImageUrl = Upload.Get[type].Virtual + mm.Sp_QrCode;
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        /// <summary>
        /// 生成新闻专题的二维码
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private string createQrCode(Song.Entities.Special ns)
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
                    img = "Spec_" + WeiSha.Common.Request.UniqueID() + ".png";
                }
                //二维码的宽高
                int wh = Business.Do<ISystemPara>()["NewsSpec_QrCode_WidthAndHeight"].Int16 ?? 200;
                //二维码模板内容
                string template = Business.Do<ISystemPara>()["NewsSpec_QrCode_Template"].String;
                //创建二维码
                Song.Extend.QrCode.Creat4Entity(ns, template, Upload.Get[type].Physics + img, wh);
                return img;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
                return null;
            }
        }
        #region 图片管理
        /// <summary>
        /// 上传图片文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuLoad.PostedFile.FileName == "") return;
                //新增图片对象
                Song.Entities.Picture mm = new Song.Entities.Picture();
                fuLoad.UpPath = type;
                fuLoad.IsMakeSmall = true;
                fuLoad.IsConvertJpg = true;
                //缩略图宽高
                int tw = Business.Do<ISystemPara>()["Special_ThumbnailWidth"].Int32 ?? 150;
                int th = Business.Do<ISystemPara>()["Special_ThumbnailHeight"].Int32 ?? 150;
                fuLoad.SmallHeight = th;
                fuLoad.SmallWidth = tw;
                fuLoad.SaveAndDeleteOld(mm.Pic_FilePath);
                //
                mm.Pic_FilePath = fuLoad.File.Server.FileName;
                mm.Pic_FilePathSmall = fuLoad.File.Server.SmallFileName;
                mm.Pic_Size = (int)fuLoad.File.Server.Size;
                mm.Pic_Width = fuLoad.File.Server.Width;
                mm.Pic_Height = fuLoad.File.Server.Height;
                //是否强制尺寸
                bool isCompel = Business.Do<ISystemPara>()["Special_IsCompelSize"].Boolean ?? false;
                int cw = Business.Do<ISystemPara>()["Special_CompelWidth"].Int32 ?? 0;
                int ch = Business.Do<ISystemPara>()["Special_CompelHeight"].Int32 ?? 0;
                if (isCompel)
                {
                    FileTo.Zoom(fuLoad.File.Server.FileFullName, cw, ch);
                }
                //
                mm.Pic_Uid = getUID();
                mm.Pic_Type = type;
                //
                Business.Do<IContents>().PictureAdd(mm);
                PictureBindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 列表数据绑定
        /// </summary>
        protected void PictureBindData(object sender, EventArgs e)
        {
            try
            {
                if (id < 1) return;
                //资源的路径
                string resPath = Upload.Get[type].Virtual;
                Song.Entities.Picture[] eas = null;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                eas = Business.Do<IContents>().PictureCount(org.Org_ID, getUID(), false, null, "", 0);
                foreach (Song.Entities.Picture entity in eas)
                {
                    entity.Pic_FilePath = resPath + entity.Pic_FilePath;
                    entity.Pic_FilePathSmall = resPath + entity.Pic_FilePathSmall;
                }
                rptPict.DataSource = eas;
                rptPict.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int id = Convert.ToInt32(ub.CommandArgument);
                //
                Song.Entities.Picture entity = Business.Do<IContents>().PictureSingle(id);
                entity.Pic_IsShow = !entity.Pic_IsShow;
                Business.Do<IContents>().PictureSave(entity);
                PictureBindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDel_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton ub = (LinkButton)sender;
                int id = Convert.ToInt32(ub.CommandArgument);
                Business.Do<IContents>().PictureDelete(id);
                PictureBindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 设置当前图片为封面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbCover_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton ub = (LinkButton)sender;
                int pid = Convert.ToInt32(ub.CommandArgument);
                Business.Do<IContents>().PictureSetCover(getUID(), pid);
                PictureBindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        #endregion

    }
}
