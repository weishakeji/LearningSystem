using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using WeiSha.WebControl;
using System.Drawing;
using System.Reflection;
using System.Xml.Serialization;
using Spring.Globalization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Xml;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Song.Site.Manage
{
    public partial class Tester : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            string txt = System.IO.File.ReadAllText(this.MapPath("~/Templates/Web/NetSchool/Course.htm"));

            txt = _replacePath(txt, "body|link|script|img");


        }
        /// <summary>
        /// 将模板页中的路径处理成实际需要的路径
        /// </summary>
        /// <param name="text">模板Html内容</param>
        /// <param name="tmFile">模板文件，即宿主页面要调用的html模板页</param>
        /// <param name="tags">要操作的html标签</param>
        /// <returns></returns>
        public static string _replacePath(string text, string tags)
        {           
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase;
            //将超链接处理为相对于模板页的路径
            string linkExpr = @"<(" + tags + @")[^>]+>";
            foreach (Match match in new Regex(linkExpr, options).Matches(text))
            {
                //html标签
                string tagName = match.Groups[1].Value.Trim();
                string tagContent = match.Groups[0].Value.Trim();
                
                string expr = @"(?<=\s+)(?<key>path|href|src|error|action|background[^=""']*)=([""'])?(?<value>[^'"">]*)\1?";
                foreach (Match m in new Regex(expr, options).Matches(tagContent))
                {
                    string val = m.Groups["value"].Value.Trim();
                    string key = m.Groups["key"].Value.Trim();
                    //外网链接不处理，如Http://开头的超链接
                    if (new Regex(@"[a-zA-z]+://[^\s]*", RegexOptions.Singleline).IsMatch(val))
                        continue;
                    //根路径，参数，#号，不处理
                    if (new Regex(@"^[{\\\/\#]").IsMatch(val))
                        continue;
                    //base64图片，参数，data:image开头，不处理
                    if (new Regex(@"^data:image").IsMatch(val))
                        continue;
                    //base64图片，参数，data:image开头，不处理
                    if (new Regex(@"^javascript:").IsMatch(val))
                        continue;
                    //将超链接转换为基于当前模板页的相对路径
                    //link = RelativePath(page, tmPath + link);
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
