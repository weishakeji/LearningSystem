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
        string DBMSName { get; }       
        /// <summary>
        /// 数据库产品的版本号
        /// </summary>
        string DbVersion { get; }
        /// <summary>
        /// 数据库的库名称
        /// </summary>
        string DbName { get; }
        /// <summary>
        /// 数据库大小，单位MB
        /// </summary>
        float DbSize();
        /// <summary>
        /// 检查数据库连接是否正确
        /// </summary>
        bool CheckConnection();
        /// <summary>
        /// 数据库里所有的表，仅表的名称
        /// </summary>
        /// <returns></returns>
        List<string> Tables();
        /// <summary>
        /// 数据库所有的数据类型
        /// </summary>
        List<string> DataTypes();
        /// <summary>
        /// 表的字段详情，包含字段名称、字段类型、字段长度、字段是否为空
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        DataTable Fields(string tablename);
        /// <summary>
        /// 表的字段，仅名称
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        List<string> FieldNames(string tablename);
        /// <summary>
        /// 表的字段，类型
        /// </summary>
        /// <returns>key为字段名，value为数据类型</returns>
        Dictionary<string,string> FieldTypes(string tablename);
        /// <summary>
        /// 查询字段
        /// </summary>
        /// <param name="dbtype">字段的数据类型</param>
        /// <param name="table">表名称</param>
        /// <param name="field">按字段模糊查询</param>
        /// <returns>key为表名，value为满足条件的字段</returns>
        Dictionary<string, string[]> FieldQuery(string dbtype, string table, string field);
        /// <summary>
        /// 指定表的索引
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        DataTable Indexs(string tablename);
        #endregion

        #region 校验
        /// <summary>
        /// 检测数据库是否有缺失，即表和字段是否与程序设计的一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为缺失的字段</returns>
        Dictionary<string, string[]> CheckFully();
        /// <summary>
        /// 检测数据库是否冗余，即表和字段是否与程序设计的一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为缺失的字段</returns>
        Dictionary<string, string[]> CheckRedundancy();
        /// <summary>
        /// 检测数据库正确性，即字段类型是否与程序设计一致
        /// </summary>
        /// <returns>Dictionary类型，Key值为表名称，Value为错误的字段; Value中的key为字段名，value为原数据类型，目标数据类型，对应的C#类型</returns>
        Dictionary<string, Dictionary<string,string>> CheckCorrect();
        #endregion

        #region 数据库信息统计
        /// <summary>
        /// 数据库表的数量
        /// </summary>
        /// <returns></returns>
        int TableTotal();
        /// <summary>
        /// 数据库中各个表的记录数
        /// </summary>
        /// <returns>key为表名，value为记录数</returns>
        Dictionary<string,int> TableCount();
        /// <summary>
        /// 数据库表的记录数
        /// </summary>
        /// <param name="tablename">表的名称</param>
        /// <returns></returns>
        int TableCount(string tablename);
        /// <summary>
        ///  数据库表的总记录，即所有表的记录数之和
        /// </summary>
        /// <returns></returns>
        int TotalCount();       
        /// <summary>
        /// 总的字段数
        /// </summary>
        int FieldTotal();
        /// <summary>
        /// 表字段数
        /// </summary>
        int FieldCount(string tablename);
        /// <summary>
        /// 索引总数
        /// </summary>
        /// <returns></returns>
        int IndexTotal();
        #endregion

        #region SQL脚本执行
        /// <summary>
        /// 执行sql语句,返回影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回影响的行数</returns>
        int ExecuteSql(string sql);
        /// <summary>
        /// 执行sql语句，返回第一行第一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回第一行第一列的数据</returns>
        object ScalarSql(string sql);
        /// <summary>
        /// 执行sql语句，返回第一行第一列的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T ScalarSql<T>(string sql);
        /// <summary>
        /// 执行sql语句，返回第一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T FirstSql<T>(string sql) where T : WeiSha.Data.Entity;
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>返回数据集</returns>
        List<T> ForSql<T>(string sql) where T : WeiSha.Data.Entity;

        /// <summary>
        /// 返回指定的数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable ForSql(string sql);
        #endregion

        #region 数据实体
        /// <summary>
        /// 数据实体，来自Song.Entities.dll
        /// </summary>
        /// <returns>key值是实体名称，value是字段（字段名、类型）</returns>
        Dictionary<string, Dictionary<string, Type>> Entities();
        /// <summary>
        /// 某个实体的属性
        /// </summary>
        /// <param name="table">表名，与实体名称相同</param>
        /// <returns></returns>
        Dictionary<string, Type> Properties(string table);
        #endregion
    }
}
