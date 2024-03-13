using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 是否清理接口参数数据中的html标签，默认全部清理
    /// </summary>
    public class HtmlClearAttribute : WebAttribute
    {

        /// <summary>
        /// 不用理清html的参数项,多上参数用逗号分隔
        /// </summary>
        public string Not { get; set; }
        public HtmlClearAttribute(string not)
        {
            Not = not;
        }
        public HtmlClearAttribute()
        {
        }
        /// <summary>
        /// 验证是否满足特性的限定
        /// </summary>
        /// <param name="method">执行的方法</param>
        /// <param name="letter">客户端传来的信息</param>
        /// <returns></returns>
        public static HtmlClearAttribute Clear(MemberInfo method, Letter letter)
        {
            HtmlClearAttribute attr = null;
            attr = LoginAttribute.GetAttr<HtmlClearAttribute>(method);
            if (attr != null)
            {
                letter.Params = letter.Params.ToDictionary(x => x.Key, (x) => {
                    string[] nots = attr.Not.Split(',');
                    bool isexist = false;
                    foreach (string s in nots)
                    {
                        if (string.IsNullOrWhiteSpace(s)) continue;
                        if (s.Equals(x.Key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            isexist = true;
                            break;
                        }
                    }
                    if (isexist) return _clearScript(x.Value);
                    return _clearTag(x.Value);
                });
            }
            else
            {
                letter.Params = letter.Params.ToDictionary(x => x.Key, x => _clearTag(x.Value));
            }
            return attr;
        }
        /// <summary>
        /// 去除HTML标签，并返回指定长度
        /// </summary>
        /// <param name="html"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static string _clearTag(string html, int length = 0)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return _clearScript(strText);
        }
        /// <summary>
        /// 清除Js脚本
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string _clearScript(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            //删除脚本
            html = Regex.Replace(html, @"(\<script(.+?)\</script\>)|(\<style(.+?)\</style\>)", "", option);
            html = Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>", "", option);
            html = Regex.Replace(html, @"//\(function\(\)[\s\S]+?}\)\(\);", "", option);
            //清理掉<!DOCTYPE，主要是为了防止XXE攻击
            html = Regex.Replace(html, @"<(\s*)!DOCTYPE[^>]*>", "", option);
            html = Regex.Replace(html, @"<(\s*)!ENTITY[^>]*>", "", option);

            return html;
        }
    }
}
