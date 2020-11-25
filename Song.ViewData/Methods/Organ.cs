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
    public class Organ : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 通过机构id获取机构信息
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构实体</returns>
        public Song.Entities.Organization ForID(int id)
        {
            return _trans(Business.Do<IOrganization>().OrganSingle(id));
        }
        /// <summary>
        /// 获取所有可用的机构
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Organization[] All()
        {
            Song.Entities.Organization[] orgs = Business.Do<IOrganization>().OrganAll(true, -1);
            for (int i = 0; i < orgs.Length; i++)
            {
                orgs[i] = _trans(orgs[i]);
            }
            return orgs;
        }
        /// <summary>
        /// 当前机构
        /// </summary>
        /// <returns></returns>
        public Song.Entities.Organization Current()
        {
            return _trans(Business.Do<IOrganization>().OrganCurrent());
        }
        #region 私有方法
        /// <summary>
        /// 处理机构对外展示的信息
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        private Song.Entities.Organization _trans(Song.Entities.Organization org)
        {
            if (org == null) return org;
            org = (Song.Entities.Organization)org.Clone();
            org.Org_Logo = WeiSha.Common.Upload.Get["Org"].Virtual + org.Org_Logo;
            return org;
        }
        #endregion
    }
}
