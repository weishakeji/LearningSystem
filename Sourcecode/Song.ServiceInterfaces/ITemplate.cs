using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;
using WeiSha.Core;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 模版管理
    /// </summary>
    public interface ITemplate : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 所有Web模版
        /// </summary>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank[] WebTemplates();
        /// <summary>
        /// 所有手机模版
        /// </summary>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank[] MobiTemplates();
        /// <summary>
        /// 机构的当前web模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank WebCurrTemplate(int orgid);
        WeiSha.Core.Templates.TemplateBank WebCurrTemplate();
        /// <summary>
        /// 机构的当前手机模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank MobiCurrTemplate(int orgid);
        WeiSha.Core.Templates.TemplateBank MobiCurrTemplate();
        /// <summary>
        /// 设置当前web模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank SetWebCurr(int orgid, string tag);
        /// <summary>
        /// 设置当前手机模版
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank SetMobiCurr(int orgid, string tag);
        /// <summary>
        /// 通过模版标识获取web模版
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank GetWebTemplate(string tag);
        /// <summary>
        /// 通过模版标识获取手机模版
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank GetMobiTemplate(string tag);
        /// <summary>
        /// 更改模版信息
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        WeiSha.Core.Templates.TemplateBank Save(WeiSha.Core.Templates.TemplateBank tmp);
        /// <summary>
        /// 设置平台信息到核心库的模版控制
        /// </summary>
        void SetPlateOrganInfo();
    }
}
