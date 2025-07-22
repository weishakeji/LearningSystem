using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Song.Entities;

namespace Song.ServiceImpls.Exam
{
    /// <summary>
    /// 考试结果
    /// </summary>
    public class Results
    {
        #region 考试结果数据对象
        /// <summary>
        /// 考试答题信息的XML对象
        /// </summary>
        public XmlDocument Details { get; set; }
        /// <summary>
        /// 考试结果数据实体对象
        /// </summary>
        public ExamResults Entity { get; set; }
        #endregion

        #region 构造函数
        public Results(ExamResults entity)
        {
            Entity = entity;
        }
        public Results(string details)
        {
            Details = new XmlDocument();
            Details.LoadXml(details);
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
        #endregion
    }
    /// <summary>
    /// 按题型分类
    /// </summary>
    public class QuesType
    {
        //<ques type="2" count="10" number="30">
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
    }
    /// <summary>
    /// 试题的答题信息
    /// </summary>
    public class Question 
    {
        //<q id="129132486367645696" class="level1" num="1" ans="" file="" sucess="false" score="0"/>
        //<q id="129132493668093952" class="level1" num="5" file="" sucess="false" score="4"> 多子产生的原因是本征半导体中掺入三价或五价杂质元素 </q>
        /// <summary>
        /// 试题ID
        /// </summary>
        public long ID { get; set; }
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
    }
}
