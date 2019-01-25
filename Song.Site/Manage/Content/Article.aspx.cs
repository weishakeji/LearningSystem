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
using System.Collections.Generic;

namespace Song.Site.Manage.Content
{
    public partial class Article : Extend.CustomPage
    {
        private int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
        //当前信息的id
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //栏目类型
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        private string _uppath = "News";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (type == "article")
                {
                    btnClose.Visible = false;
                    trCol.Visible = false;
                    tbDetails.Height = 550;
                }
                else
                {
                    ddlColumnBind();
                }
                cbSpecialBind();
                fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                ltSource.Text = Business.Do<ISystemPara>()["NewsSourceItem"].String;
                Song.Entities.Article mm;
                if (id != 0)
                {
                    mm = Business.Do<IContents>().ArticleSingle(id);
                    cbIsHot.Checked = mm.Art_IsHot;
                    //是否显示
                    cbIsShow.Checked = mm.Art_IsShow;
                    //是否置顶
                    cbIsTop.Checked = mm.Art_IsTop;
                    //是否推荐
                    cbIsRec.Checked = mm.Art_IsRec;
                    //栏目
                    ListItem li = ddlColumn.Items.FindByValue(mm.Col_Id.ToString());
                    if (li != null) li.Selected = true;
                    //唯一标识
                    ViewState["UID"] = mm.Art_Uid;
                    //作者
                    tbAuthor.Text = Extend.LoginState.Admin.CurrentUser.Acc_Name;
                    //专题
                    Song.Entities.Special[] ns = Business.Do<IContents>().Article4Special(id);
                    foreach (Song.Entities.Special n in ns)
                    {
                        ListItem lis = cbSpecial.Items.FindByValue(n.Sp_Id.ToString());
                        if (lis != null) lis.Selected = true;
                    }
                    //上线时间
                    tbPushTime.Text = mm.Art_PushTime < DateTime.Now.AddYears(-100) ? DateTime.Now.ToString() : mm.Art_PushTime.ToString();
                    AccessoryBind();
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Article();
                    ViewState["UID"] = WeiSha.Common.Request.UniqueID();
                    //分类
                    ListItem li = ddlColumn.Items.FindByValue(colid.ToString());
                    if (li != null) li.Selected = true;
                    //上线时间
                    tbPushTime.Text = DateTime.Now.ToString();
                }
                //文章标题
                tbTitle.Text = mm.Art_Title;
                tbTitleAbbr.Text = mm.Art_TitleAbbr;
                tbTitleFull.Text = mm.Art_TitleFull;
                //内容
                tbDetails.Text = mm.Art_Details;
                //简介
                tbIntro.Text = mm.Art_Intro;
                //关键字，descrtion,发布时间
                tbKeywords.Text = mm.Art_Keywords;
                tbDescr.Text = mm.Art_Descr;
                //作者，来源，标签
                tbAuthor.Text = mm.Art_Author;
                tbSource.Text = mm.Art_Source;
                tbLabel.Text = mm.Art_Label;
                //是否是图片新闻，是否是热点新闻
                cbIsImg.Checked = mm.Art_IsImg;
                fuImg.Visible = this.panelImg.Visible = cbIsImg.Checked;
                imgFile.Src = Upload.Get[_uppath].Virtual + mm.Art_Logo;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
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
        /// 专题列表绑定
        /// </summary>
        private void cbSpecialBind()
        {
            try
            {
                //所属机构的所有课程
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.Special[] ns = Business.Do<IContents>().SpecialCount(org.Org_ID, true, true, 0);
                cbSpecial.DataSource = ns;
                cbSpecial.DataTextField = "Sp_Name";
                cbSpecial.DataValueField = "Sp_Id";
                cbSpecial.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        #endregion

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Article mm=null;
            try
            {
                if (id != 0)
                {
                    mm = Business.Do<IContents>().ArticleSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.Article();
                    mm.Art_IsVerify = false;
                }
                //文章标题
                mm.Art_Title = tbTitle.Text.Trim();
                mm.Art_TitleAbbr = tbTitleAbbr.Text.Trim();
                mm.Art_TitleFull = tbTitleFull.Text.Trim();
                //是否是图片新闻，是否是热点新闻
                mm.Art_IsImg = cbIsImg.Checked;
                mm.Art_IsHot = cbIsHot.Checked;
                //是否显示
                mm.Art_IsShow = cbIsShow.Checked;
                //是否置顶
                mm.Art_IsTop = cbIsTop.Checked;
                //是否推荐
                mm.Art_IsRec = cbIsRec.Checked;
                //所在栏目
                if (type == "article")
                {
                    mm.Col_Id = -1;
                }
                else
                {
                    mm.Col_Id = Convert.ToInt32(ddlColumn.SelectedItem.Value);
                }
                //内容
                mm.Art_Details = tbDetails.Text;
                //简介
                mm.Art_Intro = tbIntro.Text.Trim();
                //作者,来源,标签
                mm.Art_Author = tbAuthor.Text.Trim();
                mm.Art_Source = tbSource.Text.Trim();
                mm.Art_Label = tbLabel.Text.Trim();
                //关键字
                mm.Art_Keywords = tbKeywords.Text.Trim();
                mm.Art_Descr = tbDescr.Text.Trim();
                mm.Art_PushTime = Convert.ToDateTime(tbPushTime.Text);
                //员工id与名称
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                mm.Acc_Id = acc.Acc_Id;
                mm.Acc_Name = acc.Acc_Name;
                mm.Art_Uid = this.getUID();
                //上传图片
                if (cbIsImg.Checked && fuImg.PostedFile.FileName != "")
                {
                    try
                    {
                        fuImg.UpPath = _uppath;
                        fuImg.IsMakeSmall = false;
                        fuImg.IsConvertJpg = true;
                        fuImg.SmallWidth = 400;
                        fuImg.SmallHeight = 247;  
                        fuImg.SaveAndDeleteOld(mm.Art_Logo);
                        //截取图片宽高
                        int width = fuImg.File.Server.Width;
                        width = width > 1000 ? 1000 : width;
                        int height = width * 618 / 1000;  //宽高比为1:0.618
                        fuImg.File.Server.ChangeSize(width, height, false);
                        mm.Art_Logo = fuImg.File.Server.FileName;
                        imgFile.Src = Upload.Get[_uppath].Virtual + mm.Art_Logo;
                    }
                    catch (Exception ex)
                    {
                        this.Alert(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
            try
            {
                if (id != 0)
                {
                    Business.Do<IContents>().ArticleSave(mm);
                }
                else
                {
                    //如果是新增
                    id = Business.Do<IContents>().ArticleAdd(mm);
                }
                //所属专题
                foreach (ListItem li in this.cbSpecial.Items)
                {
                    int spid = Convert.ToInt32(li.Value);
                    if (li.Selected == true)
                    {
                        Business.Do<IContents>().SpecialAndArticle(spid, id);
                    }
                    else
                    {
                        Business.Do<IContents>().SpecialAndArticleDel(spid, id);
                    }
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }

        #region 附件操作
        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Song.Entities.Accessory dd = new Accessory();
                //图片
                if (fuLoad.HasFile)
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.SaveAndDeleteOld(dd.As_Name);
                    dd.As_Name = fuLoad.FileName;
                    dd.As_FileName = fuLoad.File.Server.FileName;
                    dd.As_Size = fuLoad.PostedFile.ContentLength;
                    dd.As_Uid = this.getUID();
                    dd.As_Type = _uppath;
                    Business.Do<IAccessory>().Add(dd);
                }
                AccessoryBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        protected void AccessoryBind()
        {
            try
            {
                List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(this.getUID());
                foreach (Accessory ac in acs)
                {
                    ac.As_FileName = Upload.Get[_uppath].Virtual + ac.As_FileName;
                }
                dlAcc.DataSource = acs;
                dlAcc.DataKeyField = "As_Id";
                dlAcc.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lb_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lb = (LinkButton)sender;
                int id = Convert.ToInt32(lb.CommandArgument);
                Business.Do<IAccessory>().Delete(id);
                AccessoryBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        #endregion

        /// <summary>
        /// 是否是图片新闻的选择，
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cbIsImg_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            fuImg.Visible = this.panelImg.Visible = cb.Checked;
        }
        
    }
}
