using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using Song.Entities;
namespace Song.Site
{
    /// <summary>
    /// 留言板列表页
    /// </summary>
    public class MessageBoards : BasePage
    {
        //当前课程id
        private int couid = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        //提交的信息
        private string msg = WeiSha.Common.Request.Form["msg"].String;
        protected override void InitPageTemplate(HttpContext context)
        {
            this.Document.SetValue("couid", couid);
            if (!string.IsNullOrWhiteSpace(msg))
            {
                Song.Entities.Accounts student = this.Account;
                if (student != null)
                {
                    Song.Entities.MessageBoard mb = new MessageBoard();
                    mb.Ac_ID = student.Ac_ID;
                    mb.Ac_Name = student.Ac_Name;
                    mb.Ac_Photo = student.Ac_Photo;
                    mb.Mb_Content = msg;
                    mb.Cou_ID = couid;
                    mb.Mb_IsTheme = true;
                    Business.Do<IMessageBoard>().ThemeAdd(mb);
                }
            }
            //留言板列表
            Tag noTag = this.Document.GetChildTagById("MessageBoard");
            if (noTag != null)
            {
                //每页多少条记录
                int size = int.Parse(noTag.Attributes.GetValue("size", "10"));
                //简介的输出长度
                int introlen = int.Parse(noTag.Attributes.GetValue("introlen", "200"));
                int index = WeiSha.Common.Request.QueryString["index"].Int32 ?? 1;
                int sum = 0;
                Song.Entities.MessageBoard[] msgBoards = Business.Do<IMessageBoard>().ThemePager(Organ.Org_ID, couid, false, true, "", size, index, out sum);
                foreach (Song.Entities.MessageBoard no in msgBoards)
                {
                    if (string.IsNullOrWhiteSpace(no.Mb_Title))
                        no.Mb_Title = ReplaceHtmlTag(no.Mb_Title, introlen);
                }
                this.Document.SetValue("msgBoards", msgBoards);
                //总页数
                int pageSum = (int)Math.Ceiling((double)sum / (double)size);
                int[] pageAmount = new int[pageSum];
                for (int i = 0; i < pageAmount.Length; i++)
                    pageAmount[i] = i + 1;
                this.Document.SetValue("pageAmount", pageAmount);
                this.Document.SetValue("pageIndex", index);
                this.Document.SetValue("pageSize", size);
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
            if (string.IsNullOrWhiteSpace(html)) return html;
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
       
    }
}