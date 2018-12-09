using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Song.ServiceInterfaces;
using WeiSha.Common;
using System.IO;

namespace Song.Site.Manage.Card
{
    public partial class GoBack : Extend.CustomPage
    {
        //学习卡
        protected string code = WeiSha.Common.Request.QueryString["code"].String;
        protected string pw = WeiSha.Common.Request.QueryString["pw"].String;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(code, pw);
            if (card == null) return;
            //时效
            lbLimit.Text = card.Lc_LimitStart.ToString("yyyy/M/d") + " - " + card.Lc_LimitEnd.ToString("yyyy/M/d");
            //使用人，使用时间
            lbAccname.Text = card.Ac_AccName;
            lbUsetime.Text = card.Lc_CrtTime.ToString();
            lbState.Text = card.Lc_State == 1 ? "使用" : "";
            //关联课程
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(card.Lcs_ID);
            Song.Entities.Course[] courses = Business.Do<ILearningCard>().CoursesGet(set);
            rptCourse.DataSource = courses;
            rptCourse.DataBind();
        }
        /// <summary>
        /// 回滚，但保留学习记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoback1_Click(object sender, EventArgs e)
        {
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(code, pw);
            if (card == null) return;
            try
            {
                Business.Do<ILearningCard>().CardRollback(card);
                this.AlertCloseAndRefresh("回滚成功！");
            }
            catch (Exception ex)
            {
                this.Alert("错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 回滚，且清除学习记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGoback2_Click(object sender, EventArgs e)
        {
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(code, pw);
            if (card == null) return;
            try
            {
                Business.Do<ILearningCard>().CardRollback(card,true);
                this.AlertCloseAndRefresh("回滚并清除学习记录成功！");
            }
            catch (Exception ex)
            {
                this.Alert("错误：" + ex.Message);
            }
        }
    }
}