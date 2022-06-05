using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using NBear.Common;
using Song.Entities;
using NBear.IoC.Service;
using NBear.Data;
using Song.ServiceInterfaces;
using System.Resources;
using System.Reflection;



namespace Song.ServiceImpls
{
    public partial class ProductCom : IProduct
    {

        #region 产品图片管理
        /// <summary>
        /// 添加产品图片
        /// </summary>
        /// <param name="entity"></param>
        public void PictureAdd(ProductPicture entity)
        {
            //创建时间
            entity.Pp_CrtTime = DateTime.Now;
            entity.Pp_IsUse = true;
            entity.Pp_IsCover = false;
            Song.Entities.Product pd = new ContentsCom().ProductSingle(entity.Pd_Uid);
            if (pd != null)
            {
                entity.Pd_Id = pd.Pd_Id;
                entity.Pd_Name = pd.Pd_Name;
            }
            object obj = Gateway.Default.Count<ProductPicture>(ProductPicture._.Pd_Uid == entity.Pd_Uid);
            //设置当前图片封面
            DbTransaction tran = Gateway.Default.BeginTransaction();
            try
            {
                if ((int)obj < 1)
                {
                    entity.Pp_IsCover = true;
                    pd.Pd_Logo = entity.Pp_File;
                    pd.Pd_LogoSmall = entity.Pp_FileSmall;
                    Gateway.Default.Save<Product>(pd, tran);
                }
                Gateway.Default.Save<ProductPicture>(entity,tran);
                tran.Commit();
            }
            catch(Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                Gateway.Default.CloseTransaction(tran);
            }  
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PictureSave(ProductPicture entity)
        {
            Song.Entities.Product pd = new ContentsCom().ProductSingle(entity.Pd_Uid);
            if (pd != null)
            {
                entity.Pd_Id = pd.Pd_Id;
                entity.Pd_Name = pd.Pd_Name;
            }
            Gateway.Default.Save<ProductPicture>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PictureDelete(ProductPicture entity)
        {            
            //如果不是封面图片，直接删除
            if (!entity.Pp_IsCover)
            {
                Gateway.Default.Delete<ProductPicture>(ProductPicture._.Pp_Id == entity.Pp_Id);
                Song.CustomControl.FileUpload.Delete("Product", entity.Pp_File);
            }
            else
            {
                Song.Entities.Product pd = new ContentsCom().ProductSingle(entity.Pd_Uid);
                //如果是封面图片，删除后需再指定一个封面
                DbTransaction tran = Gateway.Default.BeginTransaction();
                try
                {
                    Song.Entities.ProductPicture pp = Gateway.Default.From<ProductPicture>().Where(ProductPicture._.Pd_Id == entity.Pd_Id && ProductPicture._.Pp_Id != entity.Pp_Id).OrderBy(ProductPicture._.Pp_Tax.Asc).ToFirst<ProductPicture>();
                    if (pp != null)
                    {                        
                        pp.Pp_IsCover = true;
                        pd.Pd_Logo = pp.Pp_File;
                        pd.Pd_LogoSmall = pp.Pp_FileSmall;
                        Gateway.Default.Save<Product>(pd, tran);
                        Gateway.Default.Save<ProductPicture>(pp, tran);
                        Gateway.Default.Delete<ProductPicture>(ProductPicture._.Pp_Id == entity.Pp_Id, tran);
                        Song.CustomControl.FileUpload.Delete("Product", entity.Pp_File);
                        tran.Commit();
                    }
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw e;
                }
                finally
                {
                    Gateway.Default.CloseTransaction(tran);
                }  
            }
            
        }
        /// <summary>
        /// 删除图片文件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="tran">数据库操作事务</param>
        private void _PictureDelete(ProductPicture entity, DbTransaction tran)
        {
            Song.CustomControl.FileUpload.Delete("Product", entity.Pp_File);
            Gateway.Default.Delete<ProductPicture>(ProductPicture._.Pp_Id == entity.Pp_Id,tran);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void PictureDelete(int identify)
        {
            ProductPicture pp = this.PictureSingle(identify);
            this.PictureDelete(pp);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ProductPicture PictureSingle(int identify)
        {
            return Gateway.Default.From<ProductPicture>().Where(ProductPicture._.Pp_Id == identify).ToFirst<ProductPicture>();
        }
        /// <summary>
        /// 设置图片是否使用
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="isUse"></param>
        public void PictureSetUse(int identify, bool isUse)
        {
            Gateway.Default.Update<ProductPicture>(new PropertyItem[] { ProductPicture._.Pp_IsUse }, new object[] { isUse }, ProductPicture._.Pd_Id == identify);
        }

        public void PictureSetCover(string puid, int pid)
        {
           
            //设置当前图片封面
            ProductPicture curr = this.PictureSingle(pid);
            Product pro = new ContentsCom().ProductSingle(puid);
            DbTransaction tran = Gateway.Default.BeginTransaction();
            try
            {
                //去除当前相册的封面
                Gateway.Default.Update<ProductPicture>(new PropertyItem[] { ProductPicture._.Pp_IsCover }, new object[] { false }, ProductPicture._.Pd_Uid == puid, tran);
                Gateway.Default.Update<ProductPicture>(new PropertyItem[] { ProductPicture._.Pp_IsCover }, new object[] { true }, ProductPicture._.Pp_Id == pid, tran);
                pro.Pd_Logo = curr.Pp_File;
                pro.Pd_LogoSmall = curr.Pp_FileSmall;
                Gateway.Default.Save<Product>(pro, tran);
                tran.Commit();
            }
            catch (Exception e)
            {
                tran.Rollback();
                throw e;
            }
            finally
            {
                Gateway.Default.CloseTransaction(tran);
            }  
        }

        public ProductPicture[] PictureAll(string puid, bool? isUse)
        {
            WhereClip wc = ProductPicture._.Pd_Uid == puid;
            if (isUse != null) wc.And(ProductPicture._.Pp_IsUse == isUse);   
            return Gateway.Default.From<ProductPicture>().Where(wc).OrderBy(ProductPicture._.Pp_IsCover.Asc && ProductPicture._.Pp_Tax.Desc).ToArray<ProductPicture>();
        }
        #endregion

    }
}
