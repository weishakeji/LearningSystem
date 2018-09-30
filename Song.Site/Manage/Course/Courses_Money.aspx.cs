using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WeiSha.Common;
using WeiSha.WebControl;
using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.Course
{
    public partial class Courses_Money : Extend.CustomPage
    {
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //上传资料的所有路径
        //private string _uppath = "Course";
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            //隐藏确定按钮
            EnterButton btnEnter = (EnterButton)Master.FindControl("btnEnter");
            btnEnter.Visible = false;  
            //实始化
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                BindPriceData();
            }
            //this.Master.Enter_Click += btnEnter_Click;
            this.Master.Next_Click += btnNext_Click;
        }
        #region 按钮事件
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Course cou = this.Save();
            this.Alert("操作成功");
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            Song.Entities.Course cou = this.Save();
        }
         /// <summary>
        /// 保存当前课程
        /// </summary>
        /// <returns></returns>
        private Song.Entities.Course Save()
        {
            Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return null;
            //是否免费，是否试用，以及试题数
            cou.Cou_IsFree = cbIsFree.Checked;
            cou.Cou_IsTry = cbIsTry.Checked;           
            try
            {
                Business.Do<ICourse>().CourseSave(cou);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return cou;
        }
        #endregion
       

        #region 价格管理
        /// <summary>
        /// 绑定价格信息的列表
        /// </summary>
        protected void BindPriceData()
        {            
            Song.Entities.Course cou = couid < 1 ? new Song.Entities.Course() : Business.Do<ICourse>().CourseSingle(couid);
            if (cou == null) return;
            //课程名称
            lbName.Text = cou.Cou_Name;
            //是否免费
            cbIsFree.Checked = cou.Cou_IsFree;
            Cou_IsFree_CheckedChanged(null, null);
            //是否允许试用
            cbIsTry.Checked = cou.Cou_IsTry;           
            ////每个章节，试用的试题数
            //tbTryNum.Text = cou.Cou_TryNum > 0 ? cou.Cou_TryNum.ToString() : "";
            //全局UID
            ViewState["UID"] = string.IsNullOrWhiteSpace(cou.Cou_UID) ? getUID() : cou.Cou_UID;
            Song.Entities.CoursePrice[] prices = Business.Do<ICourse>().PriceCount(0, cou.Cou_UID, null, -1);
            gvPrice.DataSource = prices;
            gvPrice.DataKeyNames = new string[] { "CP_ID" };
            gvPrice.DataBind();
        }
        /// <summary>
        /// 添加价格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAAEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.CoursePrice cp = new CoursePrice();
            cp.Cou_UID = getUID();
            //消费时间
            int span;
            int.TryParse(tbSpan.Text, out span);
            cp.CP_Span = span;
            //价格
            int price;
            int.TryParse(tbPriceAdd.Text, out price);
            cp.CP_Price = price;
            //单位
            cp.CP_Unit = ddlUnit.SelectedItem.Text;
            cp.CP_IsUse = true;
            //卡券
            int coupon = 0;
            int.TryParse(tbCoupon.Text, out coupon);
            cp.CP_Coupon = coupon;
            Business.Do<ICourse>().PriceAdd(cp);           
            BindPriceData();
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
            int id = int.Parse(this.gvPrice.DataKeys[index].Value.ToString());
            Business.Do<ICourse>().PriceDelete(id);
            BindPriceData();
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
            gvPrice.EditIndex = index;
            BindPriceData();
            //当前单位
            DropDownList ddlUnit = (DropDownList)gvPrice.Rows[index].FindControl("ddlUnit");
            string unit = img.CommandArgument;
            ListItem li = ddlUnit.Items.FindByText(unit);
            if (li != null)
            {
                ddlUnit.SelectedIndex = -1;
                li.Selected = true;
            }
        }
        /// <summary>
        /// 编辑当前数据项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditEnter_Click(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
            int index = ((GridViewRow)(btn.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvPrice.DataKeys[index].Value.ToString());
            //
            Song.Entities.CoursePrice col = Business.Do<ICourse>().PriceSingle(id);
            if (col != null)
            {
                //区间
                TextBox tb = (TextBox)gvPrice.Rows[index].FindControl("tbSpan");
                int span;
                int.TryParse(tb.Text, out span);
                col.CP_Span = span;
                //单位
                DropDownList ddlUnit = (DropDownList)gvPrice.Rows[index].FindControl("ddlUnit");
                col.CP_Unit = ddlUnit.SelectedItem.Text;
                //价格
                TextBox tbprice = (TextBox)gvPrice.Rows[index].FindControl("tbPrice");
                int price;
                int.TryParse(tbprice.Text, out price);
                col.CP_Price = price;
                //卡券
                int coupon = 0;
                TextBox tbcoupon = (TextBox)gvPrice.Rows[index].FindControl("tbCoupon");
                int.TryParse(tbcoupon.Text, out coupon);
                col.CP_Coupon = coupon;
                //是否可用
                CheckBox cb = (CheckBox)gvPrice.Rows[index].FindControl("cbIsUse");
                col.CP_IsUse = cb.Checked;
                //
                Business.Do<ICourse>().PriceSave(col);                
            }
            gvPrice.EditIndex = -1;
            BindPriceData();
        }
        /// <summary>
        /// 退出编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEditBack_Click(object sender, EventArgs e)
        {
            gvPrice.EditIndex = -1;
            BindPriceData();
        }
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbUse_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.gvPrice.DataKeys[index].Value.ToString());
            //
            Song.Entities.CoursePrice entity = Business.Do<ICourse>().PriceSingle(id);
            entity.CP_IsUse = !entity.CP_IsUse;
            Business.Do<ICourse>().PriceSave(entity);
            BindPriceData();
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            gvPrice.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvPrice.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ICourse>().PriceUp(id)) BindPriceData();

        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            gvPrice.EditIndex = -1;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.gvPrice.DataKeys[gr.RowIndex].Value);
            if (Business.Do<ICourse>().PriceDown(id)) BindPriceData();
        }
        #endregion

        /// <summary>
        /// 是否免费的单选框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cou_IsFree_CheckedChanged(object sender, EventArgs e)
        {
            cbIsTry.Enabled = !cbIsFree.Checked;
            cbIsTry.Checked = !cbIsFree.Checked;           
        }
    }
}