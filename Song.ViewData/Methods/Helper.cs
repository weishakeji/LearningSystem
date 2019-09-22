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
    public class Helper : IViewAPI
    {
        /// <summary>
        /// 接口方法列表
        /// </summary>
        /// <returns></returns>
        public List<Helper_API> List()
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
            return list;
        }

        /// <summary>
        /// 某个接口类下的方法
        /// </summary>
        /// <param name="classname">类名称</param>
        /// <returns></returns>
        public List<Helper_API_Method> Methods(string classname)
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
            //注译
            XmlNodeList nodes = readXml();
            //类下面的方法，仅获取当前类生成的方法，不包括父类
            List<Helper_API_Method> list = new List<Helper_API_Method>();
            MemberInfo[] mis = classtype.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            foreach (MethodInfo mi in mis)
            {
                string name = mi.Name;      //方法名称
                string fullname = mi.ToString();        //带参数的方法名称
                if (fullname.IndexOf(" ") > -1) fullname = fullname.Substring(fullname.IndexOf(" ") + 1);             
                string returntype = mi.ReturnParameter.ToString();      //返回类型
                string intro = string.Empty;        //方法摘要
                if (nodes != null)
                {
                    foreach (XmlNode n in nodes)
                    {
                        string miname = n.Attributes["name"].Value;                      
                        if (miname.EndsWith(fullname))
                        {
                            intro = n.SelectSingleNode("summary").InnerText.Trim();
                            break;
                        }
                    }
                }
                list.Add(new Helper_API_Method()
                {
                    Name = name,
                    FullName = fullname,
                    Return = returntype,
                    ClassName = mi.DeclaringType.Name,
                    Intro = intro
                });
            }
            //按方法名排序
            list.Sort((a, b) => a.Name.CompareTo(b.Name));
            return list;
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
    public class Helper_API
    {
        public string Name { get; set; }
        public string Intro { get; set; }
    }
    public class Helper_API_Method
    {
        public string Name { get; set; }        //方法名   
        public string FullName { get; set; }    //方法全名  
        public string Intro { get; set; }       //方法摘要说明
        public string Return { get; set; }      //返回值的类型
        public string ClassName { get; set; }    //方法所的类的名称
    }
    #endregion
}
