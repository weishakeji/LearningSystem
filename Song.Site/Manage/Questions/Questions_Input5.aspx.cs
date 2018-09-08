using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;

using WeiSha.Common;

using Song.ServiceInterfaces;
using Song.Entities;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Song.Site.Manage.Questions
{
    public partial class Questions_Input5 : Extend.CustomPage
    {
        //类型为填空题
        int type=WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ExcelInput1_OnInput(object sender, EventArgs e)
        {
            org = Business.Do<IOrganization>().OrganCurrent();
            //工作簿中的数据
            DataTable dt = ExcelInput1.SheetDataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //throw new Exception();
                    //将数据逐行导入数据库
                    _inputData(dt.Rows[i]);                    
                }
                catch
                {
                    //如果出错，将错误行返回给控件
                    ExcelInput1.AddError(dt.Rows[i]);
                }
            }
            Business.Do<IQuestions>().OnSave(null, EventArgs.Empty);
            Business.Do<IOutline>().OnSave(null, EventArgs.Empty);
        }
       

        #region 导入数据
        
        /// <summary>
        /// 将某一行数据加入到数据库
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="dl"></param>
        private void _inputData(DataRow dr)
        {
            Song.Entities.Questions obj = new Song.Entities.Questions();
            obj.Qus_IsUse = true;
            obj.Qus_Type = this.type;
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {
                //Excel的列的值
                string column = dr[rel.Key].ToString();
                //数据库字段的名称
                string field = rel.Value;
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
                    Song.Entities.Course course = Business.Do<ICourse>().CourseBatchAdd(org.Org_ID, obj.Sbj_ID, column);
                    if (course != null) obj.Cou_ID = course.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
                //唯一值
                obj.Qus_UID = WeiSha.Common.Request.UniqueID();                               
            }
            //再遍历一遍，取答案
            int ansNum = 0;
            List<Song.Entities.QuesAnswer> ansItem = new List<QuesAnswer>();
            foreach (KeyValuePair<String, String> rel in ExcelInput1.DataRelation)
            {
                //数据库字段的名称
                string field = rel.Value;
                Match match = new Regex(@"(Ans_Context)(\d+)").Match(field);
                if (match.Success)
                {
                    //Excel的列的值
                    string column = dr[rel.Key].ToString();
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
        private string tranTxt(string txt)
        {
            txt = txt.Replace("(","（");
            txt = txt.Replace(")", "）");
            txt = txt.Replace("（", "（ ");
            txt = Regex.Replace(txt, @"（[^）]_+）", "（______）");            
            return txt;
        }        
        #endregion
      
    }
}
