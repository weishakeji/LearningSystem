using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Song.Extend
{
    public class Questions
    {
        /// <summary>
        /// 处理试题的文本，用于前端显示
        /// </summary>
        /// <param name="qs"></param>
        /// <returns></returns>
        public static Song.Entities.Questions TranText(Song.Entities.Questions qs)
        {
            if (qs == null) return qs;
            qs.Qus_Title = _ClearAttr(qs.Qus_Title, new string[] {"p","span","pre","font" });
            qs.Qus_Title = Extend.Html.ClearHTML(qs.Qus_Title, "pre");
            qs.Qus_Title = _tran(qs.Qus_Title);
            qs.Qus_Explain = _ClearAttr(qs.Qus_Explain, new string[] { "p", "span", "pre", "font" });
            qs.Qus_Explain = _tran(qs.Qus_Explain);
            qs.Qus_Answer = _ClearAttr(qs.Qus_Answer, new string[] { "p", "span", "pre", "font" });
            //qs.Qus_Answer = _tranAnswer(qs.Qus_Answer);
            return qs;
        }
        public static Song.Entities.QuesAnswer TranText(Song.Entities.QuesAnswer qa)
        {
            qa.Ans_Context = _tran(qa.Ans_Context);
            return qa;
        }
        /// <summary>
        /// 试题答题项的处理
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string _tranAnswer(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            text = text.Replace("\t", "");
            //text = text.Replace("\"", "&quot;");
            text = text.Replace(" ", "&nbsp;");
            text = text.Replace("'", "&apos;");
            //text = text.Replace("<", "&lt;");
            //text = text.Replace(">", "&gt;");
            return text;
        }
        private static string _tran(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            text = text.Replace("\r", "");
            text = text.Replace("\n", "");
            text = text.Replace("\t", "");
            //text = text.Replace("\"", "&quot;");
            //text = text.Replace(" ", "&nbsp;");
            text = text.Replace("'", "&apos;");
            //text = text.Replace("<", "&lt;");
            //text = text.Replace(">", "&gt;");
            return text;
        }
        /// <summary>
        /// 清理HTML标题中的属性
        /// </summary>
        /// <param name="html"></param>
        /// <param name="tag">清除指定的标签中的属性</param>
        /// <returns></returns>
        private static string _ClearAttr(string html,string tag)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            return Regex.Replace(html, @"<("+tag+@")\s*[^><]*>", @"<$1>", 
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
        }
        private static string _ClearAttr(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            return Regex.Replace(html, @"<([a-zA-Z]+)\s*[^><]*>", @"<$1>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
        }
        private static string _ClearAttr(string html,string[] tag)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            foreach (string s in tag)
                _ClearAttr(html, s);
            return html;
        }
        /// <summary>
        /// 理解HTML标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string _ClearHTML(string html)
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
        /// 处理试题中的图片
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TransformImagePath(string text)
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase;
            //将超链接处理为相对于模版页的路径
            string linkExpr = @"<(img)[^>]+>";
            foreach (Match match in new Regex(linkExpr, options).Matches(text))
            {
                string tagName = match.Groups[1].Value.Trim();      //标签名称
                string tagContent = match.Groups[0].Value.Trim();   //标签内容
                string expr = @"(?<=\s+)(?<key>src[^=""']*)=([""'])?(?<value>[^'"">]*)\1?";
                foreach (Match m in new Regex(expr, options).Matches(tagContent))
                {
                    string key = m.Groups["key"].Value.Trim();      //属性名称
                    string val = m.Groups["value"].Value.Trim();    //属性值      
                    val = val.Replace("&apos;", "");
                    if (val.EndsWith("/")) val = val.Substring(0, val.Length - 1);
                    val = m.Groups[2].Value + "=\"" + val + "\"";
                    val = Regex.Replace(val, @"//", "/");
                    
                    tagContent = tagContent.Replace(m.Value, val);
                }
                text = text.Replace(match.Groups[0].Value.Trim(), tagContent);
            }
            return text;
        }
    }
}
