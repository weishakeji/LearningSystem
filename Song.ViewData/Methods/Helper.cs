using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;
using System.Reflection;
using System.Xml;


namespace Song.ViewData.Methods
{

    /// <summary>
    /// 接口方法的帮助
    /// </summary> 
    [HttpGet]
    public class Helper : IViewAPI
    {
        /// <summary>
        /// 接口方法列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Helper_API[] List()
        {
            List<Helper_API> list = new List<Helper_API>();
            string assemblyName = "Song.ViewData";
            Assembly assembly = Assembly.Load(assemblyName);
            //取注释       
            XmlNodeList nodes = readXml();
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
        public Helper_API_Method[] Methods(string classname)
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
            //注释文档
            XmlNodeList nodes = readXml();
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

        private XmlNodeList readXml()
        {           
            XmlNodeList nodes = null;
            string file = WeiSha.Common.Server.MapPath("/bin/Song.ViewData.XML");
            if (!System.IO.File.Exists(file)) return nodes;
            XmlDocument xml = new XmlDocument();            
            xml.Load(file);
            nodes = xml.SelectNodes("/doc/members/member");
            return nodes;
        }
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
        //返回值的摘要
        public string Intro { get; set; }
        public static Helper_API_Method_Return GetReturn(MethodInfo method, XmlNode node)
        {
            Helper_API_Method_Return ret = new Helper_API_Method_Return();
            if (node != null)
            {
                if (node.SelectSingleNode("returns") != null)
                    ret.Intro = node.SelectSingleNode("returns").InnerText.Trim();   //返回值的摘要                
            }
            if (string.IsNullOrWhiteSpace(ret.Intro)) ret.Intro = string.Empty;
            ret.Type = method.ReturnParameter.ToString();      //返回类型
            return ret;
        }
    }
    //方法的参数
    public class Helper_API_Method_Para
    {
        public string Name { get; set; }    //参数名称
        public string Type { get; set; }        //参数数据类型
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
                paras[i].Type = pi.ParameterType.FullName; 
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
                str += paras[i].ParameterType.FullName;
                if (i < paras.Length - 1) str += ",";
            }
            return str;
        }
    }
    //方法的特性
    public class Helper_API_Method_Attr
    {
        public string Name { get; set; }     //特性名称
        public bool Ignore { get; set; }
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
