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

namespace Song.Site.Manage.Money
{
    public partial class Details : Extend.CustomPage
    {
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
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
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            tbStartTime.Text = start.ToString("yyyy-MM-dd");
            tbEndTime.Text = start.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
            this.SearchBind(this.searchBox);
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            //时间区间
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime.TryParse(tbStartTime.Text, out start);
            DateTime end = start.AddMonths(1).AddDays(-1);
            DateTime.TryParse(tbEndTime.Text, out end);
            //来源
            int from = Convert.ToInt16(ddlForm.SelectedValue);
            //操作方向
            int type = Convert.ToInt16(this.ddlType.SelectedValue);
            //学员账号
            Song.Entities.Accounts st = Business.Do<IAccounts>().AccountsSingle(tbSear.Text.Trim(), org.Org_ID);
            int stid = st == null ? -1 : st.Ac_ID;
            Song.Entities.MoneyAccount[] eas = null;
            eas = Business.Do<IAccounts>().MoneyPager(org.Org_ID, stid, type, from, null, (DateTime?)start, (DateTime?)end, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Ma_ID" };
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
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteEvent(object sender, EventArgs e)
        {
            string keys = GridView1.GetKeyValues;
            foreach (string id in keys.Split(','))
                Business.Do<IAccounts>().MoneyDelete(Convert.ToInt32(id));
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
            Business.Do<IAccounts>().MoneyDelete(id);
            BindData(null, null);
        }
        /// <summary>
        /// 获取学员名称
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        protected string GetStudent(object stid)
        {
            int st = 0;
            if (stid == null) return "";
            int.TryParse(stid.ToString(), out st);
            if (st == 0) return "（不存在）";
            Song.Entities.Accounts student = Business.Do<IAccounts>().AccountsSingle(st);
            if (student == null) return "（不存在）";
            string name = string.IsNullOrWhiteSpace(student.Ac_Name) ? "(无名)" : student.Ac_Name;
            name += "：" + student.Ac_AccName;
            return name;
        }

    }
}