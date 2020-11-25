using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;
using System.Reflection;
using pili_sdk;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 直播
    /// </summary>
    public class Live : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 记录直播的各种设置项
        /// </summary>
        /// <param name="letter"></param>
        [HttpPost]
        [Admin]
        public int Setup(Letter letter)
        {
            try
            {
                //记录key
                string akey = letter.GetParameter("AccessKey").String;
                string skey = letter.GetParameter("SecretKey").String;
                if (string.IsNullOrWhiteSpace(akey) || akey.Trim() == "") throw new Exception("AccessKey不可为空");
                if (string.IsNullOrWhiteSpace(skey) || skey.Trim() == "") throw new Exception("SecretKey不可为空");
                Business.Do<ILive>().SetupKey(akey, skey);
                //直播空间
                string space = letter.GetParameter("LiveSpace").String;
                Business.Do<ILive>().SetupLiveSpace(space);
                ////播放域名
                //string rtmp = letter.GetParameter("rtmp").String;
                //string hls = letter.GetParameter("hls").String;
                //string hdl = letter.GetParameter("hdl").String;
                //Business.Do<ILive>().SetupLive(rtmp, hls, hdl);
                ////推流域名
                //Business.Do<ILive>().SetupPublish(letter.GetParameter("Publish").String);

                //HDL直播播放域名，是否启用HTTPS
                Business.Do<ILive>().SetupProtocol(letter.GetParameter("Protocol").String);
                //直播截图的域名
                Business.Do<ILive>().SetupSnapshot(letter.GetParameter("Snapshot").String);
                //点播域名
                Business.Do<ILive>().SetupVod(letter.GetParameter("Vod").String);

                Business.Do<ISystemPara>().Refresh();
                Business.Do<ILive>().Initialization();
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
        /// <summary>
        /// 获取设置项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SuperAdmin]
        public Dictionary<string, string> GetSetup()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            object obj = Business.Do<ILive>();
            Type type = obj.GetType();
            foreach (PropertyInfo pi in type.GetProperties())
            {
                if (pi.Name.StartsWith("Get"))
                {
                    string key = pi.Name.Substring(3);
                    object objResult = pi.GetValue(obj, null);
                    dic.Add(key, objResult == null ? "" : objResult.ToString());
                }
            }
            return dic;
        }
        /// <summary>
        /// 测试链接是否通过
        /// </summary>
        /// <param name="ak">accesskey密钥</param>
        /// <param name="sk">secretkey密钥</param>
        /// <param name="hubname">直播空间名</param>
        /// <returns></returns>
        public bool Test(string ak, string sk, string hubname)
        {
            try
            {
                return Business.Do<ILive>().Test(ak, sk, hubname);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
