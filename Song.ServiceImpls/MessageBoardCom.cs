using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class MessageBoardCom : IMessageBoard
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ThemeAdd(MessageBoard entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            //当前时间
            entity.Mb_CrtTime = DateTime.Now;
            entity.Mb_IP = WeiSha.Common.Request.IP.IPAddress;
            //uid
            if (string.IsNullOrWhiteSpace(entity.Mb_UID))
            {
                entity.Mb_UID = WeiSha.Common.Request.UniqueID();
            }

            Gateway.Default.Save<MessageBoard>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ThemeSave(MessageBoard entity)
        {
            Gateway.Default.Save<MessageBoard>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ThemeDelete(MessageBoard entity)
        {
            Gateway.Default.Delete<MessageBoard>(MessageBoard._.Mb_UID == entity.Mb_UID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ThemeDelete(int identify)
        {
            Song.Entities.MessageBoard tm = this.ThemeSingle(identify);
            Gateway.Default.Delete<MessageBoard>(MessageBoard._.Mb_UID == tm.Mb_UID);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public MessageBoard ThemeSingle(int identify)
        {
            return Gateway.Default.From<MessageBoard>().Where(MessageBoard._.Mb_IsTheme == true && MessageBoard._.Mb_Id == identify).ToFirst<MessageBoard>();
        }
        public MessageBoard[] ThemeCount(int orgid, int couid, string searTxt, int count)
        {
            WhereClip wc = MessageBoard._.Mb_IsTheme == true;
            if (orgid > 0) wc.And(MessageBoard._.Org_ID == orgid);
            if (couid > 0) wc.And(MessageBoard._.Cou_ID == couid);
            if (searTxt.Trim() != "") wc.And(MessageBoard._.Mb_Title.Like("%" + searTxt.Trim() + "%"));
            count = count < 1 ? int.MaxValue : count;
            return Gateway.Default.From<MessageBoard>().Where(wc).OrderBy(MessageBoard._.Mb_CrtTime.Desc).ToArray<MessageBoard>(count);
        }
        public MessageBoard[] ThemePager(int orgid, int couid, bool? isDel, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = MessageBoard._.Mb_IsTheme == true;
            if (orgid > 0) wc.And(MessageBoard._.Org_ID == orgid);
            if (couid >= 0) wc.And(MessageBoard._.Cou_ID == couid);
            if (isDel != null) wc.And(MessageBoard._.Mb_IsDel == isDel);
            if (isShow != null) wc.And(MessageBoard._.Mb_IsShow == isShow);
            if (searTxt.Trim() != "") wc.And(MessageBoard._.Mb_Title.Like("%" + searTxt.Trim() + "%"));
            countSum = Gateway.Default.Count<MessageBoard>(wc);
            return Gateway.Default.From<MessageBoard>().Where(wc).OrderBy(MessageBoard._.Mb_CrtTime.Desc).ToArray<MessageBoard>(size, (index - 1) * size);
        }
        public MessageBoard[] ThemePager(int orgid, int couid, bool? isDel, bool? isShow, bool? isAns, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = MessageBoard._.Mb_IsTheme == true;
            if (orgid > 0) wc.And(MessageBoard._.Org_ID == orgid);
            if (couid > 0) wc.And(MessageBoard._.Cou_ID == couid);
            if (isDel != null) wc.And(MessageBoard._.Mb_IsDel == isDel);
            if (isShow != null) wc.And(MessageBoard._.Mb_IsShow == isShow);
            if (isAns != null) wc.And(MessageBoard._.Mb_IsAns == isAns);
            if (searTxt.Trim() != "") wc.And(MessageBoard._.Mb_Title.Like("%" + searTxt.Trim() + "%"));
            countSum = Gateway.Default.Count<MessageBoard>(wc);
            return Gateway.Default.From<MessageBoard>().Where(wc).OrderBy(MessageBoard._.Mb_CrtTime.Desc).ToArray<MessageBoard>(size, (index - 1) * size);
        }
        public MessageBoard GetSingle(int identify)
        {
            return Gateway.Default.From<MessageBoard>().Where(MessageBoard._.Mb_Id == identify).ToFirst<MessageBoard>();
        }
        /// <summary>
        /// 添加回复留言信息
        /// </summary>
        /// <param name="entity"></param>
        public void AnswerAdd(MessageBoard entity)
        {
            //
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Song.Entities.MessageBoard theme = Gateway.Default.From<MessageBoard>().Where(MessageBoard._.Mb_IsTheme == true && MessageBoard._.Mb_UID == entity.Mb_UID).ToFirst<MessageBoard>();
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    theme.Mb_AnsTime = DateTime.Now;
                    theme.Mb_ReplyNumber += 1;
                    tran.Save<MessageBoard>(theme);
                    //
                    entity.Mb_IsTheme = false;
                    entity.Mb_CrtTime = DateTime.Now;
                    entity.Mb_IP = WeiSha.Common.Request.IP.IPAddress;
                    tran.Save<MessageBoard>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }

        }
        /// <summary>
        /// 修改回复信息
        /// </summary>
        /// <param name="entity"></param>
        public void AnswerSave(MessageBoard entity)
        {
            Gateway.Default.Save<MessageBoard>(entity);
        }
        /// <summary>
        /// 删除回复信息
        /// </summary>
        /// <param name="identify"></param>
        public void AnswerDelete(int identify)
        {
            MessageBoard mb = Gateway.Default.From<MessageBoard>().Where(MessageBoard._.Mb_Id == identify).ToFirst<MessageBoard>();
            if (mb.Mb_IsTheme)
            {
                this.ThemeDelete(mb);
            }
            else
            {
                Song.Entities.MessageBoard theme = Gateway.Default.From<MessageBoard>().Where(MessageBoard._.Mb_IsTheme == true && MessageBoard._.Mb_UID == mb.Mb_UID).ToFirst<MessageBoard>();
                using (DbTrans tran = Gateway.Default.BeginTrans())
                    try
                    {
                        theme.Mb_AnsTime = DateTime.Now;
                        theme.Mb_ReplyNumber -= 1;
                        tran.Save<MessageBoard>(theme);
                        //
                        tran.Delete<MessageBoard>(MessageBoard._.Mb_Id == identify);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        tran.Close();
                    }

            }
        }
        public MessageBoard AnswerSingle(int identify)
        {
            return Gateway.Default.From<MessageBoard>().Where(MessageBoard._.Mb_IsTheme == false && MessageBoard._.Mb_Id == identify).ToFirst<MessageBoard>();
        }
        public MessageBoard[] ListPager(string uid, int size, int index, out int countSum)
        {
            WhereClip wc = MessageBoard._.Mb_UID == uid;
            countSum = Gateway.Default.Count<MessageBoard>(wc);
            Song.Entities.MessageBoard[] msg = Gateway.Default.From<MessageBoard>().Where(wc).OrderBy(MessageBoard._.Mb_CrtTime.Asc).ToArray<MessageBoard>(size, (index - 1) * size);
            //把主题放到最前面
            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i].Mb_IsTheme == true)
                {
                    Song.Entities.MessageBoard tm = msg[0];
                    msg[0] = msg[i];
                    msg[i] = tm;
                    break;
                }
            }
            return msg;
        }
    }
}
