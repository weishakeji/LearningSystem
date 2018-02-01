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
using WeiSha.Common.Images;

namespace Song.Site.Manage.Admin
{
    public partial class Departs_Edit : Extend.CustomPage
    {
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
            Song.Entities.Depart dep = id == 0 ? new Song.Entities.Depart() : Business.Do<IDepart>().GetSingle(id);
            if (dep == null) return;
            if (dep != null) this.EntityBind(dep);   
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) throw new WeiSha.Common.ExceptionForAlert("当前机构不存在");
            Song.Entities.Depart dep = id == 0 ? new Song.Entities.Depart() : Business.Do<IDepart>().GetSingle(id);
            dep = this.EntityFill(dep) as Song.Entities.Depart;           
            //判断是否重名
            if (Business.Do<IDepart>().IsExist(org.Org_ID, dep))
            {
                Master.Alert("当前数据已经存在！");
            }
            else
            {
                try
                {
                    if (id == 0)
                    {
                        Business.Do<IDepart>().Add(dep);
                    }
                    else
                    {
                        Business.Do<IDepart>().Save(dep);
                    }

                    Master.AlertCloseAndRefresh("操作成功！");
                }
                catch (Exception ex)
                {
                    Master.Alert(ex.Message);
                }
            }
        }
        
    }
}
