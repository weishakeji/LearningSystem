using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 第三方登录管理
    /// </summary>
    public interface IThirdpartyLogin : WeiSha.Core.IBusinessInterface
    {
        /// <summary>
        /// 通过tag标签获取登录配置项
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        ThirdpartyLogin GetSingle(string tag);
        /// <summary>
        /// 通过id登录配置项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ThirdpartyLogin GetSingle(int id);
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        void Save(ThirdpartyLogin entity);
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="isuse"></param>
        /// <returns></returns>
        List<ThirdpartyLogin> GetAll(bool? isuse);
    }
}
