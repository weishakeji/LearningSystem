using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiSha.Core;
using Song.Entities;
using Song.ServiceInterfaces;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.IO;

namespace Song.ViewData.QuestionHandler
{
    /// <summary>
    /// 试题导入
    /// </summary>
    public class CourseQuesImport
    {
        #region 私有方法       
        /// <summary>
        /// 获取正确答案
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static int _get_correct_value(string text)
        {
            //取第一个数字为正确答案的索引
            Match match1 = Regex.Match(text, @"\d+", RegexOptions.Multiline);
            if (match1.Success) return match1.Value.Convert<int>();

            //取第一个字母为正确答案的索引
            Match match2 = Regex.Match(text, @"[A-Za-z]");
            if (match2.Success)
            {
                foreach (char c in match1.Value)
                    return (int)c - 64;
            }
            return 0;
        }
        /// <summary>
        /// 获取正确答案
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static List<int> _get_correct_values(string text)
        {
            List<int> values = new List<int>();
            //取第一个数字为正确答案的索引           
            MatchCollection matches1 = Regex.Matches(text, @"\d+", RegexOptions.Multiline);
            foreach (Match match in matches1)
            {
                int tm = match.Value.Convert<int>();
                if (tm < 1) continue;
                values.Add(tm);
            }

            //取第一个字母为正确答案的索引
            Match match2 = Regex.Match(text, @"[A-Za-z]");
            MatchCollection matches2 = Regex.Matches(text, @"[A-Za-z]", RegexOptions.Multiline);
            foreach (Match match in matches2)
            {
                char tm = match.Value[0];
                int c = (int)tm - 64;
                if (c < 1) continue;
                values.Add(c);
            }
            return values;
        }
        #endregion
        /// <summary>
        /// 导入单选题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="dr">数据行</param>
        /// <param name="type">题型</param>
        /// <param name="course">当前课程</param>
        /// <param name="orgid"></param>
        /// <param name="org">当前机构</param>
        /// <param name="mathing">excel列与字段的匹配关联</param>
        public static void Type1(string excel, DataRow dr, int type, Song.Entities.Course course, int orgid, JArray mathing)
        {
            Song.Entities.Questions obj = new Song.Entities.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = type;
            //正确答案
            int correct = 0;
            for (int i = 0; i < mathing.Count; i++)
            {
                //Excel的列的值
                string column = dr[mathing[i]["column"].ToString()].ToString();
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                if (field == "Qus_ID")
                {
                    if (string.IsNullOrEmpty(column) || column.Trim() == "") continue;
                    long ques = Convert.ToInt64(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = longtext(excel, column);
                    obj.Qus_Title = tranTxt(obj.Qus_Title);
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(orgid, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name" && course == null)
                {
                    Song.Entities.Course cou = Business.Do<ICourse>().CourseBatchAdd(null, orgid, obj.Sbj_ID, column);
                    if (cou != null) obj.Cou_ID = cou.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    long couid = course != null ? course.Cou_ID : obj.Cou_ID;
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(orgid, obj.Sbj_ID, couid, column);
                    if (outline != null)
                    {
                        obj.Ol_ID = outline.Ol_ID;
                        obj.Ol_Name = outline.Ol_Name;
                    }
                }
                if (field == "Qus_Explain") obj.Qus_Explain = longtext(excel, column);
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
                if (field == "Ans_IsCorrect") correct = _get_correct_value(column);                
            }
            //再遍历一遍，取答案
            List<Song.Entities.QuesAnswer> ansItem = new List<QuesAnswer>();
            for (int i = 0; i < mathing.Count; i++)
            {
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                Match match = new Regex(@"(Ans_Context)(\d+)").Match(field);
                if (match.Success)
                {
                    //Excel的列的值
                    string column = dr[mathing[i]["column"].ToString()].ToString();
                    if (column == string.Empty || column.Trim() == "") continue;
                    int index = Convert.ToInt16(match.Groups[2].Value);
                    Song.Entities.QuesAnswer ans = new Song.Entities.QuesAnswer();
                    ans.Ans_Context = longtext(excel, column);
                    ans.Ans_IsCorrect = index == correct;
                    ans.Qus_UID = obj.Qus_UID;
                    ansItem.Add(ans);
                }
            }
            //判断是否有错
            string error = "";
            if (ansItem.Count < 1) error = "缺少答案选项";
            if (correct < 1 || correct > ansItem.Count)
                error = string.Format("正确答案的设置不正确，共{0}个答案选项，不能设置为{1}", ansItem.Count, correct);
            obj.Qus_IsError = error != "";
            obj.Qus_ErrorInfo = error;
            if (course != null)
            {
                obj.Cou_ID = course.Cou_ID;
                obj.Sbj_ID = course.Sbj_ID;
            }
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            obj.Org_ID = orgid;
            CourseQuesImport.QuesInput(obj, ansItem);
        }

        /// <summary>
        ///导入多选题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="dr"></param>
        /// <param name="type"></param>
        /// <param name="course"></param>
        /// <param name="orgid"></param>
        /// <param name="mathing"></param>    
        public static void Type2(string excel, DataRow dr, int type, Song.Entities.Course course, int orgid, JArray mathing)
        {
            Song.Entities.Questions obj = new Song.Entities.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = type;
            //正确答案
            List<int> correct = null;
            //是否有答案
            bool isHavAns = false;
            for (int i = 0; i < mathing.Count; i++)
            {
                //Excel的列的值
                string column = dr[mathing[i]["column"].ToString()].ToString();
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                if (field == "Qus_ID")
                {
                    if (column == string.Empty || column.Trim() == "") continue;
                    long ques = Convert.ToInt64(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = tranTxt(longtext(excel, column));
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(orgid, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name" && course == null)
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null,orgid, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    long couid = course != null ? course.Cou_ID : obj.Cou_ID;
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(orgid, obj.Sbj_ID, couid, column);
                    if (outline != null)
                    {
                        obj.Ol_ID = outline.Ol_ID;
                        obj.Ol_Name = outline.Ol_Name;
                    }
                }
                if (field == "Qus_Explain") obj.Qus_Explain = longtext(excel, column);
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
                if (field == "Ans_IsCorrect") correct = _get_correct_values(column);          //正确答案   
            }
            //再遍历一遍，取答案
            List<Song.Entities.QuesAnswer> ansItem = new List<QuesAnswer>();
            for (int i = 0; i < mathing.Count; i++)
            {
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                Match match = new Regex(@"(Ans_Context)(\d+)").Match(field);
                if (match.Success)
                {
                    //Excel的列的值
                    string column = dr[mathing[i]["column"].ToString()].ToString();
                    if (column == string.Empty || column.Trim() == "") continue;
                    int index = Convert.ToInt16(match.Groups[2].Value);
                    Song.Entities.QuesAnswer ans = new Song.Entities.QuesAnswer();
                    ans.Ans_Context = longtext(excel, column);
                    foreach (int s in correct)
                    {
                        if (index == s)
                        {
                            ans.Ans_IsCorrect = true;
                            isHavAns = true;
                            break;
                        }
                    }
                    ans.Qus_UID = obj.Qus_UID;
                    ansItem.Add(ans);
                }
            }
            if (!isHavAns) obj.Qus_IsError = true;
            //判断是否有错
            string error = "";
            if (ansItem.Count < 1) error = "缺少答案选项";
            if (!isHavAns) error = "没有设置正确答案";
            obj.Qus_IsError = error != "";
            obj.Qus_ErrorInfo = error;
            if (course != null)
            {
                obj.Cou_ID = course.Cou_ID;
                obj.Sbj_ID = course.Sbj_ID;
            }
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            obj.Org_ID = orgid;
            CourseQuesImport.QuesInput(obj, ansItem);
        }
        /// <summary>
        /// 导入判断题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="dr"></param>
        /// <param name="type"></param>
        /// <param name="course"></param>
        /// <param name="orgid"></param>
        /// <param name="mathing"></param>    
        public static void Type3(string excel, DataRow dr, int type, Song.Entities.Course course, int orgid, JArray mathing)
        {
            Song.Entities.Questions obj = new Song.Entities.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = type;
            for (int i = 0; i < mathing.Count; i++)
            {
                //Excel的列的值
                string column = dr[mathing[i]["column"].ToString()].ToString();
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                if (field == "Qus_ID")
                {
                    if (string.IsNullOrEmpty(column) || column.Trim() == "") continue;
                    long ques = Convert.ToInt64(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = longtext(excel, column);
                }
                if (field == "Qus_Diff")
                {
                    obj.Qus_Diff = Convert.ToInt16(column);
                    obj.Qus_Diff = obj.Qus_Diff > 5 || obj.Qus_Diff < 1 ? 3 : obj.Qus_Diff;
                }
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(orgid, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name" && course == null)
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null, orgid, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    long couid = course != null ? course.Cou_ID : obj.Cou_ID;
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(orgid, obj.Sbj_ID, couid, column);
                    if (outline != null)
                    {
                        obj.Ol_ID = outline.Ol_ID;
                        obj.Ol_Name = outline.Ol_Name;
                    }
                }
                if (field == "Qus_Explain") obj.Qus_Explain = longtext(excel, column);
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
                if (field == "Qus_IsCorrect")
                {
                    if (column == string.Empty || column.Trim() == "") obj.Qus_IsError = true;
                    obj.Qus_IsCorrect = column.Trim() == "正确";
                }
            }
            obj.Qus_ErrorInfo = "";
            if (course != null)
            {
                obj.Cou_ID = course.Cou_ID;
                obj.Sbj_ID = course.Sbj_ID;
            }
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            obj.Org_ID = orgid;
            CourseQuesImport.QuesInput(obj, null);
        }
        /// <summary>
        /// 导入简答题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="dr"></param>
        /// <param name="type"></param>
        /// <param name="course"></param>
        /// <param name="orgid"></param>
        /// <param name="mathing"></param>
        public static void Type4(string excel, DataRow dr, int type, Song.Entities.Course course, int orgid, JArray mathing)
        {

            Song.Entities.Questions obj = new Song.Entities.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = type;
            for (int i = 0; i < mathing.Count; i++)
            {
                //Excel的列的值
                string column = dr[mathing[i]["column"].ToString()].ToString();
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                if (field == "Qus_ID")
                {
                    if (string.IsNullOrEmpty(column) || column.Trim() == "") continue;
                    long ques = Convert.ToInt64(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = longtext(excel, column);
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(orgid, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name" && course == null)
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null,orgid, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    long couid = course != null ? course.Cou_ID : obj.Cou_ID;
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(orgid, obj.Sbj_ID, couid, column);
                    if (outline != null)
                    {
                        obj.Ol_ID = outline.Ol_ID;
                        obj.Ol_Name = outline.Ol_Name;
                    }
                }
                if (field == "Qus_Explain") obj.Qus_Explain = longtext(excel, column);
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
                if (field == "Qus_Answer")
                {
                    if (column == string.Empty || column.Trim() == "") obj.Qus_IsError = true;
                    obj.Qus_Answer = column;
                }
            }
            obj.Qus_ErrorInfo = "";
            if (course != null)
            {
                obj.Cou_ID = course.Cou_ID;
                obj.Sbj_ID = course.Sbj_ID;
            }
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            obj.Org_ID = orgid;
            CourseQuesImport.QuesInput(obj, null);
        }
        /// <summary>
        /// 导入填空题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="dr"></param>
        /// <param name="type"></param>
        /// <param name="course"></param>
        /// <param name="orgid"></param>
        /// <param name="mathing"></param>
        public static void Type5(string excel, DataRow dr, int type, Song.Entities.Course course, int orgid, JArray mathing)
        {
            Song.Entities.Questions obj = new Song.Entities.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = type;
            for (int i = 0; i < mathing.Count; i++)
            {
                //Excel的列的值
                string column = dr[mathing[i]["column"].ToString()].ToString();
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                if (field == "Qus_ID")
                {
                    if (column == string.Empty || column.Trim() == "") continue;
                    long ques = Convert.ToInt64(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (string.IsNullOrEmpty(column) || column.Trim() == "") return;
                    obj.Qus_Title = longtext(excel, column);
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(orgid, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name" && course == null)
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null, orgid, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    long couid = course != null ? course.Cou_ID : obj.Cou_ID;
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(orgid, obj.Sbj_ID, couid, column);
                    if (outline != null)
                    {
                        obj.Ol_ID = outline.Ol_ID;
                        obj.Ol_Name = outline.Ol_Name;
                    }
                }
                if (field == "Qus_Explain") obj.Qus_Explain = longtext(excel, column);
                //唯一值
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
            }
            //再遍历一遍，取答案
            int ansNum = 0;
            List<Song.Entities.QuesAnswer> ansItem = new List<QuesAnswer>();
            for (int i = 0; i < mathing.Count; i++)
            {
                //数据库字段的名称
                string field = mathing[i]["field"].ToString();
                Match match = new Regex(@"(Ans_Context)(\d+)").Match(field);
                if (match.Success)
                {
                    //Excel的列的值
                    string column = dr[mathing[i]["column"].ToString()].ToString();
                    if (column == string.Empty || column.Trim() == "") continue;
                    Song.Entities.QuesAnswer ans = new Song.Entities.QuesAnswer();
                    ans.Ans_Context = longtext(excel, column);
                    ans.Qus_UID = obj.Qus_UID;
                    ansNum++;
                    ansItem.Add(ans);
                }
            }
            obj.Qus_Title = tranTxt(obj.Qus_Title);
            int bracketsCount = new Regex(@"（[^）]+）").Matches(obj.Qus_Title).Count;
            //判断是否有错
            string error = "";
            if (bracketsCount <= 0) error = "试题中缺少填空项！（填空项用括号标识）";
            if (ansNum <= 0) error = "缺少答案项";
            if (ansNum < bracketsCount) error = string.Format("答案项少于填空项；填空项{0}个,答案{1}个", bracketsCount, ansNum);
            //
            obj.Qus_IsError = error != "";
            obj.Qus_ErrorInfo = error;
            if (course != null)
            {
                obj.Cou_ID = course.Cou_ID;
                obj.Sbj_ID = course.Sbj_ID;
            }
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            obj.Org_ID = orgid;
            CourseQuesImport.QuesInput(obj, ansItem);
        }
        /// <summary>
        /// 处理题干
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string tranTxt(string txt)
        {
            txt = txt.Replace("(", "（");
            txt = txt.Replace(")", "）");
            txt = txt.Replace("（", "（ ");
            return txt;
        }
        /// <summary>
        /// 长文本的获取，在导入的文本中，文本长度超过单元格最大长度时，是以文本文件存放的
        /// </summary>
        /// <param name="excel">excel所在的位置</param>
        /// <param name="content">单元格的内容</param>
        /// <returns></returns>
        private static string longtext(string excel, string content)
        {
            Regex regex = new Regex(@"^\d+\.\d+\.[A-Za-z|_]+\.txt$", RegexOptions.IgnorePatternWhitespace);
            if (regex.IsMatch(content))
            {
                string path = Path.GetDirectoryName(excel);
                return System.IO.File.ReadAllText(path + "\\" + content);
            }
            return content;
        }
        /// <summary>
        /// 导入试题，在导入之前做一些处理
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ansItem"></param>
        private static void QuesInput(Questions entity, List<Song.Entities.QuesAnswer> ansItem)
        {
            //清理脚本
            entity.Qus_Title = CleanText.Title(entity.Qus_Title);
            entity.Qus_Answer = CleanText.Content(entity.Qus_Answer);
            entity.Qus_Explain = CleanText.Content(entity.Qus_Explain);
            Business.Do<IQuestions>().QuesInput(entity, ansItem);
        }
    }
}
