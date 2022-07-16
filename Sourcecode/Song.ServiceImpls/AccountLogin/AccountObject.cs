using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Song.ServiceImpls.AccountLogin
{
    /// <summary>
    /// 缓存中的账户对象
    /// </summary>
    public class AccountObject
    {
        public  int Ac_ID { get; set; }
        public  String Ac_AccName { get; set; }
        public  String Ac_Pw { get; set; }
        public  String Ac_CheckUID { get; set; }
        public  String Ac_QqOpenID { get; set; }
        public  String Ac_WeixinOpenID { get; set; }
        public  DateTime Ac_OutTime { get; set; }

        public static AccountObject Create(Song.Entities.Accounts acc)
        {
            AccountObject obj = new AccountObject();
            obj.Ac_ID = acc.Ac_ID;
            obj.Ac_Pw = acc.Ac_Pw;
            obj.Ac_AccName = acc.Ac_AccName;

            obj.Ac_CheckUID = acc.Ac_CheckUID;
            obj.Ac_OutTime = acc.Ac_OutTime;

            obj.Ac_QqOpenID = acc.Ac_QqOpenID;
            obj.Ac_WeixinOpenID = acc.Ac_WeixinOpenID;

            return obj;
        }
    }
}
