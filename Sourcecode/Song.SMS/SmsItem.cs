using System;
using System.Collections.Generic;
using System.Text;

namespace Song.SMS
{
    public class SmsItem
    {
        private string user;
        /// <summary>
        /// 短信发送帐号
        /// </summary>
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        private string pw;
        /// <summary>
        /// 短信发送的密码
        /// </summary>
        public string Password
        {
            get { return pw; }
            set { pw = value; }
        }
        private string type;
        /// <summary>
        /// 短信接口的实现类
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private string name;
        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string remarks;
        /// <summary>
        /// 标识信息
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        private string _domain;
        /// <summary>
        /// 接口的请求域，包括端口
        /// </summary>
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }
        private string _regurl;
        /// <summary>
        /// 注册的地址
        /// </summary>
        public string RegisterUrl
        {
            get { return _regurl; }
            set { _regurl = value; }
        }
        private string _payurl;
        /// <summary>
        /// 充值的地址
        /// </summary>
        public string PayUrl
        {
            get { return _payurl; }
            set { _payurl = value; }
        }
        private bool _isUse;
        /// <summary>
        /// 是否启用该接口
        /// </summary>
        public bool IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        private bool isCurrent = false;
        /// <summary>
        /// 是否当前采用的短信平台
        /// </summary>
        public bool IsCurrent
        {
            get { return isCurrent; }
            set { isCurrent = value; }
        }
    }
}
