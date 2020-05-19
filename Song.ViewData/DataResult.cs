using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

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
        /// 执行耗时（单位：毫秒）
        /// </summary>
        public double ExecSpan { get; set; }
        /// <summary>
        /// web端执行耗时（单位：毫秒）
        /// </summary>
        public double WebSpan { get; set; }
        /// <summary>
        /// 执行时间的时间戳
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// 详细的异常信息
        /// </summary>
        public Exception Exception { get; set; }       
        /// <summary>
        /// 实际返回的数据
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DataResult() { }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="obj"></param>
        public DataResult(object obj)
        {
            this.Result = obj;
            Success = obj != null;
            State = 1;
            DateTime = DateTime.Now;
            Timestamp = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds; 
            Message = obj != null ? "" : "未查询到数据";
        }
        public DataResult(object obj,double span):this(obj){
            this.ExecSpan = span;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exc"></param>
        public DataResult(Exception exc, DateTime time)
        {
            Success = false;
            DateTime = DateTime.Now;
            Timestamp = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds; 
            Exception = exc;
            if (exc.InnerException != null)
            {
                Message = exc.InnerException.Message;               
            }
            else
            {
                Message = exc.Message;              
            }
            //执行时间
            ExecSpan = ((TimeSpan)(DateTime.Now - time)).TotalMilliseconds;
            State = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="exc"></param>
        public DataResult(object obj, Exception exc)
        {
            this.Result = obj;
            Success = obj != null;
            DateTime = DateTime.Now;
            Timestamp = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds; 
            Exception = exc;
            if (exc.InnerException != null)
            {
                Message = exc.InnerException.Message;
            }
            else
            {
                Message = exc.Message;
            }
            State = 0;
        }
        #region json
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
                str += string.Format("\"{0}\":{1},", pi.Name.ToLower(), _json_property(typename, value));
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
        private string _json_property(string typename, object value)
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
                    str = str.Replace(Environment.NewLine, "");
                    str = str.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
                    str = string.Format("\"{0}\"", str);
                    break;
                case "Int32":
                case "Int64":
                    str = value.ToString().ToLower();
                    break;
                case "Boolean":
                    str = value.ToString().ToLower();
                    break;
                case "Object":
                    if (value is WeiSha.Data.Entity)
                    {
                        str = ((WeiSha.Data.Entity)value).ToJson();
                    }
                    else
                    {
                        if (value is WeiSha.Data.Entity[])
                        {
                            WeiSha.Data.Entity[] entitis = (WeiSha.Data.Entity[])value;
                            str += "[";
                            foreach (WeiSha.Data.Entity en in entitis)
                                str += en.ToJson() + ",";
                            if (str.EndsWith(",")) str = str.Substring(0, str.Length - 1);
                            str += "]";
                        }
                        else
                        {
                            str = JsonConvert.SerializeObject(value);
                            if (value is System.Data.DataTable)
                            {
                                str = str.Replace(":\"True\"", ":true").Replace(":\"False\"", ":false");
                            }
                        }
                    }
                    break;
                //case "Exception":
                //    Exception ex = (Exception)value;
                //    str = ex == null ? "" : ((ex.InnerException == null) ? ex.Message : ex.InnerException.Message);
                //    str = str.Replace("\n", ";").Replace("\t", " ").Replace("\r", ";");
                //    str = string.Format("\"{0}\"", str.Trim());
                //    break;
                default:
                    str = value == null ? "" : value.ToString();
                    str = str.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ");
                    str = string.Format("\"{0}\"", str);
                    break;
            }
            return str;
        }
        #endregion

        #region 输出xml
        /// <summary>
        /// 输出XML字符串
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            Type info = this.GetType();
            PropertyInfo[] properties = info.GetProperties();
            string str = "<DataResult>";
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                //当前属性的值
                object value = info.GetProperty(pi.Name).GetValue(this, null);
                //属性名（包括泛型名称）
                var nullableType = Nullable.GetUnderlyingType(pi.PropertyType);
                string typename = nullableType != null ? nullableType.Name : pi.PropertyType.Name;
                str += string.Format("<{0}>{1}</{0}>", pi.Name.ToLower(), _xml_property(typename, value));
            }
            if (str.EndsWith(",")) str = str.Substring(0, str.Length - 1);
            str += "</DataResult>";
            return str;
        }
        /// <summary>
        /// 为xml输出字段
        /// </summary>
        /// <param name="typename">字段的类型名称</param>
        /// <param name="value">字段的值</param>
        /// <returns></returns>
        private string _xml_property(string typename, object value)
        {
            if (value == null) return string.Empty;
            if (typename.Equals("object", StringComparison.CurrentCultureIgnoreCase)) typename = value.GetType().Name;
            string str = "";
            //根据不同类型输出
            switch (typename)
            {
                case "DateTime":
                    System.DateTime time = System.DateTime.Now;
                    if (value != null) time = Convert.ToDateTime(value);
                    str = time.ToString("yyyy/MM/dd HH:mm:ss");
                    break;
                case "String":
                    str = value == null ? "" : value.ToString();
                    str = str.Replace(Environment.NewLine, "");
                    str = string.Format("{0}", str);
                    break;
                case "Int32":
                    str = value.ToString().ToLower();
                    break;
                case "Boolean":
                    str = value.ToString().ToLower();
                    break;
                case "Exception":
                    Exception ex = (Exception)value;
                    str = ex == null ? "" : ex.Message;
                    str = string.Format("{0}", str);
                    break;
                default:
                    if (value == null) return string.Empty;
                    if (value is WeiSha.Data.Entity)
                    {
                        str = ((WeiSha.Data.Entity)value).ToXML();
                    }
                    else
                    {
                        if (value is WeiSha.Data.Entity[])
                        {
                            WeiSha.Data.Entity[] entitis = (WeiSha.Data.Entity[])value;
                            str += "<Entitis>";
                            foreach (WeiSha.Data.Entity en in entitis)
                                str += en.ToXML();
                            str += "</Entitis>";
                        }
                        else
                        {
                            try
                            {
                                string strjson = JsonConvert.SerializeObject(value);
                                string root = "TemporaryNode";
                                string xml = JsonConvert.DeserializeXNode(strjson, root, true).ToString();
                                xml = xml.Replace(string.Format("<{0}>", root), string.Empty);
                                xml = xml.Replace(string.Format("</{0}>", root), string.Empty);
                                str += xml;

                            }
                            catch
                            {

                                XmlSerializer serializer = new XmlSerializer(value.GetType());
                                string content = string.Empty;
                                using (StringWriter writer = new StringWriter())
                                {
                                    serializer.Serialize(writer, value);
                                    content = writer.ToString();
                                }
                                XmlDocument xml = new XmlDocument();
                                xml.LoadXml(content);
                                str += xml.LastChild.InnerXml;

                            }
                        }
                    }
                    break;
            }
            return str;
        }
        #endregion
    }
}
