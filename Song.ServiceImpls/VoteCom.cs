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
    public class VoteCom : IVote
    {
        //图片上传的路径
        private string _uppath = "Vote";

        #region 调查主题的操作
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int ThemeAdd(Vote entity)
        {
            entity.Vt_CrtTime = DateTime.Now;
            entity.Vt_IsTheme = true;
            object obj = Gateway.Default.Max<Vote>(Vote._.Vt_Tax, Vote._.Vt_IsTheme == true);
            if (obj != DBNull.Value)
            {
                entity.Vt_Tax = Convert.ToInt32(obj) + 1;
            }
            else
            {
                entity.Vt_Tax = 1;
            }
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            return Gateway.Default.Save<Vote>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ThemeSave(Vote entity)
        {
            Gateway.Default.Save<Vote>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ThemeDelete(Vote entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //先删除当前主题下的选择项目
                    tran.Delete<Vote>(Vote._.Vt_UniqueId == entity.Vt_UniqueId);
                    //删除当前调查主题
                    tran.Delete<Vote>(Vote._.Vt_Id == entity.Vt_Id);
                    //删除当前调查主题的图片信息
                    string img = "~/upload/" + this._uppath + "/" + entity.Vt_Image;
                    img = WeiSha.Common.Server.MapPath(img);
                    if (System.IO.File.Exists(img))
                    {
                        System.IO.File.Delete(img);
                    }
                    string imgSmall = "~/upload/" + this._uppath + "/" + entity.Vt_ImageSmall;
                    imgSmall = WeiSha.Common.Server.MapPath(imgSmall);
                    if (System.IO.File.Exists(imgSmall))
                    {
                        System.IO.File.Delete(imgSmall);
                    }
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
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ThemeDelete(int identify)
        {
            this.ThemeDelete(this.GetSingle(identify));
        }
        /// <summary>
        /// 获取主题，如果当前主题不存在，则返回最新的调查主题
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public Vote GetTheme(int identify)
        {
            Song.Entities.Vote theme = null;
            if (identify > 0)
            {
                theme = Gateway.Default.From<Vote>().Where(Vote._.Vt_IsTheme == true && Vote._.Vt_Id == identify).ToFirst<Vote>();
            }
            //如果当前主题不存在
            if (theme == null || identify < 1)
            {
                theme = Gateway.Default.From<Vote>().Where(Vote._.Vt_IsTheme==true).OrderBy(Vote._.Vt_CrtTime.Desc).ToFirst<Vote>();
            }
            return theme;
        }
        /// <summary>
        /// 获取所有简单调查的主题
        /// </summary>
        /// <param name="isShow"></param>
        /// <param name="isUse"></param>
        /// <param name="count">如果小于等于0，则取所有</param>
        /// <returns></returns>
        public Vote[] GetSimpleTheme(bool? isShow, bool? isUse, int count)
        {
            WhereClip wc = Vote._.Vt_IsTheme == true && Vote._.Vt_Type==1;
            if (isShow != null)
            {
                wc.And(Vote._.Vt_IsShow == isShow);
            }
            if (isUse != null)
            {
                wc.And(Vote._.Vt_IsUse == isUse);
            }
            if (count > 0)
            {
                return Gateway.Default.From<Vote>().Where(wc).OrderBy(Vote._.Vt_CrtTime.Desc).ToArray<Vote>(count);
            }
            return Gateway.Default.From<Vote>().Where(wc).OrderBy(Vote._.Vt_CrtTime.Desc).ToArray<Vote>();
        }
        #endregion

        #region 调查项的操作
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int ItemAdd(Vote entity)
        {
            entity.Vt_CrtTime = DateTime.Now;
            entity.Vt_IsTheme = false;
            return Gateway.Default.Save<Vote>(entity);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ItemSave(Vote entity)
        {
            Gateway.Default.Save<Vote>(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void ItemDelete(Vote entity)
        {
            Gateway.Default.Delete<Vote>(Vote._.Vt_Id == entity.Vt_Id);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ItemDelete(int identify)
        {
            Gateway.Default.Delete<Vote>(Vote._.Vt_Id == identify);
        }
        #endregion        
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public Vote GetSingle(int identify)
        {
            return Gateway.Default.From<Vote>().Where(Vote._.Vt_Id == identify).ToFirst<Vote>();
        }
        /// <summary>
        /// 向某个选项，增加一个投票数
        /// </summary>
        /// <param name="identify"></param>
        public void AddResult(int identify)
        {
            Vote vt = this.GetSingle(identify);
            vt.Vt_Number = vt.Vt_Number + 1;
            this.ItemSave(vt);
        }
        /// <summary>
        /// 向某个选项，增加指定投票数
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="num">指定的票数</param>
        public void AddResult(int identify, int num)
        {
            Vote vt = this.GetSingle(identify);
            vt.Vt_Number = vt.Vt_Number + num;
            this.ItemSave(vt);
        }
        /// <summary>
        /// 获取某个调查的调查子项
        /// </summary>
        /// <param name="uniqueid"></param>
        /// <returns></returns>
        public DataTable GetVoteItem(string uniqueid)
        {
            DataSet ds = Gateway.Default.From<Vote>().Where(Vote._.Vt_UniqueId == uniqueid && Vote._.Vt_IsTheme==false).OrderBy(Vote._.Vt_Tax.Asc).ToDataSet();
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        /// <summary>
        /// 获取某个调查的子项，并输出该调查总票数
        /// </summary>
        /// <param name="uniqueid"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public Vote[] GetVoteItem(string uniqueid, out int countSum)
        {
            object obj = Gateway.Default.Sum<Vote>(Vote._.Vt_Number, Vote._.Vt_UniqueId == uniqueid && Vote._.Vt_IsTheme==false);
            countSum = obj is DBNull ? 0 : Convert.ToInt32(obj);
            return Gateway.Default.From<Vote>().Where(Vote._.Vt_UniqueId == uniqueid).ToArray<Vote>();
        }
        public Vote[] GetPager(bool? isShow, bool? isUse, int type, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = Vote._.Vt_IsTheme == true && Vote._.Vt_Type == type;
            if (isShow != null)
            {
                wc.And(Vote._.Vt_IsShow == isShow);
            }
            if (isUse != null)
            {
                wc.And(Vote._.Vt_IsUse == isUse);
            }
            if (searTxt.Trim() != "" && searTxt != null)
            {
                wc.And(Vote._.Vt_Name.Like("%" + searTxt + "%"));
            }
            countSum = Gateway.Default.Count<Vote>(wc);
            return Gateway.Default.From<Vote>().Where(wc).OrderBy(Vote._.Vt_CrtTime.Desc).ToArray<Vote>(size, (index - 1) * size);
        }
        public Vote[] GetPager(bool? isShow, bool? isUse, int type, string searTxt, DateTime start, DateTime end, int size, int index, out int countSum)
        {
            WhereClip wc = Vote._.Vt_IsTheme == true && Vote._.Vt_Type == type;
            if (isShow != null)
            {
                wc.And(Vote._.Vt_IsShow == isShow);
            }
            if (isUse != null)
            {
                wc.And(Vote._.Vt_IsUse == isUse);
            }
            if (searTxt.Trim() != "" && searTxt != null)
            {
                wc.And(Vote._.Vt_Name.Like("%" + searTxt + "%"));
            }
            wc.And(Vote._.Vt_CrtTime > start && Vote._.Vt_CrtTime < end);
            countSum = Gateway.Default.Count<Vote>(wc);
            return Gateway.Default.From<Vote>().Where(wc).OrderBy(Vote._.Vt_Tax.Desc).ToArray<Vote>(size, (index - 1) * size);
        }
    }
}
