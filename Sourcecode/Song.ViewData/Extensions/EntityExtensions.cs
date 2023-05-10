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
    /// 实体类的扩展
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// 实体转为JObject对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static JObject ToJObject(this Entity entity)
        {
            return ToJObject(entity, null, null);
        }
        /// <summary>
        /// 将实体生成为Json对象，每个属性为json的属性
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="only">仅输出指定的属性，用逗号分隔，注意大小写</param>
        /// <param name="wipe">限定不输出的属性，用逗号分隔，注意大小写</param>
        /// <returns></returns>
        public static JObject ToJObject(this Entity entity,string only, string wipe)
        {
            return ToJObject(entity, only, wipe, null);
        }
        /// <summary>
        /// 将实体生成为Json对象，每个属性为json的属性
        /// </summary>
        /// <param name="only">仅输出指定的属性，用逗号分隔，注意大小写</param>
        /// <param name="wipe">限定不输出的属性，用逗号分隔，注意大小写</param>
        /// <param name="addParas">要增加的输出项</param>
        /// <returns></returns>
        public static JObject ToJObject(this Entity entity,string only, string wipe, Dictionary<string, object> addParas)
        {
            string[] onlyArr = null, wipeArr = null;
            if (!string.IsNullOrWhiteSpace(only)) onlyArr = only.Split(',');
            if (!string.IsNullOrWhiteSpace(wipe)) wipeArr = wipe.Split(',');
            //输出字段为json格式
            string str = "{";
            Type info = entity.GetType();
            PropertyInfo[] properties = info.GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                PropertyInfo pi = properties[j];
                //当前属性的值
                object value = info.GetProperty(pi.Name).GetValue(entity, null);
                //判断是否输出，是如果没有在指定的输出范围内，则跳过
                if (onlyArr != null && onlyArr.Length > 0 && !_tojson_isExist(pi.Name, onlyArr)) continue;
                //判断是否输出，是如果指定不允许输出，则跳过
                if (wipeArr != null && wipeArr.Length > 0 && _tojson_isExist(pi.Name, wipeArr)) continue;
                //输出属性与对应的值
                var nullableType = Nullable.GetUnderlyingType(pi.PropertyType);
                string typename = nullableType != null ? nullableType.Name : pi.PropertyType.Name;
                str += _tojson_property(typename, pi.Name, value, false) + ",";
            }
            //输出额外增加的项
            if (addParas != null && addParas.Count > 0)
            {
                foreach (KeyValuePair<string, object> e in addParas)
                {
                    if (e.Value != null)
                    {
                        str += _tojson_property(e.Value.GetType().Name, e.Key, e.Value, false) + ",";
                    }
                    else
                    {
                        str += _tojson_property("String", e.Key, e.Value, false) + ",";
                    }
                }
            }
            if (str.Length > 0 && str.Substring(str.Length - 1, 1) == ",") str = str.Substring(0, str.Length - 1);
            str += "}";
            return JObject.Parse(str);
        }
        /// <summary>
        /// 判断属性是否在数组中
        /// </summary>
        /// <param name="piname">属性名</param>
        /// <param name="arr">数组</param>
        /// <returns></returns>
        private static bool _tojson_isExist(string piname, string[] arr)
        {
            bool isExist = false;
            if (arr != null && arr.Length > 0)
            {
                foreach (string w in arr)
                {
                    if (w.Trim() == piname)
                    {
                        isExist = true;
                        break;
                    }
                }
            }
            return isExist;
        }
        /// <summary>
        /// 为json输出字段
        /// </summary>
        /// <param name="typename">字段的类型名称</param>
        /// <param name="name">字段的名称</param>
        /// <param name="value">字段的值</param>
        /// <param name="dateeval">时间值是否采用eval方式</param>
        /// <returns></returns>
        private static string _tojson_property(string typename, string name, object value, bool dateeval)
        {
            string str = "\"";
            //根据不同类型输出
            switch (typename)
            {
                case "DateTime":
                    System.DateTime time = System.DateTime.Now;
                    if (value != null) time = Convert.ToDateTime(value);
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
                    if (dateeval)
                    {   
                        //将C#时间转换成JS时间字符串    
                        string JSstring = string.Format("eval('new ' + eval('/Date({0})/').source)", timeStamp);
                        str += name + "\":" + JSstring + "";
                    }
                    else
                    {
                        //str += name + "\":\"" + timeStamp + ".0000\"";
                        str += name + "\":\"" + time.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
                    }
                    break;
                case "String":
                    str += name + "\":\"" + (value == null ? "" : Microsoft.JScript.GlobalObject.escape(value.ToString())) + "\"";
                    break;
                case "Boolean":
                    str += name + "\":" + (value == null ? "false" : value.ToString().ToLower());
                    break;
                case "Byte":
                case "SByte":
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                case "Single":
                case "Double":
                    str += name + "\":" + (value == null ? "0" : value.ToString().ToLower());
                    break;
                case "Int64":
                case "UInt64":
                    str += name + "\":\"" + value.ToString() + "\"";
                    break;
                case "Decimal":
                    decimal dec = (decimal)value;
                    dec = Math.Round(dec * 100) / 100;
                    str += name + "\":\"" + dec.ToString() + "\"";
                    break;
                default:
                    str += name + "\":\"" + (value == null ? "" : Microsoft.JScript.GlobalObject.escape(value.ToString())) + "\"";
                    break;
            }
            return str;
        }
    }
}
