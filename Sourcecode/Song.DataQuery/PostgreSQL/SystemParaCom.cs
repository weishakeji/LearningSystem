
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

namespace Song.DataQuery.PostgreSQL
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
            string sql = @"SELECT column_name as name FROM information_schema.columns WHERE table_name = '{{tablename}}'  order by column_name ";
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {
                List<string> list = new List<string>();
                while (reader.Read())
                    list.Add(reader.GetValue<string>(0));
                reader.Close();
                reader.Dispose();
                return list;
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
            string sql = @"SELECT column_name as name,
                        unnest(REGEXP_MATCHES(data_type,'^(\w[^\W]+)')) as type,
	                    data_type,
	                    character_maximum_length as length, --字符型的长度
						numeric_precision as num_length,	--数字型的长度
						numeric_scale,						--浮点数的小数位
	                    case when is_nullable='NO' then 0 else 1 end  as nullable
                    FROM information_schema.columns
                    WHERE table_name = '{{tablename}}' order by name asc;";
            sql = sql.Replace("{{tablename}}", tablename.Trim());

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            if (ds.Tables.Count < 1) return null;

            //获取主键
            string sql2 = @"SELECT kcu.column_name 
                        FROM information_schema.table_constraints tc
                        JOIN information_schema.key_column_usage kcu 
                        ON tc.constraint_name = kcu.constraint_name
                        WHERE tc.table_name = '{{tablename}}' AND tc.constraint_type = 'PRIMARY KEY';";
            sql2 = sql2.Replace("{{tablename}}", tablename.Trim());
            object primary = Gateway.Default.FromSql(sql2).ToScalar();
            //将主键加到字段的表中
            DataTable dt = ds.Tables[0];
            dt.Columns.Add(new DataColumn("primary", typeof(int)));
            DataRow parimaryDr = dt.NewRow();  //主键的行
            int parimaryIndex = -1;     //主键的行索引，用以后面与第一行交换位置
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                if (primary != null)
                {
                    if (dr["name"].ToString().Equals(primary.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        dr["primary"] = 1;
                        parimaryDr.ItemArray = dr.ItemArray;
                        parimaryIndex = i;
                    }
                    else
                    {
                        dr["primary"] = 0;
                    }
                }
            }
            if (parimaryIndex > 0)
            {
                dt.Rows[parimaryIndex].ItemArray = dt.Rows[0].ItemArray;
                dt.Rows[0].ItemArray = parimaryDr.ItemArray;
            }
            return dt;
        }
        /// <summary>
        /// 获取表的索引
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：name,tablename,columnName</returns>
        public DataTable DataIndexs(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"SELECT
                        indexname as name,
	                    tablename, 
	                    unnest(REGEXP_MATCHES(indexdef,'btree \(\""(\w[^\)]+)\""')) as columnName,
                        indexdef,*
                    FROM pg_indexes WHERE tablename = '{{tablename}}'; ";
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
    }
}
