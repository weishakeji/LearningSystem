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

namespace Song.Site.Manage.Admin
{
    public partial class Navigation_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //导航分类
        private string type = WeiSha.Common.Request.QueryString["type"].String;
        //所归属的站点类型
        private string site = WeiSha.Common.Request.QueryString["site"].String;
        //上传资料的所在路径
        private string _uppath = "Org";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bindTree(); 
                fill();
            }
        }
        /// <summary>
        /// 绑定导航
        /// </summary>
        private void bindTree()
        {
            Song.Entities.Organization org = null;
            org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Navigation[] navi = Business.Do<IStyle>().NaviAll(null, site, type, org.Org_ID);
            ddlTree.DataSource = navi;
            this.ddlTree.DataTextField = "Nav_Name";
            this.ddlTree.DataValueField = "Nav_ID";
            //this.ddlTree.
            this.ddlTree.Root = 0;
            this.ddlTree.DataBind();
            //
            ddlTree.Items.Insert(0,new ListItem("   -- 顶级 --","0"));
        }

        void fill()
        {
            Song.Entities.Navigation mm;
            if (id != 0)
            {
                mm = Business.Do<IStyle>().NaviSingle(id);
                cbIsShow.Checked = mm.Nav_IsShow;
                cbIsBold.Checked = mm.Nav_IsBold;               
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Navigation();               
            }
            tbName.Text = mm.Nav_Name;
            //tbEnName.Text = mm.Nav_EnName;
            //上级导航
            ListItem liNav = ddlTree.Items.FindByValue(mm.Nav_PID.ToString());
            if (liNav != null)
            {
                ddlTree.SelectedIndex = -1;
                liNav.Selected = true;
            }
            tbUrl.Text = mm.Nav_Url;
            //链接打开方式
            ListItem liLink = ddlTarget.Items.FindByText(mm.Nav_Target);
            if (liLink != null)
            {
                ddlTarget.SelectedIndex = -1;
                liLink.Selected = true;
            }
            //字体，颜色
            tbFont.Text = mm.Nav_Font;
            tbColor.Text = mm.Nav_Color;
            //提示信息与介绍
            tbTitle.Text = mm.Nav_Title;
            tbIntro.Text = mm.Nav_Intro;
            //排序号
            tbTax.Text = mm.Nav_Tax.ToString();
            //菜单图标
            if (!string.IsNullOrEmpty(mm.Nav_Logo) && mm.Nav_Logo.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + mm.Nav_Logo;
            }

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Navigation mm;
            if (id != 0)
            {
                mm = Business.Do<IStyle>().NaviSingle(id);
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.Navigation();
            }
            //类型与名称
            mm.Nav_Type = type;
            mm.Nav_Site = site;
            mm.Nav_Name = tbName.Text.Trim();
            //mm.Nav_EnName = tbEnName.Text.Trim();
            //上级导航
            mm.Nav_PID = Convert.ToInt32(ddlTree.SelectedValue);
            //导航地址
            string url = tbUrl.Text.Trim();
            //if (url.Length > 0 && url.Substring(0,1)!="/")
            //{
            //    url = "/" + url;
            //}
            mm.Nav_Url = url;
            //链接打开方式
            mm.Nav_Target = ddlTarget.SelectedItem.Text;
            //字体，颜色
            mm.Nav_Font = tbFont.Text.Trim();
            mm.Nav_Color = tbColor.Text.Trim();
            //提示信息与介绍
            mm.Nav_Title = tbTitle.Text.Trim();
            mm.Nav_Intro = tbIntro.Text.Trim();
            //是否显示，是否粗体显示
            mm.Nav_IsShow = cbIsShow.Checked;
            mm.Nav_IsBold = cbIsBold.Checked;
            //排序号
            mm.Nav_Tax = Convert.ToInt32(tbTax.Text);
            //图标
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(mm.Nav_Logo);
                    fuLoad.File.Server.ChangeSize(200, 200, false);
                    mm.Nav_Logo = fuLoad.File.Server.FileName;
                    //
                    imgShow.Src = fuLoad.File.Server.VirtualPath;
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
            try
            {
                if (id != 0)
                {
                    Business.Do<IStyle>().NaviSave(mm);
                }
                else
                {
                    Business.Do<IStyle>().NaviAdd(mm);
                }            
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Master.Alert(ex.Message);
            }
        }
        
    }
}
