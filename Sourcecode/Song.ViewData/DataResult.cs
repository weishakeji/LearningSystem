using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Data;
using System.Web;

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
        /// 数据类型
        /// </summary>
        public object DataType { get; set; }
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
            if (obj != null)
                this.DataType = obj.GetType().SimpleName();
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
        /// <param name="time"></param>
        public DataResult(Exception exc, DateTime time)
        {
            Success = false;
            DateTime = DateTime.Now;
            Timestamp = (long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1))).TotalMilliseconds;
            Exception = exc;
            //异常最深处的消息
            Exception exception = exc;            
            while (exception.InnerException != null)           
                exception = exception.InnerException;         
            Message = exception.Message;
            //执行时间
            ExecSpan = ((TimeSpan)(DateTime.Now - time)).TotalMilliseconds;
            if (exception is System.Data.SqlClient.SqlException)
            {
                System.Data.SqlClient.SqlException sqlexc = (System.Data.SqlClient.SqlException)exception;
                //sqlexc.
                //数据库操作异常
                //4060为链接异常
                State = 90000 + sqlexc.Number;
            }
            else if (exc is VExcept)
            {
                VExcept ve = (VExcept)exc;
                State = ve != null ? ve.State : 0;
            }
            else
            {
                State = 0;
            }
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
            if (exc is VExcept)
            {
                VExcept ve = (VExcept)exc;
                State = ve != null ? ve.State : 0;
            }
            else
            {
                State = 0;
            }
        }


        #region json
        private static int max_level = 12;
        /// <summary>
        /// 当执行结果转为Json字符串
        /// </summary>
        /// <returns></returns>
        public virtual string ToJson()
        {
           return ObjectToJson(this);
        }
        /// <summary>
        /// 将对象转为json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            return ObjectToJson(obj,null, true, 0);
        }
        /// <summary>
        /// 将对象转为json
        /// </summary>
        /// <param name="obj">数据对象</param>
        /// <param name="type">数据对象的类型</param>
        /// <param name="islower">json的属性名是否转小写</param>
        /// <param name="level">输出的层深</param>
        /// <returns></returns>
        public static string ObjectToJson(object obj,Type type, bool islower, int level)
        {
            //当大于最大层深，则不再输出
            if (level >= max_level) return "\"\"";

            if (obj == null) return level <= 1 ? "null" : "\"\"";

            if (type == null) type = obj.GetType();
            //类型名（包括泛型名称）
            string typename = type.SimpleName();
            if (typename == "DBNull") return "\"\"";          
            //长整型作为字符串处理，否则在客户端的js解析时会丢失精度
            if (typename == "Int64" || typename == "UInt64")
                return string.Format("\"{0}\"", obj.ToString());
            if (typename == "Decimal")
            {
                decimal dec = (decimal)obj;
                dec = Math.Round(dec * 100) / 100;
                return string.Format("\"{0}\"", dec.ToString());
            }
            //如果是数值型或逻辑型
            if (type.IsNumeric() || typename == "Boolean")
                return obj.ToString().ToLower();                
            //字符串作为url编码处理
            if (typename == "String")
            {
                string str = obj.ToString();
                if (string.IsNullOrWhiteSpace(str.Trim())) return "\"\"";
                str = str.Replace(Environment.NewLine, "");
                str = Microsoft.JScript.GlobalObject.escape(str);
                return string.Format("\"{0}\"", str);
            }
            //日期类型转成js所需的格式
            if (typename == "DateTime")
            {
                System.DateTime time = System.DateTime.Now;
                try
                {
                    if (obj != null) time = Convert.ToDateTime(obj);
                }
                catch
                {
                    return "\"\"";
                }
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
                if (timeStamp < 0) timeStamp = 0;
                //将C#时间转换成JS时间字符串    
                return string.Format("eval('new ' + eval('/Date({0})/').source)", timeStamp);
            }

            if (obj is System.Exception)
            {
                return JsonConvert.SerializeObject(obj);
            }
            StringBuilder sb = new StringBuilder();
            //如果是Newtonsoft.Json 的对象
            if (obj is JContainer)
            {
                string jstr = obj.ToString();
                jstr = Microsoft.JScript.GlobalObject.escape(jstr);
                sb.Append(jstr);
                return string.Format("\"{0}\"", jstr);
            }
            //如果是数组
            else if (type.IsArray)
            {
                Array array = (Array)obj;
                int lv = ++level;
                sb.Append("[");
                for (int i = 0; i < array.Length; i++)
                {
                    sb.Append(ObjectToJson(array.GetValue(i), null, false, lv));
                    if (i < array.Length - 1) sb.Append(",");
                }
                sb.Append("]");
            }
            //如果是List
            else if (obj is System.Collections.IList)
            {
                System.Collections.IList list = (System.Collections.IList)obj;
                int lv = ++level;
                sb.Append("[");
                for (int i = 0; i < list.Count; i++)
                {
                    sb.Append(ObjectToJson(list[i], null, false, lv));
                    if (i < list.Count - 1) sb.Append(",");
                }
                sb.Append("]");
            }
            //如果是DataTable
            else if (obj is DataTable)
            {
                sb.Append(DataTableToJson((DataTable)obj));
            }
            else if (type.FullName.IndexOf("Dictionary") > -1)
            {
                string str = JsonConvert.SerializeObject(obj);
                str = str.Replace(":\"True\"", ":true").Replace(":\"False\"", ":false");
                sb.Append(str);
            }
            else
            {
                PropertyInfo[] properties = type.GetProperties();
                //实体对象的属保持原样，其余小写
                bool isLower = !(obj is WeiSha.Data.Entity) && islower;
                int lv = ++level;
                sb.Append("{");
                for (int j = 0; j < properties.Length; j++)
                {
                    PropertyInfo pi = properties[j];
                    //当前属性的值
                    object value = type.GetProperty(pi.Name).GetValue(obj, null);
                    //有JsonIgnore特性的，直接跳过，不生成json
                    object[] objAttrs = pi.GetCustomAttributes(typeof(JsonIgnoreAttribute), true);
                    if (objAttrs.Length > 0) continue;
                    //json属性名
                    string attrname = isLower ? pi.Name.ToLower() : pi.Name;
                    sb.Append(string.Format("\"{0}\":{1}", attrname, ObjectToJson(value, null, false, lv)));
                    if (j < properties.Length - 1) sb.Append(",");
                }
                sb.Append("}");
            }
            return sb.ToString();
        }
        /// <summary>
        /// DataTable转json字符
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                sb.Append("{");
                for (int j = 0;j < dt.Columns.Count; j++)
                {                    
                    string val = ObjectToJson(dr[j], null, false, 1);
                    if (val == "{}") val = "";
                    string json = string.Format("{0}:{1}", dt.Columns[j].ColumnName, val);
                    json = json.Replace(":\"True\"", ":true").Replace(":\"False\"", ":false");
 
                    sb.Append(json);
                    if (j < dt.Columns.Count - 1) sb.Append(",");
                }
                sb.Append("}");
                if (i < dt.Rows.Count - 1) sb.Append(",");              
            }
            sb.Append("]");
            return sb.ToString();
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
                if (pi.Name.Equals("DataType", StringComparison.CurrentCultureIgnoreCase)) continue;
                //当前属性的值
                object value = info.GetProperty(pi.Name).GetValue(this, null);
                //属性名（包括泛型名称）
                string typename = pi.PropertyType.SimpleName();
                try
                {
                    str += string.Format("<{0}>{1}</{0}>", pi.Name.ToLower(), _xml_property(typename, value));
                }catch(Exception ex)
                {
                    throw ex;
                }
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
                case "Double":
                case "Int64":
                case "Boolean":
                    str = value.ToString().ToLower();
                    break;
                case "Exception":
                    Exception ex = (Exception)value;
                    str = ex == null ? "" : ex.Message;
                    str = string.Format("{0}", str);
                    break;
                case "JArray":
                    str = _xml_for_jsonarr((JArray)value);
                    break;
                default:
                    if (value == null) return string.Empty;
                    if (value is WeiSha.Data.Entity)
                    {
                        str = ((WeiSha.Data.Entity)value).ToXML();
                    }
                    else if (value is WeiSha.Data.Entity[])
                    {
                        WeiSha.Data.Entity[] entitis = (WeiSha.Data.Entity[])value;
                        str += "<Entitis>";
                        foreach (WeiSha.Data.Entity en in entitis)
                            str += en.ToXML();
                        str += "</Entitis>";
                    }
                    else if (value is Array)
                    {
                        Array array = (Array)value;                       
                        for (int i = 0; i < array.Length; i++)
                        {
                            string name = array.GetValue(i).GetType().Name;
                            str += "<"+name+">";
                            str += _xml_property(array.GetValue(i).GetType().Name, array.GetValue(i));
                            str += "</"+name+">";
                        }                      
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
                            using (StringWriter writer = new StringWriter())
                            {
                                XmlSerializer serializer = new XmlSerializer(value.GetType());
                                string content = string.Empty;
                                serializer.Serialize(writer, value);
                                content = writer.ToString();
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
        /// <summary>
        /// JArray对象转xml
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        private string _xml_for_jsonarr(JArray arr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Array>");
            foreach (JToken jt in arr)
            {
                if (jt is JArray)
                    sb.Append(_xml_for_jsonarr((JArray)jt));
                if (jt is JObject) 
                    sb.Append(_xml_for_json((JObject)jt));
            }
            sb.Append("</Object>");
            return sb.ToString();
        }
        private string _xml_for_json(JObject jo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Object>");
            foreach (JProperty item in jo.Children())
            {               
                sb.Append("<" + item.Name + ">");              
                if (item.Value is JArray)
                    sb.Append(_xml_for_jsonarr((JArray)item.Value));
                else if (item.Value is JObject)
                    sb.Append(_xml_for_json((JObject)item.Value));
                else
                    sb.Append(item.Value);
                sb.Append("</" + item.Name + ">");
            }
            sb.Append("</Object>");
            return sb.ToString();
        }
        #endregion
    }

    public static class DataResult_Extends
    {
        private static string[] _numberec = { "Int16", "Int32", "Int64", "UInt16", "UInt32", "UInt64",
        "Single","Double","Decimal","SByte","Byte"};
        /// <summary>
        /// 判断一个类型是否为数值型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(this Type type)
        {
            if (type.IsClass || type.IsInterface) return false;
            if (type.Name.StartsWith("Nullable"))
                return type.GetGenericArguments()[0].IsNumeric();
            return Array.IndexOf(_numberec, type.Name) >= 0;
        }
        /// <summary>
        /// 简要名称，例如泛型名称不要带`
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string SimpleName(this Type type)
        {
            //属性名（包括泛型名称）
            var nullableType = Nullable.GetUnderlyingType(type);
            string typename = nullableType != null ? nullableType.Name : type.Name;
            if (typename.IndexOf("`") > -1) typename = typename.Substring(0, typename.IndexOf("`"));
            return typename;
        }
        /// <summary>
        /// 属性名称是否转小写
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsChangeLower(this Type type)
        {
            return false;
        }
    }
}
