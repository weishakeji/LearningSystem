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
        public void MaterialAdd(ProductMaterial entity)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            Gateway.Default.Save<ProductMaterial>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void MaterialSave(ProductMaterial entity)
        {
            Gateway.Default.Save<ProductMaterial>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void MaterialDelete(int identify)
        {
            Gateway.Default.Delete<ProductMaterial>(ProductMaterial._.Pmat_Id == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ProductMaterial MaterialSingle(int identify)
        {
            return Gateway.Default.From<ProductMaterial>().Where(ProductMaterial._.Pmat_Id == identify).ToFirst<ProductMaterial>();
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public ProductMaterial[] MaterialAll(bool? isUse)
        {
            WhereClip wc = ProductMaterial._.Pmat_Id > 0;
            if (isUse != null)
            {
                wc.And(ProductMaterial._.Pmat_IsUse == isUse);
            }
            return Gateway.Default.From<ProductMaterial>().Where(wc).OrderBy(ProductMaterial._.Pmat_Id.Desc).ToArray<ProductMaterial>();
        }
        /// <summary>
        /// 分页获取材质信息
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public ProductMaterial[] MaterialPager(bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = ProductMaterial._.Pmat_Id > 0;
            if (isUse != null)
            {
                wc.And(ProductMaterial._.Pmat_IsUse == isUse);
            }
            if (searTxt != null && searTxt.Trim() != "")
            {
                wc.And(ProductMaterial._.Pmat_Name.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<ProductMaterial>(wc);
            return Gateway.Default.From<ProductMaterial>().Where(wc).OrderBy(ProductMaterial._.Pmat_Id.Desc).ToArray<ProductMaterial>(size, (index - 1) * size);
        }
    }
}
