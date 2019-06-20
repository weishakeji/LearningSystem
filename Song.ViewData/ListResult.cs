using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData
{
    /// <summary>
    /// 当请求的Song.ViewData方法是列表时，尤其是分页数据时，用该方法“包装”
    /// 不管是服务端还是客户端，都要用此方法“包装”
    /// </summary>
    public class ListResult : DataResult
    {
        /// <summary>
        /// 数据项的总数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 当前页的数据项个数
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int Pager
        {
            get
            {
                return (int)Math.Ceiling((double)Total / (double)Size);
            }
        }
        /// <summary>
        /// 当前页的索引，起始为1
        /// </summary>
        public int Index { get; set; }
        public ListResult()
        {
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="obj"></param>
        public ListResult(object obj)
        {
            this.Data = obj;
            Success = true;
            State = 1;
            DateTime = DateTime.Now;
        }
    }
}
