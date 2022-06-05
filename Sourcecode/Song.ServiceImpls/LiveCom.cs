using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using pili_sdk.pili;
using pili_sdk;



namespace Song.ServiceImpls
{
    public class LiveCom : ILive
    {
        //设置项的前缀
        string prefix = "Qiniuyun_";

        /// <summary>
        /// 初始化相关参数
        /// </summary>
        public void Initialization()
        {
            pili_sdk.Pili.Initialization(this.GetAccessKey, this.GetSecretKey, this.GetLiveSpace, "v1");
        }
        public bool Test(string accesskey, string secretkey, string hubname)
        {
            return Pili.API<IStream>().Test(accesskey, secretkey, hubname, "v1");
        }
        #region 设置
        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="secretKey"></param>
        public void SetupKey(string accessKey, string secretKey)
        {
            if(!string.IsNullOrWhiteSpace(accessKey) && accessKey.Trim()!="")
                Business.Do<ISystemPara>().Save(prefix + "AccessKey", accessKey, false);
            if (!string.IsNullOrWhiteSpace(secretKey) && secretKey.Trim() != "")
                Business.Do<ISystemPara>().Save(prefix + "SecretKey", secretKey, false);
        }
        /// <summary>
        /// 设置直播空间的名称
        /// </summary>
        /// <param name="pace"></param>
        public void SetupLiveSpace(string pace)
        {
            if (!string.IsNullOrWhiteSpace(pace) && pace.Trim() != "")
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
        /// <summary>
        /// 设置协议，是http还是https
        /// </summary>
        /// <param name="protocol"></param>
        public void SetupProtocol(string protocol)
        {
            if (!string.IsNullOrWhiteSpace(protocol))
                Business.Do<ISystemPara>().Save(prefix + "Protocol", protocol, false);
        }
        #endregion

        #region 获取参数
        /// <summary>
        /// 直播平台的密钥
        /// </summary>
        public string GetAccessKey
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "AccessKey");
            }
        }
        /// <summary>
        /// 直播平台的密钥
        /// </summary>
        public string GetSecretKey
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "SecretKey");
            }
        }
        /// <summary>
        /// 直播空间名称
        /// </summary>
        public string GetLiveSpace
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "pace");
            }
        }
        /// <summary>
        /// rtmp播放域
        /// </summary>
        public string GetRTMP
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "RTMP");
            }
        }
        /// <summary>
        /// hls播放域名
        /// </summary>
        public string GetHLS
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "HLS");
            }
        }
        /// <summary>
        /// hdl播放域名
        /// </summary>
        public string GetHDL
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "HDL");
            }
        }
        /// <summary>
        /// 访问协议，http或https
        /// </summary>
        public string GetProtocol
        {
            get
            {
                string protocol= Business.Do<ISystemPara>().GetValue(prefix + "Protocol");
                return string.IsNullOrWhiteSpace(protocol) ? "http" : protocol;
            }
        }
        /// <summary>
        /// 推流的地址
        /// </summary>
        /// <param name="streamname">直播流的名称</param>
        public string GetPublish(string streamname)
        {
            pili_sdk.pili.Stream stream = Pili.API<IStream>().GetForTitle(streamname);
            if (stream == null) return string.Empty;
            string url = string.Format("https://{0}/{1}/{2}",stream.PublishRtmpHost,stream.HubName,stream.Title);
            return url;
        }
        
        /// <summary>
        /// 直播时实截图的域名
        /// </summary>
        public string GetSnapshot
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "Snapshot");
            }
        }
        /// <summary>
        /// 点播的域名
        /// </summary>
        public string GetVod
        {
            get
            {
                return Business.Do<ISystemPara>().GetValue(prefix + "Vod");
            }
        }
        #endregion

        #region 管理直播流
        /// <summary>
        /// 创建直播流
        /// </summary>
        /// <param name="name"></param>
        public pili_sdk.pili.Stream StreamCreat(string name)
        {           

            Stream stream = null;
            try
            {
                stream = Pili.API<IStream>().Create(name);
                return stream;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public pili_sdk.pili.Stream StreamCreat()
        {
            return this.StreamCreat(string.Empty);
        }
        /// <summary>
        /// 直播流列表
        /// </summary>
        /// <returns></returns>
        public pili_sdk.pili.StreamList StreamList(string prefix, long count)
        {
            return StreamList(prefix, null, count);
        }
        /// <summary>
        /// 直播流列表
        /// </summary>
        /// <param name="prefix">直播流名称前缀</param>
        /// <param name="living">是否正在直播中</param>
        /// <param name="count">取几条记录</param>
        /// <returns></returns>
        public pili_sdk.pili.StreamList StreamList(string prefix, bool? living, long count)
        {
            string marker = null; // optional
            long limit = count; // optional
            string titlePrefix = prefix; // optional
            try
            {
                StreamList streamList = Pili.API<IStream>().List(marker, limit, titlePrefix, living);
                IList<Stream> list = streamList.Streams;
                foreach (Stream s in list)
                {
                    // access the stream
                }
                return streamList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取直播流
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public pili_sdk.pili.Stream StreamGet(string name)
        {
            return Pili.API<IStream>().GetForTitle(name);            
        }
        /// <summary>
        /// 删除直播流
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool StreamDelete(string name)
        {
            Stream stream = this.StreamGet(name);
            if (stream == null) return false;
            //
            try
            {
                string res = Pili.API<IStream>().Delete(stream);
                if ("No Content".Equals(res))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
