using System;
using System.Collections.Generic;
using System.Text;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 充值码管理
    /// </summary>
    public interface IRecharge : WeiSha.Common.IBusinessInterface
    { 
        #region 充值码设置管理
        /// <summary>
        /// 添加充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RechargeSetAdd(RechargeSet entity);
        /// <summary>
        /// 修改充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RechargeSetSave(RechargeSet entity);
        /// <summary>
        /// 删除充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RechargeSetDelete(RechargeSet entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void RechargeSetDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        RechargeSet RechargeSetSingle(int identify);
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        RechargeSet[] RechargeSetCount(int orgid, bool? isEnable, int count);
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        int RechargeSetOfCount(int orgid, bool? isEnable);
        /// <summary>
        /// 分页获取充值码设置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        RechargeSet[] RechargeSetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum);
        #endregion

        #region 充值码管理
        /// <summary>
        /// 添加充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RechargeCodeAdd(RechargeCode entity);
        /// <summary>
        /// 修改充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RechargeCodeSave(RechargeCode entity);
        /// <summary>
        /// 删除充值码设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        void RechargeCodeDelete(RechargeCode entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void RechargeCodeDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        RechargeCode RechargeCodeSingle(int identify);
        /// <summary>
        /// 校验充值码是否存在，或过期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        RechargeCode CouponCheckCode(string code);
        /// <summary>
        /// 使用该充值码
        /// </summary>
        /// <param name="entity"></param>
        void CouponUseCode(RechargeCode entity);
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        RechargeCode[] RechargeCodeCount(int orgid, int rsid, bool? isEnable, bool? isUsed, int count);
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <returns></returns>
        int RechargeCodeOfCount(int orgid, int rsid, bool? isEnable, bool? isUsed);
        /// <summary>
        /// 导出Excel格式的充值码信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充值码设置项的id</param>
        /// <returns></returns>
        string RechargeCode4Excel(string path, int orgid, int rsid);
        /// <summary>
        /// 分页获取充值码设置项
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="rsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        RechargeCode[] RechargeCodePager(int orgid, int rsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum);
        RechargeCode[] RechargeCodePager(int orgid, int rsid, string code, bool? isEnable, bool? isUsed, int size, int index, out int countSum);
        #endregion
    }
}
