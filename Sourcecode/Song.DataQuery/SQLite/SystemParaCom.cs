
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;
using WeiSha.Data;
using Song.Entities;
using Song.ServiceInterfaces;
using System.Text.RegularExpressions;

namespace Song.DataQuery.SQLite
{
    /// <summary>
    /// 系统参数，以及相关系统级设置
    /// </summary>
    public class SystemParaCom
    {
        /// <summary>
        /// 仅获取下的字段的名称，不包括类型等其它属性
        /// </summary>
        /// <param name="tablename">表</param>
        /// <returns></returns>
        public List<string> DataFieldNames(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"PRAGMA table_info('{{tablename}}')";
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {
                List<string> list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetValue<string>("name"));
                reader.Close();
                reader.Dispose();
                var sortedList = list.OrderBy(x => x).ToList();
                return sortedList;
            }
        }
        /// <summary>
        /// 获取数据字段
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：name,type,length,fulltype,isnullable,primary(主键，非零为真)</returns>
        public DataTable DataFields(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"PRAGMA table_info('{{tablename}}')";
            sql = sql.Replace("{{tablename}}", tablename.Trim());

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            if (ds.Tables.Count < 1) return null;
            //按名称排序
            DataView dataView = new DataView(ds.Tables[0]);
            dataView.Sort = "name ASC";
            DataTable dt = dataView.ToTable();
            //长度,为了保持与Postresql输出的结构一段
            dt.Columns.Add(new DataColumn("length", typeof(int)));

            //是否字段可为，此处如此处理是为了保持与Postresql输出的结构一段
            dt.Columns.Add(new DataColumn("nullable", typeof(int)));
            foreach (DataRow dr in dt.Rows)         
                dr["nullable"] = dr["notnull"].ToString() == "0" ? 0 : 1;           
            //获取主键          
            dt.Columns.Add(new DataColumn("primary", typeof(int)));
            DataRow drmain = dt.NewRow();
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                dr["primary"] = dr["pk"].ToString() == "0" ? 0 : 1;
                if (dr["primary"].ToString() == "1")
                {
                    drmain.ItemArray = dr.ItemArray;
                    dt.Rows[i].Delete();
                }
            }
            if(drmain!=null) dt.Rows.InsertAt(drmain, 0);
            //删除冗余行
            if (dt.Columns.Contains("cid")) dt.Columns.Remove("cid");
            if (dt.Columns.Contains("notnull")) dt.Columns.Remove("notnull");
            if (dt.Columns.Contains("pk")) dt.Columns.Remove("pk");
            if (dt.Columns.Contains("dflt_value")) dt.Columns.Remove("dflt_value");
            return dt;
        }
        /// <summary>
        /// 获取表的索引
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：name</returns>
        public DataTable DataIndexs(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"PRAGMA index_list('{{tablename}}');";
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
    }
}
