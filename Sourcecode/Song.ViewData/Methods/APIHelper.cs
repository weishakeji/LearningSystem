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
    public class APIHelper : ViewMethod, IViewAPI
    {
        #region 接口说明
        /// <summary>
        /// 接口列表，这里是接口类的列表
        /// </summary>
        /// <returns></returns>
        [HttpPost][HttpGet][HttpPut]
        [Cache]
        [Localhost]
        public Helper_API[] List()
        {           
            List<Helper_API> list = new List<Helper_API>();
            Assembly assembly = Assembly.GetExecutingAssembly();
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
        public Helper_API_Method[] Methods(string classname)
        {
            string assemblyName = "Song.ViewData";
            string classFullName = String.Format("{0}.Methods.{1}", assemblyName, classname);
            Assembly assembly = Assembly.GetExecutingAssembly();
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
        public string ToWord()
        {
            long snowid = Business.Do<ISystemPara>().SerialSnow();
            string filename = $"RESTful API 说明_{DateTime.Now.ToString("yyyy-MM-dd")}.{snowid}.docx";
            filename = Path.Combine(outputPathHy, filename);
            ////导出文件的位置
            //string pathname = "APItoWord";
            //string filePath = WeiSha.Core.Upload.Get["Temp"].Physics + pathname + "\\";
            //if (!System.IO.Directory.Exists(filePath))
            //    System.IO.Directory.CreateDirectory(filePath);
            //string filename = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + ".docx";

            // 创建一个新的 Word 文档对象
            XWPFDocument doc = new XWPFDocument();

            Helper_API[] apis = this.List();
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
                Helper_API_Method[] methods = this.Methods(apis[i].Name);
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
            using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                doc.Write(fs);
            }

            return WeiSha.Core.Upload.Get["Temp"].Virtual + "/" + filename;
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

        #region 导出到文档
        //导出的文件夹名称
        private string _outputPath = "APItoWord";
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
            return ToWord();
            long snowid = Business.Do<ISystemPara>().SerialSnow();
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
