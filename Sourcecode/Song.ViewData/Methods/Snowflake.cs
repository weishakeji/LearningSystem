using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 雪花id的相关处理，实际业务中用不到
    /// </summary>
    [HttpPut, HttpGet,HttpPost]
    public class Snowflake : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 生成雪花id
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public string GenerateTest(int count)
        {
            for (int i = 0; i < count; i++)
            {
                long snowid = WeiSha.Core.Request.SnowID();

            }
            return "生成 " + count + "个ID，这里仅是为了测试生成速度，请查看 execspan 值（单位 毫秒）";
        }
        /// <summary>
        /// 生成雪花id
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            long snowid = WeiSha.Core.Request.SnowID();
            return snowid.ToString();
        }

        /// <summary>
        /// 测试md5加密的新旧方法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public JObject testmd5(string str)
        {
            JObject jo = new JObject();
            //MD5加密
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            string pwd = string.Empty;
            for (int i = 0; i < s.Length; i++)
                pwd = pwd + s[i].ToString("x2");
            jo.Add("MD5", pwd);
            //
            // 创建 SHA-256 哈希算法实例
            using (SHA256 sha256 = SHA256.Create())
            {
                // 将数据转换为字节数组
                byte[] dataBytes = Encoding.UTF8.GetBytes(str);
                // 执行哈希计算
                byte[] hashBytes = sha256.ComputeHash(dataBytes);
                // 将哈希结果转换为字符串或其他格式
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "");
                jo.Add("hashString", hashString);
            }
            return jo;
        }
        public string testSHA256(string str)
        {
            string hashString = string.Empty;
            // 创建 SHA-256 哈希算法实例
            using (SHA256 sha256 = SHA256.Create())
            {
                // 将数据转换为字节数组
                byte[] dataBytes = Encoding.UTF8.GetBytes(str);
                // 执行哈希计算
                byte[] hashBytes = sha256.ComputeHash(dataBytes);
                // 将哈希结果转换为字符串或其他格式
                hashString = BitConverter.ToString(hashBytes).Replace("-", "");            
            }
            return hashString;
        }
        /// <summary>
        /// 获取地理位置信息
        /// </summary>
        /// <returns></returns>
        public JObject GetLBS()
        {
            JObject jo = new JObject();
            //来自客户端的经纬度信息
            Letter letter = this.Letter;
            decimal lng = letter.Longitude, lat = letter.Latitude;
            jo.Add("lng", lng);
            jo.Add("lat", lat);
            //IP相关数据
            WeiSha.Core.Param.Method.Position posi = WeiSha.Core.Request.Position();
            JObject ipobject = (JObject)JToken.FromObject(posi);
            jo.Add("ip", ipobject);
            //posi.Longitude

            //通过经纬度
            WeiSha.Core.Param.Method.Position geo = WeiSha.Core.Request.Position(lng, lat);
            JObject geoObj = (JObject)JToken.FromObject(geo);
            jo.Add("geo", geoObj);
            //jo.Add("ip_lng", posi.Latitude);
            //jo.Add("ip_lat", posi.Latitude);
            //jo.Add("ip", posi.Latitude);
            //jo.Add("province", posi.Province);
            //jo.Add("city", posi.City);
            //jo.Add("district", posi.District);
            return jo;
        }
    }
}
