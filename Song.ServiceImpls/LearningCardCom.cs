using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using WeiSha.Common;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using System.Xml;
using NPOI.SS.UserModel;
using System.IO;



namespace Song.ServiceImpls
{
    public class LearningCardCom : ILearningCard
    {
        #region 学习卡设置管理
        /// <summary>
        /// 添加学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SetAdd(LearningCardSet entity)
        {
            entity.Lcs_CrtTime = DateTime.Now;
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null) entity.Org_ID = org.Org_ID;
            Gateway.Default.Save<LearningCardSet>(entity);
            //生成学习卡
            LearningCard[] cards = CardGenerate(entity);
            if (cards != null)
            {
                foreach (LearningCard c in cards)
                    Gateway.Default.Save<LearningCard>(c);
            }
        }
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SetSave(LearningCardSet entity)
        {
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Song.Entities.LearningCardSet rs = tran.From<LearningCardSet>().Where(LearningCardSet._.Lcs_ID == entity.Lcs_ID).ToFirst<LearningCardSet>();
                    LearningCard[] cards = CardGenerate(entity, tran);
                    if (cards != null)
                    {
                        foreach (LearningCard c in cards)
                            Gateway.Default.Save<LearningCard>(c);
                    }
                    tran.Update<LearningCard>(new Field[] { LearningCard._.Lc_Price, LearningCard._.Lc_LimitStart, LearningCard._.Lc_LimitEnd },
                        new object[] { entity.Lcs_Price, entity.Lcs_LimitStart, entity.Lcs_LimitEnd },
                        LearningCard._.Lcs_ID == entity.Lcs_ID && LearningCard._.Lc_IsUsed == false);
                    tran.Save<LearningCardSet>(entity);
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
        }
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void SetDelete(LearningCardSet entity)
        {
            int used = CardOfCount(entity.Org_ID, entity.Lcs_ID, true, true);
            if (used > 0) throw new Exception("当前设置项中涉及的学习卡已经存在使用记录，不能删除！可以选择禁用。");
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Delete<LearningCardSet>(entity);
                    tran.Delete<LearningCard>(LearningCard._.Lcs_ID == entity.Lcs_ID);

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
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void SetDelete(int identify)
        {
            LearningCardSet set = SetSingle(identify);
            SetDelete(set);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public LearningCardSet SetSingle(int identify)
        {
            return Gateway.Default.From<LearningCardSet>().Where(LearningCardSet._.Lcs_ID == identify).ToFirst<LearningCardSet>();
        }
        /// <summary>
        /// 获取所有设置项
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public LearningCardSet[] SetCount(int orgid, bool? isEnable, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCardSet._.Org_ID == orgid;
            if (isEnable != null) wc &= LearningCardSet._.Lcs_IsEnable == isEnable;
            return Gateway.Default.From<LearningCardSet>().Where(wc).OrderBy(LearningCardSet._.Lcs_CrtTime.Desc).ToArray<LearningCardSet>(count);
        }
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <returns></returns>
        public int SetOfCount(int orgid, bool? isEnable)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCardSet._.Org_ID == orgid;
            if (isEnable != null) wc &= LearningCardSet._.Lcs_IsEnable == isEnable;
            return Gateway.Default.Count<LearningCardSet>(wc);
        }        
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="isUse"></param>
        /// <param name="searTxt"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LearningCardSet[] SetPager(int orgid, bool? isEnable, string searTxt, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCardSet._.Org_ID == orgid;
            if (isEnable != null) wc &= LearningCardSet._.Lcs_IsEnable == isEnable;
            if (!string.IsNullOrWhiteSpace(searTxt))
            {
                WhereClip like = new WhereClip();
                like &= LearningCardSet._.Lcs_Theme.Like("%" + searTxt + "%");
                like |= LearningCardSet._.Lcs_Intro.Like("%" + searTxt + "%");
                wc.And(like);
            }
            countSum = Gateway.Default.Count<LearningCardSet>(wc);
            return Gateway.Default.From<LearningCardSet>()
                .Where(wc).OrderBy(LearningCardSet._.Lcs_CrtTime.Desc).ToArray<LearningCardSet>(size, (index - 1) * size);
        }
        #endregion

        #region 学习卡管理
        /// <summary>
        /// 生成学习卡
        /// </summary>
        /// <param name="set">学习卡的设置项</param>
        /// <param name="factor">随机因子</param>
        /// <returns></returns>
        public LearningCard CardGenerate(LearningCardSet set, int factor=-1)
        {
            Song.Entities.LearningCard card = new LearningCard();
            card.Lc_Price = set.Lcs_Price;   //面额
            card.Lc_IsEnable = true;    //是否启用
            card.Lc_CrtTime = DateTime.Now;  //创建时间                
            card.Lcs_ID = set.Lcs_ID;   //设置项id                
            card.Org_ID = set.Org_ID;   //机构id              
            card.Lc_LimitStart = set.Lcs_LimitStart;   //时间效
            card.Lc_LimitEnd = set.Lcs_LimitEnd;
            //卡值码与其密码
            card.Lc_Code = _CardBuildCoder(set.Lcs_SecretKey, factor, set.Lcs_CodeLength);
            card.Lc_Pw = _CardBuildPw(factor, set.Lcs_PwLength);
            return card;
        }
        /// <summary>
        /// 批量生成学习卡
        /// </summary>
        /// <param name="set">学习卡的设置项</param>
        /// <returns></returns>
        public LearningCard[] CardGenerate(LearningCardSet set)
        {
            return CardGenerate(set, null);
        }
        /// <summary>
        /// <param name="tran">事务</param>
        /// </summary>
        /// <param name="set"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public LearningCard[] CardGenerate(LearningCardSet set, DbTrans tran)
        {
            int count = 0;  //要生成的数量
            int realcount = Gateway.Default.Count<LearningCard>(LearningCard._.Lcs_ID == set.Lcs_ID);   //实际数量
            //如果实际数量小于要生成的数量，则只生成差额
            if (realcount < set.Lcs_Count) count = set.Lcs_Count - realcount;
            //如果实际数量小于要生成的数量，则删除多余（如果已经使用的过的，不可删除）
            if (realcount > set.Lcs_Count)
            {
                bool isNull = false;
                if (tran == null)
                {
                    tran = Gateway.Default.BeginTrans();
                    isNull = true;
                }
                Song.Entities.LearningCard[] rcs = tran.From<LearningCard>().Where(LearningCard._.Lcs_ID == set.Lcs_ID && LearningCard._.Lc_IsUsed == false)
                    .ToArray<LearningCard>(realcount - set.Lcs_Count);
                foreach (LearningCard r in rcs)
                    tran.Delete<LearningCard>(r);
                if (isNull)
                {
                    tran.Commit();
                    tran.Dispose();
                    tran.Close();
                }
                return null;
            }
            //生成学习卡
            LearningCard[] cards = new LearningCard[count];
            for (int i = 0; i < count; i++)
                cards[i] = CardGenerate(set, i);
            //判断是否重复
            for (int i = 0; i < cards.Length; i++)
            {
                for (int j = 0; j < cards.Length; j++)
                {
                    if (j <= i) continue;
                    if (cards[i].Lc_Code == cards[j].Lc_Code)
                    {
                        cards[i] = CardGenerate(set, i * j + DateTime.Now.Millisecond);
                        i = j = 0;
                    }
                }
            }
            return cards;
        }
        /// <summary>
        /// 添加学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CardAdd(LearningCard entity)
        {
            entity.Lc_CrtTime = DateTime.Now;
            Gateway.Default.Save<LearningCard>(entity);
        }
        /// <summary>
        /// 修改学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CardSave(LearningCard entity)
        {
            Gateway.Default.Save<LearningCard>(entity);
        }
        /// <summary>
        /// 删除学习卡设置项
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void CardDelete(LearningCard entity)
        {
            CardRollback(entity);
            Gateway.Default.Delete<LearningCard>(LearningCard._.Lc_ID == entity.Lc_ID);
        }
        /// <summary>
        /// 删除，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void CardDelete(int identify)
        {
            LearningCard card = this.CardSingle(identify);
            this.CardDelete(card);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public LearningCard CardSingle(int identify)
        {
            return Gateway.Default.From<LearningCard>().Where(LearningCard._.Lc_ID == identify).ToFirst<LearningCard>();
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="code">学习卡编码</param>
        /// <param name="pw">学习卡密码</param>
        /// <returns></returns>
        public LearningCard CardSingle(string code, string pw)
        {
            return Gateway.Default.From<LearningCard>().Where(LearningCard._.Lc_Code == code && LearningCard._.Lc_Pw == pw)
                .ToFirst<LearningCard>();
        }
        /// <summary>
        /// 校验学习卡是否存在，或过期
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public LearningCard CardCheck(string code)
        {
            code = Regex.Replace(code, @"[^\d-]", "", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            if (code.IndexOf("-") < 0) throw new Exception("该学习卡不正确！");
            //取密码与充值码
            string pw = code.Substring(code.IndexOf("-") + 1);
            code = code.Substring(0, code.IndexOf("-"));
            //验证是否正确
            WhereClip wc = new WhereClip();
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            wc &= LearningCard._.Org_ID == org.Org_ID;
            wc &= LearningCard._.Lc_Code == code.Trim();
            wc &= LearningCard._.Lc_Pw == pw.Trim();
            wc &= LearningCard._.Lc_LimitStart < DateTime.Now;
            wc &= LearningCard._.Lc_LimitEnd > DateTime.Now;
            LearningCard single = Gateway.Default.From<LearningCard>().Where(wc).ToFirst<LearningCard>();
            if (single == null) throw new Exception("该学习卡不存在，或已经过期！");
            //如果学习卡已经被领用
            if (single.Ac_ID > 0)
            {
                if (single.Lc_IsUsed && single.Lc_State != 0) throw new Exception("该学习卡已经使用过！");
            }
            else
            {
                if (single.Lc_IsUsed) throw new Exception("该学习卡已经使用过！");
            }           
            return single;
        }
        /// <summary>
        /// 学习卡的使用数量
        /// </summary>
        /// <param name="lscid"></param>
        /// <returns></returns>
        public int CardUsedCount(int lscid)
        {
            return Gateway.Default.Count<LearningCard>(LearningCard._.Lcs_ID == lscid && LearningCard._.Lc_IsUsed == true);
        }
        /// <summary>
        /// 使用该学习卡
        /// </summary>
        /// <param name="card">学习卡</param>
        /// <param name="acc">学员账号</param>
        public void CardUse(LearningCard card, Accounts acc)
        {
            if (card.Lc_State != 0) throw new Exception("该学习卡已经使用");
            LearningCardSet set = this.SetSingle(card.Lcs_ID);
            if (set == null || set.Lcs_IsEnable == false) throw new Exception("该学习卡不可使用");
            //是否过期
            if (!(DateTime.Now > card.Lc_LimitStart && DateTime.Now < card.Lc_LimitEnd.Date.AddDays(1)))
                throw new Exception("该学习卡已经过期");
            //设置学习卡的使用信息
            card.Lc_UsedTime = DateTime.Now;
            card.Lc_State = 1;    //状态，0为初始，1为使用，-1为回滚
            card.Ac_ID = acc.Ac_ID;
            card.Ac_AccName = acc.Ac_AccName;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                //学习时间的起始时间
                DateTime start = DateTime.Now, end = DateTime.Now;
                if (set.Lcs_Unit == "日" || set.Lcs_Unit == "天") end = start.AddDays(set.Lcs_Span);
                if (set.Lcs_Unit == "周") end = start.AddDays(set.Lcs_Span * 7);
                if (set.Lcs_Unit == "月") end = start.AddMonths(set.Lcs_Span);
                if (set.Lcs_Unit == "年") end = start.AddYears(set.Lcs_Span);
                int span = (end - start).Days;
                try
                {
                    Course[] courses = this.CoursesGet(set.Lcs_RelatedCourses);
                    foreach (Course cou in courses)
                    {
                        Song.Entities.Student_Course sc = null;
                        sc = tran.From<Student_Course>().Where(Student_Course._.Ac_ID == card.Ac_ID
                            && Student_Course._.Cou_ID == cou.Cou_ID).ToFirst<Student_Course>();
                        if (sc != null)
                        {
                            //如果是免费或试用
                            if (sc.Stc_IsFree || sc.Stc_IsTry)
                            {
                                sc.Stc_StartTime = start;
                                sc.Stc_EndTime = end;
                            }
                            else
                            {
                                //已经过期，则重新设置时间
                                if (sc.Stc_EndTime < DateTime.Now)
                                {
                                    sc.Stc_StartTime = start;
                                    sc.Stc_EndTime = end;
                                }
                                else
                                {
                                    //如果未过期，则续期                                
                                    sc.Stc_EndTime = sc.Stc_EndTime.AddDays(span);
                                }
                            }                           
                        }
                        else
                        {
                            sc = new Student_Course();
                            sc.Stc_CrtTime = DateTime.Now;
                            sc.Stc_StartTime = start;
                            sc.Stc_EndTime = end;
                        }
                        sc.Ac_ID = card.Ac_ID;
                        sc.Cou_ID = cou.Cou_ID;
                        sc.Stc_Money = card.Lc_Price;                       
                        sc.Org_ID = card.Org_ID;
                        sc.Stc_IsFree = sc.Stc_IsTry = false;
                        tran.Save<Student_Course>(sc);
                    }                    
                    //使用数量加1
                    set.Lsc_UsedCount = tran.Count<LearningCard>(LearningCard._.Lcs_ID == set.Lcs_ID && LearningCard._.Lc_IsUsed == true);
                    set.Lsc_UsedCount = card.Lc_IsUsed ? set.Lsc_UsedCount : set.Lsc_UsedCount + 1;
                    tran.Save<LearningCardSet>(set);
                    //标注学习卡已经使用
                    card.Lc_IsUsed = true;
                    card.Lc_Span = span;  //记录学习卡使后，增加的学习时间（单位：天），方便回滚扣除                    
                    tran.Save<LearningCard>(card);
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
        }
        /// <summary>
        /// 获取该学习卡，只是暂存在学员账户名下，并不使用
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="acc">学员账号</param>
        public void CardGet(LearningCard entity, Accounts acc)
        {
            if (entity.Lc_State != 0) throw new Exception("该学习卡已经使用");
            LearningCardSet set = this.SetSingle(entity.Lcs_ID);
            if (set == null || set.Lcs_IsEnable == false) throw new Exception("该学习卡不可使用");
            //是否过期
            if (!(DateTime.Now > entity.Lc_LimitStart && DateTime.Now < entity.Lc_LimitEnd.Date.AddDays(1)))
                throw new Exception("该学习卡已经过期");
            //标注已经使用
            entity.Lc_IsUsed = true;
            entity.Lc_UsedTime = DateTime.Now;
            entity.Lc_State = 0;    //状态，0为初始，1为使用，-1为回滚
            entity.Ac_ID = acc.Ac_ID;
            entity.Ac_AccName = acc.Ac_AccName;
            //使用数量加1
            int usecount = Gateway.Default.Count<LearningCard>(LearningCard._.Lcs_ID == set.Lcs_ID && LearningCard._.Lc_IsUsed == true);
            set.Lsc_UsedCount = usecount + 1;
            Gateway.Default.Save<LearningCardSet>(set);
            Gateway.Default.Save<LearningCard>(entity);
        }
        /// <summary>
        /// 学习卡使用后的回滚，将删除学员的关联课程
        /// </summary>
        /// <param name="entity"></param>
        public void CardRollback(LearningCard entity)
        {
            CardRollback(entity,false);
        }
        /// <summary>
        /// 学习卡使用后的回滚，将删除学员的关联课程
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isclear">是否清理学习记录</param>
        public void CardRollback(LearningCard entity, bool isclear)
        {
           
            //只是领用，但未使用
            if (entity.Lc_IsUsed && entity.Lc_State == 0)
            {
                //标注状态为回滚
                entity.Lc_State = -1;
                Gateway.Default.Save<LearningCard>(entity);
            }
            //真正使用过，回滚较复杂
            if (entity.Lc_IsUsed && entity.Lc_State == 1)
            {
                LearningCardSet set = this.SetSingle(entity.Lcs_ID);
                //学习时间的起始时间
                int day = entity.Lc_Span;
                if (day <= 0)
                {
                    if (set.Lcs_Unit == "日" || set.Lcs_Unit == "天") day = set.Lcs_Span;
                    if (set.Lcs_Unit == "周") day = set.Lcs_Span * 7;
                    if (set.Lcs_Unit == "月") day = set.Lcs_Span * 30;
                    if (set.Lcs_Unit == "年") day = set.Lcs_Span * 365;
                }
                //关联的课程
                Course[] courses = this.CoursesGet(set.Lcs_RelatedCourses);
                using (DbTrans tran = Gateway.Default.BeginTrans())
                {
                    try
                    {
                        foreach (Course cou in courses)
                        {
                            Song.Entities.Student_Course sc = tran.From<Student_Course>().Where(Student_Course._.Ac_ID == entity.Ac_ID
                                && Student_Course._.Cou_ID == cou.Cou_ID).ToFirst<Student_Course>();
                            if (sc == null) continue;
                            //扣除学习卡所增加的时间，计算出学习结束时间
                            DateTime end = sc.Stc_EndTime.AddDays(-day);
                            if (sc.Stc_StartTime < end)
                            {
                                //如果扣除学习卡增加的时间后，仍然大于开始时间，则更改
                                tran.Update<Student_Course>(new Field[] { Student_Course._.Stc_EndTime }, new object[] { end },
                                Student_Course._.Ac_ID == entity.Ac_ID && Student_Course._.Cou_ID == cou.Cou_ID);
                            }
                            else
                            {
                                //如果扣除学习卡增加的时间后，小于开始时间，则直接删除课程
                                tran.Delete<Student_Course>(Student_Course._.Ac_ID == entity.Ac_ID && Student_Course._.Cou_ID == cou.Cou_ID);
                                if (isclear) _cardRollback_clear(entity.Ac_ID, cou.Cou_ID);
                                tran.Update<Accounts>(new Field[] { Accounts._.Ac_CurrCourse }, new object[] { -1 },
                                    Accounts._.Ac_ID == entity.Ac_ID && Accounts._.Ac_CurrCourse == cou.Cou_ID);
                            }
                        }
                        entity.Lc_State = -1;
                        tran.Save<LearningCard>(entity);
                        tran.Commit();
                        Extend.LoginState.Accounts.Refresh(entity.Ac_ID);
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
        }
        private void _cardRollback_clear(int acc,int couid)
        {
            //学习记录
            Gateway.Default.Delete<LogForStudentQuestions>(LogForStudentQuestions._.Ac_ID == acc && LogForStudentQuestions._.Cou_ID==couid);
            Gateway.Default.Delete<LogForStudentStudy>(LogForStudentStudy._.Ac_ID == acc && LogForStudentStudy._.Cou_ID == couid);
            //试题，收藏、笔记、错题
            Gateway.Default.Delete<Student_Collect>(Student_Collect._.Ac_ID == acc && Student_Collect._.Cou_ID==couid);
            Gateway.Default.Delete<Student_Notes>(Student_Notes._.Ac_ID == acc && Student_Notes._.Cou_ID == couid);
            Gateway.Default.Delete<Student_Ques>(Student_Ques._.Ac_ID == acc && Student_Ques._.Cou_ID == couid);
            //模拟测试
            Gateway.Default.Delete<TestResults>(TestResults._.Ac_ID == acc && TestResults._.Cou_ID == couid);
        }
        /// <summary>
        /// 学习卡设置项下的所有学习卡
        /// </summary>
        /// <param name="orgid">所在机构id</param>
        /// <param name="lcsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public LearningCard[] CardCount(int orgid, int lcsid, bool? isEnable, bool? isUsed, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCard._.Org_ID == orgid;
            if (lcsid > 0) wc &= LearningCard._.Lcs_ID == lcsid;
            if (isEnable != null) wc &= LearningCard._.Lc_IsEnable == isEnable;
            if (isUsed != null) wc &= LearningCard._.Lc_IsUsed == isUsed;
            return Gateway.Default.From<LearningCard>().Where(wc).OrderBy(LearningCard._.Lc_CrtTime.Desc).ToArray<LearningCard>(count);
        }
        /// <summary>
        /// 所有设置项数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">充码设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <returns></returns>
        public int CardOfCount(int orgid, int lcsid, bool? isEnable, bool? isUsed)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCard._.Org_ID == orgid;
            if (lcsid > 0) wc &= LearningCard._.Lcs_ID == lcsid;
            if (isEnable != null) wc &= LearningCard._.Lc_IsEnable == isEnable;
            if (isUsed != null) wc &= LearningCard._.Lc_IsUsed == isUsed;
            return Gateway.Default.Count<LearningCard>(wc);
        }
        /// <summary>
        /// 导出Excel格式的学习卡信息
        /// </summary>
        /// <param name="path">导出文件的路径（服务器端）</param>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <returns></returns>
        public string Card4Excel(string path, int orgid, int lcsid)
        {
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            //xml配置文件
            XmlDocument xmldoc = new XmlDocument();
            string confing = WeiSha.Common.App.Get["ExcelInputConfig"].VirtualPath + "学习卡.xml";
            xmldoc.Load(WeiSha.Common.Server.MapPath(confing));
            XmlNodeList nodes = xmldoc.GetElementsByTagName("item");
            //创建工作簿对象
            LearningCardSet rs = Gateway.Default.From<LearningCardSet>().Where(LearningCardSet._.Lcs_ID == lcsid).ToFirst<LearningCardSet>();
            ISheet sheet = hssfworkbook.CreateSheet(rs.Lcs_Theme);
            //sheet.DefaultColumnWidth = 30;
            //创建数据行对象
            IRow rowHead = sheet.CreateRow(0);
            for (int i = 0; i < nodes.Count; i++)
                rowHead.CreateCell(i).SetCellValue(nodes[i].Attributes["Column"].Value);
            //生成数据行
            ICellStyle style_size = hssfworkbook.CreateCellStyle();
            style_size.WrapText = true;
            WhereClip wc = LearningCard._.Org_ID == orgid;
            if (lcsid >= 0) wc.And(LearningCard._.Lcs_ID == lcsid);
            LearningCard[] rcodes = Gateway.Default.From<LearningCard>().Where(wc).OrderBy(LearningCard._.Lc_CrtTime.Desc).ToArray<LearningCard>();
            for (int i = 0; i < rcodes.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < nodes.Count; j++)
                {
                    Type type = rcodes[i].GetType();
                    System.Reflection.PropertyInfo propertyInfo = type.GetProperty(nodes[j].Attributes["Field"].Value); //获取指定名称的属性
                    object obj = propertyInfo.GetValue(rcodes[i], null);
                    if (obj != null)
                    {
                        string format = nodes[j].Attributes["Format"] != null ? nodes[j].Attributes["Format"].Value : "";
                        string datatype = nodes[j].Attributes["DataType"] != null ? nodes[j].Attributes["DataType"].Value : "";
                        string defvalue = nodes[j].Attributes["DefautValue"] != null ? nodes[j].Attributes["DefautValue"].Value : "";
                        string value = "";
                        switch (datatype)
                        {
                            case "date":
                                DateTime tm = Convert.ToDateTime(obj);
                                value = tm > DateTime.Now.AddYears(-100) ? tm.ToString(format) : "";
                                break;
                            default:
                                value = obj.ToString();
                                break;
                        }
                        if (defvalue.Trim() != "")
                        {
                            foreach (string s in defvalue.Split('|'))
                            {
                                string h = s.Substring(0, s.IndexOf("="));
                                string f = s.Substring(s.LastIndexOf("=") + 1);
                                if (value.ToLower() == h.ToLower()) value = f;
                            }
                        }
                        row.CreateCell(j).SetCellValue(value);
                    }
                }
            }
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
            return path;
        }
        /// <summary>
        /// 分页获取学习卡设置项
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="lcsid">学习卡设置项的id</param>
        /// <param name="isEnable">是否启用</param>
        /// <param name="isUsed">是否已经使用</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public LearningCard[] CardPager(int orgid, int lcsid, bool? isEnable, bool? isUsed, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCard._.Org_ID == orgid;
            if (lcsid > 0) wc &= LearningCard._.Lcs_ID == lcsid;
            if (isEnable != null) wc &= LearningCard._.Lc_IsEnable == isEnable;
            if (isUsed != null) wc &= LearningCard._.Lc_IsUsed == isUsed;
            countSum = Gateway.Default.Count<LearningCard>(wc);
            return Gateway.Default.From<LearningCard>()
                .Where(wc).OrderBy(LearningCard._.Lc_CrtTime.Desc).ToArray<LearningCard>(size, (index - 1) * size);
        }
        public LearningCard[] CardPager(int orgid, int lcsid, string code, bool? isEnable, bool? isUsed, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= LearningCard._.Org_ID == orgid;
            if (lcsid > 0) wc &= LearningCard._.Lcs_ID == lcsid;
            if (!string.IsNullOrWhiteSpace(code)) wc &= LearningCard._.Lc_Code.Like("%" + code + "%");
            if (isEnable != null) wc &= LearningCard._.Lc_IsEnable == isEnable;
            if (isUsed != null) wc &= LearningCard._.Lc_IsUsed == isUsed;
            countSum = Gateway.Default.Count<LearningCard>(wc);
            return Gateway.Default.From<LearningCard>()
                .Where(wc).OrderBy(LearningCard._.Lc_CrtTime.Desc).ToArray<LearningCard>(size, (index - 1) * size);
        }
        #endregion

        #region 生成
        /// <summary>
        /// 生成单个学习卡的编码
        /// </summary>
        /// <param name="scretKey">密钥</param>
        /// <param name="factor">随机因子</param>
        /// <param name="length">充值码的长度</param>
        private string _CardBuildCoder(string scretKey, int factor, int length)
        {
            if (factor <= 0) factor = DateTime.Now.Millisecond;
            //充值码基础值（来自时间）
            string baseCode = DateTime.Now.ToString("yyMMddhhmmssfff");
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) * factor);
            int rdNumber = rd.Next(0, 1000);
            baseCode += rdNumber.ToString("000");
            //充值码加密值（来自密钥中随机字符）
            if (string.IsNullOrWhiteSpace(scretKey))
                scretKey = WeiSha.Common.Request.UniqueID();
            string pwstr = "";
            while (pwstr.Length < baseCode.Length)
            {
                System.Random rdpw = new System.Random(pwstr.Length * factor);
                int tm = rd.Next(0, scretKey.Length - 1);
                pwstr += scretKey.Substring(tm, 1);
            }
            //基础值与加密值运算
            byte[] typeBase = System.Text.Encoding.Default.GetBytes(baseCode);
            byte[] typePw = System.Text.Encoding.Default.GetBytes(pwstr);
            string str = "";
            for (int i = 0; i < typeBase.Length; i++)
            {
                typeBase[i] ^= typePw[i];
                str += typeBase[i].ToString();
            }
            return str.Substring(str.Length - length);
        }
        /// <summary>
        /// 生成单个学习卡的密码
        /// </summary>
        /// <param name="factor">随机因子</param>
        /// <param name="length">密码的长度</param>
        /// <returns></returns>
        private string _CardBuildPw(int factor, int length)
        {
            if (factor <= 0) factor = DateTime.Now.Millisecond;
            System.Random rd = new System.Random(((int)DateTime.Now.Ticks) * factor);
            string lenstr = "9";
            while (lenstr.Length < length) lenstr += "9";
            int max = Convert.ToInt32(lenstr);
            int rdNumber = rd.Next(0, max);
            return rdNumber.ToString(lenstr.Replace("9", "0"));
        }
        #endregion

        #region 关联课程
        /// <summary>
        /// 获取关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public Course[] CoursesGet(LearningCardSet set)
        {
            return CoursesGet(set.Lcs_RelatedCourses);
        }
        public Course[] CoursesGet(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml)) return null;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.XmlResolver = null; 
            xmlDoc.LoadXml(xml, false);
            XmlNodeList items = xmlDoc.SelectNodes("Items/item");
            List<Course> list = new List<Course>();
            for (int i = 0; i < items.Count; i++)
            {
                string couid = ((XmlElement)items[i]).GetAttribute("Cou_ID");
                if (string.IsNullOrWhiteSpace(couid)) continue;
                int id = 0;
                int.TryParse(couid, out id);
                if (id <= 0) continue;
                Course cou = Business.Do<ICourse>().CourseSingle(id);
                if (cou == null) continue;
                list.Add(cou);                
            }
            return list.ToArray();
        }
        /// <summary>
        /// 学习卡关联的课程
        /// </summary>
        /// <param name="code">学习卡编码</param>
        /// <param name="pw">学习卡密码</param>
        /// <returns></returns>
        public Course[] CoursesForCard(string code, string pw)
        {
            LearningCard card = this.CardSingle(code, pw);
            if (card != null)
            {
                LearningCardSet set = this.SetSingle(card.Lcs_ID);
                if (set != null)
                {
                    return this.CoursesGet(set);
                }
            }
            return null;
        }
        /// <summary>
        /// 设置关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <param name="courses"></param>
        /// <returns>LearningCardSet对象中的Lcs_RelatedCourses将记录关联信息</returns>
        public LearningCardSet CoursesSet(LearningCardSet set, Course[] courses)
        {
            if (courses == null || courses.Length <= 0)
            {
                set.Lcs_RelatedCourses = string.Empty;
                set.Lcs_CoursesCount = 0;
                return set;
            }
            int[] couid = new int[courses.Length];
            for (int i = 0; i < courses.Length; i++)
                couid[i] = courses[i].Cou_ID;
            return CoursesSet(set, couid);
        }

        public LearningCardSet CoursesSet(LearningCardSet set, int[] couid)
        {
            if (couid == null || couid.Length < 1)
            {
                set.Lcs_RelatedCourses = string.Empty;
                set.Lcs_CoursesCount = 0;
                return set;
            }
            XmlDocument xmlDoc = new XmlDocument();
            //创建根节点  
            XmlNode root = xmlDoc.CreateElement("Items");
            xmlDoc.AppendChild(root);
            for (int i = 0; i < couid.Length; i++)
            {
                XmlElement item = xmlDoc.CreateElement("item");
                item.SetAttribute("Cou_ID", couid[i].ToString());
                root.AppendChild(item);
            }
            set.Lcs_RelatedCourses = root.OuterXml;
            set.Lcs_CoursesCount = couid.Length;
            return set;
        }
        /// <summary>
        /// 设置关联的课程
        /// </summary>
        /// <param name="set"></param>
        /// <param name="couids">课程id串，以逗号分隔</param>
        /// <returns></returns>
        public LearningCardSet CoursesSet(LearningCardSet set, string couids)
        {
            List<int> list = new List<int>();
            foreach (string s in couids.Split(','))
            {
                if (string.IsNullOrWhiteSpace(s)) continue;
                if (s.Trim() == "") continue;
                int id = 0;
                int.TryParse(s, out id);
                if (id == 0) continue;
                list.Add(id);
            }
            return CoursesSet(set, list.ToArray());
        }
        #endregion

        #region 学员的卡
        /// <summary>
        /// 学员名下学习卡的数量
        /// </summary>
        /// <param name="accid">学员账号id</param>
        /// <returns></returns>
        public int AccountCardOfCount(int accid)
        {
            return Gateway.Default.Count<LearningCard>(LearningCard._.Ac_ID == accid);
        }
        /// <summary>
        /// 学员名下学习卡的数量
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="state">状态，0为初始，1为使用，-1为回滚</param>
        /// <returns></returns>
        public int AccountCardOfCount(int accid, int state)
        {
            return Gateway.Default.Count<LearningCard>(LearningCard._.Ac_ID == accid && LearningCard._.Lc_State == state);
        }
        /// <summary>
        /// 学员名下的学习卡
        /// </summary>
        /// <param name="state">状态，0为初始，1为使用，-1为回滚</param>
        /// <returns></returns>
        public LearningCard[] AccountCards(int accid, int state)
        {
            return Gateway.Default.From<LearningCard>().Where(LearningCard._.Ac_ID == accid && LearningCard._.Lc_State == state)
                 .OrderBy(LearningCard._.Lc_UsedTime.Asc)
                .ToArray<LearningCard>();
        }
        /// <summary>
        /// 学员名下的所有学习卡
        /// </summary>
        /// <returns></returns>
        public LearningCard[] AccountCards(int accid)
        {
            return Gateway.Default.From<LearningCard>().Where(LearningCard._.Ac_ID == accid)
                .OrderBy(LearningCard._.Lc_UsedTime.Asc)
                .ToArray<LearningCard>();
        }
        #endregion
    }
}
