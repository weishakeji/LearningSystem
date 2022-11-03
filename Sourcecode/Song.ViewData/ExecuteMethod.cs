using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private static Dictionary<string, IViewAPI> _objects = new Dictionary<string, IViewAPI>();
        private static readonly object lock_obj = new object();
        /// <summary>
        /// 创建对象，如果存在，则直接返回；如果不存在，创建后返回
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public static IViewAPI CreateInstance(Letter letter)
        {

            string assemblyName = "Song.ViewData";
            string classFullName = String.Format("{0}.Methods.{1}", assemblyName, letter.ClassName);

            //由缓存中查找，是否存在
            IViewAPI obj = null;
            foreach (KeyValuePair<string, IViewAPI> kv in _objects)
            {
                if (classFullName.Trim().Equals(kv.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    obj = kv.Value;
                    break;
                }
            }
            if (obj != null) return obj;

            lock (lock_obj)
            {
                //如果之前未创建，则创建接口对象
                Assembly assembly = Assembly.Load(assemblyName);
                List<Type> types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Contains(typeof(IViewAPI))).ToList<Type>();
                Type type = types.Find(t => t.FullName.Equals(classFullName, StringComparison.CurrentCultureIgnoreCase));
                if (type == null) throw new Exception(string.Format("调用的接口'{0}'不存在, 可能是'{1}'拼写错误", letter.API_PATH, letter.ClassName));
                obj = (IViewAPI)System.Activator.CreateInstance(type);
                //记录到缓存
                if (!_objects.ContainsKey(type.FullName)) _objects.Add(type.FullName, obj);
                return obj;
            }
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
            IViewAPI execObj = ExecuteMethod.CreateInstance(letter);

            //2.获取要执行的方法，即$api.get("account/single")中的single
            MethodInfo method = getMethod(execObj.GetType(), letter);
            //清除缓存
            if (letter.HTTP_METHOD.Equals("put", StringComparison.CurrentCultureIgnoreCase))
                CacheAttribute.Remove(method, letter);
            if (letter.Custom_METHOD.Equals("cache", StringComparison.CurrentCultureIgnoreCase)
                && (letter.Custom_Action.Equals("clear", StringComparison.CurrentCultureIgnoreCase)
                || letter.Custom_Action.Equals("update", StringComparison.CurrentCultureIgnoreCase)))
                CacheAttribute.Remove(method, letter);

            //3#.验证方法的特性,一是验证Http动词，二是验证是否登录后操作，三是验证权限    
            //----验证Http谓词访问限制
            HttpAttribute.Verify(letter.HTTP_METHOD, method);
            //LoginAttribute.Verify(method);
            //----范围控制，本机或局域网，或同域
            bool isRange = RangeAttribute.Verify(method, letter);
            //----如果有文件上传，则验证文件
            UploadAttribute.Verify(method, letter);
            //----验证是否需要登录
            LoginAttribute loginattr = LoginAttribute.Verify(method, letter);

            //----清理参数值中的html标签,默认全部清理，通过设置not参数不过虑某参数
            HtmlClearAttribute.Clear(method, letter);
            //----验证是否购买课程,是否可以学习课程内容
            bool isBuy = PurchasedAttribute.Verify(method, letter);
            bool isStudy = StudyAttribute.Verify(method, letter);

            //4.构建执行该方法所需要的参数
            object[] parameters = getInvokeParam(method, letter);

            //5.执行方法，返回结果
            object objResult = null;    //结果
            //只有get方式时，才使用缓存
            CacheAttribute cache = null;
            //if (letter.HTTP_METHOD.Equals("put", StringComparison.CurrentCultureIgnoreCase))
            //    CacheAttribute.Remove(method, letter);
            if (letter.HTTP_METHOD.Equals("get", StringComparison.CurrentCultureIgnoreCase))
                cache = CacheAttribute.GetAttr<CacheAttribute>(method);
            if (cache != null)
            {
                //为false时，则从缓存取数据；为true时，则禁止管理员从缓存取数据
                if (!cache.AdminDisable)
                {
                    objResult = CacheAttribute.GetResult(method, letter);
                }
                else
                {
                    //当前登录的管理员
                    Entities.EmpAccount emp = LoginAdmin.Status.User(letter);
                    if (emp == null) objResult = CacheAttribute.GetResult(method, letter);
                }
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
            //Helper.Logs.Info(letter, (String)objResult);          
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
                if (!letter.HTTP_HOST.Equals(letter.WEB_HOST, StringComparison.OrdinalIgnoreCase))
                    throw VExcept.System("The API is inconsistent with the weburi, so the request is restricted", 101);
                if (!"weishakeji".Equals(letter.HTTP_Mark))
                    throw VExcept.System("The request id is incorrect", 100);

                //执行方法
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
                //记录执行速度
                int elapsedNumber = (int)Math.Floor(span / 1000) * 1000;
                if (elapsedNumber >= 1000)
                {
                    Helper.Logs.WriteLog(elapsedNumber + "_" + letter.ClassName + "_" + letter.MethodName, letter, span.ToString());
                }

                return new Song.ViewData.DataResult(res, span);       //普通数据
            }
            catch (Exception ex)
            {
                Helper.Logs.Error(letter, ex);
                //如果是自定义异常
                if (ex.InnerException is VExcept)
                {
                    VExcept except = (VExcept)ex.InnerException;
                    return new Song.ViewData.DataResult(except, time);
                }
                if (ex.InnerException is WeiSha.Data.DataException)
                {
                    WeiSha.Data.DataException except = (WeiSha.Data.DataException)ex.InnerException;
                    return new Song.ViewData.DataResult(except, time);
                }
                return new Song.ViewData.DataResult(ex, time);
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
            List<MethodInfo> methods = new List<MethodInfo>();  //与$api请求的接口名称相同的所有方法
            foreach (MethodInfo mi in type.GetMethods())
            {
                if (p.MethodName.Equals(mi.Name, StringComparison.CurrentCultureIgnoreCase))
                    methods.Add(mi);
            }
            if (methods.Count < 1)
                throw new Exception(string.Format("接口 '{0}/{1}' 不存在", p.ClassName, p.MethodName));
            //2、判断方法的参数名称，是否与传递来的参数名称匹配，参数数量必须匹配  
            //2-1 只有一个参数，且类型是Letter
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
            //2-2 判断参数个数是否相同
            MethodInfo method = null;       //当前$api请求的方法
            foreach (MethodInfo mi in methods)
            {
                int paraCount = 0;
                foreach (ParameterInfo pi in mi.GetParameters())
                    if (!pi.IsOut) paraCount++;

                //方法的参数个数，和传参的参数个数，必须相等
                if (paraCount == p.Params.Count)
                {
                    method = mi;
                    break;
                }
            }
            if (method == null)
            {
                string tips = string.Empty;
                for (int i = 0; i < methods.Count; i++)
                {
                    tips += methods[i].GetParameters().Length;
                    tips += i < methods.Count - 1 ? "、" : "";
                }
                throw new Exception(string.Format("接口 '{0}/{1}' 形参与实参的数量不符，接口形参所需：{3}，实际传参：{2}个。",
                p.ClassName, methods.Count > 0 ? methods[0].Name : p.MethodName,
                p.Params.Count, tips));
            }

            //2-3、判断参数名称与传递来的参数名称是否一致
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

            if (method == null) method = mbLetter;
            if (method == null)
            {
                string tips = string.Empty;
                for (int i = 0; i < methods.Count; i++)
                {
                    string pstr = string.Empty;
                    ParameterInfo[] pis = methods[i].GetParameters();
                    for (int j = 0; j < pis.Length; j++)
                    {
                        pstr += pis[j].Name;
                        pstr += j < pis.Length - 1 ? "," : "";
                    }
                    tips += string.Format("{0}({1})", methods[i].Name, pstr);
                    tips += i < methods.Count - 1 ? "、" : "";
                }
                throw new Exception(
                        string.Format("接口 '{0}/{1}' 的形参名称与实参名称不匹配；形参：{2}，实际传参：{3}",
                        type.Name, methods.Count > 0 ? methods[0].Name : p.MethodName,
                        tips,
                        p.ToString() == string.Empty ? "null" : p.ParamsNames()));
            }
            return method;
        }
        #region 通过方法的参数（形参 formal parameter），匹配传递来的参数（实参 actual parameter）
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
                //如果形参是Letter类型，则直接赋值
                if (letter.GetType().FullName.Equals(pi.ParameterType.FullName))
                {
                    objs[i] = letter;
                    continue;
                }
                //如果形参为输出型的，则不赋值（ViewData接口不允许此类参数）
                if (pi.IsOut)
                {
                    objs[i] = null;
                    continue;
                }
                //实参的值，即接口方法的参数所对应的客户端传来的值
                string val = letter[pi.Name].String;
                //是否是可以为空的参数，例如DateTime? int?
                Type nullableType = Nullable.GetUnderlyingType(pi.ParameterType);
                if (nullableType != null)
                {
                    if (string.IsNullOrWhiteSpace(val))
                        objs[i] = null;
                    else
                        objs[i] = WeiSha.Core.DataConvert.ToObject(Type.GetType(nullableType.FullName), val);
                    continue;
                }
                else
                {
                    //如果形参不是可以为空，且实参的确为空了
                    if (string.IsNullOrWhiteSpace(val) && !pi.ParameterType.Name.Equals("string", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (pi.ParameterType.Name.Equals("Boolean", StringComparison.CurrentCultureIgnoreCase) ||
                            //pi.ParameterType.Name.Equals("Int32", StringComparison.CurrentCultureIgnoreCase) ||
                            //pi.ParameterType.Name.Equals("Int64", StringComparison.CurrentCultureIgnoreCase) ||
                      pi.ParameterType.Name.Equals("DateTime", StringComparison.CurrentCultureIgnoreCase))
                        {
                            string tips = "接口 '{0}/{1}' 的参数 {2}({3}) ，实际传参的值为空";
                            throw new Exception(string.Format(tips, method.DeclaringType.Name, method.Name,
                                pi.Name, pi.ParameterType.Name.ToLower()));
                        }
                    }
                }
                //如果形参是数据实体
                if (pi.ParameterType.BaseType != null && pi.ParameterType.BaseType.FullName == "WeiSha.Data.Entity")
                {
                    objs[i] = GetValueToEntity<WeiSha.Data.Entity>(pi.ParameterType, val);
                    continue;
                }
                //如果形参是数组
                if (pi.ParameterType.IsArray)
                {
                    objs[i] = _getValueToArray(pi.ParameterType, val);
                    continue;
                }
                //如果是Json数据
                if (pi.ParameterType.Name.Equals("JArray"))
                {
                    objs[i] = JArray.Parse(val);
                    continue;
                }
                if (pi.ParameterType.Name.Equals("JObject"))
                {
                    objs[i] = JObject.Parse(val);
                    continue;
                }
                try
                {
                    objs[i] = WeiSha.Core.DataConvert.ToObject(pi.ParameterType, val);
                }
                catch
                {
                    string tips = "接口 '{0}/{1}' 的参数 {2}({3}) ，数据格式不正确; 实际传参：{4}";
                    if (val.Length > 100) val = val.Substring(0, 100) + "...";
                    throw new Exception(string.Format(tips, method.DeclaringType.Name, method.Name,
                        pi.Name, pi.ParameterType.Name.ToLower(), val));
                }

            }
            return objs;
        }
        /// <summary>
        /// 数据实体的转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramType">形参的类型</param>
        /// <param name="actual">实参的值</param>
        /// <returns></returns>
        public static T GetValueToEntity<T>(Type paramType, string actual) where T : WeiSha.Data.Entity
        {
            if (paramType == null) paramType = typeof(T);
            //创建实体
            object entity = paramType.Assembly.CreateInstance(paramType.FullName);
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(actual);
            PropertyInfo[] props = paramType.GetProperties();
            for (int j = 0; j < props.Length; j++)
            {
                PropertyInfo opi = props[j];    //实体属性
                foreach (KeyValuePair<string, object> kv in dic)
                {
                    if (kv.Key.Equals(opi.Name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        string val = kv.Value.ToString();
                        //实体属性的值
                        try
                        {
                            object tm = string.IsNullOrEmpty(val) ? null : WeiSha.Core.DataConvert.ChangeType(val.Trim(), opi.PropertyType);
                            opi.SetValue(entity, tm, null);
                        }
                        catch (Exception ex)
                        {
                            string msg = opi.Name + "的值：" + val.Trim() + "转换异常，无法转换为：" + opi.PropertyType.ToString();
                            throw new Exception(msg, ex);
                        }
                        continue;
                    }
                }
            }
            //实体数据修订
            Type ts = entity.GetType();
            for (int n = 0; n < props.Length; n++)
            {
                //当前属性的值
                object o = ts.GetProperty(props[n].Name).GetValue(entity, null);
                if (o != null) continue;
                //属性的类型
                Type ptype = props[n].PropertyType;
                if (props[n].PropertyType.IsGenericType && props[n].PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    ptype = props[n].PropertyType.GetGenericArguments()[0];

                string tname = ptype.Name;
                //如果为空，则设置初始值
                if (ptype.Name == "DateTime") props[n].SetValue(entity, DateTime.Now, null);
                if (ptype.Name == "Int32") props[n].SetValue(entity, 0, null);
                if (ptype.Name == "Int64") props[n].SetValue(entity, 0, null);
            }
            return (T)entity;
        }
        /// <summary>
        /// 实参转换为数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="paramType"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        private static Array _getValueToArray(Type paramType, string actual)
        {
            if (string.IsNullOrWhiteSpace(actual)) return null;
            //获取数组成员的类型
            string tName = paramType.FullName.Replace("[]", string.Empty);
            Type elType = paramType.Assembly.GetType(tName);
            //解析
            JArray jarray = JArray.Parse(actual);
            Array array = Array.CreateInstance(elType, jarray.Count);
            if (elType.BaseType != null && elType.BaseType.FullName == "WeiSha.Data.Entity")
            {
                for (int i = 0; i < jarray.Count; i++)
                    array.SetValue(GetValueToEntity<WeiSha.Data.Entity>(elType, jarray[i].ToString()), i);
                return array;
            }
            for (int i = 0; i < jarray.Count; i++)
            {
                array.SetValue(WeiSha.Core.DataConvert.ToObject(elType, jarray[i].ToString()), i);
            }
            return array;
        }
        #endregion
    }
}
