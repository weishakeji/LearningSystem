using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Song.DataQuery
{
    /// <summary>
    /// 数据查询，主要针对一些复杂的查询，需要手写SQL语句
    /// 不同的数据库Sql语句（尤其是sql函数）略有差别，不得不分别处理
    /// </summary>
    public class DbQuery
    {
        //默认的数据库
        private static string _db_default = "SqlServer9";
        //类名
        private static string _type_name = "Song.DataQuery.{0}.{1}";
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method">当前调用的方法</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T Call<T>(MethodBase method, object[] parameters)
        {
            string dbname = WeiSha.Data.Gateway.Default.DbType.ToString();
            string typename = string.Format(_type_name, dbname, method.DeclaringType.Name);

            Type type = Type.GetType(typename);
            //如果为空，采用默认的数据库类型的处理方法
            if (type == null) type = Type.GetType(string.Format(_type_name, _db_default, method.DeclaringType.Name));

            //实例对象
            object execObj = Activator.CreateInstance(type);
            //针对当前数据库的查询方法
            MethodInfo execMethod = DbQuery.getMethod(type, method);
            if (execMethod == null)
            {
                type = Type.GetType(string.Format(_type_name, _db_default, method.DeclaringType.Name));
                execMethod = DbQuery.getMethod(type, method);
            }
            if (execMethod == null) return default;
            //调用
            object objResult = execMethod.Invoke(execObj, parameters);
           
            return objResult == null ? default : (T)objResult;
        }

        public static T Call<T>(object[] parameters)
        {
            //通过堆栈，获取调用该方法的上级方法
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase method = stackFrame.GetMethod();

            return Call<T>(method, parameters);           
        }
        public static T Call<T>(object parameter)
        {
            //通过堆栈，获取调用该方法的上级方法
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase method = stackFrame.GetMethod();

            object[] parameters = new object[] { parameter };
            return Call<T>(method, parameters);
        }
        //public static T Call<T>(params object[] objs)
        //{
        //    //通过堆栈，获取调用该方法的上级方法
        //    StackTrace stackTrace = new StackTrace();
        //    StackFrame stackFrame = stackTrace.GetFrame(1);
        //    MethodBase method = stackFrame.GetMethod();

        //    return Call<T>(method, objs);
        //}
        public static T Call<T>()
        {
            //通过堆栈，获取调用该方法的上级方法
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase method = stackFrame.GetMethod();

            return Call<T>(method, null);
        }

        #region 私有方法

        /// <summary>
        /// 获取与参数类型匹配的方法
        /// </summary>
        /// <param name="type">要创建方法的对象的类型</param>
        /// <param name="method">当前调用的方法</param>
        /// <returns></returns>
        private static MethodInfo getMethod(Type type, MethodBase method)
        {
            List<Type> list = new List<Type>();
            ParameterInfo[] pis = method.GetParameters();
            foreach(ParameterInfo pi in pis)
            {
                list.Add(pi.ParameterType);
            }
            return type.GetMethod(method.Name,list.ToArray());
        }

        #endregion
    }
}
