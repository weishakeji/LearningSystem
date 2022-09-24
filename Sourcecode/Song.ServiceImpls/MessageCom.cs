using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;



namespace Song.ServiceImpls
{
    public class MessageCom : IMessage
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int Add(Message entity)
        {
            entity.Msg_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)         
                entity.Org_Id = org.Org_ID; 
            return Gateway.Default.Save<Message>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void Save(Message entity)
        {
            Gateway.Default.Save<Message>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int Delete(Message entity)
        {
            return Gateway.Default.Delete<Message>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public int Delete(int identify)
        {
            return Gateway.Default.Delete<Message>(Message._.Msg_Id == identify);
        }
        /// <summary>
        /// 删除，按主键id和学员id
        /// </summary>
        /// <param name="identify">主键id</param>
        /// <param name="acid">学员账户id</param>
        public int Delete(int identify, int acid)
        {
            return  Gateway.Default.Delete<Message>(Message._.Msg_Id == identify && Message._.Ac_ID == acid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Message GetSingle(int identify)
        {
            return Gateway.Default.From<Message>().Where(Message._.Msg_Id == identify).ToFirst<Message>();
        }

        /// <summary>
        /// 获取对象；
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public Message[] GetAll(long couid, int olid)
        {
            return GetCount(couid, olid, "asc", 0);
        }
        public Message[] GetAll(int olid,string order)
        {
            return GetCount(0, olid, order, 0);
        }
        public Message[] GetCount(long couid, int olid, string order, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Message._.Msg_Del == false);     //未标识删除的
            if (couid > 0) wc &= Message._.Cou_ID == couid;
            if (olid > 0) wc &= Message._.Ol_ID == olid;

            OrderByClip ord = Message._.Msg_CrtTime.Asc;
            if ("desc".Equals(order, StringComparison.CurrentCultureIgnoreCase)) ord = Message._.Msg_CrtTime.Desc;
            if ("asc".Equals(order, StringComparison.CurrentCultureIgnoreCase)) ord = Message._.Msg_CrtTime.Asc;
            return Gateway.Default.From<Message>().Where(wc).OrderBy(ord).ToArray<Message>(count);
        }
        /// <summary>
        /// 获取留言数量
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        public int GetOfCount(long couid, int olid)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc &= Message._.Cou_ID == couid;
            if (olid > 0) wc &= Message._.Ol_ID == olid;
            //Message._.Msg_Title
            return Gateway.Default.From<Message>().Where(wc).Count();
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="stid"></param>
        /// <param name="sear"></param>
        /// <param name="startTime">创建时间，起始范围</param>
        /// <param name="endTime">创建时间，结束的范围</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Message[] GetPager(int orgid,long couid, int olid, int stid, string sear, DateTime? startTime, DateTime? endTime, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Message._.Org_Id == orgid;
            if (couid > 0) wc &= Message._.Cou_ID == couid;
            if (olid > 0) wc &= Message._.Ol_ID == olid;
            if (stid > 0) wc &= Message._.Ac_ID == stid;
            if (!string.IsNullOrWhiteSpace(sear))
                wc.And(Message._.Msg_Context.Like("%" + sear + "$"));
            if (startTime != null)
            {
                wc.And(Message._.Msg_CrtTime >= (DateTime)startTime);
            }
            if (endTime != null)
            {
                wc.And(Message._.Msg_CrtTime < (DateTime)endTime);
            }
            countSum = Gateway.Default.Count<Message>(wc);
            return Gateway.Default.From<Message>().Where(wc).OrderBy(Message._.Msg_CrtTime.Desc).ToArray<Message>(size, (index - 1) * size);
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="stid"></param>
        /// <param name="sear"></param>
        /// <param name="startPlay"></param>
        /// <param name="endPlay"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Message[] GetPager(long couid, int olid, int stid, string sear, int startPlay, int endPlay, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc &= Message._.Cou_ID == couid;
            if (olid > 0) wc &= Message._.Ol_ID == olid;
            if (stid > 0) wc &= Message._.Ac_ID == stid;
            if (!string.IsNullOrWhiteSpace(sear))
                wc.And(Message._.Msg_Context.Like("%" + sear + "$"));
            if (startPlay >= 0)
            {
                wc.And(Message._.Msg_PlayTime >= startPlay);
            }
            if (endPlay >= 0)
            {
                wc.And(Message._.Msg_PlayTime < endPlay);
            }
            countSum = Gateway.Default.Count<Message>(wc);
            return Gateway.Default.From<Message>().Where(wc).OrderBy(Message._.Msg_CrtTime.Desc).ToArray<Message>(size, (index - 1) * size);
        }
    }
}
