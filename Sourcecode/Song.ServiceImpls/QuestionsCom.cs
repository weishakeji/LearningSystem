using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Core;
using Song.Entities;
using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Xml;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Song.ServiceImpls
{
    public class QuestionsCom : IQuestions
    {
         
        #region 试题管理

        public long QuesAdd(Questions entity)
        {
            if (entity.Qus_ID <= 0)          
                entity.Qus_ID = WeiSha.Core.Request.SnowID();
   
            entity.Qus_CrtTime = DateTime.Now;
            entity.Qus_LastTime = DateTime.Now;
            entity.Qus_Title = _ClearString(entity.Qus_Title);
            entity.Qus_Answer = _ClearString(entity.Qus_Answer);
            entity.Qus_Explain = _ClearString(entity.Qus_Explain);
            if (entity.Org_ID <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
            }
            if (string.IsNullOrWhiteSpace(entity.Qus_UID))
                entity.Qus_UID = WeiSha.Core.Request.UniqueID();          
            Gateway.Default.Save<Questions>(entity);
            this.OnAdd(entity, EventArgs.Empty);
            return entity.Qus_ID;
        }

        public void QuesSave(Questions entity)
        {
            entity.Qus_LastTime = DateTime.Now;
            entity.Qus_IsError = false;
            entity.Qus_Title = _ClearString(entity.Qus_Title);
            entity.Qus_Answer = _ClearString(entity.Qus_Answer);
            //if (entity.Qus_Type == 3)
            //{
            //    if (entity.Qus_IsCorrect == false ) entity.Qus_IsError = true;
            //}
            if (entity.Qus_Type == 4)
            {
                if (string.IsNullOrWhiteSpace(entity.Qus_Answer) || string.IsNullOrWhiteSpace(entity.Qus_Answer))
                {
                    entity.Qus_IsError = true;
                    entity.Qus_ErrorInfo = "答案不得为空";
                }
            }
            if (entity.Qus_Type == 5)
            {
                //entity.Qus_Items
                //HTML.ClearTag
            }
            entity.Qus_Explain = _ClearString(entity.Qus_Explain);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Questions>(entity);
                    tran.Update<QuesAnswer>(new Field[] { QuesAnswer._.Qus_ID }, new object[] { entity.Qus_ID }, QuesAnswer._.Qus_UID == entity.Qus_UID);
                    tran.Commit();
                    this.OnSave(entity, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            //更新统计数据
            new Task(() =>
            {
                Business.Do<IQuestions>().QuesCountUpdate(-1, entity.Sbj_ID, entity.Cou_ID, entity.Ol_ID);
            }).Start();
        }

        public void QuesInput(Questions entity, List<Song.Entities.QuesAnswer> ansItem)
        {
            entity.Qus_Title = _ClearString(entity.Qus_Title);
            entity.Qus_Answer = _ClearString(entity.Qus_Answer);
            entity.Qus_Explain = _ClearString(entity.Qus_Explain);
            //答题选项的处理
            if (ansItem != null)
            {
                //如果有试题id，则加上，好像也无所谓
                if (entity.Qus_ID > 0)
                {
                    for (int i = 0; i < ansItem.Count; i++)
                        ansItem[i].Qus_ID = entity.Qus_ID;
                } 
                entity.Qus_Items = this.AnswerToItems(ansItem.ToArray());
            }
            //判断是否存在
            Questions old = null;
            if (entity.Qus_ID > 0) old = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == entity.Qus_ID).ToFirst<Questions>();
            if (old == null)
            {
                WhereClip wc = Questions._.Qus_Type == entity.Qus_Type;
                if (entity.Ol_ID > 0) wc.And(Questions._.Ol_ID == entity.Ol_ID);
                if (entity.Cou_ID > 0) wc.And(Questions._.Cou_ID == entity.Cou_ID);
                //题干是否相同
                old = Gateway.Default.From<Questions>().Where(wc && Questions._.Qus_Title == entity.Qus_Title && Questions._.Qus_Items == entity.Qus_Items).ToFirst<Questions>();
            }
            if (old == null)
            {
                entity.Qus_ID = WeiSha.Core.Request.SnowID();
                entity.Qus_Diff = entity.Qus_Diff <= 0 ? 3 : (entity.Qus_Diff > 5 ? 5 : entity.Qus_Diff);
                entity.Qus_CrtTime = DateTime.Now;
                Gateway.Default.Save<Questions>(entity);
            }
            else
            {
                old.Copy<Song.Entities.Questions>(entity, "Qus_ID");
                Gateway.Default.Save<Questions>(old);
            }
        }
        /// <summary>
        /// 删除试题
        /// </summary>
        /// <param name="entity">试题实体</param>
        public void QuesDelete(Questions entity)
        {
            if (entity == null) return;
            Gateway.Default.Delete<Questions>(Questions._.Qus_ID == entity.Qus_ID);
            this.OnDelete(entity, EventArgs.Empty);
            //更新统计数据
            new Task(() =>
            {
                Business.Do<IQuestions>().QuesCountUpdate(entity.Org_ID, entity.Sbj_ID, entity.Cou_ID, entity.Ol_ID);
            }).Start();
        }
        public void QuesDelete(long identify)
        {
            Questions ques = this.QuesSingle(identify);
            this.QuesDelete(ques);            
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        public void QuesDelete(string[] ids)
        {
            long[] arr = ids.Select(long.Parse).ToArray();
            this.QuesDelete(arr);
        }
        public void QuesDelete(long[] idarray)
        {
            //计算试题的机构id,专业id,课程id,章节id
            List<int> orgids = new List<int>();
            List<long> sbjids = new List<long>();
            List<long> couids = new List<long>();
            WhereClip wc = new WhereClip();
            foreach (long id in idarray)
                wc.Or(Questions._.Qus_ID == id);
            List<Questions> ques = Gateway.Default.From<Questions>().Where(wc).ToList<Questions>();
            foreach(Questions q in ques)
            {
                if (!orgids.Contains(q.Org_ID)) orgids.Add(q.Org_ID);
                if (!sbjids.Contains(q.Sbj_ID)) sbjids.Add(q.Sbj_ID);
                if (!couids.Contains(q.Cou_ID)) couids.Add(q.Cou_ID);
            }
            //删除并重新统计
            Gateway.Default.Delete<Questions>(wc);
            foreach (int orgid in orgids) Business.Do<IQuestions>().QuesCountUpdate(orgid, -1, -1, -1);
            foreach (int sbjid in sbjids) Business.Do<IQuestions>().QuesCountUpdate(-1, sbjid, -1, -1);
            foreach (int couid in couids) Business.Do<IQuestions>().QuesCountUpdate(-1, couid, couid, -1);
            //删除图片等资源
            new Task(() =>
            {
                foreach (long id in idarray)
                    WeiSha.Core.Upload.Get["Ques"].DeleteDirectory(id.ToString());               
            }).Start();          
           
            new Task(() =>
            {
                //删除笔记
                WhereClip wcNotes = new WhereClip();
                foreach (long id in idarray)
                    wcNotes.Or(Student_Notes._.Qus_ID == id);
                Gateway.Default.Delete<Student_Notes>(wcNotes);
            }).Start();

            new Task(() =>
            {
                //删除收藏记录
                WhereClip wcCol = new WhereClip();
                foreach (long id in idarray)
                    wcCol.Or(Student_Collect._.Qus_ID == id);
                Gateway.Default.Delete<Student_Collect>(wcCol);

            }).Start();

            new Task(() =>
            {
                //删除错题记录
                WhereClip wcErr = new WhereClip();
                foreach (long id in idarray)
                    wcErr.Or(Student_Ques._.Qus_ID == id);
                Gateway.Default.Delete<Student_Ques>(wcErr);

            }).Start();
            this.OnDelete(idarray, EventArgs.Empty);

        }
        /// <summary>
        /// 修改课程的某些项
        /// </summary>
        /// <param name="qusid">课程id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool QuesUpdate(long qusid, Field[] fiels, object[] objs)
        {
            try
            {
                Gateway.Default.Update<Questions>(fiels, objs, Questions._.Qus_ID == qusid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改试题的某些项
        /// </summary>
        /// <param name="qusid"></param>
        /// <param name="field"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool QuesUpdate(long qusid, Field field, object obj)
        {
            Gateway.Default.Update<Questions>(field, obj, Questions._.Qus_ID == qusid);
            return true;
        }
        public Questions QuesSingle(long identify)
        {
            Song.Entities.Questions qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == identify).ToFirst<Questions>();       
            return qus;
           
        }
        /// <summary>
        ///  获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="cache">是否来自缓存</param>
        /// <returns></returns>
        public Questions QuesSingle(long identify, bool cache)
        {
            if (cache) return this.QuesSingle(identify);
            return Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == identify).ToFirst<Questions>();
        }
        public Questions QuesSingle(string uid)
        {
            if (uid == string.Empty) return null;
            Song.Entities.Questions qus =  Gateway.Default.From<Questions>().Where(Questions._.Qus_UID == uid.Trim() && Questions._.Qus_IsTitle == true).ToFirst<Questions>();
           
            return qus;
        }

        public Questions QuesSingle(string titile, int type)
        {
            if (titile == string.Empty) return null;
            WhereClip wc = new WhereClip();
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (titile.Trim() != "") wc.And(Questions._.Qus_Title.Contains(titile.Trim()));
            Song.Entities.Questions qus = Gateway.Default.From<Questions>().Where(wc).ToFirst<Questions>();
            if (qus == null) return qus;           
            return qus;
        }

        public QuesAnswer[] QuestionsAnswer(Questions qus, bool? isCorrect)
        {
            Song.Entities.QuesAnswer[] ans = this.ItemsToAnswer(qus, isCorrect);
            return ans;
        }

        public Questions[] QuesCount(int type, bool? isUse, int count)
        {
            WhereClip wc = Questions._.Qus_IsError == false && Questions._.Qus_IsWrong == false;
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Type.Asc && Questions._.Qus_ID.Asc)
                .ToArray<Questions>(count);
        }
        /// <summary>
        /// 获取某个课程或章节试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难度等级</param>
        /// <param name="isUse"></param>
        /// <param name="index">起始索引</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Questions[] QuesCount(int orgid, long sbjid, long couid, long olid, int type, int diff, bool? isUse, int index, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {                
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<IOutline>().TreeID(olid);
                foreach (long l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);               
            }
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (diff > 0) wc.And(Questions._.Qus_Diff == diff);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Type.Asc && Questions._.Qus_ID.Asc)
                .ToArray<Questions>(count, index);
        }
        /// <summary>
        /// 获取简化的某个课程或章节试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff">难度等级</param>
        /// <param name="isUse"></param>
        /// <param name="fields">要取值的字段</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        public Questions[] QuesSimplify(int orgid, long sbjid, long couid, long olid, int type, int diff, bool? isUse, Field[] fields, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<IOutline>().TreeID(olid);
                foreach (long l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);
            }
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (diff > 0) wc.And(Questions._.Qus_Diff == diff);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (fields == null) fields = new Field[] { };
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Type.Asc && Questions._.Qus_ID.Asc).Select(fields)
                .ToArray<Questions>(count);
        }
        public int QuesOfCount(int orgid, long sbjid, long couid, long olid, int type, int diff, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (olid > 0) wc.And(Questions._.Ol_ID == olid);
            ////当前章节，以及当前章节之下的所有试题
            //if (olid > 0)
            //{
            //    WhereClip wcSbjid = new WhereClip();
            //    List<long> list = Business.Do<IOutline>().TreeID(olid);
            //    foreach (long l in list)
            //        wcSbjid.Or(Questions._.Ol_ID == l);
            //    wc.And(wcSbjid);
            //}
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (diff > 0) wc.And(Questions._.Qus_Diff == diff);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.Count<Questions>(wc);
        }
        /// <summary>
        /// 统计试题数量
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="types"></param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>
        /// <returns></returns>
        public int QuesOfCount(int orgid, long sbjid, long couid, long olid, int[] types, int[] diff, bool? isUse, bool? isError, bool? isWrong)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            //专业
            if (sbjid > 0 && couid <= 0 && olid <= 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list) wcSbjid.Or(Questions._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {
                WhereClip wcOlid = new WhereClip();
                List<long> list = Business.Do<IOutline>().TreeID(olid);
                foreach (long l in list) wcOlid.Or(Questions._.Ol_ID == l);
                wc.And(wcOlid);
            }
            //试题类型
            if (types.Length > 0)
            {
                WhereClip wctype = new WhereClip();
                foreach (int ty in types) wctype |= Questions._.Qus_Type == ty;
                wc.And(wctype);               
            }
            //试题难度
            if (diff.Length > 0)
            {
                WhereClip wcdiff = new WhereClip();
                foreach (int d in diff) wcdiff |= Questions._.Qus_Diff == d;
                wc.And(wcdiff);
            }
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            return Gateway.Default.Count<Questions>(wc);
        }
        /// <summary>
        /// 试题数量更新到机构、专业、课程、章节，方便展示
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        public void QuesCountUpdate(int orgid, long sbjid, long couid, long olid)
        {
            //课程中的试题数量
            if (couid > 0)
            {
                int cou_count = this.QuesOfCount(-1, -1, couid, -1, 0, -1, null);
                Gateway.Default.Update<Course>(new Field[] { Course._.Cou_QuesCount }, new object[] { cou_count }, Course._.Cou_ID == couid);
            }
            //章节下的试题数量
            //当前章节，以及当前章节之下的所有试题
            List<long> olist = new List<long>();
            if (olid > 0)
            {
                olist = Business.Do<IOutline>().TreeID(olid);
            }
            //课程下所有章节
            else if (couid > 0)
            {
                List<Outline> outlines = Business.Do<IOutline>().OutlineAll(couid, null, null, null);
                for (int i = 0; i < outlines.Count; i++)                
                    olist.Add(outlines[i].Ol_ID);               
            }
            if (olist.Count > 0)
            {
                foreach(long id in olist)
                {
                    int olcount = this.QuesOfCount(-1, -1, -1, id, -1, -1, null);
                    Gateway.Default.Update<Outline>(new Field[] { Outline._.Ol_QuesCount }, new object[] { olcount }, Outline._.Ol_ID == id);
                }
            }
            //专业下的试题数
            if (sbjid > 0)
            {
                int sbj_count = this.QuesOfCount(-1, sbjid, -1, -1, 0, -1, null);
                Gateway.Default.Update<Subject>(new Field[] { Subject._.Sbj_QuesCount }, new object[] { sbj_count }, Subject._.Sbj_ID == sbjid);
            }
            //机构的试题数
            if (orgid > 0)
            {
                int org_count = this.QuesOfCount(orgid, -1, -1, -1, 0, -1, null);
                Gateway.Default.Update<Organization>(new Field[] { Organization._.Org_QuesCount }, new object[] { org_count }, Organization._.Org_ID == orgid);
            }
        }
        /// <summary>
        /// 获取随机试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题类型</param>
        /// <param name="diff1">难度范围</param>
        /// <param name="diff2">难度范围</param>
        /// <param name="isUse">是否允许</param>
        /// <param name="count">取的数量</param>
        /// <returns></returns>
        public Questions[] QuesRandom(int orgid, long sbjid, long couid, long olid, int type, int diff1, int diff2, bool? isUse, int count)
        {
            WhereClip wc = Questions._.Qus_IsError == false;
            //机构
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            //试题类型
            string[] types = WeiSha.Core.App.Get["QuesType"].Split(',');
            if (type < 1 || type > types.Length) type = -1;
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            //难度区间
            diff1 = diff1 < 1 ? 1 : diff1;
            diff2 = diff2 < 1 || diff2 > 5 ? 5 : diff2;
            if (diff1 > 0) wc.And(Questions._.Qus_Diff >= diff1);  //最小难度等级
            if (diff2 > 0) wc.And(Questions._.Qus_Diff <= diff2);  //最大难度
            //章节id
            if (olid > 0)
            {                
                List<long> list = Business.Do<IOutline>().TreeID(olid);
                WhereClip wcOl = new WhereClip();
                foreach (int id in list) wc.Or(Questions._.Ol_ID == id);
                wc.And(wcOl);
            }
            else if (couid > 0) wc.And(Questions._.Cou_ID==couid);  //课程id
            else if(sbjid>0) wc.And(Questions._.Sbj_ID == sbjid);   //专业id
            //随机排序
            OrderByClip order;
            if (Gateway.Default.DbType != DbProviderType.SQLServer)
                order= new OrderByClip("RANDOM()");
            else
                order = new OrderByClip("NEWID()");
            return Gateway.Default.From<Questions>().Where(wc).OrderBy(order).ToArray<Questions>(count);
        }
        ///// <summary>
        ///// 为了获取章节下面的子章节中的试题，生成相关条件判断
        ///// </summary>
        ///// <param name="olid"></param>
        ///// <returns></returns>
        //private string _quesRandom_buildOlid(long olid)
        //{
        //    string sql = "";
        //    Outline[] ols = Gateway.Default.From<Outline>().Where(Outline._.Ol_PID == olid).ToArray<Outline>();
        //    foreach (Outline o in ols)
        //    {
        //        sql += " or Ol_ID=" + o.Ol_ID;
        //        sql += _quesRandom_buildOlid(o.Ol_ID);
        //    }
        //    return sql;
        //}

        public Questions[] QuesRandom(int type, long sbjid, long couid, int diff1, int diff2, bool? isUse, int count)
        {
            return this.QuesRandom(-1, sbjid, couid, -1, type, diff1, diff2, isUse, count);           
        }
        /// <summary>
        /// 分页获取试题
        /// </summary>
        /// <param name="type">试题类型，1为单选，2为多选</param>
        /// <param name="isUse">试题是否启用的状态，如为null取所有试题</param>
        /// <param name="diff">难度</param>
        /// <param name="searTxt">检索的字符串</param>
        /// <param name="size">每页显示多少条</param>
        /// <param name="index">当前页索引</param>
        /// <param name="countSum">共计多少条</param>
        /// <returns></returns>
        public Questions[] QuesPager(int orgid, int type, bool? isUse, int diff,
            string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (diff > 0 && diff <= 5) wc.And(Questions._.Qus_Diff == diff);
            if (searTxt != string.Empty && searTxt.ToLower() != "")
                wc.And(Questions._.Qus_Title.Contains(searTxt.Trim()));
            countSum = Gateway.Default.Count<Questions>(wc);
            return Gateway.Default.From<Questions>()
                .Where(wc).OrderBy(Questions._.Qus_ID.Desc)
                .ToArray<Questions>(size, (index - 1) * size);
        }

        public Questions[] QuesPager(int orgid, int type, long sbjid, long couid, long olid, bool? isUse,
            bool? isError, bool? isWrong, int diff, string searTxt,
            int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (sbjid > 0 && couid <= 0 && olid <= 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list) wcSbjid.Or(Questions._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {
                WhereClip wcOlid = new WhereClip();
                List<long> list = Business.Do<IOutline>().TreeID(olid);
                foreach (long l in list) wcOlid.Or(Questions._.Ol_ID == l);
                wc.And(wcOlid);
            }
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            if (diff > 0 && diff <= 5) wc.And(Questions._.Qus_Diff == diff);
            if (!string.IsNullOrWhiteSpace(searTxt) && searTxt.Trim() != "")
            {
                wc.And(Questions._.Qus_Title.Contains(searTxt.Trim()));
            }
            countSum = Gateway.Default.Count<Questions>(wc);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_ID.Desc)
                .ToArray<Questions>(size, (index - 1) * size);
        }
        /// <summary>
        /// 当前试题的下一个试题，在指定范围内取，例如课程内的试题
        /// </summary>
        /// <param name="id">试题id</param>
        /// <param name="olid">章节id</param>
        /// <param name="couid">课程id</param>
        /// <param name="sbjid">专业id</param>
        /// <returns></returns>
        public Questions QuesNext(long id, long olid, long couid, long sbjid)
        {
            WhereClip wc = new WhereClip();
            if (olid > 0) wc.And(Questions._.Ol_ID == olid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            return Gateway.Default.From<Questions>().Where(wc && Questions._.Qus_ID < id)
                 .OrderBy(Questions._.Qus_ID.Desc).ToFirst<Questions>();
        }
        /// <summary>
        /// 当试题的上一个试题，在指定范围内取，例如课程内的试题
        /// </summary>
        /// <param name="id">试题id</param>
        /// <param name="olid">章节id</param>
        /// <param name="couid">课程id</param>
        /// <param name="sbjid">专业id</param>
        /// <returns></returns>
        public Questions QuesPrev(long id, long olid, long couid, long sbjid)
        {
            WhereClip wc = new WhereClip();
            if (olid > 0) wc.And(Questions._.Ol_ID == olid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            return Gateway.Default.From<Questions>().Where(wc && Questions._.Qus_ID > id)
                 .OrderBy(Questions._.Qus_ID.Asc).ToFirst<Questions>();
        }

        #endregion

        #region 试题导出
        /// <summary>
        /// 导出试题
        /// </summary>
        /// <param name="orgid">所属机构</param>
        /// <param name="type">试题类型，如单选，多选等</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="diff">难度等级，如1,2这样的字符串</param>
        /// <param name="isError">是否包括错误的试题，如果为空，则不作判断</param>
        /// <param name="isWrong">是否包括学员反馈的试题，如果为空，则不作判断</param>
        /// <returns></returns>
        public HSSFWorkbook QuestionsExport(string folder, int orgid, string type, long sbjid, long couid, long olid, string diff, bool? isError, bool? isWrong)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (olid > 0) wc.And(Questions._.Ol_ID == olid);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            //难度等级
            if (!string.IsNullOrWhiteSpace(diff))
            {
                WhereClip diffWc = new WhereClip();
                foreach (string s in diff.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;
                    int df = 0;
                    int.TryParse(s, out df);
                    if (df == 0) continue;
                    diffWc.Or(Questions._.Qus_Diff == df);
                }
                wc.And(diffWc);
            }
            //获取专业（带上级路径）与课程名称（带上级路径）
            string sbjname = string.Empty, couname = string.Empty;
            if (sbjid > 0) sbjname = Business.Do<ISubject>().SubjectName(sbjid);
            if (couid > 0) couname = Business.Do<ICourse>().CourseName(couid);
            //每页最多能放多少道题
            int pagesize = 30000;
            //试题类型，通过不同的试题类型返回工作表
            foreach (string t in type.Split(','))
            {
                int.TryParse(t, out int ty);
                if (ty == 0) continue;
                //计算有多少道题
                WhereClip where = (Questions._.Qus_Type == ty).And(wc);
                int total = Gateway.Default.Count<Questions>(where);       //当前题型的记录数
                int totalPages = (total + pagesize - 1) / pagesize;     //页数

                for (int idx = 1; idx <= totalPages; idx++)
                {
                    if (ty == 1) _buildExcelSql_1(hssfworkbook, where, sbjname, couname, folder, total, pagesize, idx);
                    if (ty == 2) _buildExcelSql_2(hssfworkbook, where, sbjname, couname, folder, total, pagesize, idx);
                    if (ty == 3) _buildExcelSql_3(hssfworkbook, where, sbjname, couname, folder, total, pagesize, idx);
                    if (ty == 4) _buildExcelSql_4(hssfworkbook, where, sbjname, couname, folder, total, pagesize, idx);
                    if (ty == 5) _buildExcelSql_5(hssfworkbook, where, sbjname, couname, folder, total, pagesize, idx);
                }
            }
            return hssfworkbook;
        }
        /// <summary>
        /// 导出试题,生成文件
        /// </summary>
        /// <param name="subpath">导出文件的路径</param>
        /// <param name="orgid"></param>
        /// <param name="type"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="diff"></param>
        /// <param name="isError"></param>
        /// <param name="isWrong"></param>       
        /// <returns></returns>
        public JObject QuestionsExportExcel(string subpath, int orgid, string type, long sbjid, long couid, long olid, string diff, bool? isError, bool? isWrong)
        {
            long snowid = WeiSha.Core.Request.SnowID();
            DateTime date = DateTime.Now;
            //导出文件的位置
            string path = Path.Combine(Upload.Get["Temp"].Physics, subpath, couid.ToString(), snowid.ToString());
            string filename = string.Format("试题导出.({0}).{1}.xls", date.ToString("yyyy-MM-dd hh-mm-ss"), couid.ToString());

            //导出Excel
            HSSFWorkbook hssfworkbook = this.QuestionsExport(path, orgid, type, sbjid, couid, olid, diff, isError, isWrong);
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
            }
            Directory.Delete(path, true);
            //
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + subpath + "/" + filename);
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
        /// <param name="sbjname"></param>
        /// <param name="couname"></param>
        /// <param name="folder">导出文件所在的文件</param>
        /// <param name="total"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        private void _buildExcelSql_1(HSSFWorkbook hssfworkbook, WhereClip where, string sbjname, string couname,string folder, int total, int size, int index)
        {
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(where).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>(size, (index - 1) * size);
            //创建工作簿对象
            string sheetname = "单选题";
            ISheet sheet = hssfworkbook.CreateSheet(total <= size ? sheetname : sheetname + "_" + index.ToString("D2"));
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
                IRow row = sheet.CreateRow(i + 1);
                QuesAnswer[] qas = this.QuestionsAnswer(q, null);
                int ansIndex = 0;

                for (int j = 0; j < qas.Length; j++)
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
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID,0, "Qus_Title", folder, q.Qus_Title));
                //专业,课程,章节
                if (string.IsNullOrWhiteSpace(sbjname)) sbjname = Business.Do<ISubject>().SubjectName(q.Sbj_ID);
                row.CreateCell(2).SetCellValue(sbjname);
                if (string.IsNullOrWhiteSpace(couname)) couname = Business.Do<ICourse>().CourseName(q.Cou_ID);
                row.CreateCell(3).SetCellValue(couname);
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(12).SetCellValue(ansIndex.ToString());
                //解析
                if (string.IsNullOrWhiteSpace(q.Qus_Explain) || q.Qus_Explain.Length <= _excel_field_max_length)
                    row.CreateCell(13).SetCellValue(q.Qus_Explain);
                else row.CreateCell(13).SetCellValue(_build_text(q.Qus_ID,0, "Qus_Explain", folder, q.Qus_Explain));

                i++;
            }
        }
        
        //多选题导出
        private void _buildExcelSql_2(HSSFWorkbook hssfworkbook,WhereClip where, string sbjname, string couname, string folder, int total, int size, int index)
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
                IRow row = sheet.CreateRow(i + 1);               
                QuesAnswer[] qas = this.QuestionsAnswer(q, null);
                string ansIndex = "";
                for (int j = 0; j < qas.Length; j++)
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
                if (string.IsNullOrWhiteSpace(sbjname))
                    sbjname = Business.Do<ISubject>().SubjectName(q.Sbj_ID);
                row.CreateCell(2).SetCellValue(sbjname);
                if (string.IsNullOrWhiteSpace(couname))
                    couname = Business.Do<ICourse>().CourseName(q.Cou_ID);
                row.CreateCell(3).SetCellValue(couname);
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
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
        private void _buildExcelSql_3(HSSFWorkbook hssfworkbook, WhereClip where, string sbjname, string couname, string folder, int total, int size, int index)
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
                string ans = "";
                if (Convert.ToString(q.Qus_IsCorrect) == "False") { ans = "错误"; } else { ans = "正确"; }
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(q.Qus_ID.ToString());
                //题干
                if (string.IsNullOrWhiteSpace(q.Qus_Title) || q.Qus_Title.Length <= _excel_field_max_length)
                    row.CreateCell(1).SetCellValue(q.Qus_Title);
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Title", folder, q.Qus_Title));
                //专业,课程,章节
                if (string.IsNullOrWhiteSpace(sbjname))
                    sbjname = Business.Do<ISubject>().SubjectName(q.Sbj_ID);
                row.CreateCell(2).SetCellValue(sbjname);
                if (string.IsNullOrWhiteSpace(couname))
                    couname = Business.Do<ICourse>().CourseName(q.Cou_ID);
                row.CreateCell(3).SetCellValue(couname);
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
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
        private void _buildExcelSql_4(HSSFWorkbook hssfworkbook, WhereClip where, string sbjname, string couname, string folder, int total, int size, int index)
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
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(q.Qus_ID.ToString());
                //题干
                if (string.IsNullOrWhiteSpace(q.Qus_Title) || q.Qus_Title.Length <= _excel_field_max_length)
                    row.CreateCell(1).SetCellValue(q.Qus_Title);
                else row.CreateCell(1).SetCellValue(_build_text(q.Qus_ID, 0, "Qus_Title", folder, q.Qus_Title));
                //专业,课程,章节
                if (string.IsNullOrWhiteSpace(sbjname))
                    sbjname = Business.Do<ISubject>().SubjectName(q.Sbj_ID);
                row.CreateCell(2).SetCellValue(sbjname);
                if (string.IsNullOrWhiteSpace(couname))
                    couname = Business.Do<ICourse>().CourseName(q.Cou_ID);
                row.CreateCell(3).SetCellValue(couname);
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
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
        private void _buildExcelSql_5(HSSFWorkbook hssfworkbook, WhereClip where, string sbjname, string couname, string folder, int total, int size, int index)
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
                IRow row = sheet.CreateRow(i + 1);               
                QuesAnswer[] qas = this.QuestionsAnswer(q, null);
                for (int j = 0; j < qas.Length; j++)
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
                if (string.IsNullOrWhiteSpace(sbjname))
                    sbjname = Business.Do<ISubject>().SubjectName(q.Sbj_ID);
                row.CreateCell(2).SetCellValue(sbjname);
                if (string.IsNullOrWhiteSpace(couname))
                    couname = Business.Do<ICourse>().CourseName(q.Cou_ID);
                row.CreateCell(3).SetCellValue(couname);
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
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


        #region 试题分类管理
        /// <summary>
        /// 添加试题类型
        /// </summary>
        /// <param name="entity">业务实体</param>
        public int TypeAdd(QuesTypes entity)
        {
            //如果没有排序号，则自动计算
            if (entity.Qt_Tax < 1)
            {
                object obj = Gateway.Default.Max<QuesTypes>(QuesTypes._.Qt_Tax, QuesTypes._.Cou_ID == entity.Cou_ID);
                entity.Qt_Tax = obj is int ? (int)obj + 1 : 0;
            }
            Gateway.Default.Save<QuesTypes>(entity);
            return entity.Qt_ID;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void TypeSave(QuesTypes entity)
        {
            Gateway.Default.Save<QuesTypes>(entity);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void TypeDelete(int identify)
        {
            Questions[] ques = Gateway.Default.From<Questions>().Where(Questions._.Qt_ID == identify).ToArray<Questions>();
            foreach (Questions q in ques)        
                QuesDelete(q);
        
            Gateway.Default.Delete<QuesTypes>(QuesTypes._.Qt_ID == identify);
        }
        /// <summary>
        /// 清理课程下的试题分类
        /// </summary>
        /// <param name="couid">课程id</param>
        public void TypeClear(long couid)
        {
            QuesTypes[] types = Gateway.Default.From<QuesTypes>().Where(QuesTypes._.Cou_ID == couid).ToArray<QuesTypes>();
            foreach (QuesTypes t in types)
            {
                TypeDelete(t.Qt_ID);
            }
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public QuesTypes TypeSingle(int identify)
        {
            Song.Entities.QuesTypes qus = Gateway.Default.From<QuesTypes>().Where(QuesTypes._.Qt_ID == identify).ToFirst<QuesTypes>();
            return qus;
        }
        /// <summary>
        /// 获取某个学科的所有试题分类
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="isUse">是否展示</param>
        /// <param name="count">取多少条，小于1取所有</param>
        /// <returns></returns>
        public QuesTypes[] TypeCount(long couid, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (couid > 0) wc.And(QuesTypes._.Cou_ID == couid);
            if (isUse != null) wc.And(QuesTypes._.Qt_IsUse == (bool)isUse);
            return Gateway.Default.From<QuesTypes>().Where(wc).OrderBy(QuesTypes._.Qt_Tax.Asc).ToArray<QuesTypes>(count);
        }
        /// <summary>
        /// 将当前项目向上移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool TypeRemoveUp(int id)
        {
            //当前对象
            QuesTypes current = Gateway.Default.From<QuesTypes>().Where(QuesTypes._.Qt_ID == id).ToFirst<QuesTypes>();
            int tax = (int)current.Qt_Tax;
            //上一个对象，即兄长对象；兄长不存则直接返回false;
            QuesTypes prev = Gateway.Default.From<QuesTypes>()
                .Where(QuesTypes._.Qt_Tax < tax && QuesTypes._.Cou_ID == current.Cou_ID)
                .OrderBy(QuesTypes._.Qt_Tax.Desc).ToFirst<QuesTypes>();
            if (prev == null) return false;
            //交换排序号
            current.Qt_Tax = prev.Qt_Tax;
            prev.Qt_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesTypes>(current);
                    tran.Save<QuesTypes>(prev);
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }              
            }
        }
        /// <summary>
        /// 将当前项目向下移动；仅在当前对象的同层移动，即同一父节点下的对象这前移动；
        /// </summary>
        /// <param name="id"></param>
        /// <returns>如果已经处于顶端，则返回false；移动成功，返回true</returns>
        public bool TypeRemoveDown(int id)
        {
            //当前对象
            QuesTypes current = Gateway.Default.From<QuesTypes>().Where(QuesTypes._.Qt_ID == id).ToFirst<QuesTypes>();
            int tax = (int)current.Qt_Tax;
            //下一个对象，即弟弟对象；弟弟不存则直接返回false;
            QuesTypes next = Gateway.Default.From<QuesTypes>()
                .Where(QuesTypes._.Qt_Tax > tax && QuesTypes._.Cou_ID == current.Cou_ID)
                .OrderBy(QuesTypes._.Qt_Tax.Asc).ToFirst<QuesTypes>();
            if (next == null) return false;
            //交换排序号
            current.Qt_Tax = next.Qt_Tax;
            next.Qt_Tax = tax;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<QuesTypes>(current);
                    tran.Save<QuesTypes>(next);
                    tran.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }               
            }
        }
        #endregion

        #region 试题答案或选项
        /// <summary>
        /// 将试题的答题选项转换为xml字符串
        /// </summary>
        /// <param name="ans"></param>
        /// <returns></returns>
        public string AnswerToItems(Song.Entities.QuesAnswer[] ans)
        {            
            if (ans == null || ans.Length < 1) return null;
            foreach (QuesAnswer qa in ans)
                qa.Ans_ID = qa.Ans_ID <= 0 ? WeiSha.Core.Request.SnowID() : qa.Ans_ID;
            //如果Ans_ID存在重复，则用雪花id赋值
            for (int i = 0; i < ans.Length; i++)
            {
                for (int j = 0; j < ans.Length; j++)
                {
                    if (i == j) continue;
                    if(ans[i].Ans_ID == ans[j].Ans_ID)                  
                        ans[i].Ans_ID = WeiSha.Core.Request.SnowID();                  
                }
            }

            XmlDocument xmlDoc = new XmlDocument();
            //创建根节点  
            XmlNode root = xmlDoc.CreateElement("Items");
            xmlDoc.AppendChild(root);
            //创建单个记录的节点
            foreach (Song.Entities.QuesAnswer a in ans)
            {
                root.AppendChild(_answerToItem(xmlDoc, a));
            }
            return xmlDoc.OuterXml;
        }
        /// <summary>
        /// 将单个答题选项转换为xml字符串
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private XmlNode _answerToItem(XmlDocument xmlDoc, Song.Entities.QuesAnswer ans)
        {
            XmlNode item = xmlDoc.CreateElement("item");
            Type t = ans.GetType();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, pi.Name, null);
                object obj = pi.GetValue(ans, null);
                if (pi.PropertyType.FullName == "System.String")
                {
                    XmlCDataSection cd = xmlDoc.CreateCDataSection(obj != null ? obj.ToString() : "");
                    node.AppendChild(cd);
                }
                else
                {
                    node.InnerText = obj != null ? obj.ToString() : "";
                }
                item.AppendChild(node);
            }
            return item;
        }
        /// <summary>
        /// 将答题选项的xml字符串，转换为QuesAnswer对象数组
        /// </summary>
        /// <param name="ques">当前试题</param>
        /// <param name="isCorrect">是否返回正确的选项，null返回全部，true只返回正确的答案，false只返回错误</param>
        /// <returns></returns>
        public Song.Entities.QuesAnswer[] ItemsToAnswer(Questions ques,bool? isCorrect)
        {
            string xml = ques.Qus_Items;
            if (string.IsNullOrWhiteSpace(xml)) return new QuesAnswer[0];
            XmlDocument xmlDoc = new XmlDocument();
            if (!string.IsNullOrWhiteSpace(xml)) xml = xml.Trim();          
            XmlNodeList list;
            try
            {
                xmlDoc.LoadXml(xml, false);
                list = xmlDoc.SelectNodes("Items/item");
            }
            catch (Exception ex)
            {
                WeiSha.Core.Log.Error(this.GetType().FullName, "当前试题ID：" + ques.Qus_ID.ToString());
                WeiSha.Core.Log.Error(this.GetType().FullName, ex);
                throw ex;
            }
            List<Song.Entities.QuesAnswer> anslist =new List<QuesAnswer>();
            for (int i = 0; i < list.Count; i++)
            {
                QuesAnswer ans = new QuesAnswer();
                Type t = ans.GetType();
                foreach (XmlNode n in list[i].ChildNodes)
                {
                    if (string.IsNullOrWhiteSpace(n.InnerText)) continue;
                    PropertyInfo pi = t.GetProperty(n.Name);
                    if (pi == null) continue;
                    object v = Convert.ChangeType(n.InnerText, pi.PropertyType);
                    t.GetProperty(n.Name).SetValue(ans, v, null);
                }
                if (isCorrect == null)
                {
                    anslist.Add(ans);
                }
                else
                {
                    if ((bool)isCorrect && ans.Ans_IsCorrect) anslist.Add(ans);
                    if (!(bool)isCorrect && !ans.Ans_IsCorrect) anslist.Add(ans);
                }
            }
            return anslist.ToArray();
        }
        /// <summary>
        /// 将答题选项的xml字符串，转换为QuesAnswer对象数组
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public Song.Entities.QuesAnswer ItemToAnswer(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml, false);
            XmlNode item = xmlDoc.FirstChild;
            QuesAnswer ans = new QuesAnswer();
            Type t = ans.GetType();
            foreach (XmlNode n in item.ChildNodes)
            {
                if (string.IsNullOrWhiteSpace(n.InnerText)) continue;
                object v = Convert.ChangeType(n.InnerText, t.GetProperty(n.Name).PropertyType);
                t.GetProperty(n.Name).SetValue(ans, v, null);
            }
            return ans;
        }
        #endregion

        #region 计算试题得分
        /// <summary>
        /// 计算当前试题的得分
        /// </summary>
        /// <param name="qid">试题的ID</param>
        /// <param name="ans">答案，选择题为id，判断题为数字，填空或简答为字符</param>
        /// <param name="num">该题的分数</param>
        /// <returns>正确返回true</returns>
        public bool ClacQues(long qid, string ans)
        {
            if (string.IsNullOrWhiteSpace(ans)) return false;
            if (ans.Trim() == "" || ans.Trim() == "undefined") return false;
            //
            Questions qus = null;           
            if (qus == null) qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == qid).ToFirst<Questions>();
            if (qus == null) return false;
            if (qus.Qus_Type == 1) return _clacQues1(qus, ans);
            if (qus.Qus_Type == 2) return _clacQues2(qus, ans);
            if (qus.Qus_Type == 3) return _clacQues3(qus, ans);
            if (qus.Qus_Type == 4) return _clacQues4(qus, ans);
            if (qus.Qus_Type == 5) return _clacQues5(qus, ans);
            return false;
        }
        /// <summary>
        /// 计算单选题
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private bool _clacQues1(Questions ques, string ans)
        {
            QuesAnswer[] ans1 = ItemsToAnswer(ques, true);            
            if (ans1.Length < 1) return false;
            foreach (string s in ans.Split(','))
            {
                if (string.IsNullOrWhiteSpace(s) || s.Trim() == "") continue;
                if (Convert.ToInt64(s) == ans1[0].Ans_ID) return true;
            }
            return false;
        }
        /// <summary>
        /// 计算多选题
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private bool _clacQues2(Questions ques, string ans)
        {
            QuesAnswer[] ans2 = ItemsToAnswer(ques, true); 
            if (ans2.Length < 1) return false;
            if (ans.Length > 0 && ans.Substring(ans.Length - 1) == ",")
                ans = ans.Substring(0, ans.Length - 1);
            string[] ansArr = ans.Split(',');
            if (ansArr.Length != ans2.Length) return false;
            int tm = ansArr.Length;
            foreach (string s in ansArr)
            {
                if (string.IsNullOrWhiteSpace(s) || s.Trim() == "") continue;
                foreach (QuesAnswer qa in ans2)
                {
                    if (Convert.ToInt64(s) == qa.Ans_ID)
                    {
                        tm--;
                        break;
                    }
                }
            }
            return tm == 0;
        }
        /// <summary>
        /// 计算判断题
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private bool _clacQues3(Questions ques, string ans)
        {
            ans = ans.Replace(",", "");
            if (Convert.ToInt64(ans) == 0 && ques.Qus_IsCorrect == true) return true;
            if (Convert.ToInt64(ans) == 1 && ques.Qus_IsCorrect == false) return true;
            return false;
        }
        /// <summary>
        /// 计算简答题
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private bool _clacQues4(Questions ques, string ans)
        {
            if (string.IsNullOrWhiteSpace(ans)) return false;
            if (string.IsNullOrWhiteSpace(ques.Qus_Answer)) return false;
            ans = WeiSha.Core.HTML.ClearTag(ans);
            ques.Qus_Answer = WeiSha.Core.HTML.ClearTag(ques.Qus_Answer);
            if (ans.Trim() == "" || ques.Qus_Answer.Trim()=="") return false;
            return ans.Equals(ques.Qus_Answer, StringComparison.OrdinalIgnoreCase);          
        }
        /// <summary>
        /// 填空题
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private bool _clacQues5(Questions ques, string ans)
        {
            QuesAnswer[] ans5 = ItemsToAnswer(ques, null); 
            if (ans5.Length < 1) return false;
            if (ans.Length > 0 && ans.Substring(ans.Length - 1) == ",")
                ans = ans.Substring(0, ans.Length - 1);
            string[] ansText = ans.Split(',');
            int corrNum = 0;
            for (int j = 0; j < ansText.Length; j++)
            {
                if (ansText[j].Trim() == "") continue;
                if (ans5.Length <= j || ans5[j] == null) continue;
                string corentTxt = WeiSha.Core.HTML.ClearTag(ans5[j].Ans_Context);
                foreach (string tm in corentTxt.Split(','))
                {
                    if (tm == string.Empty || tm.Trim() == "") continue;
                    //if (tm.Trim() == ansText[j].Trim())
                    //{
                    //    corrNum++;
                    //    break;
                    //}
                    if (tm.Equals(ansText[j],StringComparison.OrdinalIgnoreCase))
                    {
                        corrNum++;
                        break;
                    }
                }
            }
            return corrNum == ans5.Length;
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 清理字符中的非法字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string _ClearString(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return html;
            RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
            //删除脚本
            html = Regex.Replace(html, @"<script[^>]+?>[\s\S]*?</script>", "", option);
            html = Regex.Replace(html, @"<script[^>]*>[\s\S]*?</script>", "", option);
            ////删除HTML
            ////html = Regex.Replace(html, @"<(.[^>]*)>", "", option);
            //html = Regex.Replace(html, @"([\r\n])[\s]+", "", option);
            //html = Regex.Replace(html, @"-->", "", option);
            //html = Regex.Replace(html, @"<!--.*", "", option);
            //html = Regex.Replace(html, @"&(quot|#34);", "\"", option);
            //html = Regex.Replace(html, @"&(amp|#38);", "&", option);
            ////html = Regex.Replace(html, @"&(lt|#60);", "<", option);
            ////html = Regex.Replace(html, @"&(gt|#62);", ">", option);
            //html = Regex.Replace(html, @"&(nbsp|#160);", " ", option);
            //html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", option);
            //html = Regex.Replace(html, @"&(cent|#162);", "\xa2", option);
            //html = Regex.Replace(html, @"&(pound|#163);", "\xa3", option);
            //html = Regex.Replace(html, @"&(copy|#169);", "\xa9", option);
            //html = Regex.Replace(html, @"&#(\d+);", "", option);

            html = Regex.Replace(html, @"//\(function\(\)[\s\S]+?}\)\(\);", "", option);
            //html = html.Replace("<", "&lt;");
            //html = html.Replace(">", "&gt;");
            html = html.Replace("\r", "");
            html = html.Replace("\n", "");
            return html;
        }
        #endregion


        #region 事件
        public event EventHandler Save;
        public event EventHandler Add;
        public event EventHandler Delete;
        public void OnSave(object sender, EventArgs e)
        {
            if (Save != null) Save(sender, e);
        }
        public void OnAdd(object sender, EventArgs e)
        {           
            //更新章节试题数量
            if (!(sender is Questions)) return;
            Questions ques = (Questions)sender;
            if (ques == null) return;

            //更新章节试题数
            new Task(() =>
            {
                //更新试题统计数
                Business.Do<IQuestions>().QuesCountUpdate(ques.Org_ID, ques.Sbj_ID, ques.Cou_ID, ques.Ol_ID);
            }).Start();
            if (Add != null) Add(sender, e);
        }
        public void OnDelete(object sender, EventArgs e)
        {
            //如果是试题
            if (sender is Questions)
            {

                Questions ques = (Questions)sender;
                //删除笔记与收藏
                new Task(() =>
                {
                    Gateway.Default.Delete<Student_Notes>(Student_Notes._.Qus_ID == ques.Qus_ID);
                    Gateway.Default.Delete<Student_Collect>(Student_Collect._.Qus_ID == ques.Qus_ID);
                    Gateway.Default.Delete<Student_Ques>(Student_Ques._.Qus_ID == ques.Qus_ID);
                }).Start();
                //删除图片等资源
                new Task(() =>
                {
                    WeiSha.Core.Upload.Get["Ques"].DeleteDirectory(ques.Qus_ID.ToString());
                }).Start();                
            }

            //如果是试题ID的数组
            if (sender is long[])
            {
                long[] ids = (long[])sender;

            }

            if (Delete != null) Delete(sender, e);
        }
        #endregion


        #region 试题练习的记录
        /// <summary>
        /// 记录试题练习记录
        /// </summary>
        /// <param name="acc"></param>
        /// <param name="orgid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public bool ExerciseLogSave(Accounts acc, int orgid, long couid, long olid, string json, int sum, int answer, int correct, int wrong, double rate)
        {
            if (olid <= 0 || acc == null) return false;
            //new Task(() =>
            //{
                if (orgid <= 0)
                {
                    Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                    if (org != null) orgid = org.Org_ID;
                }
                LogForStudentExercise log = this.ExerciseLogGet(acc.Ac_ID, couid, olid);
                if (log == null)
                {
                    log = new LogForStudentExercise();
                    log.Lse_CrtTime = DateTime.Now;                    
                }
                if (acc != null)
                {
                    log.Ac_ID = acc.Ac_ID;
                    log.Ac_Name = acc.Ac_Name;
                    log.Ac_AccName = acc.Ac_AccName;
                }
                log.Cou_ID = couid;
                log.Org_ID = orgid;
                log.Ol_ID = olid;
                log.Lse_LastTime = DateTime.Now;
                //客户端信息
                //log.Lse_IP = WeiSha.Core.Browser.IP;
                //log.Lse_OS = WeiSha.Core.Browser.OS;
                //log.Lse_Browser = WeiSha.Core.Browser.Name + " " + WeiSha.Core.Browser.Version;
                //log.Lse_Platform = WeiSha.Core.Browser.IsMobile ? "Mobi" : "PC";
                //统计信息
                log.Lse_Answer = answer;
                log.Lse_Correct = correct;
                log.Lse_Rate = (Decimal)rate;
                log.Lse_Sum = sum;
                log.Lse_Wrong = wrong;
                //答题信息的详情，以json方式存储
                log.Lse_JsonData = json;
                Gateway.Default.Save<LogForStudentExercise>(log);
            //}).Start();
            return true;
        }
        /// <summary>
        /// 获取试题练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        public LogForStudentExercise ExerciseLogGet(int acid, long couid, long olid)
        {
            if (acid <= 0 || olid <= 0) return null;
            WhereClip wc = new WhereClip();
            wc.And(LogForStudentExercise._.Ac_ID == acid);
            wc.And(LogForStudentExercise._.Ol_ID == olid);
            if (couid > 0)           
                wc.And(LogForStudentExercise._.Cou_ID == couid);          
            return Gateway.Default.From<LogForStudentExercise>().Where(wc).ToFirst<LogForStudentExercise>();
        }
        /// <summary>
        /// 删除试题练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        public bool ExerciseLogDel(int acid, long couid, long olid)
        {
            if (acid <= 0 || olid <= 0) return false;
            new Task(() =>
            {               
                WhereClip wc = new WhereClip();
                wc.And(LogForStudentExercise._.Ac_ID == acid);
                wc.And(LogForStudentExercise._.Ol_ID == olid);
                if (couid > 0)
                    wc.And(LogForStudentExercise._.Cou_ID == couid);              
                Gateway.Default.Delete<LogForStudentExercise>(wc);
            }).Start();          
            return true;
        }

        #endregion

        #region 统计信息
        /// <summary>
        /// 试题资源的存储大小
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="count">资源个数</param>
        /// <returns>资源文件总大小，单位为字节</returns>
        public long StorageResources(int orgid, long sbjid, long couid, long olid, out int count)
        {
            string PhyPath = WeiSha.Core.Upload.Get["Ques"].Physics;
            //文件总大小
            long totalSize = 0;
            int totalCount = 0; //数量
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (olid > 0) wc.And(Questions._.Ol_ID == olid);
            using (SourceReader reader = Gateway.Default.From<Questions>().Where(wc).ToReader())
            {
                while (reader.Read())
                {
                    long id = (long)reader["Qus_ID"];                  
                    string dir = PhyPath + id.ToString();
                    if (!Directory.Exists(dir)) continue;
                    foreach(string file in Directory.GetFiles(dir))
                    {
                        totalSize += new FileInfo(file).Length;
                        totalCount++;
                    }                  

                }
                reader.Close();
                reader.Dispose();

            }
            count = totalCount; //总数
            return totalSize;
        }
        #endregion
    }
}
