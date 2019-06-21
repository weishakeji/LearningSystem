using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

namespace Song.ViewData
{
    /// <summary>
    /// 当客户端请求Song.ViewData方法时，返回值全部用该方法“包装”，方便返回json或xml格式数据
    /// 如果是服务器端方法，则无须“包装”
    /// </summary>
    public class DataResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 详细的异常信息
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// 实际返回的数据
        /// </summary>
        public object Data { get; set; }
        public DataResult() { }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="obj"></param>
        public DataResult(object obj)
        {
            this.Data = obj;
            Success = obj != null;
            State = 1;
            DateTime = DateTime.Now;
            Message = obj != null ? "" : "未查询到数据";
        }
        public DataResult(Exception exc)
        {
            Success = false;
            DateTime = DateTime.Now;
            Exception = exc;
            Message = exc.Message;
            State = 0;
        }
        public DataResult(object obj, Exception exc)
        {
            this.Data = obj;
            Success = obj != null;
            DateTime = DateTime.Now;
            Exception = exc;
            Message = exc.Message;
            State = 0;
        }
        /// <summary>
        /// 输出Json字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            Type info = this.GetType();
            PropertyInfo[] properties = info.GetProperties();
            string str = "{";
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                //当前属性的值
                object value = info.GetProperty(pi.Name).GetValue(this, null);
                //属性名（包括泛型名称）
                var nullableType = Nullable.GetUnderlyingType(pi.PropertyType);
                string typename = nullableType != null ? nullableType.Name : pi.PropertyType.Name;
                str += string.Format("\"{0}\":{1},", pi.Name, _to_property(typename, value));
            }
            if (str.EndsWith(",")) str = str.Substring(0, str.Length - 1);
            str += "}";
            return str;
        }
        /// <summary>
        /// 为json输出字段
        /// </summary>
        /// <param name="typename">字段的类型名称</param>
        /// <param name="value">字段的值</param>
        /// <returns></returns>
        private string _to_property(string typename, object value)
        {
            string str = "";
            //根据不同类型输出
            switch (typename)
            {
                case "DateTime":
                    System.DateTime time = System.DateTime.Now;
                    if (value != null) time = Convert.ToDateTime(value);
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
                    //将C#时间转换成JS时间字符串    
                    string JSstring = string.Format("eval('new ' + eval('/Date({0})/').source)", timeStamp);
                    str = JSstring;
                    break;
                case "String":
                    str = value == null ? "" : value.ToString();
                    str = string.Format("\"{0}\"", str);
                    break;
                case "Boolean":
                    str = value.ToString().ToLower();
                    break;
                case "Object":
                    str = JsonConvert.SerializeObject(value);
                    break;
                case "Exception":
                    Exception ex = (Exception)value;
                    str = ex == null ? "" : ex.Message;
                    str = string.Format("\"{0}\"", str);
                    break;
                default:
                    str = value == null ? "" : value.ToString();
                    str = string.Format("\"{0}\"", str);
                    break;
            }
            return str;
        }
    }
}
