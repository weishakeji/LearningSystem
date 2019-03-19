using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using System.IO;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 学习卡详情
    /// </summary>
    public class LearningCardView : BasePage
    {
        //学习卡的编码与密钥
        string code = WeiSha.Common.Request.QueryString["code"].String;
        string pw = WeiSha.Common.Request.QueryString["pw"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            //学习卡
            Song.Entities.LearningCard card = Business.Do<ILearningCard>().CardSingle(code, pw);
            if (card == null) return;
            //学习卡设置项
            Song.Entities.LearningCardSet set = Business.Do<ILearningCard>().SetSingle(card.Lcs_ID);
            //输出关联的课程
            Song.Entities.Course[] courses = Business.Do<ILearningCard>().CoursesGet(set);
            this.Document.SetValue("card", card);
            this.Document.SetValue("set", set);
            this.Document.SetValue("courses", courses);
            //是否已经领用
            int accid = Extend.LoginState.Accounts.UserID;
            if (accid > 0)
            {
                //如果当前学员账号id，与学习卡的归属一致，表示已经领用
                this.Document.SetValue("isget", accid == card.Ac_ID);
            }
        }
    }
}