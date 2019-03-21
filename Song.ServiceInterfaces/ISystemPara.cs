using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 系统参数管理
    /// </summary>
    public interface ISystemPara : WeiSha.Common.IBusinessInterface, System.Collections.IEnumerable
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Add(SystemPara entity);
        /// <summary>
        /// 修改，且立即刷新全局参数
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        void Save(string key, string value);
        /// <summary>
        /// 修改，且是否直接刷新全局参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isRefresh"></param>
        void Save(string key, string value, bool isRefresh);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        /// <param name="unit">参数的单位</param>
        void Save(string key, string value, string unit);
        /// <summary>
        /// 用实例保存
        /// </summary>
        /// <param name="entity"></param>
        void Save(SystemPara entity);
        /// <summary>
        /// 当前参数是否存在（通过参数名判断）
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>如果已经存在，则返回true</returns>
        bool IsExists(SystemPara entity);
        /// <summary>
        /// 刷新全局参数
        /// </summary>
        List<SystemPara> Refresh();
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void Delete(SystemPara entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void Delete(int identify);
        /// <summary>
        /// 删除，按键值
        /// </summary>
        /// <param name="key"></param>
        void Delete(string key);
        /// <summary>
        /// 根据键，获取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValue(string key);
        /// <summary>
        /// 根据键，获取值
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        WeiSha.Common.Param.Method.ConvertToAnyValue this[string key] { get;}        
        /// <summary>
        /// 获取单个实例
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SystemPara GetSingle(int id);
        /// <summary>
        /// 获取单个实例，通过键值获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        SystemPara GetSingle(string key);
        /// <summary>
        /// 获取所有参数
        /// </summary>
        /// <returns></returns>
        DataTable GetAll();
        /// <summary>
        /// 查询获取参数
        /// </summary>
        /// <param name="searKey">键名</param>
        /// <param name="searIntro">参数说明</param>
        /// <returns></returns>
        DataTable GetAll(string searKey, string searIntro);
        /// <summary>
        /// 生成流水号
        /// </summary>
        /// <returns></returns>
        string Serial();
        /// <summary>
        /// 测试是否完成授权
        /// </summary>
        bool IsLicense();
        /// <summary>
        /// 数据库完整性测试
        /// </summary>
        /// <returns>返回缺少的表与字段</returns>
        List<string> DatabaseCompleteTest();
        /// <summary>
        /// 数据库链接测试
        /// </summary>
        /// <returns>链接正确为true，否则为false</returns>
        bool DatabaseLinkTest();
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
        /// 执行sql语句，返回第一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T ScalarSql<T>(string sql) where T : WeiSha.Data.Entity;
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

    }
}
