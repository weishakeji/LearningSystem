using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
namespace Song.Site
{
    /// <summary>
    /// Notices 的摘要说明
    /// </summary>
    public class Notices : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {          
            //通知公告
            Tag noTag = this.Document.GetChildTagById("notice");
            if (noTag != null)
            {
                //每页多少条记录
                int size = int.Parse(noTag.Attributes.GetValue("size", "10"));                
                //简介的输出长度
                int introlen = int.Parse(noTag.Attributes.GetValue("introlen", "200"));
                int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                Song.Entities.Notice[] notice = Business.Do<INotice>().GetPager(Organ.Org_ID, true, "", size, index, out sum);
                foreach (Song.Entities.Notice no in notice)
                {
                    if (!string.IsNullOrWhiteSpace(no.No_Context))
                        no.No_Context = ReplaceHtmlTag(no.No_Context, introlen);                  
                }
                this.Document.SetValue("notice", notice);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)size);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
            }
            //资讯列表
            Tag newsTag = this.Document.GetChildTagById("newslist");
            if (newsTag != null)
            {
                int newsCount = int.Parse(newsTag.Attributes.GetValue("count", "10"));
                int columns = int.Parse(newsTag.Attributes.GetValue("columns", "-1"));
                Song.Entities.Article[] news = Business.Do<IContents>().ArticleCount(Organ.Org_ID, columns, newsCount, null);
                this.Document.SetValue("newslist", news);
            }            
        }
        /// <summary>
        /// 去除HTML标签，并返回指定长度
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            strText = HTML.ClearTag(strText);
            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }
}