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
    /// 站点导航
    /// </summary>
    [HttpGet]
    public class Navig : ViewMethod, IViewAPI
    {

        /// <summary>
        /// 手机端导航
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">类型，主菜单main，底部菜单foot</param>
        /// <returns></returns>
        //[Cache]
        public Song.Entities.Navigation[] Mobi(int orgid, string type)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            Song.Entities.Navigation[] navi = Business.Do<IStyle>().NaviAll(true, "mobi", type, orgid, 0);
            //导航图片的虚拟路径和物理路径
            string vpath = Upload.Get["Org"].Virtual;
            string hpath = Upload.Get["Org"].Physics;
            foreach (Song.Entities.Navigation s in navi)
            {
                if (!string.IsNullOrWhiteSpace(s.Nav_Logo))
                {
                    if(System.IO.File.Exists(hpath+s.Nav_Logo))
                        s.Nav_Logo = vpath + s.Nav_Logo;
                }       
               
            }
            return navi;
        }
    }
}
