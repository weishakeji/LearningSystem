using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Web;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 课程公告（或叫学习指南）
    /// </summary>
    [HttpGet]
    public class Guide : ViewMethod, IViewAPI
    {       

        #region 课程公告的分类
        /// <summary>
        /// 获取课程公告分类的单一实体
        /// </summary>
        /// <param name="id">课程公告分类的id</param>
        /// <returns></returns>
        public Song.Entities.GuideColumns ColumnsForID(int id)
        {
            return Business.Do<IGuide>().ColumnsSingle(id);
        }
        /// <summary>
        /// 添加课程公告分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool ColumnsAdd(Song.Entities.GuideColumns entity)
        {
            try
            {               
                Business.Do<IGuide>().ColumnsAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改课程公告的分类
        /// </summary>
        /// <param name="entity">友情课程公告的分类</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool ColumnsModify(GuideColumns entity)
        {
            Song.Entities.GuideColumns old = Business.Do<IGuide>().ColumnsSingle(entity.Gc_ID);
            if (old == null) throw new Exception("Not found entity for GuideColumns！");

            old.Copy<Song.Entities.GuideColumns>(entity);
            Business.Do<IGuide>().ColumnsSave(old);
            return true;
        }
        /// <summary>
        /// 删除课程公告分类
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int ColumnsDelete(string id)
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
                    Business.Do<IGuide>().ColumnsDelete(idval);
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
        /// 课程下的指南分类
        /// </summary>
        /// <param name="couid">机构id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        public JArray ColumnsTree(long couid, string search, bool? isuse)
        {
            Song.Entities.GuideColumns[] arr = Business.Do<IGuide>().GetColumnsAll(couid, search, isuse);
            for (int i = 0; i < arr.Length; i++)
            {
                //sbjs[i] = _tran(sbjs[i]);
            }
            return arr.Length > 0 ? _childrenNode(null, arr.ToList< GuideColumns>()) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _childrenNode(Song.Entities.GuideColumns item, List<Song.Entities.GuideColumns> items)
        {
            JArray jarr = new JArray();

            foreach (Song.Entities.GuideColumns m in items)
            {
                if (item == null)
                {
                    if (m.Gc_PID != "0") continue;
                }
                else
                {
                    if (m.Gc_PID != item.Gc_UID) continue;
                }
                string j = m.ToJson("", "Gc_CrtTime");
                JObject jo = JObject.Parse(j);
                jarr.Add(jo);
                //计算下级
                JArray charray = _childrenNode(m, items);
                if (charray.Count > 0)
                    jo.Add("children", charray);
            }
            return jarr;
        }
        /// <summary>
        /// 更改课程公告分类的排序
        /// </summary>
        /// <param name="items">课程公告分类的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool ColumnsUpdateTaxis(Song.Entities.GuideColumns[] items)
        {
            try
            {
                Business.Do<IGuide>().ColumnsUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 课程公告
        /// <summary>
        /// 获取课程公告的单一实体
        /// </summary>
        /// <param name="id">课程公告的id</param>
        /// <returns></returns>
        public Song.Entities.Guide ForID(int id)
        {
            return Business.Do<IGuide>().GuideSingle(id);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.Guide Add(Song.Entities.Guide entity)
        {
            Business.Do<IGuide>().GuideAdd(entity);
            return entity;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]   
        [HtmlClear(Not = "entity")]
        public Song.Entities.Guide Modify(Song.Entities.Guide entity)
        {
            Song.Entities.Guide old = Business.Do<IGuide>().GuideSingle(entity.Gu_Id);
            if (old == null) throw new Exception("Not found entity for Guide！");

            old.Copy<Song.Entities.Guide>(entity);
            Business.Do<IGuide>().GuideSave(old);
            return old;
        }
        /// <summary>
        /// 删除课程公告
        /// </summary>
        /// <param name="id">账户id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete]
        public int Delete(string id)
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
                    Business.Do<IGuide>().GuideDelete(idval);
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
        /// 分页获取课程公告
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="uid">课程公告的分类uid</param>
        /// <param name="show">是否在前端显示，默认为null，即显示所有</param>
        /// <param name="search">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(long couid,string uid, bool? show, string search, int size, int index)
        {
            //总记录数
            int count = 0;
            Song.Entities.Guide[] eas = Business.Do<IGuide>().GuidePager(-1, couid, uid, search, show, size, index, out count);
            ListResult result = new ListResult(eas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取课程通知
        /// </summary>
        /// <param name="couid">课程的id</param>
        /// <param name="count">取多少条通知</param>
        /// <returns></returns>
        [HttpGet, HttpPut]
        [Cache(Expires = 60)]
        public Song.Entities.Guide[] Guides(long couid, int count)
        {
            return Business.Do<IGuide>().GuideCount(-1, couid, string.Empty, count);
        }
        #endregion

    }
}
