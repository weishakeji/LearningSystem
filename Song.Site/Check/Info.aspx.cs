using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiSha.Common;
using Song.ServiceInterfaces;
namespace Song.Site.Check
{
    public partial class Info : System.Web.UI.Page
    {
        //当前所在机构
        protected Song.Entities.Organization Organ { get; private set; }
        //各项值
        protected int sbjcount, couCount, testCount, quesCount, stCount, newsCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Organ = Business.Do<IOrganization>().OrganCurrent();
            lbOrgName.Text = Organ.Org_Name;
            //专业数
            sbjcount = Business.Do<ISubject>().SubjectOfCount(this.Organ.Org_ID, true, -1);
            //课程数
            couCount = Business.Do<ICourse>().CourseOfCount(this.Organ.Org_ID, -1, -1);
            //考试数
            testCount = Business.Do<ITestPaper>().PagerOfCount(this.Organ.Org_ID, -1, -1, -1, true);
            //试题数
            quesCount = Business.Do<IQuestions>().QuesOfCount(this.Organ.Org_ID, -1, -1, -1, -1, true);
            //学员数
            stCount = Business.Do<IAccounts>().AccountsOfCount(this.Organ.Org_ID, null);
            //资讯数
            newsCount = Business.Do<IContents>().ArticleOfCount(this.Organ.Org_ID, -1);
            
        }
    }
}