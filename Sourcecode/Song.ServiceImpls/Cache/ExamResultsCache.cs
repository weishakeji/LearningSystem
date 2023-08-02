using System;
using System.Collections.Generic;
using System.Linq;
using Song.Entities;
using System.Timers;
using WeiSha.Core;
using Song.ServiceInterfaces;
using System.Threading.Tasks;

namespace Song.ServiceImpls.Cache
{
    /// <summary>
    /// 专项考试中的答题信息缓存
    /// </summary>
    public class ExamResultsCache
    {
        /// <summary>
        /// 缓存序列
        /// </summary>
        private static readonly List<ExamResultsCache_Item> list = new List<ExamResultsCache_Item>();
        private static readonly object _lock = new object();
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="exr">答题内容</param>
        /// <param name="expires"></param>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        public static string Update(ExamResults exr, int expires)
        {
            int examid = exr.Exam_ID, acid = exr.Ac_ID;
            long tpid = exr.Tp_Id;
            ExamResultsCache_Item erc = GetCache(examid, tpid, acid);
            if (erc != null)
            {
                if (expires > 0) erc.Expires = expires;
                if (exr != null) erc.Result = exr;
            }
            else
            {
                erc = new ExamResultsCache_Item(examid, tpid, acid);
                if (expires > 0) erc.Expires = expires;
                if (exr != null) erc.Result = exr;
                list.Add(erc);
                Business.Do<IExamination>().ResultAdd(exr);
            }
            if (list.Count > 0) setPolling();
            //如果交卷
            if (exr.Exr_IsSubmit && !exr.Exr_IsCalc) erc.Calculate();
            return erc.UID;
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        public static void Delete(int examid, long tpid, int acid)
        {
            lock (ExamResultsCache._lock)
            {
                string uid = ExamResultsCache_Item.GenerateUid(examid, tpid, acid);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].UID == uid) list.Remove(list[i]);
                }
                if (list.Count < 1) delPolling();
            }
        }
        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="exr"></param>
        public static void Delete(ExamResults exr)
        {
            if (exr != null)
                Delete(exr.Exam_ID, exr.Tp_Id, exr.Ac_ID);
        }
        /// <summary>
        /// 删除当前考试的所有缓存
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public static int Delete(int examid)
        {
            lock (ExamResultsCache._lock)
            {
                int count = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ExamID == examid)
                    {
                        list.Remove(list[i]);
                        count++;
                    }
                }
                if (list.Count < 1) delPolling();
                return count;
            }
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>    
        /// <returns></returns>
        public static ExamResultsCache_Item GetCache(int examid, long tpid, int acid)
        {
            string uid = ExamResultsCache_Item.GenerateUid(examid, tpid, acid);
            //先取缓存试题集
            ExamResultsCache_Item erc = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (uid == list[i].UID)
                {
                    erc = list[i];
                    break;
                }
            }
            return erc;
        }
        /// <summary>
        /// 获取缓存中的考试答题信息
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        public static ExamResults GetResults(int examid, long tpid, int acid)
        {
            ExamResultsCache_Item erc = GetCache(examid, tpid, acid);
            if (erc == null) return null;
            return erc.Result;
        }

        /// <summary>
        /// 缓存数量
        /// </summary>
        /// <returns></returns>
        public static int Count()
        {
            return list.Count;
        }
        /// <summary>
        /// 某个考试下的答题数量
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public static int Count(int examid)
        {
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ExamID == examid) count++;
            }         
            return count;
        }
        /// <summary>
        /// 清理过期试题缓存
        /// </summary>
        public static void Clear()
        {
            lock (ExamResultsCache._lock)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Result == null) continue;
                    if (list[i].Result.Exr_OverTime < DateTime.Now || list[i].Result.Exr_IsSubmit)
                    {
                        //如果有答题信息，在清除前计算成功，并存储
                        if (list[i].IsProcessing == false && !list[i].Result.Exr_IsCalc)
                        {
                            list[i].Calculate();    //清除之前计算成绩
                        }
                        list.Remove(list[i]);
                    }
                }
                if (list.Count < 1) delPolling();
            }
        }
        /// <summary>
        /// 保存到数据库
        /// </summary>
        public static void Save()
        {
            new Task(() =>
            {
                lock (ExamResultsCache._lock)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Result == null) continue;
                        Business.Do<IExamination>().ResultAdd(list[i].Result);
                    }
                }
            }).Start();
        }
        #region 设置轮询处理缓存的物理化与删除
        //间隔执行的分钟数
        private static int interval_num = 3;
        private static void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            Save();
            Clear();
        }
        private static System.Timers.Timer myTimer = null;
        private static readonly object _locktimer = new object();
        /// <summary>
        /// 设置轮询处理
        /// </summary>
        private static void setPolling()
        {
            lock (_locktimer)
            {
                if (myTimer == null)
                {
                    int mm = interval_num;    //间隔执行的分钟数
                    myTimer = new System.Timers.Timer(mm * 60 * 1000);
                    myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
                    myTimer.Enabled = true;
                    myTimer.AutoReset = true;
                    myTimer.Start();
                }
            }
        }
        /// <summary>
        /// 删除轮询操作
        /// </summary>
        private static void delPolling()
        {
            lock (_locktimer)
            {
                if (myTimer != null)
                {
                    // 停止计时器并清除
                    myTimer.Stop();
                    myTimer.Dispose();
                    myTimer = null;
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// 存放试题集的缓存对象
    /// </summary>
    public class ExamResultsCache_Item
    {
        private int _expires = 10;
        /// <summary>
        /// 唯一ID
        /// </summary>
        public string UID { get; private set; }       
        /// <summary>
        /// 失效时间，单位为分钟
        /// </summary>
        public int Expires
        {
            get { return _expires; }
            set
            {
                Deadline = DateTime.Now.AddMinutes(value);
                _expires = value;
            }
        }      
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime Deadline { get; set; }   
        /// <summary>
        /// 考试的答题信息
        /// </summary>
        public Song.Entities.ExamResults Result { get; set; }
        /// <summary>
        /// 考试场次ID
        /// </summary>
        public int ExamID { get; set; }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public long PaperID { get; set; }
        /// <summary>
        /// 学员账号ID
        /// </summary>
        public int AccountID { get; set; }
        /// <summary>
        /// 是否正在处理数据
        /// </summary>
        public bool IsProcessing { get; set; }

        private readonly object _lock = new object();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        public ExamResultsCache_Item(int examid, long tpid, int acid)
        {
            this.UID = GenerateUid(examid, tpid, acid);
            this.ExamID = examid;
            this.PaperID = tpid;
            this.AccountID = acid;
        }
        /// <summary>
        /// 生成缓存ID
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="tpid"></param>
        /// <param name="acid"></param>
        /// <returns></returns>
        public static string GenerateUid(int examid, long tpid, int acid)
        {
            return string.Format("ExamResults：{0}-{1}-{2}", examid, tpid, acid); 
        }
        /// <summary>
        /// 计算成绩
        /// </summary>
        /// <returns></returns>
        public double Calculate()
        {
            this.IsProcessing = true;
            lock (this._lock)
            {
                if (this.Result == null) return 0;
                Business.Do<IExamination>().ClacScore(this.Result);
                this.IsProcessing = false;
            }
            return this.Result.Exr_ScoreFinal;
        }
    }
}
