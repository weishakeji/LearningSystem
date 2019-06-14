using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiSha.Common;
using Song.ServiceInterfaces;
using Song.Extend;
using System.Data;
using System.Text;

namespace Song.Site.Ajax
{
    /// <summary>
    /// 我的学习记录
    /// </summary>
    public class SelfStudyLog : IHttpHandler
    {
        //学员id
        int stid = WeiSha.Common.Request.QueryString["id"].Int32 ?? 0;
        public void ProcessRequest(HttpContext context)
        {
            DataTable dt = Business.Do<IStudent>().StudentStudyCourseLog(stid);
            string json = DataTableToJson(dt);
            context.Response.Write(json);
        }

        public string DataTableToJson(DataTable table)
        {
            if (table == null) return string.Empty;
            var JsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        Type t = table.Columns[j].DataType;
                        //属性名（包括泛型名称）
                        var nullableType = Nullable.GetUnderlyingType(t);
                        string typename = nullableType != null ? nullableType.Name : t.Name;
                        //返回当前属性的值
                        string val=_to_property(typename, table.Rows[i][j]);
                        JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + val + "\"");
                        JsonString.Append(j < table.Columns.Count - 1 ? "," : "");
                    }
                    JsonString.Append(i == table.Rows.Count - 1 ? "}" : "},");
                }
                JsonString.Append("]");
            }
            return JsonString.ToString();
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
                    str = HttpUtility.UrlEncode(str).Replace("+", "%20");
                    break;
                case "Boolean":
                    str = value.ToString().ToLower();
                    break;
                default:
                    str = value == null ? "" : value.ToString();
                    break;
            }
            return str;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}