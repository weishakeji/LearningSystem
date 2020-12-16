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
    public partial class Questions_Input4 : Extend.CustomPage
    {
        int type=WeiSha.Common.Request.QueryString["type"].Int32 ?? 0;
        //来自get参数的课程id
        int couid_get = WeiSha.Common.Request.QueryString["couid"].Int32 ?? 0;
        Song.Entities.Course course = null;
        Song.Entities.Organization org = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (couid_get > 0)
            {
                course = Business.Do<ICourse>().CourseSingle(couid_get);
            }
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
                    Song.Entities.Course course = Business.Do<ICourse>().CourseBatchAdd(org.Org_ID, obj.Sbj_ID, column);
                    if (course != null) obj.Cou_ID = course.Cou_ID;
                }
                if (field == "Ol_Name")
                {
                    Song.Entities.Outline outline = Business.Do<IOutline>().OutlineBatchAdd(org.Org_ID, obj.Sbj_ID, obj.Cou_ID, column);
                    if (outline != null) obj.Ol_ID = outline.Ol_ID;
                }
                if (field == "Qus_Explain") obj.Qus_Explain = column;
                //唯一值，正确答案，类型
                obj.Qus_UID = WeiSha.Common.Request.UniqueID();
                if (field == "Qus_Answer")
                {
                    if (column == string.Empty || column.Trim() == "") obj.Qus_IsError = true;
                    obj.Qus_Answer = column;
                }
            }
            obj.Qus_ErrorInfo = "";
            if (this.course != null)
            {
                obj.Cou_ID = this.course.Cou_ID;
                obj.Sbj_ID = this.course.Sbj_ID;
            }
            if (obj.Sbj_ID == 0) throw new Exception("当前试题所属专业并不存在");
            if (obj.Cou_ID == 0) throw new Exception("当前试题所在课程并不存在");
            //if (obj.Ol_ID == 0) throw new Exception("当前试题所在章节并不存在");
            if (org != null) obj.Org_ID = org.Org_ID;
            Business.Do<IQuestions>().QuesInput(obj, null);
        }        
        #endregion
       
    }
}
