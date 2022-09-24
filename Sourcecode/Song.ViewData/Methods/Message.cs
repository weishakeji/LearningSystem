using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Data;



namespace Song.ViewData.Methods
{
    /// <summary>
    /// 课程学习中的咨询留言，可作为弹幕
    /// </summary>
    public class Message : ViewMethod, IViewAPI
    {
        /// <summary>
        /// 添加留言
        /// </summary>
        /// <param name="acc">学员账号，如果账号为空则默认为当前登录账号</param>
        /// <param name="msg">留言信息</param>
        /// <param name="playtime">视频播放时间</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public Song.Entities.Message Add(string acc,string msg, int playtime, long couid, int olid)
        {
            Song.Entities.Accounts account = null;
            if (!string.IsNullOrWhiteSpace(acc))
            {
                account = Business.Do<IAccounts>().AccountsSingle(acc, -1);               
            }
            else
            {
                account = LoginAccount.Status.User();
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
            entity.Msg_IP = WeiSha.Core.Browser.IP;

            Business.Do<IMessage>().Add(entity);
            return entity;
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
        public ListResult Pager(int orgid,int couid, int olid, string search, int size, int index)
        {
            int count = 0;
            Song.Entities.Message[] eas = null;
            eas = Business.Do<IMessage>().GetPager(orgid, couid, olid, -1, search, null, null, size, index, out count);
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
        [HttpDelete]
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
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpDelete]
        public int DeleteBatch(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IMessage>().Delete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
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
            if (mm == null) throw new Exception("Not found entity for Message！");
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
        /// <summary>
        /// 修改留言对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        public bool Modify(Song.Entities.Message entity)
        {
            Song.Entities.Message old = Business.Do<IMessage>().GetSingle(entity.Msg_Id);
            if (old == null) throw new Exception("Not found entity for Message！");

            old.Copy<Song.Entities.Message>(entity);

            Business.Do<IMessage>().Save(old);
            return true;
        }
    }
}