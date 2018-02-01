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
    /// 新闻频道页
    /// </summary>
    public class News : BasePage
    {
        //新闻栏目Id
        private int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //获取所有栏目
            Song.Entities.Columns[] cols = Business.Do<IColumns>().ColumnCount(this.Organ.Org_ID, 0, "news", true, -1);
            this.Document.SetValue("cols", cols);
            //
            this.Document.SetValue("colid", colid);
            //当前栏目
            Song.Entities.Columns col = Business.Do<IColumns>().Single(colid);
            this.Document.SetValue("col", col);
            //新闻列表
            Tag newsTag = this.Document.GetChildTagById("newslist");
            if (newsTag != null)
            {
                //每页多少条记录
                int newsSize = int.Parse(newsTag.Attributes.GetValue("size", "10"));
                //新闻栏目
                int columns = int.Parse(newsTag.Attributes.GetValue("columns", "-1"));
                columns = columns <= 0 ? colid : columns;
                //简介的输出长度
                int introlen = int.Parse(newsTag.Attributes.GetValue("introlen", "200"));
                int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                Song.Entities.Article[] news = Business.Do<IContents>().ArticlePager(Organ.Org_ID, columns, true, "", newsSize, index, out sum);
                foreach (Song.Entities.Article art in news)
                {
                    art.Art_Logo = Upload.Get["News"].Virtual + art.Art_Logo;
                    if (!string.IsNullOrWhiteSpace(art.Art_Intro))
                    {
                        art.Art_Intro = ReplaceHtmlTag(art.Art_Intro, introlen);
                    }
                    if (art.Art_IsImg) art.Art_Intro = ReplaceHtmlTag(art.Art_Details, introlen);
                }
                this.Document.SetValue("newslist", news);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)newsSize);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
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

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        /// <summary>
        /// 获取新闻栏目
        /// </summary>
        /// <param name="para">参数一个，取几个栏目</param>
        /// <returns></returns>
        protected Song.Entities.Columns[] getColumns(object[] para)
        {
            int count = 0;
            if (para.Length > 0 && para[0] is int)
                int.TryParse(para[0].ToString(), out count);
            Song.Entities.Columns[] sts = Business.Do<IColumns>().ColumnCount(Organ.Org_ID, "News", true, count);
            return sts;
        }
    }
}