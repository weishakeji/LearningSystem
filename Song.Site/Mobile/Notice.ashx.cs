using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Mobile
{
    /// <summary>
    /// Notice 的摘要说明
    /// </summary>
    public class Notice : BasePage
    {
        private int id = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        protected override void InitPageTemplate(HttpContext context)
        {
            //通知
            Song.Entities.Notice notice = Business.Do<INotice>().NoticeSingle(id);
            if (notice == null) return;
            //notice.No_Context = ReplaceHtmlTag(notice.No_Context);
            if (notice.No_IsShow && ((DateTime)notice.No_StartTime) < DateTime.Now)
                this.Document.Variables.SetValue("notice", notice);
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
    }
}