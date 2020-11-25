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
    public class Message : ViewMethod, IViewAPI
    {
        ///// <summary>
        ///// 添加留言
        ///// </summary>
        ///// <returns></returns>
        //[Student]
        //public int Add(string msg, int playtime, int couid, int olid)
        //{
        //    if (string.IsNullOrWhiteSpace(msg)) return 0;
        //    if (msg.Trim() == "") return 0;
        //    Song.Entities.Accounts acc = Extend.LoginState.Accounts.CurrentUser;
        //    if (acc == null) return 0;
        //    return this.Add(acc.Ac_AccName, playtime, couid,  olid);
        //}
        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="acc">学员账号，如果账号为空则默认为当前登录账号</param>
        /// <param name="msg">留言信息</param>
        /// <param name="playtime">视频播放时间</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public int Add(string acc,string msg, int playtime, int couid, int olid)
        {
            Song.Entities.Accounts account = null;
            if (!string.IsNullOrWhiteSpace(acc))
            {
                account = Business.Do<IAccounts>().AccountsSingle(acc, -1);               
            }
            else
            {
                account = Extend.LoginState.Accounts.CurrentUser;
            }
            if (account == null) throw new Exception("当前账号不存在");
            Song.Entities.Message entity = new Entities.Message();
            entity.Msg_Context = msg.Length > 200 ? msg.Substring(0, 200) : msg;
            entity.Msg_PlayTime = playtime;
            if (couid <= 0)
            {
                Song.Entities.Outline outline = Business.Do<IOutline>().OutlineSingle(olid);
                if (outline != null) couid = outline.Cou_ID;
            }
            entity.Cou_ID = couid;
            entity.Ol_ID = olid;
            entity.Ac_ID = account.Ac_ID;
            entity.Ac_AccName = account.Ac_AccName;
            entity.Ac_Name = account.Ac_Name;
            entity.Msg_Phone = string.IsNullOrWhiteSpace(account.Ac_MobiTel1) ? account.Ac_MobiTel2 : account.Ac_MobiTel1;
            entity.Msg_QQ = account.Ac_Qq;
            entity.Ac_Photo = account.Ac_Photo;
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
        /// 获取指定数量的留言
        /// </summary>
        /// <param name="olid">章节id</param>
        /// <param name="order">排序方式，desc或asc</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Song.Entities.Message[] Count(int olid, string order,int count)
        {
            return Business.Do<IMessage>().GetCount(-1, olid, order, count);
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
                int row=Business.Do<IMessage>().Delete(msid);
                return row > 0;
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