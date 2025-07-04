﻿using System;
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
using System.Threading.Tasks;

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
            Song.Entities.Questions old = Business.Do<IQuestions>().QuesSingle(entity.Qus_ID);
            if (old == null) throw new Exception("Not found entity for Questions！");
            //是否更改章节id
            long oldOlid = old.Ol_ID, newOlid = entity.Ol_ID;
            old.Copy<Song.Entities.Questions>(entity);
            //处理单选、多选的选项
            if (entity.Qus_Type == 1 || entity.Qus_Type == 2 || entity.Qus_Type == 5)
            {
                old.Qus_Items = Business.Do<IQuestions>().AnswerToItems(_answerToItems(entity));
            }           
            Business.Do<IQuestions>().QuesSave(old);

            //更新章节试题数
            if (oldOlid != newOlid)
            {                
                Business.Do<IOutline>().StatisticalQuestion(oldOlid);
                Business.Do<IOutline>().StatisticalQuestion(newOlid);
            }
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
                        Song.Entities.QuesAnswer obj = ExecuteMethod.ValueToEntity<Song.Entities.QuesAnswer>(null, jt.ToString());
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
            string excel = WeiSha.Core.Server.MapPath(xls);
            DataTable dt = ViewData.Helper.Excel.SheetToDatatable(excel, sheet, config);

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
                    object[] objs = new object[] { excel, dt.Rows[i], type, course, org, matching };
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
                //刷新课程与章节的统计数据，当前课程下的章节试题也会计算
                Business.Do<IQuestions>().QuesCountUpdate(-1, -1, couid, -1);
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
        /// <param name="folder">导出的文件夹，相对于subpath，更深一级</param>
        /// <param name="types">题型</param>
        /// <param name="diffs">难度</param>
        /// <param name="part">导出方式，1导出所有，2导出正常的试题，没有错误，没有用户反馈说错误的，3导出状态为错误的试题，导出用户反馈说错误的试题</param>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <returns></returns>
        public JObject ExcelExport(string subpath,string folder, string types, string diffs, int part, int orgid, long sbjid, long couid, long olid)
        {
            JObject jo = null;
            //导出所有
            if (part == 1) jo = Business.Do<IQuestions>().QuestionsExportExcel(subpath, folder, orgid, types, sbjid, couid, olid, diffs, null, null);
            //导出正常的试题，没有错误，没有用户反馈说错误的
            if (part == 2) jo = Business.Do<IQuestions>().QuestionsExportExcel(subpath, folder, orgid, types, sbjid, couid, olid, diffs, false, false);
            //导出状态为错误的试题
            if (part == 3) jo = Business.Do<IQuestions>().QuestionsExportExcel(subpath, folder, orgid, types, sbjid, couid, olid, diffs, true, null);
            //导出用户反馈说错误的试题
            if (part == 4) jo = Business.Do<IQuestions>().QuestionsExportExcel(subpath, folder, orgid, types, sbjid, couid, olid, diffs, null, true);
            return jo;
        }
        /// <summary>
        /// 删除Excel文件
        /// </summary>
        /// <param name="couid">试题所在的课程</param>
        /// <param name="filename">文件名，带后缀名，不带路径</param>
        /// <param name="subpath">导出文件的路径，相对临时路径的子路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ExcelDelete(long couid, string filename, string subpath)
        {
            return Song.ViewData.Helper.Excel.DeleteFile(filename, subpath + "/" + couid.ToString(), "Temp");          
        }
        /// <summary>
        /// 删除所有
        /// </summary>
        /// <param name="couid">试题所在的课程</param>
        /// <param name="subpath">导出文件的路径，相对临时路径的子路径</param>
        /// <returns></returns>
        [HttpDelete]
        public bool ExcelDeleteAll(long couid, string subpath)
        {
            return Song.ViewData.Helper.Excel.DeleteDirectory(subpath + "/" + couid.ToString(), "Temp");
        }
        /// <summary>
        /// 已经生成的Excel文件
        /// </summary>
        /// <param name="subpath">导出文件的路径，相对临时路径的子路径</param>
        /// <param name="couid">课程id,如果小于等于零，则取所有</param>
        /// <returns>file:文件名,url:下载地址,date:创建时间</returns>
        public JArray ExcelFiles(string subpath, string couid)
        {
            string rootpath = Path.Combine(WeiSha.Core.Upload.Get["Temp"].Physics, subpath, couid.ToString());
            JArray jarr = new JArray();
            if (!System.IO.Directory.Exists(rootpath)) return jarr;           
            if (string.IsNullOrWhiteSpace(couid)) return jarr;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(rootpath);
            //if (couid == "0" || string.IsNullOrEmpty(couid)) couid = "*";
            string[] patterns = new[] { $"*.xls", $"*.zip" };
            List<FileInfo> files = new List<FileInfo>();
            foreach (var pattern in patterns) files.AddRange(dir.GetFiles(pattern));
            files = files.OrderByDescending(f => f.CreationTime).ToList<FileInfo>();
            foreach (FileInfo f in files)
            {
                JObject jo = new JObject();
                jo.Add("name", Path.GetFileNameWithoutExtension(f.Name).Replace("." + couid, ""));
                jo.Add("file", f.Name);
                jo.Add("url", WeiSha.Core.Upload.Get["Temp"].Virtual + subpath + "/" + couid.ToString() + "/" + f.Name);
                jo.Add("date", f.CreationTime);
                jo.Add("type", Path.GetExtension(f.Name).TrimStart('.'));
                jo.Add("size", f.Length);
                jarr.Add(jo);
            }
            return jarr;
        }
        #endregion

        #region 查询
        /// <summary>
        /// 计算有多少条试题，如果涉及专业或章节，只计算当前层级，不包括下级
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
        /// 计算有多少条试题，如果涉及专业或章节，将计算所有下级的数量
        /// </summary>
        /// <param name="orgid">机构id</param>
        /// <param name="sbjid">专业id</param>
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">试题类型</param>
        /// <param name="use">启用中的试题，null取所有</param>
        /// <returns></returns>       
        public int Total(int orgid, long sbjid, long couid, long olid, int type, bool? use)
        {
            return Business.Do<IQuestions>().Total(orgid, sbjid, couid, olid, type, -1, use);
        }
        /// <summary>
        /// 计算有多少条试题
        /// </summary>
        /// <param name="orgid"></param>
        /// <param name="sbjid"></param>
        /// <param name="couid"></param>
        /// <param name="olid"></param>
        /// <param name="types"></param>
        /// <param name="use"></param>
        /// <param name="diffs"></param>
        /// <param name="error"></param>
        /// <param name="wrong"></param>
        /// <returns></returns>
        public int Total(int orgid, long sbjid, long couid, long olid, int[] types, int[] diffs, int part)
        {
            //int[] typearr= types.Split(',').Select(x => int.Parse(x)).ToArray();
            //int[] diffarr= diffs.Split(',').Select(x => int.Parse(x)).ToArray();
            int[] typearr = types;
            int[] diffarr = diffs;
            //所有
            if (part == 1) return Business.Do<IQuestions>().QuesOfCount(orgid, sbjid, couid, olid, typearr, diffarr, null, null, null);
            //正常的试题，没有错误，没有用户反馈说错误的
            if (part == 2) return Business.Do<IQuestions>().QuesOfCount(orgid, sbjid, couid, olid, typearr, diffarr, null, false, false);
            //状态为错误的试题
            if (part == 3) return Business.Do<IQuestions>().QuesOfCount(orgid, sbjid, couid, olid, typearr, diffarr, null, true, null);
            //用户反馈说错误的试题
            if (part == 4) return Business.Do<IQuestions>().QuesOfCount(orgid, sbjid, couid, olid, typearr, diffarr, null, null, true);

            return Business.Do<IQuestions>().QuesOfCount(orgid, sbjid, couid, olid, typearr, diffarr, null, null, null);
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
        /// 当前试题的下一个试题，在指定范围内取，例如课程内的试题
        /// </summary>
        /// <param name="id">试题id</param>
        /// <param name="olid">章节id</param>
        /// <param name="couid">课程id</param>
        /// <param name="sbjid">专业id</param>
        /// <returns></returns>
        public Song.Entities.Questions Next(long id, long olid, long couid, long sbjid)
        {
            return Business.Do<IQuestions>().QuesNext(id, olid, couid, sbjid);
        }
        /// <summary>
        /// 当试题的上一个试题，在指定范围内取，例如课程内的试题
        /// </summary>
        /// <param name="id">试题id</param>
        /// <param name="olid">章节id</param>
        /// <param name="couid">课程id</param>
        /// <param name="sbjid">专业id</param>
        /// <returns></returns>
        public Song.Entities.Questions Prev(long id, long olid, long couid, long sbjid)
        {
            return Business.Do<IQuestions>().QuesPrev(id, olid, couid, sbjid);
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
        /// <param name="couid">课程id</param>
        /// <param name="olid">章节id</param>
        /// <param name="type">题型</param>
        /// <param name="diff">难度</param>
        /// <param name="count">取多少道</param>   
        /// <returns></returns>
        public Dictionary<string, List<string>> Simplify(long couid, long olid, int type, int diff, int count)
        {
            //要返回的字段
            Field[] fields = new Field[] {
                Questions._.Qus_ID,
                Questions._.Qus_Type
            };
            List<Song.Entities.Questions> ques = Business.Do<IQuestions>().QuesSimplify(-1, -1, couid, olid, type, diff, true, fields, count);
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
        #endregion

        #region 处理试题内容
        private static Song.Entities.Questions _tran(Song.Entities.Questions ques)
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
                Business.Do<IQuestions>().QuesUpdate(qid,
                    new Field[] { Questions._.Qus_WrongInfo, Questions._.Qus_IsWrong },
                    new object[] { ques.Qus_WrongInfo, ques.Qus_IsWrong });
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
            List<Song.Entities.Questions> ques = Business.Do<IStudent>().QuesOftenwrong(couid, type, count);
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
            new System.Threading.Tasks.Task(() =>
            {
                Business.Do<IQuestions>().ExerciseLogSave(acc, -1, couid, olid, json.ToString(), total, answer, correct, wrong, rate);
                //试题练习的通过率
                double cou_passrate = Business.Do<IQuestions>().CalcPassRate(acid, couid);
                //保存到学员课程的购买记录，并计算综合成绩
                Student_Course sc = Business.Do<ICourse>().StudentCourse(acid, couid);
                if (sc == null) sc = Business.Do<IStudent>().SortCourseToStudent(acc, couid);
                if (sc != null) Business.Do<ICourse>().StudentScoreSave(sc, -1, (float)Math.Round(cou_passrate * 100) / 100, -1);
                
            }).Start();

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
        [Student]
        [HttpPost]
        public bool ExerciseLogRecord(int acid, long couid, float rate)
        {
            Song.Entities.Accounts acc = this.User;
            if (acc.Ac_ID != acid) return false;

            Student_Course sc = Business.Do<ICourse>().StudentCourse(acid, couid);
            if (sc == null) sc = Business.Do<IStudent>().SortCourseToStudent(acc, couid);
            if (sc == null) return false;

            Business.Do<ICourse>().StudentScoreSave(sc, -1, rate, -1);
            return true;
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

        /// <summary>
        /// 计算某个学员的练习记录的通过率
        /// </summary>
        /// <param name="acid">学员账号id</param>
        /// <param name="couid">课程id</param>
        /// <returns></returns>
        public double CalcPassRate(int acid, long couid)
        {
            return Business.Do<IQuestions>().CalcPassRate(acid, couid);
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

        #region AI相关

        /// <summary>
        /// AI试题解析
        /// </summary>
        /// <param name="qid"></param>
        /// <param name="type">题型</param>
        /// <param name="items">选项</param>
        /// <param name="answer">答案</param>
        /// <returns></returns>
        [HttpPost]
        public string AIExplain(long qid, string type, string items, string answer)
        {
            Song.Entities.Questions ques = Business.Do<IQuestions>().QuesSingle(qid);
            if (ques == null) return string.Empty;

            //设定AI的角色
            string role = Song.APIHub.LLM.Gatway.TemplateRole("Questions/Explain");
            //取试题所在专业与课程
            Song.Entities.Subject sbj = Business.Do<ISubject>().SubjectSingle(ques.Sbj_ID);
            //试题所在课程
            Song.Entities.Course cou = Business.Do<ICourse>().CourseSingle(ques.Cou_ID);
            role = APIHub.LLM.Gatway.TemplateHandle(role, sbj);
            role = APIHub.LLM.Gatway.TemplateHandle(role, cou);

            //生成问题的描述
            string message = Song.APIHub.LLM.Gatway.TemplateMsg("Questions/Explain");
            message = APIHub.LLM.Gatway.TemplateHandle(message, ques);
            message = APIHub.LLM.Gatway.TemplateHandle(message, "type", type);
            message = APIHub.LLM.Gatway.TemplateHandle(message, "items", items);
            message = APIHub.LLM.Gatway.TemplateHandle(message, "answer", answer);

            //向AI发送请求
            return APIHub.LLM.Gatway.Consult(role, message);
        }
        /// <summary>
        /// AI生成试题
        /// </summary>
        /// <param name="type">试题类型，采用数字</param>
        /// <param name="sbj">专业名称</param>
        /// <param name="cou">课程名称</param>
        /// <param name="outline">章节名称</param>
        /// <returns></returns>
        [HttpPost]
        public JObject AIGenerate(int type, string sbj, string cou, string outline)
        {
            string qtype = types[type - 1];
            //设定AI的角色
            string role = Song.APIHub.LLM.Gatway.TemplateRole("Questions/Generate");
            role = APIHub.LLM.Gatway.TemplateHandle(role, "Sbj_Name", sbj);
            role = APIHub.LLM.Gatway.TemplateHandle(role, "Cou_Name", cou);

            //生成问题的描述
            string message = Song.APIHub.LLM.Gatway.TemplateText("Questions/Generate", $"type{type}.txt");
            message = APIHub.LLM.Gatway.TemplateHandle(message, "Sbj_Name", sbj);
            message = APIHub.LLM.Gatway.TemplateHandle(message, "Cou_Name", cou);
            message = APIHub.LLM.Gatway.TemplateHandle(message, "Ol_Name", outline);
            message = APIHub.LLM.Gatway.TemplateHandle(message, "type", qtype);

            //向AI发送请求
            string result = APIHub.LLM.Gatway.Consult(role, message);
            Regex regex = new Regex(@"\{(.*)\}", RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            if (regex.IsMatch(result)) result = regex.Match(result).Groups[0].Value;
            JObject joqus = JObject.Parse(result);
            //单选题、多选题
            if (type == 1 || type == 2)
            {
                //解析生成选项
                List<QuesAnswer> ansitems = new List<QuesAnswer>();
                //答案的解析
                List<string> answer = new List<string>();
                string anstm = joqus["Qus_Answer"].ToString();
                for (int i = 0; i < anstm.Length; i++)
                {
                    char c = anstm[i];
                    if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) answer.Add(c.ToString().ToUpper());
                }
                //
                JArray jitems = joqus["Items"] as JArray;
                for (int i = 0; i < jitems.Count; i++)
                {
                    string item = jitems[i].ToString();    //选项
                    char serial = (char)(65 + i);  //序号，例如A、B、C、D

                    string context = item.IndexOf(".") > -1 ? item.Substring(item.IndexOf(".") + 1) : item;
                    QuesAnswer qa = new QuesAnswer();
                    qa.Ans_ID = WeiSha.Core.Request.SnowID();
                    qa.Qus_UID = WeiSha.Core.Request.UniqueID();
                    qa.Ans_Context = context;
                    //
                    if (Array.IndexOf(answer.ToArray(), context) > -1 || Array.IndexOf(answer.ToArray(), serial.ToString()) > -1)
                        qa.Ans_IsCorrect = true;
                    //
                    ansitems.Add(qa);
                }
                joqus.Add("Qus_Items", Business.Do<IQuestions>().AnswerToItems(ansitems.ToArray()));
            }
            //判断题
            if (type == 3) joqus = JObject.Parse(result);
            //简答题
            if (type == 4) joqus = JObject.Parse(result);
            //填空题
            if (type == 5)
            {
                string answer = joqus["Qus_Answer"].ToString();
                List<QuesAnswer> items = new List<QuesAnswer>();
                foreach (string ans in answer.Split('、'))
                {
                    QuesAnswer qa = new QuesAnswer();
                    qa.Ans_ID = WeiSha.Core.Request.SnowID();
                    qa.Qus_UID = WeiSha.Core.Request.UniqueID();
                    qa.Ans_Context = ans;
                    items.Add(qa);
                }
                joqus.Add("Qus_Items", Business.Do<IQuestions>().AnswerToItems(items.ToArray()));
            }
            joqus.Add("Qus_Type", type);
            return joqus;
        }
        /// <summary>
        /// AI计算分数
        /// </summary>
        /// <param name="qid"></param>
        /// <param name="answer"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        [HttpPost]
        public float AICalcScore(long qid, string answer, float num)
        {
            float score = Business.Do<IQuestions>().CalcScore(qid, answer,num);
            return score;
        }
        #endregion
    }
}
