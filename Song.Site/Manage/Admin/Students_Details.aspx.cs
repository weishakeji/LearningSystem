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
using System.Collections.Generic;



namespace Song.Site.Manage.Admin
{
    public partial class Students_Details : Extend.CustomPage
    {
        //学员组的id串
        private string sts = WeiSha.Common.Request.QueryString["sts"].String;
        //员工上传资料的所在路径
        private string _uppath = "Student";
        //学员列表集
        List<Song.Entities.Accounts> accounts = new List<Accounts>();
        Song.Entities.Organization org;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {                
                if (!string.IsNullOrWhiteSpace(sts))
                {
                    accounts = Business.Do<IAccounts>().AccountsCount(org.Org_ID, true, sts, -1);
                }
                else
                {
                    Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
                    if (acc != null) accounts.Add(acc);
                }
                foreach (Accounts acc in accounts)
                {
                    acc.Ac_Age = DateTime.Now.Year - acc.Ac_Age;
                    //个人照片
                    if (!string.IsNullOrEmpty(acc.Ac_Photo) && acc.Ac_Photo.Trim() != "")
                    {
                        acc.Ac_Photo= Upload.Get[_uppath].Virtual + acc.Ac_Photo;
                    }
                }
                //绑定
                rptAccounts.DataSource = accounts;
                rptAccounts.DataBind();
            }
           
        }
        /// <summary>
        /// 获取学历，数据库中记录的是学历编号
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected string getEdu(object val)
        {
            if (val != null)
            {
                ListItem li = ddlEducation.Items.FindByValue(val.ToString());
                if (li != null)
                {
                    return li.Text;
                }
            }
            return "";
        }
        /// <summary>
        /// 学员绑定事件，用于显示学习情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptAccounts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Song.Entities.Accounts acc = this.accounts[e.Item.ItemIndex];
                Repeater rtp = (Repeater)e.Item.FindControl("rtpLearnInfo");
                DataTable dt = Business.Do<IStudent>().StudentStudyCourseLog(org.Org_ID, acc.Ac_ID);
                rtp.DataSource = dt;
                rtp.DataBind();   
            }
        }
    
       
       
    }
}
