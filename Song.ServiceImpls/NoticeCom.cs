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
    public class NoticeCom :INotice
    {
        #region INotice 成员

        public void Add(Notice entity)
        {
            entity.No_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<Notice>(entity);
        }

        public void Save(Notice entity)
        {
            Gateway.Default.Save<Notice>(entity);
        }

        public void Delete(Notice entity)
        {
            Gateway.Default.Delete<Notice>(entity);
        }

        public void Delete(int identify)
        {
            Gateway.Default.Delete<Notice>(Notice._.No_Id == identify);
        }

        public void Delete(string name)
        {
            Gateway.Default.Delete<Notice>(Notice._.No_Ttl == name);
        }

        public Notice NoticeSingle(int identify)
        {
            return Gateway.Default.From<Notice>().Where(Notice._.No_Id == identify).ToFirst<Notice>();
        }

        public Notice NoticeSingle(string ttl)
        {
            return Gateway.Default.From<Notice>().Where(Notice._.No_Ttl == ttl).ToFirst<Notice>();
        }

        /// <summary>
        /// 当前公告的上一条公告
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Notice NoticePrev(int identify, int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            wc &= Notice._.No_IsShow == true;
            wc &= Notice._.No_StartTime < DateTime.Now;
            Song.Entities.Notice no = this.NoticeSingle(identify);
            return Gateway.Default.From<Notice>().OrderBy(Notice._.No_StartTime.Asc)
                .Where(wc && Notice._.No_StartTime > no.No_StartTime).ToFirst<Notice>();
        }
        /// <summary>
        /// 当前公告的下一条公告
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Notice NoticeNext(int identify,int orgid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            wc &= Notice._.No_IsShow == true;
            wc &= Notice._.No_StartTime < DateTime.Now;
            Song.Entities.Notice no = this.NoticeSingle(identify);
            return Gateway.Default.From<Notice>().OrderBy(Notice._.No_StartTime.Desc)
                .Where(wc && Notice._.No_StartTime < no.No_StartTime).ToFirst<Notice>();
        }

        public Notice[] GetAll()
        {
            return Gateway.Default.From<Notice>().OrderBy(Notice._.No_StartTime.Desc).ToArray<Notice>();
        }
        public Notice[] GetAll(int orgid, bool? isShow)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            if (isShow != null) wc &= Notice._.No_IsShow == (bool)isShow;
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_IsTop.Asc && Notice._.No_StartTime.Desc).ToArray<Notice>();
        }

        public Notice[] GetCount(int orgid, bool? isShow, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Notice._.No_StartTime < DateTime.Now);
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            if (isShow != null) wc &= Notice._.No_IsShow == (bool)isShow;
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_IsTop.Asc && Notice._.No_StartTime.Desc).ToArray<Notice>(count);
        }
        /// <summary>
        /// 取具体的数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public int OfCount(int orgid, bool? isShow)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Notice._.Org_ID == orgid);
            if (isShow != null) wc.And(Notice._.No_IsShow == isShow);
            return Gateway.Default.Count<Notice>(wc);
        }

        public Notice[] GetPager(int orgid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            countSum = Gateway.Default.Count<Notice>(wc);
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_IsTop.Asc && Notice._.No_StartTime.Desc).ToArray<Notice>(size, (index - 1) * size);
        }

        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <param name="searTxt">查询字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public Notice[] GetPager(int orgid, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            if (isShow != null) wc &= Notice._.No_IsShow == (bool)isShow;
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(Notice._.No_Ttl.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Notice>(wc);
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_IsTop.Asc && Notice._.No_StartTime.Desc).ToArray<Notice>(size, (index - 1) * size);
        }
        #endregion
        
    }
}
