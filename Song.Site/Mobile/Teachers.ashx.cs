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
    /// 当前课程的教师信息
    /// </summary>
    public class Teachers : BasePage
    {
        //计算多少天内的平均分;
        int day = 30;
        protected override void InitPageTemplate(HttpContext context)
        {
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