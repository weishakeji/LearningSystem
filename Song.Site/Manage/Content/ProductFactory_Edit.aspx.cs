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

namespace Song.Site.Manage.Content
{
    public partial class ProductFactory_Edit : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
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
                Song.Entities.ProductFactory mm;
                if (id != 0)
                {
                    mm = Business.Do<IProduct>().FactorySingle(id);
                    //是否使用与显示
                    cbIsUse.Checked = mm.Pfact_IsUse;
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.ProductFactory();
                    cbIsUse.Checked = true;
                }
                tbName.Text = mm.Pfact_Name;
                tbAddr.Text = mm.Pfact_Addr;
                tbTel.Text = mm.Pfact_Tel;
                tbZip.Text = mm.Pfact_Zip;
                tbWebsite.Text = mm.Pfact_Website;
                //说明
                this.tbIntro.Text = mm.Pfact_Intro;
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
                Song.Entities.ProductFactory mm;
                if (id != 0)
                {
                    mm = Business.Do<IProduct>().FactorySingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.ProductFactory();
                }
                mm.Pfact_Name = tbName.Text;
                mm.Pfact_Addr = tbAddr.Text;
                mm.Pfact_Tel = tbTel.Text;
                mm.Pfact_Zip = tbZip.Text;
                mm.Pfact_Website = tbWebsite.Text;
                //是否使用与显示
                mm.Pfact_IsUse = cbIsUse.Checked;
                mm.Pfact_Intro = tbIntro.Text;
                //确定操作
                if (id == 0)
                {
                    Business.Do<IProduct>().FactoryAdd(mm);
                }
                else
                {
                    Business.Do<IProduct>().FactorySave(mm);
                }
                Master.AlertCloseAndRefresh("操作成功！");
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
