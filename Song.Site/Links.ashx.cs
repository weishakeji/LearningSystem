using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.Site
{
    /// <summary>
    /// Links 的摘要说明
    /// </summary>
    public class Links : BasePage
    {

        protected override void InitPageTemplate(HttpContext context)
        {
            //友情链接分类
            Song.Entities.LinksSort[] sorts = Business.Do<ILinks>().GetSortAll(Organ.Org_ID, true, true);
            this.Document.Variables.SetValue("sorts", sorts);
            //根据分类获取友情链接
            this.Document.RegisterGlobalFunction(this.getLinks);
            if (sorts.Length < 1)
            {
                Song.Entities.Links[] links = Business.Do<ILinks>().GetLinks(Organ.Org_ID, 0, true, true, -1);
                this.Document.Variables.SetValue("links", links);
            }
            //平台入驻的机构
            Song.Entities.Organization[] orgs = Business.Do<IOrganization>().OrganCount(true, true, -1, 0);
            this.Document.Variables.SetValue("orgs", orgs);
            //根域名
            this.Document.Variables.SetValue("domain", WeiSha.Common.Request.Domain.MainName);
            
        }
        /// <summary>
        /// 友情链接
        /// </summary>
        /// <returns></returns>
        private Song.Entities.Links[] getLinks(object[] id)
        {
            int sortid = 0;
            if (id.Length > 0 && id[0] is int)
                sortid = Convert.ToInt32(id[0]);
            Song.Entities.Links[] links = Business.Do<ILinks>().GetLinks(Organ.Org_ID, sortid, true, true, -1);
            foreach (Song.Entities.Links l in links)
            {
                l.Lk_Logo = Upload.Get["Links"].Virtual + l.Lk_Logo;
            }
            return links;
        }
    }
}