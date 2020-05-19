using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using WeiSha.Common;
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



namespace Song.ServiceImpls
{
    public class QuestionsCom : IQuestions
    {
         
        #region 试题管理

        public int QuesAdd(Questions entity)
        {
            entity.Qus_CrtTime = DateTime.Now;
            entity.Qus_LastTime = DateTime.Now;
            entity.Qus_Title = _ClearString(entity.Qus_Title);
            entity.Qus_Answer = _ClearString(entity.Qus_Answer);
            entity.Qus_Explain = _ClearString(entity.Qus_Explain);
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;  
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
                if (entity.Qus_Answer == string.Empty || entity.Qus_Answer.Trim() == "") entity.Qus_IsError = true;
            }
            entity.Qus_Explain = _ClearString(entity.Qus_Explain);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<Questions>(entity);
                    tran.Update<QuesAnswer>(new Field[] { QuesAnswer._.Qus_ID }, new object[] { entity.Qus_ID }, QuesAnswer._.Qus_UID == entity.Qus_UID);
                    tran.Commit();
                    QuestionsMethod.QuestionsCache.Singleton.UpdateSingle(entity);
                    this.OnSave(entity, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    tran.Close();
                }
            }
        }

        public void QuesInput(Questions entity, List<Song.Entities.QuesAnswer> ansItem)
        {
            if (entity.Qus_ID < 1) entity.Qus_CrtTime = DateTime.Now;
            entity.Qus_Title = _ClearString(entity.Qus_Title);
            entity.Qus_Answer = _ClearString(entity.Qus_Answer);
            entity.Qus_Explain = _ClearString(entity.Qus_Explain);
            //答题选项的处理
            if (ansItem != null)
            {
                for (int i = 0; i < ansItem.Count; i++)
                {
                    //添加随机的选择项id
                    Random rd = new Random((i + 1) * DateTime.Now.Millisecond);
                    ansItem[i].Ans_ID = rd.Next(1, 1000);
                    //如果有试题id，则加上，好像也无所谓
                    if (entity.Qus_ID > 0) ansItem[i].Qus_ID = entity.Qus_ID;
                }
                entity.Qus_Items = this.AnswerToItems(ansItem.ToArray());
            }
            Gateway.Default.Save<Questions>(entity);
        }
        public void QuesDelete(int identify)
        {
            Song.Entities.Questions entity = this.QuesSingle(identify);
            if (entity == null) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                QuesDelete(entity, tran);
            }
        }
        private void QuesDelete(Questions entity, DbTrans tran)
        {
            try
            {
                if (entity != null)
                {
                    tran.Delete<Questions>(Questions._.Qus_ID == entity.Qus_ID);
                    tran.Delete<QuesAnswer>(QuesAnswer._.Qus_ID == entity.Qus_ID);
                    tran.Delete<QuesAnswer>(QuesAnswer._.Qus_UID == entity.Qus_UID);
                    tran.Delete<Student_Notes>(Student_Notes._.Qus_ID == entity.Qus_ID);
                    tran.Delete<Student_Collect>(Student_Collect._.Qus_ID == entity.Qus_ID);
                    tran.Commit();
                }
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                tran.Close();
            }
            this.OnDelete(entity, null);
        }
        public void QuesDelete(string ids)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    foreach (string idstr in ids.Split(','))
                    {
                        int intid = 0;
                        int.TryParse(idstr, out intid);
                        if (intid < 1) continue;
                        tran.Delete<Questions>(Questions._.Qus_ID == intid);
                        tran.Delete<QuesAnswer>(QuesAnswer._.Qus_ID == intid);
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;

                }
                finally
                {
                    tran.Close();
                }
            }
            this.OnDelete(null, null);
        }
        public Questions QuesSingle(int identify)
        {
            Song.Entities.Questions qus = QuestionsMethod.QuestionsCache.Singleton.GetSingle(identify);
            if (qus == null) qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == identify).ToFirst<Questions>();
            if (qus == null) return qus;            
            return qus;

        }
        /// <summary>
        ///  获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="cache">是否来自缓存</param>
        /// <returns></returns>
        public Questions QuesSingle(int identify, bool cache)
        {
            if (cache) return this.QuesSingle(identify);
            return Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == identify).ToFirst<Questions>();
        }
        public Questions QuesSingle(string uid)
        {
            if (uid == string.Empty) return null;
            Song.Entities.Questions qus = QuestionsMethod.QuestionsCache.Singleton.GetSingle(uid);
            if (qus == null) qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_UID == uid.Trim() && Questions._.Qus_IsTitle == true).ToFirst<Questions>();
            if (qus == null) return qus;
           
            return qus;
        }

        public Questions QuesSingle(string titile, int type)
        {
            if (titile == string.Empty) return null;
            WhereClip wc = new WhereClip();
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (titile.Trim() != "") wc.And(Questions._.Qus_Title.Like(titile.Trim()));
            Song.Entities.Questions qus = Gateway.Default.From<Questions>().Where(wc).ToFirst<Questions>();
            if (qus == null) return qus;           
            return qus;
        }

        public QuesAnswer[] QuestionsAnswer(Questions qus, bool? isCorrect)
        {
            Song.Entities.QuesAnswer[] ans = this.ItemsToAnswer(qus.Qus_Items, isCorrect);
            return ans;
        }

        public Questions[] QuesCount(int type, bool? isUse, int count)
        {
            WhereClip wc = Questions._.Qus_IsError == false && Questions._.Qus_IsWrong == false;
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Type.Asc && Questions._.Qus_Tax.Asc && Questions._.Qus_ID.Asc)
                .ToArray<Questions>(count);
        }
        public Questions[] QuesCount(int orgid, int sbjid, int couid, int olid, int type, int diff, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<IOutline>().TreeID(olid);
                foreach (int l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);
            }
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (diff > 0) wc.And(Questions._.Qus_Diff == diff);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Type.Asc && Questions._.Qus_Tax.Asc && Questions._.Qus_ID.Asc)
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
        public Questions[] QuesCount(int orgid, int sbjid, int couid, int olid, int type, int diff, bool? isUse, int index, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {                
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<IOutline>().TreeID(olid);
                foreach (int l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);               
            }
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (diff > 0) wc.And(Questions._.Qus_Diff == diff);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Type.Asc && Questions._.Qus_Tax.Asc && Questions._.Qus_ID.Asc)
                .ToArray<Questions>(count, index);
        }
        /// <summary>
        /// 计算试题数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题类型</param>
        /// <param name="isUse">是否使用</param>
        /// <returns></returns>
        public int QuesOfCount(int orgid, int sbjid, int couid, int olid, int type, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<IOutline>().TreeID(olid);
                foreach (int l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);
            }
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.Count<Questions>(wc);
        }
        public int QuesOfCount(int orgid, int sbjid, int couid, int olid, int type, int diff, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (sbjid > 0) wc.And(Questions._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<IOutline>().TreeID(olid);
                foreach (int l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);
            }
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (diff > 0) wc.And(Questions._.Qus_Diff == diff);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            return Gateway.Default.Count<Questions>(wc);
        }
        public DataSet QuesAns(int identify)
        {
            return Gateway.Default.From<Questions>().InnerJoin<QuesAnswer>(QuesAnswer._.Qus_UID == Questions._.Qus_UID)
                .Where(Questions._.Qus_ID == identify).OrderBy(QuesAnswer._.Ans_ID.Asc).ToDataSet();
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
        public Questions[] QuesRandom(int orgid, int sbjid, int couid, int olid, int type, int diff1, int diff2, bool? isUse, int count)
        {
            #region 
            //试题类型
            string[] types = WeiSha.Common.App.Get["QuesType"].Split(',');
            if (type < 1 || type > types.Length) type = -1;
            //难度区间
            diff1 = diff1 < 1 ? 1 : diff1;
            diff2 = diff2 < 1 || diff2 > 5 ? 5 : diff2;
            //基本属性
            string where = " Qus_IsError=false and org_id=" + orgid + " ";
            where = string.Format(where, diff1, diff2);
            if (type > 0) where += " and Qus_Type=" + type; //试题类型
            if (diff1 > 0) where += " and Qus_Diff>=" + diff1;  //最小难度等级
            if (diff2 > 0) where += " and Qus_Diff<=" + diff2;  //最大难度
            if (isUse != null) where += " and Qus_IsUse=" + ((bool)isUse).ToString().ToLower(); //是否包括未使用的试题，true为只限启用的试题                     
            if (olid > 0)
            {
                //章节id
                string olstr = _quesRandom_buildOlid(olid);
                where += " and (Ol_ID=" + olid + olstr + ")";
            }
            else
            {
                if (couid > 0)
                {
                    where += " and Cou_ID=" + couid; //课程
                }
                else
                {
                    if (sbjid > 0) where += " and Sbj_ID=" + sbjid; //专业id   
                }
            }
            //根据不同的数据库拼接SQL语句
            string sql = "";
            string dataype = WeiSha.Common.Server.DatabaseType; //数据库类型
            if (dataype != "access")
            {
                sql = "select top " + count + "  *, newid() as n from Questions where " + where + " order by n";
                sql = sql.Replace("true", "1");
                sql = sql.Replace("false", "0");
            }
            else
            {
                int rdid = new System.Random(unchecked((int)DateTime.Now.Ticks)).Next();
                sql = "select top " + count + " * from Questions where " + where + " order by rnd(" + (-1 * rdid) + "*Qus_ID)";
                sql = "select * from (" + sql + ") as t order by t.Qus_Type asc";
                return Gateway.Default.FromSql(sql).ToArray<Questions>();
            }
            sql = "select * from (" + sql + ") as t order by t.Qus_Type asc";
            #endregion
            //
            //string sqlProc = "exec PROC_QuesRandom {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}";
            //sqlProc = string.Format(sqlProc, orgid, sbjid, couid, olid, type, diff1, diff2, 1, count);
            return Gateway.Default.FromSql(sql).ToArray<Questions>();
        }
        /// <summary>
        /// 为了获取章节下面的子章节中的试题，生成相关条件判断
        /// </summary>
        /// <param name="olid"></param>
        /// <returns></returns>
        private string _quesRandom_buildOlid(int olid)
        {
            string sql = "";
            Outline[] ols = Gateway.Default.From<Outline>().Where(Outline._.Ol_PID == olid).ToArray<Outline>();
            foreach (Outline o in ols)
            {
                sql += " or Ol_ID=" + o.Ol_ID;
                sql += _quesRandom_buildOlid(o.Ol_ID);
            }
            return sql;
        }
        
        public Questions[] QuesRandom(int type, int sbjId, int couid, int diff1, int diff2, bool? isUse, int count)
        {
            return this.QuesRandom(-1, sbjId, couid, -1, type, diff1, diff2, isUse, count);           
        }
        public Questions[] QuesPager(int orgid, int type, bool? isUse, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (!string.IsNullOrWhiteSpace(searTxt) && searTxt.Trim() != "")
            {
                wc.And(Questions._.Qus_Title.Like("%" + searTxt.Trim() + "%"));
            }
            countSum = Gateway.Default.Count<Questions>(wc);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Tax.Asc && Questions._.Qus_ID.Desc)
                .ToArray<Questions>(size, (index - 1) * size);
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
                wc.And(Questions._.Qus_Title.Like("%" + searTxt.Trim() + "%"));
            countSum = Gateway.Default.Count<Questions>(wc);
            return Gateway.Default.From<Questions>()
                .Where(wc).OrderBy(Questions._.Qus_Tax.Asc && Questions._.Qus_ID.Desc)
                .ToArray<Questions>(size, (index - 1) * size);
        }

        public Questions[] QuesPager(int orgid, int type, int sbjId, int couid, int olid, bool? isUse,
            bool? isError, bool? isWrong, int diff, string searTxt,
            int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (type > 0) wc.And(Questions._.Qus_Type == type);
            if (sbjId > 0) wc.And(Questions._.Sbj_ID == sbjId);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            //当前章节，以及当前章节之下的所有试题
            if (olid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<int> list = Business.Do<IOutline>().TreeID(olid);
                foreach (int l in list)
                    wcSbjid.Or(Questions._.Ol_ID == l);
                wc.And(wcSbjid);
            }
            if (isUse != null) wc.And(Questions._.Qus_IsUse == (bool)isUse);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            if (diff > 0 && diff <= 5) wc.And(Questions._.Qus_Diff == diff);
            if (!string.IsNullOrWhiteSpace(searTxt) && searTxt.ToLower() != "")
            {
                wc.And(Questions._.Qus_Title.Like("%" + searTxt.Trim() + "%"));
            }
            countSum = Gateway.Default.Count<Questions>(wc);
            return Gateway.Default.From<Questions>().Where(wc)
                .OrderBy(Questions._.Qus_Tax.Asc && Questions._.Qus_ID.Desc)
                .ToArray<Questions>(size, (index - 1) * size);
        }
        #endregion

        #region 试题导出
        /// <summary>
        /// 导出试题
        /// </summary>
        /// <param name="orgid">所属机构</param>
        /// <param name="type">试题类型，如单选，多选等</param>
        /// <param name="sbjId">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="diff">难度等级，如1,2这样的字符串</param>
        /// <param name="isError">是否包括错误的试题，如果为空，则不作判断</param>
        /// <param name="isWrong">是否包括学员反馈的试题，如果为空，则不作判断</param>
        /// <returns></returns>
        public HSSFWorkbook QuestionsExport(int orgid, string type, int sbjId, int couid, int olid, string diff, bool? isError, bool? isWrong)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            WhereClip wc = new WhereClip();
            if (orgid > -1) wc.And(Questions._.Org_ID == orgid);
            if (sbjId > 0) wc.And(Questions._.Sbj_ID == sbjId);
            if (couid > 0) wc.And(Questions._.Cou_ID == couid);
            if (olid > 0) wc.And(Questions._.Ol_ID == olid);
            if (isError != null) wc.And(Questions._.Qus_IsError == (bool)isError);
            if (isWrong != null) wc.And(Questions._.Qus_IsWrong == (bool)isWrong);
            //难度等级
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
            //试题类型，通过不同的试题类型返回工作表
            foreach (string t in type.Split(','))
            {
                if (string.IsNullOrWhiteSpace(t)) continue;
                int ty = 0;
                int.TryParse(t, out ty);
                if (ty == 0) continue;                
                if (ty == 1) _buildExcelSql_1(hssfworkbook, wc);
                if (ty == 2) _buildExcelSql_2(hssfworkbook, wc);
                if (ty == 3) _buildExcelSql_3(hssfworkbook, wc);
                if (ty == 4) _buildExcelSql_4(hssfworkbook, wc);
                if (ty == 5) _buildExcelSql_5(hssfworkbook, wc);
            }
            return hssfworkbook;
        }
        private void _buildExcelSql_1(HSSFWorkbook hssfworkbook, WhereClip where)
        {
            WhereClip wc = (Questions._.Qus_Type == 1).And(where);
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(wc).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>();
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("单选题");
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题目", "专业", "课程", "章节", "难度", "答案选项1", "答案选项2", "答案选项3", "答案选项4", "答案选项5", "答案选项6", "正确答案", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                IRow row = sheet.CreateRow(i + 1);
                string tmpVal = "";
                QuesAnswer[] qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                int ansIndex = 0;
                for (int j = 0; j < qas.Length; j++)
                {
                    QuesAnswer c = qas[j];
                    tmpVal = c.Ans_Context;
                    row.CreateCell(6 + j).SetCellValue(tmpVal);
                    if (c.Ans_IsCorrect)
                        ansIndex = j + 1;
                }
                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                //专业,课程,章节
                row.CreateCell(2).SetCellValue(Business.Do<ISubject>().SubjectName(q.Sbj_ID));                
                row.CreateCell(3).SetCellValue(Business.Do<ICourse>().CourseName(q.Cou_ID));                
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(12).SetCellValue(ansIndex.ToString());
                row.CreateCell(13).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //多选题导出
        private void _buildExcelSql_2(HSSFWorkbook hssfworkbook,WhereClip where)
        {
            WhereClip wc = (Questions._.Qus_Type == 2).And(where);
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(wc).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>();
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("多选题");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题目", "专业", "课程", "章节", "难度", "答案选项1", "答案选项2", "答案选项3", "答案选项4", "答案选项5", "答案选项6", "正确答案", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);            
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                IRow row = sheet.CreateRow(i + 1);
                string tmpVal = "";
                QuesAnswer[] qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                string ansIndex = "";
                for (int j = 0; j < qas.Length; j++)
                {
                    QuesAnswer c = qas[j];
                    tmpVal = c.Ans_Context;
                    row.CreateCell(6 + j).SetCellValue(tmpVal);
                    if (c.Ans_IsCorrect)
                        ansIndex += Convert.ToString(j + 1) + ",";
                }
                if (ansIndex.Length > 0)
                {
                    if (ansIndex.Substring(ansIndex.Length - 1) == ",")
                        ansIndex = ansIndex.Substring(0, ansIndex.Length - 1);
                }

                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                //专业,课程,章节
                row.CreateCell(2).SetCellValue(Business.Do<ISubject>().SubjectName(q.Sbj_ID));
                row.CreateCell(3).SetCellValue(Business.Do<ICourse>().CourseName(q.Cou_ID));
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(12).SetCellValue(ansIndex.ToString());
                row.CreateCell(13).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //判断题导出
        private void _buildExcelSql_3(HSSFWorkbook hssfworkbook, WhereClip where)
        {
            WhereClip wc = (Questions._.Qus_Type == 3).And(where);
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(wc).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>();
             //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("判断题");
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题目", "专业", "课程", "章节", "难度", "答案", "试题讲解" };
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
                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                //专业,课程,章节
                row.CreateCell(2).SetCellValue(Business.Do<ISubject>().SubjectName(q.Sbj_ID));
                row.CreateCell(3).SetCellValue(Business.Do<ICourse>().CourseName(q.Cou_ID));
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(6).SetCellValue(ans);
                row.CreateCell(7).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //简答题导出
        private void _buildExcelSql_4(HSSFWorkbook hssfworkbook, WhereClip where)
        {
            WhereClip wc = (Questions._.Qus_Type == 4).And(where);
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(wc).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>();
            //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("简答题");
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题目", "专业", "课程", "章节", "难度", "答案", "试题讲解" };
            for (int h = 0; h < cells.Length; h++)
                rowHead.CreateCell(h).SetCellValue(cells[h]);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            int i = 0;
            foreach (Song.Entities.Questions q in ques)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                //专业,课程,章节
                row.CreateCell(2).SetCellValue(Business.Do<ISubject>().SubjectName(q.Sbj_ID));
                row.CreateCell(3).SetCellValue(Business.Do<ICourse>().CourseName(q.Cou_ID));
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(6).SetCellValue(q.Qus_Answer);
                row.CreateCell(7).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
        //填空题导出
        private void _buildExcelSql_5(HSSFWorkbook hssfworkbook, WhereClip where)
        {
            WhereClip wc = (Questions._.Qus_Type == 5).And(where);
            Song.Entities.Questions[] ques = Gateway.Default.From<Questions>().Where(wc).OrderBy(Questions._.Qus_ID.Asc).ToArray<Questions>();
             //创建工作簿对象
            ISheet sheet = hssfworkbook.CreateSheet("填空题");
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            //创建表头
            string[] cells = new string[] { "ID", "题目", "专业", "课程", "章节", "难度", 
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
                string tmpVal = "";
                QuesAnswer[] qas = Business.Do<IQuestions>().QuestionsAnswer(q, null);
                for (int j = 0; j < qas.Length; j++)
                {
                    QuesAnswer c = qas[j];
                    tmpVal = c.Ans_Context;
                    row.CreateCell(6 + j).SetCellValue(tmpVal);
                }

                row.CreateCell(0).SetCellValue(q.Qus_ID);
                row.CreateCell(1).SetCellValue(q.Qus_Title);
                //专业,课程,章节
                row.CreateCell(2).SetCellValue(Business.Do<ISubject>().SubjectName(q.Sbj_ID));
                row.CreateCell(3).SetCellValue(Business.Do<ICourse>().CourseName(q.Cou_ID));
                row.CreateCell(4).SetCellValue(Business.Do<IOutline>().OutlineName(q.Ol_ID));
                row.CreateCell(5).SetCellValue((int)q.Qus_Diff);
                row.CreateCell(12).SetCellValue(q.Qus_Explain);
                i++;
            }
        }
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
            {
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    QuesDelete(q, tran);
                }
            }
            Gateway.Default.Delete<QuesTypes>(QuesTypes._.Qt_ID == identify);
        }
        /// <summary>
        /// 清理课程下的试题分类
        /// </summary>
        /// <param name="couid">课程id</param>
        public void TypeClear(int couid)
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
        public QuesTypes[] TypeCount(int couid, bool? isUse, int count)
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

        #region 试题答案或选项
        /// <summary>
        /// 将试题的答题选项转换为xml字符串
        /// </summary>
        /// <param name="ans"></param>
        /// <returns></returns>
        public string AnswerToItems(Song.Entities.QuesAnswer[] ans)
        {
            if (ans == null || ans.Length < 1) return null;
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
        /// <param name="xml"></param>
        /// <param name="isCorrect">是否返回正确的选项，null返回全部，true只返回正确的答案，false只返回错误</param>
        /// <returns></returns>
        public Song.Entities.QuesAnswer[] ItemsToAnswer(string xml, bool? isCorrect)
        {
            if (string.IsNullOrWhiteSpace(xml)) return new QuesAnswer[0];
            XmlDocument xmlDoc = new XmlDocument();
            if (!string.IsNullOrWhiteSpace(xml)) xml = xml.Trim();
            xmlDoc.LoadXml(xml, false);
            XmlNodeList list = xmlDoc.SelectNodes("Items/item");
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
        /// <param name="id">试题的ID</param>
        /// <param name="ans">答案，选择题为id，判断题为数字，填空或简答为字符</param>
        /// <param name="num">该题的分数</param>
        /// <returns>正确返回true</returns>
        public bool ClacQues(int id, string ans)
        {
            if (string.IsNullOrWhiteSpace(ans)) return false;
            if (ans.Trim() == "" || ans.Trim() == "undefined") return false;
            //
            Questions qus = null;
            qus = this.QuesSingle4Cache(id);
            if (qus == null) qus = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == id).ToFirst<Questions>();
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
            QuesAnswer[] ans1 = ItemsToAnswer(ques.Qus_Items, true);            
            if (ans1.Length < 1) return false;
            foreach (string s in ans.Split(','))
            {
                if (string.IsNullOrWhiteSpace(s) || s.Trim() == "") continue;
                if (Convert.ToInt32(s) == ans1[0].Ans_ID) return true;
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
            QuesAnswer[] ans2 = ItemsToAnswer(ques.Qus_Items, true); 
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
                    if (Convert.ToInt32(s) == qa.Ans_ID)
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
            if (Convert.ToInt32(ans) == 0 && ques.Qus_IsCorrect == true) return true;
            if (Convert.ToInt32(ans) == 1 && ques.Qus_IsCorrect == false) return true;
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
            if (ans.Trim() == "") return false;
            return false;
        }
        /// <summary>
        /// 填空题
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="ans"></param>
        /// <returns></returns>
        private bool _clacQues5(Questions ques, string ans)
        {
            QuesAnswer[] ans5 = ItemsToAnswer(ques.Qus_Items, null); 
            if (ans5.Length < 1) return false;
            if (ans.Length > 0 && ans.Substring(ans.Length - 1) == ",")
                ans = ans.Substring(0, ans.Length - 1);
            string[] ansText = ans.Split(',');
            int corrNum = 0;
            for (int j = 0; j < ansText.Length; j++)
            {
                if (ansText[j].Trim() == "") continue;
                if (ans5.Length <= j || ans5[j] == null) continue;
                string corentTxt = ans5[j].Ans_Context;
                foreach (string tm in corentTxt.Split(','))
                {
                    if (tm == string.Empty || tm.Trim() == "") continue;
                    if (tm.Trim() == ansText[j].Trim())
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
            //删除脚本
            html = Regex.Replace(html, @"<script[^>]+?>[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            //html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            //html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            html = Regex.Replace(html, @"//\(function\(\)[\s\S]+?}\)\(\);", "", RegexOptions.IgnoreCase);
            //html = html.Replace("<", "&lt;");
            //html = html.Replace(">", "&gt;");
            html = html.Replace("\r", "");
            html = html.Replace("\n", "");
            return html;
        }
        #endregion

        #region 缓存管理
        /// <summary>
        /// 添加试题缓存
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public string CacheAdd(Questions[] ques, int expires)
        {
            return QuestionsMethod.QuestionsCache.Singleton.Add(ques,expires);
        }
        public string CacheAdd(Questions[] ques, int expires, string uid)
        {
            return QuestionsMethod.QuestionsCache.Singleton.Add(ques, expires, uid);
        }
        /// <summary>
        /// 更新试题缓存
        /// </summary>
        /// <param name="ques"></param>
        /// <param name="expires"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string CacheUpdate(Questions[] ques, int expires, string uid)
        {
            return QuestionsMethod.QuestionsCache.Singleton.Update(ques, expires, uid);
        }
        /// <summary>
        /// 更新答题信息缓存
        /// </summary>
        /// <param name="exr"></param>
        /// <param name="expires"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string CacheUpdate(ExamResults exr, int expires, string uid)
        {
            return QuestionsMethod.QuestionsCache.Singleton.Update(exr, expires, uid);
        }
        /// <summary>
        /// 从试题缓存中取试题
        /// </summary>
        /// <param name="qid"></param>
        /// <returns></returns>
        public Questions QuesSingle4Cache(int qid)
        {
            return QuestionsMethod.QuestionsCache.Singleton.GetSingle(qid);
        }
        /// <summary>
        /// 从试题缓存中取试题
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Questions QuesSingle4Cache(string uid)
        {
            return QuestionsMethod.QuestionsCache.Singleton.GetSingle(uid);
        }
        /// <summary>
        /// 从缓存中获取试题集
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Questions[] CacheQuestions(string uid)
        {
            List<Questions> ques= QuestionsMethod.QuestionsCache.Singleton.GetQuestions(uid);
            //试题倒序排列
            //ques = ques.OrderByDescending(x => x.Qus_ID).ToList<Questions>();
            ques = ques.OrderBy(x => x.Qus_Tax).ToList<Questions>();
            if (ques == null) return null;
            return ques.ToArray<Questions>();
        }
        /// <summary>
        /// 强制刷新，清除过期的缓存（默认每十分钟清理一次）
        /// </summary>
        public void CacheClear()
        {
            QuestionsMethod.QuestionsCache.Singleton.Clear();
        }
        /// <summary>
        /// 刷新缓存
        /// </summary>
        /// <param name="key">缓存名称</param>
        public void Refresh(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) key = "all";
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, null, -1);
            Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Delete(key);
            Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Add(ques, int.MaxValue, key);
        }
        #endregion

        #region 事件
        public event EventHandler Save;
        public event EventHandler Add;
        public event EventHandler Delete;
        public void OnSave(object sender, EventArgs e)
        {
            if (sender == null)
            {
                //取所有试题进缓存
                new Thread(new ThreadStart(() =>
                {
                    Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, null, -1);
                    Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Delete("all");
                    Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.Add(ques, int.MaxValue, "all");
                })).Start();
            }
            else
            {
                //单个试题的缓存刷新
                if (!(sender is Questions)) return;
                Questions ques = (Questions)sender;
                if (ques == null) return;
                Song.ServiceImpls.QuestionsMethod.QuestionsCache.Singleton.UpdateSingle(ques);
            }
            if (Save != null) Save(sender, e);
        }
        public void OnAdd(object sender, EventArgs e)
        {
            this.OnSave(null, e);
            //更新章节试题数量
            if (!(sender is Questions)) return;
            Questions ques = (Questions)sender;
            if (ques == null) return;
            if (ques.Ol_ID > 0)
            {
                int count = Business.Do<IOutline>().QuesOfCount(ques.Ol_ID, -1, true, true);
                Outline ol = Business.Do<IOutline>().OutlineSingle(ques.Ol_ID);
                Outline olnew = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == ques.Ol_ID).ToFirst<Outline>();
                olnew.Ol_QuesCount = count;
                Gateway.Default.Save<Outline>(olnew);
                WeiSha.Common.Cache<Song.Entities.Outline>.Data.Update(ol, olnew);
            }
            if (Add != null) Add(sender, e);
        }
        public void OnDelete(object sender, EventArgs e)
        {
            this.OnSave(null, e);
            if (sender == null)
            {
                //Business.Do<IOutline>().OutlineBuildCache();
            }
            else
            {
                //更新章节试题数量
                if (!(sender is Questions)) return;
                Questions ques = (Questions)sender;
                if (ques == null) return;
                if (ques.Ol_ID > 0)
                {
                    int count = Business.Do<IOutline>().QuesOfCount(ques.Ol_ID, -1, true, true);
                    Outline ol = Business.Do<IOutline>().OutlineSingle(ques.Ol_ID);
                    Outline olnew = Gateway.Default.From<Outline>().Where(Outline._.Ol_ID == ques.Ol_ID).ToFirst<Outline>();
                    olnew.Ol_QuesCount = count;
                    Gateway.Default.Save<Outline>(olnew);
                    WeiSha.Common.Cache<Song.Entities.Outline>.Data.Update(ol, olnew);
                }
            }
            if (Delete != null) Delete(sender, e);
        }
        #endregion
    }
}
