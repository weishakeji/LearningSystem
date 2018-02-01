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
using System.Text.RegularExpressions;
using System.IO;

namespace Song.Site.Manage.Panel
{
    public partial class HelpEditer : Extend.CustomPage
    {
        //帮助文件
        private string helpfile = WeiSha.Common.Request.QueryString["helpfile"].String;
        //功能模块的名称
        private string name = WeiSha.Common.Request.QueryString["name"].String;
        //帮助模板
        private string template = "/manage/help/HeplTemplate.html";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tbHelpContext.Text = getBodyContext();
                lbName.Text = name;
                linkHelp.NavigateUrl = helpfile;
            }
        }
        /// <summary>
        /// 获取帮助文件的内容
        /// </summary>
        /// <returns></returns>
        private string getBodyContext()
        {
            if (helpfile == "") return "";
            string context = this.getOldHtml();
            string regTxt = @"(?<=<body>).*(?=</body>)";
            Regex re = new Regex(regTxt, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            MatchCollection mc = re.Matches(context);
            if (mc.Count > 0)
                 return mc[0].Value;
             return "";
        }
        /// <summary>
        /// 获取原有帮助文件的所有HTML内容，如果不存在，则用模板创建
        /// </summary>
        /// <returns></returns>
        private string getOldHtml()
        {
            if (helpfile == "") return "";
            string context = "";
            string helpfileHy = this.Server.MapPath(helpfile);
            if (!helpfileHy.EndsWith(".html"))
            {
                return "";
            }
            if (File.Exists(helpfileHy))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(helpfileHy))
                {
                    context = sr.ReadToEnd();
                    sr.Close();
                }
            }
            else
            {
                //如果帮助文件不存在，则取帮助模板页
                using (System.IO.StreamReader sr = new System.IO.StreamReader(Server.MapPath(template)))
                {
                    context = sr.ReadToEnd();
                    sr.Close();
                }
            }
            return context;
        }
        /// <summary>
        /// 生成帮助文档的内容
        /// </summary>
        /// <returns></returns>
        private string setHelpContext()
        {
            //替换body的内容
            string regTxt = @"(?<=<body>).*(?=</body>)";
            Regex re = new Regex(regTxt, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
            string context = re.Replace(this.getOldHtml(), this.tbHelpContext.Text);
            //替换标题
            context = new Regex("(?<=<title>).*(?=</title>)").Replace(context, name);
            return context;
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            try
            {
                string tm = this.setHelpContext();
                //写入
                string helpfileHy = this.Server.MapPath(helpfile);
                //if (!File.Exists(helpfileHy))
                //{
                //    File.Create(helpfileHy);
                //}
                using (System.IO.StreamWriter sr = new System.IO.StreamWriter(helpfileHy, false))
                {
                    sr.Write(tm);
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Message.ExceptionShow(ex);
            } 

        }
    }
}
