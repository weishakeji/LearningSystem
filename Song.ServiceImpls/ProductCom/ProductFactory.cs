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
        public void FactoryAdd(ProductFactory entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<ProductFactory>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void FactorySave(ProductFactory entity)
        {
            Gateway.Default.Save<ProductFactory>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void FactoryDelete(int identify)
        {
            Gateway.Default.Delete<ProductFactory>(ProductFactory._.Pfact_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ProductFactory FactorySingle(int identify)
        {
            return Gateway.Default.From<ProductFactory>().Where(ProductFactory._.Pfact_Id == identify).ToFirst<ProductFactory>();
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public  ProductFactory[] FactoryAll(bool? isUse)
        {
            WhereClip wc = ProductFactory._.Pfact_Id > 0;
            if (isUse != null)
            {
                wc.And(ProductFactory._.Pfact_IsUse == isUse);
            }
            return Gateway.Default.From<ProductFactory>().Where(wc).OrderBy(ProductFactory._.Pfact_Id.Desc).ToArray<ProductFactory>();
        }
        /// <summary>
        /// 分页获取厂家信息
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public ProductFactory[] FactoryPager(bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = ProductFactory._.Pfact_Id > 0;
            if (isUse != null)
            {
                wc.And(ProductFactory._.Pfact_IsUse == isUse);
            }
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(ProductFactory._.Pfact_Name.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<ProductFactory>(wc);
            return Gateway.Default.From<ProductFactory>().Where(wc).OrderBy(ProductFactory._.Pfact_Id.Desc).ToArray<ProductFactory>(size, (index - 1) * size);
        }
    }
}
