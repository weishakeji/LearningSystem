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
    public class NoticeCom :INotice
    {
       

        public long Add(Notice entity)
        {
            if (entity.No_Id <= 0)         
                entity.No_Id = WeiSha.Core.Request.SnowID();
           
            entity.No_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            if (entity.No_StartTime != null)            
                entity.No_StartTime = ((DateTime)entity.No_StartTime).Date;
            if (entity.No_EndTime != null)
            {
                DateTime end = ((DateTime)entity.No_EndTime).AddDays(1);
                entity.No_EndTime = end.Date.AddSeconds(-1);
            }
            Gateway.Default.Save<Notice>(entity);
            return entity.No_Id;
        }

        public void Save(Notice entity)
        {
            if (entity.No_StartTime != null)
                entity.No_StartTime = ((DateTime)entity.No_StartTime).Date;
            if (entity.No_EndTime != null)
            {
                DateTime end = ((DateTime)entity.No_EndTime).AddDays(1);
                entity.No_EndTime = end.Date.AddSeconds(-1);
            }
            Gateway.Default.Save<Notice>(entity);
        }

        public void Delete(Notice entity)
        {          
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<Notice>(Notice._.No_Id == entity.No_Id);
                    WeiSha.Core.Upload.Get["Notice"].DeleteDirectory(entity.No_Id.ToString());
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

        public void Delete(long identify)
        {
            Notice notice = this.NoticeSingle(identify);
            this.Delete(notice);
        }

        public Notice NoticeSingle(long identify)
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
        public Notice NoticePrev(long identify, int orgid)
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
        public Notice NoticeNext(long identify,int orgid)
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
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_IsTop.Asc && Notice._.No_StartTime.Desc && Notice._.No_Id.Desc).ToArray<Notice>();
        }

        public Notice[] GetCount(int orgid, int type, bool? isShow, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Notice._.No_StartTime < DateTime.Now);
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            if (type > 0) wc &= Notice._.No_Type == type;
            if (isShow != null) wc &= Notice._.No_IsShow == (bool)isShow;
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_IsTop.Asc && Notice._.No_StartTime.Desc && Notice._.No_Id.Desc).ToArray<Notice>(count);
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
        /// <summary>
        /// 增加浏览数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="num">要增加的数量</param>
        /// <returns></returns>
        public int ViewNum(long id, int num)
        {
            Song.Entities.Notice notice =this.NoticeSingle(id);
            if (notice == null) return -1;
            if (num <= 0) return notice.No_ViewNum;

            notice.No_ViewNum += num;
            Gateway.Default.Update<Notice>(
                new Field[] { Notice._.No_ViewNum },
                new object[] { notice.No_ViewNum }, Notice._.No_Id == notice.No_Id);
            return notice.No_ViewNum;
        }
        public Notice[] GetPager(int orgid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            countSum = Gateway.Default.Count<Notice>(wc);
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_StartTime.Desc && Notice._.No_Id.Desc).ToArray<Notice>(size, (index - 1) * size);
        }

        /// <summary>
        /// 分页获取所有的公告；
        /// </summary>
        /// <param name="orgid"></param>
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
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_StartTime.Desc && Notice._.No_Id.Desc).ToArray<Notice>(size, (index - 1) * size);
        }
        /// <summary>
        /// 获取通知公告
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="type">1为普通通知，2为弹窗通知</param>
        /// <param name="forpage">弹窗所在页</param>    
        /// <param name="time">当前时间</param>
        /// <param name="isShow">是否显示</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Notice[] List(int orgid, int type, string forpage, DateTime? time, bool? isShow, int count)
        {
            WhereClip wc = new WhereClip();
            wc &= Notice._.No_Type == type;
            if (orgid > 0) wc &= Notice._.Org_ID == orgid;
            if (isShow != null) wc &= Notice._.No_IsShow == (bool)isShow;
            if (!string.IsNullOrWhiteSpace(forpage)) wc &= Notice._.No_Page == forpage;
            if (time != null)
            {
                DateTime date = ((DateTime)time).Date;
                wc &= Notice._.No_StartTime <= date && Notice._.No_EndTime >= date;
            }
            return Gateway.Default.From<Notice>().Where(wc).OrderBy(Notice._.No_IsTop.Asc && Notice._.No_StartTime.Desc && Notice._.No_Id.Desc).ToArray<Notice>(count);
        }


    }
}
