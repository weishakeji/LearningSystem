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



namespace Song.Site.Manage.Student
{
    public partial class Students_Details : Extend.CustomPage
    {
        //学员组的id串
        private string sts = WeiSha.Common.Request.QueryString["sts"].String;
        //学员ID
        private int accid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        //员工上传资料的所在路径
        private string _uppath = "Student";
        //学员列表集
        List<Song.Entities.Accounts> accounts = new List<Accounts>();
        protected Song.Entities.Organization org;
        //公章路径，位置
        string stamp = string.Empty;
        string positon = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                //关于公章
                WeiSha.Common.CustomConfig config = CustomConfig.Load(org.Org_Config);
                //机构的公章
                stamp = config["Stamp"].Value.String;
                stamp = System.IO.File.Exists(Upload.Get["Org"].Physics + stamp) ? Upload.Get["Org"].Virtual + stamp : string.Empty;
                //公章显示位置
                positon = config["StampPosition"].Value.String;
                if (string.IsNullOrEmpty(positon)) positon = "right-bottom";
                //取学员列表
                if (string.IsNullOrWhiteSpace(sts) || sts == "-1")
                {
                    Song.Entities.Accounts acc = accid > 0 ? Business.Do<IAccounts>().AccountsSingle(accid) : Extend.LoginState.Accounts.CurrentUser;
                    if (acc != null) accounts.Add(acc);
                }
                else
                {
                    accounts = Business.Do<IAccounts>().AccountsCount(org.Org_ID, true, sts, -1);
                }
                foreach (Accounts acc in accounts)
                {
                    acc.Ac_Age = DateTime.Now.Year - acc.Ac_Age;
                    //个人照片
                    if (!string.IsNullOrEmpty(acc.Ac_Photo) && acc.Ac_Photo.Trim() != "")
                    {
                        acc.Ac_Photo = Upload.Get[_uppath].Virtual + acc.Ac_Photo;
                    }
                    if (!System.IO.File.Exists(acc.Ac_Photo))
                    {
                        acc.Ac_Photo = "";
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
                //绑定学员的课程学习记录
                Song.Entities.Accounts acc = this.accounts[e.Item.ItemIndex];
                Repeater rtp = (Repeater)e.Item.FindControl("rtpLearnInfo");
                DataTable dt = Business.Do<IStudent>().StudentStudyCourseLog(org.Org_ID, acc.Ac_ID);
                if (dt != null)
                {
                    rtp.DataSource = dt;
                    rtp.DataBind();
                }
                //公章
                Image img = (Image)e.Item.FindControl("imgStamp");
                img.Visible = !string.IsNullOrWhiteSpace(stamp);
                img.ImageUrl = stamp;
                img.CssClass = "stamp " + positon;
            }
        }

    }
}
