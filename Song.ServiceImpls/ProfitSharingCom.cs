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
            Gateway.Default.Save<ProfitSharing>(entity);
            return entity.Ps_ID;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ProfitSave(ProfitSharing entity)
        {
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
    }
}
