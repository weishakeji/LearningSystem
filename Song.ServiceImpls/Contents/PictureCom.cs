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
        //图片信息的排序规则
        private OrderByClip piOrder = Picture._.Pic_IsCover.Asc && Picture._.Pic_Tax.Asc;

        public int PictureAdd(Picture entity)
        {
            //设置新对象的排序号
            object obj = Gateway.Default.Max<Picture>( Picture._.Pic_Tax,Picture._.Col_Id == entity.Col_Id);
            entity.Pic_Tax = obj is int ? (int)obj + 1 : 1;
            Song.Entities.Columns nc = Business.Do<IColumns>().Single((int)entity.Col_Id);
            if (nc != null) entity.Col_Name = nc.Col_Name;
            //所在机构
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Picture>(entity);
        }

        public void PictureSave(Picture entity)
        {            
            Song.Entities.Columns nc = Business.Do<IColumns>().Single((int)entity.Col_Id);
            if (nc != null) entity.Col_Name = nc.Col_Name;           
            Gateway.Default.Save<Picture>(entity);
        }

        public void PictureDelete(int identify)
        {
            Song.Entities.Picture entity = this.PictureSingle(identify);
            if (entity == null) return;
            PictureDelete(entity); 
        }
        public void PictureDelete(Picture entity)
        {
            //删除图片
            WeiSha.WebControl.FileUpload.Delete(entity.Pic_Type, entity.Pic_FilePath);
            //删除自身            
            Gateway.Default.Delete<Picture>(entity);
        }
        /// <summary>
        /// 删除所有图片
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="colid">栏目id</param>
        public void PictureDeleteAll(int orgid, int colid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Picture._.Org_ID == orgid);
            if (colid > 0) wc.And(Picture._.Col_Id == colid);
            Song.Entities.Picture[] entities = Gateway.Default.From<Picture>().Where(wc).ToArray<Picture>();
            foreach (Song.Entities.Picture entity in entities)
            {
                PictureDelete(entity);
            }
        }
        public void PictureIsDelete(int identify)
        {
            Gateway.Default.Update<Picture>(new Field[] { Picture._.Pic_IsDel }, new object[] { true }, Picture._.Pic_Id == identify);
        }

        public void PictureRecover(int identify)
        {
            Gateway.Default.Update<Picture>(new Field[] { Picture._.Pic_IsDel }, new object[] { false }, Picture._.Pic_Id == identify);
        }

        public Picture PictureSingle(int identify)
        {
            return Gateway.Default.From<Picture>().Where(Picture._.Pic_Id == identify).ToFirst<Picture>();
        }

        public void PictureSetCover(int colid, int pid)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            try
            {
                //去除当前相册的封面
                tran.Update<Picture>(new Field[] { Picture._.Pic_IsCover }, new object[] { false }, Picture._.Col_Id == colid);
                //设置当前图片封面
                tran.Update<Picture>(new Field[] { Picture._.Pic_IsCover }, new object[] { true }, Picture._.Pic_Id == pid);
                tran.Commit();
            }
            catch(Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                tran.Close();
            }  
        }

        public void PictureSetCover(string uid, int pid)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            try
            {
                //去除当前相册的封面
                tran.Update<Picture>(new Field[] { Picture._.Pic_IsCover }, new object[] { false }, Picture._.Pic_Uid == uid);
                //设置当前图片封面
                tran.Update<Picture>(new Field[] { Picture._.Pic_IsCover }, new object[] { true }, Picture._.Pic_Id == pid);
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

        public Picture[] PictureCount(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Picture._.Org_ID == orgid);
            if (colid != null) wc.And(Picture._.Col_Id == colid);
            if (isDel != null) wc.And(Picture._.Pic_IsDel == (bool)isDel);
            if (isShow != null) wc.And(Picture._.Pic_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Picture._.Pic_Name.Like("%" + searTxt + "%"));
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Picture>().Where(wc).OrderBy(piOrder).OrderBy(Picture._.Pic_CrtTime.Desc).ToArray<Picture>(count);
        }
        public Picture[] PictureCount(int orgid, string uid, bool? isDel, bool? isShow, string searTxt, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Picture._.Org_ID == orgid);
            wc.And(Picture._.Pic_Uid == uid);
            if (isDel != null) wc.And(Picture._.Pic_IsDel == (bool)isDel);
            if (isShow != null) wc.And(Picture._.Pic_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Picture._.Pic_Name.Like("%" + searTxt + "%"));
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<Picture>().Where(wc).OrderBy(piOrder).OrderBy(Picture._.Pic_CrtTime.Desc).ToArray<Picture>(count);
        }
        public Picture[] PicturePager(int orgid, int? colid, bool? isDel, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Picture._.Org_ID == orgid);
            if (colid != null) wc.And(Picture._.Col_Id == colid);
            if (isDel != null) wc.And(Picture._.Pic_IsDel == (bool)isDel);
             if (searTxt != null && searTxt.Trim() != "")
                wc.And(Picture._.Pic_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Picture>(wc);
            return Gateway.Default.From<Picture>().Where(wc).OrderBy(piOrder).ToArray<Picture>(size, (index - 1) * size);
        }

        public Picture[] PicturePager(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Picture._.Org_ID == orgid);
            if (colid != null) wc.And(Picture._.Col_Id == colid);
            if (isDel != null) wc.And(Picture._.Pic_IsDel == (bool)isDel);
            if (isShow != null) wc.And(Picture._.Pic_IsShow == (bool)isShow);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Picture._.Pic_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Picture>(wc);
            return Gateway.Default.From<Picture>().Where(wc).OrderBy(piOrder).ToArray<Picture>(size, (index - 1) * size);
        }
        public Picture[] PicturePager(int orgid, int? colid, bool? isDel, bool? isShow, string searTxt, bool? isHot, bool? isRec, bool? isTop, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Picture._.Org_ID == orgid);
            if (colid != null) wc.And(Picture._.Col_Id == colid);
            if (isDel != null) wc.And(Picture._.Pic_IsDel == (bool)isDel);
            if (isShow != null) wc.And(Picture._.Pic_IsShow == (bool)isShow);
            if (isHot != null) wc.And(Picture._.Pic_IsHot == (bool)isHot);
            if (isRec != null) wc.And(Picture._.Pic_IsRec == (bool)isRec);
            if (isTop != null) wc.And(Picture._.Pic_IsTop == (bool)isTop);
            if (searTxt != null && searTxt.Trim() != "")
                wc.And(Picture._.Pic_Name.Like("%" + searTxt + "%"));
            countSum = Gateway.Default.Count<Picture>(wc);
            return Gateway.Default.From<Picture>().Where(wc).OrderBy(piOrder).ToArray<Picture>(size, (index - 1) * size);
        }
    }
}
