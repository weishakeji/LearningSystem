using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song.Entities;
using System.Reflection;

namespace Song.ServiceImpls.Exam
{
    /// <summary>
    /// 考试结果
    /// </summary>
    public class Results
    {
        #region 考试结果数据
        /// <summary>
        /// 考试答题信息的XML对象
        /// </summary>
        public XmlDocument Details { get; set; }
        /// <summary>
        /// 考试结果数据实体对象
        /// </summary>
        public ExamResults Entity { get; set; }
        /// <summary>
        /// 试题类型集合
        /// </summary>
        public List<QuesType> QuesTypes { get; set; }
        #endregion

        #region 构造函数
        /// <summary>
        /// 通过成绩记录的实体，构造
        /// </summary>
        /// <param name="entity"></param>
        public Results(ExamResults entity)
        {
            Entity = entity;
            Details = new XmlDocument();
            Details.LoadXml(entity.Exr_Results);
            //解析考试成绩的属性
            this._initAttr(false);
            //解析试题的答题信息
            this._initData();
        }
        /// <summary>
        /// 通过XML构造
        /// </summary>
        /// <param name="details">答题信息的xml文本</param>
        /// <param name="isbase64">是否需要处理base64编码</param>
        public Results(string details, bool isbase64)
        {
            Details = new XmlDocument();
            Details.LoadXml(details);
            //解析考试成绩的属性
            this._initAttr(isbase64);
            //解析试题的答题信息
            this._initData();
        }
        /// <summary>
        /// 通过XML构造
        /// </summary>
        /// <param name="details">答题信息的xml文本</param>
        public Results(string details) : this(details, false)
        {

        }
        #endregion

        #region 属性
        /// <summary>
        /// 考试场次ID
        /// </summary>
        public int Examid { get; set; }
        /// <summary>
        /// 考试UID，用于获取考试主题，判断是否属于同一场考试
        /// </summary>
        public String ExamUid { get; set; }
        /// <summary>
        /// 考试主题
        /// </summary>
        public string ExamTheme { get; set; }
        /// <summary>
        /// 试卷ID
        /// </summary>
        public long TestPaperID { get; set; }
        /// <summary>
        /// 考试开始时间，例如区间考试，这里是可以开始考试的时间
        /// </summary>
        public DateTime Begin { get; set; }
        /// <summary>
        /// 考试开始答题的时间，是学员真正开始答题的时间
        /// </summary>
        public DateTime Startime { get; set; }
        /// <summary>
        /// 考试结束时间
        /// </summary>
        public DateTime Overtime { get; set; }
        /// <summary>
        /// 学员ID，对象答题信息中的stid
        /// </summary>
        public int AccountID { get; set; }
        /// <summary>
        /// 学员名称，对象答题信息中的stname
        /// </summary>
        public string AccountName { get; set; }
        /// <summary>
        /// 学员组ID，对应答题信息中的stsid
        /// </summary>
        public long SortID { get; set; }
        /// <summary>
        /// 学员性别，对应答题信息中的stsex
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 学员身份证号码，对应答题信息中的stcardid
        /// </summary>
        public string IDCardNumber { get; set; }
        /// <summary>
        /// 考试科目ID，对应答题信息中的sbjid
        /// </summary>
        public long SubjectID { get; set; }
        /// <summary>
        /// 考试科目名称，对应答题信息中的sbjname
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        ///提交方式，1为自动提交，2为交卷
        /// </summary>
        public int Pattern { get; set; }
        /// <summary>
        /// 最后答题的试题索引号，与成绩无关，当刷新考试页时，页面被重新渲染，用于显示当前答题
        /// </summary>
        public int QuesIndex { get; set; }
        /// <summary>
        /// 考试成绩，来自答题信息的计算
        /// </summary>
        public float Score
        {
            get
            {
                float score = 0;
                foreach (QuesType qtype in this.QuesTypes)
                {
                    foreach (QuesAnswer q in qtype.QuesAnswers)
                    {
                        if (q.Type != 4)
                            score += q.Sucess ? q.Score : 0;
                        else
                            score += q.Score;
                    }
                }
                return score;
            }
        }
        #endregion

        #region 解析XML
        /// <summary>
        /// 初始化答题成绩记录中的XML属性
        /// </summary>
        /// <param name="isbase64">是否需要处理base64编码</param>
        private void _initAttr(bool isbase64)
        {
            XmlNode xn = Details.SelectSingleNode("results");
            if (isbase64) xn = _getAttrBase64(xn);

            //考试id，考试主题的uid，考试主题的标题
            this.Examid = int.TryParse(xn.Attributes["examid"]?.Value, out int examid) ? examid : 0;
            this.ExamUid = xn.Attributes["uid"]?.Value;
            this.ExamTheme = xn.Attributes["theme"]?.Value;

            //时间
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //考试开始时间
            long begin = long.TryParse(xn.Attributes["begin"].Value, out long begintm) ? begintm : 0;
            DateTime beginTime = dtStart.Add(new TimeSpan(begin * 10000));
            this.Begin = beginTime;
            //考试结束时间
            long over = long.TryParse(xn.Attributes["overtime"].Value, out long overtm) ? overtm : 0;
            DateTime overTime = dtStart.Add(new TimeSpan(over * 10000));
            this.Overtime = overTime;
            //学员开始考试时间
            long start = long.TryParse(xn.Attributes["starttime"].Value, out long starttm) ? starttm : 0;
            DateTime startTime = dtStart.Add(new TimeSpan(start * 10000));
            this.Startime = startTime;

            //学员相关，学员id、名称，学员组，性别，身份证号
            this.AccountID = int.TryParse(xn.Attributes["stid"]?.Value, out int stid) ? stid : 0;
            this.AccountName = xn.Attributes["stname"]?.Value;
            this.SortID = long.TryParse(xn.Attributes["stsid"]?.Value, out long stsid) ? stsid : 0;
            this.Gender = int.TryParse(xn.Attributes["stsex"]?.Value, out int stsex) ? stsex : 0;
            this.IDCardNumber = xn.Attributes["stcardid"]?.Value;

            //试卷id,专业id,专业名称
            this.TestPaperID = long.TryParse(xn.Attributes["tpid"]?.Value, out long tpid) ? tpid : 0;
            this.SubjectID = long.TryParse(xn.Attributes["sbjid"]?.Value, out long sbjid) ? sbjid : 0;
            this.SubjectName = xn.Attributes["sbjname"]?.Value;

            //交卷方式与当前试题
            this.Pattern = int.TryParse(xn.Attributes["patter"]?.Value, out int patter) ? patter : 0;
            this.QuesIndex = int.TryParse(xn.Attributes["index"]?.Value, out int index) ? index : 0;
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="doc"></param>
        private void _initData()
        {
            //按题型分类，解析答题信息
            XmlNodeList list = Details.SelectSingleNode("results").ChildNodes;
            QuesTypes = new List<QuesType>();
            for (int i = 0; i < list.Count; i++)
            {
                QuesType qt = new QuesType(list[i]);
                QuesTypes.Add(qt);
            }
        }
        /// <summary>
        /// 将属性进行Base64解码
        /// </summary>
        /// <param name="xn"></param>
        /// <returns></returns>
        private XmlNode _getAttrBase64(XmlNode xn)
        {
            foreach (XmlAttribute attr in xn.Attributes)
            {
                string val = WeiSha.Core.DataConvert.DecryptForBase64(attr.Value);
                val = val.Replace("<", "＜");
                val = val.Replace(">", "＞");
                val = val.Replace("(", "（");
                val = val.Replace(")", "）");
                val = val.Replace("&", "＆");
                val = val.Replace("=", "〓");
                val = val.Replace("\"", "＂");
                val = val.Replace("'", "｀");
                val = val.Replace("\\", "＼");
                attr.Value = val;
            }
            return xn;
        }
        #endregion

        #region 输出XML
        /// <summary>
        /// 输出答题成绩的XML文本
        /// </summary>
        /// <param name="isbase64">是否进行base64编码</param>
        /// <returns></returns>
        public string OutputXML(bool isbase64)
        {
            XmlDocument doc = (XmlDocument)Details.Clone();
            doc = _handleAttr(doc, isbase64);
            return this.Details.OuterXml;
        }
        /// <summary>
        /// 设置XML头部的属性
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="isbase64"></param>
        /// <returns></returns>
        private XmlDocument _handleAttr(XmlDocument doc, bool isbase64)
        {
            XmlNode xn = doc.SelectSingleNode("results");

            //考试id，考试主题的uid，考试主题的标题
            xn.Attributes["examid"].Value = this.Examid.ToString();
            xn.Attributes["uid"].Value = this.ExamUid;
            xn.Attributes["theme"].Value = this.ExamTheme;

            //时间
            xn.Attributes["begin"].Value = _handleTime(Begin).ToString();
            xn.Attributes["overtime"].Value = _handleTime(Overtime).ToString();
            xn.Attributes["starttime"].Value = _handleTime(Startime).ToString();

            //学员相关，学员id、名称，学员组，性别，身份证号
            xn.Attributes["stid"].Value = this.AccountID.ToString();
            xn.Attributes["stname"].Value = this.AccountName;
            xn.Attributes["stsid"].Value = this.SortID.ToString();
            xn.Attributes["stsex"].Value = this.Gender.ToString();
            xn.Attributes["stcardid"].Value = this.IDCardNumber;

            //试卷id,专业id,专业名称
            xn.Attributes["tpid"].Value = this.TestPaperID.ToString();
            xn.Attributes["sbjid"].Value = this.SubjectID.ToString();
            xn.Attributes["sbjname"].Value = this.SubjectName;

            //交卷方式与当前试题
            xn.Attributes["patter"].Value = this.Pattern.ToString();
            xn.Attributes["index"].Value = this.QuesIndex.ToString();

            if (isbase64) xn = _setAttrBase64(xn);
            return doc;
        }
        /// <summary>
        /// 将时间对象转为毫秒数（长整型）
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private long _handleTime(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            long timeStamp = (long)(time - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }
        /// <summary>
        /// 将属性进行Base64编码
        /// </summary>
        /// <param name="xn"></param>
        /// <returns></returns>
        private XmlNode _setAttrBase64(XmlNode xn)
        {
            foreach (XmlAttribute attr in xn.Attributes)
            {
                string val = attr.Value;
                val = val.Replace("＜", "<");
                val = val.Replace("＞", ">");
                val = val.Replace("（", "(");
                val = val.Replace("）", ")");
                val = val.Replace("＆", "&");
                val = val.Replace("〓", "=");
                val = val.Replace("＂", "\"");
                val = val.Replace("｀", "'");
                val = val.Replace("＼", "\\");
                attr.Value = WeiSha.Core.DataConvert.EncryptForBase64(val);
            }
            return xn;
        }
        #endregion

        #region 重新生成成绩（即作弊）
        /// <summary>
        /// 设置成绩，即希望以该得分重新生成成绩
        /// </summary>
        /// <param name="score">期望得分</param>
        /// <param name="highest">最高分</param>
        /// <param name="lowest">最低分</param>
        public void SetScore(float score, float highest, float lowest)
        {
            //一些数据校验
            if (highest <= 0) highest = 0;
            if (lowest <= 0) lowest = 0;
            if (highest < lowest) highest = lowest;     //最高分不能小于最低分
            if (score <= 0 || score > highest || score < lowest)
                score = highest == 0 && lowest == 0 ? 0 : (float)Math.Round((highest + lowest) / 2 * 100) / 100;
            //将试题答题的对象，转为一维队列，方便处理
            List<QuesAnswer> qlist = new List<QuesAnswer>();
            foreach (QuesType qtype in this.QuesTypes)
            { 
                foreach (QuesAnswer q in qtype.QuesAnswers)              
                    qlist.Add(q);              
            }
        }
        //public void RandomScore(List<QuesAnswer> qlist)
        //{
        //    float actualScore = 0;
        //    foreach (QuesAnswer q in qlist)
        //    {
        //        actualScore += q.Sucess ? q.Score : 0;
        //    }

        //}
        #endregion
    }
    #region 相关的类
    /// <summary>
    /// 按题型分类，其XML节点的示例 
    /// ques type="2" count="10" number="30"
    /// </summary>
    public class QuesType
    {       
        /// <summary>
        /// 题型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 题量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 分数
        /// </summary>
        public float Number { get; set; }
        /// <summary>
        /// 试题
        /// </summary>
        public List<QuesAnswer> QuesAnswers { get; set; }
        public QuesType(XmlNode node)
        {
            //试题分类的属性，得分，题型等
            if (node.Attributes != null)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {                    
                    Type type = this.GetType();
                    PropertyInfo[] properties = type.GetProperties();
                    for(int i = 0; i < properties.Length; i++)
                    {
                        PropertyInfo p=properties[i];
                        if (p.Name.Equals(attr.Name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            object obj = Convert.ChangeType(attr.Value, p.PropertyType);
                            p.SetValue(this, obj);
                            break;
                        }
                    }

                }
            }
            //试题的答题信息
            QuesAnswers = new List<QuesAnswer>();
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                QuesAnswer q = new QuesAnswer(node.ChildNodes[i], i + 1);               
                QuesAnswers.Add(q);
            }

        }
    }
    /// <summary>
    /// 试题的答题信息
    /// </summary>
    public class QuesAnswer
    {
        //<q id="129132486367645696" class="level1" num="1" ans="" file="" sucess="false" score="0"/>
        //<q id="129132493668093952" class="level1" num="5" file="" sucess="false" score="4"> 多子产生的原因是本征半导体中掺入三价或五价杂质元素 </q>

        //试题操作的实现类
        private static readonly QuestionsCom com = new Song.ServiceImpls.QuestionsCom();
        /// <summary>
        /// 试题ID
        /// </summary>
        public long ID { get; set; }


        private Song.Entities.Questions _entity = null;
        /// <summary>
        /// 试题的实体对象
        /// </summary>
        public Song.Entities.Questions Entity
        {
            get
            {
                if (this._entity == null) this._entity = com.QuesSingle(this.ID);
                return _entity;
            }
        }
        private List<Song.Entities.QuesAnswer> _answerItems = null;
        /// <summary>
        /// 试题的答题项
        /// </summary>
        public List<Song.Entities.QuesAnswer> AnswerItems
        {
            get
            {
                if (this._answerItems == null) this._answerItems = com.ItemsToAnswer(this.Entity, null);
                return _answerItems;
            }
        }
        /// <summary>
        /// 题型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 试题分数
        /// </summary>
        public float Num { get; set; }
        /// <summary>
        /// 答题内容
        /// </summary>
        public string Ans { get; set; }
        /// <summary>
        /// 试题文件
        /// </summary>
        public string File { get; set; }
        /// <summary>
        /// 是否答题正确
        /// </summary>
        public bool Sucess { get; set; }
        /// <summary>
        /// 答题得分
        /// </summary>
        public float Score { get; set; }
        /// <summary>
        /// 试题的索引号，并不会反映到答题信息中，仅作为临时值
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index">试题索引</param>
        public QuesAnswer(XmlNode node,int index)
        {
            this.Type = Convert.ToInt32(node.ParentNode.Attributes["type"].Value);
            this.ID = Convert.ToInt64(node.Attributes["id"].Value);
            this.Num = Convert.ToSingle(node.Attributes["num"].Value);
            if (this.Type != 4)
                this.Ans = node.Attributes["ans"] != null ? node.Attributes["ans"].Value : "";
            else
                this.Ans = node.InnerText;
            this.File = node.Attributes["file"] != null ? node.Attributes["file"].Value : "";
            this.Sucess = node.Attributes["sucess"] != null ? Convert.ToBoolean(node.Attributes["sucess"].Value) : false;
            this.Score = node.Attributes["score"] != null ? Convert.ToSingle(node.Attributes["score"].Value) : 0;

            this.Index = index;
        }
        /// <summary>
        /// 计算试题得分
        /// </summary>
        /// <returns></returns>
        public float CalcScore()
        {
            float score = com.CalcScore(this.Entity, this.Ans, this.Num);
            this.Score = score;
            this.Sucess = score == this.Num;
            return score;
        }
        /// <summary>
        /// 生成失败的答题
        /// </summary>
        public void ToWrong()
        {
            //单选题
            if (this.Type == 1)
            {
                List<Song.Entities.QuesAnswer> _answers = this.AnswerItems;
                //错误答案项的ID
                List<long> itemid = new List<long>();
                for (int i = 0; i < _answers.Count; i++)
                {
                    if (!_answers[i].Ans_IsCorrect)
                        itemid.Add(_answers[i].Ans_ID);
                }
                Random rd = new Random(_answers.Count * this.Index + (int)DateTime.Now.Ticks);
                int idx = rd.Next(100) % itemid.Count;
                //如果有一个答案项
                if (itemid.Count <= 1) this.Ans = string.Empty;
                else this.Ans = itemid[idx].ToString();
            }
            //多选题
            if (this.Type == 2)
            {
                List<Song.Entities.QuesAnswer> _answers = this.AnswerItems;
                Random rd = new Random(_answers.Count * this.Index + (int)DateTime.Now.Ticks);
                int anscount = rd.Next(2, _answers.Count);
                //错误答案项的ID
                List<long> wrongid = new List<long>();
                List<long> correctid = new List<long>();
                List<long> allid = new List<long>();
                for (int i = 0; i < _answers.Count; i++)
                {
                    if (!_answers[i].Ans_IsCorrect) wrongid.Add(_answers[i].Ans_ID);
                    else correctid.Add(_answers[i].Ans_ID);
                    allid.Add(_answers[i].Ans_ID);
                }
                List<long> ans = new List<long>();
                //如果所有答案项都是正确的
                if (correctid.Count == _answers.Count && anscount == _answers.Count)
                    --anscount;
                //如果有错误答案项,则随机选择一个错误答案项
                if (wrongid.Count > 1)
                {
                    ans.Add(wrongid[0]);
                    wrongid.RemoveAt(0);
                    allid.RemoveAt(allid.FindIndex(n => n == wrongid[0]));
                    --anscount;
                }
                //
                while (anscount > 0)
                {
                    int idx = rd.Next(100) % allid.Count;
                    ans.Add(allid[idx]);
                    allid.RemoveAt(idx);
                    anscount--;
                }
                this.Ans = string.Join(",", ans);
            }
            this.Score = 0;
            this.Sucess = false;
        }
        /// <summary>
        /// 生成正确的答题
        /// </summary>
        public void ToCorrect()
        {
            //Random random = new Random();
            //单选题
            if (this.Type == 1)
            {
                long ansid = 0;
                for (int i = 0; i < this.AnswerItems.Count; i++)
                {
                    if (this.AnswerItems[i].Ans_IsCorrect)
                        ansid = this.AnswerItems[i].Ans_ID;
                }
                if (ansid == 0) this.Ans = string.Empty;
                else this.Ans = ansid.ToString();               
            }
            //单选题
            if (this.Type == 2)
            {
                List<Song.Entities.QuesAnswer> _answers = this.AnswerItems;
                List<long> correctid = new List<long>();
                for (int i = 0; i < _answers.Count; i++)
                {
                    if (_answers[i].Ans_IsCorrect) correctid.Add(_answers[i].Ans_ID);                   
                }
                this.Ans = string.Join(",", correctid);
            }
            this.Score = this.Num;
            this.Sucess = true;
        }
    }
    #endregion
}
