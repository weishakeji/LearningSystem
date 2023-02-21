using System;
using System.Collections.Generic;
using System.Text;

namespace Song.SMS
{
    public interface ISMS
    {
        /// <summary>
        /// 短信平台的管理项
        /// </summary>
        SmsItem Current { get; set; }
        /// <summary>
        /// 用户的账号
        /// </summary>
        string User { get; set; }
        /// <summary>
        /// 用户的密码
        /// </summary>
        string Password { get; set; }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        SmsState Send(string mobiles, string context);
        /// <summary>
        /// 定时发送短信
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="context"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        SmsState Send(string mobiles, string context, DateTime time);
        /// <summary>
        /// 查询剩余的短信条数
        /// </summary>
        /// <returns></returns>
        int Query();
        /// <summary>
        /// 查询剩余的短信条数
        /// </summary>
        /// <param name="user">账号</param>
        /// <param name="pw">密码</param>
        /// <returns></returns>
        int Query(string user, string pw);
        /// <summary>
        /// 接收回发的短信
        /// </summary>
        /// <param name="from">开始接收的时间</param>
        /// <param name="readflag">是否已读，0:未读短信，1:所有短信</param>
        /// <returns></returns>
        string ReceiveSMS(DateTime from, string readflag);
    }
}
