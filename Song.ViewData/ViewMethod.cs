using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData
{
    /// <summary>
    /// 接口的父类
    /// </summary>
    public abstract class ViewMethod
    {
        /// <summary>
        /// 客户端传来的参数
        /// </summary>
        public Letter Letter
        {
            get;
            set;
        }
    }
}
