using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Data;

namespace Song.ViewData
{
    /// <summary>
    /// 时间对象的扩展
    /// </summary>
    public static class DataTimeExtensions
    {
        /// <summary>
        /// 实体转为JObject对象
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToJsString(this DateTime date)
        {
            System.DateTime time = System.DateTime.Now;
            if (date != null) time = Convert.ToDateTime(date);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            //将C#时间转换成JS时间字符串    
            return string.Format("eval('new ' + eval('/Date({0})/').source)", timeStamp);
        }        
    }
}
