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
    public partial class ShowPicture_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;       
        //所归属的站点类型
        private string site = WeiSha.Common.Request.QueryString["site"].String;
        //上传资料的所在路径
        private string _uppath = "ShowPic";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
            }
        }
        void fill()
        {
            Song.Entities.ShowPicture mm;
            if (id != 0)
            {
                mm = Business.Do<IStyle>().ShowPicSingle(id);
                cbIsShow.Checked = mm.Shp_IsShow;                          
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.ShowPicture();               
            }
            tbUrl.Text = mm.Shp_Url;
            //链接打开方式
            ListItem liLink = ddlTarget.Items.FindByText(mm.Shp_Target);
            if (liLink != null)
            {
                ddlTarget.SelectedIndex = -1;
                liLink.Selected = true;
            }
            //背景颜色
            tbColor.Text = mm.Shp_BgColor;
            //提示信息与介绍
            tbIntro.Text = mm.Shp_Intro;
            //排序号
            tbTax.Text = mm.Shp_Tax.ToString();
            //菜单图标
            if (!string.IsNullOrEmpty(mm.Shp_File) && mm.Shp_File.Trim() != "")
            {
                this.imgShow.Src = Upload.Get[_uppath].Virtual + mm.Shp_File;
            }

        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.ShowPicture mm = id != 0 ? Business.Do<IStyle>().ShowPicSingle(id) : new Song.Entities.ShowPicture();
            //所属站点
            mm.Shp_Site = site;
            //导航地址
            string url = tbUrl.Text.Trim();
            //if (url.Length > 0 && url.Substring(0,1)!="/")
            //{
            //    url = "/" + url;
            //}
            mm.Shp_Url = url;
            //链接打开方式
            mm.Shp_Target = ddlTarget.SelectedItem.Text;
            //颜色
            mm.Shp_BgColor = tbColor.Text.Trim();
            //提示信息与介绍          
            mm.Shp_Intro = tbIntro.Text.Trim();
            //是否显示，是否粗体显示
            mm.Shp_IsShow = cbIsShow.Checked;          
            //排序号
            mm.Shp_Tax = Convert.ToInt32(tbTax.Text);
            //图标
            if (fuLoad.PostedFile.FileName != "")
            {
                try
                {
                    fuLoad.UpPath = _uppath;
                    fuLoad.IsMakeSmall = false;
                    fuLoad.IsConvertJpg = true;
                    fuLoad.SaveAndDeleteOld(mm.Shp_File);
                    //fuLoad.File.Server.ChangeSize(150, 200, false);
                    mm.Shp_File = fuLoad.File.Server.FileName;
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
                    Business.Do<IStyle>().ShowPicSave(mm);
                }
                else
                {
                    Business.Do<IStyle>().ShowPicAdd(mm);
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
