using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;



namespace Song.ServiceImpls
{
    public class TemplateCom : ITemplate
    {
        /// <summary>
        /// 所有Web模板
        /// </summary>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank[] WebTemplates()
        {
            return WeiSha.Common.Template.ForWeb.Items;
        }
        /// <summary>
        /// 所有手机模板
        /// </summary>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank[] MobiTemplates()
        {
            return WeiSha.Common.Template.ForMobile.Items;
        }
        /// <summary>
        /// 机构的当前web模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank WebCurrTemplate(int orgid)
        {
            if (orgid < 1) return this.WebCurrTemplate();
            Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) return this.WebCurrTemplate();
            return this.GetWebTemplate(org.Org_Template);   
        }
        public WeiSha.Common.Templates.TemplateBank WebCurrTemplate()
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            return this.GetWebTemplate(org.Org_Template); 
        }
        /// <summary>
        /// 机构的当前手机模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank MobiCurrTemplate(int orgid)
        {
            if (orgid < 1) return this.MobiCurrTemplate();
            Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) return this.MobiCurrTemplate();
            return this.GetMobiTemplate(org.Org_TemplateMobi);  
        }
        public WeiSha.Common.Templates.TemplateBank MobiCurrTemplate()
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            return this.GetMobiTemplate(org.Org_TemplateMobi); 
        }
        /// <summary>
        /// 设置当前web模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank SetWebCurr(int orgid, string tag)
        {
            Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == orgid).ToFirst<Organization>();
            if (org != null) org.Org_Template = tag;
            Business.Do<IOrganization>().OrganSave(org);
            return this.GetWebTemplate(org.Org_Template); 
        }
        /// <summary>
        /// 设置当前手机模板
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank SetMobiCurr(int orgid, string tag)
        {
            Organization org = Gateway.Default.From<Organization>().Where(Organization._.Org_ID == orgid).ToFirst<Organization>();
            if (org != null) org.Org_TemplateMobi = tag;
            Business.Do<IOrganization>().OrganSave(org);
            return this.GetMobiTemplate(org.Org_TemplateMobi); 
        }
        /// <summary>
        /// 通过模板标识获取web模板
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank GetWebTemplate(string tag)
        {
            return WeiSha.Common.Template.ForWeb.GetTemplate(tag);
        }
        /// <summary>
        /// 通过模板标识获取手机模板
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank GetMobiTemplate(string tag)
        {
            return WeiSha.Common.Template.ForMobile.GetTemplate(tag);
        }
        /// <summary>
        /// 更改模板信息
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public WeiSha.Common.Templates.TemplateBank Save(WeiSha.Common.Templates.TemplateBank tmp)
        {
            tmp.Save();
            return tmp;
        }
        
    }
}
