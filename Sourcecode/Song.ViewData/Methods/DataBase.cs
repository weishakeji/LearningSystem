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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.XWPF.UserModel;
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
        #region 基本信息
        /// <summary>
        /// 数据库类型，即数据库产品名称，例如Sqlserver或PostgreSql
        /// </summary>
        /// <returns></returns>
        public string DBMS() => Business.Do<IDataBase>().DBMSName;
        /// <summary>
        /// 数据库的名称
        /// </summary>
        /// <returns></returns>
        public string DbName() => Business.Do<IDataBase>().DbName;
        /// <summary>
        /// 数据库大小，单位MB
        /// </summary>
        public float DbSize() => Business.Do<IDataBase>().DbSize();
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
        /// 数据库里所有的表，仅表的名称
        /// </summary>
        /// <returns></returns>
        public List<string> Tables() => Business.Do<IDataBase>().Tables();
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
        /// 表的字段详情，包含字段名称、字段类型、字段长度、字段是否为空
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <returns></returns>
        public JArray Fields(string tablename) => Business.Do<IDataBase>().FieldJArray(tablename);       
        /// <summary>
        /// 数据库总的字段数
        /// </summary>
        /// <returns></returns>
        public int FieldTotal() => Business.Do<IDataBase>().FieldTotal();
        /// <summary>
        /// 数据库所有的数据类型
        /// </summary>
        public List<string> FieldDataTypes() => Business.Do<IDataBase>().DataTypes();
        /// <summary>
        /// 数据库总的索引数
        /// </summary>
        /// <returns></returns>
        public int IndexTotal() => Business.Do<IDataBase>().IndexTotal();
        /// <summary>
        /// 查询字段
        /// </summary>
        /// <param name="dbtype">字段的数据类型</param>
        /// <param name="table">表名称</param>
        /// <param name="field">按字段模糊查询</param>
        /// <returns></returns>
        [Localhost]
        public JObject FieldQuery(string dbtype,string table,string field)
        {
            Dictionary<string, string[]> dic = Business.Do<IDataBase>().FieldQuery(dbtype, table, field);
            JObject jobj = new JObject();
            foreach (var item in dic)
            {               
                jobj.Add(item.Key,new JArray(item.Value));              
            }
            return jobj;
        }
        /// <summary>
        /// 获取某个实体的数据库索引
        /// </summary>
        /// <param name="tablename">数据库表名，这里指对应的实体名称</param>
        /// <returns></returns>
        [Localhost]
        public DataTable Indexs(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            return Business.Do<IDataBase>().Indexs(tablename);  
        }
        /// <summary>
        /// 索引空间的总大小
        /// </summary>
        /// <returns>单位kb</returns>
        public float IndexSize() => Business.Do<IDataBase>().IndexSize();
        #endregion

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
        /// <summary>
        /// 数据库冗余的表或字段
        /// </summary>
        /// <returns></returns>
        public JArray CheckRedundancy()
        {
            bool isCorrect = Business.Do<IDataBase>().CheckConnection();
            if (!isCorrect)
                throw new Exception("数据库链接不正常！");
            JArray jarr = new JArray();
            Dictionary<string, string[]> dic = Business.Do<IDataBase>().CheckRedundancy();
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
        /// <summary>
        /// 检测数据库正确性，即字段类型是否与程序设计一致
        /// </summary>
        /// <returns>数据类型的错误的字段，先是表名，下一级是字段名；如下：<br/>
        /// <code>
        /// "Article": {
        ///                 "Acc_Id": {
        ///                        "original": "double",
        ///                        "correct": "integer",
        ///                       "csharp": "Int32"
        ///               }
        ///        },
        /// </code>
        /// </returns>
        public JObject CheckCorrect()
        {
            Dictionary<string, Dictionary<string, string>> dic = Business.Do<IDataBase>().CheckCorrect();
            JObject jtables = new JObject();
            foreach (KeyValuePair<string, Dictionary<string, string>> item in dic)
            { 
                JObject jfield = new JObject();
                foreach (KeyValuePair<string, string> item2 in item.Value)
                {
                    string[] types = item2.Value.Split(',');
                    JObject jtype= new JObject();
                    jtype.Add("original",types[0]);     //原始类型
                    jtype.Add("correct", types[1]);     //正确的类型
                    jtype.Add("csharp", types[2]);      //C#的类型
                    jfield.Add(item2.Key, jtype);
                }
                jtables.Add(item.Key, jfield);
            }
            return jtables;
        }
        #endregion

        #region 数据实体
        /// <summary>
        /// 数据实体，来自Song.Entities.dll
        /// </summary>
        /// <returns>key值是实体名称，value是字段（字段名、类型）</returns>
        public JObject Entities()
        {
            Dictionary<string, Dictionary<string, Type>> dic = Business.Do<IDataBase>().Entities();
            JObject joentity = new JObject();
            foreach (KeyValuePair<string, Dictionary<string, Type>> item in dic)
            {
                JObject jofield = new JObject();
                foreach (KeyValuePair<string, Type> item2 in item.Value)
                {
                    Type fieldtype = item2.Value;
                    Type nullableType = System.Nullable.GetUnderlyingType(fieldtype);
                    string typename = nullableType != null ? nullableType.Name : fieldtype.Name;
                    jofield.Add(item2.Key, typename);
                }
                joentity.Add(item.Key, jofield);
            }
            return joentity;
        }
        #endregion

        #region 数据库描述信息
        /// <summary>
        /// 表的描述信息，包括名称、简述与说明
        /// </summary>
        /// <returns>例如：{"Accessory":{"mark":"附件表","intro":"..."}}</returns>
        [HttpGet, Admin]
        public JObject TablesDescr() => Business.Do<IDataBase>().TablesDescr();       
        /// <summary>
        /// 保存表的描述信息
        /// </summary>
        /// <param name="detail">所有表的详情说明,json格式</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool TablesDescrUpdate(string detail)
        {
            //读取或写入
            string file = string.Format("{0}help\\datas\\entitiy\\entities.json", AppDomain.CurrentDomain.BaseDirectory);
            if (!string.IsNullOrWhiteSpace(detail))
            {
                JObject temp = (JObject)JsonConvert.DeserializeObject(detail);
                if (temp == null) throw new Exception("要写入的数据不是合法的Json类型");
                string jsonString = JsonConvert.SerializeObject(temp, Newtonsoft.Json.Formatting.Indented);
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(file, false))
                    f.Write(jsonString);
            }
            return true;
        }
        /// <summary>
        /// 表的字段描述信息
        /// </summary>       
        /// <param name="tablename">表名称</param>
        /// <returns>返回实体详情</returns>
        [HttpGet, Localhost, Admin]
        public JObject FieldsDescr(string tablename) => Business.Do<IDataBase>().FieldDescr(tablename);
        /// <summary>
        /// 更新表的字段描述信息
        /// </summary>       
        /// <param name="tablename">表名称</param>
        /// <param name="detail">表的详情说明,json格式</param>
        /// <returns>返回实体详情</returns>
        [HttpPost]
        [Admin]
        public bool FieldsDescrUpdate(string tablename, string detail)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return false;
            string file = string.Format("{0}help\\datas\\Entitiy\\{1}.json", AppDomain.CurrentDomain.BaseDirectory, tablename);
            if (!string.IsNullOrWhiteSpace(detail))
            {
                JObject temp = (JObject)JsonConvert.DeserializeObject(detail);
                if (temp == null) throw new Exception("要写入的数据不是合法的Json类型");
                string jsonString = JsonConvert.SerializeObject(temp, Newtonsoft.Json.Formatting.Indented);
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(file, false))
                    f.Write(jsonString);
            }
            return true;
        }
        #endregion

        #region 实体生成Word文档
        //导出的文件夹名称
        private string _outputPath = "EntitiestoWord";
        //物路路径
        private string outputPathHy
        {
            get
            {
                string path = Path.Combine(WeiSha.Core.Upload.Get["Temp"].Physics, _outputPath);
                Directory.CreateDirectory(path);
                return path;
            }
        }
        //虚拟路径
        private string outputPathVt => WeiSha.Core.Upload.Get["Temp"].Virtual + _outputPath + "/";
        /// <summary>
        /// 将数据库结构导出到Word
        /// </summary>
        /// <returns>导出文件的url</returns>
        public string ExportToWord()
        {
            long snowid=Business.Do<ISystemPara>().SerialSnow();
            string filename = $"数据字典_{DateTime.Now.ToString("yyyy-MM-dd")}.{snowid}.docx";
            filename = Path.Combine(outputPathHy, filename);
            Business.Do<IDataBase>().ExportToWord(filename);
            return outputPathVt + filename;
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        [Admin, Teacher]
        public JArray ExportWordFiles()
        {          
            JArray jarr = new JArray();
            if (!System.IO.Directory.Exists(outputPathHy)) return jarr;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(outputPathHy);
            FileInfo[] files = dir.GetFiles("*.docx")
                .Where(f => (f.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0)
                .OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", outputPathVt + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("type", Path.GetExtension(f.Name).TrimStart('.'));
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpDelete]
        [Admin, Teacher]
        public bool WordFileDelete(string filename)
            => ViewData.Helper.Excel.DeleteFile(filename, outputPathHy);
        #endregion
    }
}
