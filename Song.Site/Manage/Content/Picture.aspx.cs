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

namespace Song.Site.Manage.Content.Picture
{
    public partial class PictureInfo : Extend.CustomPage
    {
        //栏目分类
        int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
        //栏目类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                bool? istop = !cbIsTop.Checked ? null : (bool?)true;
                bool? ishot = !cbIsHot.Checked ? null : (bool?)true;
                bool? isrec = !cbIsRec.Checked ? null : (bool?)true;
                Song.Entities.Picture[] eas = null;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                eas = Business.Do<IContents>().PicturePager(org.Org_ID, colid, false, null, this.tbSear.Text, ishot, isrec, istop, Pager1.Size, Pager1.Index, out count);
                //资源的路径
                string resPath = Upload.Get[type].Virtual;
                foreach (Song.Entities.Picture entity in eas)
                {
                    entity.Pic_FilePath = resPath + entity.Pic_FilePath;
                    entity.Pic_FilePathSmall = resPath + entity.Pic_FilePathSmall;
                }
                rptPict.DataSource = eas;
                rptPict.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
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
                int tw = Business.Do<ISystemPara>()["Product_ThumbnailWidth"].Int32 ?? 100;
                int th = Business.Do<ISystemPara>()["Product_ThumbnailHeight"].Int32 ?? 100;
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
                bool isCompel = Business.Do<ISystemPara>()["Product_IsCompelSize"].Boolean ?? false;
                int cw = Business.Do<ISystemPara>()["Product_CompelWidth"].Int32 ?? 0;
                int ch = Business.Do<ISystemPara>()["Product_CompelHeight"].Int32 ?? 0;
                if (isCompel)
                {
                    FileTo.Zoom(fuLoad.File.Server.FileFullName, cw, ch);
                }
                //
                mm.Col_Id = colid;
                mm.Pic_Type = type;
                //
                Business.Do<IContents>().PictureAdd(mm);
                BindData(null, null);
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
                BindData(null, null);
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
                BindData(null, null);
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
                Business.Do<IContents>().PictureSetCover(colid, pid);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        #endregion
      
    }
}
