using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 第三方登录
    /// </summary>
    [HttpPut, HttpGet]
    public class OtherLogin : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 通过tag获取登录配置项
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ThirdpartyLogin GetObject(string tag)
        {
            return Business.Do<IThirdpartyLogin>().GetSingle(tag);
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="isuse"></param>
        /// <returns></returns>
        public ThirdpartyLogin[] GetAll(bool? isuse)
        {
            return Business.Do<IThirdpartyLogin>().GetAll(isuse);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public ThirdpartyLogin Update(ThirdpartyLogin entity)
        {
            Song.Entities.ThirdpartyLogin old = Business.Do<IThirdpartyLogin>().GetSingle(entity.Tl_Tag);
            if (old == null) old = new ThirdpartyLogin();
            if (!string.IsNullOrWhiteSpace(entity.Tl_Returl) && entity.Tl_Returl.EndsWith("/"))
                entity.Tl_Returl = entity.Tl_Returl.Substring(0, entity.Tl_Returl.Length - 1);
            if (!string.IsNullOrWhiteSpace(entity.Tl_Domain) && entity.Tl_Domain.EndsWith("/"))
                entity.Tl_Domain = entity.Tl_Domain.Substring(0, entity.Tl_Domain.Length - 1);
            old.Copy<Song.Entities.ThirdpartyLogin>(entity);
            Business.Do<IThirdpartyLogin>().Save(old);
          
            return old;
        }
        /// <summary>
        /// 修改使用状态
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="isue"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ModifyUse(string tag,bool isue)
        {
            if (string.IsNullOrWhiteSpace(tag)) return false;
            ThirdpartyLogin entity= Business.Do<IThirdpartyLogin>().GetSingle(tag);
            if (entity == null)
            {      
                entity = new ThirdpartyLogin();
                entity.Tl_Tag = tag;
            }
            entity.Tl_IsUse = isue;
            Business.Do<IThirdpartyLogin>().Save(entity);
            return true;
        }
        /// <summary>
        /// 获取远程页面的返回
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpPost,HttpGet]
        public string HttpRequest(string url)
        {
            Letter letter = this.Letter;
            if (letter.HTTP_METHOD.Equals("post", StringComparison.OrdinalIgnoreCase))
                return WeiSha.Core.Request.HttpPost(url);
            return  WeiSha.Core.Request.HttpGet(url);
        }

    }
}
