using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site.Mobile
{
    /// <summary>
    /// 新闻文章页
    /// </summary>
    public class Article : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {           
            //新闻文章
            int artid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
            Song.Entities.Article art = Business.Do<IContents>().ArticleSingle(artid);
            if (art == null) return;
            if ((WeiSha.Common.Request.Cookies["article_" + art.Art_Id].Int32 ?? 0) == 0)
            {
                art.Art_Number++;
                Business.Do<IContents>().ArticleSave(art);
                context.Response.Cookies["article_" + art.Art_Id].Value = art.Art_Id.ToString();
            }
            art.Art_Logo = Upload.Get["News"].Virtual + art.Art_Logo;
            this.Document.Variables.SetValue("art", art);
            //附件
            List<Song.Entities.Accessory> acs = Business.Do<IAccessory>().GetAll(art.Art_Uid);
            foreach (Song.Entities.Accessory ac in acs)
                ac.As_FileName = Upload.Get["News"].Virtual + ac.As_FileName;
            this.Document.Variables.SetValue("artAcc", acs);
            ////当前新闻的上一条
            //Song.Entities.Article artPrev = Business.Do<IContents>().ArticlePrev(artid);
            //this.Document.Variables.SetValue("artPrev", artPrev);
            ////当前新闻的下一条
            //Song.Entities.Article artNext = Business.Do<IContents>().ArticleNext(artid);
            //this.Document.Variables.SetValue("artNext", artNext);
        }
    }
}