using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;
using Song.ViewData;
using Help = Song.ViewData.Helper;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Data;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 考试专用题库的管理，有区别于课程中的题库
    /// </summary>
    public class ExamQues : ViewMethod, IViewAPI
    {
        #region 试题
        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="id">试题id</param>
        /// <returns></returns> 
        public JObject QuesForID(long id)
        {
            Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(id);
            if (ques == null) return null;
            JObject jo = ques.ToJObject();
            //关联的关键字
            List<Song.Entities.QuesTags> tags = Business.Do<IExamQues>().TagForQues(ques.Qus_ID);
            jo["Tags"] = tags.ToJArray<QuesTags>();
            //所属的分类
            List<Song.Entities.QuesPart> parts = Business.Do<IExamQues>().PartForQues(ques.Qus_ID);
            jo["Parts"] = parts.ToJArray<QuesPart>();
            //关联的知识点
            List<Song.Entities.QuesKnowledge> knls = Business.Do<IExamQues>().KnlForQues(ques.Qus_ID);
            jo["Knls"] = knls.ToJArray<QuesKnowledge>();
            return jo;
        }
        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="entity">试题</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public long QuesAdd(Song.Entities.Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls)
        {
            //清理脚本
            entity.Qus_Title = QuestionHandler.CleanText.Title(entity.Qus_Title);
            entity.Qus_Answer = QuestionHandler.CleanText.Content(entity.Qus_Answer);
            entity.Qus_Explain = QuestionHandler.CleanText.Content(entity.Qus_Explain);

            //如果存在，不保存
            Song.Entities.Questions old = Business.Do<IExamQues>().QuesSingle(entity.Qus_ID);
            if (old != null) return old.Qus_ID;
            //处理单选、多选的选项
            if (entity.Qus_Type == 1 || entity.Qus_Type == 2 || entity.Qus_Type == 5)
            {
                entity.Qus_Items = Business.Do<IQuestions>().AnswerToItems(Helper.Question.AnswerToItems(entity));
            }
            entity.Qus_Purpose = 1;    //考试专用
            Business.Do<IExamQues>().QuesAdd(entity, parts, tags, knls);          
            return entity.Qus_ID;
        }
        /// <summary>
        /// 修改试题
        /// </summary>
        /// <param name="entity">修改试题</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool QuesModify(Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls)
        {
            //清理脚本
            entity.Qus_Title = QuestionHandler.CleanText.Title(entity.Qus_Title);
            entity.Qus_Answer = QuestionHandler.CleanText.Content(entity.Qus_Answer);
            entity.Qus_Explain = QuestionHandler.CleanText.Content(entity.Qus_Explain);

            Song.Entities.Questions old = Business.Do<IQuestions>().QuesSingle(entity.Qus_ID);
            if (old == null) throw new Exception("Not found entity for Questions！");            
          
            //处理单选、多选的选项
            if (entity.Qus_Type == 1 || entity.Qus_Type == 2 || entity.Qus_Type == 5)
            {
                entity.Qus_Items = Business.Do<IQuestions>().AnswerToItems(Helper.Question.AnswerToItems(entity));
            }
            entity.Qus_Purpose = 1;    //考试专用

            old.Copy<Song.Entities.Questions>(entity);
            Business.Do<IExamQues>().QuesSave(old, parts, tags, knls);
            return true;
        }
        /// <summary>
        /// 逻辑删除试题
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int QuesDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().QuesDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int QuesRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().QuesRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int QuesRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list) i += Business.Do<IExamQues>().QuesRemove(s);
            return i;
        }
        /// <summary>
        /// 获取题库列表
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="qpid">试题分类，多个分类用逗号分隔</param>
        /// <param name="tagid">关键字，多个分类用逗号分隔</param>
        /// <param name="knlid">知识点，多个分类用逗号分隔</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难度</param>
        /// <param name="use">是否启用</param>
        /// <param name="error">是否有格式错误</param>
        /// <param name="wrong">是否有反馈的错误</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ListResult QuesPager(int orgid, string search, bool? isdeleted, string qpid, string tagid, string knlid,
            string type, string diff, bool? use, bool? error, bool? wrong,
            int size, int index)
        {
            int sum;
            List<Questions> list = Business.Do<IExamQues>().QuesPager(orgid, search, isdeleted,
                qpid.ToArray<long>(),
                tagid.ToArray<long>(),
                knlid.ToArray<long>(),
                type.ToArray<int>(),
                diff.ToArray<int>(),
                use, error, wrong, size, index, out sum);

            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }
        /// <summary>
        /// 获取试题数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="qpid">字类id</param>
        /// <param name="tagid">关键字id</param>
        /// <param name="knlid">知识点</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="diffs">难度</param>
        /// <param name="use">是否启用</param>
        /// <param name="error">错误</param>
        /// <param name="wrong">报错</param>
        /// <returns>试题类型，数量</returns>
        public JArray QuesTotal(int orgid, string qpid, string tagid, string knlid, bool? isdeleted, string diffs, bool? use, bool? error, bool? wrong)
        {
            Dictionary<int, int> dic = Business.Do<IExamQues>().QuesTotal(orgid,
                qpid.ToArray<long>(),
                tagid.ToArray<long>(),
                knlid.ToArray<long>(),
                isdeleted,
                diffs.ToArray<int>(),
                use, error, wrong);
            string[] types = Business.Do<IQuestions>().QuestionTypes();
            JArray arr = new JArray();
            foreach (KeyValuePair<int, int> kv in dic)
            {
                JObject jo = new JObject();
                jo.Add("type", kv.Key);     //试题类型
                jo.Add("name", types[kv.Key - 1]);  //试题类型名称
                jo.Add("total", kv.Value);      //试题数量
                //jo.Add("count", 0);
                //jo.Add("score", 0);
                //jo.Add("rate", 0);
                arr.Add(jo);
            }
            return arr;
        }
        /// <summary>
        /// 计算有多少条试题
        /// </summary>  
        /// <param name="orgid">机构id</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="use">是否启用</param>
        /// <param name="error">是否错误</param>
        /// <param name="wrong">是否有回馈问题</param>
        public int Total(int orgid, int[] types, string qpid, string tagid, string knlid, bool? isdeleted, int[] diffs, bool? use, bool? error, bool? wrong)
        {
            return Business.Do<IExamQues>().Total(orgid, types,
                qpid.ToArray<long>(),
                tagid.ToArray<long>(),
                knlid.ToArray<long>(),
                isdeleted, diffs,
                use, error, wrong);
        }
        #endregion

        #region 试题收藏
        /// <summary>
        /// 试题是否被收藏
        /// </summary>
        /// <param name="qusid">试题id</param>
        /// <param name="accid">管理id</param>
        /// <returns></returns>
        public bool Collected(int accid, long qusid)
        {
            return Business.Do<IExamQues>().Collected(accid, qusid);
        }
        /// <summary>
        /// 收藏试题
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool CollectAdd(int accid, long qusid) => Business.Do<IExamQues>().CollectAdd(accid, qusid);
        /// <summary>
        /// 取消试题收藏
        /// </summary>
        /// <param name="accid">管理id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int CollectRemove(int accid, string qusid)
        {
            return Business.Do<IExamQues>().CollectRemove(accid, qusid.ToArray<long>());
        }

        /// <summary>
        /// 收藏状态的变更
        /// </summary>
        /// <param name="accid">管理id</param>
        /// <param name="qusid">试题id</param>
        /// <param name="state">收藏状态，为true时创建收藏关联，否则删除收藏</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public bool CollectUpdate(int accid, long qusid,bool state)
        {
            if(state) return Business.Do<IExamQues>().CollectAdd(accid, qusid);
            else return Business.Do<IExamQues>().CollectRemove(accid, qusid);
        }
        /// <summary>
        /// 管理员收藏的试题
        /// </summary>
        /// <param name="acid">管理员的id</param>
        /// <param name="search"></param>
        /// <param name="qpid">试题分类的id，多个id用逗号分隔</param>
        /// <param name="tagid">关键字</param>
        /// <param name="knlid">知识点</param>
        /// <param name="type">题型</param>
        /// <param name="diff">难度</param>
        /// <param name="use">是否启用</param>
        /// <param name="error">是否有格式错误</param>
        /// <param name="wrong">是否有反馈的错误</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult CollectPager(int acid, string search, string qpid, string tagid, string knlid,
            string type, string diff, bool? use, bool? error, bool? wrong, int size, int index)
        {
            int sum;
            List<Questions> list = Business.Do<IExamQues>().CollectPager(acid, search,
                qpid.ToArray<long>(),
                tagid.ToArray<long>(),
                knlid.ToArray<long>(),
                type.ToArray<int>(),
                diff.ToArray<int>(),
                use, error, wrong, size, index, out sum);

            Song.ViewData.ListResult result = new ListResult(list);
            result.Index = index;
            result.Size = size;
            result.Total = sum;
            return result;
        }

        #endregion

        #region 试题分类

        /// <summary>
        /// 获取试题分类的单条数据
        /// </summary>
        /// <param name="id">试题分类id</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public Song.Entities.QuesPart PartForID(long id) => Business.Do<IExamQues>().PartSingle(id);
        /// <summary>
        /// 当前试题分类的上级父级
        /// </summary>
        /// <param name="qpid">当前试题分类的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesPart> PartParents(long qpid, bool isself) => Business.Do<IExamQues>().PartParents(qpid, isself);
        /// <summary>
        /// 添加试题分类
        /// </summary>
        /// <param name="entity">试题分类的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesPart PartAdd(Song.Entities.QuesPart entity)
        {
            Business.Do<IExamQues>().PartAdd(entity);
            return entity;
        }
        /// <summary>
        /// 修改试题分类
        /// </summary>
        /// <param name="entity">试题分类的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesPart PartModify(Song.Entities.QuesPart entity)
        {
            Song.Entities.QuesPart old = Business.Do<IExamQues>().PartSingle(entity.Qp_ID);
            if (old == null) throw new Exception("Not found entity for QuesPart！");          
            try
            { 
                old.Copy<Song.Entities.QuesPart>(entity);
                Business.Do<IExamQues>().PartSave(old);
                return entity;
            }
            catch (Exception ex)
            {               
                throw ex;
            }
        }
        /// <summary>
        /// 逻辑删除试题分类，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题分类id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int PartDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().PartDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题分类
        /// </summary>
        /// <param name="id">试题分类id，可以是多个，用逗号分隔</param>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int PartRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().PartRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题分类，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题分类id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int PartRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().PartRemove(s);
            return i;
        }
        /// <summary>
        /// 分页获取试题分类
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="pid">上级id</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="name">分类名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult PartPager(int orgid, long pid, bool? isuse, bool? isdeleted, string name, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count;
            List<Song.Entities.QuesPart> arr = Business.Do<IExamQues>().PartPager(orgid, pid, isuse, isdeleted, name, size, index, out count);            
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取指定数量的试题分类
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sear">按名称搜索</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="pid">上级节点的ID</param>
        /// <param name="count">返回指定数量</param>
        /// <returns></returns>
        public List<QuesPart> PartCount(int orgid, string sear, bool? isuse, bool? isdeleted, long pid, int count)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            return Business.Do<IExamQues>().PartCount(orgid, sear, isuse, isdeleted, pid, count);
        }
        /// <summary>
        /// 试题分类，用于前端展示，被禁用的专业不显示
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>树形数据，子节点为 children</returns>
        [Cache]
        public JArray PartTreeFront(int orgid)
        {
            List<Song.Entities.QuesPart> sbjs = Business.Do<IExamQues>().PartCount(orgid, string.Empty, true, false, -1, -1);
            return sbjs.Count > 0 ? _partsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 试题分类，树形结构，不包括被逻辑删除的
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="isuse">是否启用</param>
        /// <returns>树形数据，子节点为 children</returns>
        public JArray PartTree(int orgid, string search, bool? isuse)
        {
            List<Song.Entities.QuesPart> sbjs = Business.Do<IExamQues>().PartCount(orgid, search, isuse, false, -1, -1);
            return sbjs.Count > 0 ? _partsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _partsNode(Song.Entities.QuesPart item, List<Song.Entities.QuesPart> items)
        {
            List<Song.Entities.QuesPart> childs = new List<Song.Entities.QuesPart>();
            for (int i = 0; i < items.Count; i++)
            {
                Entities.QuesPart m = items[i];
                if (item == null && m.Qp_PID != 0) continue;
                if (item != null && m.Qp_PID != item.Qp_ID) continue;
                childs.Add(m);
                items.RemoveAt(i);
                i--;
            }
            JArray jarr = new JArray();
            for (int i = 0; i < childs.Count; i++)
            {
                JObject jo = childs[i].ToJObject();               
                //计算下级
                JArray charray = _partsNode(childs[i], items);
                if (charray.Count > 0) jo.Add("children", charray);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 更改试题分类的排序
        /// </summary>
        /// <param name="list">专业的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool PartModifyTaxis(Song.Entities.QuesPart[] list)
        {
            try
            {
                Business.Do<IExamQues>().PartUpdateTaxis(list);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id，多个id用逗号分隔</param>
        /// <param name="qtype">题型</param>
        /// <param name="use">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        public int PartQusTotal(int orgid, string qpid, int qtype, bool? use, bool children)
          => Business.Do<IExamQues>().PartQusTotal(orgid, qpid.ToArray<long>(), qtype, use, children);

        /// <summary>
        /// 试题所属的分类
        /// </summary>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        public List<QuesPart> PartForQues(long qusid) => Business.Do<IExamQues>().PartForQues(qusid);
        #endregion

        #region 试题知识点

        /// <summary>
        /// 获取试题知识点的单条数据
        /// </summary>
        /// <param name="id">试题知识点id</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public Song.Entities.QuesKnowledge KnlForID(long id)
        {
            return Business.Do<IExamQues>().KnlSingle(id);
        }
        /// <summary>
        /// 当前试题知识点的上级父级
        /// </summary>
        /// <param name="qkid">当前试题知识点的id</param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlParents(long qkid, bool isself) => Business.Do<IExamQues>().KnlParents(qkid, isself);
        /// <summary>
        /// 添加试题知识点
        /// </summary>
        /// <param name="entity">试题知识点的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesKnowledge KnlAdd(Song.Entities.QuesKnowledge entity)
        {
            Business.Do<IExamQues>().KnlAdd(entity);
            return entity;
        }
        /// <summary>
        /// 修改试题知识点
        /// </summary>
        /// <param name="entity">试题知识点的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public Song.Entities.QuesKnowledge KnlModify(Song.Entities.QuesKnowledge entity)
        {
            Song.Entities.QuesKnowledge old = Business.Do<IExamQues>().KnlSingle(entity.Qk_ID);
            if (old == null) throw new Exception("Not found entity for QuesKnowledge！");
            try
            {
                old.Copy<Song.Entities.QuesKnowledge>(entity);
                Business.Do<IExamQues>().KnlSave(old);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 逻辑删除试题知识点，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int KnlDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().KnlDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题知识点
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int KnlRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().KnlRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题知识点，下级分类也会一并删除
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int KnlRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().KnlRemove(s);
            return i;
        }
        /// <summary>
        /// 分页获取试题知识点
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="pid">上级id</param>
        /// <param name="isuse">是否启用</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="name">分类名称</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult KnlPager(int orgid, long pid, bool? isuse, bool? isdeleted, string name, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count;
            List<Song.Entities.QuesKnowledge> arr = Business.Do<IExamQues>().KnlPager(orgid, pid, isuse, isdeleted, name, size, index, out count);
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 某个机构下的专业，用于前端展示，被禁用的专业不显示
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns>专业列表</returns>
        [Cache]
        public JArray KnlTreeFront(int orgid)
        {
            List<Song.Entities.QuesKnowledge> sbjs = Business.Do<IExamQues>().KnlCount(orgid, string.Empty, true, false, -1, -1);
            return sbjs.Count > 0 ? _KnlsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 机构下的知识点，树形数据
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="isuse">是否启用</param>
        /// <returns></returns>
        public JArray KnlTree(int orgid, string search, bool? isuse)
        {
            List<Song.Entities.QuesKnowledge> sbjs = Business.Do<IExamQues>().KnlCount(orgid, search, isuse, false, -1, -1);
            return sbjs.Count > 0 ? _KnlsNode(null, sbjs) : null;
        }
        /// <summary>
        /// 生成菜单子节点
        /// </summary>
        /// <param name="item">当前菜单项</param>
        /// <param name="items">所有菜单项</param>
        /// <returns></returns>
        private JArray _KnlsNode(Song.Entities.QuesKnowledge item, List<Song.Entities.QuesKnowledge> items)
        {
            List<Song.Entities.QuesKnowledge> childs = new List<Song.Entities.QuesKnowledge>();
            for (int i = 0; i < items.Count; i++)
            {
                Entities.QuesKnowledge m = items[i];
                if (item == null && m.Qk_PID != 0) continue;
                if (item != null && m.Qk_PID != item.Qk_ID) continue;
                childs.Add(m);
                items.RemoveAt(i);
                i--;
            }
            JArray jarr = new JArray();
            for (int i = 0; i < childs.Count; i++)
            {
                JObject jo = childs[i].ToJObject();             
                //计算下级
                JArray charray = _KnlsNode(childs[i], items);
                if (charray.Count > 0) jo.Add("children", charray);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 更改试题知识点的排序
        /// </summary>
        /// <param name="list">专业的数组</param>
        /// <returns></returns>
        [HttpPost]
        [Admin]
        public bool KnlModifyTaxis(Song.Entities.QuesKnowledge[] list)
        {
            try
            {
                Business.Do<IExamQues>().KnlUpdateTaxis(list);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id,多个id用逗号分隔</param>
        /// <param name="qtype">题型</param>
        /// <param name="use">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int KnlQusTotal(int orgid, string qkid, int qtype, bool? use, bool children)
            => Business.Do<IExamQues>().KnlQusTotal(orgid, qkid.ToArray<long>(), qtype, use, children);
        #endregion

        #region 试题关键字
        /// <summary>
        /// 获取试题关键字的单条数据
        /// </summary>
        /// <param name="id">试题知识点id</param>
        /// <returns></returns>
        [Cache(AdminDisable = true)]
        public Song.Entities.QuesTags TagForID(long id)
        {
            return Business.Do<IExamQues>().TagSingle(id);
        }
        /// <summary>
        /// 添加试题关键字
        /// </summary>
        /// <param name="entity">试题关键字的实体，如果关键字有逗号或空格，会当成多个关键字</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public int TagAdd(Song.Entities.QuesTags entity)
        {
            int count = 0;
            entity.Qtag_Name = entity.Qtag_Name.Replace(",", " ").Replace("，", " ");
            foreach (string name in entity.Qtag_Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Song.Entities.QuesTags tag = new QuesTags();
                tag.Qtag_Name = name;
                tag.Qtag_Weight = entity.Qtag_Weight;
                tag.Qtag_IsDeleted = entity.Qtag_IsDeleted;
                tag.Org_ID = entity.Org_ID;
                tag.Cou_ID = entity.Cou_ID;
                Business.Do<IExamQues>().TagAdd(tag);
                count++;
            }
            return count;
        }
        /// <summary>
        /// 设置关键字与试题的关联
        /// </summary>
        /// <param name="tags">关键字，多个关键字逗号或空格个</param>
        /// <param name="quesid">试题id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public int TagConnectionQues(string tags, long quesid, long couid)
        {
            int count = 0;
            tags = tags.Replace(",", " ").Replace("，", " ");
            //当前机构
            Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int orgid = org != null ? org.Org_ID : 0;
            foreach (string name in tags.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Song.Entities.QuesTags tag = Business.Do<IExamQues>().TagSingle(name, orgid, couid);
                if (tag == null) tag = new QuesTags()
                {
                    Qtag_Name = name,
                    Qtag_Weight = 0,
                };
                tag.Qtag_IsDeleted = false;
                Business.Do<IExamQues>().TagAdd(tag);
                count++;
                //关联
                long tagid = tag.Qtag_ID;
            }
            return count;
        }
        /// <summary>
        /// 修改试题关键字
        /// </summary>
        /// <param name="entity">试题关键字的实体</param>
        /// <returns></returns>
        [Admin]
        [HttpPost]
        public Song.Entities.QuesTags TagModify(Song.Entities.QuesTags entity)
        {
            Song.Entities.QuesTags old = Business.Do<IExamQues>().TagSingle(entity.Qtag_ID);
            if (old == null) throw new Exception("Not found entity for QuesTags！");
            try
            {
                old.Copy<Song.Entities.QuesTags>(entity);
                Business.Do<IExamQues>().TagSave(old);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 逻辑删除试题关键字
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int TagDelete(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().TagDelete(s);
            return i;
        }
        /// <summary>
        /// 还原逻辑删除试题关键字
        /// </summary>
        [Admin]
        [HttpPost, HttpGet(Ignore = true)]
        public int TagRecycle(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().TagRecycle(s);
            return i;
        }

        /// <summary>
        /// 删除试题关键字
        /// </summary>
        /// <param name="id">试题知识点id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin]
        [HttpDelete, HttpGet(Ignore = true)]
        public int TagRemove(string id)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(id)) return i;
            List<long> list = id.ToList<long>();
            foreach (long s in list)
                i += Business.Do<IExamQues>().TagRemove(s);
            return i;
        }
        /// <summary>
        /// 分页获取试题关键字
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid">课程id</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="name">按名称检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult TagPager(int orgid, long couid, bool? isdeleted, string name, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count;
            List<Song.Entities.QuesTags> arr = Business.Do<IExamQues>().TagPager(orgid, couid, isdeleted, name, size, index, out count);
            ListResult result = new ListResult(arr);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取试题关联的关键字
        /// </summary>
        /// <param name="quesid"></param>
        /// <returns></returns>
        public List<Song.Entities.QuesTags> TagForQues(long quesid)
        {
            return Business.Do<IExamQues>().TagForQues(quesid);
        }
        /// <summary>
        /// 获取试题关键字的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签的id，多个id用逗号分隔</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <param name="use">是否启用</param>
        /// <returns></returns>
        public int TagQusTotal(string qtagid, long couid, int qtype, bool? use)
            => Business.Do<IExamQues>().TagQusTotal(qtagid.ToArray<long>(), couid, qtype, use);
        #endregion

        #region 试题导入导出
        /// <summary>
        /// 试题导入
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="xls">服务器端的excel文件名，即上传后的excel文件名</param>
        /// <param name="sheet">工作表的名称</param>
        /// <param name="config">配置文件，完整虚拟路径名</param>
        /// <param name="matching">excel列与字段的匹配关联</param>
        /// <param name="type">试题类型</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        /// <returns>success:成功数;error:失败数</returns>
        [HttpPost]
        public JObject ExcelImport(int orgid, string xls, int sheet, string config, JArray matching,
            int type, QuesPart[] parts, QuesKnowledge[] knls)
        {
            //获取Excel中的数据
            string excel = WeiSha.Core.Server.MapPath(xls);
            DataTable dt = ViewData.Helper.Excel.SheetToDatatable(excel, sheet, config);

            //通过反射调用导入试题的方法
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("Song.ViewData");
            Type impot = assembly.GetType("Song.ViewData.QuestionHandler.ExamQuesImport");
            string func_name = "Type" + type;   //导入试题的方法名           
            //return null;

            //开始导入，并计数
            int success = 0, error = 0;
            List<DataRow> errorDataRow = new List<DataRow>();
            List<Exception> errorOjb = new List<Exception>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //throw new Exception();
                    //将数据逐行导入数据库
                    object[] objs = new object[] { excel, dt.Rows[i], matching, type, orgid, parts.ToList(), knls.ToList(), new List<QuesTags>() };
                    impot.InvokeMember(func_name, System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                        null, null, objs);

                    success++;
                }
                catch (Exception ex)
                {
                    //如果出错，将错误行计数
                    error++;
                    errorDataRow.Add(dt.Rows[i]);
                    errorOjb.Add(ex);
                }
            }
            new Task(() =>
            {
                //刷新试题分类、标签、知识点下的试题数
                Business.Do<IExamQues>().PartQusTotalUpdateAll(orgid);
                Business.Do<IExamQues>().TagQusTotalUpdateAll(orgid);
                Business.Do<IExamQues>().KnlQusTotalUpdateAll(orgid);
            }).Start();


            JObject jo = new JObject();
            jo.Add("success", success);
            jo.Add("error", error);
            //错误数据
            JArray jarr = new JArray();
            for (int i = 0; i < errorDataRow.Count; i++)
            {
                DataRow dr = errorDataRow[i];
                JObject jrow = new JObject();
                foreach (DataColumn dc in dr.Table.Columns)
                {
                    jrow.Add(dc.ColumnName, dr[dc.ColumnName].ToString());
                }
                jrow.Add("exception", errorOjb[i].Message);
                jarr.Add(jrow);
            }
            jo.Add("datas", jarr);
            return jo;
        }
        /// <summary>
        /// 导出试题
        /// </summary>
        /// <param name="subpath">导出文件的路径，相对临时路径的子路径</param>
        /// <param name="orgid">机构id</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="use">是否启用</param>
        /// <param name="error">是否错误</param>
        /// <param name="wrong">是否有回馈问题</param>
        /// <returns></returns>
        public JObject ExcelExport(string subpath, int orgid, int[] types, string qpid, string tagid, string knlid, bool? isdeleted, int[] diffs, bool? use, bool? error, bool? wrong)
        {
            return Business.Do<IExamQues>().QuesExportExcel(subpath, orgid, types,
                qpid.ToArray<long>(),
                tagid.ToArray<long>(),
                knlid.ToArray<long>(), isdeleted, diffs, use, error, wrong);
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <param name="subpath">导出文件的路径，相对临时路径的子路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ExcelDelete(int orgid, string filename, string subpath)
        {
            return Song.ViewData.Helper.Excel.DeleteFile(filename, subpath + "/" + orgid.ToString(), "Temp");
        }
        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="subpath">导出文件的路径，相对临时路径的子路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ExcelDeleteAll(string subpath, int orgid)
        {
            return Song.ViewData.Helper.Excel.DeleteDirectory(subpath + "\\" + orgid.ToString(), "Temp");
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <param name="subpath">导出文件的路径，相对临时路径的子路径</param>
        /// <param name="orgid">机构id</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(string subpath, int orgid)
        {
            string rootpath = Path.Combine(WeiSha.Core.Upload.Get["Temp"].Physics, subpath, orgid.ToString());
            JArray jarr = new JArray();
            if (!System.IO.Directory.Exists(rootpath)) return jarr;
            if (orgid < 1) return jarr;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            //if (couid == "0" || string.IsNullOrEmpty(couid)) couid = "*";
            string[] patterns = new[] { "*.xlsx","*.xls", "*.zip" };
            List<FileInfo> files = new List<FileInfo>();
            foreach (var pattern in patterns)
                files = files.Concat(dir.GetFiles(pattern)).GroupBy(t => t.Name).Select(g => g.First()).ToList();           
            files = files.Where(f => (f.Attributes & (FileAttributes.Hidden | FileAttributes.System)) == 0)
                .OrderByDescending(f => f.CreationTime).ToList<FileInfo>();
            foreach (FileInfo f in files)
            {
                JObject jo = new JObject();
                jo.Add("name", Path.GetFileNameWithoutExtension(f.Name).Replace("." + orgid, ""));
                jo.Add("file", f.Name);
                jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + subpath + "/" + orgid.ToString() + "/" + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("type", Path.GetExtension(f.Name).TrimStart('.'));
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion
    }
}
