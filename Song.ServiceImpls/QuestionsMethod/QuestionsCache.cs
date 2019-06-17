using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Song.Entities;
using System.Timers;
using WeiSha.Common;
using Song.ServiceInterfaces;

namespace Song.ServiceImpls.QuestionsMethod
{
    /// <summary>
    /// 试题的缓存
    /// </summary>
    public class QuestionsCache
    {
        private static readonly QuestionsCache _questionsCache = new QuestionsCache();
        /// <summary>
        /// 试题的缓存对象
        /// </summary>
        public static QuestionsCache Singleton
        {
            get { return _questionsCache; }
        }
        /// <summary>
        /// 缓存序列
        /// </summary>
        private List<QuestionsCache_Item> list = new List<QuestionsCache_Item>();
        /// <summary>
        /// 缓存个数
        /// </summary>
        public int Count
        {
            get { return list.Count; }
        }
        System.Timers.Timer myTimer = null;
        private QuestionsCache()
        {
            int mm = 3;    //间隔执行的分钟数
            myTimer = new System.Timers.Timer(mm * 60 * 1000);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;
            myTimer.Start();
        }
        /// <summary>
        /// 添加试题集
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="expires"></param>
        public void Add(List<Questions> ques, int expires)
        {
            QuestionsCache_Item qcitem = new QuestionsCache_Item(ques, expires);
            list.Add(qcitem);
        }
        /// <summary>
        /// 添加试题集
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="expires"></param>
        /// <returns>返回缓存的唯一标识</returns>
        public string Add(Questions[] ques, int expires)
        {
            QuestionsCache_Item qcitem = new QuestionsCache_Item(ques, expires);
            list.Add(qcitem);
            return qcitem.UID;
        }
        /// <summary>
        /// 添加试题集
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="expires"></param>
        /// <returns>返回缓存的唯一标识</returns>
        public string Add(Questions[] ques, int expires,string uid)
        {
            QuestionsCache_Item qcitem = new QuestionsCache_Item();
            qcitem.Questions = ques.ToList<Questions>();
            qcitem.Expires = expires;
            qcitem.UID = uid;
            list.Add(qcitem);
            return qcitem.UID;
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="ques">试题列表</param>
        /// <param name="expires"></param>
        /// <param name="uid"></param>
        public string Update(Questions[] ques, int expires, string uid)
        {
            QuestionsCache_Item qci = GetCache(uid);
            if (qci != null)
            {
                if (!string.IsNullOrWhiteSpace(uid)) qci.UID = uid;
                if (expires > 0) qci.Expires = expires;
                if (ques != null) qci.Questions = ques.ToList<Questions>();
            }
            else
            {
                qci = new QuestionsCache_Item(ques, expires);
                qci.UID = uid;
                this.list.Add(qci);
            }
            return qci.UID;
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="exr">答题内容</param>
        /// <param name="expires"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string Update(ExamResults exr, int expires, string uid)
        {
            QuestionsCache_Item qci = GetCache(uid);
            if (qci != null)
            {
                if (!string.IsNullOrWhiteSpace(uid)) qci.UID = uid;
                if (expires > 0) qci.Expires = expires;
                if (exr != null) qci.Result = exr;
            }
            else
            {
                qci = new QuestionsCache_Item();
                if (expires > 0) qci.Expires = expires;
                if (exr != null) qci.Result = exr;
                qci.UID = uid;
                this.list.Add(qci);
            }
            //如果交卷
            if (exr.Exr_IsSubmit && !exr.Exr_IsCalc)qci.Calculate();
            return qci.UID;
        }
        public void Delete(string uid)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].UID == uid) list.Remove(list[i]);                
            }
        }
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="uid">用于区分缓存对象的uid</param>
        /// <returns></returns>
        public QuestionsCache_Item GetCache(string uid)
        {
            //先取缓存试题集
            QuestionsCache_Item qci = null;
            for (int i = 0; i < list.Count; i++)
            {
                if (uid == list[i].UID)
                {
                    qci = list[i];
                    break;
                }
            }
            return qci;
        }
        /// <summary>
        /// 从缓存中获取试题
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        public Song.Entities.Questions GetSingle(int qid)
        {
            Questions ques = null;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Questions.Count; j++)
                {
                    if (qid == list[i].Questions[j].Qus_ID)
                    {
                        ques = list[i].Questions[j];
                        break;
                    }
                }
            }
            return ques;
        }
        /// <summary>
        /// 从缓存中获取试题
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Song.Entities.Questions GetSingle(string uid)
        {
            Questions ques = null;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Questions.Count; j++)
                {
                    if (uid == list[i].Questions[j].Qus_UID)
                    {
                        ques = list[i].Questions[j];
                        break;
                    }
                }
            }
            return ques;
        }
        /// <summary>
        /// 从缓存中更新试题
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        public void UpdateSingle(Song.Entities.Questions ques)
        {            
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Questions.Count; j++)
                {
                    if (ques.Qus_ID == list[i].Questions[j].Qus_ID)
                    {
                        list[i].Questions[j] = ques;
                    }
                }
            }
        }
        /// <summary>
        /// 获取缓存中的试题集
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Questions> GetQuestions(string uid)
        {
            QuestionsCache_Item qci = this.GetCache(uid);
            if (uid == "all")
            {
                if (qci == null)
                {
                    Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, true, -1);
                    this.Delete("all");
                    this.Add(ques, int.MaxValue, "all");
                }
                qci = this.GetCache(uid);
            }           
            if (qci == null) return null;
            return qci.Questions;
        }
        /// <summary>
        /// 获取答题信息
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public ExamResults GetExamResults(string uid)
        {
            QuestionsCache_Item qci = this.GetCache(uid);
            if (qci == null) return null;
            return qci.Result;
        }
        /// <summary>
        /// 清理过期试题缓存
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Result == null) continue;
                if (list[i].Result.Exr_OverTime >= DateTime.Now)
                {
                    //如果有答题信息，在清除前计算成功，并存储
                    if (list[i].Result != null && list[i].IsProcessing==false && !list[i].Result.Exr_IsCalc)
                    {
                        list[i].Calculate();    //清除之前计算成绩
                    }
                    list.Remove(list[i]);
                }
            }
        }
        private static readonly object savelock = new object();
        /// <summary>
        /// 保存到数据库
        /// </summary>
        public void Save()
        {
            lock (savelock)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Result == null) continue;
                    Business.Do<IExamination>().ResultAdd(list[i].Result);
                }
            }
        }
        private void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            this.Save();
            this.Clear();            
        }
       
    }
    /// <summary>
    /// 存放试题集的缓存对象
    /// </summary>
    public class QuestionsCache_Item{
        /// <summary>
        /// 唯一ID
        /// </summary>
        public string UID { get; set; }
        private int _expires = 10;
        /// <summary>
        /// 失效时间，单位为分钟
        /// </summary>
        public int Expires
        {
            get { return _expires; }
            set {
                _deadline = DateTime.Now.AddMinutes(value);
                _expires = value;
            }
        }
        private DateTime _deadline = DateTime.Now;
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime Deadline
        {
            get { return _deadline; }
        }
        private List<Questions> _questions = new List<Questions>();
        /// <summary>
        /// 被缓存的试题
        /// </summary>
        public List<Questions> Questions
        {
            get { return _questions; }
            set { _questions = value; }
        }
        private Song.Entities.ExamResults _result=null;
        /// <summary>
        /// 考试的答题信息
        /// </summary>
        public Song.Entities.ExamResults Result
        {
            get { return _result; }
            set { _result = value; }
        }
        private readonly object _lock = new object();
        private bool _isProcessing = false;
        /// <summary>
        /// 是否正在处理数据
        /// </summary>
        public bool IsProcessing
        {
            get { return _isProcessing; }
            set { _isProcessing = value; }
        }
        #region 构造方法
        public QuestionsCache_Item()
        {
           
        }
        public QuestionsCache_Item(List<Questions> ques,int expires)
        {
            _questions = ques;
            _expires = expires;
            _deadline = DateTime.Now.AddMinutes(_expires);
            UID = Guid.NewGuid().ToString();
        }
        public QuestionsCache_Item(Questions[] ques, int expires)
        {
            _questions = ques.ToList<Questions>();
            _expires = expires;
            _deadline = DateTime.Now.AddMinutes(_expires);
            UID = Guid.NewGuid().ToString();
        }
        #endregion

        /// <summary>
        /// 计算成绩
        /// </summary>
        /// <returns></returns>
        public double Calculate()
        {
            this._isProcessing = true;
            lock (this._lock)
            {
                Business.Do<IExamination>().ClacScore(this.Result);
                this._isProcessing = false;
            }
            return this.Result.Exr_ScoreFinal;
        }
    }
}
