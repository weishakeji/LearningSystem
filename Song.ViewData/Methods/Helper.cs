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


namespace Song.ViewData.Methods
{
    
    /// <summary>
    /// 接口方法的帮助
    /// </summary> 
    public class Helper
    {        
        /// <summary>
        /// 接口方法列表
        /// </summary>
        /// <returns></returns>
        public List<string> List()
        {
            List<string> list = new List<string>();
            string assemblyName = "Song.ViewData";
            Assembly assembly = Assembly.Load(assemblyName);
            foreach (Type info in assembly.GetExportedTypes())
            {
                if (info.FullName.StartsWith("Song.ViewData.Methods"))
                    list.Add(info.Name);
            }
            list.Sort();
            return list;
        }
        /// <summary>
        /// 某个接口类下的方法
        /// </summary>
        /// <param name="classname">类名称</param>
        /// <returns></returns>
        public List<MethodInfo> Methods(string classname)
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
            //类下面的方法，仅获取当前类生成的方法，不包括父类
            List<MethodInfo> list = new List<MethodInfo>();
            MemberInfo[] mis = classtype.GetMethods( BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public); 
            foreach (MethodInfo mi in mis)                       
                  list.Add(mi);
            //按方法名排序
            list.Sort((a, b) => a.Name.CompareTo(b.Name));
            return list;
        }  
    }
}
