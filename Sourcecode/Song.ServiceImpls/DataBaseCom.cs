using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Reflection;
using System.Linq;
using System.IO;

namespace Song.ServiceImpls
{
    /// <summary>
    /// 数据库管理
    /// </summary>
    public class DataBaseCom : IDataBase
    {
        #region 基本信息
        /// <summary>
        /// 产品名称，例如PostgreSQL、SQLite
        /// </summary>
        public string DBMSName => Gateway.Default.DbType.ToString();
        /// <summary>
        /// 数据库产品的版本号
        /// </summary>
        public string DbVersion => Gateway.Default.DbVersion();
        /// <summary>
        /// 数据库的库名称
        /// </summary>
        public string DbName => Gateway.Default.DatabaseName();
        /// <summary>
        /// 数据库大小，单位MB
        /// </summary>
        public float DbSize()
        {
            double size = 0;
            if (Gateway.Default.DbType == DbProviderType.PostgreSQL)
            {
                string sql = $"SELECT pg_database_size('{DbName}') / (1024.0 * 1024.0) AS size_mb;";
                object obj = Gateway.Default.FromSql(sql).ToScalar();
                size = obj == null ? 0 : Convert.ToSingle(obj);             
            }
            if (Gateway.Default.DbType == DbProviderType.SQLServer)
            {
                string sql = $"SELECT CAST(SUM(size * 8.0 / 1024) AS DECIMAL(20,2)) FROM sys.master_files where name='{DbName}' GROUP BY name";
                object obj = Gateway.Default.FromSql(sql).ToScalar();
                size = obj == null ? 0 : Convert.ToSingle(obj);
            }
            if (Gateway.Default.DbType == DbProviderType.SQLite)
            {
                string dbfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", DbName);
                long fileSize = new FileInfo(dbfile).Length;
                size = fileSize / (1024.0 * 1024.0);
            }
            return (float)Math.Floor(size * 100) / 100;
        }
        /// <summary>
        /// 检查数据库连接是否正确
        /// </summary>
        public bool CheckConnection() => Gateway.Default.IsCorrect;
        /// <summary>
        /// 数据库里所有的表，仅表的名称
        /// </summary>
        /// <returns></returns>
        public List<string> Tables()
        {
            string sql = string.Empty;
            switch (Gateway.Default.DbType)
            {
                case DbProviderType.SQLServer:
                    sql = @"select name from sysobjects where type='U' order by name asc";
                    break;
                case DbProviderType.PostgreSQL:
                    sql = @"SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname = 'public'  order by tablename";
                    break;
                case DbProviderType.SQLite:
                    sql = @"SELECT name FROM sqlite_master WHERE type = 'table' and name!= 'sqlite_sequence' order by name";
                    break;
            }
            List<string> list = new List<string>();
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {

                while (reader.Read()) list.Add(reader.GetValue<string>(0));
                reader.Close();
                reader.Dispose();
            }
            return list;
        }
        /// <summary>
        /// 数据库所有的数据类型
        /// </summary>
        public List<string> DataTypes()
        {
            string sql = string.Empty;
            //如果是sqlserver数据库，通过遍历获取数据类型
            if (Gateway.Default.DbType == DbProviderType.SQLServer)
            {
                List<string> list = new List<string>();
                foreach(string table in this.Tables())
                {
                    DataTable dtfields = this.Fields(table);
                    foreach(DataRow dr in dtfields.Rows)
                    {
                        string type = dr["type"].ToString();
                        if (!list.Contains(type)) list.Add(type);
                    }
                }
                return list;
            }
            else
            {
                switch (Gateway.Default.DbType)
                {
                    //case DbProviderType.SQLServer:
                    //    sql = @"SELECT DISTINCT t.name AS type_name FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id ORDER BY t.name;";
                    //    break;
                    case DbProviderType.PostgreSQL:
                        sql = @"SELECT DISTINCT unnest(REGEXP_MATCHES(data_type,'^(\w[^\W]+)')) as type FROM information_schema.columns WHERE table_schema NOT IN ('pg_catalog', 'information_schema')";
                        break;
                    case DbProviderType.SQLite:
                        sql = @"WITH tables AS (SELECT name FROM sqlite_master WHERE type = 'table')
                            SELECT DISTINCT 
                                 CASE 
                                    WHEN INSTR(ti.type, '(') > 0 
                                    THEN SUBSTR(ti.type, 1, INSTR(ti.type, '(') - 1)
                                    ELSE ti.type
                                END AS clean_type_name
    
                            FROM tables t, pragma_table_info(t.name) ti
                            WHERE ti.type IS NOT NULL and ti.type!=''
                            ORDER BY ti.type;";
                        break;
                }
                List<string> list = new List<string>();
                using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
                {

                    while (reader.Read()) list.Add(reader.GetValue<string>(0));
                    reader.Close();
                    reader.Dispose();
                }
                return list;
            }
        }
        /// <summary>
        /// 表的字段详情，包含字段名称、字段类型、字段长度、字段是否为空
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public DataTable Fields(string tablename)
        {
            if (Gateway.Default.DbType == DbProviderType.SQLServer) return _sqlserver_Fields(tablename);
            if (Gateway.Default.DbType == DbProviderType.PostgreSQL) return _postgresql_Fields(tablename);
            if (Gateway.Default.DbType == DbProviderType.SQLite) return _sqlite_Fields(tablename);
            return _postgresql_Fields(tablename);
        }
        #region 获取表字段的方法，用于各个数据库
        private DataTable _postgresql_Fields(string tablename)
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
        /// 获取数据字段
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：name,type,length,fulltype,isnullable,primary(主键，非零为真)</returns>
        private DataTable _sqlite_Fields(string tablename)
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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                dr["primary"] = dr["pk"].ToString() == "0" ? 0 : 1;
                if (dr["primary"].ToString() == "1")
                {
                    drmain.ItemArray = dr.ItemArray;
                    dt.Rows[i].Delete();
                }
            }
            if (drmain != null) dt.Rows.InsertAt(drmain, 0);
            //删除冗余行
            if (dt.Columns.Contains("cid")) dt.Columns.Remove("cid");
            if (dt.Columns.Contains("notnull")) dt.Columns.Remove("notnull");
            if (dt.Columns.Contains("pk")) dt.Columns.Remove("pk");
            if (dt.Columns.Contains("dflt_value")) dt.Columns.Remove("dflt_value");
            return dt;
        }
        /// <summary>
        /// 获取数据字段
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：name,type,length,fulltype,isnullable,primary(主键，非零为真)</returns>
        private DataTable _sqlserver_Fields(string tablename)
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
        #endregion
        /// <summary>
        /// 表的字段，仅名称
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public List<string> FieldNames(string tablename)
        {
            string sql = string.Empty;
            switch (Gateway.Default.DbType)
            {
                case DbProviderType.SQLServer:
                    sql = @"SELECT name FROM syscolumns WHERE id = OBJECT_ID('{{tablename}}')";
                    break;
                case DbProviderType.PostgreSQL:
                    sql = @"SELECT column_name as name FROM information_schema.columns WHERE table_name = '{{tablename}}'  order by column_name";
                    break;
                case DbProviderType.SQLite:
                    sql = @"SELECT name FROM pragma_table_info('{{tablename}}') ORDER BY name; ";
                    break;
            }
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            List<string> list = new List<string>();
            using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
            {

                while (reader.Read()) list.Add(reader.GetValue<string>(0));
                reader.Close();
                reader.Dispose();
            }
            return list;
        }
        /// <summary>
        /// 表的字段，类型
        /// </summary>
        /// <returns>key为字段名，value为数据类型</returns>
        public Dictionary<string, string> FieldTypes(string tablename)
        {
            DataTable dt = this.Fields(tablename);
            if (dt == null) return null;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (DataRow dr in dt.Rows)
            {
                string name = dr["name"].ToString();
                string type = dr["type"].ToString();
                if (type.IndexOf("(") > -1) type = type.Substring(0, type.IndexOf("("));
                dic.Add(name, type);
            }
            return dic;
        }
        /// <summary>
        /// 查询字段
        /// </summary>
        /// <param name="dbtype">字段的数据类型</param>
        /// <param name="table">表名称</param>
        /// <param name="field">按字段模糊查询</param>
        /// <returns>key为表名，value为满足条件的字段</returns>
        public Dictionary<string, string[]> FieldQuery(string dbtype, string table, string field)
        {
            //按表查询
            List<string> tables = this.Tables();
            if (!string.IsNullOrWhiteSpace(table))
            {
                for(int i = 0; i < tables.Count; i++)
                {
                    if (!tables[i].Equals(table, StringComparison.CurrentCultureIgnoreCase))
                    {
                        tables.RemoveAt(i);
                        i--;
                    }
                }
            }
            //要返回的值
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            foreach(string tb in tables)
            {
                Dictionary<string, string> fields = this.FieldTypes(tb);
                //如果不在指定的数据类型，则删除
                if (!string.IsNullOrWhiteSpace(dbtype))
                {
                    // 直接遍历并移除符合条件的项
                    var keysToRemove = fields.Where(pair => !pair.Value.Equals(dbtype, StringComparison.CurrentCultureIgnoreCase)).Select(pair => pair.Key).ToList();
                    foreach (var key in keysToRemove) fields.Remove(key);
                }
                //如果不包含指定字段，则删除
                if (!string.IsNullOrWhiteSpace(field))
                {
                    // 直接遍历并移除符合条件的项
                    var keysToRemove = fields.Where(pair => pair.Key.IndexOf(field, StringComparison.CurrentCultureIgnoreCase) < 0).Select(pair => pair.Key).ToList();
                    foreach (var key in keysToRemove) fields.Remove(key);
                }
                if (fields.Count > 0) dic.Add(tb, fields.Keys.ToArray<string>());
            }
            return dic;
        }
        /// <summary>
        /// 指定表的索引
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataTable Indexs(string tablename)
        {
            string sql = string.Empty;
            switch (Gateway.Default.DbType)
            {
                //SQLServer
                case DbProviderType.SQLServer:
                    sql = @"SELECT
                        i.name,                --索引的名称
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
                    break;
                //PostgreSQL
                case DbProviderType.PostgreSQL:
                    sql = @"SELECT 
                                pi.indexname as name,
                                pi.tablename,
                                REPLACE(unnest(REGEXP_MATCHES(pi.indexdef,'btree \(""(\w[^\)]+)')),'""','') as columnName,
                                pi.indexdef,
                                COALESCE(ps.sizekb, 0) as sizekb
                            FROM pg_indexes pi
                            LEFT JOIN(
                                SELECT
                                    indexrelname as name,
                                    pg_relation_size(indexrelid) / 1024 as sizekb
                                FROM pg_stat_all_indexes
                                WHERE relname = '{{tablename}}'
                            ) ps ON pi.indexname = ps.name
                            WHERE pi.schemaname = 'public'
                                AND pi.tablename = '{{tablename}}'
                                AND pi.indexname NOT IN(
                                    SELECT constraint_name
                                    FROM information_schema.table_constraints
                                    WHERE table_name = '{{tablename}}'
                                    AND constraint_type = 'PRIMARY KEY'
                                )
                            ORDER BY columnName; ";
                    break;
                //SQLite
                case DbProviderType.SQLite:
                    sql = @"PRAGMA index_list('{{tablename}}');";
                    break;
            }
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            return ds.Tables[0];
        }
        #endregion

        #region 校验
        /// <summary>
        /// 检测数据库是否有缺，即表和字段是否与程序设计的一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为缺失的字段</returns>
        public Dictionary<string, string[]> CheckFully()
        {
            //所有实体
            Dictionary<string, Dictionary<string, Type>> entities = this.Entities();
            List<string> tables = this.Tables();    //所有表
            //用于记录缺失字段的对象
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            foreach (KeyValuePair<string, Dictionary<string, Type>> item in entities)
            {
                bool istbExist = tables.Contains(item.Key);     //实体是否存在于表
                if (!istbExist) dic.Add(item.Key, new string[] { });    //缺少整个表
                else
                {
                    //对比缺少的字段
                    List<string> fields = this.FieldNames(item.Key);    //表的字段
                    Dictionary<string, Type> props = item.Value;        //实体的属性                                                                        
                    List<string> lack = new List<string>();         //缺少的字段
                    foreach (KeyValuePair<string, Type> item2 in props)
                        if (!fields.Contains(item2.Key)) lack.Add(item2.Key);
                    if (lack.Count > 0) dic.Add(item.Key, lack.ToArray());
                }
            }
            return dic;
        }
        /// <summary>
        /// 检测数据库是否冗余，即表和字段是否与程序设计的一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为缺失的字段</returns>
        public Dictionary<string, string[]> CheckRedundancy()
        {
            //所有实体
            Dictionary<string, Dictionary<string, Type>> entities = this.Entities();
            List<string> tables = this.Tables();    //所有表
            //用于记录冗余信息的对象
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            foreach (string table in tables)
            {
                bool istbExist = entities.ContainsKey(table);     //表是否存在于实体
                if (!istbExist) dic.Add(table, new string[] { });    //整个表都多余
                else
                {
                    //对比冗余的字段
                    List<string> fields = this.FieldNames(table);    //表的字段
                    Dictionary<string, Type> props = entities[table];        //实体的属性                                                                        
                    List<string> redundancy = new List<string>();         //冗余的字段
                    foreach (string field in fields)
                        if (!props.ContainsKey(field)) redundancy.Add(field);
                    if (redundancy.Count > 0) dic.Add(table, redundancy.ToArray());
                }
            }
            return dic;
        }
        /// <summary>
        /// 检测数据库正确性，即字段类型是否与程序设计一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为错误的字段; Value中的key为字段名，value为原数据类型，目标数据类型，对应的C#类型</returns>
        public Dictionary<string, Dictionary<string, string>> CheckCorrect()
        {
            Dictionary<string, Dictionary<string, string>> dic = new Dictionary<string, Dictionary<string, string>>();
            //Postgresql与C#对象的关联关系
            Dictionary<string, string> dbtype_to_csharp = this._dbtype_to_csharp();
            //所有实体,key为表名，
            Dictionary<string, Dictionary<string, Type>> entities = this.Entities();
            List<string> tables = this.Tables();    //所有表
            foreach(string tb in tables)
            {
                //实体的属性与数据类型
                if (!entities.ContainsKey(tb)) continue;
                Dictionary<string, Type> properties = entities[tb];
                //表的字段与数据类型
                Dictionary<string, string> fields = this.FieldTypes(tb); 
                //
                Dictionary<string, string> dicfields = _fields_error(dbtype_to_csharp, fields, properties);
                if (dicfields.Count > 0) dic.Add(tb, dicfields);
            }
            return dic;
        }
        /// <summary>
        /// Postgresql数据类型如果不对应C#类型，则返回
        /// </summary>
        /// <param name="pgtocsharp"></param>
        /// <param name="fields"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        private Dictionary<string, string> _fields_error(Dictionary<string, string> pgtocsharp, Dictionary<string, string> fields, Dictionary<string, Type> properties)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> field in fields)
            {
                string dtype = field.Value;     //字段的数据类型
                string dname = field.Key;
                if (!properties.ContainsKey(dname)) continue;
                //Postgresql数据类型，应该对应的C#对象
                string targettype = _dbtype_to_csharp_value(pgtocsharp, dtype);               
                Type prop = properties[dname];
                Type nullableType = System.Nullable.GetUnderlyingType(prop);
                string typename = nullableType != null ? nullableType.Name : prop.Name;
                if (!typename.Equals(targettype))
                {
                    //应该对应的数据库类型
                    string correcttype= _dbtype_for_csharp_value(pgtocsharp, typename);
                    dic.Add(dname, $"{dtype},{correcttype},{typename}");
                }
            }
            return dic;
        }
       
        /// <summary>
        /// 获取数据库与C#对象的关联关系
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> _dbtype_to_csharp()
        {  
            string dbmsname = this.DBMSName;
            string filename = $"{dbmsname}_to_csharp.txt";
            string filepath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Utilities", "Database", filename);           
            if (!File.Exists(filepath)) throw new Exception(filename + "不存在");
            string text = File.ReadAllText(filepath);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach(string row in text.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(row)|| row.Trim().Length<1) continue;
                if (row.IndexOf("->") < 0) continue;
                string pg = row.Substring(0, row.IndexOf("->")).Trim();
                string csharp = row.Substring(row.IndexOf("->")+2).Trim();
                dic.Add(pg, csharp);
            }
            return dic;
        }
        /// <summary>
        /// 通过Postgresql字段的数据类型，取对应的C#类型
        /// </summary>
        /// <param name="dbtype_to_csharp"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string _dbtype_to_csharp_value(Dictionary<string, string> dbtype_to_csharp, string field)
        {
            string objtype = string.Empty;
            foreach (KeyValuePair<string, string> item in dbtype_to_csharp)
            {
                if (item.Key.Equals(field, StringComparison.CurrentCultureIgnoreCase))
                {
                    objtype = item.Value;
                    break;
                }
            }
            return objtype;
        }
        /// <summary>
        /// 通过字段对应的C#类型，取数据库字段的数据类型
        /// </summary>
        /// <param name="dbtype_to_csharp"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        private string _dbtype_for_csharp_value(Dictionary<string, string> dbtype_to_csharp, string prop)
        {
            string fieldtype = string.Empty;
            foreach (KeyValuePair<string, string> item in dbtype_to_csharp)
            {
                if (item.Value.Equals(prop, StringComparison.CurrentCultureIgnoreCase))
                {
                    fieldtype = item.Key;
                    break;
                }
            }
            return fieldtype;
        }
        #endregion

        #region 数据库信息统计
        /// <summary>
        /// 数据库表的总数
        /// </summary>
        /// <returns></returns>
        public int TableTotal()
        {
            string sql = string.Empty;
            switch (Gateway.Default.DbType)
            {
                case DbProviderType.SQLServer:
                    sql = @"SELECT COUNT(*) AS UserTableCount FROM sys.tables WHERE is_ms_shipped = 0;";
                    break;
                case DbProviderType.PostgreSQL:
                    sql = @"SELECT count(*) FROM pg_tables WHERE schemaname NOT IN ('pg_catalog', 'information_schema');";
                    break;
                case DbProviderType.SQLite:
                    sql = @"SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";
                    break;
            }
            return ScalarSql<int>(sql);
        }
        /// <summary>
        /// 数据库表的记录数
        /// </summary>
        /// <returns>key为表名，value为记录数</returns>
        public Dictionary<string, int> TableCount()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            string sql = string.Empty;
            if (Gateway.Default.DbType == DbProviderType.SQLServer || Gateway.Default.DbType == DbProviderType.PostgreSQL)
            {
                if (Gateway.Default.DbType == DbProviderType.SQLServer)
                {
                    sql = @"SELECT 
                        --SCHEMA_NAME(t.schema_id) AS SchemaName,
                        t.name AS table_name,
                        p.rows AS 'count'
                    FROM 
                        sys.tables t
                    INNER JOIN 
                        sys.partitions p ON t.object_id = p.object_id
                    WHERE 
                        p.index_id IN (0, 1) -- 堆或聚集索引
                        AND t.is_ms_shipped = 0
                    ORDER BY 
                        p.rows DESC;";
                }
                if (Gateway.Default.DbType == DbProviderType.PostgreSQL)
                {
                    sql = @"SELECT 
                            --n.nspname AS schema_name,
                            c.relname AS table_name,
                            c.reltuples AS count
                        FROM 
                            pg_class c
                        JOIN 
                            pg_namespace n ON c.relnamespace = n.oid
                        WHERE 
                            c.relkind = 'r' -- 普通表
                            AND n.nspname NOT IN ('pg_catalog', 'information_schema')
                        ORDER BY 
                            c.reltuples DESC;";

                }
                using (SourceReader reader = Gateway.Default.FromSql(sql).ToReader())
                {
                    while (reader.Read())
                    {
                        string table = reader.GetValue<string>(0);
                        int count = reader.GetValue<int>(1);
                        dic.Add(table, count);
                    }
                    reader.Close();
                    reader.Dispose();
                }
                return dic;
            }
            //遍历表计算，SQLite之能这样         
            List<string> tables = Tables();
            foreach (string tb in tables) dic.Add(tb, TableCount(tb));
            return dic;
        }
        /// <summary>
        /// 数据库表的记录数
        /// </summary>
        /// <param name="tablename">表的名称</param>
        /// <returns></returns>
        public int TableCount(string tablename) => ScalarSql<int>(@"SELECT COUNT(*) FROM """ + tablename + @"""");
        /// <summary>
        /// 数据库总记录数
        /// </summary>
        /// <returns></returns>
        public int TotalCount()
        {
            int total = 0;
            Dictionary<string, int> dic = TableCount();
            foreach (int value in dic.Values) total += value;
            return total;
        }
        /// <summary>
        /// 总的字段数
        /// </summary>
        public int FieldTotal()
        {
            if (Gateway.Default.DbType == DbProviderType.SQLServer)
            {
                string sql = @"SELECT SUM(column_count) AS total_columns
                                FROM (
                                    SELECT COUNT(*) AS column_count
                                    FROM INFORMATION_SCHEMA.COLUMNS
                                    WHERE TABLE_SCHEMA NOT IN ('sys', 'INFORMATION_SCHEMA')
                                    GROUP BY TABLE_SCHEMA, TABLE_NAME
                                ) AS table_columns;";
                return ScalarSql<int>(sql);
            }
            if (Gateway.Default.DbType == DbProviderType.PostgreSQL)
            {
                string sql = @"SELECT SUM(column_count) AS total_columns
                                FROM (
                                    SELECT COUNT(*) AS column_count
                                    FROM information_schema.columns
                                    WHERE table_schema NOT IN ('pg_catalog', 'information_schema')
                                    GROUP BY table_schema, table_name
                                ) AS table_columns;";
                return ScalarSql<int>(sql);
            }
            //通过遍历表计算字段之和，SQLite之能这样
            int total = 0;
            List<string> tables = Tables();
            foreach (string tb in tables) total += FieldCount(tb);
            return total;
        }
        /// <summary>
        /// 表字段数
        /// </summary>
        public int FieldCount(string tablename)
        {
            string sql = string.Empty;
            switch (Gateway.Default.DbType)
            {
                case DbProviderType.SQLServer:
                    sql = @"SELECT COUNT(*) AS column_count FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Accounts';";
                    break;
                case DbProviderType.PostgreSQL:
                    sql = @"SELECT COUNT(*) AS column_count FROM information_schema.columns WHERE table_schema = 'public' AND table_name = '{{tablename}}';";
                    break;
                case DbProviderType.SQLite:
                    sql = @"SELECT COUNT(*) AS column_count FROM pragma_table_info('{{tablename}}');";
                    break;
            }
            sql = sql.Replace("{{tablename}}", tablename.Trim());
            return ScalarSql<int>(sql);
        }
        /// <summary>
        /// 索引总数
        /// </summary>
        /// <returns></returns>
        public int IndexTotal()
        {
            string sql = string.Empty;
            switch (Gateway.Default.DbType)
            {
                case DbProviderType.SQLServer:
                    sql = @"SELECT COUNT(*) FROM sys.indexes WHERE object_id > 100 AND index_id > 0;";
                    break;
                case DbProviderType.PostgreSQL:
                    sql = @"SELECT count(*) FROM pg_indexes WHERE schemaname='public';";
                    break;
                case DbProviderType.SQLite:
                    sql = @"SELECT COUNT(*) FROM sqlite_master WHERE type = 'index';";
                    break;
            }
            return ScalarSql<int>(sql);
        }

        /// <summary>
        /// 索引空间的总大小
        /// </summary>
        /// <returns>单位kb</returns>
        public float IndexSize()
        {
            float size = 0;
            if (Gateway.Default.DbType == DbProviderType.PostgreSQL)
            {
                string sql = "SELECT sum(pg_relation_size(indexrelid)/1024) as sizekb FROM pg_stat_all_indexes";
                object obj = Gateway.Default.FromSql(sql).ToScalar();
                size = obj == null ? 0 : Convert.ToSingle(obj);
            }
            if (Gateway.Default.DbType == DbProviderType.SQLServer)
            {
                string sql = @"SELECT 
                            --SUM(ps.used_page_count) * 8 / 1024.0 AS TotalIndexSizeMB,
                            SUM(ps.reserved_page_count) * 8 AS TotalIndexReservedKB
                        FROM sys.dm_db_partition_stats ps
                        INNER JOIN sys.indexes i ON ps.object_id = i.object_id AND ps.index_id = i.index_id
                        WHERE i.index_id > 0;";
                object obj = Gateway.Default.FromSql(sql).ToScalar();
                size = obj == null ? 0 : Convert.ToSingle(obj);
            }
            if (Gateway.Default.DbType == DbProviderType.SQLite)
            {
                object pagecount = Gateway.Default.FromSql("PRAGMA page_count").ToScalar();
                object pagesize = Gateway.Default.FromSql("PRAGMA page_size").ToScalar();
                size = (pagecount == null ? 0 : Convert.ToSingle(pagecount)) * (pagesize == null ? 0 : Convert.ToSingle(pagesize));
                size = size / 1024 / 1024;
            }
            return (float)Math.Floor(size * 100) / 100;
        }
        #endregion

        #region SQL脚本执行
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql) => Gateway.Default.FromSql(sql).Execute();
        /// <summary>
        /// 执行sql语句，返回第一行第一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回第一行第一列的数据</returns>
        public object ScalarSql(string sql) => Gateway.Default.FromSql(sql).ToScalar();
        /// <summary>
        /// 执行sql语句，返回第一行第一列的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T ScalarSql<T>(string sql)
        {
            object obj = ScalarSql(sql);
            return (T)Convert.ChangeType(obj, typeof(T));
        }
        /// <summary>
        /// 执行sql语句，返回第一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T FirstSql<T>(string sql) where T : WeiSha.Data.Entity => Gateway.Default.FromSql(sql).ToFirst<T>();
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回数据集</returns>
        public List<T> ForSql<T>(string sql) where T : WeiSha.Data.Entity => Gateway.Default.FromSql(sql).ToList<T>();
        /// <summary>
        /// 返回指定的数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ForSql(string sql)
        {
            DataSet ds = Gateway.Default.FromSql(sql).ToDataSet();
            if (ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }
        #endregion

        #region 数据实体
        /// <summary>
        /// 数据实体，来自Song.Entities.dll
        /// </summary>
        /// <returns>key值是实体名称，value是字段（字段名、类型）</returns>
        public Dictionary<string, Dictionary<string, Type>> Entities()
        {
            //反射加载ORM实体程序集
            string assemblyName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "bin", "Song.Entities.dll");
            System.Reflection.Assembly assembly = Assembly.LoadFrom(assemblyName);
            Type[] types = assembly.GetTypes();
            //
            Dictionary<string, Dictionary<string, Type>> dic = new Dictionary<string, Dictionary<string, Type>>();
            Type parent = typeof(WeiSha.Data.Entity);   //父类，必须是Entity类
            foreach (Type t in types)
            {
                if (!t.IsSubclassOf(parent)) continue;
                PropertyInfo[] propertyinfo = t.GetProperties();
                Dictionary<string, Type> dicprop = new Dictionary<string, Type>();
                foreach (PropertyInfo pi in propertyinfo) dicprop.Add(pi.Name, pi.PropertyType);
                dic.Add(t.Name, dicprop);
            }
            return dic;
        }
        /// <summary>
        /// 某个实体的属性
        /// </summary>
        /// <param name="table">表名，与实体名称相同</param>
        /// <returns></returns>
        public Dictionary<string, Type> Properties(string table)
        {
            //反射加载ORM实体程序集
            string assemblyName = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "bin", "Song.Entities.dll");           
            System.Reflection.Assembly assembly = Assembly.LoadFrom(assemblyName);
            Type[] types = assembly.GetTypes();
            Type targetType = types.FirstOrDefault(t => t.Name.Equals(table, StringComparison.CurrentCultureIgnoreCase));
            //
            PropertyInfo[] propertyinfo = targetType.GetProperties();
            Dictionary<string, Type> dicprop = new Dictionary<string, Type>();
            foreach (PropertyInfo pi in propertyinfo) dicprop.Add(pi.Name, pi.PropertyType);            
            return dicprop;
        }
        #endregion
    }
}
