using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Common;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 机构管理
    /// </summary>
    [HttpGet]
    public class Organ : IViewAPI
    {
        /// <summary>
        /// 通过机构id获取机构信息
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构实体</returns>
        public Song.Entities.Organization ForID(int id)
        {
            return Business.Do<IOrganization>().OrganSingle(id);
        }
        /// <summary>
        /// 获取所有可用的机构
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Organization[] All()
        {
            return Business.Do<IOrganization>().OrganAll(true, -1);
        }
        /// <summary>
        /// 当前机构
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Organization Current()
        {
            return Business.Do<IOrganization>().OrganCurrent();
        }
    }
}
