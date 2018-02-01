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
using System.Text.RegularExpressions;
using System.Reflection;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using WeiSha.Common.Images;

namespace Song.Site.Manage.Content
{
    public partial class Product_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        private int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
        //缩略图的宽高
        public int thWidth = Business.Do<ISystemPara>()["Product_ThumbnailWidth"].Int32 ?? 100;
        public int thHeight = Business.Do<ISystemPara>()["Product_ThumbnailHeight"].Int32 ?? 120;
        //栏目类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            //type = Upload.Get["Product"].Physics;
            if (!this.IsPostBack)
            {
                ddlColumnBind();
                fill();
                PictureBindData(null, null);
            }
        }
        #region 基础数据

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
                //厂家
                this.ddlFactory.DataSource = Business.Do<IProduct>().FactoryAll(true);
                this.ddlFactory.DataTextField = "Pfact_Name";
                this.ddlFactory.DataValueField = "Pfact_Id";
                this.ddlFactory.DataBind();
                this.ddlFactory.Items.Insert(0, new ListItem("", "-1"));
                //产地
                this.ddlOrigin.DataSource = Business.Do<IProduct>().OriginAll(true);
                this.ddlOrigin.DataTextField = "Pori_Name";
                this.ddlOrigin.DataValueField = "Pori_Id";
                this.ddlOrigin.DataBind();
                this.ddlOrigin.Items.Insert(0, new ListItem("", "-1"));
                //材质
                this.ddlMaterial.DataSource = Business.Do<IProduct>().MaterialAll(true);
                this.ddlMaterial.DataTextField = "Pmat_Name";
                this.ddlMaterial.DataValueField = "Pmat_Id";
                this.ddlMaterial.DataBind();
                this.ddlMaterial.Items.Insert(0, new ListItem("", "-1"));
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
            Song.Entities.Product mm;
            if (id != 0)
            {
                mm = Business.Do<IContents>().ProductSingle(id);
                //是否使用,是否新产品，是否推荐
                cbIsUse.Checked = mm.Pd_IsUse;
                cbIsNew.Checked = mm.Pd_IsNew;
                cbIsRec.Checked = mm.Pd_IsRec;
                //上市时间
                tbSaleTime.Text = ((DateTime)mm.Pd_SaleTime).ToString("yyyy-MM-dd");
                //编辑
                tbAccName.Text = mm.Acc_Name;
                //产品分类
                ListItem li = ddlColumn.Items.FindByValue(mm.Col_Id.ToString());
                if (li != null) li.Selected = true;
                //厂家，产地，材质
                ListItem liFact = ddlFactory.Items.FindByValue(mm.Pfact_Id.ToString());
                if (liFact != null) liFact.Selected = true;
                ListItem liOrg = ddlOrigin.Items.FindByValue(mm.Pori_Id.ToString());
                if (liOrg != null) liOrg.Selected = true;
                ListItem liMat = ddlMaterial.Items.FindByValue(mm.Pmat_Id.ToString());
                if (liMat != null) liMat.Selected = true;
                //上线时间
                tbPushTime.Text = mm.Pd_PushTime.ToString();
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Product();
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                tbAccName.Text = acc.Acc_Name;
                //上市时间
                tbSaleTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //分类
                ListItem li = ddlColumn.Items.FindByValue(colid.ToString());
                if (li != null) li.Selected = true;
                //上线时间
                tbPushTime.Text = DateTime.Now.ToString();
            }
            //全局UID
            ViewState["UID"] = string.IsNullOrWhiteSpace(mm.Pd_Uid) ? getUID() : mm.Pd_Uid;
            //产品名称
            tbName.Text = mm.Pd_Name;
            //型号
            tbModel.Text = mm.Pd_Model;
            //编号
            tbCode.Text = mm.Pd_Code;
            //价格
            tbPrise.Text = mm.Pd_Prise.ToString();
            //重量
            tbWeight.Text = mm.Pd_Weight.ToString();
            //库存
            tbStocks.Text = mm.Pd_Stocks.ToString();
            ListItem li1 = ddlUnit.Items.FindByText(mm.Pd_Unit);
            if (li1 != null) li1.Selected = true;
            //简介
            tbIntro.Text = mm.Pd_Intro;
            //标签
            tbLabel.Text = mm.Pd_Label;
            //内容
            tbDetails.Text = mm.Pd_Details;

            //发布信息,SEO优化
            tbKeywords.Text = mm.Pd_KeyWords;
            tbDescr.Text = mm.Pd_Descr;

        }
        #endregion

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
            LinkButton ub = (LinkButton)sender;
            int pid = Convert.ToInt32(ub.CommandArgument);
            Business.Do<IContents>().PictureSetCover(getUID(), pid);
            PictureBindData(null, null);
        }
        #endregion
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                Song.Entities.Product mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().ProductSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Product();
                    mm.Pd_CrtTime = DateTime.Now;
                }
                //产品名称
                mm.Pd_Name = tbName.Text;
                //是否使用,是否新产品，是否推荐
                mm.Pd_IsUse = cbIsUse.Checked;
                mm.Pd_IsNew = cbIsNew.Checked;
                mm.Pd_IsRec = cbIsRec.Checked;
                //产品分类
                mm.Col_Id = Convert.ToInt32(ddlColumn.SelectedValue);
                //型号
                mm.Pd_Model = tbModel.Text.Trim();
                //编号
                mm.Pd_Code = tbCode.Text.Trim();
                //上市时间
                mm.Pd_SaleTime = Convert.ToDateTime(tbSaleTime.Text);
                //厂家，产地，材质
                mm.Pfact_Id = Convert.ToInt32(ddlFactory.SelectedValue);
                mm.Pori_Id = Convert.ToInt32(ddlOrigin.SelectedValue);
                mm.Pmat_Id = Convert.ToInt32(ddlMaterial.SelectedValue);
                //价格
                mm.Pd_Prise = Convert.ToSingle(tbPrise.Text.Trim() == "" ? "0" : tbPrise.Text);
                //重量
                mm.Pd_Weight = Convert.ToInt32(tbWeight.Text.Trim());
                //库存
                mm.Pd_Stocks = Convert.ToInt32(tbStocks.Text.Trim());
                mm.Pd_Unit = ddlUnit.SelectedItem.Text;
                //简介
                mm.Pd_Intro = tbIntro.Text.Trim();
                //内容
                mm.Pd_Details = tbDetails.Text;
                //标签
                mm.Pd_Label = tbLabel.Text.Trim();
                //编辑
                mm.Acc_Name = tbAccName.Text.Trim();
                //发布信息,SEO优化
                mm.Pd_KeyWords = tbKeywords.Text;
                mm.Pd_Descr = tbDescr.Text;
                mm.Pd_PushTime = tbPushTime.Text.Trim() != "" ? Convert.ToDateTime(tbPushTime.Text) : DateTime.Now;
                //唯一值
                mm.Pd_Uid = this.getUID();
                if (mm.Acc_Name != "")
                {
                    EmpAccount acc = Business.Do<IEmployee>().GetSingleByName(mm.Acc_Name);
                    if (acc != null)
                    {
                        mm.Acc_Name = acc.Acc_Name;
                        mm.Acc_Id = acc.Acc_Id;
                    }
                }
                //生成二维码
                mm.Pd_QrCode = createQrCode(mm, type);
                if (id != 0)
                {
                    Business.Do<IContents>().ProductSave(mm);
                }
                else
                {
                    //如果是新增
                    id = Business.Do<IContents>().ProductAdd(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 生成产品的二维码
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        private string createQrCode(Song.Entities.Product pd,string pathType)
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
                }
                //二维码的宽高
                int wh = Business.Do<ISystemPara>()["Product_QrCode_WidthAndHeight"].Int16 ?? 200;
                //二维码模板内容
                string template = Business.Do<ISystemPara>()["Product_QrCode_Template"].String;
                //创建二维码
                Song.Extend.QrCode.Creat4Entity(pd, template, Upload.Get[pathType].Physics + img, wh);
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
