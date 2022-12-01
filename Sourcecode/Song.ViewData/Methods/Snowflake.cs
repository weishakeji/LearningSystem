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

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 雪花id的相关处理，实际业务中用不到
    /// </summary>
    [HttpPut, HttpGet]
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
    }
}
