using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;


namespace Song.ViewData.Methods
{

    /// <summary>
    /// 模版管理
    /// </summary>
    [HttpPut, HttpGet]
    public class Template : ViewMethod, IViewAPI
    {
        /// <summary>
        /// web端模版
        /// </summary>
        /// <returns></returns>
        [Cache]
        public WeiSha.Core.Templates.TemplateBank[] WebTemplates()
        {
            return Business.Do<ITemplate>().WebTemplates();
        }
        /// <summary>
        /// 手机端模版
        /// </summary>
        /// <returns></returns>
        [Cache]
        public WeiSha.Core.Templates.TemplateBank[] MobiTemplates()
        {
            return Business.Do<ITemplate>().MobiTemplates();
        }
        /// <summary>
        /// 当前模板
        /// </summary>
        /// <param name="device">前设备名称，如web,mobi</param>
        /// <returns></returns>
        [Cache(Expires = 60)]
        public string CurrentTemplate(string device)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if ("mobi".Equals(device, StringComparison.OrdinalIgnoreCase)) return org.Org_TemplateMobi;
            return org.Org_Template;
        }
        /// <summary>
        /// 设置当前机构的模版库
        /// </summary>
        /// <param name="device">前设备名称，如web,mobi</param>
        /// <param name="tag">模版库的标签，即模版库所在的文件夹名称</param>
        /// <returns>成功则返回true</returns>
        [HttpPost][HttpGet(Ignore =true)]
        public WeiSha.Core.Templates.TemplateBank SetCurrTemplate(string device, string tag)
        {
            try
            {
                WeiSha.Core.Templates.TemplateBank bank = null;
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if ("web".Equals(device, StringComparison.OrdinalIgnoreCase))
                    bank = Business.Do<Song.ServiceInterfaces.ITemplate>().SetWebCurr(org.Org_ID, tag);
                if ("mobi".Equals(device, StringComparison.OrdinalIgnoreCase))
                    bank = Business.Do<Song.ServiceInterfaces.ITemplate>().SetMobiCurr(org.Org_ID, tag);
                Business.Do<IOrganization>().OrganBuildCache();
                return bank;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
