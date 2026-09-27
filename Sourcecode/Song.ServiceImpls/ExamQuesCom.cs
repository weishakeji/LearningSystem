using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Song.Entities;
using Song.ServiceInterfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using WeiSha.Core;
using WeiSha.Data;

namespace Song.ServiceImpls
{
    public class ExamQuesCom : IExamQues
    {
        #region 试题
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public Questions QuesSingle(long id)
        {
            return Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
        }
        /// <summary>
        /// 试题是否已经存在
        /// </summary>
        /// <param name="qus"></param>
        /// <returns></returns>
        public Questions QuesIsExist(Questions qus)
        {
            WhereClip wc = Questions._.Org_ID == qus.Org_ID && Questions._.Qus_Purpose == qus.Qus_Purpose;
            wc.And(Questions._.Qus_ID != qus.Qus_ID);           

            //单选题和多选题
            if (qus.Qus_Type == 1 || qus.Qus_Type == 2 || qus.Qus_Type == 5)
            {
                List<Questions> list = Gateway.Default.From<Questions>().Where(wc && Questions._.Qus_Title == qus.Qus_Title).ToList<Questions>();
                if (list == null || list.Count < 1) return null;
                //判断选项是否一致
                List<QuesAnswer> answerlist = Business.Do<IQuestions>().QuestionsAnswer(qus, null);
                string answer = string.Join("", answerlist.Select(a => a.Ans_Context));
                foreach (Questions tm in list)
                {
                    List<QuesAnswer> tmlist = Business.Do<IQuestions>().QuestionsAnswer(tm, null);
                    if (string.Join("", tmlist.Select(a => a.Ans_Context)) == answer) return tm;
                }
            }
            else return Gateway.Default.From<Questions>().Where(wc && Questions._.Qus_Title == qus.Qus_Title).ToFirst<Questions>();
            return null;
        }
        /// <summary>
        /// 可用的试题，用于出卷时抽题，所谓可用，即存在，use为true,deleted为false,没有错误
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Questions QuesAvailable(long id)
        {
            //用于考试的试题
            WhereClip wc = Questions._.Qus_Purpose == 1 && Questions._.Qus_IsDeleted == false;
            wc.And(Questions._.Qus_IsUse == true);
            wc &= Questions._.Qus_IsError == false;
            wc &= Questions._.Qus_IsWrong == false;
            return Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id && wc).ToFirst<Questions>();
        }
        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        public long QuesAdd(Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls, bool isFreshcount = true)
        {
            if (entity.Qus_ID <= 0) entity.Qus_ID = WeiSha.Core.Request.SnowID();
            entity.Qus_CrtTime = DateTime.Now;
            entity.Qus_LastTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            if (string.IsNullOrWhiteSpace(entity.Qus_UID)) entity.Qus_UID = WeiSha.Core.Request.UniqueID();
            entity.Qus_Purpose = 1;
            Gateway.Default.Save<Questions>(entity);
            //保存关键字的关联
            this.TagConnectionQues(tags, entity);
            //保存分类的关联
            this.PartConnectionQues(parts, entity.Qus_ID);
            //保存知识点的关联
            this.KnlConnectionQues(knls, entity.Qus_ID);

            //更新试题分类、标签、知识点的题量统计
            if (isFreshcount)
            {
                this.PartQusTotalUpdate(entity);
                this.TagQusTotalUpdate(entity);
                this.KnlQusTotalUpdate(entity);
            }
            return entity.Qus_ID;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">要修改的试题</param>
        /// <param name="tags">试题关键字</param>
        /// <param name="parts">试题分类</param>
        /// <param name="knls">知识点</param>
        public void QuesSave(Questions entity, QuesPart[] parts, QuesTags[] tags, QuesKnowledge[] knls, bool isFreshcount = true)
        {
            entity.Qus_LastTime = DateTime.Now;
            entity.Qus_IsError = false;
            //获取科目名称
            if (entity.Sbj_ID > 0 && string.IsNullOrWhiteSpace(entity.Sbj_Name))
            {
                Subject sbj = Business.Do<ISubject>().SubjectSingle(entity.Sbj_ID);
                if (sbj != null) entity.Sbj_Name = sbj.Sbj_Name;
            }
            if (entity.Qus_Type == 4)
            {
                if (string.IsNullOrWhiteSpace(entity.Qus_Answer) || string.IsNullOrWhiteSpace(entity.Qus_Answer))
                {
                    entity.Qus_IsError = true;
                    entity.Qus_ErrorInfo = "答案不得为空";
                }
            }           
            //保存
            entity.Qus_Purpose = 1;
            Gateway.Default.Save<Questions>(entity);

            //保存关键字的关联
            this.TagConnectionQues(tags, entity);
            //保存分类的关联
            this.PartConnectionQues(parts, entity.Qus_ID);
            //保存知识点的关联
            this.KnlConnectionQues(knls, entity.Qus_ID);

            //更新试题分类、标签、知识点的题量统计
            if (isFreshcount)
            {
                this.PartQusTotalUpdate(entity);
                this.TagQusTotalUpdate(entity);

                Questions former = this.QuesSingle(entity.Qus_ID);  //获取之前的试题信息
                this.KnlQusTotalUpdate(entity, former);
            }
        }

        public void QuesInput(Questions entity, List<QuesAnswer> ansItem, List<QuesPart> parts, List<QuesTags> tags, List<QuesKnowledge> knls)
        {
            entity.Qus_Purpose = 1;     //标注为考试专用
            //答题选项的处理
            if (ansItem != null)
            {
                //如果有试题id，则加上，好像也无所谓
                if (entity.Qus_ID > 0)
                {
                    for (int i = 0; i < ansItem.Count; i++)
                        ansItem[i].Qus_ID = entity.Qus_ID;
                }
                entity.Qus_Items = Business.Do<IQuestions>().AnswerToItems(ansItem);
            }
            //判断是否存在
            Questions old = null;
            if (entity.Qus_ID > 0) old = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (old == null) old = this.QuesIsExist(entity); 
            if (old == null)
            {
                this.QuesAdd(entity, parts.ToArray(), tags.ToArray(), knls.ToArray(), false);
            }
            else
            {
                old.Copy<Song.Entities.Questions>(entity, "Qus_ID,Qus_Items");
                this.QuesSave(old, parts.ToArray(), tags.ToArray(), knls.ToArray(),false);
            }          
        }
        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="entity">试题实体</param>
        public int QuesDelete(Questions entity)
        {
            int count = Gateway.Default.Update<Questions>(new Field[] { Questions._.Qus_IsDeleted, Questions._.Qus_DeleteTime }, new object[] { true, DateTime.Now }, Questions._.Qus_ID == entity.Qus_ID);
            this.PartQusTotalUpdate(entity);
            return count;
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int QuesDelete(long id)
        {
            Song.Entities.Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
            return this.QuesDelete(qus);
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int QuesRecycle(long id)
        {
            Song.Entities.Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
            this.PartQusTotalUpdate(qus);
            return Gateway.Default.Update<Questions>(Questions._.Qus_IsDeleted, false, Questions._.Qus_ID == id);
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int QuesRemove(long id)
        {
            if (this.QuesExistTestpaper(id))
            {
                throw new Exception("当前试题已经被试卷采用，不可删除");
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    //删除笔记
                    Gateway.Default.Delete<Student_Notes>(Student_Notes._.Qus_ID == id);
                    //删除收藏记录
                    Gateway.Default.Delete<Student_Collect>(Student_Collect._.Qus_ID == id);
                    Gateway.Default.Delete<QuesCollect>(QuesCollect._.Qus_ID == id);
                    //删除错题记录
                    Gateway.Default.Delete<Student_Ques>(Student_Ques._.Qus_ID == id);

                    //删除关联记录
                    Gateway.Default.Delete<Questions_QKnl>(Questions_QKnl._.Qus_ID == id);
                    Gateway.Default.Delete<Questions_QPart>(Questions_QPart._.Qus_ID == id);
                    Gateway.Default.Delete<Questions_QTags>(Questions_QTags._.Qus_ID == id);
                    //
                    //删除试题
                    int n = tran.Delete<Questions>(Questions._.Qus_ID == id);
                    //删除图片等资源
                    WeiSha.Core.Upload.Get["Ques"].DeleteDirectory(id.ToString());
                    tran.Commit();
                    return n;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 试题是否存在于试卷
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        public bool QuesExistTestpaper(long qid)
        {
            bool isexist = false;
           using (SourceReader reader = Gateway.Default.From<ExamTestPaper>().Where(ExamTestPaper._.Etp_Type==1).ToReader())
            {
                while (reader.Read())
                {
                    string config = reader["Etp_FromConfig"].ToString();
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(config);
                    foreach (XmlNode node in xmldoc.SelectNodes("//q"))
                    {
                        string qidstr = node.Attributes["id"]?.Value;
                        long quesid = qidstr.Convert<long>();
                        if (qid == quesid) return true;
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return isexist;
        }
        /// <summary>
        /// 获取随机试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param> 
        /// <param name="type">试题类型</param>
        /// <param name="diff1">难度范围</param>
        /// <param name="diff2">难度范围</param>
        /// <param name="isUse">是否允许</param>
        /// <param name="count">取的数量</param>
        /// <returns></returns>
        public List<Questions> QuesRandom(int orgid, long[] qpid, long[] tagid, long[] knlid, int type, int diff1, int diff2, bool? isUse, int count)
        {  
            //用于考试的试题
            WhereClip wc = Questions._.Qus_Purpose == 1 && Questions._.Qus_IsDeleted == false;  
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);

            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            //题型
            //试题类型
            string[] types = Business.Do<IQuestions>().QuestionTypes();
            if (type < 1 || type > types.Length) type = -1;
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            //难度 
            diff1 = diff1 < 1 ? 1 : diff1;
            diff2 = diff2 < 1 || diff2 > 5 ? 5 : diff2;
            if (diff1 > 0) wc.And(Questions._.Qus_Diff >= diff1);  //最小难度等级
            if (diff2 > 0) wc.And(Questions._.Qus_Diff <= diff2);  //最大难度
            FromSection<Questions> section = Gateway.Default.From<Questions>();
            //试题范围
            WhereClip wcrange = new WhereClip();
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.PartTreeID(qpid, orgid);
                foreach (long d in list) wcqp |= Questions_QPart._.Qp_ID == d;
                wcrange.Or(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wcrange.Or(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.KnlTreeID(knlid, orgid);
                foreach (long k in list) wcqp |= Questions_QKnl._.Qk_ID == k;
                wcrange.Or(wcqp);
            }
            wc.And(wcrange);
            //随机排序
            OrderByClip order;
            if (Gateway.Default.DbType != DbProviderType.SQLServer) order = new OrderByClip("RANDOM()");
            else order = new OrderByClip("NEWID()");
            return section.Where(wc).OrderBy(order).ToList<Questions>(count);
        }
        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="search"></param>
        /// <param name="isdeleted"></param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点</param>
        /// <param name="type"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<Questions> QuesPager(int orgid, string search, bool? isdeleted, long[] qpid, long[] tagid, long[] knlid,
            int[] type, int[] diff, bool? isUse, bool? isError, bool? isWrong,
            int size, int index, out int countSum)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (isdeleted != null) wc.And(Questions._.Qus_IsDeleted == isdeleted);    //删除状态
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            //按题干进行检索
            if (!string.IsNullOrWhiteSpace(search)) wc.And(Questions._.Qus_Title.Contains(search));
            //题型
            if (type != null && type.Length > 0)
            {
                WhereClip wctype = new WhereClip();
                foreach (int t in type) wctype |= Questions._.Qus_Type == t;
                wc.And(wctype);
            }
            //难度  
            if (diff != null && diff.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diff) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>();
            //试题范围
            WhereClip wcrange = new WhereClip();
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.PartTreeID(qpid, orgid);
                foreach (long d in list) wcqp |= Questions_QPart._.Qp_ID == d;
                wcrange.Or(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wcrange.Or(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.KnlTreeID(knlid, orgid);
                foreach (long k in list) wcqp |= Questions_QKnl._.Qk_ID == k;
                wcrange.Or(wcqp);
            }
            wc.And(wcrange);
            countSum = section.Where(wc).Count();
            OrderByClip orderBy=new OrderByClip();
            if (isdeleted != null && isdeleted == true) orderBy &= Questions._.Qus_DeleteTime.Desc;
            orderBy &= Questions._.Qus_Order.Asc;
            return section.Where(wc).OrderBy(orderBy & Questions._.Qus_ID.Desc).ToList<Questions>(size, (index - 1) * size);
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        /// <param name="ques">要更新的试题对象</param>
        /// <param name="former">原来的试题对象</param>
        public void QuesStatisticalUpdate(Questions ques, Questions former)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;
            wc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
        }
        /// <summary>
        /// 获取试题数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="qpid"></param>
        /// <param name="tagid"></param>
        /// <param name="knlid"></param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>
        /// <returns>试题类型，数量</returns>
        public Dictionary<int, int> QuesTotal(int orgid, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diff, bool? isUse, bool? isError, bool? isWrong)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (isdeleted != null) wc.And(Questions._.Qus_IsDeleted == isdeleted);    //删除状态
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            //难度  
            if (diff != null && diff.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diff) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>();
            //试题范围
            WhereClip wcrange = new WhereClip();
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.PartTreeID(qpid, orgid);
                foreach (long d in list) wcqp |= Questions_QPart._.Qp_ID == d;
                wcrange.Or(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wcrange.Or(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.KnlTreeID(knlid, orgid);
                foreach (long k in list) wcqp |= Questions_QKnl._.Qk_ID == k;
                wcrange.Or(wcqp);
            }
            wc.And(wcrange);
            //试题类型
            string[] types = Business.Do<IQuestions>().QuestionTypes();
            Dictionary<int, int> dic = new Dictionary<int, int>();
            for (int i = 0; i < types.Length; i++)
            {
                //section.Where(wc && Questions._.Qus_Type == (i + 1));
                int total = section.Where(wc && Questions._.Qus_Type == (i + 1)).SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
                dic.Add(i + 1, total);
            }
            return dic;
        }
        /// <summary>
        /// 获取试题数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否错误</param>
        /// <param name="isWrong">是否有回馈问题</param>
        /// <returns></returns>
        public int Total(int orgid, int[] types, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diffs, bool? isUse, bool? isError, bool? isWrong)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (isdeleted != null) wc.And(Questions._.Qus_IsDeleted == isdeleted);    //删除状态
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            //题型  
            if (types != null && types.Length > 0)
            {
                WhereClip wctype = new WhereClip();
                foreach (int d in types) wctype |= Questions._.Qus_Type == d;
                wc.And(wctype);
            }
            //难度  
            if (diffs != null && diffs.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diffs) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>();
            //试题范围
            WhereClip wcrange = new WhereClip();
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.PartTreeID(qpid, orgid);
                foreach (long d in list) wcqp |= Questions_QPart._.Qp_ID == d;
                wcrange.Or(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wcrange.Or(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                List<long> list = this.KnlTreeID(knlid, orgid);
                foreach (long k in list) wcqp |= Questions_QKnl._.Qk_ID == k;
                wcrange.Or(wcqp);
            }
            wc.And(wcrange);
            return section.Where(wc).Count();           
        }
        #endregion

        #region 试题分类
        /// <summary>
        /// 添加试题分类
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int PartAdd(QuesPart entity)
        {
            entity.Qp_CrtTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            QuesPart existpart = this.PartIsExist(entity.Org_ID, entity.Qp_PID, entity.Qp_Name);
            if (existpart != null) throw new Exception("当前层级下已经存在该分类名称！");
            //如果没有排序号，则自动计算
            if (entity.Qp_Order < 1)
            {
                object obj = Gateway.Default.Max<QuesPart>(QuesPart._.Qp_Order, QuesPart._.Org_ID == entity.Org_ID && QuesPart._.Qp_PID == entity.Qp_PID);
                entity.Qp_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            return Gateway.Default.Save<QuesPart>(entity);
        }
        /// <summary>
        /// 批量添加专业，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">专业名称，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        public QuesPart PartBatchAdd(int orgid, string names)
        {
            //整理名称信息
            names = names.Replace("，", ",");
            List<string> listName = new List<string>();
            foreach (string str in names.Split(','))
            {
                string s = str.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                if (s.Trim() != "") listName.Add(s.Trim());
            }
            //
            long pid = 0;
            Song.Entities.QuesPart last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.QuesPart current = PartIsExist(orgid, pid, listName[i]);
                if (current == null)
                {
                    current = new QuesPart();
                    current.Qp_Name = listName[i];
                    current.Qp_IsUse = true;
                    current.Org_ID = orgid;
                    current.Qp_PID = pid;
                    this.PartAdd(current);
                }
                last = current;
                pid = current.Qp_ID;
            }
            return last;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public QuesPart PartIsExist(int orgid, long pid, string name)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= QuesPart._.Org_ID == orgid;
            if (pid >= 0) wc &= QuesPart._.Qp_PID == pid;
            return Gateway.Default.From<QuesPart>().Where(wc && QuesPart._.Qp_Name == name.Trim()).ToFirst<QuesPart>();
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuesPart PartIsExist(QuesPart entity)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesPart._.Org_ID == entity.Org_ID;
            if (entity.Qp_PID >= 0) wc &= QuesPart._.Qp_PID == entity.Qp_PID;
            if (entity.Qp_ID > 0) wc &= QuesPart._.Qp_ID != entity.Qp_ID;
            return Gateway.Default.From<QuesPart>().Where(wc && QuesPart._.Qp_Name == entity.Qp_Name.Trim()).ToFirst<QuesPart>();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public QuesPart PartSave(QuesPart entity)
        {
            //分类的id与pid不能相等
            if (entity.Qp_PID == entity.Qp_ID) throw new Exception("QuesPart table PID Can not be equal to ID");
            QuesPart existpart = this.PartIsExist(entity);
            if (existpart != null) throw new Exception("当前层级下已经存在该分类名称！");
            QuesPart old = this.PartSingle(entity.Qp_ID);
            if (old.Qp_PID != entity.Qp_PID)
            {
                object obj = Gateway.Default.Max<QuesPart>(QuesPart._.Qp_Order, QuesPart._.Org_ID == entity.Org_ID && QuesPart._.Qp_PID == entity.Qp_PID);
                entity.Qp_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            entity.Qp_UpdateTime = DateTime.Now;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesPart>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return entity;
        }
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int PartUpdate(long qpid, Field[] fields, object[] objs)
        {
            return Gateway.Default.Update<QuesPart>(fields, objs, QuesPart._.Qp_ID == qpid);
        }
        /// <summary>
        /// 修改试题分类的某些项
        /// </summary>
        /// <param name="qpid">试题分类id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int PartUpdate(long qpid, Field field, object obj)
        {
            return Gateway.Default.Update<QuesPart>(field, obj, QuesPart._.Qp_ID == qpid);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int PartDelete(long id)
        {
            List<long> list = this.PartTreeID(id, 0);   //获取当前试题分类下的所有子试题分类id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qpid in list)
                        tran.Update<QuesPart>(new Field[] { QuesPart._.Qp_IsDeleted, QuesPart._.Qp_DeleteTime }, new object[] { true, DateTime.Now }, QuesPart._.Qp_ID == qpid);

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int PartRemove(long id)
        {
            List<long> list = this.PartTreeID(id, 0);   //获取当前试题分类下的所有子试题分类id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qpid in list)
                    {
                        tran.Delete<Questions_QPart>(Questions_QPart._.Qp_ID == qpid);
                        tran.Delete<QuesPart>(QuesPart._.Qp_ID == qpid);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int PartRecycle(long id)
        {
            return Gateway.Default.Update<QuesPart>(QuesPart._.Qp_IsDeleted, false, QuesPart._.Qp_ID == id);
        }
        /// <summary>
        /// 清空试题分类下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qpid"></param>
        public void PartClear(long qpid)
        {
            Gateway.Default.Delete<Questions_QPart>(Questions_QPart._.Qp_ID == qpid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public QuesPart PartSingle(long id)
        {
            return Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == id).ToFirst<QuesPart>();
        }
        /// <summary>
        /// 按主键获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesPart> PartSingle(long[] id)
        {
            if (id.Length == 0) return null;
            WhereClip wc=new WhereClip();
            for (int i = 0; i < id.Length; i++)
                wc.Or(QuesPart._.Qp_ID == id[i]);
            return Gateway.Default.From<QuesPart>().Where(wc).ToList<QuesPart>();
        }
        /// <summary>
        /// 当前试题分类下的所有子试题分类id，包括自身
        /// </summary>
        /// <param name="qpid">当前试题分类id</param>
        /// <param name="orgid">试题分类所属机构的ID,如果小于等于零，则取从数据库读取qpid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        public List<long> PartTreeID(long qpid, int orgid)
        {
            List<long> list = new List<long>();
            //如果未设置机构id，则取当前专业所属机构
            if (orgid <= 0)
            {
                QuesPart part = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == qpid).ToFirst<QuesPart>();
                if (part == null) return list;
                orgid = part.Org_ID;
            }
            //取同一个机构下的所有专业
            List<QuesPart> sbjs = Gateway.Default.From<QuesPart>().Where(QuesPart._.Org_ID == orgid).ToList<QuesPart>();
            list = _parttreeid(qpid, sbjs);
            return list;
        }
        /// <summary>
        /// 当前试题分类下的所有子试题分类id
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public List<long> PartTreeID(long[] qpid, int orgid)
        {
            List<long> list = new List<long>();
            foreach (long qid in qpid)
            {
                List<long> tm = this.PartTreeID(qid, orgid);
                foreach (long t in tm) if (!list.Contains(t)) list.Add(t);
            }
            return list;
        }
        private List<long> _parttreeid(long id, List<QuesPart> parts)
        {
            List<long> list = new List<long>();
            if (id > 0) list.Add(id);
            List<long> childs = parts.Where(s => s.Qp_PID == id).Select(s => s.Qp_ID).ToList();
            parts.RemoveAll(s => s.Qp_PID == id);
            for (int i = 0; i < childs.Count; i++)
            {
                list.Add(childs[i]);
                List<long> tm = _parttreeid(childs[i], parts);
                list.AddRange(tm.Except(list));
            }
            return list;
        }
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="partid">试题分类的id</param>
        /// <returns></returns>
        public string PartName(long partid)
        {
            QuesPart entity = null;
            string xpath = string.Empty;
            do
            {
                entity = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == partid)
                    .Select(new Field[] { QuesPart._.Qp_ID, QuesPart._.Qp_PID, QuesPart._.Qp_Name }).ToFirst<QuesPart>();
                if (entity != null)
                {
                    if (string.IsNullOrWhiteSpace(xpath)) xpath = entity.Qp_Name;
                    else xpath = entity.Qp_Name + "," + xpath;
                    partid = entity.Qp_PID;
                }
            } while (entity != null && partid > 0);
            return xpath;
        }
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        public string PartName(long[] partid)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < partid.Length; i++)
            {
                string name = this.PartName(partid[i]);
                if (!string.IsNullOrWhiteSpace(name)) list.Add(name);
            }
            return string.Join(";", list);
        }
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="qid">试题的id</param>
        /// <returns></returns>
        public string QuesPartName(long qid)
        {
            List<QuesPart> list = this.PartForQues(qid);
            if (list == null) return string.Empty;
            long[] idarray = list.Select(q => q.Qp_ID).ToArray();
            return this.PartName(idarray);
        }
        /// <summary>
        /// 当前试题分类，是否有子试题分类
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题分类Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        public bool PartIsChildren(int orgid, long id, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(QuesPart._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<QuesPart>(wc && QuesPart._.Qp_PID == id);
            return count > 0;
        }
        /// <summary>
        /// 获取试题分类
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QuesPart> PartCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesPart._.Qp_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(QuesPart._.Qp_Name.Contains(sear));
            if (pid >= 0) wc.And(QuesPart._.Qp_PID == pid);
            return Gateway.Default.From<QuesPart>().Where(wc).OrderBy(QuesPart._.Qp_Order.Asc && QuesPart._.Qp_ID.Asc).ToList<QuesPart>(count);
        }
        /// <summary>
        /// 当前试题分类的上级父级
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesPart> PartParents(long qpid, bool isself)
        {
            Stack st = new Stack();
            QuesPart s = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == qpid).ToFirst<QuesPart>();
            if (isself) st.Push(s);
            while (s.Qp_PID != 0)
            {
                s = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == s.Qp_PID).ToFirst<QuesPart>();
                if (s == null) break;
                st.Push(s);
            }
            List<QuesPart> list = new List<QuesPart>();
            foreach (object obj in st)
            {
                list.Add((QuesPart)obj);
            }
            return list;
        }
        /// <summary>
        /// 计算试题分类数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        public int PartOfCount(int orgid, long pid, bool? isUse, bool children)
        {
            if (pid < 0)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
                return Gateway.Default.Count<QuesPart>(wc);
            }
            //不包括子级，仅当前专业的直接下级专业
            if (!children)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
                if (pid >= 0) wc.And(QuesPart._.Qp_PID == pid);
                return Gateway.Default.Count<QuesPart>(wc);
            }
            else
            {
                //包括子级，当前专业下的所有专业数
                List<long> list = new List<long>();
                //取同一个机构下的所有章节
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
                List<QuesPart> parts = Gateway.Default.From<QuesPart>().Where(wc).ToList<QuesPart>();
                list = _parttreeid(pid, parts);
                return list.Count;
            }
        }
        /// <summary>
        /// 当前试题分类下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Questions> PartQuestions(int orgid, long qpid, int qtype, bool? isUse, bool children, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            List<long> listqpid = children ? new List<long>() { qpid } : this.PartTreeID(qpid, orgid);
            WhereClip wc2 = new WhereClip();
            foreach (long l in listqpid) wc2.Or(Questions_QPart._.Qp_ID == l);
            wc.And(wc2);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.ToList<Questions>(count);

        }
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int PartQusTotal(int orgid, long qpid, int qtype, bool? isUse, bool children)
        {
            return PartQusTotal(orgid, new long[] { qpid }, qtype, isUse, children);
        }
        /// <summary>
        /// 获取试题分类的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int PartQusTotal(int orgid, long[] qpid, int qtype, bool? isUse, bool children)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            //计算所有子级
            if (qpid != null)
            {
                List<long> listqpid = new List<long>();
                if (!children) listqpid = qpid.ToList();
                else
                {
                    foreach (long id in qpid)
                    {
                        List<long> list = this.PartTreeID(id, orgid);
                        foreach (long l in list)
                        {
                            if (!listqpid.Contains(l)) listqpid.Add(l);
                        }
                    }
                }
                WhereClip wc2 = new WhereClip();
                foreach (long l in listqpid) wc2.Or(Questions_QPart._.Qp_ID == l);
                wc.And(wc2);
            }

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        public void PartQusTotalUpdate(List<QuesPart> parts)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //当前试题关联的分类
            foreach (QuesPart part in parts)
            {
                //分类的试题数
                int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QPart._.Qp_ID == part.Qp_ID).Count();
                Gateway.Default.Update<QuesPart>(QuesPart._.QP_Count, partcount, QuesPart._.Qp_ID == part.Qp_ID);
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        public void PartQusTotalUpdate(Questions ques)
        {
            List<QuesPart> list = this.PartForQues(ques.Qus_ID);
            if (list == null || list.Count <= 0) return;
            this.PartQusTotalUpdate(list);
        }
        /// <summary>
        /// 更新试题分类所有试题数量
        /// </summary>
        public void PartQusTotalUpdate()
        {
            List<QuesPart> list = Gateway.Default.From<QuesPart>().ToList<QuesPart>();
            if (list == null || list.Count <= 0) return;
            this.PartQusTotalUpdate(list);
        }
        /// <summary>
        /// 试题所属的分类，由于是多对多关联，试题可能会属于多个分类
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        public List<QuesPart> PartForQues(long quesid)
        {
            return Gateway.Default.From<QuesPart>().LeftJoin<Questions_QPart>(Questions_QPart._.Qp_ID == QuesPart._.Qp_ID).Where(Questions_QPart._.Qus_ID == quesid).ToList<QuesPart>();
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<QuesPart> PartPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesPart._.Org_ID == orgid);
            if (pid >= 0) wc.And(QuesPart._.Qp_PID == pid);
            if (isUse != null) wc.And(QuesPart._.Qp_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesPart._.Qp_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(QuesPart._.Qp_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<QuesPart>(wc);
            OrderByClip orderBy = new OrderByClip();
            if (isdeleted != null && isdeleted == true) orderBy &= QuesPart._.Qp_DeleteTime.Desc;
            orderBy &= QuesPart._.Qp_Order.Asc;

            return Gateway.Default.From<QuesPart>().Where(wc).OrderBy(orderBy).ToList<QuesPart>(size, (index - 1) * size);
        }
        /// <summary>
        /// 更改试题分类的排序
        /// </summary>
        /// <param name="list">试题分类列表，对象中只有Qp_ID、Qp_PID、Qp_Order</param>
        /// <returns></returns>
        public bool PartUpdateTaxis(QuesPart[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.QuesPart part in list)
                    {
                        tran.Update<QuesPart>(
                            new Field[] { QuesPart._.Qp_PID, QuesPart._.Qp_Order },
                            new object[] { part.Qp_PID, part.Qp_Order },
                            QuesPart._.Qp_ID == part.Qp_ID);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        /// <param name="qpid"></param>
        /// <param name="qusid"></param> 
        /// <returns></returns>
        public int PartConnectionQues(long qpid, long qusid)
        {
            if (qpid <= 0 || qusid <= 0) return 0;
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == qusid).ToFirst<Questions>();
            QuesPart[] parts = Gateway.Default.From<QuesPart>().Where(QuesPart._.Qp_ID == qpid).ToArray<QuesPart>();          
            return PartConnectionQues(parts, ques);
        }
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        public int PartConnectionQues(QuesPart[] parts, long qusid)
        {
            if (qusid <= 0) return 0;
            Questions ques = this.QuesSingle(qusid);
            return PartConnectionQues(parts, ques);
        }
        /// <summary>
        /// 创建试题分类与试题的关联
        /// </summary>
        public int PartConnectionQues(QuesPart[] parts, Questions ques)
        {
            //原有试题分类
            List<QuesPart> list = this.PartForQues(ques.Qus_ID);

            //删除原有与试题分类的关联
            Gateway.Default.Delete<Questions_QPart>(Questions_QPart._.Qus_ID == ques.Qus_ID);
            int i = 0;
            foreach (QuesPart part in parts)
            {               
                WhereClip wc = Questions_QPart._.Qus_ID == ques.Qus_ID && Questions_QPart._.Qp_ID == part.Qp_ID;
                Questions_QPart qqpart = Gateway.Default.From<Questions_QPart>().Where(wc).ToFirst<Questions_QPart>();
                if (qqpart != null) return 0;
                qqpart = new Questions_QPart()
                {
                    Qus_ID = ques.Qus_ID,
                    Qp_ID = part.Qp_ID
                };
                i += Gateway.Default.Save<Questions_QPart>(qqpart);
                if (!list.Any(p => p.Qp_ID == part.Qp_ID)) list.Add(part);
            }
            //更新分类的试题数统计
            this.PartQusTotalUpdate(list);
            return i;
        }
        #endregion

        #region 收藏
        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        public bool CollectAdd(int accid, long qusid)
        {
            QuesCollect qc = Gateway.Default.From<QuesCollect>().Where(QuesCollect._.Qus_ID == qusid && QuesCollect._.Acc_ID == accid).ToFirst<QuesCollect>();
            if (qc != null) return false;
            return Gateway.Default.Insert<QuesCollect>(new QuesCollect()
            {
                Acc_ID = accid,
                Qus_ID = qusid,
                Qcl_CrtTime = DateTime.Now
            }) > 0;
        }
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="accid">管理员id</param>
        /// <param name="qusid">试题id</param>
        /// <returns></returns>
        public bool CollectRemove(int accid, long qusid)
        {
            Gateway.Default.Delete<QuesCollect>(QuesCollect._.Qus_ID == qusid && QuesCollect._.Acc_ID == accid);
            return true;
        }
        /// <summary>
        /// 批量取消收藏
        /// </summary>
        public int CollectRemove(int accid, long[] qusid)
        {
            int i = 0;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long l in qusid)
                    {
                        i += tran.Delete<QuesCollect>(QuesCollect._.Qus_ID == l && QuesCollect._.Acc_ID == accid);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return i;
        }
        /// <summary>
        /// 试题是否被收藏
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        public bool Collected(int accid, long qusid)
        {
            if (accid <= 0 || qusid <= 0) return false;
            return Gateway.Default.Count<QuesCollect>(QuesCollect._.Qus_ID == qusid && QuesCollect._.Acc_ID == accid) > 0;
        }
        /// <summary>
        /// 获取收藏的试题
        /// </summary>
        /// <param name="acid">管理员id</param>
        /// <param name="search">搜索题干</param>
        /// <param name="qpid">试题分类id</param>
        /// <param name="tagid">试题标签id</param>
        /// <param name="knlid">试题知识点id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">试题难度</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否有格式错误</param>
        /// <param name="isWrong">是否有反馈的错误</param>
        /// <param name="size">分页大小</param>
        /// <param name="index">分页索引</param>
        /// <param name="countSum">总记录数</param>
        /// <returns></returns>
        public List<Questions> CollectPager(int acid, string search, long[] qpid, long[] tagid, long[] knlid,
            int[] type, int[] diff, bool? isUse, bool? isError, bool? isWrong,
            int size, int index, out int countSum)
        {
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题
            wc.And(QuesCollect._.Acc_ID == acid);
            wc.And(Questions._.Qus_IsDeleted == false);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);

            //按题干进行检索
            if (!string.IsNullOrWhiteSpace(search)) wc.And(Questions._.Qus_Title.Contains(search));
            //题型
            if (type != null && type.Length > 0)
            {
                WhereClip wctype = new WhereClip();
                foreach (int t in type) wctype |= Questions._.Qus_Type == t;
                wc.And(wctype);
            }
            //难度  
            if (diff != null && diff.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diff) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<QuesCollect>(QuesCollect._.Qus_ID == Questions._.Qus_ID);
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long d in qpid) wcqp |= Questions_QPart._.Qp_ID == d;
                wc.And(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wc.And(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long k in knlid) wcqp |= Questions_QKnl._.Qk_ID == k;
                wc.And(wcqp);
            }
            countSum = section.Where(wc).Count();
            return section.Where(wc).OrderBy(QuesCollect._.Qcl_CrtTime.Desc).ToList<Questions>(size, (index - 1) * size);
        }
        #endregion

        #region 试题知识点
        /// <summary>
        /// 添加试题知识点
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int KnlAdd(QuesKnowledge entity)
        {
            entity.Qk_CrtTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            QuesKnowledge knl = this.KnlIsExist(entity.Org_ID, entity.Qk_PID, entity.Qk_Name);
            if (knl != null) throw new Exception("当前层级下的该知识点已存在！");
            //如果没有排序号，则自动计算
            if (entity.Qk_Order < 1)
            {
                object obj = Gateway.Default.Max<QuesKnowledge>(QuesKnowledge._.Qk_Order, QuesKnowledge._.Org_ID == entity.Org_ID && QuesKnowledge._.Qk_PID == entity.Qk_PID);
                entity.Qk_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            return Gateway.Default.Save<QuesKnowledge>(entity);
        }
        /// <summary>
        /// 批量添加知识点，可用于导入时
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="names">知识点，可以是用逗号分隔的多个名称</param>
        /// <returns></returns>
        public QuesKnowledge KnlBatchAdd(int orgid, string names)
        {
            //整理名称信息
            names = names.Replace("，", ",");
            List<string> listName = new List<string>();
            foreach (string str in names.Split(','))
            {
                string s = str.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");
                if (s.Trim() != "") listName.Add(s.Trim());
            }
            //
            long pid = 0;
            Song.Entities.QuesKnowledge last = null;
            for (int i = 0; i < listName.Count; i++)
            {
                Song.Entities.QuesKnowledge current = this.KnlIsExist(orgid, pid, listName[i]);
                if (current == null)
                {
                    current = new QuesKnowledge();
                    current.Qk_Name = listName[i];
                    current.Qk_IsUse = true;
                    current.Org_ID = orgid;
                    current.Qk_PID = pid;
                    this.KnlAdd(current);
                }
                last = current;
                pid = current.Qk_ID;
            }
            return last;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public QuesKnowledge KnlIsExist(int orgid, long pid, string name)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= QuesKnowledge._.Org_ID == orgid;
            if (pid >= 0) wc &= QuesKnowledge._.Qk_PID == pid;
            return Gateway.Default.From<QuesKnowledge>().Where(wc && QuesKnowledge._.Qk_Name == name.Trim()).ToFirst<QuesKnowledge>();
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public QuesKnowledge KnlIsExist(QuesKnowledge entity)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesKnowledge._.Org_ID == entity.Org_ID;
            if (entity.Qk_PID >= 0) wc &= QuesKnowledge._.Qk_PID == entity.Qk_PID;
            if (entity.Qk_ID > 0) wc &= QuesKnowledge._.Qk_ID != entity.Qk_ID;
            return Gateway.Default.From<QuesKnowledge>().Where(wc && QuesKnowledge._.Qk_Name == entity.Qk_Name.Trim()).ToFirst<QuesKnowledge>();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void KnlSave(QuesKnowledge entity)
        {
            //专业的id与pid不能相等
            if (entity.Qk_PID == entity.Qk_ID) throw new Exception("QuesKnowledge table PID Can not be equal to ID");
            QuesKnowledge old = this.KnlSingle(entity.Qk_ID);
            if (old.Qk_PID != entity.Qk_PID)
            {
                object obj = Gateway.Default.Max<QuesKnowledge>(QuesKnowledge._.Qk_Order, QuesKnowledge._.Org_ID == entity.Org_ID && QuesKnowledge._.Qk_PID == entity.Qk_PID);
                entity.Qk_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            QuesKnowledge knl = this.KnlIsExist(entity);
            if (knl != null) throw new Exception("当前层级下的该知识点已存在！");
            entity.Qk_UpdateTime = DateTime.Now;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesKnowledge>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="fields">字段</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int KnlUpdate(long qkid, Field[] fields, object[] objs)
        {
            return Gateway.Default.Update<QuesKnowledge>(fields, objs, QuesKnowledge._.Qk_ID == qkid);
        }
        /// <summary>
        /// 修改试题知识点的某些项
        /// </summary>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="field">字段</param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int KnlUpdate(long qkid, Field field, object obj)
        {
            return Gateway.Default.Update<QuesKnowledge>(field, obj, QuesKnowledge._.Qk_ID == qkid);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int KnlDelete(long id)
        {
            List<long> list = this.KnlTreeID(id, 0);   //获取当前试题知识点下的所有子试题知识点id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qkid in list)
                        tran.Update<QuesKnowledge>(new Field[] { QuesKnowledge._.Qk_IsDeleted, QuesKnowledge._.Qk_DeleteTime }, new object[] { true, DateTime.Now }, QuesKnowledge._.Qk_ID == qkid);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int KnlRemove(long id)
        {
            List<long> list = this.KnlTreeID(id, 0);   //获取当前试题知识点下的所有子试题知识点id，包括自身
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (long qkid in list)
                    {
                        tran.Delete<Questions_QKnl>(Questions_QKnl._.Qk_ID == qkid);
                        tran.Delete<QuesKnowledge>(QuesKnowledge._.Qk_ID == qkid);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return list.Count;
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int KnlRecycle(long id)
        {
            return Gateway.Default.Update<QuesKnowledge>(QuesKnowledge._.Qk_IsDeleted, false, QuesKnowledge._.Qk_ID == id);
        }
        /// <summary>
        /// 清空试题知识点下的所有试题关联关联（并不删除试题）
        /// </summary>
        /// <param name="qkid"></param>
        public void KnlClear(long qkid)
        {
            Gateway.Default.Delete<Questions_QKnl>(Questions_QKnl._.Qk_ID == qkid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public QuesKnowledge KnlSingle(long id)
        {
            return Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == id).ToFirst<QuesKnowledge>();
        }
        /// <summary>
        /// 按ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlSingle(long[] id)
        {
            if (id.Length == 0) return null;
            WhereClip wc=new WhereClip();
            for (int i = 0; i < id.Length; i++)
                wc.Or(QuesKnowledge._.Qk_ID == id[i]);
            return Gateway.Default.From<QuesKnowledge>().Where(wc).ToList<QuesKnowledge>();
        }
        /// <summary>
        /// 当前试题知识点下的所有子试题知识点id，包括自身
        /// </summary>
        /// <param name="qkid">当前试题知识点id</param>
        /// <param name="orgid">试题知识点所属机构的ID,如果小于等于零，则取从数据库读取qkid再取orgid，所以建议正确赋值，可以减少数据库读取次数</param>
        public List<long> KnlTreeID(long qkid, int orgid)
        {
            List<long> list = new List<long>();
            //如果未设置机构id，则取当前专业所属机构
            if (orgid <= 0)
            {
                QuesKnowledge Knl = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == qkid).ToFirst<QuesKnowledge>();
                if (Knl == null) return list;
                orgid = Knl.Org_ID;
            }
            //取同一个机构下的所有专业
            List<QuesKnowledge> sbjs = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Org_ID == orgid).ToList<QuesKnowledge>();
            list = _knltreeid(qkid, sbjs);
            return list;
        }
        /// <summary>
        /// 当前试题知识点下的所有子试题知识点id
        /// </summary>
        public List<long> KnlTreeID(long[] qkid, int orgid)
        {
            List<long> list = new List<long>();
            foreach (long kid in qkid)
            {
                List<long> tm = this.KnlTreeID(kid, orgid);
                foreach (long t in tm) if (!list.Contains(t)) list.Add(t);
            }
            return list;
        }
        private List<long> _knltreeid(long id, List<QuesKnowledge> Knls)
        {
            List<long> list = new List<long>();
            if (id > 0) list.Add(id);
            List<long> childs = Knls.Where(s => s.Qk_PID == id).Select(s => s.Qk_ID).ToList();
            Knls.RemoveAll(s => s.Qk_PID == id);
            for (int i = 0; i < childs.Count; i++)
            {
                list.Add(childs[i]);
                List<long> tm = _knltreeid(childs[i], Knls);
                list.AddRange(tm.Except(list));
            }
            return list;
        }
        /// <summary>
        /// 获取试题知识点名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="id">试题知识点的id</param>
        /// <returns></returns>
        public string KnlName(long id)
        {
            QuesKnowledge entity = null;
            string xpath = string.Empty;
            do
            {
                entity = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == id)
                    .Select(new Field[] { QuesKnowledge._.Qk_ID, QuesKnowledge._.Qk_PID, QuesKnowledge._.Qk_Name }).ToFirst<QuesKnowledge>();
                if (entity != null)
                {
                    if (string.IsNullOrWhiteSpace(xpath)) xpath = entity.Qk_Name;
                    else xpath = entity.Qk_Name + "," + xpath;
                    id = entity.Qk_PID;
                }
            } while (entity != null && id > 0);
            return xpath;
        }
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        public string KnlName(long[] knlid)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < knlid.Length; i++)
            {
                string name = this.KnlName(knlid[i]);
                if (!string.IsNullOrWhiteSpace(name)) list.Add(name);
            }
            return string.Join(";", list);
        }
        /// <summary>
        /// 获取试题分类名称，如果为多级，则带上父级名称
        /// </summary>
        /// <param name="qid">试题的id</param>
        /// <returns></returns>
        public string QuesKnlName(long qid)
        {

            List<QuesKnowledge> list = this.KnlForQues(qid);
            if (list == null) return string.Empty;
            long[] idarray = list.Select(q => q.Qk_ID).ToArray();
            return this.KnlName(idarray);
        }
        /// <summary>
        /// 当前试题知识点，是否有子试题知识点
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="id">当前试题知识点Id</param>
        /// <param name="isUse">是否启用</param>
        /// <returns>有子级，返回true</returns>
        public bool KnlIsChildren(int orgid, long id, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid >= 0) wc.And(QuesKnowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
            int count = Gateway.Default.Count<QuesKnowledge>(wc && QuesKnowledge._.Qk_PID == id);
            return count > 0;
        }
        /// <summary>
        /// 获取试题知识点
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="pid">上级ID</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlCount(int orgid, string sear, bool? isUse, bool? isdeleted, long pid, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
            if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesKnowledge._.Qk_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(QuesKnowledge._.Qk_Name.Contains(sear));
            if (pid >= 0) wc.And(QuesKnowledge._.Qk_PID == pid);
            return Gateway.Default.From<QuesKnowledge>().Where(wc).OrderBy(QuesKnowledge._.Qk_Order.Asc && QuesKnowledge._.Qk_ID.Asc).ToList<QuesKnowledge>(count);
        }
        /// <summary>
        /// 当前试题知识点的上级父级
        /// </summary>
        /// <param name="qkid"></param>
        /// <param name="isself">是否包括自身</param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlParents(long qkid, bool isself)
        {
            Stack st = new Stack();
            QuesKnowledge s = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == qkid).ToFirst<QuesKnowledge>();
            if (isself) st.Push(s);
            while (s.Qk_PID != 0)
            {
                s = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == s.Qk_PID).ToFirst<QuesKnowledge>();
                if (s == null) break;
                st.Push(s);
            }
            List<QuesKnowledge> list = new List<QuesKnowledge>();
            foreach (object obj in st)
            {
                list.Add((QuesKnowledge)obj);
            }
            return list;
        }
        /// <summary>
        /// 计算试题知识点数量
        /// </summary>
        /// <param name="orgid">机构id</param>       
        /// <param name="pid">上级id</param>
        /// <param name="isUse">是否启用的，null取所有</param>
        /// <param name="children">是否包括子级</param>
        /// <returns></returns>
        public int KnlOfCount(int orgid, long pid, bool? isUse, bool children)
        {
            if (pid < 0)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
                return Gateway.Default.Count<QuesKnowledge>(wc);
            }
            //不包括子级，仅当前专业的直接下级专业
            if (!children)
            {
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
                if (pid >= 0) wc.And(QuesKnowledge._.Qk_PID == pid);
                return Gateway.Default.Count<QuesKnowledge>(wc);
            }
            else
            {
                //包括子级，当前专业下的所有专业数
                List<long> list = new List<long>();
                //取同一个机构下的所有章节
                WhereClip wc = new WhereClip();
                if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
                if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
                List<QuesKnowledge> Knls = Gateway.Default.From<QuesKnowledge>().Where(wc).ToList<QuesKnowledge>();
                list = _knltreeid(pid, Knls);
                return list.Count;
            }
        }
        /// <summary>
        /// 当前试题知识点下的所有试题
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="isUse"></param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Questions> KnlQuestions(int orgid, long qkid, int qtype, bool? isUse, bool children, int count)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            List<long> listqkid = children ? new List<long>() { qkid } : this.KnlTreeID(qkid, orgid);
            WhereClip wc2 = new WhereClip();
            foreach (long l in listqkid) wc2.Or(Questions_QKnl._.Qk_ID == l);
            wc.And(wc2);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.ToList<Questions>(count);

        }
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int KnlQusTotal(int orgid, long qkid, int qtype, bool? isUse, bool children)
        {
            return KnlQusTotal(orgid, new long[] { qkid }, qtype, isUse, children);
        }
        /// <summary>
        /// 获取试题知识点的下的试题数量
        /// </summary>
        /// <param name="orgid">当前机构</param>
        /// <param name="qkid">试题知识点id</param>
        /// <param name="qtype">题型</param>
        /// <param name="isUse">是否启用的试题</param>
        /// <param name="children">是否包括下级，如果false，则取当前分类的试题</param>
        /// <returns></returns>
        public int KnlQusTotal(int orgid, long[] qkid, int qtype, bool? isUse, bool children)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);

            //计算所有子级
            if (qkid != null)
            {
                List<long> listqkid = new List<long>();
                if (!children) listqkid = qkid.ToList();
                else
                {
                    foreach (long id in qkid)
                    {
                        List<long> list = this.KnlTreeID(id, orgid);
                        foreach (long l in list)
                        {
                            if (!listqkid.Contains(l)) listqkid.Add(l);
                        }
                    }
                }
                WhereClip wc2 = new WhereClip();
                foreach (long l in listqkid) wc2.Or(Questions_QKnl._.Qk_ID == l);
                wc.And(wc2);
            }

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(wc);  
            return section.SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        /// <param name="ques">要更新的试题对象</param>
        /// <param name="former">原来的试题对象</param>
        public void KnlQusTotalUpdate(Questions ques, Questions former)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //当前试题关联的分类
            QuerySection<QuesKnowledge> section = null;
            if (ques == null || former == null) section = Gateway.Default.From<QuesKnowledge>().Where(new WhereClip());
            else
            {
                WhereClip wc = new WhereClip();
                if (ques != null) wc |= Questions_QKnl._.Qus_ID == ques.Qus_ID;
                if (former != null) wc |= Questions_QKnl._.Qus_ID == former.Qus_ID;
                section = Gateway.Default.From<QuesKnowledge>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qk_ID == QuesKnowledge._.Qk_ID).Where(wc);
            }
            //当前试题关联的知识点      
            using (SourceReader reader = section.ToReader())
            {
                while (reader.Read())
                {
                    object dvalue = reader["Qk_ID"];
                    long qkid = Convert.ToInt64(dvalue);
                    //知识点的试题数
                    int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QKnl._.Qk_ID == qkid).Count();
                    Gateway.Default.Update<QuesKnowledge>(QuesKnowledge._.Qk_Count, partcount, QuesKnowledge._.Qk_ID == qkid);
                }
                reader.Close();
                reader.Dispose();
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary> 
        public void KnlQusTotalUpdate(List<QuesKnowledge> knls)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //当前试题关联的知识点
            foreach (QuesKnowledge knl in knls)
            {
                //知识点的试题数
                int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QKnl._.Qk_ID == knl.Qk_ID).Count();
                Gateway.Default.Update<QuesKnowledge>(QuesKnowledge._.Qk_Count, partcount, QuesKnowledge._.Qk_ID == knl.Qk_ID);
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题分类下的试题数量
        /// </summary>
        public void KnlQusTotalUpdate(Questions ques)
        {
            List<QuesKnowledge> list = this.KnlForQues(ques.Qus_ID);
            if (list == null || list.Count <= 0) return;
            this.KnlQusTotalUpdate(list);
        }
        /// <summary>
        /// 更新试题分类所有试题数量
        /// </summary>
        public void KnlQusTotalUpdate()
        {
            List<QuesKnowledge> list = Gateway.Default.From<QuesKnowledge>().ToList<QuesKnowledge>();
            if (list == null || list.Count <= 0) return;
            this.KnlQusTotalUpdate(list);
        }
        /// <summary>
        /// 试题关联的知识点
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlForQues(long quesid)
        {
            return Gateway.Default.From<QuesKnowledge>().LeftJoin<Questions_QKnl>(Questions_QKnl._.Qk_ID == QuesKnowledge._.Qk_ID).Where(Questions_QKnl._.Qus_ID == quesid).ToList<QuesKnowledge>();
        }
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="pid">上级id</param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<QuesKnowledge> KnlPager(int orgid, long pid, bool? isUse, bool? isdeleted, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesKnowledge._.Org_ID == orgid);
            if (pid >= 0) wc.And(QuesKnowledge._.Qk_PID == pid);
            if (isUse != null) wc.And(QuesKnowledge._.Qk_IsUse == (bool)isUse);
            if (isdeleted != null) wc.And(QuesKnowledge._.Qk_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(QuesKnowledge._.Qk_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<QuesKnowledge>(wc);
            OrderByClip orderBy = new OrderByClip();
            if (isdeleted != null && isdeleted == true) orderBy &= QuesKnowledge._.Qk_DeleteTime.Desc;
            orderBy &= QuesKnowledge._.Qk_Order.Asc;

            return Gateway.Default.From<QuesKnowledge>().Where(wc).OrderBy(orderBy).ToList<QuesKnowledge>(size, (index - 1) * size);
        }
        /// <summary>
        /// 更改试题知识点的排序
        /// </summary>
        /// <param name="list">试题知识点列表，对象中只有Qk_ID、Qk_PID、Qk_Order</param>
        /// <returns></returns>
        public bool KnlUpdateTaxis(QuesKnowledge[] list)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (Song.Entities.QuesKnowledge Knl in list)
                    {
                        tran.Update<QuesKnowledge>(
                            new Field[] { QuesKnowledge._.Qk_PID, QuesKnowledge._.Qk_Order },
                            new object[] { Knl.Qk_PID, Knl.Qk_Order },
                            QuesKnowledge._.Qk_ID == Knl.Qk_ID);
                    }
                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        /// <param name="qkid"></param>
        /// <param name="qusid"></param>
        /// <returns></returns>
        public int KnlConnectionQues(long qkid, long qusid)
        {
            if (qusid <= 0 || qusid <= 0) return 0;
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == qusid).ToFirst<Questions>();
            QuesKnowledge[] knls = Gateway.Default.From<QuesKnowledge>().Where(QuesKnowledge._.Qk_ID == qkid).ToArray<QuesKnowledge>();
            return KnlConnectionQues(knls, ques);
        }
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        public int KnlConnectionQues(QuesKnowledge[] knls, long qusid)
        {
            if (qusid <= 0) return 0;
            Questions ques = this.QuesSingle(qusid);
            return this.KnlConnectionQues(knls, ques);           
        }
        /// <summary>
        /// 创建知识点与试题的关联
        /// </summary>
        public int KnlConnectionQues(QuesKnowledge[] knls, Questions ques)
        {
            //原有试题知识点
            List<QuesKnowledge> list = this.KnlForQues(ques.Qus_ID);

            //删除原有与试题知识点的关联
            Gateway.Default.Delete<Questions_QKnl>(Questions_QKnl._.Qus_ID == ques.Qus_ID);
            int i = 0;
            foreach (QuesKnowledge knl in knls)
            {
                Questions_QKnl qqknl = new Questions_QKnl()
                {
                    Qus_ID = ques.Qus_ID,
                    Qk_ID = knl.Qk_ID
                };
                i += Gateway.Default.Save<Questions_QKnl>(qqknl);
                if (!list.Any(p => p.Qk_ID == knl.Qk_ID)) list.Add(knl);
            }
            //更新知识点的试题数统计
            this.KnlQusTotalUpdate(list);
            return i;
        }
        #endregion

        #region 关键字
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int TagAdd(QuesTags entity)
        {
            entity.Qtag_CrtTime = DateTime.Now;
            if (entity.Org_ID <= 0)
            {
                Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            bool isexits = TagIsExist(entity.Org_ID, entity.Qtag_PID, entity.Qtag_Name);
            if (isexits) throw new Exception("关键字已经存在！");
            //如果没有排序号，则自动计算
            if (entity.Qtag_Order < 1)
            {
                WhereClip wc = QuesTags._.Org_ID == entity.Org_ID;
                if (entity.Cou_ID > 0) wc &= QuesTags._.Cou_ID == entity.Cou_ID;
                object obj = Gateway.Default.Max<QuesTags>(QuesTags._.Qtag_Order, wc);
                entity.Qtag_Order = obj != null ? Convert.ToInt32(obj) + 1 : 0;
            }
            return Gateway.Default.Save<QuesTags>(entity);
        }
        /// <summary>
        /// 添加关键字
        /// </summary>
        /// <param name="tagname"></param>
        /// <returns></returns>
        public QuesTags TagAdd(string tagname)
        {
            QuesTags tag = new QuesTags();
            tag.Qtag_Name = tagname;       
            tag.Qtag_IsDeleted = false;
            TagAdd(tag);
            return tag;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool TagIsExist(int orgid, long couid, string name)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= QuesTags._.Org_ID == orgid;
            if (couid >= 0) wc &= QuesTags._.Cou_ID == couid;
            return Gateway.Default.From<QuesTags>().Where(wc && QuesTags._.Qtag_Name == name.Trim()).Count() > 0;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool TagIsExist(QuesTags entity)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesTags._.Org_ID == entity.Org_ID;
            if (entity.Cou_ID > 0) wc &= QuesTags._.Cou_ID == entity.Cou_ID;
            if (entity.Qtag_ID > 0) wc &= QuesTags._.Qtag_ID != entity.Qtag_ID;
            return Gateway.Default.From<QuesTags>().Where(wc && QuesTags._.Qtag_Name == entity.Qtag_Name.Trim()).Count() > 0;
        }
        /// <summary>
        /// 是否已经存在
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="oneself">是否包括自身</param>
        /// <returns></returns>
        public bool TagIsExist(QuesTags entity, bool oneself = false)
        {
            WhereClip wc = new WhereClip();
            if (entity.Org_ID > 0) wc &= QuesTags._.Org_ID == entity.Org_ID;
            if (entity.Cou_ID > 0) wc &= QuesTags._.Cou_ID == entity.Cou_ID;
            if (!oneself && entity.Qtag_ID > 0) wc &= QuesTags._.Qtag_ID != entity.Qtag_ID;
            return Gateway.Default.From<QuesTags>().Where(wc && QuesTags._.Qtag_Name == entity.Qtag_Name.Trim()).Count() > 0;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void TagSave(QuesTags entity)
        {
            entity.Qtag_UpdateTime = DateTime.Now;
            bool isexits = TagIsExist(entity);
            if (isexits) throw new Exception("关键字已经存在！");
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesTags>(entity);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 逻辑删除，标记删除状态为true
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int TagDelete(long id)
        {
            return Gateway.Default.Update<QuesTags>(new Field[] { QuesTags._.Qtag_IsDeleted, QuesTags._.Qtag_DeleteTime }, new object[] { true, DateTime.Now }, QuesTags._.Qtag_ID == id);
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int TagRecycle(long id)
        {
            return Gateway.Default.Update<QuesTags>(QuesTags._.Qtag_IsDeleted, false, QuesTags._.Qtag_ID == id);
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        public int TagRemove(long id)
        {
            return Gateway.Default.Delete<QuesTags>(QuesTags._.Qtag_ID == id);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <returns></returns>
        public QuesTags TagSingle(long id)
        {
            return Gateway.Default.From<QuesTags>().Where(QuesTags._.Qtag_ID == id).ToFirst<QuesTags>();
        }
        /// <summary>
        /// 通过id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<QuesTags> TagSingle(long[] id)
        {
            if (id.Length == 0) return null;
            WhereClip wc=new WhereClip();
            for (int i = 0; i < id.Length; i++)
                wc.Or(QuesTags._.Qtag_ID == id[i]);
            return Gateway.Default.From<QuesTags>().Where(wc).ToList<QuesTags>();
        }
        /// <summary>
        /// 获取单一实体对象，按主键名称；
        /// </summary>
        /// <param name="name"></param>
        /// <param name="orgid"></param>
        /// <returns></returns>
        public QuesTags TagSingle(string name, int orgid, long couid)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            wc.And(QuesTags._.Qtag_Name == name);
            return Gateway.Default.From<QuesTags>().Where(wc).ToFirst<QuesTags>();
        }
        /// <summary>
        /// 获取关键字名称
        /// </summary>
        public string TagName(long[] tagid)
        {
            if (tagid == null || tagid.Length == 0) return string.Empty;
            WhereClip wc = new WhereClip();
            foreach (long id in tagid)
                wc.Or(QuesTags._.Qtag_ID == id);
            List<QuesTags> list = Gateway.Default.From<QuesTags>().Where(wc).ToList<QuesTags>();
            string[] arr = list.Select(q => q.Qtag_Name).ToArray();
            return string.Join(",", arr);
        }
        /// <summary>
        /// 获取试题标签名称
        /// </summary>
        /// <param name="qid">试题的id</param>
        /// <returns></returns>
        public string QuesTagName(long qid)
        {
            List<QuesTags> list = this.TagForQues(qid);
            string[] arr = list.Select(q => q.Qtag_Name).ToArray();
            return string.Join(",", arr);
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="quesid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public int TagConnectionQues(string tag, long quesid, long couid)
        {
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == quesid).ToFirst<Questions>();
            if (quesid <= 0) return 0;
            QuesTags qtag = this.TagSingle(tag, ques.Org_ID, couid > 0 ? couid : ques.Cou_ID);
            if (qtag == null)
            {
                qtag = new QuesTags()
                {
                    Qtag_Name = tag,
                    Qtag_Weight = 0,
                    Org_ID = ques.Org_ID,
                };
                this.TagAdd(qtag);
            }
            return this.TagConnectionQues(new QuesTags[] { qtag }, ques);
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        /// <param name="tagid"></param>
        /// <param name="quesid"></param>
        /// <param name="couid"></param>
        /// <returns></returns>
        public int TagConnectionQues(long tagid, long quesid, long couid)
        {
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == quesid).ToFirst<Questions>();
            QuesTags[] tags = Gateway.Default.From<QuesTags>().Where(QuesTags._.Qtag_ID == tagid).ToArray<QuesTags>();
            ques.Cou_ID = couid;
            return TagConnectionQues(tags, ques);
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        public int TagConnectionQues(QuesTags[] tags, long quesid, long couid)
        {
            if (quesid <= 0) return 0;
            Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == quesid).ToFirst<Questions>();
            ques.Cou_ID = couid;
            return TagConnectionQues(tags, ques);
        }
        /// <summary>
        /// 创建关键字与试题的关联
        /// </summary>
        public int TagConnectionQues(QuesTags[] tags, Questions ques)
        {
            //原有试题标签
            List<QuesTags> list = this.TagForQues(ques.Qus_ID);

            //删除原有与试题分类的关联
            Gateway.Default.Delete<Questions_QTags>(Questions_QTags._.Qus_ID == ques.Qus_ID);
            int i = 0;
            foreach (QuesTags tag in tags)
            {
                if (!this.TagIsExist(tag, true)) this.TagAdd(tag);
                WhereClip wc = Questions_QTags._.Qus_ID == ques.Qus_ID && Questions_QTags._.Qtag_ID == tag.Qtag_ID;
                Questions_QTags qqtag = Gateway.Default.From<Questions_QTags>().Where(wc).ToFirst<Questions_QTags>();
                if (qqtag == null)
                {
                    qqtag = new Questions_QTags()
                    {
                        Qus_ID = ques.Qus_ID,
                        Qtag_ID = tag.Qtag_ID
                    };
                }
                i += Gateway.Default.Save<Questions_QTags>(qqtag);
                if (!list.Any(p => p.Qtag_ID == tag.Qtag_ID)) list.Add(tag);
            }
            this.TagQusTotalUpdate(list);
            return i;
        }
        /// <summary>
        /// 获取试题标签
        /// </summary>
        /// <param name="orgid">机构ID</param>
        /// <param name="sear">搜索关键字</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<QuesTags> TagCount(int orgid, string sear, long couid, bool? isdeleted, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            if (!string.IsNullOrWhiteSpace(sear)) wc.And(QuesTags._.Qtag_Name.Contains(sear));
            if (isdeleted != null) wc.And(QuesTags._.Qtag_IsDeleted == (bool)isdeleted);
            return Gateway.Default.From<QuesTags>().Where(wc).OrderBy(QuesTags._.Qtag_Order.Asc).ToList<QuesTags>(count);
        }
        /// <summary>
        /// 计算试题标签的数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="couid"></param>
        /// <param name="isdeleted"></param> 
        /// <returns></returns>
        public int TagOfCount(int orgid, long couid, bool? isdeleted)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            if (isdeleted != null) wc.And(QuesTags._.Qtag_IsDeleted == (bool)isdeleted);
            return Gateway.Default.Count<QuesTags>(wc);
        }
        /// <summary>
        /// 当前试题标签下的所有试题
        /// </summary>
        /// <param name="qtagid"></param>
        /// <param name="couid"></param>
        /// <param name="qtype">试题类型</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Questions> TagQuestions(long qtagid, long couid, int qtype, bool? isuse, int count)
        {

            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            wc &= Questions._.Qus_IsDeleted == false;

            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (isuse != null) wc.And(Questions._.Qus_IsUse == (bool)isuse);

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.ToList<Questions>(count);
        }
        /// <summary>
        /// 某道试题的关键字
        /// </summary>
        /// <param name="quesid">试题id</param>
        /// <returns></returns>
        public List<QuesTags> TagForQues(long quesid)
        {
            return Gateway.Default.From<QuesTags>().LeftJoin<Questions_QTags>(Questions_QTags._.Qtag_ID == QuesTags._.Qtag_ID)
                .Where(Questions_QTags._.Qus_ID == quesid).ToList<QuesTags>();
        }
        /// <summary>
        /// 获取试题标签的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <returns></returns>
        public int TagQusTotal(long qtagid, long couid, int qtype, bool? isuse)
        {
            return TagQusTotal(new long[] { qtagid }, couid, qtype, isuse);
        }
        /// <summary>
        /// 获取试题标签的下的试题数量
        /// </summary>
        /// <param name="qtagid">试题标签id</param>
        /// <param name="couid"></param>
        /// <param name="qtype">题型</param>
        /// <param name="isuse"></param>
        public int TagQusTotal(long[] qtagid, long couid, int qtype, bool? isuse)
        {
            WhereClip wc = new WhereClip();
            wc.And(Questions._.Qus_Purpose == 1);   //考试专用试题
            wc &= Questions._.Qus_IsDeleted == false;

            if (qtype > 0) wc.And(Questions._.Qus_Type == qtype);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (isuse != null) wc.And(Questions._.Qus_IsUse == (bool)isuse);

            if (qtagid != null)
            {
                WhereClip wc2 = new WhereClip();
                foreach (long l in qtagid) wc2.Or(Questions_QTags._.Qtag_ID == l);
                wc.And(wc2);
            }

            QuerySection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(wc);
            return section.SubQuery("c").Select(Questions._.Qus_ID.At("c")).GroupBy(Questions._.Qus_ID.At("c").Group).Count();
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题标签下的试题数量
        /// </summary>
        public void TagQusTotalUpdate(List<QuesTags> tags)
        {
            //试题条件
            WhereClip queswc = Questions._.Qus_Purpose == 1;
            queswc.And(Questions._.Qus_IsDeleted == false && Questions._.Qus_IsUse == true);
            //试题的标签
            foreach (QuesTags tag in tags)
            {
                //标签的试题数
                int partcount = Gateway.Default.From<Questions>().LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID).Where(queswc && Questions_QTags._.Qtag_ID == tag.Qtag_ID).Count();
                Gateway.Default.Update<QuesTags>(QuesTags._.Qtag_Count, partcount, QuesTags._.Qtag_ID == tag.Qtag_ID);
            }
        }
        /// <summary>
        /// 试题统计更新，例如当试题被修改时，需要更新试题标签下的试题数量
        /// </summary>
        public void TagQusTotalUpdate(Questions ques)
        {
            if (ques == null) return;
            List<QuesTags> tags = this.TagForQues(ques.Qus_ID);
            if (tags == null || tags.Count <= 0) return;
            this.TagQusTotalUpdate(tags);
        }
        /// <summary>
        /// 更新试题标签所有试题数量
        /// </summary>
        public void TagQusTotalUpdate()
        {
            List<QuesTags> list = Gateway.Default.From<QuesTags>().ToList<QuesTags>();
            if (list == null || list.Count <= 0) return;
            this.TagQusTotalUpdate(list);
        }
        /// <summary>
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<QuesTags> TagPager(int orgid, long couid, bool? isdeleted, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(QuesTags._.Org_ID == orgid);
            if (couid > 0) wc.And(QuesTags._.Cou_ID == couid);
            if (isdeleted != null) wc.And(QuesTags._.Qtag_IsDeleted == (bool)isdeleted);
            if (!string.IsNullOrWhiteSpace(searTxt)) wc.And(QuesTags._.Qtag_Name.Contains(searTxt));
            countSum = Gateway.Default.Count<QuesTags>(wc);
            OrderByClip orderBy = new OrderByClip();
            if (isdeleted != null && isdeleted == true) orderBy &= QuesTags._.Qtag_DeleteTime.Desc;
            orderBy &= QuesTags._.Qtag_Order.Asc;
            return Gateway.Default.From<QuesTags>().Where(wc).OrderBy(orderBy).ToList<QuesTags>(size, (index - 1) * size);
        }
        #endregion

        #region 试题导出
        /// <summary>
        /// 导出试题
        /// </summary>
        /// <param name="subpath"></param>
        /// <param name="orgid">所属机构</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否错误</param>
        /// <param name="isWrong">是否有回馈问题</param>
        /// <returns></returns>
        public HSSFWorkbook QuesExport(string subpath, int orgid, int[] types, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diffs, bool? isUse, bool? isError, bool? isWrong)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            WhereClip wc = Questions._.Qus_Purpose == 1;    //用于考试的试题         
            wc.And(Questions._.Qus_IsDeleted == false);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);          
            //题型
            if (types != null && types.Length > 0)
            {
                WhereClip wctype = new WhereClip();
                foreach (int t in types) wctype |= Questions._.Qus_Type == t;
                wc.And(wctype);
            }
            //难度  
            if (diffs != null && diffs.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diffs) if (d > 0 && d <= 5) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            FromSection<Questions> section = Gateway.Default.From<Questions>().LeftJoin<QuesCollect>(QuesCollect._.Qus_ID == Questions._.Qus_ID);
            //试题分类
            if (qpid != null && qpid.Length > 0)
            {
                section.LeftJoin<Questions_QPart>(Questions_QPart._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long d in qpid) wcqp |= Questions_QPart._.Qp_ID == d;
                wc.And(wcqp);
            }
            //试题关键字
            if (tagid != null && tagid.Length > 0)
            {
                section.LeftJoin<Questions_QTags>(Questions_QTags._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long t in tagid) wcqp |= Questions_QTags._.Qtag_ID == t;
                wc.And(wcqp);
            }
            //关联知识点
            if (knlid != null && knlid.Length > 0)
            {
                section.LeftJoin<Questions_QKnl>(Questions_QKnl._.Qus_ID == Questions._.Qus_ID);
                WhereClip wcqp = new WhereClip();
                foreach (long k in knlid) wcqp |= Questions_QKnl._.Qk_ID == k;
                wc.And(wcqp);
            }
            //生成分类、知识点、关键字的信息
            string parts = this.PartName(qpid), knls = this.KnlName(knlid), tags = this.TagName(tagid);
            //每页最多能放多少道题
            int pagesize = 30000;
            //试题类型，通过不同的试题类型返回工作表
            foreach (int ty in types)
            {              
                //计算有多少道题
                WhereClip where = (Questions._.Qus_Type == ty).And(wc);
                int total = Gateway.Default.Count<Questions>(where);       //当前题型的记录数
                int totalPages = (total + pagesize - 1) / pagesize;     //页数

                for (int idx = 1; idx <= totalPages; idx++)
                {
                    if (ty == 1) _buildExcelSql_1(hssfworkbook, where, subpath, parts, tags, knls, total, pagesize, idx);
                    if (ty == 2) _buildExcelSql_2(hssfworkbook, where, subpath, parts, tags, knls, total, pagesize, idx);
                    if (ty == 3) _buildExcelSql_3(hssfworkbook, where, subpath, parts, tags, knls, total, pagesize, idx);
                    if (ty == 4) _buildExcelSql_4(hssfworkbook, where, subpath, parts, tags, knls, total, pagesize, idx);
                    if (ty == 5) _buildExcelSql_5(hssfworkbook, where, subpath, parts, tags, knls, total, pagesize, idx);
                }
            }
            return hssfworkbook;
        }
        /// <summary>
        /// 导出试题,生成文件
        /// </summary>
        /// <param name="subpath">导出文件的路径（服务器端），相对临时路径的子路径</param>
        /// <param name="orgid">所属机构</param>
        /// <param name="types">题型</param>
        /// <param name="qpid">分类id</param>
        /// <param name="tagid">标签id</param>
        /// <param name="knlid">知识点id</param>
        /// <param name="isdeleted">是否删除的</param>
        /// <param name="diffs">难度等级</param>
        /// <param name="isUse">是否启用</param>
        /// <param name="isError">是否错误</param>
        /// <param name="isWrong">是否有回馈问题</param>     
        /// <returns></returns>
        public JObject QuesExportExcel(string subpath, int orgid, int[] types, long[] qpid, long[] tagid, long[] knlid, bool? isdeleted, int[] diffs, bool? isUse, bool? isError, bool? isWrong)
        {
            long snowid = WeiSha.Core.Request.SnowID();
            DateTime date = DateTime.Now;
            //导出文件的位置
            string path = Path.Combine(Upload.Get["Temp"].Physics, subpath, orgid.ToString(), snowid.ToString());
            string filename = string.Format("考试试题导出.({0}).xls", date.ToString("yyyy-MM-dd hh-mm-ss"));

            //导出Excel
            HSSFWorkbook hssfworkbook = this.QuesExport(path, orgid, types, qpid, tagid, knlid, isdeleted, diffs, isUse, isError, isWrong);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            FileStream file = new FileStream(Path.Combine(path, filename), FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();

            //生成最终的导出文件，如果没有txt文件（即试题内容没有超出Excel单元格最大文本长度），则输出Excel
            //如果有txt文件，则打包输出
            string[] files = Directory.GetFiles(path, "*.txt");
            string parentFolder = Directory.GetParent(path).FullName;     // 获取上级文件夹路径
            if (files.Length < 1)
            {
                string parentFile = Path.Combine(parentFolder, filename);
                // 如果目标文件存在，可以先删除或改名
                if (File.Exists(parentFile)) File.Delete(parentFile);
                File.Move(Path.Combine(path, filename), parentFile);
            }
            else
            {
                string zipfile = Path.Combine(parentFolder, Path.ChangeExtension(filename, ".zip"));
                WeiSha.Core.Compress.ZipFiles(path, zipfile);
                filename = zipfile;
            }
            Directory.Delete(path, true);
            //
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + subpath + "/" + orgid + "/" + filename);
            jo.Add("date", date);
            return jo;
        }
        #region 导出试的私有方法
        //Excel单元格的最长文本长度
        private static int _excel_field_max_length = 32767;
        /// <summary>
        /// 将过长的内容存储到文件
        /// </summary>
        /// <param name="qid"></param>
        /// <param name="idx"></param>
        /// <param name="field">字段名称</param>
        /// <param name="folder">导出内容的文件夹，为物理路径</param>
        /// <param name="content">要保存的内容</param>
        /// <returns>文件名</returns>
        private string _build_text(long qid, int idx, string field, string folder, string content)
        {
            string name = $"{qid}.{idx}.{field}.txt";
            string fullname = folder + "\\" + name;
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            File.WriteAllText(fullname, content);
            return name;
        }
        /// <summary>
        /// 生成单选题导出Excel的SQL语句
        /// </summary>
        /// <param name="hssfworkbook"></param>
        /// <param name="where">查询条件</param>
        /// <param name="total"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        private void _buildExcelSql_1(HSSFWorkbook hssfworkbook, WhereClip where, string subpath, string parts, string tags, string knls, int total, int size, int index)
        {
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(where).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>(size, (index - 1) * size);
            //创建工作簿对象
            string sheetname = "单选题";
            ISheet sheet = hssfworkbook.CreateSheet(total <= size ? sheetname : sheetname + "_" + index.ToString("D2"));
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题干", "分类", "关键字", "知识点", "难度", "答案选项1", "答案选项2", "答案选项3", "答案选项4", "答案选项5", "答案选项6", "正确答案", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                string folder = Path.Combine(subpath, q.Org_ID.ToString());
                IRow row = sheet.CreateRow(i + 1);
                List<QuesAnswer> qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                int ansIndex = 0;

                for (int j = 0; j < qas.Count; j++)
                {
                    QuesAnswer c = qas[j];
                    if (string.IsNullOrWhiteSpace(c.Ans_Context) || c.Ans_Context.Length <= _excel_field_max_length)
                        row.CreateCell(6 + j).SetCellValue(c.Ans_Context);
                    else row.CreateCell(6 + j).SetCellValue(_build_text(q.Qus_ID, j, "Ans_Context", folder, c.Ans_Context));
                    if (c.Ans_IsCorrect) ansIndex = j + 1;
                }

                row.CreateCell(0).SetCellValue(q.Qus_ID.ToString());
                //题干
                if (string.IsNullOrWhiteSpace(q.Qus_Title) || q.Qus_Title.Length <= _excel_field_max_length)
                    row.CreateCell(1).SetCellValue(q.Qus_Title);
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Title", folder, q.Qus_Title));
                // 分类, 关键字, 知识点
                if (string.IsNullOrWhiteSpace(parts)) row.CreateCell(2).SetCellValue(this.QuesPartName(q.Qus_ID));
                else row.CreateCell(2).SetCellValue(parts);
                if (string.IsNullOrWhiteSpace(tags)) row.CreateCell(3).SetCellValue(this.QuesTagName(q.Qus_ID));
                else row.CreateCell(3).SetCellValue(tags);
                if (string.IsNullOrWhiteSpace(knls)) row.CreateCell(4).SetCellValue(this.QuesKnlName(q.Qus_ID));
                else row.CreateCell(4).SetCellValue(knls);

                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(12).SetCellValue(ansIndex.ToString());
                //解析
                if (string.IsNullOrWhiteSpace(q.Qus_Explain) || q.Qus_Explain.Length <= _excel_field_max_length)
                    row.CreateCell(13).SetCellValue(q.Qus_Explain);
                else row.CreateCell(13).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Explain", folder, q.Qus_Explain));

                i++;
            }
        }

        //多选题导出
        private void _buildExcelSql_2(HSSFWorkbook hssfworkbook, WhereClip where, string subpath, string parts, string tags, string knls, int total, int size, int index)
        {
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(where).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>(size, (index - 1) * size);
            //创建工作簿对象
            string sheetname = "多选题";
            ISheet sheet = hssfworkbook.CreateSheet(total <= size ? sheetname : sheetname + "_" + index.ToString("D2"));
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题干", "专业", "课程", "章节", "难度", "答案选项1", "答案选项2", "答案选项3", "答案选项4", "答案选项5", "答案选项6", "正确答案", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                string folder = Path.Combine(subpath, q.Org_ID.ToString());
                IRow row = sheet.CreateRow(i + 1);
                List<QuesAnswer> qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                string ansIndex = "";
                for (int j = 0; j < qas.Count; j++)
                {
                    QuesAnswer c = qas[j];
                    if (string.IsNullOrWhiteSpace(c.Ans_Context) || c.Ans_Context.Length <= _excel_field_max_length)
                        row.CreateCell(6 + j).SetCellValue(c.Ans_Context);
                    if (c.Ans_IsCorrect) ansIndex += Convert.ToString(j + 1) + ",";
                }
                if (ansIndex.Length > 0)
                {
                    if (ansIndex.Substring(ansIndex.Length - 1) == ",")
                        ansIndex = ansIndex.Substring(0, ansIndex.Length - 1);
                }

                row.CreateCell(0).SetCellValue(q.Qus_ID.ToString());
                //题干
                if (string.IsNullOrWhiteSpace(q.Qus_Title) || q.Qus_Title.Length <= _excel_field_max_length)
                    row.CreateCell(1).SetCellValue(q.Qus_Title);
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Title", folder, q.Qus_Title));
                //专业,课程,章节
                if (string.IsNullOrWhiteSpace(parts)) row.CreateCell(2).SetCellValue(this.QuesPartName(q.Qus_ID));
                else row.CreateCell(2).SetCellValue(parts);
                if (string.IsNullOrWhiteSpace(tags)) row.CreateCell(3).SetCellValue(this.QuesTagName(q.Qus_ID));
                else row.CreateCell(3).SetCellValue(tags);
                if (string.IsNullOrWhiteSpace(knls)) row.CreateCell(4).SetCellValue(this.QuesKnlName(q.Qus_ID));
                else row.CreateCell(4).SetCellValue(knls);
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(12).SetCellValue(ansIndex.ToString());
                //解析
                if (string.IsNullOrWhiteSpace(q.Qus_Explain) || q.Qus_Explain.Length <= _excel_field_max_length)
                    row.CreateCell(13).SetCellValue(q.Qus_Explain);
                else row.CreateCell(13).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Explain", folder, q.Qus_Explain));

                i++;
            }
        }
        //判断题导出
        private void _buildExcelSql_3(HSSFWorkbook hssfworkbook, WhereClip where, string subpath, string parts, string tags, string knls, int total, int size, int index)
        {
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(where).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>(size, (index - 1) * size);
            //创建工作簿对象
            string sheetname = "判断题";
            ISheet sheet = hssfworkbook.CreateSheet(total <= size ? sheetname : sheetname + "_" + index.ToString("D2"));
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题干", "专业", "课程", "章节", "难度", "答案", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                string folder = Path.Combine(subpath, q.Org_ID.ToString());
                string ans = "";
                if (Convert.ToString(q.Qus_IsCorrect) == "False") { ans = "错误"; } else { ans = "正确"; }
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(q.Qus_ID.ToString());
                //题干
                if (string.IsNullOrWhiteSpace(q.Qus_Title) || q.Qus_Title.Length <= _excel_field_max_length)
                    row.CreateCell(1).SetCellValue(q.Qus_Title);
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Title", folder, q.Qus_Title));
                //专业,课程,章节
                if (string.IsNullOrWhiteSpace(parts)) row.CreateCell(2).SetCellValue(this.QuesPartName(q.Qus_ID));
                else row.CreateCell(2).SetCellValue(parts);
                if (string.IsNullOrWhiteSpace(tags)) row.CreateCell(3).SetCellValue(this.QuesTagName(q.Qus_ID));
                else row.CreateCell(3).SetCellValue(tags);
                if (string.IsNullOrWhiteSpace(knls)) row.CreateCell(4).SetCellValue(this.QuesKnlName(q.Qus_ID));
                else row.CreateCell(4).SetCellValue(knls);
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(6).SetCellValue(ans);
                //解析
                if (string.IsNullOrWhiteSpace(q.Qus_Explain) || q.Qus_Explain.Length <= _excel_field_max_length)
                    row.CreateCell(7).SetCellValue(q.Qus_Explain);
                else row.CreateCell(7).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Explain", folder, q.Qus_Explain));
                i++;
            }
        }
        //简答题导出
        private void _buildExcelSql_4(HSSFWorkbook hssfworkbook, WhereClip where, string subpath, string parts, string tags, string knls, int total, int size, int index)
        {
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(where).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>(size, (index - 1) * size);
            //创建工作簿对象
            string sheetname = "简答题";
            ISheet sheet = hssfworkbook.CreateSheet(total <= size ? sheetname : sheetname + "_" + index.ToString("D2"));
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题干", "专业", "课程", "章节", "难度", "答案", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                string folder = Path.Combine(subpath, q.Org_ID.ToString());
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(q.Qus_ID.ToString());
                //题干
                if (string.IsNullOrWhiteSpace(q.Qus_Title) || q.Qus_Title.Length <= _excel_field_max_length)
                    row.CreateCell(1).SetCellValue(q.Qus_Title);
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Title", folder, q.Qus_Title));
                //专业,课程,章节
                if (string.IsNullOrWhiteSpace(parts)) row.CreateCell(2).SetCellValue(this.QuesPartName(q.Qus_ID));
                else row.CreateCell(2).SetCellValue(parts);
                if (string.IsNullOrWhiteSpace(tags)) row.CreateCell(3).SetCellValue(this.QuesTagName(q.Qus_ID));
                else row.CreateCell(3).SetCellValue(tags);
                if (string.IsNullOrWhiteSpace(knls)) row.CreateCell(4).SetCellValue(this.QuesKnlName(q.Qus_ID));
                else row.CreateCell(4).SetCellValue(knls);
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                //正常答案
                if (string.IsNullOrWhiteSpace(q.Qus_Answer) || q.Qus_Answer.Length <= _excel_field_max_length)
                    row.CreateCell(6).SetCellValue(q.Qus_Answer);
                else row.CreateCell(6).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Answer", folder, q.Qus_Answer));
                //解析
                if (string.IsNullOrWhiteSpace(q.Qus_Explain) || q.Qus_Explain.Length <= _excel_field_max_length)
                    row.CreateCell(7).SetCellValue(q.Qus_Explain);
                else row.CreateCell(7).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Explain", folder, q.Qus_Explain));
                i++;
            }
        }
        //填空题导出
        private void _buildExcelSql_5(HSSFWorkbook hssfworkbook, WhereClip where, string subpath, string parts, string tags, string knls, int total, int size, int index)
        {
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(where).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>(size, (index - 1) * size);
            //创建工作簿对象
            string sheetname = "填空题";
            ISheet sheet = hssfworkbook.CreateSheet(total <= size ? sheetname : sheetname + "_" + index.ToString("D2"));
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题干", "专业", "课程", "章节", "难度",
                "答案选项1", "答案选项2", "答案选项3", "答案选项4", "答案选项5", "答案选项6", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                string folder = Path.Combine(subpath, q.Org_ID.ToString());
                IRow row = sheet.CreateRow(i + 1);
                List<QuesAnswer> qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                for (int j = 0; j < qas.Count; j++)
                {
                    QuesAnswer c = qas[j];
                    if (string.IsNullOrWhiteSpace(c.Ans_Context) || c.Ans_Context.Length <= _excel_field_max_length)
                        row.CreateCell(6 + j).SetCellValue(c.Ans_Context);
                    else row.CreateCell(6 + j).SetCellValue(_build_text(q.Qus_ID, j, "Ans_Context", folder, c.Ans_Context));
                }

                row.CreateCell(0).SetCellValue(q.Qus_ID.ToString());
                //题干
                if (string.IsNullOrWhiteSpace(q.Qus_Title) || q.Qus_Title.Length <= _excel_field_max_length)
                    row.CreateCell(1).SetCellValue(q.Qus_Title);
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Title", folder, q.Qus_Title));
                //专业,课程,章节
                if (string.IsNullOrWhiteSpace(parts)) row.CreateCell(2).SetCellValue(this.QuesPartName(q.Qus_ID));
                else row.CreateCell(2).SetCellValue(parts);
                if (string.IsNullOrWhiteSpace(tags)) row.CreateCell(3).SetCellValue(this.QuesTagName(q.Qus_ID));
                else row.CreateCell(3).SetCellValue(tags);
                if (string.IsNullOrWhiteSpace(knls)) row.CreateCell(4).SetCellValue(this.QuesKnlName(q.Qus_ID));
                else row.CreateCell(4).SetCellValue(knls);
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                //解析
                if (string.IsNullOrWhiteSpace(q.Qus_Explain) || q.Qus_Explain.Length <= _excel_field_max_length)
                    row.CreateCell(12).SetCellValue(q.Qus_Explain);
                else row.CreateCell(12).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Explain", folder, q.Qus_Explain));
                i++;
            }
        }
        #endregion
        #endregion
    }
}
