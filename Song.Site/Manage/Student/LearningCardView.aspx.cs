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
    public partial class LearningCardView : Extend.CustomPage
    {
        //学习卡的编码与密钥
        string code = WeiSha.Common.Request.QueryString["code"].String;
        string pw = WeiSha.Common.Request.QueryString["pw"].String;
        protected void Page_Load(object sender, EventArgs e)
        {
            //学习卡
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(code, pw);
            if (card == null) return;
            this.EntityBind(card);
            //学习卡设置项
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(card.Lcs_ID);
            this.EntityBind(set);

            //输出关联的课程
            Song.Entities.Course[] courses = Business.Do<ILearningCard>().CoursesGet(set);
            rptCourses.DataSource = courses;
            rptCourses.DataBind();
        }
    }
}