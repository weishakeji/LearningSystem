using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Reflection;
using System.Xml;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using NPOI.XWPF.UserModel;
using System.IO;

namespace Song.ViewData.Methods
{

    /// <summary>
    /// 接口方法的帮助
    /// </summary> 
    [HttpPut, HttpGet]
    public class Helper : ViewMethod, IViewAPI
    {
        #region 接口说明
        /// <summary>
        /// 接口方法列表
        /// </summary>
        /// <returns></returns>
        [HttpPost][HttpGet][HttpPut]
        [Cache]
        [Localhost]
        public Helper_API[] APIList()
        {           
            List<Helper_API> list = new List<Helper_API>();
            string assemblyName = "Song.ViewData";
            Assembly assembly = Assembly.Load(assemblyName);
            //取注释       
            XmlNodeList nodes = _readXml();
            Type[] types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IViewAPI)))
                .ToArray();
            foreach (Type info in types)
            {
                string intro = string.Empty;
                if (nodes != null)
                {
                    foreach (XmlNode n in nodes)
                    {
                        string name = n.Attributes["name"].Value;
                        if (("T:" + info.FullName).Equals(name))
                        {
                            intro = n.InnerText.Trim();
                            break;
                        }
                    }
                }
                list.Add(new Helper_API()
                {
                    Name = info.Name,
                    Intro = intro
                });
            }
            list.Sort((a, b) => a.Name.CompareTo(b.Name));
            return list.ToArray<Helper_API>();
        }

        /// <summary>
        /// 某个接口类下的方法
        /// </summary>
        /// <param name="classname">类名称</param>
        /// <returns></returns>
        /// <remarks>备注信息</remarks>
        /// <example><![CDATA[
        /// 
        ///  
        /// ]]></example>
        /// <exception cref="System.Exception">异常</exception>
        [HttpGet]
        [Localhost]
        public Helper_API_Method[] APIMethods(string classname)
        {
            string assemblyName = "Song.ViewData";
            string classFullName = String.Format("{0}.Methods.{1}", assemblyName, classname);
            Assembly assembly = Assembly.Load(assemblyName);
            //当前类的反射对象
            Type classtype = null;
            foreach (Type info in assembly.GetExportedTypes())
            {
                if (info.FullName.Equals(classFullName, StringComparison.CurrentCultureIgnoreCase))
                {
                    classtype = info;
                    break;
                }
            }
            if (classtype == null)
            {
                throw new Exception("接口类：" + classFullName + " 不存在");
            }
            //注释文档
            XmlNodeList nodes = _readXml();
            //类下面的方法，仅获取当前类生成的方法，不包括父类
            List<Helper_API_Method> list = new List<Helper_API_Method>();
            MemberInfo[] mis = classtype.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo mi in mis)
            {
                string fullname = Helper_API_Method.GetFullName(mi);        //带参数的方法名称    
                //方法的注释
                XmlNode node = Helper_API_Method.GetNode(mi, nodes);
                list.Add(new Helper_API_Method()
                {
                    Name = mi.Name,
                    FullName = fullname,
                    Paras = Helper_API_Method_Para.GetParas(mi, node),
                    Return = Helper_API_Method_Return.GetReturn(mi, node),
                    ClassName = mi.DeclaringType.Name,
                    Class = mi.DeclaringType.FullName,
                    Intro = Helper_API_Method.GetHelp(node, "summary"),
                    Remarks = Helper_API_Method.GetHelp(node, "remarks"),
                    Example = Helper_API_Method.GetHelp(node, "example"),
                    Attrs = Helper_API_Method_Attr.GetAttrs(mi)
                });
            }
            //按方法名排序
            list.Sort((a, b) => a.Name.CompareTo(b.Name));
            return list.ToArray<Helper_API_Method>();
        }

        private XmlNodeList _readXml()
        {
            XmlNodeList nodes = null;
            string file = WeiSha.Core.Server.MapPath("/bin/Song.ViewData.XML");
            if (!System.IO.File.Exists(file)) return nodes;
            XmlDocument xml = new XmlDocument();
            xml.Load(file);
            nodes = xml.SelectNodes("/doc/members/member");
            return nodes;
        }
        /// <summary>
        /// API输出为word文档，将Song.ViewData项目中所有的 RESTful API 接口方法输出为word文档
        /// </summary>
        /// <returns>word文档的url路径</returns>
        public string APItoWord()
        {
            //导出文件的位置
            string pathname = "APItoWord";
            string filePath = WeiSha.Core.Upload.Get["Temp"].Physics + pathname + "\\";
            if (!System.IO.Directory.Exists(filePath))
                System.IO.Directory.CreateDirectory(filePath);
            string filename = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".docx";

            // 创建一个新的 Word 文档对象
            XWPFDocument doc = new XWPFDocument();

            Helper_API[] apis = this.APIList();
            for(int i = 0; i < apis.Length; i++)
            {
                //一级标题，接口类的名称
                XWPFParagraph h1 = doc.CreateParagraph();
                h1.Style = "2";
                h1.SpacingBefore = 20;
                XWPFRun r = h1.CreateRun();
                r.SetText((i+1)+". " +apis[i].Name);
                r.FontSize = 16;
                r.IsBold = true;
                //接口类的说明
                if (!string.IsNullOrWhiteSpace(apis[i].Intro))              
                    _createParagraph(doc, "简述：" + apis[i].Intro, 720, 14);
                //接口方法
                Helper_API_Method[] methods = this.APIMethods(apis[i].Name);
                for (int j= 0; j < methods.Length; j++)
                {
                    //接口方法的名称
                    string order = (i + 1) + "." + (j + 1) + ". ";
                    XWPFRun name = _createParagraph(doc, order + apis[i].Name + " / " + methods[j].Name, 360, 14);
                    name.IsBold = true;
                    //接口方法的说明
                    if (!string.IsNullOrWhiteSpace(methods[j].Intro))                   
                        _createParagraph(doc, "摘要：" + methods[j].Intro, 720, 14); 
                    //特性
                    if (methods[j].Attrs.Length > 0)
                    {                       
                        string attrstext = string.Empty;
                        foreach (Helper_API_Method_Attr s in methods[j].Attrs)
                        {
                            attrstext += "[" + s.Name;
                            if (s.Expires > 0) attrstext += "(Expires = " + s.Expires + ")";
                            attrstext += "] ";
                        }
                        _createParagraph(doc, "特性：" + attrstext, 720, 14);
                    }
                    //请求地址
                    _createParagraph(doc, "请求地址：/api/v2/"+ apis[i].Name.ToLower() + "/" + methods[j].Name.ToLower(), 720, 14);
                    //参数
                    if (methods[j].Paras.Length < 1)
                    {
                        _createParagraph(doc, "参数：（无）", 720, 14);
                    }
                    else
                    {
                        _createParagraph(doc, "参数：", 720, 14);
                        Helper_API_Method_Para[] paras = methods[j].Paras;
                        for (int n = 0; n < paras.Length; n++)
                        {
                            string ptext = "(" + (n + 1) + ") " + paras[n].Name+ "：";
                            ptext += paras[n].Type+"，";
                            ptext += paras[n].Nullable ? "可以为空" : "不可为空";                           
                            if (!string.IsNullOrEmpty(paras[n].Intro))
                                ptext += "；" + paras[n].Intro;
                            _createParagraph(doc, ptext, 720, 14);
                        }
                    }
                    //返回值
                    _createParagraph(doc, "返回值说明：" + (string.IsNullOrWhiteSpace(methods[j].Return.Intro) ? "（无）" : methods[j].Return.Intro), 720, 14);
                    _createParagraph(doc, "返回值类型：" + methods[j].Return.Type, 720, 14);
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

        #region 数据字典
        /// <summary>
        /// 保存数据实体的列表
        /// </summary>
        /// <param name="detail">实体的详情说明,json格式</param>
        /// <returns></returns>
        [Localhost]
        [HttpPost]
        [Admin]
        public bool EntitiesUpdate(string detail)
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
        /// 数据实体的信息，包括名称、简述与说明
        /// </summary>
        /// <returns>例如：{"Accessory":{"mark":"附件表","intro":"..."}}</returns>
        [HttpGet, Localhost]
        public JObject Entities()
        {
            //数据库所有的表
            List<string> entities = Business.Do<IDataBase>().Tables();
            //获取实体的原有记录项
            JObject details = null;
            string file = string.Format("{0}help\\datas\\entitiy\\entities.json", AppDomain.CurrentDomain.BaseDirectory);
            if (System.IO.File.Exists(file))
                details = (JObject)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(file));
            details = details == null ? new JObject() : details;
            JToken jtoken = details;

            //生成实体的说明列表
            JObject jo = new JObject();
            foreach (string entity in entities)
            {
                JObject temp = null;
                if (details.Count > 0)
                {
                    foreach (JProperty jp in jtoken)
                    {
                        if (jp.Name.Equals(entity, StringComparison.OrdinalIgnoreCase))
                        {
                            temp = (JObject)details[jp.Name];
                            jo.Add(entity, temp);
                            break;
                        }
                    }
                }
                if (temp == null)
                {
                    JObject obj = new JObject();
                    obj.Add("mark", "");
                    obj.Add("intro", "");
                    jo.Add(entity, obj);
                }
            }
            return jo;
        }
        /// <summary>
        /// 获取某个实体的数据类型
        /// </summary>
        /// <param name="tablename">数据库表名，这里指对应的实体名称</param>
        /// <returns></returns>
        [HttpPost]
        [Localhost]
        public JObject EntityFields(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;

            System.Data.DataTable dt = Business.Do<ISystemPara>().DataFields(tablename);
            JObject fields = new JObject();
            foreach (DataRow dr in dt.Rows)
            {
                JObject pattr = new JObject();
                for (int i = 1; i < dt.Columns.Count; i++)
                {                    
                    pattr.Add(dt.Columns[i].ColumnName, dr[dt.Columns[i].ColumnName].ToString());
                }
                fields.Add(dr["name"].ToString(), pattr);
            }
            return fields;
        }
        /// <summary>
        /// 获取某个实体的数据库索引
        /// </summary>
        /// <param name="tablename">数据库表名，这里指对应的实体名称</param>
        /// <returns></returns>
        [HttpPost]
        [Localhost]
        public DataTable EntityIndexs(string tablename)
        {
            if (string.IsNullOrWhiteSpace(tablename)) return null;
            return Business.Do<ISystemPara>().DataIndexs(tablename);
            //List<string> list = new List<string>();
            //JArray jarr = new JArray();
            //foreach (DataRow row in dt.Rows)
            //{
            //    //索引的列名
            //    string column = row["ColumnName"].ToString();
            //    bool isexist = false;
            //    foreach(string s in list)
            //    {
            //        if (s.Equals(column, StringComparison.OrdinalIgnoreCase))
            //        {
            //            isexist = true;
            //            break;
            //        }
            //    }
            //    if (!isexist)
            //    {
            //        list.Add(column);
            //        JObject jo = new JObject();
            //        jo.Add("name", column);
            //        //int descending = 0;
            //        //int.TryParse(row["IsDescending"]?.ToString(),out descending);
            //        //jo.Add("order", descending == 0 ? "升序" : "降序");
            //        jarr.Add(jo);
            //    }
            //}           
            //return jarr;
        }        
        /// <summary>
        /// 实体详细说明的获取
        /// </summary>       
        /// <param name="name">实体名称</param>
        /// <returns>返回实体详情</returns>
        [HttpGet]
        public JObject EntityDetails(string name)
        {
            string file = string.Format("{0}help\\datas\\Entitiy\\{1}.json", AppDomain.CurrentDomain.BaseDirectory, name);
            //if (!System.IO.File.Exists(file)) System.IO.File.Create(file);
            string details = System.IO.File.Exists(file) ? System.IO.File.ReadAllText(file, Encoding.UTF8) : "";
            //获取实体和实体属性的名称
            List<string> list = Business.Do<ISystemPara>().DataFieldNames(name);           

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
        /// 实体详细说明的写入
        /// </summary>       
        /// <param name="name">实体名称</param>
        /// <param name="detail">实体的详情说明,json格式</param>
        /// <returns>返回实体详情</returns>
        [Localhost]
        [HttpPost]
        [Admin]
        public bool EntityDetails(string name, string detail)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            string file = string.Format("{0}help\\datas\\Entitiy\\{1}.json", AppDomain.CurrentDomain.BaseDirectory, name);
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
            JObject entities = this.Entities();
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
                JObject fields = this.EntityFields(entity.Name);   //字段名称与数据类型（来自数据库）
                JObject details = this.EntityDetails(entity.Name);        //字段说明与简述（来自配置信息）
                Dictionary<string, JObject> dic = new Dictionary<string, JObject>();
                string primarykey = string.Empty;       //主键名称
                JObject primaryval = null;              //主键的属性
                foreach (JProperty field in fields.Properties())
                {
                    JObject attr = (JObject)field.Value;
                    //记录主键
                    if (attr["primary"].ToString() == "1")
                    {
                        primarykey = field.Name;
                        primaryval = attr;
                        continue;
                    }
                    //合并字段名称与简述信息
                    foreach (JProperty detail in details.Properties())
                    {
                        if (field.Name.Equals(detail.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            JObject dattr = (JObject)detail.Value;
                            foreach (JProperty t in dattr.Properties())
                            {
                                attr.Add(t.Name, t.Value.ToString());
                            }
                        }
                    }
                    dic.Add(field.Name, attr);
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
                    string name = "("+(++j) + "). " + kv.Key;
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
        #endregion

        #region 生成验证码
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="leng">验证码长度</param>
        /// <param name="acc">账号</param>
        /// <returns>返回类为json类型，value:加密后的校检码，验证时提交到服务端；base64：图片的base64编码</returns>
        [HttpPost]
        public Dictionary<string, string> CheckCodeImg(int leng, string acc)
        {
            return CodeImg(leng, 1, acc);
        }
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="leng">长度</param>
        /// <param name="type">类型:0为数字与大小写字母，1为纯数字，2为纯小字母，3为纯大写字母，4为大小写字母，5数字加小写，6数字加大写</param>
        /// <param name="acc"></param>
        /// <returns></returns>
        [HttpPost]
        public Dictionary<string, string> CodeImg(int leng, int type, string acc)
        {
            if (leng <= 0) throw new Exception("长度不得小于等于零");
            //设定生成几位随机数
            string tmp = RndNum(leng, type);
            string val = ConvertToAnyValue.Create(acc + tmp).MD5;
            //生成图片
            System.Drawing.Bitmap image = CreateImage(tmp);
            string base64 = WeiSha.Core.Images.ImageTo.ToBase64(image);
            //
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("value", val);
            dic.Add("base64", "data:image/Gif;base64," + base64);
            return dic;
        }
        /// <summary>
        /// 生成随机字符串，可以选择长度与类型
        /// </summary>
        /// <param name="VcodeNum">随机字符串的长度</param>
        /// <param name="type">生成的随机数类型，0为数字与大小写字母，1为纯数字，2为纯小字母，3为纯大写字母，4为大小写字母，5数字加小写，6数字加大写，</param>
        /// <returns></returns>
        private string RndNum(int VcodeNum, int type)
        {
            string Vchar;
            //数字串
            string num = "0,1,2,3,4,5,6,7,8,9,";
            //小写字母
            string lower = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            //大写字母
            string upper = lower.ToUpper();
            switch (type)
            {
                case 0:
                    Vchar = num + lower + upper;
                    break;
                case 1:
                    Vchar = num;
                    break;
                case 2:
                    Vchar = lower;
                    break;
                case 3:
                    Vchar = upper;
                    break;
                case 4:
                    Vchar = upper + lower;
                    break;
                case 5:
                    Vchar = num + lower;
                    break;
                case 6:
                    Vchar = num + upper;
                    break;
                default:
                    Vchar = num + lower + upper;
                    break;
            }
            Vchar = Vchar.Substring(0, Vchar.Length - 1);
            string[] VcArray = Vchar.Split(new Char[] { ',' });
            string VNum = "";
            Random rand = new Random();
            for (int i = 1; i < VcodeNum + 1; i++)
            {
                VNum += VcArray[rand.Next(VcArray.Length)];
            }
            return VNum;
        }
        /// <summary>
        /// 生成验证码的图片，将字符串填充到图片，并输出到数据流
        /// </summary>
        /// <param name="checkCode">需要生成图片的字符串</param>
        private System.Drawing.Bitmap CreateImage(string checkCode)
        {
            int iwidth = (int)(checkCode.Length * 13);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(iwidth, 18);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点

            for (int i = 0; i < 20; i++)
            {
                int x = rand.Next(image.Width);
                int y = rand.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }

            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < checkCode.Length; i++)
            {
                int cindex = rand.Next(c.Length);
                int findex = rand.Next(font.Length);

                System.Drawing.Font f = new System.Drawing.Font(font[findex], 12, System.Drawing.FontStyle.Bold);
                Brush b = new System.Drawing.SolidBrush(c[cindex]);
                //字符上下位置不同
                int ii = 0;
                if ((i + 1) % 2 == 0)
                {
                    ii = 1;
                }
                g.DrawString(checkCode.Substring(i, 1), f, b, 1 + (i * 13), ii);
            }
            //画一个边框
            //g.DrawRectangle(new Pen(Color.Black,0),0,0,image.Width-1,image.Height-1);

            //输出到浏览器
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            //image.Dispose();
            return image;
        }
        #endregion

        #region 获取地理信息

        /// <summary>
        /// 通过经纬度，获取地理信息
        /// </summary>
        /// <param name="lng">经度</param>
        /// <param name="lat">纬度</param>
        /// <returns>Province省份，City城市，District县区，Street街道</returns>
        public JObject Position(string lng,string lat)
        {
            //解析地址
            WeiSha.Core.Param.Method.Position posi = WeiSha.Core.Request.Position(lng, lat);  
            JObject jo = new JObject();
            jo.Add("Province", posi.Province);
            jo.Add("City", posi.City);
            jo.Add("District", posi.District);
            jo.Add("Street", posi.Street);
            return jo;
        }
        #endregion
      
    }
    #region 一些需要用到的类
    //接口类
    public class Helper_API
    {
        public string Name { get; set; }
        public string Intro { get; set; }
    }
    //接口类中的方法 
    public class Helper_API_Method
    {
        public string Name { get; set; }        //方法名   
        public string FullName { get; set; }    //方法全名         
        public string Intro { get; set; }       //方法摘要说明
        public string Remarks { get; set; }       //方法备注说明
        public string Example { get; set; }       //方法的示例
        public Helper_API_Method_Attr[] Attrs { get; set; }          //方法的特性
        public Helper_API_Method_Para[] Paras { get; set; }         //方法的参数
        public Helper_API_Method_Return Return { get; set; }      //返回值的类型
        public string ClassName { get; set; }    //方法所的类的名称
        public string Class { get; set; }       //方法所的类的完整名称
        public static string GetHelp(XmlNode node, string txt)
        {
            string intro = string.Empty;
            if (node == null) return string.Empty;
            XmlNode n = node.SelectSingleNode(txt);
            if (n == null) return string.Empty;
            return n.InnerText.Trim();
        }
        //方法的完整名，包括方法名+(参数)
        public static string GetFullName(MethodInfo mi)
        {
            string paras = Helper_API_Method_Para.GetParaString(mi);
            if (paras.Length < 1) return string.Format("{0}.{1}", mi.ReflectedType.FullName, mi.Name);
            return string.Format("{0}.{1}({2})", mi.ReflectedType.FullName, mi.Name, paras);
        }
        //获取方法的注释节点
        public static XmlNode GetNode(MethodInfo mi, XmlNodeList nodes)
        {
            if (nodes == null) return null;
            XmlNode node = null;
            string fullname = GetFullName(mi);
            foreach (XmlNode n in nodes)
            {
                if (n.Attributes["name"].Value.EndsWith(fullname))
                {
                    node = n;
                    break;
                }
            }
            return node;
        }
    }
    //方法的返回值
    public class Helper_API_Method_Return
    {
        //返回值的类型
        public string Type { get; set; }
        private string _intro = string.Empty;
        //返回值的摘要
        public string Intro { get
            {
                return _intro;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _intro = value.Trim();
            }
        }
        //是否可为空 
        public bool Nullable { get; set; }   
        public static Helper_API_Method_Return GetReturn(MethodInfo method, XmlNode node)
        {
            Helper_API_Method_Return ret = new Helper_API_Method_Return();
            if (node != null)
            {
                if (node.SelectSingleNode("returns") != null)
                    ret.Intro = node.SelectSingleNode("returns").InnerText;   //返回值的摘要                
            }          
            Type nullableType = System.Nullable.GetUnderlyingType(method.ReturnParameter.ParameterType);
            ret.Type = nullableType != null ? nullableType.FullName + "?" : method.ReturnParameter.ToString();
            if (ret.Type.IndexOf("System.Collections.Generic.List`1[") > -1)
            {
                ret.Type = ret.Type.Replace("System.Collections.Generic.List`1[","List<");
                ret.Type = ret.Type.Replace("]", ">");
            }
            if (ret.Type.IndexOf("System.Collections.Generic.Dictionary`2[") > -1)
            {
                ret.Type = ret.Type.Replace("System.Collections.Generic.Dictionary`2[", "Dictionary<");
                ret.Type = ret.Type.Replace("]", ">");
            }
            ret.Nullable = nullableType != null;
            return ret;
        }
    }
    //方法的参数
    public class Helper_API_Method_Para
    {
        public string Name { get; set; }    //参数名称
        public string Type { get; set; }        //参数数据类型
        public bool Nullable { get; set; }    //是否可为空 
        public string Intro { get; set; }       //参数的摘要
        public static Helper_API_Method_Para[] GetParas(MethodInfo method)
        {
            ParameterInfo[] paramInfos = method.GetParameters();
            Helper_API_Method_Para[] paras = new Helper_API_Method_Para[paramInfos.Length];
            for (int i = 0; i < paramInfos.Length; i++)
            {
                ParameterInfo pi = paramInfos[i];
                paras[i] = new Helper_API_Method_Para();
                paras[i].Name = pi.Name;
                Type nullableType = System.Nullable.GetUnderlyingType(pi.ParameterType);
                if (nullableType == null)
                {                   
                    paras[i].Type = pi.ParameterType.FullName;
                    paras[i].Nullable = false;
                }
                else
                {
                    paras[i].Type = nullableType.FullName + "?";
                    paras[i].Nullable = true;
                }     
                //如果是字符串型，都可以为空
                if(pi.ParameterType.FullName.Equals("System.String"))
                    paras[i].Nullable = true;
            }
            return paras;
        }
        public static Helper_API_Method_Para[] GetParas(MethodInfo method, XmlNode node)
        {
            Helper_API_Method_Para[] paras = GetParas(method);
            if (node == null) return paras;
            for (int i = 0; i < paras.Length; i++)
            {
                Helper_API_Method_Para pi = paras[i];
                foreach (XmlNode n in node.SelectNodes("param"))
                {
                    string name = n.Attributes["name"].Value;
                    if (name.Equals(pi.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        pi.Intro = n.InnerText.Trim();
                        break;
                    }
                }
            }
            return paras;
        }
        /// <summary>
        /// 获取参数的类型，多个参数串连
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetParaString(MethodInfo method)
        {
            string str = string.Empty;
            ParameterInfo[] paras = method.GetParameters();
            for (int i = 0; i < paras.Length; i++)
            {
                var nullableType = System.Nullable.GetUnderlyingType(paras[i].ParameterType);
                //string typename = nullableType != null ? nullableType.Name : type.Name;
                if (nullableType == null)
                {
                    str += paras[i].ParameterType.FullName;
                }
                else
                {
                    str += string.Format("System.Nullable{{{0}}}", nullableType.FullName);
                }
                if (i < paras.Length - 1) str += ",";
            }
            return str;
        }
    }
    //方法的特性
    public class Helper_API_Method_Attr
    {
        public string Name { get; set; }     //特性名称
        public bool Ignore { get; set; }    //是否可以被忽视
        public int Expires { get; set; }   //缓存的过期时效
        public static Helper_API_Method_Attr[] GetAttrs(MethodInfo method)
        {
            //所有特性
            Type[] attrs = WebAttribute.Initialization();
            List<WeishaAttr> list = new List<WeishaAttr>();
            foreach (Type att in attrs)
            {
                //取类上面的特性
                object[] attrsObj = method.DeclaringType.GetCustomAttributes(att, true);
                for (int i = 0; i < attrsObj.Length; i++)
                {
                    WeishaAttr attr = attrsObj[i] as WeishaAttr;
                    if (list.Contains(attr))
                    {
                        if (attr.Ignore) list[i].Ignore = true;
                    }
                    else
                    {
                        list.Add(attr);
                    }
                }
                //取方法上的特性
                object[] attrsMethod = method.GetCustomAttributes(att, true);
                for (int i = 0; i < attrsMethod.Length; i++)
                {
                    WeishaAttr attr = attrsMethod[i] as WeishaAttr;
                    if (list.Contains(attr))
                    {
                        if (attr.Ignore) list[i].Ignore = true;
                    }
                    else
                    {
                        list.Add(attr);
                    }
                }
            }
            //ignore为true的全部移除，不输出
            for (int i = 0; i < list.Count; i++)
            {
                WeishaAttr attr = list[i] as WeishaAttr;
                if (attr == null) continue;
                if (attr.Ignore) list.RemoveAt(i);
            }
            //去除"Attribute"字样
            Helper_API_Method_Attr[] arr = new Helper_API_Method_Attr[list.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = new Helper_API_Method_Attr();
                arr[i].Name = list[i].GetType().Name.Replace("Attribute", "");
                if (list[i] is WeishaAttr)
                    arr[i].Ignore = ((WeishaAttr)list[i]).Ignore;
                if (list[i] is CacheAttribute)
                    arr[i].Expires = ((CacheAttribute)list[i]).Expires;
            }
            return arr;
        }
    }
    #endregion
}
