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

namespace Song.Site.Manage.Common
{
    public partial class DailyLogView : Extend.CustomPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                fill();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 设置初始界面
        /// </summary>
        private void fill()
        {
            try
            {
                this.tbEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.tbStart.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                //院系下拉绑定
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                Song.Entities.Depart[] nc = Business.Do<IDepart>().GetAll(orgid, true, true);
                this.ddlDepart.DataSource = nc;
                this.ddlDepart.DataTextField = "dep_cnName";
                this.ddlDepart.DataValueField = "dep_id";
                this.ddlDepart.DataBind();
                //
                ddlDepart_SelectedIndexChanged(null, null);
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
                //分类
                string type = rblType.SelectedItem.Text;
                DateTime start = DateTime.Now.AddYears(-100);
                DateTime end = DateTime.Now.AddYears(100);
                if (tbStart.Text.Trim() != "")
                {
                    start = Convert.ToDateTime(tbStart.Text.Trim());
                }
                if (this.tbEnd.Text.Trim() != "")
                {
                    end = Convert.ToDateTime(tbEnd.Text.Trim());
                }
                Song.Entities.DailyLog[] eas = null;
                if (ddlEmp.Items.Count > 0)
                {
                    int accid = Convert.ToInt16(ddlEmp.SelectedValue);
                    eas = Business.Do<IDailyLog>().GetPager(accid, type, start, end, Pager1.Size, Pager1.Index, out count);

                    GridView1.DataSource = eas;
                    GridView1.DataKeyNames = new string[] { "Dlog_Id" };
                    GridView1.DataBind();
                }
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
                    Business.Do<IDailyLog>().Delete(Convert.ToInt16(id));
                }
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 当院系改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDepart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //当前选择的院系id
                int depId = Convert.ToInt16(ddlDepart.SelectedItem.Value);
                int orgid = Extend.LoginState.Admin.CurrentUser.Org_ID;
                EmpAccount[] eas = null;
                eas = Business.Do<IEmployee>().GetAll(orgid,depId, true,null);

                ddlEmp.DataSource = eas;
                ddlEmp.DataTextField = "Acc_Name";
                ddlEmp.DataValueField = "Acc_Id";
                ddlEmp.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
    }
}
