using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song.Entities;
using System.Reflection;
using WeiSha.Core;
using System.Threading;

namespace Song.ServiceImpls.Exam
{
    /// <summary>
    /// 批量处理考试成绩
    /// </summary>
    public class BatchResults
    {
       
        private static ExaminationCom examCom = new ExaminationCom();
       

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="examid"></param>
        public BatchResults(int examid)
        {
            this.ExamID = examid;
            this.Exam = examCom.ExamSingle(examid);           
        }
        public BatchResults(int examid, int minScore, int maxScore, DateTime minTime, DateTime maxTime, int minSpan, int maxSpan)
        {
            this.ExamID = examid;
            this.Exam = examCom.ExamSingle(examid);
            this.SetValue(minScore, maxScore, minTime, maxTime, minSpan, maxSpan);
        }
        #region 属性
        private readonly object _lockobj = new object();
        /// <summary>
        /// 考试场次的ID
        /// </summary>
        public int ExamID { get; set; }
        /// <summary>
        /// 考试场次
        /// </summary>
        public Examination Exam { get; set; }
        /// <summary>
        /// 学员总数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 当前学员数，即未计算的学员数
        /// </summary>
        public int Count => this._accounts == null ? 0 : this._accounts.Count;
        private List<Accounts> _accounts = null;
        public List<Accounts> Accounts
        {
            get
            {
                lock (this._lockobj)
                {
                    if (this._accounts == null)
                    {
                        //缺考的学员
                        _accounts = examCom.AbsenceExamAccounts(this.ExamID, null, null, null, 0, int.MaxValue, 1, out int countSum);
                        this.Total = countSum;
                    }
                    return this._accounts;
                }
            }
        }
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
        public DateTime MinTime { get; set; }
        public DateTime MaxTime { get; set; }
        public int MinSpsn { get; set; }
        public int MaxSpsn{ get; set; }

        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            lock (this._lockobj)
            {
                if (this._accounts == null)
                {
                    //缺考的学员
                    _accounts = examCom.AbsenceExamAccounts(this.ExamID, null, null, null, 0, int.MaxValue, 1, out int countSum);

                    this.Total = countSum;
                }
            }
        }
        /// <summary>
        /// 设置随得分的区间，考试时间与用时
        /// </summary>
        /// <param name="minScore"></param>
        /// <param name="maxScore"></param>
        /// <param name="minTime"></param>
        /// <param name="maxTime"></param>
        /// <param name="minSpan"></param>
        /// <param name="maxSpan"></param>
        public void SetValue(int minScore,int maxScore,DateTime minTime,DateTime maxTime,int minSpan,int maxSpan)
        {
            if (minScore < 0) minScore = 0;
            if (maxScore < minScore) maxScore = minScore;
            this.MinScore = minScore;
            this.MaxScore = maxScore;

            //
            this.MinTime = minTime;
            this.MaxTime = maxTime > DateTime.Now ? DateTime.Now : maxTime;

            //
            if (minSpan < 0) minSpan = 0;
            if (maxSpan < minSpan) maxSpan = minSpan;
            this.MinSpsn = minSpan;
            this.MaxSpsn = maxSpan;
        }
        /// <summary>
        /// 设置或创建成绩
        /// </summary>
        public void CreateScore()
        {
            int i = 0;
            while (this.Accounts.Count > 0)
            {
                //Thread.Sleep(300);
                Song.Entities.Accounts acc = this.Accounts[0];
                float score = this.RandomScore(acc);
                DateTime start = this.RandomTime();
                int dura = this.RandomDura();
                examCom.ResultSetScore(this.Exam, acc, score, start, dura);
                this.Accounts.RemoveAt(0);
            }
            BatchResults.RomveTask(this.ExamID);
        }
        /// <summary>
        /// 设置或创建成绩
        /// </summary>
        public void CreateScore(int minScore, int maxScore, DateTime minTime, DateTime maxTime, int minSpan, int maxSpan)
        {
            this.SetValue(minScore, maxScore, minTime, maxTime, minSpan, maxSpan);
            this.CreateScore();
        }
        /// <summary>
        /// 生成随机成绩得
        /// </summary>
        /// <returns></returns>
        public int RandomScore(Accounts acc)
        {
            //按学历计算权重
            int eduweight = 5, acedu = acc.Ac_Education.Convert<int>();
            if (acedu > 0 && acedu < 90)
            {
                if (acedu == 81) eduweight = 1;     //小学
                if (acedu == 71) eduweight = 3;     //初中
                if (acedu == 61) eduweight = 6;     //高中
                if (acedu == 41) eduweight = 7;
                if (acedu == 31) eduweight = 9;     //专科
                if (acedu <= 21) eduweight = 10;    //本科
            }
            //按年龄权重
            double ageweight = 5; int acage = DateTime.Now.Year - acc.Ac_Age;
            if (acage > 70) acage = 70;
            if (acage < 20) acage = 20;
            // 定义峰值年龄（权重最高的年龄）
            const int peakAge = 30;
            int ageDifference = Math.Abs(acage - peakAge);
            const int maxAgeDifference = 30; // 年龄差超过此值权重为1                                             
            ageweight = 10 - 10 * ageDifference / maxAgeDifference;
            ageweight = Math.Max(0, Math.Min(10, ageweight));
            //随机权重
            double rdweight = new Random((int)(WeiSha.Core.Request.SnowID() % int.MaxValue)).Next(10);

            //最终权重
            double weight = (eduweight + ageweight + rdweight) / 3 / 10;
            //供选择的得分区间
            int scorerange = this.MaxScore - this.MinScore;
            int rdrange = (int)(scorerange * (1 - weight));     //随机数的生成区间
            int randomScore = new Random((int)(WeiSha.Core.Request.SnowID() % int.MaxValue)).Next(scorerange);
            randomScore += (int)(scorerange * weight) + this.MinScore;            
            return randomScore;
        }
        /// <summary>
        /// 随机开始考试的时间
        /// </summary>
        /// <returns></returns>
        public DateTime RandomTime()
        {
            if (this.Exam.Exam_DateType == 1) return this.Exam.Exam_Date;
            TimeSpan timeSpan = this.MaxTime - this.MinTime;
            double randomMilliseconds = new Random((int)(WeiSha.Core.Request.SnowID() % int.MaxValue)).NextDouble() * timeSpan.TotalMilliseconds;
            return this.MinTime.AddMilliseconds(randomMilliseconds);
        }
        /// <summary>
        /// 随机考试用时，单位分钟
        /// </summary>
        /// <returns></returns>
        public int RandomDura()
        {
            Random random = new Random((int)(WeiSha.Core.Request.SnowID() % int.MaxValue));
            return random.Next(this.Exam.Exam_Span / 2) + this.Exam.Exam_Span;
        }
        #endregion

        #region 异步管理
        //用处理记录处理的异步线程
        private static readonly List<BatchResults> _list = new List<BatchResults>();
        private static readonly object _locktask = new object();

        public static void Start(BatchResults batchResults)
        {
            lock (_locktask)
            {
                BatchResults br = _list.Find(t => t.ExamID == batchResults.ExamID);
                if (br == null)
                {
                    Task.Run(() =>
                    {
                        batchResults.CreateScore();
                    });
                    _list.Add(batchResults);                  
                }               
            }
        }
        public static void Start(int examid, int minScore, int maxScore, DateTime minTime, DateTime maxTime, int minSpan, int maxSpan)
        {
            lock (_locktask)
            {
                BatchResults br = _list.Find(t => t.ExamID == examid);
                if (br == null)
                {
                    br = new BatchResults(examid, minScore, maxScore, minTime, maxTime, minSpan, maxSpan);
                    br.Init();
                    Task.Run(() =>
                    {
                        br.CreateScore();
                    });
                    _list.Add(br);
                }
            }
        }
        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="examid"></param>
        /// <returns>一个参数为学员总数，一个为剩余的数量</returns>
        public static (int,int) GetTask(int examid)
        {
            lock (_locktask)
            {

                BatchResults br = _list.Find(t => t.ExamID == examid);
                if (br != null) return (br.Total, br.Count);
                return (0, 0);
            }
        }
        public static void RomveTask(int examid)
        {
            lock (_locktask)
            {
                int index = _list.FindIndex(t => t.ExamID == examid);
                if (index >= 0 && _list.Count > index) _list.RemoveAt(index);
            }
        }
        #endregion
    }
}
