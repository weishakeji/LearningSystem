using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private int _size = 1;
        /// <summary>
        /// 当前页的数据项个数
        /// </summary>
        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value <= 0 ? 1 : value;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
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
            this.Result = obj;
            Success = true;
            State = 1;
            DateTime = DateTime.Now;
            Timestamp = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds; 
        }       
    }
}
