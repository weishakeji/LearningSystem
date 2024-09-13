using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Data;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// SQL查询监控
    /// </summary>
    public class DatabaseLog : WeiSha.Data.Logger.IExecuteLog
    {
        /// <summary>
        /// 查询开始之前
        /// </summary>
        /// <param name="command"></param>
        public void Begin(IDbCommand command)
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 查询完成之后
        /// </summary>
        /// <param name="command"></param>
        /// <param name="retValue">返回值</param>
        /// <param name="elapsedTime">实例测量得出的总运行时间（以毫秒为单位</param>
        public void End(IDbCommand command, ReturnValue retValue, long elapsedTime)
        {
            if ((int)WeiSha.Core.App.Get["LOG_LEVEL"].Int32 <= 2) return;
            //1秒内的不统计
            if (elapsedTime < 1000) return;

            string sql = command.CommandText;
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                System.Data.IDbDataParameter para = (IDbDataParameter)command.Parameters[i];
                //System.Data.SqlClient.SqlParameter para = (System.Data.SqlClient.SqlParameter)command.Parameters[i];
                string vl = para.Value.ToString();
                string tp = para.DbType.ToString();
                if (tp.IndexOf("Int") > -1)
                    sql = sql.Replace("@p" + i.ToString(), vl);
                if (tp == "String")
                    sql = sql.Replace("@p" + i.ToString(), "'" + vl + "'");
                if (tp == "Boolean")
                {
                    if(Gateway.Default.DbType==DbProviderType.PostgreSQL)
                        sql = sql.Replace("@p" + i.ToString(), vl == "True" ? "true" : "false");
                    else
                        sql = sql.Replace("@p" + i.ToString(), vl == "True" ? "1" : "0");
                }
                if (tp == "DateTime")
                    sql = sql.Replace("@p" + i.ToString(), "'" + ((DateTime)para.Value).ToString("yyyy/MM/dd HH:mm:ss") + "'");
            }

            try
            {
                System.Web.HttpContext _context = System.Web.HttpContext.Current;
                if (_context == null || _context.Request == null) return;
                string path = _context.Request.Url.AbsolutePath;
                path = path.IndexOf("v1/") > -1 ? path.Substring(path.LastIndexOf("v1/") + 3) : path;
                path = path.Replace("/", "_");
                WriteLog(path, sql, retValue, elapsedTime);
            }
            catch { }
        }
        #region 日志写入的方法
        //日志目录,相对于根路径
        private static string log_path = "logs_sql_Elapsed";
        private static readonly object _lock = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="retValue"></param>
        /// <param name="elapsedTime"></param>
        public static void WriteLog(string path, string sql, ReturnValue retValue, long elapsedTime)
        {
            Task task = new Task(() =>
            {
                _writeLog(path, sql, retValue, elapsedTime);
            });
            task.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="retValue"></param>
        /// <param name="elapsedTime"></param>
        /// <returns></returns>
        protected static void _writeLog(string path, string sql, ReturnValue retValue, long elapsedTime)
        {


            long elapsedNumber = elapsedTime / 1000 * 1000;
            if (elapsedNumber <= 0) return;
            //如果日志目录不存在就创建
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            //string apiName=path.i
            string currPath = rootPath + log_path + "/" + (elapsedNumber + "." + path) + "/";
            if (!Directory.Exists(currPath)) Directory.CreateDirectory(currPath);

            //日志文件
            DateTime time = DateTime.Now;
            int logcount = System.IO.Directory.GetFiles(currPath).Length + 1;
            string filename = currPath + time.ToString("yyyy.MM.dd HH.mm.ss fff ") + logcount + ".log";

            //创建或打开日志文件，向日志文件末尾追加记录
            using (StreamWriter mySw = File.AppendText(filename))
            {
                //向日志文件写入内容
                StringBuilder sb = new StringBuilder();
                try
                {
                    sb.AppendLine(string.Format("=>SQL elapsed time: {0},\t API: {1} ", time, path));
                    sb.AppendLine(sql);
                    sb.AppendLine("------------------- ReturnValue  ------------------------------");
                    sb.AppendLine("ElapsedTime: " + elapsedTime.ToString());
                    //返回值
                    string json = string.Empty;
                    if (retValue.Data is SourceReader)
                    {
                        //SourceTable table = ((SourceReader)retValue.Data).ToTable();
                        //json = DataResult.ObjectToJson(table);
                    }
                    else if (retValue.Data is DataSet)
                    {
                        DataSet ds = (DataSet)retValue.Data;
                        json = DataResult.ObjectToJson(ds.Tables[0]);
                    }
                    else
                    {
                        json = DataResult.ObjectToJson(retValue.Data);
                    }
                    sb.AppendLine("ReturnValue: " + json);
                }
                catch (Exception ex)
                {
                    mySw.WriteLine(sb.ToString());
                    mySw.WriteLine(ex.ToString());
                }

                try
                {
                    mySw.WriteLine(sb.ToString());
                }
                catch (Exception ex)
                {
                    mySw.WriteLine(sb.ToString());
                    mySw.WriteLine(ex.ToString());
                }
                //输出空行
                int empty_line = 5;
                while (empty_line-- > 0) mySw.WriteLine("");
                //关闭日志文件
                mySw.Close();
            }
        }
        #endregion

    }
}
