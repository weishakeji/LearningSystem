using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 平台系统参数
    /// </summary>
    [HttpPut, HttpGet,HttpPost]
    public class Systempara : ViewMethod, IViewAPI
    {        
        /// <summary>
        /// 平台信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JObject PlatInfo()
        {
            string title = Business.Do<ISystemPara>().GetValue("PlatInfo_title");
            string intro = Business.Do<ISystemPara>().GetValue("PlatInfo_intro");
            JObject jo = new JObject();
            jo.Add("title", title);
            jo.Add("intro", intro);
            return jo;
        }
        /// <summary>
        /// 保存平台信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="intro"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool PlatInfoUpdate(string title,string intro)
        {
            try
            {
                Business.Do<ISystemPara>().Save("PlatInfo_title", title);
                Business.Do<ISystemPara>().Save("PlatInfo_intro", intro);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取GUID，全局唯一ID
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string UniqueID()
        {
            return WeiSha.Core.Request.UniqueID();
        }
        /// <summary>
        /// 根据GUID获取19位的唯一数字序列 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Unique64ID()
        {
            long id= WeiSha.Core.Request.Unique64ID();
            return id.ToString();
        }
        /// <summary>
        /// 雪花ID 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string SnowID()
        {
            long id = Business.Do<ISystemPara>().SerialSnow();
            return id.ToString();
        }
    }
}
