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
using WeiSha.WebControl;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Sys
{
    public partial class ProfitSharing_Edit : Extend.CustomPage
    {
        //要操作的对象主键
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
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
            Song.Entities.ProfitSharing theme = id < 1 ? null : Business.Do<IProfitSharing>().ThemeSingle(id);
            if (theme != null)
            {
                tbName.Text = theme.Ps_Name;
                tbIntro.Text = theme.Ps_Intro;
                cbIsUse.Checked = theme.Ps_IsUse;
            }
            BindGridviewData();
        }
        protected void BindGridviewData()
        {
            Song.Entities.ProfitSharing[] profits = Business.Do<IProfitSharing>().ProfitAll(id, null);
            gvProfit.DataSource = profits;
            gvProfit.DataKeyNames = new string[] { "Ps_ID" };
            gvProfit.DataBind();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.ProfitSharing theme = id < 1 ? new Song.Entities.ProfitSharing() : Business.Do<IProfitSharing>().ThemeSingle(id);
            if (theme != null)
            {
                theme.Ps_Name = tbName.Text.Trim();
                theme.Ps_Intro = tbIntro.Text.Trim();
                theme.Ps_IsUse = cbIsUse.Checked;
            }
            if (id < 1)
            {
                id = Business.Do<IProfitSharing>().ThemeAdd(theme);
                //刷新页面
                string encrypt = WeiSha.Common.DataConvert.EncryptForBase64(id.ToString());
                encrypt = System.Web.HttpUtility.UrlEncode(encrypt);
                this.Response.Redirect(this.AddPara("id", encrypt));
            }
            else
            {
                Business.Do<IProfitSharing>().ThemeSave(theme);
                this.Message.Alert("操作成功！");
            }
           
        }
        #region 列表中的事件
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvProfit.DataKeys[index].Value.ToString());
            //
            Song.Entities.ProfitSharing entity = Business.Do<IProfitSharing>().ProfitSingle(id);
            entity.Ps_IsUse = !entity.Ps_IsUse;
            Business.Do<IProfitSharing>().ProfitSave(entity);
            BindGridviewData();
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {

            WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvProfit.DataKeys[index].Value.ToString());
            Business.Do<IProfitSharing>().ProfitDelete(id);
            BindGridviewData();
        }
        /// <summary>
        /// 进入编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {

            WeiSha.WebControl.RowEdit img = (WeiSha.WebControl.RowEdit)sender;
            int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
            gvProfit.EditIndex = index;
            plAddProfit.Enabled = false;    //禁用新增
            BindGridviewData();
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            gvProfit.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvProfit.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IProfitSharing>().ProfitRemoveUp(id)) BindGridviewData();

        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            gvProfit.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvProfit.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IProfitSharing>().ProfitRemoveDown(id)) BindGridviewData();
        }
        /// <summary>
        /// 编辑当前数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditEnter_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
            GridViewRow row = (GridViewRow)btn.Parent.Parent;   //当前所在行
            int id = int.Parse(this.gvProfit.DataKeys[row.RowIndex].Value.ToString());
            //
            Song.Entities.ProfitSharing profit = Business.Do<IProfitSharing>().ProfitSingle(id);
            if (profit != null)
            {
                //资金比例
                TextBox tbm = (TextBox)row.FindControl("tbMoneyEdit");
                int money;
                int.TryParse(tbm.Text, out money);
                profit.Ps_Moneyratio = money;
                //卡券比例
                TextBox tbc = (TextBox)row.FindControl("tbCouponEdit");
                int coupon;
                int.TryParse(tbc.Text, out coupon);
                profit.Ps_Couponratio = coupon;
                //是否可用
                CheckBox cb = (CheckBox)row.FindControl("cbIsUse");
                profit.Ps_IsUse = cb.Checked;
                //保存
                try
                {
                    Business.Do<IProfitSharing>().ProfitSave(profit);
                }
                catch (Exception ex)
                {
                    this.Message.Alert(ex.Message);
                }
            }
            gvProfit.EditIndex = -1;
            plAddProfit.Enabled = true;    //启用新增
            BindGridviewData();
        }
        /// <summary>
        /// 退出编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditBack_Click(object sender, EventArgs e)
        {
            gvProfit.EditIndex = -1;
            plAddProfit.Enabled = true;    //启用新增
            BindGridviewData();
        }
        #endregion
        /// <summary>
        /// 添加分润比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEnter_Click(object sender, EventArgs e)
        {
            int pid = id <= 0 ? 0 : id; //方案id
            //如果是新增状态
            if (id <= 0)
            {
                Song.Entities.ProfitSharing theme = new Entities.ProfitSharing();
                theme.Ps_Name = tbName.Text.Trim();    //分润方案名称
                theme.Ps_IsUse = cbIsUse.Checked;   //是否启用
                theme.Ps_Intro = tbName.Text.Trim();    //说明
                pid = Business.Do<IProfitSharing>().ThemeAdd(theme);
            }
            Song.Entities.ProfitSharing profit = new Entities.ProfitSharing();
            //资金分润比例
            int money=0;
            int.TryParse(tbMoneyAdd.Text,out money);
            profit.Ps_Moneyratio = money;
            //卡券分润比例
            int coupon = 0;
            int.TryParse(this.tbCouponAdd.Text, out coupon);
            profit.Ps_Couponratio = coupon;
            //是否启用，上级ID
            profit.Ps_IsUse = true;
            profit.Ps_PID = pid;
            try
            {
                Business.Do<IProfitSharing>().ProfitAdd(profit);
            }
            catch (Exception ex)
            {
                this.Message.Alert(ex.Message);
            }
            //刷新页面
            if (id <= 0)
            {
                string encrypt = WeiSha.Common.DataConvert.EncryptForBase64(pid.ToString());
                encrypt = System.Web.HttpUtility.UrlEncode(encrypt);
                this.Response.Redirect(this.AddPara("id", encrypt));
            }
            else
            {
                BindGridviewData();
                this.tbCouponAdd.Text = this.tbMoneyAdd.Text = "";
            }
        }
    }
}
