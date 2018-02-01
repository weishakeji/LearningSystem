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
    public partial class EmpGroup : Extend.CustomPage
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
            try
            {
                Song.Entities.Organization org;
                if (Extend.LoginState.Admin.IsSuperAdmin)
                    org = Business.Do<IOrganization>().OrganSingle(Extend.LoginState.Admin.CurrentUser.Org_ID);
                else
                    org = Business.Do<IOrganization>().OrganCurrent();
                Song.Entities.EmpGroup[] mm;
                mm = Business.Do<IEmpGroup>().GetAll(org.Org_ID);
                //mm.Length
                GridView1.DataSource = mm;
                GridView1.DataKeyNames = new string[] { "EGrp_Id" };
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
                    Business.Do<IEmpGroup>().Delete(Convert.ToInt16(id));
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
                Business.Do<IEmpGroup>().Delete(id);
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
                Song.Entities.EmpGroup entity = Business.Do<IEmpGroup>().GetSingle(id);
                entity.EGrp_IsUse = !entity.EGrp_IsUse;
                Business.Do<IEmpGroup>().Save(entity);
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
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                //Company.WeiSha.WebControl.GridView gv = (Company.WeiSha.WebControl.GridView)gr.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);

                if (Business.Do<IEmpGroup>().RemoveUp(orgid,id))
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
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
                int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
                if (Business.Do<IEmpGroup>().RemoveDown(orgid,id))
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
