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
using System.Text.RegularExpressions;

namespace Song.Site.Manage.Card
{
    public partial class Learningcard_Code : Extend.CustomPage
    {
        //学习卡设置项的id
        protected int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                this.SearchBind();
                BindData(null, null);
            }
        }
        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindData(object sender, EventArgs e)
        {
            //总记录数
            int count = 0;
            Song.Entities.LearningCard[] eas = null;
            eas = Business.Do<ILearningCard>().CardPager(org.Org_ID, id, tbCode.Text.Trim(), null, null, Pager1.Size, Pager1.Index, out count);
            GridView1.DataSource = eas;
            GridView1.DataKeyNames = new string[] { "Lc_ID" };
            GridView1.DataBind();

            Pager1.RecordAmount = count;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Pager1.Qurey = this.SearchQuery();
            Pager1.Index = 1;
        }
        /// <summary>
        /// 回滚按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int id = Convert.ToInt32(btn.CommandArgument);
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(id);
            if (card == null) return;
            try
            {
                Business.Do<ILearningCard>().CardRollback(card);
                this.Alert("回滚成功！");
                BindData(null, null);
            }
            catch (Exception ex)
            {
                this.Alert("错误：" + ex.Message);
            }
        }
    }
}
