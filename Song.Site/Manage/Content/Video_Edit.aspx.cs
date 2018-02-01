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
    public partial class Video_Edit : Extend.CustomPage
    {
        //图片Id
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //相册的id，用于图片移动到不同相册
        private int paid = WeiSha.Common.Request.QueryString["paid"].Int32 ?? 0;
        //栏目类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        public string flv = "null";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlAlbumBind();
                fill();
                PictureBindData(null, null);
            }
        }
        private void loyout()
        {

        }
        /// <summary>
        /// 栏目下拉绑定
        /// </summary>
        private void ddlAlbumBind()
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
                //lbDir1.Text = lbDir2.Text = Upload.Get[type].Virtual;
                Song.Entities.Video mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().VideoSingle(id);
                    //唯一标识
                    ViewState["UID"] = mm.Vi_Uid == null || mm.Vi_Uid.Length < 1 ? WeiSha.Common.Request.UniqueID() : mm.Vi_Uid;
                    //是否使用与显示
                    cbIsRec.Checked = mm.Vi_IsRec;
                    cbIsShow.Checked = mm.Vi_IsShow;
                    //热点与置顶
                    cbIsHot.Checked = mm.Vi_IsHot;
                    cbIsTop.Checked = mm.Vi_IsTop;
                    //栏目分类
                    ListItem li = ddlColumn.Items.FindByValue(mm.Col_Id.ToString());
                    if (li != null) li.Selected = true;
                    //上线时间
                    tbPushTime.Text = mm.Vi_PushTime.ToString();
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Video();
                    ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                }
                if (mm.Vi_FilePath != string.Empty && mm.Vi_FilePath != "")
                {
                    tbVideo.Text = mm.Vi_VideoFile;
                    //flv = Upload.Get[type].Virtual + mm.Vi_VideoFile;
                    flv = mm.Vi_VideoFile;
                    //如果视频不存在
                    if (!System.IO.File.Exists(Server.MapPath(flv)))
                    {
                        this.videoBox.Visible = false;
                        this.videoError.Visible = true;
                    }
                    else
                    {
                        this.videoBox.Visible = true;
                        this.videoError.Visible = false;
                    }
                }

                tbName.Text = mm.Vi_Name;
                //标签
                tbLabel.Text = mm.Vi_Label;
                //内容
                tbDetails.Text = mm.Vi_Details;
                //发布设置
                tbKeywords.Text = mm.Vi_Keywords;
                tbDescr.Text = mm.Vi_Descr;
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
            Song.Entities.Video mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IContents>().VideoSingle(id);

                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Video();
                    EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                    mm.Acc_Id = acc.Acc_Id;
                    mm.Acc_Name = acc.Acc_Name;
                }
                mm.Vi_Name = tbName.Text.Trim();
                mm.Vi_FilePath = tbVideo.Text.Trim();
                mm.Col_Id = Convert.ToInt32(ddlColumn.SelectedValue);
                mm.Col_Name = ddlColumn.SelectedItem.Text;
                mm.Vi_Label = tbLabel.Text.Trim();
                //是否使用与显示
                mm.Vi_IsRec = cbIsRec.Checked;
                mm.Vi_IsShow = cbIsShow.Checked;
                //热点与置顶
                mm.Vi_IsHot = cbIsHot.Checked;
                mm.Vi_IsTop = cbIsTop.Checked;
                //详细信息
                mm.Vi_Details = tbDetails.Text;
                //发布设置
                mm.Vi_Keywords = tbKeywords.Text;
                mm.Vi_Descr = tbDescr.Text;
                mm.Vi_PushTime = tbPushTime.Text.Trim() != "" ? Convert.ToDateTime(tbPushTime.Text) : DateTime.Now;
                //其它
                mm.Vi_Uid = getUID();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }        
            //确定操作
            try
            {
                if (id == 0)
                {
                    mm.Vi_CrtTime = DateTime.Now;
                    Business.Do<IContents>().VideoAdd(mm);
                }
                else
                {
                    Business.Do<IContents>().VideoSave(mm);
                }
                flv = mm.Vi_FilePath;
                //如果视频不存在
                if (!System.IO.File.Exists(Server.MapPath(flv)))
                {
                    this.videoBox.Visible = false;
                    this.videoError.Visible = true;
                }
                else
                {
                    this.videoBox.Visible = true;
                    this.videoError.Visible = false;
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
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
                int tw = Business.Do<ISystemPara>()["VideoThumbnailWidth"].Int32 ?? 100;
                int th = Business.Do<ISystemPara>()["VideoThumbnailHeight"].Int32 ?? 100;
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
                bool isCompel = Business.Do<ISystemPara>()["VideoIsCompelSize"].Boolean ?? false;
                int cw = Business.Do<ISystemPara>()["VideoCompelWidth"].Int32 ?? 0;
                int ch = Business.Do<ISystemPara>()["VideoCompelHeight"].Int32 ?? 0;
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