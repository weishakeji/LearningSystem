using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Text.RegularExpressions;
using WeiSha.Common;

namespace Song.Site.Manage.Student
{
    public partial class Subordinates : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.Accounts st = this.Master.Account;
            if (st == null) return;
            this.Form.DefaultButton = this.btnSear.UniqueID;
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!IsPostBack)
            {
                InitBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 界面的初始绑定
        /// </summary>
        private void InitBind()
        {
            //抽有下级会员数量
            int subsum = Business.Do<IAccounts>().SubordinatesCount(Master.Account.Ac_ID, true);
            ltSubSum.Text = subsum.ToString();
            //直接下级数量
            int subcount = Business.Do<IAccounts>().SubordinatesCount(Master.Account.Ac_ID, false);
            ltSubCount.Text = subcount.ToString();
            
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;           
            int pid= this.Master.Account.Ac_ID;
            Song.Entities.Accounts[] eas = null;
            eas = Business.Do<IAccounts>().AccountsPager(org.Org_ID, -1, pid, null, tbAccName.Text, tbName.Text.Trim(), tbPhone.Text.Trim(), Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "ac_id" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
        }
        /// <summary>
        /// 搜索按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnsear_Click(object sender, EventArgs e)
        {
            Pager1.Qurey = this.SearchQuery(this.searchBox);
            Pager1.Index = 1;
        }

        /// <summary>
        /// 获取学员名称
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        protected string GetStudent(string stid)
        {
            int st = 0;
            int.TryParse(stid, out st);
            if (st == 0) return "";
            Song.Entities.Accounts student = Business.Do<IAccounts>().AccountsSingle(st);
            if (student == null) return "";
            string name = string.IsNullOrWhiteSpace(student.Ac_Name) ? "(无名)" : student.Ac_Name;
            name += "：" + student.Ac_AccName;
            return name;
        }
    }
}