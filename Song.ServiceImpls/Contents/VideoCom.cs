using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Resources;
using System.Reflection;

namespace Song.ServiceImpls
{
    public partial class ContentsCom : IContents
    {
        //视频信息的排序规则
        private OrderByClip viOrder = Video._.Vi_IsCover.Asc && Video._.Vi_Tax.Desc;
        public int VideoAdd(Video entity)
        {
            //设置新对象的排序号
            object obj = Gateway.Default.Max<Video>(Video._.Vi_Tax, Video._.Vi_Tax > -1);
            entity.Vi_Tax = obj is int ? (int)obj + 1 : 1;
             Song.Entities.Columns nc = Business.Do<IColumns>().Single((int)entity.Col_Id);
            if (nc != null) entity.Col_Name = nc.Col_Name;
            //所在机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Video>(entity);
        }

        public void VideoSave(Video entity)
        {
            Song.Entities.Columns nc = Business.Do<IColumns>().Single((int)entity.Col_Id);
            if (nc != null) entity.Col_Name = nc.Col_Name;
            Gateway.Default.Save<Video>(entity);
        }

        public void VideoDelete(int identify)
        {
            Video entity = this.VideoSingle(identify);
            if (entity == null) return;
            VideoDelete(entity);               
        }
        public void VideoDelete(Video entity)
        {           
            //删除视频
            WeiSha.WebControl.FileUpload.Delete("Video", entity.Vi_FilePath);
            //删除自身            
            Gateway.Default.Delete<Video>(entity);
        }
        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目分类id</param>
        public void VideoDeleteAll(int orgid, int colid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Video._.Org_ID == orgid);
            if (colid > 0) wc.And(Video._.Col_Id == colid);
            Song.Entities.Video[] entities = Gateway.Default.From<Video>().Where(wc).ToArray<Video>();
            foreach (Song.Entities.Video entity in entities)
            {
                VideoDelete(entity);
            }
        }

        public void VideoIsDelete(int identify)
        {
            Gateway.Default.Update<Video>(new Field[] { Video._.Vi_IsDel }, new object[] { true }, Video._.Vi_Id == identify);
        }

        public void VideoRecover(int identify)
        {
            Gateway.Default.Update<Video>(new Field[] { Video._.Vi_IsDel }, new object[] { false }, Video._.Vi_Id == identify);
        }

        public Video VideoSingle(int identify)
        {
            return Gateway.Default.From<Video>().Where(Video._.Vi_Id == identify).ToFirst<Video>();
        }

        public void VideoSetCover(int colid, int vid)
        {
           
            //设置当前视频封面
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //去除当前相册的封面
                    tran.Update<Video>(new Field[] { Video._.Vi_IsCover }, new object[] { false }, Video._.Col_Id == colid);
                    tran.Update<Video>(new Field[] { Video._.Vi_IsCover }, new object[] { true }, Video._.Col_Id == vid);
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

        public Video[] VideoCount(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int count)
        {

            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Video._.Org_ID == orgid);
            if (colid != null && colid > 0) wc.And(Video._.Col_Id == colid);
            if (isDel != null) wc.And(Video._.Vi_IsDel == isDel);
            if (isShow != null) wc.And(Video._.Vi_IsShow == isShow);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Video._.Vi_Name.Like("%" + searTxt + "%"));
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Video>().Where(wc).OrderBy(viOrder && Video._.Vi_CrtTime.Desc).ToArray<Video>(count);
        }

        public Video[] VideoPager(int orgid, int? colid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Video._.Org_ID == orgid);
            if (colid != null && colid > 0) wc.And(Video._.Col_Id == colid);
            countSum = Gateway.Default.Count<Video>(wc);
            return Gateway.Default.From<Video>().Where(wc).OrderBy(viOrder).ToArray<Video>(size, (index - 1) * size);
        }

        public Video[] VideoPager(int orgid, int? colid, bool? isDel, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Video._.Org_ID == orgid);
            if (colid != null && colid > 0) wc.And(Video._.Col_Id == colid);
            if (isDel != null) wc.And(Video._.Vi_IsDel == isDel);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Video._.Vi_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Video>(wc);
            return Gateway.Default.From<Video>().Where(wc).OrderBy(viOrder).ToArray<Video>(size, (index - 1) * size);
        }

        public Video[] VideoPager(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Video._.Org_ID == orgid);
            if (colid != null && colid > 0) wc.And(Video._.Col_Id == colid);
            if (isDel != null) wc.And(Video._.Vi_IsDel == isDel);
            if (isShow != null) wc.And(Video._.Vi_IsShow == isShow);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Video._.Vi_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Video>(wc);
            return Gateway.Default.From<Video>().Where(wc).OrderBy(viOrder).ToArray<Video>(size, (index - 1) * size);
        }
        public Video[] VideoPager(int orgid, int? colid, bool? isDel, bool? isShow, bool? isHot, bool? isRec, bool? isTop, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Video._.Org_ID == orgid);
            if (colid != null && colid > 0) wc.And(Video._.Col_Id == colid);
            if (isDel != null) wc.And(Video._.Vi_IsDel == isDel);
            if (isShow != null) wc.And(Video._.Vi_IsShow == (bool)isShow);
            if (isHot != null) wc.And(Video._.Vi_IsHot == (bool)isHot);
            if (isRec != null) wc.And(Video._.Vi_IsRec == (bool)isRec);
            if (isTop != null) wc.And(Video._.Vi_IsTop == (bool)isTop);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Video._.Vi_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Video>(wc);
            return Gateway.Default.From<Video>().Where(wc).OrderBy(viOrder).ToArray<Video>(size, (index - 1) * size);
        }
    }
}
