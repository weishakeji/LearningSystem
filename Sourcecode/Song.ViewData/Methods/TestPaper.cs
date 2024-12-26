using Newtonsoft.Json.Linq;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;
using WeiSha.Core;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 试卷管理
    /// </summary>
    [HttpPut, HttpGet]
    public class TestPaper : ViewMethod, IViewAPI
    {
        //资源的虚拟路径和物理路径
        public static string PathKey = "TestPaper";

        public static string VirPath = WeiSha.Core.Upload.Get[PathKey].Virtual;
        public static string PhyPath = WeiSha.Core.Upload.Get[PathKey].Physics;

        #region 增删改查

        /// <summary>
        /// 获取试卷信息
        /// </summary>
        /// <param name="id">试卷id</param>
        /// <returns></returns>
        [HttpPost]
        public Song.Entities.TestPaper ForID(long id)
        {
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PaperSingle(id);
            if (tp == null) throw new Exception("试卷不存在！");
            return _tran(tp);
        }

        ///<summary>
        /// 创建试卷
        /// </summary>
        /// <param name="entity">试卷对象</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "entity")]
        public Song.Entities.TestPaper Add(Song.Entities.TestPaper entity)
        {
            try
            {
                string filename = string.Empty, smallfile = string.Empty;
                try
                {
                    //只保存第一张图片
                    foreach (string key in this.Files)
                    {
                        HttpPostedFileBase file = this.Files[key];
                        filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                        file.SaveAs(PhyPath + filename);
                        break;
                    }
                    entity.Tp_Logo = filename;

                    Business.Do<ITestPaper>().PaperAdd(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改试卷信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost, HttpGet(Ignore = true)]
        [Upload(Extension = "jpg,png,gif", MaxSize = 1024, CannotEmpty = false)]
        [HtmlClear(Not = "entity")]
        public Song.Entities.TestPaper Modify(Song.Entities.TestPaper entity)
        {
            try
            {
                string filename = string.Empty, smallfile = string.Empty;
                try
                {
                    Song.Entities.TestPaper old = Business.Do<ITestPaper>().PaperSingle(entity.Tp_Id);
                    if (old == null) throw new Exception("Not found entity for TestPaper！");
                    //如果有上传文件
                    if (this.Files.Count > 0)
                    {
                        //只保存第一张图片
                        foreach (string key in this.Files)
                        {
                            HttpPostedFileBase file = this.Files[key];
                            filename = WeiSha.Core.Request.UniqueID() + Path.GetExtension(file.FileName);
                            file.SaveAs(PhyPath + filename);
                            break;
                        }
                        entity.Tp_Logo = filename;
                        if (!string.IsNullOrWhiteSpace(old.Tp_Logo))
                            WeiSha.Core.Upload.Get[PathKey].DeleteFile(old.Tp_Logo);
                    }
                    //如果没有上传图片，且新对象没有图片，则删除旧图
                    else if (string.IsNullOrWhiteSpace(entity.Tp_Logo) && !string.IsNullOrWhiteSpace(old.Tp_Logo))
                    {
                        WeiSha.Core.Upload.Get[PathKey].DeleteFile(old.Tp_Logo);
                    }

                    old.Copy<Song.Entities.TestPaper>(entity);
                    Business.Do<ITestPaper>().PaperSave(old);
                    return old;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除试卷
        /// </summary>
        /// <param name="id">试卷id，可以是多个，用逗号分隔</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpDelete]
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
                    Business.Do<ITestPaper>().PaperDelete(idval);
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
        /// 修改试卷的状态
        /// </summary>
        /// <param name="id">试卷的id，可以是多个，用逗号分隔</param>
        /// <param name="use">是否启用</param>
        /// <param name="rec">是否推荐</param>
        /// <returns></returns>
        [HttpPost]
        [Admin, Teacher]
        public int ModifyState(string id, bool use, bool? rec)
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
                    if (rec != null)
                    {
                        Business.Do<ITestPaper>().PaperUpdate(idval,
                        new WeiSha.Data.Field[] {
                        Song.Entities.TestPaper._.Tp_IsUse,
                        Song.Entities.TestPaper._.Tp_IsRec },
                        new object[] { use, rec });
                    }
                    else
                    {
                        Business.Do<ITestPaper>().PaperUpdate(idval, new WeiSha.Data.Field[] { Song.Entities.TestPaper._.Tp_IsUse },
                            new object[] { use });
                    }
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
        /// 前端的分页获取试卷
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="diff">难度等级</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult ShowPager(long couid, string search, int diff, int size, int index)
        {
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            int orgid = org.Org_ID;
            int count = 0;
            Song.Entities.TestPaper[] tps = Business.Do<ITestPaper>().PaperPager(orgid, -1, couid, diff, true, search, size, index, out count);
            for (int i = 0; i < tps.Length; i++)
                tps[i] = _tran(tps[i]);
            ListResult result = new ListResult(tps);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }

        /// <summary>
        /// 获取某个课程的结课考试
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="use">是否查询禁用的结课考试,null取所有</param>
        /// <returns></returns>
        public Song.Entities.TestPaper FinalPaper(long couid, bool? use)
        {
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().FinalPaper(couid, use);
            return _tran(tp);
        }

        /// <summary>
        /// 分页获取所有试卷
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid">课程id</param>
        /// <param name="search">按名称检索</param>
        /// <param name="isuse"></param>
        /// <param name="diff">难度等级</param>
        /// <param name="size">每页几条</param>
        /// <param name="index">第几页</param>
        /// <returns></returns>
        public ListResult Pager(int orgid, long sbjid, long couid, string search, bool? isuse, int diff, int size, int index)
        {
            int count = 0;
            Song.Entities.TestPaper[] tps = Business.Do<ITestPaper>()
                .PaperPager(orgid, sbjid, couid, diff, isuse, search, size, index, out count);
            for (int i = 0; i < tps.Length; i++)
                tps[i] = _tran(tps[i]);
            ListResult result = new ListResult(tps);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }

        /// <summary>
        /// 处理试卷对象的一些信息
        /// </summary>
        /// <param name="paper"></param>
        /// <returns></returns>
        private Song.Entities.TestPaper _tran(Song.Entities.TestPaper paper)
        {
            if (paper == null) return paper;
            paper.Tp_Logo = System.IO.File.Exists(PhyPath + paper.Tp_Logo) ? VirPath + paper.Tp_Logo : "";
            //试卷所属的专业名称
            if (paper.Sbj_ID > 0 && string.IsNullOrWhiteSpace(paper.Sbj_Name))
            {
                Song.Entities.Subject sbj = Business.Do<ISubject>().SubjectSingle(paper.Sbj_ID);
                if (sbj != null) paper.Sbj_Name = sbj.Sbj_Name;
            }
            //试卷所属的课程名称
            if (paper.Cou_ID > 0 && string.IsNullOrWhiteSpace(paper.Cou_Name))
            {
                Song.Entities.Course cour = Business.Do<ICourse>().CourseSingle(paper.Cou_ID);
                if (cour != null) paper.Cou_Name = cour.Cou_Name;
            }
            return paper;
        }

        #endregion 增删改查

        #region 出卷相关

        /// <summary>
        /// 获取试题的大项
        /// </summary>
        /// <returns></returns>
        public List<TestPaperItem> Types(long tpid)
        {
            Song.Entities.TestPaper tp = Business.Do<ITestPaper>().PaperSingle(tpid);
            return Business.Do<ITestPaper>().GetItemForAny(tp);
        }

        /// <summary>
        /// 生成随机试卷，试题随机抽取
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public JArray GenerateRandom(long tpid)
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

        #endregion 出卷相关

        #region 成绩

        /// <summary>
        /// 递交答题信息与成绩
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [HttpPut, HttpPost]
        [HtmlClear(Not = "result")]
        [Student]
        public JObject InResult(string result)
        {
            JObject jo = new JObject();
            //如果为空，则返回-1，表示错误
            if (result == "")
            {
                jo.Add("score", 0);
                jo.Add("trid", 0);
                return jo;
            }
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(result, false);
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
            XmlNode xn = getAttrBase64(resXml.SelectSingleNode("results"));
            //学员Id,学员名称
            int stid = 0, stsid = 0, stsex = 0;
            int.TryParse(getAttr(xn, "stid"), out stid);
            int.TryParse(getAttr(xn, "stsid"), out stsid);
            int.TryParse(getAttr(xn, "stsex"), out stsex);
            string stname = getAttr(xn, "stname");
            string stsname = getAttr(xn, "stsname");
            string cardid = getAttr(xn, "stcardid"); //学员身份证
            //***验证是否是当前学员
            Song.Entities.Accounts acc = this.User;
            if (acc.Ac_ID != stid) throw new Exception("当前登录学员信息与成绩提交的信息不匹配");

            //课程id,课程名称
            long couid = 0;
            long.TryParse(getAttr(xn, "couid"), out couid);
            string couname = getAttr(xn, "couname");

            //***课程是否购买或过期，如果课程免费，则自动创建课程与学员的关联记录
            Student_Course purchase = Business.Do<ICourse>().StudentCourse(stid, couid, true);
            if (purchase == null) purchase = Business.Do<IStudent>().SortCourseToStudent(stid, couid);
            if (purchase == null || (!purchase.Stc_IsFree && purchase.Stc_EndTime < DateTime.Now && purchase.Stc_StartTime > DateTime.Now))
                throw new Exception("未购买课程或已经过期");

            //机构信息
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();

            //试卷id，试卷名称
            long tpid = 0;
            long.TryParse(getAttr(xn, "tpid"), out tpid);
            string tpname = getAttr(xn, "tpname");
            //***如果结课考试，则验证结课条件是否满足
            Song.Entities.TestPaper paper = Business.Do<ITestPaper>().PaperSingle(tpid);
            if (paper == null) throw new Exception("试卷不存在");
            if (paper.Tp_IsFinal)
            {
                string txt = string.Format("学员“{0}”未满足结课考试条件：", acc.Ac_Name);
                WeiSha.Core.CustomConfig config = CustomConfig.Load(org.Org_Config);
                //视频学习进度是否达成
                double condition_video = config["finaltest_condition_video"].Value.Double ?? 0;
                //视频数
                int video = Business.Do<IOutline>().OutlineOfCount(couid, -1, true, true, true, null);
                if (video > 0 && condition_video > purchase.Stc_StudyScore)             
                    throw new Exception(string.Format(txt + "视频学习应达到{0}%，实际学习进度{1}%", condition_video, purchase.Stc_StudyScore));              
                //试题练习通过率是否达成
                double condition_ques = config["finaltest_condition_ques"].Value.Double ?? 0;
                if (condition_ques>0 && condition_ques > purchase.Stc_QuesScore)            
                    throw new Exception(string.Format(txt + "试题通过率应达到{0}%，实际通过率为{1}%", condition_ques, purchase.Stc_QuesScore));              
                //最多可以考几次
                int finaltest_count = config["finaltest_count"].Value.Int32 ?? 1;
                Song.Entities.TestResults[] trs = Business.Do<ITestPaper>().ResultsCount(stid, tpid);
                if (finaltest_count <= trs.Length)               
                    throw new Exception(string.Format("最多允许考试{0}次， 已经考了{1}次，", finaltest_count, trs.Length));               
            }

            //专业id,专业名称
            long sbjid = 0;
            long.TryParse(getAttr(xn, "sbjid"), out sbjid);
            string sbjname = getAttr(xn, "sbjname");

            //UID与考试主题
            string uid = getAttr(xn, "uid");
            //string theme = xn.Attributes["theme"].Value.ToString();
            //提交方式，1为自动提交，2为交卷
            int patter = Convert.ToInt32(xn.Attributes["patter"].Value);
            float score = (float)Convert.ToDouble(getAttr(xn, "score"));    //考试成绩
            bool isClac = getAttr(xn, "isclac") == "true" ? true : false;   //是否在客户端计算过成绩
            //
            Song.Entities.TestResults exr = new TestResults();
            exr.Tp_Id = tpid;
            exr.Tp_Name = tpname;
            exr.Cou_ID = couid;
            exr.Sbj_ID = sbjid;
            exr.Sbj_Name = sbjname;
            exr.Ac_ID = stid;
            exr.Ac_Name = stname;
            exr.St_IDCardNumber = cardid;
            exr.Sts_ID = stid;
            exr.Sts_Name = stsname;
            exr.St_Sex = stsex;     //性别
            exr.Tr_IP = WeiSha.Core.Browser.IP;
            exr.Tr_Mac = "";
            exr.Tr_UID = uid;
            exr.Tr_Results = resXml.OuterXml;
            exr.Tr_Score = score;

            exr.Org_ID = org.Org_ID;
            exr.Org_Name = org.Org_Name;
            //得分
            score = Business.Do<ITestPaper>().ResultsAdd(exr, false);
            //如果为结课考试，则更新成绩
            if (paper.Tp_IsFinal)
            {               
                Thread t1 = new Thread(() =>
                {
                    try
                    {
                        float highest = Business.Do<ITestPaper>().ResultsHighest(paper.Tp_Id, stid);
                        purchase.Stc_ExamScore = highest;
                        Business.Do<ICourse>().StudentScoreSave(purchase, -1, -1, highest);
                    }
                    catch (Exception ex)
                    {
                        WeiSha.Core.Log.Error(this.GetType().FullName, ex);
                    }
                });
                t1.Start();
            }
            //返回得分与成绩id
            jo.Add("score", score);
            jo.Add("trid", exr.Tr_ID);
            jo.Add("tpid", exr.Tp_Id);
            return jo;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="xn"></param>
        /// <param name="attr"></param>
        /// <returns></returns>
        private string getAttr(XmlNode xn, string attr)
        {
            if (xn.Attributes[attr] == null) return string.Empty;
            return xn.Attributes[attr].Value.Trim();
        }

        /// <summary>
        /// 将属性进行Base64解码
        /// </summary>
        /// <param name="xn"></param>
        /// <returns></returns>
        private XmlNode getAttrBase64(XmlNode xn)
        {
            foreach (XmlAttribute attr in xn.Attributes)
            {
                string val = WeiSha.Core.DataConvert.DecryptForBase64(attr.Value);
                val = val.Replace("<", "＜");
                val = val.Replace(">", "＞");
                val = val.Replace("(", "（");
                val = val.Replace(")", "）");
                val = val.Replace("&", "＆");
                val = val.Replace("=", "〓");
                val = val.Replace("\"", "＂");
                val = val.Replace("'", "｀");
                val = val.Replace("\\", "＼");
                attr.Value = val;
            }
            return xn;
        }

        /// <summary>
        /// 测试成绩
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ResultsPager(int stid, long tpid, int size, int index)
        {
            int count = 0;
            Song.Entities.TestResults[] trs = Business.Do<ITestPaper>().ResultsPager(stid, tpid, size, index, out count);
            ListResult result = new ListResult(trs);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }

        /// <summary>
        /// 测试成绩
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <param name="tpname">试卷名称</param>
        /// <param name="couid">课程id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="orgid">机构id</param>
        /// <param name="stname">学员名称</param>
        /// <param name="cardid">学员身份证</param>
        /// <param name="score_min">按成绩查询，成绩区间最小值</param>
        /// <param name="score_max">按成绩查询，成绩区间最大值</param>
        /// <param name="time_min">按交卷时间查询，成绩区间最小值</param>
        /// <param name="time_max">按交卷时间查询，成绩区间最小值</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ResultsQueryPager(int stid, long tpid, string tpname, long couid, long sbjid, int orgid,
            string stname, string cardid, float score_min, float score_max, DateTime? time_min, DateTime? time_max,
            int size, int index)
        {
            int count = 0;
            Song.Entities.TestResults[] trs = Business.Do<ITestPaper>().ResultsPager(stid, tpid, tpname, couid, sbjid, orgid,
                stname, cardid, score_min, score_max, time_min, time_max,
                size, index, out count);
            ListResult result = new ListResult(trs);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }

        /// <summary>
        /// 计算成绩
        /// </summary>
        /// <param name="trid">成绩记录的id</param>
        /// <returns></returns>
        public float ResultsCalc(int trid)
        {
            return Business.Do<ITestPaper>().ResultsCalc(trid);
        }
        /// <summary>
        /// 批量计算试卷的所有成绩
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public bool ResultsBatchCalc(long tpid)
        {
            return Business.Do<ITestPaper>().ResultsBatchCalc(tpid);
        }
        /// <summary>
        /// 所有测试成绩
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public Song.Entities.TestResults[] ResultsAll(int stid, long tpid)
        {
            if (stid <= 0) throw new Exception("学员id为空，无法获取成绩");
            if (tpid <= 0) throw new Exception("试卷id为空，无法获取成绩");
            Song.Entities.TestResults[] trs = Business.Do<ITestPaper>().ResultsCount(stid, tpid);
            return trs;
        }
        /// <summary>
        /// 试卷的成绩数，即参加考试的人次
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public int ResultsOfCount(long tpid)
        {
            return Business.Do<ITestPaper>().ResultsOfCount(tpid);
        }
        /// <summary>
        /// 获取某个试卷的学员最高分
        /// </summary>
        /// <param name="stid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        public float ResultHighest(int stid, long tpid)
        {
            if (stid <= 0) throw new Exception("学员id为空，无法获取成绩");
            if (tpid <= 0) throw new Exception("试卷id为空，无法获取成绩");
            return Business.Do<ITestPaper>().ResultsHighest(tpid, stid);
        }

        /// <summary>
        /// 获取测试成绩
        /// </summary>
        /// <param name="id">测试成绩记录的id</param>
        /// <returns></returns>
        public Song.Entities.TestResults ResultForID(int id)
        {
            Song.Entities.TestResults result = Business.Do<ITestPaper>().ResultsSingle(id);
            if (result == null) return result;
            XmlDocument resXml = new XmlDocument();
            resXml.LoadXml(result.Tr_Results, false);
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
            result.Tr_Results = resXml.InnerXml;
            return result;
        }

        /// <summary>
        /// 删除测试成绩
        /// </summary>
        /// <param name="trid">测试成绩的id</param>
        /// <returns></returns>
        [HttpDelete, Admin, Teacher, Student]
        public int ResultDelete(string trid)
        {
            int i = 0;
            if (string.IsNullOrWhiteSpace(trid)) return i;
            string[] arr = trid.Split(',');
            foreach (string s in arr)
            {
                int idval = 0;
                int.TryParse(s, out idval);
                if (idval == 0) continue;
                try
                {
                    Business.Do<ITestPaper>().ResultsDelete(idval);
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
        /// 清空学员的某个测试的所有历史成绩
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        [HttpDelete, Admin, Teacher, Student]
        public int ResultClearForStuednt(int acid, long tpid)
        {
            Song.Entities.Accounts account = this.User;
            if (account != null && account.Ac_ID == acid)
            {
                return Business.Do<ITestPaper>().ResultsClear(acid, tpid);
            }
            return -1;
        }
        /// <summary>
        /// 清空学员的某个测试的所有历史成绩
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns></returns>
        [HttpDelete, Admin, Teacher]
        public int ResultClear(long tpid)
        {
            return Business.Do<ITestPaper>().ResultsClear(tpid);
        }

        /// <summary>
        /// 记录结课考试成绩，记录到学员购买课程的记录上
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <param name="score">结课考试的最高分，如果小于零，则重新获取</param>
        /// <returns></returns>
        [Student, Admin, Teacher]
        [HttpPost]
        public float ResultLogRecord(int acid, long couid, float score)
        {
            Song.Entities.Accounts acc = this.User;
            if (acc.Ac_ID != acid) return 0;
            //结课考试的最高分小于零，从成绩中再次读取
            if (score <= 0)
            {
                //获取当前课程的结课考试
                Song.Entities.TestPaper tp = Business.Do<ITestPaper>().FinalPaper(couid, null);
                if (tp != null) score = Business.Do<ITestPaper>().ResultsHighest(tp.Tp_Id, acc.Ac_ID);
            }

            //获取学员与课程的关联信息，如果没有则创建
            Student_Course sc = Business.Do<ICourse>().StudentCourse(acid, couid);
            if (sc == null) sc = Business.Do<IStudent>().SortCourseToStudent(acc, couid);
            if (sc == null) return 0;

            if (sc.Stc_ExamScore != score)
            {
                sc.Stc_ExamScore = score;
                new Thread(() =>
                {
                    try
                    {
                        Business.Do<ICourse>().StudentScoreSave(sc, -1, -1, score);
                    }
                    catch (Exception ex)
                    {
                        WeiSha.Core.Log.Error(this.GetType().FullName, ex);
                    }
                }).Start();

                return score;
            }
            return score;
        }

        #endregion

        #region 成绩导出
        private static string outputPath = "TestResultToExcel";
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ResultFiles(long tpid)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + tpid + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            JArray jarr = new JArray();
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            FileInfo[] files = dir.GetFiles("*.xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (System.IO.FileInfo f in files)
            {
                JObject jo = new JObject();
                jo.Add("file", f.Name);
                jo.Add("url", string.Format("{0}/{1}/{2}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, tpid, f.Name));
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        /// <summary>
        /// 试卷（模块考试）的考试成绩导出
        /// </summary>
        /// <param name="tpid">试卷id</param>    
        /// <returns></returns>
        [HttpPost]
        public JObject ResultsOutput(long tpid)
        {  
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\" + tpid + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            //
            Song.Entities.TestPaper paper = Business.Do<ITestPaper>().PaperSingle(tpid);
            DateTime date = DateTime.Now;
            string filename = string.Format("《{0}》的成绩_({1}).xls", paper.Tp_Name, date.ToString("yyyy-MM-dd hh-mm-ss"));
            string filePath = rootpath + filename;
            filePath = Business.Do<ITestPaper>().ResultsOutput(filePath, tpid);
            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", string.Format("{0}/{1}/{2}", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath, tpid, filename));
            jo.Add("date", date);            
            return jo;
        }
        /// <summary>
        /// 删除成绩导出的Excel文件
        /// </summary>
        /// <param name="tpid">试卷id</param>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ResultExcelDelete(long tpid, string filename)
        {
            return Song.ViewData.Helper.Excel.DeleteFile(filename, outputPath + "\\" + tpid, "Temp");
        }
        #endregion
    }
}