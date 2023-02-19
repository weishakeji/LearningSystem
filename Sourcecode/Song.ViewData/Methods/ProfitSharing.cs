using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using System.Data;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 分润管理
    /// </summary>
    [HttpPut, HttpGet]
    public class ProfitSharing : ViewMethod, IViewAPI
    {
        #region 分润方案
        public Song.Entities.ProfitSharing ThemeCurrent()
        {
            return Business.Do<IProfitSharing>().ThemeCurrent();
        }
        /// <summary>
        /// 分润主题（或叫方案）
        /// </summary>
        /// <param name="id">专业id</param>
        /// <returns></returns>
        public Song.Entities.ProfitSharing ThemeForID(int id)
        {
            return Business.Do<IProfitSharing>().ThemeSingle(id);
        }
        /// <summary>
        /// 所有分润方案
        /// </summary>
        /// <returns></returns>
        [SuperAdmin]
        public Song.Entities.ProfitSharing[] ThemeList()
        {
            return Business.Do<IProfitSharing>().ThemeAll(null);
        }
        /// <summary>
        /// 所有可用的分润方案
        /// </summary>
        /// <returns></returns>
        public Song.Entities.ProfitSharing[] ThemeUseList()
        {
            return Business.Do<IProfitSharing>().ThemeAll(true);
        }
        /// <summary>
        /// 主题是否存在
        /// </summary>
        /// <param name="name">主题名称</param>
        /// <param name="id">主题id</param>
        /// <returns></returns>
        public bool ThemeExist(string name,int id)
        {
            return Business.Do<IProfitSharing>().ThemeExist(name, id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpDelete]
        public int ThemeDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {

                    Business.Do<IProfitSharing>().ThemeDelete(idval);
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 更改排序
        /// </summary>
        /// <param name="items">数组</param>
        /// <returns></returns>
        [HttpPost]
        [SuperAdmin]
        public bool ModifyTaxis(Song.Entities.ProfitSharing[] items)
        {
            try
            {
                Business.Do<IProfitSharing>().UpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region  分润项
        /// <summary>
        /// 某个分润方案下的分润项
        /// </summary>
        /// <returns></returns>
        public Song.Entities.ProfitSharing[] ProfitList(int tid)
        {
            return Business.Do<IProfitSharing>().ProfitAll(tid, null);
        }
        /// <summary>
        /// 修改分润项
        /// </summary>
        /// <param name="theme">主题</param>
        /// <param name="items">分润项</param>
        /// <returns></returns>
        [SuperAdmin]
        [HttpPost]
        public bool ProfitSave(Song.Entities.ProfitSharing theme, Song.Entities.ProfitSharing[] items)
        {
            Business.Do<IProfitSharing>().ProfitSave(theme, items);
            return true;
        }
        #endregion
  
    }
}
