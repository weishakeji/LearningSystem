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

namespace Song.ViewData.QuestionHandler
{
    /// <summary>
    /// 试题导入
    /// </summary>
    public class Import
    {
        /// <summary>
        /// 导入单选题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <param name="type">题型</param>
        /// <param name="course">当前课程</param>
        /// <param name="org">当前机构</param>
        /// <param name="mathing">excel列与字段的匹配关联</param>
        public static void Type1(DataRow dr,int type, Song.Entities.Course course,Song.Entities.Organization org, JArray mathing)
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
                    int ques = Convert.ToInt32(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = column;
                    obj.Qus_Title = tranTxt(obj.Qus_Title);
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(org.Org_ID, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name")
                {
                    Song.Entities.Course cou = Business.Do<ICourse>().CourseBatchAdd(null,org.Org_ID, obj.Sbj_ID, column);
                    if (cou != null) obj.Cou_ID = cou.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
                if (field == "Ans_IsCorrect")
                {
                    if (new Regex(@"^\d+$", RegexOptions.Multiline).Match(column).Success)
                        correct = column == string.Empty ? 0 : Convert.ToInt32(column);
                }
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
                    ans.Ans_Context = column;
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
            if (org != null) obj.Org_ID = org.Org_ID;
            Business.Do<IQuestions>().QuesInput(obj, ansItem);
        }

        /// <summary>
        ///导入多选题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        public static void Type2(DataRow dr, int type, Song.Entities.Course course, Song.Entities.Organization org, JArray mathing)
        {
            Song.Entities.Questions obj = new Song.Entities.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = type;
            //正确答案
            string[] correct = null;
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
                    int ques = Convert.ToInt32(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = tranTxt(column);
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(org.Org_ID, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name")
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null,org.Org_ID, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
                if (field == "Ans_IsCorrect")
                {
                    column = Regex.Replace(column, @"[^1-9]", ",");
                    correct = column.Split(',');
                }
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
                    ans.Ans_Context = column;
                    foreach (string s in correct)
                    {
                        if (s == string.Empty || s.Trim() == "") continue;
                        if (index == Convert.ToInt32(s))
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
            if (org != null) obj.Org_ID = org.Org_ID;
            Business.Do<IQuestions>().QuesInput(obj, ansItem);
        }
        /// <summary>
        /// 导入判断题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        public static void Type3(DataRow dr, int type, Song.Entities.Course course, Song.Entities.Organization org, JArray mathing)
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
                    int ques = Convert.ToInt32(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = column;
                }
                if (field == "Qus_Diff")
                {
                    obj.Qus_Diff = Convert.ToInt16(column);
                    obj.Qus_Diff = obj.Qus_Diff > 5 || obj.Qus_Diff < 1 ? 3 : obj.Qus_Diff;
                }
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(org.Org_ID, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name")
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null,org.Org_ID, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Core.Request.UniqueID();
                if (field == "Qus_IsCorrect")
                {
                    if (column == string.Empty || column.Trim() == "") obj.Qus_IsError = true;
                    obj.Qus_IsCorrect = column.Trim() == "正确";
                }
            }
            obj.Qus_ErrorInfo = "";
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            if (org != null) obj.Org_ID = org.Org_ID;
            Business.Do<IQuestions>().QuesInput(obj, null);
        }
        /// <summary>
        /// 导入简答题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        public static void Type4(DataRow dr, int type, Song.Entities.Course course, Song.Entities.Organization org, JArray mathing)
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
                    int ques = Convert.ToInt32(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (column == string.Empty || column.Trim() == "") return;
                    obj.Qus_Title = column;
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(org.Org_ID, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name")
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null,org.Org_ID, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
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
            if (org != null) obj.Org_ID = org.Org_ID;
            Business.Do<IQuestions>().QuesInput(obj, null);
        }
        /// <summary>
        /// 导入填空题，将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        public static void Type5(DataRow dr, int type, Song.Entities.Course course, Song.Entities.Organization org, JArray mathing)
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
                    int ques = Convert.ToInt32(column);
                    Song.Entities.Questions isHavObj = Business.Do<IQuestions>().QuesSingle(ques);
                    if (isHavObj != null) obj = isHavObj;
                }
                //题干难度、专业、试题讲解
                if (field == "Qus_Title")
                {
                    if (string.IsNullOrEmpty(column) || column.Trim() == "") return;
                    obj.Qus_Title = column;
                }
                if (field == "Qus_Diff") obj.Qus_Diff = Convert.ToInt16(column);
                if (field == "Sbj_Name")
                {
                    Song.Entities.Subject subject = Business.Do<ISubject>().SubjectBatchAdd(org.Org_ID, column);
                    if (subject != null)
                    {
                        obj.Sbj_Name = subject.Sbj_Name;
                        obj.Sbj_ID = subject.Sbj_ID;
                    }
                }
                if (field == "Cou_Name")
                {
                    Song.Entities.Course cour = Business.Do<ICourse>().CourseBatchAdd(null,org.Org_ID, obj.Sbj_ID, column);
                    if (cour != null) obj.Cou_ID = cour.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
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
                    ans.Ans_Context = column;
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
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            if (org != null) obj.Org_ID = org.Org_ID;
            Business.Do<IQuestions>().QuesInput(obj, ansItem);
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
    }
}
