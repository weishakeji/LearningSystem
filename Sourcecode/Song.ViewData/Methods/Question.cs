using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using Song.ViewData.Attri;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Threading;
using WeiSha.Data;
using System.Data;
using NPOI.HSSF.UserModel;
using System.IO;

namespace Song.ViewData.Methods
{
    /// <summary>
    /// 试题
    /// </summary>
    [HttpPut, HttpGet]
    public class Question : ViewMethod, IViewAPI
    {
        #region 题型
        private static readonly string[] types= WeiSha.Core.App.Get["QuesType"].Split(',');
        /// <summary>
        /// 题型
        /// </summary>
        /// <returns></returns>
        [Cache(Expires = int.MaxValue)]
        public string[] Types()
        {
            string[] types = Question.types;
            for (int i = 0; i < types.Length; i++)
                types[i] = types[i].Trim();
            return types;
        }
        #endregion

        #region 试题编辑
        /// <summary>
        /// 删除试题,因为要统计章节的试题数，所以提交了更多id
        /// </summary>
        /// <param name="qusid">试题id，数组类型</param>
        /// <param name="olid">被删除试题的章节id</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpDelete]
        public int Delete(long[] qusid, long[] olid)
        {          
            if (qusid == null || qusid.Length<1) return 0;          
            Business.Do<IQuestions>().QuesDelete(qusid);
            //更新章节试题数
            Business.Do<IOutline>().StatisticalQuestion(olid);
            return qusid.Length;          
        }
        /// <summary>
        /// 添加试题
        /// </summary>
        /// <param name="entity">试题</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public long Add(Song.Entities.Questions entity)
        {
            //处理单选、多选的选项
            if (entity.Qus_Type == 1 || entity.Qus_Type == 2 || entity.Qus_Type == 5)
            {
                entity.Qus_Items = Business.Do<IQuestions>().AnswerToItems(_answerToItems(entity));
            }
            Business.Do<IQuestions>().QuesAdd(entity);
            return entity.Qus_ID;
        }
        /// <summary>
        /// 修改试题
        /// </summary>
        /// <param name="entity">修改试题</param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        [HtmlClear(Not = "entity")]
        public bool Modify(Song.Entities.Questions entity)
        {
            //Thread.Sleep(5000);
            //return true;

            Song.Entities.Questions old = Business.Do<IQuestions>().QuesSingle(entity.Qus_ID);
            if (old == null) throw new Exception("Not found entity for Questions！");

            old.Copy<Song.Entities.Questions>(entity);
            //处理单选、多选的选项
            if (entity.Qus_Type == 1 || entity.Qus_Type == 2 || entity.Qus_Type == 5)
            {
                old.Qus_Items = Business.Do<IQuestions>().AnswerToItems(_answerToItems(entity));
            }
            Business.Do<IQuestions>().QuesSave(old);
            return true;
        }
        /// <summary>
        /// 将试题的答题选项(Json)转换为数组
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Song.Entities.QuesAnswer[] _answerToItems(Song.Entities.Questions entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Qus_Items)) return null;
            List<Song.Entities.QuesAnswer> items = new List<QuesAnswer>();
            JArray jaryy = JArray.Parse(entity.Qus_Items);
            if (jaryy != null)
            {
                for(int i = 0; i < jaryy.Count; i++)
                {
                    JToken jt = jaryy[i];
                    try
                    {
                        Song.Entities.QuesAnswer obj = ExecuteMethod.GetValueToEntity<Song.Entities.QuesAnswer>(null, jt.ToString());
                        if (string.IsNullOrWhiteSpace(obj.Ans_Context)) continue;                     
                        //生成答案项的id
                        if (obj.Ans_ID <= 0)                       
                            obj.Ans_ID = WeiSha.Core.Request.SnowID();                     
                        //填空题，每项都是正确的
                        if (entity.Qus_Type == 5)
                        {
                            obj.Ans_IsCorrect = true;
                            obj.Ans_Context = HTML.ClearTag(obj.Ans_Context);
                        }
                        items.Add(obj);
                    }
                    catch { }
                }
            }
            return items.ToArray();
        }
        /// <summary>
        /// 修改使用状态
        /// </summary>
        /// <param name="id">试题id，可以是多个，用逗号分隔</param>
        /// <param name="use"></param>
        /// <returns></returns>
        [Admin, Teacher]
        [HttpPost]
        public int ChangeUse(string id, bool use)
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
                    Business.Do<IQuestions>().QuesUpdate(idval,
                    new WeiSha.Data.Field[] { Song.Entities.Questions._.Qus_IsUse },
                    new object[] { use });
                    i++;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return i;
        }
        #endregion

        #region 试题导入导出
        /// <summary>
        /// 试题导入
        /// </summary>
        /// <param name="xls">服务器端的excel文件名，即上传后的excel文件名</param>
        /// <param name="sheet">工作表的名称</param>
        /// <param name="config">配置文件，完整虚拟路径名</param>
        /// <param name="matching">excel列与字段的匹配关联</param>
        /// <param name="type">试题类型</param>
        /// <param name="couid">试题所属课程的id</param>
        /// <returns>success:成功数;error:失败数</returns>
        public JObject ExcelImport(string xls, int sheet, string config, JArray matching, int type, long couid)
        {
            //获取Excel中的数据
            string phyPath = WeiSha.Core.Upload.Get["Temp"].Physics;
            DataTable dt = ViewData.Helper.Excel.SheetToDatatable(phyPath + xls, sheet, config);

            //当前机构和课程
            Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
            Song.Entities.Course course = null;
            if (couid > 0) course = Business.Do<ICourse>().CourseSingle(couid);
            //通过反射调用导入试题的方法
            System.Reflection.Assembly assembly = System.Reflection.Assembly.Load("Song.ViewData");
            Type impot = assembly.GetType("Song.ViewData.QuestionHandler.Import");
            string func_name = "Type" + type;   //导入试题的方法名           

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
                    object[] objs = new object[] { dt.Rows[i], type, course, org, matching };
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
            //试题导入有可能新增了章节，这里刷新一下章节的缓存
            Business.Do<IOutline>().BuildCache(couid);

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

        public JObject ExcelExport(string types, string diffs, int part, int orgid, long sbjid, long couid, long olid)
        {
            //导出
            HSSFWorkbook hssfworkbook = null;
            //导出所有
            if (part == 1) hssfworkbook = Business.Do<IQuestions>().QuestionsExport(orgid, types, sbjid, couid, olid, diffs, null, null);
            //导出正常的试题，没有错误，没有用户反馈说错误的
            if (part == 2) hssfworkbook = Business.Do<IQuestions>().QuestionsExport(orgid, types, sbjid, couid, olid, diffs, false, false);
            //导出状态为错误的试题
            if (part == 3) hssfworkbook = Business.Do<IQuestions>().QuestionsExport(orgid, types, sbjid, couid, olid, diffs, true, null);
            //导出用户反馈说错误的试题
            if (part == 4) hssfworkbook = Business.Do<IQuestions>().QuestionsExport(orgid, types, sbjid, couid, olid, diffs, null, true);

            string outputPath = "QuestionToExcel";
            //导出文件的位置
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + outputPath + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);
            DateTime date = DateTime.Now;
            string filename = string.Format("试题导出.({0}).{1}.xls", date.ToString("yyyy-MM-dd hh-mm-ss"), couid.ToString());
            string filePath = rootpath + filename;
            FileStream file = new FileStream(filePath, FileMode.Create);
            if (hssfworkbook != null)
                hssfworkbook.Write(file);
            file.Close();

            JObject jo = new JObject();
            jo.Add("file", filename);
            jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + outputPath + "/" + filename);
            jo.Add("date", date);
            return jo;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpDelete]
        public bool ExcelDelete(string filename, string path)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + path + "\\";
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
        /// <param name="path"></param>
        /// <param name="couid">课程id,如果小于等于零，则取所有</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(string path, string couid)
        {
            string rootpath = WeiSha.Core.Upload.Get["Temp"].Physics + path + "\\";
            if (!System.IO.Directory.Exists(rootpath))
                System.IO.Directory.CreateDirectory(rootpath);

            JArray jarr = new JArray();
            if (string.IsNullOrWhiteSpace(couid)) return jarr;
            //string[] files = System.IO.Directory.GetFiles(rootpath, "*." + couid + ".xls");
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            FileInfo[] files = dir.GetFiles("*." + couid + ".xls").OrderByDescending(f => f.CreationTime).ToArray();
            foreach (FileInfo f in files)
            {
                string name = f.Name.Replace("." + couid, "");
                JObject jo = new JObject();
                jo.Add("name", name);
                jo.Add("file", f.Name);
                jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + path + "/" + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 计算有多少条试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题类型</param>
        /// <param name="use">启用中的试题，null取所有</param>
        /// <returns></returns>
        [Cache]
        public int Count(int orgid, long sbjid, long couid, long olid, int type, bool? use)
        {
            return Business.Do<IQuestions>().QuesOfCount(orgid, sbjid, couid, olid, type,-1, use);
        }

        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="id">试题id</param>
        /// <returns></returns>
        [Cache]
        [HttpPut,HttpPost]
        public Song.Entities.Questions ForID(long id)
        {
            Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(id);
            if (ques == null) return null;
            return _tran(ques);
        }
        /// <summary>
        /// 分页获取试题
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="type">试题类型</param>
        /// <param name="use">是否启用</param>
        /// <param name="error">是否有错误</param>
        /// <param name="wrong">是否有学员反馈错误</param>      
        /// <param name="search">检索字符</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult Pager(int orgid, long sbjid, long couid, long olid, int type, bool? use, bool? error, bool? wrong, string search, int size, int index)
        {
            if (orgid <= 0)
            {
                Song.Entities.Organization org = Business.Do<IOrganization>().OrganCurrent();
                orgid = org.Org_ID;
            }
            //总记录数
            int count = 0;
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesPager
                (orgid, type, sbjid, couid, olid, use, error, wrong, -1, search, size, index, out count);
            ListResult result = new ListResult(ques);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 按课程或章节输出试题
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题的题型分类</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        [Study]
        [Cache]
        public Song.Entities.Questions[] ForCourse(long couid, long olid, int type, int count)
        {
            if (couid <= 0 && olid <= 0) return null;
            int total = Business.Do<IQuestions>().QuesOfCount(-1, -1, couid, olid, type, -1, true);
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesCount(-1, -1, couid, olid, type, -1, true, 0 - 1, count);
            for (int i = 0; i < ques.Length; i++)
            {
                ques[i] = _tran(ques[i]);
            }
            return ques;
        }
        /// <summary>
        /// 获取试题的简化信息，例如仅包含试题id与类型
        /// </summary>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="type"></param>
        /// <param name="count"></param>   
        /// <returns></returns>
        public Dictionary<string, List<string>> Simplify(long couid, long olid, int type, int count)
        {
            //要返回的字段
            Field[] fields = new Field[] {
                Questions._.Qus_ID,
                Questions._.Qus_Type
            };
            Song.Entities.Questions[] ques = Business.Do<IQuestions>().QuesSimplify(-1, -1, couid, olid, type, -1, true, fields, count);
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            for (int i = 1; i <= Question.types.Length; i++)
            {
                List<string> list = new List<string>();
                foreach (Song.Entities.Questions q in ques)
                {
                    if (q.Qus_Type != i) continue;
                    list.Add(q.Qus_ID.ToString());
                }
                if (list.Count > 0) dic.Add("type_"+i.ToString(), list);
            }
            return dic;
        }
        #endregion

        #region 处理试题内容
        public static Song.Entities.Questions _tran(Song.Entities.Questions ques)
        {
            return ques;

            //if (ques == null) return ques;
            //ques.Qus_Title = _tranText(ques.Qus_Title);
            //ques.Qus_Answer = _tranText(ques.Qus_Answer);
            //ques.Qus_Explain = _tranText(ques.Qus_Explain);   
          
            //return ques;
        }
        private static string _tranText(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt)) return string.Empty;

            txt = txt.Replace("&lt;", "<");
            txt = txt.Replace("&gt;", ">");
            txt = txt.Replace("\n", "<br/>");
            txt = Html.ClearScript(txt);
            txt = Html.ClearAttr(txt, "p", "div", "font", "span", "a");
            txt = TransformImagePath(txt);
            txt = txt.Replace("&nbsp;", " ");
            return txt;
        }
        /// <summary>
        /// 处理试题中的图片
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string TransformImagePath(string text)
        {
            RegexOptions options = RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase;
            //将超链接处理为相对于模版页的路径
            string linkExpr = @"<(img)[^>]+>";
            foreach (Match match in new Regex(linkExpr, options).Matches(text))
            {
                string tagName = match.Groups[1].Value.Trim();      //标签名称
                string tagContent = match.Groups[0].Value.Trim();   //标签内容
                string expr = @"(?<=\s+)(?<key>src[^=""']*)=([""'])?(?<value>[^'"">]*)\1?";
                foreach (Match m in new Regex(expr, options).Matches(tagContent))
                {
                    string key = m.Groups["key"].Value.Trim();      //属性名称
                    string val = m.Groups["value"].Value.Trim();    //属性值    
                    if (val.StartsWith("http://", StringComparison.OrdinalIgnoreCase)) continue;
                    if (val.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) continue;
                    if (val.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase)) continue;

                    val = val.Replace("&apos;", "");
                    if (val.EndsWith("/")) val = val.Substring(0, val.Length - 1);
                    val = m.Groups[2].Value + "=\"" + val + "\"";
                    val = Regex.Replace(val, @"//", "/");

                    tagContent = tagContent.Replace(m.Value, val);
                }
                text = text.Replace(match.Groups[0].Value.Trim(), tagContent);
            }
            return text;
        }
        #endregion

        #region 试题收藏
        /// <summary>
        /// 学员收藏的试题,仅包含试题id与类型
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Dictionary<string, List<string>> CollecQues(int acid, long couid, int type)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().CollectCount(acid, 0, couid, type, -1);
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            for (int i = 1; i <= Question.types.Length; i++)
            {
                List<string> list = new List<string>();
                foreach (Song.Entities.Questions q in ques)
                {
                    if (q.Qus_Type != i) continue;
                    list.Add(q.Qus_ID.ToString());
                }
                if (list.Count > 0) dic.Add("type_" + i.ToString(), list);
            }
            return dic;
        }
        /// <summary>
        /// 添加试题收藏
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpPost]
        public bool CollectAdd(int acid, long qid, long couid)
        {
            try
            {
                Student_Collect stc = Business.Do<IStudent>().CollectSingle(acid, qid);
                if (stc == null)
                {
                    stc = new Entities.Student_Collect();
                    stc.Ac_ID = acid;
                    stc.Qus_ID = qid;
                    stc.Cou_ID = couid;
                    Business.Do<IStudent>().CollectAdd(stc);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除收藏
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool CollectDelete(int acid,long qid)
        {
            try
            {
                Business.Do<IStudent>().CollectDelete(qid, acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空试题收藏
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool CollectClear(int acid, long couid)
        {
            try
            {
                Business.Do<IStudent>().CollectClear(couid, acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 是否收藏试题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        [HttpGet]
        public bool CollectExist(int acid, long qid)
        {
            try
            {
                Student_Collect sc = Business.Do<IStudent>().CollectSingle(acid, qid);
                return sc != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 试题笔记
        /// <summary>
        /// 学员记过笔记的题,仅包含试题id与类型
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public Dictionary<string, List<string>> NotesQues(int acid, long couid, int type)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().NotesCount(acid, couid, type, -1);
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            for (int i = 1; i <= Question.types.Length; i++)
            {
                List<string> list = new List<string>();
                foreach (Song.Entities.Questions q in ques)
                {
                    if (q.Qus_Type != i) continue;
                    list.Add(q.Qus_ID.ToString());
                }
                if (list.Count > 0) dic.Add("type_" + i.ToString(), list);
            }
            return dic;
        }
        /// <summary>
        /// 编辑试题的笔记
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <param name="note">笔记内容</param>
        /// <returns></returns>
        [HttpPost]
        public bool NotesModify(int acid, long qid, string note)
        {
            
            try
            {
                //如果笔记内容为空，则删除记录
                if (string.IsNullOrWhiteSpace(note))
                {
                    Business.Do<IStudent>().NotesDelete(qid, acid);
                    return false;
                }
                else
                {
                    //如果不为空
                    Song.Entities.Student_Notes sn = Business.Do<IStudent>().NotesSingle(qid, acid);
                    if (sn != null)
                    {
                        sn.Stn_Context = note;
                        Business.Do<IStudent>().NotesSave(sn);
                    }
                    else
                    {
                        sn = new Student_Notes();
                        sn.Stn_Context = note;
                        sn.Qus_ID = qid;
                        sn.Ac_ID = acid;
                        Business.Do<IStudent>().NotesAdd(sn);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空试题笔记
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpDelete]
        public bool NotesClear(int acid, long couid)
        {
            try
            {
                Business.Do<IStudent>().NotesClear(couid, acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 试题笔记
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        [HttpGet]
        public Song.Entities.Student_Notes NotesSingle(int acid, long qid)
        {
            try
            {
                Song.Entities.Student_Notes note = Business.Do<IStudent>().NotesSingle(qid, acid);
                return note;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 试题报错
        /// <summary>
        /// 添加试题错误
        /// </summary>
        /// <param name="qid">试题id</param>
        /// <param name="error">错误信息</param>
        /// <returns>是否为错误试题，true为错误，false为正常</returns>
        [HttpPost]
        [Student]
        public bool WrongModify(long qid, string error)
        {
            try
            {
                Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(qid);
                if (ques == null) return false;
                ques.Qus_WrongInfo = error;
                ques.Qus_IsWrong = !string.IsNullOrWhiteSpace(error);
                Business.Do<IQuestions>().QuesSave(ques);
                return ques.Qus_IsWrong;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///// <summary>
        ///// 试题错误信息
        ///// </summary>
        ///// <param name="qid"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public string WrongInfo(long qid)
        //{

        //}
        #endregion

        #region 错题回顾
        /// <summary>
        /// 学员答错题时，记录错题，以便后续复习
        /// </summary>
        /// <param name="acid">学员Id</param>
        /// <param name="qid">试题id</param>
        /// <param name="couid">试题所在课程的id</param>
        /// <returns></returns>
        [HttpPost]
        public bool ErrorAdd(int acid,long qid,long couid)
        {
            //如果未设置学员id，则取当前登录的学员账号id
            if (acid <= 0)
            {
                Song.Entities.Accounts acc = this.User;
                if (acc != null) acid = acc.Ac_ID;
            }
            try
            {
                Song.Entities.Student_Ques stc = new Entities.Student_Ques();
                stc.Ac_ID = acid;
                stc.Qus_ID = qid;
                stc.Cou_ID = couid;
                stc.Squs_CrtTime = DateTime.Now;
                Business.Do<IStudent>().QuesAdd(stc);
                return true;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 学员答错的题，仅包含试题id与类型
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>       
        public Dictionary<string, List<string>> ErrorQues(int acid,long couid,int type)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().QuesAll(acid, 0, couid, type);
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            for (int i = 1; i <= Question.types.Length; i++)
            {
                List<string> list = new List<string>();
                foreach (Song.Entities.Questions q in ques)
                {
                    if (q.Qus_Type != i) continue;
                    list.Add(q.Qus_ID.ToString());
                }
                if (list.Count > 0) dic.Add("type_" + i.ToString(), list);
            }
            return dic;
        }
        /// <summary>
        /// 学员答错的题数
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">试题id</param>
        /// <param name="type">试题类型</param>
        /// <returns></returns>
        public int ErrorQuesCount(int acid, long couid, int type)
        {
            return Business.Do<IStudent>().QuesOfCount(acid, 0, couid, type);
        }
        /// <summary>
        /// 学员错题所属的课程，即通过错题获取课程列表
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="course">课程名称，可模糊查询</param>
        /// <param name="size"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListResult ErrorCourse(int acid,string course, int size, int index)
        {
            int count = 0;
            Song.Entities.Course[] courses = Business.Do<IStudent>().QuesForCourse(acid, course, size, index, out count);
            for (int i = 0; i < courses.Length; i++)
            {
                Song.Entities.Course c = Methods.Course._tran(courses[i]);
                c.Cou_Intro = c.Cou_Target = c.Cou_Content = "";
                if (!string.IsNullOrWhiteSpace(c.Cou_Name))
                    c.Cou_Name = c.Cou_Name.Replace("\"", "&quot;");
                courses[i] = c;
            }
            ListResult result = new ListResult(courses);
            result.Index = index;
            result.Size = size;
            result.Total = count;
            return result;
        }
        /// <summary>
        /// 高频错题，某个课程下做错最多的试题,仅包含试题id与类型
        /// </summary>
        /// <param name="couid">课程id</param>
        /// <param name="type">试题类型</param>
        /// <param name="count">取多少条</param>
        /// <returns></returns>
        [Cache(Expires = 120)]
        public Dictionary<string, List<string>> ErrorOftenQues(long couid, int type, int count)
        {
            Song.Entities.Questions[] ques = Business.Do<IStudent>().QuesOftenwrong(couid, type, count);
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            for (int i = 1; i <= Question.types.Length; i++)
            {
                List<string> list = new List<string>();
                foreach (Song.Entities.Questions q in ques)
                {
                    if (q.Qus_Type != i) continue;
                    list.Add(q.Qus_ID.ToString());
                }
                if (list.Count > 0) dic.Add("type_" + i.ToString(), list);
            }
            return dic;
        }
        /// <summary>
        /// 删除答错的题
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="qid">试题id</param>
        /// <returns></returns>
        public bool ErrorDelete(int acid,long qid)
        {
            try
            {
                Business.Do<IStudent>().QuesDelete(qid,acid);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 清空答错的试题，按课程清除
        /// </summary>
        /// <param name="acid">学员id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        [HttpDelete]
        public int ErrorClear(int acid,long couid)
        {
            try
            {
                return Business.Do<IStudent>().QuesClear(couid, acid);               
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        #endregion

        #region 试题练习记录
        /// <summary>
        /// 记录试题练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost, Student]
        public bool ExerciseLogSave(int acid, long couid, long olid, JObject json)
        {
            Song.Entities.Accounts acc = this.User;
            if (acc == null) return false;
            if (acid != acc.Ac_ID) return false;
            //统计数据
            int total, answer, correct, wrong;
            double rate;
            JToken countJo = json["count"];           
            int.TryParse(countJo["num"].ToString(), out total);
            int.TryParse(countJo["answer"].ToString(), out answer);
            int.TryParse(countJo["correct"].ToString(), out correct);
            int.TryParse(countJo["wrong"].ToString(), out wrong);
            double.TryParse(countJo["rate"].ToString(), out rate);
            //最后时间           
            JToken currJo = json["current"];
            if (currJo != null)
            {
                if (currJo.HasValues)
                {
                    string time = currJo["time"].ToString();
                    if (time.IndexOf(".") > -1) time = time.Substring(0, time.LastIndexOf("."));
                    long timeTricks;
                    long.TryParse(time, out timeTricks);
                    timeTricks = new DateTime(1970, 1, 1).Ticks + timeTricks * 10000;
                    DateTime last = new DateTime(timeTricks);
                }
            }
            //new System.Threading.Tasks.Task(() =>
            //{
                Business.Do<IQuestions>().ExerciseLogSave(acc, -1, couid, olid, json.ToString(), total, answer, correct, wrong, rate);
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
            return Business.Do<IQuestions>().ExerciseLogGet(acid, couid, olid);
        }
        /// <summary>
        /// 记录试题练通过率，记录到学员购买课程的记录上
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="rate">试题练习的通过率</param>
        /// <returns></returns>
        [Student][HttpPost]
        public bool ExerciseLogRecord(int acid, long couid, double rate)
        {
            Song.Entities.Accounts acc = this.User;
            if (acc.Ac_ID != acid) return false;

            Student_Course sc = Business.Do<ICourse>().StudentCourse(acid, couid);
            if (sc == null) sc = Business.Do<IStudent>().SortCourseToStudent(acc, couid);
            if (sc == null) return false;

            if (sc.Stc_QuesScore != rate)
            {
                sc.Stc_QuesScore = rate;
                Business.Do<ICourse>().StudentScoreSave(sc, -1, rate, -1);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 删除试题练习记录
        /// </summary>
        /// <param name="acid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <returns></returns>
        [HttpDelete,HttpGet(Ignore =true)]
        public bool ExerciseLogDel(int acid, long couid, long olid)
        {
            if (acid <= 0 || couid <= 0 || olid <= 0) return false;
            Business.Do<IQuestions>().ExerciseLogDel(acid, couid, olid);
            return true;
        }
        #endregion

        #region 统计数据
        /// <summary>
        /// 试题的资源存储大小
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <returns>count:文件总数;length:资源总大小，单位Mb;size:资源总大小，单位为最大单位 </returns>
        public JObject StorageResources(int orgid, long sbjid, long couid, long olid)
        {
            int count;
            long totalLength = Business.Do<IQuestions>().StorageResources(orgid, sbjid, couid, olid, out count);
            JObject jo = new JObject();
            jo.Add("count", count);  //资源文件数量
            //总大小，单位mb
            double length = ((double)(totalLength / 1024)) / 1024;
            jo.Add("length", Math.Round(length * 100) / 100);   //取小数点后两位
            //视频总大小，单位为最大，例如GB或Tb
            double size = totalLength;
            string[] arr = new string[] { "Bit", "Kb", "Mb", "Gb", "Tb" };
            for (int i = 0; i < arr.Length; i++)
            {
                if (size < 1024)
                {
                    jo.Add("size", (Math.Round(size * 100) / 100).ToString() + " " + arr[i]);
                    break;
                }
                else
                    size /= 1024;
            }
            return jo;
        }
        #endregion
    }
}
