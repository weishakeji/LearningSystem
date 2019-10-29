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
            entity.Ac_AccName = acc.Ac_AccName;
            entity.Ac_Name = acc.Ac_Name;
            entity.Msg_Phone = string.IsNullOrWhiteSpace(acc.Ac_MobiTel1) ? acc.Ac_MobiTel2 : acc.Ac_MobiTel1;
            entity.Msg_QQ = acc.Ac_Qq;
            entity.Ac_Photo = acc.Ac_Photo;
            entity.Msg_IP = WeiSha.Common.Browser.IP;

            return Business.Do<IMessage>().Add(entity);
        }
        /// <summary>
        /// 获取章节的所有留言
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <param name="order">排序方式，desc或asc</param>
        /// <returns></returns>
        public Song.Entities.Message[] All(int olid, string order)
        {
            return Business.Do<IMessage>().GetAll(olid, order);
        }
        /// <summary>
        /// 获取留言数量
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public int Count(int couid, int olid)
        {
            return Business.Do<IMessage>().GetOfCount(couid, olid);
        }
        /// <summary>
        /// 分页获取留言信息
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="search">检索</param>
        /// <param name="size">当页多条记录</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult Pager(int couid, int olid, string search, int size, int index)
        {
            int count = 0;
            Song.Entities.Message[] eas = null;
            eas = Business.Do<IMessage>().GetPager(couid, olid, -1, search, null, null, size, index, out count);
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="msid">留言id</param>
        [Student, Admin, Teacher]
        public bool Delete(int msid)
        {

            try
            {
                Business.Do<IMessage>().Delete(msid);
                return true;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
        /// <summary>
        /// 更新留言内容
        /// </summary>
        /// <param name="msid">留言id</param>
        /// <param name="msg">留言内容</param>
        /// <returns></returns>
        public bool Update(int msid,string msg)
        {
            Song.Entities.Message mm = Business.Do<IMessage>().GetSingle(msid);
            if (mm == null) throw new Exception("当前信息不存在");
            mm.Msg_Context = msg;           
            try
            {
                Business.Do<IMessage>().Save(mm);
                return true;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}