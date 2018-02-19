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

namespace Song.Site.Manage.Sys
{
    public partial class ProfitSharing : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = "";
            if (!this.IsPostBack)
            {
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {            
            Song.Entities.ProfitSharing[] mm = Business.Do<IProfitSharing>().ThemeAll(null);
            GridView1.DataSource = mm;
            GridView1.DataKeyNames = new string[] { "Ps_Id" };
            GridView1.DataBind();            
        }
        #region 按钮事件       
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {            
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
            {
                Business.Do<IProfitSharing>().ThemeDelete(Convert.ToInt32(id));
            }
            BindData(null, null);           
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
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            Business.Do<IProfitSharing>().ThemeDelete(id);
            BindData(null, null);           
        }
        #endregion

        #region 列表中的事件
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {            
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.ProfitSharing entity = Business.Do<IProfitSharing>().ThemeSingle(id);
            entity.Ps_IsUse = !entity.Ps_IsUse;
            Business.Do<IProfitSharing>().ThemeSave(entity);
            BindData(null, null);          
        }
        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {

            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            //Company.WeiSha.WebControl.GridView gv = (Company.WeiSha.WebControl.GridView)gr.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IProfitSharing>().ThemeRemoveUp(id))
            {
                BindData(null, null);
            }
            else
            {
                //Alert("该项已经处于其所属分类的最顶端，无法上移！");
                //GridView1.Rows[0]
            }
        }
        //下移
        protected void lbDown_Click(object sender, EventArgs e)
        {
            int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            if (Business.Do<IProfitSharing>().ThemeRemoveDown(id))
            {
                BindData(null, null);
            }
            else
            {
                //Alert("该项已经处于其所属分类的最底端，无法下移！");
            }
        }
        #endregion
    }
}
