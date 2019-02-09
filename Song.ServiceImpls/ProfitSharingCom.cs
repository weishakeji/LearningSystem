using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WeiSha.Common;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;



namespace Song.ServiceImpls
{
    public class ProfitSharingCom : IProfitSharing
    {
        #region 分润方案的管理
        /// <summary>
        /// 添加分润主题
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int ThemeAdd(ProfitSharing entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<ProfitSharing>(ProfitSharing._.Ps_Level, ProfitSharing._.Ps_IsTheme == true);
            int tax = obj is int ? (int)obj : 0;
            entity.Ps_Level = tax + 1;
            entity.Ps_IsTheme = true;
            Gateway.Default.Save<ProfitSharing>(entity);
            return entity.Ps_ID;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ThemeSave(ProfitSharing entity)
        {
            Gateway.Default.Save<ProfitSharing>(entity);
        }
        /// <summary>
        /// 当前分润方案
        /// </summary>
        /// <returns></returns>
        public ProfitSharing ThemeCurrent()
        {
            Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org == null) return null;
            return ThemeCurrent(org.Org_ID);
        }
        /// <summary>
        /// 机构的分润方案
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public ProfitSharing ThemeCurrent(int orgid)
        {
            //课程所在机构
            Organization org = Business.Do<IOrganization>().OrganSingle(orgid);
            if (org == null) return null;
            //所属机构等级
            OrganLevel olevel = Business.Do<IOrganization>().LevelSingle(org.Olv_ID);
            if (olevel == null) return null;
            //分润方案
            ProfitSharing psTheme = this.ThemeSingle(olevel.Ps_ID);
            if (psTheme == null) return null;       //如果没有设置分润方案
            if (!psTheme.Ps_IsUse) return null;     //如果分润方案没有启用
            return psTheme;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ThemeDelete(ProfitSharing entity)
        {
            this.ThemeDelete(entity.Ps_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ThemeDelete(int identify)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<ProfitSharing>(ProfitSharing._.Ps_ID==identify);
                    tran.Delete<ProfitSharing>(ProfitSharing._.Ps_PID == identify);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ProfitSharing ThemeSingle(int identify)
        {
            return Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_ID == identify).ToFirst<ProfitSharing>();
        }
        /// <summary>
        /// 获取对象；即所有分类；
        /// </summary>
        /// <returns></returns>
        public ProfitSharing[] ThemeAll(bool? isUse)
        {
            WhereClip wc = ProfitSharing._.Ps_IsTheme == true;
            if (isUse != null) wc.And(ProfitSharing._.Ps_IsUse == (bool)isUse);
            return Gateway.Default.From<ProfitSharing>().Where(wc).OrderBy(ProfitSharing._.Ps_Level.Asc).ToArray<ProfitSharing>();
        }
        /// <summary>
        /// 将当前项目向上移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ThemeRemoveUp(int id)
        {
            //当前对象
            ProfitSharing current = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_ID == id).ToFirst<ProfitSharing>();
            //当前对象排序号
            int orderValue = (int)current.Ps_Level;
            //上一个对象，即兄长对象；
            ProfitSharing up = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_IsTheme == true && ProfitSharing._.Ps_Level < orderValue)
                .OrderBy(ProfitSharing._.Ps_Level.Desc).ToFirst<ProfitSharing>();
            //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
            if (up == null) return false;            
            //交换排序号
            current.Ps_Level = up.Ps_Level;
            up.Ps_Level = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ProfitSharing>(current);
                    tran.Save<ProfitSharing>(up);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }
            
        }
        /// <summary>
        /// 将当前项目向下移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ThemeRemoveDown(int id)
        {
            //当前对象
            ProfitSharing current = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_ID == id).ToFirst<ProfitSharing>();
            //当前对象排序号
            int orderValue = (int)current.Ps_Level;
            //下一个对象，即弟弟对象；
            ProfitSharing down = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_IsTheme == true && ProfitSharing._.Ps_Level > orderValue)
                .OrderBy(ProfitSharing._.Ps_Level.Asc).ToFirst<ProfitSharing>();
            //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
            if (down == null) return false;
            //交换排序号
            current.Ps_Level = down.Ps_Level;
            down.Ps_Level = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ProfitSharing>(current);
                    tran.Save<ProfitSharing>(down);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }            
        }
        #endregion

        #region 分润等级比例设置
        /// <summary>
        /// 添加分润项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int ProfitAdd(ProfitSharing entity)
        {
            //添加对象，并设置排序号
            object obj = Gateway.Default.Max<ProfitSharing>(ProfitSharing._.Ps_Level, ProfitSharing._.Ps_PID == entity.Ps_PID);
            int tax = obj is int ? (int)obj : 0;
            entity.Ps_Level = tax + 1;
            entity.Ps_IsTheme = false;
            //验证资金分润比例之合，是否大于100
            object objm = Gateway.Default.Sum<ProfitSharing>(ProfitSharing._.Ps_Moneyratio, ProfitSharing._.Ps_PID == entity.Ps_PID);
            int summ = objm is int ? (int)objm : 0;
            if ((summ + entity.Ps_Moneyratio) > 100) throw new Exception("资金分润比例之合，不得大于100");
            //验证卡券分润比例之合，是否大于100
            object objc = Gateway.Default.Sum<ProfitSharing>(ProfitSharing._.Ps_Moneyratio, ProfitSharing._.Ps_PID == entity.Ps_PID);
            int sumc = objc is int ? (int)objc : 0;
            if ((sumc + entity.Ps_Couponratio) > 100) throw new Exception("卡券分润比例之合，不得大于100");
            Gateway.Default.Save<ProfitSharing>(entity);
            return entity.Ps_ID;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ProfitSave(ProfitSharing entity)
        {
            //验证资金分润比例之合，是否大于100
            object objm = Gateway.Default.Sum<ProfitSharing>(ProfitSharing._.Ps_Moneyratio, ProfitSharing._.Ps_PID == entity.Ps_PID && ProfitSharing._.Ps_ID != entity.Ps_ID);
            int summ = objm is int ? (int)objm : 0;
            if ((summ + entity.Ps_Moneyratio) > 100) throw new Exception("资金分润比例之合，不得大于100");
            //验证卡券分润比例之合，是否大于100
            object objc= Gateway.Default.Sum<ProfitSharing>(ProfitSharing._.Ps_Moneyratio, ProfitSharing._.Ps_PID == entity.Ps_PID && ProfitSharing._.Ps_ID != entity.Ps_ID);
            int sumc = objc is int ? (int)objc : 0;
            if ((sumc + entity.Ps_Couponratio) > 100) throw new Exception("卡券分润比例之合，不得大于100");
            //保存
            Gateway.Default.Save<ProfitSharing>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ProfitDelete(ProfitSharing entity)
        {
            Gateway.Default.Delete<ProfitSharing>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ProfitDelete(int identify)
        {
            Gateway.Default.Delete<ProfitSharing>(ProfitSharing._.Ps_ID == identify);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public ProfitSharing ProfitSingle(int identify)
        {
            return Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_ID == identify).ToFirst<ProfitSharing>();
        }
        /// <summary>
        /// 获取对象；即所有分类；
        /// </summary>
        /// <param name="theme">方案主题的id</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public ProfitSharing[] ProfitAll(int theme, bool? isUse)
        {
            WhereClip wc = ProfitSharing._.Ps_IsTheme == false && ProfitSharing._.Ps_PID == theme;
            if (isUse != null) wc.And(ProfitSharing._.Ps_IsUse == (bool)isUse);
            return Gateway.Default.From<ProfitSharing>().Where(wc).OrderBy(ProfitSharing._.Ps_Level.Asc).ToArray<ProfitSharing>();
        }
        /// <summary>
        /// 将当前项目向上移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ProfitRemoveUp(int id)
        {
            //当前对象
            ProfitSharing current = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_ID == id).ToFirst<ProfitSharing>();
            //当前对象排序号
            int orderValue = (int)current.Ps_Level;
            //上一个对象，即兄长对象；
            ProfitSharing up = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_PID == current.Ps_PID && ProfitSharing._.Ps_Level < orderValue)
                .OrderBy(ProfitSharing._.Ps_Level.Desc).ToFirst<ProfitSharing>();
            //如果兄长对象不存在，则表示当前节点在兄弟中是老大；即是最顶点；
            if (up == null) return false;
            //交换排序号
            current.Ps_Level = up.Ps_Level;
            up.Ps_Level = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ProfitSharing>(current);
                    tran.Save<ProfitSharing>(up);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }
        }
        /// <summary>
        /// 将当前项目向下移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool ProfitRemoveDown(int id)
        {
            //当前对象
            ProfitSharing current = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_ID == id).ToFirst<ProfitSharing>();
            //当前对象排序号
            int orderValue = (int)current.Ps_Level;
            //下一个对象，即弟弟对象；
            ProfitSharing down = Gateway.Default.From<ProfitSharing>().Where(ProfitSharing._.Ps_PID == current.Ps_PID && ProfitSharing._.Ps_Level > orderValue)
                .OrderBy(ProfitSharing._.Ps_Level.Asc).ToFirst<ProfitSharing>();
            //如果弟对象不存在，则表示当前节点在兄弟中是老幺；即是最底端；
            if (down == null) return false;
            //交换排序号
            current.Ps_Level = down.Ps_Level;
            down.Ps_Level = orderValue;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ProfitSharing>(current);
                    tran.Save<ProfitSharing>(down);
                    tran.Commit();
                    return true;
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
                finally
                {
                    tran.Close();
                }
            }      
        }
        #endregion

        #region 分润计算
        /// <summary>
        /// 计算分润
        /// </summary>
        /// <param name="couid">课程id,需要知道当前课程在哪个机构，哪个机构等级，从而获取分润方案</param>
        /// <param name="money">消费的资金数</param>
        /// <param name="coupon">消费的卡数</param>
        /// <returns></returns>
        public ProfitSharing[] Clac(int couid, double money, double coupon)
        {
            Course cou = Business.Do<ICourse>().CourseSingle(couid);
            return Clac(cou, money, coupon);
        }
        public ProfitSharing[] Clac(Course cou, double money, double coupon)
        {
            if (cou == null) return null;
            //分润方案
            ProfitSharing psTheme = this.ThemeCurrent(cou.Org_ID);
            if (psTheme == null) return null;       //如果没有设置分润方案
            if (!psTheme.Ps_IsUse) return null;     //如果分润方案没有启用
            ProfitSharing[] profits = this.ProfitAll(psTheme.Ps_ID, true);
            if (profits.Length < 1) return null;
            //计算
            for (int i = 0; i < profits.Length; i++)
            {
                profits[i].Ps_MoneyValue = (decimal)profits[i].Ps_Moneyratio * ((decimal)money / 100);
                profits[i].Ps_CouponValue = (int)Math.Floor(profits[i].Ps_Couponratio * ((double)coupon) / 100);
            }
            return profits;
        }
        /// <summary>
        /// 分配利润
        /// </summary>
        /// <param name="cou">当前课程</param>
        /// <param name="acc">当前学员账户</param>
        /// <param name="money">消费的资金数</param>
        /// <param name="coupon">消费的卡数</param>
        public void Distribution(Course cou, Accounts acc, double money, double coupon)
        {
            Accounts[] parents = Business.Do<IAccounts>().Parents(acc);
            if (parents.Length < 1) return;
            //计算分润
            ProfitSharing[] ps = this.Clac(cou, money, coupon);
            if (ps == null) return;
            int len = ps.Length > parents.Length ? parents.Length : ps.Length;
            for (int i = 0; i < len; i++)
            {
                //写入资金分润
                if (ps[i].Ps_MoneyValue > 0)
                {
                    MoneyAccount ma = new MoneyAccount();
                    ma.Ma_Money = ps[i].Ps_MoneyValue;
                    ma.Ac_ID = parents[i].Ac_ID;
                    ma.Ma_Source = "分润";
                    ma.Ma_Info = string.Format("{0}（{1}）购买课程《{2}》,获取收益{3}", acc.Ac_Name, acc.Ac_AccName, cou.Cou_Name, ps[i].Ps_MoneyValue);
                    ma.Ma_From = 5; //
                    ma.Ma_IsSuccess = true;
                    ma.Org_ID = parents[i].Org_ID;
                    if (ma.Ma_Money > 0)
                        ma = Business.Do<IAccounts>().MoneyIncome(ma);
                }
                //写入卡券分润
                if (ps[i].Ps_CouponValue > 0)
                {
                    Song.Entities.CouponAccount ca = new CouponAccount();
                    ca.Ac_ID = parents[i].Ac_ID;
                    ca.Ca_Source = "分润";
                    ca.Ca_Value = ps[i].Ps_CouponValue;                    
                    ca.Ca_Total = parents[i].Ac_Coupon; //当前卡券总数
                    ca.Ca_Info = string.Format("{0}（{1}）购买课程《{2}》,获取收益{3}", acc.Ac_Name, acc.Ac_AccName, cou.Cou_Name, ps[i].Ps_CouponValue);
                    ca.Ca_From = 5;
                    if (ca.Ca_Value > 0) Business.Do<IAccounts>().CouponAdd(ca);
                }
            }
        }
        #endregion
    }
}
