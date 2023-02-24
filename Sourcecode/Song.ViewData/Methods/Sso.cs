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
    /// 单点登录的管理
    /// </summary>
    public class Sso : ViewMethod, IViewAPI
    {
        #region 增删改查
        /// <summary>
        /// 所有配置项
        /// </summary>
        /// <param name="use"></param>
        /// <returns></returns>
        [HttpGet]
        public SingleSignOn[] All(bool? use)
        {
            return Business.Do<ISSO>().GetAll(use);
        }
        /// <summary>
        /// 获取单一实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public SingleSignOn ForID(int id)
        {
            return Business.Do<ISSO>().GetSingle(id);
        }
        /// <summary>
        /// 通过APPID获取登录接口对象,用于前端登录时使用，如果登录接口被禁用，则返回null
        /// </summary>
        /// <param name="appid">链接分类的id</param>
        /// <returns>如果登录接口被禁用，则返回null</returns>
        public SingleSignOn ForAPPID(string appid)
        {
            SingleSignOn sso= Business.Do<ISSO>().GetSingle(appid);
            if (sso == null) return null;
            return sso.SSO_IsUse ? sso : null;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Exist(string name, int id)
        {
            return Business.Do<ISSO>().Exist(name,id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<ISSO>().Delete(idval);               
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Add(SingleSignOn entity)
        {
            //域名全部转小写
            if (!string.IsNullOrWhiteSpace(entity.SSO_Domain))
                entity.SSO_Domain = entity.SSO_Domain.ToLower();
            Business.Do<ISSO>().Add(entity);
            return true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Modify(SingleSignOn entity)
        {
            Song.Entities.SingleSignOn old = Business.Do<ISSO>().GetSingle(entity.SSO_ID);
            if (old == null) throw new Exception("Not found entity for SingleSignOn！");
            //域名全部转小写
            if (!string.IsNullOrWhiteSpace(entity.SSO_Domain))
                entity.SSO_Domain = entity.SSO_Domain.ToLower();
            old.Copy<Song.Entities.SingleSignOn>(entity);
            Business.Do<ISSO>().Save(old);
            return true;
        }
        #endregion
    }
}
