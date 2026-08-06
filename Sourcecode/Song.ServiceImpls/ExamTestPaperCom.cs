using System;
using System.Collections.Generic;
using System.Text;
using System.Data;


using WeiSha.Core;
using Song.Entities;

using WeiSha.Data;
using Song.ServiceInterfaces;
using System.Data.Common;
using System.Xml;



namespace Song.ServiceImpls
{
    public class ExamTestPaperCom : IExamTestPaper
    {
        #region 相关操作类
        private static OrganizationCom orgCom = new OrganizationCom();
        private static SubjectCom subjectCom = new SubjectCom();
        private static CourseCom courseCom = new CourseCom();
        private static OutlineCom outlineCom = new OutlineCom();
        private static QuestionsCom questionsCom = new QuestionsCom();
        private static AccountsCom accountCom = new AccountsCom();
        private static TestPaperCom testPaperCom = new TestPaperCom();
        #endregion
        #region 试卷管理
        /// <summary>
        /// 添加试卷
        /// </summary>
        /// <param name="entity">试卷对象</param>
        public long PaperAdd(ExamTestPaper entity)
        {
            if (entity.Etp_Id <= 0) entity.Etp_Id = WeiSha.Core.Request.SnowID();

            if (entity.Org_ID <= 0)
            {
                Song.Entities.Organization org = orgCom.OrganCurrent();
                if (org != null) entity.Org_ID = org.Org_ID;
               
            }
            entity.Etp_CrtTime = DateTime.Now;
            //判断是否有简答题，还没有编写
            entity.Etp_IsManual = this.PaperIsManual(entity);
            //
            Gateway.Default.Save<ExamTestPaper>(entity);
            return entity.Etp_Id;
        }
        /// <summary>
        /// 修改试卷
        /// </summary>
        /// <param name="entity">业务实体</param>
        public void PaperSave(ExamTestPaper entity)
        {
            entity.Etp_Lasttime = DateTime.Now;
            //判断是否有简答题
            entity.Etp_IsManual = this.PaperIsManual(entity);
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    tran.Save<ExamTestPaper>(entity);                 
                   
                    tran.Update<Examination>(new Field[] { Examination._.Exam_PassScore, Examination._.Exam_Total, Examination._.Exam_IsManual, Examination._.Exam_QuesCount },
                        new object[] { entity.Etp_PassScore, entity.Etp_Total, entity.Etp_IsManual, entity.Etp_Count }, Examination._.Etp_Id == entity.Etp_Id);
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
        /// <param name="etpid">试卷的id</param>
        /// <param name="fiels"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public bool PaperUpdate(long etpid, Field[] fiels, object[] objs)
        {
            Gateway.Default.Update<ExamTestPaper>(fiels, objs, ExamTestPaper._.Etp_Id == etpid);
            return true;
        }
        /// <summary>
        /// 删除试卷，按主键ID；
        /// </summary>
        /// <param name="etpid">实体的主键</param>
        public int PaperDelete(long etpid)
        {
            int count = Gateway.Default.From<Examination>().Where(Examination._.Etp_Id == etpid).Count();
            if (count > 0) throw new WeiSha.Core.ExceptionForPrompt($"当前试卷已被考试采用，不能删除");
            return Gateway.Default.Update<ExamTestPaper>(new Field[] { ExamTestPaper._.Etp_IsDeleted, ExamTestPaper._.Etp_DeleteTime },
                new object[] { true, DateTime.Now },
                ExamTestPaper._.Etp_Id == etpid && ExamTestPaper._.Etp_IsDeleted == false);
        }
        /// <summary>
        /// 回收，标记删除状态为false
        /// </summary>
        public int PaperRecycle(long etpid)
        {
            return Gateway.Default.Update<ExamTestPaper>(ExamTestPaper._.Etp_IsDeleted, false, ExamTestPaper._.Etp_Id == etpid && ExamTestPaper._.Etp_IsDeleted == true);
        }
        /// <summary>
        /// 真正删除，按主键ID；
        /// </summary>
        /// <param name="etpid">实体的主键</param>
        public int PaperRemove(long etpid)
        {            
            Song.Entities.ExamTestPaper tp = this.PaperSingle(etpid);
            if (tp == null) return 0;
            int count = Gateway.Default.From<Examination>().Where(Examination._.Etp_Id == etpid).Count();
            if (count > 0) throw new WeiSha.Core.ExceptionForPrompt($"当前试卷《“{tp.Etp_Name}”》已被考试采用，不能删除");
            using (DbTrans tran = Gateway.Default.BeginTrans())
            {
                try
                {
                    Examination exam = Gateway.Default.From<Examination>().Where(Examination._.Etp_Id == etpid).ToFirst<Examination>();
                    if (exam != null) throw new WeiSha.Core.ExceptionForPrompt($"试卷“{tp.Etp_Name}”已被考试采用，不能删除");

                    tran.Delete<ExamTestPaper>(ExamTestPaper._.Etp_Id == etpid);
                    //删除图片文件
                    string img = WeiSha.Core.Upload.Get["ExamTestPaper"].Physics + tp.Etp_Logo;
                    if (System.IO.File.Exists(img)) System.IO.File.Delete(img);
                    //删除成绩
                    tran.Delete<ExamResults>(ExamResults._.Etp_Id == etpid);
                    WeiSha.Core.Upload.Get["ExamTestPaper"].DeleteDirectory(tp.Etp_Id.ToString());
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            return 1;
        }
        /// <summary>
        /// 获取单一试卷实体对象，按主键ID；
        /// </summary>
        /// <param name="etpid">实体的主键</param>
        /// <returns></returns>
        public ExamTestPaper PaperSingle(long etpid)
        {
            return  Gateway.Default.From<ExamTestPaper>().Where(ExamTestPaper._.Etp_Id == etpid).ToFirst<ExamTestPaper>();
        }
        /// <summary>
        /// 获取单一试卷实体对象，按试卷名称；
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ExamTestPaper PaperSingle(string name)
        {
            return Gateway.Default.From<ExamTestPaper>().Where(ExamTestPaper._.Etp_Name == name).ToFirst<ExamTestPaper>();
        }
        /// <summary>
        /// 判断是否有简答题
        /// </summary>
        public bool PaperIsManual(long etpid)
        {
            ExamTestPaper tp=this.PaperSingle(etpid);
            if (tp == null) return false;
            return PaperIsManual(tp);
        }
        /// <summary>
        /// 判断是否有简答题
        /// </summary>
        public bool PaperIsManual(ExamTestPaper entity)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(entity.Etp_FromConfig);
            //各题型的占比
            XmlNodeList nodeitems = xmldoc.SelectNodes("/testpaper/questions/ques");
            if (nodeitems != null && nodeitems.Count > 0)
            {
                foreach (XmlNode item in nodeitems)
                {
                    if (item.Attributes["type"]?.Value == "4")
                    {
                        if(Convert.ToInt32(item.Attributes["count"]?.Value) > 0) return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取指定数据的试卷
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="accid">管理员id</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count">指定数量</param>
        /// <returns></returns>
        public List<ExamTestPaper> PaperCount(int orgid, int accid, bool? isdeleted, int diff, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (accid > 0) wc &= ExamTestPaper._.Acc_Id == accid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;          
            return Gateway.Default.From<ExamTestPaper>().Where(wc).OrderBy(ExamTestPaper._.Etp_Id.Desc).ToList<ExamTestPaper>(count);
        }
        /// <summary>
        /// 获取指定数据的试卷
        /// </summary>
        /// <param name="search"></param>
        /// <param name="accid">管理员id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="diff"></param>
        /// <param name="isUse"></param>
        /// <param name="count">指定数量</param>
        /// <returns></returns>
        public List<ExamTestPaper> PaperCount(int orgid, string search, int accid, bool? isdeleted, int diff, bool? isUse, int count)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (accid > 0) wc &= ExamTestPaper._.Acc_Id == accid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(search)) wc &= ExamTestPaper._.Etp_Name.Contains(search);
            return Gateway.Default.From<ExamTestPaper>().Where(wc).OrderBy(ExamTestPaper._.Etp_Id.Desc).ToList<ExamTestPaper>(count);
        }
        /// <summary>
        /// 计算有多少个试卷
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="diff"></param>
        /// <param name="isdeleted"></param>
        /// <param name="isUse"></param>
        /// <returns></returns>
        public int PaperOfCount(int orgid, int diff, bool? isdeleted, bool? isUse)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;
            return Gateway.Default.Count<ExamTestPaper>(wc);
        }
        /// <summary>
        /// 分页获取试卷
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="accid">管理员id</param>
        /// <param name="diff">难度等级</param>
        /// <param name="isUse">是否使用</param>
        /// <param name="sear">标题检索</param>
        /// <param name="isdeleted">是否删除</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <param name="countSum"></param>
        /// <returns></returns>
        public List<ExamTestPaper> PaperPager(int orgid, int accid, string sear, bool? isdeleted, int diff, bool? isUse, int size, int index, out int countSum)
        {
            WhereClip wc = new WhereClip();
            if (orgid > 0) wc &= ExamTestPaper._.Org_ID == orgid;
            if (accid > 0) wc &= ExamTestPaper._.Acc_Id == accid;
            if (diff > 0) wc &= (ExamTestPaper._.Etp_Diff <= diff && ExamTestPaper._.Etp_Diff2 >= diff);
            if (isdeleted != null) wc &= ExamTestPaper._.Etp_IsDeleted == (bool)isdeleted;
            if (isUse != null) wc &= ExamTestPaper._.Etp_IsUse == (bool)isUse;
            if (!string.IsNullOrWhiteSpace(sear)) wc &= ExamTestPaper._.Etp_Name.Contains(sear);
            countSum = Gateway.Default.Count<ExamTestPaper>(wc);
            OrderByClip orderBy = new OrderByClip();
            if (isdeleted != null && isdeleted == true) orderBy &= ExamTestPaper._.Etp_DeleteTime.Desc;
            orderBy &= ExamTestPaper._.Etp_Id.Desc;
            return Gateway.Default.From<ExamTestPaper>().Where(wc).OrderBy(orderBy).ToList<ExamTestPaper>(size, (index - 1) * size);
        }

        #endregion

        #region 试卷的试题项
        /// <summary>
        /// 试题数量
        /// </summary>
        /// <param name="etpid"></param>
        /// <returns></returns>
        public int QuesCount(long etpid)
        {
            object obj = Gateway.Default.From<ExamTestPaper>().Where(ExamTestPaper._.Etp_Id == etpid).Select(ExamTestPaper._.Etp_Count).ToScalar();
            return obj == null ? 0 : Convert.ToInt32(obj);
        }
        #endregion

        #region 出卷
        /// <summary>
        /// 获取试卷的试题项
        /// </summary>
        public List<TestPaperItem> PaperItems(long etpid)
        {
            ExamTestPaper paper = this.PaperSingle(etpid);
            return this.PaperItems(paper);
        }
        /// <summary>
        /// 获取试卷的试题项
        /// </summary>
        public List<TestPaperItem> PaperItems(ExamTestPaper tp)
        {
            if (tp == null) return null;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tp.Etp_FromConfig);
            //
            XmlNode rootnode = doc.SelectSingleNode("/testpaper");
            if (rootnode == null) throw new Exception("试卷配置文件有误");
            List<TestPaperItem> list = new List<TestPaperItem>();
            //试题大项
            foreach (XmlNode quesnode in rootnode.SelectNodes("questions/ques"))
            {
                TestPaperItem item = new TestPaperItem();
                item.TPI_Type = quesnode.GetAttr<int>("type");       //试题类型，数值
                item.TPI_TypeName = quesnode.GetAttr("byname");        //试题类型的别名，中文               
                item.TPI_Percent = quesnode.GetAttr<int>("percent");     //当前题型的分数占比
                item.TPI_Number = quesnode.GetAttr<int>("number");
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// 出卷，输出试卷内容
        /// </summary>
        /// <param name="etpid">试卷id</param>
        /// <param name="isanswer">试题是否带答案，模拟考试一般带答案，方便前端计算成绩</param>
        /// <returns></returns>
        public Dictionary<TestPaperItem, List<Questions>> Putout(long etpid, bool isanswer)
        {
            ExamTestPaper paper = this.PaperSingle(etpid);
            return Putout(paper, isanswer);
        }
        /// <summary>
        /// 出卷，输出试卷内容
        /// </summary>
        /// <param name="tp">试卷对象</param>
        /// <param name="isanswer">试题是否带答案，模拟考试一般带答案，方便前端计算成绩</param>
        /// <returns></returns>
        public Dictionary<TestPaperItem, List<Questions>> Putout(ExamTestPaper tp, bool isanswer)
        {
            if (tp.Etp_Type == 1) return _putout_1(tp, isanswer);
            else return _putout_2(tp, isanswer);
        }
        /// <summary>
        /// 静态试卷的出卷
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="isanswer"></param>
        /// <returns></returns>
        private Dictionary<TestPaperItem, List<Questions>> _putout_1(ExamTestPaper tp, bool isanswer)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tp.Etp_FromConfig);
            //
            XmlNode rootnode=doc.SelectSingleNode("/testpaper");
            if (rootnode == null) throw new Exception("试卷配置文件有误");
        
            Dictionary<TestPaperItem, List<Questions>> dic = new Dictionary<TestPaperItem, List<Questions>>();
            //试题大项
            foreach (XmlNode quesnode in rootnode.SelectNodes("questions/ques"))
            {
                TestPaperItem item = new TestPaperItem();
                item.TPI_Type = quesnode.GetAttr<int>("type");        //试题类型，数值
                item.TPI_TypeName = quesnode.GetAttr("byname");        //试题类型的别名，中文
                //item.TPI_Count = quesnode.Attributes["count"].Value.Convert<int>();     //试题数量，这是xml中记录的数量，不是实际数量
                item.TPI_Percent = quesnode.GetAttr<int>("percent");     //当前题型的分数占比
                item.TPI_Number = quesnode.GetAttr<int>("number");              
                //
                List<Questions> queslist=new List<Questions>();
                foreach (XmlNode qnode in quesnode.SelectNodes("q"))
                { 
                    long qid = qnode.GetAttr<long>("qid");
                    Questions ques = Gateway.Default.From<Questions>().Where(Questions._.Qus_ID == qid).ToFirst<Questions>();
                    if (ques != null)queslist.Add(ques);
                }
                item.TPI_Count = queslist.Count;
                dic.Add(item, queslist);
            }
            //计算每道试题的分数
            foreach (KeyValuePair<Song.Entities.TestPaperItem, List<Questions>>  item in dic)
            {
                float num = (float)item.Key.TPI_Number;    //当前题型占的分数
                List<Questions> qusTm = item.Value;
                _clacQuesScore(qusTm, num, isanswer);               
            }
            return dic;
        }
        /// <summary>
        ///动态试卷的出卷
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="isanswer"></param>
        /// <returns></returns>
        private Dictionary<TestPaperItem, List<Questions>> _putout_2(ExamTestPaper tp, bool isanswer)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tp.Etp_FromConfig);
            //
            XmlNode rootnode = doc.SelectSingleNode("/testpaper");
            if (rootnode == null) throw new Exception("试卷配置文件有误");
            //试卷类型
            int type = rootnode.Attributes["type"].Value.Convert<int>();
            //试卷关联的分类、标签、知识点
            long[] qpid = null, tagid = null, knlid = null;
            XmlNode rangenode=rootnode.SelectSingleNode("range");
            if (rangenode != null)
            {
                XmlNode parts=rangenode.SelectSingleNode("parts");
                if (parts != null) qpid = string.IsNullOrWhiteSpace(parts.InnerText) ? new long[] { } : parts.InnerText.ToArray<long>();
                XmlNode knls = rangenode.SelectSingleNode("knls");
                if (knls != null) knlid = string.IsNullOrWhiteSpace(knls.InnerText) ? new long[] { } : knls.InnerText.ToArray<long>();
                XmlNode tags = rangenode.SelectSingleNode("tags");
                if (tags != null) tagid = string.IsNullOrWhiteSpace(tags.InnerText) ? new long[] { } : tags.InnerText.ToArray<long>();
            }

            Dictionary<TestPaperItem, List<Questions>> dic = new Dictionary<TestPaperItem, List<Questions>>();
            //试题大项
            foreach (XmlNode quesnode in rootnode.SelectNodes("questions/ques"))
            {
                TestPaperItem item = new TestPaperItem();
                item.TPI_Type = quesnode.GetAttr<int>("type");       //试题类型，数值
                item.TPI_TypeName = quesnode.GetAttr("byname");        //试题类型的别名，中文
                item.TPI_Count = quesnode.GetAttr<int>("count");     //试题数量，这是xml中记录的数量，不是实际数量
                item.TPI_Percent = quesnode.GetAttr<int>("percent");     //当前题型的分数占比
                item.TPI_Number = quesnode.GetAttr<int>("number");
                List<Questions> queslist = new List<Questions>();
                if (item.TPI_Count > 0)
                {
                    queslist = Business.Do<IExamQues>().QuesRandom(0, qpid, tagid, knlid, item.TPI_Type, tp.Etp_Diff, tp.Etp_Diff2, true, item.TPI_Count);
                    item.TPI_Count = queslist.Count;
                }
                dic.Add(item, queslist);
            }
            //计算每道试题的分数
            foreach (KeyValuePair<Song.Entities.TestPaperItem, List<Questions>> item in dic)
            {
                float num = (float)item.Key.TPI_Number;    //当前题型占的分数
                List<Questions> qusTm = item.Value;
                _clacQuesScore(qusTm, num, isanswer);
            }
            return dic;
        }
        /// <summary>
        /// 计算每道试题的分数
        /// </summary>
        /// <param name="ques">试题</param>
        /// <param name="total">试题的总分</param>
        /// <param name="isanswer"></param>
        /// <returns></returns>
        private List<Questions> _clacQuesScore(List<Questions> ques, float total, bool isanswer)
        {
            if (ques.Count < 1) return ques;
            //分配模式，1为按试题数分配总分; 2为按难度值分配总分
            int distribution_model = 1;

            //总分数（当前试题类型）
            decimal surplus = (decimal)total;
            //最难的题，用于放置多余分数，默认是最后一道
            Song.Entities.Questions diffMax = ques[ques.Count - 1];
            for (int i = ques.Count - 1; i >= 0; i--)
            {
                //试题解析、错误信息，不向外输出
                ques[i].Qus_Explain = ques[i].Qus_ErrorInfo = string.Empty;
                if (!isanswer) ques[i] = _clearAnswer(ques[i]);

                if (ques[i].Qus_Diff > diffMax.Qus_Diff)
                    diffMax = ques[i];
            }
            //按题数分配分数
            if (distribution_model == 1)
            {
                decimal quesAvg = Math.Floor(surplus / ques.Count * 100) / 100;
                for (int j = 0; j < ques.Count; j++)
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
                for (int i = ques.Count - 1; i >= 0; i--)
                    diffSum += ques[i].Qus_Diff;
                //每一个难度点，占用多少分数
                diffAvg = (decimal)total / diffSum;
                //计算每一道题的分数，用难度值乘以diffAvg
                for (int j = 0; j < ques.Count; j++)
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
        /// <summary>
        /// 清除答案
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        private Questions _clearAnswer(Questions q)
        {
            if (q.Qus_Type == 1 || q.Qus_Type == 2)
            {
                XmlDocument doc = new XmlDocument();
                q.Qus_Items = q.Qus_Items.Replace((char)0x0B, ' ');
                doc.LoadXml(q.Qus_Items);
                foreach (XmlElement n in doc.SelectNodes("Items/item"))
                    n.SelectSingleNode("Ans_IsCorrect").InnerText = string.Empty;
                q.Qus_Items = doc.OuterXml;
            }
            if (q.Qus_Type == 3) q.Qus_IsCorrect = false;
            //简答题，答案清空
            if (q.Qus_Type == 4) q.Qus_Answer = string.Empty;
            //填空题
            if (q.Qus_Type == 5)
            {
                XmlDocument doc = new XmlDocument();
                q.Qus_Items = q.Qus_Items.Replace((char)0x0B, ' ');
                doc.LoadXml(q.Qus_Items);
                foreach (XmlElement n in doc.SelectNodes("Items/item"))
                    n.SelectSingleNode("Ans_Context").InnerText = string.Empty;
                q.Qus_Items = doc.OuterXml;
            }
            return q;
        }
        /// <summary>
        /// 出卷，按历史答题内容生成试卷
        /// </summary>
        /// <param name="results">学员答题的xml记录</param>
        /// <param name="isanswer">试题是否带答案，模拟考试一般带答案，方便前端计算成绩</param>
        /// <returns></returns>
        public Dictionary<TestPaperItem, List<Questions>> Putout(string results, bool isanswer)
        {
            XmlDocument docxml = new XmlDocument();
            docxml.XmlResolver = null;
            docxml.LoadXml(results, false);
            return Putout(docxml, isanswer);
        }
        /// <summary>
        /// 出卷，按历史答题内容生成试卷
        /// </summary>
        public Dictionary<TestPaperItem, List<Questions>> Putout(XmlDocument resxml, bool isanswer)
        {
            XmlNode results = resxml.SelectSingleNode("results");
            long tpid = results.GetAttr<long>("tpid");  //试卷id
            ExamTestPaper tp = this.PaperSingle(tpid);
            if (tp == null) return null;
            List<TestPaperItem> items = this.PaperItems(tp);
            Dictionary<TestPaperItem, List<Questions>> dic = new Dictionary<TestPaperItem, List<Questions>>();
            foreach (TestPaperItem item in items)
            {
                XmlNode xn= results.SelectSingleNode($"ques[@type='{item.TPI_Type}']");
                if (xn == null) continue;
                List<Questions> qlist = new List<Questions>();
                for (int n = 0; n < xn.ChildNodes.Count; n++)
                {
                    XmlNode qn = xn.ChildNodes[n];
                    long qid = qn.GetAttr<long>("id");       //试题id                 
                    if (qid <= 0) continue;
                    Song.Entities.Questions q = questionsCom.QuesSingle(qid);
                    if (q == null) continue;

                    //试题分数
                    q.Qus_Number = qn.GetAttr<float>("num");
                    q.Qus_Explain = q.Qus_Answer = "";
                    if (!isanswer) q = _clearAnswer(q);
                    qlist.Add(q);
                }
                dic.Add(item, qlist);
            }           
            return dic;
        }
        #endregion
    }
}
