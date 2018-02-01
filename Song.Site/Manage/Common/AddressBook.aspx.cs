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
using System.Collections.Generic;


namespace Song.Site.Manage.Common
{
    public partial class AddressBook :Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bindSort();
                BindData(null, null);
            }
        }
        private void bindSort()
        {
            try
            {
                List<Song.Entities.AddressSort> asort = Business.Do<IAddressList>().SortAll(true);
                ddlTpye.DataSource = asort;
                ddlTpye.DataTextField = "ads_name";
                ddlTpye.DataValueField = "ads_id";
                ddlTpye.DataBind();
                this.ddlTpye.Items.Insert(0, new ListItem(" -- 所有分类 -- ", "-1"));
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
                //当前选择的栏目id
                string type = ddlTpye.SelectedValue;
                if (type == "-1")
                {
                    type = "";
                }
                else
                {
                    type = ddlTpye.SelectedItem.Text;
                }
                EmpAccount acc = Extend.LoginState.Admin.CurrentUser;
                List<Song.Entities.AddressList> eas = null;
                eas = Business.Do<IAddressList>().AddressPager(acc.Acc_Id, type, this.tbSear.Text, Pager1.Size, Pager1.Index, out count);
                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "Adl_Id" };
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
                    Business.Do<IAddressList>().AddressDelete(Convert.ToInt16(id));
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
                Business.Do<IAddressList>().AddressDelete(id);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
