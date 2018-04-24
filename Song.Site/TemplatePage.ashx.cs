using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VTemplate.Engine;
using System.Text;

namespace Song.Site
{
    /// <summary>
    /// TemplatePage 的摘要说明
    /// </summary>
    public class TemplatePage : BasePage
    {
        protected override void InitPageTemplate(HttpContext context)
        {
            
        }
        public override void LoadCurrentTemplate()
        {
            //是否是手机端网页
            string filePath = this.Request.Url.AbsolutePath;
            bool isMobi = isMobilePage(out filePath);
            //if (isMobi) filePath = filePath.Substring(prefix.Length);

            //取模板对象
            WeiSha.Common.Templates.TemplateBank curr = isMobi ?
                WeiSha.Common.Template.ForMobile.SetCurrent(this.Organ.Org_TemplateMobi)
                : WeiSha.Common.Template.ForWeb.SetCurrent(this.Organ.Org_Template);
            if (curr == null) throw new Exception("没有任何模板可用！");
            //是否是公共页面
            if (curr.Config.Public == null) throw new Exception("未找到公共模板库！");
            bool isPublic = curr.Config.Public.PageExists(filePath);
            if (isPublic) curr = curr.Config.Public;
            //当前模板的所在路径
            string tmFile = curr.Path.Physics + filePath + ".htm";
            //装载模板
            this.Document = null;
            if (!System.IO.File.Exists(tmFile))
            {
                tmFile = WeiSha.Common.Template.ForWeb.Default.Path.Physics + filePath + ".htm";
                if (!System.IO.File.Exists(tmFile))
                {
                    tmFile = curr.Path.Physics + "Notfound.htm";
                }
                this.Document = new TemplateDocument(tmFile, Encoding.UTF8, this.DocumentConfig);
            }
            else
            {
                this.Document = TemplateDocument.FromFileCache(tmFile, Encoding.UTF8, this.DocumentConfig);
            }
        }
    }
}