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
    public partial class ProductMaterial_Edit : Extend.CustomPage
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
                Song.Entities.ProductMaterial mm;
                if (id != 0)
                {
                    mm = Business.Do<IProduct>().MaterialSingle(id);
                    //是否使用与显示
                    cbIsUse.Checked = mm.Pmat_IsUse;
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.ProductMaterial();
                    cbIsUse.Checked = true;
                }
                tbName.Text = mm.Pmat_Name;
                //说明
                this.tbIntro.Text = mm.Pmat_Intro;
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
                Song.Entities.ProductMaterial mm;
                if (id != 0)
                {
                    mm = Business.Do<IProduct>().MaterialSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.ProductMaterial();
                }
                mm.Pmat_Name = tbName.Text.Trim();
                //说明
                mm.Pmat_Intro = tbIntro.Text;
                //是否使用与显示
                mm.Pmat_IsUse = cbIsUse.Checked;
                //确定操作
                if (id == 0)
                {
                    Business.Do<IProduct>().MaterialAdd(mm);
                }
                else
                {
                    Business.Do<IProduct>().MaterialSave(mm);
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
