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

namespace Song.Site.Manage.Site
{
    public partial class User : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {                
                ddlGroupBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 用户组下拉绑定
        /// </summary>
        private void ddlGroupBind()
        {
            try
            {
                Song.Entities.UserGroup[] group = Business.Do<IUser>().GetGroupAll(true);
                this.ddlGroup.DataSource = group;
                this.ddlGroup.DataTextField = "UGrp_Name";
                this.ddlGroup.DataValueField = "UGrp_Id";
                this.ddlGroup.DataBind();
                //
                this.ddlGroup.Items.Insert(0, new ListItem(" -- 所有用户 -- ", "-1"));
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            try
            {
                //总记录数
                int count = 0;
                //当前选择的用户组id
                int groupId = Convert.ToInt16(ddlGroup.SelectedItem.Value);
                bool? isUse = null;
                if (ddlIsUse.SelectedValue != "null")
                {
                    isUse = Convert.ToBoolean(ddlIsUse.SelectedValue);
                }
                Song.Entities.User[] eas = null;
                eas = Business.Do<IUser>().GetUserPager(groupId, isUse, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "user_id" };
                GridView1.DataBind();

                Pager1.RecordAmount = count;
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Index = 1;
            BindData(null, null);
        }   
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            try
            {
                string keys = GridView1.GetKeyValues;
                foreach (string id in keys.Split(','))
                {
                    Business.Do<IUser>().DeleteUser(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WeiSha.WebControl.RowDelete img = (WeiSha.WebControl.RowDelete)sender;
                int index = ((GridViewRow)(img.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Business.Do<IUser>().DeleteUser(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        #region 列表中的事件
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbShow_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.User entity = Business.Do<IUser>().GetUserSingle(id);
                entity.User_IsUse = !entity.User_IsUse;
                Business.Do<IUser>().SaveUser(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        #endregion
    }
}
