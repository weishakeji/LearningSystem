using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 短信管理
    /// </summary>
    public interface ISMS : WeiSha.Common.IBusinessInterface
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        void MessageAdd(SmsMessage entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void MessageSave(SmsMessage entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void MessageDelete(SmsMessage entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void MessageDelete(int identify);       
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        SmsMessage GetSingle(int identify);        
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="type">1为针对分类，2为针对个人，3为针对员工</param>
        /// <param name="box">1为草稿箱，2为已发送，3为垃圾箱</param>
        /// <param name="state">1为发送成功，2为发送失败，3为部分失败</param>
        /// <param name="search">按内容检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        SmsMessage[] MessagePager(int? type, int? box, int? state,string search, int size, int index, out int countSum);


        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="keyname">写入cookis的key值名称</param>
        /// <returns>是否发送成功</returns>
        bool SendVcode(string phone, string keyname);
        /// <summary>
        /// 格式化短信内容，将一些替换符转成实际内容
        /// </summary>
        /// <param name="msg">短信内容。其中包括的替代符：{vcode}验证码，{platform}平台名称，{org}机构简称,{date}时间。</param>
        /// <param name="rnd">随机字符</param>
        /// <returns></returns>
        string MessageFormat(string msg, string rnd);
    }
}
