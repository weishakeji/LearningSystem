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
    public partial class Download_Edit : Extend.CustomPage
    {
        private int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
        //当前信息的id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //栏目类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        //文件上传的物理路径
        //private string type = "Download";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlCoulmnBind();
                fill();
                PictureBindData(null, null);
            }
        }
        /// <summary>
        /// 栏目下拉绑定
        /// </summary>
        private void ddlCoulmnBind()
        {
            try
            {
                //所属机构的所有课程
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.Columns[] nc = Business.Do<IColumns>().ColumnCount(org.Org_ID, type, true, -1);
                this.ddlColumn.DataSource = nc;
                this.ddlColumn.DataTextField = "Col_Name";
                this.ddlColumn.DataValueField = "Col_Id";
                this.ddlColumn.DataBind();
                //类型
                Song.Entities.DownloadType[] dt = Business.Do<IContents>().DownloadTypeCount(org.Org_ID, true, 0);
                this.ddlSort.DataSource = dt;
                this.ddlSort.DataTextField = "Dty_Name";
                this.ddlSort.DataValueField = "Dty_Id";
                this.ddlSort.DataBind();
                //适用环境
                Song.Entities.DownloadOS[] os = Business.Do<IContents>().DownloadOSCount(org.Org_ID, true, 0);
                this.cblOS.DataSource = os;
                this.cblOS.DataTextField = "Dos_Name";
                this.cblOS.DataValueField = "Dos_Id";
                this.cblOS.DataBind();
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
                Song.Entities.Download mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().DownloadSingle(id);
                    //唯一标识
                    ViewState["UID"] = mm.Dl_Uid == null || mm.Dl_Uid.Length < 1 ? WeiSha.Common.Request.UniqueID() : mm.Dl_Uid;
                    //是否使用与显示，以及热点与置顶
                    cbIsRec.Checked = mm.Dl_IsRec;
                    cbIsShow.Checked = mm.Dl_IsShow;
                    cbIsHot.Checked = mm.Dl_IsHot;
                    cbIsTop.Checked = mm.Dl_IsTop;
                    //更新时间
                    tbUpdateTime.Text = ((DateTime)mm.Dl_UpdateTime).ToString("yyyy-MM-dd");
                    //资源文件
                    hlSource.Text = mm.Dl_FilePath;
                    hlSource.NavigateUrl = Upload.Get[type].Virtual + mm.Dl_FilePath;
                    //栏目
                    ListItem li = ddlSort.Items.FindByValue(mm.Col_Id.ToString());
                    if (li != null) li.Selected = true;
                    //类型
                    ListItem liTy = ddlColumn.Items.FindByValue(mm.Dty_Id.ToString());
                    if (liTy != null) liTy.Selected = true;
                    //适用系统
                    if (mm.Dl_OS != null && mm.Dl_OS.Length > 0)
                    {
                        foreach (string o in mm.Dl_OS.Split('/'))
                        {
                            ListItem lii = cblOS.Items.FindByText(o);
                            if (lii != null) lii.Selected = true;
                        }
                    }
                    //发布时间
                    tbPushTime.Text = mm.Dl_PushTime.ToString();
                    //图片
                    //this.imgShow.Src = Upload.Get[type].Virtual + mm.Dl_Logo;
                    //二维码
                    this.imgQrCode.ImageUrl = Upload.Get[type].Virtual + mm.Dl_QrCode;
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Download();
                    ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                    //更新时间
                    tbUpdateTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    //上传控件必填属性
                    fuSource.Attributes.Add("nullable", "false");
                    //分类
                    ListItem li = ddlColumn.Items.FindByValue(colid.ToString());
                    if (li != null) li.Selected = true;
                }
                tbName.Text = mm.Dl_Name;
                //版本号
                tbVersion.Text = mm.Dl_Version;
                //介绍
                this.tbIntro.Text = mm.Dl_Intro;
                tbDetails.Text = mm.Dl_Details;
                //所有者
                tbAuthor.Text = mm.Dl_Author;
                //标签
                tbLabel.Text = mm.Dl_Label;

                //发布设置相关
                tbKeywords.Text = mm.Dl_Keywords;
                tbDescr.Text = mm.Dl_Descr;
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
                Song.Entities.Download mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().DownloadSingle(id);
                    //唯一标识
                    ViewState["UID"] = mm.Dl_Uid;
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Download();
                    ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                }
                mm.Dl_Name = tbName.Text;
                //是否使用与显示,热点与置顶
                mm.Dl_IsRec = cbIsRec.Checked;
                mm.Dl_IsShow = cbIsShow.Checked;
                mm.Dl_IsHot = cbIsHot.Checked;
                mm.Dl_IsTop = cbIsTop.Checked;
                //栏目
                mm.Col_Id = Convert.ToInt32(ddlColumn.SelectedItem.Value);
                mm.Col_Name = ddlColumn.SelectedItem.Text;
                //类型
                mm.Dty_Id = Convert.ToInt32(ddlSort.SelectedItem.Value);
                mm.Dty_Type = ddlSort.SelectedItem.Text;
                //版本号
                mm.Dl_Version = tbVersion.Text.Trim();
                //更新时间
                mm.Dl_UpdateTime = Convert.ToDateTime(tbUpdateTime.Text.Trim());
                //介绍
                mm.Dl_Intro = this.tbIntro.Text.Trim();
                //所有者
                mm.Dl_Author = tbAuthor.Text.Trim();
                //标签
                mm.Dl_Label = tbLabel.Text.Trim();
                //适用系统
                string ostr = "";
                foreach (ListItem li in cblOS.Items)
                {
                    if (li.Selected) ostr += li.Text + "/";
                }
                if (ostr != "") ostr = ostr.Substring(0, ostr.Length - 1);
                mm.Dl_OS = ostr;
                //资源
                if (this.fuSource.PostedFile.FileName != "")
                {
                    try
                    {
                        fuSource.UpPath = type;
                        fuSource.IsConvertJpg = false;
                        fuSource.SaveAndDeleteOld(mm.Dl_FilePath);

                        mm.Dl_FilePath = fuSource.File.Server.FileName;
                        //imgShow.Src = fuSource.File.Server.VirtualPath; ;
                        mm.Dl_Size = (int)fuSource.File.Server.Size;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
                //详细信息
                mm.Dl_Details = tbDetails.Text;
                //编辑人的信息
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                mm.Acc_Id = acc.Acc_Id;
                mm.Acc_Name = acc.Acc_Name;
                //发布设置
                mm.Dl_Keywords = tbKeywords.Text;
                mm.Dl_Descr = tbDescr.Text;
                mm.Dl_PushTime = tbPushTime.Text.Trim() != "" ? Convert.ToDateTime(tbPushTime.Text) : DateTime.Now;
                //唯一值
                mm.Dl_Uid = this.getUID();
                //确定操作
                if (id == 0)
                {
                    mm.Dl_CrtTime = DateTime.Now;
                    Business.Do<IContents>().DownloadAdd(mm);
                }
                else
                {
                    Business.Do<IContents>().DownloadSave(mm);
                }

                //生成二维码
                mm.Dl_QrCode = createQrCode(mm);
                //二维码
                this.imgQrCode.ImageUrl = Upload.Get[type].Virtual + mm.Dl_QrCode;
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 生成下载信息的二维码
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private string createQrCode(Song.Entities.Download di)
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
                }
                //二维码的宽高
                int wh = Business.Do<ISystemPara>()["Download_QrCode_WidthAndHeight"].Int16 ?? 200;
                //二维码模板内容
                string template = Business.Do<ISystemPara>()["Download_QrCode_Template"].String;
                //创建二维码
                Song.Extend.QrCode.Creat4Entity(di, template, Upload.Get[type].Physics + img, wh);
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
                int tw = Business.Do<ISystemPara>()["Download_ThumbnailWidth"].Int32 ?? 100;
                int th = Business.Do<ISystemPara>()["Download_ThumbnailHeight"].Int32 ?? 100;
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
                bool isCompel = Business.Do<ISystemPara>()["Download_IsCompelSize"].Boolean ?? false;
                int cw = Business.Do<ISystemPara>()["Download_CompelWidth"].Int32 ?? 0;
                int ch = Business.Do<ISystemPara>()["Download_CompelHeight"].Int32 ?? 0;
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
