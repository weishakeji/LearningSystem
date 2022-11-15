using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 知识库
    /// </summary>
    public class Knowledge : ViewMethod, IViewAPI
    {
        #region 分类
        /// <summary>
        /// 课程知识库的分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool SortAdd(Song.Entities.KnowledgeSort entity)
        {
            try
            {
                Business.Do<IKnowledge>().SortAdd(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 课程知识库的分类，树形
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="search"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        [HttpGet]
        public JArray SortTree(long couid, string search, bool? isuse)
        {
            //顶级分类
            Song.Entities.KnowledgeSort[] kns = Business.Do<IKnowledge>().GetSortAll(-1, couid, search, isuse);
            return kns.Length > 0 ? _sortTree(null, kns) : null; ;
        }
        private JArray _sortTree(Song.Entities.KnowledgeSort item, Song.Entities.KnowledgeSort[] items)
        {
            JArray jarr = new JArray();

            foreach (Song.Entities.KnowledgeSort m in items)
            {

                if (item == null)
                {
                    if (m.Kns_PID != 0) continue;
                }
                else
                {
                    if (m.Kns_PID != item.Kns_ID) continue;
                }

                string j = m.ToJson("", "Kns_CrtTime");

                JObject jo = JObject.Parse(j);
                jarr.Add(jo);
                //计算下级
                JArray charray = _sortTree(m, items);
                if (charray.Count > 0)
                    jo.Add("children", charray);
                //jo.Add("children", _sortTree(m, items));
            }
            return jarr;
        }
        /// <summary>
        /// 获取知识库分类的对象
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.KnowledgeSort SortForID(long id)
        {
            return Business.Do<IKnowledge>().SortSingle(id);
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id">分类id</param>
        /// <returns></returns>
        [HttpDelete]
        [Teacher, Admin]
        public bool SortDelete(long id)
        {
            try
            {
                Business.Do<IKnowledge>().SortDelete(id);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改知识库分类
        /// </summary>
        /// <param name="entity">分类的实体</param>
        /// <returns></returns>
        [HttpPut][HttpPost]
        public bool SortModify(KnowledgeSort entity)
        {
            try
            {
                Song.Entities.KnowledgeSort old = Business.Do<IKnowledge>().SortSingle(entity.Kns_ID);
                if (old == null) throw new Exception("Not found entity for KnowledgeSort！");

                old.Copy<Song.Entities.KnowledgeSort>(entity);
                Business.Do<IKnowledge>().SortSave(old);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更改分类的排序
        /// </summary>
        /// <param name="items">分类的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool SortUpdateTaxis(Song.Entities.KnowledgeSort[] items)
        {
            try
            {
                Business.Do<IKnowledge>().SortUpdateTaxis(items);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 知识内容
        /// <summary>
        /// 分页获取知识内容
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="kns">知识库分类id</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="search">用于检索的字符串</param>
        /// <param name="size">每页多少条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        [HttpGet]
        public ListResult Pager(long couid, long kns, bool? isuse, string search, int size, int index)
        {
            int count = 0;
            Song.Entities.Knowledge[] kls = null;
            kls = Business.Do<IKnowledge>().KnowledgePager(couid, kns, search, isuse, size, index, out count);
            ListResult result = new ListResult(kls);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取知识
        /// </summary>
        /// <param name="id">知识id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Knowledge ForID(long id)
        {
            return Business.Do<IKnowledge>().KnowledgeSingle(id);
        }
        /// <summary>
        /// 获取知识
        /// </summary>
        /// <param name="uid">知识id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Knowledge ForUID(string uid)
        {
            return Business.Do<IKnowledge>().KnowledgeSingle(uid);
        }
        /// <summary>
        /// 删除知识
        /// </summary>
        /// <param name="id">知识id</param>
        /// <returns></returns>
        [HttpDelete]
        [Teacher,Admin]
        public int Delete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            string[] arr = id.Split(',');
            foreach (string s in arr)
            {
                long idval = 0;
                long.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IKnowledge>().KnowledgeDelete(idval);
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
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.Knowledge Add(Song.Entities.Knowledge entity)
        {
            Business.Do<IKnowledge>().KnowledgeAdd(entity);
            return entity;
        }
        /// <summary>
        /// 修改知识内容
        /// </summary>
        /// <param name="entity">知识的实体</param>
        /// <returns></returns>
        [HttpPut,HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Modify(Song.Entities.Knowledge entity)
        {
            try
            {
                Song.Entities.Knowledge old = Business.Do<IKnowledge>().KnowledgeSingle(entity.Kn_ID);
                if (old == null) throw new Exception("Not found entity for Knowledge！");

                old.Copy<Song.Entities.Knowledge>(entity);
                Business.Do<IKnowledge>().KnowledgeSave(old);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
