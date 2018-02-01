using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Song.Extend
{
    public class Html
    {
        /// <summary>
        /// 理解HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ClearHTML(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            //删除脚本
            html = Regex.Replace(html, @"<script[^>]+?>[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, @"//\(function\(\)[\s\S]+?}\)\(\);", "", RegexOptions.IgnoreCase);
            //html = html.Replace("<", "&lt;");
            //html = html.Replace(">", "&gt;");
            html = html.Replace("\r", "");
            html = html.Replace("\n", "");
            return html.Trim();
        }
        /// <summary>
        /// 清除指定的html标签
        /// </summary>
        /// <param name="html"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static string ClearHTML(string html, params string[] elements)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option=RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            foreach (string el in elements)
            {
                html = Regex.Replace(html, @"<" + el + @"[^>]*>", "", option);
                html = Regex.Replace(html, @"</" + el + @"[^>]*>", "", option);
            }           
            return html.Trim();
        }
    }
}
