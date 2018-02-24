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

namespace Song.Site.Manage.Sys
{
    public partial class OrganLevel_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //绑定分润方案
                Song.Entities.ProfitSharing[] pss = Business.Do<IProfitSharing>().ThemeAll(null);
                ddlProfit.DataSource = pss;
                ddlProfit.DataBind();
                ddlProfit.Items.Insert(0,new ListItem("--请选择--","-1"));
                //
                fill();
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {            
            Song.Entities.OrganLevel mm;
            if (id != 0)
            {
                mm = Business.Do<IOrganization>().LevelSingle(id);
                cbIsUse.Checked = (bool)mm.Olv_IsUse;
            }
            else
            {
                //如果是新增
                mm = new Song.Entities.OrganLevel();
            }
            tbName.Text = mm.Olv_Name;
            tbTag.Text = mm.Olv_Tag;
            tbLevel.Text = mm.Olv_Level.ToString();
            //分润方案
            ListItem liProf = ddlProfit.Items.FindByValue(mm.Ps_ID.ToString());
            if (liProf != null)
            {
                ddlProfit.SelectedIndex = -1;
                liProf.Selected = true;
            }
            //说明
            tbIntro.Text = mm.Olv_Intro;
            
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
                Song.Entities.OrganLevel mm;
                if (id != 0)
                {
                    mm = Business.Do<IOrganization>().LevelSingle(id);
                }
                else
                {
                    //如果是新增
                    mm = new Song.Entities.OrganLevel();
                }
                mm.Olv_Name = tbName.Text.Trim();
                mm.Olv_Tag = tbTag.Text.Trim();
                mm.Olv_Level = Convert.ToInt16(tbLevel.Text);
                //说明
                mm.Olv_Intro = tbIntro.Text.Trim();
                mm.Olv_IsUse = cbIsUse.Checked;
                //分润方案
                int profitid = 0;
                int.TryParse(ddlProfit.SelectedValue, out profitid);
                mm.Ps_ID = profitid;
                //确定操作
                if (id == 0)
                {
                    Business.Do<IOrganization>().LevelAdd(mm);
                }
                else
                {
                    Business.Do<IOrganization>().LevelSave(mm);
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
