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
using System.Xml;



namespace Song.ServiceImpls
{
    public class TestPaperCom : ITestPaper
    {


        #region ITestPaper 成员

        public long PaperAdd(TestPaper entity)
        {
            if (entity.Tp_Id <= 0) entity.Tp_Id = WeiSha.Core.Request.SnowID();
           
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            if (org != null)
            {
                entity.Org_ID = org.Org_ID;
                entity.Org_Name = org.Org_Name;
            }
            entity.Tp_CrtTime = DateTime.Now;
            //相关联的课程名称
            if (entity.Cou_ID > 0 && string.IsNullOrWhiteSpace(entity.Cou_Name))
            {
                Song.Entities.Course cour = Gateway.Default.From<Course>().Where(Course._.Cou_ID == entity.Cou_ID).ToFirst<Course>();
                if (cour != null) entity.Cou_Name = cour.Cou_Name;
            }           
            //试卷所属的专业名称
            if (entity.Sbj_ID > 0 && string.IsNullOrWhiteSpace(entity.Sbj_Name))
            {
                Song.Entities.Subject sbj = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == entity.Sbj_ID).ToFirst<Subject>();
                if (sbj != null) entity.Sbj_Name = sbj.Sbj_Name;
            }
            //如果为结课考试，则取消当前课程下的其它试卷状态
            if (entity.Tp_IsFinal)
            {
                Gateway.Default.Update<TestPaper>(new Field[] { TestPaper._.Tp_IsFinal },
              new object[] { false }, TestPaper._.Cou_ID == entity.Cou_ID);
            }
            Gateway.Default.Save<TestPaper>(entity);
            return entity.Tp_Id;
            //if (items == null)
            //{
            //    return Gateway.Default.Save<TestPaper>(entity);
            //}
            //int id = -1;
            //using (DbTrans tran = Gateway.Default.BeginTrans())
            //    try
            //    {
            //        int sumCount = 0;
            //        foreach (TestPaperItem tpi in items)
            //        {
            //            tpi.Tp_UID = entity.Tp_UID;
            //            tran.Save<TestPaperItem>(tpi);
            //            sumCount += (int)tpi.TPI_Count;
            //        }
            //        entity.Tp_Count = sumCount;
            //        id = tran.Save<TestPaper>(entity);
            //        tran.Commit();
            //        return id;
            //    }
            //    catch (Exception ex)
            //    {
            //        tran.Rollback();
            //        throw ex;
            //    }   
        }

        public void PaperSave(TestPaper entity)
        {
            entity.Tp_Lasttime = DateTime.Now;
            //相关联的课程名称
            if (entity.Cou_ID > 0 && string.IsNullOrWhiteSpace(entity.Cou_Name))
            {
                Song.Entities.Course cour = Gateway.Default.From<Course>().Where(Course._.Cou_ID == entity.Cou_ID).ToFirst<Course>();
                if (cour != null) entity.Cou_Name = cour.Cou_Name;
            }
            //试卷所属的专业名称
            if (entity.Sbj_ID > 0 && string.IsNullOrWhiteSpace(entity.Sbj_Name))
            {
                Song.Entities.Subject sbj = Gateway.Default.From<Subject>().Where(Subject._.Sbj_ID == entity.Sbj_ID).ToFirst<Subject>();
                if (sbj != null) entity.Sbj_Name = sbj.Sbj_Name;
            }
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {                    
                    tran.Save<TestPaper>(entity);
                    //如果为结课考试，则取消当前课程下的其它试卷状态
                    if (entity.Tp_IsFinal)
                    {
                        tran.Update<TestPaper>(new Field[] { TestPaper._.Tp_IsFinal },
                      new object[] { false }, TestPaper._.Cou_ID == entity.Cou_ID && TestPaper._.Tp_Id != entity.Tp_Id);
                    }
                    tran.Update<Examination>(new Field[] { Examination._.Exam_PassScore, Examination._.Exam_Total },
                        new object[] { entity.Tp_PassScore, entity.Tp_Total }, Examination._.Tp_Id == entity.Tp_Id);
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
        /// 修改试卷的某些项
        /// </summary>
        /// <param name="id">试卷id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool PaperUpdate(long id, Field[] fiels, object[] objs)
        {
            try
            {
                Gateway.Default.Update<TestPaper>(fiels, objs, TestPaper._.Tp_Id == id);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PaperDelete(long identify)
        {
            Song.Entities.TestPaper tp = this.PaperSingle(identify);
            if (tp == null) return;
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Tp_Id == identify).ToFirst<Examination>();
                    if (exam != null) throw new WeiSha.Core.ExceptionForPrompt("该试卷已被考试采用，不能删除");
                    tran.Delete<TestPaper>(TestPaper._.Tp_Id == identify);
                    //删除图片文件
                    string img = WeiSha.Core.Upload.Get["TestPaper"].Physics + tp.Tp_Logo;
                    if (System.IO.File.Exists(img)) System.IO.File.Delete(img);
                    //删除成绩
                    tran.Delete<TestResults>(TestResults._.Tp_Id == identify);
                    WeiSha.Core.Upload.Get["TestPaper"].DeleteDirectory(tp.Tp_Id.ToString());
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        public TestPaper PaperSingle(long identify)
        {
            TestPaper pager = Gateway.Default.From<TestPaper>().Where(TestPaper._.Tp_Id == identify).ToFirst<TestPaper>();
            return pager;
        }

        public TestPaper PaperSingle(string name)
        {
            return Gateway.Default.From<TestPaper>().Where(TestPaper._.Tp_Name == name).ToFirst<TestPaper>();
        }
        /// <summary>
        /// 获取某个课程的结课考试
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="use"></param>
        /// <returns></returns>
        public TestPaper FinalPaper(long couid,bool? use)
        {
            WhereClip wc = TestPaper._.Cou_ID == couid;
            wc.And(TestPaper._.Tp_IsFinal == true);
            if (use != null)
            {
                wc.And(TestPaper._.Tp_IsUse == (bool)use);
            }
            return Gateway.Default.From<TestPaper>().Where(wc).OrderBy(TestPaper._.Tp_Id.Desc).ToFirst<TestPaper>();
        }
        public TestPaper[] PaperCount(int orgid, long sbjid, long couid, int diff, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(TestPaper._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (int l in list)
                    wcSbjid.Or(TestPaper._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (couid > 0) wc.And(TestPaper._.Cou_ID == couid);
            if (diff > 0) wc.And(TestPaper._.Tp_Diff == diff);
            if (isUse != null) wc.And(TestPaper._.Tp_IsUse == (bool)isUse);
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<TestPaper>().Where(wc).OrderBy(TestPaper._.Tp_CrtTime.Desc).ToArray<TestPaper>(count);
        }

        public int PaperOfCount(int orgid, long sbjid, long couid, int diff, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(TestPaper._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(TestPaper._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (couid > 0) wc.And(TestPaper._.Cou_ID == couid);
            if (diff > 0) wc.And(TestPaper._.Tp_Diff == diff);
            if (isUse != null) wc.And(TestPaper._.Tp_IsUse == (bool)isUse);
            return Gateway.Default.Count<TestPaper>(wc);
        }
        
        public TestPaper[] PaperCount(string search, int orgid, long sbjid, long couid, int diff, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc.And(TestPaper._.Org_ID == orgid);
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(TestPaper._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (couid > 0) wc.And(TestPaper._.Cou_ID == couid);
            if (diff > 0) wc.And(TestPaper._.Tp_Diff == diff);
            if (isUse != null) wc.And(TestPaper._.Tp_IsUse == (bool)isUse);
            if (search != null && search.Trim() != "") wc.And(TestPaper._.Tp_Name.Contains(search));
            count = count > 0 ? count : int.MaxValue;
            return Gateway.Default.From<TestPaper>().Where(wc).OrderBy(TestPaper._.Tp_CrtTime.Desc).ToArray<TestPaper>(count);
        }

        public TestPaper[] PaperPager(int orgid, long sbjid, long couid, int diff, bool? isUse, string sear, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= TestPaper._.Org_ID == orgid;
            if (sbjid > 0)
            {
                WhereClip wcSbjid = new WhereClip();
                List<long> list = Business.Do<ISubject>().TreeID(sbjid, orgid);
                foreach (long l in list)
                    wcSbjid.Or(TestPaper._.Sbj_ID == l);
                wc.And(wcSbjid);
            }
            if (couid > 0) wc.And(TestPaper._.Cou_ID == couid);
            if (diff > 0) wc.And(TestPaper._.Tp_Diff == diff);
            if (isUse != null) wc.And(TestPaper._.Tp_IsUse == (bool)isUse);
            if (sear != null && sear.Trim() != "") wc.And(TestPaper._.Tp_Name.Contains(sear));
            countSum = Gateway.Default.Count<TestPaper>(wc);
            return Gateway.Default.From<TestPaper>().Where(wc).OrderBy(TestPaper._.Tp_CrtTime.Desc).ToArray<TestPaper>(size, (index - 1) * size);
        }

        public List<TestPaperItem> GetItemForAll(TestPaper tp)
        {
            List<TestPaperItem> tpi = new List<TestPaperItem>();
            if (tp != null && !string.IsNullOrWhiteSpace(tp.Tp_FromConfig))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tp.Tp_FromConfig, false);
                XmlNodeList nodes = xmlDoc.SelectNodes("Config/All/Items/TestPaperItem");
                for (int i = 0; i < nodes.Count; i++)
                {
                    TestPaperItem item = new TestPaperItem();
                    item.FromXML(nodes[i].OuterXml);
                    tpi.Add(item);
                }
            }
            if (tpi == null)
            {
                tpi = Gateway.Default.From<TestPaperItem>()
                    .Where(TestPaperItem._.Tp_UID == tp.Tp_UID && TestPaperItem._.TPI_Count > 0)
                    .OrderBy(TestPaperItem._.TPI_Type.Asc)
                    .ToList<TestPaperItem>();
            }
            return _getItemCoutomOrder(tpi);
        }
        /// <summary>
        /// 按章节抽题时，各题型占比
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <returns></returns>
        public List<TestPaperItem> GetItemForOlPercent(TestPaper tp)
        {
            List<TestPaperItem> tpi = new List<TestPaperItem>();    
            if (tp != null && !string.IsNullOrWhiteSpace(tp.Tp_FromConfig))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tp.Tp_FromConfig, false);
                XmlNodeList nodes = xmlDoc.SelectNodes("Config/Outline/Percent/TestPaperItem");
                for (int i = 0; i < nodes.Count; i++)
                {
                    TestPaperItem item = new TestPaperItem();
                    item.FromXML(nodes[i].OuterXml);
                    tpi.Add(item);
                }
            }
            return _getItemCoutomOrder(tpi);
        }
        /// <summary>
        /// 按章节抽题时，各章节题型数量
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <param name="olid">章节id，如果小于1，则取所有</param>
        /// <returns></returns>
        public List<TestPaperItem> GetItemForOlCount(TestPaper tp, long olid)
        {
            List<TestPaperItem> list = new List<TestPaperItem>();
            if (tp != null && !string.IsNullOrWhiteSpace(tp.Tp_FromConfig))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tp.Tp_FromConfig, false);
                XmlNodeList nodes = xmlDoc.SelectNodes("Config/Outline/Items/TestPaperItem");                
                for (int i = 0; i < nodes.Count; i++)
                {
                    TestPaperItem tpi = new TestPaperItem();                   
                    tpi.FromXML(nodes[i].OuterXml);
                    if (olid <= 0) list.Add(tpi);
                    if (olid > 0 && tpi.Ol_ID == olid) list.Add(tpi);
                }
            }
            return _getItemCoutomOrder(list);
        }
        /// <summary>
        /// 返回试卷的大项，不管是按课程，还是按章节
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public List<TestPaperItem> GetItemForAny(TestPaper tp)
        {
            List<TestPaperItem> tpi = null;
            List<TestPaperItem> list = new List<TestPaperItem>();
            if (tp.Tp_FromType == 0)
            {
                tpi = this.GetItemForAll(tp);                
                foreach (Song.Entities.TestPaperItem t in tpi)
                    if (t.TPI_Count > 0) list.Add(t);
            }
            if (tp.Tp_FromType == 1)
            {
                tpi = this.GetItemForOlPercent(tp);
                foreach (Song.Entities.TestPaperItem t in tpi)
                    if (t.TPI_Percent > 0) list.Add(t);
            }
            return _getItemCoutomOrder(list);
        }
        /// <summary>
        /// 自定义题型的出现顺序，方法比较弱智
        /// </summary>
        /// <param name="tpi"></param>
        /// <returns></returns>
        private List<TestPaperItem> _getItemCoutomOrder(List<TestPaperItem> tpi)
        {       
            //按照自己的思路排列顺序，方法有些笨            
            int[] order = { 1, 2, 5, 3, 4 };
            List<TestPaperItem> list = new List<TestPaperItem>();
            for (int i = 0; i < order.Length; i++)
            {
                for (int j = 0; j < tpi.Count; j++)
                {
                    TestPaperItem t = tpi[j];
                    if (order[i] == t.TPI_Type)
                    {
                        list.Add(t);
                        break;
                    }
                }
            }
            return list;
        }
        #endregion

        #region 出卷
        /// <summary>
        /// 出卷，输出试卷内容
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <returns></returns>
        public Dictionary<TestPaperItem, Questions[]> Putout(TestPaper tp)
        {
            if (tp.Tp_FromType == 1) 
                return _putout_1(tp);
            else
                return _putout_0(tp);
        }
        /// <summary>
        /// 按课程抽题组卷
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        private Dictionary<TestPaperItem, Questions[]> _putout_0(TestPaper tp)
        {
            //获取试题项,例如单选题、多选题
            List<Song.Entities.TestPaperItem> tpi = this.GetItemForAll(tp);           
            List<TestPaperItem> list = new List<TestPaperItem>();
            foreach (Song.Entities.TestPaperItem t in tpi)           
                if (t.TPI_Count > 0) list.Add(t);           
            tpi = _getItemCoutomOrder(list);
            //开始出卷
            Dictionary<TestPaperItem, Questions[]> dic = new Dictionary<TestPaperItem, Questions[]>();
            foreach (Song.Entities.TestPaperItem t in tpi)
            {
                int type = (int)t.TPI_Type;
                int count = (int)t.TPI_Count;    //当前题型的试题数
                float num = (float)t.TPI_Number;    //当前题型占的分数
                if (count < 1) continue;
                //当前类型的试题
                Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesRandom(tp.Org_ID, (int)tp.Sbj_ID, tp.Cou_ID, -1, type,tp.Tp_Diff, tp.Tp_Diff2, true, count);
                if (ques.Length < 1) continue;
                ques = clacScore(ques, num);
                dic.Add(t, ques);
            }
            return dic;
        }
        /// <summary>
        /// 按章节抽题组卷
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        private Dictionary<TestPaperItem, Questions[]> _putout_1(TestPaper tp)
        {
            //获取试题项,例如单选题、多选题
            List<Song.Entities.TestPaperItem> tpi = this.GetItemForOlPercent(tp);
            List<TestPaperItem> list = new List<TestPaperItem>();
            foreach (Song.Entities.TestPaperItem t in tpi) if (t.TPI_Percent > 0) list.Add(t);
            tpi = _getItemCoutomOrder(list);
            //***************  开始抽取试题
            List<Questions> listQus = new List<Questions>();
            //1、先取当前课程下的试题，按题型分好组
            List<Questions>[] arr = new List<Questions>[5];
            List<Questions> ques_course = Gateway.Default.From<Questions>().Where(Questions._.Cou_ID == tp.Cou_ID
                && Questions._.Qus_Diff >= tp.Tp_Diff && Questions._.Qus_Diff <= tp.Tp_Diff2).ToList<Questions>();
            for (int i = 0; i < arr.Length; i++)           
                arr[i] = (from l in ques_course where l.Qus_Type == i + 1 select l).ToList<Questions>();

            //2、获取章节，按章节配置项取试题
            List<Outline> outlines = Business.Do<IOutline>().OutlineCount(tp.Cou_ID, 0, true, 0);
            foreach (Outline ol in outlines)
            {
                //3、取当前章节的各题型数量
                List<TestPaperItem> tpols = this.GetItemForOlCount(tp, ol.Ol_ID);
                if (tpols.Count < 1) continue;
                foreach (TestPaperItem it in tpols)
                {
                    if (it.TPI_Count < 1) continue;
                    List<Questions> questions = arr[it.TPI_Type - 1];
                    if (questions == null || questions.Count < 1) continue;
                    
                    int isexist = tpi.FindIndex(t => it.TPI_Type == t.TPI_Type);
                    if (isexist < 0) continue;

                    //4、抽取试题，并将抽到题汇集到一起，即到listQus列表中去   
                    List<long> treeid = Business.Do<IOutline>().TreeID(ol.Ol_ID);
                    //自定义查询条件
                    Func<Questions, bool> exp = x =>
                    {
                        var where_exp = false;
                        foreach(long id in treeid)                     
                            where_exp = where_exp || x.Ol_ID == id;                      
                        return where_exp;
                    };                 
                    List<Questions> ques = questions.Where(exp).ToList<Questions>();                  
                    //将选中的试题随机抽取到总的试题数组，类似洗牌算法
                    int i = it.TPI_Count;
                    while (i > 0 && ques.Count > 0)
                    {
                        Random random = new Random(DateTime.Now.Millisecond + ques.Count);
                        int rnd = random.Next(ques.Count);
                        listQus.Add(ques[rnd]);
                        ques.RemoveAt(rnd);
                        i--;
                    }                   
                }
            }
            //****************  开始出卷
            Dictionary<TestPaperItem, Questions[]> dic = new Dictionary<TestPaperItem, Questions[]>();
            foreach (Song.Entities.TestPaperItem t in tpi)
            {
                float num = (float)t.TPI_Number;    //当前题型占的分数
                List<Questions> qusTm = new List<Questions>();
                //当前类型的试题
                foreach (Questions q in listQus)               
                    if (q.Qus_Type == t.TPI_Type) qusTm.Add(q);               
                if (qusTm.Count < 1) continue;
                Questions[] ques = clacScore(qusTm.ToArray(), num);
                dic.Add(t, ques);
            }
            return dic;
        }
        /// <summary>
        /// 计算每道试题的分数
        /// </summary>
        /// <param name="ques">试题</param>
        /// <param name="total">试题的总分</param>
        /// <returns></returns>
        private Song.Entities.Questions[] clacScore(Song.Entities.Questions[] ques, float total)
        {
            if (ques.Length < 1) return ques;
            //分配模式，1为按试题数分配总分; 2为按难度值分配总分
            int distribution_model = 1;

            //总分数（当前试题类型）
            decimal surplus = (decimal)total;
            //最难的题，用于放置多余分数，默认是最后一道
            Song.Entities.Questions diffMax = ques[ques.Length - 1];
            for (int i = ques.Length - 1; i >= 0; i--)
            {
                ques[i].Qus_Explain = ques[i].Qus_Answer = ques[i].Qus_ErrorInfo = string.Empty;
                if (ques[i].Qus_Diff > diffMax.Qus_Diff)
                    diffMax = ques[i];
            }
            //按题数分配分数
            if (distribution_model == 1)
            {
                decimal quesAvg = Math.Floor(surplus / ques.Length * 100) / 100;
                for (int j = 0; j < ques.Length; j++)
                {
                    ques[j].Qus_Number = (float)quesAvg;
                    surplus = surplus - quesAvg;
                }
                //将分不完的分数，添加到最难的题上
                if (surplus > 0) diffMax.Qus_Number += (float)surplus;
            }
            //按难度计算每道题的分数
            if (distribution_model == 2)
            {
                decimal diffSum = 0, diffAvg = 0;
                for (int i = ques.Length - 1; i >= 0; i--)              
                    diffSum += ques[i].Qus_Diff;
                //每一个难度点，占用多少分数
                diffAvg = (decimal)total / diffSum;
                //计算每一道题的分数，用难度值乘以diffAvg
                for (int j = 0; j < ques.Length; j++)
                {                 
                    decimal curr = ques[j].Qus_Diff * diffAvg;
                    curr = ((decimal)Math.Floor(curr * 100)) / 100;
                    ques[j].Qus_Number = (float)curr;
                    surplus = surplus - curr;
                }
                //将分不完的分数，添加到最难的题上
                if (surplus > 0) diffMax.Qus_Number += (float)surplus;
            }
            return ques;
        }
        #endregion

        #region 试卷测试的答题
        /// <summary>
        /// 添加测试成绩,返回得分
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="force"></param>
        /// <returns>返回得分</returns>
        public float ResultsAdd(TestResults entity, bool force)
        {
            entity.Tr_CrtTime = DateTime.Now;
            return ResultsSave(entity, force);
        }
        /// <summary>
        /// 修改测试成绩,返回得分
        /// </summary>
        /// <param name="entity">业务实体</param>
        /// <param name="force"></param>
        /// <returns>返回得分</returns>
        public float ResultsSave(TestResults entity, bool force)
        {        
            //计算得分
            float score = 0;
            //
            XmlDocument resXml = new XmlDocument();
            string resultXML = entity.Tr_Results;
            resXml.LoadXml(resultXML, false);
            XmlNode xn = resXml.SelectSingleNode("results");
            //是否已经计算过
            if (xn.Attributes["isclac"] != null)
            {
                if (xn.Attributes["isclac"].Value == "true" && force == false)
                {
                    if (xn.Attributes["score"] != null)
                    {
                        score = (float)Convert.ToDouble(xn.Attributes["score"].Value);
                    }
                }
                else
                {
                    //如果没有计算过，则在服务器端计算
                    entity.Tr_Score = (float)ExaminationCom._ClacScore(resultXML, out resultXML);
                    entity.Tr_Results = resultXML;
                    score = (float)entity.Tr_Score;
                }
            }
            if (entity.Cou_ID <= 0)
            {
                TestPaper pager = Gateway.Default.From<TestPaper>().Where(TestPaper._.Tp_Id == entity.Tp_Id).ToFirst<TestPaper>();
                if (pager != null)
                {
                    entity.Cou_ID = pager.Cou_ID;
                }
            }
            Gateway.Default.Save<TestResults>(entity);
            return score;           
        }
        /// <summary>
        /// 计算成绩，根据成绩id
        /// </summary>
        /// <param name="trid"></param>
        /// <returns></returns>
        public float ResultsCalc(int trid)
        {
            TestResults tr = Gateway.Default.From<TestResults>().Where(TestResults._.Tr_ID == trid).ToFirst<TestResults>();
            if (tr == null) return -1;
            return this.ResultsSave(tr, true);
        }
        /// <summary>
        /// 批量计算试卷的所有成绩
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public bool ResultsBatchCalc(long tpid)
        {
            List<TestResults> trs = Gateway.Default.From<TestResults>().Where(TestResults._.Tp_Id == tpid).ToList<TestResults>();
            for(int i = 0; i < trs.Count; i++)
            {
                this.ResultsSave(trs[i], true);
            }           
            return true;
        }
        /// <summary>
        /// 当前考试的及格率
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public float ResultsPassrate(long identify)
        {
            Song.Entities.TestPaper mm = Gateway.Default.From<TestPaper>().Where(TestPaper._.Tp_Id == identify).ToFirst<TestPaper>();
            //参考人次
            int sum = Gateway.Default.Count<TestResults>(TestResults._.Tp_Id == identify);
            //及格数
            int pass = Gateway.Default.Count<TestResults>(TestResults._.Tp_Id == identify && TestResults._.Tr_Score >= mm.Tp_PassScore);
            if (sum == 0) return 0;
            return pass / (float)sum;
        }
        /// <summary>
        /// 参考人次
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public int ResultsPersontime(long identify)
        {
            return Gateway.Default.Count<TestResults>(TestResults._.Tp_Id == identify);
        }
        /// <summary>
        /// 计算该试卷的所有测试的平均分
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public float ResultsAverage(long identify)
        {
            object obj = Gateway.Default.Avg<TestResults>(TestResults._.Tr_Score, TestResults._.Tp_Id == identify);
            if (obj == null) return 0;
            float avg = 0;
            float.TryParse(obj.ToString(), out avg);
            return avg;
        }
        /// <summary>
        /// 计算该试卷的所有测试的最高分
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public TestResults ResultsHighest(long identify)
        {
            return Gateway.Default.From<TestResults>().Where(TestResults._.Tp_Id == identify).OrderBy(TestResults._.Tr_Score.Desc).ToFirst<TestResults>();
        }
        /// <summary>
        /// 计算该试卷的某个学员的最高分
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <param name="stid">学员id</param>
        /// <returns></returns>
        public float ResultsHighest(long tpid, int stid)
        {
            WhereClip wc = new WhereClip();
            wc.And(TestResults._.Ac_ID == stid);
            wc.And(TestResults._.Tp_Id == tpid);
            object score = Gateway.Default.Max<TestResults>(TestResults._.Tr_Score, wc);
            return score == null ? 0 : (float)score;         
        }
        /// <summary>
        /// 计算该试卷的所有测试的最低分
        /// </summary>
        /// <param name="identify"></param>
        /// <returns></returns>
        public TestResults ResultsLowest(long identify)
        {
            return Gateway.Default.From<TestResults>().Where(TestResults._.Tp_Id == identify).OrderBy(TestResults._.Tr_Score.Asc).ToFirst<TestResults>();
        }
        /// <summary>
        /// 删除测试成绩，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        public void ResultsDelete(int identify)
        {
            Gateway.Default.Delete<TestResults>(TestResults._.Tr_ID == identify);
        }
        /// <summary>
        /// 清空某个试卷的某个学员的所有测试成绩
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="tpid">试卷id</param>
        public int ResultsClear(int acid, long tpid)
        {
            return Gateway.Default.Delete<TestResults>(TestResults._.Ac_ID == acid && TestResults._.Tp_Id == tpid);
        }
        /// <summary>
        /// 获取单一实体对象，按主键ID；
        /// </summary>
        /// <param name="identify">实体的主键</param>
        /// <returns></returns>
        public TestResults ResultsSingle(int identify)
        {
            return Gateway.Default.From<TestResults>().Where(TestResults._.Tr_ID == identify).ToFirst<TestResults>();
        }
        /// <summary>
        /// 获取某员工的测试成绩
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="couid"></param>
        /// <param name="search"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public TestResults[] ResultsCount(int stid, long couid, string search, int count)
        {
            WhereClip wc = TestResults._.Tr_ID > -1;
            if (stid > 0) wc.And(TestResults._.Ac_ID == stid);         
            if (couid > 0) wc.And(TestResults._.Cou_ID == couid);
            if (!string.IsNullOrWhiteSpace(search)) wc.And(TestResults._.Tp_Name.Contains(search));
            return Gateway.Default.From<TestResults>().Where(wc).OrderBy(TestResults._.Tr_CrtTime.Desc).ToArray<TestResults>(count);
        }
        /// <summary>
        /// 获取某员工的测试成绩
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="tpid"></param>    
        /// <returns></returns>
        public TestResults[] ResultsCount(int stid, long tpid)
        {
            WhereClip wc = new WhereClip();
            if (tpid > 0) wc.And(TestResults._.Tp_Id == tpid);
            if (stid > 0) wc.And(TestResults._.Ac_ID == stid);          
            return Gateway.Default.From<TestResults>().Where(wc).OrderBy(TestResults._.Tr_CrtTime.Desc).ToArray<TestResults>();
        }
        /// <summary>
        /// 分页获取测试成绩
        /// </summary>
        /// <param name="stid"></param>
        /// <param name="sbjid"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public TestResults[] ResultsPager(int stid, long sbjid, long couid, int size, int index, out int countSum)
        {
            WhereClip wc = TestResults._.Tr_ID > -1;
            if (stid > 0) wc.And(TestResults._.Ac_ID == stid);
            if (sbjid > 0) wc.And(TestResults._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(TestResults._.Cou_ID == couid);
            countSum = Gateway.Default.Count<TestResults>(wc);
            return Gateway.Default.From<TestResults>().Where(wc).OrderBy(TestResults._.Tr_CrtTime.Desc).ToArray<TestResults>(size, (index - 1) * size);
        }

        public TestResults[] ResultsPager(int stid, long tpid, string tpname, long couid, long sbjid, int orgid,
            string acc, string cardid, float score_min, float score_max, DateTime? time_min, DateTime? time_max,
            int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (stid > 0) wc.And(TestResults._.Ac_ID == stid);
            if (tpid > 0) wc.And(TestResults._.Tp_Id == tpid);
            if (sbjid > 0) wc.And(TestResults._.Sbj_ID == sbjid);
            if (couid > 0) wc.And(TestResults._.Cou_ID == couid);
            if (orgid > 0) wc.And(TestResults._.Org_ID == orgid);
            if (!string.IsNullOrWhiteSpace(tpname)) wc.And(TestResults._.Tp_Name.Contains(tpname));

            if (!string.IsNullOrWhiteSpace(acc)) wc.And(TestResults._.Ac_Name.Contains(acc));
            if (!string.IsNullOrWhiteSpace(cardid)) wc.And(TestResults._.St_IDCardNumber.Contains(cardid));
            //成绩区间
            if (score_min >= 0) wc.And(TestResults._.Tr_Score >= score_min);
            if (score_max >= 0) wc.And(TestResults._.Tr_Score <= score_max);
            //时间区间
            if (time_min!=null) wc.And(TestResults._.Tr_CrtTime >=(DateTime)time_min);
            if (time_max != null) wc.And(TestResults._.Tr_CrtTime <= (DateTime)time_max);

            countSum = Gateway.Default.Count<TestResults>(wc);
            TestResults[] exr = Gateway.Default.From<TestResults>().Where(wc).OrderBy(TestResults._.Tr_CrtTime.Desc).ToArray<TestResults>(size, (index - 1) * size);
            return exr;
        }
        /// <summary>
        /// 按试卷分页返回测试成绩
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public TestResults[] ResultsPager(int stid, long tpid, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (stid > 0) wc &= TestResults._.Ac_ID == stid;
            if (tpid > 0) wc &= TestResults._.Tp_Id == tpid;
            countSum = Gateway.Default.Count<TestResults>(wc);
            TestResults[] exr = Gateway.Default.From<TestResults>().Where(wc).OrderBy(TestResults._.Tr_CrtTime.Desc).ToArray<TestResults>(size, (index - 1) * size);
            return exr;
        }
        #endregion
    }
}
