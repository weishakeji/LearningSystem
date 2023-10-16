using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;



namespace Song.ServiceImpls
{
    public class TemplateCom : ITemplate
    {
        /// <summary>
        /// 所有Web模版
        /// </summary>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank[] WebTemplates()
        {
            return WeiSha.Core.Template.ForWeb.Items;
        }
        /// <summary>
        /// 所有手机模版
        /// </summary>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank[] MobiTemplates()
        {
            return WeiSha.Core.Template.ForMobile.Items;
        }
        /// <summary>
        /// 机构的当前web模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank WebCurrTemplate(int orgid)
        {
            if (orgid < 1) return this.WebCurrTemplate();
            Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) return this.WebCurrTemplate();
            return this.GetWebTemplate(org.Org_Template);   
        }
        public WeiSha.Core.Templates.TemplateBank WebCurrTemplate()
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            return this.GetWebTemplate(org.Org_Template); 
        }
        /// <summary>
        /// 机构的当前手机模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank MobiCurrTemplate(int orgid)
        {
            if (orgid < 1) return this.MobiCurrTemplate();
            Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) return this.MobiCurrTemplate();
            return this.GetMobiTemplate(org.Org_TemplateMobi);  
        }
        public WeiSha.Core.Templates.TemplateBank MobiCurrTemplate()
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            return this.GetMobiTemplate(org.Org_TemplateMobi); 
        }
        /// <summary>
        /// 设置当前web模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank SetWebCurr(int orgid, string tag)
        {
            Gateway.Default.Update<Organization>(new Field[] { Organization._.Org_Template }, new object[] { tag },
                Organization._.Org_ID == orgid);
            this.SetPlateOrganInfo();
            return this.GetWebTemplate(tag);
        }
        /// <summary>
        /// 设置当前手机模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank SetMobiCurr(int orgid, string tag)
        {
            Gateway.Default.Update<Organization>(new Field[] { Organization._.Org_TemplateMobi }, new object[] { tag },
              Organization._.Org_ID == orgid);
            this.SetPlateOrganInfo();
            return this.GetMobiTemplate(tag);
        }
        /// <summary>
        /// 通过模版标识获取web模版
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank GetWebTemplate(string tag)
        {
            return WeiSha.Core.Template.ForWeb.GetTemplate(tag);
        }
        /// <summary>
        /// 通过模版标识获取手机模版
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank GetMobiTemplate(string tag)
        {
            return WeiSha.Core.Template.ForMobile.GetTemplate(tag);
        }
        /// <summary>
        /// 更改模版信息
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public WeiSha.Core.Templates.TemplateBank Save(WeiSha.Core.Templates.TemplateBank tmp)
        {
            tmp.Save();
            return tmp;
        }
        /// <summary>
        /// 设置平台信息到核心库的模版控制
        /// </summary>
        public void SetPlateOrganInfo()
        {            
            WeiSha.Core.PlateOrganInfo.Clear();
            try
            {
                List<Song.Entities.Organization> org = WeiSha.Core.Business.Do<IOrganization>().OrganAll(true, -1, string.Empty);
                foreach (Song.Entities.Organization o in org)
                {
                    WeiSha.Core.PlateOrganInfo info = new WeiSha.Core.PlateOrganInfo();
                    info.Domain = o.Org_TwoDomain;
                    info.WebTemplateName = o.Org_Template;
                    info.MobiTemplateName = o.Org_TemplateMobi;
                    info.IsDefault = o.Org_IsDefault;
                    info.IsRoot = o.Org_IsRoot;
                    info.Org_ExtraWeb = o.Org_ExtraWeb;
                    info.Org_ExtraMobi = o.Org_ExtraMobi;
                    WeiSha.Core.PlateOrganInfo.Add(info);
                }
            }
            catch
            {

            }
        }
    }
}
