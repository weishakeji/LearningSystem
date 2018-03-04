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

namespace Song.Site.Manage.Pay
{
    public partial class PayList : Extend.CustomPage
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
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganRoot();
            Song.Entities.PayInterface[] eas = Business.Do<IPayInterface>().PayAll(org.Org_ID, null, null);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Pai_ID" };
            GridView1.DataBind();


        }
        protected void btnsear_Click(object sender, EventArgs e)
        {
            BindData(null, null);
        }   
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
                Business.Do<IPayInterface>().PayDelete(Convert.ToInt32(id));
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
            Business.Do<IPayInterface>().PayDelete(id);
            BindData(null, null);
        }
        #region 列表中的事件
        /// <summary>
        /// 修改是否使用的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbEnable_Click(object sender, EventArgs e)
        {
            StateButton ub = (StateButton)sender;
            int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
            int id = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
            //
            Song.Entities.PayInterface entity = Business.Do<IPayInterface>().PaySingle(id);
            entity.Pai_IsEnable = !entity.Pai_IsEnable;
            Business.Do<IPayInterface>().PaySave(entity);
            BindData(null, null);
        }
        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbUp_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (Business.Do<IPayInterface>().PayRemoveUp(org.Org_ID, id))
                BindData(null, null);
        }
        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbDown_Click(object sender, EventArgs e)
        {
            GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
            int id = Convert.ToInt32(this.GridView1.DataKeys[gr.RowIndex].Value);
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (Business.Do<IPayInterface>().PayRemoveDown(org.Org_ID, id))
                BindData(null, null);
        }
        #endregion
    }
}
