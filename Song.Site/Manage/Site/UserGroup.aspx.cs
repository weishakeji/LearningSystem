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
    public partial class UserGroup : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            try
            {
                Song.Entities.UserGroup[] mm;
                mm = Business.Do<IUser>().GetGroupAll();
                //mm.Length
                GridView1.DataSource = mm;
                GridView1.DataKeyNames = new string[] { "UGrp_Id" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        #region 按钮事件       
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddEvent(object sender, EventArgs e)
        {
            
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
                    Business.Do<IUser>().DeleteGroup(Convert.ToInt16(id));
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
                Business.Do<IUser>().DeleteGroup(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
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
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                //
                Song.Entities.UserGroup entity = Business.Do<IUser>().GetGroupSingle(id);
                entity.UGrp_IsUse = !entity.UGrp_IsUse;
                Business.Do<IUser>().SaveGroup(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        //上移
        protected void lbUp_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                //Company.WeiSha.WebControl.GridView gv = (Company.WeiSha.WebControl.GridView)gr.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);

                if (Business.Do<IUser>().GroupRemoveUp(id))
                {
                    BindData(null, null);
                }
                else
                {
                    //Alert("该项已经处于其所属分类的最顶端，无法上移！");
                    //GridView1.Rows[0]
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        //下移
        protected void lbDown_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
                if (Business.Do<IUser>().GroupRemoveDown(id))
                {
                    BindData(null, null);
                }
                else
                {
                    //Alert("该项已经处于其所属分类的最底端，无法下移！");
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 
        }
        #endregion
    }
}
