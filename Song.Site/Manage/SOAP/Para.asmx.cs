using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;

namespace Song.Site.Manage.SOAP
{
    /// <summary>
    /// Para 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Para : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetPara(string key)
        {
            string value = Business.Do<ISystemPara>().GetValue(key);
            //如果当前返回值为空，则取“_”之前的key。
            //说明：_之后一般是当前参数的所属用户ID
            if (value == null)
            {
                if (key.IndexOf("_") > -1)
                {
                    key = key.Substring(0, key.LastIndexOf("_"));
                    value = Business.Do<ISystemPara>().GetValue(key);
                }
            }
            return value;
        }
        /// <summary>
        /// 设置参数数数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [WebMethod]
        public string SetPara(string key,string value)
        {
            Business.Do<ISystemPara>().Save(key, value,true);
            return "1";
        }
    }
}
