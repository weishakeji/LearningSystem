using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 数据库相关
    /// </summary>
    public class DataBase : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 数据库类型，即数据库产品名称，例如Sqlserver或PostgreSql
        /// </summary>
        /// <returns></returns>
        public string DbType() => Business.Do<IDataBase>().ProductName;
        /// <summary>
        /// 数据库的名称
        /// </summary>
        /// <returns></returns>
        public string DbName() => Business.Do<IDataBase>().DbName;
        /// <summary>
        /// 数据库版本
        /// </summary>
        /// <returns></returns>
        public string DbVersion() => Business.Do<IDataBase>().DbVersion;
        /// <summary>
        /// 数据库是否链接正常
        /// </summary>
        /// <returns></returns>
        public bool CheckConn() => Business.Do<IDataBase>().CheckConnection();
        /// <summary>
        /// 平台所有数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> TableCount() => Business.Do<IDataBase>().TableCount();
        /// <summary>
        /// 数据记录的总数
        /// </summary>
        /// <returns></returns>
        public int DataTotal() => WeiSha.Data.Gateway.Default.Total;
        /// <summary>
        /// 数据库的初始时间
        /// </summary>
        /// <returns></returns>
        public DateTime InitDate()
        {
            DateTime? date = WeiSha.Core.Database.InitialDate;
            return (DateTime)date;
        }
        /// <summary>
        /// 数据库总的字段数
        /// </summary>
        /// <returns></returns>
        public int FieldTotal() => Business.Do<IDataBase>().FieldTotal();
        /// <summary>
        /// 数据库总的索引数
        /// </summary>
        /// <returns></returns>
        public int IndexTotal() => Business.Do<IDataBase>().IndexTotal();

        #region 校验数据库
        /// <summary>
        /// 数据库的字段与表，相较于业务系统，是否有缺失
        /// </summary>
        /// <returns>string, string[],前者为表名，后者为字段</returns>
        public JArray CheckFully()
        {
            bool isCorrect = Business.Do<IDataBase>().CheckConnection();
            if (!isCorrect)
                throw new Exception("数据库链接不正常！");
            JArray jarr = new JArray();
            Dictionary<string, string[]> dic = Business.Do<IDataBase>().CheckFully();
            foreach (KeyValuePair<string, string[]> item in dic)
            {
                JObject jo = new JObject();
                JArray jval = new JArray();
                foreach (string s in item.Value) jval.Add(s);
                jo.Add(item.Key, jval);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion
    }
}
