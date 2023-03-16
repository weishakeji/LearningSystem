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
            if (old == null) throw new Exception("Not found entity for ThirdpartyLogin！");

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
        #region QQ登录
        /// <summary>
        /// 获取QQ登录的配置信息
        /// </summary>
        /// <returns></returns>
        public JObject QQ()
        {
            JObject jo = new JObject();
            //qq登录
            jo.Add("LoginIsUse", Business.Do<ISystemPara>()["QQLoginIsUse"].Boolean ?? true);
            //是否允许qq直接注册登录
            jo.Add("DirectIs", Business.Do<ISystemPara>()["QQDirectIs"].Boolean ?? true);
            jo.Add("APPID", Business.Do<ISystemPara>()["QQAPPID"].String);
            jo.Add("APPKey", Business.Do<ISystemPara>()["QQAPPKey"].String);
            jo.Add("Returl", Business.Do<ISystemPara>()["QQReturl"].String);
            return jo;
        }
        /// <summary>
        /// 保存QQ登录的配置信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool QQUpdate(JObject data)
        {
            IEnumerable<JProperty> properties = data.Properties();
            foreach (JProperty item in properties)
            {
                // item.Name 为 键
                // item.Value 为 值
                //if (item.Name == "QQLoginIsUse")
                //    Business.Do<ISystemPara>().Save("QQLoginIsUse", item.Value.ToString(), false);
                //if (item.Name == "QQDirectIs")
                //    Business.Do<ISystemPara>().Save("QQDirectIs", item.Value.ToString(), false);

                Business.Do<ISystemPara>().Save("QQ"+item.Name, item.Value.ToString(), false);             
               
            }
            Business.Do<ISystemPara>().Refresh();
            return true;
        }
        #endregion

        #region 微信登录
        /// <summary>
        /// 获取Weixin登录的配置信息
        /// </summary>
        /// <returns></returns>
        public JObject Weixin()
        {
            JObject jo = new JObject();
            //微信登录
            jo.Add("LoginIsUse", Business.Do<ISystemPara>()["WeixinLoginIsUse"].Boolean ?? true);
            //是否允许微信直接注册登录
            jo.Add("DirectIs", Business.Do<ISystemPara>()["WeixinDirectIs"].Boolean ?? true);

            //微信开放平台设置
            jo.Add("APPID", Business.Do<ISystemPara>()["WeixinAPPID"].String);
            jo.Add("Secret", Business.Do<ISystemPara>()["WeixinSecret"].String);     
            jo.Add("Returl", Business.Do<ISystemPara>()["WeixinReturl"].String);

            //微信公众号设置
            jo.Add("pubAPPID", Business.Do<ISystemPara>()["WeixinpubAPPID"].String);
            jo.Add("pubSecret", Business.Do<ISystemPara>()["WeixinpubSecret"].String);
            jo.Add("pubReturl", Business.Do<ISystemPara>()["WeixinpubReturl"].String);
            return jo;
        }
        /// <summary>
        /// 保存Weixin登录的配置信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool WeixinUpdate(JObject data)
        {
            IEnumerable<JProperty> properties = data.Properties();
            foreach (JProperty item in properties)
            {
                Business.Do<ISystemPara>().Save("Weixin" + item.Name, item.Value.ToString(), false);
            }
            Business.Do<ISystemPara>().Refresh();
            return true;
        }
        #endregion

        #region 金碟云之家轻应用登录
        /// <summary>
        /// 获取金碟云之家登录的配置信息
        /// </summary>
        /// <returns></returns>
        public JObject Yunzhijia()
        {
            JObject jo = new JObject();
            //
            jo.Add("LoginIsuse", Business.Do<ISystemPara>()["YunzhijiaLoginIsuse"].Boolean ?? true);
            jo.Add("Appid", Business.Do<ISystemPara>()["YunzhijiaAppid"].String);
            jo.Add("AppSecret", Business.Do<ISystemPara>()["YunzhijiaAppSecret"].String);
            jo.Add("Domain", Business.Do<ISystemPara>()["YunzhijiaDomain"].String);
            jo.Add("Acc", Business.Do<ISystemPara>()["YunzhijiaAcc"].String);        

            return jo;
        }
        /// <summary>
        /// 保存金碟云之家登录的配置信息
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool YunzhijiaUpdate(JObject data)
        {
            IEnumerable<JProperty> properties = data.Properties();
            foreach (JProperty item in properties)
            {
                Business.Do<ISystemPara>().Save("Yunzhijia" + item.Name, item.Value.ToString(), false);
            }
            Business.Do<ISystemPara>().Refresh();
            return true;
        }
        #endregion

    }
}
