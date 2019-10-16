using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData.Attri
{
    /// <summary>
    /// 限制请求必须来自本机
    /// </summary>
    public class LocalhostAttribute : RangeAttribute
    {        
        
    }
}
