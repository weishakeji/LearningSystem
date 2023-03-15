using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Core;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;

namespace Song.ServiceImpls
{
    public class ThirdpartyLoginCom : IThirdpartyLogin
    {
        /// <summary>
        /// 通过tag标签获取登录配置项
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ThirdpartyLogin GetSingle(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) return null;
            tag = tag.ToLower();
            return Gateway.Default.From<ThirdpartyLogin>().Where(ThirdpartyLogin._.Tl_Tag == tag).ToFirst<ThirdpartyLogin>();
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        public void Save(ThirdpartyLogin entity)
        {
            Gateway.Default.Save<ThirdpartyLogin>(entity);
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="isuse"></param>
        /// <returns></returns>
        public ThirdpartyLogin[] GetAll(bool? isuse)
        {
            WhereClip wc = new WhereClip();
            if (isuse != null) wc.And(ThirdpartyLogin._.Tl_IsUse == (bool)isuse);
            return Gateway.Default.From<ThirdpartyLogin>().Where(wc).ToArray<ThirdpartyLogin>();
        }
    }
}
