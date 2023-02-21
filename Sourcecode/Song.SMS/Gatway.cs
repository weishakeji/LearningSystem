using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection; 

namespace Song.SMS
{
    public class Gatway
    {
        /// <summary>
        /// 发送短信的入口，实现IOC依赖反转
        /// </summary>
        /// <returns></returns>
        public static Song.SMS.ISMS Service
        {
            get
            {
                //webconfig中配置的短信接口类
                string type = Config.Current.Type;
                //类名
                string className = type.Substring(0, type.IndexOf(","));
                //库名
                string ddlName = type.Substring(type.IndexOf(",") + 1);
                //创建反射
                Type info = System.Type.GetType(className);
                if (info == null) return null;
                //实例化标签对象
                object obj = System.Activator.CreateInstance(info);
                Song.SMS.ISMS sms = (Song.SMS.ISMS)obj;
                sms.Current = Config.Current;
                return sms;
            }
        }
        /// <summary>
        /// 设置当前使用的短信平台
        /// </summary>
        /// <param name="name">短信平台名称，来自于web.config中的配置</param>
        public static void GetCurrentPlate(string name)
        {
            Config.Singleton.CurrentName = name;
        }
        /// <summary>
        /// 根据短信平台的名称，获取短信平台接口实例
        /// </summary>
        /// <param name="remark">短信接口备注名，来自于webconfig中的设置项</param>
        /// <returns></returns>
        public static Song.SMS.ISMS GetService(string remark)
        {
            SmsItem current = Config.Current;
            foreach (SmsItem item in Config.SmsItems)
            {
                if (item.Remarks == remark)
                {
                    current = item;
                    break;
                }
            }
            string type = current.Type;
            //类名
            string className = type.Substring(0, type.IndexOf(","));
            //库名
            string ddlName = type.Substring(type.IndexOf(",") + 1);
            //创建反射
            Type info = System.Type.GetType(className);
            if (info == null) return null;
            //实例化标签对象
            object obj = System.Activator.CreateInstance(info);
            Song.SMS.ISMS sms = (Song.SMS.ISMS)obj;
            sms.Current = current;
            return sms;           
        }
    }
}
