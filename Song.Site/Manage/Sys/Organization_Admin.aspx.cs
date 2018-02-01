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
using System.Text.RegularExpressions;
using WeiSha.WebControl;

namespace Song.Site.Manage.Sys
{
    public partial class Organization_Admin : Extend.CustomPage
    {
        //机构id
        private int id = WeiSha.Common.Request.QueryString["id"].Decrypt().Int32 ?? 0;
        //超级管理员角色的id
        protected string superid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Position super = Business.Do<IPosition>().GetAdmin(id);
            superid = super.Posi_Id.ToString();
            if (!this.IsPostBack)
            {
                BindData(null,null);
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
                //当前选择的机构id
                int orgId = (int)Extend.LoginState.Admin.CurrentUser.Org_ID;
                EmpAccount[] eas = null;
                eas = Business.Do<IEmployee>().GetAll4Org(id, true, "");

                GridView1.DataSource = eas;
                GridView1.DataKeyNames = new string[] { "acc_id" };
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 修改是否显示的状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sbAdmin_Click(object sender, EventArgs e)
        {
            try
            {
                StateButton ub = (StateButton)sender;
                int index = ((GridViewRow)(ub.Parent.Parent)).RowIndex;
                //获取员工id
                int accid = int.Parse(this.GridView1.DataKeys[index].Value.ToString());
                Song.Entities.EmpAccount entity = Business.Do<IEmployee>().GetSingle(accid);
                //分厂的管理岗位
                Song.Entities.Position posi = Business.Do<IPosition>().GetAdmin(id);

                entity.Posi_Id = posi.Posi_Id;
                entity.Posi_Name = posi.Posi_Name;  
                Business.Do<IEmployee>().Save(entity);
                BindData(null, null);
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            }
        }
        /// <summary>
        /// 判断当前员工是不是分厂管理员
        /// </summary>
        /// <param name="accid"></param>
        /// <returns></returns>
        protected bool GetAminState(string accid)
        {
           return  Business.Do<IEmployee>().IsAdmin(Convert.ToInt32(accid));
        }
    }
}
