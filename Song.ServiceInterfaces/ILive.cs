using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 直播管理
    /// </summary>
    public interface ILive : WeiSha.Common.IBusinessInterface
    {
        #region 设置
        /// <summary>
        /// 设置直接平台的密钥
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        void SetupKey(string accessKey, string secretKey);
        /// <summary>
        /// 设置直播空间名称
        /// </summary>
        /// <param name="pace"></param>
        void SetupLiveSpace(string pace);       
        /// <summary>
        /// 设置播放的域名
        /// </summary>
        /// <param name="rtmp"></param>
        /// <param name="hls"></param>
        /// <param name="hdl"></param>
        void SetupLive(string rtmp, string hls, string hdl);
        /// <summary>
        /// 设置推流的域名
        /// </summary>
        /// <param name="domain"></param>
        void SetupPublish(string domain);
        /// <summary>
        /// 设置直播时实截图的域名
        /// </summary>
        /// <param name="domain"></param>
        void SetupSnapshot(string domain);
        /// <summary>
        /// 设置点播域名
        /// </summary>
        /// <param name="domain"></param>
        void SetupVod(string domain);
        #endregion

        #region 获取参数
        /// <summary>
        /// 直播平台的密钥
        /// </summary>
        string GetAccessKey();
        /// <summary>
        /// 直播平台的密钥
        /// </summary>
        string GetSecretKey();
        /// <summary>
        /// 直播空间名称
        /// </summary>
        string GetLiveSpace();
        /// <summary>
        /// rtmp播放域
        /// </summary>
        string GetRTMP();
        /// <summary>
        /// hls播放域名
        /// </summary>
        string GetHLS();
        /// <summary>
        /// hdl播放域名
        /// </summary>
        string GetHDL();
        /// <summary>
        /// 推流的域名
        /// </summary>
        string GetPublish();
        /// <summary>
        /// 直播时实截图的域名
        /// </summary>
        string GetSnapshot();
        /// <summary>
        /// 点播的域名
        /// </summary>
        string GetVod();
        #endregion
    }
}
