using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 数据库管理
    /// </summary>
    public interface IDataBase : WeiSha.Core.IBusinessInterface
    {
        #region 基本信息
        /// <summary>
        /// 产品名称，例如PostgreSQL、SQLite
        /// </summary>
        string ProductName { get; }       
        /// <summary>
        /// 数据库产品的版本号
        /// </summary>
        string DbVersion { get; }
        /// <summary>
        /// 数据库的库名称
        /// </summary>
        string DbName { get; }
        /// <summary>
        /// 检查数据库连接是否正确
        /// </summary>
        bool CheckConnection();
        /// <summary>
        /// 数据库里所有的表
        /// </summary>
        /// <returns></returns>
        List<string> Tables();
        /// <summary>
        /// 数据库所有的数据类型
        /// </summary>
        List<string> DataTypes();
        /// <summary>
        /// 表的字段
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        DataTable Fields(string tablename);
        /// <summary>
        /// 指定表的索引
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        DataTable Indexs(string tablename);
        #endregion

        #region 校验
        /// <summary>
        /// 检测数据库完整性，即表和字段是否与程序设计的一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为缺失的字段</returns>
        Dictionary<string, string[]> CheckFully();
        /// <summary>
        /// 检测数据库正确性，即字段类型是否与程序设计一致
        /// </summary>
        /// <returns>ictionary类型，Key值为表名称，Value为错误</returns>
        Dictionary<string, Dictionary<string,string>> CheckCorrect();
        #endregion

        #region 数据库信息统计
        /// <summary>
        /// 数据库总记录数
        /// </summary>
        /// <returns></returns>
        int TotalCount();
        /// <summary>
        /// 数据库表的记录数
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        int Count(string tablename);
        /// <summary>
        /// 总的字段数
        /// </summary>
        int FieldTotal();
        /// <summary>
        /// 表字段数
        /// </summary>
        int FieldCount(string tablename);
        #endregion

    }
}
