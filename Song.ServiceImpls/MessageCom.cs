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
        /// <returns></returns>
        public Message[] GetAll()
        {
            return Gateway.Default.From<Message>().OrderBy(Message._.Msg_CrtTime.Desc).ToArray<Message>();
        }
        public Message[] GetPager(int orgid, int stid, string sear, DateTime startTime, DateTime endTime, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            countSum = Gateway.Default.Count<Message>(wc);
            return Gateway.Default.From<Message>().Where(wc).OrderBy(Message._.Msg_CrtTime.Desc).ToArray<Message>(size, (index - 1) * size);
        }
    }
}
