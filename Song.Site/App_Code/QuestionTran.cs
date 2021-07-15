using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Song.Site
{
    public class QuestionTran
    {
        public static string TransformPath(string text)
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
                    //外网链接不处理，如Http://开头的超链接
                    if (new Regex(@"[a-zA-z]+://[^\s]*", RegexOptions.Singleline).IsMatch(val)) continue;
                    //根路径，参数，#号，不处理
                    if (new Regex(@"^[{\\\/\#]").IsMatch(val)) continue;
                    //base64图片，参数，data:image开头，不处理
                    if (new Regex(@"^data:image").IsMatch(val)) continue;
                    //脚本不处理
                    if (new Regex(@"^javascript:").IsMatch(val)) continue;                  
                  
                    //val = RelativePath(tmPath, val);
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