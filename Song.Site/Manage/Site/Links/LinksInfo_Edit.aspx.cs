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

namespace Song.Site.Manage.Site.Links
{
    public partial class LinksInfo_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        private string _uppath = "Links";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ddlSortBind();
                fill();
            }
        }
        /// <summary>
        /// 栏目下拉绑定
        /// </summary>
        private void ddlSortBind()
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.LinksSort[] eas = Business.Do<ILinks>().GetSortAll(org.Org_ID, true, null);
            this.ddlSort.DataSource = eas;
            this.ddlSort.DataTextField = "Ls_Name";
            this.ddlSort.DataValueField = "Ls_Id";
            this.ddlSort.DataBind();
            //
            //this.ddlSort.Items.Insert(0, new ListItem(" -- 选择分类 -- ", "-1"));
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            Song.Entities.Links mm;
            if (id != 0)
            {
                mm = Business.Do<ILinks>().LinksSingle(id);
                cbIsShow.Checked = mm.Lk_IsShow;
                cbIsUse.Checked = mm.Lk_IsUse;
                ListItem li = ddlSort.Items.FindByValue(mm.Ls_Id.ToString());
                if (li != null) li.Selected=true;
                //图片
                this.imgShow.Src = Upload.Get[_uppath].Virtual + mm.Lk_Logo;
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Links();
            }
            tbName.Text = mm.Lk_Name;
            tbUrl.Text = mm.Lk_Url;
            tbEmail.Text = mm.Lk_Email;
            //说明
            tbIntro.Text = mm.Lk_Tootip;
            //申请信息
            plApply.Visible = mm.Lk_IsApply;
            tbMaster.Text = mm.Lk_SiteMaster;
            tbQQ.Text = mm.Lk_QQ;
            tbMobile.Text = mm.Lk_Mobile;
            tbExplain.Text = mm.Lk_Explain;
            //是否通过审核
            cbVerify.Checked = mm.Lk_IsVerify;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {

            Song.Entities.Links mm;
            if (id != 0)
            {
                mm = Business.Do<ILinks>().LinksSingle(id);                
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Links();
            }
            mm.Lk_Name = tbName.Text;
            mm.Lk_IsShow = cbIsShow.Checked;
            mm.Lk_IsUse = cbIsUse.Checked;
            mm.Lk_Url = tbUrl.Text;
            mm.Lk_Email = tbEmail.Text;
            //说明
            mm.Lk_Tootip = tbIntro.Text;
            //当前选择的栏目id
            int col = Convert.ToInt16(this.ddlSort.SelectedItem.Value);
            mm.Ls_Id = col;
            //图片
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = true;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SmallHeight = 100;
                    fuLoad.SaveAndDeleteOld(mm.Lk_Logo);
                    //重新赋值
                    mm.Lk_Logo = fuLoad.File.Server.FileName;
                    mm.Lk_LogoSmall = fuLoad.File.Server.SmallFileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            //申请信息
            mm.Lk_SiteMaster = tbMaster.Text;
            mm.Lk_QQ = tbQQ.Text;
            mm.Lk_Mobile = tbMobile.Text;
            mm.Lk_Explain = tbExplain.Text;
            //是否通过审核
            cbVerify.Checked = mm.Lk_IsVerify;
            //确定操作
            if (id == 0)
            {
                Business.Do<ILinks>().LinksAdd(mm);
            }
            else
            {
                Business.Do<ILinks>().LinksSave(mm);
            }
            Master.AlertCloseAndRefresh("操作成功！");
        }
    }
}
