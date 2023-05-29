using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Newtonsoft.Json.Linq;
//using System.Web.Mvc;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using WeiSha.Core;


namespace Song.ViewData.Methods
{
    /// <summary>
    /// 考试管理
    /// </summary>
    [HttpPut, HttpGet]
    public class Exam : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string PathKey = "Exam";
        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;

        #region 考试主题

        /// <summary>
        /// 考试主题
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>    
        public Song.Entities.Examination ThemeForUID(string uid)
        {
            return Business.Do<IExamination>().ExamSingle(uid);
        }
        /// <summary>
        /// 考试主题
        /// </summary>
        /// <param name="entity">考试主题</param>
        /// <returns></returns>    
        public Song.Entities.Examination ThemeModify(Song.Entities.Examination entity)
        {
            Song.Entities.Examination old = Business.Do<IExamination>().ExamSingle(entity.Exam_ID);
            if (old == null) throw new Exception("Not found entity for Examination！");

            old.Copy<Song.Entities.Examination>(entity);
            Business.Do<IExamination>().ExamSave(old);
            return entity;
        }
        /// <summary>
        /// 删除考试
        /// </summary>
        /// <param name="id">考试主题的id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin,Teacher]
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
                    Business.Do<IExamination>().ExamDelete(idval);
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
        /// 修改考试的状态
        /// </summary>
        /// <param name="id">考试的id，可以是多个，用逗号分隔</param>
        /// <param name="use">是否启用</param>    
        /// <returns></returns>
        [HttpPost]
        [Admin,Teacher]
        public int ModifyState(string id, bool use)
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
                    Song.Entities.Examination old = Business.Do<IExamination>().ExamSingle(idval);
                    if (old == null) continue;
                    old.Exam_IsUse = use;
                    Business.Do<IExamination>().ExamSave(old);
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
        /// 
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="items"></param>
        /// <param name="groups"></param>
        [HttpPost]
        [Admin, Teacher]
        public bool Add(Examination theme, Examination[] items, ExamGroup[] groups)
        {
            Song.Entities.Teacher teacher = this.Teacher;
            Business.Do<IExamination>().ExamAdd(teacher, theme, items, groups);
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="theme"></param>
        /// <param name="items"></param>
        /// <param name="groups"></param>
        [HttpPost]
        [Admin, Teacher]
        public bool Modify(Examination theme, Examination[] items, ExamGroup[] groups)
        {
            Song.Entities.Examination old = Business.Do<IExamination>().ExamSingle(theme.Exam_ID);
            if (old == null) throw new Exception("Not found entity for Examination！");
            old.Copy<Song.Entities.Examination>(theme);
            //考试场次
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    Song.Entities.Examination ex_old = Business.Do<IExamination>().ExamSingle(items[i].Exam_ID);
                    if (ex_old != null)
                    {
                        ex_old.Copy<Song.Entities.Examination>(items[i]);
                        items[i] = ex_old;
                    }
                }
            }
            //与学员组的关联
            List<ExamGroup> sorts = null;
            if (groups != null) sorts = groups.ToList<ExamGroup>();

            Business.Do<IExamination>().ExamSave(old, items, groups);

            return true;
        }
        #endregion

        #region 出卷与交卷
        /// <summary>
        /// 当前考试的状态
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public JObject State(int examid)
        {
            JObject jo = new JObject();
            //学员是否登录
            Song.Entities.Accounts acc = LoginAccount.Status.User();
            jo.Add("islogin", acc != null);
            if (acc == null) return jo;

            //是否可以考试
            Song.Entities.Examination exam = Business.Do<IExamination>().ExamSingle(examid);

            if (exam == null || !exam.Exam_IsUse || exam.Exam_IsTheme)
                jo.Add("exist", false);
            else
                jo.Add("exist", true);
            if (exam != null)
            {
                jo.Add("uid", exam.Exam_UID);
                jo.Add("subject", exam.Sbj_ID.ToString());
                jo.Add("paper", exam.Tp_Id.ToString());
            }
            else
            {
                return jo;
            }
            //1为固定时间开始，2为限定时间区间考试
            if (exam != null) jo.Add("type", exam.Exam_DateType);
            //当前考生是否可以参加该场考试
            bool isAllow = Business.Do<IExamination>().ExamIsForStudent(examid, acc.Ac_ID);
            jo.Add("allow", isAllow);

            //关于考试状态的一些时间
            bool isStart, isOver, isSubmit;
            DateTime startTime, overTime;
            //答题记录
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultForCache(examid, exam.Tp_Id, acc.Ac_ID);
            if (exr != null) jo.Add("exrid", exr.Exr_ID);
            //判断是否已经开始、是否已经结束
            if (exam.Exam_DateType == 1)
            {
                //固定时间开始
                isStart = exam.Exam_Date <= DateTime.Now;    //是否开始
                isOver = DateTime.Now > exam.Exam_Date.AddMinutes(exam.Exam_Span);   //是否结束
                startTime = exam.Exam_Date;           //开始时间
                overTime = exam.Exam_Date.AddMinutes(exam.Exam_Span);     //结束时间
                isSubmit = exr != null ? exr.Exr_IsSubmit : false;    //是否交卷
            }
            else
            {
                //按时间区间
                isStart = DateTime.Now > exam.Exam_Date && DateTime.Now < exam.Exam_DateOver;    //是否开始
                isOver = DateTime.Now > exam.Exam_DateOver;   //是否结束
                startTime = exam.Exam_Date <= DateTime.Now ? DateTime.Now : exam.Exam_Date;        //开始时间
                overTime = exam.Exam_DateOver.AddMinutes(exam.Exam_Span);     //结束时间
                isSubmit = exr != null ? exr.Exr_IsSubmit : false;    //是否交卷               
                if (exr != null && !string.IsNullOrWhiteSpace(exr.Exr_Results))
                {
                    XmlDocument resXml = new XmlDocument();
                    resXml.LoadXml(exr.Exr_Results, false);
                    XmlNode xn = resXml.LastChild;
                    //考试的开始与结束时间，防止学员刷新考试界面，导致时间重置
                    long lbegin, lover;
                    long.TryParse(xn.Attributes["begin"] != null ? xn.Attributes["begin"].Value : "0", out lbegin);
                    long.TryParse(xn.Attributes["overtime"] != null ? xn.Attributes["overtime"].Value : "0", out lover);
                    lbegin = lbegin * 10000;
                    lover = lover * 10000;
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    DateTime beginTime = dtStart.Add(new TimeSpan(lbegin));
                    overTime = dtStart.Add(new TimeSpan(lover));    //得到转换后的结束时间
                    startTime = exam.Exam_Date <= beginTime ? beginTime : exam.Exam_Date;        //开始时间
                    isOver = DateTime.Now > overTime;
                    isSubmit = DateTime.Now > overTime || exr.Exr_IsSubmit; //是否交卷
                }
            }
            jo.Add("isstart", isStart);
            jo.Add("isover", isOver);
            jo.Add("startTime", WeiSha.Core.Server.getTime(startTime));
            jo.Add("overTime", WeiSha.Core.Server.getTime(overTime));
            jo.Add("issubmit", isSubmit);
            //正在考试
            bool doing = startTime <= DateTime.Now && overTime > DateTime.Now && !isSubmit;
            jo.Add("doing", doing && !isOver);
            //答题详情
            jo.Add("result", _resultJson(exr));
            return jo;
        }
        /// <summary>
        /// 根据考试答题记录的ID，获取详细的答题信息
        /// </summary>
        /// <param name="exrid"></param>
        /// <returns></returns>
        public JObject ResultJson(int exrid)
        {
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultSingle(exrid);
            return _resultJson(exr);
        }
        private JObject _resultJson(Song.Entities.ExamResults exr)
        {
            JObject jo = new JObject();
            if (exr == null || string.IsNullOrWhiteSpace(exr.Exr_Results)) return jo;
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null;
            resXml.LoadXml(exr.Exr_Results, false);
            //           
            XmlNode root = resXml.LastChild;
            foreach (XmlAttribute attr in root.Attributes)
                jo.Add(attr.Name, attr.Value);
            JArray qgroup = new JArray();
            XmlNodeList quesGroupNodes = resXml.GetElementsByTagName("ques");
            for (int i = 0; i < quesGroupNodes.Count; i++)
            {
                XmlNode node = quesGroupNodes[i];
                JObject ques = new JObject();
                int type = Convert.ToInt32(node.Attributes["type"].Value);
                ques.Add("type", type);
                float count = 0, number = 0;
                float.TryParse(node.Attributes["count"].Value, out count);
                float.TryParse(node.Attributes["number"].Value, out number);
                ques.Add("count", count);
                ques.Add("number", number);
                JArray qarray = new JArray();
                for (int n = 0; n < node.ChildNodes.Count; n++)
                {
                    JObject jq = new JObject();
                    XmlNode q = node.ChildNodes[n];
                    jq.Add("id", q.Attributes["id"].Value);

                    string ans = string.Empty, file = string.Empty;
                    //如果是单选、多选、判断
                    if (type == 1 || type == 2 || type == 3)
                        ans = q.Attributes["ans"].Value;
                    if (type == 4 || type == 5) ans = q.InnerText;
                    if (type == 5) file = q.Attributes["file"] != null ? q.Attributes["file"].Value : "";
                    ans = Html.ClearHTML(ans);
                    jq.Add("ans", ans);
                    if (q.Attributes["file"] != null) jq.Add("file", file);

                    if (q.Attributes["class"] != null) jq.Add("cls", q.Attributes["class"].Value);                   
                    if (q.Attributes["num"] != null) jq.Add("num", q.Attributes["num"].Value);
                    qarray.Add(jq);
                }
                ques.Add("q", qarray);
                qgroup.Add(ques);
            }
            jo.Add("ques", qgroup);
            return jo;
        }
        /// <summary>
        /// 出卷
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="stid">学员id</param>
        /// <returns></returns>
        public JArray MakeoutPaper(int examid, long tpid,int stid)
        {
            //获取答题信息
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultForCache(examid, tpid, stid);
            JArray array = new JArray();
            if (exr == null)
            {
                //第一次，随机出题
                array = _putout(tpid);
            }
            else
            {
                //如果已经交过卷
                if (exr.Exr_IsSubmit)
                {                   
                    throw new Exception("已经交过卷");
                }
                array = _putout(exr); 
            }
            return array;
        }
        /// <summary>
        /// 随机出题
        /// </summary>
        /// <returns></returns>
        private JArray _putout(long tpid)
        {
            //取果是第一次打开，则随机生成试题，此为获取试卷
            Song.Entities.TestPaper paper = Business.Do<ITestPaper>().PaperSingle(tpid);
            if (paper == null) return null;
            //生成试卷
            Dictionary<TestPaperItem, Questions[]> dics = Business.Do<ITestPaper>().Putout(paper);
            JArray jarr = new JArray();
            foreach (var di in dics)
            {
                //按题型输出
                Song.Entities.TestPaperItem pi = (Song.Entities.TestPaperItem)di.Key;   //试题类型                
                Song.Entities.Questions[] questions = (Song.Entities.Questions[])di.Value;   //当前类型的试题
                int type = (int)pi.TPI_Type;    //试题类型
                int count = questions.Length;  //试题数目
                float num = (float)pi.TPI_Number;   //占用多少分
                if (count < 1) continue;
                JObject jo = new JObject();
                jo.Add("type", type);
                jo.Add("count", count);
                jo.Add("number", num);
                JArray ques = new JArray();
                foreach (Song.Entities.Questions q in questions)
                {
                    string json = q.ToJson("", "Qus_CrtTime,Qus_LastTime");
                    ques.Add(JObject.Parse(json));
                }
                jo.Add("ques", ques);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 如果已经提交过答案，通过提交的答题返回试题
        /// </summary>
        /// <param name="exr"></param>
        /// <returns></returns>
        private JArray _putout(Song.Entities.ExamResults exr)
        {
            XmlDocument resXml = new XmlDocument();
            resXml.XmlResolver = null;
            resXml.LoadXml(exr.Exr_Results, false);
            JArray jarr = new JArray();
            XmlNodeList quesnodes = resXml.GetElementsByTagName("ques");
            for (int i = 0; i < quesnodes.Count; i++)
            {
                XmlNode node = quesnodes[i];
                string type = node.Attributes["type"].Value;
                string count = node.Attributes["count"].Value;
                string num = node.Attributes["number"].Value;
                JObject quesObj = new JObject();
                quesObj.Add("type", type);
                quesObj.Add("count", count);
                quesObj.Add("number", num);
              
                //Song.Entities.Questions[] ques = new Questions[node.ChildNodes.Count];
                JArray ques = new JArray();
                for (int n = 0; n < node.ChildNodes.Count; n++)
                {
                    long id = Convert.ToInt64(node.ChildNodes[n].Attributes["id"].Value);
                    Song.Entities.Questions q = Business.Do<IQuestions>().QuesSingle(id);
                    if (q == null) continue;
                    float qnum = 0;
                    if (node.ChildNodes[n].Attributes["num"] != null)
                    {
                        float.TryParse(node.ChildNodes[n].Attributes["num"].Value, out qnum);
                    }
                    q.Qus_Number = qnum;
                    q.Qus_Explain = "";
                    q.Qus_Answer = "";
                    //ques[n] = q;
                    string json = q.ToJson("", "Qus_CrtTime,Qus_LastTime");
                    ques.Add(JObject.Parse(json));
                }
                quesObj.Add("ques", ques);
                jarr.Add(quesObj);
            }
            return jarr;
        }
        /// <summary>
        /// 提交考试答题信息
        /// </summary>
        /// <param name="xml">答题信息,xml格式</param>
        /// <param name="async">是否异步计算成绩，true后台异步计算，false为立即计算（并返回成绩值）</param>
        /// <returns>examid:考试id; exrid:成绩记录id; score:成绩得分（如果实时计算的话）; resubmit:是否重复提交</returns>
        [HttpPost, HttpPut]
        [HtmlClear(Not = "xml")]
        public JObject SubmitResult(string xml, bool async)
        {
            JObject jo = new JObject();
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(xml, false);
            //遍历试题答题内容
            XmlNodeList quesnodes = resXml.GetElementsByTagName("ques");
            foreach (XmlNode ques in quesnodes)
            {
                int type = 0;
                int.TryParse(ques.Attributes["type"].Value, out type);
                //填空和简答,清理冗余html标签
                if (type == 4 || type == 5)
                {
                    foreach (XmlNode q in ques.ChildNodes)
                        q.InnerText = Html.ClearHTML(q.InnerText);

                }
            }
            XmlNode xn = resXml.SelectSingleNode("results");
            //试卷id，考试id
            long tpid;
            long.TryParse(xn.Attributes["tpid"].Value, out tpid);
            int examid;
            int.TryParse(xn.Attributes["examid"].Value, out examid);

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //考试开始时间
            long begin;
            long.TryParse(xn.Attributes["begin"].Value, out begin);
            DateTime beginTime = dtStart.Add(new TimeSpan(begin * 10000));
            //考试结束时间
            long over;
            long.TryParse(xn.Attributes["overtime"].Value, out over);
            DateTime overTime = dtStart.Add(new TimeSpan(over * 10000));
            //学员开始考试时间
            long start;
            long.TryParse(xn.Attributes["starttime"].Value, out start);
            DateTime startTime = dtStart.Add(new TimeSpan(start * 10000));
            //学生Id,学生名称
            int stid;
            int.TryParse(xn.Attributes["stid"].Value, out stid);
            string stname = xn.Attributes["stname"].Value.ToString();
            //学生性别，分组，身份证号
            int stsex;
            int.TryParse(xn.Attributes["stsex"].Value, out stsex);
            long stsid;
            long.TryParse(xn.Attributes["stsid"].Value, out stsid);
            string stcardid = xn.Attributes["stcardid"].Value.ToString();
            //学科Id,学科名称
            long sbjid;
            long.TryParse(xn.Attributes["sbjid"].Value, out sbjid);
            string sbjname = xn.Attributes["sbjname"].Value.ToString();
            //UID与考试主题
            string uid = xn.Attributes["uid"].Value.ToString();
            string theme = xn.Attributes["theme"].Value.ToString();
            //提交方式，1为自动提交，2为交卷
            int patter = Convert.ToInt32(xn.Attributes["patter"].Value);
            //
            Song.Entities.Examination exam = Business.Do<IExamination>().ExamSingle(examid);
            //如果考试不存在
            if (exam == null) return null;
            //如果考试已经结束
            int span = (int)exam.Exam_Span;
            //if (DateTime.Now > ((DateTime)exam.Exam_Date).AddMinutes(span + 5)) return 0;  

            try
            {
                Song.Entities.ExamResults exr = new ExamResults();
                exr.Exr_IsSubmit = patter == 2;
                exr.Exam_ID = examid;
                exr.Exam_Name = exam.Exam_Name;
                exr.Tp_Id = tpid;
                exr.Ac_ID = stid;
                exr.Ac_Name = stname;
                exr.Sts_ID = stsid;
                exr.Ac_Sex = stsex;
                exr.Ac_IDCardNumber = stcardid;
                exr.Sbj_ID = sbjid;
                exr.Sbj_Name = sbjname;
                exr.Exr_IP = WeiSha.Core.Browser.IP;
                exr.Exr_Mac = WeiSha.Core.Request.UniqueID();   //原本是网卡的mac地址,此处不再记录
                exr.Exr_Results = xml;
                exr.Exam_UID = uid;
                exr.Exam_Title = theme;
                exr.Exr_IsSubmit = patter == 2;
                if (exr.Exr_IsSubmit) exr.Exr_SubmitTime = DateTime.Now;
                exr.Exr_OverTime = overTime;
                exr.Exr_CrtTime = startTime;
                exr.Exr_LastTime = DateTime.Now;

                string cacheUid = string.Format("ExamResults：{0}-{1}-{2}", examid, tpid, stid);    //缓存的uid
                Business.Do<IExamination>().ResultCacheUpdate(exr, -1, cacheUid);
                if (patter == 1) return jo;
                exr = Business.Do<IExamination>().ResultSubmit(exr);
                //是否重复提交
                jo.Add("resubmit", exr.Exr_IsCalc);
                //如果是手动提交，且没有计算成绩的，此处计算成绩
                float score = -1;
                if (exr.Exr_IsSubmit && !exr.Exr_IsCalc)
                {
                    //异步计算成绩
                    if (async)
                    {
                        //后台异步计算
                        Exam_Calc handler = new Exam_Calc(exr);
                        System.Threading.Tasks.Task task = new System.Threading.Tasks.Task(handler.Calc);
                        task.Start();
                    }
                    else
                    {
                        //实时计算成绩
                        Business.Do<IExamination>().ClacScore(exr);
                    }
                }
                if(exr.Exr_IsCalc) score = exr.Exr_ScoreFinal;
                jo.Add("examid", exr.Exam_ID);
                jo.Add("exrid", exr.Exr_ID);
                jo.Add("score", score);
                jo.Add("async", async);
            }
            catch
            {
                return null;
            }
            return jo;
        }
        #endregion

        #region 文件上传管理
        /// <summary>
        /// 考试中上传附件
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="examid">考试id</param>
        /// <param name="qid">试题id</param>
        /// <returns>qid:试题id;state:是否成功;name:文件名; url:文件完整路径</returns>
        [HttpPost]
        [Student]
        [Upload(Extension = "", MaxSize = 5120, CannotEmpty = true)]
        public JObject FileUp(int stid, int examid, long qid)
        {
            JObject jo = new JObject();
            jo.Add("qid", qid.ToString());

            string filepath = PhyPath + examid + "\\" + stid + "\\";
            //只保存第一张图片
            foreach (string key in this.Files)
            {
                HttpPostedFileBase file = this.Files[key];
                if (!System.IO.Directory.Exists(filepath))
                    System.IO.Directory.CreateDirectory(filepath);
                //删除原来的文件
                foreach (FileInfo f in new DirectoryInfo(filepath).GetFiles())
                {
                    if (f.Name.IndexOf("_") > 0)
                    {
                        string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                        if (idtm == qid.ToString()) f.Delete();
                    }
                }
                string filename = file.FileName;
                //处理文件名长度，由于文件名前面还要加上试题id，防止文件名过长
                int maxLength = 220;
                if (filename.Length > maxLength)
                {
                    if (filename.IndexOf(".") > -1)
                    {
                        string ext = filename.Substring(filename.LastIndexOf("."));
                        string name = filename.Substring(0, filename.LastIndexOf("."));
                        if(name.Length> maxLength - ext.Length)
                        {
                            name = name.Substring(0, maxLength - ext.Length);
                            filename = name + ext;
                        }
                    }
                    else
                    {
                        filename = filename.Substring(0, maxLength);
                    }
                }
                jo.Add("name", filename);
                file.SaveAs(filepath + qid + "_" + filename);
                jo.Add("url", string.Format("{0}{1}/{2}/{3}", VirPath, examid, stid, qid + "_" + filename));
                jo.Add("state", true);
                break;
            }
            return jo;
        }
        /// <summary>
        /// 考试中的简答题附件
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="examid">考试id</param>
        /// <param name="qid">试题id</param>
        /// <returns>qid:试题id;state:是否成功;name:文件名; url:文件完整路径</returns>
        public JObject FileLoad(int stid, int examid, long qid)
        {
            JObject jo = new JObject();
            jo.Add("qid", qid.ToString());
            //文件所在路径
            string filepath = PhyPath + examid + "\\" + stid + "\\";
            if (!System.IO.Directory.Exists(filepath))
            {
                jo.Add("state", false);
                return jo;
            }
            string file = string.Empty;
            foreach (FileInfo f in new DirectoryInfo(filepath).GetFiles())
            {
                if (f.Name.IndexOf("_") > 0)
                {
                    string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                    if (idtm == qid.ToString())
                    {
                        file = f.Name.Substring(f.Name.IndexOf("_") + 1);
                        jo.Add("name", file);
                        jo.Add("url", string.Format("{0}{1}/{2}/{3}", VirPath, examid, stid, f.Name));
                        jo.Add("state", true);
                    }
                }
            }
            if (string.IsNullOrEmpty(file))           
                jo.Add("state", false);          
            return jo;
        }
        /// <summary>
        /// 删除考试中的附件
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="examid">考试id</param>
        /// <param name="qid">试题id</param>
        /// <returns>state:是否成功;qid:试题id</returns>
        [Student]
        [HttpDelete]
        public JObject FileDelete(int stid, int examid, long qid)
        {
            JObject jo = new JObject();
            jo.Add("qid", qid.ToString());
            //文件所在路径
            string filepath = PhyPath + examid + "\\" + stid + "\\";
            if (!System.IO.Directory.Exists(filepath))
            {
                jo.Add("state", false);
                return jo;
            }
            //文件名
            string file = "";
            foreach (FileInfo f in new DirectoryInfo(filepath).GetFiles())
            {
                if (f.Name.IndexOf("_") > 0)
                {
                    string idtm = f.Name.Substring(0, f.Name.IndexOf("_"));
                    if (idtm == qid.ToString()) file = f.FullName;
                }
            }
            if (!string.IsNullOrWhiteSpace(file) && System.IO.File.Exists(file))                   
                    System.IO.File.Delete(file);
            jo.Add("state", true);
            return jo;
        }
        #endregion

        #region 增删改查
        /// <summary>
        /// 根据ID查询考试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>    
        public Song.Entities.Examination ForID(int id)
        {
            return Business.Do<IExamination>().ExamSingle(id);
        }        
        /// <summary>
        /// 获取考试主题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="start">时间范围查询的开始时间</param>
        /// <param name="end"></param>
        /// <param name="search">按考试主题检索</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ThemePager(int orgid, DateTime? start, DateTime? end, string search, int size, int index)
        {
            int count = 0;
            Song.Entities.Examination[] datas = Business.Do<IExamination>().GetPager(orgid, start, end, true, search, size, index, out count);
            ListResult result = new ListResult(datas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 获取考试主题
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="search"></param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpPost]
        [Admin,Teacher]
        public ListResult ThemeAdminPager(int orgid, DateTime? start, DateTime? end, bool? use, string search, int size, int index)
        {
            int count = 0;
            Song.Entities.Examination[] datas = Business.Do<IExamination>().GetPager(orgid, start, end, use, search, size, index, out count);
            ListResult result = new ListResult(datas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 某个考试主题下的所有考试场次
        /// </summary>
        /// <param name="uid">考试主题的uid</param>
        /// <returns></returns>
        public Song.Entities.Examination[] Exams(string uid)
        {
            Song.Entities.Examination[] exams = Business.Do<IExamination>().ExamItem(uid);
            for (int i = 0; i < exams.Length; i++)
            {
                DateTime examDate = exams[i].Exam_Date < DateTime.Now.AddYears(-100) ? DateTime.Now : (DateTime)exams[i].Exam_Date;
                exams[i].Exam_Date = examDate.AddYears(100) < DateTime.Now ? DateTime.Now : examDate;
            }
            return exams;
        }
        /// <summary>
        /// 获取参考人员信息
        /// </summary>
        /// <param name="type">类型，1为全体学员，2为分组</param>
        /// <param name="uid">考试主题的uid</param>
        /// <returns>返回的是字符串</returns>
        public string GroupType(string type, string uid)
        {
            if (type == "1") return "全体学员";
            if (type == "2")
            {
                Song.Entities.StudentSort[] sts = Business.Do<IExamination>().GroupForStudentSort(uid);
                string strDep = "";
                for (int i = 0; i < sts.Length; i++)
                {
                    strDep += sts[i].Sts_Name;
                    if (i < sts.Length - 1) strDep += ",";
                }
                if(string.IsNullOrWhiteSpace(strDep))
                    strDep = "(没有学员组)";
                return strDep;
            }
            return "";
        }
        /// <summary>
        /// 获取参考学员组的信息
        /// </summary>
        /// <param name="uid">考试主题的uid</param>
        /// <returns>学员组</returns>
        public Song.Entities.StudentSort[] Groups(string uid)
        {
            return Business.Do<IExamination>().GroupForStudentSort(uid);
        }
        /// <summary>
        /// 某场考试的参考人数
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <returns>id:考试id,number:参考人数</returns>
        [HttpGet]
        public JObject AttendNumber(int examid)
        {            
            int num = Business.Do<IExamination>().Number4Exam(examid);
            JObject jo = new JObject();
            jo.Add("id", examid);
            jo.Add("number", num);
            return jo;
        }
        /// <summary>
        /// 考试主题下的参考人数
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <returns>id:考试id,number:参考人数</returns>
        [HttpGet]
        public JObject AttendTheme(int examid)
        {
            Song.Entities.Examination theme = Business.Do<IExamination>().ExamSingle(examid);
            if (!theme.Exam_IsTheme)
            {
                return this.AttendNumber(examid);
            }
            int total = 0;
            Song.Entities.Examination[] exams = Business.Do<IExamination>().ExamItem(theme.Exam_UID);
            for (int i = 0; i < exams.Length; i++)
            {
                total += Business.Do<IExamination>().Number4Exam(exams[i].Exam_ID);
            }
            JObject jo = new JObject();
            jo.Add("id", examid);
            jo.Add("number", total);
            return jo;
        }
        /// <summary>
        /// 某场考试的平均分数
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <returns>id:考试id,average:平均数</returns>
        [HttpGet]
        public JObject Average4Exam(int examid)
        {
            double avg = Business.Do<IExamination>().Avg4Exam(examid);
            avg = Math.Round(avg * 100) / 100;
            JObject jo = new JObject();
            jo.Add("id", examid);
            jo.Add("average", avg);
            return jo;
        }
        /// <summary>
        /// 是否需要人工批阅
        /// </summary>
        /// <param name="examid"></param>
        /// <returns>id:考试id,manual:是否需要人工批阅，true为需要</returns>
        public JObject Manual4Exam(int examid)
        {
            bool manual = false;
            //考生数，如果没有人考试，则不需要批阅
            int students = Business.Do<IExamination>().Number4Exam(examid);
            if (students > 0)
            {
                Song.Entities.Examination exas = Business.Do<IExamination>().ExamSingle(examid);
                Song.Entities.TestPaper pager = Business.Do<ITestPaper>().PaperSingle(exas.Tp_Id);
                if (pager != null)
                {
                    List<Song.Entities.TestPaperItem> items = Business.Do<ITestPaper>().GetItemForAny(pager);
                    foreach (Song.Entities.TestPaperItem ti in items)
                    {
                        if (ti.TPI_Type == 4) {
                            manual = true;
                            break;
                        }
                    }                    
                }
            }
            JObject jo = new JObject();
            jo.Add("id", examid);
            jo.Add("manual", manual);
            return jo;
        }

        #endregion

        #region 考试成绩
        /// <summary>
        /// 当前考试下所有学员的的学员组
        /// </summary>
        /// <param name="examid">考试主题的id</param>
        /// <returns></returns>
        public Song.Entities.StudentSort[]  Sort4Theme(int examid)
        {
            return Business.Do<IExamination>().StudentSort4Theme(examid);
        }
        /// <summary>
        /// 考试中某场次的平均成绩
        /// </summary>
        /// <param name="id">考试场次的id</param>
        /// <returns></returns>
        public double ExamAvg(int id)
        {
            double res = Business.Do<IExamination>().Avg4Exam(id);
            return Math.Round(Math.Round(res * 10000) / 10000, 2, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// 当前考试主题下的所有成绩，包括各个场次
        /// </summary>
        /// <param name="examid">考试主题的id</param>
        /// <param name="sort">分组id</param>
        /// <returns></returns>
        public DataTable Results(int examid,int sort)
        {
            DataTable dt = null;    //数据源  
            Song.Entities.Examination theme = Business.Do<IExamination>().ExamSingle(examid);
            if (theme == null) return dt;
            string stsid = "";
            switch (sort)
            {
                //所有学员
                case 0:
                    //当前考试限定的学生分组
                    Song.Entities.StudentSort[] sts = Business.Do<IExamination>().GroupForStudentSort(theme.Exam_UID);
                    //如果没有设定分组，则取当前参加考试的学员的分组
                    if (sts == null || sts.Length < 1) sts = Business.Do<IExamination>().StudentSort4Theme(examid);
                    foreach (Song.Entities.StudentSort ss in sts)
                        stsid += ss.Sts_ID + ",";
                    stsid += "-1";
                    dt = Business.Do<IExamination>().Result4Theme(examid, stsid);
                    break;
                //未分组学员
                case -1:
                    dt = Business.Do<IExamination>().Result4Theme(examid, -1);
                    break;
                //当前分组学员
                default:
                    dt = Business.Do<IExamination>().Result4Theme(examid, sort);
                    break;
            }
            return dt;
        }
        /// <summary>
        /// 通过考试成绩的id，获取成绩信息
        /// </summary>
        /// <param name="id">考试成绩记录的id</param>
        /// <returns></returns>
        public ExamResults ResultForID(int id)
        {
            return Business.Do<IExamination>().ResultSingle(id);
        }
        /// <summary>
        /// 保存考试成绩，用于教师批阅后的保存
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [Teacher,Admin]
        [HttpPost]
        [HtmlClear(Not = "result")]
        public bool ResultModify(ExamResults result)
        {
            ExamResults exr=Business.Do<IExamination>().ResultSingle(result.Exr_ID);
            if (exr == null) return false;
            exr.Copy<Song.Entities.ExamResults>(result);
            Business.Do<IExamination>().ResultSave(exr);
            return true;
        }
        /// <summary>
        /// 获取成绩记录
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="stid">学员id</param>
        /// <returns></returns>
        public ExamResults Result(int examid,long  tpid,int stid)
        {
            Song.Entities.ExamResults exr = Business.Do<IExamination>().ResultForCache(examid, tpid, stid);
            //if(exr==null) exr= Business.Do<IExamination>().ResultSingle(examid, tpid, stid);
            return exr;
        }
        /// <summary>
        /// 考试成绩回顾
        /// </summary>
        /// <param name="id">考试成绩记录的id</param>
        /// <returns></returns>
        public ExamResults ResultReview(int id)
        {
            //成绩记录
            ExamResults result = Business.Do<IExamination>().ResultSingle(id);
            if (result == null) throw new Exception("未找到成绩记录");
            //加载答题记录
            XmlDocument resXml = new XmlDocument();
            //考试信息
            Song.Entities.Examination exam = Business.Do<IExamination>().ExamSingle(result.Exam_ID);
            if (exam == null) return result;          
            //试卷
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PaperSingle(result.Tp_Id);
            if (tp == null) throw new Exception("考试所用试卷不存在");
            
            resXml.LoadXml(result.Exr_Results, false);
            //遍历试题答题内容
            XmlNodeList quesnodes = resXml.GetElementsByTagName("ques");
            foreach (XmlNode ques in quesnodes)
            {
                int type = 0;
                int.TryParse(ques.Attributes["type"].Value, out type);
                //填空和简答,清理冗余html标签
                if (type == 4 || type == 5)
                {
                    foreach(XmlNode q in ques.ChildNodes)                  
                        q.InnerText = Html.ClearHTML(q.InnerText);
                   
                }               
            }
            result.Exr_Results = resXml.InnerXml;
            //判断开始时间与结束时间，是否考试结束等
            bool isOver;
            //判断是否已经开始、是否已经结束
            if (exam.Exam_DateType == 1)
            {
                //固定时间开始               
                isOver = DateTime.Now > exam.Exam_Date.AddMinutes(exam.Exam_Span);   //是否结束
            }
            else
            {
                return result;
                isOver = DateTime.Now > exam.Exam_DateOver;   //是否结束                         
                if (result != null && !string.IsNullOrWhiteSpace(result.Exr_Results))
                {
                    XmlNode xn = resXml.LastChild;
                    //考试的开始与结束时间，防止学员刷新考试界面，导致时间重置
                    long lover;
                    long.TryParse(xn.Attributes["overtime"] != null ? xn.Attributes["overtime"].Value : "0", out lover);
                    lover = lover * 10000;
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    DateTime overTime = dtStart.Add(new TimeSpan(lover));    //得到转换后的结束时间
                    isOver = DateTime.Now > overTime;                    
                }
            }
            if(!isOver) throw new Exception("考试时间尚未结束，为防止泄题，请稍后查看回顾");
           
            return result;
        }

        /// <summary>
        /// 某场考试的成绩
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <param name="name">学员姓名<</param>
        /// <param name="idcard">学员身份证号</param>
        /// <param name="min">按分数区间获取记录，此处是最低分</param>
        /// <param name="max">最高分</param>
        /// <param name="manual">是否批阅</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Result4Exam(int examid, string name, string idcard, float min, float max, bool? manual, int size, int index)
        {
            int count = 0;
            Song.Entities.ExamResults[] datas = Business.Do<IExamination>().Results(examid, name, idcard, min, max, manual, size, index, out count);
            ListResult result = new ListResult(datas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 某个学员的考试成绩
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="search">考试场次的标题</param>      
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Result4Student(int acid, int orgid, long sbjid, string search, int size, int index)
        {
            int count = 0;
            Song.Entities.ExamResults[] datas = null;
            datas = Business.Do<IExamination>().GetAttendPager(acid, sbjid, orgid, search, size, index, out count);
            ListResult result = new ListResult(datas);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 删除考试成绩，按学员id与考试id
        /// </summary>
        /// <param name="acid">学员id,可以为多个</param>
        /// <param name="examid">考试id</param>
        /// <returns></returns>
        [HttpDelete]
        public int ResultDelete4Acc(string acid,int examid)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(acid)) return i;
            string[] arr = acid.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IExamination>().ResultDelete(idval, examid);
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
        /// 删除考试成绩，按成绩记录的id
        /// </summary>
        /// <param name="exrid">成绩记录的id,可以为多个</param>
        /// <returns></returns>
        [HttpDelete]
        public int ResultDelete(string exrid)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(exrid)) return i;
            string[] arr = exrid.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<IExamination>().ResultDelete(idval);
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
        /// 删除考试下的所有成绩
        /// </summary>
        /// <param name="examid">考试id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ResultClear(int examid)
        {
            Business.Do<IExamination>().ResultClear(examid);
            return true;
        }
        /// <summary>
        /// 计算考试成绩，通过考试成绩的记录计算，一般用于重新计算
        /// </summary>
        /// <param name="exrid">考试记录的id</param>
        /// <returns></returns>
        public double ClacScore(int exrid)
        {
            ExamResults exr=Business.Do<IExamination>().ResultSingle(exrid);
            if (exr == null) return 0;
            exr=Business.Do<IExamination>().ClacScore(exr);
            if (exr == null) return 0;
            return exr.Exr_ScoreFinal;
        }
        #endregion

        #region
        /// <summary>
        /// 学员今天以及之后的考试，过期的不再显示
        /// </summary>
        /// <param name="acid">学员ID</param>
        /// <param name="search">考试检索</param>
        /// <param name="size">每次几条</param>
        /// <param name="index">第几页</param>
        /// <returns>考试场次，而非考试主题</returns>
        public ListResult SelfExam4Todaylate(int acid, string search, int size, int index)
        {
            int count = 0;
            DateTime start = DateTime.Now.Date;
            List<Song.Entities.Examination> todaylate = Business.Do<IExamination>().GetSelfExam(acid, start, null, search, size, index, out count);
            ListResult result = new ListResult(todaylate);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }        
        #endregion

        #region 导出考试成绩
        private static string outputPath = "ExamresultToExcel";
        /// <summary>
        /// 生成excel
        /// </summary>
        /// <param name="examid">考试的id</param> 
        /// <returns></returns>
        [HttpPost]
        public JObject ExcelOutput(int examid)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + examid + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("考试成绩.{0}.({1}).xls", examid, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IExamination>().Export4Excel(filePath, examid);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", string.Format("{0}/{1}/{2}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, examid, filename));
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 导出参加考试的学员成绩
        /// </summary>
        /// <param name="examid">考试主题的id</param>
        /// <param name="sorts">学员组的id</param>
        /// <returns></returns>
        public JObject OutputParticipate(int examid)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + examid + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("考试成绩.{0}.({1}).xls", examid, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IExamination>().OutputParticipate(filePath, examid, null);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", string.Format("{0}/{1}/{2}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, examid, filename));
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 导出所有，包括未参加考试的学员
        /// </summary>
        /// <param name="examid"></param>
        /// <returns></returns>
        public JObject OutputAll(int examid)
        {
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + examid + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            DateTime date = DateTime.Now;
            string filename = string.Format("考试成绩-全部.{0}.({1}).xls", examid, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<IExamination>().OutputAll(filePath, examid);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", string.Format("{0}/{1}/{2}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, examid, filename));
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="examid"></param>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ExcelDelete(int examid, string filename)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + examid + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            string filePath = rootpath + filename;
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <param name="examid"></param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(int examid)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + examid + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            JArray jarr = new JArray();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            foreach (System.IO.FileInfo f in dir.GetFiles("*.xls"))
            {
                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", string.Format("{0}/{1}/{2}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, examid, f.Name));
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion
    }

    /// <summary>
    /// 考试成绩计算，用于异步方法
    /// </summary>
    public class Exam_Calc
    {
        public Song.Entities.ExamResults ExamResult { get; set; }
       
        public Exam_Calc(Song.Entities.ExamResults result)
        {
            this.ExamResult = result;
        }
        public void Calc()
        {
            Business.Do<IExamination>().ClacScore(this.ExamResult);
        }
    }
}
