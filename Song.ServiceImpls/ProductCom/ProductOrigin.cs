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
    public partial class ProductCom : IProduct
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        public void OriginAdd(ProductOrigin entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<ProductOrigin>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void OriginSave(ProductOrigin entity)
        {
            Gateway.Default.Save<ProductOrigin>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void OriginDelete(int identify)
        {
            Gateway.Default.Delete<ProductOrigin>(ProductOrigin._.Pori_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ProductOrigin OriginSingle(int identify)
        {
            return Gateway.Default.From<ProductOrigin>().Where(ProductOrigin._.Pori_Id == identify).ToFirst<ProductOrigin>();
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public ProductOrigin[] OriginAll(bool? isUse)
        {
            WhereClip wc = ProductOrigin._.Pori_Id > 0;
            if (isUse != null)
            {
                wc.And(ProductOrigin._.Pori_IsUse == isUse);
            }
            return Gateway.Default.From<ProductOrigin>().Where(wc).OrderBy(ProductOrigin._.Pori_Id.Desc).ToArray<ProductOrigin>();
        }
        /// <summary>
        /// 分页获取产地信息
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public ProductOrigin[] OriginPager(bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = ProductOrigin._.Pori_Id > 0;
            if (isUse != null)
            {
                wc.And(ProductOrigin._.Pori_IsUse == isUse);
            }
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(ProductOrigin._.Pori_Name.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<ProductOrigin>(wc);
            return Gateway.Default.From<ProductOrigin>().Where(wc).OrderBy(ProductOrigin._.Pori_Id.Desc).ToArray<ProductOrigin>(size, (index - 1) * size);
        }
    }
}
