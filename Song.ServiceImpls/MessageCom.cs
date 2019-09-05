using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


using WeiSha.Common;
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
            {
                entity.Org_Id = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
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
        public void Delete(Message entity)
        {
            Gateway.Default.Delete<Message>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void Delete(int identify)
        {
            Gateway.Default.Delete<Message>(Message._.Msg_Id == identify);
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
        public Message[] GetAll(int couid, int olid)
        {
            return GetCount(couid, olid, 0);
        }
        public Message[] GetCount(int couid, int olid, int count)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc &= Message._.Cou_ID == couid;
            if (olid > 0) wc &= Message._.Ol_ID == olid;
            return Gateway.Default.From<Message>().Where(wc).OrderBy(Message._.Msg_CrtTime.Desc).ToArray<Message>(count);
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
        public Message[] GetPager(int couid, int olid, int stid, string sear, DateTime? startTime, DateTime? endTime, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
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
        public Message[] GetPager(int couid, int olid, int stid, string sear, int startPlay, int endPlay, int size, int index, out int countSum)
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
