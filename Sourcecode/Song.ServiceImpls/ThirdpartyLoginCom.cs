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
            //tag = tag.ToLower();
            return Gateway.Default.From<ThirdpartyLogin>().Where(ThirdpartyLogin._.Tl_Tag == tag || ThirdpartyLogin._.Tl_Tag == tag.ToLower()).ToFirst<ThirdpartyLogin>();
        }
        /// <summary>
        /// 通过id登录配置项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ThirdpartyLogin GetSingle(int id)
        {
            return Gateway.Default.From<ThirdpartyLogin>().Where(ThirdpartyLogin._.Tl_ID == id).ToFirst<ThirdpartyLogin>();
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
        public List<ThirdpartyLogin> GetAll(bool? isuse)
        {
            WhereClip wc = new WhereClip();
            if (isuse != null) wc.And(ThirdpartyLogin._.Tl_IsUse == (bool)isuse);
            return Gateway.Default.From<ThirdpartyLogin>().Where(wc).OrderBy(ThirdpartyLogin._.Tl_ID.Asc).ToList<ThirdpartyLogin>();
        }
    }
}
