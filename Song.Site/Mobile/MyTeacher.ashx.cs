using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 我的老师，即当前课程的老师
    /// </summary>
    public class MyTeacher : BasePage
    {
        //计算多少天内的平均分;
        int day = 30;
        //教师信息存储的路径
        private string thPath = "Teacher";
        protected override void InitPageTemplate(HttpContext context)
        {
            Song.Entities.Course currCourse = Extend.LoginState.Accounts.Course();
            if (currCourse == null) return;
            //当前课程主负责老师
            Song.Entities.Teacher th = Business.Do<ITeacher>().TeacherSingle(currCourse.Th_ID);
            if (th != null)
            {
                th.Th_Photo = string.IsNullOrWhiteSpace(th.Th_Photo) ? null : Upload.Get[thPath].Virtual + th.Th_Photo;
                this.Document.SetValue("th", th);
            }
            this.Document.RegisterGlobalFunction(this.getCommentScore);    //计算教师的平均分
        }
        /// <summary>
        /// 计算教师评分
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        protected string getCommentScore(object[] para)
        {
            int thid = 0;
            if (para.Length > 0 && para[0] is int)
            {
                int.TryParse(para[0].ToString(), out thid);
            }
            double score = Business.Do<ITeacher>().CommentScore(thid, day);
            score = Math.Round(score * 10) / 10;
            return score.ToString();
        }
    }
}