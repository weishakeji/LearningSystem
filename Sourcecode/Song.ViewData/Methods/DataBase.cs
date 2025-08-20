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
        public DataTable Fields(string tablename) => Business.Do<IDataBase>().Fields(tablename);
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
        [HttpGet, Localhost]
        public JObject TablesDescr()
        {
            //数据库所有的表
            List<string> tables = Business.Do<IDataBase>().Tables();
            //获取实体的原有记录项
            JObject details = null;
            string file = string.Format("{0}help\\datas\\entitiy\\entities.json", AppDomain.CurrentDomain.BaseDirectory);
            if (System.IO.File.Exists(file))
                details = (JObject)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(file));
            details = details == null ? new JObject() : details;
            JToken jtoken = details;

            //生成实体的说明列表
            JObject jo = new JObject();
            foreach (string tb in tables)
            {
                JObject temp = null;
                if (details.Count > 0)
                {
                    foreach (JProperty jp in jtoken)
                    {
                        if (jp.Name.Equals(tb, StringComparison.OrdinalIgnoreCase))
                        {
                            temp = (JObject)details[jp.Name];
                            jo.Add(tb, temp);
                            break;
                        }
                    }
                }
                if (temp == null)
                {
                    JObject obj = new JObject();
                    obj.Add("mark", "");
                    obj.Add("intro", "");
                    jo.Add(tb, obj);
                }
            }
            return jo;
        }
        /// <summary>
        /// 保存表的描述信息
        /// </summary>
        /// <param name="detail">所有表的详情说明,json格式</param>
        /// <returns></returns>
        [Localhost]
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
        [HttpGet]
        public JObject FieldsDescr(string tablename)
        {
            string file = string.Format("{0}help\\datas\\Entitiy\\{1}.json", AppDomain.CurrentDomain.BaseDirectory, tablename);
            //if (!System.IO.File.Exists(file)) System.IO.File.Create(file);
            string details = System.IO.File.Exists(file) ? System.IO.File.ReadAllText(file, Encoding.UTF8) : "";
            //获取实体和实体属性的名称
            List<string> list = Business.Do<IDataBase>().FieldNames(tablename);

            JObject jitem = (JObject)JsonConvert.DeserializeObject(details);
            jitem = jitem == null ? new JObject() : jitem;
            foreach (string entity in list)
            {
                bool isexist = false;
                if (jitem.Count > 0)
                {
                    JToken record = jitem;
                    foreach (JProperty jp in record)
                    {
                        if (jp.Name.Equals(entity, StringComparison.OrdinalIgnoreCase))
                        {
                            isexist = true;
                            break;
                        }
                    }
                }
                if (!isexist)
                {
                    JObject obj = new JObject();
                    obj.Add("mark", "");
                    obj.Add("intro", "");
                    obj.Add("relation", "");
                    jitem.Add(entity, obj);
                }
            }
            return jitem;
        }
        /// <summary>
        /// 更新表的字段描述信息
        /// </summary>       
        /// <param name="tablename">表名称</param>
        /// <param name="detail">表的详情说明,json格式</param>
        /// <returns>返回实体详情</returns>
        [Localhost]
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
        /// <summary>
        /// 数据实体生成Word文档
        /// </summary>
        /// <returns></returns>
        public string EntitiestoWord()
        {
            //导出文件的位置
            string pathname = "EntitiestoWord";
            string filePath = WeiSha.Core.Upload.Get["Temp"].Physics + pathname + "\\";
            if (!System.IO.Directory.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);
            string filename = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".docx";

            // 创建一个新的 Word 文档对象
            XWPFDocument doc = new XWPFDocument();
            JObject entities = this.TablesDescr();
            int i = 0;
            foreach (JProperty entity in entities.Properties())
            {
                JObject item = (JObject)entity.Value;
                //一级标题，实体的名称
                XWPFParagraph h1 = doc.CreateParagraph();
                h1.Style = "2";
                h1.SpacingBefore = 20;
                XWPFRun r = h1.CreateRun();
                string title = (++i) + ". " + entity.Name;
                string mark = item["mark"].ToString();      //实体的简述
                string intro = item["intro"].ToString();    //实体的说明
                if (!string.IsNullOrWhiteSpace(mark)) title += "：" + mark;
                r.SetText(title);
                r.FontSize = 16;
                r.IsBold = true;
                if (!string.IsNullOrWhiteSpace(intro))
                    _createParagraph(doc, "说明：" + intro, 360, 14);
                // item.Name 为 键
                // item.Value 为 值

                //实体的属性（即表的字段）
                DataTable fields = this.Fields(entity.Name);   //字段名称与数据类型（来自数据库）
                JObject details = this.FieldsDescr(entity.Name);        //字段说明与简述（来自配置信息）
                Dictionary<string, JObject> dic = new Dictionary<string, JObject>();
                string primarykey = string.Empty;       //主键名称
                DataRow primaryval = null;              //主键的属性
                foreach (DataRow field in fields.Rows)
                {
                    //JObject attr = (JObject)field.Value;
                    //记录主键
                    if (field["primary"].ToString() == "1")
                    {
                        primarykey = field["name"].ToString();
                        primaryval = field;
                        continue;
                    }
                    //合并字段名称与简述信息
                    foreach (JProperty detail in details.Properties())
                    {
                        if (field["name"].ToString().Equals(detail.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            JObject dattr = (JObject)detail.Value;
                            foreach (JProperty t in dattr.Properties())
                            {
                                //attr.Add(t.Name, t.Value.ToString());
                            }
                        }
                    }
                    //dic.Add(field["name"].ToString(), attr);
                }
                if (primaryval != null)
                    _createParagraph(doc, "主键：" + primarykey + " （" + primaryval["type"] + "）", 360, 14);
                ////索引
                //JArray indexs = this.EntityIndexs(entity.Name);
                //if (indexs.Count > 0)
                //{
                //    string indexstr = "索引：";
                //    for (int n = 0; n < indexs.Count; n++)
                //    {
                //        JObject idx = (JObject)indexs[n];
                //        indexstr += idx["name"].ToString();
                //        //indexstr += " (" + idx["order"].ToString() + ")";
                //        if (n < indexs.Count - 1) indexstr += "，";

                //    }                    
                //    _createParagraph(doc, indexstr, 360, 14);
                //}

                //字段输出
                _createParagraph(doc, "字段：", 360, 14);
                int j = 0;
                foreach (KeyValuePair<string, JObject> kv in dic)
                {
                    string name = "(" + (++j) + "). " + kv.Key;
                    string type = kv.Value["type"].ToString();
                    if (type == "nvarchar")
                    {
                        int leng = 0;
                        int.TryParse(kv.Value["length"].ToString(), out leng);
                        if (leng <= 0) type = "nvarchar(max)";
                        else
                            type = "nvarchar(" + (leng / 2) + ")";
                    }
                    name += "  (" + type + ") ";
                    string detailmark = kv.Value["mark"].ToString();
                    if (!string.IsNullOrWhiteSpace(detailmark))
                        name += "：" + detailmark;
                    _createParagraph(doc, name, 720, 14);
                    //关联表
                    string relation = kv.Value["relation"].ToString();
                    if (!string.IsNullOrWhiteSpace(relation))
                        _createParagraph(doc, "关联表：" + relation, 1200, 14);
                    //字段说明
                    string detailintro = kv.Value["intro"].ToString();
                    if (!string.IsNullOrWhiteSpace(detailintro))
                        _createParagraph(doc, "说明：" + detailintro, 1200, 14);

                }

            }

            // 保存文档到文件
            using (FileStream fs = new FileStream(filePath + filename, FileMode.Create, FileAccess.Write))
            {
                doc.Write(fs);
            }
            return WeiSha.Core.Upload.Get["Temp"].Virtual + pathname + "/" + filename;
        }
        private XWPFRun _createParagraph(XWPFDocument doc, string text, int indent, int fontsize)
        {
            //返回值
            XWPFParagraph p = doc.CreateParagraph();
            p.IndentationLeft = indent;
            XWPFRun retnrun = p.CreateRun();
            retnrun.SetText(text);
            retnrun.FontSize = fontsize;
            return retnrun;
        }
        private void _createParagraph(XWPFTableCell cell, string text)
        {
            //返回值
            XWPFParagraph p = cell.AddParagraph();
            //p.IndentationLeft = 50;
            //cell.GetCTTc().AddNewTcPr().noWrap.val = false;
            //cell.GetCTTc().AddNewTcPr().AddNewWrap().val = ST_OnOff.on;
            p.Alignment = ParagraphAlignment.CENTER;
            p.IsWordWrapped = false;
            p.SpacingBefore = 0;
            XWPFRun retnrun = p.CreateRun();
            retnrun.SetText(text);
            retnrun.FontSize = 12;
        }
        #endregion
    }
}
