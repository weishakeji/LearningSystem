using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using VTemplate.Engine;
using System.Text.RegularExpressions;
namespace Song.Site.Mobile
{
    /// <summary>
    /// 新闻列表页
    /// </summary>
    public class NewsList : BasePage
    {
        int colid = WeiSha.Common.Request.QueryString["colid"].Int32 ?? -1;  //栏目id
        protected override void InitPageTemplate(HttpContext context)
        {
            if (Request.ServerVariables["REQUEST_METHOD"] == "GET")
            {
                Song.Entities.Columns col = Business.Do<IColumns>().Single(colid);
                this.Document.Variables.SetValue("column", col);
            }
            //此页面的ajax提交，全部采用了POST方式
            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {
                int size = WeiSha.Common.Request.Form["size"].Int32 ?? 10;  //每页多少条
                int index = WeiSha.Common.Request.Form["index"].Int32 ?? 1;  //第几页                
                int sumcount = 0;
                Song.Entities.Article[] arts = Business.Do<IContents>().ArticlePager(Organ.Org_ID, colid, true, "", size, index, out sumcount);
                string json = "{\"size\":" + size + ",\"index\":" + index + ",\"sumcount\":" + sumcount + ",";
                json += "\"items\":[";
                for (int i = 0; i < arts.Length; i++)
                {
                    Song.Entities.Article art = arts[i];
                    //处理详情
                    art.Art_Details = "";
                    //art.Art_Details = Regex.Replace(art.Art_Details, @"\s{1,}", " ", RegexOptions.Singleline);
                    //art.Art_Details = HttpUtility.UrlEncode(art.Art_Details);
                    art.Art_Title = art.Art_Title.Replace("\"", "&quot;");
                    art.Art_Title = art.Art_Title;
                    //
                    art.Art_Logo = Upload.Get["News"].Virtual + art.Art_Logo;                    
                    if (string.IsNullOrWhiteSpace(art.Art_Intro))
                        art.Art_Intro = ReplaceHtmlTag(art.Art_Details, 50);                   
                    json += art.ToJson() + ",";                   
                }
                if (json.EndsWith(",")) json = json.Substring(0, json.Length - 1);
                json += "]}";
                Response.Write(json);
                Response.End();
            }
        }
        /// <summary>
        /// 去除HTML标签，并返回指定长度
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }
}