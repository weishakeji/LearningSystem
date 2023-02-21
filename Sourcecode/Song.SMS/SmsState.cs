using System;
using System.Collections.Generic;
using System.Text;

namespace Song.SMS
{
    /// <summary>
    /// 短信的发送状态
    /// </summary>
    public class SmsState
    {
        private bool _success;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }
        private string _result = "";
        /// <summary>
        /// 发送短信的返回结果
        /// </summary>
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }
        private int _code = -1;
        /// <summary>
        /// 发送后的返回代码，一般0为发送成功
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
        private string _description = "";
        /// <summary>
        /// 发送短信的详细返回信息
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _failList = "";
        /// <summary>
        /// 如果发送失败，失败列表
        /// </summary>
        public string FailList
        {
            get { return _failList; }
            set { _failList = value; }
        }
    }
}
