using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;
using System.Data;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 产品的管理，以及产品相关的管理
    /// </summary>
    public interface IProduct : WeiSha.Common.IBusinessInterface
    {

        #region 产品咨询留言的管理
        /// <summary>
        /// 添加产品咨询留言
        /// </summary>
        /// <param name="entity">业务实体</param>
        int MessageAdd(ProductMessage entity);
        /// <summary>
        /// 修改产品咨询留言
        /// </summary>
        /// <param name="entity">业务实体</param>
        void MessageSave(ProductMessage entity);
        /// <summary>
        /// 删除产品咨询留言
        /// </summary>
        /// <param name="entity">业务实体</param>
        void MessageDelete(ProductMessage entity);
        /// <summary>
        /// 彻底删除产品咨询留言，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void MessageDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ProductMessage MessageSingle(int identify);
        /// <summary>
        /// 获取当前咨询留言关联的产品信息
        /// </summary>
        /// <param name="pmid"></param>
        /// <returns></returns>
        Product ProductByMessage(int pmid);
        /// <summary>
        /// 获取留言分页信息
        /// </summary>
        /// <param name="pdid">产品id</param>
        /// <param name="searTxt"></param>
        /// <param name="isAns">是否回复</param>
        /// <param name="isShow">是否允许显示</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        ProductMessage[] GetProductMessagePager(int? pdid, string searTxt, bool? isAns,bool? isShow, int size, int index, out int countSum);
        ProductMessage[] GetProductMessagePager(string searTxt, bool? isAns, bool? isShow, int size, int index, out int countSum);
        #endregion
        

        #region 产品厂家
        /// <summary>
        /// 新增产品厂家
        /// </summary>
        /// <param name="entity"></param>
        void FactoryAdd(ProductFactory entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void FactorySave(ProductFactory entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void FactoryDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ProductFactory FactorySingle(int identify);
        /// <summary>
        /// 获取所有产品厂家
        /// </summary>
        /// <param name="isUse"></param>
        /// <returns></returns>
        ProductFactory[] FactoryAll(bool? isUse);
        /// <summary>
        /// 分页获取厂家信息
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        ProductFactory[] FactoryPager(bool? isUse, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 产品材质
        /// <summary>
        /// 新增产品材质
        /// </summary>
        /// <param name="entity"></param>
        void MaterialAdd(ProductMaterial entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void MaterialSave(ProductMaterial entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void MaterialDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ProductMaterial MaterialSingle(int identify);
        /// <summary>
        /// 获取所有产品材质
        /// </summary>
        /// <param name="isUse"></param>
        /// <returns></returns>
        ProductMaterial[] MaterialAll(bool? isUse);
        /// <summary>
        /// 分页获取材质信息
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        ProductMaterial[] MaterialPager(bool? isUse, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 产品产地
        /// <summary>
        /// 新增产品产地
        /// </summary>
        /// <param name="entity"></param>
        void OriginAdd(ProductOrigin entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void OriginSave(ProductOrigin entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void OriginDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ProductOrigin OriginSingle(int identify);
        /// <summary>
        /// 获取所有产品产地
        /// </summary>
        /// <param name="isUse"></param>
        /// <returns></returns>
        ProductOrigin[] OriginAll(bool? isUse);
        /// <summary>
        /// 分页获取产地信息
        /// </summary>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        ProductOrigin[] OriginPager(bool? isUse, string searTxt, int size, int index, out int countSum);
        #endregion
    }
}
