using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Manage.Student
{
    public partial class Link : Extend.CustomPage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = this.btnEnter.UniqueID;
            if (!IsPostBack) fill();

        }

        private void fill()
        {
            Song.Entities.Accounts acc = id == 0 ? this.Master.Account : Business.Do<IAccounts>().AccountsSingle(id);
            if (acc == null) return;
            //将数据实体绑定到界面控件
            this.EntityBind(acc);
           
        }
        protected void btnEnter_Click(object sender, EventArgs e)
        {
            Song.Entities.Accounts th = this.Master.Account;
            if (th == null) return;
            //将界面中的录入，填充到实体
            th = this.EntityFill(th) as Song.Entities.Accounts;           
            Business.Do<IAccounts>().AccountsSave(th);
        }
    }
}