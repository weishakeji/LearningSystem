using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;
using System.Data;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 用于调试
    /// </summary>
    [HttpPost, HttpGet]
    public class Debug : ViewMethod, IViewAPI
    {

        public DataTable Result4StudentSort(int examid)
        {
            return Business.Do<IExamination>().Result4StudentSort(examid);
        }

       public DataTable ForSql(string sql)
        {
            MethodBase method = MethodBase.GetCurrentMethod();

            //object paramValue = method.ReflectedType.GetMethod(method.Name).Invoke(this, new object[] { param1, param2 });

            return Business.Do<ISystemPara>().ForSql(sql);
        }

        /// <summary>
        /// 所有表
        /// </summary>
        /// <returns></returns>
        public List<string> DataTables()
        {
            return Business.Do<ISystemPara>().DataTables();
        }
        /// <summary>
        /// 仅获取下的字段的名称，不包括类型等其它属性
        /// </summary>
        /// <returns></returns>
        public List<string> DataFieldNames(string tablename)
        {
            return Business.Do<ISystemPara>().DataFieldNames(tablename);
        }
        /// <summary>
        /// 获取数据字段
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：name,type,length,fulltype,isnullable,primary(主键，非零为真)</returns>
        public DataTable DataFields(string tablename)
        {
            return Business.Do<ISystemPara>().DataFields(tablename);
        }
        /// <summary>
        /// 获取表的索引
        /// </summary>
        /// <param name="tablename">表名称</param>
        /// <returns>数据列包括：IndexName,TableName,ColumnName,IndexType(CLUSTERED或NONCLUSTERED),IsDescending(1表示降序排序，为0表示升序排序)</returns>
        public DataTable DataIndexs(string tablename)
        {
            return Business.Do<ISystemPara>().DataIndexs(tablename);
        }
    }
}
