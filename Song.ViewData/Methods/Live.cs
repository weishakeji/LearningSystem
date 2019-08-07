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

namespace Song.ViewData.Methods
{
    public class Live
    {
        /// <summary>
        /// 记录直播的各种设置项
        /// </summary>
        /// <param name="letter"></param>
        public int Setup(Letter letter)
        {
            try
            {
                //记录key
                string akey = letter.GetParameter("AccessKey").String;
                string skey = letter.GetParameter("SecretKey").String;
                Business.Do<ILive>().SetupKey(akey, skey);
                //直播空间
                string space = letter.GetParameter("LiveSpace").String;
                Business.Do<ILive>().SetupLiveSpace(space);
                //播放域名
                string rtmp = letter.GetParameter("rtmp").String;
                string hls = letter.GetParameter("hls").String;
                string hdl = letter.GetParameter("hdl").String;
                Business.Do<ILive>().SetupLive(rtmp, hls, hdl);
                //推流域名
                Business.Do<ILive>().SetupPublish(letter.GetParameter("Publish").String);
                //直播截图的域名
                Business.Do<ILive>().SetupSnapshot(letter.GetParameter("Snapshot").String);
                //点播域名
                Business.Do<ILive>().SetupVod(letter.GetParameter("Vod").String);

                Business.Do<ISystemPara>().Refresh();
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
        public string GetSetup()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            object obj = Business.Do<ILive>();
            Type type = obj.GetType();
            foreach (MethodInfo mi in type.GetMethods())
            {
                if (mi.Name.StartsWith("Get"))
                {
                    string key = mi.Name.Substring(3);
                    object objResult = mi.Invoke(obj, null);
                    dic.Add(key, objResult == null ? "" : objResult.ToString());
                }
            }
            return JsonConvert.SerializeObject(dic);
        }
    }
}
