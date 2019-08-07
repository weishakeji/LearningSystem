using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class LiveCom : ILive
    {
        //设置项的前缀
        string prefix = "Qiniuyun_";

        #region 设置
        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public void SetupKey(string accessKey, string secretKey)
        {
            Business.Do<ISystemPara>().Save(prefix + "AccessKey", accessKey, false);
            Business.Do<ISystemPara>().Save(prefix + "SecretKey", secretKey, false);
        }
        /// <summary>
        /// 设置直播空间的名称
        /// </summary>
        /// <param name="pace"></param>
        public void SetupLiveSpace(string pace)
        {
            if (!string.IsNullOrWhiteSpace(pace))
                Business.Do<ISystemPara>().Save(prefix + "pace", pace, false);
        }
        /// <summary>
        /// 设置推流地址
        /// </summary>
        /// <param name="domain"></param>
        public void SetupPublish(string domain)
        {
            if (!string.IsNullOrWhiteSpace(domain))
                Business.Do<ISystemPara>().Save(prefix + "Publish", domain, false);
        }
        /// <summary>
        /// 设置直播域名
        /// </summary>
        /// <param name="rtmp"></param>
        /// <param name="hls"></param>
        /// <param name="hdl"></param>
        public void SetupLive(string rtmp, string hls, string hdl)
        {
            if (!string.IsNullOrWhiteSpace(rtmp))
                Business.Do<ISystemPara>().Save(prefix + "RTMP", rtmp, false);
            if (!string.IsNullOrWhiteSpace(hls))
                Business.Do<ISystemPara>().Save(prefix + "HLS", hls, false);
            if (!string.IsNullOrWhiteSpace(hdl))
                Business.Do<ISystemPara>().Save(prefix + "HDL", hdl, false);
        }
        /// <summary>
        /// 设置封面CDN域名
        /// </summary>
        /// <param name="domain"></param>
        public void SetupSnapshot(string domain)
        {
            if (!string.IsNullOrWhiteSpace(domain))
                Business.Do<ISystemPara>().Save(prefix + "Snapshot", domain, false);
        }
        /// <summary>
        /// 设置点播域名
        /// </summary>
        /// <param name="domain"></param>
        public void SetupVod(string domain)
        {
            if (!string.IsNullOrWhiteSpace(domain))
                Business.Do<ISystemPara>().Save(prefix + "Vod", domain, false);
        }
        #endregion

        #region 获取参数
        /// <summary>
        /// 直播平台的密钥
        /// </summary>
        public string GetAccessKey()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "AccessKey");
        }
        /// <summary>
        /// 直播平台的密钥
        /// </summary>
        public string GetSecretKey()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "SecretKey");
        }
        /// <summary>
        /// 直播空间名称
        /// </summary>
        public string GetLiveSpace()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "pace");
        }
        /// <summary>
        /// rtmp播放域
        /// </summary>
        public string GetRTMP()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "RTMP");
        }
        /// <summary>
        /// hls播放域名
        /// </summary>
        public string GetHLS()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "HLS");
        }
        /// <summary>
        /// hdl播放域名
        /// </summary>
        public string GetHDL()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "HDL");
        }
        /// <summary>
        /// 推流的域名
        /// </summary>
        public string GetPublish()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "Publish");
        }
        /// <summary>
        /// 直播时实截图的域名
        /// </summary>
        public string GetSnapshot()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "Snapshot");
        }
        /// <summary>
        /// 点播的域名
        /// </summary>
        public string GetVod()
        {
            return Business.Do<ISystemPara>().GetValue(prefix + "Vod");
        }
        #endregion
    }
}
