using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Song.ViewData.Helper
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public class Logs
    {
        //日志目录,相对于根路径
        private static string log_path = "logs_viewdata";
        private static int log_level = -1;
        #region 异常写入记录

        /// <summary>
        /// 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        /// </summary>
        public static int LOG_LEVEL
        {
            get
            {
                if (log_level >= 0) return log_level;
                int level = WeiSha.Core.App.Get["LOG_LEVEL"].Int16 ?? 0;
                log_level = level >= 0 ? level : level;
                return log_level;
            }
        }

        /// <summary>
        /// 向日志文件写入调试信息
        /// </summary>
        /// <param name="letter">类名</param>
        /// <param name="content">要写入的内容</param>
        public static void Debug(Letter letter, string content)
        {
            if (LOG_LEVEL >= 3) WriteLog("DEBUG", letter, content);
        }

        /// <summary>
        /// 向日志文件写入信息
        /// </summary>
        /// <param name="letter">类名</param>
        /// <param name="content">要写入的内容</param>
        public static void Info(Letter letter, string content)
        {
            if (LOG_LEVEL >= 2) WriteLog("INFO", letter, content);

        }
        /// <summary>
        /// 向日志文件出错信息
        /// </summary>
        /// <param name="letter">类名</param>
        /// <param name="content">要写入的内容</param>
        public static void Error(Letter letter, string content)
        {
            if (LOG_LEVEL >= 1) WriteLog("ERROR", letter, content);

        }
        /// <summary>
        /// 向日志文件出错信息
        /// </summary>
        /// <param name="letter">接口请求的对方</param>
        /// <param name="ex">错误信息</param>
        public static void Error(Letter letter, Exception ex)
        {
            if (LOG_LEVEL < 1) return;
            StringBuilder sb = new StringBuilder();
            //输出异常，全部输出（子异常也输出）
            Exception except = ex;
            while (except != null)
            {
                sb.AppendLine("-------------------------------------");
                sb.AppendLine("Exception:" + ex.GetType().FullName);
                if (ex is VExcept)
                {
                    VExcept ve = (VExcept)ex;
                    sb.AppendLine("StateCode:" + ve.State);
                }
                sb.AppendLine("Message:" + ex.Message);
                sb.AppendLine("Source:" + ex.Source);
                sb.AppendLine("StackTrace:" + ex.StackTrace);
                sb.AppendLine("TargetSite:" + ex.TargetSite);
                sb.AppendLine("ToString:" + ex.ToString());
                except = except.InnerException;
            }
            WriteLog("ERROR", letter, sb.ToString());
        }
        private static readonly object _lock = new object();
        /// 实际写日志的方法
        /// </summary>
        /// <param name="logtype">日志类型</param>
        /// <param name="letter">请求接口的对象方法</param>
        /// <param name="content">实际内容</param>
        public static void WriteLog(string logtype, Letter letter, string content)
        {
            Task task = new Task(() =>
            {
                Helper.Logs._writeLog(logtype, letter, content);
            });
            task.Start();
        }
        /// <summary>
        /// 实际写日志的方法
        /// </summary>
        /// <param name="logtype">日志类型</param>
        /// <param name="letter">请求接口的对象方法</param>
        /// <param name="content">实际内容</param>
        protected static string _writeLog(string logtype, Letter letter, string content)
        {
            lock (_lock)
            {
                string api_path = letter.ClassName + "." + letter.MethodName;
                //日志写的路径，{{log_path}}/{{log_type}}/{{date}}
                string path = string.Format("/{0}/{1}/{2}/", log_path, logtype, DateTime.Now.ToString("yyyy.MM.dd"));
                path = WeiSha.Core.Server.MapPath(path + api_path + "/");
                //如果日志目录不存在就创建
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                //日志文件
                DateTime time = DateTime.Now;
                int logcount = System.IO.Directory.GetFiles(path).Length+1;
                string filename = path + time.ToString("yyyy.MM.dd HH.mm.ss fff ") + logcount + ".log";
                

                //创建或打开日志文件，向日志文件末尾追加记录
                using (StreamWriter mySw = File.AppendText(filename))
                {
                    //向日志文件写入内容
                    StringBuilder sb = new StringBuilder();
                    try
                    {
                        sb.AppendLine(string.Format("=>{0}: {1},\t API: {2} ", logtype, time, letter.API_PATH));
                        sb.AppendLine("Browser:" + letter.Browser);
                        //来源网页,http谓词,
                        sb.AppendLine("WEB_PAGE:" + letter.WEB_PAGE);
                        sb.AppendLine("WEB_HOST:" + letter.WEB_HOST);
                        sb.AppendLine("HTTP_REFERER:" + letter.HTTP_REFERER);
                        sb.AppendLine("HTTP_METHOD:" + letter.HTTP_METHOD);
                        sb.AppendLine("HTTP_HOST:" + letter.HTTP_HOST);
                        sb.AppendLine("Custom_METHOD:" + letter.Custom_METHOD);
                        sb.AppendLine("Custom_Action:" + letter.Custom_Action);
                        sb.AppendLine("-------------------------------------");
                    }
                    catch (Exception ex)
                    {
                        mySw.WriteLine(sb.ToString());
                        mySw.WriteLine(ex.ToString());
                    }

                    try
                    {
                        //客户端递交的参数
                        if (letter.Params.Count > 0)
                        {
                            sb.AppendLine("Params:");
                            foreach (KeyValuePair<string, string> kv in letter.Params)
                            {
                                sb.AppendLine(kv.Key + ":" + kv.Value);
                            }
                        }
                        else
                        {
                            sb.AppendLine("Params:(null)");
                        }
                        sb.AppendLine("-------------------------------------");
                        sb.AppendLine("online:" + LoginAccount.list.Count.ToString());
                       
                        //学员，教师，管理员
                        Song.Entities.Accounts acc = LoginAccount.Status.User(letter);
                        if (acc == null)
                        {
                            sb.AppendLine("Account:(null)");
                        }
                        else
                        {
                            sb.AppendLine(string.Format("Account:{0}\tID:{1}\tName:{2}\tMobile:{3}\tIDCard:{4}",
                                acc.Ac_AccName, acc.Ac_ID, acc.Ac_Name, acc.Ac_MobiTel1, acc.Ac_IDCardNumber));
                            Song.Entities.Teacher th = LoginAccount.Status.Teacher(letter);
                            if (acc.Ac_IsTeacher && th != null)
                            {
                                sb.AppendLine(string.Format("Teacher:{0}\t", th.Th_Name));
                            }
                        }
                        Song.Entities.EmpAccount emp = LoginAdmin.Status.User(letter);
                        if (emp == null)
                            sb.AppendLine("Admin:(null)");
                        else
                            sb.AppendLine(string.Format("Admin:{0}\tID:{1}\tName:{2}\tMobile:{3}\tOrgid:{4}",
                                emp.Acc_AccName, emp.Acc_Id, emp.Acc_Name, emp.Acc_MobileTel, emp.Org_ID));

                        mySw.WriteLine(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        mySw.WriteLine(sb.ToString());
                        mySw.WriteLine(ex.ToString());
                    }
                    mySw.WriteLine(content);

                    mySw.WriteLine("===========================================");

                    int empty_line = 5;
                    while (empty_line-- > 0) mySw.WriteLine("");
                    //关闭日志文件
                    mySw.Close();
                }

                return filename;
            }
        }

        #endregion
    }
}
