using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Song.Extend
{
    public class ExamResultsList
    {
    }
    public class ResultObject
    {
        Song.Entities.ExamResults _result;
        /// <summary>
        /// 记录成绩的对象
        /// </summary>
        public Song.Entities.ExamResults Result
        {
            get { return _result; }
        }
        private bool _isStored = false;
        /// <summary>
        /// 标记成绩记录是否已经存储到数据库
        /// </summary>
        public bool IsStored
        {
            get { return _isStored; }
            set { _isStored = value; }
        }
        private bool _isCalculating = false;
        /// <summary>
        /// 是否正在计算
        /// </summary>
        public bool IsCalculating
        {
            get { return _isCalculating; }
            set { _isCalculating = value; }
        }
        private bool _isCalculated = false;
        /// <summary>
        /// 是否已经完成计算
        /// </summary>
        public bool IsCalculated
        {
            get { return _isCalculated; }
            set { _isCalculated = value; }
        }
        
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="result"></param>
        public ResultObject(Song.Entities.ExamResults result)
        {
            _result = result;
        }
    }
}
