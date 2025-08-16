using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;



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
        public string ProductName => Gateway.Default.DatabaseName();
        /// <summary>
        /// 数据库产品的版本号
        /// </summary>
        public string DbVersion => Gateway.Default.DbVersion();
        /// <summary>
        /// 数据库的库名称
        /// </summary>
        public string DbName => Gateway.Default.DatabaseName();
        /// <summary>
        /// 检查数据库连接是否正确
        /// </summary>
        public bool CheckConnection() => Gateway.Default.IsCorrect;
        /// <summary>
        /// 数据库里所有的表
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
            switch (Gateway.Default.DbType)
            {
                case DbProviderType.SQLServer:
                    sql = @"SELECT DISTINCT t.name AS type_name FROM sys.columns c JOIN sys.types t ON c.user_type_id = t.user_type_id ORDER BY t.name;";
                    break;
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
        /// <summary>
        /// 表的字段
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public DataTable Fields(string tablename)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 指定表的索引
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataTable Indexs(string tablename)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 校验
        /// <summary>
        /// 检测数据库完整性，即表和字段是否与程序设计的一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为缺失的字段</returns>
        public Dictionary<string, string[]> CheckFully()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 检测数据库正确性，即字段类型是否与程序设计一致
        /// </summary>
        /// <returns>ictionary类型，Key值为表名称，Value为错误</returns>
        public Dictionary<string, Dictionary<string, string>> CheckCorrect()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 数据库信息统计
        /// <summary>
        /// 数据库总记录数
        /// </summary>
        /// <returns></returns>
        public int TotalCount()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 数据库表的记录数
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public int Count(string tablename)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 总的字段数
        /// </summary>
        public int FieldTotal()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 表字段数
        /// </summary>
        public int FieldCount(string tablename)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
