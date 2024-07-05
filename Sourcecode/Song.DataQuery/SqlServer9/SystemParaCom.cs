
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

namespace Song.DataQuery.SqlServer9
{
    /// <summary>
    /// 系统参数，以及相关系统级设置
    /// </summary>
    public class SystemParaCom
    {
        /// <summary>
        ///  数据库名称
        /// </summary>
        /// <returns></returns>
        public string DataBaseName()
        {
            object obj = Gateway.Default.FromSql("SELECT DB_NAME() AS DatabaseName").ToScalar();
            if (obj == null) return string.Empty;
            return obj.ToString();
        }
        /// <summary>
        ///  数据库版本号
        /// </summary>
        /// <returns></returns>
        public string DbVersion()
        {
            object version = Gateway.Default.FromSql("select @@version").ToScalar();
            if (version == null) return string.Empty;
            string str = version.ToString();
            str = str.Replace("\n", "").Replace("\t", "").Replace("\r", "");
            return str;
        }
        /// <summary>
        /// 数据库里所有的表
        /// </summary>
        /// <returns></returns>
        public List<string> DataTables()
        {
            string sql = "select name from sysobjects where type='U' order by name asc";
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
        /// 仅获取下的字段的名称，不包括类型等其它属性
        /// </summary>
        /// <param name="tablename">表</param>
        /// <returns></returns>
        public List<string> DataFieldNames(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            string sql = @"SELECT name FROM syscolumns WHERE id = OBJECT_ID('{{tablename}}')";
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
            string sql = @"
                    SELECT name,type_name(xtype) AS type,
                    --长度，无限长为-1
                    length,
                    --是否可空，0为可空
                    isnullable as nullable
                    FROM syscolumns
                    WHERE (id = OBJECT_ID('{{tablename}}'))
                    order by name asc";
            sql = sql.Replace("{{tablename}}", tablename.Trim());

            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            if (ds.Tables.Count < 1) return null;
            //获取主键
            string sql2 = @"
                SELECT distinct	
	                COLUMN_NAME=stuff((
		                SELECT '|'+COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
		                where TABLE_NAME ='{{tablename}}'
                    FOR XML path('')
                    ), 1, 1, '')
                FROM
	                INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                where TABLE_NAME ='{{tablename}}'";
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
        /// <returns>数据列包括：name,TableName,columnName,IndexType(CLUSTERED或NONCLUSTERED),IsDescending(1表示降序排序，为0表示升序排序)</returns>
        public DataTable DataIndexs(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            //下述脚本，仅支持sqlserver
            string sql = @"SELECT
                        i.name AS IndexName,                --索引的名称
                        OBJECT_NAME(i.object_id) AS tablename,
                        c.name AS columnName,               --索引的列
                        is_nullable as nullable,            --是否可以为空
                        i.type_desc AS IndexType,
                        ic.is_descending_key AS IsDescending  --排序方式,0为升序，1为降序
                    FROM
                        sys.indexes i
                    INNER JOIN 
                        sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                    INNER JOIN 
                        sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                    WHERE
                        OBJECT_NAME(i.object_id) = '{{tablename}}'
                    ORDER BY
                        OBJECT_NAME(i.object_id), i.name, ic.key_ordinal";
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
    }
}
