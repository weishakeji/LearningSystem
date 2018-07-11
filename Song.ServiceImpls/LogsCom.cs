using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;



namespace Song.ServiceImpls
{
    public class LogsCom :ILogs
    {
        #region 登录日志的管理
        /// <summary>
        /// 增加登录日志
        /// </summary>
        public void AddLoginLogs()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Song.Entities.Logs log = new Logs();
            //登录时间
            log.Log_Time = DateTime.Now;
            log.Log_Type = "login";
            log.Acc_Id = Extend.LoginState.Admin.CurrentUser.Acc_Id;
            log.Acc_Name = Extend.LoginState.Admin.CurrentUser.Acc_Name;
            log.Log_IP = WeiSha.Common.Browser.IP;
            log.Log_OS = this.GetOSNameByUserAgent(_context.Request.UserAgent);
            log.Log_Browser = _context.Request.Browser.Browser + " " + _context.Request.Browser.Version;
            log.Log_FileName = _context.Request.Url.AbsolutePath;
            this.Add(log);
            //时限长度
            int span = Business.Do<ISystemPara>()["SysLoginTimeSpan"].Int16 ?? 30;
            this.Clear(span, "login");            
        }
        /// <summary>
        /// 增加操作日志
        /// </summary>
        public void AddOperateLogs()
        {
            System.Web.HttpContext _context = System.Web.HttpContext.Current;
            Song.Entities.Logs log = new Logs();
            //操作时间
            log.Log_Time = DateTime.Now;
            log.Log_Type = "operate";
            log.Acc_Id = Extend.LoginState.Admin.CurrentUser.Acc_Id;
            log.Acc_Name = Extend.LoginState.Admin.CurrentUser.Acc_Name;
            log.Log_IP = WeiSha.Common.Browser.IP;
            log.Log_OS = this.GetOSNameByUserAgent(_context.Request.UserAgent);
            log.Log_Browser = _context.Request.Browser.Browser + " " + _context.Request.Browser.Version;
            log.Log_FileName = _context.Request.Url.AbsolutePath;
            string file = log.Log_FileName.ToLower().Replace("/manage/", "");
            ManageMenu mm=Gateway.Default.From<ManageMenu>().Where(ManageMenu._.MM_Link==file).ToFirst<ManageMenu>();
            if (mm != null)
            {
                log.Log_MenuName = mm.MM_Name;
                log.Log_MenuId = mm.MM_Id;
            }
            this.Add(log);
            //时限长度
            int span = Business.Do<ISystemPara>()["SysWorkTimeSpan"].Int16 ?? 30;
            this.Clear(span, "operate");            
        }
        /// <summary>
        /// 根据 User Agent 获取操作系统名称
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        private string GetOSNameByUserAgent(string userAgent)
        {
            return WeiSha.Common.Server.OS;            
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Add(Logs entity)
        {
            Gateway.Default.Save<Logs>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            Gateway.Default.Delete<Logs>(Logs._.Log_Id==identify);
        }
        /// <summary>
        /// 根据分类、对象id删除
        /// </summary>
        /// <param name="accId">用户id</param>
        public void Delete4Acc(int accId)
        {
            Gateway.Default.Delete<Logs>(Logs._.Acc_Id == accId);
        }
        /// <summary>
        /// 清理多少天之前的日志
        /// </summary>
        /// <param name="day">天数</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        public void Clear(int day, string type)
        {
            Gateway.Default.Delete<Logs>(Logs._.Log_Type == type && Logs._.Log_Time < DateTime.Now.AddDays(-day));
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Logs GetSingle(int identify)
        {
            return Gateway.Default.From<Logs>().Where(Logs._.Log_Id == identify).ToFirst<Logs>();
        }
        /// <summary>
        /// 获取某用户最近访问的操作项
        /// </summary>
        /// <param name="accId">用户id</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="count">返回的个数</param>
        /// <returns></returns>
        public DataSet GetLately(int accId, string type, int count)
        {
            string sql = @"select top {count} * from 
                (SELECT logs.Log_MenuName, logs.Log_MenuId, logs.Log_FileName, Max(logs.Log_Time) AS Log_Time
                FROM logs
                WHERE 1=1 AND logs.Log_Type='{type}' 
                GROUP BY logs.Log_MenuName, logs.Log_MenuId, logs.Log_FileName) as t order by log_time desc";
            sql = sql.Replace("{type}", type);
            sql = sql.Replace("{count}",count.ToString());
            if (accId > 0)
            {
                sql=sql.Replace("1=1","Acc_Id="+accId);
            }
            return Gateway.Default.FromSql(sql).ToDataSet();
        }
        /// <summary>
        /// 获取某用户某时间段内，访问次数最多的操作项
        /// </summary>
        /// <param name="accId">用户id</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="count">返回的个数</param>
        /// <returns></returns>
        public DataSet GetFrequently(int accId, string type, int count)
        {
            string sql = @"select top {count} * from 
                (SELECT logs.Log_MenuName, logs.Log_MenuId, logs.Log_FileName, count(logs.Log_MenuName) AS num
                FROM logs
                WHERE 1=1 AND logs.Log_Type='{type}'
                GROUP BY logs.Log_MenuName, logs.Log_MenuId, logs.Log_FileName) as t  order by num desc,log_menuId desc";
            sql = sql.Replace("{type}", type);
            sql = sql.Replace("{count}", count.ToString());
            if (accId > 0)
                sql = sql.Replace("1=1", "Acc_Id=" + accId);
            return Gateway.Default.FromSql(sql).ToDataSet();
        }
        /// <summary>
        /// 分页获取所有日志记录
        /// </summary>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public Logs[] GetPager(string type, int size, int index, out int countSum)
        {
            WhereClip wc = Logs._.Log_Type == type;
            countSum = Gateway.Default.Count<Logs>(wc);
            return Gateway.Default.From<Logs>().Where(wc).OrderBy(Logs._.Log_Time.Desc).ToArray<Logs>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Logs[] GetPager(string type, DateTime start, DateTime end, 
            int size, int index, out int countSum)
        {
            WhereClip wc = Logs._.Log_Type == type;
            wc.And(Logs._.Log_Time > start && Logs._.Log_Time < end);
            countSum = Gateway.Default.Count<Logs>(wc);
            return Gateway.Default.From<Logs>()
                .Where(wc).OrderBy(Logs._.Log_Time.Desc)
                .ToArray<Logs>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取所有日志记录
        /// </summary>
        /// <param name="accId">员工id</param>
        /// <param name="type">日志类别，暂分为login,operate，即登录日志，操作日志</param>
        /// <param name="size">每页显示几条记录</param>
        /// <param name="index">当前第几页</param>
        /// <param name="countSum">记录总数</param>
        /// <returns></returns>
        public Logs[] GetPager(int accId, string type, int size, int index, out int countSum)
        {
            WhereClip wc = Logs._.Log_Type == type;
            wc.And(Logs._.Acc_Id == accId);
            countSum = Gateway.Default.Count<Logs>(wc);
            return Gateway.Default.From<Logs>().Where(wc).OrderBy(Logs._.Log_Time.Desc).ToArray<Logs>(size, (index - 1) * size);
        }
        public Logs[] GetPager(int accId, string type, DateTime start, DateTime end, int size, int index, out int countSum)
        {
            WhereClip wc = Logs._.Log_Type == type;
            wc.And(Logs._.Acc_Id == accId);
            wc.And(Logs._.Log_Time > start && Logs._.Log_Time < end);
            countSum = Gateway.Default.Count<Logs>(wc);
            return Gateway.Default.From<Logs>().Where(wc).OrderBy(Logs._.Log_Time.Desc).ToArray<Logs>(size, (index - 1) * size);
        }
        public Logs[] GetPager(int accId, string mmSear, string type, DateTime start, DateTime end, int size, int index, out int countSum)
        {
            WhereClip wc = Logs._.Log_Type == type;
            if (accId > 0)
            {
                wc.And(Logs._.Acc_Id == accId);
            }
            if (mmSear.Trim() != "")
            {
                wc.And(Logs._.Log_MenuName.Like("%" + mmSear + "%"));
            }
            wc.And(Logs._.Log_Time > start && Logs._.Log_Time < end);
            countSum = Gateway.Default.Count<Logs>(wc);
            
            return Gateway.Default.From<Logs>().Where(wc).OrderBy(Logs._.Log_Time.Desc).ToArray<Logs>(size, (index - 1) * size);
        }
        #endregion

        #region 学员练习记录
        /// <summary>
        /// 添加练习记录
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="ques">试题id</param>
        /// <param name="index">试题页面中的索引</param>
        /// <returns></returns>
        public LogForStudentQuestions QuestionAdd(int acid, int couid, int olid, int ques, int index)
        {
            LogForStudentQuestions entity = new LogForStudentQuestions();
            entity.Ac_ID = acid;
            entity.Cou_ID = couid;
            entity.Ol_ID = olid;
            entity.Qus_ID = ques;
            entity.Lsq_Index = index;
            entity.Lsq_CrtTime = DateTime.Now;
            entity.Lsq_LastTime = entity.Lsq_CrtTime;
            Gateway.Default.Save<LogForStudentQuestions>(entity);
            return entity;
        }
        /// <summary>
        /// 修改练习记录
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="ques">试题id</param>
        /// <param name="index">试题页面中的索引</param>
        /// <returns></returns>
        public LogForStudentQuestions QuestionUpdate(int acid, int couid, int olid, int ques, int index)
        {
            LogForStudentQuestions entity = this.QuestionSingle(acid, couid, 0);
            if (entity == null) entity = this.QuestionAdd(acid, couid, 0, ques, index);
            entity.Qus_ID = ques;
            entity.Lsq_Index = index;
            entity.Lsq_LastTime = DateTime.Now;
            entity.Ol_ID = olid;
            Gateway.Default.Save<LogForStudentQuestions>(entity);
            return entity;
        }
        /// <summary>
        /// 获取练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        public LogForStudentQuestions QuestionSingle(int acid, int couid, int olid)
        {
            WhereClip wc = new WhereClip();
            wc &= LogForStudentQuestions._.Ac_ID == acid;
            wc &= LogForStudentQuestions._.Cou_ID == couid;
            if (olid > 0) wc &= LogForStudentQuestions._.Ol_ID == olid;
            return Gateway.Default.From<LogForStudentQuestions>().Where(wc).ToFirst<LogForStudentQuestions>();
        }
        /// <summary>
        /// 删除学员的练习记录
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        public void QuestionDelete(int acid, int couid, int olid)
        {
            WhereClip wc = new WhereClip();
            wc &= LogForStudentQuestions._.Ac_ID == acid;
            wc &= LogForStudentQuestions._.Cou_ID == couid;
            if (olid > 0) wc &= LogForStudentQuestions._.Ol_ID == olid;
            Gateway.Default.Delete<LogForStudentQuestions>(wc);
        }
        #endregion
    }
}
