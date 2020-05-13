using Song.ViewData.Attri;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Song.ViewData
{
    /// <summary>
    /// 执行具体的方法
    /// </summary>
    public class ExecuteMethod
    {

        #region 单件模式
        private static readonly ExecuteMethod _instance = new ExecuteMethod();
        private ExecuteMethod() { }
        /// <summary>
        /// 返回实例
        /// </summary>
        /// <returns></returns>
        public static ExecuteMethod GetInstance()
        {
            return _instance;
        }
        #endregion

        #region 创建并缓存实例对象
        //存储对象的键值对，key为对象的类名称（全名），value为对象自身
        private Dictionary<string, object> _objects = new Dictionary<string, object>();
        /// <summary>
        /// 创建对象，如果存在，则直接返回；如果不存在，创建后返回
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static object CreateInstance(Letter letter)
        {
            string assemblyName = "Song.ViewData";
            string classFullName = String.Format("{0}.Methods.{1}", assemblyName, letter.ClassName);
            object obj = null;
            //由缓存中查找，是否存在
            ExecuteMethod curr = ExecuteMethod.GetInstance();
            foreach (KeyValuePair<string, object> kv in curr._objects)
            {
                if (classFullName.Trim().Equals(kv.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    obj = kv.Value;
                    break;
                }
            }
            if (obj != null) return obj;
            //如果之前未创建，则重新创建
            Type type = null;
            Assembly assembly = Assembly.Load(assemblyName);
            Type[] types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(IViewAPI)))
                .ToArray();
            foreach (Type info in types)
            {
                if (info.FullName.Equals(classFullName, StringComparison.CurrentCultureIgnoreCase))
                {
                    type = info;
                    break;
                }
            }
            if (type == null) throw new Exception(
                string.Format("调用的对象'{0}'不存在, 可能是'{1}'拼写错误",
                classFullName, letter.ClassName));
            obj = System.Activator.CreateInstance(type);    //创建对象
            ExecuteMethod.GetInstance()._objects.Add(type.FullName, obj);   //记录到缓存
            return obj;
        }
        #endregion

        /// <summary>
        /// 执行，按实际结果返回
        /// </summary>
        /// <param name="letter">客户端递交的参数信息</param>
        /// <returns></returns>
        public static object Exec(Letter letter)
        {
            //1.创建对象,即$api.get("account/single")中的account
            object execObj = ExecuteMethod.CreateInstance(letter);
            //2.获取要执行的方法，即$api.get("account/single")中的single
            MethodInfo method = getMethod(execObj.GetType(), letter);
            //3#.验证方法的特性,一是验证Http动词，二是验证是否登录后操作，三是验证权限    
            //----验证Http谓词访问限制
            HttpAttribute.Verify(letter.HTTP_METHOD, method);
            //LoginAttribute.Verify(method);
            //----范围控制，本机或局域网，或同域
            bool isRange = RangeAttribute.Verify(letter, method);
            //----验证是否需要登录
            LoginAttribute loginattr = LoginAttribute.Verify(method); 
            

            //4.构建执行该方法所需要的参数
            object[] parameters = getInvokeParam(method, letter);
            //5.执行方法，返回结果
            object objResult = null;    //结果
            //只有get方式时，才使用缓存
            CacheAttribute cache = null;
            if (letter.HTTP_METHOD == "GET")           
                cache = CacheAttribute.GetAttr<CacheAttribute>(method);       
            if (cache != null)
            {
                objResult = CacheAttribute.GetResult(method, letter);
                if (objResult == null)
                {
                    objResult = method.Invoke(execObj, parameters);
                    CacheAttribute.Insert(cache.Expires, method, letter, objResult);
                }
            }
            else
            {
                objResult = method.Invoke(execObj, parameters);
            }            
            //将执行结果写入日志
            LoginAttribute.LogWrite(loginattr, objResult);
            return objResult;
        }
        /// <summary>
        /// 执行，返回结构封装到DataResult对象中
        /// </summary>
        /// <param name="letter">客户端递交的参数信息</param>
        /// <returns></returns>
        public static DataResult ExecToResult(Letter letter)
        {
            DateTime time = DateTime.Now;
            try
            {   
                if (!"weishakeji".Equals(letter.HTTP_Mark))
                    return new Song.ViewData.DataResult(new Exception("请求标识不正确"));

                object res = Exec(letter);
                //计算耗时                
                double span = ((TimeSpan)(DateTime.Now - time)).TotalMilliseconds;
                //
                //如果是分页数据
                if (res is ListResult)
                {
                    ListResult list = (ListResult)res;
                    list.ExecSpan = span;
                    return list;
                }
                return new Song.ViewData.DataResult(res, span);       //普通数据
            }
            catch (Exception ex)
            {
                return new Song.ViewData.DataResult(ex,time);
            }
        }
        /// <summary>
        /// 要执行的方法，根据方法名、参数数量
        /// </summary>
        /// <param name="type">要调用的对象的类型</param>
        /// <param name="p"></param>
        /// <returns></returns>
        private static MethodInfo getMethod(Type type, Letter p)
        {
            //1、先判断方法是否存在
            List<MethodInfo> methods = new List<MethodInfo>();
            foreach (MethodInfo mi in type.GetMethods())
            {
                if (p.MethodName.Equals(mi.Name, StringComparison.CurrentCultureIgnoreCase))
                    methods.Add(mi);
            }
            if (methods.Count < 1)
                throw new Exception(string.Format("调用方法'{0}.{1}'不存在", p.ClassName, p.MethodName));
            //2、判断方法的参数名称，是否与传递来的参数名称匹配，参数数量必须匹配  
            //只有一个参数，且类型是Letter
            MethodInfo mbLetter = null;
            for (int i = 0; i < methods.Count; i++)
            {
                ParameterInfo[] pis = methods[i].GetParameters();
                if (pis.Length == 1 && p.GetType().FullName.Equals(pis[0].ParameterType.FullName))
                {
                    mbLetter = methods[i];
                    methods.Remove(methods[i]);
                    break;
                }
            }
            MethodInfo method = null;
            foreach (MethodInfo mi in methods)
            {
                //2-1、判断参数个数是否相同
                int paraCount = 0;
                foreach (ParameterInfo pi in mi.GetParameters())
                {
                    if (!pi.IsOut) paraCount++;
                }
                //方法的参数个数，和传参的参数个数，必须相等
                if (paraCount == p.Params.Count)
                {
                    method = mi;
                    break;
                }
            }
            //2-2、判断参数名称与传递来的参数名称是否一致
            if (method != null)
            {
                bool ismatch = true;    //是否匹配
                foreach (ParameterInfo pi in method.GetParameters())
                {
                    //如果参数是Parameter类型，则跳过匹配
                    if (p.GetType().FullName.Equals(pi.ParameterType.FullName)) continue;
                    //只要有一个参数不匹配，即中断
                    if (!p.ExistParameter(pi.Name))
                    {
                        ismatch = false;
                        break;
                    }
                }
                if (!ismatch) method = null;
            }
            if (method == null) method = mbLetter;
            if (method == null) throw new Exception(
                string.Format("所调用方法'{0}.{1}'的参数名称与实际传参不匹配；实际传参：{2}",
                type.FullName, p.MethodName,
                p.ToString() == string.Empty ? "null" : p.ToString()));
            return method;
        }
        /// <summary>
        /// 返回方法执行所需要的参数
        /// </summary>
        /// <param name="method">要执行的方法</param>
        /// <param name="letter">传递来的参数</param>
        /// <returns></returns>
        private static object[] getInvokeParam(MethodInfo method, Letter letter)
        {
            ParameterInfo[] paramInfos = method.GetParameters();
            object[] objs = new object[paramInfos.Length];
            for (int i = 0; i < objs.Length; i++)
            {
                ParameterInfo pi = paramInfos[i];
                //如果参数是Letter类型，则直接赋值
                if (letter.GetType().FullName.Equals(pi.ParameterType.FullName))
                {
                    objs[i] = letter;
                }
                else
                {
                    //普通参数，不是输出参数
                    if (!pi.IsOut)
                    {
                        string val = letter[pi.Name].String;
                        if (!pi.ParameterType.Name.Equals("string", StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (string.IsNullOrWhiteSpace(val)) throw new Exception("参数 " + pi.Name + " 的值为空");
                        }
                        try
                        {
                            switch (pi.ParameterType.Name)
                            {
                                case "DateTime":
                                    if (val == null || string.IsNullOrWhiteSpace(val.ToString()))
                                    {
                                        objs[i] = DateTime.Now;
                                        break;
                                    }
                                    DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                                    long lTime = long.Parse(val + "0000");
                                    TimeSpan toNow = new TimeSpan(lTime);
                                    objs[i] = dt.Add(toNow);
                                    break;
                                default:
                                    objs[i] = Convert.ChangeType(val, pi.ParameterType);
                                    break;
                            }
                            //objs[i] = Convert.ChangeType(val, pi.ParameterType);
                        }
                        catch
                        {
                            throw new Exception("参数 " + pi.Name + " 的值，数据格式不正确");
                        }
                    }
                    else
                    {
                        objs[i] = null;
                    }
                }
            }
            return objs;
        }

    }
}
