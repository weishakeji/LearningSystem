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
using WeiSha.Common.Images;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Content.Picture
{
    public partial class PictureInfo_Edit : Extend.CustomPage
    {
        //图片Id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
        //栏目类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlColumnBind();
                fill();
            }
        }
        /// <summary>
        /// 栏目下拉绑定
        /// </summary>
        private void ddlColumnBind()
        {
            try
            {
                //栏目分类
                //所属机构的所有课程
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.Columns[] nc = Business.Do<IColumns>().ColumnCount(org.Org_ID, "product", true, -1);
                this.ddlColumn.DataSource = nc;
                this.ddlColumn.DataTextField = "Col_Name";
                this.ddlColumn.DataValueField = "Col_Id";
                this.ddlColumn.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                Song.Entities.Picture mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().PictureSingle(id);
                    //是否使用与显示
                    cbIsRec.Checked = mm.Pic_IsRec;
                    cbIsShow.Checked = mm.Pic_IsShow;
                    //热点与置顶
                    cbIsHot.Checked = mm.Pic_IsHot;
                    cbIsTop.Checked = mm.Pic_IsTop;
                    //图片
                    this.imgShow.Src = Upload.Get[type].Virtual + mm.Pic_FilePath;
                    //
                    ListItem li = ddlColumn.Items.FindByValue(mm.Col_Id.ToString());
                    if (li != null) li.Selected = true;
                    //上线时间
                    tbPushTime.Text = mm.Pic_PushTime.ToString();
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Picture();
                    fuLoad.Attributes.Add("nullable", "false");
                    ListItem li = ddlColumn.Items.FindByValue(colid.ToString());
                    if (li != null) li.Selected = true;
                    //上线时间
                    tbPushTime.Text = DateTime.Now.ToString();
                }
                tbName.Text = mm.Pic_Name;
                //说明
                this.tbIntro.Text = mm.Pic_Intro;
                //标签
                tbLabel.Text = mm.Pic_Label;
                //发布信息,SEO优化
                tbKeywords.Text = mm.Pic_Keywords;
                tbDescr.Text = mm.Pic_Descr;
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
            Song.Entities.Picture mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IContents>().PictureSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Picture();
                    EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                    mm.Acc_Id = acc.Acc_Id;
                    mm.Acc_Name = acc.Acc_Name;
                }
                mm.Pic_Name = tbName.Text;
                //是否使用与显示
                mm.Pic_IsRec = cbIsRec.Checked;
                mm.Pic_IsShow = cbIsShow.Checked;
                //热点与置顶
                mm.Pic_IsHot = cbIsHot.Checked;
                mm.Pic_IsTop = cbIsTop.Checked;
                //说明
                mm.Pic_Intro = this.tbIntro.Text;
                //发布信息,SEO优化
                mm.Pic_Keywords = tbKeywords.Text.Trim();
                mm.Pic_Descr = tbDescr.Text.Trim();
                tbPushTime.Text = DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                //图片
                UpFile(mm);
                //确定操作
                if (id == 0)
                {
                    mm.Pic_CrtTime = DateTime.Now;
                    Business.Do<IContents>().PictureAdd(mm);
                }
                else
                {
                    Business.Do<IContents>().PictureSave(mm);
                }

                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        /// <summary>
        /// 图片文件上传
        /// </summary>
        /// <param name="mm"></param>
        protected void UpFile(Song.Entities.Picture mm)
        {
            try
            {
                if (fuLoad.PostedFile.FileName == "") return;
                //新增图片对象
                fuLoad.UpPath = type;
                fuLoad.IsMakeSmall = true;
                fuLoad.IsConvertJpg = true;
                //缩略图宽高
                int tw = Business.Do<ISystemPara>()["Picture_ThumbnailWidth"].Int32 ?? 100;
                int th = Business.Do<ISystemPara>()["Picture_ThumbnailHeight"].Int32 ?? 100;
                fuLoad.SmallHeight = th;
                fuLoad.SmallWidth = tw;
                fuLoad.SaveAndDeleteOld(mm.Pic_FilePath);
                mm.Pic_FilePath = fuLoad.File.Server.FileName;
                mm.Pic_FilePathSmall = fuLoad.File.Server.SmallFileName;
                mm.Pic_Size = (int)fuLoad.File.Server.Size;
                mm.Pic_Width = fuLoad.File.Server.Width;
                mm.Pic_Height = fuLoad.File.Server.Height;
                //是否强制尺寸
                bool isCompel = Business.Do<ISystemPara>()["Picture_IsCompelSize"].Boolean ?? false;
                int cw = Business.Do<ISystemPara>()["Picture_CompelWidth"].Int32 ?? 0;
                int ch = Business.Do<ISystemPara>()["Picture_CompelHeight"].Int32 ?? 0;
                if (isCompel)
                {
                    FileTo.Zoom(fuLoad.File.Server.FileFullName, cw, ch);
                }
                //水印图片处理
                bool isAddWater = Business.Do<ISystemPara>()["Picture_Watermark_IsAdd"].Boolean ?? false;
                if (isAddWater)
                {
                    string imgFile = Business.Do<ISystemPara>()["Picture_Watermark"].String;
                    imgFile = Upload.Get[type].Physics + imgFile;
                    int opacity = Business.Do<ISystemPara>()["Picture_Watermark_Opacity"].Int16 ?? 60;
                    string local = Business.Do<ISystemPara>()["Picture_Watermark_Local"].String;
                    FileTo.OverlayImage(fuLoad.File.Server.FileFullName, imgFile, local, opacity);
                }
                //
                imgShow.Src = fuLoad.File.Server.VirtualPath;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
