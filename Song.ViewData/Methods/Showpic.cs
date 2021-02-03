using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 轮换图片
    /// </summary>
    public class Showpic : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 手机端轮换图片
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet]        
        public Song.Entities.ShowPicture[] Mobi(int orgid)
        {
            Song.Entities.ShowPicture[] shp = Business.Do<IStyle>().ShowPicAll(true, "mobi", orgid);

            foreach (Song.Entities.ShowPicture s in shp)
                s.Shp_File = Upload.Get["ShowPic"].Virtual + s.Shp_File;
            return shp;
        }
        /// <summary>
        /// 电脑端轮换图片
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.ShowPicture[] Web(int orgid)
        {
            Song.Entities.ShowPicture[] shp = Business.Do<IStyle>().ShowPicAll(true, "web", orgid);

            foreach (Song.Entities.ShowPicture s in shp)
                s.Shp_File = Upload.Get["ShowPic"].Virtual + s.Shp_File;
            return shp;
        }
    }
}
