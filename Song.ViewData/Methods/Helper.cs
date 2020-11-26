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
using System.Drawing;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace Song.ViewData.Methods
{

    /// <summary>
    /// 接口方法的帮助
    /// </summary> 
    [HttpGet]
    public class Helper : ViewMethod, IViewAPI
    {
        #region 接口说明
        /// <summary>
        /// 接口方法列表
        /// </summary>
        /// <returns></returns>
        [HttpPost][HttpGet][HttpPut]
        [Cache]
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
        #endregion

        #region 数据字典
        public string Entities()
        {
            return this.Entities(string.Empty);
        }
        /// <summary>
        /// 数据实体
        /// </summary>
        /// <param name="detail">实体的详情说明,json格式</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public string Entities(string detail)
        {
            //读取或写入
            string file = string.Format("{0}help\\datas\\entitiy\\entities.json", AppDomain.CurrentDomain.BaseDirectory);
            if (!string.IsNullOrWhiteSpace(detail))
            {
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(file, false))               
                    f.Write(detail);               
            }
            Assembly assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "\\bin\\Song.Entities.dll");
            Type[] types = assembly.GetExportedTypes()
                .Where(t => t.BaseType.FullName.Equals("WeiSha.Data.Entity", StringComparison.CurrentCultureIgnoreCase))
                .ToArray();
            string details = string.Empty;
            if (System.IO.File.Exists(file))
                details = System.IO.File.ReadAllText(file);
            JObject jitem = (JObject)JsonConvert.DeserializeObject(details);
            jitem = jitem == null ? new JObject() : jitem;

            List<string> list = new List<string>();
            for (int i = 0; i < types.Length; i++)            
                list.Add(types[i].Name);
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
                    jitem.Add(entity, obj);
                }
            }
            details = JsonConvert.SerializeObject(jitem);
            return details;
            //return list.ToArray();
        }
        /// <summary>
        /// 获取某个实体的类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string EntityField(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            Assembly assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "\\bin\\Song.Entities.dll");
            Type type = assembly.GetExportedTypes()
                .Where(t => t.FullName.Substring(t.FullName.Length - name.Length).Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();
            if (type == null) return null;

            JObject fields = new JObject();
            //获取属性
            PropertyInfo[] properties = type.GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                if (fields.ContainsKey(properties[j].Name)) continue;
                Type ptype = properties[j].PropertyType;
                JObject pattr = new JObject();
                //属性的类型名称，包括泛型
                if (ptype.IsGenericType && ptype.GetGenericTypeDefinition() == typeof(Nullable<>))
                    ptype = ptype.GetGenericArguments()[0];
                pattr.Add("type", ptype.Name.ToLower());
                //属性是否可以为空
                pattr.Add("nullable", properties[j].PropertyType.Name.IndexOf("Nullable") >= 0);
                //
                fields.Add(properties[j].Name, pattr);
            }
            return fields.ToString();
        }
        /// <summary>
        /// 实体详细说明的获取
        /// </summary>       
        /// <param name="name">实体名称</param>
        /// <param name="detail">实体的详情说明,json格式</param>
        /// <returns>返回实体详情</returns>
        [HttpGet]
        public string EntityDetails(string name)
        {
            return this.EntityDetails(name, string.Empty);
        }
        /// <summary>
        /// 实体详细说明的获取
        /// </summary>       
        /// <param name="name">实体名称</param>
        /// <param name="detail">实体的详情说明,json格式</param>
        /// <returns>返回实体详情</returns>
        [SuperAdmin]
        [HttpPost]
        public string EntityDetails(string name, string detail)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            Assembly assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "\\bin\\Song.Entities.dll");
            Type type = assembly.GetExportedTypes()
                .Where(t => t.FullName.Substring(t.FullName.Length - name.Length).Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();
            if (type == null) return null;
            //读取或写入
            string file = string.Format("{0}help\\datas\\Entitiy\\{1}.json", AppDomain.CurrentDomain.BaseDirectory, name);
            if (!string.IsNullOrWhiteSpace(detail))
            {
                using (System.IO.StreamWriter f = new System.IO.StreamWriter(file, false))
                    f.Write(detail);
            }
            string details = string.Empty;
            if (System.IO.File.Exists(file))
                details = System.IO.File.ReadAllText(file);

            //获取实体和实体属性的名称
            List<string> list = new List<string>();            
            foreach (PropertyInfo p in type.GetProperties())
                list.Add(p.Name);

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
            details = JsonConvert.SerializeObject(jitem);
            return details;
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
        /// <returns></returns>
        private Dictionary<string, string> CodeImg(int leng, int type, string acc)
        {
            //设定生成几位随机数
            string tmp = RndNum(leng, type);
            string val = new Song.ViewData.ConvertToAnyValue(acc + tmp).MD5;
            //生成图片
            System.Drawing.Bitmap image = CreateImage(tmp);
            string base64 = WeiSha.Common.Images.ImageTo.ToBase64(image);
            //
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("value", val);
            dic.Add("base64", "data:image/JPG;base64," + base64);
            return dic;
            //存储随机数
            //System.Web.HttpContext context = System.Web.HttpContext.Current;
            //System.Web.HttpCookie cookie = new System.Web.HttpCookie(name);
            //cookie.Value = val;
            //context.Response.Cookies.Add(cookie);
            //生成图片
            //System.Drawing.Bitmap image = CreateImage(tmp);
            //string base64 = WeiSha.Common.Images.ImageTo.ToBase64(image);
            //return "data:image/JPG;base64," + base64;
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

                Font f = new System.Drawing.Font(font[findex], 12, System.Drawing.FontStyle.Bold);
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
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            //pg.Response.ClearContent();
            //pg.Response.ContentType = "image/Gif";
            //pg.Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            //image.Dispose();
            return image;
        }
        #endregion

        /// <summary>
        /// 公司产品版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Cache(Expires = 999)]
        public DataTable ProductEdition()
        {
            return WeiSha.Common.Parameters.Authorization.VersionLevel.LevelTable;
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
