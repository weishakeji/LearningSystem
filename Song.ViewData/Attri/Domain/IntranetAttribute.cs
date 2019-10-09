using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 限制请求必须来局域网
    /// </summary>
    public class IntranetAttribute : WeishaAttr
    {        
        /// <summary>
        /// 将执行结果写入日志
        /// </summary>
        /// <param name="execResult"></param>
        public void LogWrite(object execResult)
        {

        }
    }
}
