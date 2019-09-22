using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Common;
using System.Data;



namespace Song.ViewData.Methods
{
    /// <summary>
    /// 课程学习中的咨询留言，可作为弹幕
    /// </summary>
    public class Message : IViewAPI
    {       
        /// <summary>
        /// 添加留言
        /// </summary>
        /// <returns></returns>
        [Student]
        public int Add(string msg, int playtime, int couid, int olid)
        {
            if (string.IsNullOrWhiteSpace(msg)) return 0;
            if (msg.Trim() == "") return 0;
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            if (acc == null) return 0;

            Song.Entities.Message entity = new Entities.Message();
            entity.Msg_Context = msg.Length > 200 ? msg.Substring(0, 200) : msg;
            entity.Msg_PlayTime = playtime;
            entity.Cou_ID = couid;
            entity.Ol_ID = olid;
            entity.Ac_ID = acc.Ac_ID;
            entity.Ac_Name = acc.Ac_Name;
            entity.Msg_Phone = string.IsNullOrWhiteSpace(acc.Ac_MobiTel1) ? acc.Ac_MobiTel2 : acc.Ac_MobiTel1;
            entity.Msg_QQ = acc.Ac_Qq;
            entity.Ac_Photo = acc.Ac_Photo;

            return Business.Do<IMessage>().Add(entity);
        }
        /// <summary>
        /// 获取章节的所有留言
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public Song.Entities.Message[] All(int olid)
        {
            return Business.Do<IMessage>().GetAll(olid);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="msid"></param>
        [Student]
        public void Delete(int msid)
        {
            Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
            if (acc == null) return;
            Business.Do<IMessage>().Delete(msid, acc.Ac_ID);
        }
    }
}
