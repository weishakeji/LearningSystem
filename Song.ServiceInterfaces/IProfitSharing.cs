using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Song.Entities;

namespace Song.ServiceInterfaces
{
    /// <summary>
    /// 分润的管理
    /// </summary>
    public interface IProfitSharing: WeiSha.Common.IBusinessInterface
    {
        #region 分润方案的管理
        /// <summary>
        /// 添加分润主题
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ThemeAdd(ProfitSharing entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ThemeSave(ProfitSharing entity);
        /// <summary>
        /// 当前分润方案
        /// </summary>
        /// <returns></returns>
        ProfitSharing ThemeCurrent();
        /// <summary>
        /// 机构的分润方案
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        ProfitSharing ThemeCurrent(int orgid);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ThemeDelete(ProfitSharing entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ThemeDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ProfitSharing ThemeSingle(int identify);
        /// <summary>
        /// 获取对象；即所有分类；
        /// </summary>
        /// <returns></returns>
        ProfitSharing[] ThemeAll(bool? isUse);
        /// <summary>
        /// 将当前项目向上移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ThemeRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ThemeRemoveDown(int id);
        #endregion

        #region 分润等级比例设置
        /// <summary>
        /// 添加分润项
        /// </summary>
        /// <param name="entity">业务实体</param>
        int ProfitAdd(ProfitSharing entity);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ProfitSave(ProfitSharing entity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        void ProfitDelete(ProfitSharing entity);
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        void ProfitDelete(int identify);
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        ProfitSharing ProfitSingle(int identify);
        /// <summary>
        /// 分润方案的分润项；
        /// </summary>
        /// <param name="theme">方案主题的id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        ProfitSharing[] ProfitAll(int theme, bool? isUse);
        /// <summary>
        /// 将当前项目向上移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ProfitRemoveUp(int id);
        /// <summary>
        /// 将当前项目向下移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        bool ProfitRemoveDown(int id);
        #endregion

        #region 分润计算
        /// <summary>
        /// 计算分润
        /// </summary>
        /// <param name="couid">课程id,需要知道当前课程在哪个机构，哪个机构等级，从而获取分润方案</param>
        /// <param name="money">消费的资金数</param>
        /// <param name="coupon">消费的卡数</param>
        /// <returns></returns>
        ProfitSharing[] Clac(int couid, double money, double coupon);
        ProfitSharing[] Clac(Course cou, double money, double coupon);
        /// <summary>
        /// 分配利润
        /// </summary>
        /// <param name="cou">当前课程</param>
        /// <param name="acc">当前学员账户</param>
        /// <param name="money">消费的资金数</param>
        /// <param name="coupon">消费的卡数</param>
        void Distribution(Course cou, Accounts acc, double money, double coupon);
        #endregion
    }
}
