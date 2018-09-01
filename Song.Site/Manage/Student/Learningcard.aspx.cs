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
using System.IO;

namespace Song.Site.Manage.Student
{
    public partial class Learningcard : Extend.CustomPage
    {
        Song.Entities.Organization org;
        //当前学员的学习卡数量，以及使用数
        protected int cardcount, usecount;
        protected void Page_Load(object sender, EventArgs e)
        {

            org = Business.Do<IOrganization>().OrganCurrent();
            if (!this.IsPostBack)
            {
                //当前学员的学习卡数量
                int accid = Extend.LoginState.Accounts.CurrentUserId;
                if (accid > 0)
                {
                    cardcount = Business.Do<ILearningCard>().AccountCardOfCount(accid);                    
                    usecount = cardcount - Business.Do<ILearningCard>().AccountCardOfCount(accid, 0);                    
                    //学员的学习卡
                    Song.Entities.LearningCard[] cards = Business.Do<ILearningCard>().AccountCards(accid);
                    
                } 
            }
        }
        private void init()
        {


        }
    }
}
